﻿using MatBlazor;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoloX.BlazorLayout.Examples.WithMatBlazor.Shared
{
    public partial class MainLayout
    {
        [CascadingParameter]
        public MatTheme Theme { get; set; }

        [Parameter]
        public bool MatchDisplaySize { get; set; }

        [Parameter]
        public bool SmallNav { get; set; }

        private void ToggleNav()
        {
            SmallNav = !SmallNav;
        }
    }
}
