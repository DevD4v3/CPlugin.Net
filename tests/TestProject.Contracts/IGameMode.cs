namespace TestProject.Contracts;

public interface IGameMode
{
    public string Name { get; }
    string ExecuteAction();
}
