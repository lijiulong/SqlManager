using System.Data.OracleClient;

namespace Franksoft.SqlManager.DbProviders
{
    public class OracleClientProvider : BaseDbProvider
    {
        public OracleClientProvider()
        {
        }

        public OracleClientProvider(string connectionString)
        {
            this.Initialize(connectionString);
        }

        public OracleClientProvider(OracleConnection connection)
        {
            this.ConnectionString = connection.ConnectionString;
            this.Connection = connection;
            this.Command = new OracleCommand();
            this.Command.Connection = this.Connection;
            this.Adapter = new OracleDataAdapter();
        }

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
