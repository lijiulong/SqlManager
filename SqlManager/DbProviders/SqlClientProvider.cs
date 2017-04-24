using System.Data.SqlClient;

namespace Franksoft.SqlManager.DbProviders
{
    /// <summary>
    /// A simple implementation of <see cref="IDbProvider"/> provided by SqlManager for Sql Server ADO.Net classes.
    /// </summary>
    public class SqlClientProvider : BaseDbProvider
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public SqlClientProvider()
        {
        }

        /// <summary>
        /// Initializes <see cref="SqlClientProvider"/> instance with specific connection string.
        /// </summary>
        /// <param name="connectionString">
        /// Connection string used to initialize <see cref="SqlClientProvider"/> instance.
        /// </param>
        public SqlClientProvider(string connectionString)
        {
            this.Initialize(connectionString);
        }

        /// <summary>
        /// Initializes <see cref="SqlClientProvider"/> instance with specific <see cref="SqlConnection"/> instance.
        /// </summary>
        /// <param name="connection">
        /// <see cref="SqlConnection"/> instance used to initialize <see cref="SqlClientProvider"/> instance.
        /// </param>
        public SqlClientProvider(SqlConnection connection)
        {
            this.ConnectionString = connection.ConnectionString;
            this.Connection = connection;
            this.Command = new SqlCommand();
            this.Command.Connection = this.Connection;
            this.Adapter = new SqlDataAdapter();
        }

        /// <summary>
        /// Initializes <see cref="SqlClientProvider"/> instance with specific connection string. 
        /// You can use this method to change connection string without create a new provider instance.
        /// </summary>
        /// <param name="connectionString">
        /// Connection string used to initialize <see cref="SqlClientProvider"/> instance.
        /// </param>
        public override void Initialize(string connectionString)
        {
            if (!string.IsNullOrEmpty(connectionString) && !string.Equals(this.ConnectionString, connectionString))
            {
                this.ConnectionString = connectionString;
                this.Connection = new SqlConnection(connectionString);
                this.Command = new SqlCommand();
                this.Command.Connection = this.Connection;
                this.Adapter = new SqlDataAdapter();

                this.Connection.Open();
            }
        }
    }
}
