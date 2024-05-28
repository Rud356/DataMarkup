using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Shared
{
    public static class Regions
    {
        public const string AppStartRegion = nameof(AppStartRegion);
        public const string MainRegion = nameof(MainRegion);
        public const string SettingsRegion = nameof(SettingsRegion);
    }

    public static class Navigation
    {
        public const string AboutPage = "AboutView";
        public const string MarkupPage = "MarkupWindow";
        public const string MarkupProjectSettings = "ProjectSettingsWindow";
    }
}
