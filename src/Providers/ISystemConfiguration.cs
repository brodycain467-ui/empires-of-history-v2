using System.Collections.Generic;

namespace EmpiresOfHistoryV2.Providers;

public interface ISystemConfiguration
{
    void Configure(IReadOnlyDictionary<string, string> settings);
}
