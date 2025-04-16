namespace BlogSiggaApp.Settings
{
    public static class AppSettings
    {
        public static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, "database.db");

        public static string ApiBaseUrl => "https://jsonplaceholder.typicode.com";
    }
}
