using System.Net.Http;
using Microsoft.AspNetCore.Http;

namespace Infrastructure
{
    public class GrillMenuRequestService : BaseApiService<GrillMenu>, IGrillMenuRequestService
    {
        protected override string RouteName => "GrillMenu";

        public GrillMenuRequestService(IHttpClientFactory clientFactory) : base(clientFactory)
        { }

    }
}
