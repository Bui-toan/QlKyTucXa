using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QlKyTucXa.Classes
{
	internal class DataProcesser
	{
		string strConnect = "Data Source=DUYTIEN\\SQLEXPRESS;" +
               "DataBase=QLKTX;User ID=sa;" +
			   "Password=123456789;Integrated Security=false";

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
		public object GetColumnValue(string sqlQuery, string username)
		{
			object result = null; // Biến để lưu giá trị trả về

			try
			{
				OpenConnection(); // Mở kết nối

				SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConncect);
				sqlCommand.Parameters.AddWithValue("@username", username); // Thêm tham số truy vấn

				result = sqlCommand.ExecuteScalar(); // Lấy giá trị của cột đầu tiên từ kết quả

				CloseConnection(); // Đóng kết nối
			}
			catch (Exception ex)
			{
				Console.WriteLine("Có lỗi xảy ra: " + ex.Message);
			}
			finally
			{
				CloseConnection();
			}

			return result; // Trả về giá trị
		}
	}
}
