using QlKyTucXa.Classes;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace QlKyTucXa
{
	public partial class ChiTietPhong123 : Form
	{
		private DataProcesser dataProcesser;
		private string maPhong;

		public ChiTietPhong123(string maPhong)
		{
			InitializeComponent();
			dataProcesser = new DataProcesser();
			this.maPhong = maPhong;
			LoadThongTinPhong();
			LoadThietBiPhong();
		}
		private void LoadMaThietbiData()
		{
			try
			{
				// Truy vấn lấy Mã thiết bị
				string query = "SELECT mathietbi FROM Thietbi";

				// Lấy dữ liệu từ cơ sở dữ liệu
				DataTable dtMaThietbi = dataProcesser.ReadData(query);

				// Gán dữ liệu cho ComboBox Mã thiết bị
				cmbMathietbi.Items.Clear(); // Xóa các mục cũ
				foreach (DataRow row in dtMaThietbi.Rows)
				{
					cmbMathietbi.Items.Add(row["mathietbi"].ToString());
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Có lỗi khi tải danh sách mã thiết bị: " + ex.Message);
			}
		}
		// Load thông tin chi tiết phòng
		private void LoadThongTinPhong()
		{
			try
			{
				string query = "SELECT MaPhong, Tenphong, Tennha, Loaiphong, Songuoitoida, Songuoidao, Ghichu, TienPhong " +
							   $"FROM Phong WHERE MaPhong = '{maPhong}'";

				// Sử dụng DataProcesser để thực thi truy vấn
				DataTable phongTable = dataProcesser.ExecuteQuery(query);

				if (phongTable.Rows.Count > 0)
				{
					dataGridViewThongTinPhong.DataSource = phongTable;
					CustomizeDataGridThongTinPhong();
				}
				else
				{
					MessageBox.Show("Không tìm thấy thông tin phòng.", "Thông báo");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Lỗi khi tải thông tin phòng: " + ex.Message);
			}
		}



		// Load thông tin thiết bị trong phòng
		private void LoadThietBiPhong()
		{
			try
			{
				string query = "SELECT tb.Mathietbi, tb.Tenthietbi, tbp.Soluong, tbp.Tinhtrang " +
							   "FROM Thietbi_phong tbp " +
							   "JOIN Thietbi tb ON tbp.Mathietbi = tb.Mathietbi " +
							   $"WHERE tbp.MaPhong = '{maPhong}'";

				DataTable thietBiTable = dataProcesser.ExecuteQuery(query);

				if (thietBiTable.Rows.Count > 0)
				{
					dataGridViewThietBiPhong.DataSource = thietBiTable;
					CustomizeDataGridThietBiPhong();
				}
				else
				{
					MessageBox.Show("Không có thiết bị trong phòng này.", "Thông báo");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Lỗi khi tải thông tin thiết bị: " + ex.Message);
			}
		}


		// css
		private void CustomizeDataGridView(DataGridView dataGridView)
		{
			// Màu nền DataGridView
			dataGridView.BackgroundColor = Color.White;
			dataGridView.GridColor = Color.Gainsboro;
			dataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

			// Tùy chỉnh Header
			dataGridView.EnableHeadersVisualStyles = false;
			dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGreen;
			dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
			dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
			dataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

			// Tùy chỉnh ô dữ liệu
			dataGridView.DefaultCellStyle.BackColor = Color.White;
			dataGridView.DefaultCellStyle.ForeColor = Color.DimGray;
			dataGridView.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
			dataGridView.DefaultCellStyle.SelectionBackColor = Color.CornflowerBlue;
			dataGridView.DefaultCellStyle.SelectionForeColor = Color.White;
			dataGridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

			// Tùy chỉnh hàng xen kẽ
			dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.Lavender;

			// Tùy chỉnh Row Template
			dataGridView.RowTemplate.Height = 35;

			// Chế độ tự động điều chỉnh kích thước cột
			dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

			// Viền và ẩn cột Row Header
			dataGridView.BorderStyle = BorderStyle.Fixed3D;
			dataGridView.RowHeadersVisible = false;
		}


		// Tùy chỉnh DataGridView thông tin phòng
		private void CustomizeDataGridThongTinPhong()
		{
			dataGridViewThongTinPhong.Columns["MaPhong"].HeaderText = "Mã Phòng";
			dataGridViewThongTinPhong.Columns["Tenphong"].HeaderText = "Tên Phòng";
			dataGridViewThongTinPhong.Columns["Tennha"].HeaderText = "Tên Tòa";
			dataGridViewThongTinPhong.Columns["Loaiphong"].HeaderText = "Loại Phòng";
			dataGridViewThongTinPhong.Columns["Songuoitoida"].HeaderText = "Số Người Tối Đa";
			dataGridViewThongTinPhong.Columns["Songuoidao"].HeaderText = "Số Người Đã Ở";
			dataGridViewThongTinPhong.Columns["Ghichu"].HeaderText = "Ghi Chú";
			dataGridViewThongTinPhong.Columns["TienPhong"].HeaderText = "Tiền Phòng";

			dataGridViewThongTinPhong.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			dataGridViewThongTinPhong.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			CustomizeDataGridView(dataGridViewThongTinPhong);
		}

		// Tùy chỉnh DataGridView thiết bị trong phòng
		private void CustomizeDataGridThietBiPhong()
		{
			dataGridViewThietBiPhong.Columns["mathietbi"].HeaderText = "Mã Thiết Bị";
			dataGridViewThietBiPhong.Columns["tenthietbi"].HeaderText = "Tên Thiết Bị";
			dataGridViewThietBiPhong.Columns["soluong"].HeaderText = "Số Lượng";
			dataGridViewThietBiPhong.Columns["Tinhtrang"].HeaderText = "Tình Trạng";

			dataGridViewThietBiPhong.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			dataGridViewThietBiPhong.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			CustomizeDataGridView(dataGridViewThietBiPhong);
		}

		private void ChiTietPhong_Load(object sender, EventArgs e)
		{
			this.Location = new Point(445, 10);
			LoadMaThietbiData();
		}

		private void groupBox1_Enter(object sender, EventArgs e)
		{

		}

		private void dataGridViewThietBiPhong_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex >= 0)
			{
				DataGridViewRow row = dataGridViewThietBiPhong.Rows[e.RowIndex];
				cmbMathietbi.Text = row.Cells["mathietbi"].Value.ToString();
				txtTenThietBi.Text = row.Cells["tenthietbi"].Value.ToString();
				numSoLuong.Value = Convert.ToInt32(row.Cells["soluong"].Value);
				txtTinhTrang.Text = row.Cells["Tinhtrang"].Value.ToString(); // Gán giá trị vào TextBox
				txtTinhTrang.Enabled = true; // Đảm bảo TextBox có thể chỉnh sửa
			}
		}

		private void btnThem_Click(object sender, EventArgs e)
		{
			try
			{
				string mathietbi = cmbMathietbi.SelectedItem.ToString(); // Lấy mã thiết bị từ ComboBox
				string tenthietbi = txtTenThietBi.Text; // Tên thiết bị (được điền tự động)

				// Kiểm tra xem mã thiết bị đã tồn tại trong bảng Thietbi chưa
				string checkQuery = $"SELECT COUNT(*) FROM Thietbi_phong WHERE Mathietbi = '{mathietbi}' AND MaPhong = '{maPhong}'";
				DataTable dtCheck = dataProcesser.ReadData(checkQuery);

				// Nếu đã tồn tại, hiển thị thông báo và dừng thực hiện
				if (dtCheck.Rows.Count > 0 && Convert.ToInt32(dtCheck.Rows[0][0]) > 0)
				{
					MessageBox.Show("Thiết bị đã tồn tại trong phòng này!", "Thông báo");
					return; // Ngừng thêm
				}
				else if (numSoLuong.Value <= 0)
				{
					MessageBox.Show("Phải nhập số lượng thiết bị!", "Thông báo");
					return; // Ngừng thêm
				}
				else
				{
					string query = $"INSERT INTO Thietbi_phong (MaPhong, Mathietbi, soluong, Tinhtrang) " +
							   $"VALUES ('{maPhong}', '{mathietbi}', {numSoLuong.Value}, N'{txtTinhTrang.Text}')";
					dataProcesser.ChangeData(query);

					LoadThietBiPhong(); // Tải lại dữ liệu
					MessageBox.Show("Thêm thiết bị vào phòng thành công!", "Thông báo");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Lỗi khi thêm thiết bị: " + ex.Message);
			}

		}



		private void btnSua_Click(object sender, EventArgs e)
		{
			try
			{
				if (cmbMathietbi.SelectedItem == null)
				{
					MessageBox.Show("Vui lòng chọn thiết bị để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}

				string mathietbi = cmbMathietbi.SelectedItem.ToString(); // Mã thiết bị từ ComboBox
				string tinhtrang = txtTinhTrang.Text;

				if (string.IsNullOrWhiteSpace(tinhtrang))
				{
					MessageBox.Show("Vui lòng nhập đầy đủ tình trạng thiết bị!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}
				string checkQuery = $"SELECT COUNT(*) FROM Thietbi_phong WHERE Mathietbi = '{mathietbi}' AND MaPhong = '{maPhong}'";
				DataTable dtCheck = dataProcesser.ReadData(checkQuery);

				// Nếu đã tồn tại, hiển thị thông báo và dừng thực hiện

				if (numSoLuong.Value <= 0)
				{
					MessageBox.Show("Phải nhập số lượng thiết bị!", "Thông báo");
					return; // Ngừng thêm
				}
				else
				{
					// Cập nhật thông tin thiết bị trong bảng Thietbi_phong
					string updateQuery = $"UPDATE Thietbi_phong " +
									 $"SET soluong = {numSoLuong.Value}, Tinhtrang = N'{tinhtrang}' " +
									 $"WHERE MaPhong = '{maPhong}' AND Mathietbi = '{mathietbi}'";

					dataProcesser.ChangeData(updateQuery);
					LoadThietBiPhong(); // Tải lại dữ liệu thiết bị trong phòng
					MessageBox.Show("Cập nhật thiết bị thành công!", "Thông báo");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Lỗi khi cập nhật thiết bị: " + ex.Message);
			}

		}



		private void btnXoa_Click(object sender, EventArgs e)
		{
			try
			{
				// Kiểm tra xem thiết bị đã được chọn chưa
				if (cmbMathietbi.SelectedItem == null)
				{
					MessageBox.Show("Vui lòng chọn thiết bị để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}

				string mathietbi = cmbMathietbi.SelectedItem.ToString(); // Mã thiết bị từ ComboBox

				string deleteQuery = $"DELETE FROM Thietbi_phong WHERE MaPhong = '{maPhong}' AND Mathietbi = '{mathietbi}'";
				dataProcesser.ChangeData(deleteQuery);

				string query = "SELECT tb.Mathietbi, tb.Tenthietbi, tbp.Soluong, tbp.Tinhtrang " +
							   "FROM Thietbi_phong tbp " +
							   "JOIN Thietbi tb ON tbp.Mathietbi = tb.Mathietbi " +
							   $"WHERE tbp.MaPhong = '{maPhong}'";

				DataTable thietBiTable = dataProcesser.ExecuteQuery(query);
				dataGridViewThietBiPhong.DataSource = thietBiTable;

				MessageBox.Show("Xóa thiết bị thành công!", "Thông báo");

				LoadThietBiPhong(); // Tải lại dữ liệu thiết bị trong phòng

				CustomizeDataGridThietBiPhong();

			}
			catch (Exception ex)
			{
				MessageBox.Show("Lỗi khi xóa thiết bị: " + ex.Message);
			}
		}

		private void btnThoat_Click(object sender, EventArgs e)
		{
			AddNewRoom addNewRoomForm = new AddNewRoom();
			addNewRoomForm.Owner = this; // Gán Form trước làm Owner
			addNewRoomForm.Show();
			this.Hide(); // Ẩn Form trước
		}

		private void txtMaThietBi_TextChanged(object sender, EventArgs e)
		{

		}

		private void cmbMathietbi_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				// Kiểm tra xem ComboBox có chọn được giá trị không
				if (cmbMathietbi.SelectedItem != null)
				{
					string selectedMaThietbi = cmbMathietbi.SelectedItem.ToString();

					// Truy vấn lấy tên thiết bị (tenthietbi) từ mã thiết bị
					string query = $"SELECT tenthietbi FROM Thietbi WHERE mathietbi = '{selectedMaThietbi}'";

					// Lấy dữ liệu tên thiết bị từ cơ sở dữ liệu
					DataTable dtTenthietbi = dataProcesser.ReadData(query);

					// Kiểm tra xem có dữ liệu trả về không
					if (dtTenthietbi.Rows.Count > 0)
					{
						string tenthietbi = dtTenthietbi.Rows[0]["tenthietbi"].ToString();
						txtTenThietBi.Text = tenthietbi;  // Gán tên thiết bị vào ComboBox

						// Đổi màu chữ của ComboBox Tenthietbi
						txtTenThietBi.ForeColor = Color.Red;  // Đổi màu chữ thành màu đỏ (có thể thay đổi theo ý muốn)

						txtTenThietBi.Enabled = false;  // Tắt khả năng thay đổi ComboBox
					}
					else
					{
						MessageBox.Show("Không tìm thấy tên thiết bị cho mã thiết bị đã chọn.");
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Lỗi khi tải tên thiết bị: " + ex.Message);
			}
		}

		private void txtTenThietBi_TextChanged(object sender, EventArgs e)
		{

		}
	}
}
