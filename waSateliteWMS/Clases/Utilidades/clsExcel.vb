Imports OfficeOpenXml
Imports System.IO

Public Class ClsExcel

    Public Property Ruta As String
    Public Property NombreArchivo As String
    Public Property NombreHoja As String
    Public Property Datos As DataTable

    Public Sub GenerarReporte()

        If File.Exists(Ruta & "\" & NombreArchivo) Then
            File.Delete(Ruta & "\" & NombreArchivo)
        End If

        Dim xlPackage As ExcelPackage
        Dim newFile As New IO.FileInfo(Ruta & "\" & NombreArchivo)
        Dim contColumnas As Int32 = 1
        Dim contFilas As Int32 = 1
        Try
            xlPackage = New ExcelPackage(newFile)

            Dim worksheet As ExcelWorksheet = xlPackage.Workbook.Worksheets.Add(NombreHoja)

            For Each Columna As DataColumn In Datos.Columns
                worksheet.Cell(1, contColumnas).Value = Columna.ColumnName
                contFilas = 2
                For Each Fila As DataRow In Datos.Rows
                    worksheet.Cell(contFilas, contColumnas).Value = "" & Fila.Item(contColumnas - 1)
                    contFilas += 1
                Next
                contColumnas += 1
            Next

            xlPackage.Save()

        Catch ex As Exception
            Throw ex
        Finally
            xlPackage.Dispose()
        End Try

    End Sub


End Class
