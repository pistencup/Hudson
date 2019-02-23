using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Hudson.Core.RequestRecorder
{
	public class DefaultRequestRecorder : IRequestRecorder
	{
		public Exception OnException(RecorderContext recorderContext, Exception exception)
		{
			return exception;
		}

		public void OnRequest(RecorderContext recorderContext)
		{
			
		}

		public void OnResponse(RecorderContext recorderContext)
		{
		}
	}
}
