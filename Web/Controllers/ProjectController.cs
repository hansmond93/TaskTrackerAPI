using System;
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
    public class ProjectController : BaseController
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpPost]
        //[Route("Create")]
        public async Task<ApiResponse<string>> Create(CreateProjectVM model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return new ApiResponse<string>(errors: ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);

                var result = await _projectService.Create(model);

                if (result.Any())
                    return new ApiResponse<string>(errors: result.Select(e=>e.ErrorMessage).ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

                return new ApiResponse<string>("Successfully created project");
            });
        }

        [HttpGet]
        //[Route("GetProject")]
        public async Task<ApiResponse<IEnumerable<GetProjectVM>>> GetProject([FromQuery]PagedResourceParameters pagedResourceParameters)
        {

            return await HandleApiOperationAsync(async () =>
            {
               
                var result = _projectService.GetAll(pagedResourceParameters);
                if (result == null)
                    return new ApiResponse<IEnumerable<GetProjectVM>>(codes: ApiResponseCodes.OK, message: "No Records Found");

                //var paginationMetadata = new
                //{
                //    pageSize = result.PageSize,
                //    currentPage = result.CurrentPage,
                //    totalPages = result.TotalPages,
                //    hasNext = result.HasNext,
                //    hasPrevious = result.HasPrevious
                //};

                //Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                return new ApiResponse<IEnumerable<GetProjectVM>>(result);
            });
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ApiResponse<GetProjectVM>> GetSingleProject(int id)
        {
            return await HandleApiOperationAsync(async () =>
            {

                var result = await _projectService.GetSingleProjectById(id);
                if (result == null)
                    return new ApiResponse<GetProjectVM>(codes: ApiResponseCodes.NOT_FOUND, message: "Invalid Project Id");

                return new ApiResponse<GetProjectVM>(result);
            });
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ApiResponse<string>> Update(int id, CreateProjectVM model)
        {
            return await HandleApiOperationAsync(async () =>
            {

                var result = await _projectService.Update(id, model);
                if (result == null)
                    return new ApiResponse<string>(codes: ApiResponseCodes.NOT_FOUND, message: "Invalid Project Id");

                if(result.Any())
                    return new ApiResponse<string>(errors: result.Select(e => e.ErrorMessage).ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

                return new ApiResponse<string>("Projected Updated Successfully");
            });
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<ApiResponse<string>> Delete(int id)
        {
            return await HandleApiOperationAsync(async () =>
            {

                var result = await _projectService.Delete(id);

                if (result.Any())
                    return new ApiResponse<string>(errors: result.Select(e => e.ErrorMessage).ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

                return new ApiResponse<string>("Projected Deleted Successfully");
            });
        }

        [HttpPost]
        public async Task<ApiResponse<string>> AddUserToProject(AddUserToProjectVM model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return new ApiResponse<string>(errors: ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);

                var result = await _projectService.AddUserToProject(model.UserId, model.ProjectId);

                if (result.Any())
                    return new ApiResponse<string>(errors: result.Select(e => e.ErrorMessage).ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

                return new ApiResponse<string>("Successfully added user to the project");
            });
        }

        [HttpPost]
        public async Task<ApiResponse<string>> RemoveUserFromProject(AddUserToProjectVM model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return new ApiResponse<string>(errors: ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);

                var result = await _projectService.RemoveUserFromProject(model.UserId, model.ProjectId);

                if (result.Any())
                    return new ApiResponse<string>(errors: result.Select(e => e.ErrorMessage).ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

                return new ApiResponse<string>("Successfully removed User from the Project");
            });
        }

        [HttpPost]
        public async Task<ApiResponse<string>> CloseProject(int projectId, CloseProjectVM model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return new ApiResponse<string>(errors: ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);

                var result = await _projectService.CloseProject(projectId, model);

                if (result.Any())
                    return new ApiResponse<string>(errors: result.Select(e => e.ErrorMessage).ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

                return new ApiResponse<string>("Success! Project has been closed Successfully");
            });
        }


    }
}