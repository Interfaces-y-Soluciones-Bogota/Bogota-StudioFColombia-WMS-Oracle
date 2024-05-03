Imports System.Data.SqlClient

Public Class clsGPClientes

    Public Function almacenarCliente(ByVal dsDatos As DataSet) As String
        Dim Resultado As String = ""
        Dim objConfiguracion As New clsConfiguracion
        Dim ws As wsMobilistic.SGAMovilWebServiceSoap = New wsMobilistic.SGAMovilWebServiceSoapClient
        Dim resultKeyWeb As wsMobilistic.WebSecurityResponse = ws.GetSecurityKeyWeb(objConfiguracion.WMS_GP_USER)
        Dim requestCliente As wsMobilistic.RequestExtended


        If resultKeyWeb.ResultCode = 0 Then
            For Each Cliente As DataRow In dsDatos.Tables(0).Rows
                Try
                    requestCliente = New wsMobilistic.RequestExtended With {
                            .User = objConfiguracion.WMS_GP_USER,
                            .UserId = objConfiguracion.WMS_GP_USER_ID,
                            .ValidationKey = resultKeyWeb.ValidationKey,
                            .IsEnabled = objConfiguracion.WMS_GP_ISENABLE,
                            .Name = Cliente.Item("Name"),
                            .Description = Cliente.Item("Description"),
                            .Code = Cliente.Item("Code"),
                            .Contact = Cliente.Item("Contact"),
                            .Address = Cliente.Item("Address"),
                            .Region = Cliente.Item("Regional"),
                            .Department = Cliente.Item("Department"),
                            .City = Cliente.Item("City"),
                            .Channel = Cliente.Item("Channel"),
                            .Contact2 = Cliente.Item("Contact2"),
                            .Telephone1 = Cliente.Item("Telephone"),
                            .Telephone2 = Cliente.Item("Telephone2"),
                            .NitOutcomeReceiver = Cliente.Item("Nit"),
                            .CompanyName = Cliente.Item("CompanyName"),
                            .BranchCode = Cliente.Item("BranchCode"),
                            .BranchName = Cliente.Item("BranchName")
                     }



                    Dim result = ws.AddOutcomeReceiver(requestCliente)

                    If result.ResultCode <> 0 Then
                        Resultado = Resultado & " Error al almacenar el cliente" & Cliente.Item("Code") & " Mensaje de resultado:" & result.ResultMessage & " Codigo del resultado:" & result.ResultCode
                    Else
                        ejecutarProcedimiento("sp_WMS_GP_STORE_ACTUALIZAR_ESTADO", Cliente.Item("Code"))
                    End If
                Catch ex As Exception
                    Resultado = Resultado & " Error al almacenar el cliente" & Cliente.Item("Code") & ex.Message
                End Try
            Next
        Else
            Resultado = " Error al generar el ValidationKey" & " Error:" & resultKeyWeb.ErrorMessage & " Mensaje del resultado:" & resultKeyWeb.ResultMessage & " Codigo del resultado:" & resultKeyWeb.ResultCode
        End If

        Return Resultado
    End Function

    Public Function actualizarCliente(ByVal dsDatos As DataSet) As String
        Dim Resultado As String = ""
        Dim objConfiguracion As New clsConfiguracion
        Dim ws As wsMobilistic.SGAMovilWebServiceSoap = New wsMobilistic.SGAMovilWebServiceSoapClient
        Dim resultKeyWeb As wsMobilistic.WebSecurityResponse = ws.GetSecurityKeyWeb(objConfiguracion.WMS_GP_USER)
        Dim requestCliente As wsMobilistic.RequestExtended

        If resultKeyWeb.ResultCode = 0 Then
            For Each Cliente As DataRow In dsDatos.Tables(0).Rows
                Try
                    requestCliente = New wsMobilistic.RequestExtended With {
                        .User = objConfiguracion.WMS_GP_USER,
                        .UserId = objConfiguracion.WMS_GP_USER_ID,
                        .ValidationKey = resultKeyWeb.ValidationKey,
                        .IsEnabled = objConfiguracion.WMS_GP_ISENABLE,
                        .Name = Cliente.Item("Name"),
                        .Description = Cliente.Item("Description"),
                        .Code = Cliente.Item("Code"),
                        .Contact = Cliente.Item("Contact"),
                        .Address = Cliente.Item("Address"),
                        .Region = Cliente.Item("Regional"),
                        .Department = Cliente.Item("Department"),
                        .City = Cliente.Item("City"),
                        .Channel = Cliente.Item("Channel"),
                        .Contact2 = Cliente.Item("Contact2"),
                        .Telephone1 = Cliente.Item("Telephone"),
                        .Telephone2 = Cliente.Item("Telephone2"),
                        .NitOutcomeReceiver = Cliente.Item("Nit"),
                        .CompanyName = Cliente.Item("CompanyName"),
                        .BranchCode = Cliente.Item("BranchCode"),
                        .BranchName = Cliente.Item("BranchName")
                    }

                    Dim result = ws.EditOutcomeReceiver(requestCliente)

                    If result.ResultCode <> 0 Then
                        Resultado = Resultado & " Error al actualizar el cliente" & Cliente.Item("Code") & " Mensaje de resultado:" & result.ResultMessage & " Codigo del resultado:" & result.ResultCode
                    Else
                        ejecutarProcedimiento("sp_WMS_GP_STORE_ACTUALIZAR_ESTADO", Cliente.Item("Code"))
                    End If
                Catch ex As Exception
                    Resultado = Resultado & " Error al actualizar el cliente" & Cliente.Item("Code") & ex.Message
                End Try
            Next
        Else
            Resultado = " Error al generar el ValidationKey" & " Error:" & resultKeyWeb.ErrorMessage & " Mensaje del resultado:" & resultKeyWeb.ResultMessage & " Codigo del resultado:" & resultKeyWeb.ResultCode
        End If

        Return Resultado
    End Function

    Public Sub ejecutarProcedimiento(ByVal procedimiento As String, ByVal Parametro As String)

        Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)

        Try
            Dim sqlComando As SqlCommand = New SqlCommand
            sqlComando.CommandTimeout = 3600
            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = procedimiento
            sqlComando.Parameters.AddWithValue("Code", Parametro)
            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            sqlConexion.Close()
        End Try
    End Sub


End Class
