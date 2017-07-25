using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RastreioECO.Infra
{
    public static class RastreioHelper
    {
        private const string CONNECTION_STRING = "Data Source=192.168.0.25;Initial Catalog=DB_MTCRL;User Id=Desenvolvimento;Password=a1234;MultipleActiveResultSets=True;";
        public static SqlConnection GetOpenConnection()
        {
            SqlConnection con = new SqlConnection(CONNECTION_STRING);
            con.Open();
            return con;
        }

    }
}
