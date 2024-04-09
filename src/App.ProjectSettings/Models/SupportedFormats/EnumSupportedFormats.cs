using App.MarkupProject.Models.Interfaces;
using App.MarkupProject.Models.SupportedFormats;
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
        public static EnumSupportedFormats formatToEnum(IMarkupFormatter format)
        {
            throw new NotImplementedException();
        }

        public static EnumSupportedFormats formatToEnum(CocoDatasetFormat format)
        {
            return EnumSupportedFormats.CocoDataset;
        }
    }
}
