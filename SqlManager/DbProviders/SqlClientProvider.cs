using System.Data.SqlClient;

namespace Franksoft.SqlManager.DbProviders
{
    public class SqlClientProvider : BaseDbProvider
    {
        public SqlClientProvider()
        {
        }

        public SqlClientProvider(string connectionString)
        {
            this.Initialize(connectionString);
        }

        public SqlClientProvider(SqlConnection connection)
        {
            this.ConnectionString = connection.ConnectionString;
            this.Connection = connection;
            this.Command = new SqlCommand();
            this.Command.Connection = this.Connection;
            this.Adapter = new SqlDataAdapter();
        }

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
