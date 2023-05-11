using Azure;
using Azure.AI.FormRecognizer;
using Azure.AI.FormRecognizer.Models;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

internal class FormRecognizerClientWrapper : IFormRecognizerClient
{
    private readonly FormRecognizerConfiguration _configuration;

    public FormRecognizerClientWrapper(IOptions<FormRecognizerConfiguration> configuration)
    {
        _configuration = configuration?.Value ?? throw new ArgumentNullException(nameof(configuration));
    }
    
    public RecognizeCustomFormsOperation CreateCustomFormsOperation(string operationId)
    {
        FormRecognizerClient client = GetClient();
        var operation = new RecognizeCustomFormsOperation(operationId, client);
        return operation;
    }

    public async Task<RecognizeCustomFormsOperation> StartRecognizeCustomFormsAsync(string modelId, Stream form, CancellationToken cancellationToken = default)
    {
        FormRecognizerClient client = GetClient();
        var operation = await client.StartRecognizeCustomFormsAsync(modelId, form, null, cancellationToken).ConfigureAwait(false);
        return operation;
    }

    private FormRecognizerClient GetClient()
    {
        var credential = new AzureKeyCredential(_configuration.ApiKey);
        var client = new FormRecognizerClient(_configuration.Endpoint, credential);
        return client;
    }
}
