namespace EmpiresOfHistoryV2.Providers;

public interface ISystemSerializer
{
    string Serialize();
    void Deserialize(string json);
}
