using Microsoft.AspNetCore.Components;

namespace BugTrackerUI.UIInterfaces
{
    public interface ITab
    {
        RenderFragment ChildContent { get; }
    }
}
