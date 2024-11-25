using QlKyTucXa.Classes;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QlKyTucXa
{
	public partial class AddNewStudent : Form
	{
		private string maQueGoc = string.Empty;
		private string maKhoaGoc = string.Empty;
		private string maLopGoc = string.Empty;
		private string maPhongGoc = string.Empty;
		DataProcesser dataProcesser = new DataProcesser();
		public AddNewStudent()
		{
			InitializeComponent();
		}

		private void AddNewStudent_Load(object sender, EventArgs e)
		{
			this.Location = new Point(485, 170);
			LoadPhongData();
			LoadMaQueData();
			LoadMaLopData();
			LoadMaKhoaData();
		}
		private void ClearInputFields()
		{
			txtMaSoThue.Clear();
			txtMaSinhVien.Clear();
			txtTenSinhVien.Clear();
			dtpNgaySinh.Value = DateTime.Now;
			rbtnNam.Checked = true;
			cbMaQue.SelectedIndex = -1;
			cbMaKhoa.SelectedIndex = -1;
			cbMaLop.SelectedIndex = -1;
			cbPhong.SelectedIndex = -1;
			dtpNgayBatDau.Value = DateTime.Now;
			dtpNgayKetThuc.Value = DateTime.Now;
			txtGhiChu.Clear();
			pbAnh.Image = null;
		}
		private void LoadMaQueData()
		{
			try
			{
				// Truy vấn lấy Mã quê
				string query = "SELECT Maque FROM Que";

				// Lấy dữ liệu từ cơ sở dữ liệu
				DataTable dtMaQue = dataProcesser.ReadData(query);

				// Gán dữ liệu cho ComboBox
				cbMaQue.Items.Clear(); // Xóa các mục cũ
				foreach (DataRow row in dtMaQue.Rows)
				{
					cbMaQue.Items.Add(row["Maque"].ToString());
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Có lỗi khi tải danh sách mã quê: " + ex.Message);
			}
		}
		private void LoadMaLopData()
		{
			try
			{
				string query = "SELECT Malop FROM Lop";
				DataTable dtMaLop = dataProcesser.ReadData(query);

				cbMaLop.Items.Clear(); // Xóa các mục cũ
				foreach (DataRow row in dtMaLop.Rows)
				{
					cbMaLop.Items.Add(row["Malop"].ToString());
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Có lỗi khi tải danh sách mã lớp: " + ex.Message);
			}
		}
		private void LoadMaKhoaData()
		{
			try
			{
				string query = "SELECT Makhoa FROM Khoa";
				DataTable dtMaKhoa = dataProcesser.ReadData(query);

				cbMaKhoa.Items.Clear(); // Xóa các mục cũ
				foreach (DataRow row in dtMaKhoa.Rows)
				{
					cbMaKhoa.Items.Add(row["Makhoa"].ToString());
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Có lỗi khi tải danh sách mã khoa: " + ex.Message);
			}
		}
		private void LoadPhongData()
		{
			try
			{
				// Truy vấn SQL để lấy danh sách phòng
				string query = "SELECT Maphong FROM Phong WHERE Songuoidao < Songuoitoida;";

				// Lấy dữ liệu từ cơ sở dữ liệu
				DataTable dtPhong = dataProcesser.ReadData(query);

				// Xóa dữ liệu cũ trong ComboBox
				cbPhong.Items.Clear();

				// Thêm các giá trị vào ComboBox
				foreach (DataRow row in dtPhong.Rows)
				{
					cbPhong.Items.Add(row["Maphong"].ToString());
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Có lỗi khi tải danh sách phòng: " + ex.Message);
			}
		}

		private void btnXoa_Click(object sender, EventArgs e)
		{
			var result = MessageBox.Show("Bạn có muốn xóa thông tin vừa nhập không?", "Xác nhận", MessageBoxButtons.YesNo);
			if (result == DialogResult.Yes)
			{
				ClearInputFields();
			}
		}

		private void btnChonAnh_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				// Lấy đường dẫn tệp
				string filePath = openFileDialog.FileName;

				// Hiển thị ảnh trong PictureBox
				pbAnh.Image = Image.FromFile(filePath);

				// Lưu đường dẫn ảnh vào TextBox ẩn
				txtDuongDanAnh.Text = filePath;
			}
		}

		private void btnTimKiem_Click(object sender, EventArgs e)
		{
			// Lấy mã sinh viên từ TextBox tìm kiếm
			string maSinhVien = txtMaSinhVien.Text.Trim();

			// Kiểm tra nếu TextBox trống
			if (string.IsNullOrEmpty(maSinhVien))
			{
				MessageBox.Show("Vui lòng nhập mã sinh viên để tìm kiếm.");
				return;
			}

			try
			{
				// Tạo câu lệnh truy vấn SQL để lấy thông tin sinh viên
				string querySinhVien = $"SELECT * FROM Sinhvien WHERE Masinhvien = '{maSinhVien}'";

				// Tạo câu lệnh truy vấn SQL để lấy thông tin thuê phòng
				string querySVPhong = $"SELECT * FROM SV_Phong WHERE Masv = '{maSinhVien}'";

				// Lấy dữ liệu từ cơ sở dữ liệu
				DataTable dtSinhVien = dataProcesser.ReadData(querySinhVien);
				DataTable dtSVPhong = dataProcesser.ReadData(querySVPhong);

				// Kiểm tra nếu không tìm thấy sinh viên
				if (dtSinhVien.Rows.Count == 0)
				{
					MessageBox.Show("Không tìm thấy sinh viên với mã này.");
					return;
				}

				// Lấy thông tin sinh viên và hiển thị vào các TextBox
				DataRow sinhVienRow = dtSinhVien.Rows[0];
				txtMaSinhVien.Text = sinhVienRow["Masinhvien"].ToString();
				txtTenSinhVien.Text = sinhVienRow["Tensinhvien"].ToString();
				dtpNgaySinh.Value = DateTime.Parse(sinhVienRow["Ngaysinh"].ToString());
				string gioiTinh = sinhVienRow["Gioitinh"].ToString();
				if (gioiTinh == "Nam")
				{
					rbtnNam.Checked = true;
				}
				else
				{
					rbtnNu.Checked = true;
				}
				cbMaQue.Text = sinhVienRow["Maque"].ToString();
				cbMaKhoa.Text = sinhVienRow["Makhoa"].ToString();
				cbMaLop.Text = sinhVienRow["Malop"].ToString();
				txtDuongDanAnh.Text = sinhVienRow["DuongDanAnh"].ToString();
				// Hiển thị ảnh (nếu có) vào PictureBox
				if (!string.IsNullOrEmpty(txtDuongDanAnh.Text))
				{
					pbAnh.Image = Image.FromFile(txtDuongDanAnh.Text);
				}
				else
				{
					pbAnh.Image = null;
				}

				// Kiểm tra nếu không tìm thấy thông tin thuê phòng
				if (dtSVPhong.Rows.Count == 0)
				{
					MessageBox.Show("Sinh viên này chưa có thông tin thuê phòng.");
					return;
				}

				// Lấy thông tin thuê phòng và hiển thị
				DataRow svPhongRow = dtSVPhong.Rows[0];
				cbPhong.SelectedItem = svPhongRow["Maphong"].ToString();
				dtpNgayBatDau.Value = DateTime.Parse(svPhongRow["NgayBdau"].ToString());
				dtpNgayKetThuc.Value = DateTime.Parse(svPhongRow["Ngaykt"].ToString());
				txtGhiChu.Text = svPhongRow["Ghichu"].ToString();
				txtMaSoThue.Text = svPhongRow["MaSoThue"].ToString();
				maQueGoc = sinhVienRow["Maque"].ToString();
				maKhoaGoc = sinhVienRow["Makhoa"].ToString();
				maLopGoc = sinhVienRow["Malop"].ToString();
				maPhongGoc = svPhongRow["Maphong"].ToString();

			}
			catch (Exception ex)
			{
				MessageBox.Show("Có lỗi khi tìm kiếm: " + ex.Message);
			}
		}

		private void btnSua_Click(object sender, EventArgs e)
		{
			string[] inputData = new string[]
			{
				txtMaSoThue.Text.Trim(),
				txtMaSinhVien.Text.Trim(),
				txtTenSinhVien.Text.Trim(),
				dtpNgaySinh.Value.ToString("yyyy-MM-dd"),
				rbtnNam.Checked ? "Nam" : "Nữ",
				cbMaQue.SelectedItem?.ToString(),
				cbMaKhoa.SelectedItem?.ToString(),
				cbMaLop.SelectedItem?.ToString(),
				cbPhong.SelectedItem?.ToString(),
				dtpNgayBatDau.Value.ToString("yyyy-MM-dd"),
				dtpNgayKetThuc.Value.ToString("yyyy-MM-dd"),
				txtGhiChu.Text.Trim(),
				txtDuongDanAnh.Text.Trim()
			 };

			// Kiểm tra dữ liệu đầu vào
			if (inputData.Any(string.IsNullOrWhiteSpace))
			{
				MessageBox.Show("Vui lòng nhập đầy đủ thông tin vào tất cả các trường.");
				return;
			}

			if (DateTime.Parse(inputData[10]) < DateTime.Parse(inputData[9]))
			{
				MessageBox.Show("Ngày kết thúc không thể nhỏ hơn ngày bắt đầu.");
				return;
			}
			// Lấy dữ liệu hiện tại
			string maSinhVien = txtMaSinhVien.Text.Trim();
			string tenSinhVien = txtTenSinhVien.Text.Trim();
			string ngaySinh = dtpNgaySinh.Value.ToString("yyyy-MM-dd");
			string gioiTinh = rbtnNam.Checked ? "Nam" : "Nữ";
			string maQueMoi = cbMaQue.SelectedItem?.ToString();
			string maKhoaMoi = cbMaKhoa.SelectedItem?.ToString();
			string maLopMoi = cbMaLop.SelectedItem?.ToString();
			string maPhongMoi = cbPhong.SelectedItem?.ToString();
			string ngayBatDau = dtpNgayBatDau.Value.ToString("yyyy-MM-dd");
			string ngayKetThuc = dtpNgayKetThuc.Value.ToString("yyyy-MM-dd");
			string ghiChu = txtGhiChu.Text.Trim();
			string duongDanAnh = txtDuongDanAnh.Text.ToString();

			if (string.IsNullOrWhiteSpace(maSinhVien))
			{
				MessageBox.Show("Vui lòng nhập mã sinh viên để sửa.");
				return;
			}

			var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn sửa thông tin không?", "Xác nhận sửa", MessageBoxButtons.YesNo);
			if (confirmResult == DialogResult.Yes)
			{
				try
				{
					// Cập nhật bảng Sinhvien
					string updateSinhVienQuery = $"UPDATE Sinhvien SET " +
												 $"Tensinhvien = N'{tenSinhVien}', " +
												 $"Ngaysinh = '{ngaySinh}', " +
												 $"Gioitinh = N'{gioiTinh}', " +
												 $"Maque = N'{maQueMoi}', " +
												 $"Makhoa = N'{maKhoaMoi}', " +
												 $"Malop = N'{maLopMoi}', " +
												 $"DuongDanAnh = '{duongDanAnh}' " +
												 $"WHERE Masinhvien = N'{maSinhVien}'";
					dataProcesser.ChangeData(updateSinhVienQuery);

					// Cập nhật bảng SV_Phong nếu thay đổi mã phòng

					string updateSVPhongQuery = $"UPDATE SV_Phong SET " +
												 $"Maphong = N'{maPhongMoi}', " +
												 $"NgayBdau = '{ngayBatDau}', " +
												 $"Ngaykt = '{ngayKetThuc}', " +
												 $"Ghichu = N'{ghiChu}' " +
												 $"WHERE Masv = N'{maSinhVien}'";
					dataProcesser.ChangeData(updateSVPhongQuery);


					MessageBox.Show("Thông tin sinh viên đã được cập nhật thành công.");
				}
				catch (Exception ex)
				{
					MessageBox.Show("Có lỗi xảy ra khi sửa thông tin: " + ex.Message);
				}
			}
		}

		private void btnThem_Click(object sender, EventArgs e)
		{
			string[] inputData = new string[]
			{
				txtMaSoThue.Text.Trim(),
				txtMaSinhVien.Text.Trim(),
				txtTenSinhVien.Text.Trim(),
				dtpNgaySinh.Value.ToString("yyyy-MM-dd"),
				rbtnNam.Checked ? "Nam" : "Nữ",
				cbMaQue.SelectedItem?.ToString(),
				cbMaKhoa.SelectedItem?.ToString(),
				cbMaLop.SelectedItem?.ToString(),
				cbPhong.SelectedItem?.ToString(),
				dtpNgayBatDau.Value.ToString("yyyy-MM-dd"),
				dtpNgayKetThuc.Value.ToString("yyyy-MM-dd"),
				txtGhiChu.Text.Trim(),
				txtDuongDanAnh.Text.Trim()
			 };

			// Kiểm tra dữ liệu đầu vào
			if (inputData.Any(string.IsNullOrWhiteSpace))
			{
				MessageBox.Show("Vui lòng nhập đầy đủ thông tin vào tất cả các trường.");
				return;
			}

			if (DateTime.Parse(inputData[10]) < DateTime.Parse(inputData[9]))
			{
				MessageBox.Show("Ngày kết thúc không thể nhỏ hơn ngày bắt đầu.");
				return;
			}

			// Kiểm tra trùng khóa chính
			string checkSinhVienQuery = $"SELECT COUNT(*) FROM Sinhvien WHERE Masinhvien = '{inputData[1]}'";
			int existsSinhVien = (int)dataProcesser.GetScalarValue(checkSinhVienQuery);
			if (existsSinhVien > 0)
			{
				MessageBox.Show("Mã sinh viên đã tồn tại. Vui lòng nhập mã khác.");
				return;
			}

			string checkSVPhongQuery = $"SELECT COUNT(*) FROM SV_Phong WHERE MaSoThue = '{inputData[0]}'";
			int existsSVPhong = (int)dataProcesser.GetScalarValue(checkSVPhongQuery);
			if (existsSVPhong > 0)
			{
				MessageBox.Show("Mã số thuê đã tồn tại. Vui lòng nhập mã khác.");
				return;
			}

			// Truy vấn thêm dữ liệu vào bảng Sinhvien
			string insertSinhVien = $"INSERT INTO Sinhvien (Masinhvien, Tensinhvien, Ngaysinh, Gioitinh, Maque, Makhoa, Malop, DuongDanAnh) " +
									$"VALUES (N'{inputData[1]}', N'{inputData[2]}', '{inputData[3]}', N'{inputData[4]}', " +
									$"'{inputData[5]}', '{inputData[6]}', '{inputData[7]}', N'{inputData[12]}')";

			string insertSVPhong = $"INSERT INTO SV_Phong (MaSoThue, Masv, Maphong, NgayBdau, Ngaykt, Ghichu, TrangThai) " +
								   $"VALUES (N'{inputData[0]}', N'{inputData[1]}', N'{inputData[8]}', '{inputData[9]}', '{inputData[10]}', N'{inputData[11]}', 1)";
			string updatePhong = $"UPDATE Phong SET Songuoidao = Songuoidao + 1 WHERE MaPhong = '{inputData[8]}'";
			try
			{
				dataProcesser.ChangeData(insertSinhVien);
				dataProcesser.ChangeData(insertSVPhong);
				dataProcesser.ChangeData(updatePhong);

				MessageBox.Show("Thêm sinh viên và thông tin thuê phòng thành công!");
				ClearInputFields();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
			}
		}

		private void guna2Button1_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
