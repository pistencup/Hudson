# Hudson
Hudson��[pistencup](https://github.com/pistencup/introduction)���߰���dotNET����, ����pistencup���߰���ο���[pistencup](https://github.com/pistencup/introduction).

# ��������֧��
�������Ѱ����������֧��, ��ο�: [���ṩ��֧��](https://github.com/pistencup/introduction#%E5%B7%B2%E6%8F%90%E4%BE%9B%E7%9A%84%E6%94%AF%E6%8C%81)

# ���ʹ��
1. Ŀǰ��Ҫ���б���Hudson.Core�����뵽��Ŀ������.

2. �ڰ�������Api�������Ŀ��, ��Ҫ��nuget�����б�����Ӷ�[Refit](https://github.com/reactiveui/refit)��Ŀ������.

3. Hudson����[Steeltoe.Discovery](https://github.com/SteeltoeOSS/Discovery)���ṩ��eurekaע�����ʹ�eureka��ȡ�����б�Ĺ���.
������Ҫ��appsetting.json�ļ������ض���:
```
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

4. Hudson����[Refit](https://github.com/reactiveui/refit)���ṩԶ�̵��õĶ�������.
�ڶ���Զ��Api����Ŀ��, ��Ҫ��Ӷ�[Refit](https://github.com/reactiveui/refit)������, ������ӿ�:
```
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
5. `WorkWithEurekaAttribute`�Ƕ�`ServiceClientAttribute`�ļ���չ, Ĭ���ṩ��`workWithEureka`����Ϊ`true`.
��`ServiceClientAttribute`��`WorkWithEurekaAttribute`��ǵĽӿ�, ����Ӧ������ʱ�Զ���IOCע���Ӧ��`HttpClient`.
����Ŀ�п���ֱ��ͨ��`ServiceProvider`���ʵ��������Զ�̵���:
```
    public class HomeController : Controller
    {
        private readonly CloudContext cloudContext;
        private readonly ISample sampleClient;
        private readonly ISampleApi sampleApiClient;
        public HomeController(ISample sampleClient, ISampleApi sampleApiClient, CloudContext cloudContext)
        {
            this.cloudContext = cloudContext;
            this.sampleClient = sampleClient;
            this.sampleApiClient = sampleApiClient;
        }

        public async Task<IActionResult> Index()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

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

6. ����CloudContext����: