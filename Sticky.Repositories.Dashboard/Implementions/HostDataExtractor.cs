using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;
using Sticky.Models.Context;
using Sticky.Models.Etc;
using Sticky.Dto.Dashboard.Response;

namespace Sticky.Repositories.Dashboard.Implementions
{
    public class HostDataExtractor : IHostDataExtractor
    {
        private readonly StickyDbContext _db;
        private readonly DashboardAPISetting _setting;
        private readonly ICategoryFinder _categoryFinder;

        public HostDataExtractor(StickyDbContext db,IOptions<DashboardAPISetting> options,ICategoryFinder categoryFinder)
        {
            _categoryFinder = categoryFinder;
            _setting = options.Value;
            _db = db;
        }
        public async Task<HostDataResult> ExtractHostInfoAsync(int hostId)
        {
            var host = await _db.Hosts.FirstOrDefaultAsync(c => c.Id == hostId);
            List<KeyValuePair<string, bool>> completion = new List<KeyValuePair<string, bool>>
            {
                new KeyValuePair<string, bool>("ApprovedHost", host.HostValidated),
                new KeyValuePair<string, bool>("ScriptInserted", host.PageValidated),
                new KeyValuePair<string, bool>("ProductFeatures", host.ProductValidated),
                new KeyValuePair<string, bool>("ProductCategory", host.CategoryValidated),
                new KeyValuePair<string, bool>("UserCartFeatures", host.AddToCardValidated)
            };
            HostDataResult model = new HostDataResult
            {
                Features = completion,
                OveralComplition = !host.HostValidated ? 10 : host.HostValidated & !host.PageValidated ? 20 : host.HostValidated & host.PageValidated & !host.ProductValidated ? 50 : host.HostValidated & host.PageValidated & host.ProductValidated & !host.CategoryValidated ? 60 : host.HostValidated & host.PageValidated & host.ProductValidated & host.CategoryValidated & !host.AddToCardValidated ? 80 : 100
            };
            List<UserCountPerDayAggrigated> sqlList = new List<UserCountPerDayAggrigated>();
            using (SqlConnection sqlConnection = new SqlConnection(_setting.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("GetUserCountPerDay", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    sqlCommand.Parameters.AddWithValue("@StartDate", DateTime.Now.AddDays(-30));
                    sqlCommand.Parameters.AddWithValue("@EndDate", DateTime.Now);
                    sqlCommand.Parameters.AddWithValue("@HostId", hostId);
                    await sqlConnection.OpenAsync();
                    SqlDataReader dr = sqlCommand.ExecuteReader();
                    while (dr.Read())
                    {
                        UserCountPerDayAggrigated newrow = new UserCountPerDayAggrigated()
                        {
                            Date = dr.GetDateTime(dr.GetOrdinal("LogDate")),
                            UserCount = dr.GetInt32(dr.GetOrdinal("UserCount"))
                        };
                        sqlList.Add(newrow);
                    }
                }
                sqlConnection.Close();
            }

            PerDayRecord chartRecord = new PerDayRecord();
            for (int i = 30; i >= 0; i--)
            {
                DateTime now = DateTime.Now.AddDays(-i);
                var currentRowFromSql = sqlList.Where(c => c.Date.Date == now.Date);
                if (currentRowFromSql.Count() == 0)
                {
                    var currentDay = 0;
                    chartRecord.Labels.Add(now.Date.ToString("MM/dd/yyyy"));
                    chartRecord.Values.Add(currentDay);
                }
                else
                {
                    var currentDay = currentRowFromSql.FirstOrDefault().UserCount;
                    chartRecord.Labels.Add(now.Date.ToString("MM/dd/yyyy"));
                    chartRecord.Values.Add(currentDay);
                }


            }
            var topc = await _categoryFinder.FindMatchedCategoriesAsync(hostId, null);
            model.Top20Categories = new PerDayRecord()
            {
                Labels = topc.Select(c => c.CategoryName).ToList(),
                Values = topc.Select(c => c.Counter).ToList()
            };
            model.VisitedPagesPerDay = chartRecord;
            return model;
        }
    }
}
