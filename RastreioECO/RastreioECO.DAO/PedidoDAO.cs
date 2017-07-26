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

        public List<Pedido> ListarPedidos(bool somenteNaoSubido)
        {
            DataTable dt = new DataTable();
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "";
                command.CommandText += Environment.NewLine + " select	'False' Checado,";
                command.CommandText += Environment.NewLine + " 		    rtrim(ltrim(pedidoweb)) as pedidoweb, ";
                command.CommandText += Environment.NewLine + " 		    rtrim(ltrim(pedidoweb)) as pedidooriginal, ";
                command.CommandText += Environment.NewLine + " 		    valos, ";
                command.CommandText += Environment.NewLine + "          convert(int,notas) as notas, ";
                command.CommandText += Environment.NewLine + " 		    c.nfechv, ";
                command.CommandText += Environment.NewLine + " 		    c.nfexml, ";
                command.CommandText += Environment.NewLine + " 		    CASE ISNULL(c.chkRastreio,0)";
                command.CommandText += Environment.NewLine + " 		       WHEN 0 THEN 'Não'";
                command.CommandText += Environment.NewLine + " 		       ELSE 'Sim'";
                command.CommandText += Environment.NewLine + " 		    END as ChkRastreio ";
                command.CommandText += Environment.NewLine + " from 	sigmvcab a with(nolock)    ";
                command.CommandText += Environment.NewLine + " 			   left join ";
                command.CommandText += Environment.NewLine + " 		    sigcdcli b with(nolock)";
                command.CommandText += Environment.NewLine + " 			   on a.contads = b.iclis ";
                command.CommandText += Environment.NewLine + " 			   left join ";
                command.CommandText += Environment.NewLine + " 		    sigmvnfi c with(nolock)";
                command.CommandText += Environment.NewLine + " 			   on a.empdopnums = c.empdopnums  ";
                command.CommandText += Environment.NewLine + " where 	a.pedidoweb<> space(50)  ";
                command.CommandText += Environment.NewLine + " and      rtrim(CONVERT(VARCHAR(MAX), c.nfexml)) != ''";
                command.CommandText += Environment.NewLine + " and      a.dopes in ('VENDA E-COMM AVULSA ', 'VENDA E-COMMERCE    ','VENDA TROCA E-COMMER') ";
                command.CommandText += (somenteNaoSubido? " and      c.chkrastreio = 0":"");


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

        public void MudarFlagPedido(string pedidoWeb)
        {
            using (SqlCommand command = connection.CreateCommand())
            {

                command.CommandText = "";
                command.CommandText += Environment.NewLine + " UPDATE sigmvnfi";
                command.CommandText += Environment.NewLine + " SET    chkrastreio = 1 ";
                command.CommandText += Environment.NewLine + " WHERE  EMPDOPNUMS = ";
                command.CommandText += Environment.NewLine + " (";
                command.CommandText += Environment.NewLine + " 		SELECT EMPDOPNUMS";
                command.CommandText += Environment.NewLine + " 		FROM   SIGMVCAB with(nolock)";
                command.CommandText += Environment.NewLine + " 		WHERE  PEDIDOWEB = @P_PEDIDOWEB";
                command.CommandText += Environment.NewLine + " )";

                SqlParameter pPedidoWeb = command.CreateParameter();
                pPedidoWeb.ParameterName = "P_PEDIDOWEB";
                pPedidoWeb.Value = pedidoWeb;

                command.Parameters.Add(pPedidoWeb);

                command.ExecuteNonQuery();
            }
        }
    }
}
