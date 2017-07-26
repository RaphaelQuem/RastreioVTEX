using RastreioEco.Model;
using RastreioECO.Controller;
using RastreioECO.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RastreioECO
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        VTEXController controller;
        public MainWindow()
        {
            ViewModel = new ListaPedidoVM();
            controller = new VTEXController();
            InitializeComponent();
        }
        public ListaPedidoVM ViewModel
        {
            get
            { return DataContext as ListaPedidoVM; }

            set
            { DataContext = value; }
        }

        private void chk_Checked(object sender, RoutedEventArgs e)
        {
            foreach (Pedido pedido in ViewModel.Pedidos)
            {
                pedido.Checado = Convert.ToBoolean(ViewModel.CheckAllChecked);
            }
            ListaPedidos.Items.Refresh();
        }

        private void subir_Click(object sender, RoutedEventArgs e)
        {
            controller.SubirRastreioEXML(ViewModel.Pedidos.Where(p => p.Checado.Equals(true)).ToList());
            ViewModel.Pedidos = ViewModel.GetPedidos();
            ViewModel.Log = controller.log.ToString();
        }

        private void chksimnao_Checked(object sender, RoutedEventArgs e)
        {
            ViewModel.Pedidos = ViewModel.GetPedidos();
        }
    }
}
