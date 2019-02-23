using Hudson.Core.Util;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hudson.Core
{
	/// <summary>
	/// 为调用链定制的请求上下文信息
	/// </summary>
	public class CloudContext
	{
		private const string HEADER_NAME_REQUESTID = "RequestID";
		private const string HEADER_NAME_PREVIOUS_SPANID = "SourceSpanID";
		private const string HEADER_NAME_CALL_INDEX = "CallIndex";

		private int nextCallIndex = 0;

		public string RequestID { get; private set; }
		/// <summary>
		/// 上一个处理块的标识
		/// </summary>
		public string PreviousSpanID { get; private set; }
		/// <summary>
		/// 当前处理块的标识
		/// </summary>
		public string CurrentSpanID { get; private set; }
		/// <summary>
		/// 当前处理块的调用序号
		/// </summary>
		public string CallIndex { get; private set; }

		public CloudContext(IHttpContextAccessor httpContextAccessor, IIdWorker idWorker)
		{
			var httpContext = httpContextAccessor.HttpContext;

			RequestID = GetRequestHeader(httpContext, HEADER_NAME_REQUESTID, () => { return idWorker.NextRequestId(); });

			PreviousSpanID = GetRequestHeader(httpContext, HEADER_NAME_PREVIOUS_SPANID, "");
			CallIndex = GetRequestHeader(httpContext, HEADER_NAME_CALL_INDEX, "1");

			CurrentSpanID = idWorker.NextSpanId();
		}
		/// <summary>
		/// 为即将发起的请求生成context信息, 并附着到请求头上
		/// </summary>
		/// <returns></returns>
		public void MakeNextChainRequest(HttpRequestMessage requestMessage)
		{
			SetRequestHeader(requestMessage, HEADER_NAME_REQUESTID, RequestID);
			SetRequestHeader(requestMessage, HEADER_NAME_PREVIOUS_SPANID, CurrentSpanID);
			SetRequestHeader(requestMessage, HEADER_NAME_CALL_INDEX, Interlocked.Increment(ref nextCallIndex).ToString());
		}

		#region 帮助方法
		/// <summary>
		/// 读取请求头信息或使用默认值
		/// </summary>
		/// <param name="httpContext"></param>
		/// <param name="headerName"></param>
		/// <param name="getDefaultValue"></param>
		/// <returns></returns>
		private string GetRequestHeader(HttpContext httpContext, string headerName, Func<string> getDefaultValue)
		{
			var val = httpContext.Request.Headers[headerName];
			if (string.IsNullOrEmpty(val))
			{
				return getDefaultValue();
			}
			return val;
		}
		/// <summary>
		/// 读取请求头信息或使用默认值
		/// </summary>
		/// <param name="httpContext"></param>
		/// <param name="headerName"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		private string GetRequestHeader(HttpContext httpContext, string headerName, string defaultValue)
		{
			var val = httpContext.Request.Headers[headerName];
			if (string.IsNullOrEmpty(val))
			{
				return defaultValue;
			}
			return val;
		}
		/// <summary>
		/// 设置请求头, 如果该Header已存在, 则删除原Header
		/// </summary>
		/// <param name="requestMessage"></param>
		/// <param name="headerName"></param>
		/// <param name="headerValue"></param>
		private void SetRequestHeader(HttpRequestMessage requestMessage, string headerName, string headerValue)
		{
			if (requestMessage.Headers.Contains(headerName))
			{
				requestMessage.Headers.Remove(headerName);
			}
			requestMessage.Headers.Add(headerName, headerValue);
		}
		#endregion
	}
}
