namespace EmpiresOfHistoryV2.Providers;

public interface ISystemProvider
{
    string SystemId { get; }
    string? GetValue(string key);
}
