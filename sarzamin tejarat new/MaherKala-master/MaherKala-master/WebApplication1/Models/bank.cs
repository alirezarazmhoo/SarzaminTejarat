using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
	public class bank
	{
		public int Id { get; set; }

		[Required(ErrorMessage ="نام بانک را وارد کنید")]
		[DisplayName("عنوان بانک")]
	    public string Name { get; set; }
	}
}