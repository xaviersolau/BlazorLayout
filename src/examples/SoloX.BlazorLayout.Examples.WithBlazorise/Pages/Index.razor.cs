﻿using Blazorise;
using Microsoft.AspNetCore.Components;
using SoloX.BlazorLayout.Core;
using SoloX.BlazorLayout.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoloX.BlazorLayout.Examples.WithBlazorise.Pages
{
    public partial class Index
    {
        [CascadingParameter]
        public Theme Theme { get; set; }

        [CascadingParameter]
        public ScreenSize ScreenSize { get; set; }

        [CascadingParameter]
        public ScrollInfo ScrollInfo { get; set; }

        private void UpdateTheme()
        {
            Theme.ColorOptions = new ThemeColorOptions()
            {
                Primary = Theme.BackgroundOptions.Primary,
                Secondary = Theme.BackgroundOptions.Secondary,
                Dark = Theme.BackgroundOptions.Dark,
                Light = Theme.BackgroundOptions.Light,
            };
            Theme.TextColorOptions = new ThemeTextColorOptions()
            {
                Primary = Theme.BackgroundOptions.Primary,
                Secondary = Theme.BackgroundOptions.Secondary,
                Dark = Theme.BackgroundOptions.Dark,
                Light = Theme.BackgroundOptions.Light,
            };

            Theme.ThemeHasChanged();
        }

    }
}
