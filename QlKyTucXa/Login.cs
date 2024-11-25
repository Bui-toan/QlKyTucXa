
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;

namespace QlKyTucXa
{
	public partial class Login : Form
	{
		private List<string> imagePaths = new List<string>(); // Danh sách lưu trữ đường dẫn ảnh
		private int imageIndex = 0;
		private System.Timers.Timer imageSliderTimer;
		private Classes.DataProcesser dtBase = new Classes.DataProcesser();

		public Login()
		{
			InitializeComponent();
			SetupForm();
			LoadImagesFromFolder();
			StartImageSlider();
		}

		// Cấu hình form, cho phép co giãn
		private void SetupForm()
		{
			this.FormBorderStyle = FormBorderStyle.Sizable;
			this.MinimumSize = new Size(400, 300); // Kích thước tối thiểu
			pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
		}

		// Load hình ảnh từ thư mục
		private void LoadImagesFromFolder()
		{
			imagePaths.Add(@"C:\Users\bui toan\Downloads\image\image1.png");
			imagePaths.Add(@"C:\Users\bui toan\Downloads\image\image2.png");
			imagePaths.Add(@"C:\Users\bui toan\Downloads\image\image3.png");

			// Hiển thị ảnh đầu tiên nếu có ảnh trong thư mục
			if (imagePaths.Count > 0)
			{
				pictureBox1.Image = Image.FromFile(imagePaths[0]);
			}
			else
			{
				MessageBox.Show("Không tìm thấy ảnh trong thư mục.");
			}
		}

		// Bắt đầu trình chiếu ảnh tự động
		private void StartImageSlider()
		{
			//Khởi tạo Timer với khoảng thời gian 2 giây.
			//Gắn sự kiện Elapsed để định nghĩa công việc cần thực hiện mỗi khi Timer "hết giờ".
			//Đảm bảo Timer tự động lặp lại công việc khi hết khoảng thời gian(nhờ AutoReset = true).
			//Bật Timer để bắt đầu hoạt động.
			imageSliderTimer = new System.Timers.Timer(2000); // Chuyển ảnh mỗi 2 giây
			imageSliderTimer.Elapsed += OnTimedEvent;
			imageSliderTimer.AutoReset = true;
			imageSliderTimer.Enabled = true;
		}

		// Hàm xử lý khi đến giờ đổi ảnh
		private void OnTimedEvent(object source, ElapsedEventArgs e)
		{
			if (imagePaths.Count > 0)
			{
				imageIndex = (imageIndex + 1) % imagePaths.Count;

				// Giải phóng ảnh cũ trước khi tải ảnh mới
				if (pictureBox1.Image != null)
				{
					pictureBox1.Image.Dispose();
				}

				try
				{
					// Tải và hiển thị ảnh mới từ danh sách
					pictureBox1.Image = Image.FromFile(imagePaths[imageIndex]);
				}
				catch (Exception ex)
				{
					MessageBox.Show("Không thể tải ảnh: " + ex.Message);
				}
			}
		}

		// Xử lý sự kiện đăng nhập
		private void btnLogin_Click(object sender, EventArgs e)
		{
			string username = txtUsername.Text;
			string password = txtPassword.Text;

			string query = $"SELECT * FROM Users WHERE MaNhanVien = '{username}' AND Password = '{password}'";
			DataTable dt = dtBase.ReadData(query);

			if (dt.Rows.Count > 0)
			{
				string maNhanVien = dt.Rows[0]["MaNhanVien"].ToString();
				string tenNhanVien = dt.Rows[0]["TenNhanVien"].ToString();

				// Ẩn form đăng nhập và mở Dashboard
				this.Hide();
				Dashboard dashboard = new Dashboard();
				dashboard.Show();

			}
			else
			{
				MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng.");
			}
		}

		// Xử lý sự kiện điều chỉnh kích thước form


		// Xử lý sự kiện đóng ứng dụng
		private void btnExit_Click(object sender, EventArgs e)
		{
			Application.Exit();
			this.Close();
		}

		// Xử lý sự kiện click vào label (nếu cần thêm chức năng)
		private void guna2HtmlLabel1_Click(object sender, EventArgs e)
		{
			// Thêm logic nếu cần
		}
	}
}