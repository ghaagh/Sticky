using RestSharp;
using Sticky.Infrastructure.Message;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sticky.Application.ResponseUpdater.Services;

internal class DruidQueryService
{
    private readonly string _druidClient;
    private const int TIMEOUT = 4000;

    public DruidQueryService(string druidClient)
    {
        _druidClient = druidClient;
    }

    public async Task<IRestResponse<IEnumerable<MessageModel>>> GetCategoryInformationAsync(IEnumerable<string> categoryNames, IEnumerable<long> userIdBatch)
    {
        var restClient = new RestClient(_druidClient)
        {
            Timeout = TIMEOUT
        };

        var request = CreateRestRequest(categoryNames, userIdBatch);

        return await restClient.ExecuteAsync<IEnumerable<MessageModel>>(request);
    }

    private DruidQueryService() { }

    private RestRequest CreateRestRequest(IEnumerable<string> categoryNames, IEnumerable<long> userIdBatchList)
    {

        var druidRestRequest = new RestRequest("druid/v2/sql", RestSharp.Method.POST);

        druidRestRequest.AddJsonBody(new
        {
            query = CreateDruidRequestBody(categoryNames, userIdBatchList)
        });
        return druidRestRequest;
    }

    private static string CreateDruidRequestBody(IEnumerable<string> categories, IEnumerable<long> userBatchList)
    {
        return $"select Distinct(CategoryName),UserId from ProductActions WHERE UserId in " +
                              $"({string.Join(',', userBatchList.Select(c => "'" + c + "'"))}) " +
                              $"AND CategoryName In({string.Join(',', categories)}) GROUP BY CategoryName,UserId limit 200";


    }
}

