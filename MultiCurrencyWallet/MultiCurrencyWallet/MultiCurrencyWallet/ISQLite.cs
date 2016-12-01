
using SQLite;

namespace MultiCurrencyWallet
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}
