# Hudson
Hudson是[pistencup](https://github.com/pistencup/introduction)工具包的dotNET部分, 关于pistencup工具包请参考：[pistencup](https://github.com/pistencup/introduction).

# 其他语言支持
如果你在寻找其他语言支持, 请参考: [已提供的支持](https://github.com/pistencup/introduction#%E5%B7%B2%E6%8F%90%E4%BE%9B%E7%9A%84%E6%94%AF%E6%8C%81)

# 如何使用
1. 目前需要自行编译Hudson.Core并加入到项目依赖中.

2. 在包含服务Api定义的项目中, 需要从nuget或自行编译添加对[Refit](https://github.com/reactiveui/refit)项目的依赖.

3. Hudson依赖[Steeltoe.Discovery](https://github.com/SteeltoeOSS/Discovery)来提供向eureka注册服务和从eureka获取服务列表的能力.
所以需要向appsetting.json文件添加相关配置:
```json
  "spring": {
    "application": {
      "name": "sample-server"
    }
  },
  "eureka": {
    "client": {
      "serviceUrl": "http://eureka-server:10001/eureka/",
      "shouldRegisterWithEureka": true
    },
    "instance": {
      "port": 5000
    }
  }
```

4. Hudson依赖[Refit](https://github.com/reactiveui/refit)来提供远程调用能力.
在定义远端Api的项目中, 需要添加对[Refit](https://github.com/reactiveui/refit)的依赖, 并定义接口:
```C#
using Refit;
using System.Threading.Tasks;

namespace SampleServer.Api
{
    [WorkWithEureka("sample-server")]
    //equals to [ServiceClient("http://cheok.com/", true)]
    public interface ISample
    {
        [Get("/home/get123")]
        Task<string> Get123();

        [Get("/get321")]
        Task<string> Get321();
    }
}
```
5. `WorkWithEurekaAttribute`是对`ServiceClientAttribute`的简单扩展, 默认指定了`workWithEureka`参数为`true`.
被`ServiceClientAttribute`或`WorkWithEurekaAttribute`标记的接口, 会在应用启动时自动向IOC注册对应的`HttpClient`.
在项目中可以直接通过`ServiceProvider`获得实例并进行远程调用:
```C#
    public class HomeController : Controller
    {
        private readonly CloudContext cloudContext;
        private readonly ISample sampleClient;
        private readonly ISampleApi sampleApiClient;
        //从构造函数获得Api对象
        public HomeController(ISample sampleClient, ISampleApi sampleApiClient, CloudContext cloudContext)
        {
            this.cloudContext = cloudContext;
            this.sampleClient = sampleClient;
            this.sampleApiClient = sampleApiClient;
        }

        public async Task<IActionResult> Index()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            //发起远程调用
            string rst = await sampleClient.Get123();
            dic.Add("sampleClient.Get123()", rst);

            dic.Add("sampleClient.Get321()", await sampleClient.Get321());

            dic.Add("sampleApiClient.Get(\"whosyourdaddy\")", Newtonsoft.Json.JsonConvert.SerializeObject(await sampleApiClient.Get("whosyourdaddy")));

            dic.Add("cloudcontext", Newtonsoft.Json.JsonConvert.SerializeObject(cloudContext));
            

            ViewData["dic"] = dic;
            return View();
        }
    }
```

6. 关于`CloudContext`对象: `CloudContext`是调用上下文的持有者, 主要提供以下成员:

| Field | 功能 | 描述 |
| :--- | :--- | :--- |
| RequestID | 请求标识 | 当前请求第一次到达服务端时生成的唯一标识, 这个标识会在所有后续调用中传递 |
| PreviousSpanID | 上一个处理节点标识 | 请求头中获得的上一个处理块的标识, 用于构造请求链, 如果请求头中这个数据为空, 则说明这是第一个处理节点 |
| CurrentSpanID | 当前处理块的标识 | 为当前处理节点生成的标识, 对于请求链中的第一个请求,这个值与RequestID一致 |
| CallIndex | 调用序号 | 从请求头中获得的当前处理块在同级处理过程中的调用序号 |
| GroupName | 处理分组 | 未完成功能, 用于标记当前请求由哪个分组的服务节点处理 |

应用中请 ***不要*** 从构造函数构造`CloudContext`对象, 使用`ServiceProvider`来获取对每个请求共享的对象,避免对调用过程分析造成困扰.

7. 更多功能,请clone并参考示例项目.