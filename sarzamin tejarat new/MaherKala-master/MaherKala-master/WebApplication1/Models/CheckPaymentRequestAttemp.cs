using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
	public class CheckPaymentRequestAttemp
	{
		public int Id { get; set; }
		[DisplayName("وضعیت")]
		public CheckPaymentRequestAttempStatus CheckPaymentRequestAttempStatus { get; set; }
		[DisplayName("تاریخ ثبت")]
		public DateTime CreatedDate { get; set; }
		[DisplayName("نظر ادمین")]
		public string AdminComment { get; set; }
		public int MarketerUserId { get; set; }
		public MarketerUser MarketerUser { get; set; }
		public ICollection<CheckPaymentRequestAttempPictures> CheckPaymentRequestAttempPictures { get; set; }
		public int CheckPaymentConditaionId { get; set; }
		public CheckPaymentConditaion CheckPaymentConditaion { get; set; }
		[DisplayName("واریزپیش پرداخت")]
		public InitializePricePaymentConditaion InitializePricePaymentConditaionType { get; set; }
	}
	public enum CheckPaymentRequestAttempStatus
	{
		[Display(Name = "تایید نشده")]
		Rejected,
		[Display(Name = "در انتظار تایید")]
		Waiting,
		[Display(Name = "تایید شده")]
		Confirm,
	}
public enum InitializePricePaymentConditaion
{
	[Display(Name = "پرداخت شده")]
	Payed,
	[Display(Name = "در انتظار تایید")]
	notPayed,
}
}
