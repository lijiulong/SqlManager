using System.Configuration;
using System.Windows.Forms;

using Franksoft.SqlManager.DbProviders;

namespace Franksoft.SqlManager.Tester
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["test"].ConnectionString;
            OleDbProvider provider = new OleDbProvider(connectionString);

            SqlManager.Instance.DbProvider = provider;
            using (var reader = SqlManager.Instance.GetStandaloneQueryReader("a"))
            {
                while(reader.Read())
                {
                    
                }
            }
        }
    }
}
