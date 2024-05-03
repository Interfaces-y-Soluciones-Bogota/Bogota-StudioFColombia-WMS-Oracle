Imports System.Data.SqlClient

Public Class clsGPMaestros
    Dim objTarea As New clsTarea

    Public Sub cargarMaestro()

        Dim objCorreo As New clsCorreo
        Dim dsConsulta As New DataSet
        Dim dsMaestro As DataSet
        Dim swDatos As Boolean
        Dim objUnoEE As New wsUnoEE.WSUNOEE
        Dim Resultado As String = ""

        swDatos = False

        Try
            Dim objConfiguracion As New clsConfiguracion
            If LogInicial(Environment.GetCommandLineArgs()) Then


                'Preparar los datos
                Select Case objTarea.Tarea
                    'INSERTAR_PRODUCTOS
                    Case 121
                        ejecutarProcedimiento("sp_WMS_GP_ITEM_ALMACENAR_ORIGEN")
                        ejecutarProcedimiento("sp_WMS_GP_MergeSync_ITEM")
                    'ACTUALIZAR_PRODUCTOS
                    Case 122
                        ejecutarProcedimiento("sp_WMS_GP_ITEM_ALMACENAR_ORIGEN")
                        ejecutarProcedimiento("sp_WMS_GP_MergeSync_ITEM")
                    'INSERTAR_CLIENTES
                    Case 123
                        ejecutarProcedimiento("sp_WMS_GP_STORE_ALMACENAR_ORIGEN")
                        ejecutarProcedimiento("sp_WMS_GP_MergeSync_STORE")
                    'ACTUALIZAR_CLIENTES
                    Case 124
                        ejecutarProcedimiento("sp_WMS_GP_STORE_ALMACENAR_ORIGEN")
                        ejecutarProcedimiento("sp_WMS_GP_MergeSync_STORE")
                    'ACTUALIZAR_INVENTARIO
                    Case 125
                        ejecutarProcedimiento("sp_WMS_GP_INVENTARIO_ALMACENAR")
                End Select

                'Consultar datos via Web Service de Siesa
                If ConsutarXML(dsConsulta) Then
                    Dim objPlano As New clsPlano(objTarea.RutaGeneracionPlano)
                    objTarea.LogFechaInicioGeneracionPlano()
                    objPlano.Path = objTarea.RutaGeneracionPlano


                    For Each Pais As DataRow In dsConsulta.Tables(1).Rows
                        Try
                            dsMaestro = objUnoEE.EjecutarConsultaXML(Pais.Item("Query"))
                            If dsMaestro.Tables(0).Rows.Count > 0 Then
                                swDatos = True
                            End If
                            objTarea.LogGeneracionDePlano(1)
                            objTarea.LogFechaFinGeneracionPlano()
                        Catch ex As Exception
                            objTarea.LogGeneracionDePlano(0)
                            objTarea.LogMensajesError(ex.Message)
                            'objCorreo.EnviarCorreoTarea("GTIntegration-WMS: GP-Consulta de datos", objTarea.Destinatarios, ex.InnerException.Message, objTarea.Tarea)
                        End Try
                    Next

                    'Consumir los WS de Mobilistic
                    Try
                        objTarea.LogFechaInicioWebServiceSiesa()
                        If swDatos Then


                            Select Case objTarea.Tarea
                               'INSERTAR_PRODUCTOS
                                Case 121
                                    Dim objGPProducto As New clsGPProductos
                                    Resultado = objGPProducto.almacenarProducto(dsMaestro)
                                'ACTUALIZAR_PRODUCTOS
                                Case 122
                                    Dim objGPProducto As New clsGPProductos
                                    Resultado = objGPProducto.actualizarProducto(dsMaestro)
                                'INSERTAR_CLIENTES
                                Case 123
                                    Dim objGPCliente As New clsGPClientes
                                    Resultado = objGPCliente.almacenarCliente(dsMaestro)
                                'ACTUALIZAR_CLIENTES
                                Case 124
                                    Dim objGPCliente As New clsGPClientes
                                    Resultado = objGPCliente.actualizarCliente(dsMaestro)
                                'ACTUALIZAR_INVENTARIO
                                Case 125
                                    Dim objGPInventario As New clsGPInventario
                                    Resultado = objGPInventario.actualizarInventario(dsMaestro)
                                'ACTUALIZAR_PEDIDO
                                Case 129
                                    Dim objGPedidos As New clsGPPedidos
                                    Resultado = objGPedidos.actualizarPedido(dsMaestro)
                                'ACTUALIZAR_PEDIDO CANCELADO
                                Case 151
                                    Dim objGPedidos As New clsGPPedidos
                                    Resultado = objGPedidos.actualizarPedidoCancelado(dsMaestro)
                            End Select
                        End If

                        If Resultado = "" Then
                            objTarea.LogWebServiceSiesa(1)
                            objTarea.LogEjecucionCompleta()
                        Else
                            objTarea.LogWebServiceSiesa(0)
                            objTarea.LogMensajesError(Resultado)
                            'objCorreo.EnviarCorreoTarea("GTIntegration-WMS: GP-Consumo de WS de Mobilistic", objTarea.Destinatarios, Resultado, objTarea.Tarea)
                        End If


                    Catch ex As Exception
                        objTarea.LogWebServiceSiesa(0)
                        objTarea.LogMensajesError(ex.Message)
                        'objCorreo.EnviarCorreoTarea("GTIntegration-WMS: GP-Consumo de WS de Mobilistic", objTarea.Destinatarios, ex.InnerException.Message, objTarea.Tarea)
                    End Try

                    objTarea.LogFechaFin()
                End If
            End If
        Catch ex As Exception
            objTarea.LogMensajesError(ex.Message)
        Finally
            objTarea.LogFin(objTarea.idLogPrincipal)
        End Try

    End Sub

    Private Function LogInicial(ByVal args As String()) As Boolean
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
                'objCorreo.EnviarCorreoTarea("GTIntegration-WMS: GP-Validaciones", objTarea.Destinatarios, "El argumento enviado debe ser numérico, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea", objTarea.Tarea)
                Return False
            End If
        Else
            objTarea.LogRecuperacionDatosOrigen(0)
            objTarea.LogMensajesError("No se envió el Id de la tarea como argumento al programa, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea")
            'objCorreo.EnviarCorreoTarea("GTIntegration-WMS: GP-Validaciones", objTarea.Destinatarios, "No se envió el Id de la tarea como argumento al programa, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea", objTarea.Tarea)
            Return False
        End If

    End Function

    Private Function ConsutarXML(ByRef dsConsulta As DataSet) As Boolean
        Dim objCorreo As New clsCorreo

        Try
            objTarea.LogFechaInicioRecuperacionDatosOrigen()
            dsConsulta = objTarea.DatosOrigen(True)
            objTarea.LogFechaFinRecuperacionDatosOrigen()
            objTarea.LogRecuperacionDatosOrigen(1)
            Return True
        Catch ex As Exception
            objTarea.LogRecuperacionDatosOrigen(0)
            objTarea.LogMensajesError(ex.Message)
            'objCorreo.EnviarCorreoTarea("GTIntegration-WMS: Obtencion datos fuentes de la tarea", objTarea.Destinatarios, ex.Message, objTarea.Tarea)
            Return False
        End Try

    End Function

    Public Sub ejecutarProcedimiento(ByVal procedimiento As String)

        Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)

        Try
            Dim sqlComando As SqlCommand = New SqlCommand
            sqlComando.CommandTimeout = 3600
            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = procedimiento
            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            sqlConexion.Close()
        End Try
    End Sub



End Class
