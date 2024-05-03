Imports System.Data.SqlClient
Imports System.Text

Public Class clsDespachoCargas
    Dim objTarea As New clsTarea
    Dim objCorreo As New clsCorreo

    Public Sub descompromisoParcial(ByVal NombreCampoIdDocumento As String)
        Dim dsOrigen As DataSet
        objTarea.BaseDatos = "SQL"
        Dim Resultado As String = ""
        Dim idTareaValido As Boolean = True
        Dim objValidacionDocumento As New clsTraslados

        'Id de la tarea enviado como parametro
        Dim args As String() = Environment.GetCommandLineArgs()

        If args.Length = 2 Then
            If IsNumeric(args(1)) Then
                objTarea.Tarea = args(1)
                dsOrigen = objTarea.DatosOrigen(False)
                If dsOrigen.Tables.Count > 0 Then
                    If dsOrigen.Tables(0).Rows.Count = 0 Then
                        Exit Sub
                    End If
                End If
                objTarea.LogPrincipalAlmacenar()
                objTarea.LogInicio()
            Else
                objTarea.LogRecuperacionDatosOrigen(0)
                objTarea.LogMensajesError("El argumento enviado debe ser numérico, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea")
                'objCorreo.EnviarCorreoTarea("Validaciones", objTarea.Destinatarios, "El argumento enviado debe ser numérico, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea", objTarea.Tarea)
                Exit Sub
            End If
        Else
            objTarea.LogRecuperacionDatosOrigen(0)
            objTarea.LogMensajesError("No se envió el Id de la tarea como argumento al programa, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea")
            'objCorreo.EnviarCorreoTarea("Validaciones", objTarea.Destinatarios, "No se envió el Id de la tarea como argumento al programa, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea", objTarea.Tarea)
            Exit Sub
        End If

        'Obtener los datos fuentes del cliente para la tarea X
        Try
            objTarea.LogFechaInicioRecuperacionDatosOrigen()
            dsOrigen = objTarea.DatosOrigen(False)
            If idTareaValido Then
                objTarea.LogFechaFinRecuperacionDatosOrigen()
                objTarea.LogRecuperacionDatosOrigen(1)
            Else
                objTarea.LogRecuperacionDatosOrigen(0)
                objTarea.LogMensajesError("No se encontró el Id de tarea: " & objTarea.Tarea & " en la base de datos, verifique la configuración de la tarea programada de Windows")
                'objCorreo.EnviarCorreoTarea("Obtencion datos fuentes de la tarea", objTarea.Destinatarios, "No se encontró el Id de tarea: " & objTarea.Tarea & " en la base de datos, verifique la configuración de la tarea programada de Windows", objTarea.Tarea)
                Exit Sub
            End If
        Catch ex As Exception
            objTarea.LogFechaFin()
            objTarea.LogFin(objTarea.idLogPrincipal)
            objTarea.LogRecuperacionDatosOrigen(0)
            objTarea.LogMensajesError(ex.Message)
            'objCorreo.EnviarCorreoTarea("Exception - Obtencion datos fuentes de la tarea", objTarea.Destinatarios, ex.Message, objTarea.Tarea)
            Exit Sub
        End Try

        Try
            Dim ProcesoCompleto As Boolean = True
            Dim strMensajesSiesa As New StringBuilder
            Dim dsOrigenDatosPorDocumento As New DataSet
            Dim dvDocumento As DataView
            Dim Consec_docto As String = ""
            Dim MensajeGT As New StringBuilder

            objTarea.LogFechaInicioWebServiceSiesa()
            objTarea.LogFechaInicioGeneracionPlano()

            dvDocumento = New DataView(dsOrigen.Tables(0))

            For Each Documento As DataRow In dsOrigen.Tables(0).Rows
                Try
                    ejecutarProcedimientoDescompromisoParcial("sp_WMS_Requisiciones_DescompromisoParcial", Documento.Item("CO"), Documento.Item("TipoRequisicion"), Documento.Item("NroRequisicion"))
                Catch ex As Exception
                    MensajeGT.AppendLine("No se descomprometió el documento tipo: " & Documento.Item("TipoRequisicion") & " Número:" & Documento.Item("NroRequisicion") & " ," & ex.Message.ToString)
                End Try
            Next

            If MensajeGT.ToString = "" Then
                objTarea.LogGeneracionDePlano(1)
                objTarea.LogWebServiceSiesa(1)
                objTarea.LogEjecucionCompleta()
            Else
                objTarea.LogGeneracionDePlano(0)
                objTarea.LogWebServiceSiesa(0)
                objTarea.LogMensajesError(MensajeGT.ToString)
            End If

            objTarea.LogFechaFinWebServiceSiesa()
            objTarea.LogFechaFinGeneracionPlano()

        Catch ex As Exception
            objTarea.LogFechaFin()
            objTarea.LogFin(objTarea.idLogPrincipal)
            objTarea.LogGeneracionDePlano(0)
            objTarea.LogWebServiceSiesa(0)
            objTarea.LogMensajesError(ex.InnerException.Message)
        Finally
            objTarea.LogFechaFin()
        End Try

    End Sub
    Public Sub descompromisoParcialPedidos(ByVal NombreCampoIdDocumento As String)
        Dim dsOrigen As DataSet
        objTarea.BaseDatos = "SQL"
        Dim Resultado As String = ""
        Dim idTareaValido As Boolean = True
        Dim objValidacionDocumento As New clsTraslados

        'Id de la tarea enviado como parametro
        Dim args As String() = Environment.GetCommandLineArgs()

        If args.Length = 2 Then
            If IsNumeric(args(1)) Then
                objTarea.Tarea = args(1)
                dsOrigen = objTarea.DatosOrigen(False)
                If dsOrigen.Tables.Count > 0 Then
                    If dsOrigen.Tables(0).Rows.Count = 0 Then
                        Exit Sub
                    End If
                End If
                objTarea.LogPrincipalAlmacenar()
                objTarea.LogInicio()
            Else
                objTarea.LogRecuperacionDatosOrigen(0)
                objTarea.LogMensajesError("El argumento enviado debe ser numérico, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea")
                'objCorreo.EnviarCorreoTarea("Validaciones", objTarea.Destinatarios, "El argumento enviado debe ser numérico, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea", objTarea.Tarea)
                Exit Sub
            End If
        Else
            objTarea.LogRecuperacionDatosOrigen(0)
            objTarea.LogMensajesError("No se envió el Id de la tarea como argumento al programa, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea")
            'objCorreo.EnviarCorreoTarea("Validaciones", objTarea.Destinatarios, "No se envió el Id de la tarea como argumento al programa, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea", objTarea.Tarea)
            Exit Sub
        End If

        'Obtener los datos fuentes del cliente para la tarea X
        Try
            objTarea.LogFechaInicioRecuperacionDatosOrigen()
            dsOrigen = objTarea.DatosOrigen(False)
            If idTareaValido Then
                objTarea.LogFechaFinRecuperacionDatosOrigen()
                objTarea.LogRecuperacionDatosOrigen(1)
            Else
                objTarea.LogRecuperacionDatosOrigen(0)
                objTarea.LogMensajesError("No se encontró el Id de tarea: " & objTarea.Tarea & " en la base de datos, verifique la configuración de la tarea programada de Windows")
                'objCorreo.EnviarCorreoTarea("Obtencion datos fuentes de la tarea", objTarea.Destinatarios, "No se encontró el Id de tarea: " & objTarea.Tarea & " en la base de datos, verifique la configuración de la tarea programada de Windows", objTarea.Tarea)
                Exit Sub
            End If
        Catch ex As Exception
            objTarea.LogFechaFin()
            objTarea.LogFin(objTarea.idLogPrincipal)
            objTarea.LogRecuperacionDatosOrigen(0)
            objTarea.LogMensajesError(ex.Message)
            'objCorreo.EnviarCorreoTarea("Exception - Obtencion datos fuentes de la tarea", objTarea.Destinatarios, ex.Message, objTarea.Tarea)
            Exit Sub
        End Try

        Try
            Dim ProcesoCompleto As Boolean = True
            Dim strMensajesSiesa As New StringBuilder
            Dim dsOrigenDatosPorDocumento As New DataSet
            Dim dvDocumento As DataView
            Dim Consec_docto As String = ""
            Dim MensajeGT As New StringBuilder

            objTarea.LogFechaInicioWebServiceSiesa()
            objTarea.LogFechaInicioGeneracionPlano()

            dvDocumento = New DataView(dsOrigen.Tables(0))

            For Each Documento As DataRow In dsOrigen.Tables(0).Rows
                Try
                    ejecutarProcedimientoDescompromisoParcialPedido("sp_WMS_Pedidos_DescompromisoParcial", Documento.Item("CO"), Documento.Item("TipoPedido"), Documento.Item("NroPedido"))
                Catch ex As Exception
                    MensajeGT.AppendLine("No se descomprometió el documento tipo: " & Documento.Item("TipoPedido") & " Número:" & Documento.Item("NroPedido") & " ," & ex.Message.ToString)
                End Try
            Next

            If MensajeGT.ToString = "" Then
                objTarea.LogGeneracionDePlano(1)
                objTarea.LogWebServiceSiesa(1)
                objTarea.LogEjecucionCompleta()
            Else
                objTarea.LogGeneracionDePlano(0)
                objTarea.LogWebServiceSiesa(0)
                objTarea.LogMensajesError(MensajeGT.ToString)
            End If

            objTarea.LogFechaFinWebServiceSiesa()
            objTarea.LogFechaFinGeneracionPlano()

        Catch ex As Exception
            objTarea.LogFechaFin()
            objTarea.LogFin(objTarea.idLogPrincipal)
            objTarea.LogGeneracionDePlano(0)
            objTarea.LogWebServiceSiesa(0)
            objTarea.LogMensajesError(ex.InnerException.Message)
        Finally
            objTarea.LogFechaFin()
        End Try

    End Sub
    Public Sub compromisoParcial(ByVal NombreCampoIdDocumento As String)

        Dim dsOrigen As DataSet
        objTarea.BaseDatos = "SQL"
        Dim Plano As String
        Dim Resultado As String = ""
        Dim idTareaValido As Boolean = True
        Dim objValidacionDocumento As New clsTraslados

        'Id de la tarea enviado como parametro
        Dim args As String() = Environment.GetCommandLineArgs()

        If args.Length = 2 Then
            If IsNumeric(args(1)) Then
                objTarea.Tarea = args(1)
                dsOrigen = objTarea.DatosOrigen(False)
                If dsOrigen.Tables.Count > 0 Then
                    If dsOrigen.Tables(0).Rows.Count = 0 Then
                        Exit Sub
                    End If
                End If
                objTarea.LogPrincipalAlmacenar()
                objTarea.LogInicio()
            Else
                objTarea.LogRecuperacionDatosOrigen(0)
                objTarea.LogMensajesError("El argumento enviado debe ser numérico, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea")
                'objCorreo.EnviarCorreoTarea("Validaciones", objTarea.Destinatarios, "El argumento enviado debe ser numérico, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea", objTarea.Tarea)
                Exit Sub
            End If
        Else
            objTarea.LogRecuperacionDatosOrigen(0)
            objTarea.LogMensajesError("No se envió el Id de la tarea como argumento al programa, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea")
            'objCorreo.EnviarCorreoTarea("Validaciones", objTarea.Destinatarios, "No se envió el Id de la tarea como argumento al programa, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea", objTarea.Tarea)
            Exit Sub
        End If

        'Obtener los datos fuentes del cliente para la tarea X
        Try
            objTarea.LogFechaInicioRecuperacionDatosOrigen()
            dsOrigen = objTarea.DatosOrigen(False)
            If idTareaValido Then
                objTarea.LogFechaFinRecuperacionDatosOrigen()
                objTarea.LogRecuperacionDatosOrigen(1)
            Else
                objTarea.LogRecuperacionDatosOrigen(0)
                objTarea.LogMensajesError("No se encontró el Id de tarea: " & objTarea.Tarea & " en la base de datos, verifique la configuración de la tarea programada de Windows")
                'objCorreo.EnviarCorreoTarea("Obtencion datos fuentes de la tarea", objTarea.Destinatarios, "No se encontró el Id de tarea: " & objTarea.Tarea & " en la base de datos, verifique la configuración de la tarea programada de Windows", objTarea.Tarea)
                Exit Sub
            End If
        Catch ex As Exception
            objTarea.LogFechaFin()
            objTarea.LogFin(objTarea.idLogPrincipal)
            objTarea.LogRecuperacionDatosOrigen(0)
            objTarea.LogMensajesError(ex.Message)
            'objCorreo.EnviarCorreoTarea("Exception - Obtencion datos fuentes de la tarea", objTarea.Destinatarios, ex.Message, objTarea.Tarea)
            Exit Sub
        End Try

        Try
            Dim dvDocumento As DataView
            Dim NroRequisicion As String
            Dim MensajeGT As New StringBuilder
            Dim OperacionCompleta As Boolean

            objTarea.LogFechaInicioWebServiceSiesa()
            objTarea.LogFechaInicioGeneracionPlano()

            dvDocumento = New DataView(dsOrigen.Tables(0))
            Dim dtRequisicionesValidas As DataTable
            Dim dtRequisicionesInvalias As DataTable

            dvDocumento.RowFilter = "Valido = 0"
            dtRequisicionesInvalias = dvDocumento.ToTable
            dvDocumento.RowFilter = "Valido = 1"
            dtRequisicionesValidas = dvDocumento.ToTable.DefaultView.ToTable(True, "TipoRequisicion", "NroRequisicion")

            For Each DocumentoIncorrecto As DataRow In dtRequisicionesInvalias.Rows
                If DocumentoIncorrecto.Item("ValidacionCantidades") = "Parcial" Then
                    NroRequisicion = DocumentoIncorrecto.Item("NroRequisicion")
                    ejecutarProcedimientoCompromisoParcial("sp_WMS_Requisiciones_CompromisoParcial", DocumentoIncorrecto.Item("CO"), DocumentoIncorrecto.Item("TipoRequisicion"), DocumentoIncorrecto.Item("NroRequisicion"), DocumentoIncorrecto.Item("CantidadDespachada"), DocumentoIncorrecto.Item("RowidMovimiento"))
                Else
                    'Almacenar Log con información los documentos incorrectos
                    MensajeGT.AppendLine("La cantidad despachada es superior a la cantidad comprometida en el ERP, tipo de documento:" & DocumentoIncorrecto.Item("TipoRequisicion").ToString & " ,número de documento:" & DocumentoIncorrecto.Item("NroRequisicion").ToString & " ,referencia:" & DocumentoIncorrecto.Item("Referencia").ToString & " ,talla:" & DocumentoIncorrecto.Item("Talla").ToString & " ,color:" & DocumentoIncorrecto.Item("Color").ToString & " ,cantidad despachada:" & DocumentoIncorrecto.Item("CantidadDespachada").ToString & " ,cantidad comprometida:" & DocumentoIncorrecto.Item("CantidadComprometida").ToString)
                End If
            Next

            For Each DocumentoRequisicion As DataRow In dtRequisicionesValidas.Rows
                OperacionCompleta = True
                dvDocumento.RowFilter = "TipoRequisicion='" & DocumentoRequisicion.Item("TipoRequisicion") & "' and NroRequisicion='" & DocumentoRequisicion.Item("NroRequisicion") & "'"

                'Recorrer los movimientos de una requisicion
                For Each MovimientoRequisicion As DataRow In dvDocumento.ToTable.Rows
                    Try
                        NroRequisicion = MovimientoRequisicion.Item("NroRequisicion")
                        ejecutarProcedimientoCompromisoParcial("sp_WMS_Requisiciones_CompromisoParcial", MovimientoRequisicion.Item("CO"), MovimientoRequisicion.Item("TipoRequisicion"), MovimientoRequisicion.Item("NroRequisicion"), MovimientoRequisicion.Item("CantidadDespachada"), MovimientoRequisicion.Item("RowidMovimiento"))
                    Catch ex As Exception
                        'Almacenar log con el error generado al realizar el compromiso parcial
                        MensajeGT.AppendLine("No se ha generado el compromiso parcial:" & MovimientoRequisicion.Item("TipoRequisicion").ToString & " ,número de documento:" & MovimientoRequisicion.Item("NroRequisicion").ToString & " ,referencia:" & MovimientoRequisicion.Item("Referencia").ToString & " ,talla:" & MovimientoRequisicion.Item("Talla").ToString & " ,color:" & MovimientoRequisicion.Item("Color").ToString & " ,cantidad despachada:" & MovimientoRequisicion.Item("CantidadDespachada").ToString & " ,cantidad comprometida:" & MovimientoRequisicion.Item("CantidadComprometida").ToString & " Mensaje de error:" & ex.Message)
                        OperacionCompleta = False
                    End Try
                Next

                If OperacionCompleta = True Then
                    Try
                        ejecutarProcedimientoCompromisoParcialActualizarEstado("sp_WMS_Requisiciones_CompromisoParcial_ActualizarEstado", DocumentoRequisicion.Item("TipoRequisicion"), DocumentoRequisicion.Item("NroRequisicion"))
                    Catch ex As Exception
                        MensajeGT.AppendLine("No se ha actualizado el estado para el documento:" & DocumentoRequisicion.Item("TipoRequisicion").ToString & " ,número de documento:" & DocumentoRequisicion.Item("NroRequisicion").ToString & " Mensaje de error:" & ex.Message)
                    End Try
                End If
            Next


            If MensajeGT.ToString = "" Then
                objTarea.LogGeneracionDePlano(1)
                objTarea.LogWebServiceSiesa(1)
                objTarea.LogEjecucionCompleta()
            Else
                objTarea.LogGeneracionDePlano(0)
                objTarea.LogWebServiceSiesa(0)
                objTarea.LogMensajesError(MensajeGT.ToString)
            End If

            objTarea.LogFechaFinWebServiceSiesa()
            objTarea.LogFechaFinGeneracionPlano()

        Catch ex As Exception
            objTarea.LogFechaFin()
            objTarea.LogFin(objTarea.idLogPrincipal)
            objTarea.LogGeneracionDePlano(0)
            objTarea.LogWebServiceSiesa(0)
            objTarea.LogMensajesError(ex.Message)
        Finally
            objTarea.LogFechaFin()
        End Try

    End Sub
    Public Sub compromisoParcialPedidos(ByVal NombreCampoIdDocumento As String)

        Dim dsOrigen As DataSet
        objTarea.BaseDatos = "SQL"
        Dim Plano As String
        Dim Resultado As String = ""
        Dim idTareaValido As Boolean = True
        Dim objValidacionDocumento As New clsTraslados

        'Id de la tarea enviado como parametro
        Dim args As String() = Environment.GetCommandLineArgs()

        If args.Length = 2 Then
            If IsNumeric(args(1)) Then
                objTarea.Tarea = args(1)
                dsOrigen = objTarea.DatosOrigen(False)
                If dsOrigen.Tables.Count > 0 Then
                    If dsOrigen.Tables(0).Rows.Count = 0 Then
                        Exit Sub
                    End If
                End If
                objTarea.LogPrincipalAlmacenar()
                objTarea.LogInicio()
            Else
                objTarea.LogRecuperacionDatosOrigen(0)
                objTarea.LogMensajesError("El argumento enviado debe ser numérico, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea")
                'objCorreo.EnviarCorreoTarea("Validaciones", objTarea.Destinatarios, "El argumento enviado debe ser numérico, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea", objTarea.Tarea)
                Exit Sub
            End If
        Else
            objTarea.LogRecuperacionDatosOrigen(0)
            objTarea.LogMensajesError("No se envió el Id de la tarea como argumento al programa, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea")
            'objCorreo.EnviarCorreoTarea("Validaciones", objTarea.Destinatarios, "No se envió el Id de la tarea como argumento al programa, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea", objTarea.Tarea)
            Exit Sub
        End If

        'Obtener los datos fuentes del cliente para la tarea X
        Try
            objTarea.LogFechaInicioRecuperacionDatosOrigen()
            dsOrigen = objTarea.DatosOrigen(False)
            If idTareaValido Then
                objTarea.LogFechaFinRecuperacionDatosOrigen()
                objTarea.LogRecuperacionDatosOrigen(1)
            Else
                objTarea.LogRecuperacionDatosOrigen(0)
                objTarea.LogMensajesError("No se encontró el Id de tarea: " & objTarea.Tarea & " en la base de datos, verifique la configuración de la tarea programada de Windows")
                'objCorreo.EnviarCorreoTarea("Obtencion datos fuentes de la tarea", objTarea.Destinatarios, "No se encontró el Id de tarea: " & objTarea.Tarea & " en la base de datos, verifique la configuración de la tarea programada de Windows", objTarea.Tarea)
                Exit Sub
            End If
        Catch ex As Exception
            objTarea.LogFechaFin()
            objTarea.LogFin(objTarea.idLogPrincipal)
            objTarea.LogRecuperacionDatosOrigen(0)
            objTarea.LogMensajesError(ex.Message)
            'objCorreo.EnviarCorreoTarea("Exception - Obtencion datos fuentes de la tarea", objTarea.Destinatarios, ex.Message, objTarea.Tarea)
            Exit Sub
        End Try


        Try
            Dim dvDocumento As DataView
            Dim NroPedido As String
            Dim MensajeGT As New StringBuilder
            Dim OperacionCompleta As Boolean

            objTarea.LogFechaInicioWebServiceSiesa()
            objTarea.LogFechaInicioGeneracionPlano()

            dvDocumento = New DataView(dsOrigen.Tables(0))
            Dim dtPedidosValidas As DataTable
            Dim dtPedidosInvalias As DataTable

            dvDocumento.RowFilter = "Valido = 0"
            dtPedidosInvalias = dvDocumento.ToTable
            dvDocumento.RowFilter = "Valido = 1"
            dtPedidosValidas = dvDocumento.ToTable.DefaultView.ToTable(True, "CO", "TipoPedido", "NroPedido")

            For Each DocumentoIncorrecto As DataRow In dtPedidosInvalias.Rows
                If DocumentoIncorrecto.Item("ValidacionCantidades") = "Parcial" Then
                    NroPedido = DocumentoIncorrecto.Item("NroPedido")
                    ejecutarProcedimientoCompromisoParcialPedido("sp_WMS_Pedidos_CompromisoParcial", DocumentoIncorrecto.Item("CO"), DocumentoIncorrecto.Item("TipoPedido"), DocumentoIncorrecto.Item("NroPedido"), DocumentoIncorrecto.Item("CantidadDespachada"), DocumentoIncorrecto.Item("RowidMovimiento"))
                Else
                    'Almacenar Log con información los documentos incorrectos
                    MensajeGT.AppendLine("La cantidad despachada es superior a la cantidad comprometida en el ERP, tipo de documento:" & DocumentoIncorrecto.Item("TipoPedido").ToString & " ,número de documento:" & DocumentoIncorrecto.Item("NroPedido").ToString & " ,referencia:" & DocumentoIncorrecto.Item("Referencia").ToString & " ,talla:" & DocumentoIncorrecto.Item("Talla").ToString & " ,color:" & DocumentoIncorrecto.Item("Color").ToString & " ,cantidad despachada:" & DocumentoIncorrecto.Item("CantidadDespachada").ToString & " ,cantidad comprometida:" & DocumentoIncorrecto.Item("CantidadComprometida").ToString)
                End If
            Next

            For Each DocumentoPedido As DataRow In dtPedidosValidas.Rows
                OperacionCompleta = True
                dvDocumento.RowFilter = "TipoPedido='" & DocumentoPedido.Item("TipoPedido") & "' and NroPedido='" & DocumentoPedido.Item("NroPedido") & "'"

                'Recorrer los movimientos de una requisicion
                For Each MovimientoPedido As DataRow In dvDocumento.ToTable.Rows
                    Try
                        NroPedido = MovimientoPedido.Item("NroPedido")
                        ejecutarProcedimientoCompromisoParcialPedido("sp_WMS_Pedidos_CompromisoParcial", MovimientoPedido.Item("CO"), MovimientoPedido.Item("TipoPedido"), MovimientoPedido.Item("NroPedido"), MovimientoPedido.Item("CantidadDespachada"), MovimientoPedido.Item("RowidMovimiento"))
                    Catch ex As Exception
                        'Almacenar log con el error generado al realizar el compromiso parcial
                        MensajeGT.AppendLine("No se ha generado el compromiso parcial:" & MovimientoPedido.Item("TipoPedido").ToString & " ,número de documento:" & MovimientoPedido.Item("NroPedido").ToString & " ,referencia:" & MovimientoPedido.Item("Referencia").ToString & " ,talla:" & MovimientoPedido.Item("Talla").ToString & " ,color:" & MovimientoPedido.Item("Color").ToString & " ,cantidad despachada:" & MovimientoPedido.Item("CantidadDespachada").ToString & " ,cantidad comprometida:" & MovimientoPedido.Item("CantidadComprometida").ToString & " Mensaje de error:" & ex.Message)
                        OperacionCompleta = False
                    End Try
                Next

                If OperacionCompleta = True Then
                    Try
                        ejecutarProcedimientoCompromisoParcialActualizarEstadoPedido("sp_WMS_Pedido_CompromisoParcial_ActualizarEstado", DocumentoPedido.Item("CO"), DocumentoPedido.Item("TipoPedido"), DocumentoPedido.Item("NroPedido"))
                    Catch ex As Exception
                        MensajeGT.AppendLine("No se ha actualizado el estado para el documento:" & DocumentoPedido.Item("TipoPedido").ToString & " ,número de documento:" & DocumentoPedido.Item("NroPedido").ToString & " Mensaje de error:" & ex.Message)
                    End Try
                End If
            Next


            If MensajeGT.ToString = "" Then
                objTarea.LogGeneracionDePlano(1)
                objTarea.LogWebServiceSiesa(1)
                objTarea.LogEjecucionCompleta()
            Else
                objTarea.LogGeneracionDePlano(0)
                objTarea.LogWebServiceSiesa(0)
                objTarea.LogMensajesError(MensajeGT.ToString)
            End If

            objTarea.LogFechaFinWebServiceSiesa()
            objTarea.LogFechaFinGeneracionPlano()

        Catch ex As Exception
            objTarea.LogFechaFin()
            objTarea.LogFin(objTarea.idLogPrincipal)
            objTarea.LogGeneracionDePlano(0)
            objTarea.LogWebServiceSiesa(0)
            objTarea.LogMensajesError(ex.Message)
        Finally
            objTarea.LogFechaFin()
        End Try

    End Sub
    Private Sub ejecutarProcedimientoCompromisoParcial(ByVal procedimiento As String,
                                      ByVal CO As String,
                                      ByVal TipoRequisicion As String,
                                      ByVal NroRequisicion As String,
                                      ByVal CantidadDespachada As String,
                                      ByVal RowidMovimiento As String)

        Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)

        Try
            Dim sqlComando As SqlCommand = New SqlCommand
            sqlComando.CommandTimeout = 3600
            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = procedimiento
            sqlComando.Parameters.AddWithValue("CO", CO)
            sqlComando.Parameters.AddWithValue("TipoRequisicion", TipoRequisicion)
            sqlComando.Parameters.AddWithValue("NroRequisicion", NroRequisicion)
            sqlComando.Parameters.AddWithValue("CantidadDespachada", CantidadDespachada)
            sqlComando.Parameters.AddWithValue("RowidMovimiento", RowidMovimiento)
            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            sqlConexion.Close()
        End Try
    End Sub
    Private Sub ejecutarProcedimientoDescompromisoParcialPedido(ByVal procedimiento As String,
                                      ByVal CO As String,
                                      ByVal TipoPedido As String,
                                      ByVal NroPedido As String
                                      )

        Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)

        Try
            Dim sqlComando As SqlCommand = New SqlCommand
            sqlComando.CommandTimeout = 3600
            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = procedimiento
            sqlComando.Parameters.AddWithValue("CO", CO)
            sqlComando.Parameters.AddWithValue("TipoPedido", TipoPedido)
            sqlComando.Parameters.AddWithValue("NroPedido", NroPedido)
            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex

        Finally
            sqlConexion.Close()
        End Try
    End Sub
    Private Sub ejecutarProcedimientoCompromisoParcialPedido(ByVal procedimiento As String,
                                      ByVal CO As String,
                                      ByVal TipoPedido As String,
                                      ByVal NroPedido As String,
                                      ByVal CantidadDespachada As String,
                                      ByVal RowidMovimiento As String)

        Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)

        Try
            Dim sqlComando As SqlCommand = New SqlCommand
            sqlComando.CommandTimeout = 3600
            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = procedimiento
            sqlComando.Parameters.AddWithValue("CO", CO)
            sqlComando.Parameters.AddWithValue("TipoPedido", TipoPedido)
            sqlComando.Parameters.AddWithValue("NroPedido", NroPedido)
            sqlComando.Parameters.AddWithValue("CantidadDespachada", CantidadDespachada)
            sqlComando.Parameters.AddWithValue("RowidMovimiento", RowidMovimiento)
            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            sqlConexion.Close()
        End Try
    End Sub
    Private Sub ejecutarProcedimientoDescompromisoParcial(ByVal procedimiento As String,
                                      ByVal CO As String,
                                      ByVal TipoRequisicion As String,
                                      ByVal NroRequisicion As String
                                      )

        Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)

        Try
            Dim sqlComando As SqlCommand = New SqlCommand
            sqlComando.CommandTimeout = 3600
            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = procedimiento
            sqlComando.Parameters.AddWithValue("CO", CO)
            sqlComando.Parameters.AddWithValue("TipoRequisicion", TipoRequisicion)
            sqlComando.Parameters.AddWithValue("NroRequisicion", NroRequisicion)
            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex

        Finally
            sqlConexion.Close()
        End Try
    End Sub
    Private Sub ejecutarProcedimientoCompromisoParcialActualizarEstado(ByVal procedimiento As String,
                                      ByVal TipoRequisicion As String,
                                      ByVal NroRequisicion As String
                                      )

        Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)

        Try
            Dim sqlComando As SqlCommand = New SqlCommand
            sqlComando.CommandTimeout = 3600
            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = procedimiento
            sqlComando.Parameters.AddWithValue("TipoRequisicion", TipoRequisicion)
            sqlComando.Parameters.AddWithValue("NroRequisicion", NroRequisicion)
            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            sqlConexion.Close()
        End Try
    End Sub
    Private Sub ejecutarProcedimientoCompromisoParcialActualizarEstadoPedido(ByVal procedimiento As String,
                              ByVal CO As String,
                              ByVal TipoPedido As String,
                              ByVal NroPedido As String
                              )

        Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)

        Try
            Dim sqlComando As SqlCommand = New SqlCommand
            sqlComando.CommandTimeout = 3600
            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = procedimiento
            sqlComando.Parameters.AddWithValue("CO", CO)
            sqlComando.Parameters.AddWithValue("TipoPedido", TipoPedido)
            sqlComando.Parameters.AddWithValue("NroPedido", NroPedido)
            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            sqlConexion.Close()
        End Try
    End Sub
End Class

