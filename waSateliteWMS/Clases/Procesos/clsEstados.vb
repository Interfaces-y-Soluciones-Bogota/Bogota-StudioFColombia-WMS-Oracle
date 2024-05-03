Imports System.Data.SqlClient

Public Class clsEstados
    Inherits clsConfiguracion
    Public Property Tabla As String
    Public Property intID As Integer
    Public Property VarcharID1 As String
    Public Property VarcharID2 As String
    Public Property VarcharID3 As String
    Public Property VarcharID4 As String
    Public Property Estado As Integer
    Public Property TipoPedido As String
    Public Property NroPedido As String
    Public Property TipoPedidoRQI As String
    Public Property NroPedidoRQI As String
    Public Property TipoPedidoPVE As String
    Public Property NroPedidoPVE As String
    Public Property TipoPedidoPV_PVI As String
    Public Property NroPedidoPV_PVI As String
    Public Sub ValidarTotalCantidadesRemisionadasyCantidadesDespachadasPV_PVI()
        Dim sqlComando As New SqlCommand

        sqlComando.CommandTimeout = 36000000
        sqlComando.Connection = sqlConexion
        sqlComando.CommandType = CommandType.StoredProcedure
        sqlComando.CommandText = "sp_WMS_ActualizacionEstadosRemisionesPV_PVI"
        sqlComando.Parameters.AddWithValue("TipoPedidoPV_PVI", TipoPedidoPV_PVI)
        sqlComando.Parameters.AddWithValue("NroPedidoPV_PVI", NroPedidoPV_PVI)
        Try
            sqlComando.Connection.Open()
            sqlComando.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            sqlComando.Connection.Close()
        End Try

    End Sub
    Public Sub ModificarEstado()
        Dim sqlComando As New SqlCommand

        sqlComando.CommandTimeout = 36000000
        sqlComando.Connection = sqlConexion
        sqlComando.CommandType = CommandType.StoredProcedure
        sqlComando.CommandText = "sp_WMS_ESTADO_ACTUALIZAR"
        sqlComando.Parameters.AddWithValue("Tabla", Tabla)
        sqlComando.Parameters.AddWithValue("intID", intID)
        sqlComando.Parameters.AddWithValue("VarcharID1", VarcharID1)
        sqlComando.Parameters.AddWithValue("VarcharID2", VarcharID2)
        sqlComando.Parameters.AddWithValue("VarcharID3", VarcharID3)
        sqlComando.Parameters.AddWithValue("VarcharID4", VarcharID4)
        sqlComando.Parameters.AddWithValue("Estado", Estado)

        Try
            sqlComando.Connection.Open()
            sqlComando.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            sqlComando.Connection.Close()
        End Try

    End Sub

    Public Sub CompararCantidadesComprometidasParcial()
        Dim sqlComando As New SqlCommand

        sqlComando.CommandTimeout = 36000000
        sqlComando.Connection = sqlConexion
        sqlComando.CommandType = CommandType.StoredProcedure
        sqlComando.CommandText = "sp_WMS_Actualizaccion_CompromisosParciales"
        sqlComando.Parameters.AddWithValue("TipoPedido", TipoPedido)
        sqlComando.Parameters.AddWithValue("NroPedido", NroPedido)


        Try
            sqlComando.Connection.Open()
            sqlComando.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            sqlComando.Connection.Close()
        End Try

    End Sub

    Public Sub CompararCantidadesComprometidasParcialRQI()
        Dim sqlComando As New SqlCommand

        sqlComando.CommandTimeout = 36000000
        sqlComando.Connection = sqlConexion
        sqlComando.CommandType = CommandType.StoredProcedure
        sqlComando.CommandText = "sp_WMS_Actualizaccion_CompromisosParcialesRQI"
        sqlComando.Parameters.AddWithValue("TipoPedidoRQI", TipoPedidoRQI)
        sqlComando.Parameters.AddWithValue("NroPedidoRQI", NroPedidoRQI)
        Try
            sqlComando.Connection.Open()
            sqlComando.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            sqlComando.Connection.Close()
        End Try

    End Sub

    Public Sub CompararCantidadesComprometidasParcialPVE()
        Dim sqlComando As New SqlCommand

        sqlComando.CommandTimeout = 36000000
        sqlComando.Connection = sqlConexion
        sqlComando.CommandType = CommandType.StoredProcedure
        sqlComando.CommandText = "sp_WMS_Actualizaccion_CompromisosParcialesPVE"
        sqlComando.Parameters.AddWithValue("TipoPedidoPVE", TipoPedidoPVE)
        sqlComando.Parameters.AddWithValue("NroPedidoPVE", NroPedidoPVE)
        Try
            sqlComando.Connection.Open()
            sqlComando.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            sqlComando.Connection.Close()
        End Try

    End Sub

    Public Sub cambiarEstadoRemisionEcommerce(ByVal F350_ID_CO As String,
                                             ByVal F430_ID_TIPO_DOCTO As String,
                                             ByVal F430_CONSEC_DOCTO As Integer)
        Dim sqlComando As New SqlCommand

        sqlComando.CommandTimeout = 36000000
        sqlComando.Connection = sqlConexion
        sqlComando.CommandType = CommandType.StoredProcedure
        sqlComando.CommandText = "sp_wms_actualizar_estados_remision_ecommerce"
        sqlComando.Parameters.AddWithValue("F350_ID_CO", F350_ID_CO)
        sqlComando.Parameters.AddWithValue("F430_ID_TIPO_DOCTO", F430_ID_TIPO_DOCTO)
        sqlComando.Parameters.AddWithValue("F430_CONSEC_DOCTO", F430_CONSEC_DOCTO)

        Try
            sqlComando.Connection.Open()
            sqlComando.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            sqlComando.Connection.Close()
        End Try

    End Sub

    Public Sub cambiarEstadoCancelacionRequisiciones(ByVal f440_id_co As String,
                                        ByVal f440_id_tipo_docto As String,
                                        ByVal f440_consec_docto As Integer,
                                        ByVal f441_referencia_item As String,
                                        ByVal f441_id_ext1_detalle As String,
                                        ByVal f441_id_ext2_detalle As String)

        Dim sqlComando As New SqlCommand

        sqlComando.CommandTimeout = 36000000
        sqlComando.Connection = sqlConexion
        sqlComando.CommandType = CommandType.StoredProcedure
        sqlComando.CommandText = "sp_wms_actualizar_estados_cancelacion"
        sqlComando.Parameters.AddWithValue("f440_id_co", f440_id_co)
        sqlComando.Parameters.AddWithValue("f440_id_tipo_docto", f440_id_tipo_docto)
        sqlComando.Parameters.AddWithValue("f440_consec_docto", f440_consec_docto)
        sqlComando.Parameters.AddWithValue("f441_referencia_item", f441_referencia_item)
        sqlComando.Parameters.AddWithValue("f441_id_ext1_detalle", f441_id_ext1_detalle)
        sqlComando.Parameters.AddWithValue("f441_id_ext2_detalle", f441_id_ext2_detalle)

        Try
            sqlComando.Connection.Open()
            sqlComando.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            sqlComando.Connection.Close()
        End Try

    End Sub

    Public Sub cambiarEstadoCancelacionPedidos(ByVal f430_id_co As String, ByVal f430_id_tipo_docto As String,
                                       ByVal f430_consec_docto As Integer, ByVal f431_referencia_item As String,
                                       ByVal f431_id_ext1_detalle As String, ByVal f431_id_ext2_detalle As String,
                                       ByVal f431_nro_registro As String)

        Dim sqlComando As New SqlCommand

        sqlComando.CommandTimeout = 36000000
        sqlComando.Connection = sqlConexion
        sqlComando.CommandType = CommandType.StoredProcedure
        sqlComando.CommandText = "sp_WMS_Pedido_Cancelacion_ActualizarEstado"
        sqlComando.Parameters.AddWithValue("f430_id_co", f430_id_co)
        sqlComando.Parameters.AddWithValue("f430_id_tipo_docto", f430_id_tipo_docto)
        sqlComando.Parameters.AddWithValue("f430_consec_docto", f430_consec_docto)
        sqlComando.Parameters.AddWithValue("f431_referencia_item", f431_referencia_item)
        sqlComando.Parameters.AddWithValue("f431_id_ext1_detalle", f431_id_ext1_detalle)
        sqlComando.Parameters.AddWithValue("f431_id_ext2_detalle", f431_id_ext2_detalle)
        sqlComando.Parameters.AddWithValue("f431_nro_registro", f431_nro_registro)

        Try
            sqlComando.Connection.Open()
            sqlComando.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            sqlComando.Connection.Close()
        End Try

    End Sub





    Public Sub cambiarEstadoItemEcommerce(ByVal F350_ID_CO As String,
                                            ByVal F350_ID_TIPO_DOCTO As String,
                                            ByVal F350_CONSEC_DOCTO As Integer,
                                            ByVal idDespacho As Integer)
        Dim sqlComando As New SqlCommand

        sqlComando.CommandTimeout = 36000000
        sqlComando.Connection = sqlConexion
        sqlComando.CommandType = CommandType.StoredProcedure
        sqlComando.CommandText = "sp_wms_actualizar_estados_itemServicio_ecommerce"
        sqlComando.Parameters.AddWithValue("F350_ID_CO", F350_ID_CO)
        sqlComando.Parameters.AddWithValue("F350_ID_TIPO_DOCTO", F350_ID_TIPO_DOCTO)
        sqlComando.Parameters.AddWithValue("F350_CONSEC_DOCTO", F350_CONSEC_DOCTO)
        sqlComando.Parameters.AddWithValue("idDespacho", idDespacho)

        Try
            sqlComando.Connection.Open()
            sqlComando.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            sqlComando.Connection.Close()
        End Try

    End Sub

    Public Sub verificacionEstadosTareas(ByVal tarea As Integer)
        Dim sqlComando As New SqlCommand

        sqlComando.CommandTimeout = 36000000
        sqlComando.Connection = sqlConexion
        sqlComando.CommandType = CommandType.StoredProcedure
        sqlComando.CommandText = "sp_wms_verificacion_estados_tareas"
        sqlComando.Parameters.AddWithValue("tarea", tarea)
        Try
            sqlComando.Connection.Open()
            sqlComando.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            sqlComando.Connection.Close()
        End Try

    End Sub





End Class
