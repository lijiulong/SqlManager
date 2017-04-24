using System.Data.SQLite;

namespace Franksoft.SqlManager.DbProviders
{
    /// <summary>
    /// A simple implementation of <see cref="IDbProvider"/> provided by SqlManager for SQLite ADO.Net classes.
    /// </summary>
    public class SQLiteProvider : BaseDbProvider
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public SQLiteProvider()
        {
        }

        /// <summary>
        /// Initializes <see cref="SQLiteProvider"/> instance with specific connection string.
        /// </summary>
        /// <param name="connectionString">
        /// Connection string used to initialize <see cref="SQLiteProvider"/> instance.
        /// </param>
        public SQLiteProvider(string connectionString)
        {
            this.Initialize(connectionString);
        }

        /// <summary>
        /// Initializes <see cref="SQLiteProvider"/> instance with specific <see cref="SQLiteConnection"/> instance.
        /// </summary>
        /// <param name="connection">
        /// <see cref="SQLiteConnection"/> instance used to initialize <see cref="SQLiteProvider"/> instance.
        /// </param>
        public SQLiteProvider(SQLiteConnection connection)
        {
            this.ConnectionString = connection.ConnectionString;
            this.Connection = connection;
            this.Command = new SQLiteCommand();
            this.Command.Connection = this.Connection;
            this.Adapter = new SQLiteDataAdapter();
        }

        /// <summary>
        /// Initializes <see cref="SQLiteProvider"/> instance with specific connection string. 
        /// You can use this method to change connection string without create a new provider instance.
        /// </summary>
        /// <param name="connectionString">
        /// Connection string used to initialize <see cref="SQLiteProvider"/> instance.
        /// </param>
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
