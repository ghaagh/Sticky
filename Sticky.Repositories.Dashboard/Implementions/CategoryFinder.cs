using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Sticky.Dto.Dashboard.Response;
using Sticky.Models.Etc;
namespace Sticky.Repositories.Dashboard.Implementions
{
    public class CategoryFinder : ICategoryFinder
    {
        private readonly DashboardAPISetting _setting;
        public CategoryFinder(IOptions<DashboardAPISetting> setting)
        {
            _setting = setting.Value;
        }
        public async  Task<List<CategorytResult>> FindMatchedCategoriesAsync(int hostId, string keyword)
        {
            List<CategorytResult> list = new List<CategorytResult>();
            using (SqlConnection sqlConnection = new SqlConnection(_setting.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("SearchInHostCategories", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    if (!string.IsNullOrEmpty(keyword))
                        sqlCommand.Parameters.AddWithValue("@Keyword", keyword);
                    else
                        sqlCommand.Parameters.AddWithValue("@Keyword", DBNull.Value);
                    sqlCommand.Parameters.AddWithValue("@HostId", hostId);
                    await sqlConnection.OpenAsync();
                    SqlDataReader dr = sqlCommand.ExecuteReader();
                    while (dr.Read())
                    {
                        CategorytResult newrow = new CategorytResult();
                        if (!dr.IsDBNull(dr.GetOrdinal(nameof(newrow.CategoryName))))
                            newrow.CategoryName = dr.GetString(dr.GetOrdinal(nameof(newrow.CategoryName)));
                        if (!dr.IsDBNull(dr.GetOrdinal(nameof(newrow.Id))))
                            newrow.Id = dr.GetInt32(dr.GetOrdinal(nameof(newrow.Id)));
                        if (!dr.IsDBNull(dr.GetOrdinal(nameof(newrow.Counter))))
                            newrow.Counter = dr.GetInt32(dr.GetOrdinal(nameof(newrow.Counter)));
                        if (!dr.IsDBNull(dr.GetOrdinal(nameof(newrow.HostId))))
                            newrow.HostId = dr.GetInt32(dr.GetOrdinal(nameof(newrow.HostId)));
                        list.Add(newrow);
                    }
                    sqlConnection.Close();
                }



            }
            return list;
        }
    }
}
