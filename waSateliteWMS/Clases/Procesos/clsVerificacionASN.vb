Imports System.Data.SqlClient
Public Class clsVerificacionASN
    Inherits clsConfiguracion

    Public Sub guardarEncabezadoVerificacionASN(
        ByVal hdr_group_nbr As String,
        ByVal shipment_nbr As String,
        ByVal facility_code As String,
        ByVal company_code As String,
        ByVal trailer_nbr As String,
        ByVal ref_nbr As String,
        ByVal shipment_type As String,
        ByVal load_nbr As String,
        ByVal manifest_nbr As String,
        ByVal trailer_type As String,
        ByVal vendor_info As String,
        ByVal origin_info As String,
        ByVal origin_code As String,
        ByVal orig_shipped_units As String,
        ByVal shipped_date As String,  'date
        ByVal orig_shipped_lpns As String,
        ByVal shipment_hdr_cust_field_1 As String,
        ByVal shipment_hdr_cust_field_2 As String,
        ByVal shipment_hdr_cust_field_3 As String,
        ByVal shipment_hdr_cust_field_4 As String,
        ByVal shipment_hdr_cust_field_5 As String,
        ByVal verification_date As String,
        ByRef idVerificacionASN As Integer
        )

        Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)

        Try

            Dim sqlComando As SqlCommand = New SqlCommand
            Dim dsResultado As New DataSet

            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = "sp_WMS_SVS_CABECERA_VERIFICACION_ASN_Guardar"
            sqlComando.CommandTimeout = 180000

            sqlComando.Parameters.AddWithValue("@hdr_group_nbr", hdr_group_nbr)
            sqlComando.Parameters.AddWithValue("@shipment_nbr", shipment_nbr)
            sqlComando.Parameters.AddWithValue("@facility_code", facility_code)
            sqlComando.Parameters.AddWithValue("@company_code", company_code)
            sqlComando.Parameters.AddWithValue("@trailer_nbr", trailer_nbr)
            sqlComando.Parameters.AddWithValue("@ref_nbr", ref_nbr)
            sqlComando.Parameters.AddWithValue("@shipment_type", shipment_type)
            sqlComando.Parameters.AddWithValue("@load_nbr", load_nbr)
            sqlComando.Parameters.AddWithValue("@manifest_nbr", manifest_nbr)
            sqlComando.Parameters.AddWithValue("@trailer_type", trailer_type)
            sqlComando.Parameters.AddWithValue("@vendor_info", vendor_info)
            sqlComando.Parameters.AddWithValue("@origin_info", origin_info)
            sqlComando.Parameters.AddWithValue("@origin_code", origin_code)
            If orig_shipped_units <> "" Then
                sqlComando.Parameters.AddWithValue("@orig_shipped_units", orig_shipped_units)
            End If
            If shipped_date <> "" Then
                sqlComando.Parameters.AddWithValue("@shipped_date", shipped_date.Replace("000000", ""))
            End If
            If orig_shipped_lpns <> "" Then
                sqlComando.Parameters.AddWithValue("@orig_shipped_lpns", orig_shipped_lpns)
            End If
            sqlComando.Parameters.AddWithValue("@shipment_hdr_cust_field_1", shipment_hdr_cust_field_1)
            sqlComando.Parameters.AddWithValue("@shipment_hdr_cust_field_2", shipment_hdr_cust_field_2)
            sqlComando.Parameters.AddWithValue("@shipment_hdr_cust_field_3", shipment_hdr_cust_field_3)
            sqlComando.Parameters.AddWithValue("@shipment_hdr_cust_field_4", shipment_hdr_cust_field_4)
            sqlComando.Parameters.AddWithValue("@shipment_hdr_cust_field_5", shipment_hdr_cust_field_5)
            If verification_date <> "" Then
                sqlComando.Parameters.AddWithValue("@verification_date", verification_date.Replace("000000", ""))
            End If

            Dim pidDespacho As New SqlParameter
            pidDespacho.Direction = ParameterDirection.Output
            pidDespacho.ParameterName = "idVerificacionASN"
            pidDespacho.Value = 0
            sqlComando.Parameters.Add(pidDespacho)

            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

            idVerificacionASN = sqlComando.Parameters(22).Value

        Catch ex As Exception
            Throw ex
        Finally
            sqlConexion.Close()
        End Try
    End Sub

    Public Sub guardarDetalleVerificacionASN(
        ByVal idVerificacionASN As Integer,
        ByVal hdr_group_nbr As String,
        ByVal seq_nbr As String,
        ByVal lpn_nbr As String,
        ByVal lpn_weight As String,
        ByVal lpn_volume As String,
        ByVal item_alternate_code As String,
        ByVal item_part_a As String,
        ByVal item_part_b As String,
        ByVal item_part_c As String,
        ByVal item_part_d As String,
        ByVal item_part_e As String,
        ByVal item_part_f As String,
        ByVal pre_pack_code As String,
        ByVal pre_pack_ratio As String,
        ByVal pre_pack_ratio_seq As String,
        ByVal pre_pack_total_units As String,
        ByVal invn_attr_a As String,
        ByVal invn_attr_b As String,
        ByVal invn_attr_c As String,
        ByVal shipped_qty As String,
        ByVal priority_date As String, ' date
        ByVal po_nbr As String,
        ByVal pallet_nbr As String,
        ByVal putaway_type As String,
        ByVal received_qty As String,
        ByVal expiry_date As String, ' date
        ByVal batch_nbr As String,
        ByVal rcv_xdock_facility_code As String,
        ByVal shipment_dtl_cust_field_1 As String,
        ByVal shipment_dtl_cust_field_2 As String,
        ByVal shipment_dtl_cust_field_3 As String,
        ByVal shipment_dtl_cust_field_4 As String,
        ByVal shipment_dtl_cust_field_5 As String,
        ByVal lpn_is_physical_pallet_flg As String,
        ByVal po_seq_nbr As String,
        ByVal lock_code As String,
        ByVal serial_nbr As String,
        ByVal invn_attr_d As String,
        ByVal invn_attr_e As String,
        ByVal invn_attr_f As String,
        ByVal invn_attr_g As String,
        ByVal rcvd_trailer_nbr As String,
        ByVal po_dtl_line_schedule_nbrs As String
        )

        Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)

        Try

            Dim sqlComando As SqlCommand = New SqlCommand
            Dim dsResultado As New DataSet

            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = "sp_WMS_SVS_DETALLE_VERIFICACION_ASN_Guardar"
            sqlComando.CommandTimeout = 180000

            sqlComando.Parameters.AddWithValue("@idVerificacionASN", idVerificacionASN)

            sqlComando.Parameters.AddWithValue("@hdr_group_nbr", hdr_group_nbr)
            If seq_nbr <> "" Then
                sqlComando.Parameters.AddWithValue("@seq_nbr", seq_nbr)
            End If
            sqlComando.Parameters.AddWithValue("@lpn_nbr", lpn_nbr)
            If lpn_weight <> "" Then
                sqlComando.Parameters.AddWithValue("@lpn_weight", lpn_weight)
            End If
            If lpn_volume <> "" Then
                sqlComando.Parameters.AddWithValue("@lpn_volume", lpn_volume)
            End If
            sqlComando.Parameters.AddWithValue("@item_alternate_code", item_alternate_code)
            sqlComando.Parameters.AddWithValue("@item_part_a", item_part_a)
            sqlComando.Parameters.AddWithValue("@item_part_b", item_part_b)
            sqlComando.Parameters.AddWithValue("@item_part_c", item_part_c)
            sqlComando.Parameters.AddWithValue("@item_part_d", item_part_d)
            sqlComando.Parameters.AddWithValue("@item_part_e", item_part_e)
            sqlComando.Parameters.AddWithValue("@item_part_f ", item_part_f)
            sqlComando.Parameters.AddWithValue("@pre_pack_code", pre_pack_code)
            If pre_pack_ratio <> "" Then
                sqlComando.Parameters.AddWithValue("@pre_pack_ratio", pre_pack_ratio)
            End If
            If pre_pack_ratio_seq <> "" Then
                sqlComando.Parameters.AddWithValue("@pre_pack_ratio_seq", pre_pack_ratio_seq)
            End If
            If pre_pack_total_units <> "" Then
                sqlComando.Parameters.AddWithValue("@pre_pack_total_units", pre_pack_total_units)
            End If
            sqlComando.Parameters.AddWithValue("@invn_attr_a", invn_attr_a)
            sqlComando.Parameters.AddWithValue("@invn_attr_b", invn_attr_b)
            sqlComando.Parameters.AddWithValue("@invn_attr_c ", invn_attr_c)
            If shipped_qty <> "" Then
                sqlComando.Parameters.AddWithValue("@shipped_qty", shipped_qty)
            End If
            If priority_date <> "" Then
                sqlComando.Parameters.AddWithValue("@priority_date", priority_date.Replace("000000", ""))
            End If
            sqlComando.Parameters.AddWithValue("@po_nbr", po_nbr)
            sqlComando.Parameters.AddWithValue("@pallet_nbr ", pallet_nbr)
            sqlComando.Parameters.AddWithValue("@putaway_type", putaway_type)
            If received_qty <> "" Then
                sqlComando.Parameters.AddWithValue("@received_qty", received_qty)
            End If
            If expiry_date <> "" Then
                sqlComando.Parameters.AddWithValue("@expiry_date", expiry_date.Replace("000000", ""))
            End If
            sqlComando.Parameters.AddWithValue("@batch_nbr", batch_nbr)
            sqlComando.Parameters.AddWithValue("@rcv_xdock_facility_code", rcv_xdock_facility_code)
            sqlComando.Parameters.AddWithValue("@shipment_dtl_cust_field_1", shipment_dtl_cust_field_1)
            sqlComando.Parameters.AddWithValue("@shipment_dtl_cust_field_2", shipment_dtl_cust_field_2)
            sqlComando.Parameters.AddWithValue("@shipment_dtl_cust_field_3", shipment_dtl_cust_field_3)
            sqlComando.Parameters.AddWithValue("@shipment_dtl_cust_field_4", shipment_dtl_cust_field_4)
            sqlComando.Parameters.AddWithValue("@shipment_dtl_cust_field_5", shipment_dtl_cust_field_5)
            sqlComando.Parameters.AddWithValue("@lpn_is_physical_pallet_flg", lpn_is_physical_pallet_flg)
            If po_seq_nbr <> "" Then
                sqlComando.Parameters.AddWithValue("@po_seq_nbr", po_seq_nbr)
            End If
            sqlComando.Parameters.AddWithValue("@lock_code", lock_code)
            sqlComando.Parameters.AddWithValue("@serial_nbr", serial_nbr)
            sqlComando.Parameters.AddWithValue("@invn_attr_d", invn_attr_d)
            sqlComando.Parameters.AddWithValue("@invn_attr_e", invn_attr_e)
            sqlComando.Parameters.AddWithValue("@invn_attr_f", invn_attr_f)
            sqlComando.Parameters.AddWithValue("@invn_attr_g", invn_attr_g)
            sqlComando.Parameters.AddWithValue("@rcvd_trailer_nbr", rcvd_trailer_nbr)
            sqlComando.Parameters.AddWithValue("@po_dtl_line_schedule_nbrs", po_dtl_line_schedule_nbrs)

            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            sqlConexion.Close()
        End Try

    End Sub

    Public Sub guardarEncabezadoVerificacionDespachos(
                                                        ByVal hdr_group_nbr As String,
                                                        ByVal facility_code As String,
                                                        ByVal company_code As String,
                                                        ByVal action_code As String,
                                                        ByVal load_type As String,
                                                        ByVal load_manifest_nbr As String,
                                                        ByVal trailer_nbr As String,
                                                        ByVal trailer_type As String,
                                                        ByVal driver As String,
                                                        ByVal seal_nbr As String,
                                                        ByVal pro_nbr As String,
                                                        ByVal route_nbr As String,
                                                        ByVal freight_class As String,
                                                        ByVal hdr_bol_nbr As String,
                                                        ByVal total_nbr_of_oblpns As String,
                                                        ByVal total_weight As String,
                                                        ByVal total_volume As String,
                                                        ByVal total_shipping_charge As String,
                                                        ByVal ship_date As String,
                                                        ByVal sched_delivery_date As String,
                                                        ByVal carrier_code As String,
                                                        ByVal externally_planned_load_nbr As String,
                                                        ByRef idDespacho As Integer)

        Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)

        Try

            Dim sqlComando As SqlCommand = New SqlCommand
            Dim dsResultado As New DataSet

            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = "sp_WMS_SLS_CABECERA_DESPACHOS_Guardar"
            sqlComando.CommandTimeout = 180000

            sqlComando.Parameters.AddWithValue("@hdr_group_nbr", hdr_group_nbr)
            sqlComando.Parameters.AddWithValue("@facility_code", facility_code)
            sqlComando.Parameters.AddWithValue("@company_code", company_code)
            sqlComando.Parameters.AddWithValue("@action_code", action_code)
            sqlComando.Parameters.AddWithValue("@load_type", load_type)
            sqlComando.Parameters.AddWithValue("@load_manifest_nbr", load_manifest_nbr)
            sqlComando.Parameters.AddWithValue("@trailer_nbr", trailer_nbr)
            sqlComando.Parameters.AddWithValue("@trailer_type", trailer_type)
            sqlComando.Parameters.AddWithValue("@driver", driver)
            sqlComando.Parameters.AddWithValue("@seal_nbr", seal_nbr)
            sqlComando.Parameters.AddWithValue("@pro_nbr", pro_nbr)
            sqlComando.Parameters.AddWithValue("@route_nbr", route_nbr)
            sqlComando.Parameters.AddWithValue("@freight_class", freight_class)
            sqlComando.Parameters.AddWithValue("@hdr_bol_nbr", hdr_bol_nbr)
            If total_nbr_of_oblpns <> "" Then
                sqlComando.Parameters.AddWithValue("@total_nbr_of_oblpns", total_nbr_of_oblpns)
            End If
            If total_weight <> "" Then
                sqlComando.Parameters.AddWithValue("@total_weight", total_weight)
            End If
            If total_volume <> "" Then
                sqlComando.Parameters.AddWithValue("@total_volume", total_volume)
            End If
            If total_shipping_charge <> "" Then
                sqlComando.Parameters.AddWithValue("@total_shipping_charge", total_shipping_charge)
            End If
            If ship_date <> "" Then
                sqlComando.Parameters.AddWithValue("@ship_date", ship_date.Replace("000000", ""))
            End If
            If sched_delivery_date <> "" Then
                sqlComando.Parameters.AddWithValue("@sched_delivery_date", sched_delivery_date.Replace("000000", ""))
            End If
            sqlComando.Parameters.AddWithValue("@carrier_code", carrier_code)
            sqlComando.Parameters.AddWithValue("@externally_planned_load_nbr", externally_planned_load_nbr)

            Dim pidDespacho As New SqlParameter
            pidDespacho.Direction = ParameterDirection.Output
            pidDespacho.ParameterName = "idDespacho"
            pidDespacho.Value = 0

            sqlComando.Parameters.Add(pidDespacho)

            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

            idDespacho = sqlComando.Parameters(21).Value

        Catch ex As Exception
            Throw ex
        Finally
            sqlConexion.Close()
        End Try
    End Sub

    Public Sub guardarDetalleVerificacionDespachos(
                                                    ByVal idDespacho As Integer,
                                                    ByVal hdr_group_nbr As String,
                                                    ByVal line_nbr As String,
                                                    ByVal seq_nbr As String,
                                                    ByVal stop_shipment_nbr As String,
                                                    ByVal stop_bol_nbr As String,
                                                    ByVal stop_nbr_of_oblpns As String,
                                                    ByVal stop_weight As String,
                                                    ByVal stop_volume As String,
                                                    ByVal stop_shipping_charge As String,
                                                    ByVal shipto_facility_code As String,
                                                    ByVal shipto_name As String,
                                                    ByVal shipto_addr As String,
                                                    ByVal shipto_addr2 As String,
                                                    ByVal shipto_addr3 As String,
                                                    ByVal shipto_city As String,
                                                    ByVal shipto_state As String,
                                                    ByVal shipto_zip As String,
                                                    ByVal shipto_country As String,
                                                    ByVal shipto_phone_nbr As String,
                                                    ByVal shipto_email As String,
                                                    ByVal shipto_contact As String,
                                                    ByVal dest_facility_code As String,
                                                    ByVal cust_name As String,
                                                    ByVal cust_addr As String,
                                                    ByVal cust_addr2 As String,
                                                    ByVal cust_addr3 As String,
                                                    ByVal cust_city As String,
                                                    ByVal cust_state As String,
                                                    ByVal cust_zip As String,
                                                    ByVal cust_country As String,
                                                    ByVal cust_phone_nbr As String,
                                                    ByVal cust_email As String,
                                                    ByVal cust_contact As String,
                                                    ByVal cust_nbr As String,
                                                    ByVal order_nbr As String,
                                                    ByVal ord_date As String,
                                                    ByVal exp_date As String,
                                                    ByVal req_ship_date As String,
                                                    ByVal start_ship_date As String,
                                                    ByVal stop_ship_date As String,
                                                    ByVal host_allocation_nbr As String,
                                                    ByVal customer_po_nbr As String,
                                                    ByVal sales_order_nbr As String,
                                                    ByVal sales_channel As String,
                                                    ByVal dest_dept_nbr As String,
                                                    ByVal order_hdr_cust_field_1 As String,
                                                    ByVal order_hdr_cust_field_2 As String,
                                                    ByVal order_hdr_cust_field_3 As String,
                                                    ByVal order_hdr_cust_field_4 As String,
                                                    ByVal order_hdr_cust_field_5 As String,
                                                    ByVal order_seq_nbr As String,
                                                    ByVal order_dtl_cust_field_1 As String,
                                                    ByVal order_dtl_cust_field_2 As String,
                                                    ByVal order_dtl_cust_field_3 As String,
                                                    ByVal order_dtl_cust_field_4 As String,
                                                    ByVal order_dtl_cust_field_5 As String,
                                                    ByVal ob_lpn_nbr As String,
                                                    ByVal item_alternate_code As String,
                                                    ByVal item_part_a As String,
                                                    ByVal item_part_b As String,
                                                    ByVal item_part_c As String,
                                                    ByVal item_part_d As String,
                                                    ByVal item_part_e As String,
                                                    ByVal item_part_f As String,
                                                    ByVal pre_pack_code As String,
                                                    ByVal pre_pack_ratio As String,
                                                    ByVal pre_pack_ratio_seq As String,
                                                    ByVal pre_pack_total_units As String,
                                                    ByVal invn_attr_a As String,
                                                    ByVal invn_attr_b As String,
                                                    ByVal invn_attr_c As String,
                                                    ByVal hazmat As String,
                                                    ByVal shipped_uom As String,
                                                    ByVal shipped_qty As String,
                                                    ByVal pallet_nbr As String,
                                                    ByVal dest_company_code As String,
                                                    ByVal batch_nbr As String,
                                                    ByVal expiry_date As String,
                                                    ByVal tracking_nbr As String,
                                                    ByVal master_tracking_nbr As String,
                                                    ByVal package_type As String,
                                                    ByVal payment_method As String,
                                                    ByVal carrier_account_nbr As String,
                                                    ByVal ship_via_code As String,
                                                    ByVal ob_lpn_weight As String,
                                                    ByVal ob_lpn_volume As String,
                                                    ByVal ob_lpn_shipping_charge As String,
                                                    ByVal ob_lpn_type As String,
                                                    ByVal asset_nbr As String,
                                                    ByVal asset_seal_nbr As String,
                                                    ByVal serial_nbr As String,
                                                    ByVal customer_po_type As String,
                                                    ByVal customer_vendor_code As String,
                                                    ByVal order_hdr_cust_date_1 As String,
                                                    ByVal order_hdr_cust_date_2 As String,
                                                    ByVal order_hdr_cust_date_3 As String,
                                                    ByVal order_hdr_cust_date_4 As String,
                                                    ByVal order_hdr_cust_date_5 As String,
                                                    ByVal order_hdr_cust_number_1 As String,
                                                    ByVal order_hdr_cust_number_2 As String,
                                                    ByVal order_hdr_cust_number_3 As String,
                                                    ByVal order_hdr_cust_number_4 As String,
                                                    ByVal order_hdr_cust_number_5 As String,
                                                    ByVal order_hdr_cust_decimal_1 As String,
                                                    ByVal order_hdr_cust_decimal_2 As String,
                                                    ByVal order_hdr_cust_decimal_3 As String,
                                                    ByVal order_hdr_cust_decimal_4 As String,
                                                    ByVal order_hdr_cust_decimal_5 As String,
                                                    ByVal order_hdr_cust_short_text_1 As String,
                                                    ByVal order_hdr_cust_short_text_2 As String,
                                                    ByVal order_hdr_cust_short_text_3 As String,
                                                    ByVal order_hdr_cust_short_text_4 As String,
                                                    ByVal order_hdr_cust_short_text_5 As String,
                                                    ByVal order_hdr_cust_short_text_6 As String,
                                                    ByVal order_hdr_cust_short_text_7 As String,
                                                    ByVal order_hdr_cust_short_text_8 As String,
                                                    ByVal order_hdr_cust_short_text_9 As String,
                                                    ByVal order_hdr_cust_short_text_10 As String,
                                                    ByVal order_hdr_cust_short_text_11 As String,
                                                    ByVal order_hdr_cust_short_text_12 As String,
                                                    ByVal order_hdr_cust_long_text_1 As String,
                                                    ByVal order_hdr_cust_long_text_2 As String,
                                                    ByVal order_hdr_cust_long_text_3 As String,
                                                    ByVal order_dtl_cust_date_1 As String,
                                                    ByVal order_dtl_cust_date_2 As String,
                                                    ByVal order_dtl_cust_date_3 As String,
                                                    ByVal order_dtl_cust_date_4 As String,
                                                    ByVal order_dtl_cust_date_5 As String,
                                                    ByVal order_dtl_cust_number_1 As String,
                                                    ByVal order_dtl_cust_number_2 As String,
                                                    ByVal order_dtl_cust_number_3 As String,
                                                    ByVal order_dtl_cust_number_4 As String,
                                                    ByVal order_dtl_cust_number_5 As String,
                                                    ByVal order_dtl_cust_decimal_1 As String,
                                                    ByVal order_dtl_cust_decimal_2 As String,
                                                    ByVal order_dtl_cust_decimal_3 As String,
                                                    ByVal order_dtl_cust_decimal_4 As String,
                                                    ByVal order_dtl_cust_decimal_5 As String,
                                                    ByVal order_dtl_cust_short_text_1 As String,
                                                    ByVal order_dtl_cust_short_text_2 As String,
                                                    ByVal order_dtl_cust_short_text_3 As String,
                                                    ByVal order_dtl_cust_short_text_4 As String,
                                                    ByVal order_dtl_cust_short_text_5 As String,
                                                    ByVal order_dtl_cust_short_text_6 As String,
                                                    ByVal order_dtl_cust_short_text_7 As String,
                                                    ByVal order_dtl_cust_short_text_8 As String,
                                                    ByVal order_dtl_cust_short_text_9 As String,
                                                    ByVal order_dtl_cust_short_text_10 As String,
                                                    ByVal order_dtl_cust_short_text_11 As String,
                                                    ByVal order_dtl_cust_short_text_12 As String,
                                                    ByVal order_dtl_cust_long_text_1 As String,
                                                    ByVal order_dtl_cust_long_text_2 As String,
                                                    ByVal order_dtl_cust_long_text_3 As String,
                                                    ByVal invn_attr_d As String,
                                                    ByVal invn_attr_e As String,
                                                    ByVal invn_attr_f As String,
                                                    ByVal invn_attr_g As String,
                                                    ByVal order_type As String,
                                                    ByVal rcvd_trailer_nbr As String,
                                                    ByVal stop_seal_nbr As String,
                                                    ByVal ship_request_line As String)

        Dim sqlConexion As New SqlConnection(My.Settings.strConexionGT)

        Try

            Dim sqlComando As SqlCommand = New SqlCommand
            Dim dsResultado As New DataSet

            sqlComando.Connection = sqlConexion
            sqlComando.CommandType = CommandType.StoredProcedure
            sqlComando.CommandText = "sp_WMS_SLS_DETALLE_DESPACHOS_Guardar"
            sqlComando.CommandTimeout = 180000

            sqlComando.Parameters.AddWithValue("@idDespacho", idDespacho)

            sqlComando.Parameters.AddWithValue("@hdr_group_nbr", hdr_group_nbr)
            If line_nbr <> "" Then
                sqlComando.Parameters.AddWithValue("@line_nbr", line_nbr)
            End If
            If seq_nbr <> "" Then
                sqlComando.Parameters.AddWithValue("@seq_nbr", seq_nbr)
            End If
            sqlComando.Parameters.AddWithValue("@stop_shipment_nbr", stop_shipment_nbr)
            If stop_bol_nbr <> "" Then
                sqlComando.Parameters.AddWithValue("@stop_bol_nbr", stop_bol_nbr)
            End If
            If stop_nbr_of_oblpns <> "" Then
                sqlComando.Parameters.AddWithValue("@stop_nbr_of_oblpns", stop_nbr_of_oblpns)
            End If
            If stop_weight <> "" Then
                sqlComando.Parameters.AddWithValue("@stop_weight", stop_weight)
            End If
            If stop_volume <> "" Then
                sqlComando.Parameters.AddWithValue("@stop_volume", stop_volume)
            End If
            If stop_shipping_charge <> "" Then
                sqlComando.Parameters.AddWithValue("@stop_shipping_charge", stop_shipping_charge)
            End If
            sqlComando.Parameters.AddWithValue("@shipto_facility_code", shipto_facility_code)
            sqlComando.Parameters.AddWithValue("@shipto_name", shipto_name)
            sqlComando.Parameters.AddWithValue("@shipto_addr", shipto_addr)
            sqlComando.Parameters.AddWithValue("@shipto_addr2", shipto_addr2)
            sqlComando.Parameters.AddWithValue("@shipto_addr3", shipto_addr3)
            sqlComando.Parameters.AddWithValue("@shipto_city", shipto_city)
            sqlComando.Parameters.AddWithValue("@shipto_state", shipto_state)
            sqlComando.Parameters.AddWithValue("@shipto_zip", shipto_zip)
            sqlComando.Parameters.AddWithValue("@shipto_country", shipto_country)
            sqlComando.Parameters.AddWithValue("@shipto_phone_nbr", shipto_phone_nbr)
            sqlComando.Parameters.AddWithValue("@shipto_email", shipto_email)
            sqlComando.Parameters.AddWithValue("@shipto_contact", shipto_contact)
            sqlComando.Parameters.AddWithValue("@dest_facility_code", dest_facility_code)
            sqlComando.Parameters.AddWithValue("@cust_name", cust_name)
            sqlComando.Parameters.AddWithValue("@cust_addr", cust_addr)
            sqlComando.Parameters.AddWithValue("@cust_addr2", cust_addr2)
            sqlComando.Parameters.AddWithValue("@cust_addr3", cust_addr3)
            sqlComando.Parameters.AddWithValue("@cust_city", cust_city)
            sqlComando.Parameters.AddWithValue("@cust_state", cust_state)
            sqlComando.Parameters.AddWithValue("@cust_zip", cust_zip)
            sqlComando.Parameters.AddWithValue("@cust_country", cust_country)
            sqlComando.Parameters.AddWithValue("@cust_phone_nbr", cust_phone_nbr)
            sqlComando.Parameters.AddWithValue("@cust_email", cust_email)
            sqlComando.Parameters.AddWithValue("@cust_contact", cust_contact)
            sqlComando.Parameters.AddWithValue("@cust_nbr", cust_nbr)
            sqlComando.Parameters.AddWithValue("@order_nbr", order_nbr)
            If ord_date <> "" Then
                sqlComando.Parameters.AddWithValue("@ord_date", ord_date.Replace("000000", ""))
            End If
            If exp_date <> "" Then
                sqlComando.Parameters.AddWithValue("@exp_date", exp_date.Replace("000000", ""))
            End If
            If req_ship_date <> "" Then
                sqlComando.Parameters.AddWithValue("@req_ship_date", req_ship_date.Replace("000000", ""))
            End If
            If start_ship_date <> "" Then
                sqlComando.Parameters.AddWithValue("@start_ship_date", start_ship_date.Replace("000000", ""))
            End If
            If stop_ship_date <> "" Then
                sqlComando.Parameters.AddWithValue("@stop_ship_date", stop_ship_date.Replace("000000", ""))
            End If
            sqlComando.Parameters.AddWithValue("@host_allocation_nbr", host_allocation_nbr)
            sqlComando.Parameters.AddWithValue("@customer_po_nbr", customer_po_nbr)
            sqlComando.Parameters.AddWithValue("@sales_order_nbr", sales_order_nbr)
            sqlComando.Parameters.AddWithValue("@sales_channel", sales_channel)
            sqlComando.Parameters.AddWithValue("@dest_dept_nbr", dest_dept_nbr)
            sqlComando.Parameters.AddWithValue("@order_hdr_cust_field_1", order_hdr_cust_field_1)
            sqlComando.Parameters.AddWithValue("@order_hdr_cust_field_2", order_hdr_cust_field_2)
            sqlComando.Parameters.AddWithValue("@order_hdr_cust_field_3", order_hdr_cust_field_3)
            sqlComando.Parameters.AddWithValue("@order_hdr_cust_field_4", order_hdr_cust_field_4)
            sqlComando.Parameters.AddWithValue("@order_hdr_cust_field_5", order_hdr_cust_field_5)
            If order_seq_nbr <> "" Then
                sqlComando.Parameters.AddWithValue("@order_seq_nbr", order_seq_nbr)
            End If
            sqlComando.Parameters.AddWithValue("@order_dtl_cust_field_1", order_dtl_cust_field_1)
            sqlComando.Parameters.AddWithValue("@order_dtl_cust_field_2", order_dtl_cust_field_2)
            sqlComando.Parameters.AddWithValue("@order_dtl_cust_field_3", order_dtl_cust_field_3)
            sqlComando.Parameters.AddWithValue("@order_dtl_cust_field_4", order_dtl_cust_field_4)
            sqlComando.Parameters.AddWithValue("@order_dtl_cust_field_5", order_dtl_cust_field_5)
            sqlComando.Parameters.AddWithValue("@ob_lpn_nbr", ob_lpn_nbr)
            sqlComando.Parameters.AddWithValue("@item_alternate_code", item_alternate_code)
            sqlComando.Parameters.AddWithValue("@item_part_a", item_part_a)
            sqlComando.Parameters.AddWithValue("@item_part_b", item_part_b)
            sqlComando.Parameters.AddWithValue("@item_part_c", item_part_c)
            sqlComando.Parameters.AddWithValue("@item_part_d", item_part_d)
            sqlComando.Parameters.AddWithValue("@item_part_e", item_part_e)
            sqlComando.Parameters.AddWithValue("@item_part_f", item_part_f)
            sqlComando.Parameters.AddWithValue("@pre_pack_code", pre_pack_code)
            If pre_pack_ratio <> "" Then
                sqlComando.Parameters.AddWithValue("@pre_pack_ratio", pre_pack_ratio)
            End If
            If pre_pack_ratio_seq <> "" Then
                sqlComando.Parameters.AddWithValue("@pre_pack_ratio_seq", pre_pack_ratio_seq)
            End If
            If pre_pack_total_units <> "" Then
                sqlComando.Parameters.AddWithValue("@pre_pack_total_units", pre_pack_total_units)
            End If
            sqlComando.Parameters.AddWithValue("@invn_attr_a", invn_attr_a)
            sqlComando.Parameters.AddWithValue("@invn_attr_b", invn_attr_b)
            sqlComando.Parameters.AddWithValue("@invn_attr_c", invn_attr_c)
            If hazmat <> "" Then
                sqlComando.Parameters.AddWithValue("@hazmat", hazmat)
            End If
            sqlComando.Parameters.AddWithValue("@shipped_uom", shipped_uom)
            If shipped_qty <> "" Then
                sqlComando.Parameters.AddWithValue("shipped_qty", shipped_qty)
            End If
            sqlComando.Parameters.AddWithValue("@pallet_nbr", pallet_nbr)
            sqlComando.Parameters.AddWithValue("@dest_company_code", dest_company_code)
            sqlComando.Parameters.AddWithValue("@batch_nbr", batch_nbr)
            If expiry_date <> "" Then
                sqlComando.Parameters.AddWithValue("@expiry_date", expiry_date.Replace("000000", ""))
            End If
            sqlComando.Parameters.AddWithValue("@tracking_nbr", tracking_nbr)
            sqlComando.Parameters.AddWithValue("@master_tracking_nbr", master_tracking_nbr)
            sqlComando.Parameters.AddWithValue("@package_type", package_type)
            sqlComando.Parameters.AddWithValue("@payment_method", payment_method)
            sqlComando.Parameters.AddWithValue("@carrier_account_nbr", carrier_account_nbr)
            sqlComando.Parameters.AddWithValue("@ship_via_code", ship_via_code)
            If ob_lpn_weight <> "" Then
                sqlComando.Parameters.AddWithValue("ob_lpn_weight", ob_lpn_weight)
            End If
            If ob_lpn_volume <> "" Then
                sqlComando.Parameters.AddWithValue("@ob_lpn_volume", ob_lpn_volume)
            End If
            If ob_lpn_shipping_charge <> "" Then
                sqlComando.Parameters.AddWithValue("@ob_lpn_shipping_charge", ob_lpn_shipping_charge)
            End If
            sqlComando.Parameters.AddWithValue("@ob_lpn_type", ob_lpn_type)
            sqlComando.Parameters.AddWithValue("@asset_nbr", asset_nbr)
            sqlComando.Parameters.AddWithValue("@asset_seal_nbr", asset_seal_nbr)
            sqlComando.Parameters.AddWithValue("@serial_nbr", serial_nbr)
            sqlComando.Parameters.AddWithValue("@customer_po_type", customer_po_type)
            sqlComando.Parameters.AddWithValue("@customer_vendor_code", customer_vendor_code)
            If order_hdr_cust_date_1 <> "" Then
                sqlComando.Parameters.AddWithValue("@order_hdr_cust_date_1", order_hdr_cust_date_1.Replace("000000", ""))
            End If
            If order_hdr_cust_date_2 <> "" Then
                sqlComando.Parameters.AddWithValue("@order_hdr_cust_date_2", order_hdr_cust_date_2.Replace("000000", ""))
            End If
            If order_hdr_cust_date_3 <> "" Then
                sqlComando.Parameters.AddWithValue("@order_hdr_cust_date_3", order_hdr_cust_date_3.Replace("000000", ""))
            End If
            If order_hdr_cust_date_4 <> "" Then
                sqlComando.Parameters.AddWithValue("@order_hdr_cust_date_4", order_hdr_cust_date_4.Replace("000000", ""))
            End If
            If order_hdr_cust_date_5 <> "" Then
                sqlComando.Parameters.AddWithValue("@order_hdr_cust_date_5", order_hdr_cust_date_5.Replace("000000", ""))
            End If
            If order_hdr_cust_number_1 <> "" Then
                sqlComando.Parameters.AddWithValue("order_hdr_cust_number_1", order_hdr_cust_number_1)
            End If
            If order_hdr_cust_number_2 <> "" Then
                sqlComando.Parameters.AddWithValue("@order_hdr_cust_number_2", order_hdr_cust_number_2)
            End If
            If order_hdr_cust_number_3 <> "" Then
                sqlComando.Parameters.AddWithValue("@order_hdr_cust_number_3", order_hdr_cust_number_3)
            End If
            If order_hdr_cust_number_4 <> "" Then
                sqlComando.Parameters.AddWithValue("order_hdr_cust_number_4", order_hdr_cust_number_4)
            End If
            If order_hdr_cust_number_5 <> "" Then
                sqlComando.Parameters.AddWithValue("order_hdr_cust_number_5", order_hdr_cust_number_5)
            End If
            If order_hdr_cust_decimal_1 <> "" Then
                sqlComando.Parameters.AddWithValue("@order_hdr_cust_decimal_1", order_hdr_cust_decimal_1)
            End If
            If order_hdr_cust_decimal_2 <> "" Then
                sqlComando.Parameters.AddWithValue("@order_hdr_cust_decimal_2", order_hdr_cust_decimal_2)
            End If
            If order_hdr_cust_decimal_3 <> "" Then
                sqlComando.Parameters.AddWithValue("order_hdr_cust_decimal_3", order_hdr_cust_decimal_3)
            End If
            If order_hdr_cust_decimal_4 <> "" Then
                sqlComando.Parameters.AddWithValue("@order_hdr_cust_decimal_4", order_hdr_cust_decimal_4)
            End If
            If order_hdr_cust_decimal_5 <> "" Then
                sqlComando.Parameters.AddWithValue("order_hdr_cust_decimal_5", order_hdr_cust_decimal_5)
            End If
            sqlComando.Parameters.AddWithValue("@order_hdr_cust_short_text_1", order_hdr_cust_short_text_1)
            sqlComando.Parameters.AddWithValue("@order_hdr_cust_short_text_2", order_hdr_cust_short_text_2)
            sqlComando.Parameters.AddWithValue("@order_hdr_cust_short_text_3", order_hdr_cust_short_text_3)
            sqlComando.Parameters.AddWithValue("@order_hdr_cust_short_text_4", order_hdr_cust_short_text_4)
            sqlComando.Parameters.AddWithValue("@order_hdr_cust_short_text_5", order_hdr_cust_short_text_5)
            sqlComando.Parameters.AddWithValue("@order_hdr_cust_short_text_6", order_hdr_cust_short_text_6)
            sqlComando.Parameters.AddWithValue("@order_hdr_cust_short_text_7", order_hdr_cust_short_text_7)
            sqlComando.Parameters.AddWithValue("@order_hdr_cust_short_text_8", order_hdr_cust_short_text_8)
            sqlComando.Parameters.AddWithValue("@order_hdr_cust_short_text_9", order_hdr_cust_short_text_9)
            sqlComando.Parameters.AddWithValue("@order_hdr_cust_short_text_10", order_hdr_cust_short_text_10)
            sqlComando.Parameters.AddWithValue("@order_hdr_cust_short_text_11", order_hdr_cust_short_text_11)
            sqlComando.Parameters.AddWithValue("@order_hdr_cust_short_text_12", order_hdr_cust_short_text_12)
            sqlComando.Parameters.AddWithValue("@order_hdr_cust_long_text_1", order_hdr_cust_long_text_1)
            sqlComando.Parameters.AddWithValue("@order_hdr_cust_long_text_2", order_hdr_cust_long_text_2)
            sqlComando.Parameters.AddWithValue("@order_hdr_cust_long_text_3", order_hdr_cust_long_text_3)
            If order_dtl_cust_date_1 <> "" Then
                sqlComando.Parameters.AddWithValue("@order_dtl_cust_date_1", order_dtl_cust_date_1.Replace("000000", ""))
            End If
            If order_dtl_cust_date_2 <> "" Then
                sqlComando.Parameters.AddWithValue("@order_dtl_cust_date_2", order_dtl_cust_date_2.Replace("000000", ""))
            End If
            If order_dtl_cust_date_3 <> "" Then
                sqlComando.Parameters.AddWithValue("@order_dtl_cust_date_3", order_dtl_cust_date_3.Replace("000000", ""))
            End If
            If order_dtl_cust_date_4 <> "" Then
                sqlComando.Parameters.AddWithValue("@order_dtl_cust_date_4", order_dtl_cust_date_4.Replace("000000", ""))
            End If
            If order_dtl_cust_date_5 <> "" Then
                sqlComando.Parameters.AddWithValue("@order_dtl_cust_date_5", order_dtl_cust_date_5.Replace("000000", ""))
            End If
            If order_dtl_cust_number_1 <> "" Then
                sqlComando.Parameters.AddWithValue("order_dtl_cust_number_1", order_dtl_cust_number_1)
            End If
            If order_dtl_cust_number_2 <> "" Then
                sqlComando.Parameters.AddWithValue("@order_dtl_cust_number_2", order_dtl_cust_number_2)
            End If
            If order_dtl_cust_number_3 <> "" Then
                sqlComando.Parameters.AddWithValue("@order_dtl_cust_number_3", order_dtl_cust_number_3)
            End If
            If order_dtl_cust_number_4 <> "" Then
                sqlComando.Parameters.AddWithValue("order_dtl_cust_number_4", order_dtl_cust_number_4)
            End If
            If order_dtl_cust_number_5 <> "" Then
                sqlComando.Parameters.AddWithValue("order_dtl_cust_number_5", order_dtl_cust_number_5)
            End If
            If order_dtl_cust_decimal_1 <> "" Then
                sqlComando.Parameters.AddWithValue("@order_dtl_cust_decimal_1", order_dtl_cust_decimal_1)
            End If
            If order_dtl_cust_decimal_2 <> "" Then
                sqlComando.Parameters.AddWithValue("@order_dtl_cust_decimal_2", order_dtl_cust_decimal_2)
            End If
            If order_dtl_cust_decimal_3 <> "" Then
                sqlComando.Parameters.AddWithValue("order_dtl_cust_decimal_3", order_dtl_cust_decimal_3)
            End If
            If order_dtl_cust_decimal_4 <> "" Then
                sqlComando.Parameters.AddWithValue("@order_dtl_cust_decimal_4", order_dtl_cust_decimal_4)
            End If
            If order_dtl_cust_decimal_5 <> "" Then
                sqlComando.Parameters.AddWithValue("order_dtl_cust_decimal_5", order_dtl_cust_decimal_5)
            End If
            sqlComando.Parameters.AddWithValue("@order_dtl_cust_short_text_1", order_dtl_cust_short_text_1)
            sqlComando.Parameters.AddWithValue("@order_dtl_cust_short_text_2", order_dtl_cust_short_text_2)
            sqlComando.Parameters.AddWithValue("@order_dtl_cust_short_text_3", order_dtl_cust_short_text_3)
            sqlComando.Parameters.AddWithValue("@order_dtl_cust_short_text_4", order_dtl_cust_short_text_4)
            sqlComando.Parameters.AddWithValue("@order_dtl_cust_short_text_5", order_dtl_cust_short_text_5)
            sqlComando.Parameters.AddWithValue("@order_dtl_cust_short_text_6", order_dtl_cust_short_text_6)
            sqlComando.Parameters.AddWithValue("@order_dtl_cust_short_text_7", order_dtl_cust_short_text_7)
            sqlComando.Parameters.AddWithValue("@order_dtl_cust_short_text_8", order_dtl_cust_short_text_8)
            sqlComando.Parameters.AddWithValue("@order_dtl_cust_short_text_9", order_dtl_cust_short_text_9)
            sqlComando.Parameters.AddWithValue("@order_dtl_cust_short_text_10", order_dtl_cust_short_text_10)
            sqlComando.Parameters.AddWithValue("@order_dtl_cust_short_text_11", order_dtl_cust_short_text_11)
            sqlComando.Parameters.AddWithValue("@order_dtl_cust_short_text_12", order_dtl_cust_short_text_12)
            sqlComando.Parameters.AddWithValue("@order_dtl_cust_long_text_1", order_dtl_cust_long_text_1)
            sqlComando.Parameters.AddWithValue("@order_dtl_cust_long_text_2", order_dtl_cust_long_text_2)
            sqlComando.Parameters.AddWithValue("@order_dtl_cust_long_text_3", order_dtl_cust_long_text_3)
            sqlComando.Parameters.AddWithValue("@invn_attr_d", invn_attr_d)
            sqlComando.Parameters.AddWithValue("@invn_attr_e", invn_attr_e)
            sqlComando.Parameters.AddWithValue("@invn_attr_f", invn_attr_f)
            sqlComando.Parameters.AddWithValue("@invn_attr_g", invn_attr_g)
            sqlComando.Parameters.AddWithValue("@order_type", order_type)
            sqlComando.Parameters.AddWithValue("@rcvd_trailer_nbr", rcvd_trailer_nbr)
            sqlComando.Parameters.AddWithValue("@stop_seal_nbr", stop_seal_nbr)
            sqlComando.Parameters.AddWithValue("@ship_request_line", ship_request_line)

            sqlConexion.Open()
            sqlComando.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            sqlConexion.Close()
        End Try

    End Sub






    Public Function validarOPPendienteTAL() As Boolean

        Dim dsConfiguracionGT As New DataSet
        Dim objDA As New SqlDataAdapter
        Dim sqlComando As New SqlCommand

        sqlComando.Connection = sqlConexion
        sqlComando.CommandType = CommandType.StoredProcedure
        sqlComando.CommandText = "sp_WMS_EntregasPorOrdenDeProduccionPendientesTAL"
        objDA.SelectCommand = sqlComando

        Try
            objDA.Fill(dsConfiguracionGT)

            If dsConfiguracionGT.Tables(0).Rows(0).Item(0).ToString <> "0" Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception

        End Try

    End Function

    Public Function validarOPPendienteXDOCK() As Boolean

        Dim dsConfiguracionGT As New DataSet
        Dim objDA As New SqlDataAdapter
        Dim sqlComando As New SqlCommand

        sqlComando.Connection = sqlConexion
        sqlComando.CommandType = CommandType.StoredProcedure
        sqlComando.CommandText = "sp_WMS_EntregasPorOrdenDeProduccionPendientesXDOCK"
        objDA.SelectCommand = sqlComando

        Try
            objDA.Fill(dsConfiguracionGT)

            If dsConfiguracionGT.Tables(0).Rows(0).Item(0).ToString <> "0" Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception

        End Try

    End Function


End Class