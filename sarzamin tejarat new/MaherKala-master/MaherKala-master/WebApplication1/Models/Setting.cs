using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Models
{
    public class Setting
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="متن درباره ما را وارد کنید")]
        [AllowHtml]
        public string AboutUs { get; set; }
        [Required(ErrorMessage = "متن درباره ما را وارد کنید")]
        [AllowHtml]
        public string CallUs { get; set; }
        [Required(ErrorMessage = "متن درباره ما را وارد کنید")]
        [AllowHtml]
        public string HowToWork { get; set; }
        public int FirstCategory { get; set; }
        public int SecoundCategory { get; set; }
        public string Notificaion { get; set; }
        public string NotificaionUrl { get; set; }
        public string Email { get; set; }
        public string EmailPassword { get; set; }
        public string HostName { get; set; }
        public string Domain{ get; set; }
        public string SiteName { get; set; }
        public int? TransportationEsfahan { get; set; }
        public int? TransportationNajafabad { get; set; }
        public int? TransportationOther { get; set; }
    }
}