Imports System.Data.SqlClient

Public Class clsTraslados
    Inherits clsConfiguracion

    Dim objTarea As clsTarea



    Public Sub AlmacenarTrasladosPendiente(ByVal TipoTraslado As String)
        Dim objCorreo As New clsCorreo
        objTarea = New clsTarea

        Try
            If logInicial(Environment.GetCommandLineArgs()) Then

                Try
                    objTarea.LogFechaInicioRecuperacionDatosOrigen()
                    objTarea.LogFechaInicioGeneracionPlano()
                    objTarea.LogFechaInicioWebServiceSiesa()

                    ConsultarTraslados(TipoTraslado)

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
                    objCorreo.EnviarCorreoTarea("GTIntegration-WMS: Traslados TR", objTarea.Destinatarios, ex.Message, objTarea.Tarea)
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

    Private Sub ConsultarTraslados(ByVal TipoTraslado As String)

        Dim dsConfiguracionGT As New DataSet
        Dim sqlComando As New SqlCommand


        sqlComando.CommandTimeout = 36000000
        sqlComando.Connection = sqlConexion
        sqlComando.CommandType = CommandType.StoredProcedure
        sqlComando.CommandText = "sp_WMS_TrasladosConsultar"
        sqlComando.Parameters.AddWithValue("Tipo", TipoTraslado)

        Try
            sqlComando.Connection.Open()
            sqlComando.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            sqlComando.Connection.Close()
        End Try

    End Sub

    Public Sub AlmacenarPedidosPendientesEcommerce()
        Dim objCorreo As New clsCorreo
        objTarea = New clsTarea

        Try
            If logInicial(Environment.GetCommandLineArgs()) Then

                Try
                    objTarea.LogFechaInicioRecuperacionDatosOrigen()
                    objTarea.LogFechaInicioGeneracionPlano()
                    objTarea.LogFechaInicioWebServiceSiesa()

                    ConsultarPedidosPendientesEcommerce()

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
                    objCorreo.EnviarCorreoTarea("GTIntegration-WMS: Consulta pedidos Ecommerce ", objTarea.Destinatarios, ex.Message, objTarea.Tarea)
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

    Public Sub AlmacenarRequisicionesPendientesRQI()
        Dim objCorreo As New clsCorreo
        objTarea = New clsTarea

        Try
            If logInicial(Environment.GetCommandLineArgs()) Then

                Try
                    objTarea.LogFechaInicioRecuperacionDatosOrigen()
                    objTarea.LogFechaInicioGeneracionPlano()
                    objTarea.LogFechaInicioWebServiceSiesa()

                    ConsultarRequisicionesPendientesRQI()

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
                    objCorreo.EnviarCorreoTarea("GTIntegration-WMS: Consulta RQI", objTarea.Destinatarios, ex.Message, objTarea.Tarea)
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

    Public Sub AlmacenarPedidosPendientesPV_PVI()
        Dim objCorreo As New clsCorreo
        objTarea = New clsTarea

        Try
            If logInicial(Environment.GetCommandLineArgs()) Then

                Try
                    objTarea.LogFechaInicioRecuperacionDatosOrigen()
                    objTarea.LogFechaInicioGeneracionPlano()
                    objTarea.LogFechaInicioWebServiceSiesa()

                    ConsultarPedidosPendientesPV_PVI()

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
                    objCorreo.EnviarCorreoTarea("GTIntegration-WMS: Consulta Pedidos PV_PVI", objTarea.Destinatarios, ex.Message, objTarea.Tarea)
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

    Private Sub ConsultarPedidosPendientesEcommerce()

        Dim dsConfiguracionGT As New DataSet
        Dim sqlComando As New SqlCommand


        sqlComando.CommandTimeout = 36000000
        sqlComando.Connection = sqlConexion
        sqlComando.CommandType = CommandType.StoredProcedure
        sqlComando.CommandText = "sp_WMS_Almacenar_PedidosEcommerce"

        Try
            sqlComando.Connection.Open()
            sqlComando.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            sqlComando.Connection.Close()
        End Try

    End Sub


    Public Function ConsultarTrasladoSiesa(ByVal Notas As String) As Boolean

        Dim dsResultado As New DataSet
        Dim sqlComando As New SqlCommand
        Dim sqlDa As New SqlDataAdapter

        sqlDa.SelectCommand = sqlComando


        sqlComando.CommandTimeout = 36000000
        sqlComando.Connection = sqlConexion
        sqlComando.CommandType = CommandType.StoredProcedure
        sqlComando.CommandText = "sp_WMS_TrasladoValidarDocumento"
        sqlComando.Parameters.AddWithValue("@Notas", Notas)


        Try
            sqlDa.Fill(dsResultado)

            If dsResultado.Tables(0).Rows(0).Item("Documentos") > 0 Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            Throw ex
        Finally
            sqlComando.Connection.Close()
        End Try

    End Function

    Public Function ConsultarEOP(ByVal f350_notas As String, ByVal f350_id_tipo_docto As String) As Boolean

        Dim dsResultado As New DataSet
        Dim sqlComando As New SqlCommand
        Dim sqlDa As New SqlDataAdapter

        sqlDa.SelectCommand = sqlComando


        sqlComando.CommandTimeout = 36000000
        sqlComando.Connection = sqlConexion
        sqlComando.CommandType = CommandType.StoredProcedure
        sqlComando.CommandText = "sp_WMS_ValidarEC_EOP"
        sqlComando.Parameters.AddWithValue("@f350_notas", f350_notas)
        sqlComando.Parameters.AddWithValue("@f350_id_tipo_docto", f350_id_tipo_docto)


        Try
            sqlDa.Fill(dsResultado)

            If dsResultado.Tables(0).Rows(0).Item("Documentos") >= 1 Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            Throw ex
        Finally
            sqlComando.Connection.Close()
        End Try

    End Function

    Public Sub actualizarEstadoTrasladoAjuste(ByVal notas As String, ByVal tipo_docto As String, ByVal consec_docto As String)

        Dim sqlComando As SqlCommand = New SqlCommand

        sqlComando.CommandTimeout = 3600
        sqlComando.Connection = sqlConexion
        sqlComando.CommandType = CommandType.StoredProcedure
        sqlComando.CommandText = "sp_WMS_ESTADO_ACTUALIZAR_AJUSTES_TRASLADOS"
        sqlComando.Parameters.AddWithValue("Notas", notas)
        sqlComando.Parameters.AddWithValue("Tipo_docto", tipo_docto)
        sqlComando.Parameters.AddWithValue("Consec_docto", consec_docto)

        Try
            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            sqlConexion.Close()
        End Try
    End Sub
    Private Sub ConsultarRequisicionesPendientesRQI()

        Dim dsConfiguracionGT As New DataSet
        Dim sqlComando As New SqlCommand


        sqlComando.CommandTimeout = 36000000
        sqlComando.Connection = sqlConexion
        sqlComando.CommandType = CommandType.StoredProcedure
        sqlComando.CommandText = "sp_WMS_Almacenar_PedidosRQI"

        Try
            sqlComando.Connection.Open()
            sqlComando.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            sqlComando.Connection.Close()
        End Try

    End Sub

    Private Sub ConsultarPedidosPendientesPV_PVI()

        Dim dsConfiguracionGT As New DataSet
        Dim sqlComando As New SqlCommand


        sqlComando.CommandTimeout = 36000000
        sqlComando.Connection = sqlConexion
        sqlComando.CommandType = CommandType.StoredProcedure
        sqlComando.CommandText = "sp_WMS_Almacenar_PedidosPV_PVI"

        Try
            sqlComando.Connection.Open()
            sqlComando.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            sqlComando.Connection.Close()
        End Try

    End Sub

    Public Sub AlmacenarTrasladosLogisticaInversa()
        Dim objCorreo As New clsCorreo
        objTarea = New clsTarea

        Try
            If logInicial(Environment.GetCommandLineArgs()) Then

                Try
                    objTarea.LogFechaInicioRecuperacionDatosOrigen()
                    objTarea.LogFechaInicioGeneracionPlano()
                    objTarea.LogFechaInicioWebServiceSiesa()

                    ConsultarTrasladosLI()

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
                    objCorreo.EnviarCorreoTarea("GTIntegration-WMS: Requisiciones RQI", objTarea.Destinatarios, ex.Message, objTarea.Tarea)
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

    Private Sub ConsultarTrasladosLI()

        Dim dsConfiguracionGT As New DataSet
        Dim sqlComando As New SqlCommand


        sqlComando.CommandTimeout = 36000000
        sqlComando.Connection = sqlConexion
        sqlComando.CommandType = CommandType.StoredProcedure
        sqlComando.CommandText = "sp_WMS_Almacenar_LogisticaInversa"

        Try
            sqlComando.Connection.Open()
            sqlComando.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            sqlComando.Connection.Close()
        End Try

    End Sub

    Public Sub AlmacenarPedidosPendientes()
        Dim objCorreo As New clsCorreo
        objTarea = New clsTarea

        Try
            If logInicial(Environment.GetCommandLineArgs()) Then

                Try
                    objTarea.LogFechaInicioRecuperacionDatosOrigen()
                    objTarea.LogFechaInicioGeneracionPlano()
                    objTarea.LogFechaInicioWebServiceSiesa()

                    'ConsultarPedidos()

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
                    objCorreo.EnviarCorreoTarea("GTIntegration-WMS: Requisiciones RQI", objTarea.Destinatarios, ex.Message, objTarea.Tarea)
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

    Public Function ConsultarTrasladosAjustesSiesa(ByVal f350_notas As String, ByVal f350_id_tipo_docto As String,
                                                   ByVal f350_consec_docto As Integer, ByVal f350_id_co As String) As Boolean

        Dim dsResultado As New DataSet
        Dim sqlComando As New SqlCommand
        Dim sqlDa As New SqlDataAdapter

        sqlDa.SelectCommand = sqlComando

        sqlComando.CommandTimeout = 36000000
        sqlComando.Connection = sqlConexion
        sqlComando.CommandType = CommandType.StoredProcedure
        sqlComando.CommandText = "sp_WMS_ValidarDocumentoTrasladoAjuste"
        sqlComando.Parameters.AddWithValue("@f350_notas", f350_notas)
        sqlComando.Parameters.AddWithValue("@f350_id_tipo_docto", f350_id_tipo_docto)
        sqlComando.Parameters.AddWithValue("@f350_consec_docto", f350_consec_docto)
        sqlComando.Parameters.AddWithValue("@f350_id_co", f350_id_co)


        Try
            sqlDa.Fill(dsResultado)

            If dsResultado.Tables(0).Rows(0).Item("Documentos") > 0 Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            Throw ex
        Finally
            sqlComando.Connection.Close()
        End Try

    End Function

    Public Sub controlEstadosTrasladosAjustes()

        Dim sqlComando As SqlCommand = New SqlCommand

        sqlComando.CommandTimeout = 3600
        sqlComando.Connection = sqlConexion
        sqlComando.CommandType = CommandType.StoredProcedure
        sqlComando.CommandText = "sp_WMS_ControlEstadosTrasladosAjustes"

        Try
            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            sqlConexion.Close()
        End Try
    End Sub

    Public Function controlEstadosTrasladosAjustesTRW_AJW(ByRef ConsTRW As Integer, ByRef ConsAJW As Integer)

        Dim dsResultado As New DataSet
        Dim sqlDa As New SqlDataAdapter

        Dim sqlComando As SqlCommand = New SqlCommand

        sqlComando.CommandTimeout = 3600
        sqlComando.Connection = sqlConexion
        sqlComando.CommandType = CommandType.StoredProcedure
        sqlComando.CommandText = "sp_WMS_ControlEstadosTrasladosAjustesTRW_AJW"
        sqlDa.SelectCommand = sqlComando
        Try
            sqlDa.Fill(dsResultado)
            ConsTRW = dsResultado.Tables(0).Rows(0).Item("TRW")
            ConsAJW = dsResultado.Tables(1).Rows(0).Item("AJW")

        Catch ex As Exception
            Throw ex
        Finally
            sqlComando.Connection.Close()
        End Try
    End Function

    Public Sub actualizarConsecutivoManual(ByVal intProximoConsecutivo As Integer, ByVal strTipoDocumento As String)

        Dim sqlComando As SqlCommand = New SqlCommand

        sqlComando.CommandTimeout = 3600
        sqlComando.Connection = sqlConexion
        sqlComando.CommandType = CommandType.StoredProcedure
        sqlComando.CommandText = "sp_WMS_ActualizarConsecutivoManual"
        sqlComando.Parameters.AddWithValue("@intProximoConsecutivo", intProximoConsecutivo)
        sqlComando.Parameters.AddWithValue("@strTipoDocumento", strTipoDocumento)

        Try
            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            sqlConexion.Close()
        End Try
    End Sub

    Public Sub controlEstadosTrasladosAjustesTTW(ByRef ConsTTW As Integer)

        Dim dsResultado As New DataSet
        Dim sqlDa As New SqlDataAdapter

        Dim sqlComando As SqlCommand = New SqlCommand

        sqlComando.CommandTimeout = 3600
        sqlComando.Connection = sqlConexion
        sqlComando.CommandType = CommandType.StoredProcedure
        sqlComando.CommandText = "sp_WMS_ControlEstadosConsecutivoTTW"
        sqlDa.SelectCommand = sqlComando
        Try
            sqlDa.Fill(dsResultado)
            ConsTTW = dsResultado.Tables(0).Rows(0).Item("TTW")

        Catch ex As Exception
            Throw ex
        Finally
            sqlComando.Connection.Close()
        End Try
    End Sub

    Public Function consultarDatosReporteRQI() As DataSet

        Dim dsResultado As New DataSet
        Dim sqlDa As New SqlDataAdapter

        Dim sqlComando As SqlCommand = New SqlCommand

        sqlComando.CommandTimeout = 3600
        sqlComando.Connection = sqlConexion
        sqlComando.CommandType = CommandType.StoredProcedure
        sqlComando.CommandText = "sp_WMS_ConsultaDeDatosReporteRQI"
        sqlDa.SelectCommand = sqlComando

        Try
            sqlDa.Fill(dsResultado)

            Return dsResultado

        Catch ex As Exception
            Throw ex
        Finally
            sqlComando.Connection.Close()
        End Try
    End Function

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

End Class
