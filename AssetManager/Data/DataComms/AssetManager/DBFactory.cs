namespace AssetManager
{
    public static class DBFactory
    {

        public static IDataBase GetDatabase()
        {
            if (GlobalSwitches.CachedMode)
            {
                return new SQLiteDatabase(false);
            }
            else
            {
                return new MySQLDatabase();
            }
        }

    }
}
