Imports Renci.SshNet
Imports System.IO
Imports System.Data.SqlClient

Public Class clsSFTP
    Public Property PathLocal As String
    Public Property PathSFTP As String
    Public Property Prefijo As String

    Public Property ServidorFTP_WMS As String
    Public Property PuertoFTP_WMS As String
    Public Property UsuarioFTP_WMS As String
    Public Property ClaveFTP_WMS As String
    Dim objTarea As New clsTarea
    Public Property Tarea As Integer = 0
    Public Property NombreArchivo As String
    Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)


    Public Sub subirArchivosSFTP()

        Dim Ftp As Renci.SshNet.SftpClient = New Renci.SshNet.SftpClient(ServidorFTP_WMS, PuertoFTP_WMS, UsuarioFTP_WMS, ClaveFTP_WMS)

        If Tarea = 36 Or Tarea = 37 Or Tarea = 39 Or Tarea = 69 Then
            NombreArchivo = PathLocal.Substring(PathLocal.LastIndexOf("\") + 1, PathLocal.Length - PathLocal.LastIndexOf("\") - 1)
        Else
            NombreArchivo = PathLocal.Substring(PathLocal.LastIndexOf("\") + 1, PathLocal.Length - PathLocal.LastIndexOf("\") - 1)
            NombreArchivo = NombreArchivo.Replace(".txt", Now.ToString("yyyyMMddhhmmss") & ".txt")
        End If

        Try
            Ftp.Connect()
            Dim fs As System.IO.Stream = System.IO.File.OpenRead(PathLocal)
            Ftp.UploadFile(fs, PathSFTP & NombreArchivo, True)
            fs.Close()
        Catch ex As Exception
            Throw ex
        Finally
            Ftp.Disconnect()
            Ftp.Dispose()
        End Try

    End Sub

    Public Sub subirArchivosProcesadosSFTP()

        Dim Ftp As Renci.SshNet.SftpClient = New Renci.SshNet.SftpClient(ServidorFTP_WMS, PuertoFTP_WMS, UsuarioFTP_WMS, ClaveFTP_WMS)

        Dim NombreArchivo As String

        NombreArchivo = PathLocal.Substring(PathLocal.LastIndexOf("\") + 1, PathLocal.Length - PathLocal.LastIndexOf("\") - 1)
        'NombreArchivo = NombreArchivo.Replace(".txt", Now.ToString("yyyyMMddhhmmss") & ".txt")

        Try
            Ftp.Connect()
            Dim fs As System.IO.Stream = System.IO.File.OpenRead(PathLocal)
            Ftp.UploadFile(fs, PathSFTP & NombreArchivo, True)
            fs.Close()
        Catch ex As Exception
            Throw ex
        Finally
            Ftp.Disconnect()
            Ftp.Dispose()
        End Try

    End Sub


    Public Sub eliminiarArchivosSFTP(ByVal Ruta As String)

        Dim Ftp As Renci.SshNet.SftpClient = New Renci.SshNet.SftpClient(ServidorFTP_WMS, PuertoFTP_WMS, UsuarioFTP_WMS, ClaveFTP_WMS)

        Try
            Ftp.Connect()
            Ftp.Delete(Ruta)
        Catch ex As Exception
            Throw ex
        Finally
            Ftp.Disconnect()
            Ftp.Dispose()
        End Try

    End Sub

    Public Sub descargarArchivos(ByVal idTarea As Integer, ByVal objConfiguracion As clsConfiguracion)
        Dim objCorreo As New clsCorreo
        Dim Ftp As Renci.SshNet.SftpClient = New Renci.SshNet.SftpClient(ServidorFTP_WMS, PuertoFTP_WMS, UsuarioFTP_WMS, ClaveFTP_WMS)

        Try
            objTarea.Tarea = idTarea
            objTarea.DatosOrigen(False)
            PathLocal = objTarea.RutaGeneracionPlano

            If logInicial(Environment.GetCommandLineArgs()) Then

                Try
                    objTarea.LogFechaInicioRecuperacionDatosOrigen()
                    objTarea.LogFechaInicioGeneracionPlano()
                    objTarea.LogFechaInicioWebServiceSiesa()

                    Ftp.Connect()
                    Dim objtmp As IEnumerable(Of Sftp.SftpFile)
                    objtmp = Ftp.ListDirectory(PathSFTP)

                    For Each Archivo As Sftp.SftpFile In objtmp
                        If Archivo.Name.Contains(Prefijo) Then
                            Dim Salida As System.IO.Stream = New System.IO.FileStream(PathLocal & Archivo.Name, System.IO.FileMode.Create)
                            Ftp.DownloadFile(PathSFTP & Archivo.Name, Salida)
                            Salida.Close()
                            Select Case Prefijo
                                Case "SVS"
                                    almacenarVerificacionASN(PathLocal & Archivo.Name)
                                    moverArchivoExitoso(PathLocal & Archivo.Name, PathSFTP & "/success/", ServidorFTP_WMS, PuertoFTP_WMS, UsuarioFTP_WMS, ClaveFTP_WMS)
                                    eliminiarArchivosSFTP(PathSFTP & Archivo.Name)
                                Case "SLS"
                                    almacenarDespachoCargas(PathLocal & Archivo.Name)
                                    moverArchivoExitoso(PathLocal & Archivo.Name, PathSFTP & "/success/", ServidorFTP_WMS, PuertoFTP_WMS, UsuarioFTP_WMS, ClaveFTP_WMS)
                                    eliminiarArchivosSFTP(PathSFTP & Archivo.Name)
                                Case "IHT"
                                    almacenarHistorialInventario(PathLocal & Archivo.Name)
                                    moverArchivoExitoso(PathLocal & Archivo.Name, PathSFTP & "/success/", ServidorFTP_WMS, PuertoFTP_WMS, UsuarioFTP_WMS, ClaveFTP_WMS)
                                    eliminiarArchivosSFTP(PathSFTP & Archivo.Name)
                            End Select
                        End If
                    Next

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
                    objCorreo.EnviarCorreoTarea("GTIntegration-WMS: ASN", objTarea.Destinatarios, ex.Message, objTarea.Tarea)
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

    Public Sub descargarArchivosSFTP(ByVal idTarea As Integer, ByVal objConfiguracion As clsConfiguracion)
        Dim objCorreo As New clsCorreo
        Dim Ftp As Renci.SshNet.SftpClient = New Renci.SshNet.SftpClient(ServidorFTP_WMS, PuertoFTP_WMS, UsuarioFTP_WMS, ClaveFTP_WMS)
        Dim objHisInv As New clsHistorialInventario

        Try
            objTarea.Tarea = idTarea
            objTarea.DatosOrigen(False)
            PathLocal = objTarea.RutaGeneracionPlano

            Ftp.Connect()
            Dim objtmp As IEnumerable(Of Sftp.SftpFile)
            objtmp = Ftp.ListDirectory(PathSFTP)

            If objtmp.Count > 0 Then
                If logInicial(Environment.GetCommandLineArgs()) Then
                    objTarea.LogFechaInicioRecuperacionDatosOrigen()
                    objTarea.LogFechaInicioGeneracionPlano()
                    objTarea.LogFechaInicioWebServiceSiesa()

                    For Each Archivo As Sftp.SftpFile In objtmp
                        If Archivo.Name.Contains(Prefijo) Then
                            Dim Salida As System.IO.Stream = New System.IO.FileStream(PathLocal & Archivo.Name, System.IO.FileMode.Create)
                            Ftp.DownloadFile(PathSFTP & Archivo.Name, Salida)
                            Salida.Close()

                            If ConsultarArchivosSFTP(Archivo.Name) = False Then
                                Select Case Prefijo
                                    Case "SVS"
                                        almacenarVerificacionASN(PathLocal & Archivo.Name)
                                        guardarNombreArchivoSFTP(Archivo.Name)
                                        moverArchivoExitoso(PathLocal & Archivo.Name, PathSFTP & "/success/", ServidorFTP_WMS, PuertoFTP_WMS, UsuarioFTP_WMS, ClaveFTP_WMS)
                                        eliminiarArchivosSFTP(PathSFTP & Archivo.Name)
                                    Case "SLS"
                                        almacenarDespachoCargas(PathLocal & Archivo.Name)
                                        guardarNombreArchivoSFTP(Archivo.Name)
                                        moverArchivoExitoso(PathLocal & Archivo.Name, PathSFTP & "/success/", ServidorFTP_WMS, PuertoFTP_WMS, UsuarioFTP_WMS, ClaveFTP_WMS)
                                        eliminiarArchivosSFTP(PathSFTP & Archivo.Name)
                                    Case "IHT"
                                        almacenarHistorialInventario(PathLocal & Archivo.Name)
                                        guardarNombreArchivoSFTP(Archivo.Name)
                                        moverArchivoExitoso(PathLocal & Archivo.Name, PathSFTP & "/success/", ServidorFTP_WMS, PuertoFTP_WMS, UsuarioFTP_WMS, ClaveFTP_WMS)
                                        eliminiarArchivosSFTP(PathSFTP & Archivo.Name)
                                End Select

                            Else
                                guardarNombreArchivoSFTP(Archivo.Name & "_duplicado")
                                moverArchivoExitoso(PathLocal & Archivo.Name, PathSFTP & "/success/", ServidorFTP_WMS, PuertoFTP_WMS, UsuarioFTP_WMS, ClaveFTP_WMS)
                                eliminiarArchivosSFTP(PathSFTP & Archivo.Name)
                            End If
                        End If
                    Next

                    objTarea.LogWebServiceSiesa(1)
                    objTarea.LogFechaFinWebServiceSiesa()
                    objTarea.LogGeneracionDePlano(1)
                    objTarea.LogFechaFinGeneracionPlano()
                    objTarea.LogFechaFinRecuperacionDatosOrigen()
                    objTarea.LogRecuperacionDatosOrigen(1)
                    objTarea.LogEjecucionCompleta()
                    objTarea.LogFechaFin()
                End If
            End If

        Catch ex As Exception
            objTarea.LogRecuperacionDatosOrigen(0)
            objTarea.LogGeneracionDePlano(0)
            objTarea.LogWebServiceSiesa(0)
            objTarea.LogMensajesError(ex.Message)
            objCorreo.EnviarCorreoTarea("GTIntegration-WMS: ASN", objTarea.Destinatarios, ex.Message, objTarea.Tarea)
        Finally
            objTarea.LogFin(objTarea.idLogPrincipal)
        End Try

    End Sub

    Private Sub almacenarVerificacionASN(ByVal RutaArchivo As String)
        Dim objCorreo As New clsCorreo
        Dim sr As StreamReader = New StreamReader(RutaArchivo)
        Dim Linea As String
        Dim delimiter As Char = "|"
        Dim ArrCadena As String()
        Dim objVerificacionASN As New clsVerificacionASN
        Dim idVerificacionASN As Integer = 0

        Try
            Do While sr.Peek() >= 0
                Linea = RTrim(sr.ReadLine())
                ArrCadena = Linea.Split(delimiter)
                If Linea.Substring(0, 4) = "[H1]" Then
                    objVerificacionASN.guardarEncabezadoVerificacionASN("[H1]", ArrCadena(0).Replace("[H1]", ""), ArrCadena(1), ArrCadena(2), ArrCadena(3), ArrCadena(4), ArrCadena(5), ArrCadena(6), ArrCadena(7), ArrCadena(8), ArrCadena(9), ArrCadena(10),
                                                   ArrCadena(11), ArrCadena(12), ArrCadena(13), ArrCadena(14), ArrCadena(15), ArrCadena(16), ArrCadena(17), ArrCadena(18), ArrCadena(19), ArrCadena(20), idVerificacionASN)
                ElseIf Linea.Substring(0, 4) = "[H2]" Then
                    objVerificacionASN.guardarDetalleVerificacionASN(idVerificacionASN, "[H2]", ArrCadena(0).Replace("[H2]", ""), ArrCadena(1), ArrCadena(2), ArrCadena(3), ArrCadena(4), ArrCadena(5), ArrCadena(6), ArrCadena(7), ArrCadena(8), ArrCadena(9), ArrCadena(10),
                                               ArrCadena(11), ArrCadena(12), ArrCadena(13), ArrCadena(14), ArrCadena(15), ArrCadena(16), ArrCadena(17), ArrCadena(18), ArrCadena(19), ArrCadena(20), ArrCadena(21),
                                               ArrCadena(22), ArrCadena(23), ArrCadena(24), ArrCadena(25), ArrCadena(26), ArrCadena(27), ArrCadena(28), ArrCadena(29), ArrCadena(30), ArrCadena(31), ArrCadena(32),
                                               ArrCadena(33), ArrCadena(34), ArrCadena(35), ArrCadena(36), ArrCadena(37), ArrCadena(38), ArrCadena(39), ArrCadena(40), ArrCadena(41))
                End If
            Loop
        Catch ex As Exception
            objTarea.LogMensajesError(ex.Message)
        End Try

    End Sub

    Private Sub almacenarDespachoCargas(ByVal RutaArchivo As String)

        Dim sr As StreamReader = New StreamReader(RutaArchivo)
        Dim Linea As String
        Dim delimiter As Char = "|"
        Dim ArrCadena As String()
        Dim objVerificacionASN As New clsVerificacionASN
        Dim idDespacho As Integer = 0



        Do While sr.Peek() >= 0
            Linea = RTrim(sr.ReadLine())
            ArrCadena = Linea.Split(delimiter)
            If Linea.Substring(0, 4) = "[H1]" Then
                objVerificacionASN.guardarEncabezadoVerificacionDespachos("[H1]", ArrCadena(0).Replace("[H1]", ""), ArrCadena(1), ArrCadena(2), ArrCadena(3), ArrCadena(4), ArrCadena(5), ArrCadena(6), ArrCadena(7), ArrCadena(8), ArrCadena(9), ArrCadena(10),
                                               ArrCadena(11), ArrCadena(12), ArrCadena(13), ArrCadena(14), ArrCadena(15), ArrCadena(16), ArrCadena(17), ArrCadena(18), ArrCadena(19), ArrCadena(20), idDespacho)
            ElseIf Linea.Substring(0, 4) = "[H2]" Then
                objVerificacionASN.guardarDetalleVerificacionDespachos(idDespacho, "[H2]", ArrCadena(0).Replace("[H2]", ""), ArrCadena(1), ArrCadena(2), ArrCadena(3), ArrCadena(4), ArrCadena(5), ArrCadena(6), ArrCadena(7), ArrCadena(8), ArrCadena(9), ArrCadena(10), ArrCadena(11), ArrCadena(12), ArrCadena(13), ArrCadena(14), ArrCadena(15), ArrCadena(16), ArrCadena(17), ArrCadena(18), ArrCadena(19), ArrCadena(20), ArrCadena(21),
                                           ArrCadena(22), ArrCadena(23), ArrCadena(24), ArrCadena(25), ArrCadena(26), ArrCadena(27), ArrCadena(28), ArrCadena(29), ArrCadena(30), ArrCadena(31), ArrCadena(32), ArrCadena(33), ArrCadena(34), ArrCadena(35), ArrCadena(36), ArrCadena(37), ArrCadena(38), ArrCadena(39), ArrCadena(40), ArrCadena(41), ArrCadena(42), ArrCadena(43), ArrCadena(44), ArrCadena(45), ArrCadena(46),
                                           ArrCadena(47), ArrCadena(48), ArrCadena(49), ArrCadena(50), ArrCadena(51), ArrCadena(52), ArrCadena(53), ArrCadena(54), ArrCadena(55), ArrCadena(56), ArrCadena(57), ArrCadena(58), ArrCadena(59), ArrCadena(60), ArrCadena(61), ArrCadena(62), ArrCadena(63), ArrCadena(64), ArrCadena(65), ArrCadena(66), ArrCadena(67), ArrCadena(68), ArrCadena(69), ArrCadena(70), ArrCadena(71),
                                           ArrCadena(72), ArrCadena(73), ArrCadena(74), ArrCadena(75), ArrCadena(76), ArrCadena(77), ArrCadena(78), ArrCadena(79), ArrCadena(80), ArrCadena(81), ArrCadena(82), ArrCadena(83), ArrCadena(84), ArrCadena(85), ArrCadena(86), ArrCadena(87), ArrCadena(88), ArrCadena(89), ArrCadena(90), ArrCadena(91), ArrCadena(92), ArrCadena(93), ArrCadena(94), ArrCadena(95), ArrCadena(96),
                                           ArrCadena(97), ArrCadena(98), ArrCadena(99), ArrCadena(100), ArrCadena(101), ArrCadena(102), ArrCadena(103), ArrCadena(104), ArrCadena(105), ArrCadena(106), ArrCadena(107), ArrCadena(108), ArrCadena(109), ArrCadena(110), ArrCadena(111), ArrCadena(112), ArrCadena(113), ArrCadena(114), ArrCadena(115), ArrCadena(116), ArrCadena(117), ArrCadena(118), ArrCadena(119), ArrCadena(120),
                                           ArrCadena(121), ArrCadena(122), ArrCadena(123), ArrCadena(124), ArrCadena(125), ArrCadena(126), ArrCadena(127), ArrCadena(128), ArrCadena(129), ArrCadena(130), ArrCadena(131), ArrCadena(132), ArrCadena(133), ArrCadena(134), ArrCadena(135), ArrCadena(136), ArrCadena(137), ArrCadena(138), ArrCadena(139), ArrCadena(140), ArrCadena(141), ArrCadena(142), ArrCadena(143), ArrCadena(144),
                                           ArrCadena(145), ArrCadena(146), ArrCadena(147), ArrCadena(148), ArrCadena(149), ArrCadena(150), ArrCadena(151), ArrCadena(152), ArrCadena(153), ArrCadena(154), ArrCadena(155), ArrCadena(156), ArrCadena(157), ArrCadena(158), ArrCadena(159))
            End If
        Loop
    End Sub

    Private Sub almacenarHistorialInventario(ByVal RutaArchivo As String)
        Dim sr As StreamReader = New StreamReader(RutaArchivo)
        Dim Linea As String
        Dim delimiter As Char = "|"
        Dim ArrCadena As String()
        Dim objHistorialInventario As New clsHistorialInventario
        Dim PrimerRegistro As Boolean = True
        Dim idHistorial As Integer


        Do While sr.Peek() >= 0
            Linea = RTrim(sr.ReadLine())
            ArrCadena = Linea.Split(delimiter)

            If PrimerRegistro = True Then
                objHistorialInventario.guardarHistorialInventarioEncabezado(idHistorial)
            End If

            objHistorialInventario.guardarHistorialInventario(idHistorial, ArrCadena(0), ArrCadena(1), ArrCadena(2), ArrCadena(3), ArrCadena(4), ArrCadena(5), ArrCadena(6), ArrCadena(7), ArrCadena(8), ArrCadena(9), ArrCadena(10),
                                           ArrCadena(11), ArrCadena(12), ArrCadena(13), ArrCadena(14), ArrCadena(15), ArrCadena(16), ArrCadena(17), ArrCadena(18), ArrCadena(19), ArrCadena(20), ArrCadena(21),
                                           ArrCadena(22), ArrCadena(23), ArrCadena(24), ArrCadena(25), ArrCadena(26), ArrCadena(27), ArrCadena(28), ArrCadena(29), ArrCadena(30), ArrCadena(31), ArrCadena(32),
                                           ArrCadena(33), ArrCadena(34), ArrCadena(35), ArrCadena(36), ArrCadena(37), ArrCadena(38), ArrCadena(39), ArrCadena(40), ArrCadena(41), ArrCadena(42), ArrCadena(43),
                                           ArrCadena(44), ArrCadena(45), ArrCadena(46), ArrCadena(47), ArrCadena(48), ArrCadena(49), ArrCadena(50), ArrCadena(51), ArrCadena(52), ArrCadena(53), ArrCadena(54),
                                           ArrCadena(55), ArrCadena(56), ArrCadena(57), ArrCadena(58), ArrCadena(59), ArrCadena(60), ArrCadena(61), ArrCadena(62), ArrCadena(63),
                                           ArrCadena(64), ArrCadena(65), ArrCadena(66), ArrCadena(67), ArrCadena(68), ArrCadena(69))

            PrimerRegistro = False
        Loop

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
                objCorreo.EnviarCorreoTarea("GTIntegration-WMS: Validaciones", objTarea.Destinatarios, "El argumento enviado debe ser numérico, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea", objTarea.Tarea)
                Return False
            End If
        Else
            objTarea.LogRecuperacionDatosOrigen(0)
            objTarea.LogMensajesError("No se envió el Id de la tarea como argumento al programa, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea")
            objCorreo.EnviarCorreoTarea("GTIntegration-WMS: Validaciones", objTarea.Destinatarios, "No se envió el Id de la tarea como argumento al programa, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea", objTarea.Tarea)
            Return False
        End If

    End Function

    Private Sub moverArchivoExitoso(ByVal PathLocal As String, ByVal PathSFTP As String, ByVal ServidorFTP_WMS As String, ByVal PuertoFTP_WMS As String, ByVal UsuarioFTP_WMS As String, ByVal ClaveFTP_WMS As String)
        Dim objFTP As New clsSFTP
        objFTP.PathLocal = PathLocal
        objFTP.PathSFTP = PathSFTP
        objFTP.ServidorFTP_WMS = ServidorFTP_WMS
        objFTP.PuertoFTP_WMS = PuertoFTP_WMS
        objFTP.UsuarioFTP_WMS = UsuarioFTP_WMS
        objFTP.ClaveFTP_WMS = ClaveFTP_WMS
        objFTP.subirArchivosProcesadosSFTP()
    End Sub

    Public Sub guardarNombreArchivoSFTP(ByVal NombreArchivo As String)

        Try

            Dim sqlComando As SqlCommand = New SqlCommand
            Dim sqlAdaptador As SqlDataAdapter = New SqlDataAdapter
            Dim dsResultado As New DataSet

            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = "sp_WMS_AlmacenarNombreArchivoSFTP"
            sqlAdaptador.SelectCommand = sqlComando
            sqlComando.CommandTimeout = 180000

            sqlComando.Parameters.AddWithValue("@NombreArchivo", NombreArchivo)

            sqlAdaptador.SelectCommand = sqlComando
            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            sqlConexion.Close()
        End Try
    End Sub

    Public Function ConsultarArchivosSFTP(ByVal NombreArchivo As String) As Boolean

        Dim dsResultado As New DataSet
        Dim sqlComando As New SqlCommand
        Dim sqlDa As New SqlDataAdapter

        sqlDa.SelectCommand = sqlComando


        sqlComando.CommandTimeout = 36000000
        sqlComando.Connection = sqlConexion
        sqlComando.CommandType = CommandType.StoredProcedure
        sqlComando.CommandText = "sp_WMS_VALIDACION_NOMBREARCHIVO_SFTP"
        sqlComando.Parameters.AddWithValue("@NombreArchivo", NombreArchivo)
        sqlComando.CommandTimeout = 180000

        Try
            sqlDa.Fill(dsResultado)

            If dsResultado.Tables(0).Rows(0).Item("NombredeArchivo") >= 2 Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            Throw ex
        Finally
            sqlComando.Connection.Close()
        End Try

    End Function

End Class