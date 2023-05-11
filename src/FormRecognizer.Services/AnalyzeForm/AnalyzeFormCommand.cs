using MediatR;
using OneOf;

namespace FormRecognizer.Services.AnalyzeForm;

public class AnalyzeFormCommand : IRequest<OneOf<AnalyzeFormOperationCreated, AnalyzeFormOperationError>>
{
    public AnalyzeFormCommand(string modelId, Stream form)
    {
        ModelId = modelId ?? throw new ArgumentNullException(nameof(modelId));
        Form = form ?? throw new ArgumentNullException(nameof(form));
    }

    public string ModelId { get; }
    public Stream Form { get; }
}
