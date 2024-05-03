Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Public Class clsMonoProceso
    Inherits clsConfiguracion
    Dim objTarea As New clsTarea
    Dim objCorreo As New clsCorreo
    Dim MensajeErrorSiesa As String = ""

    Public Sub Sincronizacion()

        Dim dsOrigen As DataSet
        objTarea.BaseDatos = "SQL"
        Dim Plano As String
        Dim Resultado As String = ""
        Dim idTareaValido As Boolean = True

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
                'objCorreo.EnviarCorreoTarea("GTIntegration-WMS: Validaciones", objTarea.Destinatarios, "El argumento enviado debe ser numérico, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea", objTarea.Tarea)
                Exit Sub
            End If
        Else
            objTarea.LogRecuperacionDatosOrigen(0)
            objTarea.LogMensajesError("No se envió el Id de la tarea como argumento al programa, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea")
            'objCorreo.EnviarCorreoTarea("GTIntegration-WMS: Validaciones", objTarea.Destinatarios, "No se envió el Id de la tarea como argumento al programa, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea", objTarea.Tarea)
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


        'Invocar Web Service de GT para Obtener el plano o realizar la importacion
        Try

            objTarea.LogFechaInicioGeneracionPlano()
            Dim objGenericTransfer As New wsGT.wsGenerarPlano
            objGenericTransfer.Timeout = 18000000
            Plano = objGenericTransfer.ImportarDatosDS(objTarea.IdDocumento,
                                                    objTarea.NombreDocumento,
                                                    2, 1, "gt", "gt",
                                                    dsOrigen, objTarea.RutaGeneracionPlano)

            objTarea.LogGeneracionDePlano(1)

            If Plano <> "Importacion exitosa" Then
                objTarea.LogWebServiceSiesa(0)
                almacenarLogErrores(Plano)
                'objCorreo.EnviarCorreoTarea("Resultado Consumo GTIntegration", objTarea.Destinatarios, Plano, objTarea.Tarea)
            Else
                objTarea.LogWebServiceSiesa(1)
                objTarea.LogEjecucionCompleta()
            End If

            objTarea.LogFin(objTarea.idLogPrincipal)
            objTarea.LogFechaFin()

        Catch ex As Exception
            objTarea.LogFechaFin()
            objTarea.LogFin(objTarea.idLogPrincipal)
            objTarea.LogGeneracionDePlano(0)
            objTarea.LogMensajesError(ex.InnerException.Message)
            'objCorreo.EnviarCorreoTarea("Exception - Consumo WS GTIntegration", objTarea.Destinatarios, ex.InnerException.Message, objTarea.Tarea)
            Exit Sub
        End Try

    End Sub

    Public Sub SincronizacionUnoAUnoMultipleBodega(ByVal NombreCampoIdDocumento As String, ByVal NombreCampoIdMovimiento As String, Optional ByVal NombreCampodvDoctoRelacion As String = "")

        Dim dsOrigen As DataSet
        objTarea.BaseDatos = "SQL"
        Dim Plano As String
        Dim Resultado As String = ""
        Dim idTareaValido As Boolean = True

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

        'Obtener los datos fuentes 
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


        'Invocar Web Service de GT para Obtener el plano o realizar la importacion
        Try

            objTarea.LogFechaInicioGeneracionPlano()

            Dim ProcesoCompleto As Boolean = True
            Dim strMensajesSiesa As New StringBuilder
            Dim NumeroSecciones As Integer
            Dim dsOrigenDatosPorDocumento As New DataSet
            Dim dtDocumento As DataTable
            Dim dvDocumento As DataView
            Dim dtMovimiento As DataTable
            Dim dvMovimiento As DataView
            Dim dvDoctoRelacion As DataView
            Dim dtDoctoRelacion As DataTable



            NumeroSecciones = dsOrigen.Tables.Count

            dvDocumento = New DataView(dsOrigen.Tables(0))
            dvMovimiento = New DataView(dsOrigen.Tables(1))

            For Each Documento As DataRow In dsOrigen.Tables(0).Rows

                dsOrigenDatosPorDocumento.Tables.Clear()
                dvDocumento.RowFilter = NombreCampoIdDocumento & " = '" & Documento.Item(NombreCampoIdDocumento) & "'"
                dvMovimiento.RowFilter = NombreCampoIdMovimiento & " = '" & Documento.Item(NombreCampoIdDocumento) & "'"

                dtDocumento = dvDocumento.ToTable
                dtMovimiento = dvMovimiento.ToTable
                dsOrigenDatosPorDocumento.Tables.Add(dtDocumento)
                dsOrigenDatosPorDocumento.Tables.Add(dtMovimiento)


                If Documento.Item("ValidacionBodega").ToString() = "Si" Then
                    If dtMovimiento.Rows(0).Item("adj_qty") < 0 Then


                        Dim cantidaDisponible As Int16 = ConsultarExistencia(dtMovimiento.Rows(0).Item("f470_id_bodega"), dtMovimiento.Rows(0).Item("f470_referencia_item"), dtMovimiento.Rows(0).Item("f470_id_ext1_detalle"), dtMovimiento.Rows(0).Item("f470_id_ext2_detalle"))
                        Dim cantidadRequerida = Math.Abs(CInt(dtMovimiento.Rows(0).Item("adj_qty")))

                        If cantidaDisponible < cantidadRequerida Then

                            If cantidaDisponible > 0 Then
                                dtMovimiento.Rows(0).Item("f470_cant_base") = cantidaDisponible
                                CargarSiesa(dsOrigenDatosPorDocumento, Plano, ProcesoCompleto, strMensajesSiesa)
                            End If

                            cantidadRequerida = cantidadRequerida - cantidaDisponible
                            cantidaDisponible = ConsultarExistencia(Documento.Item("Bodega2").ToString(), dtMovimiento.Rows(0).Item("f470_referencia_item"), dtMovimiento.Rows(0).Item("f470_id_ext1_detalle"), dtMovimiento.Rows(0).Item("f470_id_ext2_detalle"))

                            If cantidaDisponible <= cantidadRequerida Then

                                If cantidaDisponible > 0 Then
                                    dtMovimiento.Rows(0).Item("f470_cant_base") = cantidaDisponible
                                    dtMovimiento.Rows(0).Item("f470_id_bodega") = Documento.Item("Bodega2").ToString()
                                    CargarSiesa(dsOrigenDatosPorDocumento, Plano, ProcesoCompleto, strMensajesSiesa)
                                End If

                                cantidadRequerida = cantidadRequerida - cantidaDisponible

                                cantidaDisponible = ConsultarExistencia(Documento.Item("Bodega3").ToString(), dtMovimiento.Rows(0).Item("f470_referencia_item"), dtMovimiento.Rows(0).Item("f470_id_ext1_detalle"), dtMovimiento.Rows(0).Item("f470_id_ext2_detalle"))

                                If cantidaDisponible <= cantidadRequerida Then
                                    If cantidaDisponible > 0 Then
                                        dtMovimiento.Rows(0).Item("f470_cant_base") = cantidaDisponible
                                        dtMovimiento.Rows(0).Item("f470_id_bodega") = Documento.Item("Bodega3").ToString()
                                        CargarSiesa(dsOrigenDatosPorDocumento, Plano, ProcesoCompleto, strMensajesSiesa)
                                    End If

                                    cantidadRequerida = cantidadRequerida - cantidaDisponible

                                    If cantidadRequerida > 0 Then
                                        'objCorreo.EnviarCorreoTarea(
                                        '    "Ajustes - Cantidades faltantes",
                                        '    objTarea.CorreosNotificaciones,
                                        '    String.Format("<br> No se pudo completar el ajuste por salida requerido para el group_nro: {0},<br>  Referencia: {1}, <br> Extension 1: {2}, <br> Extension 2: {3}, <br> Bodegas: {4}, {5}, {6}, <br> Cantidad faltante: {7}",
                                        '                  Documento.Item("group_nbr"),
                                        '                  dtMovimiento.Rows(0).Item("f470_referencia_item"),
                                        '                  dtMovimiento.Rows(0).Item("f470_id_ext1_detalle"),
                                        '                  dtMovimiento.Rows(0).Item("f470_id_ext2_detalle"),
                                        '                  dtMovimiento.Rows(0).Item("f470_id_bodega"),
                                        '                  Documento.Item("Bodega2").ToString(),
                                        '                  Documento.Item("Bodega3").ToString(),
                                        '                  cantidadRequerida),
                                        '                  objTarea.Tarea
                                        '                  )

                                    End If

                                Else
                                    dtMovimiento.Rows(0).Item("f470_cant_base") = cantidadRequerida
                                    dtMovimiento.Rows(0).Item("f470_id_bodega") = Documento.Item("Bodega3").ToString()
                                    CargarSiesa(dsOrigenDatosPorDocumento, Plano, ProcesoCompleto, strMensajesSiesa)
                                End If
                            Else
                                dtMovimiento.Rows(0).Item("f470_cant_base") = cantidadRequerida
                                dtMovimiento.Rows(0).Item("f470_id_bodega") = Documento.Item("Bodega2").ToString()
                                CargarSiesa(dsOrigenDatosPorDocumento, Plano, ProcesoCompleto, strMensajesSiesa)
                            End If
                        Else
                            CargarSiesa(dsOrigenDatosPorDocumento, Plano, ProcesoCompleto, strMensajesSiesa)
                        End If
                    Else
                        CargarSiesa(dsOrigenDatosPorDocumento, Plano, ProcesoCompleto, strMensajesSiesa)
                    End If
                Else
                    CargarSiesa(dsOrigenDatosPorDocumento, Plano, ProcesoCompleto, strMensajesSiesa)
                End If

            Next


            objTarea.LogGeneracionDePlano(1)

            If ProcesoCompleto = True Then
                objTarea.LogWebServiceSiesa(1)
                objTarea.LogEjecucionCompleta()
            Else
                objTarea.LogWebServiceSiesa(0)
                almacenarLogErrores(Plano)
                'objCorreo.EnviarCorreoTarea("Resultado Consumo GTIntegration", objTarea.Destinatarios, strMensajesSiesa.ToString, objTarea.Tarea)
            End If

            objTarea.LogFin(objTarea.idLogPrincipal)
            objTarea.LogFechaFin()

        Catch ex As Exception
            objTarea.LogFechaFin()
            objTarea.LogFin(objTarea.idLogPrincipal)
            objTarea.LogGeneracionDePlano(0)
            objTarea.LogMensajesError(ex.InnerException.Message)
            'objCorreo.EnviarCorreoTarea("Exception - Consumo WS GTIntegration", objTarea.Destinatarios, ex.InnerException.Message, objTarea.Tarea)
            Exit Sub
        End Try

    End Sub

    Private Function ConsultarExistencia(ByVal Bodega As String, ByVal Referencia As String, ByVal Ext1 As String, ByVal Ext2 As String) As Int16

        Dim dsConfiguracionGT As New DataSet
        Dim sqlComando As New SqlCommand
        Dim sqlAdaptador As SqlDataAdapter = New SqlDataAdapter
        Dim ds As New DataSet


        sqlComando.CommandTimeout = 36000000
        sqlComando.Connection = sqlConexion
        sqlComando.CommandType = CommandType.StoredProcedure
        sqlComando.CommandText = "sp_WMS_ConsultarBodegas"

        sqlComando.Parameters.AddWithValue("@Bodega", Bodega)
        sqlComando.Parameters.AddWithValue("@RefItem", Referencia)
        sqlComando.Parameters.AddWithValue("@Ext1", Ext1)
        sqlComando.Parameters.AddWithValue("@Ext2", Ext2)

        Try
            sqlAdaptador.SelectCommand = sqlComando
            sqlAdaptador.Fill(ds)
            Return ds.Tables(0).Rows(0).Item("cantidad")
        Catch ex As Exception
            Throw ex
        Finally
            sqlComando.Parameters.Clear()
            sqlComando.Connection.Close()
        End Try

    End Function

    Private Sub CargarSiesa(ByVal dsOrigenDatosPorDocumento As DataSet, ByRef Plano As String, ByRef ProcesoCompleto As Boolean, ByRef strMensajesSiesa As StringBuilder)
        Dim objValidacionDocumento As New clsTraslados

        Dim objGenericTransfer As New wsGT.wsGenerarPlano
        objGenericTransfer.Timeout = 18000000
        If objValidacionDocumento.ConsultarTrasladoSiesa(dsOrigenDatosPorDocumento.Tables(0).Rows(0).Item("f350_notas")) = False Then
            Plano = objGenericTransfer.ImportarDatosDS(objTarea.IdDocumento,
                        objTarea.NombreDocumento,
                        2, 1, "gt", "gt",
                        dsOrigenDatosPorDocumento, objTarea.RutaGeneracionPlano)
            If Plano <> "Importacion exitosa" Then
                ProcesoCompleto = False
                strMensajesSiesa.AppendLine(Plano)
            End If
        Else
            Plano = "planoPreviamenteImportado"
        End If

    End Sub

    Public Sub SincronizacionUnoAUno(ByVal NombreCampoIdDocumento As String, ByVal NombreCampoIdMovimiento As String, Optional ByVal NombreCampodvDoctoRelacion As String = "")

        Dim dsOrigen As DataSet
        objTarea.BaseDatos = "SQL"
        Dim Plano As String
        Dim Resultado As String = ""
        Dim idTareaValido As Boolean = True
        Dim MensajeError As String
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

        'Validación y verificación de estados
        Dim objEstadoVerificacion As New clsEstados
        objEstadoVerificacion.verificacionEstadosTareas(objTarea.Tarea)

        'Obtener los datos fuentes del cliente para la tarea X
        Try
            objTarea.LogFechaInicioRecuperacionDatosOrigen()
            'dsOrigen = objTarea.DatosOrigen(False)
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


        'Invocar Web Service de GT para Obtener el plano o realizar la importacion
        Try
            Dim objGenericTransfer As New wsGT.wsGenerarPlano
            Dim ProcesoCompleto As Boolean = True
            Dim strMensajesSiesa As New StringBuilder
            Dim NumeroSecciones As Integer
            Dim dsOrigenDatosPorDocumento As New DataSet
            Dim dtDocumento As DataTable
            Dim dvDocumento As DataView
            Dim dtMovimiento As DataTable
            Dim dvMovimiento As DataView
            Dim dvDoctoRelacion As DataView
            Dim dtDoctoRelacion As DataTable
            Dim Consec_docto As String = ""
            Dim ActualizarEstado As Boolean
            Dim MensajeGT As New StringBuilder

            objGenericTransfer.Timeout = 18000000

            objTarea.LogFechaInicioWebServiceSiesa()
            objTarea.LogFechaInicioGeneracionPlano()

            NumeroSecciones = dsOrigen.Tables.Count

            If NumeroSecciones = 1 Then
                dvDocumento = New DataView(dsOrigen.Tables(0))

                For Each Documento As DataRow In dsOrigen.Tables(0).Rows
                    dsOrigenDatosPorDocumento.Tables.Clear()

                    If objTarea.Tarea = 78 Then
                        dvDocumento.RowFilter = "F350_ID_CO= '" & Documento.Item("F350_ID_CO") & "' and F350_ID_TIPO_DOCTO = '" & Documento.Item("F350_ID_TIPO_DOCTO") & "' and F350_CONSEC_DOCTO = " & Documento.Item("F350_CONSEC_DOCTO") & " and f462_notas = '" & Documento.Item("f462_notas") & "'"
                        'ElseIf objTarea.Tarea = 167 Then
                        '    dvDocumento.RowFilter = "f440_id_co= '" & Documento.Item("f440_id_co") & "' and f440_id_tipo_docto = '" & Documento.Item("f440_id_tipo_docto") & "' and f440_consec_docto = " & Documento.Item("f440_consec_docto") & " and f441_nro_registro = '" & Documento.Item("f441_nro_registro") & "'"
                    ElseIf objTarea.Tarea = 182 Then
                        dvDocumento.RowFilter = "f430_id_co= '" & Documento.Item("f430_id_co") & "' and f430_id_tipo_docto = '" & Documento.Item("f430_id_tipo_docto") & "' and f430_consec_docto = " & Documento.Item("f430_consec_docto") & " and f431_nro_registro = '" & Documento.Item("f431_nro_registro") & "'"
                    Else
                        dvDocumento.RowFilter = NombreCampoIdDocumento & " = '" & Documento.Item(NombreCampoIdDocumento) & "'"
                    End If

                    dtDocumento = dvDocumento.ToTable
                    dsOrigenDatosPorDocumento.Tables.Add(dtDocumento)

                    If objTarea.Tarea = 160 Or objTarea.Tarea = 162 Then 'Pedidos PV-PVI - Control de cambios (Plan B)
                        ActualizarEstado = False
                        If Consec_docto <> dtDocumento.Rows(0).Item("f430_consec_docto").ToString Then
                            Plano = objGenericTransfer.ImportarDatosDS(objTarea.IdDocumento,
                           objTarea.NombreDocumento,
                           2, 1, "gt", "gt",
                           dsOrigenDatosPorDocumento, objTarea.RutaGeneracionPlano)
                            ActualizarEstado = True
                        End If
                        Consec_docto = dtDocumento.Rows(0).Item("f430_consec_docto")
                    ElseIf objTarea.Tarea = 166 Or objTarea.Tarea = 168 Then 'Pedidos RQI - Control de cambios (Plan B)
                        ActualizarEstado = False
                        If Consec_docto <> dtDocumento.Rows(0).Item("f440_consec_docto").ToString Then
                            Plano = objGenericTransfer.ImportarDatosDS(objTarea.IdDocumento,
                           objTarea.NombreDocumento,
                           2, 1, "gt", "gt",
                           dsOrigenDatosPorDocumento, objTarea.RutaGeneracionPlano)
                            ActualizarEstado = True
                        End If
                        Consec_docto = dtDocumento.Rows(0).Item("f440_consec_docto")
                    Else
                        Plano = objGenericTransfer.ImportarDatosDS(objTarea.IdDocumento,
                                                objTarea.NombreDocumento,
                                                2, 1, "gt", "gt",
                                                dsOrigenDatosPorDocumento, objTarea.RutaGeneracionPlano)
                    End If

                    If Plano <> "Importacion exitosa" Then
                        ProcesoCompleto = False
                        strMensajesSiesa.AppendLine(Plano)

                        If objTarea.Tarea = 147 Or objTarea.Tarea = 154 Then 'Ecommerce
                            MensajeErrorSiesa = descomponerLogErrores(Plano)
                            MensajeGT.AppendLine("No se importo el plano. Tipo de pedido: " + dtDocumento.Rows(0).Item("f430_id_tipo_docto").ToString + " ,nro de pedido: " + dtDocumento.Rows(0).Item("f430_consec_docto").ToString + " ," + MensajeErrorSiesa.ToString)
                        ElseIf objTarea.Tarea = 77 Then 'Transferencia desde RQI
                            MensajeErrorSiesa = descomponerLogErrores(Plano)
                            MensajeGT.AppendLine("No se importo el plano. Tipo y número de pedido: " + dtDocumento.Rows(0).Item("f350_notas").ToString + " ," + MensajeErrorSiesa.ToString)
                        ElseIf objTarea.Tarea = 78 Then 'Ecommerce
                            MensajeErrorSiesa = descomponerLogErrores(Plano)
                            MensajeGT.AppendLine("No se importo el plano. Tipo de pedido: " + dtDocumento.Rows(0).Item("F430_ID_TIPO_DOCTO").ToString + " ,nro de pedido: " + dtDocumento.Rows(0).Item("F430_CONSEC_DOCTO").ToString + " ," + MensajeErrorSiesa.ToString)
                        ElseIf objTarea.Tarea = 160 Or objTarea.Tarea = 162 Then 'Pedidos PV-PVI - Control de cambios(Plan b)
                            MensajeErrorSiesa = descomponerLogErrores(Plano)
                            MensajeGT.AppendLine("No se importo el plano. Tipo de pedido: " + dtDocumento.Rows(0).Item("f430_id_tipo_docto").ToString + " ,nro de pedido: " + dtDocumento.Rows(0).Item("f430_consec_docto").ToString + " ," + MensajeErrorSiesa.ToString)
                        ElseIf objTarea.Tarea = 161 Then 'Pedidos PV-PVI - Control de cambios(Plan b)
                            MensajeErrorSiesa = descomponerLogErrores(Plano)
                            MensajeGT.AppendLine("No se importo el plano. Tipo de pedido: " + dtDocumento.Rows(0).Item("F430_ID_TIPO_DOCTO").ToString + " ,nro de pedido: " + dtDocumento.Rows(0).Item("F430_CONSEC_DOCTO").ToString + " ," + MensajeErrorSiesa.ToString)
                        ElseIf objTarea.Tarea = 166 Or objTarea.Tarea = 168 Then 'Pedidos RQI - Control de cambios(Plan b)
                            MensajeErrorSiesa = descomponerLogErrores(Plano)
                            MensajeGT.AppendLine("No se importo el plano. Tipo de pedido: " + dtDocumento.Rows(0).Item("f440_id_tipo_docto").ToString + " ,nro de pedido: " + dtDocumento.Rows(0).Item("f440_consec_docto").ToString + " ," + MensajeErrorSiesa.ToString)
                        End If

                    Else
                        If dtDocumento.Rows.Count > 0 Then

                            Select Case objTarea.Tarea
                                Case 160 'Descomprometer Parcial Pedidos                                PRODUCCION
                                    If ActualizarEstado = True Then
                                        Dim objEstado As New clsEstados
                                        objEstado.Tabla = "WMS_DETALLE_DESPACHO_CARGAS"
                                        objEstado.Estado = 1
                                        objEstado.VarcharID1 = dtDocumento.Rows(0).Item("f430_consec_docto")
                                        objEstado.VarcharID2 = dtDocumento.Rows(0).Item("f430_id_tipo_docto")
                                        objEstado.ModificarEstado()
                                    End If
                                Case 162 'Comromete Parcial Pedidos
                                    If ActualizarEstado = True Then
                                        'Dim objEstado As New clsEstados
                                        'objEstado.Tabla = "WMS_DETALLE_DESPACHO_CARGAS"
                                        'objEstado.Estado = 2
                                        'objEstado.VarcharID1 = dtDocumento.Rows(0).Item("f430_consec_docto")
                                        'objEstado.VarcharID2 = dtDocumento.Rows(0).Item("f430_id_tipo_docto")
                                        'objEstado.ModificarEstado()
                                        Dim objEstado As New clsEstados
                                        objEstado.TipoPedido = dtDocumento.Rows(0).Item("f430_id_tipo_docto")
                                        objEstado.NroPedido = dtDocumento.Rows(0).Item("f430_consec_docto")
                                        objEstado.CompararCantidadesComprometidasParcial()
                                    End If
                                Case 147 'Compromiso Parcial ECOMMERCE                
                                    If ActualizarEstado = True Then
                                        Dim objEstado As New clsEstados
                                        objEstado.TipoPedidoPVE = dtDocumento.Rows(0).Item("f430_id_tipo_docto")
                                        objEstado.NroPedidoPVE = dtDocumento.Rows(0).Item("f430_consec_docto")
                                        objEstado.CompararCantidadesComprometidasParcialPVE()
                                    End If
                                Case 161 'Remisionar Pedidos
                                    'Dim objEstado As New clsEstados
                                    'objEstado.Tabla = "WMS_DETALLE_DESPACHO_CARGAS"
                                    'objEstado.Estado = 3
                                    'objEstado.VarcharID1 = dtDocumento.Rows(0).Item("F430_CONSEC_DOCTO")
                                    'objEstado.VarcharID2 = dtDocumento.Rows(0).Item("F430_ID_TIPO_DOCTO")
                                    'objEstado.ModificarEstado()
                                    Dim objEstado As New clsEstados
                                    objEstado.TipoPedidoPV_PVI = dtDocumento.Rows(0).Item("f430_id_tipo_docto")
                                    objEstado.NroPedidoPV_PVI = dtDocumento.Rows(0).Item("f430_consec_docto")
                                    objEstado.ValidarTotalCantidadesRemisionadasyCantidadesDespachadasPV_PVI()
                                Case 166 'Compromete Parcial Requisicion
                                    'Dim objEstado As New clsEstados
                                    'objEstado.Tabla = "WMS_DETALLE_DESPACHO_CARGAS"
                                    'objEstado.Estado = 2
                                    'objEstado.VarcharID1 = dtDocumento.Rows(0).Item("f440_consec_docto")
                                    'objEstado.VarcharID2 = "RQI"
                                    'objEstado.ModificarEstado()
                                    Dim objEstado As New clsEstados
                                    objEstado.TipoPedidoRQI = dtDocumento.Rows(0).Item("f440_id_tipo_docto")
                                    objEstado.NroPedidoRQI = dtDocumento.Rows(0).Item("f440_consec_docto")
                                    objEstado.CompararCantidadesComprometidasParcialRQI()
                                Case 168 'Descomprometer Parcial Requisicion
                                    Dim objEstado As New clsEstados
                                    objEstado.Tabla = "WMS_DETALLE_DESPACHO_CARGAS"
                                    objEstado.Estado = 1
                                    objEstado.VarcharID1 = dtDocumento.Rows(0).Item("f440_consec_docto")
                                    objEstado.VarcharID2 = "RQI"
                                    objEstado.ModificarEstado()
                                'Case 167 'Cancelar Requisiciones 
                                '    Dim objEstado As New clsEstados
                                '    objEstado.Tabla = "WMS_HISTORIAL_INVENTARIO"
                                '    objEstado.Estado = 1
                                '    objEstado.VarcharID1 = dtDocumento.Rows(0).Item("f440_consec_docto")
                                '    objEstado.VarcharID2 = "RQI"
                                '    objEstado.ModificarEstado()
                                Case 78 'Cambio de estado - Remisión Ecommerce
                                    Dim objEstado As New clsEstados
                                    objEstado.cambiarEstadoRemisionEcommerce(dtDocumento.Rows(0).Item("F350_ID_CO"),
                                                                             dtDocumento.Rows(0).Item("F430_ID_TIPO_DOCTO"),
                                                                             dtDocumento.Rows(0).Item("F430_CONSEC_DOCTO"))

                                Case 167 'Cancelación requisiciones
                                    Dim objEstado As New clsEstados

                                    For Each Movimiento As DataRow In dsOrigenDatosPorDocumento.Tables(0).Rows
                                        objEstado.cambiarEstadoCancelacionRequisiciones(Movimiento.Item("f440_id_co"),
                                                                       Movimiento.Item("f440_id_tipo_docto"),
                                                                       Movimiento.Item("f440_consec_docto"),
                                                                       Movimiento.Item("f441_referencia_item"),
                                                                       Movimiento.Item("f441_id_ext1_detalle"),
                                                                       Movimiento.Item("f441_id_ext2_detalle"))
                                    Next
                                Case 182 'Cancelacion de pedidos via conector
                                    Dim objEstado As New clsEstados

                                    For Each Movimiento As DataRow In dsOrigenDatosPorDocumento.Tables(0).Rows
                                        objEstado.cambiarEstadoCancelacionPedidos(Movimiento.Item("f430_id_co"),
                                                                       Movimiento.Item("f430_id_tipo_docto"),
                                                                       Movimiento.Item("f430_consec_docto"),
                                                                       Movimiento.Item("f431_referencia_item"),
                                                                       Movimiento.Item("f431_id_ext1_detalle"),
                                                                       Movimiento.Item("f431_id_ext2_detalle"),
                                                                       Movimiento.Item("f431_nro_registro"))
                                    Next
                            End Select
                        End If
                    End If
                Next
            ElseIf NumeroSecciones = 2 Then

                dvDocumento = New DataView(dsOrigen.Tables(0))
                dvMovimiento = New DataView(dsOrigen.Tables(1))

                For Each Documento As DataRow In dsOrigen.Tables(0).Rows
                    dsOrigenDatosPorDocumento.Tables.Clear()

                    dvDocumento.RowFilter = NombreCampoIdDocumento & " = '" & Documento.Item(NombreCampoIdDocumento) & "'"
                    dvMovimiento.RowFilter = NombreCampoIdMovimiento & " = '" & Documento.Item(NombreCampoIdDocumento) & "'"

                    dtDocumento = dvDocumento.ToTable
                    dtMovimiento = dvMovimiento.ToTable
                    dsOrigenDatosPorDocumento.Tables.Add(dtDocumento)
                    dsOrigenDatosPorDocumento.Tables.Add(dtMovimiento)

                    If objTarea.Tarea = 159 And objTarea.Tarea = 41 Then
                        If objValidacionDocumento.ConsultarEOP(Documento.Item("f350_notas"), Documento.Item("f350_id_tipo_docto")) = False Then
                            Plano = objGenericTransfer.ImportarDatosDS(objTarea.IdDocumento,
                                            objTarea.NombreDocumento,
                                            2, 1, "gt", "gt",
                                            dsOrigenDatosPorDocumento, objTarea.RutaGeneracionPlano)
                        Else
                            Plano = "planoPreviamenteImportado"
                        End If
                    Else
                        Plano = objGenericTransfer.ImportarDatosDS(objTarea.IdDocumento,
                                            objTarea.NombreDocumento,
                                            2, 1, "gt", "gt",
                                            dsOrigenDatosPorDocumento, objTarea.RutaGeneracionPlano)
                    End If

                    If Plano <> "planoPreviamenteImportado" Then
                        If Plano <> "Importacion exitosa" Then
                            ProcesoCompleto = False
                            strMensajesSiesa.AppendLine(Plano)

                            If objTarea.Tarea = 41 Then 'TIPO ASN TAL
                                MensajeErrorSiesa = descomponerLogErrores(Plano)
                                MensajeGT.AppendLine("No se importo el plano. Tipo ASN TAL, Nro Orden de Produccion: " + dtDocumento.Rows(0).Item("shipment_hdr_cust_field_1").ToString + " ," + MensajeErrorSiesa.ToString)
                            ElseIf objTarea.Tarea = 45 Then '---TIPO ASN TAL--------------------------------------------------------------------------
                                MensajeErrorSiesa = descomponerLogErrores(Plano)
                                MensajeGT.AppendLine("No se importo el plano. Tipo ASN TAL, Nro Orden de Produccion: " + dtDocumento.Rows(0).Item("NroOP").ToString + " ," + MensajeErrorSiesa.ToString)
                            ElseIf objTarea.Tarea = 49 Then '---TIPO ASN XDOCK--------------------------------------------------------------------------
                                MensajeErrorSiesa = descomponerLogErrores(Plano)
                                MensajeGT.AppendLine("No se importo el plano. Tipo ASN XDOCK, Nro Orden de Produccion: " + dtDocumento.Rows(0).Item("NroOP").ToString + " ," + MensajeErrorSiesa.ToString)
                            ElseIf objTarea.Tarea = 50 Then '---TIPO ASN TAL--------------------------------------------------------------------------
                                MensajeErrorSiesa = descomponerLogErrores(Plano)
                                MensajeGT.AppendLine("No se importo el plano. Tipo ASN XDOCK, Nro Orden de Produccion: " + dtDocumento.Rows(0).Item("shipment_hdr_cust_field_1").ToString + " ," + MensajeErrorSiesa.ToString)
                            ElseIf objTarea.Tarea = 55 Then '---TIPO ASN PRO--------------------------------------------------------------------------
                                MensajeErrorSiesa = descomponerLogErrores(Plano)
                                MensajeGT.AppendLine("No se importo el plano. Tipo ASN PRO, Nro Orden de Produccion: " + dtDocumento.Rows(0).Item("f420_consec_docto").ToString + " ," + MensajeErrorSiesa.ToString)
                            ElseIf objTarea.Tarea = 83 Then '---TIPO ASN TRA-------------------------------------------------------------------------
                                MensajeErrorSiesa = descomponerLogErrores(Plano)
                                MensajeGT.AppendLine("No se importo el plano. Tipo ASN TRA, Nro ID ASN: " + dtDocumento.Rows(0).Item("f350_consec_docto").ToString + " ," + MensajeErrorSiesa.ToString)
                            ElseIf objTarea.Tarea = 84 Then '---SALIDA EN TRANSITO-------------------------------------------------------------------------
                                MensajeErrorSiesa = descomponerLogErrores(Plano)
                                MensajeGT.AppendLine("No se importo el plano. Tipo ASN TRA, Nro ID ASN: " + dtDocumento.Rows(0).Item("f350_consec_docto").ToString + " ," + MensajeErrorSiesa.ToString)
                            ElseIf objTarea.Tarea = 131 Then '---GP REMISIÓN DIRECTA-------------------------------------------------------------------------
                                MensajeErrorSiesa = descomponerLogErrores(Plano)
                                MensajeGT.AppendLine("No se importo el plano. Tipo de pedido y Nro de pedido: " + dtDocumento.Rows(0).Item("f460_notas").ToString + " ," + MensajeErrorSiesa.ToString)
                            ElseIf objTarea.Tarea = 143 Then '---LOGISTICA INVERSA-------------------------------------------------------------------------
                                MensajeErrorSiesa = descomponerLogErrores(Plano)
                                MensajeGT.AppendLine("No se importo el plano. Tipo ASN TRA, Nro transferencia de salida: " + dtDocumento.Rows(0).Item("f350_consec_docto_base").ToString + " ," + MensajeErrorSiesa.ToString)
                            ElseIf objTarea.Tarea = 144 Then '---TIPO ASN DEV - MUE -REC -TAL-------------------------------------------------------------------------
                                MensajeErrorSiesa = descomponerLogErrores(Plano)
                                MensajeGT.AppendLine("No se importo el plano. Tipo ASN TRA, Nro transferencia de salida: " + dtDocumento.Rows(0).Item("f350_consec_docto_base").ToString + " ," + MensajeErrorSiesa.ToString)
                            ElseIf objTarea.Tarea = 145 Then '---TIPO ASN TRA-------------------------------------------------------------------------
                                MensajeErrorSiesa = descomponerLogErrores(Plano)
                                MensajeGT.AppendLine("No se importo el plano. Tipo ASN TRA, Nro TR Salida en Transito: " + dtDocumento.Rows(0).Item("f350_consec_docto_base").ToString + " ," + MensajeErrorSiesa.ToString)
                            ElseIf objTarea.Tarea = 146 Then '---GP RQI-------------------------------------------------------------------------
                                MensajeErrorSiesa = descomponerLogErrores(Plano)
                                MensajeGT.AppendLine("No se importo el plano. Tipo y Nro de pedido: " + dtDocumento.Rows(0).Item("f350_notas").ToString + " ," + MensajeErrorSiesa.ToString)
                            End If

                        Else
                            objTarea.LogFin(objTarea.idLogPrincipal)
                        End If
                    End If

                Next
            ElseIf NumeroSecciones = 3 Then

                dvDocumento = New DataView(dsOrigen.Tables(0))
                dvMovimiento = New DataView(dsOrigen.Tables(1))
                dvDoctoRelacion = New DataView(dsOrigen.Tables(2))

                For Each Documento As DataRow In dsOrigen.Tables(0).Rows
                    dsOrigenDatosPorDocumento.Tables.Clear()

                    If objTarea.Tarea = 128 Then
                        dvDocumento.RowFilter = "F350_ID_CO= '" & Documento.Item("F350_ID_CO") & "' and F350_ID_TIPO_DOCTO = '" & Documento.Item("F350_ID_TIPO_DOCTO") & "' and F350_CONSEC_DOCTO = " & Documento.Item("F350_CONSEC_DOCTO") & " and orden = " & Documento.Item("orden")
                        dvMovimiento.RowFilter = "F350_ID_CO= '" & Documento.Item("F350_ID_CO") & "' and F350_ID_TIPO_DOCTO = '" & Documento.Item("F350_ID_TIPO_DOCTO") & "' and F350_CONSEC_DOCTO = " & Documento.Item("F350_CONSEC_DOCTO") & " and orden = " & Documento.Item("orden")
                        dvDoctoRelacion.RowFilter = "F350_ID_CO= '" & Documento.Item("F350_ID_CO") & "' and F350_ID_TIPO_DOCTO = '" & Documento.Item("F350_ID_TIPO_DOCTO") & "' and F350_CONSEC_DOCTO = " & Documento.Item("F350_CONSEC_DOCTO") & " and orden = " & Documento.Item("orden")
                    Else
                        dvDocumento.RowFilter = NombreCampoIdDocumento & " = '" & Documento.Item(NombreCampoIdDocumento) & "'"
                        dvMovimiento.RowFilter = NombreCampoIdMovimiento & " = '" & Documento.Item(NombreCampoIdDocumento) & "'"
                        dvDoctoRelacion.RowFilter = NombreCampodvDoctoRelacion & " = '" & Documento.Item(NombreCampoIdDocumento) & "'"
                    End If

                    dtDocumento = dvDocumento.ToTable
                    dtMovimiento = dvMovimiento.ToTable
                    dtDoctoRelacion = dvDoctoRelacion.ToTable

                    dsOrigenDatosPorDocumento.Tables.Add(dtDocumento)
                    dsOrigenDatosPorDocumento.Tables.Add(dtMovimiento)
                    dsOrigenDatosPorDocumento.Tables.Add(dtDoctoRelacion)

                    Plano = objGenericTransfer.ImportarDatosDS(objTarea.IdDocumento,
                                            objTarea.NombreDocumento,
                                            2, 1, "gt", "gt",
                                            dsOrigenDatosPorDocumento, objTarea.RutaGeneracionPlano)

                    If Plano <> "Importacion exitosa" Then
                        ProcesoCompleto = False
                        strMensajesSiesa.AppendLine(Plano)

                        If objTarea.Tarea = 128 Then 'Ecommerce
                            MensajeErrorSiesa = descomponerLogErrores(Plano)
                            MensajeGT.AppendLine("No se importo el plano. Tipo de factura: " + dtDocumento.Rows(0).Item("F350_ID_TIPO_DOCTO").ToString + " ,nro de pedido: " + dtDocumento.Rows(0).Item("F350_CONSEC_DOCTO").ToString + " ," + MensajeErrorSiesa.ToString)
                        ElseIf objTarea.Tarea = 163 Then 'Pedidos PV-PVI - Control de cambios(Plan b)
                            MensajeErrorSiesa = descomponerLogErrores(Plano)
                            MensajeGT.AppendLine("No se importo el plano. Tipo de factura: " + dtDocumento.Rows(0).Item("F350_ID_TIPO_DOCTO").ToString + " ,nro de pedido: " + dtDocumento.Rows(0).Item("F350_CONSEC_DOCTO").ToString + " ," + MensajeErrorSiesa.ToString)
                        End If

                        'Else
                        '    Dim objEstado As New clsEstados
                        '    objEstado.cambiarEstadoItemEcommerce(dtDocumento.Rows(0).Item("F350_ID_CO"),
                        '                                         dtDocumento.Rows(0).Item("F350_ID_TIPO_DOCTO"),
                        '                                         dtDocumento.Rows(0).Item("F350_CONSEC_DOCTO"),
                        '                                         dtDocumento.Rows(0).Item("idDespacho"))
                    End If
                Next

            End If

            objTarea.LogFechaFinWebServiceSiesa()
            objTarea.LogFechaFinGeneracionPlano()
            objTarea.LogGeneracionDePlano(1)

            If ProcesoCompleto = True Then
                objTarea.LogWebServiceSiesa(1)
                objTarea.LogEjecucionCompleta()
            Else

                'Almacenar errores Log Técnico
                If objTarea.Tarea = 78 Or objTarea.Tarea = 128 Or objTarea.Tarea = 147 Or objTarea.Tarea = 154 Then 'Ecommerce
                    objTarea.LogWebServiceSiesa(0)
                    objTarea.LogMensajesError(MensajeGT.ToString)
                    almacenarLogErrores(Plano)

                ElseIf objTarea.Tarea = 160 Or objTarea.Tarea = 161 Or objTarea.Tarea = 162 Or objTarea.Tarea = 163 Then 'Pedidos PV-PVI - Control de cambios(Plan b)
                    objTarea.LogWebServiceSiesa(0)
                    objTarea.LogMensajesError(MensajeGT.ToString)
                    almacenarLogErrores(Plano)

                ElseIf objTarea.Tarea = 77 Then 'Transferencia desde RQI
                    objTarea.LogWebServiceSiesa(0)
                    objTarea.LogMensajesError(MensajeGT.ToString)
                    almacenarLogErrores(Plano)

                ElseIf objTarea.Tarea = 166 Or objTarea.Tarea = 168 Then 'Pedidos RQI - Control de cambios(Plan b)
                    objTarea.LogWebServiceSiesa(0)
                    objTarea.LogMensajesError(MensajeGT.ToString)
                    almacenarLogErrores(Plano)

                ElseIf objTarea.Tarea = 41 Or objTarea.Tarea = 45 Or objTarea.Tarea = 49 Or objTarea.Tarea = 50 Or objTarea.Tarea = 55 Or objTarea.Tarea = 83 Or objTarea.Tarea = 144 Or objTarea.Tarea = 145 Then 'ASN
                    objTarea.LogWebServiceSiesa(0)
                    objTarea.LogMensajesError(MensajeGT.ToString)
                    almacenarLogErrores(Plano)

                ElseIf objTarea.Tarea = 84 Then 'Salida en transito
                    objTarea.LogWebServiceSiesa(0)
                    objTarea.LogMensajesError(MensajeGT.ToString)
                    almacenarLogErrores(Plano)

                ElseIf objTarea.Tarea = 131 Or objTarea.Tarea = 146 Then 'GP
                    objTarea.LogWebServiceSiesa(0)
                    objTarea.LogMensajesError(MensajeGT.ToString)
                    almacenarLogErrores(Plano)

                ElseIf objTarea.Tarea = 143 Then 'Logistica Inversa
                    objTarea.LogWebServiceSiesa(0)
                    objTarea.LogMensajesError(MensajeGT.ToString)
                    almacenarLogErrores(Plano)

                Else
                    objTarea.LogWebServiceSiesa(0)
                    almacenarLogErrores(Plano)
                    MensajeErrorSiesa = descomponerLogErrores(Plano)
                    objTarea.LogMensajesError(MensajeGT.ToString + " , " + MensajeErrorSiesa)
                End If
            End If

            If objTarea.Tarea = 78 Then
                objTarea.LogFechaFin()
            Else
                objTarea.LogFin(objTarea.idLogPrincipal)
                objTarea.LogFechaFin()
            End If

        Catch ex As Exception
            objTarea.LogFechaFin()
            objTarea.LogFin(objTarea.idLogPrincipal)
            objTarea.LogFechaFinGeneracionPlano()
            objTarea.LogGeneracionDePlano(0)
            objTarea.LogMensajesError(ex.InnerException.Message)
            'objCorreo.EnviarCorreoTarea("Exception - Consumo WS GTIntegration", objTarea.Destinatarios, ex.InnerException.Message, objTarea.Tarea)
            Exit Sub
        End Try

    End Sub

    Public Sub SincronizacionTransferenciasAjustes(ByVal NombreCampoIdDocumento As String, ByVal NombreCampoIdMovimiento As String, Optional ByVal NombreCampodvDoctoRelacion As String = "")
        Dim dsOrigen As DataSet
        objTarea.BaseDatos = "SQL"
        Dim Plano As String
        Dim Resultado As String = ""
        Dim idTareaValido As Boolean = True
        Dim MensajeError As String
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


        'Invocar Web Service de GT para Obtener el plano o realizar la importacion
        Try
            Dim objGenericTransfer As New wsGT.wsGenerarPlano
            Dim ProcesoCompleto As Boolean = True
            Dim strMensajesSiesa As New StringBuilder
            Dim NumeroSecciones As Integer
            Dim dsOrigenDatosPorDocumento As New DataSet
            Dim dtDocumento As DataTable
            Dim dvDocumento As DataView
            Dim dtMovimiento As DataTable
            Dim dvMovimiento As DataView
            Dim dvDoctoRelacion As DataView
            Dim dtDoctoRelacion As DataTable
            Dim Consec_docto As String = ""
            Dim ActualizarEstado As Boolean
            Dim MensajeGT As New StringBuilder
            Dim TrasladoExitosoGT As New StringBuilder

            objGenericTransfer.Timeout = 18000000

            objTarea.LogFechaInicioWebServiceSiesa()
            objTarea.LogFechaInicioGeneracionPlano()

            NumeroSecciones = dsOrigen.Tables.Count

            If NumeroSecciones = 2 Then

                dvDocumento = New DataView(dsOrigen.Tables(0))
                dvMovimiento = New DataView(dsOrigen.Tables(1))

                For Each Documento As DataRow In dsOrigen.Tables(0).Rows
                    dsOrigenDatosPorDocumento.Tables.Clear()
                    dvDocumento.RowFilter = NombreCampoIdDocumento & " = '" & Documento.Item(NombreCampoIdDocumento) & "'"
                    dvMovimiento.RowFilter = NombreCampoIdMovimiento & " = '" & Documento.Item(NombreCampoIdDocumento) & "'"

                    dtDocumento = dvDocumento.ToTable
                    dtMovimiento = dvMovimiento.ToTable
                    dsOrigenDatosPorDocumento.Tables.Add(dtDocumento)
                    dsOrigenDatosPorDocumento.Tables.Add(dtMovimiento)

                    If Documento.Item("f350_id_tipo_docto") = "AJ" And dtMovimiento.Rows(0).Item("f470_cant_base") > 0 Then
                        If Documento.Item("ValidacionBodega").ToString() = "Si" Then
                            If dtMovimiento.Rows(0).Item("adj_qty") < 0 Then


                                Dim cantidaDisponible As Int16 = ConsultarExistencia(dtMovimiento.Rows(0).Item("f470_id_bodega"), dtMovimiento.Rows(0).Item("f470_referencia_item"), dtMovimiento.Rows(0).Item("f470_id_ext1_detalle"), dtMovimiento.Rows(0).Item("f470_id_ext2_detalle"))
                                Dim cantidadRequerida = Math.Abs(CInt(dtMovimiento.Rows(0).Item("adj_qty")))

                                If cantidaDisponible < cantidadRequerida Then

                                    If cantidaDisponible > 0 Then
                                        dtMovimiento.Rows(0).Item("f470_cant_base") = cantidaDisponible
                                        CargarSiesa(dsOrigenDatosPorDocumento, Plano, ProcesoCompleto, strMensajesSiesa)
                                    End If

                                    cantidadRequerida = cantidadRequerida - cantidaDisponible
                                    cantidaDisponible = ConsultarExistencia(Documento.Item("Bodega2").ToString(), dtMovimiento.Rows(0).Item("f470_referencia_item"), dtMovimiento.Rows(0).Item("f470_id_ext1_detalle"), dtMovimiento.Rows(0).Item("f470_id_ext2_detalle"))

                                    If cantidaDisponible <= cantidadRequerida Then

                                        If cantidaDisponible > 0 Then
                                            dtMovimiento.Rows(0).Item("f470_cant_base") = cantidaDisponible
                                            dtMovimiento.Rows(0).Item("f470_id_bodega") = Documento.Item("Bodega2").ToString()
                                            CargarSiesa(dsOrigenDatosPorDocumento, Plano, ProcesoCompleto, strMensajesSiesa)
                                        End If

                                        cantidadRequerida = cantidadRequerida - cantidaDisponible

                                        cantidaDisponible = ConsultarExistencia(Documento.Item("Bodega3").ToString(), dtMovimiento.Rows(0).Item("f470_referencia_item"), dtMovimiento.Rows(0).Item("f470_id_ext1_detalle"), dtMovimiento.Rows(0).Item("f470_id_ext2_detalle"))

                                        If cantidaDisponible <= cantidadRequerida Then
                                            If cantidaDisponible > 0 Then
                                                dtMovimiento.Rows(0).Item("f470_cant_base") = cantidaDisponible
                                                dtMovimiento.Rows(0).Item("f470_id_bodega") = Documento.Item("Bodega3").ToString()
                                                CargarSiesa(dsOrigenDatosPorDocumento, Plano, ProcesoCompleto, strMensajesSiesa)
                                            End If

                                            cantidadRequerida = cantidadRequerida - cantidaDisponible

                                        Else
                                            dtMovimiento.Rows(0).Item("f470_cant_base") = cantidadRequerida
                                            dtMovimiento.Rows(0).Item("f470_id_bodega") = Documento.Item("Bodega3").ToString()
                                            CargarSiesa(dsOrigenDatosPorDocumento, Plano, ProcesoCompleto, strMensajesSiesa)
                                        End If
                                    Else
                                        dtMovimiento.Rows(0).Item("f470_cant_base") = cantidadRequerida
                                        dtMovimiento.Rows(0).Item("f470_id_bodega") = Documento.Item("Bodega2").ToString()
                                        CargarSiesa(dsOrigenDatosPorDocumento, Plano, ProcesoCompleto, strMensajesSiesa)
                                    End If
                                Else
                                    CargarSiesa(dsOrigenDatosPorDocumento, Plano, ProcesoCompleto, strMensajesSiesa)
                                End If
                            Else
                                CargarSiesa(dsOrigenDatosPorDocumento, Plano, ProcesoCompleto, strMensajesSiesa)
                            End If
                        Else
                            CargarSiesa(dsOrigenDatosPorDocumento, Plano, ProcesoCompleto, strMensajesSiesa)
                        End If
                    Else
                        If objValidacionDocumento.ConsultarTrasladoSiesa(Documento.Item("f350_notas")) = False Then
                            Plano = objGenericTransfer.ImportarDatosDS(objTarea.IdDocumento,
                                                                   objTarea.NombreDocumento,
                                                                   2, 1, "gt", "gt",
                                                                   dsOrigenDatosPorDocumento, objTarea.RutaGeneracionPlano)
                        Else
                            Plano = "planoPreviamenteImportado"
                        End If
                    End If

                    If Plano <> "planoPreviamenteImportado" Then
                        If Plano <> "Importacion exitosa" Then
                            ProcesoCompleto = False
                            strMensajesSiesa.AppendLine(Plano)

                            If Documento.Item("f350_id_tipo_docto") = "TR" And Documento.Item("f350_id_clase_docto") = 67 Then 'TRASLADO ENTRE BODEGAS 
                                For Each Movimiento As DataRow In dsOrigenDatosPorDocumento.Tables(1).Rows
                                    MensajeErrorSiesa = descomponerLogErrores(Plano)
                                    MensajeGT.AppendLine("Fallo la transferencia - Tipo de documento: " & dtDocumento.Rows(0).Item("ref_value_6").ToString &
                                                         ", Consecutivo: " & dtDocumento.Rows(0).Item("shipment_nbr").ToString & ", Bodega de salida: " &
                                                         Documento.Item("f450_id_bodega_salida").ToString & ", Bodega de entrada: " &
                                                         Documento.Item("f450_id_bodega_entrada").ToString & ", con cantidad: " & Movimiento.Item("f470_cant_base").ToString &
                                                         " para la referencia: " & Movimiento.Item("f470_referencia_item").ToString & " con color: " & Movimiento.Item("f470_id_ext1_detalle").ToString &
                                                         " y talla: " & Movimiento.Item("f470_id_ext2_detalle") & ", " & MensajeErrorSiesa.ToString)
                                Next
                            ElseIf Documento.Item("f350_id_tipo_docto") = "AJ" And Documento.Item("f350_id_clase_docto") = 63 Then 'AJUSTE UNIDADES/COSTO
                                If dtMovimiento.Rows(0).Item("f470_cant_base") > 0 Then
                                    For Each Movimiento As DataRow In dsOrigenDatosPorDocumento.Tables(1).Rows
                                        MensajeErrorSiesa = descomponerLogErrores(Plano)
                                        MensajeGT.AppendLine("No se importo el plano - Tipo de documento: " & dtDocumento.Rows(0).Item("f350_id_tipo_docto").ToString &
                                                             ", Consecutivo: " & dtDocumento.Rows(0).Item("f350_consec_docto").ToString & ", Bodega: " &
                                                             Movimiento.Item("f470_id_bodega").ToString & ", cantidad: " & Movimiento.Item("f470_cant_base").ToString &
                                                             " para la referencia: " & Movimiento.Item("f470_referencia_item").ToString & " con color: " & Movimiento.Item("f470_id_ext1_detalle").ToString &
                                                             " y talla: " & Movimiento.Item("f470_id_ext2_detalle") & ", " & MensajeErrorSiesa.ToString)
                                    Next
                                Else
                                    For Each Movimiento As DataRow In dsOrigenDatosPorDocumento.Tables(1).Rows
                                        MensajeErrorSiesa = descomponerLogErrores(Plano)
                                        MensajeGT.AppendLine("No se importo el plano - Tipo de documento: " & dtDocumento.Rows(0).Item("f350_id_tipo_docto").ToString &
                                                             ", Consecutivo: " & dtDocumento.Rows(0).Item("f350_consec_docto").ToString & ", Bodega: " &
                                                             Movimiento.Item("f470_id_bodega").ToString & ", costo: " & Movimiento.Item("f470_costo_prom_uni").ToString &
                                                             " para la referencia: " & Movimiento.Item("f470_referencia_item").ToString & " con color: " & Movimiento.Item("f470_id_ext1_detalle").ToString &
                                                             " y talla: " & Movimiento.Item("f470_id_ext2_detalle") & ", " & MensajeErrorSiesa.ToString)
                                    Next
                                End If
                            End If
                        Else
                            'objTarea.LogFin(objTarea.idLogPrincipal)
                            objValidacionDocumento.actualizarEstadoTrasladoAjuste(Documento.Item("f350_notas"), Documento.Item("f350_id_tipo_docto"), Documento.Item("f350_consec_docto"))

                            If dtDocumento.Rows(0).Item("activity_code") = 25 And dtDocumento.Rows(0).Item("ref_value_6").ToString = "TAL" Then 'TRANSLADO EXITOSO ENTRE BODEGAS
                                For Each Movimiento As DataRow In dsOrigenDatosPorDocumento.Tables(1).Rows
                                    TrasladoExitosoGT.AppendLine("Transferencia Exitosa - Tipo de documento: " & dtDocumento.Rows(0).Item("ref_value_6").ToString &
                                                         ", Consecutivo: " & dtDocumento.Rows(0).Item("shipment_nbr").ToString & ", Bodega de salida: " &
                                                         Documento.Item("f450_id_bodega_salida").ToString & ", Bodega de entrada: " &
                                                         Documento.Item("f450_id_bodega_entrada").ToString & ", con cantidad: " & Movimiento.Item("f470_cant_base").ToString &
                                                         " para la referencia: " & Movimiento.Item("f470_referencia_item").ToString & " con color: " & Movimiento.Item("f470_id_ext1_detalle").ToString &
                                                         " y talla: " & Movimiento.Item("f470_id_ext2_detalle"))
                                Next
                            End If
                        End If
                    End If
                Next
                If TrasladoExitosoGT.ToString <> "" Then
                    objCorreo.EnviarCorreoTarea("GTIntegration-WMS: Traslado exitoso entre bodegas", objTarea.Destinatarios, TrasladoExitosoGT.ToString, objTarea.Tarea)
                End If
            End If

            objTarea.LogFechaFinWebServiceSiesa()
            objTarea.LogFechaFinGeneracionPlano()
            objTarea.LogGeneracionDePlano(1)

            If ProcesoCompleto = True Then
                objTarea.LogWebServiceSiesa(1)
                objTarea.LogEjecucionCompleta()
            Else
                objTarea.LogWebServiceSiesa(0)
                almacenarLogErrores(Plano)
                objTarea.LogMensajesError(MensajeGT.ToString)
            End If

            'objTarea.LogFin(objTarea.idLogPrincipal)
            objTarea.LogFechaFin()

        Catch ex As Exception
            objTarea.LogFechaFin()
            objTarea.LogFin(objTarea.idLogPrincipal)
            objTarea.LogFechaFinGeneracionPlano()
            objTarea.LogGeneracionDePlano(0)
            objTarea.LogMensajesError(ex.Message)
            Exit Sub
        End Try

    End Sub

    Public Sub SincronizacionTransferenciasAjustesMasivos(ByVal NombreCampoIdDocumento As String, ByVal NombreCampoIdMovimiento As String, Optional ByVal NombreCampodvDoctoRelacion As String = "")

        Dim dsOrigen As DataSet
        objTarea.BaseDatos = "SQL"
        Dim Plano As String
        Dim Resultado As String = ""
        Dim idTareaValido As Boolean = True
        Dim objValidacionDocumento As New clsTraslados
        Dim mensajeGT As New StringBuilder

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

        Dim ConsTRW As Integer = 0 'Consecutivos manual traslados
        Dim ConsAJW As Integer = 0 'Consecutivos manual ajustes

        'Asignación de consecutivos como quedaron asignados en el ERP
        objValidacionDocumento.controlEstadosTrasladosAjustesTRW_AJW(ConsTRW, ConsAJW)

        'Marcado de estados
        objValidacionDocumento.controlEstadosTrasladosAjustes()

        'Actualizacion de consecutivos en GTI
        objValidacionDocumento.actualizarConsecutivoManual(ConsTRW, "TRW")
        objValidacionDocumento.actualizarConsecutivoManual(ConsAJW, "AJW")

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

        'Validar existencias en Bodegas segun la prioridad que corresponda por la entidad dinamica
        Dim dsOrigenPriorizado As DataSet
        Dim objPrioridadBodegas As New clsPrioridadBodegas
        dsOrigenPriorizado = objPrioridadBodegas.PriorizarBodegas(dsOrigen)

        'Invocar Web Service de GT para Obtener el plano o realizar la importacion
        Try
            Dim objGenericTransfer As New wsGT.wsGenerarPlano
            Dim strMensajesSiesa As New StringBuilder
            Dim dsOrigenDatosPorDocumento As New DataSet
            Dim TrasladoExitosoGT As New StringBuilder

            objGenericTransfer.Timeout = 18000000

            objTarea.LogFechaInicioWebServiceSiesa()
            objTarea.LogFechaInicioGeneracionPlano()

            'Paginación de registros
            Dim dvDocumento As New DataView(dsOrigenPriorizado.Tables(0))

            Dim numeroDeRegistrosPorPagina As Integer = 500
            Dim totalRegistros As Integer = dsOrigenPriorizado.Tables(0).Rows.Count
            Dim numeroDePaginas As Integer = Math.Ceiling(dsOrigenPriorizado.Tables(0).Rows.Count / numeroDeRegistrosPorPagina)

            Dim numInicial As Integer = 1
            Dim numFinal As Integer = 500

            For index As Integer = 1 To numeroDePaginas
                Dim dsResultadoPaginacion As DataSet = objPrioridadBodegas.CrearDataSet()

                For Each Movimiento As DataRow In dsOrigenPriorizado.Tables(1).Rows
                    dvDocumento.RowFilter = "f350_id_co= '" & Movimiento.Item("f470_id_co") & "' and f350_id_tipo_docto = '" & Movimiento.Item("f470_id_tipo_docto") & "' and f350_consec_docto = " & Movimiento.Item("f470_consec_docto")

                    If dvDocumento.ToTable.Rows(0).Item("numRow") >= numInicial And dvDocumento.ToTable.Rows(0).Item("numRow") <= numFinal Then
                        dsResultadoPaginacion.Tables(0).Rows.Add(dvDocumento.ToTable.Rows(0).Item("f350_id_co"), dvDocumento.ToTable.Rows(0).Item("f350_id_tipo_docto"), dvDocumento.ToTable.Rows(0).Item("f350_consec_docto"), dvDocumento.ToTable.Rows(0).Item("f350_fecha"),
                                                           dvDocumento.ToTable.Rows(0).Item("f350_id_clase_docto"), dvDocumento.ToTable.Rows(0).Item("f350_notas"), dvDocumento.ToTable.Rows(0).Item("f450_id_concepto"), dvDocumento.ToTable.Rows(0).Item("f450_id_bodega_salida"),
                                                           dvDocumento.ToTable.Rows(0).Item("f450_id_bodega_entrada"), dvDocumento.ToTable.Rows(0).Item("f450_docto_alterno"), dvDocumento.ToTable.Rows(0).Item("ValidacionBodega"), dvDocumento.ToTable.Rows(0).Item("ref_value_6"),
                                                           dvDocumento.ToTable.Rows(0).Item("shipment_nbr"), dvDocumento.ToTable.Rows(0).Item("activity_code"), dvDocumento.ToTable.Rows(0).Item("Bodega2"), dvDocumento.ToTable.Rows(0).Item("Bodega3"),
                                                           dvDocumento.ToTable.Rows(0).Item("orden"), dvDocumento.ToTable.Rows(0).Item("f470_id_motivo"), dvDocumento.ToTable.Rows(0).Item("numRow"))

                        objPrioridadBodegas.agregarRegistro(dsResultadoPaginacion, dvDocumento, Movimiento)
                    End If
                Next

                'Generación e importación del plano
                Plano = objGenericTransfer.ImportarDatosDS(objTarea.IdDocumento,
                            objTarea.NombreDocumento,
                            2, 1, "gt", "gt",
                            dsResultadoPaginacion, objTarea.RutaGeneracionPlano)

                'Asignación de consecutivos como quedaron asignados en el ERP
                objValidacionDocumento.controlEstadosTrasladosAjustesTRW_AJW(ConsTRW, ConsAJW)

                'Marcado de estados
                objValidacionDocumento.controlEstadosTrasladosAjustes()

                'Actualizacion de consecutivos en GTI
                objValidacionDocumento.actualizarConsecutivoManual(ConsTRW, "TRW")
                objValidacionDocumento.actualizarConsecutivoManual(ConsAJW, "AJW")

                If Plano <> "Importacion exitosa" Then
                    'Descomponer error
                    mensajeGT.AppendLine(descomponerLogErrores(Plano))
                End If

                numInicial = numInicial + numeroDeRegistrosPorPagina
                numFinal = numFinal + numeroDeRegistrosPorPagina
            Next

            objTarea.LogFechaFinWebServiceSiesa()
            objTarea.LogFechaFinGeneracionPlano()

            If objPrioridadBodegas.mensajeErrorBodegas <> "" Then
                objTarea.LogMensajesError(objPrioridadBodegas.mensajeErrorBodegas)
            End If

            If mensajeGT.ToString <> "" Or objPrioridadBodegas.mensajeErrorBodegas <> "" Then
                objTarea.LogGeneracionDePlano(0)
                objTarea.LogWebServiceSiesa(0)

                'almacenar error
                objTarea.LogMensajesError(objPrioridadBodegas.mensajeErrorBodegas & mensajeGT.ToString)
                objCorreo.EnviarCorreoTarea("GTIntegration-WMS: Error de transferencia/Ajuste - Tarea 181", objTarea.Destinatarios, mensajeGT.ToString, objTarea.Tarea)
            Else
                objTarea.LogGeneracionDePlano(1)
                objTarea.LogWebServiceSiesa(1)
                objTarea.LogEjecucionCompleta()
            End If

            objTarea.LogFechaFin()
        Catch ex As Exception
            objTarea.LogFechaFin()
            objTarea.LogFechaFinGeneracionPlano()
            objTarea.LogGeneracionDePlano(0)
            objTarea.LogWebServiceSiesa(0)
            objTarea.LogMensajesError(ex.Message)
            Exit Sub
        End Try

    End Sub

    Public Sub SincronizacionTransferenciasDesdeRqi(ByVal NombreCampoIdDocumento As String, ByVal NombreCampoIdMovimiento As String, Optional ByVal NombreCampodvDoctoRelacion As String = "")

        Dim dsOrigen As DataSet
        Dim dsOrigenReporte As DataSet
        objTarea.BaseDatos = "SQL"
        Dim Plano As String = ""
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
                Exit Sub
            End If
        Else
            objTarea.LogRecuperacionDatosOrigen(0)
            objTarea.LogMensajesError("No se envió el Id de la tarea como argumento al programa, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea")
            Exit Sub
        End If

        Dim ConsTTW As Integer = 0  'Consecutivo manual

        'Asignación de consecutivo del ERP
        objValidacionDocumento.controlEstadosTrasladosAjustesTTW(ConsTTW)

        'Actualizacion de consecutivo en GTI
        objValidacionDocumento.actualizarConsecutivoManual(ConsTTW, "TTW")

        'Marcado de estados
        objTarea.LogFin(objTarea.idLogPrincipal)

        'Obtener los datos fuentes del cliente para la tarea X
        Try
            objTarea.LogFechaInicioRecuperacionDatosOrigen()
            dsOrigen = objTarea.DatosOrigen(False)

            'Obtener datos reporte
            dsOrigenReporte = objValidacionDocumento.consultarDatosReporteRQI()

            If idTareaValido Then
                objTarea.LogFechaFinRecuperacionDatosOrigen()
                objTarea.LogRecuperacionDatosOrigen(1)
            Else
                objTarea.LogRecuperacionDatosOrigen(0)
                objTarea.LogMensajesError("No se encontró el Id de tarea: " & objTarea.Tarea & " en la base de datos, verifique la configuración de la tarea programada de Windows")
                Exit Sub
            End If
        Catch ex As Exception
            objTarea.LogFechaFin()
            objTarea.LogFin(objTarea.idLogPrincipal)
            objTarea.LogRecuperacionDatosOrigen(0)
            objTarea.LogMensajesError(ex.Message)
            Exit Sub
        End Try

        'Invocar Web Service de GT para Obtener el plano o realizar la importacion
        Try
            Dim objGenericTransfer As New wsGT.wsGenerarPlano
            Dim strMensajesSiesa As New StringBuilder
            Dim TrasladoExitosoGT As New StringBuilder

            objGenericTransfer.Timeout = 18000000

            objTarea.LogFechaInicioWebServiceSiesa()
            objTarea.LogFechaInicioGeneracionPlano()

            'Generación e importación del plano
            Plano = objGenericTransfer.ImportarDatosDS(objTarea.IdDocumento,
                            objTarea.NombreDocumento,
                            2, 1, "gt", "gt",
                            dsOrigen, objTarea.RutaGeneracionPlano)

            'Asignación de consecutivo del ERP
            objValidacionDocumento.controlEstadosTrasladosAjustesTTW(ConsTTW)

            'Actualizacion de consecutivo en GTI
            objValidacionDocumento.actualizarConsecutivoManual(ConsTTW, "TTW")

            'Marcado de estados
            objTarea.LogFin(objTarea.idLogPrincipal)

            objTarea.LogFechaFinWebServiceSiesa()
            objTarea.LogFechaFinGeneracionPlano()

            If Plano <> "Importacion exitosa" Then
                objTarea.LogGeneracionDePlano(0)
                objTarea.LogWebServiceSiesa(0)

                'Almacena los errores de Siesa en un DataSet
                Dim erroresDataSet As DataSet = generarDataSetErrores(Plano)

                'Enviar reporte excel
                generarReporteExcel(dsOrigen, dsOrigenReporte, erroresDataSet)

                'Descomponer y almacenar error
                Dim msgError = ""

                MensajeErrorSiesa = descomponerLogErrores(Plano)
                If MensajeErrorSiesa <> "" Then
                    objTarea.LogMensajesError(MensajeErrorSiesa)
                    msgError = MensajeErrorSiesa.ToString
                Else
                    objTarea.LogMensajesError(Plano)
                    msgError = Plano.ToString
                End If
            Else
                Dim exitosoDataSet As DataSet = generarDataSetErrores(Plano)

                'Enviar reporte excel
                generarReporteExcel(dsOrigen, dsOrigenReporte, exitosoDataSet)

                objTarea.LogGeneracionDePlano(1)
                objTarea.LogWebServiceSiesa(1)
                objTarea.LogEjecucionCompleta()
            End If

            objTarea.LogFechaFin()
        Catch ex As Exception
            objTarea.LogFechaFin()
            objTarea.LogFechaFinGeneracionPlano()
            objTarea.LogGeneracionDePlano(0)
            objTarea.LogWebServiceSiesa(0)
            objTarea.LogMensajesError(ex.Message)
            Exit Sub
        End Try



    End Sub
    Private Sub almacenarLogErrores(ByVal ErroresSiesa As String)
        Dim dsErroresSiesa As DataSet = New DataSet
        Dim ErrorSiesa As New StringBuilder

        If ErroresSiesa.Contains("<NewDataSet>") Then
            ErroresSiesa = ErroresSiesa.Replace("Error al importar el plano", "")
            ErroresSiesa = ErroresSiesa.Substring(0, ErroresSiesa.LastIndexOf(">") + 1)
            Dim xmlSR As System.IO.StringReader = New System.IO.StringReader(ErroresSiesa)
            dsErroresSiesa.ReadXml(xmlSR, XmlReadMode.Auto)

            For Each FilaError As System.Data.DataRow In dsErroresSiesa.Tables(0).Rows
                ErrorSiesa.Append("Linea:" & FilaError.Item(0).ToString & " Tipo de Registro:" & FilaError.Item(1).ToString & " SubTipo de resgistro:" & FilaError.Item(2).ToString & " Version:" & FilaError.Item(3).ToString & " Nivel" & FilaError.Item(4).ToString & " Error" & FilaError.Item(5).ToString & " Detalle:" & FilaError.Item(6).ToString & vbCrLf)
                ErrorSiesa.Append("<br />")
                objTarea.LogDetalleAlmacenar(FilaError.Item("f_nro_linea").ToString, FilaError.Item("f_tipo_reg").ToString, FilaError.Item("f_subtipo_reg").ToString, FilaError.Item("f_version").ToString, FilaError.Item("f_nivel").ToString, FilaError.Item("f_valor").ToString, FilaError.Item("f_detalle").ToString)
            Next
        End If
    End Sub

    Private Function descomponerLogErrores(ByVal ErroresSiesa As String) As String
        Dim dsErroresSiesa As DataSet = New DataSet
        Dim ErrorSiesaDescompuesto As New StringBuilder



        If ErroresSiesa.Contains("<NewDataSet>") Then
            ErrorSiesaDescompuesto.Append("Mensaje ERP Siesa Enterprise: ")
            ErroresSiesa = ErroresSiesa.Substring(ErroresSiesa.ToString.IndexOf("<NewDataSet>"), 13 + ErroresSiesa.ToString.IndexOf("</NewDataSet>") - ErroresSiesa.ToString.IndexOf("<NewDataSet>"))
            Dim xmlSR As System.IO.StringReader = New System.IO.StringReader(ErroresSiesa)
            dsErroresSiesa.ReadXml(xmlSR, XmlReadMode.Auto)

            For Each FilaError As System.Data.DataRow In dsErroresSiesa.Tables(0).Rows
                ErrorSiesaDescompuesto.AppendLine("Linea:" & FilaError.Item(0).ToString & " Tipo de Registro:" & FilaError.Item(1).ToString & " SubTipo de resgistro:" & FilaError.Item(2).ToString & " Version:" & FilaError.Item(3).ToString & " Nivel" & FilaError.Item(4).ToString & " Error" & FilaError.Item(5).ToString & " Detalle:" & FilaError.Item(6).ToString & vbCrLf)
            Next
        End If

        Return ErrorSiesaDescompuesto.ToString

    End Function

    Private Function generarDataSetErrores(ByVal ErroresSiesa As String) As DataSet
        Dim dsErroresSiesa As DataSet = New DataSet

        If ErroresSiesa <> "Importacion exitosa" Then
            If ErroresSiesa.Contains("<NewDataSet>") Then
                ErroresSiesa = ErroresSiesa.Substring(ErroresSiesa.ToString.IndexOf("<NewDataSet>"), 13 + ErroresSiesa.ToString.IndexOf("</NewDataSet>") - ErroresSiesa.ToString.IndexOf("<NewDataSet>"))
                Dim xmlSR As System.IO.StringReader = New System.IO.StringReader(ErroresSiesa)
                dsErroresSiesa.ReadXml(xmlSR, XmlReadMode.Auto)

            End If
        Else
            Dim dt As New DataTable("error")

            dt.Columns.Add("f_nro_linea", GetType(String))
            dt.Columns.Add("f_tipo_reg", GetType(String))
            dt.Columns.Add("f_subtipo_reg", GetType(String))
            dt.Columns.Add("f_version", GetType(String))
            dt.Columns.Add("f_nivel", GetType(String))
            dt.Columns.Add("f_valor", GetType(String))
            dt.Columns.Add("f_detalle", GetType(String))

            dsErroresSiesa.Tables.Add(dt)
        End If

        Return dsErroresSiesa

    End Function

    Private Sub generarReporteExcel(ByVal dsOrigen As DataSet, ByVal dsOrigenReporte As DataSet, ByVal erroresDataSet As DataSet)
        'Generación del reporte 
        Dim objReporteDataSet As New clsReporte

        Dim dsDatos As New DataTable
        dsDatos = objReporteDataSet.almacenarDatosDataSet(dsOrigen, dsOrigenReporte, erroresDataSet)

        'Excel
        Dim objReporte As New ClsExcel
        objReporte.NombreArchivo = "Reporte_GT" & Now.ToString("yyyyMMddhhmmss") & ".xlsx"
        objReporte.NombreHoja = "Reporte transferencias"
        objReporte.Ruta = "C:\inetpub\wwwroot\GTIntegrationproduccionwms\Reporte"
        objReporte.Datos = dsDatos
        objReporte.GenerarReporte()
        objCorreo.EnviarCorreoTareaConAdjunto("Integración transferencias con granularidad desde RQI", objTarea.Destinatarios, objReporte.Ruta & "\" & objReporte.NombreArchivo, objTarea.Tarea)
    End Sub

End Class
