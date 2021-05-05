using MatBlazor;
using Microsoft.AspNetCore.Components;
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

        private void UpdateTheme()
        {
            Theme.ThemeHasChanged();
        }
    }
}
