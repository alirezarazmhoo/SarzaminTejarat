using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
	public class CreditPaymentRequestAttemp
	{
		public int Id { get; set; }
		[DisplayName("وضعیت")]
		public CreditPaymentRequestAttempStatus CreditPaymentRequestAttempStatus { get; set; }
		[DisplayName("تاریخ ثبت")]
		public DateTime CreatedDate { get; set; }
		[DisplayName("نظر ادمین")]
		public string AdminComment { get; set; }
		public int MarketerUserId { get; set; }
		public MarketerUser MarketerUser { get; set; }
		public ICollection<CreditPaymentRequestAttempPictures> creditPaymentRequestAttempPictures { get; set; }
		public int CreditPayConditationsId { get; set; }
		public CreditPayConditations CreditPayConditations { get; set; }
		[DisplayName("واریزپیش پرداخت")]
		public InitializePricePaymentConditaion InitializePricePaymentConditaionType { get; set; }

	}
	public enum CreditPaymentRequestAttempStatus
	{
		[Display(Name = "تایید نشده")]
		Rejected,
		[Display(Name = "در انتظار تایید")]
		Waiting,
		[Display(Name = "تایید شده")]
		Confirm,
	}
}