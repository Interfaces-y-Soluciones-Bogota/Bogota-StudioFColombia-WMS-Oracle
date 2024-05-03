Imports System.Data.SqlClient

Public Class clsGPEcommerce

    Dim objTarea As New clsTarea
    Public Sub transferMovement(ByVal idTarea As Integer)
        Dim objCorreo As New clsCorreo
        Dim Resultado As String = ""
        Dim objConfiguracion As New clsConfiguracion
        Dim ws As wsMobilistic.SGAMovilWebServiceSoap = New wsMobilistic.SGAMovilWebServiceSoapClient
        Dim resultKeyWeb As wsMobilistic.WebSecurityResponse = ws.GetSecurityKeyWeb(objConfiguracion.WMS_GP_USER)
        Dim movimientoRequest As wsMobilistic.RequestEnhance

        Try
            If logInicial(Environment.GetCommandLineArgs()) Then
                Try
                    objTarea.LogFechaInicioRecuperacionDatosOrigen()
                    objTarea.LogFechaInicioGeneracionPlano()
                    objTarea.LogFechaInicioWebServiceSiesa()

                    If resultKeyWeb.ResultCode = 0 Then

                        movimientoRequest = New wsMobilistic.RequestEnhance With {
                                    .User = objConfiguracion.WMS_GP_USER,
                                    .UserId = objConfiguracion.WMS_GP_USER_ID,
                                    .ValidationKey = resultKeyWeb.ValidationKey
                        }

                        Dim result = ws.GetTransferMovementList(movimientoRequest)

                        If result.ResultCode = 0 Then
                            For Each Movement As waSateliteWMS.wsMobilistic.TransferMovementResponse In result.ItemList
                                ejecutarProcedimiento(Movement.BarCode, Movement.CreationDate, Movement.Id,
                                                      Movement.IdPositionSubIndexDestination, Movement.IdPositionSubIndexOrigin, Movement.IdProduct, Movement.IdUserProfile,
                                                      Movement.IsInUse, Movement.Measure, Movement.Plate, Movement.Quantity,
                                                      Movement.Reference, Movement.Size, Movement.ColorCode)
                            Next
                        Else
                            Resultado = Resultado & "Error-Mensaje de resultado: " & result.ResultMessage & " Codigo del resultado: " & result.ResultCode
                        End If

                    Else
                        Resultado = Resultado & " Error: " & resultKeyWeb.ErrorMessage & " Mensaje del resultado: " & resultKeyWeb.ResultMessage
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
                    objCorreo.EnviarCorreoTarea("GTIntegration-WMS: GP-TRASLADOS", objTarea.CorreosNotificaciones, ex.Message, objTarea.Tarea)
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

    Private Sub ejecutarProcedimiento(ByVal barcode As String,
                                      ByVal creation_date As Date,
                                      ByVal id As Integer,
                                      ByVal id_position_sub_index_destination As Integer,
                                      ByVal id_position_sub_index_origin As Integer,
                                      ByVal id_product As Integer,
                                      ByVal id_user_profile As Integer,
                                      ByVal is_in_use As Boolean,
                                      ByVal measure As Double,
                                      ByVal plate As String,
                                      ByVal quantity As Integer,
                                      ByVal reference As String,
                                      ByVal size As String,
                                      ByVal color_code As String)

        Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)

        Try
            Dim sqlComando As SqlCommand = New SqlCommand
            sqlComando.CommandTimeout = 3600
            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = "sp_WMS_GP_TRASLADOS_ALMACENAR"

            sqlComando.Parameters.AddWithValue("barcode", barcode)
            sqlComando.Parameters.AddWithValue("creation_date", creation_date)
            sqlComando.Parameters.AddWithValue("id", id)
            sqlComando.Parameters.AddWithValue("id_position_sub_index_destination", id_position_sub_index_destination)
            sqlComando.Parameters.AddWithValue("id_position_sub_index_origin", id_position_sub_index_origin)
            sqlComando.Parameters.AddWithValue("id_product", id_product)
            sqlComando.Parameters.AddWithValue("id_user_profile", id_user_profile)
            sqlComando.Parameters.AddWithValue("is_in_use", is_in_use)
            sqlComando.Parameters.AddWithValue("measure", measure)
            sqlComando.Parameters.AddWithValue("plate", plate)
            sqlComando.Parameters.AddWithValue("quantity", quantity)
            sqlComando.Parameters.AddWithValue("reference", reference)
            sqlComando.Parameters.AddWithValue("size", size)
            sqlComando.Parameters.AddWithValue("color_code", color_code)


            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            sqlConexion.Close()
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
                objCorreo.EnviarCorreoTarea("GTIntegration-WMS: GP-Validaciones", objTarea.Destinatarios, "El argumento enviado debe ser numérico, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea", objTarea.Tarea)
                Return False
            End If
        Else
            objTarea.LogRecuperacionDatosOrigen(0)
            objTarea.LogMensajesError("No se envió el Id de la tarea como argumento al programa, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea")
            objCorreo.EnviarCorreoTarea("GTIntegration-WMS: GP-Validaciones", objTarea.Destinatarios, "No se envió el Id de la tarea como argumento al programa, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea", objTarea.Tarea)
            Return False
        End If

    End Function
End Class