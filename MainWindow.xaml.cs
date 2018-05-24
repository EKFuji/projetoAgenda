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
            
            //gravar no banco de dados

            if (this.operacao == "inserir")
            {
                contato c = new contato();
                c.nome = txtNome.Text;
                c.email = txtEmail.Text;
                c.telefone = txtTel.Text;
                using (agendaEntities ctx = new agendaEntities())
                {
                    ctx.contatos.Add(c);
                    ctx.SaveChanges();
                }
            }
           if (this.operacao == "alterar")
            {
                using (agendaEntities ct = new agendaEntities())
                {
                    contato c = ct.contatos.Find(Convert.ToInt32(txtID.Text));
                    if (c!= null)
                    {
                       
                        c.nome = txtNome.Text;
                        c.email = txtEmail.Text;
                        c.telefone = txtTel.Text;
                        ct.SaveChanges();
                    }
                }
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
            this.LimpaCampos();
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
                int a = ctx.contatos.Count();

                /*
                 int a = ctx.contatos.Sum(c => c.id);
                  Sum vai somar os contatos
                  Entre parenteses temos uma função lambda(anonima), um método que não é executado
                  em si mesmo
                 */
                lblContatos.Content = "Total de registros: "+a.ToString();

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

        private void btnLocalizar_Click(object sender, RoutedEventArgs e)
        {
            if (txtID.Text.Trim().Count() > 0)
            {
                //buscar pelo código
                try
                {
                    int id = Convert.ToInt32(txtID.Text);
                    using (agendaEntities ctx = new agendaEntities())
                    {
                        var consulta = ctx.contatos;
                        dgDados.ItemsSource = consulta.ToList();
                        contato c = ctx.contatos.Find(id);
                        dgDados.ItemsSource = new contato[1] { c };
                    }
                }
                catch
                {

                }

            }
            if (txtNome.Text.Trim().Count() > 0)
            {
                //buscar por nome
                try
                {
                    using (agendaEntities ctx = new agendaEntities())
                    {
                        var consulta = from c in ctx.contatos
                                       where c.nome.Contains(txtNome.Text)
                                       select c;
                        dgDados.ItemsSource = consulta.ToList();
                    }
                }
                catch
                {

                }
            }
            if (txtEmail.Text.Trim().Count() > 0)
            {
                //buscar por email
                try
                {
                    using (agendaEntities ctx = new agendaEntities())
                    {
                        var consulta = from c in ctx.contatos
                                       where c.email.Contains(txtEmail.Text)
                                       select c;
                        dgDados.ItemsSource = consulta.ToList();
                    }
                }
                catch
                {

                }
            }
            if (txtTel.Text.Trim().Count() > 0)
            {
                //buscar por telefone
                try
                {
                    using (agendaEntities ctx = new agendaEntities())
                    {
                        var consulta = from c in ctx.contatos
                                       where c.telefone.Contains(txtTel.Text)
                                       select c;
                        dgDados.ItemsSource = consulta.ToList();
                    }
                }
                catch
                {

                }
            }
        }

        private void dgDados_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
           if(dgDados.SelectedIndex >= 0)
            {
                //contato c = (contato)dgDados.Items[dgDados.SelectedIndex];
                contato c = (contato)dgDados.SelectedItem;
                txtID.Text = c.id.ToString();
                txtNome.Text = c.nome;
                txtEmail.Text = c.email;
                txtTel.Text = c.telefone;
                this.AlterarBotoes(3);
            }
        }

        private void btnAlterar_Click(object sender, RoutedEventArgs e)
        {
            this.operacao = "alterar";
            this.AlterarBotoes(2);
            txtID.IsEnabled = false;
            
        }

        private void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            using(agendaEntities ctx = new agendaEntities())
            {
                contato c = ctx.contatos.Find(Convert.ToInt32(txtID.Text));
                if(c != null)
                {
                    ctx.contatos.Remove(c);
                    ctx.SaveChanges();
                }
                this.ListarContatos();
                this.AlterarBotoes(1);
                this.LimpaCampos();
            }
        }
    }
}
