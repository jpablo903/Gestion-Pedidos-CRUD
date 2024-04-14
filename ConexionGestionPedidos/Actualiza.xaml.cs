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
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace ConexionGestionPedidos
{
    /// <summary>
    /// Lógica de interacción para Actualiza.xaml
    /// </summary>
    public partial class Actualiza : Window
    {
        private int z;

        public Actualiza(int elId)
        {
            InitializeComponent();

            z = elId;

            string miConexion = ConfigurationManager.ConnectionStrings["ConexionGestionPedidos.Properties.Settings.DevDBConnectionString"].ConnectionString;

            miConexionSql = new SqlConnection(miConexion);
        }

        SqlConnection miConexionSql;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string consulta = "UPDATE CLIENTE SET nombre=@nombre WHERE Id=" + z;

            SqlCommand miSqlComand = new SqlCommand(consulta, miConexionSql);

            miConexionSql.Open();

            miSqlComand.Parameters.AddWithValue("@nombre", cuadroActualiza.Text);

            miSqlComand.ExecuteNonQuery();

            miConexionSql.Close();

            this.Close();

           
        }
    }
}
