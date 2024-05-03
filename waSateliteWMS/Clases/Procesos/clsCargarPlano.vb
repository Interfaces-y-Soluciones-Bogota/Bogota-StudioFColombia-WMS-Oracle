Imports System.Data.SqlClient

Public Class clsCargarPlano
    Dim objTarea As New clsTarea
    Dim ObjEcommerce As New clsPedidosEcommerce

    Public Sub cargarPlanoASN_TAL(ByVal idTarea As Integer)
        Dim objCorreo As New clsCorreo
        Dim dsConsulta As New DataSet
        Dim swPlanoGenerado As Boolean = False

        Try
            Dim objConfiguracion As New clsConfiguracion
            If logInicial(idTarea) Then

                consutarXML(dsConsulta)
                Dim objPlano As New clsPlano(objTarea.RutaGeneracionPlano)
                Dim objASN As New clsASN

                Try
                    objTarea.LogFechaInicioGeneracionPlano()
                    objPlano.Path = objTarea.RutaGeneracionPlano.Replace(".txt", Now.ToString("yyyyMMddhhmmss") & ".txt")
                    objPlano.generarPlanoASN(dsConsulta, swPlanoGenerado)
                    objTarea.LogGeneracionDePlano(1)
                    objTarea.LogFechaFinGeneracionPlano()
                Catch ex As Exception
                    objTarea.LogGeneracionDePlano(0)
                    objTarea.LogMensajesError(ex.Message)
                    objCorreo.EnviarCorreoTarea("GTIntegration-WMS: Generacion del plano", objTarea.Destinatarios, ex.InnerException.Message, objTarea.Tarea)
                End Try

                If swPlanoGenerado Then
                    Try
                        objTarea.LogFechaInicioWebServiceSiesa()
                        Dim objSFTP As New clsSFTP
                        objSFTP.Tarea = idTarea
                        objSFTP.PathLocal = objPlano.Path
                        objSFTP.PathSFTP = objConfiguracion.RutaFTPInput_WMS
                        objSFTP.ServidorFTP_WMS = objConfiguracion.ServidorFTP_WMS
                        objSFTP.PuertoFTP_WMS = objConfiguracion.PuertoFTP_WMS
                        objSFTP.UsuarioFTP_WMS = objConfiguracion.UsuarioFTP_WMS
                        objSFTP.ClaveFTP_WMS = objConfiguracion.ClaveFTP_WMS

                        objSFTP.subirArchivosSFTP()

                        objASN.encabezadoASN_GuardarEnvio_TAL()
                        objASN.detalleASN_GuardarEnvio_TAL()
                        objASN.encabezadoASN_Eliminar_TAL()
                        objASN.detalleASN_Eliminar_TAL()


                        objTarea.LogWebServiceSiesa(1)
                        objTarea.LogFechaFinWebServiceSiesa()
                    Catch ex As Exception
                        objTarea.LogWebServiceSiesa(0)
                        objTarea.LogMensajesError(ex.Message)
                        objCorreo.EnviarCorreoTarea("GTIntegration-WMS: Carga del plano al servidor SFTP", objTarea.Destinatarios, ex.InnerException.Message, objTarea.Tarea)
                    End Try
                Else
                    objTarea.LogWebServiceSiesa(1)
                End If

                objTarea.LogEjecucionCompleta()
                objTarea.LogFechaFin()

            End If
        Catch ex As Exception
            objTarea.LogMensajesError(ex.Message)
        Finally
            objTarea.LogFin(objTarea.idLogPrincipal)
        End Try
    End Sub

    Public Sub cargarPlanoASN_XDOCK(ByVal idTarea As Integer)
        Dim objCorreo As New clsCorreo
        Dim dsConsulta As New DataSet
        Dim swPlanoGenerado As Boolean = False

        Try
            Dim objConfiguracion As New clsConfiguracion
            If logInicial(idTarea) Then

                consutarXML(dsConsulta)
                Dim objPlano As New clsPlano(objTarea.RutaGeneracionPlano)
                Dim objASN As New clsASN

                Try
                    objTarea.LogFechaInicioGeneracionPlano()
                    objPlano.Path = objTarea.RutaGeneracionPlano
                    objPlano.generarPlanoASN(dsConsulta, swPlanoGenerado)
                    objTarea.LogGeneracionDePlano(1)
                    objTarea.LogFechaFinGeneracionPlano()
                Catch ex As Exception
                    objTarea.LogGeneracionDePlano(0)
                    objTarea.LogMensajesError(ex.Message)
                    objCorreo.EnviarCorreoTarea("GTIntegration-WMS: Generacion del plano", objTarea.Destinatarios, ex.InnerException.Message, objTarea.Tarea)
                End Try


                If swPlanoGenerado Then
                    Try
                        objTarea.LogFechaInicioWebServiceSiesa()
                        Dim objSFTP As New clsSFTP
                        objSFTP.PathLocal = objPlano.Path
                        objSFTP.PathSFTP = objConfiguracion.RutaFTPInput_WMS
                        objSFTP.ServidorFTP_WMS = objConfiguracion.ServidorFTP_WMS
                        objSFTP.PuertoFTP_WMS = objConfiguracion.PuertoFTP_WMS
                        objSFTP.UsuarioFTP_WMS = objConfiguracion.UsuarioFTP_WMS
                        objSFTP.ClaveFTP_WMS = objConfiguracion.ClaveFTP_WMS
                        objSFTP.subirArchivosSFTP()

                        objASN.encabezadoASN_GuardarEnvio_XDOCK()
                        objASN.detalleASN_GuardarEnvio_XDOCK()

                        objASN.detalleASN_Eliminar_XDOCK()
                        objASN.encabezadoASN_Eliminar_XDOCK()


                        objTarea.LogWebServiceSiesa(1)
                        objTarea.LogFechaFinWebServiceSiesa()
                    Catch ex As Exception
                        objTarea.LogWebServiceSiesa(0)
                        objTarea.LogMensajesError(ex.Message)
                        objCorreo.EnviarCorreoTarea("GTIntegration-WMS: Carga del plano al servidor SFTP", objTarea.Destinatarios, ex.InnerException.Message, objTarea.Tarea)
                    End Try
                Else
                    objTarea.LogWebServiceSiesa(1)
                End If

                objTarea.LogEjecucionCompleta()
                objTarea.LogFechaFin()

            End If
        Catch ex As Exception
            objTarea.LogMensajesError(ex.Message)
        Finally
            objTarea.LogFin(objTarea.idLogPrincipal)
        End Try
    End Sub

    Public Sub cargarPlanoASN_TRA(ByVal idTarea As Integer)
        Dim objCorreo As New clsCorreo
        Dim dsConsulta As New DataSet
        Dim swPlanoGenerado As Boolean = False
        Dim objASN As New clsASN

        Try
            Dim objConfiguracion As New clsConfiguracion
            If logInicial(idTarea) Then

                objASN.encabezadoASN_ActualizarConsecutivo_TRA()
                consutarXML(dsConsulta)
                Dim objPlano As New clsPlano(objTarea.RutaGeneracionPlano)


                Try
                    objTarea.LogFechaInicioGeneracionPlano()
                    objPlano.Path = objTarea.RutaGeneracionPlano.Replace(".txt", Now.ToString("yyyyMMddhhmmss") & ".txt")
                    objPlano.generarPlanoASN(dsConsulta, swPlanoGenerado)
                    objTarea.LogGeneracionDePlano(1)
                    objTarea.LogFechaFinGeneracionPlano()
                Catch ex As Exception
                    objTarea.LogGeneracionDePlano(0)
                    objTarea.LogMensajesError(ex.Message)
                    objCorreo.EnviarCorreoTarea("GTIntegration-WMS: Generacion del plano", objTarea.Destinatarios, ex.InnerException.Message, objTarea.Tarea)
                End Try


                If swPlanoGenerado Then
                    Try
                        objTarea.LogFechaInicioWebServiceSiesa()
                        Dim objSFTP As New clsSFTP
                        objSFTP.Tarea = idTarea
                        objSFTP.PathLocal = objPlano.Path
                        objSFTP.PathSFTP = objConfiguracion.RutaFTPInput_WMS
                        objSFTP.ServidorFTP_WMS = objConfiguracion.ServidorFTP_WMS
                        objSFTP.PuertoFTP_WMS = objConfiguracion.PuertoFTP_WMS
                        objSFTP.UsuarioFTP_WMS = objConfiguracion.UsuarioFTP_WMS
                        objSFTP.ClaveFTP_WMS = objConfiguracion.ClaveFTP_WMS
                        objSFTP.subirArchivosSFTP()

                        objASN.encabezadoASN_GuardarEnvio_TRA()
                        objASN.detalleASN_GuardarEnvio_TRA()
                        objASN.detalleASN_Eliminar_TRA()
                        objASN.encabezadoASN_Eliminar_TRA()

                        objTarea.LogWebServiceSiesa(1)
                        objTarea.LogFechaFinWebServiceSiesa()
                    Catch ex As Exception
                        objTarea.LogWebServiceSiesa(0)
                        objTarea.LogMensajesError(ex.Message)
                        objCorreo.EnviarCorreoTarea("GTIntegration-WMS: Carga del plano al servidor SFTP", objTarea.Destinatarios, ex.InnerException.Message, objTarea.Tarea)
                    End Try
                Else
                    objTarea.LogWebServiceSiesa(1)
                End If

                objTarea.LogEjecucionCompleta()
                objTarea.LogFechaFin()

            End If
        Catch ex As Exception
            objTarea.LogMensajesError(ex.Message)
        Finally
            objTarea.LogFin(objTarea.idLogPrincipal)
        End Try
    End Sub

    Public Sub cargarPlanoASN_DEV(ByVal idTarea As Integer)
        Dim objCorreo As New clsCorreo
        Dim dsConsulta As New DataSet
        Dim swPlanoGenerado As Boolean = False
        Dim objASN As New clsASN

        Try
            Dim objConfiguracion As New clsConfiguracion
            If logInicial(idTarea) Then

                'objASN.encabezadoASN_ActualizarConsecutivo_DEV()
                consutarXML(dsConsulta)
                Dim objPlano As New clsPlano(objTarea.RutaGeneracionPlano)

                Try
                    objTarea.LogFechaInicioGeneracionPlano()
                    objPlano.Path = objTarea.RutaGeneracionPlano
                    objPlano.generarPlanoASN(dsConsulta, swPlanoGenerado)
                    objTarea.LogGeneracionDePlano(1)
                    objTarea.LogFechaFinGeneracionPlano()
                Catch ex As Exception
                    objTarea.LogGeneracionDePlano(0)
                    objTarea.LogMensajesError(ex.Message)
                    objCorreo.EnviarCorreoTarea("GTIntegration-WMS: Generacion del plano", objTarea.Destinatarios, ex.InnerException.Message, objTarea.Tarea)
                End Try


                If swPlanoGenerado Then
                    Try
                        objTarea.LogFechaInicioWebServiceSiesa()
                        Dim objSFTP As New clsSFTP
                        objSFTP.PathLocal = objPlano.Path
                        objSFTP.PathSFTP = objConfiguracion.RutaFTPInput_WMSLogInversa
                        objSFTP.ServidorFTP_WMS = objConfiguracion.ServidorFTP_WMS
                        objSFTP.PuertoFTP_WMS = objConfiguracion.PuertoFTP_WMS
                        objSFTP.UsuarioFTP_WMS = objConfiguracion.UsuarioFTP_WMS
                        objSFTP.ClaveFTP_WMS = objConfiguracion.ClaveFTP_WMS
                        'objSFTP.PathSFTP = "/data/LI01/input/"
                        objSFTP.subirArchivosSFTP()

                        objASN.encabezadoASN_GuardarEnvio_DEV()
                        objASN.detalleASN_GuardarEnvio_DEV()
                        objASN.encabezadoASN_Eliminar_DEV()
                        objASN.detalleASN_Eliminar_DEV()


                        objTarea.LogWebServiceSiesa(1)
                        objTarea.LogFechaFinWebServiceSiesa()
                    Catch ex As Exception
                        objTarea.LogWebServiceSiesa(0)
                        objTarea.LogMensajesError(ex.Message)
                        objCorreo.EnviarCorreoTarea("GTIntegration-WMS: Carga del plano al servidor SFTP", objTarea.Destinatarios, ex.InnerException.Message, objTarea.Tarea)
                    End Try
                Else
                    objTarea.LogWebServiceSiesa(1)
                End If

                objTarea.LogEjecucionCompleta()
                objTarea.LogFechaFin()

            End If
        Catch ex As Exception
            objTarea.LogMensajesError(ex.Message)
        Finally
            objTarea.LogFin(objTarea.idLogPrincipal)
        End Try
    End Sub

    Public Sub cargarPedidos(ByVal idTarea As Integer, ByVal RutaFTP As String)

        Dim objCorreo As New clsCorreo
        Dim dsConsulta As New DataSet
        Dim swPlanoGenerado As Boolean = False
        Dim ResultadoExitoso = True

        Try
            Dim objConfiguracion As New clsConfiguracion
            If logInicial(idTarea) Then

                consutarXML(dsConsulta)
                Dim objPlano As New clsPlano(objTarea.RutaGeneracionPlano)
                Dim objASN As New clsASN

                Try
                    objTarea.LogFechaInicioGeneracionPlano()
                    objPlano.Path = objTarea.RutaGeneracionPlano
                    objPlano.generarPlanoASNPedidos(dsConsulta, swPlanoGenerado)
                    objTarea.LogGeneracionDePlano(1)
                    objTarea.LogFechaFinGeneracionPlano()
                Catch ex As Exception
                    ResultadoExitoso = False
                    objTarea.LogGeneracionDePlano(0)
                    objTarea.LogMensajesError(ex.Message)
                    objCorreo.EnviarCorreoTarea("GTIntegration-WMS: Generacion del plano", objTarea.Destinatarios, ex.InnerException.Message, objTarea.Tarea)
                End Try
                If swPlanoGenerado Then
                    Try
                        objTarea.LogFechaInicioWebServiceSiesa()
                        Dim objSFTP As New clsSFTP
                        objSFTP.PathLocal = objPlano.Path
                        objSFTP.PathSFTP = RutaFTP
                        objSFTP.ServidorFTP_WMS = objConfiguracion.ServidorFTP_WMS
                        objSFTP.PuertoFTP_WMS = objConfiguracion.PuertoFTP_WMS
                        objSFTP.UsuarioFTP_WMS = objConfiguracion.UsuarioFTP_WMS
                        objSFTP.ClaveFTP_WMS = objConfiguracion.ClaveFTP_WMS
                        objSFTP.subirArchivosSFTP()
                        objTarea.LogWebServiceSiesa(1)
                        objTarea.LogFechaFinWebServiceSiesa()
                    Catch ex As Exception
                        ResultadoExitoso = False
                        objTarea.LogWebServiceSiesa(0)
                        objTarea.LogMensajesError(ex.Message)
                        objCorreo.EnviarCorreoTarea("GTIntegration-WMS: Carga del plano al servidor SFTP", objTarea.Destinatarios, ex.InnerException.Message, objTarea.Tarea)
                    End Try
                Else
                    ResultadoExitoso = False
                    objTarea.LogWebServiceSiesa(1)
                End If

                objTarea.LogEjecucionCompleta()
                objTarea.LogFechaFin()
            End If

            If ResultadoExitoso = True Then
                objTarea.LogFin(objTarea.idLogPrincipal)
            End If

        Catch ex As Exception
            objTarea.LogMensajesError(ex.Message)

        End Try
    End Sub

    Public Sub cargarPedidosCopiaLocal(ByVal idTarea As Integer, ByVal RutaFTP As String)

        Dim objCorreo As New clsCorreo
        Dim dsConsulta As New DataSet
        Dim swPlanoGenerado As Boolean = False
        Dim ResultadoExitoso = True

        Try
            Dim objConfiguracion As New clsConfiguracion
            If logInicial(idTarea) Then

                consutarXML(dsConsulta)
                Dim objPlano As New clsPlano(objTarea.RutaGeneracionPlano)
                Dim objASN As New clsASN

                Try
                    objTarea.LogFechaInicioGeneracionPlano()
                    objPlano.Path = objTarea.RutaGeneracionPlano
                    objPlano.generarPlanoASNPedidosCopiaLocal(dsConsulta, swPlanoGenerado)
                    objTarea.LogGeneracionDePlano(1)
                    objTarea.LogFechaFinGeneracionPlano()
                Catch ex As Exception
                    ResultadoExitoso = False
                    objTarea.LogGeneracionDePlano(0)
                    objTarea.LogMensajesError(ex.Message)
                    objCorreo.EnviarCorreoTarea("GTIntegration-WMS: Generacion del plano", objTarea.Destinatarios, ex.InnerException.Message, objTarea.Tarea)
                End Try
                If swPlanoGenerado Then
                    Try
                        objTarea.LogFechaInicioWebServiceSiesa()
                        Dim objSFTP As New clsSFTP
                        objSFTP.PathLocal = objPlano.Path
                        objSFTP.PathSFTP = RutaFTP
                        objSFTP.ServidorFTP_WMS = objConfiguracion.ServidorFTP_WMS
                        objSFTP.PuertoFTP_WMS = objConfiguracion.PuertoFTP_WMS
                        objSFTP.UsuarioFTP_WMS = objConfiguracion.UsuarioFTP_WMS
                        objSFTP.ClaveFTP_WMS = objConfiguracion.ClaveFTP_WMS
                        objSFTP.subirArchivosSFTP()
                        objTarea.LogWebServiceSiesa(1)
                        objTarea.LogFechaFinWebServiceSiesa()
                    Catch ex As Exception
                        ResultadoExitoso = False
                        objTarea.LogWebServiceSiesa(0)
                        objTarea.LogMensajesError(ex.Message)
                        objCorreo.EnviarCorreoTarea("GTIntegration-WMS: Carga del plano al servidor SFTP", objTarea.Destinatarios, ex.InnerException.Message, objTarea.Tarea)
                    End Try
                Else
                    ResultadoExitoso = False
                    objTarea.LogWebServiceSiesa(1)
                End If

                objTarea.LogEjecucionCompleta()
                objTarea.LogFechaFin()
            End If

            If ResultadoExitoso = True Then
                objTarea.LogFin(objTarea.idLogPrincipal)
            End If

        Catch ex As Exception
            objTarea.LogMensajesError(ex.Message)

        End Try
    End Sub
    Public Sub cargarPedidosCopiaLocalEcommerce(ByVal idTarea As Integer, ByVal RutaFTP As String)

        Dim objCorreo As New clsCorreo
        Dim dsConsulta As New DataSet
        Dim swPlanoGenerado As Boolean = False

        Try
            Dim objConfiguracion As New clsConfiguracion
            If logInicial(idTarea) Then

                consutarXML(dsConsulta)
                Dim objPlano As New clsPlano(objTarea.RutaGeneracionPlano)
                Dim objASN As New clsASN

                Try
                    objTarea.LogFechaInicioGeneracionPlano()
                    objPlano.Path = objTarea.RutaGeneracionPlano
                    objPlano.generarPlanoASNPedidosCopiaLocal(dsConsulta, swPlanoGenerado)
                    objTarea.LogGeneracionDePlano(1)
                    objTarea.LogFechaFinGeneracionPlano()
                Catch ex As Exception
                    objTarea.LogGeneracionDePlano(0)
                    objTarea.LogMensajesError(ex.Message)
                    objCorreo.EnviarCorreoTarea("GTIntegration-WMS: Generacion del plano", objTarea.Destinatarios, ex.InnerException.Message, objTarea.Tarea)
                End Try
                If swPlanoGenerado Then
                    Try
                        objTarea.LogFechaInicioWebServiceSiesa()
                        Dim objSFTP As New clsSFTP
                        objSFTP.PathLocal = objPlano.Path
                        objSFTP.PathSFTP = RutaFTP
                        objSFTP.ServidorFTP_WMS = objConfiguracion.ServidorFTP_WMS
                        objSFTP.PuertoFTP_WMS = objConfiguracion.PuertoFTP_WMS
                        objSFTP.UsuarioFTP_WMS = objConfiguracion.UsuarioFTP_WMS
                        objSFTP.ClaveFTP_WMS = objConfiguracion.ClaveFTP_WMS
                        objSFTP.subirArchivosSFTP()
                        objTarea.LogWebServiceSiesa(1)
                        objTarea.LogFechaFinWebServiceSiesa()

                        'Validación marcado de estados
                        ObjEcommerce.actualizarEstadoPedidosEcommerce(objSFTP.PathLocal)
                    Catch ex As Exception
                        objTarea.LogWebServiceSiesa(0)
                        objTarea.LogMensajesError(ex.Message)
                        objCorreo.EnviarCorreoTarea("GTIntegration-WMS: Carga del plano al servidor SFTP", objTarea.Destinatarios, ex.InnerException.Message, objTarea.Tarea)
                    End Try
                Else
                    objTarea.LogWebServiceSiesa(1)
                End If

                objTarea.LogEjecucionCompleta()
                objTarea.LogFechaFin()
            End If

        Catch ex As Exception
            objTarea.LogMensajesError(ex.Message)

        End Try
    End Sub
    Public Sub cargarPedidosGP(ByVal idTarea As Integer, ByVal RutaFTP As String)

        Dim objCorreo As New clsCorreo
        Dim dsConsulta As New DataSet
        Dim swPlanoGenerado As Boolean = False
        Dim ResultadoExitoso = True

        Try
            Dim objConfiguracion As New clsConfiguracion
            If logInicial(idTarea) Then

                consutarXML(dsConsulta)
                Dim objPlano As New clsPlano(objTarea.RutaGeneracionPlano)
                Dim objASN As New clsASN

                Try
                    objTarea.LogFechaInicioGeneracionPlano()
                    objPlano.Path = objTarea.RutaGeneracionPlano
                    objPlano.generarPlanoASNPedidos(dsConsulta, swPlanoGenerado)
                    objTarea.LogGeneracionDePlano(1)
                Catch ex As Exception
                    ResultadoExitoso = False
                    objTarea.LogGeneracionDePlano(0)
                    objTarea.LogMensajesError(ex.Message)
                    objCorreo.EnviarCorreoTarea("GTIntegration-WMS: GP-Generacion del plano", objTarea.Destinatarios, ex.InnerException.Message, objTarea.Tarea)
                End Try
                If swPlanoGenerado Then
                    Try
                        objTarea.LogFechaInicioWebServiceSiesa()
                        Dim objSFTP As New clsSFTP
                        objSFTP.PathLocal = objPlano.Path
                        objSFTP.PathSFTP = RutaFTP
                        objSFTP.ServidorFTP_WMS = objConfiguracion.ServidorFTP_WMS
                        objSFTP.PuertoFTP_WMS = objConfiguracion.PuertoFTP_WMS
                        objSFTP.UsuarioFTP_WMS = objConfiguracion.UsuarioFTP_WMS
                        objSFTP.ClaveFTP_WMS = objConfiguracion.ClaveFTP_WMS
                        objSFTP.subirArchivosSFTP()
                        objTarea.LogWebServiceSiesa(1)
                        objTarea.LogFechaFinWebServiceSiesa()


                        For Each Item As DataRow In dsConsulta.Tables(1).Rows
                            ejecutarProcedimiento("sp_WMS_GP_PEDIDO_ITEM_ACTUALIZAR_ESTADO", Item.Item("cust_field_4"), Item.Item("cust_field_5"))
                        Next

                    Catch ex As Exception
                        ResultadoExitoso = False
                        objTarea.LogWebServiceSiesa(0)
                        objTarea.LogMensajesError(ex.Message)
                        objCorreo.EnviarCorreoTarea("GTIntegration-WMS: Carga del plano al servidor SFTP", objTarea.Destinatarios, ex.InnerException.Message, objTarea.Tarea)
                    End Try
                Else
                    ResultadoExitoso = False
                    objTarea.LogWebServiceSiesa(1)
                End If

                objTarea.LogEjecucionCompleta()
                objTarea.LogFechaFin()
            End If

            If ResultadoExitoso = True Then
                objTarea.LogFin(objTarea.idLogPrincipal)
            End If
        Catch ex As Exception
            objTarea.LogMensajesError(ex.Message)
        End Try
    End Sub

    Private Function consutarXML(ByRef dsConsulta As DataSet) As Boolean
        Dim objCorreo As New clsCorreo

        Try
            objTarea.LogFechaInicioRecuperacionDatosOrigen()

            dsConsulta = objTarea.DatosOrigen(False)

            objTarea.LogFechaFinRecuperacionDatosOrigen()
            objTarea.LogRecuperacionDatosOrigen(1)
            Return True
        Catch ex As Exception
            objTarea.LogRecuperacionDatosOrigen(0)
            objTarea.LogMensajesError(ex.Message)
            objCorreo.EnviarCorreoTarea("GTIntegration-WMS: Obtencion datos fuentes de la tarea", objTarea.Destinatarios, ex.Message, objTarea.Tarea)
            Return False
        End Try

    End Function

    Private Function logInicial(ByVal idTarea As Integer) As Boolean
        Dim objCorreo As New clsCorreo
        objTarea.Tarea = idTarea
        objTarea.LogPrincipalAlmacenar()
        objTarea.LogInicio()
        Return True
    End Function

    Public Sub ejecutarProcedimiento(ByVal procedimiento As String, ByVal Parametro_id As Integer, ByVal Parametro_id_outcome As Integer)

        Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)

        Try
            Dim sqlComando As SqlCommand = New SqlCommand
            sqlComando.CommandTimeout = 3600
            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = procedimiento
            sqlComando.Parameters.AddWithValue("cust_field_4", Parametro_id)
            sqlComando.Parameters.AddWithValue("cust_field_5", Parametro_id_outcome)
            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            sqlConexion.Close()
        End Try
    End Sub


End Class