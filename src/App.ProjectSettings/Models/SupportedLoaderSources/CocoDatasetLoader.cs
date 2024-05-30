using App.ProjectSettings.DTO;
using App.ProjectSettings.Exceptions;
using App.ProjectSettings.Models.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.ProjectSettings.Models.SupportedLoaderSources
{
    public class CocoDatasetLoader : IMarkupLoader
    {
        public IList<Tuple<string, Tuple<int, int>, IList<MarkupDTO>>> LoadMarkup(string markupText)
        {
            object obj = JsonConvert.DeserializeObject(markupText);
            CocoFormatDTO? markup = JsonConvert.DeserializeObject<CocoFormatDTO>(markupText);

            if (markup is null)
            {
                throw new MissingMarkupException();
            }

            List<Tuple<string, Tuple<int, int>, IList<MarkupDTO>>> images = new();
            foreach (var image in markup.images)
            {
                if (image is null)
                    continue;

                List<MarkupDTO> markupParsed = new();
                foreach (
                    Annotation annotation in markup.annotations
                    .Select(x => x).Where(x => x.image_id == image.id)
                )
                {
                    MarkupFigureType markupFigureType;
                    List<Tuple<int, int>> points = new();

                    if (annotation.segmentation is not null && annotation.segmentation.Count() > 0)
                    {
                        markupFigureType = MarkupFigureType.polygon;
                        foreach (var point in annotation.segmentation)
                        {
                            points.Add(new Tuple<int, int>((int)point[0], (int)point[1]));
                        }
                    }
                    else
                    {
                        markupFigureType = MarkupFigureType.bbox;
                        // Top left
                        points.Add(new Tuple<int, int>((int) annotation.bbox[0], (int) annotation.bbox[1]));
                        // Top right
                        points.Add(new Tuple<int, int>((int)(annotation.bbox[0] + annotation.bbox[2]), (int)annotation.bbox[1]));
                        // Bottom right
                        points.Add(new Tuple<int, int>((int)(annotation.bbox[0] + annotation.bbox[2]), (int)(annotation.bbox[1] + annotation.bbox[3])));
                        // Bottom left
                        points.Add(new Tuple<int, int>((int)(annotation.bbox[0] + annotation.bbox[2]), (int)(annotation.bbox[1] + annotation.bbox[3])));
                    }

                    markupParsed.Add(new MarkupDTO(annotation.category_id, markupFigureType, points));
                }

                var imageLoaded = new Tuple<string, Tuple<int, int>, IList<MarkupDTO>>(
                    image.file_name,
                    new Tuple<int, int>(image.width, image.height),
                    markupParsed
                );
                images.Add(imageLoaded);
            }

            return images;
        }
    }
}
