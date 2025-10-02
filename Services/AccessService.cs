using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News2025.Services
{
    public class AccessService : IAccessService
    {
        private readonly string _connectionString;

        public AccessService(string databasePath)
        {
            try
            {
                if (string.IsNullOrEmpty(databasePath))
                    throw new FileNotFoundException("Đường dẫn file Access không được để trống!");

                if (!File.Exists(databasePath))
                    throw new FileNotFoundException($"File Access không tồn tại tại đường dẫn: {databasePath}");

                // Chuỗi kết nối đến file Access
                _connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={databasePath};Persist Security Info=False;";

                // Thực hiện kết nối kiểm tra để đảm bảo DB tồn tại và hoạt động
                using (var connection = new OleDbConnection(_connectionString))
                {
                    connection.Open();
                    Console.WriteLine("Database kết nối thành công trong constructor");
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
                throw new Exception("Không thể khởi tạo AccessService vì file Access không tồn tại.", ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
                throw new Exception("Không có quyền truy cập file Access. Vui lòng kiểm tra quyền truy cập.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi khởi tạo AccessService: {ex.Message}");
                throw new Exception("Lỗi không xác định khi khởi tạo AccessService.", ex);
            }
        }

        // Các phương thức hiện có giữ nguyên
        public int ExecuteQuery(string query)
        {
            try
            {
                using (var connection = new OleDbConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new OleDbCommand(query, connection))
                    {
                        return command.ExecuteNonQuery();
                    }
                }
            }
            catch (OleDbException ex)
            {
                throw new Exception($"Lỗi khi thực thi câu lệnh SQL: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi không xác định khi thực thi câu lệnh SQL: {ex.Message}", ex);
            }
        }

        public string QueryColumnValue(string tableName, string column1, string column2, string column1Value)
        {
            try
            {
                using (var connection = new OleDbConnection(_connectionString))
                {
                    connection.Open();

                    string query = $"SELECT [{column2}] FROM [{tableName}] WHERE [{column1}] = @value";

                    using (var command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@value", column1Value);

                        var result = command.ExecuteScalar();

                        return result?.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi truy vấn dữ liệu: {ex.Message}", ex);
            }
        }

        public int? QueryIntColumnValue(string tableName, string column1, string column2, string column1Value)
        {
            string result = QueryColumnValue(tableName, column1, column2, column1Value);
            return int.TryParse(result, out int value) ? value : (int?)null;
        }

        public List<(string Name, string Description)> GetNameAndDescriptionFromTable(string tableName)
        {
            var data = new List<(string Name, string Description)>();

            try
            {
                using (var connection = new OleDbConnection(_connectionString))
                {
                    connection.Open();

                    string query = $"SELECT [Name], [Description] FROM [{tableName}]";

                    using (var command = new OleDbCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader["Name"]?.ToString() ?? "";
                            string description = reader["Description"]?.ToString() ?? "";

                            data.Add((name, description));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi đọc bảng {tableName}: {ex.Message}", ex);
            }

            return data;
        }
        public List<(string Name1, string Description1, string Name2, string Description2)> GetPhongVanBangData()
        {
            var data = new List<(string Name1, string Description1, string Name2, string Description2)>();

            try
            {
                using (var connection = new OleDbConnection(_connectionString))
                {
                    connection.Open();

                    string query = "SELECT [Name1], [Description1], [Name2], [Description2] FROM [Phongvanbang]";

                    using (var command = new OleDbCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name1 = reader["Name1"]?.ToString() ?? "";
                            string description1 = reader["Description1"]?.ToString() ?? "";
                            string name2 = reader["Name2"]?.ToString() ?? "";
                            string description2 = reader["Description2"]?.ToString() ?? "";

                            data.Add((name1, description1, name2, description2));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi đọc dữ liệu từ bảng Phongvanbang: " + ex.Message, ex);
            }

            return data;
        }
        public List<(string Name1, string Description1, string Description2)> GetPhongVanLechData()
        {
            var data = new List<(string Name1, string Description1, string Description2)>();

            try
            {
                using (var connection = new OleDbConnection(_connectionString))
                {
                    connection.Open();

                    string query = "SELECT [Name1], [Description1], [Description2] FROM [Phongvanlech]";

                    using (var command = new OleDbCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name1 = reader["Name1"]?.ToString() ?? "";
                            string description1 = reader["Description1"]?.ToString() ?? "";
                            string description2 = reader["Description2"]?.ToString() ?? "";

                            data.Add((name1, description1, description2));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi đọc dữ liệu từ bảng Phongvanlech: " + ex.Message, ex);
            }

            return data;
        }
        public List<(string Name1, string Description1, string Name2, string Description2, string Name3, string Description3)> GetPhongVan3NguoiData()
        {
            var data = new List<(string Name1, string Description1, string Name2, string Description2, string Name3, string Description3)>();

            try
            {
                using (var connection = new OleDbConnection(_connectionString))
                {
                    connection.Open();

                    string query = "SELECT [Name1], [Description1], [Name2], [Description2], [Name3], [Description3] FROM [Phongvan3nguoi]";

                    using (var command = new OleDbCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name1 = reader["Name1"]?.ToString() ?? "";
                            string description1 = reader["Description1"]?.ToString() ?? "";
                            string name2 = reader["Name2"]?.ToString() ?? "";
                            string description2 = reader["Description2"]?.ToString() ?? "";
                            string name3 = reader["Name3"]?.ToString() ?? "";
                            string description3 = reader["Description3"]?.ToString() ?? "";

                            data.Add((name1, description1, name2, description2, name3, description3));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi đọc dữ liệu từ bảng Phongvan3nguoi: " + ex.Message, ex);
            }

            return data;
        }

    }
    public interface IAccessService
    {
        int ExecuteQuery(string query);
        string QueryColumnValue(string tableName, string column1, string column2, string column1Value);
        int? QueryIntColumnValue(string tableName, string column1, string column2, string column1Value);
        List<(string Name, string Description)> GetNameAndDescriptionFromTable(string tableName);
        List<(string Name1, string Description1, string Name2, string Description2)> GetPhongVanBangData();
        List<(string Name1, string Description1, string Description2)> GetPhongVanLechData();
        List<(string Name1, string Description1, string Name2, string Description2, string Name3, string Description3)> GetPhongVan3NguoiData();

    }

}
