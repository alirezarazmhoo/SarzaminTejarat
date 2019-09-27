using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class MarketerTutorialFiles
    {
        public int Id { get; set; }
        public string FileUrl { get; set; }

        public string ImageUrl { get; set; }

        //[ForeignKey("MarketerTutorialId")]
        public int MarketerTutorialID { get; set; }

        public  MarketerTutorial MarketerTutorial { get; set; }
    }
}