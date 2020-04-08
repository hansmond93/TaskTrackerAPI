using Application.Interfaces;
using Core.AspNetCore;
using Core.DataAccess.Repository;
using Core.DataAccess.UnitOfWork;
using Core.Utils;
using Core.ViewModel;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly IRepository<Task> _taskRepo;
        private readonly IRepository<Project> _projectRepo;
        private readonly UserManager<User> _userManager;
        private readonly List<ValidationResult> errorResults = new List<ValidationResult>();
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpUserService _httpUserService;

        public TaskService(IRepository<Task> taskRepo, IUnitOfWork unitOfWork, IRepository<Project> projectRepo, UserManager<User> userManager, IHttpUserService httpUserService)
        {
            _taskRepo = taskRepo;
            _unitOfWork = unitOfWork;
            _projectRepo = projectRepo;
            _userManager = userManager;
            _httpUserService = httpUserService;
        }

        protected bool IsValid<T>(T entity)
        {
            return Validator.TryValidateObject(entity, new ValidationContext(entity, null, null), errorResults, false);

        }


        public async System.Threading.Tasks.Task<List<ValidationResult>> Create(CreateTaskVM model)
        {
            if (!IsValid(model))
            {
                return errorResults;
            }


            var task = new Task
            {
                Description = model.Description.Trim(),
                Hours = model.Hours,
                Minutes = model.Minutes,
                ProjectId = model.ProjectId,
                CreatorUserId = _httpUserService.GetCurrentUser().UserId,
                UserId = _httpUserService.GetCurrentUser().UserId
            };

            await _taskRepo.InsertAsync(task);
            await _unitOfWork.SaveChangesAsync();

            return errorResults;
        }

        public PagedResource<GetTaskVM> GetAll(PagedResourceParameters pagedResourceParameters)
        {
            var resultBeforePaging = _taskRepo.GetAll()?.OrderByDescending(x => x.CreationTime).Select(x => new GetTaskVM 
            { 
                Description = x.Description, 
                Hours = (byte)x.Hours,
                Minutes = (byte)x.Minutes,
                Id = x.Id 

            }).AsQueryable();

            if (!string.IsNullOrEmpty(pagedResourceParameters.searchQuery))
            {
                var searchQuery = pagedResourceParameters.searchQuery.Trim().ToLowerInvariant();

                resultBeforePaging = resultBeforePaging.ToList().Where(x => x.Description.ToLowerInvariant().Contains(searchQuery)).AsQueryable();

            }

            return PagedResource<GetTaskVM>.Create(resultBeforePaging, pagedResourceParameters.PageNumber, pagedResourceParameters.PageSize);


        }

        public async System.Threading.Tasks.Task<GetTaskVM> GetSingleTaskById(int id)
        {
            var data  =await _taskRepo.FirstOrDefaultAsync(id);

            if (data == null)
                return null;

            var result = new GetTaskVM
            {
                Description = data.Description,
                Minutes = (byte)data.Minutes,
                Hours = (byte)data.Hours,
                Id = data.Id
            };

            return result;

        }

        public async System.Threading.Tasks.Task<Tuple<PagedResource<GetTaskVM>, string>> GetAllTaskByUser(int id, PagedResourceParameters pagedResourceParameters)
        {
            var taskList = new PagedResource<GetTaskVM>();
            string errors = string.Empty;

            //check if the id matches an active user
            var userData = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);

            if(userData == null)
            {
                errors = "Invalid User, User not found";
                return Tuple.Create(taskList, errors);
            }

            var data = _taskRepo.GetAll().Where(x=> x.User.Id == id).OrderByDescending(x => x.CreationTime).Select( x=> new GetTaskVM
            {
                Description = x.Description,
                Minutes = (byte)x.Minutes,
                Hours = (byte)x.Hours,
                Id = x.Id

            });

            if(!String.IsNullOrEmpty(pagedResourceParameters.searchQuery))
            {
                var searchQuery = pagedResourceParameters.searchQuery.Trim().ToLowerInvariant();

                data = data.ToList().Where(x => x.Description.ToLowerInvariant().Contains(searchQuery)).AsQueryable();

            }

            var dataVM = PagedResource<GetTaskVM>.Create(data, pagedResourceParameters.PageNumber, pagedResourceParameters.PageSize);

            return Tuple.Create(dataVM, errors);

        }

        public async System.Threading.Tasks.Task<Tuple<PagedResource<GetTaskVM>, string>> GetAllTaskByProject(int id, PagedResourceParameters pagedResourceParameters)
        {
            var taskList = new PagedResource<GetTaskVM>();
            string errors = string.Empty;

            //check if the id matches a project
            var projectData = await _projectRepo.FirstOrDefaultAsync(id);

            if(projectData == null)
            {
                errors = "Invalid Project, Project not found";
                return Tuple.Create(taskList, errors);
            }

            var data = _taskRepo.GetAll().Where(x => x.ProjectId == id).OrderByDescending(x => x.CreationTime).Select(x => new GetTaskVM
            {
                Description = x.Description,
                Hours = (byte)x.Hours,
                Minutes = (byte)x.Minutes,
                Id = x.Id
            });

            if (!String.IsNullOrEmpty(pagedResourceParameters.searchQuery))
            {
                var searchQuery = pagedResourceParameters.searchQuery.Trim().ToLowerInvariant();

                data = data.ToList().Where(x => x.Description.ToLowerInvariant().Contains(searchQuery)).AsQueryable();
            }

            var dataVM = PagedResource<GetTaskVM>.Create(data, pagedResourceParameters.PageNumber, pagedResourceParameters.PageSize);

            return Tuple.Create(dataVM, errors);

        }


        public async System.Threading.Tasks.Task<Tuple<PagedResource<GetTaskVM>, string>> GetAllTaskByProjectForUser(int userId, int projectId, PagedResourceParameters pagedResourceParameters)
        {
            var taskList = new PagedResource<GetTaskVM>();
            string errors = string.Empty;

            //check if the ProjectId matches a Project
            var projectData = await _projectRepo.FirstOrDefaultAsync(projectId);
            if (projectData == null)
            {
                errors = "Invalid Project, Project not found";
                return Tuple.Create(taskList, errors);
            }

            //check if the id matches an active user
            var userData = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId && x.IsDeleted != true);

            if (userData == null)
            {
                errors = "Invalid User, User not found";
                return Tuple.Create(taskList, errors);
            }

            var data = _taskRepo.GetAll().OrderByDescending(x => x.CreationTime).Where(x => x.ProjectId == projectId && x.User.Id == userId).Select(x => new GetTaskVM
            {
                Description = x.Description,
                Hours = (byte)x.Hours,
                Minutes = (byte)x.Minutes,
                Id = x.Id

            });

            if (!String.IsNullOrEmpty(pagedResourceParameters.searchQuery))
            {
                var searchQuery = pagedResourceParameters.searchQuery.Trim().ToLowerInvariant();

                data = data.ToList().Where(x => x.Description.ToLowerInvariant().Contains(searchQuery)).AsQueryable();
            }

            var dataVM = PagedResource<GetTaskVM>.Create(data, pagedResourceParameters.PageNumber, pagedResourceParameters.PageSize);

            return Tuple.Create(dataVM, errors);


        }

        public async System.Threading.Tasks.Task<List<ValidationResult>> Update(int id, EditTaskVM model)
        {
            if (!IsValid(model))
            {
                return errorResults;
            }

            var result = await _taskRepo.FirstOrDefaultAsync(id);

            if (result == null)
                return null;

            result.Description = model.Description;
            result.Hours = model.Hours;
            result.Minutes = model.Minutes;
            result.LastModifierUserId = _httpUserService.GetCurrentUser().UserId;

            await _taskRepo.UpdateAsync(result);
            await _unitOfWork.SaveChangesAsync();

            return errorResults;

        }

        public async System.Threading.Tasks.Task<List<ValidationResult>> Delete(int id)
        {
            var result = _taskRepo.FirstOrDefaultAsync(id);

            if (result == null)
            {
                errorResults.Add(new ValidationResult("Error!, You can not delete a Task that does not exists"));

                return errorResults;
            }
            await _taskRepo.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            return errorResults;
        }

       

    }
}
