using SQLite;

namespace DentalAssistantXF.Services
{
    public interface ISQLiteConnection
    {
        SQLiteAsyncConnection GetConnection();
    }
}
