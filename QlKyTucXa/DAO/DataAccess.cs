using System;
using System.Data;
using System.Data.SqlClient;

namespace QlKyTucXa.DAO
{
    public class DataAccess
    {
        private readonly string _connectionString;

        public DataAccess()
        {

            _connectionString = "Data Source=DESKTOP-KHDUPVB\\SQLEXPRESS;" +
                "DataBase=QLKTX;Integrated Security=true";
        }

        // Method to execute non-query commands (e.g., INSERT, UPDATE, DELETE)
        public bool ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        // Method to execute scalar queries (e.g., single value queries like COUNT, SUM)
        public object ExecuteScalar(string query, SqlParameter[] parameters = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    try
                    {
                        connection.Open();
                        return command.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        return null;
                    }
                }
            }
        }

        // Method to retrieve data as a DataTable (useful for displaying in DataGridView)
        public DataTable GetDataTable(string query, SqlParameter[] parameters = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    using (var adapter = new SqlDataAdapter(command))
                    {
                        var dataTable = new DataTable();
                        try
                        {
                            adapter.Fill(dataTable);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                        }
                        return dataTable;
                    }
                }
            }
        }

        // Method to retrieve data using a data reader (useful for reading data row-by-row)
        public void ExecuteReader(string query, SqlParameter[] parameters = null, Action<SqlDataReader> processRow = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    try
                    {
                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                processRow?.Invoke(reader);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
        }
    }
}
