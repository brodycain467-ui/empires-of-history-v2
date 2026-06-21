using System.Collections.Generic;
using System.Linq;

namespace EmpiresOfHistoryV2.Validation;

public enum ValidationSeverity
{
    Info,
    Warning,
    Error
}

public sealed record ValidationIssue(ValidationSeverity Severity, string Message, string Context);

public class ValidationReport
{
    private readonly List<ValidationIssue> _issues = [];

    public IReadOnlyList<ValidationIssue> Issues => _issues;
    public bool IsValid => _issues.All(i => i.Severity != ValidationSeverity.Error);

    public void AddInfo(string message, string context = "") => _issues.Add(new ValidationIssue(ValidationSeverity.Info, message, context));
    public void AddWarning(string message, string context = "") => _issues.Add(new ValidationIssue(ValidationSeverity.Warning, message, context));
    public void AddError(string message, string context = "") => _issues.Add(new ValidationIssue(ValidationSeverity.Error, message, context));
}
