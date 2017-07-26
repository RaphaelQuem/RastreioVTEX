using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RastreioECO.DAO;
using System.Data.SqlClient;
using RastreioECO.Infra;
using RastreioEco.Model;
using System.Collections.Generic;

namespace RastreioECO.Test
{
    [TestClass]
    public class PedidoDAOTest
    {
        [TestMethod]
        public void GetPedidosTest()
        {
            using (SqlConnection connection = RastreioHelper.GetOpenConnection())
            {
                PedidoDAO dao = new PedidoDAO(connection);

                List<Pedido> pedidos = dao.ListarPedidos(true);

                Assert.IsTrue(pedidos.Count > 0);

            }
        }
    }
}
