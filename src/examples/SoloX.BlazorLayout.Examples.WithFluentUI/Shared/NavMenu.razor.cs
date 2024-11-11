using Microsoft.AspNetCore.Components;

namespace SoloX.BlazorLayout.Examples.WithFluentUI.Shared
{
    public partial class NavMenu
    {
        [Parameter]
        public bool SmallNav { get; set; }

        private string ComputeStyle()
        {
            return SmallNav ? "padding: 2px 0;" : "padding: 2px 8px;";
        }

    }
}
