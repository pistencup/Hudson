using System;
using System.Collections.Generic;
using System.Text;

namespace Hudson.Core.Util
{
	public interface IIdWorker
	{
		string NextRequestId();

		string NextSpanId();
	}
}
