Imports System.IO
Imports System.Text

Public Class clsPlano
    Public Property Path As String

    Public Sub New(ByVal Path As String)
        If File.Exists(Path) Then
            File.Delete(Path)
        End If
    End Sub
    Public Sub generarPlano(ByVal Datos As DataTable)
        Dim sbDatosPlano As New StringBuilder

        Try
            For Each Fila As DataRow In Datos.Rows
                For Each Columna As String In Fila.ItemArray
                    sbDatosPlano.Append(Columna.ToString & "|")
                Next
                sbDatosPlano.Remove(sbDatosPlano.Length - 1, 1)
                sbDatosPlano.Append(vbCrLf)
            Next

            If File.Exists(Path) = False Then
                File.WriteAllText(Path, sbDatosPlano.ToString)
            Else
                File.AppendAllText(Path, sbDatosPlano.ToString)
            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub generarPlanoASN(ByVal Datos As DataSet, ByRef swPlanoGenerado As Boolean)
        Dim sbDatosPlano As New StringBuilder
        Dim dvEncabezado As New DataView(Datos.Tables(0))
        Dim dvDetalle As New DataView(Datos.Tables(1))
        ' Path = Path.Replace(".txt", Now.ToString("yyyyMMddhhmmss") & ".txt")

        Try

            For Each Fila As DataRow In dvEncabezado.ToTable.Rows

                swPlanoGenerado = True
                For Each Columna As String In Fila.ItemArray
                    sbDatosPlano.Append(Columna.ToString & "|")
                Next

                sbDatosPlano.Remove(sbDatosPlano.Length - 1, 1)
                sbDatosPlano.Append(vbCrLf)

                dvDetalle.RowFilter = " cust_field_2 = " & Fila.Item("shipment_nbr")

                For Each FilaDetalle As DataRow In dvDetalle.ToTable.Rows
                    For Each Columna As String In FilaDetalle.ItemArray
                        sbDatosPlano.Append(Columna.ToString & "|")
                    Next
                    sbDatosPlano.Remove(sbDatosPlano.Length - 1, 1)
                    sbDatosPlano.Append(vbCrLf)
                Next
            Next

            If swPlanoGenerado Then
                If File.Exists(Path) = False Then
                    File.WriteAllText(Path, sbDatosPlano.ToString)
                Else
                    File.AppendAllText(Path, sbDatosPlano.ToString)
                End If
            End If

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub generarPlanoASNPedidos(ByVal Datos As DataSet, ByRef swPlanoGenerado As Boolean)
        Dim sbDatosPlano As New StringBuilder
        Dim dvEncabezado As New DataView(Datos.Tables(0))
        Dim dvDetalle As New DataView(Datos.Tables(1))
        ' Path = Path.Replace(".txt", Now.ToString("yyyyMMddhhmmss") & ".txt")

        Try

            For Each Fila As DataRow In dvEncabezado.ToTable.Rows

                swPlanoGenerado = True
                For Each Columna As String In Fila.ItemArray
                    sbDatosPlano.Append(Columna.ToString & "|")
                Next

                sbDatosPlano.Remove(sbDatosPlano.Length - 1, 1)
                sbDatosPlano.Append(vbCrLf)

                dvDetalle.RowFilter = " cust_field_5 = " & Fila.Item("cust_field_5")

                For Each FilaDetalle As DataRow In dvDetalle.ToTable.Rows
                    For Each Columna As String In FilaDetalle.ItemArray
                        sbDatosPlano.Append(Columna.ToString & "|")
                    Next
                    sbDatosPlano.Remove(sbDatosPlano.Length - 1, 1)
                    sbDatosPlano.Append(vbCrLf)
                Next
            Next


            If swPlanoGenerado Then
                If File.Exists(Path) = False Then
                    File.WriteAllText(Path, sbDatosPlano.ToString)
                Else
                    File.AppendAllText(Path, sbDatosPlano.ToString)
                End If
            End If

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub generarPlanoASNPedidosCopiaLocal(ByVal Datos As DataSet, ByRef swPlanoGenerado As Boolean)
        Dim sbDatosPlano As New StringBuilder
        Dim dvEncabezado As New DataView(Datos.Tables(0))
        Dim dvDetalle As New DataView(Datos.Tables(1))
        ' Path = Path.Replace(".txt", Now.ToString("yyyyMMddhhmmss") & ".txt")

        Try

            For Each Fila As DataRow In dvEncabezado.ToTable.Rows

                swPlanoGenerado = True
                For Each Columna As String In Fila.ItemArray
                    sbDatosPlano.Append(Columna.ToString & "|")
                Next

                sbDatosPlano.Remove(sbDatosPlano.Length - 1, 1)
                sbDatosPlano.Append(vbCrLf)

                dvDetalle.RowFilter = " cust_field_5 = " & Fila.Item("cust_field_5")

                For Each FilaDetalle As DataRow In dvDetalle.ToTable.Rows
                    For Each Columna As String In FilaDetalle.ItemArray
                        sbDatosPlano.Append(Columna.ToString & "|")
                    Next
                    sbDatosPlano.Remove(sbDatosPlano.Length - 1, 1)
                    sbDatosPlano.Append(vbCrLf)
                Next
            Next


            If swPlanoGenerado Then
                If File.Exists(Path) = False Then
                    File.WriteAllText(Path, sbDatosPlano.ToString)
                    Dim NombreArchivo As String
                    NombreArchivo = Path.Substring(Path.LastIndexOf("\") + 1, Path.Length - Path.LastIndexOf("\") - 1)
                    NombreArchivo = NombreArchivo.Replace(".txt", Now.ToString("yyyyMMddhhmmss") & ".txt")
                    File.Copy(Path, Path.Substring(0, Path.LastIndexOf("\")) & "\" & NombreArchivo, True)
                Else
                    File.AppendAllText(Path, sbDatosPlano.ToString)
                    Dim NombreArchivo As String
                    NombreArchivo = Path.Substring(Path.LastIndexOf("\") + 1, Path.Length - Path.LastIndexOf("\") - 1)
                    NombreArchivo = NombreArchivo.Replace(".txt", Now.ToString("yyyyMMddhhmmss") & ".txt")
                    File.Copy(Path, Path.Substring(0, Path.LastIndexOf("\")) & "\" & NombreArchivo, True)
                End If
            End If

        Catch ex As Exception
            Throw ex
        End Try

    End Sub
End Class
