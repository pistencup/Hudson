using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hudson.Core.RequestRecorder
{
	public interface IRequestRecorder
	{
		void OnRequest(RecorderContext recorderContext);

		void OnResponse(RecorderContext recorderContext);

		Exception OnException(RecorderContext recorderContext, Exception exception);
	}
}
