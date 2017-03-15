using System.Data.SQLite;

namespace Franksoft.SqlManager.DbProviders
{
    public class SQLiteProvider : BaseDbProvider
    {
        public SQLiteProvider()
        {
        }

        public SQLiteProvider(string connectionString)
        {
            this.Initialize(connectionString);
        }

        public SQLiteProvider(SQLiteConnection connection)
        {
            this.ConnectionString = connection.ConnectionString;
            this.Connection = connection;
            this.Command = new SQLiteCommand();
            this.Command.Connection = this.Connection;
            this.Adapter = new SQLiteDataAdapter();
        }

        public override void Initialize(string connectionString)
        {
            if (!string.IsNullOrEmpty(connectionString) && !string.Equals(this.ConnectionString, connectionString))
            {
                this.ConnectionString = connectionString;
                this.Connection = new SQLiteConnection(connectionString);
                this.Command = new SQLiteCommand();
                this.Command.Connection = this.Connection;
                this.Adapter = new SQLiteDataAdapter();

                this.Connection.Open();
            }
        }
    }
}
