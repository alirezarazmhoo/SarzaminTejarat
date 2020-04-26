using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
	public class Check
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "مشتری را مشخص کنید")]
		public int MarketerUserId { get; set; }
		[DisplayName("مشتری")]
		public MarketerUser MarketerUser { get; set; }
		[DisplayName("وضعیت چک")]
		public CheckStatus CheckStatus { get; set; }
		[Required(ErrorMessage ="بانک را مشخص کنید")]
		public int BankId { get; set; }
		[DisplayName("بانک")]
		public bank Bank { get; set; }
		[DisplayName("تاریخ")]

		public DateTime Date { get; set; }
		[Required(ErrorMessage ="مبلغ را وارد کنید")]
		[DisplayName("قیمت")]

		public int Price { get; set; }

		[Required(ErrorMessage = "شماره چک را وارد کنید")]
		[DisplayName("شماره چک")]

		public string CheckCode { get; set; }

	}


	public enum CheckStatus
	{
		[Display(Name = "پاس شده")]
		Passed,
		[Display(Name = "پاس نشده")]
		NotPassed,
		[Display(Name = "برگشت خورده")]
		Returned
		
	}
}