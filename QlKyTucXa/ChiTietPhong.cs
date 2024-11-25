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
    public partial class ChiTietPhong : Form
    {
        DataProcesser db = new DataProcesser();
        string sql = null;

        public ChiTietPhong()
        {
            InitializeComponent();
        }

        private void ChiTietPhong_Load(object sender, EventArgs e)
        {
            this.Location = new Point(465, 155);
            cbTimKiemTheo.Items.Add("Mã phòng");
            cbTimKiemTheo.Items.Add("Tên tòa + Tên phòng");
            cbTimKiemTheo.SelectedIndex = 0;
        }

        DataTable GetPhongByMaPhong(string maPhong)
        {
            sql = "select * from Phong"
                + " where MaPhong LIKE '%" + maPhong + "%'";
            DataTable table = db.ReadData(sql);
            return table;
        }

        DataTable GetPhongByTenNhaVaTenPhong(string tenNha, string tenPhong)
        {
            sql = "select * from Phong"
                + " where Tennha = N'" + tenNha + "'"
                + " and Tenphong = N'" + tenPhong + "'";
            DataTable table = db.ReadData(sql);
            return table;
        }

        DataTable GetSinhVienByMaPhong(string maPhong)
        {
            sql = "select sv.Masinhvien as 'Mã sinh viên', Tensinhvien as 'Tên sinh viên', Ngaysinh as 'Ngày sinh',"
                + " gioitinh as 'Giới tính', Tenque as 'Tên quê', Tenkhoa as 'Tên khoa', Tenlop as 'Tên lớp'"
                + " from Phong p"
                + " join SV_Phong svp on p.MaPhong = svp.Maphong"
                + " join Sinhvien sv on svp.Masv = sv.Masinhvien"
                + " join Que q on sv.Maque = q.MaQue"
                + " join Khoa k on sv.Makhoa = k.Makhoa"
                + " join Lop l on sv.Malop = l.Malop"
                + " where p.MaPhong = N'" + maPhong + "'"
                + " and TrangThai = 1"
                + " and NgayBdau < GETDATE()"
                + " and Ngaykt >= GETDATE()";
            DataTable table = db.ReadData(sql);
            return table;
        }

        DataTable GetThietBiByMaPhong(string maPhong)
        {
            sql = "select tb.mathietbi as 'Mã thiết bị', tenthietbi as 'Tên thiết bị',"
                + " soluong as 'Số lượng', Tinhtrang as 'Tình trạng'"
                + " from Phong p"
                + " join Thietbi_phong tbp on p.MaPhong = tbp.Maphong"
                + " join Thietbi tb on tbp.Mathietbi = tb.mathietbi"
                + " where p.MaPhong = N'" + maPhong + "'";
            DataTable table = db.ReadData(sql);
            return table;
        }

        private void cbMaPhong_TextChanged(object sender, EventArgs e)
        {
            cbMaPhong.DroppedDown = false;
        }

        private void cbMaPhong_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbMaPhong.SelectedItem.ToString() == "")
            {
                cbTenToaNha.Items.Clear();
                cbTenToaNha.Items.Add("");
                cbTenToaNha.SelectedIndex = 0;
                cbTenPhong.Items.Clear();
                cbTenPhong.Items.Add("");
                cbTenPhong.SelectedIndex = 0;
                return;
            }

            cbTenToaNha.Items.Clear();
            cbTenToaNha.Items.Add(GetPhongByMaPhong(cbMaPhong.Text).Rows[0]["Tennha"].ToString());
            cbTenToaNha.SelectedIndex = 0;
            cbTenPhong.Items.Clear();
            cbTenPhong.Items.Add(GetPhongByMaPhong(cbMaPhong.Text).Rows[0]["Tenphong"].ToString());
            cbTenPhong.SelectedIndex = 0;
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cbMaPhong.Text) && cbTimKiemTheo.SelectedIndex == 0)
            {
                MessageBox.Show("Vui lòng chọn mã phòng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrEmpty(cbTenToaNha.Text) && cbTimKiemTheo.SelectedIndex == 1)
            {
                MessageBox.Show("Vui lòng chọn tên tòa nhà", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrEmpty(cbTenPhong.Text) && cbTimKiemTheo.SelectedIndex == 1)
            {
                MessageBox.Show("Vui lòng chọn tên phòng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            sql = "select * from Phong"
                + " where MaPhong = N'" + cbMaPhong.Text + "'";
            DataTable table = db.ReadData(sql);
            if (table.Rows.Count == 0)
            {
                MessageBox.Show("Mã phòng không đúng!\nMở danh sách thả xuống để xem các phòng có mã tương tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            cbTenToaNha.Items.Clear();
            cbTenToaNha.Items.Add(table.Rows[0]["Tennha"].ToString());
            cbTenToaNha.SelectedIndex = 0;
            cbTenPhong.Items.Clear();
            cbTenPhong.Items.Add(table.Rows[0]["Tenphong"].ToString());
            cbTenPhong.SelectedIndex = 0;
            dgvSinhVien.DataSource = GetSinhVienByMaPhong(cbMaPhong.Text);
            dgvThietBi.DataSource = GetThietBiByMaPhong(cbMaPhong.Text);
        }

        private void cbTimKiemTheo_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedItem = cbTimKiemTheo.SelectedIndex;
            if (selectedItem == 0)
            {
                cbMaPhong.Enabled = true;
                cbMaPhong.Text = "";
                cbTenToaNha.Enabled = false;
                cbTenPhong.Enabled = false;
            }
            if (selectedItem == 1)
            {
                cbMaPhong.Enabled = false;
                cbTenToaNha.Enabled = true;

                cbTenToaNha.Items.Clear();
                DataTable table = db.ReadData("select distinct Tennha from Phong");
                foreach (DataRow item in table.Rows)
                {
                    cbTenToaNha.Items.Add(item["Tennha"].ToString());
                }
                cbTenPhong.Items.Clear();
                cbTenPhong.Items.Add("");
                cbTenPhong.SelectedIndex = 0;
            }
        }

        private void cbTenToaNha_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbTimKiemTheo.SelectedIndex == 0)
            {
                return;
            }

            cbTenPhong.Enabled = true;

            cbTenPhong.Items.Clear();
            sql = "select Tenphong from Phong"
                + " where Tennha = N'" + cbTenToaNha.SelectedItem.ToString() + "'";
            DataTable table = db.ReadData(sql);

            if (table.Rows.Count == 0)
            {
                cbTenPhong.Items.Clear();
                cbTenPhong.Items.Add("");
                cbTenPhong.SelectedIndex = 0;
                return;
            }

            foreach (DataRow item in table.Rows)
            {
                cbTenPhong.Items.Add(item["Tenphong"].ToString());
            }
        }

        private void cbTenPhong_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbTenPhong.SelectedItem == null || cbTenPhong.SelectedItem.ToString() == "") return;
            cbMaPhong.Text = GetPhongByTenNhaVaTenPhong(cbTenToaNha.SelectedItem.ToString(), cbTenPhong.SelectedItem.ToString()).Rows[0]["MaPhong"].ToString();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbMaPhong_DropDown(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cbMaPhong.Text))
            {
                cbMaPhong.Items.Clear();
                cbMaPhong.Items.Add("");
                return;
            }

            if (this.GetPhongByMaPhong(cbMaPhong.Text).Rows.Count == 0)
            {
                cbMaPhong.Items.Clear();
                cbMaPhong.SelectionStart = cbMaPhong.Text.Length;
                cbMaPhong.Items.Add("");
                return;
            }

            cbMaPhong.Items.Clear();
            cbMaPhong.SelectionStart = cbMaPhong.Text.Length;

            foreach (DataRow item in GetPhongByMaPhong(cbMaPhong.Text).Rows)
            {
                cbMaPhong.Items.Add(item["MaPhong"].ToString());
            }
        }
    }
}
