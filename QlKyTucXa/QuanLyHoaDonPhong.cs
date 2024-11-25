using Guna.UI2.WinForms;
using QlKyTucXa.DAO;
using QlKyTucXa.Singleton;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
namespace QlKyTucXa
{
    public partial class QuanLyHoaDonPhong : Form
    {
        private readonly DataAccess _dataAccess;
        private string MaPhong;
        public QuanLyHoaDonPhong()
        {
            InitializeComponent();
            _dataAccess = DaoSingleton.GetInstance();
        }

        private void QuanLyHoaDonPhong_Load(object sender, System.EventArgs e)
        {
            LoadNam();
            LoadThang();
            ReloadButton();

            CustomizeDataGridView(dgv_ChuaLapHoaDon);
            CustomizeDataGridView(dgv_ChuaDongTien);
            CustomizeDataGridView(dgv_DaThanhToan);

            CustomizeChuaLapHoaDon(dgv_ChuaLapHoaDon);
            CustomizeChuaDongTien(dgv_ChuaDongTien);
            CustomizeDaThanhToan(dgv_DaThanhToan);

            // Disable row resizing
            DisableDataGridView(dgv_ChuaDongTien);
            DisableDataGridView(dgv_ChuaLapHoaDon);
            DisableDataGridView(dgv_DaThanhToan);
        }
        private void CustomizeChuaLapHoaDon(DataGridView dgv)
        {
            int totalWidth = dgv.Width - dgv.RowHeadersWidth;

            dgv.Columns["MaPhong"].HeaderText = "Mã Phòng";
            dgv.Columns["MaPhong"].Width = (int)(totalWidth * 0.2);
            dgv.Columns["MaPhong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.Columns["TenPhong"].HeaderText = "Tên Phòng";
            dgv.Columns["TenPhong"].Width = (int)(totalWidth * 0.2);
            dgv.Columns["TenPhong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.Columns["Loaiphong"].HeaderText = "Loại Phòng";
            dgv.Columns["Loaiphong"].Width = (int)(totalWidth * 0.3);
            dgv.Columns["Loaiphong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.Columns["Songuoidao"].HeaderText = "Số Người ở";
            dgv.Columns["Songuoidao"].Width = (int)(totalWidth * 0.3);
            dgv.Columns["Songuoidao"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }
        private void CustomizeChuaDongTien(DataGridView dgv)
        {
            int totalWidth = dgv.Width - dgv.RowHeadersWidth;

            dgv.Columns["MaHoaDon"].HeaderText = "Mã Hóa Đơn";
            dgv.Columns["MaHoaDon"].Width = (int)(totalWidth * 0.2);
            dgv.Columns["MaHoaDon"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.Columns["MaPhong"].HeaderText = "Mã Phòng";
            dgv.Columns["MaPhong"].Width = (int)(totalWidth * 0.15);
            dgv.Columns["MaPhong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.Columns["TenPhong"].HeaderText = "Tên Phòng";
            dgv.Columns["TenPhong"].Width = (int)(totalWidth * 0.15);
            dgv.Columns["TenPhong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.Columns["NgayTao"].HeaderText = "Ngày Tạo";
            dgv.Columns["NgayTao"].Width = (int)(totalWidth * 0.15);
            dgv.Columns["NgayTao"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.Columns["TongTien"].HeaderText = "Tổng Tiền";
            dgv.Columns["TongTien"].Width = (int)(totalWidth * 0.35);
            dgv.Columns["TongTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }
        private void CustomizeDaThanhToan(DataGridView dgv)
        {
            int totalWidth = dgv.Width - dgv.RowHeadersWidth;

            dgv.Columns["MaHoaDon"].HeaderText = "Mã Hóa Đơn";
            dgv.Columns["MaHoaDon"].Width = (int)(totalWidth * 0.2);
            dgv.Columns["MaHoaDon"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.Columns["MaPhong"].HeaderText = "Mã Phòng";
            dgv.Columns["MaPhong"].Width = (int)(totalWidth * 0.15);
            dgv.Columns["MaPhong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.Columns["TenPhong"].HeaderText = "Tên Phòng";
            dgv.Columns["TenPhong"].Width = (int)(totalWidth * 0.15);
            dgv.Columns["TenPhong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.Columns["Ngaydong"].HeaderText = "Ngày Đóng";
            dgv.Columns["Ngaydong"].Width = (int)(totalWidth * 0.15);
            dgv.Columns["Ngaydong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.Columns["TongTien"].HeaderText = "Tổng Tiền";
            dgv.Columns["TongTien"].Width = (int)(totalWidth * 0.35);
            dgv.Columns["TongTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }
        private void LoadNam()
        {
            int namNhoNhat = GetMinYearFromDatabase();
            int namHienTai = DateTime.Now.Year;

            cbNam.Items.Clear();
            for (int i = namNhoNhat; i <= namHienTai; i++)
            {
                cbNam.Items.Add(i);
            }

            // Chọn năm hiện tại
            cbNam.SelectedItem = namHienTai;
        }
        private void LoadThang()
        {
            cbThang.Items.Clear();
            UpdateThangState();
        }
        private void UpdateThangState()
        {
            int namHienTai = DateTime.Now.Year;
            int thangHienTai = DateTime.Now.Month;
            int namDuocChon = int.Parse(cbNam.SelectedItem.ToString());
            cbThang.Items.Clear();
            for (int i = 1; i <= 12; i++)
            {
                if (namDuocChon < namHienTai || i <= thangHienTai)
                {
                    cbThang.Items.Add(i);
                }
            }
            cbThang.SelectedItem = thangHienTai;
        }
        private int GetMinYearFromDatabase()
        {
            string query = "SELECT MIN(Nam) AS MinYear FROM HoaDon";

            var result = _dataAccess.ExecuteScalar(query);

            return result != DBNull.Value ? Convert.ToInt32(result) : DateTime.Now.Year;
        }

        private void cbThang_SelectedIndexChanged(object sender, EventArgs e)
        {
            int thang = int.Parse(cbThang.SelectedItem.ToString());
            int nam = int.Parse(cbNam.SelectedItem.ToString());

            dgv_ChuaLapHoaDon.DataSource = GetPhongChuaLapHoaDon(thang, nam);


            dgv_ChuaDongTien.DataSource = GetPhongChuaDongTien(thang, nam);
            if (dgv_ChuaDongTien.Columns["TongTien"] != null)
            {
                dgv_ChuaDongTien.Columns["TongTien"].DefaultCellStyle.Format = "N2";
            }

            dgv_DaThanhToan.DataSource = GetPhongDaThanhToan(thang, nam);
            if (dgv_DaThanhToan.Columns["TongTien"] != null)
            {
                dgv_DaThanhToan.Columns["TongTien"].DefaultCellStyle.Format = "N2";
            }


        }

        private object GetPhongDaThanhToan(int thang, int nam, string searchText = "")
        {
            string sql = "SELECT H.MaHoaDon, P.MaPhong, P.Tenphong, H.Ngaydong, " +
                "(COALESCE(H.Tiendien, 0) + COALESCE(H.Tiennuoc, 0) + COALESCE(H.Tienvesinh, 0) + COALESCE(H.Tienphat, 0)) AS [TongTien] " +
                "FROM HoaDon H " +
                "JOIN Phong P ON H.MaPhong = P.MaPhong " +
                "WHERE H.Thang = @Thang AND H.Nam = @Nam AND H.TrangThai = 1 " +
                (string.IsNullOrEmpty(searchText) ? "" : "AND (P.MaPhong LIKE @SearchText OR H.MaHoaDon LIKE @SearchText)");
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Thang",thang),
                new SqlParameter("@Nam",nam)
            };

            if (!string.IsNullOrEmpty(searchText))
            {
                parameters.Add(new SqlParameter("@SearchText", $"%{searchText}%"));
            }

            return _dataAccess.GetDataTable(sql, parameters.ToArray());
        }

        private object GetPhongChuaDongTien(int thang, int nam, string searchText = "")
        {
            string sql = "SELECT H.MaHoaDon, P.MaPhong, P.Tenphong, H.NgayTao," +
                "(COALESCE(H.Tiendien, 0) + COALESCE(H.Tiennuoc, 0) + COALESCE(H.Tienvesinh, 0) + COALESCE(H.Tienphat, 0)) AS [TongTien] " +
                "FROM HoaDon H " +
                "JOIN Phong P ON H.MaPhong = P.MaPhong " +
                "WHERE H.Thang = @Thang AND H.Nam = @Nam AND H.TrangThai = 0 " +
                (string.IsNullOrEmpty(searchText) ? "" : "AND (P.MaPhong LIKE @SearchText OR H.MaHoaDon LIKE @SearchText");
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Thang",thang),
                new SqlParameter("@Nam",nam)
            };

            if (!string.IsNullOrEmpty(searchText))
            {
                parameters.Add(new SqlParameter("@SearchText", $"%{searchText}%"));
            }
            return _dataAccess.GetDataTable(sql, parameters.ToArray());
        }

        private object GetPhongChuaLapHoaDon(int thang, int nam, string searchText = "")
        {
            string sql = "SELECT P.MaPhong, P.Tenphong, P.Loaiphong, P.Tennha, P.Songuoidao,P.Songuoitoida " +
                "FROM Phong P " +
                "LEFT JOIN HoaDon H ON P.MaPhong = H.MaPhong AND H.Thang = @Thang AND H.Nam = @Nam " +
                "WHERE H.MaHoaDon IS NULL " + (string.IsNullOrEmpty(searchText) ? "" : "AND P.MaPhong Like @SearchText");
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Thang",thang),
                new SqlParameter("@Nam",nam)
            };

            if (!string.IsNullOrEmpty(searchText))
            {
                parameters.Add(new SqlParameter("@SearchText", $"%{searchText}%"));
            }
            return _dataAccess.GetDataTable(sql, parameters.ToArray());
        }

        private void CustomizeDataGridView(Guna.UI2.WinForms.Guna2DataGridView dgv)
        {
            // Set header style
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 12, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(100, 88, 255); // Custom color
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Set header height
            dgv.ColumnHeadersHeight = 40;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // Set border style for the header
            dgv.EnableHeadersVisualStyles = false; // Required to apply custom styles
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            // Set row style (optional)
            dgv.RowTemplate.Height = 30;
            dgv.DefaultCellStyle.Font = new Font("Arial", 10);
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Color.Black;
            dgv.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
        }

        private void DisableDataGridView(Guna2DataGridView dgv)
        {
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToResizeRows = false;
            dgv.ReadOnly = true;
            dgv.MultiSelect = false;
        }

        private void dgv_ChuaLapHoaDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                MaPhong = dgv_ChuaLapHoaDon.Rows[e.RowIndex].Cells["MaPhong"].Value.ToString();
            }
            else
            {
                MaPhong = string.Empty;
            }
            ReloadButton();
        }

        private void ReloadButton()
        {
            if (!string.IsNullOrEmpty(MaPhong))
            {
                btn_LapHoaDon.Enabled = true;
            }
            else
            {
                btn_LapHoaDon.Enabled = false;
            }
        }

        private void btn_LapHoaDon_Click(object sender, EventArgs e)
        {
            int thang = int.Parse(cbThang.SelectedItem.ToString());
            int nam = int.Parse(cbNam.SelectedItem.ToString());
            var modal = new TaoHoaDonPhong(MaPhong, thang, nam);
            modal.FormClosed += (s, eve) =>
            {
                ReloadData(thang, nam);
            };

            modal.Show();
        }

        private void ReloadData(int thang, int nam)
        {

            dgv_ChuaLapHoaDon.DataSource = GetPhongChuaLapHoaDon(thang, nam);

            dgv_ChuaDongTien.DataSource = GetPhongChuaDongTien(thang, nam);

            dgv_DaThanhToan.DataSource = GetPhongDaThanhToan(thang, nam);
        }

        private void txt_TimKiemPhongChuaLapHoaDon_TextChanged(object sender, EventArgs e)
        {
            int thang = int.Parse(cbThang.SelectedItem.ToString());
            int nam = int.Parse(cbNam.SelectedItem.ToString());
            string searchText = txt_TimKiemPhongChuaLapHoaDon.Text.Trim();
            dgv_ChuaLapHoaDon.DataSource = GetPhongChuaLapHoaDon(thang, nam, searchText);
        }

        private void txt_TimKiemHoaDonChuaThanhToan_TextChanged(object sender, EventArgs e)
        {
            int thang = int.Parse(cbThang.SelectedItem.ToString());
            int nam = int.Parse(cbNam.SelectedItem.ToString());
            string searchText = txt_TimKiemHoaDonChuaThanhToan.Text.Trim();
            dgv_ChuaDongTien.DataSource = GetPhongChuaDongTien(thang, nam, searchText);
        }

        private void txt_TimKiemHoaDonDaThanhToan_TextChanged(object sender, EventArgs e)
        {
            int thang = int.Parse(cbThang.SelectedItem.ToString());
            int nam = int.Parse(cbNam.SelectedItem.ToString());
            string searchText = txt_TimKiemHoaDonDaThanhToan.Text.Trim();
            dgv_DaThanhToan.DataSource = GetPhongDaThanhToan(thang, nam, searchText);
        }

        private void btn_XuatDanhSachChuaLap_Click(object sender, EventArgs e)
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

            exSheet.Range["D4"].Font.Size = 20;
            exSheet.Range["D4"].Font.Bold = true;
            exSheet.Range["D4"].Font.Color = Color.Red;
            exSheet.Range["D4"].Value = "DANH SÁCH PHÒNG CHƯA LẬP HÓA ĐƠN THÁNG " + int.Parse(cbThang.SelectedItem.ToString());

            exSheet.Range["A6:G6"].Font.Size = 12;
            exSheet.Range["A6:G6"].Font.Bold = true;

            exSheet.Range["A6"].Value = "STT";

            exSheet.Range["B6"].Value = "Mã Phòng";
            exSheet.Range["B6"].ColumnWidth = 12;

            exSheet.Range["C6"].Value = "Tên Phòng";
            exSheet.Range["C6"].ColumnWidth = 12;

            exSheet.Range["D6"].Value = "Loại Phòng";
            exSheet.Range["D6"].ColumnWidth = 14;

            exSheet.Range["E6"].Value = "Khu nhà";

            exSheet.Range["F6"].Value = "Số Người Ở";
            exSheet.Range["F6"].ColumnWidth = 13;

            exSheet.Range["G6"].Value = "Số Người tối đa";
            exSheet.Range["G6"].ColumnWidth = 15;
            int row = 7;
            if (dgv_ChuaLapHoaDon.Rows.Count > 0)
            {
                for (int i = 0; i < dgv_ChuaLapHoaDon.Rows.Count; i++)
                {
                    exSheet.Range["A" + (row + i).ToString()].Value = (i + 1).ToString();
                    exSheet.Range["B" + (row + i).ToString()].Value = dgv_ChuaLapHoaDon.Rows[i].Cells[0].Value.ToString();
                    exSheet.Range["C" + (row + i).ToString()].Value = dgv_ChuaLapHoaDon.Rows[i].Cells[1].Value.ToString();
                    exSheet.Range["D" + (row + i).ToString()].Value = dgv_ChuaLapHoaDon.Rows[i].Cells[2].Value.ToString();
                    exSheet.Range["E" + (row + i).ToString()].Value = dgv_ChuaLapHoaDon.Rows[i].Cells[3].Value.ToString();
                    exSheet.Range["F" + (row + i).ToString()].Value = dgv_ChuaLapHoaDon.Rows[i].Cells[4].Value.ToString();
                    exSheet.Range["G" + (row + i).ToString()].Value = dgv_ChuaLapHoaDon.Rows[i].Cells[5].Value.ToString();
                }
                row += dgv_ChuaLapHoaDon.Rows.Count;
                exSheet.Range["F" + row.ToString()].Value = "Tổng Số Lượng: " + dgv_ChuaLapHoaDon.Rows.Count.ToString();
            }
            else
            {
                exSheet.Range["D7"].Value = "Không có phòng nào chưa lập hóa đơn trong tháng " + int.Parse(cbThang.SelectedItem.ToString());
            }
            exSheet.Name = "Danh Sach Thang " + int.Parse(cbThang.SelectedItem.ToString());
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

        private void btn_CapNhatHoaDon_Click(object sender, EventArgs e)
        {

        }
    }
}
