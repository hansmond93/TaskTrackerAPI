using Application.Interfaces;
using Core.DataAccess.Repository;
using Core.Messaging.Email;
using Core.Timing;
using Core.ViewModel;
using Core.ViewModel.Enums;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ReportService : ScheduleResolver
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public ReportService(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration) : base(serviceScopeFactory, configuration)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        
        protected override string Schedule => _configuration.GetSection("ReportSchedule:default").Value;

        public override System.Threading.Tasks.Task ProcessInScope(IServiceProvider serviceProvider)
        {
            
            var data = ReportJob();
            //var dataName = data.Name;
            //using (Stream file = File.OpenWrite($"C:\\Games\\{dataName}.xls"))
            //{
            //    file.Write(data.reportData, 0, data.reportData.Length);
            //}
            SendEmailToAdmin(data);


            return System.Threading.Tasks.Task.CompletedTask;
        }

        private ReportDataModel ReportJob()
        {

            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _projectRepo = scope.ServiceProvider.GetRequiredService<IRepository<Entities.Project>>();
                var _taskRepo = scope.ServiceProvider.GetRequiredService<IRepository<Entities.Task>>();
                var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<Entities.User>>();


                Byte[] fileBytes = null;
                ReportDataModel data = new ReportDataModel();

                var users = _projectRepo.GetAllIncluding(x => x.ProjectUsers).Where(x => x.Status == ProjectState.Active && x.IsDeleted != true)
                                    .SelectMany(x => x.ProjectUsers).Where(x => x.User.IsDeleted != true).Select(x => x.UserId).Distinct().ToList();

                var fridayDate = Clock.Now;
                var mondayDate = fridayDate.AddDays(-4);  //to subtract 4 days to monday
                mondayDate = new DateTime(mondayDate.Year, mondayDate.Month, mondayDate.Day, 0, 0, 0);   //Reset the time to 12am midnight on Monday  

                using (ExcelPackage pck = new ExcelPackage())
                {

                    foreach (var user in users)
                    {
                        int currentRow = 1;
                        int taskHourTotal;
                        int taskMinuteTotal;
                        int weekHourTotal = 0;
                        int weekMinuteTotal = 0;

                        var userName = _userManager.Users.Where(x => x.Id == user).Select(x => new { x.FirstName,  x.LastName}).FirstOrDefault();

                        var FullName = $"{userName.LastName} {userName.FirstName}";

                        var tasks = _taskRepo.GetAllList().Where(x => x.UserId == user && x.CreationTime >= mondayDate && x.CreationTime <= fridayDate)
                                                      .OrderByDescending(x => x.CreationTime)
                                                      .GroupBy(x => x.ProjectId);

                        //Test QUERY
                        //var tasks = _taskRepo.GetAllList().Where(x => x.UserId == user)
                        //                              .OrderByDescending(x => x.CreationTime)
                        //                              .GroupBy(x => x.ProjectId);



                        var userWorkSheet = pck.Workbook.Worksheets.Add(FullName);

                        foreach (var project in tasks)
                        {
                            taskHourTotal = (int)project.Sum(x => x.Hours); 
                            taskMinuteTotal = (int)project.Sum(x => x.Minutes);

                            var projectName = _projectRepo.FirstOrDefault(x => x.Id == project.Key).Name;

                            userWorkSheet.Cells["A" + currentRow.ToString()].Value = projectName;
                            userWorkSheet.Cells["A" + currentRow.ToString()].Style.Font.Bold = true;

                            currentRow++;

                            userWorkSheet.Cells["B" + currentRow.ToString()].Value = "TASKS";
                            userWorkSheet.Cells["C" + currentRow.ToString()].Value = "HOURS";
                            userWorkSheet.Cells["D" + currentRow.ToString()].Value = "MINUTES";

                            currentRow++;

                            var userTasks = _taskRepo.GetAllList(x => x.UserId == user && x.ProjectId == project.Key);

                            foreach (var task in userTasks)
                            {
                                userWorkSheet.Cells["B" + currentRow.ToString()].Value = task.Description;
                                userWorkSheet.Cells["C" + currentRow.ToString()].Value = task.Hours;
                                userWorkSheet.Cells["D" + currentRow.ToString()].Value = task.Minutes;

                                currentRow++;
                            }

                            //Adjust the Time
                            AdjsutTime(ref taskHourTotal, ref taskMinuteTotal);
                            userWorkSheet.Cells["A" + currentRow.ToString()].Value = "PROJECT TOTAL";
                            userWorkSheet.Cells["C" + currentRow.ToString()].Value = taskHourTotal;
                            userWorkSheet.Cells["D" + currentRow.ToString()].Value = taskMinuteTotal;


                            weekHourTotal += taskHourTotal;
                            weekMinuteTotal += taskMinuteTotal;

                            currentRow += 2; //add two new lines before the next project

                        }

                        //Adjust the Time
                        AdjsutTime(ref weekHourTotal, ref weekMinuteTotal);
                        userWorkSheet.Cells["A" + currentRow.ToString()].Value = "WEEKLY TOTAL";
                        userWorkSheet.Cells["C" + currentRow.ToString()].Value = weekHourTotal;
                        userWorkSheet.Cells["D" + currentRow.ToString()].Value = weekMinuteTotal;
                    }

                    fileBytes = pck.GetAsByteArray();
                    data.reportData = fileBytes;
                    data.Name = $"TimeTrackerReportFor{mondayDate.ToString("dd-MM-yyyy")}-To-{fridayDate.ToString("dd-MM-yyyy")}";
                }
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        private void SendEmailToAdmin(ReportDataModel model)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<Entities.User>>();
            var _smtpEmailService = scope.ServiceProvider.GetRequiredService<IMailService>();


            var senderEmail = _configuration.GetSection("Emails:sender").Value;
            var adminEmail = _configuration.GetSection("Emails:admin").Value;
            var attachmentStream = new MemoryStream(model.reportData);

            _smtpEmailService.SendMail(new Mail(senderEmail, "Time Tracker Report", adminEmail)
            {
                Body = "Kindly Find Attached Report for this Week",
                Attachments = new List<Attachment>()
                {
                    new Attachment(attachmentStream, model.Name, $"application/vnd.ms-excel")
                }
            });
        }

        private void AdjsutTime(ref int hour, ref int minutes)
        {
            hour += (minutes / 60);
            minutes %= 60;

        }

    }
}
