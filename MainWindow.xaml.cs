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

namespace agenda
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        private string operacao;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            
            //contato com os dados da tela
            contato c = new contato();
            c.nome = txtNome.Text;
            c.email = txtEmail.Text;
            c.telefone = txtTel.Text;
            if(operacao == "inserir")
            {
                //gravar no banco de dados
                using (agendaEntities ctx = new agendaEntities())
                {
                    ctx.contatos.Add(c);
                    ctx.SaveChanges();
                }
            }
           /* if (operacao = "alterar")
            {

            }*/
            

        }

        private void btnInserir_Click(object sender, RoutedEventArgs e)
        {
            this.operacao = "inserir";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.ListarContatos();
        }
        private void ListarContatos()
        {
            using (agendaEntities ctx = new agendaEntities())
            {
                var consulta = ctx.contatos;
                dgDados.ItemsSource = consulta.ToList();
                //ItemsSource representa a origem de onde se encontra os dados do dgGrid
                //ToList eu estou convertendo para a forma de lista.

            }
        }
    }
}
