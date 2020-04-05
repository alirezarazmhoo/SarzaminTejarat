using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
	public class Customer
	{
		[Key]
		public int Id { get; set; }

		[Required(ErrorMessage = "نام و نام خانوادگی خالی است")]
		public string FullName { get; set; }
		[Required(ErrorMessage = "نام و نام خانوادگی خالی است")]
		public string Address { get; set; }

		[MaxLength(15, ErrorMessage = "موبایل بسیار طولانی است")]
		public string Mobile { get; set; }
		[MaxLength(10, ErrorMessage = "کد پستی باید ده رقم باشد")]
		[MinLength(10, ErrorMessage = "کد پستی باید ده رقم باشد")]
		[Required(ErrorMessage = "کد پستی خالی است")]
		public string PostalCode { get; set; }

		[MaxLength(20, ErrorMessage = "تلفن بیش از حد معمول طولانی است")]
		[Required(ErrorMessage = "فیلد تلفن اجباری است")]
		public string Phone { get; set; }

		[Required(ErrorMessage = "فیلد مختصات افقی اجباری است")]
		public double Lat { get; set; }
		[Required(ErrorMessage = "فیلد مختصات عمودی اجباری است")]
		public double Lng { get; set; }

		public string IDCardPhotoAddress { get; set; }

		public string PersonalPicture { get; set; }

		[MaxLength(10, ErrorMessage = "کد ملی دقیقا باید ده رقم باشد")]
		[MinLength(10, ErrorMessage = "کد ملی دقیقا باید ده رقم باشد")]
		[Required(ErrorMessage = "فیلد شماره ملی اجباری است")]
		
		public string IDCardNumber { get; set; }

		[MaxLength(10, ErrorMessage = "شماره شناسنامه بیش از حد طولانی است")]
		[Required(ErrorMessage = "فیلد شماره شناسنامه اجباری است")]
		public string CertificateNumber { get; set; }

		[Required(ErrorMessage = "وضعیت تاهل را مشخص کنید")]
		public string IsMarrid { get; set; }

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
		public string IDCardPicture { get; set; }

		public DateTime CreatedDate { get; set; }


		public int MarketerUserId { get; set; }
		public MarketerUser MarketerUser { get; set; }

	}
}