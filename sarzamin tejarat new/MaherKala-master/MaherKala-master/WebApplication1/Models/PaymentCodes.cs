using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
	public class PaymentCodes
	{
		public int Id { get; set; }
		public int CodeNumber { get; set; }
		public PaymentCodeType PaymentCodeType { get; set; }
	    public long Price { get; set; }
		public DateTime CreateDate { get; set; }
		public bool IsUsed { get; set; } = false;
		public int MarketerUserId { get; set; }
		public MarketerUser MarketerUser { get; set; }
	}
	public enum PaymentCodeType
	{
		[Display(Name ="کد نخفیف خرید چکی")]
		PaymentDiscountCode,
		[Display(Name = "کد نخفیف خرید اعتباری")]
		CreditDiscountCode,
		[Display(Name = "کد ورود به صفحه پرداخت چک")]
		CheckPaymenyLogin,
		[Display(Name = "کد ورود به صفحه پرداخت سفته")]
		CreditPaymentLogin,

	}
}