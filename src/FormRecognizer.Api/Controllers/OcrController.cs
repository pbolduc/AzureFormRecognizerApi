using Azure.AI.FormRecognizer.Models;
using FormRecognizer.Services.AnalyzeForm;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using System.ComponentModel.DataAnnotations;

namespace FormRecognizer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OcrController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OcrController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Submits a form recognizer job.
        /// </summary>
        /// <param name="modelId">The model id</param>
        /// <param name="file">The file to OCR</param>
        /// <returns></returns>
        /// <response code="201">The job was submitted</response>
        /// <response code="500">There was a internal server error</response>
        [HttpPost]
        [ProducesResponseType(typeof(string), 201)]
        public async Task<IActionResult> AnalyzeFormAsync([Required] string modelId, [Required] IFormFile file)
        {
            using var stream = file.OpenReadStream();

            AnalyzeFormCommand command = new(modelId, stream);

            OneOf<AnalyzeFormOperationCreated, AnalyzeFormOperationError> response = await _mediator.Send(command);

            IActionResult result = response.Match(
                created => StatusCode(201, created.OperationId),
                error => this.StatusCode(500, error.Exception.Message));

            return result;
        }

        /// <summary>
        /// Gets the form recognizer job results. If the job is not complete status code 201 (Created) is returned with the operation id. If the job is complete, returns
        /// the OCR result
        /// </summary>
        /// <param name="operationId"></param>
        /// <returns></returns>
        /// <response code="200">The job is complete</response>
        /// <response code="201">The job is processing</response>
        [HttpGet]
        [ProducesResponseType(typeof(RecognizedForm), 200)]
        [ProducesResponseType(typeof(string), 201)]
        public async Task<IActionResult> GetAnalyzeFormResultAsync([Required] string operationId)
        {
            AnalyzeFormResultQuery query = new(operationId);

            OneOf<AnalyzeFormOperationCreated, AnalyzeFormOperationComplete, AnalyzeFormOperationError> response = await _mediator.Send(query);

            IActionResult result = response.Match(
                created => StatusCode(201, created.OperationId),
                complete => Ok(complete.Form),
                error => this.StatusCode(500, error.Exception.Message));

            return result;
        }
    }
}
