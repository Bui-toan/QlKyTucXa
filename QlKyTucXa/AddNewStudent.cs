using QlKyTucXa.Classes;
using System;
using System.Windows.Forms;

namespace QlKyTucXa
{
	public partial class AddNewStudent : Form
	{
		DataProcesser dataProcesser = new DataProcesser();
		public AddNewStudent()
		{
			InitializeComponent();
		}

		private void AddNewStudent_Load(object sender, EventArgs e)
		{

		}
		private void ClearInputFields()
		{
			txtMaSinhVien.Clear();
			txtTenSinhVien.Clear();
			dtpNgaySinh.Value = DateTime.Now;
			rbtnNam.Checked = true;
			txtTenQue.Clear();
			txtTenKhoa.Clear();
			txtTenLop.Clear();
			cbPhong.SelectedIndex = -1;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			// Lấy dữ liệu từ các điều khiển
			string maSinhVien = txtMaSinhVien.Text.Trim();
			string tenSinhVien = txtTenSinhVien.Text.Trim();
			string ngaySinh = dtpNgaySinh.Value.ToString("yyyy-MM-dd");
			string gioiTinh = rbtnNam.Checked ? "Nam" : "Nữ";
			string tenQue = txtTenQue.Text.Trim();
			string tenKhoa = txtTenKhoa.Text.Trim();
			string tenLop = txtTenLop.Text.Trim();
			string phong = cbPhong.SelectedItem?.ToString();

			// Kiểm tra dữ liệu bắt buộc
			if (string.IsNullOrWhiteSpace(maSinhVien) || string.IsNullOrWhiteSpace(tenSinhVien))
			{
				MessageBox.Show("Vui lòng nhập đầy đủ thông tin bắt buộc.");
				return;
			}

			// Tạo chuỗi truy vấn SQL để thêm sinh viên
			string insertQuery = $"INSERT INTO Sinhvien (Masinhvien, Tensinhvien, Ngaysinh, Gioitinh, Tenque, Tenkhoa, Tenlop) " +
								 $"VALUES (N'{maSinhVien}', N'{tenSinhVien}', '{ngaySinh}', N'{gioiTinh}', N'{tenQue}', N'{tenKhoa}', N'{tenLop}')";
			try
			{
				dataProcesser.ChangeData(insertQuery);

				// Thông báo thêm sinh viên thành công
				MessageBox.Show("Thêm sinh viên thành công!");

				// Xóa trắng các trường nhập liệu
				ClearInputFields();

			}
			catch (Exception ex)
			{
				MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
			}
		}

	}
}
