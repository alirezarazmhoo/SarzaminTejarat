using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Plannn
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "سطح پلن را مشخص کنید")]
        [DisplayName("سطح")]

        public int Level { get; set; }

        [DisplayName("توضیحات")]
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        [Required(ErrorMessage = "ارزش پلن را مشخص کنید")]
        [DisplayName("ارزش پلن")]
        public int Price { get; set; }
        [Required]
        [DisplayName("نوع پلن")]
        public int PlanTypeID { get; set; }

        public virtual PlanType PlanType { get; set; }

        public ICollection<MarketerUser> MarketerUsers { get; set; }
        public ICollection<MarketerImprovePlan> MarketerImprovePlans { get; set; }

    }
}