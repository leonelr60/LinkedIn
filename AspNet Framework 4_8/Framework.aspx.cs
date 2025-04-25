using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using waFichaClinica.Models;
using System.Data;
using System.Globalization;

namespace waFichaClinica.WForms
{
    public partial class Citas : System.Web.UI.Page
    {
        public int cod_pac = 0;

        protected void Traduce_pagina()
        {
            int icont = 0;
            string sSql = "";
            string sObject = "";
            sSql = sSql + "exec multilanguage_converter";
            sSql = sSql + " '" + User.Identity.Name + "'";
            sSql = sSql + ", 'Citas'";
            sdsSQL.SelectCommand = sSql;
            DataView dv = (DataView)sdsSQL.Select(DataSourceSelectArguments.Empty);
            DataTable dt = dv.ToTable();
            if (dt.Rows.Count > 0)
            {
                while (icont <= dt.Rows.Count - 1)
                {

                    sObject = dt.Rows[icont].ItemArray[0].ToString();
                    try
                    {
                        ContentPlaceHolder content = (ContentPlaceHolder)Master.FindControl("MainContent");
                        if (sObject.Substring(0, 3) == "lbl" || sObject.Substring(0, 3) == "lab")
                        {
                            Label oObject = content.FindControl(sObject) as Label;
                            oObject.Text = dt.Rows[icont].ItemArray[1].ToString();

                        }
                        if (sObject.Substring(0, 3) == "lbt")
                        {
                            LinkButton oObject = content.FindControl(sObject) as LinkButton;
                            oObject.Text = dt.Rows[icont].ItemArray[1].ToString();

                        }
                        if (sObject.Substring(0, 3) == "chk")
                        {
                            CheckBox oObject = content.FindControl(sObject) as CheckBox;
                            oObject.Text = dt.Rows[icont].ItemArray[1].ToString();
                        }
                        if (sObject.Substring(0, 3) == "btn")
                        {
                            Button oObject = content.FindControl(sObject) as Button;
                            oObject.Text = dt.Rows[icont].ItemArray[1].ToString();

                        }
                    }
                    catch (Exception e)
                    {
                        string sMsg = e.Message;
                    }

                    icont = icont + 1;
                }

            }
        }

        protected void Verifica_roles()
        {
            string sSql = "";
            sSql = sSql + "SELECT sr.Object";
            sSql = sSql + " FROM AspNetUserRoles r";
            sSql = sSql + " INNER JOIN AspNetSubRoles sr ON sr.Id = r.RoleId";
            sSql = sSql + " INNER JOIN AspNetUsers u ON u.Id = r.UserId";
            sSql = sSql + " WHERE u.UserName = '" + User.Identity.Name + "'";
            sSql = sSql + " AND sr.Form = 'DEFAULT'";
            sSql = sSql + " AND sr.Object = 'tblCitas'";
            sdsSQL.SelectCommand = sSql;
            DataView dv = (DataView)sdsSQL.Select(DataSourceSelectArguments.Empty);
            DataTable dt = dv.ToTable();
            if (dt.Rows.Count <= 0)
            {
                Response.Redirect("/Default");

            }
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (IsPostBack)
                {

                }
                else
                {

                    gvPacientes.Visible = false;
                    cod_pac = 0;

                    ClrCitas.Visible = false;
                    lblHora.Visible = false;
                    txtApellidos.ToolTip = "";
                    btnRecuperar.Visible = false;
                    ddlHora.Visible = false;
                    ClrCitas.Style.Value = "display:none";
                    tdCalendar.Style.Value = "width:0%;vertical-align:top";
                    tdWeek.Style.Value = "width:100%";
                    tdDatosHora.Style.Value = "width:0%";
                    tdDatosHora2.Style.Value = "width:100%";
                    ClrCitas.SelectedDate = DateTime.Parse(DateTime.Today.ToShortDateString());
                    sdsHorario.DataBind();
                    ddlHora.DataBind();
                    Verifica_roles();
                    string sSql = "";
                    sSql = sSql + "SELECT anua.UserName";
                    sSql = sSql + " FROM AspNetUsers anu inner join AspNetUserAccount anua on anua.UserId = anu.Id ";
                    sSql = sSql + " WHERE anu.USERNAME = '" + User.Identity.Name.ToUpper() + "'";
                    sdsSQL.SelectCommand = sSql;
                    DataView dv = (DataView)sdsSQL.Select(DataSourceSelectArguments.Empty);
                    DataTable dt = dv.ToTable();
                    if (dt.Rows.Count > 0)
                    {
                        txtUser.Text = dt.Rows[0].ItemArray[0].ToString();
                    }
                    //Selecciona la Semana Actual
                    string sDay = DateTime.Parse(DateTime.Today.ToShortDateString()).DayOfWeek.ToString();
                    int iDay = 0;
                    if(sDay == "Sunday")
                    {
                        iDay = 0;
                    }
                    if (sDay == "Monday")
                    {
                        iDay = 1;
                    }
                    if (sDay == "Tuesday")
                    {
                        iDay = 2;
                    }
                    if (sDay == "Wednesday")
                    {
                        iDay = 3;
                    }
                    if (sDay == "Thursday")
                    {
                        iDay = 4;
                    }
                    if (sDay == "Friday")
                    {
                        iDay = 5;
                    }
                    if (sDay == "Saturday")
                    {
                        iDay = 6;
                    }
                    iDay = iDay * -1;
                    string sFecini = DateTime.Today.AddDays(double.Parse(iDay.ToString())).ToShortDateString();
                    string sFecFin = DateTime.Parse(sFecini).AddDays(6).ToShortDateString();
                    txtFecIni.Text = sFecini;
                    txtFecFin.Text = sFecFin;
                    lblLeyenda.Text = "" + txtFecIni.Text + " - " + txtFecFin.Text;
                    lblLeyenda.Font.Bold = true;
                    
                    sSql = " EXEC wsp_citas_x_semana '" + DateTime.Parse(txtFecIni.Text).ToString("yyyyMMdd") + "', '" + DateTime.Parse(txtFecFin.Text).ToString("yyyyMMdd") + "', ";
                    sSql = sSql + " '" + txtUser.Text.ToString() + "'";
                    sdsSQL.SelectCommand = sSql;
                    DataView dv1 = (DataView)sdsSQL.Select(DataSourceSelectArguments.Empty);
                    DataTable dt1 = dv1.ToTable();
                    int i = 0;
                    sdsCitasxSemana.DataBind();
                    gvSemana.DataBind();
                    Cambiardiassemana();
                    sdsCitasxSemana.DataBind();
                    gvSemana.DataBind();
                    gvSemana.Visible = true;
                    for (i = 0; i <= gvSemana.Rows.Count - 1; i++)
                    {
                        GridViewRow row = gvSemana.Rows[i];
                        for (int j = 0; j <= gvSemana.Columns.Count - 1; j++)
                        {
                            gvSemana.Rows[i].Cells[j].ToolTip = DateTime.Parse(txtFecIni.Text).AddDays(j).DayOfWeek + " " + DateTime.Parse(txtFecIni.Text).AddDays(j).Day.ToString();
                            gvSemana.Rows[i].Cells[j].BackColor = System.Drawing.Color.White;
                            gvSemana.Rows[i].Cells[j].ForeColor = System.Drawing.Color.Black;
                            LinkButton btn = (LinkButton)row.Cells[j].Controls[0];
                            if (btn.Text.Contains("AVAILABLE"))
                            {

                            }
                            else
                            {
                                gvSemana.Rows[i].Cells[j].BackColor = System.Drawing.Color.DarkRed;
                                gvSemana.Rows[i].Cells[j].ForeColor = System.Drawing.Color.White;
                            }
                        }
                    }
                    Pintar_examenes_clinicos();
                    btnAtras.Height = gvSemana.Height;
                    btnAdelante.Height = gvSemana.Height;
                    txtApellidos.Attributes.Add("onfocus", "this.select();");
                    txtNombres.Attributes.Add("onfocus", "this.select();");
                    Traduce_pagina();
                    txtApellidos.Focus();
                }
            }
            else
            {
                Response.Redirect("/Account/Login");
            }
        }

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            gvPacientes.Visible = true;
            sdsBusquedaNombre.DataBind();
            gvPacientes.DataBind();
            sdsHistCitas.DataBind();
            gvHistCitas.DataBind();
        }

        protected void Limpiar()
        {

            txtApellidos.Text = "";
            txtAfiliacion.Text = "";
            txtDir.Text = "";
            txtEmail.Text = "";
            txtFecNac.Text = "";
            
            txtNombres.Text = "";
            txtPersonaEncargada.Text = "";
            txtReferido.Text = "";
            txtRelacion.Text = "";
            txtTelCasa.Text = "";
            txtTelCasaPE.Text = "";
            txtTelCel.Text = "";
            txtObsCita.Text = "";
            txtTelCelPE.Text = "";
            gvPacientes.Visible = false;
            cod_pac = 0;
            txtApellidos.ToolTip = "";
            btnGuardar.Enabled = true;
            Limpiar_bold();

            sdsHorario.DataBind();
            ddlHora.DataBind();
            Pintar_examenes_clinicos();
            sdsHistCitas.DataBind();
            gvHistCitas.DataBind();
            txtApellidos.Focus();
        }

        protected void Limpiar_bold()
        {
            int i = 0;

            for (i = 0; i <= gvSemana.Rows.Count - 1; i++)
            {
                GridViewRow row = gvSemana.Rows[i];
                for (int j = 0; j <= gvSemana.Columns.Count - 1; j++)
                {
                    LinkButton btn = (LinkButton)row.Cells[j].Controls[0];
                    btn.Font.Bold = false;

                }
            }
        }

        protected void Cambiardiassemana()
        {
            if(gvSemana.Rows.Count > 0)
            {
                GridViewRow row = gvSemana.Rows[0];
                for (int j = 0; j <= gvSemana.Columns.Count - 1; j++)
                {
                    if (j == 0)
                    {
                        gvSemana.Columns[j].HeaderText = "Sunday " + DateTime.Parse(txtFecIni.Text.ToString()).ToString("MMM d");
                    }
                    if (j == 1)
                    {
                        gvSemana.Columns[j].HeaderText = "Monday " + DateTime.Parse(txtFecIni.Text.ToString()).AddDays(j).ToString("MMM d");
                    }
                    if (j == 2)
                    {
                        gvSemana.Columns[j].HeaderText = "Tuesday " + DateTime.Parse(txtFecIni.Text.ToString()).AddDays(j).ToString("MMM d");
                    }
                    if (j == 3)
                    {
                        gvSemana.Columns[j].HeaderText = "Wednesday " + DateTime.Parse(txtFecIni.Text.ToString()).AddDays(j).ToString("MMM d");
                    }
                    if (j == 4)
                    {
                        gvSemana.Columns[j].HeaderText = "Thursday " + DateTime.Parse(txtFecIni.Text.ToString()).AddDays(j).ToString("MMM d");
                    }
                    if (j == 5)
                    {
                        gvSemana.Columns[j].HeaderText = "Friday " + DateTime.Parse(txtFecIni.Text.ToString()).AddDays(j).ToString("MMM d");
                    }
                    if (j == 6)
                    {
                        gvSemana.Columns[j].HeaderText = "Saturday " + DateTime.Parse(txtFecIni.Text.ToString()).AddDays(j).ToString("MMM d");
                    }
                }
            }
            
        }

        protected void Pintar_examenes_clinicos()
        {
            int i = 0;
            int iRow = 0;

            //string[] aColores;
            string sSql = "SELECT le.txt_examen, lc.txt_color, lc.txt_fore FROM lconf_citas_colores lc INNER JOIN lexamen_clinico le ON le.cod_usuario = lc.cod_usuario and le.cod_examen = lc.cod_examen WHERE lc.cod_usuario = '" + txtUser.Text + "'";
            sdsSQL.SelectCommand = sSql;
            DataView dv1 = (DataView)sdsSQL.Select(DataSourceSelectArguments.Empty);
            DataTable dt1 = dv1.ToTable();
            //aColores = new string[dt1.Rows.Count - 1];
            if (dt1.Rows.Count > 0)
            {
                for(iRow = 0; iRow <= dt1.Rows.Count -1; iRow ++)
                {
                    string sVal = "[" + dt1.Rows[iRow].ItemArray[0].ToString() + "]";
                    string sColor = dt1.Rows[iRow].ItemArray[1].ToString();
                    string sColorFore = dt1.Rows[iRow].ItemArray[2].ToString();
                    for (i = 0; i <= gvSemana.Rows.Count - 1; i++)
                    {
                        GridViewRow row = gvSemana.Rows[i];
                        for (int j = 0; j <= gvSemana.Columns.Count - 1; j++)
                        {
                            LinkButton btn = (LinkButton)row.Cells[j].Controls[0];

                            if (btn.Text.Contains(sVal))
                            {
                                row.Cells[j].BackColor = System.Drawing.Color.FromName(sColor);
                                btn.ForeColor = System.Drawing.Color.FromName(sColorFore);
                                
                            }
                        }
                    }
                }
                

            }



            
        }

        protected void DdlPais_SelectedIndexChanged(object sender, EventArgs e)
        {
            sdsDeptos.DataBind();
            ddlDepto.DataBind();
        }

        protected void DdlDepto_SelectedIndexChanged(object sender, EventArgs e)
        {
            sdsMunicipios.DataBind();
            ddlMunicipio.DataBind();

        }


        protected void BtnLimpiar_Click(object sender, EventArgs e)
        {
            btnCrear.Enabled = true;
            btnCrear.Visible = true;
            btnGuardarPac.Enabled = false;
            btnGuardarPac.Visible = false;
            Limpiar();
        }

        protected void GvPacientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvPacientes.Visible = false;
            
            txtFecNac.Text = "";
            txtDir.Text = "";
            txtTelCel.Text = "";
            txtTelCasa.Text = "";
            txtEmail.Text = "";
            txtPersonaEncargada.Text = "";
            txtRelacion.Text = "";
            txtTelCelPE.Text = "";
            txtTelCasaPE.Text = "";
            txtReferido.Text = "";
            txtUser.ToolTip = "";
            txtAfiliacion.Text = "";
            txtObsCita.Text = "";
            gvPacientes.Visible = false;
            cod_pac = int.Parse(gvPacientes.SelectedRow.Cells[1].Text);
            txtApellidos.ToolTip = cod_pac.ToString();
            txtApellidos.Text = gvPacientes.SelectedRow.Cells[2].Text;
            txtNombres.Text = gvPacientes.SelectedRow.Cells[3].Text;
            string sSql = "";
            sSql = sSql + "SELECT lp.*";
            sSql = sSql + " FROM lpacientes lp";
            sSql = sSql + " WHERE lp.cod_paciente = " + cod_pac.ToString();
            sSql = sSql + " AND cod_usuario = '" + txtUser.Text.ToString() + "'";
            sdsSQL.SelectCommand = sSql;
            DataView dv = (DataView)sdsSQL.Select(DataSourceSelectArguments.Empty);
            DataTable dt = dv.ToTable();
            if (dt.Rows.Count > 0)
            {
                txtFecNac.Text = DateTime.Parse(dt.Rows[0].ItemArray[3].ToString()).ToString("yyyy-MM-dd");
                txtDir.Text = dt.Rows[0].ItemArray[4].ToString();
                ddlPais.Items.FindByValue(dt.Rows[0].ItemArray[5].ToString()).Selected = true;
                sdsDeptos.DataBind();
                ddlDepto.DataBind();
                ddlDepto.Items.FindByValue(dt.Rows[0].ItemArray[6].ToString()).Selected = true;
                sdsMunicipios.DataBind();
                ddlMunicipio.DataBind();
                ddlMunicipio.Items.FindByValue(dt.Rows[0].ItemArray[7].ToString()).Selected = true;
                txtTelCel.Text = dt.Rows[0].ItemArray[8].ToString();
                txtTelCasa.Text = dt.Rows[0].ItemArray[9].ToString();
                txtEmail.Text = dt.Rows[0].ItemArray[10].ToString();
                txtPersonaEncargada.Text = dt.Rows[0].ItemArray[11].ToString();
                txtRelacion.Text = dt.Rows[0].ItemArray[12].ToString();
                txtTelCelPE.Text = dt.Rows[0].ItemArray[13].ToString();
                txtTelCasaPE.Text = dt.Rows[0].ItemArray[14].ToString();
                txtReferido.Text = dt.Rows[0].ItemArray[15].ToString();
                txtAfiliacion.Text = dt.Rows[0].ItemArray[18].ToString();
                
            }
            sdsHistCitas.DataBind();
            gvHistCitas.DataBind();
            gvSemana.Focus();
        }

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            string sSql = "";
            sSql = "";
            bool bVal = false;
            bVal = false;
            if (txtApellidos.ToolTip.Length > 0)
            {
                //recorremos si hay citas de mas de un turno
                for (int irow = 0; irow <= gvSemana.Rows.Count - 1; irow++)
                {
                    GridViewRow row = gvSemana.Rows[irow];
                    for (int jcell = 0; jcell <= gvSemana.Columns.Count - 1; jcell++)
                    {
                        LinkButton btn = (LinkButton)row.Cells[jcell].Controls[0];
                        if (btn.Font.Bold == true)
                        {
                            
                            sdsHorario.DataBind();
                            ddlHora.DataBind();
                            string sVal4 = btn.Text;
                            if (sVal4.Contains("["))
                                sVal4 = sVal4.Substring(0, sVal4.IndexOf("[") - 1);
                            ddlHora.Items.FindByText(sVal4).Selected = true;
                            sSql = " SELECT 1 FROM citas WHERE fec_cita = '" + DateTime.Parse(ClrCitas.SelectedDate.ToShortDateString()).ToString("yyyyMMdd") + "'";
                            sSql = sSql + " AND txt_hora = '" + ddlHora.SelectedValue + "'";
                            sSql = sSql + " AND cod_usuario = '" + txtUser.Text.ToString() + "'";
                            sdsSQL.SelectCommand = sSql;
                            DataView dv = (DataView)sdsSQL.Select(DataSourceSelectArguments.Empty);
                            DataTable dt = dv.ToTable();
                            if (dt.Rows.Count > 0)
                            {
                                bVal = true;
                            }
                            else
                            {
                                bVal = false;
                            }

                            if (bVal == false)
                            {
                                sSql = "INSERT INTO citas SELECT ";
                                sSql = sSql + "'" + DateTime.Parse(ClrCitas.SelectedDate.ToShortDateString()).ToString("yyyyMMdd") + "'";
                                sSql = sSql + ", '" + ddlHora.SelectedValue + "'";
                                sSql = sSql + ", " + txtApellidos.ToolTip;
                                if (chkConfirmado.Checked == true)
                                {
                                    sSql = sSql + ", -1"; //Confirmado
                                }
                                else
                                {
                                    sSql = sSql + ", 0"; //Confirmado
                                }
                                if (chkNoVino.Checked == true)
                                {
                                    sSql = sSql + ", -1"; //No vino
                                }
                                else
                                {
                                    sSql = sSql + ", 0"; //No vino
                                }
                                if (chkCancelada.Checked == true)
                                {
                                    sSql = sSql + ", -1"; //Cancelada
                                }
                                else
                                {
                                    sSql = sSql + ", 0"; //Cancelada
                                }
                                sSql = sSql + ", 0"; // Disponible
                                sSql = sSql + ", '" + txtUser.Text.ToString() + "'";
                                sdsSQL.InsertCommand = sSql;
                                sdsSQL.Insert();

                                sSql = "INSERT INTO citas_adic SELECT ";
                                sSql = sSql + "'" + DateTime.Parse(ClrCitas.SelectedDate.ToShortDateString()).ToString("yyyyMMdd") + "'";
                                sSql = sSql + ", '" + ddlHora.SelectedValue + "'";
                                sSql = sSql + ", " + txtApellidos.ToolTip;
                                sSql = sSql + ", " + ddlMotivoCita.SelectedValue;
                                sSql = sSql + ", '" + txtUser.Text + "'";
                                sSql = sSql + ", '" + txtObsCita.Text.ToUpper() + "'";
                                if (chkMessageConf.Checked == true)
                                {
                                    sSql = sSql + ", -1"; //Confirmado
                                }
                                else
                                {
                                    sSql = sSql + ", 0"; //Confirmado
                                }
                                if (chkEmailConf.Checked == true)
                                {
                                    sSql = sSql + ", -1"; //No vino
                                }
                                else
                                {
                                    sSql = sSql + ", 0"; //No vino
                                }
                                if (chkTelConf.Checked == true)
                                {
                                    sSql = sSql + ", -1"; //Cancelada
                                }
                                else
                                {
                                    sSql = sSql + ", 0"; //Cancelada
                                }

                                sdsSQL.InsertCommand = sSql;
                                sdsSQL.Insert();

                                sSql = "INSERT INTO citas_hist SELECT ";
                                sSql = sSql + "'" + DateTime.Parse(ClrCitas.SelectedDate.ToShortDateString()).ToString("yyyyMMdd") + "'";
                                sSql = sSql + ", '" + ddlHora.SelectedValue + "'";
                                sSql = sSql + ", " + txtApellidos.ToolTip;
                                sSql = sSql + ", " + ddlMotivoCita.SelectedValue;
                                sSql = sSql + ", '" + txtUser.Text + "'";
                                sSql = sSql + ", '" + txtObsCita.Text.ToUpper() + "'";
                                sSql = sSql + ", getdate()";
                                if (chkConfirmado.Checked == true)
                                {
                                    sSql = sSql + ", -1"; //Confirmado
                                }
                                else
                                {
                                    sSql = sSql + ", 0"; //Confirmado
                                }
                                if (chkNoVino.Checked == true)
                                {
                                    sSql = sSql + ", -1"; //No vino
                                }
                                else
                                {
                                    sSql = sSql + ", 0"; //No vino
                                }
                                if (chkCancelada.Checked == true)
                                {
                                    sSql = sSql + ", -1"; //Cancelada
                                }
                                else
                                {
                                    sSql = sSql + ", 0"; //Cancelada
                                }
                                sSql = sSql + ", 0"; // Disponible
                                sdsSQL.InsertCommand = sSql;
                                sdsSQL.Insert();
                            }
                            else
                            {
                                sSql = "UPDATE citas SET cod_paciente = " + txtApellidos.ToolTip;
                                if (chkConfirmado.Checked == true)
                                {
                                    sSql = sSql + ", sn_confirmado = -1";
                                }
                                else
                                {
                                    sSql = sSql + ", sn_confirmado = 0";
                                }
                                if (chkNoVino.Checked == true)
                                {
                                    sSql = sSql + ", sn_novino = -1";
                                }
                                else
                                {
                                    sSql = sSql + ", sn_novino = 0";
                                }
                                if (chkCancelada.Checked == true)
                                {
                                    sSql = sSql + ", sn_cancelada = -1";
                                }
                                else
                                {
                                    sSql = sSql + ", sn_cancelada = 0";
                                }
                                if (chkCancelada.Checked == true || chkNoVino.Checked == true)
                                {
                                    sSql = sSql + ", sn_disponible = -1";
                                }
                                else
                                {
                                    sSql = sSql + ", sn_disponible = 0";
                                }
                                sSql = sSql + " WHERE ";
                                sSql = sSql + " txt_hora = '" + ddlHora.SelectedValue + "'";
                                sSql = sSql + " AND fec_cita = '" + DateTime.Parse(ClrCitas.SelectedDate.ToShortDateString()).ToString("yyyyMMdd") + "'";
                                sSql = sSql + " AND cod_usuario = '" + txtUser.Text.ToString() + "'";
                                sdsSQL.UpdateCommand = sSql;
                                sdsSQL.Update();

                                sSql = "UPDATE citas_adic SET cod_examen = " + ddlMotivoCita.SelectedValue;
                                sSql = sSql + ", cod_paciente = " + txtApellidos.ToolTip;
                                sSql = sSql + ", txt_obs = '" + txtObsCita.Text.ToUpper() + "'";
                                if (chkMessageConf.Checked == true)
                                {
                                    sSql = sSql + ", sn_mensaje = -1"; //Confirmado
                                }
                                else
                                {
                                    sSql = sSql + ", sn_mensaje =  0"; //Confirmado
                                }
                                if (chkEmailConf.Checked == true)
                                {
                                    sSql = sSql + ", sn_email = -1"; //No vino
                                }
                                else
                                {
                                    sSql = sSql + ", sn_email = 0"; //No vino
                                }
                                if (chkTelConf.Checked == true)
                                {
                                    sSql = sSql + ", sn_telefono = -1"; //Cancelada
                                }
                                else
                                {
                                    sSql = sSql + ", sn_telefono = 0"; //Cancelada
                                }
                                sSql = sSql + " WHERE txt_hora = '" + ddlHora.SelectedValue + "'";
                                sSql = sSql + " AND fec_cita = '" + DateTime.Parse(ClrCitas.SelectedDate.ToShortDateString()).ToString("yyyyMMdd") + "'";
                                sSql = sSql + " AND cod_usuario = '" + txtUser.Text.ToString() + "'";
                                sdsSQL.UpdateCommand = sSql;
                                sdsSQL.Update();

                                sSql = "INSERT INTO citas_hist SELECT ";
                                sSql = sSql + "'" + DateTime.Parse(ClrCitas.SelectedDate.ToShortDateString()).ToString("yyyyMMdd") + "'";
                                sSql = sSql + ", '" + ddlHora.SelectedValue + "'";
                                sSql = sSql + ", " + txtApellidos.ToolTip;
                                sSql = sSql + ", " + ddlMotivoCita.SelectedValue;
                                sSql = sSql + ", '" + txtUser.Text + "'";
                                sSql = sSql + ", '" + txtObsCita.Text.ToUpper() + "'";
                                sSql = sSql + ", getdate()";
                                if (chkConfirmado.Checked == true)
                                {
                                    sSql = sSql + ", sn_confirmado = -1";
                                }
                                else
                                {
                                    sSql = sSql + ", sn_confirmado = 0";
                                }
                                if (chkNoVino.Checked == true)
                                {
                                    sSql = sSql + ", sn_novino = -1";
                                }
                                else
                                {
                                    sSql = sSql + ", sn_novino = 0";
                                }
                                if (chkCancelada.Checked == true)
                                {
                                    sSql = sSql + ", sn_cancelada = -1";
                                }
                                else
                                {
                                    sSql = sSql + ", sn_cancelada = 0";
                                }
                                if (chkCancelada.Checked == true || chkNoVino.Checked == true)
                                {
                                    sSql = sSql + ", sn_disponible = -1";
                                }
                                else
                                {
                                    sSql = sSql + ", sn_disponible = 0";
                                }
                                sdsSQL.InsertCommand = sSql;
                                sdsSQL.Insert();
                            }
                        }
                        
                    }
                }
                

                //Actualiza Grid
                sSql = " EXEC wsp_citas_x_semana '" + DateTime.Parse(txtFecIni.Text).ToString("yyyyMMdd") + "', '" + DateTime.Parse(txtFecFin.Text).ToString("yyyyMMdd") + "', ";
                sSql = sSql + " '" + txtUser.Text.ToString() + "'";
                sdsSQL.SelectCommand = sSql;
                DataView dv1 = (DataView)sdsSQL.Select(DataSourceSelectArguments.Empty);
                DataTable dt1 = dv1.ToTable();
                int i = 0;
                sdsCitasxSemana.DataBind();
                gvSemana.DataBind();
                gvSemana.Visible = true;
                for (i = 0; i <= gvSemana.Rows.Count - 1; i++)
                {
                    GridViewRow row = gvSemana.Rows[i];
                    for (int j = 0; j <= gvSemana.Columns.Count - 1; j++)
                    {
                        gvSemana.Rows[i].Cells[j].ToolTip = DateTime.Parse(txtFecIni.Text).AddDays(j).DayOfWeek + " " + DateTime.Parse(txtFecIni.Text).AddDays(j).Day.ToString();
                        gvSemana.Rows[i].Cells[j].BackColor = System.Drawing.Color.White;
                        gvSemana.Rows[i].Cells[j].ForeColor = System.Drawing.Color.Black;
                        LinkButton btn = (LinkButton)row.Cells[j].Controls[0];
                        if (btn.Text.Contains("AVAILABLE"))
                        {

                        }
                        else
                        {
                            gvSemana.Rows[i].Cells[j].BackColor = System.Drawing.Color.DarkRed;
                            gvSemana.Rows[i].Cells[j].ForeColor = System.Drawing.Color.White;
                        }
                    }
                }

                Response.Write(@"<script language='javascript'>alert('The data has been saved.')</script>");
                Limpiar();
            }
            else
            {
                Response.Write(@"<script language='javascript'>alert('Please select a patient or create a new one.')</script>");
            }
            

        }

        protected void BtnCrear_Click(object sender, EventArgs e)
        {
            string sApellidos = txtApellidos.Text.ToUpper();
            string sNombres = txtNombres.Text.ToUpper();
            Limpiar();
            txtApellidos.Text = sApellidos;
            txtNombres.Text = sNombres;
            btnCrear.Enabled = false;
            btnCrear.Visible = false;
            btnGuardarPac.Enabled = true;
            btnGuardarPac.Visible = true;
            
            txtApellidos.Focus();
        }

        protected void BtnGuardarPac_Click(object sender, EventArgs e)
        {
            string sSql = "";
            string sID = "";
            bool bVal = false;

            if(txtFecNac.Text.Length==0)
            {
                txtFecNac.Text = "01/01/1900";
            }

            sSql = " SELECT 1 FROM lpacientes WHERE txt_apellidos = '" + txtApellidos.Text.ToUpper() + "'";
            sSql = sSql + " AND txt_nombre = '" + txtNombres.Text.ToUpper() + "'";
            sSql = sSql + " AND cod_usuario = '" + txtUser.Text.ToString() + "'";
            sdsSQL.SelectCommand = sSql;
            DataView dvv = (DataView)sdsSQL.Select(DataSourceSelectArguments.Empty);
            DataTable dtv = dvv.ToTable();
            if (dtv.Rows.Count > 0)
            {
                Response.Write(@"<script language='javascript'>alert('The name does exist, please verify.')</script>");
                return;
            }

            if (txtApellidos.ToolTip.Length == 0)
            {
                sSql = " SELECT (isnull(nro_ult,0)+1) FROM numeraciones WHERE cod_num = 'PACIENTES'";
                sSql = sSql + " AND cod_usuario = '" + txtUser.Text.ToString() + "'";
                sdsSQL.SelectCommand = sSql;
                DataView dv = (DataView)sdsSQL.Select(DataSourceSelectArguments.Empty);
                DataTable dt = dv.ToTable();
                if (dt.Rows.Count > 0)
                {
                    sID = dt.Rows[0].ItemArray[0].ToString();
                    txtApellidos.ToolTip = sID;
                    bVal = true;
                }
                else
                {
                    Response.Write(@"<script language='javascript'>alert('Error obtaining correlative.')</script>");
                    return;
                }

                sSql = " UPDATE numeraciones SET nro_ult = " + sID + " WHERE cod_num = 'PACIENTES'";
                sSql = sSql + " AND cod_usuario = '" + txtUser.Text.ToString() + "'";
                sdsSQL.UpdateCommand = sSql;
                sdsSQL.Update();
            }
            cod_pac = int.Parse(txtApellidos.ToolTip);
            if (cod_pac > 0)
            {




                if (bVal == true)
                {
                    sSql = "INSERT INTO lpacientes";
                    sSql = sSql + " SELECT ";
                    sSql = sSql + " " + cod_pac.ToString();
                    sSql = sSql + ", '" + txtApellidos.Text.ToUpper() + "'";
                    sSql = sSql + ", '" + txtNombres.Text.ToUpper() + "'";
                    sSql = sSql + ", '" + DateTime.Parse(txtFecNac.Text).ToString("yyyyMMdd") + "'";
                    sSql = sSql + ", '" + txtDir.Text.ToUpper() + "'";
                    sSql = sSql + ", " + ddlPais.SelectedValue.ToString() + "";
                    sSql = sSql + ", " + ddlDepto.SelectedValue.ToString() + "";
                    sSql = sSql + ", " + ddlMunicipio.SelectedValue.ToString() + "";
                    sSql = sSql + ", '" + txtTelCel.Text.ToUpper() + "'";
                    sSql = sSql + ", '" + txtTelCasa.Text.ToUpper() + "'";
                    sSql = sSql + ", '" + txtEmail.Text.ToUpper() + "'";
                    sSql = sSql + ", '" + txtPersonaEncargada.Text.ToUpper() + "'";
                    sSql = sSql + ", '" + txtRelacion.Text.ToUpper() + "'";
                    sSql = sSql + ", '" + txtTelCelPE.Text.ToUpper() + "'";
                    sSql = sSql + ", '" + txtTelCasaPE.Text.ToUpper() + "'";
                    sSql = sSql + ", '" + txtReferido.Text.ToUpper() + "', '', ''";
                    sSql = sSql + ", '" + txtAfiliacion.Text.ToUpper() + "'";
                    sSql = sSql + ", '" + txtUser.Text.ToString() + "'";
                    sdsSQL.InsertCommand = sSql;
                    sdsSQL.Insert();
                }
                else
                {
                    sSql = "UPDATE lpacientes";
                    sSql = sSql + " SET ";
                    sSql = sSql + " txt_apellidos = '" + txtApellidos.Text.ToUpper() + "'";
                    sSql = sSql + ", txt_nombre = '" + txtNombres.Text.ToUpper() + "'";
                    sSql = sSql + ", fec_nac = '" + DateTime.Parse(txtFecNac.Text).ToString("yyyyMMdd") + "'";
                    sSql = sSql + ", txt_dir = '" + txtDir.Text.ToUpper() + "'";
                    sSql = sSql + ", cod_pais = " + ddlPais.SelectedValue.ToString() + "";
                    sSql = sSql + ", cod_depto = " + ddlDepto.SelectedValue.ToString() + "";
                    sSql = sSql + ", cod_municipio = " + ddlMunicipio.SelectedValue.ToString() + "";
                    sSql = sSql + ", txt_tel_cel = '" + txtTelCel.Text.ToUpper() + "'";
                    sSql = sSql + ", txt_tel_casa = '" + txtTelCasa.Text.ToUpper() + "'";
                    sSql = sSql + ", txt_email = '" + txtEmail.Text.ToUpper() + "'";
                    sSql = sSql + ", txt_persona_enc = '" + txtPersonaEncargada.Text.ToUpper() + "'";
                    sSql = sSql + ", txt_relacion = '" + txtRelacion.Text.ToUpper() + "'";
                    sSql = sSql + ", txt_tel_cel_pe = '" + txtTelCelPE.Text.ToUpper() + "'";
                    sSql = sSql + ", txt_tel_casa_pe = '" + txtTelCasaPE.Text.ToUpper() + "'";
                    sSql = sSql + ", txt_referido_por = '" + txtReferido.Text.ToUpper() + "'";
                    sSql = sSql + ", txt_afiliacion = '" + txtAfiliacion.Text.ToUpper() + "'";
                    sSql = sSql + " WHERE cod_paciente = ";
                    sSql = sSql + " " + cod_pac.ToString();
                    sSql = sSql + " AND cod_usuario = '" + txtUser.Text.ToString() + "'";

                    sdsSQL.UpdateCommand = sSql;
                    sdsSQL.Update();
                }

                btnCrear.Enabled = true;
                btnCrear.Visible = true;
                btnGuardarPac.Enabled = false;
                btnGuardarPac.Visible = false;
                
                Response.Write(@"<script language='javascript'>alert('The data has been saved.')</script>");

            }     
        }

        protected void ClrCitas_SelectionChanged(object sender, EventArgs e)
        {
            if(ClrCitas.SelectedDates.Count > 1)
            {
                string sDateIni = "";
                string sDateFin = "";
                int i = 0;
                foreach (DateTime d in ClrCitas.SelectedDates)
                {

                    if (i == 0)
                    {
                        sDateIni = d.ToShortDateString();
                    }
                    else
                    {
                        sDateFin = d.ToShortDateString();
                    }
                    i = i + 1;
                }
                txtFecIni.Text = sDateIni;
                txtFecFin.Text = sDateFin;
                Cambiardiassemana();
                string sSql = " EXEC wsp_citas_x_semana '" + DateTime.Parse(txtFecIni.Text).ToString("yyyyMMdd") + "', '" + DateTime.Parse(txtFecFin.Text).ToString("yyyyMMdd") +"', ";
                sSql = sSql + " '" + txtUser.Text.ToString() + "'";
                sdsSQL.SelectCommand = sSql;
                DataView dv = (DataView)sdsSQL.Select(DataSourceSelectArguments.Empty);
                DataTable dt = dv.ToTable();
                
                sdsCitasxSemana.DataBind();
                gvSemana.DataBind();
                gvSemana.Visible = true;
                for (i = 0; i <= gvSemana.Rows.Count -1; i++)
                {
                    GridViewRow row = gvSemana.Rows[i];
                    for (int j = 0; j <= gvSemana.Columns.Count -1; j++)
                    {
                        gvSemana.Rows[i].Cells[j].ToolTip = DateTime.Parse(txtFecIni.Text).AddDays(j).DayOfWeek + " " + DateTime.Parse(txtFecIni.Text).AddDays(j).Day.ToString();
                        gvSemana.Rows[i].Cells[j].BackColor = System.Drawing.Color.White;
                        gvSemana.Rows[i].Cells[j].ForeColor = System.Drawing.Color.Black;
                        LinkButton btn = (LinkButton)row.Cells[j].Controls[0];
                        if (btn.Text.Contains("AVAILABLE"))
                        {

                        }
                        else
                        {
                            gvSemana.Rows[i].Cells[j].BackColor = System.Drawing.Color.DarkRed;
                            gvSemana.Rows[i].Cells[j].ForeColor = System.Drawing.Color.White;
                        }
                    }
                }
                Limpiar_bold();
                Pintar_examenes_clinicos();
                lblLeyenda.Text = "" + txtFecIni.Text + " - " + txtFecFin.Text;
                gvSemana.Focus();
            }
            else
            {
                Limpiar_bold();
                Pintar_examenes_clinicos();
                sdsHorario.DataBind();
                ddlHora.DataBind();
            }
            
        }

        protected void RecuperarCita()
        {
            string sSql = "";
            sSql = "";
            bool bVal = false;
            bVal = false;
            string sHora = "";
            sHora = ddlHora.SelectedValue;

            sSql = " SELECT cod_paciente,sn_confirmado, sn_novino, sn_cancelada FROM citas WHERE fec_cita = '" + DateTime.Parse(ClrCitas.SelectedDate.ToShortDateString()).ToString("yyyyMMdd") + "'";
            sSql = sSql + " AND txt_hora = '" + ddlHora.SelectedValue + "'";
            sSql = sSql + " AND cod_usuario = '" + txtUser.Text.ToString() + "'";
            sdsSQL.SelectCommand = sSql;
            DataView dv = (DataView)sdsSQL.Select(DataSourceSelectArguments.Empty);
            DataTable dt = dv.ToTable();
            if (dt.Rows.Count > 0)
            {
                bVal = true;
            }
            else
            {
                bVal = false;
            }

            if (bVal == true)
            {
                txtApellidos.ToolTip = dt.Rows[0].ItemArray[0].ToString();
                cod_pac = int.Parse(dt.Rows[0].ItemArray[0].ToString());
                if (dt.Rows[0].ItemArray[1].ToString() == "-1")
                {
                    chkConfirmado.Checked = true;
                }
                else
                {
                    chkConfirmado.Checked = false;
                }
                if (dt.Rows[0].ItemArray[2].ToString() == "-1")
                {
                    chkNoVino.Checked = true;
                }
                else
                {
                    chkNoVino.Checked = false;
                }
                if (dt.Rows[0].ItemArray[3].ToString() == "-1")
                {
                    chkCancelada.Checked = true;
                }
                else
                {
                    chkCancelada.Checked = false;
                }
                gvPacientes.Visible = false;
                txtObsCita.Text = "";
                sSql = " SELECT cod_examen, txt_obs, sn_mensaje, sn_email, sn_telefono FROM citas_adic WHERE fec_cita = '" + DateTime.Parse(ClrCitas.SelectedDate.ToShortDateString()).ToString("yyyyMMdd") + "'";
                sSql = sSql + " AND txt_hora = '" + ddlHora.SelectedValue + "'";
                sSql = sSql + " AND cod_usuario = '" + txtUser.Text.ToString() + "'";
                sdsSQL.SelectCommand = sSql;
                DataView dva = (DataView)sdsSQL.Select(DataSourceSelectArguments.Empty);
                DataTable dta = dva.ToTable();
                if (dta.Rows.Count > 0)
                {
                    ddlMotivoCita.DataBind();
                    ddlMotivoCita.Items.FindByValue(dta.Rows[0].ItemArray[0].ToString()).Selected = true;
                    txtObsCita.Text = dta.Rows[0].ItemArray[1].ToString();
                    if (dta.Rows[0].ItemArray[2].ToString() == "-1")
                    {
                        chkMessageConf.Checked = true;
                    }
                    else
                    {
                        chkMessageConf.Checked = false;
                    }
                    if (dta.Rows[0].ItemArray[3].ToString() == "-1")
                    {
                        chkEmailConf.Checked = true;
                    }
                    else
                    {
                        chkEmailConf.Checked = false;
                    }
                    if (dta.Rows[0].ItemArray[4].ToString() == "-1")
                    {
                        chkTelConf.Checked = true;
                    }
                    else
                    {
                        chkTelConf.Checked = false;
                    }
                }

                sSql = "";
                sSql = sSql + "SELECT lp.*";
                sSql = sSql + " FROM lpacientes lp";
                sSql = sSql + " WHERE lp.cod_paciente = " + cod_pac.ToString();
                sSql = sSql + " AND cod_usuario = '" + txtUser.Text.ToString() + "'";
                sdsSQL.SelectCommand = sSql;
                DataView dv2 = (DataView)sdsSQL.Select(DataSourceSelectArguments.Empty);
                DataTable dt2 = dv2.ToTable();
                if (dt2.Rows.Count > 0)
                {
                    txtApellidos.Text = dt2.Rows[0].ItemArray[1].ToString();
                    txtNombres.Text = dt2.Rows[0].ItemArray[2].ToString();
                    txtFecNac.Text = DateTime.Parse(dt2.Rows[0].ItemArray[3].ToString()).ToString("yyyy-MM-dd");
                    txtDir.Text = dt2.Rows[0].ItemArray[4].ToString();
                    ddlPais.Items.FindByValue(dt2.Rows[0].ItemArray[5].ToString()).Selected = true;
                    sdsDeptos.DataBind();
                    ddlDepto.DataBind();
                    ddlDepto.Items.FindByValue(dt2.Rows[0].ItemArray[6].ToString()).Selected = true;
                    sdsMunicipios.DataBind();
                    ddlMunicipio.DataBind();
                    ddlMunicipio.Items.FindByValue(dt2.Rows[0].ItemArray[7].ToString()).Selected = true;
                    txtTelCel.Text = dt2.Rows[0].ItemArray[8].ToString();
                    txtTelCasa.Text = dt2.Rows[0].ItemArray[9].ToString();
                    txtEmail.Text = dt2.Rows[0].ItemArray[10].ToString();
                    txtPersonaEncargada.Text = dt2.Rows[0].ItemArray[11].ToString();
                    txtRelacion.Text = dt2.Rows[0].ItemArray[12].ToString();
                    txtTelCelPE.Text = dt2.Rows[0].ItemArray[13].ToString();
                    txtTelCasaPE.Text = dt2.Rows[0].ItemArray[14].ToString();
                    txtReferido.Text = dt2.Rows[0].ItemArray[15].ToString();
                    txtAfiliacion.Text = dt2.Rows[0].ItemArray[18].ToString();
                    ddlHora.Items.FindByValue(sHora).Selected = true;
                }
            }
            
        }

        protected void BtnRecuperar_Click(object sender, EventArgs e)
        {
            RecuperarCita();
            
        }

        
        protected void GvSemana_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gv, "Select$" + row.RowIndex.ToString(), true));





        }

        protected void GvSemana_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string commandName = e.CommandName.ToString().Trim();
            GridViewRow row = gvSemana.Rows[Convert.ToInt32(e.CommandArgument)];
            string sToolTip = "";
            Boolean bRecuperar = false;
            Boolean bPrimerSeleccionPaciente = false;
            string sValAnterior = "-999";
            if(txtApellidos.ToolTip.Length == 0)
            {
                bPrimerSeleccionPaciente = true;

            }
            if(txtApellidos.ToolTip.Length > 0)
            {
                sValAnterior = txtApellidos.ToolTip;
            }
            bRecuperar = false;
            switch (commandName)
            {
                case "Sunday":
                    LinkButton btn = (LinkButton)row.Cells[0].Controls[0];
                    sToolTip = DateTime.Parse(txtFecIni.Text).AddDays(0).ToShortDateString();
                    if(btn.Font.Bold == false)
                    {
                        btn.Font.Bold = true;
                    }
                    else
                    {
                        btn.Font.Bold = false;
                    }
                    
                    ClrCitas.SelectedDate = DateTime.Parse(sToolTip);
                    ClrCitas.DataBind();
                    sdsHorario.DataBind();
                    ddlHora.DataBind();
                    string sVal = btn.Text;
                    
                        if (sVal.Contains("["))
                        {
                            sVal = sVal.Substring(0, sVal.IndexOf("[") - 1);
                            bRecuperar = true;
                        }
                        
                    
                    ddlHora.Items.FindByText(sVal).Selected = true;
                    break;
                case "Monday":
                    LinkButton btn1 = (LinkButton)row.Cells[1].Controls[0];
                    sToolTip = DateTime.Parse(txtFecIni.Text).AddDays(1).ToShortDateString();
                    if (btn1.Font.Bold == false)
                    {
                        btn1.Font.Bold = true;
                    }
                    else
                    {
                        btn1.Font.Bold = false;
                    }
                    ClrCitas.SelectedDate = DateTime.Parse(sToolTip);
                    ClrCitas.DataBind();
                    sdsHorario.DataBind();
                    ddlHora.DataBind();
                    string sVal1 = btn1.Text;
                        if (sVal1.Contains("["))
                        {
                            sVal1 = sVal1.Substring(0, sVal1.IndexOf("[") - 1);
                            bRecuperar = true;
                        }
                    
                    ddlHora.Items.FindByText(sVal1).Selected = true;
                    break;
                case "Tuesday":
                    LinkButton btn2 = (LinkButton)row.Cells[2].Controls[0];
                    sToolTip = DateTime.Parse(txtFecIni.Text).AddDays(2).ToShortDateString();
                    if (btn2.Font.Bold == false)
                    {
                        btn2.Font.Bold = true;
                    }
                    else
                    {
                        btn2.Font.Bold = false;
                    }
                    ClrCitas.SelectedDate = DateTime.Parse(sToolTip);
                    ClrCitas.DataBind();
                    sdsHorario.DataBind();
                    ddlHora.DataBind();
                    string sVal2 = btn2.Text;
                        if (sVal2.Contains("["))
                        {
                            sVal2 = sVal2.Substring(0, sVal2.IndexOf("[") - 1);
                            bRecuperar = true;
                        }
                    
                    ddlHora.Items.FindByText(sVal2).Selected = true;
                    break;
                case "Wednesday":
                    LinkButton btn3 = (LinkButton)row.Cells[3].Controls[0];
                    sToolTip = DateTime.Parse(txtFecIni.Text).AddDays(3).ToShortDateString();
                    if (btn3.Font.Bold == false)
                    {
                        btn3.Font.Bold = true;
                    }
                    else
                    {
                        btn3.Font.Bold = false;
                    }
                    ClrCitas.SelectedDate = DateTime.Parse(sToolTip);
                    ClrCitas.DataBind();
                    sdsHorario.DataBind();
                    ddlHora.DataBind();
                    string sVal3 = btn3.Text;
                        if (sVal3.Contains("["))
                        {
                            sVal3 = sVal3.Substring(0, sVal3.IndexOf("[") - 1);
                            bRecuperar = true;
                        }
                    

                    ddlHora.Items.FindByText(sVal3).Selected = true;
                    break;
                case "Thursday":
                    LinkButton btn4 = (LinkButton)row.Cells[4].Controls[0];
                    sToolTip = DateTime.Parse(txtFecIni.Text).AddDays(4).ToShortDateString();
                    if (btn4.Font.Bold == false)
                    {
                        btn4.Font.Bold = true;
                    }
                    else
                    {
                        btn4.Font.Bold = false;
                    }
                    ClrCitas.SelectedDate = DateTime.Parse(sToolTip);
                    ClrCitas.DataBind();
                    sdsHorario.DataBind();
                    ddlHora.DataBind();
                    string sVal4 = btn4.Text;
                    
                        if (sVal4.Contains("["))
                        {
                            sVal4 = sVal4.Substring(0, sVal4.IndexOf("[") - 1);
                            bRecuperar = true;
                        }
                    

                    ddlHora.Items.FindByText(sVal4).Selected = true;
                    break;
                case "Friday":
                    LinkButton btn5 = (LinkButton)row.Cells[5].Controls[0];
                    sToolTip = DateTime.Parse(txtFecIni.Text).AddDays(5).ToShortDateString();
                    if (btn5.Font.Bold == false)
                    {
                        btn5.Font.Bold = true;
                    }
                    else
                    {
                        btn5.Font.Bold = false;
                    }
                    ClrCitas.SelectedDate = DateTime.Parse(sToolTip);
                    ClrCitas.DataBind();
                    sdsHorario.DataBind();
                    ddlHora.DataBind();
                    string sVal5 = btn5.Text;
                    
                        if (sVal5.Contains("["))
                        {
                            sVal5 = sVal5.Substring(0, sVal5.IndexOf("[") - 1);
                            bRecuperar = true;
                        }
                    

                    ddlHora.Items.FindByText(sVal5).Selected = true;
                    break;
                case "Saturday":
                    LinkButton btn6 = (LinkButton)row.Cells[6].Controls[0];
                    sToolTip = DateTime.Parse(txtFecIni.Text).AddDays(6).ToShortDateString() ;
                    if (btn6.Font.Bold == false)
                    {
                        btn6.Font.Bold = true;
                    }
                    else
                    {
                        btn6.Font.Bold = false;
                    }
                    ClrCitas.SelectedDate = DateTime.Parse(sToolTip);
                    ClrCitas.DataBind();
                    sdsHorario.DataBind();
                    ddlHora.DataBind();
                    string sVal6 = btn6.Text;
                    
                        if (sVal6.Contains("["))
                        {
                            sVal6 = sVal6.Substring(0, sVal6.IndexOf("[") - 1);
                            bRecuperar = true;
                        }
                    

                    ddlHora.Items.FindByText(sVal6).Selected=true;
                    break;
                default: break;
            }
            if(bRecuperar==true)
            {
                RecuperarCita();
                if (bPrimerSeleccionPaciente || sValAnterior != txtApellidos.ToolTip)
                {
                    Limpiar_bold();
                    string sPaciente = txtApellidos.Text.ToUpper() + " " + txtNombres.Text.ToUpper();
                    string sFecha = sToolTip;
                    Seleccionarcitaspaciente(sPaciente, sFecha);
                }

            }
            if(txtApellidos.Text.Length == 0)
            {
                txtApellidos.Focus();
            }
            
        }

        protected void BtnAtras_Click(object sender, EventArgs e)
        {
            string sIni = DateTime.Parse(txtFecIni.Text.ToString()).AddDays(-7).ToShortDateString();
            string sFecini = sIni;
            string sFecFin = DateTime.Parse(sFecini).AddDays(6).ToShortDateString();
            txtFecIni.Text = sFecini;
            txtFecFin.Text = sFecFin;
            lblLeyenda.Text = "" + txtFecIni.Text + " - " + txtFecFin.Text;
            Cambiardiassemana();
            string sSql = "";
            sSql = " EXEC wsp_citas_x_semana '" + DateTime.Parse(txtFecIni.Text).ToString("yyyyMMdd") + "', '" + DateTime.Parse(txtFecFin.Text).ToString("yyyyMMdd") + "', ";
            sSql = sSql + " '" + txtUser.Text.ToString() + "'";
            sdsSQL.SelectCommand = sSql;
            DataView dv1 = (DataView)sdsSQL.Select(DataSourceSelectArguments.Empty);
            DataTable dt1 = dv1.ToTable();
            int i = 0;
            sdsCitasxSemana.DataBind();
            gvSemana.DataBind();
            gvSemana.Visible = true;
            for (i = 0; i <= gvSemana.Rows.Count - 1; i++)
            {
                GridViewRow row = gvSemana.Rows[i];
                for (int j = 0; j <= gvSemana.Columns.Count - 1; j++)
                {
                    gvSemana.Rows[i].Cells[j].ToolTip = DateTime.Parse(txtFecIni.Text).AddDays(j).DayOfWeek + " " + DateTime.Parse(txtFecIni.Text).AddDays(j).Day.ToString();
                    gvSemana.Rows[i].Cells[j].BackColor = System.Drawing.Color.White;
                    gvSemana.Rows[i].Cells[j].ForeColor = System.Drawing.Color.Black;
                    LinkButton btn = (LinkButton)row.Cells[j].Controls[0];
                    if (btn.Text.Contains("AVAILABLE"))
                    {

                    }
                    else
                    {
                        gvSemana.Rows[i].Cells[j].BackColor = System.Drawing.Color.DarkRed;
                        gvSemana.Rows[i].Cells[j].ForeColor = System.Drawing.Color.White;
                    }
                }
            }
            Limpiar_bold();
            Pintar_examenes_clinicos();
        }

        protected void BtnAdelante_Click(object sender, EventArgs e)
        {
            string sIni = DateTime.Parse(txtFecFin.Text.ToString()).AddDays(1).ToShortDateString();
            string sFecini = sIni;
            string sFecFin = DateTime.Parse(sFecini).AddDays(6).ToShortDateString();
            txtFecIni.Text = sFecini;
            txtFecFin.Text = sFecFin;
            lblLeyenda.Text = "" + txtFecIni.Text + " - " + txtFecFin.Text;
            Cambiardiassemana();
            string sSql = "";
            sSql = " EXEC wsp_citas_x_semana '" + DateTime.Parse(txtFecIni.Text).ToString("yyyyMMdd") + "', '" + DateTime.Parse(txtFecFin.Text).ToString("yyyyMMdd") + "', ";
            sSql = sSql + " '" + txtUser.Text.ToString() + "'";
            sdsSQL.SelectCommand = sSql;
            DataView dv1 = (DataView)sdsSQL.Select(DataSourceSelectArguments.Empty);
            DataTable dt1 = dv1.ToTable();
            int i = 0;
            sdsCitasxSemana.DataBind();
            gvSemana.DataBind();
            
            
            gvSemana.Visible = true;
            for (i = 0; i <= gvSemana.Rows.Count - 1; i++)
            {
                GridViewRow row = gvSemana.Rows[i];
                for (int j = 0; j <= gvSemana.Columns.Count - 1; j++)
                {
                    gvSemana.Rows[i].Cells[j].ToolTip = DateTime.Parse(txtFecIni.Text).AddDays(j).DayOfWeek + " " + DateTime.Parse(txtFecIni.Text).AddDays(j).Day.ToString();
                    gvSemana.Rows[i].Cells[j].BackColor = System.Drawing.Color.White;
                    gvSemana.Rows[i].Cells[j].ForeColor = System.Drawing.Color.Black;
                    LinkButton btn = (LinkButton)row.Cells[j].Controls[0];
                    if (btn.Text.Contains("AVAILABLE"))
                    {

                    }
                    else
                    {
                        gvSemana.Rows[i].Cells[j].BackColor = System.Drawing.Color.DarkRed;
                        gvSemana.Rows[i].Cells[j].ForeColor = System.Drawing.Color.White;
                    }
                }
            }
            Limpiar_bold();
            Pintar_examenes_clinicos();
        }

        protected void TxtApellidos_TextChanged(object sender, EventArgs e)
        {
            if(txtApellidos.Text.Length == 0 && txtNombres.Text.Length == 0)
            {
                gvPacientes.Visible = false;
                return;
            }
            if(txtApellidos.Text.Length == 0)
            {
                txtApellidos.Text = "%";
            }
            if (txtNombres.Text.Length == 0)
            {
                txtNombres.Text = "%";
            }
            BtnBuscar_Click(null, null);
            
            txtNombres.Focus();
        }

        protected void TxtNombres_TextChanged(object sender, EventArgs e)
        {
            if (txtApellidos.Text.Length == 0 && txtNombres.Text.Length == 0)
            {
                gvPacientes.Visible = false;
                return;
            }
            if (txtApellidos.Text.Length == 0)
            {
                txtApellidos.Text = "%";
            }
            if (txtNombres.Text.Length == 0)
            {
                txtNombres.Text = "%";
            }
            BtnBuscar_Click(null, null);
            if(gvPacientes.Visible == true)
            {
                gvPacientes.Focus();
            }
            
        }

        protected void GuardarPac()
        {
            string sSql = "";
            string sID = "";
            bool bVal = false;
            txtApellidos.ToolTip = "";
            if (txtFecNac.Text.Length == 0)
            {
                txtFecNac.Text = "01/01/1900";
            }

            sSql = " SELECT cod_paciente FROM lpacientes WHERE txt_apellidos = '" + txtApellidos.Text.ToUpper() + "'";
            sSql = sSql + " AND txt_nombre = '" + txtNombres.Text.ToUpper() + "'";
            sSql = sSql + " AND cod_usuario = '" + txtUser.Text.ToString() + "'";
            sdsSQL.SelectCommand = sSql;
            DataView dvv = (DataView)sdsSQL.Select(DataSourceSelectArguments.Empty);
            DataTable dtv = dvv.ToTable();
            if (dtv.Rows.Count > 0)
            {
                //Response.Write(@"<script language='javascript'>alert('El nombre ya existe, favor de verificar o recuperar el código existente del paciente.')</script>");
                txtApellidos.ToolTip = dtv.Rows[0].ItemArray[0].ToString();
                return;
            }

            if (txtApellidos.ToolTip.Length == 0)
            {
                sSql = " SELECT (isnull(nro_ult,0)+1) FROM numeraciones WHERE cod_num = 'PACIENTES'";
                sSql = sSql + " AND cod_usuario = '" + txtUser.Text.ToString() + "'";
                sdsSQL.SelectCommand = sSql;
                DataView dv = (DataView)sdsSQL.Select(DataSourceSelectArguments.Empty);
                DataTable dt = dv.ToTable();
                if (dt.Rows.Count > 0)
                {
                    sID = dt.Rows[0].ItemArray[0].ToString();
                    txtApellidos.ToolTip = sID;
                    bVal = true;
                }
                else
                {
                    //Response.Write(@"<script language='javascript'>alert('Error al obtener correlativo.')</script>");

                    return;
                }

                sSql = " UPDATE numeraciones SET nro_ult = " + sID + " WHERE cod_num = 'PACIENTES'";
                sSql = sSql + " AND cod_usuario = '" + txtUser.Text.ToString() + "'";
                sdsSQL.UpdateCommand = sSql;
                sdsSQL.Update();
            }
            cod_pac = int.Parse(txtApellidos.ToolTip);
            if (cod_pac > 0)
            {




                if (bVal == true)
                {
                    sSql = "INSERT INTO lpacientes";
                    sSql = sSql + " SELECT ";
                    sSql = sSql + " " + cod_pac.ToString();
                    sSql = sSql + ", '" + txtApellidos.Text.ToUpper() + "'";
                    sSql = sSql + ", '" + txtNombres.Text.ToUpper() + "'";
                    sSql = sSql + ", '" + DateTime.Parse(txtFecNac.Text).ToString("yyyyMMdd") + "'";
                    sSql = sSql + ", '" + txtDir.Text.ToUpper() + "'";
                    sSql = sSql + ", " + ddlPais.SelectedValue.ToString() + "";
                    sSql = sSql + ", " + ddlDepto.SelectedValue.ToString() + "";
                    sSql = sSql + ", " + ddlMunicipio.SelectedValue.ToString() + "";
                    sSql = sSql + ", '" + txtTelCel.Text.ToUpper() + "'";
                    sSql = sSql + ", '" + txtTelCasa.Text.ToUpper() + "'";
                    sSql = sSql + ", '" + txtEmail.Text.ToUpper() + "'";
                    sSql = sSql + ", '" + txtPersonaEncargada.Text.ToUpper() + "'";
                    sSql = sSql + ", '" + txtRelacion.Text.ToUpper() + "'";
                    sSql = sSql + ", '" + txtTelCelPE.Text.ToUpper() + "'";
                    sSql = sSql + ", '" + txtTelCasaPE.Text.ToUpper() + "'";
                    sSql = sSql + ", '" + txtReferido.Text.ToUpper() + "', '', ''";
                    sSql = sSql + ", '" + txtAfiliacion.Text.ToUpper() + "'";
                    sSql = sSql + ", '" + txtUser.Text.ToString() + "'";
                    sdsSQL.InsertCommand = sSql;
                    sdsSQL.Insert();
                }
                else
                {
                    sSql = "UPDATE lpacientes";
                    sSql = sSql + " SET ";
                    sSql = sSql + " txt_apellidos = '" + txtApellidos.Text.ToUpper() + "'";
                    sSql = sSql + ", txt_nombre = '" + txtNombres.Text.ToUpper() + "'";
                    sSql = sSql + ", fec_nac = '" + DateTime.Parse(txtFecNac.Text).ToString("yyyyMMdd") + "'";
                    sSql = sSql + ", txt_dir = '" + txtDir.Text.ToUpper() + "'";
                    sSql = sSql + ", cod_pais = " + ddlPais.SelectedValue.ToString() + "";
                    sSql = sSql + ", cod_depto = " + ddlDepto.SelectedValue.ToString() + "";
                    sSql = sSql + ", cod_municipio = " + ddlMunicipio.SelectedValue.ToString() + "";
                    sSql = sSql + ", txt_tel_cel = '" + txtTelCel.Text.ToUpper() + "'";
                    sSql = sSql + ", txt_tel_casa = '" + txtTelCasa.Text.ToUpper() + "'";
                    sSql = sSql + ", txt_email = '" + txtEmail.Text.ToUpper() + "'";
                    sSql = sSql + ", txt_persona_enc = '" + txtPersonaEncargada.Text.ToUpper() + "'";
                    sSql = sSql + ", txt_relacion = '" + txtRelacion.Text.ToUpper() + "'";
                    sSql = sSql + ", txt_tel_cel_pe = '" + txtTelCelPE.Text.ToUpper() + "'";
                    sSql = sSql + ", txt_tel_casa_pe = '" + txtTelCasaPE.Text.ToUpper() + "'";
                    sSql = sSql + ", txt_referido_por = '" + txtReferido.Text.ToUpper() + "'";
                    sSql = sSql + ", txt_afiliacion = '" + txtAfiliacion.Text.ToUpper() + "'";
                    sSql = sSql + " WHERE cod_paciente = ";
                    sSql = sSql + " " + cod_pac.ToString();
                    sSql = sSql + " AND cod_usuario = '" + txtUser.Text.ToString() + "'";

                    sdsSQL.UpdateCommand = sSql;
                    sdsSQL.Update();
                }

                txtApellidos.Text = txtApellidos.Text.ToUpper();
                txtNombres.Text = txtNombres.Text.ToUpper();
                //Response.Write(@"<script language='javascript'>alert('Los datos han sido grabados.')</script>");

            }
        }

        protected void BtnAddPac_Click(object sender, EventArgs e)
        {
            GuardarPac();
            txtApellidos.Text = txtApellidos.Text.ToUpper();
            txtNombres.Text = txtNombres.Text.ToUpper();
            gvSemana.Focus();
            gvPacientes.Visible = false;
        }

        protected void ChkVerMes_CheckedChanged(object sender, EventArgs e)
        {
            if(chkVerMes.Checked == true)
            {
                ClrCitas.Style.Value = "display:normal";
                tdCalendar.Style.Value = "width:24%;vertical-align:top";
                tdWeek.Style.Value = "width:75%";
                tdDatosHora.Style.Value = "width:24%";
                tdDatosHora2.Style.Value = "width:75%";
                ClrCitas.Visible = true;
            }
            else
            {
                ClrCitas.Style.Value = "display:none";
                tdCalendar.Style.Value = "width:0%;vertical-align:top";
                tdWeek.Style.Value = "width:100%";
                tdDatosHora.Style.Value = "width:0%";
                tdDatosHora2.Style.Value = "width:100%";
                ClrCitas.Visible = false;
            }
        }

        protected void Seleccionarcitaspaciente(string sPaciente, string sFecha)
        {
            int i = 0;

            for (i = 0; i <= gvSemana.Rows.Count - 1; i++)
            {
                GridViewRow row = gvSemana.Rows[i];
                for (int j = 0; j <= gvSemana.Columns.Count - 1; j++)
                {
                    LinkButton btn = (LinkButton)row.Cells[j].Controls[0];
                    if (btn.Text.Contains(sPaciente))
                    {
                        string sDayCell = row.Cells[j].ToolTip.Substring(row.Cells[j].ToolTip.Length - 2, 2);
                        string sDayParam = DateTime.Parse(sFecha).Day.ToString();
                        if (int.Parse(sDayCell.ToString()) == int.Parse(sDayParam.ToString()))
                        {
                            btn.Font.Bold = true;
                        }
                        
                    }
                    

                }
            }
        }
    }
}