using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ProjectDB2
{
    public class DBconnection{
        SqlConnection cn=new SqlConnection();
        SqlCommand cm=new SqlCommand();
        SqlDataReader dr;

        private string con;
        
        public string MyConnection(){
            con = @"Data Source=MUJTABA-NOTEBOO\SQLEXPRESS;Initial Catalog=master;Integrated Security=True";
            return con;
        }
    }
}
  