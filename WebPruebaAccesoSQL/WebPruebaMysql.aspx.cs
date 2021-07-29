using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

namespace WebPruebaAccesoSQL
{
    public partial class WebPruebaMysql : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private MySqlConnection conexionMysql(ref string mensaje)
        {
            //crear la conexión a la base de datos
            MySqlConnection conecMysql = new MySqlConnection();
            conecMysql.ConnectionString = "Server = localhost; Port=3306; Database = escuela; Uid = root; Pwd =" +
                TextBox1.Text + ";";
            try
            {
                conecMysql.Open();
                mensaje = "Conexión abierta con Mysql";
            }
            catch (Exception h)
            {
                mensaje = "Error: " + h.Message;
                conecMysql = null;
            }

            return conecMysql;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
           

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            //abrir la conexion

            //solo referencias
            MySqlConnection conecNueva = null;
            MySqlCommand carrito = null;
            DataSet contenedor = null;
            MySqlDataAdapter trailer = null;

            string query = "select * from materia";
            string m = "";
            conecNueva = conexionMysql(ref m);

            if(conecNueva != null)
            {
                //creo la consulta, y le indico por cual corretera ira.
                carrito = new MySqlCommand();
                carrito.CommandText = query;
                carrito.Connection = conecNueva;

                trailer = new MySqlDataAdapter();
                trailer.SelectCommand = carrito;

                //llenar el dataSet
                contenedor = new DataSet();
                try
                {
                    trailer.Fill(contenedor);
                    TextBox2.Text = "Consulta correcta";
                    GridView1.DataSource = contenedor.Tables[0];
                    GridView1.DataBind();
                }
                catch(Exception w)
                {
                    TextBox2.Text = "Erro: " + w.Message;
                }
                conecNueva.Close();
                conecNueva.Dispose();
        
            }
            else
            {
                TextBox2.Text = m;
            }

        }
    }
}