using QlKyTucXa.DAO;
using QlKyTucXa.Models;
using QlKyTucXa.Singleton;
using QlKyTucXa.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
namespace QlKyTucXa
{
    public partial class ChiTietHoaDon : Form
    {
        private string maHoaDon;
        private readonly DataAccess _dataAccess;
        public ChiTietHoaDon(string maHoaDon)
        {
            InitializeComponent();
            this.maHoaDon = maHoaDon;
            _dataAccess = DaoSingleton.GetInstance();
        }

        private void ChiTietHoaDon_Load(object sender, System.EventArgs e)
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
            txt_MaNhanVien.Text = UserInfor.MaNhanVien;
            txt_TenNhanVien.Text = UserInfor.TenNhanVien;
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

        private void btn_ExportHoaDon_Click(object sender, System.EventArgs e)
        {
            Excel.Application exApp = new Excel.Application();
            Excel.Workbook exBook = exApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
            Excel.Worksheet exSheet = (Excel.Worksheet)exBook.Worksheets[1];

            exSheet.Range["A1"].Font.Size = 15;
            exSheet.Range["A1"].Font.Bold = true;
            exSheet.Range["A1"].Font.Color = Color.Green;
            exSheet.Range["A1"].Value = "KÝ TÚC XÁ ĐH GIAO THÔNG VẬN TẢI";

            Excel.Range dc = (Excel.Range)exSheet.Cells[2, 1];
            dc.Font.Size = 13;
            dc.Font.Color = Color.Blue;
            dc.Value = "Số 99 - Nguyễn Chí Thanh - P.Láng Hạ - Q.Đống Đa - TP. Hà Nội";

            exSheet.Range["C4"].Font.Size = 20;
            exSheet.Range["C4"].Font.Bold = true;
            exSheet.Range["C4"].Font.Color = Color.Red;
            exSheet.Range["C4"].Value = $"CHI TIẾT HÓA ĐƠN PHÒNG {txt_TenPhong.Text.ToUpper()}";

            exSheet.Range["A6:A12"].Font.Size = 12;
            exSheet.Range["A6:A12"].Font.Bold = true;
            exSheet.Range["A6:A12"].RowHeight = 20;
            exSheet.Range["A6:A12"].ColumnWidth = 13;

            exSheet.Range["A6"].Value = "Mã Hóa Đơn";


            exSheet.Range["A7"].Value = "Ngày Lập";

            exSheet.Range["A8"].Value = "Tên Phòng";

            exSheet.Range["A9"].Value = "Tiền Phòng";
            exSheet.Range["A10"].Value = "Tiền Điện";
            exSheet.Range["A11"].Value = "Tiền Nước";
            exSheet.Range["A12"].Value = "Tiền Vệ Sinh";


            exSheet.Range["B6:B12"].Font.Size = 12;
            exSheet.Range["B6:B12"].ColumnWidth = 15;

            exSheet.Range["B6"].Value = txt_MaHoaDon.Text;
            exSheet.Range["B6"].HorizontalAlignment = HorizontalAlignment.Right;

            exSheet.Range["B7"].Value = DateTime.Parse(dtp_NgayLap.Text).ToString("dd/MM/yyyy");

            exSheet.Range["B8"].Value = txt_TenPhong.Text;
            exSheet.Range["B8"].HorizontalAlignment = HorizontalAlignment.Right;

            exSheet.Range["B9"].Value = decimal.Parse(txt_TienPhong.Text).ToString("N0");
            exSheet.Range["B10"].Value = decimal.Parse(txt_TienDien.Text).ToString("N0");
            exSheet.Range["B11"].Value = decimal.Parse(txt_TienNuoc.Text).ToString("N0");
            exSheet.Range["B12"].Value = decimal.Parse(txt_TienVeSinh.Text).ToString("N0");

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
            exSheet.Range["B13"].Value = "Tổng Tiền: " + tongTien.ToString("N0") + " VNĐ";

            exBook.Activate();

            // Lưu file 
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Exel 97-2002 Workbook|*.xls|Excel Workbook|*.xlsx|All Files|*.*";
            save.FilterIndex = 2;
            if (save.ShowDialog() == DialogResult.OK)
            {
                exBook.SaveAs(save.FileName.ToLower());
            }
            exApp.Quit();
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
    }
}
