Imports System.Data.Odbc
Imports System.Data.SqlClient

Public Class clsConfiguracion
    'SQL
    Public Property sqlConexion As New SqlConnection(My.Settings.strConexionGT)
    Public Property sqlComando As SqlCommand = New SqlCommand
    Public Property sqlAdaptador As SqlDataAdapter = New SqlDataAdapter

    'Propiedades de correo
    Public Property EnviarNotificaciones As Boolean = True
    Public Property ServidorDeCorreo As String
    Public Property Puerto As String
    Public Property RequiereAutenticacion As Boolean = True
    Public Property SSL As Boolean = True
    Public Property CorreoRemitente As String
    Public Property UsuarioMail As String
    Public Property ClaveMail As String
    Public Property CorreosNotificaciones As String
    Public Property AdjuntarArchivoCorreo As String

    Public Property ServidorFTP_WMS As String
    Public Property PuertoFTP_WMS As String
    Public Property UsuarioFTP_WMS As String
    Public Property ClaveFTP_WMS As String
    Public Property RutaFTPInput_WMS As String
    Public Property RutaFTPOutput_WMS As String

    'Propiedades Multiproceso Hijo
    Public Property ProcesosParalelos As Integer
    Public Property NumFilasMultiProcesos As Integer
    Public Property RutaLog As String
    Public Property RutaPlanos As String


    ''' <summary>
    ''' Se usa en diferentes puntos del software para cargar los parametros del sistema, si se agrega una nueva variable es necesario modificar el codigo
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()

        Dim ds As New DataSet

        Try
            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = "sp_Propiedades_Select"
            sqlAdaptador.SelectCommand = sqlComando
            sqlAdaptador.Fill(ds)


            For Each Parametro As DataRow In ds.Tables(0).Rows
                If Parametro.Item("nombrePropiedad").ToString = "ProcesosParalelos" Then
                    ProcesosParalelos = Parametro.Item("valorEntero")
                ElseIf Parametro.Item("nombrePropiedad").ToString = "NumFilasMultiProcesos" Then
                    NumFilasMultiProcesos = Parametro.Item("valorEntero")
                ElseIf Parametro.Item("nombrePropiedad").ToString = "RutaLog" Then
                    RutaLog = Parametro.Item("valorTexto1")
                ElseIf Parametro.Item("nombrePropiedad").ToString = "RutaPlanos" Then
                    RutaPlanos = Parametro.Item("valorTexto1")
                ElseIf Parametro.Item("nombrePropiedad").ToString = "EnviarNotificaciones" Then
                    EnviarNotificaciones = Parametro.Item("valorBooleano")
                ElseIf Parametro.Item("nombrePropiedad").ToString = "ServidorDeCorreo" Then
                    ServidorDeCorreo = Parametro.Item("valorTexto1").ToString
                ElseIf Parametro.Item("nombrePropiedad").ToString = "Puerto" Then
                    Puerto = Parametro.Item("valorTexto1").ToString
                ElseIf Parametro.Item("nombrePropiedad").ToString = "RequiereAutenticacion" Then
                    RequiereAutenticacion = Parametro.Item("valorBooleano")
                ElseIf Parametro.Item("nombrePropiedad").ToString = "SSL" Then
                    SSL = Parametro.Item("valorBooleano")
                ElseIf Parametro.Item("nombrePropiedad").ToString = "CorreoRemitente" Then
                    CorreoRemitente = Parametro.Item("valorTexto1").ToString
                ElseIf Parametro.Item("nombrePropiedad").ToString = "UsuarioMail" Then
                    UsuarioMail = Parametro.Item("valorTexto1").ToString
                ElseIf Parametro.Item("nombrePropiedad").ToString = "ClaveMail" Then
                    ClaveMail = Parametro.Item("valorTexto1").ToString
                ElseIf Parametro.Item("nombrePropiedad").ToString = "CorreosNotificaciones" Then
                    CorreosNotificaciones = Parametro.Item("valorTexto1").ToString
                ElseIf Parametro.Item("nombrePropiedad").ToString = "AdjuntarArchivoCorreo" Then
                    AdjuntarArchivoCorreo = Parametro.Item("valorTexto1").ToString
                ElseIf Parametro.Item("nombrePropiedad").ToString = "ServidorFTP_WMS" Then
                    ServidorFTP_WMS = Parametro.Item("valorTexto1").ToString
                ElseIf Parametro.Item("nombrePropiedad").ToString = "PuertoFTP_WMS" Then
                    PuertoFTP_WMS = Parametro.Item("valorTexto1").ToString
                ElseIf Parametro.Item("nombrePropiedad").ToString = "UsuarioFTP_WMS" Then
                    UsuarioFTP_WMS = Parametro.Item("valorTexto1").ToString
                ElseIf Parametro.Item("nombrePropiedad").ToString = "ClaveFTP_WMS" Then
                    ClaveFTP_WMS = Parametro.Item("valorTexto1").ToString
                ElseIf Parametro.Item("nombrePropiedad").ToString = "RutaFTPInput_WMS" Then
                    RutaFTPInput_WMS = Parametro.Item("valorTexto1").ToString
                ElseIf Parametro.Item("nombrePropiedad").ToString = "RutaFTPOutput_WMS" Then
                    RutaFTPOutput_WMS = Parametro.Item("valorTexto1").ToString
                End If
            Next

        Catch ex As Exception
            Throw ex
        Finally
            sqlComando.Parameters.Clear()
            sqlComando.Connection.Close()
        End Try

    End Sub

End Class