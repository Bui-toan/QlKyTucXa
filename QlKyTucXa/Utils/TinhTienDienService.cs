namespace QlKyTucXa.Utils
{
    public static class TinhTienDienService
    {
        public static decimal Calc(string soDien)
        {
            if (!int.TryParse(soDien, out int kwh) || kwh < 0)
            {
                return 0;
            }
            decimal total = decimal.Zero;
            if (kwh <= 50)
            {
                total = kwh * 1678;
            }
            else if (kwh <= 100)
            {
                total = 50 * 1678 + (kwh - 50) * 1734;
            }
            else if (kwh <= 200)
            {
                total = 50 * 1678 + 50 * 1734 + (kwh - 100) * 2014;
            }
            else if (kwh <= 300)
            {
                total = 50 * 1678 + 50 * 1734 + 100 * 2014 + (kwh - 200) * 2536;
            }
            else if (kwh <= 400)
            {
                total = 50 * 1678 + 50 * 1734 + 100 * 2014 + 100 * 2536 + (kwh - 300) * 2834;
            }
            else
            {
                total = 50 * 1678 + 50 * 1734 + 100 * 2014 + 100 * 2536 + 100 * 2834 + (kwh - 400) * 2927;
            }

            return total;
        }
        public static int GetSoDien(decimal tienDien)
        {
            if (tienDien <= 0)
            {
                return 0;
            }

            int[] thresholds = { 50, 50, 100, 100, 100 }; // kWh limits for each tier
            decimal[] rates = { 1678m, 1734m, 2014m, 2536m, 2834m, 2927m }; // Rate per kWh for each tier

            decimal totalCost = 0;
            int totalKwh = 0;

            for (int i = 0; i < thresholds.Length; i++)
            {
                int maxKwh = thresholds[i];
                decimal rate = rates[i];

                decimal tierCost = maxKwh * rate;

                if (tienDien <= totalCost + tierCost)
                {
                    int remainingKwh = (int)((tienDien - totalCost) / rate);
                    totalKwh += remainingKwh;
                    return totalKwh;
                }

                totalCost += tierCost;
                totalKwh += maxKwh;
            }

            // For kWh above 400
            totalKwh += (int)((tienDien - totalCost) / rates[rates.Length - 1]);
            return totalKwh;
        }
    }
}
