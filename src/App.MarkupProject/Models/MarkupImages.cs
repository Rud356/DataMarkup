using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using ReactiveUI;
using System.IO;
using App.MarkupProject.Models.Interfaces;

namespace App.MarkupProject.Models
{
    public class MarkupImage : ReactiveObject, IMarkupImage
    {
        private ObservableCollection<IMarkupFigure> _imagesFigures;

        public MarkupImage(string imagePath)
        {
            if (File.Exists(imagePath))
            {
                throw new FileNotFoundException();
            }

            string[] ALLOWED_EXTENSIONS = { ".jpeg", ".png" };
            if (!ALLOWED_EXTENSIONS.Contains<string>(Path.GetExtension(imagePath).ToLower()))
            {
                throw new FileFormatException("Not allowed file format loaded");
            }

            ImagePath = imagePath;
            _imagesFigures = new();

        }

        public MarkupImage(string imagePath, bool isIncludedInExport) : this(imagePath)
        {
            IsIncludedInExport = isIncludedInExport;
        }

        [Reactive] public string ImagePath { get; }

        [Reactive] public bool IsIncludedInExport { get; set; }

        [Reactive] public ObservableCollection<IMarkupFigure> Markup { get => _imagesFigures; }
    }
}
