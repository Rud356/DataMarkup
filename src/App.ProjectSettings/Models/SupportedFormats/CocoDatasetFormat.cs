using App.MarkupProject.Models.Interfaces;
using App.ProjectSettings.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace App.MarkupProject.Models.SupportedFormats
{
    public class CocoDatasetFormat : IMarkupFormatter
    {
        public CocoDatasetFormat() {}

        public void saveMarkup(
            string saveTo,
            string ProjectName,
            IList<string> classes,
            IList<ImageDTO> imagesMarkup
        )
        {
            if (!File.Exists(saveTo))
            {
                File.Create(saveTo);
            }

            object license = new
            {
                name = "",
                id = 0,
                url = ""
            };
            object info = new
            {
                contributor = "",
                date_created = "",
                description = "",
                url = "",
                version = "",
                year = ""
            };

            // TODO: add categories list for mapping classes to
            // TODO: finish implementation
            List<object> images = new List<object>();
            List<object> annotations = new List<object>();
            int imageCounter = 0;
            foreach (ImageDTO img in imagesMarkup)
            {
                object image = new
                {
                    id = imageCounter,
                    width = img.Resolution.Item1,
                    height = img.Resolution.Item2,
                    file_name = Path.GetFileName(img.ImagePath),
                    license = 0,
                    flickr_url = "",
                    coco_url = "",
                    date_captured = 0
                };
                images.Add(image);

                object annotation = new
                {
                    id = imageCounter,
                };
                imageCounter++;
                annotations.Add(annotation);
            }
            
        }
    }
}
