using Core.Utils;
using Core.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.Interfaces
{
    public interface ITaskService
    {
        System.Threading.Tasks.Task<List<ValidationResult>> Create(CreateTaskVM model);

        System.Threading.Tasks.Task<GetTaskVM> GetSingleTaskById(int id);

        PagedResource<GetTaskVM> GetAll(PagedResourceParameters pagedResourceParameters);

        System.Threading.Tasks.Task<List<ValidationResult>> Update(int id, EditTaskVM model);

        System.Threading.Tasks.Task<List<ValidationResult>> Delete(int id);

        System.Threading.Tasks.Task<Tuple<PagedResource<GetTaskVM>, string>> GetAllTaskByUser(int id, PagedResourceParameters pagedResourceParameters);
        System.Threading.Tasks.Task<Tuple<PagedResource<GetTaskVM>, string>> GetAllTaskByProject(int id, PagedResourceParameters pagedResourceParameters);

        System.Threading.Tasks.Task<Tuple<PagedResource<GetTaskVM>, string>> GetAllTaskByProjectForUser(int userId, int projectId, PagedResourceParameters pagedResourceParameters);

    }
}
