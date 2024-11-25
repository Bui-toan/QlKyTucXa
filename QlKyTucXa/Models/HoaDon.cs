using System;

namespace QlKyTucXa.Models
{
    public class HoaDon
    {
        public string MaHoaDon { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }
        public string MaPhong { get; set; }
        public decimal? Tiendien { get; set; }
        public decimal? Tiennuoc { get; set; }
        public decimal? Tienvesinh { get; set; }
        public decimal? Tienphat { get; set; }
        public DateTime? NgayTao { get; set; }
        public DateTime? Ngaydong { get; set; }
        public int TrangThai { get; set; }
    }
}
