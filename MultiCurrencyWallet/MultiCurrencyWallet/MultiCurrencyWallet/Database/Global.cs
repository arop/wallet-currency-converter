using SQLite;

namespace MultiCurrencyWallet.Database
{
    class Global
    {

        [PrimaryKey]
        public string key { get; set; }
        public string value { get; set; }

        public Global(string k, string v)
        {
            key = k;
            value = v;
        }

        public Global()
        {
        }

    }
}
