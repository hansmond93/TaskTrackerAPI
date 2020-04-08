using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Core.AspNetCore;
using Core.Utils;
using Core.ViewModel;
using Core.ViewModel.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    public class TaskController : BaseController
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }


        [HttpPost]
        //[Route("Create")]
        public async Task<ApiResponse<string>> Create(CreateTaskVM model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return new ApiResponse<string>(errors: ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);

                var result = await _taskService.Create(model);

                if (result.Any())
                    return new ApiResponse<string>(errors: result.Select(e => e.ErrorMessage).ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

                return new ApiResponse<string>("Successfully created task");
            });
        }


        [HttpGet]
        //[Route("GetProject")]
        public async Task<ApiResponse<IEnumerable<GetTaskVM>>> GetAllTasks([FromQuery]PagedResourceParameters pagedResourceParameters)
        {
            return await HandleApiOperationAsync(async () =>
            {

                var result = _taskService.GetAll(pagedResourceParameters);
                if (result == null)
                    return new ApiResponse<IEnumerable<GetTaskVM>>(codes: ApiResponseCodes.OK, message: "No Records Found");

                //var paginationMetadata = new
                //{
                //    pageSize = result.PageSize,
                //    currentPage = result.CurrentPage,
                //    totalPages = result.TotalPages,
                //    hasNext = result.HasNext,
                //    hasPrevious = result.HasPrevious
                //};

                //Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                return new ApiResponse<IEnumerable<GetTaskVM>>(result);
            });
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ApiResponse<GetTaskVM>> GetSingleTask(int id)
        {
            return await HandleApiOperationAsync(async () =>
            {

                var result = await  _taskService.GetSingleTaskById(id);
                if (result == null)
                    return new ApiResponse<GetTaskVM>(codes: ApiResponseCodes.NOT_FOUND, message: "Invalid Task Id");

                return new ApiResponse<GetTaskVM>(result);
            });
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ApiResponse<IEnumerable<GetTaskVM>>> GetTasksByUserId([FromQuery]PagedResourceParameters pagedResourceParameters, int id)
        {
            return await HandleApiOperationAsync(async () =>
            {

                var result = await _taskService.GetAllTaskByUser(id, pagedResourceParameters);

                if (result.Item2 != string.Empty)
                    return new ApiResponse<IEnumerable<GetTaskVM>>(codes: ApiResponseCodes.NOT_FOUND, message: $"{result.Item2}");

                if (result.Item2 == string.Empty && !result.Item1.Any())
                    return new ApiResponse<IEnumerable<GetTaskVM>>(codes: ApiResponseCodes.OK, message: "User does not have any task");

                //var paginationMetadata = new
                //{
                //    pageSize = result.Item1.PageSize,
                //    currentPage = result.Item1.CurrentPage,
                //    totalPages = result.Item1.TotalPages,
                //    hasNext = result.Item1.HasNext,
                //    hasPrevious = result.Item1.HasPrevious
                //};

                //Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                return new ApiResponse<IEnumerable<GetTaskVM>>(result.Item1);
            });
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ApiResponse<IEnumerable<GetTaskVM>>> GetTasksByProject([FromQuery]PagedResourceParameters pagedResourceParameters, int id)
        {
            return await HandleApiOperationAsync(async () =>
            {

                var result = await _taskService.GetAllTaskByProject(id, pagedResourceParameters);

                if (result.Item2 != string.Empty)
                    return new ApiResponse<IEnumerable<GetTaskVM>>(codes: ApiResponseCodes.NOT_FOUND, message: $"{result.Item2}");

                if (result.Item2 == string.Empty && !result.Item1.Any())
                    return new ApiResponse<IEnumerable<GetTaskVM>>(codes: ApiResponseCodes.OK, message: "Projects does not have any task");

                //var paginationMetadata = new
                //{
                //    pageSize = result.Item1.PageSize,
                //    currentPage = result.Item1.CurrentPage,
                //    totalPages = result.Item1.TotalPages,
                //    hasNext = result.Item1.HasNext,
                //    hasPrevious = result.Item1.HasPrevious
                //};

                //Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                return new ApiResponse<IEnumerable<GetTaskVM>>(result.Item1);
            });
        }

        [HttpGet]
        [Route("{userId}/{projectId}")]
        public async Task<ApiResponse<IEnumerable<GetTaskVM>>> GetTasksByProjectUser([FromQuery]PagedResourceParameters pagedResourceParameters, int userId, int projectId)
        {
            return await HandleApiOperationAsync(async () =>
            {

                var result = await _taskService.GetAllTaskByProjectForUser(userId, projectId, pagedResourceParameters);

                if (result.Item2 != string.Empty)
                    return new ApiResponse<IEnumerable<GetTaskVM>>(codes: ApiResponseCodes.NOT_FOUND, message: $"{result.Item2}");

                if (result.Item2 == string.Empty && !result.Item1.Any())
                    return new ApiResponse<IEnumerable<GetTaskVM>>(codes: ApiResponseCodes.OK, message: "User does not have any task for this Project");

                //var paginationMetadata = new
                //{
                //    pageSize = result.Item1.PageSize,
                //    currentPage = result.Item1.CurrentPage,
                //    totalPages = result.Item1.TotalPages,
                //    hasNext = result.Item1.HasNext,
                //    hasPrevious = result.Item1.HasPrevious
                //};

                //Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                return new ApiResponse<IEnumerable<GetTaskVM>>(result.Item1);
            });
        }



        [HttpPut]
        [Route("{id}")]
        public async Task<ApiResponse<string>> Update(int id, EditTaskVM model)
        {
            return await HandleApiOperationAsync(async () =>
            {

                var result = await _taskService.Update(id, model);
                if (result == null)
                    return new ApiResponse<string>(codes: ApiResponseCodes.NOT_FOUND, message: "Invalid Task Id");

                if (result.Any())
                    return new ApiResponse<string>(errors: result.Select(e => e.ErrorMessage).ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

                return new ApiResponse<string>("Task Updated Successfully");
            });
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<ApiResponse<string>> Delete(int id)
        {
            return await HandleApiOperationAsync(async () =>
            {

                var result = await _taskService.Delete(id);

                if (result.Any())
                    return new ApiResponse<string>(errors: result.Select(e => e.ErrorMessage).ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

                return new ApiResponse<string>("Task Deleted Successfully");
            });
        }
    }
}