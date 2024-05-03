Imports System.Data.SqlClient

Public Class clsGPProductos

    Public Function almacenarProducto(ByVal dsDatos As DataSet) As String
        Dim Resultado As String = ""
        Dim objConfiguracion As New clsConfiguracion
        Dim ws As wsMobilistic.SGAMovilWebServiceSoap = New wsMobilistic.SGAMovilWebServiceSoapClient
        Dim resultKeyWeb As wsMobilistic.WebSecurityResponse = ws.GetSecurityKeyWeb(objConfiguracion.WMS_GP_USER)
        Dim requestProducto As wsMobilistic.ProductAddEditRequest

        If resultKeyWeb.ResultCode = 0 Then

            For Each Producto As DataRow In dsDatos.Tables(0).Rows
                Try
                    requestProducto = New wsMobilistic.ProductAddEditRequest With {
                            .User = objConfiguracion.WMS_GP_USER,
                            .UserId = objConfiguracion.WMS_GP_USER_ID,
                            .ValidationKey = resultKeyWeb.ValidationKey,
                            .IdClient = objConfiguracion.WMS_GP_ID_CLIENTE,
                            .MeasureType = objConfiguracion.WMS_GP_MEASURETYPE,
                            .IsEnabled = objConfiguracion.WMS_GP_ISENABLE,
                            .Serializable = objConfiguracion.WMS_GP_SERIALIZABLE,
                            .Reference = Producto.Item("Reference"),
                            .Description = Producto.Item("Description"),
                            .BarCode = Producto.Item("Barcode"), 'Campo de validación SKU
                            .ProductGroup = Producto.Item("ProductGroup"),
                            .ProductSubGroup = Producto.Item("ProductSubGroup"),
                            .Brand = Producto.Item("Brand"),
                            .Model = Producto.Item("Model"),
                            .Price = Producto.Item("Price"),
                            .Size = Producto.Item("Size"),
                            .Color = Producto.Item("Color"),
                            .ColorCode = Producto.Item("ColorCode")
                        }

                    Dim result = ws.AddSingleProduct(requestProducto)

                    If result.ResultCode <> 0 Then
                        Resultado = Resultado & " Error al almacenar la referencia" & Producto.Item("Reference") & " Mensaje de resultado:" & result.ResultMessage & " Codigo del resultado:" & result.ResultCode
                    Else
                        ejecutarProcedimiento("sp_WMS_GP_ITEM_ACTUALIZAR_ESTADO", Producto.Item("ID"))
                    End If
                Catch ex As Exception
                    Resultado = Resultado & " Error al almacenar la referencia" & Producto.Item("Reference") & ex.Message
                End Try
            Next

        Else
            Resultado = " Error al generar el ValidationKey" & " Error:" & resultKeyWeb.ErrorMessage & " Mensaje del resultado:" & resultKeyWeb.ResultMessage & " Codigo del resultado:" & resultKeyWeb.ResultCode
        End If

        Return Resultado
    End Function


    Public Function actualizarProducto(ByVal dsDatos As DataSet) As String
        Dim Resultado As String = ""
        Dim objConfiguracion As New clsConfiguracion
        Dim ws As wsMobilistic.SGAMovilWebServiceSoap = New wsMobilistic.SGAMovilWebServiceSoapClient
        Dim resultKeyWeb As wsMobilistic.WebSecurityResponse = ws.GetSecurityKeyWeb(objConfiguracion.WMS_GP_USER)
        Dim requestProducto As wsMobilistic.ProductAddEditRequest

        If resultKeyWeb.ResultCode = 0 Then
            For Each Producto As DataRow In dsDatos.Tables(0).Rows
                Try
                    requestProducto = New wsMobilistic.ProductAddEditRequest With {
                        .User = objConfiguracion.WMS_GP_USER,
                        .UserId = objConfiguracion.WMS_GP_USER_ID,
                        .ValidationKey = resultKeyWeb.ValidationKey,
                        .IdClient = objConfiguracion.WMS_GP_ID_CLIENTE,
                        .MeasureType = objConfiguracion.WMS_GP_MEASURETYPE,
                        .IsEnabled = objConfiguracion.WMS_GP_ISENABLE,
                        .Serializable = objConfiguracion.WMS_GP_SERIALIZABLE,
                        .Reference = Producto.Item("Reference"),
                        .Description = Producto.Item("Description"),
                        .BarCode = Producto.Item("Barcode"), 'Campo de validación SKU
                        .ProductGroup = Producto.Item("ProductGroup"),
                        .ProductSubGroup = Producto.Item("ProductSubGroup"),
                        .Brand = Producto.Item("Brand"),
                        .Model = Producto.Item("Model"),
                        .Price = Producto.Item("Price"),
                        .Size = Producto.Item("Size"),
                        .Color = Producto.Item("Color"),
                        .ColorCode = Producto.Item("ColorCode")
                    }

                    Dim result = ws.EditProduct(requestProducto)

                    If result.ResultCode <> 0 Then
                        Resultado = Resultado & " Error al actualizar la referencia" & Producto.Item("Reference") & " Mensaje de resultado:" & result.ResultMessage & " Codigo del resultado:" & result.ResultCode
                    Else
                        ejecutarProcedimiento("sp_WMS_GP_ITEM_ACTUALIZAR_ESTADO", Producto.Item("ID"))
                    End If
                Catch ex As Exception
                    Resultado = Resultado & " Error al actualizar la referencia" & Producto.Item("Reference") & ex.Message
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
            sqlComando.Parameters.AddWithValue("id", Parametro)
            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            sqlConexion.Close()
        End Try
    End Sub


End Class
