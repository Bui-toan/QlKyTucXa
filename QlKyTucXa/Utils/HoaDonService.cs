namespace QlKyTucXa.Utils
{
    public class HoaDonService
    {
        public static string GenerateMaHoaDon(string maPhong, int Thang, int Nam)
        {
            return $"{maPhong}{Thang:00}{Nam}";
        }
    }

}