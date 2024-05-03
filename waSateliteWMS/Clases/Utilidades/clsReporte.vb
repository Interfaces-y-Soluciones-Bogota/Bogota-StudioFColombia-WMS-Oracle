Public Class clsReporte

    Public Function almacenarDatosDataSet(ByVal dsOrigen As DataSet, ByVal dsOrigenReporte As DataSet, ByVal erroresDataSet As DataSet) As DataTable
        Dim dtResultado As DataTable = CrearDataTable()
        Dim dvOrigenReporte As New DataView(dsOrigenReporte.Tables(0))
        Dim dvErrores As New DataView(erroresDataSet.Tables(0))
        Dim contFilas As Int16
        contFilas = 2

        For Each Registro As DataRow In dsOrigen.Tables(0).Rows
            dvOrigenReporte.RowFilter = "f350_notas = '" & Registro.Item("f350_notas") & "'"

            dvErrores.RowFilter = "f_nro_linea=" & contFilas

            Dim dr As DataRow = dtResultado.NewRow
            dr.Item("ID DESPACHO") = Registro.Item("idDespacho")
            dr.Item("NÚMERO CARGA") = dvOrigenReporte.ToTable.Rows(0).Item("NÚMERO CARGA")
            dr.Item("NÚMERO RQI") = dvOrigenReporte.ToTable.Rows(0).Item("NÚMERO RQI")
            dr.Item("NÚMERO TRASLADO GENERADO") = Registro.Item("f350_id_tipo_docto") & Registro.Item("f350_consec_docto")
            dr.Item("CÓDIGO DESTINO") = " " & dvOrigenReporte.ToTable.Rows(0).Item("CÓDIGO DESTINO")
            dr.Item("NOMBRE DESTINO") = dvOrigenReporte.ToTable.Rows(0).Item("NOMBRE DESTINO")
            dr.Item("UNIDADES TOTALES") = dvOrigenReporte.ToTable.Rows(0).Item("UNIDADES TOTALES")
            dr.Item("NÚMERO CARTÓN ASOCIADO") = dvOrigenReporte.ToTable.Rows(0).Item("NÚMERO CARTÓN ASOCIADO")

            If dvErrores.ToTable.Rows.Count = 1 Then
                dr.Item("MOTIVO/OBSERVACIÓN") = dvErrores.ToTable.Rows(0).Item("f_detalle")
            Else
                dr.Item("MOTIVO/OBSERVACIÓN") = "Exitoso"
            End If

            dtResultado.Rows.Add(dr)
            contFilas += 1
        Next

        Return dtResultado

    End Function

    Private Function CrearDataTable() As DataTable
        Dim dtRegistros As New DataTable("registros")

        'Registros
        dtRegistros.Columns.Add("ID DESPACHO", GetType(Integer))
        dtRegistros.Columns.Add("NÚMERO CARGA", GetType(String))
        dtRegistros.Columns.Add("NÚMERO RQI", GetType(String))
        dtRegistros.Columns.Add("NÚMERO TRASLADO GENERADO", GetType(String))
        dtRegistros.Columns.Add("CÓDIGO DESTINO", GetType(String))
        dtRegistros.Columns.Add("NOMBRE DESTINO", GetType(String))
        dtRegistros.Columns.Add("UNIDADES TOTALES", GetType(Integer))
        dtRegistros.Columns.Add("NÚMERO CARTÓN ASOCIADO", GetType(String))
        dtRegistros.Columns.Add("MOTIVO/OBSERVACIÓN", GetType(String))

        Return dtRegistros

    End Function
End Class
