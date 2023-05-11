using Azure.AI.FormRecognizer.Models;

namespace FormRecognizer.Services.AnalyzeForm;

public record AnalyzeFormOperationComplete(RecognizedForm Form);
