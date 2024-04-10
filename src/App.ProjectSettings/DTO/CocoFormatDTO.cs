using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.ProjectSettings.DTO
{
    internal class License
    {
        public string name { get; set; }
        public int id { get; set; }
        public string url { get; set; }

        public License(string name, int id, string url)
        {
            this.name = name;
            this.id = id;
            this.url = url;
        }
    }

    internal class Info
    {
        public string contributor { get; set; }
        public string date_created { get; set; }
        public string description { get; set;}
        public string url { get; set;}
        public string version {  get; set; }
        public string year { get; set; }

        public Info(string contributor, string date_created, string description, string url, string version, string year)
        {
            this.contributor = contributor;
            this.date_created = date_created;
            this.description = description;
            this.url = url;
            this.version = version;
            this.year = year;
        }
    }

    internal class Category
    {
        public int id { get; set; }
        public string name { get; set; }
        public string supercategory { get; set;}

        public Category(int id, string name, string supercategory)
        {
            this.id = id;
            this.name = name;
            this.supercategory = supercategory;
        }
    }

    internal class Image
    {
        public int id { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string file_name { get; set; }
        public int license {  get; set; }
        public string flickr_url { get; set;}
        public string coco_url { get; set; }
        public int date_captured { get; set;}

        public Image(int id, int width, int height, string file_name, int license, string flickr_url, string coco_url, int date_captured)
        {
            this.id = id;
            this.width = width;
            this.height = height;
            this.file_name = file_name;
            this.license = license;
            this.flickr_url = flickr_url;
            this.coco_url = coco_url;
            this.date_captured = date_captured;
        }
    }

    internal class Annotation
    {
        public int id { get; set; }
        public int image_id { get; set; }
        public int category_id { get; set; }
        public List<List<float>> segmenation { get; set; }
        public float area { get; set; }
        public List<float> bbox { get; set; }
        public bool iscword { get; set; }
        public JObject attributes { get; set; }

        public Annotation(int id, int image_id, int category_id, List<List<float>> segmenation, float area, List<float> bbox, bool iscword, JObject attributes)
        {
            this.id = id;
            this.image_id = image_id;
            this.category_id = category_id;
            this.segmenation = segmenation;
            this.area = area;
            this.bbox = bbox;
            this.iscword = iscword;
            this.attributes = attributes;
        }
    }

    internal class CocoFormatDTO
    {
        public List<License> licenses { get; set; }
        public Info info { get; set; }
        public List<Category> categories { get; set; }
        public List<Image> images { get; set; }

        public List<Annotation> annotations { get; set; }

        public CocoFormatDTO(List<License> licenses, Info info, List<Category> categories, List<Image> images, List<Annotation> annotations)
        {
            this.licenses = licenses;
            this.info = info;
            this.categories = categories;
            this.images = images;
            this.annotations = annotations;
        }
    }
}
