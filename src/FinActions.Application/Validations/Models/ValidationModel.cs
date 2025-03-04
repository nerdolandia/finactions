namespace FinActions.Application.Validations.Models;
public sealed class ValidationModel
{
    public string title { get; set; }
    public Dictionary<string, string[]> errors = new();
}

