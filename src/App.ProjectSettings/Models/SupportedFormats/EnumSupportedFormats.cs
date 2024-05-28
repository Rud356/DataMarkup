using App.ProjectSettings.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.ProjectSettings.Models.SupportedFormats
{
    public enum EnumSupportedFormats
    {
        CocoDataset = 0,
    }

    internal static class SupportedFormats
    {
        public static EnumSupportedFormats FormatToEnum(IMarkupFormatter format)
        {
            if (format.GetType().ToString() == "CocoDatasetFormat")
            {
                    return EnumSupportedFormats.CocoDataset;
            }

            return EnumSupportedFormats.CocoDataset;
        }

        public static EnumSupportedFormats FormatToEnum(CocoDatasetFormat format)
        {
            return EnumSupportedFormats.CocoDataset;
        }
    }
}
