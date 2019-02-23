using System;
using System.Collections.Generic;
using System.Text;

namespace Hudson.Core.Util
{
	public class GUIDWorker : IIdWorker
	{
		public string NextRequestId()
		{
			return Guid.NewGuid().ToString("N");
		}

		public string NextSpanId()
		{
			return Guid.NewGuid().ToString("N");
		}
	}
}
