using System.Data.OleDb;

namespace Franksoft.SqlManager.DbProviders
{
    /// <summary>
    /// A simple implementation of <see cref="IDbProvider"/> provided by SqlManager for OleDb ADO.Net classes.
    /// </summary>
    public class OleDbProvider : BaseDbProvider
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public OleDbProvider()
        {
        }

        /// <summary>
        /// Initializes <see cref="OleDbProvider"/> instance with specific connection string.
        /// </summary>
        /// <param name="connectionString">
        /// Connection string used to initialize <see cref="OleDbProvider"/> instance.
        /// </param>
        public OleDbProvider(string connectionString)
        {
            this.Initialize(connectionString);
        }

        /// <summary>
        /// Initializes <see cref="OleDbProvider"/> instance with specific <see cref="OleDbConnection"/> instance.
        /// </summary>
        /// <param name="connection">
        /// <see cref="OleDbConnection"/> instance used to initialize <see cref="OleDbProvider"/> instance.
        /// </param>
        public OleDbProvider(OleDbConnection connection)
        {
            this.ConnectionString = connection.ConnectionString;
            this.Connection = connection;
            this.Command = new OleDbCommand();
            this.Command.Connection = this.Connection;
            this.Adapter = new OleDbDataAdapter();
        }

        /// <summary>
        /// Initializes <see cref="OleDbProvider"/> instance with specific connection string. 
        /// You can use this method to change connection string without create a new provider instance.
        /// </summary>
        /// <param name="connectionString">
        /// Connection string used to initialize <see cref="OleDbProvider"/> instance.
        /// </param>
        public override void Initialize(string connectionString)
        {
            if (!string.IsNullOrEmpty(connectionString) && !string.Equals(this.ConnectionString, connectionString))
            {
                this.ConnectionString = connectionString;
                this.Connection = new OleDbConnection(connectionString);
                this.Command = new OleDbCommand();
                this.Command.Connection = this.Connection;
                this.Adapter = new OleDbDataAdapter();

                this.Connection.Open();
            }
        }
    }
}
