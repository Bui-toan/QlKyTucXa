using System;
using System.Windows.Forms;

namespace QlKyTucXa
{
	public partial class Dashboard : Form
	{
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
	}
}
