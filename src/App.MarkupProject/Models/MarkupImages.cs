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
using System.Windows.Controls;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace App.MarkupProject.Models
{
    public class MarkupImage : ReactiveObject, IMarkupImage
    {
        private ObservableCollection<IMarkupFigure> _imagesFigures;

        public MarkupImage(string imagePath)
        {
            Resolution = GetResolution(imagePath);
            ImagePath = imagePath;
            IsIncludedInExport = true;
            _imagesFigures = new();
        }

        public MarkupImage(string imagePath, bool isIncludedInExport) : this(imagePath)
        {
            IsIncludedInExport = isIncludedInExport;
        }

        public MarkupImage(
            string imagePath,
            bool isIncludedInExport,
            IList<IMarkupFigure> imagesFigures
        ) : this(imagePath, isIncludedInExport, imagesFigures, GetResolution(imagePath))
        { }

        public MarkupImage(
            string imagePath,
            bool isIncludedInExport,
            IList<IMarkupFigure> imagesFigures,
            Tuple<int, int> resolution
        )
        {
            Resolution = resolution;
            ImagePath = imagePath;
            _imagesFigures = new(imagesFigures);
        }

        [Reactive] public string ImagePath { get; }

        [Reactive] public bool IsIncludedInExport { get; set; }

        [Reactive] public ObservableCollection<IMarkupFigure> Markup { get => _imagesFigures; }
        public Tuple<int, int> Resolution { get; }

        private static Tuple<int, int> GetResolution(string path)
        {
            if (File.Exists(path))
            {
                throw new FileNotFoundException();
            }

            string[] ALLOWED_EXTENSIONS = { ".jpeg", ".png" };
            if (!ALLOWED_EXTENSIONS.Contains<string>(Path.GetExtension(path).ToLower()))
            {
                throw new FileFormatException("Not allowed file format loaded");
            }

            var img = Bitmap.FromFile(path);
            int docHeight = (int) (img.Height / img.VerticalResolution);
            int docWidth = (int) (img.Width / img.HorizontalResolution);

            return new Tuple<int, int>(docWidth, docHeight);
        }
    }
}
