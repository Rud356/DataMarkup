using App.MarkupProject.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using App.ProjectSettings.DTO;
using System.Collections.ObjectModel;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using App.ProjectSettings.Models.Interfaces;

namespace App.MarkupProject.Models
{
    public class MarkupProject : ReactiveObject, IMarkupProject
    {
        private ObservableCollection<IMarkupImage> _images;

        public MarkupProject(IProjectConfigLoader configLoader)
        {
            ConfigLoader = configLoader;
            _images = new();
            loadImages();
        }

        IProjectConfigLoader _projConf;

        [Reactive]
        public IProjectConfigLoader ConfigLoader {
            get {  return _projConf; }
            set
            {
                this.RaiseAndSetIfChanged(ref _projConf, value);
            }
        }

        [Reactive]
        public IMarkupFormatter Formatter
        {
            get => ConfigLoader.ProjectConfigObj.DataFormat;
            set => ConfigLoader.ProjectConfigObj.DataFormat = value;
        }

        [Reactive] 
        public ObservableCollection<IMarkupImage> Images => _images;

        [Reactive]
        public string Name
        {
            get => ConfigLoader.ProjectConfigObj.ProjectName;
            set => ConfigLoader.ProjectConfigObj.ProjectName = value;
        }

        [Reactive]
        public ref ObservableCollection<string> Labels
        {
            get => ref ConfigLoader.ProjectConfigObj.MarkupClasses;
        }
        public void ExportProject()
        {
            List<ImageDTO> images = new();

            foreach (var image in Images) 
            {
                List<MarkupDTO> markupDTOs = new();

                var rectangles = new List<Rectangle>();
                var polygons = new List<Polygon>();

                foreach (var markup in image.Markup)
                {
                    if (markup == null)
                        continue;

                    if (markup.AssignedClassID == -1)
                        continue;

                    if (markup is Rectangle)
                    {
                        rectangles.Add((Rectangle)markup);
                    }
                    else
                    {
                        polygons.Add((Polygon)markup);
                    }
                }

                foreach (var polygon in polygons)
                {
                    markupDTOs.Add(
                        new MarkupDTO(
                            polygon.AssignedClassID,
                            MarkupFigureType.polygon,
                            polygon.Points.Select(v => v.ToTuple()).ToList()
                        )
                    );
                }

                foreach (var rectangle in rectangles)
                {
                    markupDTOs.Add(
                        new MarkupDTO(
                            rectangle.AssignedClassID,
                            MarkupFigureType.bbox,
                            rectangle.Points.Select(v => v.ToTuple()).ToList()
                        )
                    );
                }

                ImageDTO imgDTO = new(
                    image.ImagePath,
                    image.Resolution,
                    markupDTOs
                );
                images.Add(imgDTO);
            }

            Formatter.saveMarkup(
                Path.Combine(ConfigLoader.ProjectConfigObj.ProjectPath, "instance_default.json"),
                ConfigLoader.ProjectConfigObj.ProjectName,
                ConfigLoader.ProjectConfigObj.MarkupClasses.ToList(),
                images
            );
        }

        private void loadImages()
        {
            List<string> passedImages = new();
            string markupPath = Path.Combine(ConfigLoader.ProjectConfigObj.ProjectPath, "instance_default.json");
            if (File.Exists(markupPath))
            {
                try
                {
                    var markup = ConfigLoader.ProjectConfigObj.DataFormat.loadMarkup(File.ReadAllText(markupPath));

                    foreach ( var imageMarkup in markup )
                    {
                        List<IMarkupFigure> figures = new();

                        foreach (var fig in imageMarkup.Item3)
                        {
                            if (fig.MarkupType == MarkupFigureType.polygon)
                            {
                                Polygon poly = new(ref Labels);

                                foreach (var p in fig.Points)
                                {
                                    poly.Points.Add(new Vertex(p.Item1, p.Item2));
                                }
                                poly.AssignedClass = Labels[fig.AssignedClassID];
                                figures.Add(poly);
                            }
                            else if (fig.MarkupType== MarkupFigureType.bbox)
                            {
                                figures.Add(
                                    new Rectangle(
                                        ref Labels,
                                        new Tuple<int, int, int, int>(
                                            // Bbox contains 4 points in dto, which have clock-wise order of points (x, y) pixels
                                            fig.Points[0].Item1, fig.Points[0].Item2,
                                            fig.Points[3].Item1, fig.Points[3].Item2
                                        ), fig.AssignedClassID
                                    )
                                );
                            }
                            else continue;
                        }

                        Images.Add(new MarkupImage(
                            Path.Combine(ConfigLoader.ProjectConfigObj.ProjectPath, imageMarkup.Item1),
                            true,
                            figures,
                            imageMarkup.Item2
                        ));
                        passedImages.Add(Path.GetFileName(imageMarkup.Item1));
                    }
                }

                catch
                { }
            }

            foreach (var file in Directory.GetFiles(ConfigLoader.ProjectConfigObj.ProjectPath))
            {
                try
                {
                    if (passedImages.Contains(Path.GetFileName(file)))
                    {
                        continue;
                    }

                    MarkupImage img = new(file);
                    Images.Add(img);
                }
                catch (FileFormatException)
                {
                    continue;
                }
            }
        }
    }
}
