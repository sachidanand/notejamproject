using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Notejam.Api.Business;
using Notejam.Api.Persistance;

namespace Notejam.Api
{
    public class NotesManagementApis
    {
        private readonly INotesRepository _document;
        private readonly INotesManagement _notesManagement;
        public NotesManagementApis(
          INotesRepository document, INotesManagement notesManagement)
        {
            _document = document;
            _notesManagement = notesManagement;
        }
        [HttpPost]
        [FunctionName("CreateNote")]
        [OpenApiOperation(operationId: "CreateNote", tags: new[] { "CreateNote" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "NoteDto", In = ParameterLocation.Header, Required = true, Type = typeof(NoteRquestModel), Description = "The **Note DTO** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> CreateNote(
           [HttpTrigger(AuthorizationLevel.Function, "post", Route = "notesmanagement/createnote")] HttpRequestMessage req,
           ILogger log)
        {
            log.LogInformation("Request for new note to be created..");

            var noteRquestModel = await req.Content.ReadAsAsync<NoteRquestModel>().ConfigureAwait(false);

            var notModel = new NoteModel
            {
                NoteName = noteRquestModel.NoteName,
                NoteText = noteRquestModel.NoteText,
                PadType = noteRquestModel.PadType,
                UserId = "12"
            };
            log.LogInformation($"Note title is {noteRquestModel.NoteName}");
            _notesManagement.CreateNote(notModel);



            return new OkObjectResult("success..");
        }

        [HttpPost]
        [FunctionName("GetAllNotes")]
        [OpenApiOperation(operationId: "GetAllNotes", tags: new[] { "GetAllNotes" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(NotesResponseModel), Description = "Returns all notes")]
        public async Task<IActionResult> GetAllNotes(
          [HttpTrigger(AuthorizationLevel.Function, "get", Route = "notesmanagement/getnotes")] HttpRequestMessage req,
          ILogger log)
        {
            var lstNotesResponse = new List<NotesResponseModel>();
            log.LogInformation("Get all notes endpoint called.");

            log.LogInformation("Returning all notes..");
           var lstNotes =  await _notesManagement.GetAllNotesAsync("SELECT * FROM Notes").ConfigureAwait(false);
            
            foreach (var document in lstNotes)
            {
                var noteModel = new NotesResponseModel
                {
                    Id = document.Id,
                    NoteName = document.NoteName,
                    NoteText = document.NoteText,
                    PadType = document.PadType,
                    CreatedDate = document.CreatedDate,
                    LastModifiedDate = document.LastModifiedDate,
                    UserId = document.UserId
                };
                lstNotesResponse.Add(noteModel);

            }


            return new OkObjectResult(lstNotesResponse);
        }
    }
}
