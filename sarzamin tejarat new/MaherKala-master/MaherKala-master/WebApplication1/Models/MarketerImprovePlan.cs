using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class MarketerImprovePlan
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "ارزش پلن را مشخص کنید")]
        [DisplayName("پلن فعلی")]
        public int? CurrentPlanTypeId { get; set; }
        [DisplayName("سطح فعلی")]
        public int CurrentLevel { get; set; }
        [DisplayName("ارتقا به")]
        public int? CanGoPlanTypeId { get; set; }
        [DisplayName("در سطح")]
        public int LevelCanGo { get; set; }
        [DisplayName("با قیمت ")]
        public long Price { get; set; }

        public virtual PlanType CurrentPlanType { get; set; }
        public virtual PlanType CanGoPlanType { get; set; }




    }
}