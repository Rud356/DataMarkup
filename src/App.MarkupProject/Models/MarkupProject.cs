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

namespace App.MarkupProject.Models
{
    public class MarkupProject : ReactiveObject, IMarkupProject
    {
        private ObservableCollection<IMarkupImage> _images;

        public MarkupProject(IProjectConfigLoader configLoader)
        {
            ConfigLoader = configLoader;
            _images = new();
        }

        [Reactive]
        public IProjectConfigLoader ConfigLoader { get; }

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

        public void ExportProject()
        {
            List<ImageDTO> images = new();

            foreach (var image in Images) 
            {
                List<MarkupDTO> markupDTOs = new();

                var rectangles = image.Markup.OfType<Rectangle>().ToList();
                var polygons = image.Markup.OfType<Polygon>().ToList();

                foreach (var polygon in polygons)
                {
                    markupDTOs.Add(
                        new MarkupDTO(
                            polygon.AssignedClassID,
                            MarkupFigureType.polygon,
                            polygon.Points
                        )
                    );
                }

                foreach (var rectangle in rectangles)
                {
                    markupDTOs.Add(
                        new MarkupDTO(
                            rectangle.AssignedClassID,
                            MarkupFigureType.bbox,
                            rectangle.Points
                        )
                    );
                }

                ImageDTO imgDTO = new(
                    image.ImagePath,
                    image.Resolution,
                    markupDTOs
                );
            }

            Formatter.saveMarkup(
                Path.Combine(ConfigLoader.ProjectConfigObj.ProjectConfigPath, "instance_default.json"),
                ConfigLoader.ProjectConfigObj.ProjectName,
                ConfigLoader.ProjectConfigObj.MarkupClasses.ToList(),
                images
            );
        }
    }
}
