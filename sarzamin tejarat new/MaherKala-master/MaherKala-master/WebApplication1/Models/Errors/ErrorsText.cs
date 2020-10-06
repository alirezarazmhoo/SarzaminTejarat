using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.Errors
{
	public class ErrorsText
	{
		public const string MarketerNotFound  ="بازاریاب مورد نظر یافت نشد";
		public const string RepeatError = "اطلاعات تکراری است";
		public const string IDCardNumberError = "شماره ملی تکراری است";
		public const string MobileError = "شماره همراه تکراری است";
		public const string MinPictureError = "عکس خراب است";
		public const string MaxPictureError = "فایل بیش از حد بزرگ است";
		public const string UserNotFound = "کاربر مورد نظر یافت نشد";
		public const string EmptyMarketerUserId = "ای دی بازاریاب خالی است";
		public const string BadData = "اطلاعات ناقص است";
		public const string EmptyToken = "توکن خالی است";
		public const string ProductNotFound = "کالای مورد نظر یافت نشد";
		public const string UndefindedUserType = "نوع کاربر نامعتبر است";
		public const string ConflitId = "ای دی داخل یوارال با ای دی مشتری باید برابر باشد";
		public const string PaySettingError = "حداقل قیمت از حداکثر  قیمت پرداخت حضوری  بیشتر است" ;
		public const string PaySettingError2 = "حداقل و حداکثر قیمت های پرداخت حضوری رعایت نشده است";
		public const string IncorrectInputNumber = "ورودی قیمت ها صحیح نیست لطفا فقط عدد وارد کنید";
		public const string SystemError = "در حال حاضر انجام عملیات مقئور نمی باشد ";
		public const string MobileEmptyError = "شماره همراه خالی است";
		public const string MobileIncorrectTypeError = "شماره همراه صحیح وارد نشده";
		public const string MobileNotUserFoundByThisNumber = "کاربری با این شماره همراه یافت نشد";
		public const string Forbiden = "شما مجوز انجام این عملیات را ندارید";
		public const string CustomerIdisIncorrect = "ای دی مشتری فقط باید عدد باشد";
		public const string CountIsZero = "تعداد کالا نمی تواند صفر باشد";
		public const string CantSendSms = "ارسال پیامک مقدور نیست لطفا از داخل آپ کد را مشاهده کنید";
		public const string EmptyFactorId = "ای دی فاکتور خالی است";
		public const string EmptyDiscountCode = "کد تخفیف خالی است";
		public const string WrongDiscountCode = "کد تخفیف اشتباه است";
		public const string FactorNotFound = "فاکتور مورد نظر یافت نشد";
		public const string expiredDiscountCode = "کد تخفیف منقضی شده است";
		public const string OwnerDiscountCodeError = "استفاده از این کد تنها برای صاحب آن مجاز است";
		public const string InvalidId = "ای دی غیر عددی است";
		public const string EmptyPass = "پسوردخالی است";
		public const string PassIncorrectTypeError = "فرمت پسورد غیرعددی است";
		public const string InCorrrectInformations = "اطلاعات وارد شده نامعتبر است";
		public const string expiredLoginCode = "کد ورود منقضی شده است";
		public const string CodeAccepted = "کد مورد نظر تایید شد";
		public const string SmsNotSendt = "ارسال پیامک مقدور نیست";
		public const string SmsNotSendtButSaved = "ارسال پیامک مقدور نیست اما کد تخفیف برای این کاربر ثبت شد";
		public const string EmptyMessageText = "متن پیام خالی است";




	}


	public class SucccessText
	{
		public const string Created = "بازاریاب مورد نظر یافت نشد";
		public const string SmsSent = "ارسال پیامک با موفقیت انجام شد ";

	}
}