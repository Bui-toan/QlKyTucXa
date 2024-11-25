using Guna.UI2.WinForms;
using QlKyTucXa.DAO;
using QlKyTucXa.Singleton;
using QlKyTucXa.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QlKyTucXa
{
    public partial class CapNhatHoaDon : Form
    {
        private string maHoaDon;
        private readonly DataAccess _dataAccess;
        public CapNhatHoaDon(string maHoaDon)
        {
            InitializeComponent();
            _dataAccess = DaoSingleton.GetInstance();
            this.maHoaDon = maHoaDon;
        }

        private void CapNhatHoaDon_Load(object sender, EventArgs e)
        {
            SetUpHoaDon();
        }
        private void SetUpHoaDon()
        {
            cb_TrangThai.DataSource = new List<TrangThaiHoaDonItem>
            {
                new TrangThaiHoaDonItem {Key = "Chưa Thanh Toán",Value = 0 },
                new TrangThaiHoaDonItem {Key = "Đã Thanh Toán",Value = 1 },
            };
            cb_TrangThai.DisplayMember = "Key";
            cb_TrangThai.ValueMember = "Value";

            string sql = "SELECT H.MaHoaDon, H.Thang,H.Nam,H.Tiendien,H.Tiennuoc,H.Tienvesinh," +
                "H.NgayTao, H.TrangThai,P.Tienphong,P.Tenphong,P.Songuoidao,P.Songuoitoida,P.MaPhong " +
                "from HoaDon as H join Phong as P on H.MaPhong = P.MaPhong " +
                "where H.MaHoaDon = @MaHoaDon";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@MaHoaDon",maHoaDon)
            };
            _dataAccess.ExecuteReader(sql, parameters, reader =>
            {
                txt_MaHoaDon.Text = reader["MaHoaDon"].ToString();
                txt_Thang.Text = reader["Thang"].ToString();
                txt_Nam.Text = reader["Nam"].ToString();
                txt_SoDien.Text = TinhTienDienService.GetSoDien(decimal.Parse(reader["Tiendien"].ToString())).ToString();
                txt_TienDien.Text = decimal.Parse(reader["Tiendien"].ToString()).ToString("N0");
                txt_TienNuoc.Text = decimal.Parse(reader["Tiennuoc"].ToString()).ToString("N0");
                txt_TienPhong.Text = decimal.Parse(reader["Tienphong"].ToString()).ToString("N0");
                txt_TienVeSinh.Text = decimal.Parse(reader["Tienvesinh"].ToString()).ToString("N0");
                txt_MaPhong.Text = reader["MaPhong"].ToString();
                txt_TenPhong.Text = reader["Tenphong"].ToString();
                txt_SoNguoiDaO.Text = int.Parse(reader["Songuoidao"].ToString()).ToString();
                txt_SoNguoiToiDa.Text = int.Parse(reader["songuoitoida"].ToString()).ToString();
                cb_TrangThai.SelectedValue = int.Parse(reader["TrangThai"].ToString());
            });
            calcTongTien();
        }
        private void calcTongTien()
        {
            decimal tongTien = 0;
            if (!string.IsNullOrEmpty(txt_TienPhong.Text))
            {
                tongTien += decimal.Parse(txt_TienPhong.Text);
            }
            if (!string.IsNullOrEmpty(txt_TienDien.Text))
            {
                tongTien += decimal.Parse(txt_TienDien.Text);
            }
            if (!string.IsNullOrEmpty(txt_TienNuoc.Text))
            {
                tongTien += decimal.Parse(txt_TienNuoc.Text);
            }
            if (!string.IsNullOrEmpty(txt_TienVeSinh.Text))
            {
                tongTien += decimal.Parse(txt_TienVeSinh.Text);
            }
            lbl_TongTien.Text = CurrencyFormatter.FormatCurrency(tongTien);
        }
        private void txt_Tien_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void txt_SoDien_TextChange(object sender, EventArgs e)
        {
            var tiendien = TinhTienDienService.Calc(txt_SoDien.Text);
            txt_TienDien.Text = tiendien.ToString("N0");
        }
        private void txt_TienNuoc_TextChanged(object sender, EventArgs e)
        {
            txt_TienNuoc.TextChanged -= txt_TienNuoc_TextChanged;

            if (int.TryParse(txt_TienNuoc.Text.Replace(",", ""), out int number)) // Parse without separators
            {
                txt_TienNuoc.Text = number.ToString("N0");
                txt_TienNuoc.SelectionStart = txt_TienNuoc.Text.Length; // Set cursor to the end
            }

            txt_TienNuoc.TextChanged += txt_TienNuoc_TextChanged;
        }
        private void txt_TienVeSinh_TextChanged(object sender, EventArgs e)
        {
            txt_TienVeSinh.TextChanged -= txt_TienVeSinh_TextChanged;

            if (int.TryParse(txt_TienVeSinh.Text.Replace(",", ""), out int number)) // Parse without separators
            {
                txt_TienVeSinh.Text = number.ToString("N0");
                txt_TienVeSinh.SelectionStart = txt_TienVeSinh.Text.Length; // Set cursor to the end
            }
            txt_TienVeSinh.TextChanged += txt_TienVeSinh_TextChanged;
        }

        private void txt_Tien_Leave(object sender, EventArgs e)
        {
            calcTongTien();
        }

        private void btn_UpdateHoaDon_Click(object sender, EventArgs e)
        {
            string validationResult = ValidateHoaDon();
            if (!string.IsNullOrEmpty(validationResult))
            {
                MessageBox.Show(validationResult, "Lỗi Nhập dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string sql = "Update HoaDon " +
                "SET Tiendien = @Tiendien, Tiennuoc = @Tiennuoc, Tienvesinh = @Tienvesinh," +
                "Ngaydong = @Ngaydong,TrangThai = @TrangThai " +
                "Where MaHoaDon = @MaHoaDon";
            var trangthai = (int)cb_TrangThai.SelectedValue;
            DateTime? NgayDong;
            if (trangthai != 0)
            {
                NgayDong = DateTime.Now;
            }
            else
            {
                NgayDong = null;
            }
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Tiendien",decimal.Parse(txt_TienDien.Text)),
                new SqlParameter("@Tiennuoc",decimal.Parse(txt_TienNuoc.Text)),
                new SqlParameter("@Tienvesinh",decimal.Parse(txt_TienVeSinh.Text)),
                new SqlParameter("@Ngaydong", NgayDong ?? (object)DBNull.Value),
                new SqlParameter("@TrangThai",trangthai),
                new SqlParameter("@MaHoaDon",maHoaDon),
            };

            var result = _dataAccess.ExecuteNonQuery(sql, parameters);
            if (result)
            {
                MessageBox.Show("Cập nhật hóa đơn Thành Công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
        }
        private string ValidateHoaDon()
        {
            StringBuilder errorMessage = new StringBuilder();
            if (string.IsNullOrEmpty(txt_TienDien.Text))
            {
                txt_TienDien.BorderColor = Color.Red;
                errorMessage.AppendLine("Tiền điện không được để trống.");
            }
            else
            {
                txt_TienDien.BorderColor = Color.LightGray;
            }
            if (string.IsNullOrEmpty(txt_TienNuoc.Text))
            {
                txt_TienNuoc.BorderColor = Color.Red;
                errorMessage.AppendLine("Tiền nước không được để trống.");
            }
            else
            {
                txt_TienNuoc.BorderColor = Color.LightGray; // Reset to default
            }

            if (string.IsNullOrEmpty(txt_TienVeSinh.Text))
            {
                txt_TienVeSinh.BorderColor = Color.Red;
                errorMessage.AppendLine("Tiền vệ sinh không được để trống.");
            }
            else
            {
                txt_TienVeSinh.BorderColor = Color.LightGray; // Reset to default
            }

            if (string.IsNullOrEmpty(txt_SoDien.Text))
            {
                txt_SoDien.BorderColor = Color.Red;
                errorMessage.AppendLine("Số điện không được để trống.");
            }
            else
            {
                txt_SoDien.BorderColor = Color.LightGray;
            }
            return errorMessage.ToString();
        }

        private void txt_box_Leave(object sender, EventArgs e)
        {
            Guna2TextBox textBox = (Guna2TextBox)sender;
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.BorderColor = Color.Red;
            }
            else
            {
                textBox.BorderColor = Color.LightGray;
            }
        }

        private void btn_CancelUpdateHoaDon_Click(object sender, EventArgs e)
        {
            var dialog = MessageBox.Show("Bạn có chắc chắn muốn hủy hóa đơn?", "Hủy Hóa Đơn", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialog == DialogResult.Yes)
            {
                string sql = "DELETE FROM HoaDon where MaHoaDon = @MaHoaDon";
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaHoaDon",maHoaDon)
                };
                var result = _dataAccess.ExecuteNonQuery(sql, parameters);
                if (result)
                {
                    MessageBox.Show("Xóa hóa đơn thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
            }
        }
    }
}
