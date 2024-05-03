Imports System.Data.SqlClient

Public Class clsPedidos
    Inherits clsConfiguracion

    Dim objTarea As clsTarea

    Public Sub AlmacenarPedidosPendiente()
        Dim objCorreo As New clsCorreo
        objTarea = New clsTarea

        Try
            If logInicial(Environment.GetCommandLineArgs()) Then

                Try
                    objTarea.LogFechaInicioRecuperacionDatosOrigen()
                    objTarea.LogFechaInicioGeneracionPlano()
                    objTarea.LogFechaInicioWebServiceSiesa()

                    ConsultarPedidos()

                    objTarea.LogWebServiceSiesa(1)
                    objTarea.LogFechaFinWebServiceSiesa()
                    objTarea.LogGeneracionDePlano(1)
                    objTarea.LogFechaFinGeneracionPlano()
                    objTarea.LogFechaFinRecuperacionDatosOrigen()
                    objTarea.LogRecuperacionDatosOrigen(1)
                Catch ex As Exception
                    objTarea.LogRecuperacionDatosOrigen(0)
                    objTarea.LogMensajesError(ex.Message)
                    objTarea.LogGeneracionDePlano(0)
                    objTarea.LogMensajesError(ex.Message)
                    objTarea.LogWebServiceSiesa(0)
                    objTarea.LogMensajesError(ex.Message)
                    objCorreo.EnviarCorreoTarea("WMS -Traslados TR", objTarea.CorreosNotificaciones, ex.Message, objTarea.Tarea)
                End Try

                objTarea.LogEjecucionCompleta()
                objTarea.LogFechaFin()
            End If
        Catch ex As Exception
            objTarea.LogMensajesError(ex.Message)
        Finally
            objTarea.LogFin(objTarea.idLogPrincipal)
        End Try
    End Sub


    Private Sub ConsultarPedidos()

        Dim dsConfiguracionGT As New DataSet
        Dim sqlComando As New SqlCommand


        sqlComando.CommandTimeout = 36000000
        sqlComando.Connection = sqlConexion
        sqlComando.CommandType = CommandType.StoredProcedure
        sqlComando.CommandText = "sp_WMS_PedidosConsultar"

        Try
            sqlComando.Connection.Open()
            sqlComando.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            sqlComando.Connection.Close()
        End Try

    End Sub

    Private Function logInicial(ByVal args As String()) As Boolean
        Dim objCorreo As New clsCorreo

        If args.Length = 2 Then
            If IsNumeric(args(1)) Then
                objTarea.Tarea = args(1)
                objTarea.LogPrincipalAlmacenar()
                objTarea.LogInicio()
                Return True
            Else
                objTarea.LogRecuperacionDatosOrigen(0)
                objTarea.LogMensajesError("El argumento enviado debe ser numérico, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea")
                objCorreo.EnviarCorreoTarea("WMS Validaciones", objTarea.CorreosNotificaciones, "El argumento enviado debe ser numérico, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea", objTarea.Tarea)
                Return False
            End If
        Else
            objTarea.LogRecuperacionDatosOrigen(0)
            objTarea.LogMensajesError("No se envió el Id de la tarea como argumento al programa, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea")
            objCorreo.EnviarCorreoTarea("WMS Validaciones", objTarea.CorreosNotificaciones, "No se envió el Id de la tarea como argumento al programa, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea", objTarea.Tarea)
            Return False
        End If

    End Function


    '    Private Sub guardarEncabezadoPedidos(
    '                ByVal hdr_group_nbr As String,
    '                ByVal facility_code As String,
    '                ByVal company_code As String,
    '                ByVal order_nbr As String,
    '                ByVal order_type As String,
    '                ByVal ord_DateTime As String,
    '                ByVal exp_DateTime As String,
    '                ByVal req_ship_DateTime As String,
    'ByVal dest_facility_code As String,
    'ByVal cust_name As String,
    'ByVal cust_addr As String,
    'ByVal cust_addr2 As String,
    'ByVal cust_addr3 As String,
    'ByVal ref_nbr As String,
    'ByVal action_code As String,
    'ByVal route_nbr As String,
    'ByVal cust_city As String,
    'ByVal cust_state As String,
    'ByVal cust_zip As String,
    'ByVal cust_country As String,
    'ByVal cust_phone_nbr As String,
    'ByVal cust_email As String,
    'ByVal cust_contact As String,
    'ByVal cust_nbr As String,
    'ByVal shipto_facility_code As String,
    'ByVal shipto_name As String,
    'ByVal shipto_addr As String,
    'ByVal shipto_addr2 As String,
    'ByVal shipto_addr3 As String,
    'ByVal shipto_city As String,
    'ByVal shipto_state As String,
    'ByVal shipto_zip As String,
    'ByVal shipto_country As String,
    'ByVal shipto_phone_nbr As String,
    'ByVal shipto_email As String,
    'ByVal shipto_contact As String,
    'ByVal dest_company_code As String,
    'ByVal As Decimal,
    'ByVal ship_via_code As String,
    'ByVal carrier_account_nbr As String,
    'ByVal payment_method As String,
    'ByVal host_allocation_nbr As String,
    'ByVal customer_po_nbr As String,
    'ByVal sales_order_nbr As String,
    'ByVal sales_channel As String,
    'ByVal dest_dept_nbr As String,
    'ByVal start_ship_DateTime As String,
    'ByVal stop_ship_DateTime As String,
    'ByVal spl_instr As String,
    'ByVal vas_group_code As String,
    'ByVal currency_code As String,
    'ByVal stage_location_barcode As String,
    'ByVal cust_field_1 As String,
    'ByVal cust_field_2 As String,
    'ByVal cust_field_3 As String,
    'ByVal cust_field_4 As String,
    'ByVal cust_field_5 As String,
    'ByVal ob_lpn_type As String,
    'ByVal gift_msg As String,
    'ByVal sched_ship_DateTime As String,
    'ByVal customer_po_type As String,
    'ByVal customer_vendor_code As String,
    'ByVal cust_date_1 As String,
    'ByVal cust_date_2 As String,
    'ByVal cust_date_3 As String,
    'ByVal cust_date_4 As String,
    'ByVal cust_date_5 As String,
    'ByVal cust_number_1 As Integer,
    'ByVal cust_number_2 As Integer,
    'ByVal cust_number_3 As Integer,
    'ByVal cust_number_4 As Integer,
    'ByVal cust_number_5 As Integer,
    'ByVal cust_decimal_1 As Decimal,
    'ByVal cust_decimal_2 As Decimal,
    'ByVal cust_decimal_3 As Decimal,
    'ByVal cust_decimal_4 As Decimal,
    'ByVal cust_decimal_5 As Decimal,
    'ByVal cust_short_text_1 As String,
    'ByVal cust_short_text_2 As String,
    'ByVal cust_short_text_3 As String,
    'ByVal cust_short_text_4 As String,
    'ByVal cust_short_text_5 As String,
    'ByVal cust_short_text_6 As String,
    'ByVal cust_short_text_7 As String,
    'ByVal cust_short_text_8 As String,
    'ByVal cust_short_text_9 As String,
    'ByVal cust_short_text_10 As String,
    'ByVal cust_short_text_11 As String,
    'ByVal cust_short_text_12 As String,
    'ByVal cust_long_text_1 As String,
    'ByVal cust_long_text_2 As String,
    'ByVal cust_long_text_3 As String,
    'ByVal order_nbr_to_replace)	As String,

    '        )

    '        Try
    '            Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)
    '            Dim sqlComando As SqlCommand = New SqlCommand
    '            Dim sqlAdaptador As SqlDataAdapter = New SqlDataAdapter
    '            Dim dsResultado As New DataSet

    '            sqlComando.Connection = sqlConexion
    '            sqlComando.CommandType = CommandType.StoredProcedure
    '            sqlComando.CommandText = "sp_WMS_SVS_CABECERA_VERIFICACION_ASN_Guardar"
    '            sqlAdaptador.SelectCommand = sqlComando

    '            sqlComando.Parameters.AddWithValue("@hdr_group_nbr", hdr_group_nbr)
    '            sqlComando.Parameters.AddWithValue("@shipment_nbr", shipment_nbr)
    '            sqlComando.Parameters.AddWithValue("@facility_code", facility_code)
    '            sqlComando.Parameters.AddWithValue("@company_code", company_code)
    '            sqlComando.Parameters.AddWithValue("@trailer_nbr", trailer_nbr)
    '            sqlComando.Parameters.AddWithValue("@ref_nbr", ref_nbr)
    '            sqlComando.Parameters.AddWithValue("@shipment_type", shipment_type)
    '            sqlComando.Parameters.AddWithValue("@load_nbr", load_nbr)
    '            sqlComando.Parameters.AddWithValue("@manifest_nbr", manifest_nbr)
    '            sqlComando.Parameters.AddWithValue("@trailer_type", trailer_type)
    '            sqlComando.Parameters.AddWithValue("@vendor_info", vendor_info)
    '            sqlComando.Parameters.AddWithValue("@origin_info", origin_info)
    '            sqlComando.Parameters.AddWithValue("@origin_code", origin_code)
    '            sqlComando.Parameters.AddWithValue("@orig_shipped_units", orig_shipped_units)
    '            sqlComando.Parameters.AddWithValue("@shipped_date", shipped_date)
    '            sqlComando.Parameters.AddWithValue("@orig_shipped_lpns", orig_shipped_lpns)
    '            sqlComando.Parameters.AddWithValue("@shipment_hdr_cust_field_1", shipment_hdr_cust_field_1)
    '            sqlComando.Parameters.AddWithValue("@shipment_hdr_cust_field_2", shipment_hdr_cust_field_2)
    '            sqlComando.Parameters.AddWithValue("@shipment_hdr_cust_field_3", shipment_hdr_cust_field_3)
    '            sqlComando.Parameters.AddWithValue("@shipment_hdr_cust_field_4", shipment_hdr_cust_field_4)
    '            sqlComando.Parameters.AddWithValue("@shipment_hdr_cust_field_5", shipment_hdr_cust_field_5)
    '            sqlComando.Parameters.AddWithValue("@verification_date", verification_date)
    '            sqlAdaptador.SelectCommand = sqlComando
    '            sqlConexion.Open()
    '            sqlComando.ExecuteNonQuery()

    '        Catch ex As Exception
    '            Throw ex
    '        End Try
    '    End Sub
End Class
