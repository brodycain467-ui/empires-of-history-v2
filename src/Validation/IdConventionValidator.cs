using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace EmpiresOfHistoryV2.Validation;

public class IdConventionValidator
{
    private static readonly Dictionary<string, Regex> Patterns = new()
    {
        ["Nation"] = new Regex("^nat_[a-z0-9_]+$", RegexOptions.Compiled),
        ["Province"] = new Regex("^prov_[a-z0-9_]+$", RegexOptions.Compiled),
        ["County"] = new Regex("^county_[a-z0-9_]+$", RegexOptions.Compiled),
        ["City"] = new Regex("^city_[a-z0-9_]+$", RegexOptions.Compiled),
        ["Leader"] = new Regex("^leader_[a-z0-9_]+$", RegexOptions.Compiled),
        ["Government"] = new Regex("^gov_[a-z0-9_]+$", RegexOptions.Compiled),
        ["Religion"] = new Regex("^religion_[a-z0-9_]+$", RegexOptions.Compiled),
        ["PoliticalParty"] = new Regex("^party_[a-z0-9_]+$", RegexOptions.Compiled),
        ["Technology"] = new Regex("^tech_[a-z0-9_]+$", RegexOptions.Compiled),
        ["Resource"] = new Regex("^resource_[a-z0-9_]+$", RegexOptions.Compiled),
        ["Event"] = new Regex("^event_[a-z0-9_]+$", RegexOptions.Compiled),
        ["Company"] = new Regex("^company_[a-z0-9_]+$", RegexOptions.Compiled),
    };

    public bool IsValid(string type, string id) =>
        Patterns.TryGetValue(type, out var pattern) && pattern.IsMatch(id);

    public ValidationReport Validate(string type, IEnumerable<string> ids)
    {
        var report = new ValidationReport();
        if (!Patterns.TryGetValue(type, out var pattern))
        {
            report.AddError($"Unknown ID type '{type}'.", "id_type");
            return report;
        }

        foreach (var id in ids)
        {
            if (!pattern.IsMatch(id))
                report.AddError($"ID '{id}' does not match {type} convention.", type);
        }

        return report;
    }
}
