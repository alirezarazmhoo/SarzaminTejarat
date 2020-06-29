using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
	public class CreditPayConditations
	{
		public int Id { get; set; }
		[DisplayName("توضیحات")]
		public string Description { get; set; }
		[DisplayName("مبلغ اعتبار")]
		[Required(ErrorMessage = "مبلغ اعتبار را وارد کنید")]

		public long CheckPrice { get; set; }
		[DisplayName("درصدکارمزد")]
		[Required(ErrorMessage = "درصد کارمزد را وارد کنید")]
		public int Interestrate { get; set; }
		[DisplayName("مبلغ اولیه پرداخت")]
		[Required(ErrorMessage = "مبلغ اولیه پرداخت را وارد کنید")]


		public long InitialPayment { get; set; }
		[DisplayName("دسته بندی")]

		public CreditPayType conditaionType { get; set; }
	}
	public enum CreditPayType
	{
		[Display(Name = "برنزی")]
		Bronze,
		[Display(Name = "نقره ای")]
		Silver,
		[Display(Name = "طلایی")]

		Gold
	}

}