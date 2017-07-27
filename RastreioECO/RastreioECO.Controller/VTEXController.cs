using RastreioEco.Model;
using RastreioECO.Controller.WsVTEX;
using RastreioECO.DAO;
using RastreioECO.Infra;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.ServiceModel;
using System.Text;

namespace RastreioECO.Controller
{
    public class VTEXController
    {
        private const string SERVICE_URL = "https://webservice-montecarlo.vtexcommerce.com.br/AdminWebService/Service.svc";
        TotalController controller;
        public StringBuilder log;
        public VTEXController()
        {
            log = new StringBuilder();
            controller = new TotalController();
        }
        public void SubirRastreioEXML(List<Pedido> pedidos)
        {
            log.Clear();

            ServiceClient vtexservice = getVtexService(SERVICE_URL, "montecarlo", "monteservice123");
            vtexservice.Open();
            foreach (Pedido pedido in pedidos)
            {

                bool sucesso = false;
                int tentativas = 0;
                while (!sucesso && tentativas < 3)
                {
                    try
                    {

                        WsVTEX.OrderInvoiceDTO dados = new WsVTEX.OrderInvoiceDTO();

                        dados.OrderIdV3 = pedido.PedidoWeb;
                        dados.IssuanceDate = DateTime.Now;
                        dados.InvoiceValue = pedido.Valos;
                        dados.Courier = "Total Express";
                        dados.InvoiceNumber = pedido.Notas.ToString();
                        dados.TrackingNumber = pedido.Notas.ToString();

                        vtexservice.UpdateNotifyShipping(dados);
                        controller.UploadXMLPedido(pedido);
                        
                        MudarFlagPedido(pedido,1,"=");

                        sucesso = true;
                    }
                    catch (Exception ex)
                    {
                        log.AppendLine(ex.Message);
                        if(tentativas.Equals(2))
                            MudarFlagPedido(pedido,2,"IN");
                        tentativas++;
                        sucesso = false;
                    }
                }
            }

        }
        private void MudarFlagPedido(Pedido pedido,int flag,string inEqual)
        {
            using (SqlConnection connection = RastreioHelper.GetOpenConnection())
            {

                PedidoDAO dao = new PedidoDAO(connection);

                dao.MudarFlagPedido(pedido.PedidoOriginal,flag,inEqual);
            }
        }
        private ServiceClient getVtexService(string strWebService, string strUser, string strPassword)
        {
            bool hasValidation = !(string.IsNullOrWhiteSpace(strUser)) && !(string.IsNullOrWhiteSpace(strPassword));

            BasicHttpBinding objBinding = new BasicHttpBinding();

            int nDefaultLength = 2000000;

            objBinding.ReaderQuotas.MaxDepth = nDefaultLength;
            objBinding.ReaderQuotas.MaxArrayLength = nDefaultLength;
            objBinding.ReaderQuotas.MaxBytesPerRead = nDefaultLength;
            objBinding.ReaderQuotas.MaxNameTableCharCount = nDefaultLength;
            objBinding.ReaderQuotas.MaxStringContentLength = nDefaultLength;
            objBinding.MaxReceivedMessageSize = nDefaultLength;
            objBinding.MaxBufferPoolSize = nDefaultLength;
            objBinding.MaxBufferSize = nDefaultLength;

            objBinding.CloseTimeout = new TimeSpan(0, 2, 0);
            objBinding.OpenTimeout = objBinding.CloseTimeout;
            objBinding.ReceiveTimeout = objBinding.CloseTimeout;
            objBinding.SendTimeout = objBinding.CloseTimeout;

            if (hasValidation)
            {
                objBinding.Security.Mode = BasicHttpSecurityMode.Transport;
                objBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
            }

            ServiceClient objServiceClient = new ServiceClient(objBinding, new EndpointAddress(strWebService));

            if (hasValidation)
            {
                objServiceClient.ClientCredentials.UserName.UserName = strUser;
                objServiceClient.ClientCredentials.UserName.Password = strPassword;
            }

            return objServiceClient;
        }
    }
}
