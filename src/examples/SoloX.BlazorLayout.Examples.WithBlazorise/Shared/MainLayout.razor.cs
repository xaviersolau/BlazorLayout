using Blazorise;
using Microsoft.AspNetCore.Components;
using SoloX.BlazorLayout.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoloX.BlazorLayout.Examples.WithBlazorise.Shared
{
    public partial class MainLayout
    {
        [CascadingParameter]
        public Theme Theme { get; set; }

        [Parameter]
        public bool MatchDisplaySize { get; set; }
        [Parameter]
        public bool HideHeader { get; set; }
        [Parameter]
        public bool HideFooter { get; set; }

        [Parameter]
        public bool SmallNav { get; set; }

        private void ToggleNav()
        {
            SmallNav = !SmallNav;
        }
    }
}
