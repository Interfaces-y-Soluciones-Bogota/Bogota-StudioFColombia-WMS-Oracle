Imports System.IO
Imports System.Data.SqlClient

Public Class clsPedidosEcommerce
    Public Sub actualizarEstadoPedidosEcommerce(ByVal RutaArchivo As String)

        Dim sr As StreamReader = New StreamReader(RutaArchivo)
        Dim Linea As String
        Dim delimiter As Char = "|"
        Dim ArrCadena As String()

        Try
            Do While sr.Peek() >= 0
                Linea = RTrim(sr.ReadLine())
                ArrCadena = Linea.Split(delimiter)
                If Linea.Substring(0, 4) = "[H1]" Then
                    Dim consec_docto = ArrCadena(53)
                    Dim tipo_docto = ArrCadena(54)
                    Dim oc_docto = ArrCadena(55)
                    Dim tabla = "Tbl_WMS_DespachoPedidos_Ecommerce"

                    CambiarEstadoEcommerce(consec_docto, tipo_docto, oc_docto, tabla)
                End If
            Loop
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Sub CambiarEstadoEcommerce(ByVal consec_docto As Integer, ByVal tipo_docto As String, ByVal oc_docto As String, ByVal tabla As String)

        Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)

        Try
            Dim sqlComando As SqlCommand = New SqlCommand
            sqlComando.CommandTimeout = 3600
            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = "sp_wms_actualizar_estados_ecommerce"
            sqlComando.Parameters.AddWithValue("@Tabla", tabla)
            sqlComando.Parameters.AddWithValue("@consec_docto", consec_docto)
            sqlComando.Parameters.AddWithValue("@tipo_docto", tipo_docto)
            sqlComando.Parameters.AddWithValue("@oc_docto", oc_docto)
            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            sqlConexion.Close()
        End Try
    End Sub
End Class
