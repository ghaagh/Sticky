using Microsoft.EntityFrameworkCore;
using Sticky.Models.Context;
using Sticky.Models.Etc;
using Sticky.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sticky.Repositories.Advertisement.Implementions
{
    public class CategoryLogger : ICategoryLogger
    {
        private readonly IRedisCache _redisCache;
        public CategoryLogger(IRedisCache redisCache)
        {
            _redisCache = redisCache;
        }
        public async Task LogCategory(int hostId, string category, double counter)
        {
            var categoryDatabase = _redisCache.GetDatabase(RedisDatabases.Categories);
            await categoryDatabase.HashIncrementAsync(hostId.ToString(), category, counter);

        }
        public async Task FlushToSql(string connectionString)
        {
            DbContextOptionsBuilder<StickyDbContext> optionsBuilder = new DbContextOptionsBuilder<StickyDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            var hosts = new List<int>();
            using (var _db = new StickyDbContext(optionsBuilder.Options))
            {
                hosts = await _db.Hosts.Where(x => x.HostValidated).Select(c=>c.Id).ToListAsync();
            }
            var database = _redisCache.GetDatabase(RedisDatabases.Categories);
            foreach(var hostId in hosts)
            {
                var allkeys = await database.HashGetAllAsync(hostId.ToString());
                if (allkeys.Count() != 0)
                {
                    DataTable table = new DataTable();
                    table.Columns.Add(new DataColumn("HostId", typeof(int)));
                    table.Columns.Add(new DataColumn("Category", typeof(string)));
                    table.Columns.Add(new DataColumn("Counter", typeof(long)));
                    foreach (var price in allkeys)
                    {
                        DataRow row = table.NewRow();
                        row["HostId"] = hostId;
                        row["Category"] = price.Name;
                        row["Counter"] = long.Parse(price.Value);
                        table.Rows.Add(row);
                    }
                    try
                    {
                        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                        {
                            using (SqlCommand sqlCommand = new SqlCommand("UpdateCategories", sqlConnection))
                            {
                                sqlCommand.CommandType = CommandType.StoredProcedure;
                                sqlCommand.Parameters.AddWithValue("@Categories", table);
                                await sqlConnection.OpenAsync();
                                sqlCommand.CommandTimeout = 60000;
                                await sqlCommand.ExecuteNonQueryAsync();
                                sqlConnection.Close();
                            }

                        }
                        await database.KeyDeleteAsync(hostId.ToString());
                    }
                    catch
                    {
                    }
                }
            }

        }
    }
}
