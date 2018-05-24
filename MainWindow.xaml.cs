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
           if (operacao == "alterar")
            {

            }
            this.ListarContatos();
            this.AlterarBotoes(1);
            this.LimpaCampos();

        }

        private void btnInserir_Click(object sender, RoutedEventArgs e)
        {
            this.operacao = "inserir";
            this.AlterarBotoes(2);
            txtID.Text = "";
            txtID.IsEnabled = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.ListarContatos();
            this.AlterarBotoes(1);
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
        private void AlterarBotoes(int op)
        {
            btnAlterar.IsEnabled = false;
            btnInserir.IsEnabled = false;
            btnExcluir.IsEnabled = false;
            btnCancelar.IsEnabled = false;
            btnLocalizar.IsEnabled = false;
            btnSalvar.IsEnabled = false;
            if (op == 1)
            {
                //ativar opções iniciais
                btnInserir.IsEnabled = true;
                btnLocalizar.IsEnabled = true;
            }
            if (op == 2)
            {
                //inserir um valor
                btnSalvar.IsEnabled = true;
                btnCancelar.IsEnabled = true;
            }
            if (op == 3)
            {
                btnAlterar.IsEnabled = true;
                btnExcluir.IsEnabled = true;
            }
        }
        private void LimpaCampos()
        {
            txtID.IsEnabled = true; 
            txtID.Clear();
            txtNome.Clear();
            txtEmail.Clear();
            txtTel.Clear();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.AlterarBotoes(1);
            this.LimpaCampos();
        }
    }
}
