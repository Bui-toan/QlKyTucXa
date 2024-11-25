using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QlKyTucXa.Classes
{
    internal class DataProcesser
    {
        string strConnect = "Data Source=DESKTOP-KHDUPVB\\SQLEXPRESS;" +
                "DataBase=QLKTX;Integrated Security=true";


        SqlConnection sqlConncect = null;

        //Open a connection to Server
        void OpenConnection()
        {
            sqlConncect = new SqlConnection(strConnect);
            if (sqlConncect.State != ConnectionState.Open)
                sqlConncect.Open();
        }

        //Close a Connection
        void CloseConnection()
        {
            if (sqlConncect.State != ConnectionState.Closed)
            {
                sqlConncect.Close();
                sqlConncect.Dispose();
            }
        }
        //read Data from a Select statement and return a DataTable
        public DataTable ReadData(string sqlSelect)
        {
            DataTable dt = new DataTable();
            OpenConnection();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlSelect, sqlConncect);
            dataAdapter.Fill(dt);
            CloseConnection();
            dataAdapter.Dispose();
            return dt;
        }

        //Change Data 
        public void ChangeData(string sql)
        {
            OpenConnection();
            SqlCommand sqlcmm = new SqlCommand();
            sqlcmm.Connection = sqlConncect;
            sqlcmm.CommandText = sql;
            try
            {
                sqlcmm.ExecuteNonQuery();  // Thực thi câu lệnh SQL
            }
            catch (Exception ex)
            {
                // Log lỗi nếu có
                MessageBox.Show("Lỗi khi thực thi truy vấn: " + ex.Message);
            }
            finally
            {
                CloseConnection();  // Đóng kết nối sau khi thực hiện xong
                sqlcmm.Dispose();   // Giải phóng tài nguyên
            }
        }
        public object GetScalarValue(string query)
        {
            using (SqlConnection conn = new SqlConnection(strConnect))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                return cmd.ExecuteScalar();
            }
        }
        public DataTable ExecuteQuery(string query)
        {
            DataTable dataTable = new DataTable();
            try
            {
                OpenConnection(); // Mở kết nối
                using (SqlCommand cmd = new SqlCommand(query, sqlConncect))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable); // Đổ dữ liệu vào DataTable
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi thực thi truy vấn: " + ex.Message);
            }
            finally
            {
                CloseConnection(); // Đảm bảo đóng kết nối
            }
            return dataTable; // Trả về kết quả
        }

    }
}
