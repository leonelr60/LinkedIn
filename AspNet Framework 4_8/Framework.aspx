<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Citas.aspx.cs" Inherits="waFichaClinica.WForms.Citas" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div style="z-index: -100; position: fixed; top: 0px; left: 0px; width: 100%; height: 100%;  opacity: 0.5;">
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <div style="width: 100%">
                <asp:TextBox ID="txtUser" runat="server" Enabled="false" Visible="false" Font-Size="Small"></asp:TextBox>

                <h2>Appointments </h2>
                <h3>Personal Data </h3>
                <hr />
                <asp:SqlDataSource ID="sdsSQL" runat="server" ConnectionString='<%$ ConnectionStrings:CoMedOLConnectionString %>' SelectCommand="SELECT 1"></asp:SqlDataSource>
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 50%">
                            <asp:Label ID="lblApellidos" CssClass="control-label" runat="server" Text="Last Name:"></asp:Label>
                            <asp:TextBox ID="txtApellidos" CssClass="form-control" runat="server" Width="99%" AutoPostBack="true" OnTextChanged="TxtApellidos_TextChanged"></asp:TextBox>
                            <asp:GridView ID="gvPacientes" CssClass="table" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" DataSourceID="sdsBusquedaNombre" OnSelectedIndexChanged="GvPacientes_SelectedIndexChanged">
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775"></AlternatingRowStyle>

                                <Columns>
                                    <asp:CommandField ShowSelectButton="True"></asp:CommandField>
                                    <asp:BoundField DataField="cod_paciente" HeaderText="Code" SortExpression="cod_paciente"></asp:BoundField>
                                    <asp:BoundField DataField="txt_apellidos" HeaderText="Last Name" SortExpression="txt_apellidos"></asp:BoundField>
                                    <asp:BoundField DataField="txt_nombre" HeaderText="First Name" SortExpression="txt_nombre"></asp:BoundField>
                                </Columns>

                                <EditRowStyle BackColor="#999999"></EditRowStyle>

                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"></FooterStyle>

                                <HeaderStyle BackColor="#444444" Font-Bold="False" ForeColor="White"></HeaderStyle>

                                <PagerStyle HorizontalAlign="Center" BackColor="#284775" ForeColor="White"></PagerStyle>

                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333"></RowStyle>

                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333"></SelectedRowStyle>

                                <SortedAscendingCellStyle BackColor="#E9E7E2"></SortedAscendingCellStyle>

                                <SortedAscendingHeaderStyle BackColor="#506C8C"></SortedAscendingHeaderStyle>

                                <SortedDescendingCellStyle BackColor="#FFFDF8"></SortedDescendingCellStyle>

                                <SortedDescendingHeaderStyle BackColor="#6F8DAE"></SortedDescendingHeaderStyle>
                            </asp:GridView>
                            <asp:SqlDataSource runat="server" ID="sdsBusquedaNombre" ConnectionString='<%$ ConnectionStrings:CoMedOLConnectionString %>' SelectCommand="SELECT lp.cod_paciente, lp.txt_apellidos, lp.txt_nombre
FROM lpacientes lp
WHERE lp.txt_apellidos LIKE '%'+upper(@apellidos)+'%' AND lp.txt_nombre LIKE '%'+upper(@nombres)+'%' AND lp.cod_usuario = @cod_usuario  ">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="txtApellidos" PropertyName="Text" Name="apellidos"></asp:ControlParameter>
                                    <asp:ControlParameter ControlID="txtNombres" PropertyName="Text" Name="nombres"></asp:ControlParameter>
                                    <asp:ControlParameter ControlID="txtUser" PropertyName="Text" Name="cod_usuario"></asp:ControlParameter>
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </td>
                        <td style="width: 50%">
                            <asp:Label ID="lblNombres" CssClass="control-label" runat="server" Text="First Name:"></asp:Label>
                            <table>
                                <tr>
                                    <td style="width: 50%">
                                        <asp:TextBox ID="txtNombres" CssClass="form-control" runat="server" Width="100%" AutoPostBack="true" OnTextChanged="TxtNombres_TextChanged"></asp:TextBox>
                                    </td>
                                    <td style="width: 50%">
                                        <asp:Button ID="btnBuscar" CssClass="btn btn-light" runat="server" Text="..." OnClick="BtnBuscar_Click" />
                                        <asp:Button ID="btnAddPac" CssClass="btn btn-light" runat="server" Text="+" OnClick="BtnAddPac_Click" />
                                    </td>
                                </tr>
                            </table>

                        </td>
                    </tr>
                    <tr>
                        <td style="width: 50%">
                            <asp:Label ID="lblFecNac" CssClass="control-label" runat="server" Text="Birth Date:"></asp:Label>
                            <asp:TextBox ID="txtFecNac" TextMode="Date" CssClass="form-control" runat="server"></asp:TextBox>
                        </td>
                        
                        <td style="width: 50%">
                            <asp:Label ID="lblDir" CssClass="control-label" runat="server" Text="Address:"></asp:Label>
                            <asp:TextBox ID="txtDir" CssClass="form-control" runat="server" Width="99%"></asp:TextBox>
                        </td>
                        
                    </tr>
                    <tr>
                        <td style="width: 50%">
                            <asp:Label ID="lblPais" CssClass="control-label" runat="server" Text="Country:"></asp:Label>
                            <asp:DropDownList ID="ddlPais" CssClass="form-control" runat="server" DataSourceID="sdsPaises" DataTextField="txt_pais" DataValueField="cod_pais" OnSelectedIndexChanged="DdlPais_SelectedIndexChanged" AutoPostBack="True" Width="99%"></asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="sdsPaises" ConnectionString='<%$ ConnectionStrings:CoMedOLConnectionString %>' SelectCommand="SELECT [cod_pais], [txt_pais] FROM [lpaises]"></asp:SqlDataSource>
                        </td>
                        <td style="width: 50%">
                            <asp:Label ID="lblDepto" CssClass="control-label" runat="server" Text="State:"></asp:Label>
                            <asp:DropDownList ID="ddlDepto" CssClass="form-control" runat="server" DataSourceID="sdsDeptos" DataTextField="txt_depto" DataValueField="cod_depto" OnSelectedIndexChanged="DdlDepto_SelectedIndexChanged" AutoPostBack="True" Width="99%"></asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="sdsDeptos" ConnectionString='<%$ ConnectionStrings:CoMedOLConnectionString %>' SelectCommand="SELECT [cod_depto], [txt_depto] FROM [ldeptos] WHERE ([cod_pais] = @cod_pais)">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddlPais" PropertyName="SelectedValue" Name="cod_pais" Type="Int32"></asp:ControlParameter>
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </td>
                        
                    </tr>
                    <tr>
                        <td style="width: 50%">
                            <asp:Label ID="lblMunicipio" CssClass="control-label" runat="server" Text="County / City:"></asp:Label>
                            <asp:DropDownList ID="ddlMunicipio" CssClass="form-control" runat="server" DataSourceID="sdsMunicipios" DataTextField="txt_municipio" DataValueField="cod_municipio" AutoPostBack="True" Width="99%"></asp:DropDownList>

                            <asp:SqlDataSource runat="server" ID="sdsMunicipios" ConnectionString='<%$ ConnectionStrings:CoMedOLConnectionString %>' SelectCommand="SELECT [cod_municipio], [txt_municipio] FROM [lmunicipios] WHERE (([cod_pais] = @cod_pais) AND ([cod_depto] = @cod_depto))">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddlPais" PropertyName="SelectedValue" Name="cod_pais" Type="Int32"></asp:ControlParameter>
                                    <asp:ControlParameter ControlID="ddlDepto" PropertyName="SelectedValue" Name="cod_depto" Type="Int32"></asp:ControlParameter>
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </td>
                        <td style="width: 50%">
                            <asp:Label ID="lblTelCel" CssClass="control-label" runat="server" Text="Mobil Phone:"></asp:Label>
                            <asp:TextBox ID="txtTelCel" CssClass="form-control" runat="server"></asp:TextBox>
                        </td>
                        
                    </tr>
                    <tr>
                        <td style="width: 50%">
                            <asp:Label ID="lblTelCasa" CssClass="control-label" runat="server" Text="Home Phone:"></asp:Label>
                            <asp:TextBox ID="txtTelCasa" CssClass="form-control" runat="server"></asp:TextBox>
                        </td>
                        
                        <td style="width: 50%">
                            <asp:Label ID="lblEmail" CssClass="control-label" runat="server" Text="EMail:"></asp:Label>
                            <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server" Width="99%"></asp:TextBox>
                        </td>
                        
                    </tr>
                    <tr>
                        <td style="width: 50%">
                            <asp:Label ID="lblPersonaEncargada" CssClass="control-label" runat="server" Text="Person in Charge:"></asp:Label>
                            <asp:TextBox ID="txtPersonaEncargada" CssClass="form-control" runat="server"></asp:TextBox>
                        </td>
                        
                        <td style="width: 50%">
                            <asp:Label ID="lblRelacion" CssClass="control-label" runat="server" Text="Relation:"></asp:Label>
                            <asp:TextBox ID="txtRelacion" CssClass="form-control" runat="server"></asp:TextBox>
                        </td>
                        
                    </tr>
                    <tr>
                        <td style="width: 50%">
                            <asp:Label ID="lblTelCelPE" CssClass="control-label" runat="server" Text="Rel. Mobil Phone:"></asp:Label>
                            <asp:TextBox ID="txtTelCelPE" CssClass="form-control" runat="server"></asp:TextBox>
                        </td>
                        
                        <td style="width: 50%">
                            <asp:Label ID="lblTelCasaPE" CssClass="control-label" runat="server" Text="Rel. Home Phone:"></asp:Label>
                            <asp:TextBox ID="txtTelCasaPE" CssClass="form-control" runat="server"></asp:TextBox>
                        </td>
                        
                    </tr>
                    <tr>
                        <td style="width: 50%">
                            <asp:Label ID="lblReferido" CssClass="control-label" runat="server" Text="Reffered By:"></asp:Label>
                            <asp:TextBox ID="txtReferido" CssClass="form-control" runat="server"></asp:TextBox>
                        </td>
                        
                        <td style="width: 50%">
                            <asp:Label ID="lblAfiliacion" CssClass="control-label" runat="server" Text="Social Security:"></asp:Label>
                            <asp:TextBox ID="txtAfiliacion" CssClass="form-control" runat="server" Width="100%"></asp:TextBox>
                        </td>
                        
                    </tr>

                </table>
                <hr />
                <h3>Appointments History</h3>
                <asp:GridView ID="gvHistCitas" runat="server" Font-Size="Small" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" DataSourceID="sdsHistCitas">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775"></AlternatingRowStyle>

                    <Columns>
                        <asp:BoundField DataField="Fecha" HeaderText="Date" SortExpression="Fecha"></asp:BoundField>
                        <asp:BoundField DataField="Hora" HeaderText="Time" SortExpression="Hora"></asp:BoundField>
                        <asp:BoundField DataField="Estado" HeaderText="Appointment State" ReadOnly="True" SortExpression="Estado"></asp:BoundField>
                        <asp:BoundField DataField="Registro" HeaderText="Registry" ReadOnly="True" SortExpression="Registro"></asp:BoundField>
                    </Columns>

                    <EditRowStyle BackColor="#999999"></EditRowStyle>

                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"></FooterStyle>

                    <HeaderStyle BackColor="#444444" Font-Bold="False" ForeColor="White"></HeaderStyle>

                    <PagerStyle HorizontalAlign="Center" BackColor="#284775" ForeColor="White"></PagerStyle>

                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333"></RowStyle>

                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333"></SelectedRowStyle>

                    <SortedAscendingCellStyle BackColor="#E9E7E2"></SortedAscendingCellStyle>

                    <SortedAscendingHeaderStyle BackColor="#506C8C"></SortedAscendingHeaderStyle>

                    <SortedDescendingCellStyle BackColor="#FFFDF8"></SortedDescendingCellStyle>

                    <SortedDescendingHeaderStyle BackColor="#6F8DAE"></SortedDescendingHeaderStyle>
                </asp:GridView>
                <asp:SqlDataSource runat="server" ID="sdsHistCitas" ConnectionString='<%$ ConnectionStrings:CoMedOLConnectionString %>' SelectCommand="select convert(varchar(10),fec_cita,102) Fecha, txt_hora Hora, 
case when sn_confirmado = -1 then '|CONFIRMADA| ' else '' end  +
case when sn_novino = -1 then '|NO VINO| ' else '' end  +
case when sn_cancelada = -1 then '|CANCELADA| ' else '' end Estado, fec_modif Registro , fec_cita Cita 
from citas_hist where cod_usuario = @cod_usuario and cod_paciente = @cod_paciente
and fec_modif = (select max(ch.fec_modif) from citas_hist ch
where ch.cod_usuario = citas_hist.cod_usuario and ch.cod_paciente = citas_hist.cod_paciente 
and ch.fec_cita = citas_hist.fec_cita and ch.txt_hora = citas_hist.txt_hora)
group by convert(varchar(10),fec_cita,102), txt_hora,
case when sn_confirmado = -1 then '|CONFIRMADA| ' else '' end  +
case when sn_novino = -1 then '|NO VINO| ' else '' end  +
case when sn_cancelada = -1 then '|CANCELADA| ' else '' end, fec_modif, fec_cita ORDER BY fec_cita DESC, txt_hora DESC,fec_modif DESC">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="txtUser" PropertyName="Text" Name="cod_usuario"></asp:ControlParameter>
                        <asp:ControlParameter ControlID="txtApellidos" PropertyName="ToolTip" Name="cod_paciente"></asp:ControlParameter>
                    </SelectParameters>
                </asp:SqlDataSource>

                <div style ="width:100%; overflow-x: scroll;">
                <asp:TextBox ID="txtFecIni" runat="server" Visible="false"></asp:TextBox>
                <asp:TextBox ID="txtFecFin" runat="server" Visible="false"></asp:TextBox>
                <asp:CheckBox ID="chkVerMes" CssClass="checkbox" runat="server" Text="See Monthly Calendar?" AutoPostBack="true" OnCheckedChanged="ChkVerMes_CheckedChanged" />
                <table id="tblCalendar" runat="server" style="width: 100%">
                    <tr>
                        <td id="tdCalendar" runat="server" style="width: 24%; vertical-align: top">
                            <asp:Calendar ID="ClrCitas" SelectionMode="DayWeek" runat="server" BackColor="White" BorderColor="White" BorderWidth="1px" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black" Height="80%" NextPrevFormat="FullMonth" Width="100%" OnSelectionChanged="ClrCitas_SelectionChanged">
                                <DayHeaderStyle Font-Bold="True" Font-Size="8pt"></DayHeaderStyle>

                                <NextPrevStyle VerticalAlign="Bottom" Font-Bold="True" Font-Size="8pt" ForeColor="#333333"></NextPrevStyle>

                                <OtherMonthDayStyle ForeColor="#999999"></OtherMonthDayStyle>

                                <SelectedDayStyle BackColor="#333399" ForeColor="White"></SelectedDayStyle>

                                <TitleStyle BackColor="White" BorderColor="Black" BorderWidth="4px" Font-Bold="True" Font-Size="12pt" ForeColor="#333399"></TitleStyle>

                                <TodayDayStyle BackColor="#CCCCCC"></TodayDayStyle>
                            </asp:Calendar>
                        </td>
                        <td id="tdWeek" runat="server" style="width: 75%">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 4%; height: 100%; vertical-align: top">
                                        <asp:Button ID="btnAtras" CssClass="btn btn-light" runat="server" Text="<" Width="100%" Height="100%" OnClick="BtnAtras_Click" />
                                    </td>
                                    <td style="width: 90%">
                                        <asp:Label ID="lblLeyenda" CssClass="control-label" runat="server" Text="" Font-Size="Small"></asp:Label>
                                        <asp:GridView ID="gvSemana" OnRowCommand="GvSemana_RowCommand" Font-Size="X-Small" runat="server" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="Both" AutoGenerateColumns="False" DataSourceID="sdsCitasxSemana" OnRowDataBound="GvSemana_RowDataBound">

                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775"></AlternatingRowStyle>

                                            <Columns>
                                                <asp:ButtonField DataTextField="domingo" HeaderText="Sunday" SortExpression="domingo" CommandName="Sunday"></asp:ButtonField>
                                                <asp:ButtonField DataTextField="lunes" HeaderText="Monday" SortExpression="lunes" CommandName="Monday"></asp:ButtonField>
                                                <asp:ButtonField DataTextField="martes" HeaderText="Tuesday" SortExpression="martes" CommandName="Tuesday"></asp:ButtonField>
                                                <asp:ButtonField DataTextField="miercoles" HeaderText="Wednesday" SortExpression="miercoles" CommandName="Wednesday"></asp:ButtonField>
                                                <asp:ButtonField DataTextField="jueves" HeaderText="Thursday" SortExpression="jueves" CommandName="Thursday"></asp:ButtonField>
                                                <asp:ButtonField DataTextField="viernes" HeaderText="Friday" SortExpression="viernes" CommandName="Friday"></asp:ButtonField>
                                                <asp:ButtonField DataTextField="sabado" HeaderText="Saturday" SortExpression="sabado" CommandName="Saturday"></asp:ButtonField>

                                                <%--<asp:BoundField DataField="domingo" HeaderText="Domingo" SortExpression="domingo"></asp:BoundField>
                                                <asp:BoundField DataField="lunes" HeaderText="Lunes" SortExpression="lunes"></asp:BoundField>
                                                <asp:BoundField DataField="martes" HeaderText="Martes" SortExpression="martes"></asp:BoundField>
                                                <asp:BoundField DataField="miercoles" HeaderText="Miércoles." SortExpression="miercoles"></asp:BoundField>
                                                <asp:BoundField DataField="jueves" HeaderText="Jueves" SortExpression="jueves"></asp:BoundField>
                                                <asp:BoundField DataField="viernes" HeaderText="Viernes" SortExpression="viernes"></asp:BoundField>
                                                <asp:BoundField DataField="sabado" HeaderText="Sábado" SortExpression="sabado"></asp:BoundField>--%>
                                            </Columns>

                                            <EditRowStyle BackColor="#999999"></EditRowStyle>

                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"></FooterStyle>

                                            <HeaderStyle BackColor="#444444" Font-Bold="False" ForeColor="White"></HeaderStyle>

                                            <PagerStyle HorizontalAlign="Center" BackColor="#284775" ForeColor="White"></PagerStyle>

                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333"></RowStyle>

                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333"></SelectedRowStyle>

                                            <SortedAscendingCellStyle BackColor="#E9E7E2"></SortedAscendingCellStyle>

                                            <SortedAscendingHeaderStyle BackColor="#506C8C"></SortedAscendingHeaderStyle>

                                            <SortedDescendingCellStyle BackColor="#FFFDF8"></SortedDescendingCellStyle>

                                            <SortedDescendingHeaderStyle BackColor="#6F8DAE"></SortedDescendingHeaderStyle>
                                        </asp:GridView>
                                        <asp:SqlDataSource runat="server" ID="sdsCitasxSemana" ConnectionString='<%$ ConnectionStrings:CoMedOLConnectionString %>' SelectCommand="SELECT [domingo], [lunes], [martes], [miercoles], [jueves], [viernes], [sabado] FROM [tmp_citas] WHERE ([cod_usuario] = @cod_usuario) ORDER BY [id_orden]">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="txtUser" PropertyName="Text" Name="cod_usuario" Type="String"></asp:ControlParameter>
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </td>
                                    <td style="width: 4%; height: 100%; vertical-align: top">
                                        <asp:Button ID="btnAdelante" CssClass="btn btn-light" runat="server" Text=">" Width="100%" Height="100%" OnClick="BtnAdelante_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td id="tdDatosHora" runat="server" style="width: 24%"></td>
                        <td id="tdDatosHora2" runat="server" style="width: 75%">
                            <asp:Label ID="lblHora" CssClass="control-label" runat="server" Text="Appointment Time:"></asp:Label>
                            <asp:DropDownList ID="ddlHora" CssClass="form-control" runat="server" AutoPostBack="True" DataSourceID="sdsHorario" DataTextField="txt_cita" DataValueField="txt_hora" Width="100%"></asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="sdsHorario" ConnectionString='<%$ ConnectionStrings:CoMedOLConnectionString %>' SelectCommand="select lh.txt_hora, lh.txt_hora + ' - ' +
CASE WHEN isnull(c.cod_paciente,-1) = -1 OR c.sn_disponible = -1 OR c.sn_cancelada = -1 
THEN 'AVAILABLE' ELSE (lp.txt_apellidos + ' ' + lp.txt_nombre) END txt_cita
from lhorario lh
left outer join citas c on c.txt_hora = lh.txt_hora and c.fec_cita = @fecha and c.cod_usuario = @cod_usuario
left outer join lpacientes lp on lp.cod_paciente = c.cod_paciente and lp.cod_usuario = c.cod_usuario
WHERE lh.cod_usuario = @cod_usuario
order by lh.id_orden">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ClrCitas" PropertyName="SelectedDate" Name="fecha"></asp:ControlParameter>
                                    <asp:ControlParameter ControlID="txtUser" PropertyName="Text" Name="cod_usuario"></asp:ControlParameter>
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:Button ID="btnRecuperar" CssClass="btn btn-light" runat="server" Text="Recover" OnClick="BtnRecuperar_Click" />
                            <h3>Status</h3>
                            <div class="col-m-4"><asp:CheckBox ID="chkTelConf" CssClass="checkbox" runat="server" Text="Telephone Reminder" /></div>
                            <div class="col-m-4"><asp:CheckBox ID="chkEmailConf" CssClass="checkbox" runat="server" Text="Email Reminder" /></div>
                            <div class="col-m-4"><asp:CheckBox ID="chkMessageConf" CssClass="checkbox" runat="server" Text="Message (MMS) Reminder" /></div>
                            <br />
                            <div class="col-m-4"><asp:CheckBox ID="chkConfirmado" CssClass="checkbox" runat="server" Text="Confirmed" /></div>
                            <div class="col-m-4"><asp:CheckBox ID="chkNoVino" CssClass="checkbox" runat="server" Text="He/She Did Not Show Up" /></div>
                            <div class="col-m-4"><asp:CheckBox ID="chkCancelada" CssClass="checkbox" runat="server" Text="Canceled" /></div>
                            <asp:Label ID="lblMotivo" CssClass="control-label" runat="server" Text="Reason of Appointment:"></asp:Label>
                            <asp:DropDownList ID="ddlMotivoCita" CssClass="form-control" Width="100%" runat="server" AutoPostBack="True" DataSourceID="sdsMotivoCita" DataTextField="txt_examen" DataValueField="cod_examen"></asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="sdsMotivoCita" ConnectionString='<%$ ConnectionStrings:CoMedOLConnectionString %>' SelectCommand="select -1 cod_examen, 'NO APLICA' txt_examen
union all
select cod_examen, txt_examen from lexamen_clinico where cod_usuario = @cod_usuario
">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="txtUser" PropertyName="Text" Name="cod_usuario"></asp:ControlParameter>
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:TextBox ID="txtObsCita" CssClass="form-control" TextMode="MultiLine" Width="100%" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>

                </div><%--Div for mobile screen--%>  
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress" runat="server">
        <ProgressTemplate>

            <div style="top: 75%; width: 100%; text-align: center; position: absolute; border: inset; background-color: lightsteelblue; left: 0px">

                <h2>Loading, please wait...</h2>

            </div>


        </ProgressTemplate>
    </asp:UpdateProgress>
    <hr />
    <div style="width: 100%">

        <table style="width: 100%">
            <tr>
                <td style="width: 48%">
                    <asp:Button ID="btnGuardar" CssClass="btn btn-light" runat="server" Text="Save Appointment" OnClick="BtnGuardar_Click" />
                    <asp:Button ID="btnLimpiar" CssClass="btn btn-light" runat="server" Text="Clear" OnClick="BtnLimpiar_Click" />
                    <asp:Button ID="btnCrear" CssClass="btn btn-light" runat="server" Text="Create Patient" OnClick="BtnCrear_Click" />
                    <asp:Button ID="btnGuardarPac" CssClass="btn btn-light" runat="server" Text="Save Patient" Enabled="false" Visible="false" OnClick="BtnGuardarPac_Click" />
            </tr>

        </table>

    </div>
</asp:Content>
