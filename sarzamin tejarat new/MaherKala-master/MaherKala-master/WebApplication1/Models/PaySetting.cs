using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
	public class PaySetting
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public long Value { get; set; }
		public PaySettingType Type { get; set; }
	}
	public enum PaySettingType
	{
		MinimumForPayInPerson,
		MaximumForPayInPerson,
		MinimumForCheckPay,
		MinimumForCreditPay
	}
}