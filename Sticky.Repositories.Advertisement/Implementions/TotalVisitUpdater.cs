using MongoDB.Driver;
using Sticky.Models.Etc;
using Sticky.Repositories.Common;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Sticky.Repositories.Advertisement.Implementions
{
    public class TotalVisitUpdater : ITotalVisitUpdater
    {
        private readonly IRedisCache _redisCache;
        private readonly IHostCache _hostCache;
        public TotalVisitUpdater(IRedisCache redisCache,IHostCache hostCache)
        {
            _redisCache = redisCache;
            _hostCache = hostCache;
        }

        public async Task UpdateTotalVisit(int hostId)
        {
            var database = _redisCache.GetDatabase(RedisDatabases.UserTotalVisits);
            var hashkey = DateTime.Now.ToString("yyyy_MM_dd");
            await database.HashIncrementAsync(hostId.ToString(), hashkey);
            //using (SqlConnection sqlConnection = new SqlConnection(_configurations.ConnectionString))
            //{
            //    SqlCommand sqlCommand = new SqlCommand("UpdateTotalVisit", sqlConnection);
            //    sqlCommand.CommandType = CommandType.StoredProcedure;
            //    sqlCommand.Parameters.AddWithValue("@HostId", hostId);
            //    await sqlConnection.OpenAsync();
            //    await sqlCommand.ExecuteNonQueryAsync();
            //    sqlConnection.Close();
            //}
        }
        public async Task FlushToSql(string connectionString)
        {
            var database = _redisCache.GetDatabase(RedisDatabases.UserTotalVisits);
            var hosts = await _hostCache.GetListOfHostAsync();
            foreach(var item in hosts)
            {
                var allkeys =await  database.HashGetAllAsync(item.Id.ToString());
                if (allkeys.Count() != 0)
                {
                    foreach(var keyItem in allkeys)
                    {
                        if (keyItem.Value != 0)
                        {
                           var date = keyItem.Name.ToString().Split("_").Select(c=>int.Parse(c)).ToArray();
                            var datetime = new DateTime(date[0], date[1], date[2]);
                            var counter = keyItem.Value;
                            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                            {
                                using (SqlCommand sqlCommand = new SqlCommand("UpdateTotalVisit", sqlConnection))
                                {
                                sqlCommand.CommandType = CommandType.StoredProcedure;
                                sqlCommand.Parameters.AddWithValue("@HostId", item.Id);
                                sqlCommand.Parameters.AddWithValue("@Date", datetime);
                                sqlCommand.Parameters.AddWithValue("@Count", (int)counter);
                                await sqlConnection.OpenAsync();
                                await sqlCommand.ExecuteNonQueryAsync();
                                sqlConnection.Close();
                                await  database.HashDecrementAsync(item.Id.ToString(), keyItem.Name, (double)counter);
                                }
 

                            }
                        }
 
                    }
                }


            }

        }

    }
}
