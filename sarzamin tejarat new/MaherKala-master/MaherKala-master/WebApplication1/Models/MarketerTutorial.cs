using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class MarketerTutorial
    {
        public int Id { get; set; }
        //[Column(TypeName = "varchar(MAX)")]
   
        [DisplayName("توضیحات")]
        public string Description { get; set; }

        public ICollection<MarketerTutorialFiles> MarketerTutorialFiles { get; set; }
    }
}