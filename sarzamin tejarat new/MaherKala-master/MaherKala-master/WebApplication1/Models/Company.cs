using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Company
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="نام شرکت خالی است")]
        [DisplayName("نام شرکت")]
        public string Name { get; set; }
        [Required(ErrorMessage = "آدرس شرکت خالی است")]
        [DisplayName(" آدرس")]
        public string Address { get; set; }
        [Required(ErrorMessage = "شناسه ملی شرکت خالی است")]
        [DisplayName(" شناسه ملی")]
        public long NationalCode { get; set; }
        [Required(ErrorMessage = " تلفن ثابت شرکت خالی است")]
        [DisplayName(" تلفن ثابت ")]
        public long PhoneNumber { get; set; }
        [Required(ErrorMessage = "استان خالی است")]
        [DisplayName(" استان  ")]
        public string cityName { get; set; }
        [Required(ErrorMessage = "شهرستان خالی است")]
        [DisplayName(" شهرستان  ")]

        public string subCityName { get; set; }

            }
}