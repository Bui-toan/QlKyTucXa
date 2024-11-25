using QlKyTucXa.DAO;

namespace QlKyTucXa.Singleton
{
    public sealed class DaoSingleton
    {
        private static DataAccess Instance = null;

        public static DataAccess GetInstance()
        {
            if (Instance == null)
            {
                Instance = new DataAccess();
            }
            return Instance;
        }
        private DaoSingleton() { }
    }
}
