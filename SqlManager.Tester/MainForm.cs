using System.Configuration;
using System.Windows.Forms;

using Franksoft.SqlManager.DbProviders;
using Franksoft.SqlManager.Mock;

namespace Franksoft.SqlManager.Tester
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["test"].ConnectionString;
            //OleDbProvider provider = new OleDbProvider(connectionString);
            SQLiteProvider sqliteProvider = new SQLiteProvider();
            MockProvider provider = new MockProvider(sqliteProvider);

            SqlManager.Instance.DbProvider = provider;
            using (var reader = SqlManager.Instance.GetStandaloneQueryReader("a"))
            {
                while(reader.Read())
                {
                    string a = reader["A"].ToString();
                }
            }

            using (var reader = SqlManager.Instance.GetStandaloneQueryReader("a"))
            {
                while (reader.Read())
                {
                    string a = reader["NAME"].ToString();
                }
            }
        }
    }
}
