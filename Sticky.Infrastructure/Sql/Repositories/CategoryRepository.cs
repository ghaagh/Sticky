using Microsoft.Data.SqlClient;
using Sticky.Domain.CategoryAggrigate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Sticky.Infrastructure.Sql.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly string _connectionString;
        const int loadCounter = 100;
        private readonly string command = $"Select top({loadCounter}) * from {nameof(Context.Categories)} where {nameof(Category.HostId)}=@HostId and {nameof(Category.Name)} like '%'+@Keyword+'%'";
        public CategoryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<List<Category>> SearchAsync(long hostId, string keyword)
        {
            List<Category> list = new List<Category>();
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(command, sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.Text;
                    if (!string.IsNullOrEmpty(keyword))
                        sqlCommand.Parameters.AddWithValue("@Keyword", keyword);
                    else
                        sqlCommand.Parameters.AddWithValue("@Keyword", DBNull.Value);
                    sqlCommand.Parameters.AddWithValue("@HostId", hostId);
                    await sqlConnection.OpenAsync();
                    SqlDataReader dr = sqlCommand.ExecuteReader();

                    while (dr.Read())
                    {
                        var category = new Category(!dr.IsDBNull(dr.GetOrdinal(nameof(Category.Name))) ? dr.GetString(dr.GetOrdinal(nameof(Category.Name))) : string.Empty);
                        category.Id = !dr.IsDBNull(dr.GetOrdinal(nameof(Category.Id))) ? dr.GetInt64(dr.GetOrdinal(nameof(Category.Id))) : 0;
                        category.Counter = !dr.IsDBNull(dr.GetOrdinal(nameof(Category.Counter))) ? dr.GetInt64(dr.GetOrdinal(nameof(Category.Counter))) : 0;
                        list.Add(category);
                    }
                    sqlConnection.Close();
                }
            }
            return list;
        }
    }
}
