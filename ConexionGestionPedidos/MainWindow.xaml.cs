using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Data.SqlClient;
using System.Data;

namespace ConexionGestionPedidos
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string miConexion = ConfigurationManager.ConnectionStrings["ConexionGestionPedidos.Properties.Settings.DevDBConnectionString"].ConnectionString;

            miConexionSql = new SqlConnection(miConexion);

            MuestraCliente();

            MuestraTodosPedidos();

        }

        private void MuestraCliente() {

            try
            {
                string consulta = "SELECT * FROM CLIENTE";

                SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(consulta, miConexionSql);

                using (miAdaptadorSql)
                {

                    DataTable clienteTablas = new DataTable();

                    miAdaptadorSql.Fill(clienteTablas);

                    listaClientes.DisplayMemberPath = "nombre";
                    listaClientes.SelectedValuePath = "Id";
                    listaClientes.ItemsSource = clienteTablas.DefaultView;


                }

            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void MuestraPedido()
        {
            try
            {
                string consulta = "SELECT * FROM PEDIDO P INNER JOIN CLIENTE C ON C.ID=P .cCliente" +
                                " WHERE C.ID=@ClienteId";

                SqlCommand sqlComando = new SqlCommand(consulta, miConexionSql);

                SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(sqlComando);

                using (miAdaptadorSql)
                {

                    sqlComando.Parameters.AddWithValue("@ClienteId", listaClientes.SelectedValue);

                    DataTable pedidosTablas = new DataTable();

                    miAdaptadorSql.Fill(pedidosTablas);

                    pedidosCliente.DisplayMemberPath = "fechaPedido";
                    pedidosCliente.SelectedValuePath = "Id";
                    pedidosCliente.ItemsSource = pedidosTablas.DefaultView;

                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void MuestraTodosPedidos() {
            try
            {
                string consulta = "SELECT *, CONCAT(CCliente, ' ' , fechaPedido, ' ' , formaPago) AS INFOCOMPLETA FROM PEDIDO";

                SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(consulta, miConexionSql);

                using (miAdaptadorSql)
                {

                    DataTable pedidosTablas = new DataTable();

                    miAdaptadorSql.Fill(pedidosTablas);

                    todosPedidos.DisplayMemberPath = "INFOCOMPLETA";
                    todosPedidos.SelectedValuePath = "Id";
                    todosPedidos.ItemsSource = pedidosTablas.DefaultView;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        SqlConnection miConexionSql;

        /*private void listaClientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MuestraPedido();
        }*/

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(todosPedidos.SelectedValue.ToString());

            string consulta = "DELETE FROM PEDIDO WHERE ID=@PEDIDOID";

            SqlCommand miSqlComand = new SqlCommand(consulta, miConexionSql);

            miConexionSql.Open();

            miSqlComand.Parameters.AddWithValue("@PEDIDOID", todosPedidos.SelectedValue);

            miSqlComand.ExecuteNonQuery();

            miConexionSql.Close();

            MuestraTodosPedidos();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string consulta = "INSERT INTO CLIENTE (nombre) VALUES (@nombre)";

            SqlCommand miSqlComand = new SqlCommand(consulta, miConexionSql);

            miConexionSql.Open();

            miSqlComand.Parameters.AddWithValue("@nombre", insertarCliente.Text);

            miSqlComand.ExecuteNonQuery();

            miConexionSql.Close();

            MuestraCliente();

            insertarCliente.Text = "";

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string consulta = "DELETE FROM CLIENTE WHERE ID=@CLIENTEID";

            SqlCommand miSqlComand = new SqlCommand(consulta, miConexionSql);

            miConexionSql.Open();

            miSqlComand.Parameters.AddWithValue("@CLIENTEID", listaClientes.SelectedValue);

            miSqlComand.ExecuteNonQuery();

            miConexionSql.Close();

            MuestraCliente();
        }

        private void listaClientes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MuestraPedido();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Actualiza ventanaActualizar = new Actualiza((int)listaClientes.SelectedValue);

            ventanaActualizar.Show();

            try
            {
                string consulta = "SELECT nombre FROM CLIENTE WHERE Id=@ClId";

                SqlCommand miSqlComand = new SqlCommand(consulta, miConexionSql);

                SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(miSqlComand);

                using (miAdaptadorSql)
                {

                    miSqlComand.Parameters.AddWithValue("@ClId", listaClientes.SelectedValue);

                    DataTable clienteTabla = new DataTable();

                    miAdaptadorSql.Fill(clienteTabla);

                    ventanaActualizar.cuadroActualiza.Text = clienteTabla.Rows[0]["nombre"].ToString();



                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //ventanaActualizar.ShowDialog();

            //MuestraCliente();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            MuestraCliente();
        }
    }
}
