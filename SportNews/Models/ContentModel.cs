using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SportNews.Models
{
    public class ContentModel
    {
        public int Stt { get; set; }
        public long news_id { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string title { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string descp { get; set; }

        [Display(Name = "Source")]
        public string source { get; set; }

        [Display(Name = "Date")]
        public string created_date { get; set; }

        [Display(Name = "Image")]
        public string imageLnk { get; set; }

        [Required]
        [Display(Name = "Category")]
        public long category_id { get; set; }
        public List<Category> catList { get; set; }
        public string cat_name { get; set; }

    }

    public class Category
    {
        public long category_id { get; set; }
        public string category_name { get; set; }
        public string created_date { get; set; }

    }

    public class newslist
    {
        public List<ContentModel> ctLst;
    }
}