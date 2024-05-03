Imports System.Data.SqlClient

Public Class clsHistorialInventario

    Public Sub guardarHistorialInventarioEncabezado(byref idHistorial As Integer)

        Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)

        Try
            Dim sqlComando As SqlCommand = New SqlCommand
            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = "sp_WMS_IHT_HISTORIAL_INVENTARIO_ENCABEZADO_Guardar"
            sqlComando.CommandTimeout = 180000

            Dim sqlParametro As New SqlParameter
            sqlParametro.Direction = ParameterDirection.Output
            sqlParametro.ParameterName = "idHistorial"
            sqlParametro.Value = 0
            sqlComando.Parameters.Add(sqlParametro)

            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

            idHistorial = sqlComando.Parameters(0).Value
        Catch ex As Exception
            Throw ex
        Finally
            sqlConexion.Close()
        End Try
    End Sub

    Public Sub guardarHistorialInventario(
        ByVal idHistorial As Integer,
        ByVal group_nbr As String,
        ByVal seq_nbr As String,
        ByVal facility_code As String,
        ByVal company_code As String,
        ByVal activity_code As String,
        ByVal reason_code As String,
        ByVal lock_code As String,
        ByVal lpn_nbr As String,
        ByVal location As String,
        ByVal item_code As String,
        ByVal item_alternate_code As String,
        ByVal item_part_a As String,
        ByVal item_part_b As String,
        ByVal item_part_c As String,
        ByVal item_part_d As String,
        ByVal item_part_e As String,
        ByVal item_part_f As String,
        ByVal item_description As String,
        ByVal shipment_nbr As String,
        ByVal trailer_nbr As String,
        ByVal po_nbr As String,
        ByVal po_line_nbr As String,
        ByVal vendor_code As String,
        ByVal order_nbr As String,
        ByVal order_seq_nbr As String,
        ByVal to_facility_code As String,
        ByVal orig_qty As String,
        ByVal adj_qty As String,
        ByVal lpns_shipped As String,
        ByVal units_shipped As String,
        ByVal lpns_received As String,
        ByVal units_received As String,
        ByVal ref_code_1 As String,
        ByVal ref_value_1 As String,
        ByVal ref_code_2 As String,
        ByVal ref_value_2 As String,
        ByVal ref_code_3 As String,
        ByVal ref_value_3 As String,
        ByVal ref_code_4 As String,
        ByVal ref_value_4 As String,
        ByVal ref_code_5 As String,
        ByVal ref_value_5 As String,
        ByVal create_date As String,
        ByVal invn_attr_a As String,
        ByVal invn_attr_b As String,
        ByVal invn_attr_c As String,
        ByVal shipment_line_nbr As String,
        ByVal serial_nbr As String,
        ByVal invn_attr_d As String,
        ByVal invn_attr_e As String,
        ByVal invn_attr_f As String,
        ByVal invn_attr_g As String,
        ByVal work_order_nbr As String,
        ByVal work_order_seq_nbr As String,
        ByVal screen_name As String,
        ByVal module_name As String,
        ByVal ref_code_6 As String,
        ByVal ref_value_6 As String,
        ByVal ref_code_7 As String,
        ByVal ref_value_7 As String,
        ByVal ref_code_8 As String,
        ByVal ref_value_8 As String,
        ByVal ref_code_9 As String,
        ByVal ref_value_9 As String,
        ByVal ref_code_10 As String,
        ByVal ref_value_10 As String,
        ByVal ref_code_11 As String,
        ByVal ref_value_11 As String,
        ByVal ref_code_12 As String,
        ByVal ref_value_12 As String
        )

        Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)

        Try
            Dim sqlComando As SqlCommand = New SqlCommand
            Dim dsResultado As New DataSet

            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = "sp_WMS_IHT_HISTORIAL_INVENTARIO_Guardar"
            sqlComando.CommandTimeout = 180000


            sqlComando.Parameters.AddWithValue("@idHistorial", idHistorial)

            If group_nbr <> "" Then
                sqlComando.Parameters.AddWithValue("@group_nbr", group_nbr)
            End If
            If seq_nbr <> "" Then
                sqlComando.Parameters.AddWithValue("@seq_nbr", seq_nbr)
            End If
            sqlComando.Parameters.AddWithValue("facility_code", facility_code)
            sqlComando.Parameters.AddWithValue("company_code", company_code)
            If activity_code <> "" Then
                sqlComando.Parameters.AddWithValue("@activity_code", activity_code)
            End If
            sqlComando.Parameters.AddWithValue("reason_code", reason_code)
            sqlComando.Parameters.AddWithValue("lock_code", lock_code)
            sqlComando.Parameters.AddWithValue("lpn_nbr", lpn_nbr)
            sqlComando.Parameters.AddWithValue("location", location)
            sqlComando.Parameters.AddWithValue("item_code", item_code)
            sqlComando.Parameters.AddWithValue("item_alternate_code", item_alternate_code)
            sqlComando.Parameters.AddWithValue("item_part_a", item_part_a)
            sqlComando.Parameters.AddWithValue("item_part_b", item_part_b)
            sqlComando.Parameters.AddWithValue("item_part_c", item_part_c)
            sqlComando.Parameters.AddWithValue("item_part_d", item_part_d)
            sqlComando.Parameters.AddWithValue("item_part_e", item_part_e)
            sqlComando.Parameters.AddWithValue("item_part_f", item_part_f)
            sqlComando.Parameters.AddWithValue("item_description", item_description)
            sqlComando.Parameters.AddWithValue("shipment_nbr", shipment_nbr)
            sqlComando.Parameters.AddWithValue("trailer_nbr", trailer_nbr)
            sqlComando.Parameters.AddWithValue("po_nbr", po_nbr)
            If po_line_nbr <> "" Then
                sqlComando.Parameters.AddWithValue("@po_line_nbr", po_line_nbr)
            End If
            sqlComando.Parameters.AddWithValue("vendor_code", vendor_code)
            sqlComando.Parameters.AddWithValue("order_nbr", order_nbr)
            If order_seq_nbr <> "" Then
                sqlComando.Parameters.AddWithValue("@order_seq_nbr", order_seq_nbr)
            End If
            sqlComando.Parameters.AddWithValue("to_facility_code", to_facility_code)
            If orig_qty <> "" Then
                sqlComando.Parameters.AddWithValue("@orig_qty", orig_qty)
            End If
            If adj_qty <> "" Then
                sqlComando.Parameters.AddWithValue("@adj_qty", adj_qty)
            End If
            If lpns_shipped <> "" Then
                sqlComando.Parameters.AddWithValue("@lpns_shipped", lpns_shipped)
            End If
            If units_shipped <> "" Then
                sqlComando.Parameters.AddWithValue("@units_shipped", units_shipped)
            End If
            If lpns_received <> "" Then
                sqlComando.Parameters.AddWithValue("@lpns_received", lpns_received)
            End If
            If units_received <> "" Then
                sqlComando.Parameters.AddWithValue("@units_received", units_received)
            End If
            sqlComando.Parameters.AddWithValue("ref_code_1", ref_code_1)
            sqlComando.Parameters.AddWithValue("ref_value_1", ref_value_1)
            sqlComando.Parameters.AddWithValue("ref_code_2", ref_code_2)
            sqlComando.Parameters.AddWithValue("ref_value_2", ref_value_2)
            sqlComando.Parameters.AddWithValue("ref_code_3", ref_code_3)
            sqlComando.Parameters.AddWithValue("ref_value_3", ref_value_3)
            sqlComando.Parameters.AddWithValue("ref_code_4", ref_code_4)
            sqlComando.Parameters.AddWithValue("ref_value_4", ref_value_4)
            sqlComando.Parameters.AddWithValue("ref_code_5", ref_code_5)
            sqlComando.Parameters.AddWithValue("ref_value_5", ref_value_5)
            If create_date <> "" Then
                sqlComando.Parameters.AddWithValue("@create_date", create_date.Replace("000000", ""))
            End If
            sqlComando.Parameters.AddWithValue("invn_attr_a", invn_attr_a)
            sqlComando.Parameters.AddWithValue("invn_attr_b", invn_attr_b)
            sqlComando.Parameters.AddWithValue("invn_attr_c", invn_attr_c)
            If shipment_line_nbr <> "" Then
                sqlComando.Parameters.AddWithValue("@shipment_line_nbr", shipment_line_nbr)
            End If
            sqlComando.Parameters.AddWithValue("serial_nbr", serial_nbr)
            sqlComando.Parameters.AddWithValue("invn_attr_d", invn_attr_d)
            sqlComando.Parameters.AddWithValue("invn_attr_e", invn_attr_e)
            sqlComando.Parameters.AddWithValue("invn_attr_f", invn_attr_f)
            sqlComando.Parameters.AddWithValue("invn_attr_g", invn_attr_g)
            sqlComando.Parameters.AddWithValue("work_order_nbr", work_order_nbr)
            If work_order_seq_nbr <> "" Then
                sqlComando.Parameters.AddWithValue("@work_order_seq_nbr", work_order_seq_nbr)
            End If
            sqlComando.Parameters.AddWithValue("screen_name", screen_name)
            sqlComando.Parameters.AddWithValue("module_name", module_name)
            sqlComando.Parameters.AddWithValue("ref_code_6", ref_code_6)
            sqlComando.Parameters.AddWithValue("ref_value_6", ref_value_6)
            sqlComando.Parameters.AddWithValue("ref_code_7", ref_code_7)
            sqlComando.Parameters.AddWithValue("ref_value_7", ref_value_7)
            sqlComando.Parameters.AddWithValue("ref_code_8", ref_code_8)
            sqlComando.Parameters.AddWithValue("ref_value_8", ref_value_8)
            sqlComando.Parameters.AddWithValue("ref_code_9", ref_code_9)
            sqlComando.Parameters.AddWithValue("ref_value_9", ref_value_9)
            sqlComando.Parameters.AddWithValue("ref_code_10", ref_code_10)
            sqlComando.Parameters.AddWithValue("ref_value_10", ref_value_10)
            sqlComando.Parameters.AddWithValue("ref_code_11", ref_code_11)
            sqlComando.Parameters.AddWithValue("ref_value_11", ref_value_11)
            sqlComando.Parameters.AddWithValue("ref_code_12", ref_code_12)
            sqlComando.Parameters.AddWithValue("ref_value_12", ref_value_12)


            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            sqlConexion.Close()
        End Try

    End Sub

End Class
