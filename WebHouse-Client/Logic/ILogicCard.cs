using WebHouse_Client.Components;

namespace WebHouse_Client.Logic;

public interface ILogicCard
{
    public IComponentCard? Component { get; }
    public void CreateComponent();
}