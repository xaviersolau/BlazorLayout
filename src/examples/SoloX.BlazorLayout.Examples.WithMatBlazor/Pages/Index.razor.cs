using MatBlazor;
using Microsoft.AspNetCore.Components;
using SoloX.BlazorLayout.Core;
using SoloX.BlazorLayout.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoloX.BlazorLayout.Examples.WithMatBlazor.Pages
{
    public partial class Index
    {
        [CascadingParameter]
        public MatTheme Theme { get; set; }

        [CascadingParameter]
        public ScreenSize ScreenSize { get; set; }

        [CascadingParameter]
        public ScrollInfo ScrollInfo { get; set; }

        private void UpdateTheme()
        {
            Theme.ThemeHasChanged();
        }
    }
}
