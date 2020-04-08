using Core.Utils;
using Core.ViewModel;
using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
   public interface IProjectService
    {
        Task<List<ValidationResult>> Create(CreateProjectVM model);

        PagedResource<GetProjectVM> GetAll(PagedResourceParameters pagedResourceParameters);

        Task<GetProjectVM> GetSingleProjectById(int id);

        Task<List<ValidationResult>> Update(int id, CreateProjectVM model);

        Task<List<ValidationResult>> Delete(int id);

        Task<List<ValidationResult>> RemoveUserFromProject(int userId, int projectid);

        Task<List<ValidationResult>> AddUserToProject(int userId, int projectId);

        Task<List<ValidationResult>> CloseProject(int projectId, CloseProjectVM model);


    }
}
