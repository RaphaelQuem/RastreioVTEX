using RastreioEco.Model;
using RastreioECO.DAO;
using RastreioECO.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RastreioECO.ViewModel
{
    public class ListaPedidoVM:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        private List<Pedido> pedidos;
        public List<Pedido> Pedidos { get { return pedidos; } set { pedidos = value; NotifyPropertyChanged("Pedidos");} }
        private Pedido pedido;
        public Pedido Pedido { get { return pedido; } set { pedido = value; NotifyPropertyChanged("Pedido"); } }
        private string log;
        public string Log { get { return log; } set { log = value; NotifyPropertyChanged("Log"); } }
        public bool checkAllChecked;
        public bool CheckAllChecked { get { return checkAllChecked; } set { checkAllChecked = value; NotifyPropertyChanged("CheckAllChecked"); } }
        public bool naoSubidoChecked;
        public bool NaoSubidoChecked { get { return naoSubidoChecked; } set { naoSubidoChecked = value; NotifyPropertyChanged("NaoSubidoChecked"); } }
        public ListaPedidoVM()
        {
            NaoSubidoChecked = true;
            Pedidos = GetPedidos();
        }

        public List<Pedido> GetPedidos()
        {
            using (SqlConnection  connection = RastreioHelper.GetOpenConnection())
            {
                PedidoDAO dao = new PedidoDAO(connection);

                return dao.ListarPedidos(NaoSubidoChecked);
            }
        }
    }
}
