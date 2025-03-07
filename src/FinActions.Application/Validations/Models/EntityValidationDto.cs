namespace FinActions.Application.Validations.Models;

public sealed record EntityValidationDto (string title, string description, string type, int statusCode);
