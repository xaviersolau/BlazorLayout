using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoloX.BlazorLayout.Examples.WithBlazorise.Shared
{
    public partial class NavMenuItem
    {
        [Parameter]
        public bool SmallNav { get; set; }

        [Parameter]
        public string Href { get; set; }

        [Parameter]
        public string Icon { get; set; }

        [Parameter]
        public string Label { get; set; }

        [Parameter]
        public NavLinkMatch NavLinkMatch { get; set; }
    }
}
