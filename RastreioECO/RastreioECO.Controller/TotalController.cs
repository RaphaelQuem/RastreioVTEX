using Microsoft.Win32;
using RastreioEco.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RastreioECO.Controller
{
    public class TotalController
    {
        string host;
        string user;
        string pass;
        string dir;
        string xmldir;
        StringBuilder log;

        public TotalController()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE", true);


                key = key.OpenSubKey("TotalUploader", true);

                host = (key.GetValue("Host") == null ? "" : key.GetValue("Host")).ToString();
                user = (key.GetValue("User") == null ? "" : key.GetValue("User")).ToString();
                pass = (key.GetValue("Pass") == null ? "" : key.GetValue("Pass")).ToString();
                dir = (key.GetValue("Dir") == null ? "" : key.GetValue("Dir")).ToString();
                xmldir = (key.GetValue("XMLdir") == null ? "" : key.GetValue("XMLdir")).ToString();
                log = new StringBuilder();
            }
            catch
            {
                throw new Exception("Erro ao tentar ler as chaves do registro!");
            }
        }

        public void UploadXMLPedido(Pedido pedido)
        {
            bool sucesso = false;
            int tentativas = 0;

            while (!sucesso && tentativas < 3)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(xmldir + "\\" + pedido.NfeChv + ".xml", true))
                    {
                        writer.Write(pedido.NfeXML);
                    }

                    EnviarNfe(xmldir, pedido.NfeChv + ".xml");
                    sucesso = true;
                }
                catch (Exception ex)
                {
                    if(tentativas.Equals(2))
                    {
                        throw new Exception(log.ToString() + Environment.NewLine + ex.Message);
                    }
                    log.AppendLine(ex.Message);
                    tentativas++;
                    
                }
            }

        }
        private void EnviarNfe(string diretorioArq, string nomeArq)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(host + dir + nomeArq);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(user, pass);
            request.UseBinary = true;
            request.UsePassive = true;
            request.KeepAlive = true;

            string arquivo = diretorioArq + "\\" + nomeArq;

            StreamReader sourceStream = new StreamReader(arquivo);
            byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
            sourceStream.Close();
            request.ContentLength = fileContents.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(fileContents, 0, fileContents.Length);
            requestStream.Close();

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            response.Close();
        }
    }
}
