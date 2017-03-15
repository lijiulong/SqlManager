using System.Data.OleDb;

namespace Franksoft.SqlManager.DbProviders
{
    public class OleDbProvider : BaseDbProvider
    {
        public OleDbProvider()
        {
        }

        public OleDbProvider(string connectionString)
        {
            this.Initialize(connectionString);
        }

        public OleDbProvider(OleDbConnection connection)
        {
            this.ConnectionString = connection.ConnectionString;
            this.Connection = connection;
            this.Command = new OleDbCommand();
            this.Command.Connection = this.Connection;
            this.Adapter = new OleDbDataAdapter();
        }        

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
