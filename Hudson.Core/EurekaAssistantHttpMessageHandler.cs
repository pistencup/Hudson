using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Steeltoe.Common.Discovery;
using Steeltoe.Common.Http.Discovery;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hudson.Core
{
	/// <summary>
	/// HttpMessageHandler, 用于Discovery和请求头修改.
	/// </summary>
	public class EurekaAssistantHttpMessageHandler : DiscoveryHttpMessageHandler
	{
		private readonly IHttpContextAccessor httpContextAccessor = null;
		public EurekaAssistantHttpMessageHandler(IHttpContextAccessor httpContextAccessor, IDiscoveryClient discoveryClient, ILogger<DiscoveryHttpClientHandler> logger = null) : base(discoveryClient, logger)
		{
			this.httpContextAccessor = httpContextAccessor;
		}

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var cloudContext = (CloudContext)httpContextAccessor.HttpContext.RequestServices.GetService(typeof(CloudContext));

			cloudContext.MakeNextChainRequest(request);

			return base.SendAsync(request, cancellationToken);
		}
	}
}
