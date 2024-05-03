Imports System.Data.SqlClient

Public Class clsPrioridadBodegas
    Inherits clsConfiguracion

    Public mensajeErrorBodegas As String = ""

    Public Function PriorizarBodegas(ByVal dsDatos As DataSet) As DataSet

        Dim dsBodegasPrioridad As DataSet
        Dim dvInventario As DataView
        Dim dsResultado As DataSet = CrearDataSet()
        Dim dvDocumento As New DataView(dsDatos.Tables(0))
        Dim swRegistroCompleto As Boolean
        Dim consecutivo = 0

        dsBodegasPrioridad = ConsultarBodegasPrioridadAjustes()
        dvInventario = ConsultarInventarioPrioridadAjustes()
        dvInventario.AllowEdit = True


        Try
            For Each Registros As DataRow In dsDatos.Tables(1).Rows
                dvDocumento.RowFilter = "f350_id_co= '" & Registros.Item("f470_id_co") & "' and f350_id_tipo_docto = '" & Registros.Item("f470_id_tipo_docto") & "' and f350_consec_docto = " & Registros.Item("f470_consec_docto")

                If (dvDocumento.ToTable.Rows(0).Item("f350_id_tipo_docto") = "AJW" And dvDocumento.ToTable.Rows(0).Item("f350_notas").Contains("RCL")) Then
                    If consecutivo <> dvDocumento.ToTable.Rows(0).Item("f350_consec_docto") Then
                        dsResultado.Tables(0).Rows.Add(dvDocumento.ToTable.Rows(0).Item("f350_id_co"), dvDocumento.ToTable.Rows(0).Item("f350_id_tipo_docto"), dvDocumento.ToTable.Rows(0).Item("f350_consec_docto"), dvDocumento.ToTable.Rows(0).Item("f350_fecha"),
                                                               dvDocumento.ToTable.Rows(0).Item("f350_id_clase_docto"), dvDocumento.ToTable.Rows(0).Item("f350_notas"), dvDocumento.ToTable.Rows(0).Item("f450_id_concepto"), dvDocumento.ToTable.Rows(0).Item("f450_id_bodega_salida"),
                                                               dvDocumento.ToTable.Rows(0).Item("f450_id_bodega_entrada"), dvDocumento.ToTable.Rows(0).Item("f450_docto_alterno"), dvDocumento.ToTable.Rows(0).Item("ValidacionBodega"), dvDocumento.ToTable.Rows(0).Item("ref_value_6"),
                                                               dvDocumento.ToTable.Rows(0).Item("shipment_nbr"), dvDocumento.ToTable.Rows(0).Item("activity_code"), dvDocumento.ToTable.Rows(0).Item("Bodega2"), dvDocumento.ToTable.Rows(0).Item("Bodega3"),
                                                               dvDocumento.ToTable.Rows(0).Item("orden"), dvDocumento.ToTable.Rows(0).Item("f470_id_motivo"), dvDocumento.ToTable.Rows(0).Item("numRow"))
                    End If

                    consecutivo = dvDocumento.ToTable.Rows(0).Item("f350_consec_docto")

                Else
                    dsResultado.Tables(0).Rows.Add(dvDocumento.ToTable.Rows(0).Item("f350_id_co"), dvDocumento.ToTable.Rows(0).Item("f350_id_tipo_docto"), dvDocumento.ToTable.Rows(0).Item("f350_consec_docto"), dvDocumento.ToTable.Rows(0).Item("f350_fecha"),
                                                           dvDocumento.ToTable.Rows(0).Item("f350_id_clase_docto"), dvDocumento.ToTable.Rows(0).Item("f350_notas"), dvDocumento.ToTable.Rows(0).Item("f450_id_concepto"), dvDocumento.ToTable.Rows(0).Item("f450_id_bodega_salida"),
                                                           dvDocumento.ToTable.Rows(0).Item("f450_id_bodega_entrada"), dvDocumento.ToTable.Rows(0).Item("f450_docto_alterno"), dvDocumento.ToTable.Rows(0).Item("ValidacionBodega"), dvDocumento.ToTable.Rows(0).Item("ref_value_6"),
                                                           dvDocumento.ToTable.Rows(0).Item("shipment_nbr"), dvDocumento.ToTable.Rows(0).Item("activity_code"), dvDocumento.ToTable.Rows(0).Item("Bodega2"), dvDocumento.ToTable.Rows(0).Item("Bodega3"),
                                                           dvDocumento.ToTable.Rows(0).Item("orden"), dvDocumento.ToTable.Rows(0).Item("f470_id_motivo"), dvDocumento.ToTable.Rows(0).Item("numRow"))
                End If

                swRegistroCompleto = False
                'Si el documento es TRW o el ajuste es una entrada o es un ajuste en costo o es una reclasificación no se realiza ninguna operación sobre los datos origen
                If Registros.Item("f470_id_tipo_docto") = "TRW" Or (Registros.Item("f470_id_tipo_docto") = "AJW" And Registros.Item("adj_qty") > 0) Or (Registros.Item("f470_id_tipo_docto") = "AJW" And Registros.Item("f470_costo_prom_uni") <> 0) Or (dvDocumento.ToTable.Rows(0).Item("f350_id_tipo_docto") = "AJW" And dvDocumento.ToTable.Rows(0).Item("f350_notas").Contains("RCL")) Then
                    agregarRegistro(dsResultado, dvDocumento, Registros)
                Else


                    'Caso 1: Se encuentran todas las existencias en una sola bodega
                    'Acción: Se asigna la bodega en la cual se encuentran las existencias según la prioridad
                    For Each Bodega As DataRow In dsBodegasPrioridad.Tables(0).Rows
                        dvInventario.RowFilter = "Clave='" & Bodega.Item("f150_id") & "-" & Registros.Item("f470_referencia_item") & "-" & Registros.Item("f470_id_ext1_detalle") & "-" & Registros.Item("f470_id_ext2_detalle") & "'"
                        If dvInventario.ToTable.Rows.Count > 0 Then
                            If dvInventario.ToTable.Rows(0).Item("Existencia") >= Registros.Item("f470_cant_base") Then
                                'dvDocumento.RowFilter = "f350_id_co= '" & Registros.Item("f470_id_co") & "' and f350_id_tipo_docto = '" & Registros.Item("f470_id_tipo_docto") & "' and f350_consec_docto = " & Registros.Item("f470_consec_docto")

                                dsResultado.Tables(1).Rows.Add(Registros.Item("f470_consec_docto"), Registros.Item("f470_id_co"), Registros.Item("f470_id_tipo_docto"), Registros.Item("f470_nro_registro"),
                                                                           Bodega.Item("f150_id"), Registros.Item("f470_id_ubicación_aux"), Registros.Item("f470_id_concepto"), Registros.Item("f470_id_motivo"),
                                                                           Registros.Item("f470_cant_base"), Registros.Item("f470_costo_prom_uni"), Registros.Item("f470_id_ubicación_aux_ent"), Registros.Item("f470_referencia_item"),
                                                                           Registros.Item("f470_id_ext1_detalle"), Registros.Item("f470_id_ext2_detalle"), Registros.Item("adj_qty"), Registros.Item("orden"),
                                               Registros.Item("group_nbr"), Registros.Item("seq_nbr"))

                                'Descontar existencias del dataView de inventario
                                dvInventario(0)("Existencia") = dvInventario.ToTable.Rows(0).Item("Existencia") - Registros.Item("f470_cant_base")

                                swRegistroCompleto = True
                                Exit For
                            End If
                        End If
                    Next

                    If swRegistroCompleto = False Then
                        Dim TotalExistenciasEnBodegas As Integer
                        Dim tmpCantidadBase As Integer
                        Dim CantidaPendiente As Integer
                        TotalExistenciasEnBodegas = 0
                        tmpCantidadBase = 0
                        CantidaPendiente = Registros.Item("f470_cant_base")

                        For Each Bodega As DataRow In dsBodegasPrioridad.Tables(0).Rows
                            dvInventario.RowFilter = "Clave='" & Bodega.Item("f150_id") & "-" & Registros.Item("f470_referencia_item") & "-" & Registros.Item("f470_id_ext1_detalle") & "-" & Registros.Item("f470_id_ext2_detalle") & "'"
                            If dvInventario.ToTable.Rows.Count > 0 Then
                                If dvInventario.ToTable.Rows(0).Item("Existencia") > 0 Then
                                    TotalExistenciasEnBodegas = TotalExistenciasEnBodegas + dvInventario.ToTable.Rows(0).Item("Existencia")
                                End If
                            End If
                        Next

                        'Caso 2: Se encuentran todas las existencias en varias bodegas
                        'Acción: Se deben generar los registros correspondientes a las existencias de cada bodega
                        If TotalExistenciasEnBodegas >= Registros.Item("f470_cant_base") Then
                            For Each Bodega As DataRow In dsBodegasPrioridad.Tables(0).Rows
                                dvInventario.RowFilter = "Clave='" & Bodega.Item("f150_id") & "-" & Registros.Item("f470_referencia_item") & "-" & Registros.Item("f470_id_ext1_detalle") & "-" & Registros.Item("f470_id_ext2_detalle") & "'"
                                If dvInventario.ToTable.Rows.Count > 0 Then
                                    If dvInventario.ToTable.Rows(0).Item("Existencia") > 0 Then

                                        tmpCantidadBase = dvInventario.ToTable.Rows(0).Item("Existencia")
                                        If CantidaPendiente - tmpCantidadBase > 0 Then
                                            CantidaPendiente = CantidaPendiente - tmpCantidadBase
                                        Else
                                            tmpCantidadBase = CantidaPendiente
                                            CantidaPendiente = CantidaPendiente - tmpCantidadBase
                                        End If

                                        dsResultado.Tables(1).Rows.Add(Registros.Item("f470_consec_docto"), Registros.Item("f470_id_co"), Registros.Item("f470_id_tipo_docto"), Registros.Item("f470_nro_registro"),
                                               Bodega.Item("f150_id"), Registros.Item("f470_id_ubicación_aux"), Registros.Item("f470_id_concepto"), Registros.Item("f470_id_motivo"),
                                               tmpCantidadBase, Registros.Item("f470_costo_prom_uni"), Registros.Item("f470_id_ubicación_aux_ent"), Registros.Item("f470_referencia_item"),
                                               Registros.Item("f470_id_ext1_detalle"), Registros.Item("f470_id_ext2_detalle"), Registros.Item("adj_qty"), Registros.Item("orden"),
                                               Registros.Item("group_nbr"), Registros.Item("seq_nbr"))

                                        'Descontar existencias del dataView de inventario
                                        dvInventario(0)("Existencia") = dvInventario.ToTable.Rows(0).Item("Existencia") - tmpCantidadBase

                                        If CantidaPendiente = 0 Then
                                            Exit For
                                        End If

                                    End If
                                End If
                            Next
                            swRegistroCompleto = True
                        End If

                        'Caso 3: No se encuentren existencias en ninguna bodega
                        'Acción: Se debe almacenar el log de errores
                        If TotalExistenciasEnBodegas = 0 Then
                            dsResultado.Tables(1).Rows.Add(Registros.Item("f470_consec_docto"), Registros.Item("f470_id_co"), Registros.Item("f470_id_tipo_docto"), Registros.Item("f470_nro_registro"),
                                               Registros.Item("f470_id_bodega"), Registros.Item("f470_id_ubicación_aux"), Registros.Item("f470_id_concepto"), Registros.Item("f470_id_motivo"),
                                               Registros.Item("f470_cant_base"), Registros.Item("f470_costo_prom_uni"), Registros.Item("f470_id_ubicación_aux_ent"), Registros.Item("f470_referencia_item"),
                                               Registros.Item("f470_id_ext1_detalle"), Registros.Item("f470_id_ext2_detalle"), Registros.Item("adj_qty"), Registros.Item("orden"),
                                               Registros.Item("group_nbr"), Registros.Item("seq_nbr"))
                            swRegistroCompleto = True
                        End If


                        'Caso 4: Se ecuentran existencias pero estas no cubren la cantidad requerida
                        'Acción: Se debe almacenar el log de errores y debe quedar el documento pendiente por procesar
                        If swRegistroCompleto = False Then
                            If TotalExistenciasEnBodegas <= Registros.Item("f470_cant_base") And TotalExistenciasEnBodegas <> 0 Then
                                For Each Bodega As DataRow In dsBodegasPrioridad.Tables(0).Rows
                                    dvInventario.RowFilter = "Clave='" & Bodega.Item("f150_id") & "-" & Registros.Item("f470_referencia_item") & "-" & Registros.Item("f470_id_ext1_detalle") & "-" & Registros.Item("f470_id_ext2_detalle") & "'"
                                    If dvInventario.ToTable.Rows.Count > 0 Then
                                        If dvInventario.ToTable.Rows(0).Item("Existencia") > 0 Then
                                            tmpCantidadBase = dvInventario.ToTable.Rows(0).Item("Existencia")
                                            If CantidaPendiente - tmpCantidadBase > 0 Then
                                                CantidaPendiente = CantidaPendiente - tmpCantidadBase
                                            Else
                                                tmpCantidadBase = CantidaPendiente
                                            End If

                                            dsResultado.Tables(1).Rows.Add(Registros.Item("f470_consec_docto"), Registros.Item("f470_id_co"), Registros.Item("f470_id_tipo_docto"), Registros.Item("f470_nro_registro"),
                                                   Bodega.Item("f150_id"), Registros.Item("f470_id_ubicación_aux"), Registros.Item("f470_id_concepto"), Registros.Item("f470_id_motivo"),
                                                   tmpCantidadBase, Registros.Item("f470_costo_prom_uni"), Registros.Item("f470_id_ubicación_aux_ent"), Registros.Item("f470_referencia_item"),
                                                   Registros.Item("f470_id_ext1_detalle"), Registros.Item("f470_id_ext2_detalle"), Registros.Item("adj_qty"), Registros.Item("orden"),
                                                   Registros.Item("group_nbr"), Registros.Item("seq_nbr"))

                                            'Descontar existencias del dataView de inventario
                                            dvInventario(0)("Existencia") = dvInventario.ToTable.Rows(0).Item("Existencia") - tmpCantidadBase
                                        End If
                                    End If
                                Next

                                'Almacena la cantidad pendiente en la tabla historial de inventarios
                                'PrioridadAjustesAlmacenarPendiente(Registros.Item("group_nbr"), Registros.Item("seq_nbr"), CantidaPendiente)

                                'Cantidad pendiente por ser procesada
                                mensajeErrorBodegas = "Mensaje de error GTI: El ajuste con documento " & Registros.Item("f470_id_tipo_docto") & Registros.Item("f470_consec_docto") &
                                                      " no completo el total cantidad base, cantidad pendiente: " & CantidaPendiente & ", SKU: " &
                                                      Registros.Item("f470_referencia_item") & "-" & Registros.Item("f470_id_ext1_detalle") & "-" & Registros.Item("f470_id_ext2_detalle")

                                swRegistroCompleto = True
                            End If
                        End If

                    End If
                End If
            Next

            Return dsResultado
        Catch ex As Exception
            Throw ex
        End Try

    End Function


    Public Sub agregarRegistro(ByRef dsResultado As DataSet, ByVal dvDocumento As DataView, ByVal Registros As DataRow)
        dsResultado.Tables(1).Rows.Add(Registros.Item("f470_consec_docto"), Registros.Item("f470_id_co"), Registros.Item("f470_id_tipo_docto"), Registros.Item("f470_nro_registro"),
                               Registros.Item("f470_id_bodega"), Registros.Item("f470_id_ubicación_aux"), Registros.Item("f470_id_concepto"), Registros.Item("f470_id_motivo"),
                               Registros.Item("f470_cant_base"), Registros.Item("f470_costo_prom_uni"), Registros.Item("f470_id_ubicación_aux_ent"), Registros.Item("f470_referencia_item"),
                               Registros.Item("f470_id_ext1_detalle"), Registros.Item("f470_id_ext2_detalle"), Registros.Item("adj_qty"), Registros.Item("orden"),
                               Registros.Item("group_nbr"), Registros.Item("seq_nbr"))

    End Sub


    Private Function ConsultarBodegasPrioridadAjustes() As DataSet

        Dim dsResultado As New DataSet
        Dim sqlComando As New SqlCommand
        Dim sqlDa As New SqlDataAdapter

        sqlDa.SelectCommand = sqlComando
        sqlComando.CommandTimeout = 36000000
        sqlComando.Connection = sqlConexion
        sqlComando.CommandType = CommandType.StoredProcedure
        sqlComando.CommandText = "sp_WMS_BodegasPrioridadAjustes"

        Try
            sqlDa.Fill(dsResultado)
            Return dsResultado
        Catch ex As Exception
            Throw ex
        Finally
            sqlComando.Connection.Close()
        End Try

    End Function

    Private Function ConsultarInventarioPrioridadAjustes() As DataView

        Dim dsResultado As New DataSet
        Dim sqlComando As New SqlCommand
        Dim sqlDa As New SqlDataAdapter


        sqlDa.SelectCommand = sqlComando
        sqlComando.CommandTimeout = 36000000
        sqlComando.Connection = sqlConexion
        sqlComando.CommandType = CommandType.StoredProcedure
        sqlComando.CommandText = "sp_WMS_InventarioPrioridadAjustes"

        Try
            sqlDa.Fill(dsResultado)
            Dim dvResutado As New DataView(dsResultado.Tables(0))
            Return dvResutado
        Catch ex As Exception
            Throw ex
        Finally
            sqlComando.Connection.Close()
        End Try

    End Function

    Public Function CrearDataSet() As DataSet
        Dim dtDocumento As New DataTable("Documentos")
        Dim dtMovimiento As New DataTable("Movimientos")
        Dim dsDatos As New DataSet


        'Documento
        dtDocumento.Columns.Add("f350_id_co", GetType(String))
        dtDocumento.Columns.Add("f350_id_tipo_docto", GetType(String))
        dtDocumento.Columns.Add("f350_consec_docto", GetType(Integer))
        dtDocumento.Columns.Add("f350_fecha", GetType(String))
        dtDocumento.Columns.Add("f350_id_clase_docto", GetType(Integer))
        dtDocumento.Columns.Add("f350_notas", GetType(String))
        dtDocumento.Columns.Add("f450_id_concepto", GetType(Integer))
        dtDocumento.Columns.Add("f450_id_bodega_salida", GetType(String))
        dtDocumento.Columns.Add("f450_id_bodega_entrada", GetType(String))
        dtDocumento.Columns.Add("f450_docto_alterno", GetType(String))
        dtDocumento.Columns.Add("ValidacionBodega", GetType(String))
        dtDocumento.Columns.Add("ref_value_6", GetType(String))
        dtDocumento.Columns.Add("shipment_nbr", GetType(String))
        dtDocumento.Columns.Add("activity_code", GetType(Integer))
        dtDocumento.Columns.Add("Bodega2", GetType(String))
        dtDocumento.Columns.Add("Bodega3", GetType(String))
        dtDocumento.Columns.Add("orden", GetType(String))
        dtDocumento.Columns.Add("f470_id_motivo", GetType(String))
        dtDocumento.Columns.Add("numRow", GetType(Integer))

        'Movimiento
        dtMovimiento.Columns.Add("f470_consec_docto", GetType(Integer))
        dtMovimiento.Columns.Add("f470_id_co", GetType(String))
        dtMovimiento.Columns.Add("f470_id_tipo_docto", GetType(String))
        dtMovimiento.Columns.Add("f470_nro_registro", GetType(Int64))
        dtMovimiento.Columns.Add("f470_id_bodega", GetType(String))
        dtMovimiento.Columns.Add("f470_id_ubicación_aux", GetType(String))
        dtMovimiento.Columns.Add("f470_id_concepto", GetType(Integer))
        dtMovimiento.Columns.Add("f470_id_motivo", GetType(String))
        dtMovimiento.Columns.Add("f470_cant_base", GetType(Decimal))
        dtMovimiento.Columns.Add("f470_costo_prom_uni", GetType(Decimal))
        dtMovimiento.Columns.Add("f470_id_ubicación_aux_ent", GetType(String))
        dtMovimiento.Columns.Add("f470_referencia_item", GetType(String))
        dtMovimiento.Columns.Add("f470_id_ext1_detalle", GetType(String))
        dtMovimiento.Columns.Add("f470_id_ext2_detalle", GetType(String))
        dtMovimiento.Columns.Add("adj_qty", GetType(Decimal))
        dtMovimiento.Columns.Add("orden", GetType(String))
        dtMovimiento.Columns.Add("group_nbr", GetType(Integer))
        dtMovimiento.Columns.Add("seq_nbr", GetType(Integer))

        dsDatos.Tables.Add(dtDocumento)
        dsDatos.Tables.Add(dtMovimiento)

        Return dsDatos

    End Function

    Private Sub PrioridadAjustesAlmacenarPendiente(ByVal group_nbr As Integer, ByVal seq_nbr As String, ByVal cantidadPendiente As Integer)

        Dim sqlComando As SqlCommand = New SqlCommand

        sqlComando.CommandTimeout = 3600
        sqlComando.Connection = sqlConexion
        sqlComando.CommandType = CommandType.StoredProcedure
        sqlComando.CommandText = "sp_WMS_PrioridadAjustesAlmacenarPendiente"
        sqlComando.Parameters.AddWithValue("@group_nbr", group_nbr)
        sqlComando.Parameters.AddWithValue("@seq_nbr", seq_nbr)
        sqlComando.Parameters.AddWithValue("@cantidadPendiente", cantidadPendiente)

        Try
            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            sqlConexion.Close()
        End Try
    End Sub





End Class
