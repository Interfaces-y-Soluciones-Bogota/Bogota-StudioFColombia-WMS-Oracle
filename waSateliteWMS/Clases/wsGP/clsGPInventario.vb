Imports System.Data.SqlClient

Public Class clsGPInventario

    Public Function actualizarInventario(ByVal dsDatos As DataSet) As String
        Dim Resultado As String = ""
        Dim objConfiguracion As New clsConfiguracion
        Dim ws As wsMobilistic.SGAMovilWebServiceSoap = New wsMobilistic.SGAMovilWebServiceSoapClient
        Dim resultKeyWeb As wsMobilistic.WebSecurityResponse = ws.GetSecurityKeyWeb(objConfiguracion.WMS_GP_USER)
        Dim requestInventario As wsMobilistic.StockPositionAddEditRequest

        If resultKeyWeb.ResultCode = 0 Then
            For Each Inventario As DataRow In dsDatos.Tables(0).Rows
                Try
                    requestInventario = New wsMobilistic.StockPositionAddEditRequest With {
                        .User = objConfiguracion.WMS_GP_USER,
                        .UserId = objConfiguracion.WMS_GP_USER_ID,
                        .ValidationKey = resultKeyWeb.ValidationKey,
                        .BarCode = Inventario.Item("BarCode"), 'Campo de validación - Sku
                        .IdPositionSubIndex = Inventario.Item("IdPositionSubIndex"),
                        .Quantity = Inventario.Item("Quantity")
                    }

                    '.IdStock = Inventario.Item("objConfiguracion.WMS_GP_STOCK_ID") Valor fijo Int "3326" 
                    '.Plate = Inventario.Item("objConfiguracion.WMS_GP_STOCK_PLATE") Valor fijo Varchar "0"
                    '.Measure = Inventario.Item("objConfiguracion.WMS_GP_STOCK_PLATE") Valor fijo Decimal "0"

                    Dim result = ws.EditStockPosition(requestInventario)

                    If result.ResultCode <> 0 Then
                        Resultado = Resultado & " Error al actualizar el inventario con id:" & Inventario.Item("Sku") & " Mensaje de resultado:" & result.ResultMessage & " Codigo del resultado:" & result.ResultCode
                    Else
                        ejecutarProcedimiento("sp_WMS_GP_INVENTARIO_ACTUALIZAR_ESTADO", Inventario.Item("Sku"))
                    End If

                Catch ex As Exception
                    Resultado = Resultado & " " & ex.Message
                End Try
            Next
        Else
            Resultado = Resultado & " Error:" & resultKeyWeb.ErrorMessage & " Mensaje del resultado:" & resultKeyWeb.ResultMessage
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
            sqlComando.Parameters.AddWithValue("Sku", Parametro)
            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            sqlConexion.Close()
        End Try
    End Sub

End Class
