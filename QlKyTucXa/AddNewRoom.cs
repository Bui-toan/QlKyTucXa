using QlKyTucXa.Classes;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace QlKyTucXa
{
	public partial class AddNewRoom : Form
	{
		DataProcesser dataProcesser = new DataProcesser(); // Khởi tạo đối tượng DataProcesser

		public AddNewRoom()
		{
			InitializeComponent();
			LoadComboBoxData();
		}

		// Nạp dữ liệu vào ComboBox
		private void LoadComboBoxData()
		{
			try
			{
				// Nạp dữ liệu cho comboBoxToa (Tennha)
				string queryToa = "SELECT DISTINCT Tennha FROM Phong";
				DataTable toaTable = dataProcesser.ReadData(queryToa);
				comboBoxToa.DataSource = toaTable;
				comboBoxToa.DisplayMember = "Tennha";

				// Nạp dữ liệu cho comboBoxLoaiPhong
				string queryLoaiPhong = "SELECT DISTINCT Loaiphong FROM Phong";
				DataTable loaiPhongTable = dataProcesser.ReadData(queryLoaiPhong);
				comboBoxLoaiPhong.DataSource = loaiPhongTable;
				comboBoxLoaiPhong.DisplayMember = "Loaiphong";
			}
			catch (Exception ex)
			{
				MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
			}
		}

		// Sự kiện khi nhấn nút Tìm Kiếm
		private void buttonTimKiem_Click(object sender, EventArgs e)
		{
			string maPhong = textBoxMaPhong.Text.Trim();
			string tenPhong = textBoxTenPhong.Text.Trim();
			string tenNha = comboBoxToa.Text.Trim();
			string loaiPhong = comboBoxLoaiPhong.Text.Trim();

			string query = "SELECT MaPhong, Tenphong, Tennha, Loaiphong, Songuoitoida, Songuoidao FROM Phong WHERE 1=1";
			if (!string.IsNullOrEmpty(maPhong))
				query += " AND MaPhong = @MaPhong";
			if (!string.IsNullOrEmpty(tenPhong))
				query += " AND Tenphong LIKE @TenPhong";
			if (!string.IsNullOrEmpty(tenNha))
				query += " AND Tennha = @Tennha";
			if (!string.IsNullOrEmpty(loaiPhong))
				query += " AND Loaiphong = @Loaiphong";

			try
			{
				// Đọc dữ liệu từ cơ sở dữ liệu
				DataTable dt = dataProcesser.ReadData(query.Replace("@MaPhong", $"'{maPhong}'")
														   .Replace("@TenPhong", $"'%{tenPhong}%'")
														   .Replace("@Tennha", $"'{tenNha}'")
														   .Replace("@Loaiphong", $"'{loaiPhong}'"));

				if (dt.Rows.Count > 0)
				{
					dataGridView1.DataSource = dt;
					CustomizeDataGridView();

					// Hiển thị các cột cần thiết
					foreach (DataGridViewColumn column in dataGridView1.Columns)
					{
						column.Visible = false;
					}

					dataGridView1.Columns["MaPhong"].Visible = true;
					dataGridView1.Columns["Tenphong"].Visible = true;
					dataGridView1.Columns["Tennha"].Visible = true;
					dataGridView1.Columns["Loaiphong"].Visible = true;

					dataGridView1.Columns["MaPhong"].HeaderText = "Mã Phòng";
					dataGridView1.Columns["Tenphong"].HeaderText = "Tên Phòng";
					dataGridView1.Columns["Tennha"].HeaderText = "Tên Tòa";
					dataGridView1.Columns["Loaiphong"].HeaderText = "Loại Phòng";

				}
				else
				{
					MessageBox.Show("Không tìm thấy phòng phù hợp.", "Thông báo");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi");
			}
		}


		// Cài đặt hiển thị DataGridView
		private void CustomizeDataGridView()
		{
			// Thay đổi màu nền
			dataGridView1.BackgroundColor = Color.White;

			// Cài đặt lưới
			dataGridView1.GridColor = Color.LightGray;
			dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

			// Thay đổi header (tiêu đề cột)
			dataGridView1.EnableHeadersVisualStyles = false; // Bắt buộc để tùy chỉnh tiêu đề
			dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.LightSkyBlue;
			dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
			dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
			dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

			// Thay đổi dòng
			dataGridView1.DefaultCellStyle.BackColor = Color.White;
			dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
			dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
			dataGridView1.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
			dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black;

			// Thay đổi dòng xen kẽ
			dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;

			// Căn chỉnh văn bản trong tiêu đề và ô
			dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

			// Tùy chỉnh chiều cao và độ rộng
			dataGridView1.RowTemplate.Height = 30;
			dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

			// Thay đổi đường viền và các cài đặt chung
			dataGridView1.BorderStyle = BorderStyle.Fixed3D;
			dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
			dataGridView1.RowHeadersVisible = false; // Ẩn cột row header
		}

		// Sự kiện CellFormatting để đổi màu các dòng


		private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if (dataGridView1.Rows[e.RowIndex].DataBoundItem != null)
			{
				var row = dataGridView1.Rows[e.RowIndex];
				int songuoitoida = Convert.ToInt32(row.Cells["Songuoitoida"].Value ?? 0);
				int songuoidao = Convert.ToInt32(row.Cells["Songuoidao"].Value ?? 0);

				// So sánh và thay đổi màu sắc của dòng
				if (songuoitoida == songuoidao)
				{
					row.DefaultCellStyle.BackColor = Color.Red;
					row.DefaultCellStyle.ForeColor = Color.White;
				}
				else
				{
					row.DefaultCellStyle.BackColor = Color.Green;
					row.DefaultCellStyle.ForeColor = Color.White;
				}
			}
		}

		private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex >= 0) // Đảm bảo người dùng nhấp vào dòng hợp lệ
			{
				string maPhong = dataGridView1.Rows[e.RowIndex].Cells["MaPhong"].Value.ToString();

				// Mở form chi tiết phòng
				ChiTietPhong123 chitietPhongForm = new ChiTietPhong123(maPhong);
				chitietPhongForm.ShowDialog();

			}
		}

		private void AddNewRoom_Load(object sender, EventArgs e)
		{
			this.Location = new Point(475, 160);
		}

		private void guna2Button1_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
