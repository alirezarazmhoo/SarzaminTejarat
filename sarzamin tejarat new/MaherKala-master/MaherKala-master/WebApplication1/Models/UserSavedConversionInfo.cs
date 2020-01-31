using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
	public class UserSavedConversionInfo
	{
		public int Id { get; set; }
		public int? UserId { get; set; }
		public string TokenMessage { get; set; }
	}
}