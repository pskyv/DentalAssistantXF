using DentalAssistantXF.Android.Services;
using DentalAssistantXF.Services;
using SQLite;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteDb))]
namespace DentalAssistantXF.Android.Services
{
    public class SQLiteDb : ISQLiteConnection
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, "DentalAssistant.db3");

            return new SQLiteAsyncConnection(path);
        }
    }
}