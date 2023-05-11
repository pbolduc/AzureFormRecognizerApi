using MediatR;
using OneOf;

namespace FormRecognizer.Services.AnalyzeForm;

public class AnalyzeFormResultQuery : IRequest<OneOf<AnalyzeFormOperationCreated, AnalyzeFormOperationComplete, AnalyzeFormOperationError>>
{
    public AnalyzeFormResultQuery(string operationId)
    {
        OperationId = operationId;
    }

    public string OperationId { get; }
}
