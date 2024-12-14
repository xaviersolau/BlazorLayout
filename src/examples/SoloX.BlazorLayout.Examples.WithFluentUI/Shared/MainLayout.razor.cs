using Microsoft.AspNetCore.Components;

namespace SoloX.BlazorLayout.Examples.WithFluentUI.Shared
{
    public partial class MainLayout
    {
        [Parameter]
        public bool MatchDisplaySize { get; set; }
        [Parameter]
        public bool HideHeader { get; set; }
        [Parameter]
        public bool HideFooter { get; set; }

        [Parameter]
        public bool DisableHorizontalNavScroll { get; set; }

        [Parameter]
        public bool SmallNav { get; set; }

        private void ToggleNav()
        {
            SmallNav = !SmallNav;
        }
    }
}
