using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//aqui mis import
using ClassMiAccesoSQL;
using System.Data.SqlClient;
using System.Data;

namespace WebPruebaAccesoSQL
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        ClassAccesoSQL objacc2 = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack == false)
            {

                /* conexión cubículo
                  @"Data Source=DESKTOP-OTBVNUJ\SQLEXPRESS_2017; Initial Catalog=BDTIENDA; Integrated Security = true;");
                
                conexión casa

                 @"Data Source=DESKTOP-0J2HDN7\SQLEXPRESS2017; Initial Catalog=BDTIENDA; Integrated Security = true;");
                 */
                objacc2 = new ClassAccesoSQL(
                    @"Data Source=DESKTOP-H5Q2S4F\MSSQLSERVER01; Initial Catalog=BDTIENDA; Integrated Security = true;");
                Session["objacc2"] = objacc2;
                SqlConnection conectTres = null;
                string query = "select * from Empleado";
                string m = "";
                conectTres = objacc2.AbrirConexion(ref m);
                SqlDataReader atrapa = null;
                atrapa = objacc2.ConsultarReader(query, conectTres, ref m);

                DropDownList1.Items.Clear();
                if(atrapa != null)
                {
                    while(atrapa.Read())
                    {
                        DropDownList1.Items.Add(new ListItem(atrapa[1].ToString(),
                            atrapa[0].ToString()));
                    }
                    conectTres.Close();
                    conectTres.Dispose();
                }
                else
                {
                    TextBox3.Text = "Ocurrio un error en la consulta de empleados";
                }
            }
            else
            {
                objacc2 = (ClassAccesoSQL)Session["objacc2"];
            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {

          
            //para insertar un ticket debo de hacer primero un arreglos de 
            //paramatros
            SqlParameter[] parametros = new SqlParameter[3];

            DateTime fechahora = DateTime.Now;

            //paramatro del id
            parametros[0] = new SqlParameter
            {
                ParameterName = "id",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Input,
                Value = TextBox1.Text

            };
            //parametro de la llave foranea de empleado
            parametros[1] = new SqlParameter
            {
                ParameterName = "femp",
                SqlDbType = System.Data.SqlDbType.Int,
                //tambien es de entrada
                Direction = System.Data.ParameterDirection.Input,
                Value = DropDownList1.SelectedValue

            };
            //parametro de la hora
            parametros[2] = new SqlParameter
            {
                ParameterName = "fechahora",
                SqlDbType = System.Data.SqlDbType.DateTime,
                //tambien es de entrada
                Direction = System.Data.ParameterDirection.Input,
                Value = fechahora

            };

            string sentencia = "insert into ticket values(@id,@femp,@fechahora);";
            string m = "";
            objacc2.ModificaBDunPocoMasSegura(sentencia, objacc2.AbrirConexion(ref m),
                ref m, parametros);
            TextBox3.Text = m;
       
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TextBox3.Text = "texto seleccionado: " + DropDownList1.SelectedItem +
            //    "Valor item: " + DropDownList1.SelectedValue + 
            //    "Posición item: " + DropDownList1.SelectedIndex;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            DataSet caja = null;
            string consulta = "select ID_NUMERO as ticket, NOMBRE as Empleado, " +
                "FECHACOMPRA from TICKET, EMPLEADO where FKEMPLEADO = ID_EMPLEADO; ";

            string h = "";
            caja = objacc2.ConsultaDS(consulta, objacc2.AbrirConexion(ref h), ref h);
            if (caja != null)
            {
                GridView1.DataSource = caja.Tables[0];
                GridView1.DataBind();
            }
            else
            {
                TextBox3.Text = h;
            }
        }
    }
}