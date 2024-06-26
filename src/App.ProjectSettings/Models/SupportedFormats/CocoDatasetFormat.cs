﻿using App.ProjectSettings.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using DynamicData.Tests;
using App.ProjectSettings.Models.Interfaces;
using App.ProjectSettings.Models.SupportedLoaderSources;

namespace App.ProjectSettings.Models.SupportedFormats
{
    public class CocoDatasetFormat : IMarkupFormatter
    {
        private CocoDatasetLoader _loader;
        public CocoDatasetFormat()
        {
            _loader = new CocoDatasetLoader();
        }

        public void saveMarkup(
            string saveTo,
            string ProjectName,
            IList<string> classes,
            IList<ImageDTO> imagesMarkup
        )
        {
            if (!File.Exists(saveTo))
            {
                var f = File.Create(saveTo);
                f.Close();
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

            // Adding categories to export
            int categoryID = 0;
            List<object> categories = new List<object>();
            foreach (string categoryName in classes)
            {
                object categoryInfo = new
                {
                    id = categoryID,
                    name = categoryName,
                    supercategory = ""
                };
                categories.Add(categoryInfo);
                categoryID++;
            }

            // TODO: finish implementation
            List<object> images = new List<object>();
            List<object> annotations = new List<object>();
            int imageCounter = 0;
            int markupFigureID = 0;
            foreach (ImageDTO img in imagesMarkup)
            {
                // Adding image to list
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
                

                foreach (MarkupDTO markup in img.MarkupDTOs)
                {
                    List<object> segmentation = new List<object>();
                    float area = 0;
                    List<float> bbox = new List<float>();

                    if (markup.MarkupType == MarkupFigureType.polygon)
                    {
                        var bottom = markup.Points.Max();
                        var top = markup.Points.Min();

                        if (top is null || bottom is null)
                            break;

                        foreach (Tuple<int, int> point in markup.Points)
                        {
                            List<float> pointPosition = new();
                            pointPosition.Add(point.Item1);
                            pointPosition.Add(point.Item2);
                            segmentation.Add(pointPosition);
                        }
                        bbox.Add(top.Item1);
                        bbox.Add(top.Item2);
                        int w = bottom.Item1 - top.Item1, h = bottom.Item2 - top.Item2;
                        bbox.Add(w);
                        bbox.Add(h);
                        
                        area = GetArea(markup.Points);
                    }
                    else if (markup.MarkupType == MarkupFigureType.bbox)
                    {
                        if (markup.Points.Count != 4)
                        {
                            break;
                        }
                        List<Tuple<int, int>> points = markup.Points.OrderBy(o => o).ToList();
                        Tuple<int, int> top = points.First();
                        Tuple<int, int> bottom = points.Last();
                        bbox.Add(top.Item1);
                        bbox.Add(top.Item2);
                        // According to https://github.com/cocodataset/cocoapi/issues/34
                        // it has to be width and height, so bottom - top coordinates, since 0, 0 is top left
                        // and all others are positive values from there
                        int w = bottom.Item1 - top.Item1, h = bottom.Item2 - top.Item2;
                        bbox.Add(w);
                        bbox.Add(h);
                        area = GetArea(new Tuple<int, int>(w, h));
                    }
                    else continue;

                    object annotation = new
                    {
                        id = markupFigureID,
                        image_id = imageCounter,
                        category_id = markup.AssignedClassID,
                        segmentation,
                        area,
                        bbox,
                        iscrowd = 0
                    };
                    annotations.Add(annotation);
                    markupFigureID++;
                }
                imageCounter++;
            }

            object projectsMarkup = new
            {
                license,
                info,
                categories,
                images,
                annotations
            };
            string output = JsonConvert.SerializeObject(projectsMarkup);
            File.WriteAllText(saveTo, output);
        }

        private static float GetDeterminant(float x1, float y1, float x2, float y2)
        {
            // from https://stackoverflow.com/questions/2034540/calculating-area-of-irregular-polygon-in-c-sharp
            // Gives close enough area for polygons
            return x1 * y2 - x2 * y1;
        }

        private static float GetArea(IList<Tuple<int, int>> vertices)
        {
            if (vertices.Count < 3)
            {
                return 0;
            }

            float area = GetDeterminant(vertices[vertices.Count - 1].Item1, vertices[vertices.Count - 1].Item2, vertices[0].Item1, vertices[0].Item2);
            for (int i = 1; i < vertices.Count; i++)
            {
                area += GetDeterminant(vertices[i - 1].Item1, vertices[i - 1].Item2, vertices[i].Item1, vertices[i].Item2);
            }
            return Math.Abs(area / 2);
        }

        private static float GetArea(Tuple<int, int> offsets)
        { 
            return offsets.Item1*offsets.Item2;
        }

        public IList<Tuple<string, Tuple<int, int>, IList<MarkupDTO>>> loadMarkup(string fromString)
        {
            return _loader.LoadMarkup(fromString);
        }
    }
}
