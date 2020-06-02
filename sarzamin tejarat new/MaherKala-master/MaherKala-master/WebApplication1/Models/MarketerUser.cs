using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class MarketerUser
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "فیلد نام اجباری است")]
        [MaxLength(30, ErrorMessage = "نام بیش از حد معمول طولانی است")]
        public string Name { get; set; }
        [Required(ErrorMessage = "فیلد نام خانوادگی اجباری است")]
        [MaxLength(30, ErrorMessage = "نام خانوادگی بیش از حد معمول طولانی است")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "فیلد موبایل اجباری است")]
        [MaxLength(11, ErrorMessage = "شماره موبایل باید دقیقا یازده رقم باشد")]
        [MinLength(11, ErrorMessage = "شماره موبایل باید دقیقا یازده رقم باشد")]
        public string Mobile { get; set; }
        [MaxLength(20, ErrorMessage = "تلفن بیش از حد معمول طولانی است")]
        [Required(ErrorMessage = "فیلد تلفن اجباری است")]
        public string Phone { get; set; }
        [MaxLength(300)]
        public string Password { get; set; }
        [Required(ErrorMessage = "فیلد آدرس اجباری است")]
        [MaxLength(300, ErrorMessage = "آدرس بیش از حد معمول طولانی است")]
        public string Address { get; set; }
        [Required(ErrorMessage = "فیلد مختصات افقی اجباری است")]
        public double Lat { get; set; }
        [Required(ErrorMessage = "فیلد مختصات عمودی اجباری است")]
        public double Lng { get; set; }
        [MaxLength(300)]
        public string IDCardPhotoAddress { get; set; }
        [MaxLength(300)]
        public string PersonalPicture { get; set; }
        [MaxLength(10, ErrorMessage = "کد ملی دقیقا باید ده رقم باشد")]
        [MinLength(10, ErrorMessage = "کد ملی دقیقا باید ده رقم باشد")]
        [Required(ErrorMessage = "فیلد شماره ملی اجباری است")]
        public string IDCardNumber { get; set; }
        [MaxLength(10, ErrorMessage = "شماره شناسنامه بیش از حد طولانی است")]
        [Required(ErrorMessage = "فیلد شماره شناسنامه اجباری است")]
        public string CertificateNumber { get; set; }
        [Required(ErrorMessage = "وضعیت تاهل را مشخص کنید")]
        public bool IsMarrid { get; set; }
        [MaxLength(50, ErrorMessage = "شماره حساب بیش از حد معمول طولانی است")]
        [Required(ErrorMessage = "فیلد شماره حساب اجباری است")]
        public string AccountNumber { get; set; }

        [MaxLength(16, ErrorMessage = "شماره کارت دقیقا باید 16 رقم باشد")]
        [MinLength(16, ErrorMessage = "شماره کارت دقیقا باید 16 رقم باشد")]
        [Required(ErrorMessage = "فیلد شماره کارت اجباری است")]
        public string CardAccountNumber { get; set; }
        [MaxLength(24, ErrorMessage = "شماره شبا  دقیقا باید 24رقم باشد")]
        [MinLength(24, ErrorMessage = "شماره شبا دقیقا باید 24رقم باشد")]
        [Required(ErrorMessage = "فیلد شماره شبا اجباری است")]
        public string IBNA { get; set; }
        [MaxLength(300, ErrorMessage = "توضیحات بیش از حد طولانی است")]
        public string Description { get; set; }

        public string Api_Token { get; set; }
        public bool IsAvailable { get; set; }
        public int Parent_Id { get; set; }
        public bool IsFirstTime { get; set; }
        public int FactorCounter { get; set; }


        public byte Usertype { get; set; }

        //کدمعرف
        public string PersonalReagentCode { get; set; }
        //وضعیت تایید توسط پدرش
        public bool AcceptedByParent { get; set; }

        //تعداد زیرمجموعه
        public int SubsetCount { get; set; }


        public int? PlannnID { get; set; }
        public virtual Plannn Plannn { get; set; }

        //زمان ثبت نام 
        public DateTime CreatedDate { get; set; }

        public bool Islazy { get; set; }

        public string TextAddress { get; set; }

        public bool CanCheckPayment { get; set; } = false;

        public bool CanPromissoryPayment { get; set; } = false;

        public bool AlreadySmsSent { get; set; } = false; 
             


    }
}