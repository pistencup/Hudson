# Hudson
Hudson��[pistencup](https://github.com/pistencup/introduction)���߰���dotNET����, ����pistencup���߰���ο���[pistencup](https://github.com/pistencup/introduction).

# ��������֧��
�������Ѱ����������֧��, ��ο�: [���ṩ��֧��](https://github.com/pistencup/introduction#%E5%B7%B2%E6%8F%90%E4%BE%9B%E7%9A%84%E6%94%AF%E6%8C%81)

# ���ʹ��
1. Ŀǰ��Ҫ���б���Hudson.Core�����뵽��Ŀ������.

2. �ڰ�������Api�������Ŀ��, ��Ҫ��nuget�����б�����Ӷ�[Refit](https://github.com/reactiveui/refit)��Ŀ������.

3. Hudson����[Steeltoe.Discovery](https://github.com/SteeltoeOSS/Discovery)���ṩ��eurekaע�����ʹ�eureka��ȡ�����б������.
������Ҫ��appsetting.json�ļ�����������:
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

4. Hudson����[Refit](https://github.com/reactiveui/refit)���ṩԶ�̵�������.
�ڶ���Զ��Api����Ŀ��, ��Ҫ��Ӷ�[Refit](https://github.com/reactiveui/refit)������, ������ӿ�:
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
5. `WorkWithEurekaAttribute`�Ƕ�`ServiceClientAttribute`�ļ���չ, Ĭ��ָ����`workWithEureka`����Ϊ`true`.
��`ServiceClientAttribute`��`WorkWithEurekaAttribute`��ǵĽӿ�, ����Ӧ������ʱ�Զ���IOCע���Ӧ��`HttpClient`.
����Ŀ�п���ֱ��ͨ��`ServiceProvider`���ʵ��������Զ�̵���:
```C#
    public class HomeController : Controller
    {
        private readonly CloudContext cloudContext;
        private readonly ISample sampleClient;
        private readonly ISampleApi sampleApiClient;
        //�ӹ��캯�����Api����
        public HomeController(ISample sampleClient, ISampleApi sampleApiClient, CloudContext cloudContext)
        {
            this.cloudContext = cloudContext;
            this.sampleClient = sampleClient;
            this.sampleApiClient = sampleApiClient;
        }

        public async Task<IActionResult> Index()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            //����Զ�̵���
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

6. ����`CloudContext`����: `CloudContext`�ǵ��������ĵĳ�����, ��Ҫ�ṩ���³�Ա:

| Field | ���� | ���� |
| :--- | :--- | :--- |
| RequestID | �����ʶ | ��ǰ�����һ�ε�������ʱ���ɵ�Ψһ��ʶ, �����ʶ�������к��������д��� |
| PreviousSpanID | ��һ������ڵ��ʶ | ����ͷ�л�õ���һ�������ı�ʶ, ���ڹ���������, �������ͷ���������Ϊ��, ��˵�����ǵ�һ������ڵ� |
| CurrentSpanID | ��ǰ�����ı�ʶ | Ϊ��ǰ����ڵ����ɵı�ʶ, �����������еĵ�һ������,���ֵ��RequestIDһ�� |
| CallIndex | ������� | ������ͷ�л�õĵ�ǰ�������ͬ����������еĵ������ |
| GroupName | ������� | δ��ɹ���, ���ڱ�ǵ�ǰ�������ĸ�����ķ���ڵ㴦�� |

Ӧ������ ***��Ҫ*** �ӹ��캯������`CloudContext`����, ʹ��`ServiceProvider`����ȡ��ÿ��������Ķ���,����Ե��ù��̷����������.

7. ���๦��,��clone���ο�ʾ����Ŀ.