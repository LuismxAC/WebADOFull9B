using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ClassMiAccesoSQL;
using System.Data.SqlClient;
using System.Data;

using System.Text;

namespace WebPruebaAccesoSQL
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        ClassAccesoSQL objacc = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if(IsPostBack==false)
            {

                /* conexión cubículo
                  @"Data Source=DESKTOP-OTBVNUJ\SQLEXPRESS_2017; Initial Catalog=BDTIENDA; Integrated Security = true;");
                
                conexión casa

                 @"Data Source=DESKTOP-0J2HDN7\SQLEXPRESS2017; Initial Catalog=BDTIENDA; Integrated Security = true;");
                 */
                objacc = new ClassAccesoSQL(
                    @"Data Source=DESKTOP-H5Q2S4F\MSSQLSERVER01; Initial Catalog=BDTIENDA; Integrated Security = true;");
                Session["objacc"] = objacc;
            }
            else
            {
                objacc = (ClassAccesoSQL) Session["objacc"];
            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            SqlConnection temp = null;
            string mensaje = "";
            string limpia = "";
            temp = objacc.AbrirConexion(ref mensaje);
            TextBox1.Text = mensaje;
           // limpia = QuitaComillas(mensaje);
            TextBox2.Text = limpia;
            if(temp!=null)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(),
                    "messg3B", "msgbox3('Correcto','" + mensaje + " ','success');",
                    true);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(),
                   "messg3B", "msgbox3(`Peligro`,`" + mensaje + "`,`error`);",
                   true);

            }                      
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            SqlDataReader caja = null;
            SqlConnection cnab = null;
            string m = "";
            cnab = objacc.AbrirConexion(ref m);
            if (cnab != null)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(),
                    "messg3B", "msgbox3('Correcto','" + m + " ','success');",
                    true);
                caja = objacc.ConsultarReader("select * from EMPLEADO", cnab, ref m);
                if(caja!=null)
                {
                    //la consulta es correcta y se muestran los datos
                    
                    ListBox1.Items.Clear();
                    while(caja.Read())
                    {
                        ListBox1.Items.Add(caja[0] + " " + caja["NOMBRE"]);
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(),
                   "messg77", "msgbox3(`Peligro`,`" + m + "`,`error`);",
                   true);
                }

                cnab.Close();
                cnab.Dispose();                    


            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(),
                   "messg88", "msgbox3(`Peligro`,`" + m + "`,`error`);",
                   true);

            }


        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            DataSet contenedor = null;

            SqlConnection cnab = null;
            string m = "";
            cnab = objacc.AbrirConexion(ref m);
            if (cnab != null)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(),
                    "messg3B", "msgbox3('Correcto','" + m + " ','success');",
                    true);
                contenedor = objacc.ConsultaDS ("select * from EMPLEADO", cnab, ref m);
                cnab.Close();
                cnab.Dispose();
                if (contenedor != null)
                {

                    //la consulta es correcta y se muestran los datos
                    GridView1.DataSource = contenedor.Tables[0];
                    GridView1.DataBind();
                    Session["Tabla1"] = contenedor;
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(),
                   "messg77", "msgbox3(`Peligro`,`" + m + "`,`error`);",
                   true);
                }             

            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(),
                   "messg88", "msgbox3(`Peligro`,`" + m + "`,`error`);",
                   true);

            }

        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            DataSet t = Session["Tabla1"] as DataSet;
            ListBox1.Items.Clear();
            ListBox1.Items.Add("Datos recuperados del dataTable 0");
            DataRow registro = null;
            for (int w=t.Tables[0].Rows.Count-1; w>=0; w--)            
            {
                registro = t.Tables[0].Rows[w];
                ListBox1.Items.Add(registro[1] + " -- " + registro[0]);

            }
        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            string sentencia = "Insert into empleado values(" +
                txtid.Text + ",'" + txtnom.Text + "');";
            SqlConnection t = null;
            string m = "";
            Boolean resp = false;
            t = objacc.AbrirConexion(ref m);
            resp=objacc.ModificaBDinsegura(sentencia, t, ref m);
            if(resp)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(),
                    "messg3B5", "msgbox3('Correcto','" + m + " ','success');",
                    true);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(),
                   "messg885", "msgbox3(`Peligro`,`" + m + "`,`error`);",
                   true);
            }
            TextBox2.Text = sentencia;

        }

        protected void Button6_Click(object sender, EventArgs e)
        {
            // declaración de parámetros
            SqlParameter first = new SqlParameter("id", SqlDbType.Int);
            SqlParameter second = new SqlParameter("nombre", SqlDbType.NVarChar, 50);

            //dando valores a los parámetros
            first.Value = txtid.Text;
            second.Value = txtnom.Text;

            //establecer la dirección de los parámetros
            first.Direction = ParameterDirection.Input;
            second.Direction = ParameterDirection.Input;




            string sentencia = "Insert Into empleado values(@id, @nombre);";
            SqlConnection conexion = null;
            string mensaje = "";
            Boolean resp = false;
            conexion = objacc.AbrirConexion(ref mensaje);

            resp = objacc.InsertaEmpleadoconPar(sentencia, conexion, ref mensaje,
                first, second);

            if (resp)
            {

                this.ClientScript.RegisterStartupScript(this.GetType(),
                   "msgModificacion1", "msgbox3(`Correcto`,`" + mensaje + "`, `success`)", true);
            }
            else
            {
                this.ClientScript.RegisterStartupScript(this.GetType(),
                    "msgModificacion2", "msgbox3(`Peligro`,`"+ mensaje + "`, `error`)", true);
            }
        }

        protected void Button7_Click(object sender, EventArgs e)
        {
            SqlParameter[] misparametros = new SqlParameter[4];

            misparametros[0] = new SqlParameter("Idprod", SqlDbType.Int);
            misparametros[0].Value = Txtidprod.Text;
            misparametros[0].Direction = ParameterDirection.Input;

            misparametros[1] = new SqlParameter
            {
                ParameterName = "Descri",
                SqlDbType = SqlDbType.NVarChar,
                Size = 50,
                Direction = ParameterDirection.Input,
                Value = txtdesc.Text
            };

            misparametros[2] = new SqlParameter
            {
                ParameterName = "Cate",
                SqlDbType = SqlDbType.NVarChar,
                Size = 15,
                Direction = ParameterDirection.Input,
                Value = txtcatego.Text
            };
            misparametros[3] = new SqlParameter
            {
                ParameterName = "Precio",
                SqlDbType = SqlDbType.Float,
                Direction = ParameterDirection.Input,
                Value = txtprecio.Text
            };

            string sentencia = "Insert Into Productos values(@Idprod, @Descri, @Cate,@Precio);";
            SqlConnection conexion = null;
            string mensaje = "";
            Boolean resp = false;
            conexion = objacc.AbrirConexion(ref mensaje);

            resp = objacc.ModificaBDunPocoMasSegura(sentencia, conexion,
                ref mensaje, misparametros);


            if (resp)
            {

                this.ClientScript.RegisterStartupScript(this.GetType(),
                    "msgModificacion3", "msgbox3(`Correcto`,`" + mensaje + "`, `success`)", true);
            }
            else
            {
                this.ClientScript.RegisterStartupScript(this.GetType(),
                     "msgModificacion4", "msgbox3(`Peligro`,`" + mensaje + "`, `error`)", true);
            }
        }

        protected void Button9_Click(object sender, EventArgs e)
        {
            string query = "select  * from PRODUCTOS";
            DataSet caja = null;
            string h = "";
            caja = objacc.ConsultaDS(query, objacc.AbrirConexion(ref h),
                ref h);
            if(caja!=null)
            {
                GridView2.DataSource = caja.Tables[0];
                GridView2.DataBind();
            }
            else
            {
                this.ClientScript.RegisterStartupScript(this.GetType(),
                     "msgModificacion7", "msgbox3(`Peligro`,`" + h + "`, `error`)", true);
            }
        }

        protected void Button10_Click(object sender, EventArgs e)
        {
            DataSet t = Session["Tabla1"] as DataSet;
            DataTable tabla = t.Tables[0];
            DataRow ractual = null;
            int r = 0;
            
            for(r=0; r<= tabla.Rows.Count-1;r++)
            {
                ractual = tabla.Rows[r];
                if(ractual["ID_EMPLEADO"].ToString()== txtidbuscar.Text)
                {
                    ractual["NOMBRE"] = txtnommod.Text;
                }
            }
        }

        protected void Button11_Click(object sender, EventArgs e)
        {
            Response.Redirect("WebForm2.aspx");
        }
    }
}

