using Guna.UI2.WinForms;
using QlKyTucXa.DAO;
using QlKyTucXa.Models;
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
    public partial class TaoHoaDonPhong : Form
    {
        private string maPhong;
        private readonly DataAccess _dataAccess;
        private HoaDon hoaDon;
        private int thang;
        private int nam;
        public TaoHoaDonPhong(string maPhong, int thang, int nam)
        {
            InitializeComponent();
            this.maPhong = maPhong;
            _dataAccess = DaoSingleton.GetInstance();
            hoaDon = new HoaDon();
            this.nam = nam;
            this.thang = thang;
        }

        private void HoaDonPhong_Load(object sender, EventArgs e)
        {
            LoadPhong();
            SetUpHoaDon();
            Text = $"Hóa đơn Phòng {maPhong}";
        }

        private void SetUpHoaDon()
        {
            txt_MaHoaDon.Text = HoaDonService.GenerateMaHoaDon(maPhong, thang, nam);
            txt_MaNhanVien.Text = UserInfor.MaNhanVien;
            txt_TenNhanVien.Text = UserInfor.TenNhanVien;
            txt_Thang.Text = thang.ToString();
            txt_Nam.Text = nam.ToString();

            cb_TrangThai.DataSource = new List<TrangThaiHoaDonItem>
            {
                new TrangThaiHoaDonItem {Key = "Chưa Thanh Toán",Value = 0 },
                new TrangThaiHoaDonItem {Key = "Đã Thanh Toán",Value = 1 },
            };
            cb_TrangThai.DisplayMember = "Key";
            cb_TrangThai.ValueMember = "Value";

            hoaDon.MaHoaDon = txt_MaHoaDon.Text;
            hoaDon.MaPhong = maPhong;
            hoaDon.Thang = thang;
            hoaDon.Nam = nam;
            hoaDon.NgayTao = DateTime.Now;
            hoaDon.Ngaydong = null;
        }

        private void LoadPhong()
        {
            string sql = "Select MaPhong,Tenphong,Songuoidao,Songuoitoida,TienPhong from Phong Where MaPhong = @MaPhong";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@MaPhong",maPhong)
            };
            _dataAccess.ExecuteReader(sql, parameters, reader =>
            {
                var tongTien = Convert.ToDecimal(reader["TienPhong"]);
                txt_MaPhong.Text = reader["MaPhong"].ToString();
                txt_TenPhong.Text = reader["Tenphong"].ToString();
                txt_TienPhong.Text = tongTien.ToString("N0");
                txt_SoNguoiDaO.Text = Convert.ToInt32(reader["Songuoidao"]).ToString();
                txt_SoNguoiToiDa.Text = Convert.ToInt32(reader["Songuoitoida"]).ToString();
                lbl_TongTien.Text = CurrencyFormatter.FormatCurrency(tongTien);
            });
        }

        private void txt_SoDien_TextChange(object sender, EventArgs e)
        {
            var tiendien = TinhTienDienService.Calc(txt_SoDien.Text);
            txt_TienDien.Text = tiendien.ToString("N0");
        }

        private void txt_Tien_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txt_TienNuoc_TextChanged(object sender, EventArgs e)
        {
            txt_TienNuoc.TextChanged -= txt_TienNuoc_TextChanged;

            if (int.TryParse(txt_TienNuoc.Text.Replace(",", ""), out int number)) // Parse without separators
            {
                txt_TienNuoc.Text = number.ToString("N0");
                txt_TienNuoc.SelectionStart = txt_TienNuoc.Text.Length; // Set cursor to the end
            }

            // Reattach event
            txt_TienNuoc.TextChanged += txt_TienNuoc_TextChanged;
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
            Guna2TextBox textBox = (Guna2TextBox)sender;
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.BorderColor = Color.Red;
            }
            else
            {
                textBox.BorderColor = Color.LightGray;
            }
            calcTongTien();
        }

        private void btn_addHoaDon_Click(object sender, EventArgs e)
        {
            string validationResult = ValidateHoaDon();
            if (!string.IsNullOrEmpty(validationResult))
            {
                MessageBox.Show(validationResult, "Lỗi Nhập dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            setHoaDon();
            var sql = "INSERT INTO HoaDon(MaHoaDon,Thang,Nam,MaPhong,Tiendien,Tiennuoc," +
                "Tienvesinh,NgayTao,Ngaydong,TrangThai) " +
                "VALUES(@MaHoaDon,@Thang,@Nam,@MaPhong,@Tiendien,@Tiennuoc,@Tienvesinh,@NgayTao,@Ngaydong,@TrangThai)";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@MaHoaDon",hoaDon.MaHoaDon),
                new SqlParameter("@Thang",hoaDon.Thang),
                new SqlParameter("@Nam",hoaDon.Nam),
                new SqlParameter("@MaPhong",hoaDon.MaPhong),
                new SqlParameter("@Tiendien",hoaDon.Tiendien),
                new SqlParameter("@Tiennuoc",hoaDon.Tiennuoc),
                new SqlParameter("@Tienvesinh",hoaDon.Tienvesinh),
                new SqlParameter("@NgayTao",hoaDon.NgayTao),
                new SqlParameter("@Ngaydong",hoaDon.Ngaydong ?? (object)DBNull.Value),
                new SqlParameter("@TrangThai",hoaDon.TrangThai)
            };
            var result = _dataAccess.ExecuteNonQuery(sql, parameters);
            if (result)
            {
                MessageBox.Show("Tạo Hóa đơn thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void setHoaDon()
        {
            hoaDon.MaHoaDon = txt_MaHoaDon.Text;
            hoaDon.Thang = thang;
            hoaDon.Nam = nam;
            hoaDon.MaPhong = maPhong;
            hoaDon.Tiennuoc = Decimal.Parse(txt_TienNuoc.Text);
            hoaDon.Tiendien = Decimal.Parse(txt_TienDien.Text);
            hoaDon.Tienvesinh = Decimal.Parse(txt_TienVeSinh.Text);
            hoaDon.NgayTao = DateTime.Now;
            hoaDon.TrangThai = (int)cb_TrangThai.SelectedValue;
            if (hoaDon.TrangThai != 0)
            {
                hoaDon.Ngaydong = DateTime.Now;
            }
            else
            {
                hoaDon.Ngaydong = null;
            }
        }

        private void cb_TrangThai_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_TrangThai.SelectedIndex != -1)
            {
                var selectedItem = cb_TrangThai.SelectedItem;
                var selectedValue = cb_TrangThai.SelectedValue;
            }
        }
    }
}
