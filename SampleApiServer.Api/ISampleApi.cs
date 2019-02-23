using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleApiServer.Api
{
    [WorkWithEureka("sample-api-server")]
    public interface ISampleApi
    {
        [Get("/api/values")]
        Task<IEnumerable<string>> Get(string arg);

        // GET api/values/5
        [Get("/api/values/{id}")]
        Task<string> Get(int id);

        // POST api/values
        [Post("/api/values")]
        Task<string> Post([Body(BodySerializationMethod.UrlEncoded)] string value);

        // PUT api/values/5
        [Put("/api/values/{id}")]
        Task<string> Put([AliasAs("id")]int aid, [Body(BodySerializationMethod.UrlEncoded)] string value);

        // DELETE api/values/5
        [Delete("/api/values/{id}")]
        Task<string> Delete(int id);
    }
}
