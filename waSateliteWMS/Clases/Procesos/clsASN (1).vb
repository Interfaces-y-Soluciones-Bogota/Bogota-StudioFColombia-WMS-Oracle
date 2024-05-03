Imports System.Data.SqlClient
Public Class clsASN
    Inherits clsConfiguracion

    Dim objTarea As New clsTarea
    Public Property FechaInicial As String
    Public Property FechaFinal As String
    Public Sub importarASN(ByVal TipoASN As String)
        Dim objCorreo As New clsCorreo

        Try
            If logInicial(Environment.GetCommandLineArgs()) Then

                Try
                    objTarea.LogFechaInicioRecuperacionDatosOrigen()
                    objTarea.LogFechaInicioGeneracionPlano()
                    objTarea.LogFechaInicioWebServiceSiesa()

                    Dim objAsn As New wsASNOracle.AsnService
                    Dim objListaASN() As wsASNOracle.Asn
                    Dim objRegistroASN As wsASNOracle.Asn
                    Dim objListaItemsASN As New wsASNOracle.AsnDetail
                    Dim verificacion As Int16

                    'Dim f1 As Date = CDate("01/22/2018")
                    'Dim f2 As Date = CDate("01/24/2018")

                    'objListaASN = objAsn.GetAvailableAsns(f1, True, f2, True)

                    objListaASN = objAsn.GetAvailableAsns(CDate(FechaInicial).AddDays(-1), True, CDate(FechaFinal).AddDays(1), True)


                    If Not objListaASN Is Nothing Then
                        For Each objRegistroASN In objListaASN
                            If TipoASN = objRegistroASN.Type.ToString.Replace(" ", "") Then
                                guardarEncabezadoTmpASN(objRegistroASN.Date, objRegistroASN.Destiny, objRegistroASN.Document, objRegistroASN.Id, objRegistroASN.Origin, objRegistroASN.Prefix, objRegistroASN.Type, verificacion)
                                If verificacion = 1 Then
                                    For Each objListaItemsASN In objRegistroASN.Items
                                        guardarDetalleTmpASN(objListaItemsASN.Color, objListaItemsASN.Id, objListaItemsASN.LpnNumber, objListaItemsASN.Price, objListaItemsASN.ProductType, objListaItemsASN.Quantity, objListaItemsASN.Reference, objListaItemsASN.Size)
                                    Next
                                End If
                            End If
                        Next
                    End If

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
                    objCorreo.EnviarCorreoTarea("GTIntegration-WMS: ASN", objTarea.Destinatarios, ex.Message, objTarea.Tarea)
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

    Public Sub encabezadoASN_GuardarEnvio_TAL()

        Try
            Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)
            Dim sqlComando As SqlCommand = New SqlCommand
            Dim sqlAdaptador As SqlDataAdapter = New SqlDataAdapter
            Dim dsResultado As New DataSet

            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = "sp_WMS_EncabezadoASN_GuardarEnvio_TAL"
            sqlAdaptador.SelectCommand = sqlComando

            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub encabezadoASN_GuardarEnvio_XDOCK()

        Try
            Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)
            Dim sqlComando As SqlCommand = New SqlCommand
            Dim sqlAdaptador As SqlDataAdapter = New SqlDataAdapter
            Dim dsResultado As New DataSet

            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = "sp_WMS_EncabezadoASN_GuardarEnvio_XDOCK"
            sqlAdaptador.SelectCommand = sqlComando

            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub encabezadoASN_GuardarEnvio_TRA()

        Try
            Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)
            Dim sqlComando As SqlCommand = New SqlCommand
            Dim sqlAdaptador As SqlDataAdapter = New SqlDataAdapter
            Dim dsResultado As New DataSet

            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = "sp_WMS_EncabezadoASN_GuardarEnvio_TRA"
            sqlAdaptador.SelectCommand = sqlComando

            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub encabezadoASN_GuardarEnvio_DEV()

        Try
            Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)
            Dim sqlComando As SqlCommand = New SqlCommand
            Dim sqlAdaptador As SqlDataAdapter = New SqlDataAdapter
            Dim dsResultado As New DataSet

            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = "sp_WMS_EncabezadoASN_GuardarEnvio_DEV"
            sqlAdaptador.SelectCommand = sqlComando

            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub detalleASN_GuardarEnvio_TAL()

        Try
            Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)
            Dim sqlComando As SqlCommand = New SqlCommand
            Dim sqlAdaptador As SqlDataAdapter = New SqlDataAdapter
            Dim dsResultado As New DataSet

            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = "sp_WMS_DetalleASN_GuardarEnvio_TAL"
            sqlAdaptador.SelectCommand = sqlComando

            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub detalleASN_GuardarEnvio_XDOCK()

        Try
            Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)
            Dim sqlComando As SqlCommand = New SqlCommand
            Dim sqlAdaptador As SqlDataAdapter = New SqlDataAdapter
            Dim dsResultado As New DataSet

            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = "sp_WMS_DetalleASN_GuardarEnvio_XDOCK"
            sqlAdaptador.SelectCommand = sqlComando

            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub detalleASN_GuardarEnvio_TRA()

        Try
            Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)
            Dim sqlComando As SqlCommand = New SqlCommand
            Dim sqlAdaptador As SqlDataAdapter = New SqlDataAdapter
            Dim dsResultado As New DataSet

            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = "sp_WMS_DetalleASN_GuardarEnvio_TRA"
            sqlAdaptador.SelectCommand = sqlComando

            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub detalleASN_GuardarEnvio_DEV()

        Try
            Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)
            Dim sqlComando As SqlCommand = New SqlCommand
            Dim sqlAdaptador As SqlDataAdapter = New SqlDataAdapter
            Dim dsResultado As New DataSet

            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = "sp_WMS_DetalleASN_GuardarEnvio_DEV"
            sqlAdaptador.SelectCommand = sqlComando

            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub encabezadoASN_Eliminar_TAL()

        Try
            Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)
            Dim sqlComando As SqlCommand = New SqlCommand
            Dim sqlAdaptador As SqlDataAdapter = New SqlDataAdapter
            Dim dsResultado As New DataSet

            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = "sp_WMS_EncabezadoASN_Eliminar_TAL"
            sqlAdaptador.SelectCommand = sqlComando

            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub encabezadoASN_Eliminar_XDOCK()

        Try
            Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)
            Dim sqlComando As SqlCommand = New SqlCommand
            Dim sqlAdaptador As SqlDataAdapter = New SqlDataAdapter
            Dim dsResultado As New DataSet

            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = "sp_WMS_EncabezadoASN_Eliminar_XDOCK"
            sqlAdaptador.SelectCommand = sqlComando

            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub encabezadoASN_Eliminar_TRA()

        Try
            Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)
            Dim sqlComando As SqlCommand = New SqlCommand
            Dim sqlAdaptador As SqlDataAdapter = New SqlDataAdapter
            Dim dsResultado As New DataSet

            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = "sp_WMS_EncabezadoASN_Eliminar_TRA"
            sqlAdaptador.SelectCommand = sqlComando

            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub encabezadoASN_Eliminar_DEV()

        Try
            Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)
            Dim sqlComando As SqlCommand = New SqlCommand
            Dim sqlAdaptador As SqlDataAdapter = New SqlDataAdapter
            Dim dsResultado As New DataSet

            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = "sp_WMS_EncabezadoASN_Eliminar_DEV"
            sqlAdaptador.SelectCommand = sqlComando

            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub detalleASN_Eliminar_TAL()

        Try
            Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)
            Dim sqlComando As SqlCommand = New SqlCommand
            Dim sqlAdaptador As SqlDataAdapter = New SqlDataAdapter
            Dim dsResultado As New DataSet

            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = "sp_WMS_DetalleASN_EliminarTAL"
            sqlAdaptador.SelectCommand = sqlComando
            sqlAdaptador.SelectCommand = sqlComando

            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub detalleASN_Eliminar_XDOCK()

        Try
            Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)
            Dim sqlComando As SqlCommand = New SqlCommand
            Dim sqlAdaptador As SqlDataAdapter = New SqlDataAdapter
            Dim dsResultado As New DataSet

            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = "sp_WMS_DetalleASN_EliminarXDOCK"
            sqlAdaptador.SelectCommand = sqlComando
            sqlAdaptador.SelectCommand = sqlComando

            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub detalleASN_Eliminar_TRA()

        Try
            Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)
            Dim sqlComando As SqlCommand = New SqlCommand
            Dim sqlAdaptador As SqlDataAdapter = New SqlDataAdapter
            Dim dsResultado As New DataSet

            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = "sp_WMS_DetalleASN_EliminarTRA"
            sqlAdaptador.SelectCommand = sqlComando
            sqlAdaptador.SelectCommand = sqlComando

            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub detalleASN_Eliminar_DEV()

        Try
            Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)
            Dim sqlComando As SqlCommand = New SqlCommand
            Dim sqlAdaptador As SqlDataAdapter = New SqlDataAdapter
            Dim dsResultado As New DataSet



            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = "sp_WMS_DetalleASN_EliminarDEV"
            sqlAdaptador.SelectCommand = sqlComando
            sqlAdaptador.SelectCommand = sqlComando

            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub


    Public Sub encabezadoASN_ActualizarConsecutivo_TRA()

        Try
            Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)
            Dim sqlComando As SqlCommand = New SqlCommand
            Dim sqlAdaptador As SqlDataAdapter = New SqlDataAdapter
            Dim dsResultado As New DataSet

            sqlComando.CommandTimeout = 1800000
            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = "sp_WMS_Consecutivo_TRA"
            sqlAdaptador.SelectCommand = sqlComando
            sqlAdaptador.SelectCommand = sqlComando

            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub encabezadoASN_ActualizarConsecutivo_DEV()

        Try
            Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)
            Dim sqlComando As SqlCommand = New SqlCommand
            Dim sqlAdaptador As SqlDataAdapter = New SqlDataAdapter
            Dim dsResultado As New DataSet

            sqlComando.CommandTimeout = 1800000
            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = "sp_WMS_Consecutivo_DEV"
            sqlAdaptador.SelectCommand = sqlComando
            sqlAdaptador.SelectCommand = sqlComando

            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub



    Private Sub guardarEncabezadoTmpASN(ByVal Fecha As String, ByVal Destiny As String, ByVal Document As String, ByVal Id As Integer, ByVal Origin As String, ByVal Prefix As String, ByVal TipoASN As String, ByRef Verificacion As Int16)

        Try
            Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)
            Dim sqlComando As SqlCommand = New SqlCommand
            Dim sqlAdaptador As SqlDataAdapter = New SqlDataAdapter
            Dim dsResultado As New DataSet
            Dim FechaFormatoAlmacenar As String = ""

            If Fecha <> "" Then
                FechaFormatoAlmacenar = CDate(Fecha).ToString("yyyMMdd hh:mm:ss")
            End If

            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = "sp_WMS_EncabezadoASNtmp_Guardar"
            sqlAdaptador.SelectCommand = sqlComando
            sqlComando.Parameters.AddWithValue("@Date", FechaFormatoAlmacenar)
            sqlComando.Parameters.AddWithValue("@Destiny", Destiny)
            sqlComando.Parameters.AddWithValue("@Document", Document)
            sqlComando.Parameters.AddWithValue("@Id", Id)
            sqlComando.Parameters.AddWithValue("@Origin", Origin)
            sqlComando.Parameters.AddWithValue("@Prefix", Prefix)
            sqlComando.Parameters.AddWithValue("@TipoASN", TipoASN)

            Dim objParametroVerificacion As New SqlParameter
            objParametroVerificacion.Direction = ParameterDirection.InputOutput
            objParametroVerificacion.ParameterName = "@Verificacion"
            objParametroVerificacion.Value = Verificacion
            sqlComando.Parameters.Add(objParametroVerificacion)

            sqlAdaptador.SelectCommand = sqlComando

            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

            Verificacion = sqlComando.Parameters(7).Value

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub guardarDetalleTmpASN(ByVal Color As String, ByVal Id As Integer, ByVal LpnNumber As String, ByVal Price As Decimal, ByVal ProductType As String, ByVal Quantity As Decimal, ByVal Reference As String, ByVal Size As String)

        Try
            Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)
            Dim sqlComando As SqlCommand = New SqlCommand
            Dim sqlAdaptador As SqlDataAdapter = New SqlDataAdapter
            Dim dsResultado As New DataSet

            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = "sp_WMS_DetalleASNtmp_Guardar"
            sqlAdaptador.SelectCommand = sqlComando
            sqlComando.Parameters.AddWithValue("@Color", Color)
            sqlComando.Parameters.AddWithValue("@Id", Id)
            sqlComando.Parameters.AddWithValue("@LpnNumber", LpnNumber)
            sqlComando.Parameters.AddWithValue("@Price", Price)
            sqlComando.Parameters.AddWithValue("@ProductType", ProductType)
            sqlComando.Parameters.AddWithValue("@Quantity", Quantity)
            sqlComando.Parameters.AddWithValue("@Reference", Reference)
            sqlComando.Parameters.AddWithValue("@Size", Size)

            sqlAdaptador.SelectCommand = sqlComando

            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub generarASN(ByVal TipoASN As String)
        Dim objCorreo As New clsCorreo

        If logInicial(Environment.GetCommandLineArgs()) Then
            Try
                Dim dsTraslados As DataSet
                dsTraslados = consultarTrasladosPendientes(TipoASN)
                If dsTraslados.Tables(0).Rows.Count <> 0 Then

                    objTarea.LogFechaInicioRecuperacionDatosOrigen()
                    objTarea.LogFechaInicioGeneracionPlano()
                    objTarea.LogFechaInicioWebServiceSiesa()

                    Dim objAsn As New wsASNOracle.AsnService
                    Dim objRegistroASN As wsASNOracle.Asn
                    Dim objtemsASN As wsASNOracle.AsnDetail
                    Dim resultado As New wsASNOracle.Transaction
                    Dim ContItem As Integer = 0
                    Dim ContAsN As Integer = 0
                    Dim dvEncabezado As New DataView(dsTraslados.Tables(0))
                    Dim dvDetalle As New DataView(dsTraslados.Tables(1))

                    For Each Documento As DataRow In dvEncabezado.ToTable.Rows
                        Dim objListaASN(0) As wsASNOracle.Asn

                        ContItem = 0
                        ContAsN = 0
                        objRegistroASN = New wsASNOracle.Asn
                        objRegistroASN.Date = CDate(Documento.Item("Date")).ToString("yyyy-MM-dd")
                        objRegistroASN.Destiny = Documento.Item("Destiny")
                        objRegistroASN.Document = Documento.Item("Document")
                        objRegistroASN.Id = Documento.Item("Id")
                        objRegistroASN.Origin = Documento.Item("Origin")
                        objRegistroASN.Prefix = Documento.Item("Prefix")
                        objRegistroASN.Type = Documento.Item("Type")

                        objRegistroASN.IdSpecified = True
                        objRegistroASN.DateSpecified = True

                        dvDetalle.RowFilter = "f350_rowid=" & Documento.Item("f350_rowid") & " And  f350_id_co = " & Documento.Item("f350_id_co")
                        Dim objListatemsASN(dvDetalle.ToTable.Rows.Count - 1) As wsASNOracle.AsnDetail

                        For Each Item As DataRow In dvDetalle.ToTable.Rows
                            objtemsASN = New wsASNOracle.AsnDetail
                            objtemsASN.Color = Item.Item("Color")
                            objtemsASN.Id = Item.Item("Id")
                            objtemsASN.LpnNumber = Item.Item("LpnNumber")

                            If Item.Item("Price") <> "" Then
                                objtemsASN.Price = Item.Item("Price")
                                objtemsASN.PriceSpecified = True
                            Else
                                objtemsASN.PriceSpecified = False
                            End If

                            objtemsASN.ProductType = Item.Item("ProductType")
                            objtemsASN.Quantity = CInt(Item.Item("Quantity"))
                            objtemsASN.Reference = Item.Item("Reference")
                            objtemsASN.Size = Item.Item("Size")
                            objtemsASN.QuantitySpecified = True
                            objListatemsASN(ContItem) = objtemsASN
                            ContItem += 1
                        Next

                        objRegistroASN.Items = objListatemsASN
                        objListaASN(ContAsN) = objRegistroASN
                        ContAsN += 1

                        resultado = objAsn.SaveAsns(objListaASN)

                        If resultado.IsSuccessful = True Then
                            actualizarIntegrado(Documento.Item("f350_rowid"), Documento.Item("f350_id_co"), TipoASN)
                        End If
                    Next

                    objTarea.LogWebServiceSiesa(1)
                    objTarea.LogFechaFinWebServiceSiesa()
                    objTarea.LogGeneracionDePlano(1)
                    objTarea.LogFechaFinGeneracionPlano()
                    objTarea.LogFechaFinRecuperacionDatosOrigen()
                    objTarea.LogRecuperacionDatosOrigen(1)
                End If
            Catch ex As Exception
                objTarea.LogRecuperacionDatosOrigen(0)
                objTarea.LogMensajesError(ex.Message)
                objTarea.LogGeneracionDePlano(0)
                objTarea.LogMensajesError(ex.Message)
                objTarea.LogWebServiceSiesa(0)
                objTarea.LogMensajesError(ex.Message)
                objCorreo.EnviarCorreoTarea("GTIntegration-WMS: ASN", objTarea.Destinatarios, ex.Message, objTarea.Tarea)
            End Try

            objTarea.LogEjecucionCompleta()
            objTarea.LogFechaFin()
        End If

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
                objCorreo.EnviarCorreoTarea("GTIntegration-WMS: Validaciones", objTarea.Destinatarios, "El argumento enviado debe ser numérico, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea", objTarea.Tarea)
                Return False
            End If
        Else
            objTarea.LogRecuperacionDatosOrigen(0)
            objTarea.LogMensajesError("No se envió el Id de la tarea como argumento al programa, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea")
            objCorreo.EnviarCorreoTarea("GTIntegration-WMS: Validaciones", objTarea.Destinatarios, "No se envió el Id de la tarea como argumento al programa, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea", objTarea.Tarea)
            Return False
        End If

    End Function

    Private Function consultarTrasladosPendientes(ByVal Tipo As String) As DataSet

        Dim dsConfiguracionGT As New DataSet
        Dim objDA As New SqlDataAdapter
        Dim sqlComando As New SqlCommand

        sqlComando.Connection = sqlConexion
        sqlComando.CommandType = CommandType.StoredProcedure
        sqlComando.CommandText = "sp_WMS_EnvioTrasladosWS"
        sqlComando.Parameters.AddWithValue("Tipo", Tipo)
        objDA.SelectCommand = sqlComando

        Try
            objDA.Fill(dsConfiguracionGT)
            Return dsConfiguracionGT

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Private Function actualizarIntegrado(ByVal f350_rowid As Integer, ByVal f350_id_co As Integer, ByVal Tipo As String) As DataSet

        Dim dsConfiguracionGT As New DataSet
        Dim objDA As New SqlDataAdapter
        Dim sqlComando As New SqlCommand

        sqlComando.Connection = sqlConexion
        sqlComando.CommandType = CommandType.StoredProcedure
        sqlComando.CommandText = "sp_WMS_Estados_Traslados"
        sqlComando.Parameters.AddWithValue("f350_rowid", f350_rowid)
        sqlComando.Parameters.AddWithValue("f350_id_co", f350_id_co)
        sqlComando.Parameters.AddWithValue("Tipo", Tipo)
        objDA.SelectCommand = sqlComando

        Try
            objDA.Fill(dsConfiguracionGT)
            Return dsConfiguracionGT

        Catch ex As Exception
            Throw ex
        End Try

    End Function
End Class
