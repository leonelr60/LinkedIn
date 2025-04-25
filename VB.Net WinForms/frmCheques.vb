Public Class frmCheques
    Dim clConnect As New clsConnect
    Dim clFunciones As New clsFunctions
    Dim iNro_Chequera As Long
    Dim iFecX As Long
    Dim iFecY As Long
    Dim iBenefX As Long
    Dim iBenefY As Long
    Dim iImporteX As Long
    Dim iImporteY As Long
    Dim iLetrasX As Long
    Dim iLetrasY As Long
    Dim iNoNegX As Long
    Dim iNoNegY As Long
    Public lnroPol As Long
    Public dFecPol As String

    Private Sub frmCheques_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim sSql As String
        If Len(LTrim(RTrim(txtCta.Tag))) = 0 Then
            '    Exit Sub
        End If
        
        If Mid(txtCta.Tag, 1, 4) = "True" Then
            clConnect.Connect(frmLogin.sStringC)
            sSql = "select c.cod_moneda, c.nombre, c.cta_cble ,  ch.cod_chequera , "
            sSql = sSql & " ch.cheque_ini , "
            sSql = sSql & " ch.cheque_fin , "
            sSql = sSql & " ch.cheque_ult , ch.codigo "
            sSql = sSql & " from ctasbancarias c "
            sSql = sSql & " inner join monedas m on m.codigo = c.cod_moneda"
            sSql = sSql & " inner join chequeras ch on ch.cod_empre = c.cod_empre"
            sSql = sSql & " and ch.codigo = c.codigo and ch.cod_moneda = c.cod_moneda"
            sSql = sSql & " and ch.sn_activa = -1"
            sSql = sSql & " and ch.cod_chequera = " & IIf(Mid(txtCta.Tag, 5, Len(LTrim(RTrim(txtCta.Tag)))) = "N/A", 0, Mid(txtCta.Tag, 5, Len(LTrim(RTrim(txtCta.Tag)))))
            sSql = sSql & " where c.cod_empre = " & clFunciones.cboGetItem(frmMenu.cboEmpresas, 1)
            sSql = sSql & " and c.codigo = '" & LTrim(RTrim(txtCta.Text)) & "'"
            clConnect.Query(1, sSql, 0, clConnect)
            If clConnect.oDataReader.HasRows Then
                Do While clConnect.oDataReader.Read()
                    txtNombre.Text = clConnect.oDataReader.Item(1)
                    clFunciones.cboSetItem(cboMonedas, "", clConnect.oDataReader.Item(0), 0, 1)
                    iNro_Chequera = clConnect.oDataReader.Item(3)
                    txtNombre.Text = txtNombre.Text & "[" & iNro_Chequera & "-" & clConnect.oDataReader.Item(7) & "]"
                    txtCta.Tag = ""
                    Me.txtUltChGen.Text = clConnect.oDataReader.Item(6)
                Loop
            End If
            clConnect.Disconnect()

        End If
        If Len(LTrim(RTrim(txtNomCheque.Tag))) = 0 Then
            Exit Sub
        End If

        If Mid(txtNomCheque.Tag, 1, 4) = "True" Then
            clConnect.Connect(frmLogin.sStringC)
            sSql = "select txt_nombre from proveedores where sn_activa = -1 and cod_empre = " & clFunciones.cboGetItem(frmMenu.cboEmpresas, 1)
            sSql = sSql & " and codigo = " & LTrim(RTrim(txtNomCheque.Text))
            clConnect.Query(1, sSql, 0, clConnect)
            If clConnect.oDataReader.HasRows Then
                Do While clConnect.oDataReader.Read()
                    txtNomCheque.Text = clConnect.oDataReader.Item(0)
                    txtNomCheque.Tag = ""

                Loop
            End If
            clConnect.Disconnect()
        End If
    End Sub

   
    Private Sub frmCheques_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim oDataReader As Odbc.OdbcDataReader
        Dim sSql As String
        Dim i As Long

        clConnect.Connect(frmLogin.sStringC)
        If frmMenu.ContabilidadToolStripMenuItem.Visible = True Then
            txtFecha.Text = clConnect.Today
        Else
            txtFecha.Text = Today
        End If
        txtFecha.Text = Mid(txtFecha.Text, 1, 10)
        Me.txtFecCheque.Text = txtFecha.Text
        ' Monedas
        oDataReader = Nothing
        sSql = "select * from monedas"
        sSql = sSql & " WHERE cod_empre = " & clFunciones.cboGetItem(frmMenu.cboEmpresas, 1)
        clConnect.Query(1, sSql, 0, clConnect)
        If clConnect.oDataReader.HasRows Then
            i = 0
            cboMonedas.DropDownStyle = ComboBoxStyle.DropDownList
            cboMonedas.Items.Clear()
            Do While clConnect.oDataReader.Read()
                cboMonedas.Items.Add(i)
                cboMonedas.Items.Item(i) = clConnect.oDataReader.Item(0) & " - " & clConnect.oDataReader.Item(1)
                i = i + 1
            Loop
            If cboMonedas.Items.Count > 0 Then
                clFunciones.cboSetItem(cboMonedas, "", 0, 0, 2)
            End If
        End If
        oDataReader = Nothing
        sSql = "select codigo, txt_desc from tipo_cheque where sn_activo = -1 and cod_empre = " & clFunciones.cboGetItem(frmMenu.cboEmpresas, 1)
        clConnect.Query(1, sSql, 0, clConnect)
        If clConnect.oDataReader.HasRows Then
            i = 0
            cboTCheque.DropDownStyle = ComboBoxStyle.DropDownList
            cboTCheque.Items.Clear()
            Do While clConnect.oDataReader.Read()
                cboTCheque.Items.Add(i)
                cboTCheque.Items.Item(i) = clConnect.oDataReader.Item(0) & " - " & clConnect.oDataReader.Item(1)
                i = i + 1
            Loop
            If cboTCheque.Items.Count > 0 Then
                clFunciones.cboSetItem(cboTCheque, "", 0, 0, 2)
            End If
        End If
        clConnect.Disconnect()
        Me.txtHechoPor.Text = frmLogin.sUser
    End Sub

    Private Sub buscaProveedor()
        Dim oDataReader As Odbc.OdbcDataReader
        Dim sSql As String
        Dim oTran As Odbc.OdbcTransaction
        oTran = Nothing
        txtNomCheque.Text = UCase(txtNomCheque.Text)
        clConnect.Connect(frmLogin.sStringC)
        ' Busca NIT Existente
        oDataReader = Nothing
        sSql = "select txt_nombre from proveedores where sn_activa = -1 and cod_empre = " & clFunciones.cboGetItem(frmMenu.cboEmpresas, 1)
        sSql = sSql & " and cod_nit = '" & LTrim(RTrim(txtNomCheque.Text)) & "' or txt_nombre like '%" & LTrim(RTrim(txtNomCheque.Text)) & "%'"
        clConnect.Query(1, sSql, 0, clConnect)
        If clConnect.oDataReader.HasRows Then
            Dim aArray As New ArrayList
            Dim aVisible As New ArrayList
            sSql = "select codigo, txt_nombre from proveedores where sn_activa = -1 and cod_empre = " & clFunciones.cboGetItem(frmMenu.cboEmpresas, 1)
            sSql = sSql & " and cod_nit = '" & LTrim(RTrim(txtNomCheque.Text)) & "' or txt_nombre like '%" & LTrim(RTrim(txtNomCheque.Text)) & "%'"

            aArray.Clear()
            aArray.Add("Código")
            aArray.Add("Nombre")
            aVisible.Clear()
            aVisible.Add("True")
            aVisible.Add("True")
            frmConsulta.iCols = 2
            frmConsulta.sSql = sSql
            frmConsulta.aHeaders = aArray
            frmConsulta.aVisible = aVisible
            frmConsulta.sObject = "Txt"
            frmConsulta.oObject = txtNomCheque
            frmConsulta.iColDevTag = 0
            frmConsulta.Show()
        Else
            
        End If
        clConnect.Disconnect()
    End Sub
    Private Sub cmdConsulta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdConsulta.Click
        Dim sSql As String
        Dim aArray As New ArrayList
        Dim aVisible As New ArrayList
        sSql = "select c.codigo, c.cod_moneda, m.nombre nom_moneda, c.nombre,  ch.cod_chequera , ch.codigo chequera"
        sSql = sSql & " from ctasbancarias c"
        sSql = sSql & " inner join monedas m on m.codigo = c.cod_moneda"
        sSql = sSql & " inner join chequeras ch on ch.cod_empre = c.cod_empre and "
        sSql = sSql & " ch.codigo = c.codigo and ch.cod_moneda = c.cod_moneda"
        sSql = sSql & " and ch.sn_activa = -1"
        sSql = sSql & " where c.cod_empre = " & clFunciones.cboGetItem(frmMenu.cboEmpresas, 1)

        If Len(LTrim(RTrim(txtCta.Text))) > 0 Then
            sSql = sSql & " and c.codigo like '" & LTrim(RTrim(txtCta.Text)) & "%'"
        End If
        aArray.Clear()
        aArray.Add("Código")
        aArray.Add("Cod. Moneda")
        aArray.Add("Moneda")
        aArray.Add("Nombre Cuenta")
        aArray.Add("Chequera No.")
        aArray.Add("Chequera")
        aVisible.Clear()
        aVisible.Add("True")
        aVisible.Add("False")
        aVisible.Add("True")
        aVisible.Add("True")
        aVisible.Add("True")
        aVisible.Add("True")
        frmConsulta.iCols = 6
        frmConsulta.sSql = sSql
        frmConsulta.aHeaders = aArray
        frmConsulta.aVisible = aVisible
        frmConsulta.sObject = "Txt"
        frmConsulta.oObject = txtCta
        frmConsulta.iColDevTag = 4
        frmConsulta.sInStr = "inner join chequeras ch"
        frmConsulta.Show()
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        clConnect.Connect(frmLogin.sStringC)
        If frmMenu.ContabilidadToolStripMenuItem.Visible = True Then
            txtFecha.Text = clConnect.Today
        Else
            txtFecha.Text = Today
        End If
        txtFecha.Text = Mid(txtFecha.Text, 1, 10)
        txtCta.Text = ""
        txtNombre.Text = ""
        txtCta.Tag = ""
        chkCircula.Checked = False
        chkNNego.Checked = False
        txtDescrip.Text = ""
        txtFecCheque.Text = ""
        txtMonto.Text = ""
        txtNomCheque.Text = ""
        iNro_Chequera = 0
        lnroPol = 0
        dFecPol = ""
        Me.txtFecCheque.Text = txtFecha.Text
        clConnect.Disconnect()

    End Sub

    

    

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim sSql As String
        Dim sSql1 As String
        Dim nro_correla As String
        Dim oTran As Odbc.OdbcTransaction
        oTran = Nothing
        If Len(LTrim(RTrim(txtCambio.Text))) = 0 Then
            txtCambio.Text = 1
        End If
        If Len(LTrim(RTrim(txtCta.Text))) > 0 And iNro_Chequera > 0 Then
            sSql1 = " cod_empre = " & clFunciones.cboGetItem(frmMenu.cboEmpresas, 1)
            sSql1 = sSql1 & " and codigo = '" & LTrim(RTrim(txtCta.Text)) & "'"
            sSql1 = sSql1 & " and cod_moneda = " & clFunciones.cboGetItem(cboMonedas, 1) & ""
            sSql1 = sSql1 & " and cod_chequera = " & iNro_Chequera & ""
            nro_correla = clFunciones.getCorrelaNumber("cheque_ult, cheque_ini, cheque_fin", "chequeras", sSql1, "cheque_ult", 0)
            If nro_correla = 0 Then
                MsgBox("Error Recuperando Correlativo...", MsgBoxStyle.Information + MsgBoxStyle.OkOnly, "DELTA")
                Exit Sub
            End If
            lnroPol = 0
            dFecPol = ""
            'INGRESA CONTABILIDAD
            frmIngDiario.bModulo = True
            frmIngDiario.lCodModulo = 2
            frmIngDiario.lCodSub = clFunciones.cboGetItem(Me.cboTCheque, 1)
            frmIngDiario.dMonto = Me.txtMonto.Text * txtCambio.Text
            frmIngDiario.sObs = Me.txtDescrip.Text & ": Cheque Nro. " & nro_correla & " Cta. " & LTrim(RTrim(txtCta.Text))
            frmIngDiario.sCodBco = LTrim(RTrim(txtCta.Text))
            frmIngDiario.Show()
            'FIN CONTA

            clConnect.Connect(frmLogin.sStringC)
            If frmMenu.ContabilidadToolStripMenuItem.Visible = True Then
                txtFecha.Text = clConnect.Today
            Else
                txtFecha.Text = Today
            End If
            txtFecha.Text = Mid(txtFecha.Text, 1, 10)
            oTran = clConnect.oconn.BeginTransaction
            Try
                sSql = " insert into cheques values ("
                sSql = sSql & "" & clFunciones.cboGetItem(frmMenu.cboEmpresas, 1)
                sSql = sSql & ",'" & LTrim(RTrim(txtCta.Text)) & "'"
                sSql = sSql & "," & clFunciones.cboGetItem(cboMonedas, 1)
                sSql = sSql & "," & iNro_Chequera & ""
                sSql = sSql & "," & clFunciones.cboGetItem(cboTCheque, 1) & ""
                sSql = sSql & "," & nro_correla & ""
                If frmLogin.iDB = 2 Or frmLogin.iDB = 1 Then
                    sSql = sSql & ",'" & Format(CDate(LTrim(RTrim(txtFecCheque.Text))), "yyyyMMdd") & "'"
                    sSql = sSql & ", '" & Format(CDate(txtFecha.Text), "yyyyMMdd") & "'"
                Else
                    sSql = sSql & ",'" & LTrim(RTrim(txtFecCheque.Text)) & "'"
                    sSql = sSql & ", '" & txtFecha.Text & "'"
                End If

                sSql = sSql & ",'" & LTrim(RTrim(txtNomCheque.Text)) & "'"
                sSql = sSql & "," & LTrim(RTrim(txtMonto.Text))
                sSql = sSql & ",'" & LTrim(RTrim(txtDescrip.Text)) & "'"
                sSql = sSql & ",'" & frmLogin.sUser & "'"
                sSql = sSql & "," & IIf(chkNNego.Checked = True, "-1", "0") & ""
                sSql = sSql & "," & IIf(chkCircula.Checked = True, "-1", "0") & ""
                sSql = sSql & ",0"
                sSql = sSql & ",0"
                sSql = sSql & ")"
                clConnect.Query(2, sSql, 1, oTran)
                'Graba Datos adicionales
                sSql = " insert into cheques_adic values ("
                sSql = sSql & "" & clFunciones.cboGetItem(frmMenu.cboEmpresas, 1)
                sSql = sSql & ",'" & LTrim(RTrim(txtCta.Text)) & "'"
                sSql = sSql & "," & clFunciones.cboGetItem(cboMonedas, 1)
                sSql = sSql & "," & iNro_Chequera & ""
                sSql = sSql & "," & nro_correla & ""
                If frmLogin.iDB = 2 Or frmLogin.iDB = 1 Then
                    sSql = sSql & ",'" & Format(CDate(LTrim(RTrim(txtFecCheque.Text))), "yyyyMMdd") & "'"
                Else
                    sSql = sSql & ",'" & LTrim(RTrim(txtFecCheque.Text)) & "'"
                End If

                sSql = sSql & ",'" & LTrim(RTrim(UCase(txtHechoPor.Text))) & "'"
                sSql = sSql & ",'" & LTrim(RTrim(UCase(txtRevisadoPor.Text))) & "'"
                sSql = sSql & ",'" & LTrim(RTrim(UCase(txtAutorizadoPor.Text))) & "'"
                sSql = sSql & ")"
                clConnect.Query(2, sSql, 1, oTran)
                'Graba datos cuentas por pagar
                If Me.Tag = "frmCxPPagoDoc" Then
                    sSql = " insert into cuentas_x_pagar_ch values ("
                    sSql = sSql & "" & clFunciones.cboGetItem(frmMenu.cboEmpresas, 1)
                    sSql = sSql & "," & clFunciones.cboGetItem(cboMonedas, 1)
                    sSql = sSql & ",'" & LTrim(RTrim(frmCxPPagoDoc.txtNit.Text)) & "'"
                    sSql = sSql & ",'" & LTrim(RTrim(frmCxPPagoDoc.txtSerie.Text)) & "'"
                    sSql = sSql & "," & LTrim(RTrim(frmCxPPagoDoc.txtFac.Text)) & ""
                    If frmLogin.iDB = 2 Or frmLogin.iDB = 1 Then
                        sSql = sSql & ",'" & Format(CDate(LTrim(RTrim(txtFecha.Text))), "yyyyMMdd") & "'"
                    Else
                        sSql = sSql & ",'" & LTrim(RTrim(txtFecha.Text)) & "'"
                    End If

                    sSql = sSql & "," & iNro_Chequera & ""
                    sSql = sSql & "," & nro_correla & ""
                    If frmLogin.iDB = 2 Or frmLogin.iDB = 1 Then
                        sSql = sSql & ",'" & Format(CDate(LTrim(RTrim(txtFecCheque.Text))), "yyyyMMdd") & "'"
                    Else
                        sSql = sSql & ",'" & LTrim(RTrim(txtFecCheque.Text)) & "'"
                    End If

                    sSql = sSql & ",-1,0"
                    sSql = sSql & ",'" & frmLogin.sUser & "'"
                    sSql = sSql & "," & LTrim(RTrim(txtMonto.Text))
                    sSql = sSql & "," & LTrim(RTrim(txtMonto.Text))
                    sSql = sSql & ")"
                    clConnect.Query(2, sSql, 1, oTran)
                    frmCxPPagoDoc.Close()
                End If
                If Me.Tag = "frmImpPagoPlan" Then
                    sSql = " update planillas set sn_pagado = -1"
                    sSql = sSql & " where cod_empre = " & clFunciones.cboGetItem(frmMenu.cboEmpresas, 1)
                    sSql = sSql & " and codigo = 0" & frmImpPagoPlan.txtNroPlanilla.Text
                    sSql = sSql & " and cod_emp = 0" & frmImpPagoPlan.dgDist.CurrentRow.Cells.Item(0).Value
                    clConnect.Query(3, sSql, 1, oTran)
                End If
                oTran.Commit()
                clConnect.Disconnect()
                ImprimeChequeDirecto()
                MsgBox("Datos Grabados con éxito, Cheque Generado : " & nro_correla, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, "DELTA")
                txtUltChGen.Text = nro_correla
                cmdCancel_Click(sender, e)
                If Me.Tag = "frmCxPPagoDoc" Then
                    Me.Close()
                    frmCxPPagoDoc.Show()
                End If
                If Me.Tag = "frmImpPagoPlan" Then
                    Me.Close()
                    frmImpPagoPlan.Tag = "REFRESH"
                    frmImpPagoPlan.Show()
                End If
            Catch err As Exception
                If oTran.Connection.State = ConnectionState.Open Then
                    oTran.Rollback()
                End If
                If clConnect.oconn.State = ConnectionState.Open Then
                    clConnect.Disconnect()
                End If
                MsgBox(err.Message, MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "DELTA")
            End Try

        End If
    End Sub

    Private Sub txtNomCheque_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNomCheque.LostFocus
        txtNomCheque.Text = UCase(txtNomCheque.Text)
    End Sub

    Private Sub txtDescrip_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDescrip.LostFocus
        txtDescrip.Text = UCase(txtDescrip.Text)
    End Sub

    
    

    Private Sub btnPreview_Click(sender As Object, e As EventArgs) Handles btnPreview.Click
        ImprimeCheque()

    End Sub

    Private Sub ImprimeCheque()
        PrieviewCh.sFecCh = Me.txtFecCheque.Text
        PrieviewCh.sBenef = Me.txtNomCheque.Text
        PrieviewCh.sImporte = Me.txtMonto.Text
        PrieviewCh.iNoNeg = IIf(Me.chkNNego.Checked = True, -1, 0)
        PrieviewCh.sCuenta = Me.txtCta.Text
        PrieviewCh.sFecPol = ""
        PrieviewCh.lNroPol = 0
        PrieviewCh.iPreview = -1
        PrieviewCh.lCpto = clFunciones.cboGetItem(cboTCheque, 1)
        PrieviewCh.sDescrip = Me.txtDescrip.Text
        PrieviewCh.sUltGen = CInt(Me.txtUltChGen.Text) + 1
        PrieviewCh.sHechoPor = Me.txtHechoPor.Text
        PrieviewCh.sRevisadoPor = Me.txtRevisadoPor.Text
        PrieviewCh.sAutorizadoPor = Me.txtAutorizadoPor.Text
        PrieviewCh.Show()
        PrieviewCh.Close()
    End Sub

    Private Sub ImprimeChequeDirecto()
        PrieviewCh.sFecCh = Me.txtFecCheque.Text
        PrieviewCh.sBenef = Me.txtNomCheque.Text
        PrieviewCh.sImporte = Me.txtMonto.Text
        PrieviewCh.iNoNeg = IIf(Me.chkNNego.Checked = True, -1, 0)
        PrieviewCh.sCuenta = Me.txtCta.Text
        PrieviewCh.sFecPol = Me.dFecPol
        PrieviewCh.lNroPol = Me.lnroPol
        PrieviewCh.iPreview = 0
        PrieviewCh.sHechoPor = Me.txtHechoPor.Text
        PrieviewCh.sRevisadoPor = Me.txtRevisadoPor.Text
        PrieviewCh.sAutorizadoPor = Me.txtAutorizadoPor.Text
        PrieviewCh.Show()
    End Sub

    Private Sub btnGirado_Click(sender As Object, e As EventArgs) Handles btnGirado.Click
        buscaProveedor()

    End Sub
End Class