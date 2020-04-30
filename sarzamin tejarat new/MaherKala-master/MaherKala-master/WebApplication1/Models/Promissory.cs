using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
	public class Promissory
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "قیمت خالی است")]
		[DisplayName("قیمت")]
		public long Price { get; set; }
		[Required(ErrorMessage = "تاریخ خالی است")]
		[DisplayName("تاریخ")]
		public DateTime Date { get; set; }
		[Required(ErrorMessage = "نام بستانکار خالی است")]
		[DisplayName("بستانکار")]
		public string Creditor { get; set; }
		[Required(ErrorMessage = "نام متعهد خالی است")]
		[DisplayName("متعهد")]
		public int MarketerUserId { get; set; }
		[DisplayName("آدرس متعهد")]
		public string CommittedAddress { get; set; }
		[DisplayName("محل پرداخت")]
		public string PlaceOfPayment { get; set; }
		public MarketerUser MarketerUser { get; set; }

	}
}