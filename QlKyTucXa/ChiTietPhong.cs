using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using QlKyTucXa.Classes;
using TheArtOfDevHtmlRenderer.Adapters;

namespace QlKyTucXa
{
    public partial class ChiTietPhong : Form
    {
        private DataProcesser dataProcesser;
        private string maPhong;

        public ChiTietPhong(string maPhong)
        {
            InitializeComponent();
            dataProcesser = new DataProcesser();
            this.maPhong = maPhong;
            LoadThongTinPhong();
            LoadThietBiPhong();
        }

        // Load thông tin chi tiết phòng
        private void LoadThongTinPhong()
        {
            try
            {
                string query = "SELECT MaPhong, Tenphong, Tennha, Loaiphong, Songuoitoida, Songuoidao, Ghichu, TienPhong " +
                               "FROM Phong WHERE MaPhong = @MaPhong";

                DataTable phongTable = new DataTable();

                using (SqlConnection connection = new SqlConnection("Data Source=DUYTIEN\\SQLEXPRESS;" +
                                                                     "DataBase=QLKTX;User ID=sa;" +
                                                                     "Password=123456789;Integrated Security=false"))
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@MaPhong", maPhong);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(phongTable);
                        }
                    }
                }

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
                               "WHERE tbp.MaPhong = @MaPhong";

                DataTable thietBiTable = new DataTable();

                using (SqlConnection connection = new SqlConnection("Data Source=DUYTIEN\\SQLEXPRESS;" +
                                                                     "DataBase=QLKTX;User ID=sa;" +
                                                                     "Password=123456789;Integrated Security=false"))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@MaPhong", maPhong);
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(thietBiTable);
                        }
                    }
                }

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

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void dataGridViewThietBiPhong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewThietBiPhong.Rows[e.RowIndex];
                txtMaThietBi.Text = row.Cells["mathietbi"].Value.ToString();
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
                // Kiểm tra xem Mathietbi có tồn tại trong bảng Thietbi hay không
                string checkQuery = "SELECT COUNT(*) FROM Thietbi WHERE mathietbi = @Mathietbi";
                bool isMathietbiExists = false;

                using (SqlConnection connection = new SqlConnection("Data Source=DUYTIEN\\SQLEXPRESS;" +
                                                                     "DataBase=QLKTX;User ID=sa;" +
                                                                     "Password=123456789;Integrated Security=false"))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(checkQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@Mathietbi", txtMaThietBi.Text);
                        isMathietbiExists = (int)cmd.ExecuteScalar() > 0;
                    }
                }

                // Nếu Mathietbi không tồn tại, thêm vào bảng Thietbi
                if (!isMathietbiExists)
                {
                    string insertThietbiQuery = "INSERT INTO Thietbi (mathietbi, tenthietbi) " +
                                                "VALUES (@Mathietbi, @TenThietbi)";

                    using (SqlConnection connection = new SqlConnection("Data Source=DUYTIEN\\SQLEXPRESS;" +
                                                                         "DataBase=QLKTX;User ID=sa;" +
                                                                         "Password=123456789;Integrated Security=false"))
                    {
                        connection.Open();
                        using (SqlCommand cmd = new SqlCommand(insertThietbiQuery, connection))
                        {
                            cmd.Parameters.AddWithValue("@Mathietbi", txtMaThietBi.Text);
                            cmd.Parameters.AddWithValue("@TenThietbi", txtTenThietBi.Text);

                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Thiết bị đã được thêm vào hệ thống!", "Thông báo");
                }

                // Thêm thiết bị vào phòng (bảng Thietbi_phong)
                string query = "INSERT INTO Thietbi_phong (MaPhong, Mathietbi, soluong, Tinhtrang) " +
                               "VALUES (@MaPhong, @Mathietbi, @SoLuong, @TinhTrang)";

                using (SqlConnection connection = new SqlConnection("Data Source=DUYTIEN\\SQLEXPRESS;" +
                                                                     "DataBase=QLKTX;User ID=sa;" +
                                                                     "Password=123456789;Integrated Security=false"))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@MaPhong", maPhong);
                        cmd.Parameters.AddWithValue("@Mathietbi", txtMaThietBi.Text);
                        cmd.Parameters.AddWithValue("@SoLuong", numSoLuong.Value);
                        cmd.Parameters.AddWithValue("@TinhTrang", txtTinhTrang.Text);

                        cmd.ExecuteNonQuery();
                    }
                }

                LoadThietBiPhong();
                MessageBox.Show("Thêm thiết bị vào phòng thành công!", "Thông báo");
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
                string query = "UPDATE Thietbi_phong SET soluong = @SoLuong, Tinhtrang = @TinhTrang " +
                               "WHERE MaPhong = @MaPhong AND Mathietbi = @Mathietbi";

                using (SqlConnection connection = new SqlConnection("Data Source=DUYTIEN\\SQLEXPRESS;" +
                                                                     "DataBase=QLKTX;User ID=sa;" +
                                                                     "Password=123456789;Integrated Security=false"))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@MaPhong", maPhong);
                        cmd.Parameters.AddWithValue("@Mathietbi", txtMaThietBi.Text); // Gửi giá trị vào SQL
                        cmd.Parameters.AddWithValue("@SoLuong", numSoLuong.Value);
                        cmd.Parameters.AddWithValue("@TinhTrang", txtTinhTrang.Text);

                        cmd.ExecuteNonQuery();
                    }
                }

                LoadThietBiPhong();
                MessageBox.Show("Cập nhật thiết bị thành công!", "Thông báo");
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
                string query = "DELETE FROM Thietbi_phong WHERE MaPhong = @MaPhong AND Mathietbi = @Mathietbi";

                using (SqlConnection connection = new SqlConnection("Data Source=DUYTIEN\\SQLEXPRESS;" +
                                                                     "DataBase=QLKTX;User ID=sa;" +
                                                                     "Password=123456789;Integrated Security=false"))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@MaPhong", maPhong);
                        cmd.Parameters.AddWithValue("@Mathietbi", txtMaThietBi.Text);

                        cmd.ExecuteNonQuery();
                    }
                }

                LoadThietBiPhong();
                MessageBox.Show("Xóa thiết bị thành công!", "Thông báo");
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
    }
}
