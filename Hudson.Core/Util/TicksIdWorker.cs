using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hudson.Core.Util
{
	public class TicksIdWorker : IIdWorker
	{
		public string NextRequestId()
		{
			return GetId();
		}

		public string NextSpanId()
		{
			return GetId();
		}

		/// <summary>
		/// ticks generator, use Microsoft.AspNetCore.Http.Features.HttpRequestIdentifierFeature
		/// 
		/// map the DateTime.UtcNow.Ticks to "0123456789ABCDEFGHIJKLMNOPQRSTUV"
		/// much faster than long.ToString()
		/// </summary>
		/// <returns></returns>
		private string GetId()
		{
			return new HttpRequestIdentifierFeature().TraceIdentifier;
		}
	}
}
