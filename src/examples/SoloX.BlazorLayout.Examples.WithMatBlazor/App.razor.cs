using MatBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoloX.BlazorLayout.Examples.WithMatBlazor
{
    public partial class App
    {
        private MatTheme Theme { get; } = new MatTheme()
        {
            Primary = MatThemeColors.Purple._700.Value,
            Secondary = MatThemeColors.Green._500.Value,
            Surface = MatThemeColors.LightBlue._100.Value,
        };
    }
}
