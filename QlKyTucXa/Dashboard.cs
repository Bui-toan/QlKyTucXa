using QlKyTucXa.Classes;
using System;
using System.Data;
using System.Windows.Forms;

namespace QlKyTucXa
{
    public partial class Dashboard : Form
    {
        DataProcesser db = new DataProcesser();
        string sql = null;

        public Dashboard()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private void btnThemSv_Click(object sender, EventArgs e)
        {
            AddNewStudent addNewStudent = new AddNewStudent();
            addNewStudent.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn có muốn thoát không?", "Xác nhận", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                this.Close();
                Login login = new Login();
                login.Show();
            }
        }

        private void btnQlp_Click(object sender, EventArgs e)
        {
            AddNewRoom addNewRoom = new AddNewRoom();
            addNewRoom.Show();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            //Danh sách mã số thuê > 30
            sql = "select MaSoThue, Ngaykt from SV_Phong"
                + " WHERE DATEDIFF(DAY, Ngaykt, GETDATE()) > 30";
            DataTable dsMaSoThue1 = db.ReadData(sql);

            //Danh sách mã số thuê <= 30
            sql = "select MaSoThue, Ngaykt from SV_Phong"
                + " WHERE DATEDIFF(DAY, Ngaykt, GETDATE()) <= 30";
            DataTable dsMaSoThue2 = db.ReadData(sql);

            foreach (DataRow r in dsMaSoThue1.Rows)
            {
                sql = "select svp.MaSoThue, ngaytra from SV_Phong svp"
                    + " join Traphong tp on svp.MaSoThue = tp.MaSoThue"
                    + " where svp.MaSoThue = N'" + r["MaSoThue"].ToString() + "'";
                DataTable dsSVP_MST = db.ReadData(sql);

                //Kiểm tra xem có trong TraPhong không
                if (dsSVP_MST.Rows.Count == 0)
                {
                    db.ChangeData("DELETE FROM SV_Phong WHERE MaSoThue = N'" + r["MaSoThue"].ToString() + "'");
                }
                else
                {
                    db.ChangeData("DELETE FROM Traphong WHERE MaSoThue = N'" + r["MaSoThue"].ToString() + "'");
                    db.ChangeData("DELETE FROM SV_Phong WHERE MaSoThue = N'" + r["MaSoThue"].ToString() + "'");
                }
            }

            foreach (DataRow r in dsMaSoThue2.Rows)
            {
                sql = "select svp.MaSoThue, ngaytra from SV_Phong svp"
                    + " join Traphong tp on svp.MaSoThue = tp.MaSoThue"
                    + " where svp.MaSoThue = N'" + r["MaSoThue"].ToString() + "'"
                    + " and DATEDIFF(DAY, ngaytra, GETDATE()) > 30";
                DataTable dsSVP_MST = db.ReadData(sql);

                //Kiểm tra xem có trong TraPhong không
                if (dsSVP_MST.Rows.Count != 0)
                {
                    db.ChangeData("DELETE FROM Traphong WHERE MaSoThue = N'" + r["MaSoThue"].ToString() + "'");
                    db.ChangeData("DELETE FROM SV_Phong WHERE MaSoThue = N'" + r["MaSoThue"].ToString() + "'");
                }
            }
        }

        private void btnTTP_Click(object sender, EventArgs e)
        {
            ChiTietPhong ctp = new ChiTietPhong();
            ctp.Show();
        }

        private void btnTraPhong_Click(object sender, EventArgs e)
        {
            TraPhong traPhong = new TraPhong();
            traPhong.Show();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            var qlHaDon = new QuanLyHoaDonPhong();
            qlHaDon.Show();
        }
    }
}
