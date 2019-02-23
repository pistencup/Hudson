using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hudson.Core.RequestRecorder
{
	/// <summary>
	/// 记录器上下文
	/// </summary>
	public class RecorderContext
	{
		public CloudContext CloudContext { get; set; }

		public HttpContext HttpContext { get; set; }
		
		public ActionExecutingContext ActionExecutingContext { get; set; }

		public ActionExecutedContext ActionExecutedContext { get; set; }

		public long ActionDuration { get; set; }
	}
}
