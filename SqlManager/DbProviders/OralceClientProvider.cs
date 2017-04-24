using System.Data.OracleClient;

namespace Franksoft.SqlManager.DbProviders
{
    /// <summary>
    /// A simple implementation of <see cref="IDbProvider"/> provided by SqlManager for Oracle client ADO.Net classes.
    /// </summary>
    public class OracleClientProvider : BaseDbProvider
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public OracleClientProvider()
        {
        }

        /// <summary>
        /// Initializes <see cref="OracleClientProvider"/> instance with specific connection string.
        /// </summary>
        /// <param name="connectionString">
        /// Connection string used to initialize <see cref="OracleClientProvider"/> instance.
        /// </param>
        public OracleClientProvider(string connectionString)
        {
            this.Initialize(connectionString);
        }

        /// <summary>
        /// Initializes <see cref="OracleClientProvider"/> instance with specific 
        /// <see cref="OracleConnection"/> instance.
        /// </summary>
        /// <param name="connection">
        /// <see cref="OracleConnection"/> instance used to initialize <see cref="OracleClientProvider"/> instance.
        /// </param>
        public OracleClientProvider(OracleConnection connection)
        {
            this.ConnectionString = connection.ConnectionString;
            this.Connection = connection;
            this.Command = new OracleCommand();
            this.Command.Connection = this.Connection;
            this.Adapter = new OracleDataAdapter();
        }

        /// <summary>
        /// Initializes <see cref="OracleClientProvider"/> instance with specific connection string. 
        /// You can use this method to change connection string without create a new provider instance.
        /// </summary>
        /// <param name="connectionString">
        /// Connection string used to initialize <see cref="OracleClientProvider"/> instance.
        /// </param>
        public override void Initialize(string connectionString)
        {
            if (!string.IsNullOrEmpty(connectionString) && !string.Equals(this.ConnectionString, connectionString))
            {
                this.ConnectionString = connectionString;
                this.Connection = new OracleConnection(connectionString);
                this.Command = new OracleCommand();
                this.Command.Connection = this.Connection;
                this.Adapter = new OracleDataAdapter();

                this.Connection.Open();
            }
        }
    }
}
