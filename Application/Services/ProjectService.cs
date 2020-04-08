using Application.Interfaces;
using Core.DataAccess.Repository;
using Core.DataAccess.UnitOfWork;
using Core.ViewModel;
using Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Core.Timing;
using Microsoft.AspNetCore.Identity;
using Core.Utils;
using Core.AspNetCore;
using Core.ViewModel.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IRepository<Project> _projectRepo;
        private readonly List<ValidationResult> errorResults = new List<ValidationResult>();
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IHttpUserService _httpUserService;



        public ProjectService( IRepository<Project> projectRepo, IUnitOfWork unitOfWork, UserManager<User> userManager, IHttpUserService httpUserService)
        {
            _projectRepo = projectRepo;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _httpUserService = httpUserService;

        }

        protected bool IsValid<T>(T entity)
        {
            return Validator.TryValidateObject(entity, new ValidationContext(entity, null, null), errorResults, false);
        
        }

        public async Task<List<ValidationResult>> Create(CreateProjectVM model)
        {
            if (!IsValid(model))
            {
                return errorResults;
            }

            var data = await _projectRepo.FirstOrDefaultAsync(x => x.Code.ToLower() == model.Code.Trim().ToLower()
                                                    || x.Name.ToLower() == model.Name.Trim().ToLower());

            if(data != null)
            {
                errorResults.Add(new ValidationResult("Project with Similar Project Name or Project Code already exists"));
                return errorResults;
            }

            var project = new Project
            {
                Name = model.Name.Trim(),
                Code = model.Code.Trim(),
                Description = model.Description.Trim(),
                IsRecurring = model.IsRecurring,
                CreatorUserId = _httpUserService.GetCurrentUser().UserId,
                Status = ProjectState.Active

            };

            await _projectRepo.InsertAsync(project);
            await _unitOfWork.SaveChangesAsync();

            return errorResults; 
        }

        public PagedResource<GetProjectVM> GetAll(PagedResourceParameters pagedResourceParameters)
        {
            var resultBeforePaging = _projectRepo.GetAllIncluding(x => x.ProjectUsers)?.OrderByDescending(p => p.CreationTime)?.Select(x => new GetProjectVM
            {
                Name = x.Name,
                Code = x.Code,
                Description = x.Description,
                IsRecurring = x.IsRecurring ?? false,
                Status = (ProjectState)x.Status,
                ClosureDate = x.ClosureDate,
                ReasonForClosure = x.ReasonForClosure

            }).AsQueryable();

            if(!string.IsNullOrEmpty(pagedResourceParameters.searchQuery))
            {
                var searchQuery = pagedResourceParameters.searchQuery.Trim().ToLowerInvariant();

                resultBeforePaging = resultBeforePaging.ToList().Where(x => x.Name.ToLowerInvariant().Contains(searchQuery)
                   || x.Code.ToLowerInvariant().Contains(searchQuery)).AsQueryable();
            }

            return PagedResource<GetProjectVM>.Create(resultBeforePaging, pagedResourceParameters.PageNumber, pagedResourceParameters.PageSize);

        }

        public async Task<GetProjectVM> GetSingleProjectById(int id)
        {
            var data = await _projectRepo.FirstOrDefaultAsync(id);

            if(data == null)
                return null;

            var result = new GetProjectVM
            {
                Name = data.Name,
                Code = data.Code,
                Description = data.Description,
                IsRecurring = data.IsRecurring  ?? false,
                Status = (ProjectState)data.Status,
                ClosureDate = data.ClosureDate,
                ReasonForClosure = data.ReasonForClosure
            };

            return result;

        }

        //check if it fills last modified user
        public async Task<List<ValidationResult>> Update(int id, CreateProjectVM model)
        {
            if(!IsValid(model))
            {
                return errorResults;
            }

            var result = await _projectRepo.FirstOrDefaultAsync(id);

            if (result == null)
                return null;

            result.Name = model.Name;
            result.Code = model.Code;
            result.Description = model.Description;
            result.IsRecurring = model.IsRecurring;
            result.LastModifierUserId = _httpUserService.GetCurrentUser().UserId;


            await _projectRepo.UpdateAsync(result);
            await _unitOfWork.SaveChangesAsync();
            
            return errorResults;

        }

        public async Task<List<ValidationResult>> Delete(int id)
        {
            var result = await _projectRepo.FirstOrDefaultAsync(id);

            if(result == null)
            {
                errorResults.Add(new ValidationResult("Error!, You can not delete a Project that does not exists"));

                return errorResults;
            }
            await _projectRepo.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            return errorResults;
        }

        public async Task<List<ValidationResult>> AddUserToProject(int userId, int projectId)
        {
            var result = await _projectRepo.FirstOrDefaultAsync(x => x.Id == projectId);

            if(result == null)
            {
                errorResults.Add(new ValidationResult("Error!, Invalid Project, Project not found "));
                return errorResults;
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId && x.IsDeleted != true);

            if(result == null)
            {
                errorResults.Add(new ValidationResult("Error!, Invalid User, User does not exist"));
                return errorResults;
            }

            //check if user is not already addded to the same project
            var checkProjectUser = _projectRepo.GetAllIncluding(x => x.ProjectUsers).SingleOrDefault(x => x.Id == projectId).ProjectUsers
                                                                                    .Where(x => x.UserId == userId).SingleOrDefault();

            if(checkProjectUser != null)
            {
                errorResults.Add(new ValidationResult("Error!, You can not add a user that is already added to a project"));
                return errorResults;
            }

            var projectUser = new ProjectUser
            {
                UserId = userId,
                ProjectId = projectId
            };

            result.ProjectUsers.Add(projectUser);
            await _unitOfWork.SaveChangesAsync();

            return errorResults;
        }

        public async Task<List<ValidationResult>> RemoveUserFromProject(int userId, int projectid)
        {
            var result = _projectRepo.GetAllIncluding(x => x.ProjectUsers).SingleOrDefault(x => x.Id == projectid );

            if(result == null)
            {
                errorResults.Add(new ValidationResult("Error!, Invalid Project, Project does not exist"));
                return errorResults;
            }

            //check if the user is assigned to that project
            var projectUser = result.ProjectUsers.Where(x => x.ProjectId == projectid && x.UserId == userId).SingleOrDefault();

            if(projectUser == null)
            {
                errorResults.Add(new ValidationResult("Error!, Cannot remove User from a Project they do not belong to! "));
                return errorResults;
            }

            result.ProjectUsers.Remove(projectUser);
            await _unitOfWork.SaveChangesAsync();

            return errorResults;
        }

        public async Task<List<ValidationResult>> CloseProject(int projectId, CloseProjectVM model)
        {
            var result = await _projectRepo.FirstOrDefaultAsync(x => x.Id == projectId);

            if (result == null)
            {
                errorResults.Add(new ValidationResult("Error!, Invalid Project, Project does not exist"));
                var data = errorResults;
            }

            if(result.Status == ProjectState.Closed)
            {
                errorResults.Add(new ValidationResult("Error!, Project already closed!"));
                return errorResults;
            }

            result.ReasonForClosure = model.ReasonForClosure;
            result.Status = ProjectState.Closed;
            result.ClosureDate = Clock.Now;

            _unitOfWork.SaveChanges();

            return errorResults;

        }
    }
}
