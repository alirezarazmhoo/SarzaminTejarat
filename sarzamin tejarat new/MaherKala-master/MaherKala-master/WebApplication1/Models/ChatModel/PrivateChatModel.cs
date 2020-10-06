using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Models.ChatModel
{
	public class PrivateChatModel
	{
		public int TotalPage { get; set; }
		public int PageNumber { get; set; }
		public int TotalItemCount { get; set; }
		public List<Converstions> PrivateMessage { get; set; }
	}
}