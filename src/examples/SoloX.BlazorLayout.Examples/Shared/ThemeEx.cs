using MatBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoloX.BlazorLayout.Examples.Shared
{
    public static class ThemeEx
    {
        public static string GetClass(this MatTheme theme, string classToAdd)
        {
            return $"{theme.GetClass()} {classToAdd}";
        }
    }
}
