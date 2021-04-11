using MatBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoloX.BlazorLayout.Examples
{
    public partial class App
    {
        private MatTheme Theme { get; } = new MatTheme()
        {
            Surface = MatThemeColors.LightBlue._100.Value,
        };
    }
}
