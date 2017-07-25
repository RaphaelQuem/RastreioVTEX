using RastreioEco.Model;
using RastreioECO.Infra;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RastreioECO.DAO
{
    public class PedidoDAO
    {
        private SqlConnection connection;
        public PedidoDAO(SqlConnection pCon)
        {
            connection = pCon;
        }

        public List<Pedido> ListarPedidos()
        {
            DataTable dt = new DataTable();
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "";
                command.CommandText += Environment.NewLine + " select	rtrim(ltrim(pedidoweb)) as pedidoweb, ";
                command.CommandText += Environment.NewLine + " 		    valos, ";
                command.CommandText += Environment.NewLine + "          convert(int,notas) as notas, ";
                command.CommandText += Environment.NewLine + " 		    c.nfechv, ";
                command.CommandText += Environment.NewLine + " 		    c.nfexml ";
                command.CommandText += Environment.NewLine + " from 	sigmvcab a with(nolock)    ";
                command.CommandText += Environment.NewLine + " 			   left join ";
                command.CommandText += Environment.NewLine + " 		    sigcdcli b ";
                command.CommandText += Environment.NewLine + " 			   on a.contads = b.iclis ";
                command.CommandText += Environment.NewLine + " 			   left join ";
                command.CommandText += Environment.NewLine + " 		    sigmvnfi c ";
                command.CommandText += Environment.NewLine + " 			   on a.empdopnums = c.empdopnums  ";
                command.CommandText += Environment.NewLine + " where 	a.pedidoweb<> space(50)  ";
                command.CommandText += Environment.NewLine + " and      a.dopes in ('VENDA E-COMM AVULSA ', 'VENDA E-COMMERCE    ','VENDA TROCA E-COMMER') ";


                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                        return dt.ToModelList<Pedido>();
                    else
                        return new List<Pedido>();
                }
            }

        }
    }
}
