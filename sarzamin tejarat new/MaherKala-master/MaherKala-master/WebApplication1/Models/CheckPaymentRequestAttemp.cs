using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
	public class CheckPaymentRequestAttemp
	{
		public int Id { get; set; }
		public CheckPaymentRequestAttempStatus CheckPaymentRequestAttempStatus { get; set; }
		public DateTime CreatedDate { get; set; }
		public string AdminComment { get; set; }
		public int MarketerUserId { get; set; }
		public MarketerUser MarketerUser { get; set; }
		public ICollection<CheckPaymentRequestAttempPictures> CheckPaymentRequestAttempPictures { get; set; }
		public int CheckPaymentConditaionId { get; set; }
		public CheckPaymentConditaion CheckPaymentConditaion { get; set; }

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
}
