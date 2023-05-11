using Azure.AI.FormRecognizer.Models;

namespace Microsoft.Extensions.DependencyInjection;

public interface IFormRecognizerClient
{
    Task<RecognizeCustomFormsOperation> StartRecognizeCustomFormsAsync(string modelId, Stream form, CancellationToken cancellationToken = default);

    RecognizeCustomFormsOperation CreateCustomFormsOperation(string operationId);

}


