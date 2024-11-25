using QlKyTucXa.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QlKyTucXa
{
	public partial class TraPhong : Form
	{
        DataProcesser db = new DataProcesser();
        string sql = null;

        public TraPhong()
		{
			InitializeComponent();
		}

        private void TraPhong_Load(object sender, EventArgs e)
        {
            this.Location = new Point(465, 155);
            sql = "select svp.MaSoThue as N'Mã số thuê', Tensinhvien as N'Tên sinh viên',"
                + " svp.Masv as N'Mã sinh viên', Maphong as N'Mã phòng',"
                + " NgayBdau as N'Ngày bắt đầu', Ngaykt as N'Ngày kết thúc',"
                + " ngaytra as N'Ngày trả', tienvipham as N'Tiền vi phạm'"
                + " from SV_Phong svp"
                + " join Traphong tp on svp.MaSoThue = tp.MaSoThue"
                + " join Sinhvien sv on svp.Masv = sv.Masinhvien"
                + " where DATEDIFF(DAY, ngaytra, GETDATE()) <= 30";
            DataTable table = db.ReadData(sql);
            dgvTraPhong.DataSource = table;

            cbTimKiemTheo.Items.Add("Mã số thuê");
            cbTimKiemTheo.Items.Add("Mã sinh viên");
            cbTimKiemTheo.SelectedIndex = 0;
        }

        DataTable GetInforByMaSoThue()
        {
            sql = "select * from SV_Phong svp"
                + " join Sinhvien sv on svp.Masv = sv.Masinhvien"
                + " where MaSoThue = N'" + txtMaSoThue.Text + "'";

            DataTable table = db.ReadData(sql);
            return table;
        }

        DataTable GetInforByMaSinhVien()
        {
            sql = "select * from SV_Phong svp"
                + " join Sinhvien sv on svp.Masv = sv.Masinhvien"
                + " where svp.Masv = N'" + txtMaSinhVien.Text + "'";

            DataTable table = db.ReadData(sql);
            return table;
        }

        private void cbTimKiemTheo_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedItem = cbTimKiemTheo.SelectedIndex;
            if (selectedItem == 0)
            {
                txtMaSoThue.Enabled = true;
                txtMaSoThue.Text = "";
                txtMaSinhVien.Enabled = false;
            }
            if (selectedItem == 1)
            {
                txtMaSoThue.Enabled = false;
                txtMaSinhVien.Text = "";
                txtMaSinhVien.Enabled = true;
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if (cbTimKiemTheo.SelectedIndex == 0)
            {
                if (string.IsNullOrEmpty(txtMaSoThue.Text))
                {
                    MessageBox.Show($"Bạn chưa nhập mã số thuê!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (GetInforByMaSoThue().Rows.Count == 0)
                {
                    MessageBox.Show($"Mã số thuê không tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (int.Parse(GetInforByMaSoThue().Rows[0]["TrangThai"].ToString()) != 1)
                {
                    MessageBox.Show($"Sinh viên đã trả phòng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (DateTime.Now.Date > ((DateTime)GetInforByMaSoThue().Rows[0]["Ngaykt"]).Date)
                {
                    MessageBox.Show($"Hợp đồng đã hết hạn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                txtMaSinhVien.Text = GetInforByMaSoThue().Rows[0]["Masv"].ToString();
            }

            if (cbTimKiemTheo.SelectedIndex == 1)
            {
                if (string.IsNullOrEmpty(txtMaSinhVien.Text))
                {
                    MessageBox.Show($"Bạn chưa nhập mã sinh viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (GetInforByMaSinhVien().Rows.Count == 0)
                {
                    DataTable tb = db.ReadData("select * from Sinhvien");
                    foreach (DataRow r in tb.Rows)
                    {
                        if (r["Masinhvien"].ToString() == txtMaSinhVien.Text)
                        {
                            MessageBox.Show($"Sinh viên chưa có hợp đồng thuê!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    MessageBox.Show($"Mã sinh viên không đúng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                foreach (DataRow r in GetInforByMaSinhVien().Rows)
                {
                    if (int.Parse(r["TrangThai"].ToString()) == 0)
                    {
                        MessageBox.Show($"Sinh viên đã trả phòng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if (DateTime.Now.Date < ((DateTime)r["Ngaykt"]).Date)
                    {
                        txtMaSoThue.Text = GetInforByMaSinhVien().Rows[0]["MaSoThue"].ToString();
                        break;
                    }
                    MessageBox.Show($"Tất cả hợp đồng đã hết hạn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            txtTenSinhVien.Text = GetInforByMaSoThue().Rows[0]["Tensinhvien"].ToString();
            txtMaPhong.Text = GetInforByMaSoThue().Rows[0]["Maphong"].ToString();
            dtNgayBatDau.Value = (DateTime)GetInforByMaSoThue().Rows[0]["NgayBdau"];
            dtNgayKetThuc.Value = (DateTime)GetInforByMaSoThue().Rows[0]["Ngaykt"];
            btnXoa.Enabled = true;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Xác nhận cập nhật sinh viên trả phòng?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            sql = "UPDATE SV_Phong SET TrangThai = 0 WHERE MaSoThue = N'" + txtMaSoThue.Text + "'";
            db.ChangeData(sql);

            sql = "UPDATE Phong SET Songuoidao = Songuoidao - 1 WHERE MaPhong = N'" + txtMaPhong.Text + "'";
            db.ChangeData(sql);

            sql = "INSERT INTO Traphong (MaSoThue, ngaytra, tienvipham)"
                + " VALUES('" + txtMaSoThue.Text + "', GETDATE(), CASE"
                + " WHEN '" + dtNgayKetThuc.Value.Year + "-"
                + dtNgayKetThuc.Value.Month + "-" + dtNgayKetThuc.Value.Day
                + "' <= GETDATE() THEN 0 ELSE 500000 END)";
            db.ChangeData(sql);

            sql = "select svp.MaSoThue as N'Mã số thuê', Tensinhvien as N'Tên sinh viên',"
                + " svp.Masv as N'Mã sinh viên', Maphong as N'Mã phòng',"
                + " NgayBdau as N'Ngày bắt đầu', Ngaykt as N'Ngày kết thúc',"
                + " ngaytra as N'Ngày trả', tienvipham as N'Tiền vi phạm'"
                + " from SV_Phong svp"
                + " join Traphong tp on svp.MaSoThue = tp.MaSoThue"
                + " join Sinhvien sv on svp.Masv = sv.Masinhvien"
                + " where DATEDIFF(DAY, ngaytra, GETDATE()) <= 30";
            DataTable table = db.ReadData(sql);
            dgvTraPhong.DataSource = table;

            txtMaSoThue.Text = "";
            txtMaSinhVien.Text = "";
            txtTenSinhVien.Text = "";
            txtMaPhong.Text = "";
            dtNgayBatDau.Value = DateTime.Now;
            dtNgayKetThuc.Value = DateTime.Now;
            btnXoa.Enabled = false;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
