Imports System.Data.SqlClient

Public Class clsMaestros
    Dim objTarea As New clsTarea

    Public Sub cargarMaestro()

        Dim objSFTP As New clsSFTP
        Dim objCorreo As New clsCorreo
        Dim dsConsulta As New DataSet
        Dim objUnoEE As New wsUnoEE.WSUNOEE
        Dim ConsultaPorPais As DataSet
        Dim swDatos As Boolean
        swDatos = False


        objUnoEE.Timeout = 3600000

        Try
            Dim objConfiguracion As New clsConfiguracion
            If LogInicial(Environment.GetCommandLineArgs()) Then

                Select Case objTarea.Tarea
                    Case 36
                        ejecutarProcedimiento("sp_WMS_ITEM_ALMACENAR_ORIGEN")
                        ejecutarProcedimiento("sp_WMS_MergeSync_ITEM")

                    Case 37
                        ejecutarProcedimiento("sp_WMS_STORE_ALMACENAR_ORIGEN")
                        ejecutarProcedimiento("sp_WMS_MergeSync_STORE")
                End Select

                If ConsutarXML(dsConsulta) Then

                    Dim objPlano As New clsPlano(objTarea.RutaGeneracionPlano)
                    objTarea.LogFechaInicioGeneracionPlano()

                    objPlano.Path = objTarea.RutaGeneracionPlano.Replace(".txt", Now.ToString("yyyyMMddhhmmss") & ".txt")
                    objSFTP.Tarea = objTarea.Tarea

                    Dim dsActualizarItems As New DataSet

                    For Each Pais As DataRow In dsConsulta.Tables(1).Rows
                        Try
                            ConsultaPorPais = objUnoEE.EjecutarConsultaXML(Pais.Item("Query"))
                            If ConsultaPorPais.Tables(0).Rows.Count > 0 Then
                                objPlano.generarPlano(ConsultaPorPais.Tables(0))
                                swDatos = True
                            End If

                            dsActualizarItems = ConsultaPorPais

                            ConsultaPorPais.Clear()
                            ConsultaPorPais.Dispose()
                            System.GC.Collect()
                            objTarea.LogGeneracionDePlano(1)
                            objTarea.LogFechaFinGeneracionPlano()
                        Catch ex As Exception
                            objTarea.LogGeneracionDePlano(0)
                            objTarea.LogMensajesError(ex.Message)
                            objCorreo.EnviarCorreoTarea("GTIntegration-WMS: Generacion del plano", objTarea.Destinatarios, ex.InnerException.Message, objTarea.Tarea)
                        End Try
                    Next

                    Try
                        objTarea.LogFechaInicioWebServiceSiesa()
                        If swDatos Then
                            objSFTP.PathLocal = objPlano.Path
                            objSFTP.PathSFTP = objConfiguracion.RutaFTPInput_WMS
                            objSFTP.ServidorFTP_WMS = objConfiguracion.ServidorFTP_WMS
                            objSFTP.PuertoFTP_WMS = objConfiguracion.PuertoFTP_WMS
                            objSFTP.UsuarioFTP_WMS = objConfiguracion.UsuarioFTP_WMS
                            objSFTP.ClaveFTP_WMS = objConfiguracion.ClaveFTP_WMS

                            objSFTP.subirArchivosSFTP()

                            For Each item As DataRow In dsActualizarItems.Tables(0).Rows

                                ejecutarProcedimientoConParametros("sp_WMS_Actualizar_Estado_Items", item.Item("item_alternate_code"))

                            Next

                        End If
                        objTarea.LogWebServiceSiesa(1)
                        objTarea.LogEjecucionCompleta()
                    Catch ex As Exception
                        objTarea.LogWebServiceSiesa(0)
                        objTarea.LogMensajesError(ex.Message)
                        objCorreo.EnviarCorreoTarea("GTIntegration-WMS: Carga del plano al servidor SFTP", objTarea.Destinatarios, ex.InnerException.Message, objTarea.Tarea)
                    End Try

                    objTarea.LogFechaFin()
                End If
            End If
        Catch ex As Exception
            objTarea.LogMensajesError(ex.Message)
        Finally
            objTarea.LogFin(objTarea.idLogPrincipal)
        End Try

    End Sub

    Private Function LogInicial(ByVal args As String()) As Boolean
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
                objCorreo.EnviarCorreoTarea("GTIntegration-WMS Validaciones", objTarea.Destinatarios, "El argumento enviado debe ser numérico, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea", objTarea.Tarea)
                Return False
            End If
        Else
            objTarea.LogRecuperacionDatosOrigen(0)
            objTarea.LogMensajesError("No se envió el Id de la tarea como argumento al programa, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea")
            objCorreo.EnviarCorreoTarea("GTIntegration-WMS Validaciones", objTarea.Destinatarios, "No se envió el Id de la tarea como argumento al programa, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea", objTarea.Tarea)
            Return False
        End If

    End Function

    Private Function ConsutarXML(ByRef dsConsulta As DataSet) As Boolean
        Dim objCorreo As New clsCorreo

        Try
            objTarea.LogFechaInicioRecuperacionDatosOrigen()
            dsConsulta = objTarea.DatosOrigen(True)
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

    Public Sub ejecutarProcedimiento(ByVal procedimiento As String)

        Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)


        Try
            Dim sqlComando As SqlCommand = New SqlCommand
            sqlComando.CommandTimeout = 360000
            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = procedimiento
            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            sqlConexion.Close()
        End Try
    End Sub

    Public Sub ejecutarProcedimientoConParametros(ByVal procedimiento As String, ByVal parametro As String)

        Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)


        Try
            Dim sqlComando As SqlCommand = New SqlCommand
            sqlComando.CommandTimeout = 360000
            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = procedimiento
            sqlComando.Parameters.AddWithValue("@Item", parametro)
            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            sqlConexion.Close()
        End Try
    End Sub

End Class
