using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace SoloX.BlazorLayout.Examples.WithFluentUI.Shared
{
    public partial class NavMenuItem
    {
        [Parameter]
        public bool SmallNav { get; set; }

        [Parameter]
        public string Href { get; set; }

        [Parameter]
        public Icon Icon { get; set; }

        [Parameter]
        public string Label { get; set; }

        [Parameter]
        public NavLinkMatch NavLinkMatch { get; set; }
    }
}
