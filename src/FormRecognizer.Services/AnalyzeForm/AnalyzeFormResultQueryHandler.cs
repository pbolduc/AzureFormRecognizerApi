using Azure.AI.FormRecognizer.Models;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OneOf;

namespace FormRecognizer.Services.AnalyzeForm;

internal class AnalyzeFormResultQueryHandler : IRequestHandler<AnalyzeFormResultQuery, OneOf<AnalyzeFormOperationCreated, AnalyzeFormOperationComplete, AnalyzeFormOperationError>>
{
    private readonly IFormRecognizerClient _client;

    public AnalyzeFormResultQueryHandler(IFormRecognizerClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task<OneOf<AnalyzeFormOperationCreated, AnalyzeFormOperationComplete, AnalyzeFormOperationError>> Handle(AnalyzeFormResultQuery request, CancellationToken cancellationToken)
    {
        RecognizeCustomFormsOperation operation = _client.CreateCustomFormsOperation(request.OperationId);

        await operation.UpdateStatusAsync(cancellationToken)
            .ConfigureAwait(false);

        var response = CreateResponse(operation);

        return response;
    }

    private static OneOf<AnalyzeFormOperationCreated, AnalyzeFormOperationComplete, AnalyzeFormOperationError> CreateResponse(RecognizeCustomFormsOperation operation)
    {
        if (!operation.HasCompleted)
        {
            return new AnalyzeFormOperationCreated(operation.Id);
        }

        if (operation.HasValue && operation.Value.Count != 0)
        {
            return new AnalyzeFormOperationComplete(operation.Value[0]);
        }

        // not sure why we would get here
        return new AnalyzeFormOperationError(new InvalidOperationException("Operation is complete, but there is no form response"));
    }
}
