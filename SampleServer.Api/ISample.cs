using Refit;
using System;
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
