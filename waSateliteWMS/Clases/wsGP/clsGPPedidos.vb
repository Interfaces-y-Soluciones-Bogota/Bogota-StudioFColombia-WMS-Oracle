Imports System.ComponentModel
Imports System.Data.SqlClient

Public Class clsGPPedidos

    Dim objTarea As New clsTarea
    Public Sub almacenarPedido(ByVal idTarea As Integer)
        Dim objCorreo As New clsCorreo
        Dim Resultado As String = ""
        Dim objConfiguracion As New clsConfiguracion
        Dim ws As wsMobilistic.SGAMovilWebServiceSoap = New wsMobilistic.SGAMovilWebServiceSoapClient
        Dim resultKeyWeb As wsMobilistic.WebSecurityResponse = ws.GetSecurityKeyWeb(objConfiguracion.WMS_GP_USER)


        Try
            If logInicial(Environment.GetCommandLineArgs()) Then
                Try
                    objTarea.LogFechaInicioRecuperacionDatosOrigen()
                    objTarea.LogFechaInicioGeneracionPlano()
                    objTarea.LogFechaInicioWebServiceSiesa()

                    If resultKeyWeb.ResultCode = 0 Then
                        Dim request = New wsMobilistic.Request With {
                                .User = objConfiguracion.WMS_GP_USER,
                                .UserId = objConfiguracion.WMS_GP_USER_ID,
                                .ValidationKey = resultKeyWeb.ValidationKey
                            }

                        Dim response As wsMobilistic.OutcomeDetailListResponseList = ws.GetOutcomeWithDetailListReleased(request)
                        If response.ResultCode = 0 Then
                            For Each Pedido As waSateliteWMS.wsMobilistic.OutcomeDetailListResponse In response.ItemList
                                ejecutarProcedimientoAlmacenarPedido(Pedido.IdOutcomeType, Pedido.OutcomeType, Pedido.IdOutcomeState,
                                                                     Pedido.OutcomeState, Pedido.IdOutcomeReceiver, Pedido.OutcomeReceiver,
                                                                     Pedido.Number, Pedido.DocumentNumber, Pedido.Description,
                                                                     Pedido.OutcomeDate, Pedido.CreationDate, Pedido.MinimumDispatchDate,
                                                                     Pedido.IsDispatched, Pedido.IdClient, Pedido.Id, Pedido.IsInUse,
                                                                     Pedido.OutcomeReceiverCode, Pedido.NIT, Pedido.CompanyName,
                                                                     Pedido.BranchCode, Pedido.BranchName, Pedido.CityReceiver, Pedido.ChannelReceiver,
                                                                     Pedido.DepartmentReceiver, Pedido.CountryReceiver, Pedido.BillTo)
                                For Each Item As waSateliteWMS.wsMobilistic.OutcomeDetailResponse In Pedido.ItemList
                                    ejecutarProcedimientoAlmacenarItem(Item.AsignedQuantity, Item.BarCode, Item.Color, Item.ColorCode, Item.Description, Item.DispatchedDate,
                                                                       Item.DispatchedQuantity, Item.Id, Item.IdClient, Item.IdClientProject, Item.IdOutcome,
                                                                       Item.IdProduct, Item.InProcessA, Item.IsAssigned, Item.IsDispatched, Item.IsInUse,
                                                                       Item.IsReleased, Item.ItemState, Item.Outcome, Item.PendingPerAsign, Item.PendingPerRelease,
                                                                       Item.PendingQuantity, Item.Product, Item.Project, Item.Quantity, Item.ReleasedQuantity,
                                                                       Item.Size, Item.TotalAsigned, Item.Price)
                                Next
                            Next
                        End If
                    Else
                        Resultado = Resultado & " Error:" & resultKeyWeb.ErrorMessage & " Mensaje del resultado:" & resultKeyWeb.ResultMessage
                    End If


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
                    'objCorreo.EnviarCorreoTarea("GTIntegration-WMS: GP-CONSULTAR PEDIDOS", objTarea.Destinatarios, ex.Message, objTarea.Tarea)
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

    Public Function actualizarPedido(ByVal dsDatos As DataSet) As String
        Dim Resultado As String = ""
        Dim objConfiguracion As New clsConfiguracion
        Dim ws As wsMobilistic.SGAMovilWebServiceSoap = New wsMobilistic.SGAMovilWebServiceSoapClient
        Dim resultKeyWeb As wsMobilistic.WebSecurityResponse = ws.GetSecurityKeyWeb(objConfiguracion.WMS_GP_USER)
        Dim updateItem As wsMobilistic.OutcomeDetailAddEditRequest

        If resultKeyWeb.ResultCode = 0 Then
            For Each Item As DataRow In dsDatos.Tables(0).Rows
                Try

                    updateItem = New wsMobilistic.OutcomeDetailAddEditRequest With {
                            .User = objConfiguracion.WMS_GP_USER,
                            .UserId = objConfiguracion.WMS_GP_USER_ID,
                            .ValidationKey = resultKeyWeb.ValidationKey,
                            .ItemState = 5, ' Valor fijo - estado del ítem (cerrado)
                            .IdOutcome = Item.Item("IdOutcome"), 'ID del pedido   IdOutcome
                            .Id = Item.Item("Id"), ' ID del ítem 
                            .DispatchedQuantity = Item.Item("DispatchedQuantity") ' Cantidad despachada
                    }

                    Dim result = ws.EditOutcomeDetail(updateItem)

                    If result.ResultCode <> 0 Then
                        Resultado = Resultado & " Error al actualizar el pedido con id: " & Item.Item("IdOutcome") & " Mensaje de resultado: " & result.ResultMessage & " Codigo del resultado: " & result.ResultCode
                    Else
                        ejecutarProcedimiento("sp_WMS_GP_PEDIDO_ITEM_ACTUALIZAR_ESTADO_DESPACHO", Item.Item("Id"))
                    End If

                Catch ex As Exception
                    Resultado = Resultado & " " & ex.Message
                End Try
            Next

        Else
            Resultado = Resultado & " Error: " & resultKeyWeb.ErrorMessage & " Mensaje del resultado: " & resultKeyWeb.ResultMessage
        End If

        Return Resultado

    End Function

    Public Function actualizarPedidoCancelado(ByVal dsDatos As DataSet) As String
        Dim Resultado As String = ""
        Dim objConfiguracion As New clsConfiguracion
        Dim ws As wsMobilistic.SGAMovilWebServiceSoap = New wsMobilistic.SGAMovilWebServiceSoapClient
        Dim resultKeyWeb As wsMobilistic.WebSecurityResponse = ws.GetSecurityKeyWeb(objConfiguracion.WMS_GP_USER)
        Dim updateItem As wsMobilistic.OutcomeDetailAddEditRequest

        If resultKeyWeb.ResultCode = 0 Then
            For Each Item As DataRow In dsDatos.Tables(0).Rows
                Try

                    updateItem = New wsMobilistic.OutcomeDetailAddEditRequest With {
                        .User = objConfiguracion.WMS_GP_USER,
                        .UserId = objConfiguracion.WMS_GP_USER_ID,
                        .ValidationKey = resultKeyWeb.ValidationKey,
                        .ItemState = 6, ' Valor fijo - estado del ítem (cancelado)
                        .IdOutcome = Item.Item("IdOutcome"), 'ID del pedido   IdOutcome
                        .Id = Item.Item("Id"), ' ID del ítem 
                        .CancelledQuantity = Item.Item("CancelledQuantity"),
                        .CancelledDate = Date.Now.ToString("yyyy-MM-dd")
                    }


                    Dim result = ws.EditOutcomeDetail(updateItem)

                    If result.ResultCode <> 0 Then
                        Resultado = Resultado & " Error al actualizar el pedido con id: " & Item.Item("IdOutcome") & " Mensaje de resultado: " & result.ResultMessage & " Codigo del resultado: " & result.ResultCode
                    Else
                        ejecutarProcedimientoCancelado("sp_WMS_GP_PEDIDO_ITEM_ACTUALIZAR_ESTADO_DESPACHO_CANCELADOS", Item.Item("IdOutcome"), Item.Item("BarCode"))
                    End If

                Catch ex As Exception
                    Resultado = Resultado & " " & ex.Message
                End Try
            Next

        Else
            Resultado = Resultado & " Error: " & resultKeyWeb.ErrorMessage & " Mensaje del resultado: " & resultKeyWeb.ResultMessage
        End If

        Return Resultado

    End Function


    Private Sub ejecutarProcedimientoAlmacenarPedido(ByVal id_outcome_type As Integer,
                                  ByVal outcome_type As String,
                                  ByVal id_outcome_state As Integer,
                                  ByVal outcome_state As String,
                                  ByVal id_outcome_receiver As Integer,
                                  ByVal outcome_receiver As String,
                                  ByVal Number As String,
                                  ByVal document_number As String,
                                  ByVal description As String,
                                  ByVal outcome_date As Date,
                                  ByVal creation_date As Date,
                                  ByVal minimum_dispatch_date As Date,
                                  ByVal is_dispatched As Boolean,
                                  ByVal id_client As Integer,
                                  ByVal id As Integer,
                                  ByVal is_in_use As Boolean,
                                  ByVal outcome_receiver_code As String,
                                  ByVal nit As String,
                                  ByVal company_name As String,
                                  ByVal branch_code As String,
                                  ByVal branch_name As String,
                                  ByVal city_receiver As String,
                                  ByVal channel_receiver As String,
                                  ByVal department_receiver As String,
                                  ByVal country_receiver As String,
                                  ByVal bill_to As String
                                )

        Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)

        Try
            Dim sqlComando As SqlCommand = New SqlCommand
            sqlComando.CommandTimeout = 3600
            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = "sp_WMS_GP_PEDIDO_ALMACENAR"

            sqlComando.Parameters.AddWithValue("creation_date", creation_date)
            sqlComando.Parameters.AddWithValue("description", description)
            sqlComando.Parameters.AddWithValue("document_number", document_number)
            sqlComando.Parameters.AddWithValue("id", id)
            sqlComando.Parameters.AddWithValue("id_client", id_client)
            sqlComando.Parameters.AddWithValue("id_outcome_receiver", id_outcome_receiver)
            sqlComando.Parameters.AddWithValue("id_outcome_state", id_outcome_state)
            sqlComando.Parameters.AddWithValue("id_outcome_type", id_outcome_type)
            sqlComando.Parameters.AddWithValue("is_dispatched", is_dispatched)
            sqlComando.Parameters.AddWithValue("is_in_use", is_in_use)
            sqlComando.Parameters.AddWithValue("minimum_dispatch_date", minimum_dispatch_date)
            sqlComando.Parameters.AddWithValue("number", Number)
            sqlComando.Parameters.AddWithValue("outcome_date", outcome_date)
            sqlComando.Parameters.AddWithValue("outcome_receiver", outcome_receiver)
            sqlComando.Parameters.AddWithValue("outcome_state", outcome_state)
            sqlComando.Parameters.AddWithValue("outcome_type", outcome_type)
            sqlComando.Parameters.AddWithValue("outcome_receiver_code", outcome_receiver_code)
            sqlComando.Parameters.AddWithValue("nit", nit)
            sqlComando.Parameters.AddWithValue("company_name", company_name)
            sqlComando.Parameters.AddWithValue("branch_code", branch_code)
            sqlComando.Parameters.AddWithValue("branch_name", branch_name)
            sqlComando.Parameters.AddWithValue("city_receiver", city_receiver)
            sqlComando.Parameters.AddWithValue("channel_receiver", channel_receiver)
            sqlComando.Parameters.AddWithValue("department_receiver", department_receiver)
            sqlComando.Parameters.AddWithValue("country_receiver", country_receiver)
            sqlComando.Parameters.AddWithValue("bill_to", bill_to)

            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            sqlConexion.Close()
        End Try
    End Sub


    Private Sub ejecutarProcedimientoAlmacenarItem(ByVal asigned_quantity As Integer,
                             ByVal barcode As String,
                             ByVal color As String,
                             ByVal color_code As String,
                             ByVal description As String,
                             ByVal dispatched_date As Date,
                             ByVal dispatched_quantity As Int64,
                             ByVal id As Integer,
                             ByVal id_client As Integer,
                             ByVal id_client_project As Integer,
                             ByVal id_outcome As Integer,
                             ByVal id_product As Integer,
                             ByVal in_process_a As Integer,
                             ByVal is_assigned As Boolean,
                             ByVal is_dispatched As Boolean,
                             ByVal is_in_use As Boolean,
                             ByVal is_released As Boolean,
                             ByVal item_state As Integer,
                             ByVal outcome As String,
                             ByVal pending_per_asign As Integer,
                             ByVal pending_per_release As Integer,
                             ByVal pending_quantity As Int64,
                             ByVal product As String, 'referencia del ítem
                             ByVal project As String,
                             ByVal quantity As Int64,
                             ByVal released_quantity As Integer,
                             ByVal size As String,
                             ByVal total_asigned As Integer,
                             ByVal price As Double)

        Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)

        Try
            Dim sqlComando As SqlCommand = New SqlCommand
            sqlComando.CommandTimeout = 3600
            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = "sp_WMS_GP_PEDIDO_ITEM_ALMACENAR"

            sqlComando.Parameters.AddWithValue("asigned_quantity", asigned_quantity)
            sqlComando.Parameters.AddWithValue("barcode", barcode)
            sqlComando.Parameters.AddWithValue("color", color)
            sqlComando.Parameters.AddWithValue("color_code", color_code)
            sqlComando.Parameters.AddWithValue("description", description)
            sqlComando.Parameters.AddWithValue("dispatched_date", dispatched_date)
            sqlComando.Parameters.AddWithValue("dispatched_quantity", dispatched_quantity)
            sqlComando.Parameters.AddWithValue("id", id)
            sqlComando.Parameters.AddWithValue("id_client", id_client)
            sqlComando.Parameters.AddWithValue("id_client_project", id_client_project)
            sqlComando.Parameters.AddWithValue("id_outcome", id_outcome)
            sqlComando.Parameters.AddWithValue("id_product", id_product)
            sqlComando.Parameters.AddWithValue("in_process_a", in_process_a)
            sqlComando.Parameters.AddWithValue("is_assigned", is_assigned)
            sqlComando.Parameters.AddWithValue("is_dispatched", is_dispatched)
            sqlComando.Parameters.AddWithValue("is_in_use", is_in_use)
            sqlComando.Parameters.AddWithValue("is_released", is_released)
            sqlComando.Parameters.AddWithValue("item_state", item_state)
            sqlComando.Parameters.AddWithValue("outcome", outcome)
            sqlComando.Parameters.AddWithValue("pending_per_asign", pending_per_asign)
            sqlComando.Parameters.AddWithValue("pending_per_release", pending_per_release)
            sqlComando.Parameters.AddWithValue("pending_quantity", pending_quantity)
            sqlComando.Parameters.AddWithValue("product", product)
            sqlComando.Parameters.AddWithValue("project", project)
            sqlComando.Parameters.AddWithValue("quantity", quantity)
            sqlComando.Parameters.AddWithValue("released_quantity", released_quantity)
            sqlComando.Parameters.AddWithValue("size", size)
            sqlComando.Parameters.AddWithValue("total_asigned", total_asigned)
            sqlComando.Parameters.AddWithValue("price", price)

            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            sqlConexion.Close()
        End Try
    End Sub

    Private Sub ejecutarProcedimiento(ByVal procedimiento As String, ByVal parametro As String)

        Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)

        Try
            Dim sqlComando As SqlCommand = New SqlCommand
            sqlComando.CommandTimeout = 3600
            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = procedimiento
            sqlComando.Parameters.AddWithValue("id", parametro)
            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            sqlConexion.Close()
        End Try
    End Sub

    Private Sub ejecutarProcedimientoCancelado(ByVal procedimiento As String, ByVal IdOutcome As String, ByVal BarCode As String)

        Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)

        Try
            Dim sqlComando As SqlCommand = New SqlCommand
            sqlComando.CommandTimeout = 3600
            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = procedimiento
            sqlComando.Parameters.AddWithValue("IdOutcome", IdOutcome)
            sqlComando.Parameters.AddWithValue("BarCode", BarCode)
            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            sqlConexion.Close()
        End Try
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
                objCorreo.EnviarCorreoTarea("GTIntegration-WMS: GP-Validaciones", objTarea.Destinatarios, "El argumento enviado debe ser numérico, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea", objTarea.Tarea)
                Return False
            End If
        Else
            objTarea.LogRecuperacionDatosOrigen(0)
            objTarea.LogMensajesError("No se envió el Id de la tarea como argumento al programa, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea")
            objCorreo.EnviarCorreoTarea("GTIntegration-WMS: GP-Validaciones", objTarea.Destinatarios, "No se envió el Id de la tarea como argumento al programa, verifique la configuración de la tarea programada y configure como argumento el Id de la tarea", objTarea.Tarea)
            Return False
        End If

    End Function
End Class
