namespace FinActions.Application.Validations.Models;
public sealed class ModelValidationDto
{
    public string title { get; set; }
    public Dictionary<string, string[]> errors { get; init; } = [];
};

