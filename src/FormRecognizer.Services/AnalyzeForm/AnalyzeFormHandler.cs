using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OneOf;

namespace FormRecognizer.Services.AnalyzeForm;

internal class AnalyzeFormHandler : IRequestHandler<AnalyzeFormCommand, OneOf<AnalyzeFormOperationCreated, AnalyzeFormOperationError>>
{
    private readonly IFormRecognizerClient _client;

    public AnalyzeFormHandler(IFormRecognizerClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task<OneOf<AnalyzeFormOperationCreated, AnalyzeFormOperationError>> Handle(AnalyzeFormCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var operation = await _client.StartRecognizeCustomFormsAsync(request.ModelId, request.Form, cancellationToken)
                .ConfigureAwait(false);

            return new AnalyzeFormOperationCreated(operation.Id);

        }
        catch (Exception exception)
        {
            return new AnalyzeFormOperationError(exception);
        }
    }
}
