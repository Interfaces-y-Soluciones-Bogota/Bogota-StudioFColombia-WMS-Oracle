Public Class frmSateliteWMS
    Private Sub frmSateliteWMS_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim args As String()
        args = Environment.GetCommandLineArgs()


        Try
            Select Case args(1)
            'Lectura de Maestros Items
                Case 36
                    Dim objMaesto As New clsMaestros
                    objMaesto.cargarMaestro()

            'Lectura de Maestros Store
                Case 37
                    Dim objMaesto As New clsMaestros
                    objMaesto.cargarMaestro()
'----------------------------------------10. Recepción(Entrada) y almacenaje talleres (CEDI)-----------------------------------
            '2. Leer WS ASN (Tipo TAL)
                Case 38
                    Dim objAsn As New clsASN
                    objAsn.FechaInicial = Now.Date.ToString("yyyy-MM-dd")
                    objAsn.FechaFinal = Now.Date.ToString("yyyy-MM-dd")
                    'objAsn.FechaInicial = "2017-11-16"
                    'objAsn.FechaFinal = "2017-11-16"
                    objAsn.importarASN("TAL")

                    '3. Generar ASN - Estructura Oracle 
                    Dim objCargaPlanoASN As New clsCargarPlano
                    objCargaPlanoASN.cargarPlanoASN_TAL(39)

            '6. Leer Estructura de Oracle de Entradas
                Case 40
                    Dim objSftp As New clsSFTP
                    Dim objConfiguracion As New clsConfiguracion
                    objSftp.PathSFTP = objConfiguracion.RutaFTPOutput_WMS
                    objSftp.ServidorFTP_WMS = objConfiguracion.ServidorFTP_WMS
                    objSftp.PuertoFTP_WMS = objConfiguracion.PuertoFTP_WMS
                    objSftp.UsuarioFTP_WMS = objConfiguracion.UsuarioFTP_WMS
                    objSftp.ClaveFTP_WMS = objConfiguracion.ClaveFTP_WMS
                    objSftp.Prefijo = "SVS"

                    objSftp.descargarArchivosSFTP(40, objConfiguracion)
            '7. Crear EC
                Case 45
                    Dim objMonoProceso As New clsMonoProceso
                    objMonoProceso.SincronizacionUnoAUno("f350_consec_docto", "f470_consec_docto")
            '7.1 Crear EOP a bodega No Disponible
                Case 41
                    Dim objVerificacionAsn As New clsVerificacionASN
                    If objVerificacionAsn.validarOPPendienteTAL Then
                        Dim objMonoProceso As New clsMonoProceso
                        objMonoProceso.SincronizacionUnoAUno("f350_consec_docto", "f470_consec_docto")
                    End If
            '10. Leer estructura Oracle con Historico
                Case 42
                    Dim objSftp As New clsSFTP
                    Dim objConfiguracion As New clsConfiguracion
                    'objSftp.PathLocal = "C:\inetpub\wwwroot\GTIntegration\Planos\WMS\InputGTI\IHT\"
                    objSftp.PathSFTP = objConfiguracion.RutaFTPOutput_WMS
                    objSftp.Prefijo = "IHT"
                    objSftp.ServidorFTP_WMS = objConfiguracion.ServidorFTP_WMS
                    objSftp.PuertoFTP_WMS = objConfiguracion.PuertoFTP_WMS
                    objSftp.UsuarioFTP_WMS = objConfiguracion.UsuarioFTP_WMS
                    objSftp.ClaveFTP_WMS = objConfiguracion.ClaveFTP_WMS

                    objSftp.descargarArchivosSFTP(42, objConfiguracion)
            '12 Genera TR a bodegas disponibles por canal
                Case 43
                    Dim objMonoProceso As New clsMonoProceso
                    objMonoProceso.SincronizacionUnoAUno("f350_consec_docto", "f470_consec_docto")

            '12.1 Las marcadas como segundas van a la bodega correspondiente
                Case 58
                    Dim objMonoProceso As New clsMonoProceso
                    objMonoProceso.Sincronizacion()


            '12.1 Matriz Historial de Inventarios
                'Case 117
                '    Dim objMonoProceso As New clsMonoProceso
                '    objMonoProceso.SincronizacionTransferenciasAjustes("f350_consec_docto", "f470_consec_docto")

                '12.2 Matriz Historial de Inventarios - importación de datos masivos
                Case 181
                    Dim objMonoProceso As New clsMonoProceso
                    objMonoProceso.SincronizacionTransferenciasAjustesMasivos("f350_consec_docto", "f470_consec_docto")

                    'Documento de Ajuste - Cantidades
                    'Case 126
                    '    Dim objMonoProceso As New clsMonoProceso
                    'objMonoProceso.SincronizacionUnoAUnoMultipleBodega("f350_consec_docto", "f470_consec_docto")

                    'Documento de Ajuste - Costo
                    'Case 150
                    '    Dim objMonoProceso As New clsMonoProceso
                    'objMonoProceso.SincronizacionUnoAUno("f350_consec_docto", "f470_consec_docto")

            'Traslados directos pos-- por eop
                Case 159
                    Dim objMonoProceso As New clsMonoProceso
                    objMonoProceso.SincronizacionUnoAUno("f350_consec_docto", "f470_consec_docto")

'-----------------------------------------------11. Recepción (Entrada) y almacenaje de talleres XDOCK (CEDI)----------------------------
            '2. Leer WS ASN (Tipo XDOCK)
                Case 46
                    Dim objAsn As New clsASN
                    objAsn.FechaInicial = Now.Date.ToString("yyyy-MM-dd")
                    objAsn.FechaFinal = Now.Date.ToString("yyyy-MM-dd")
                    'objAsn.FechaInicial = "2017-11-15"
                    'objAsn.FechaFinal = "2017-11-15"
                    objAsn.importarASN("XDOCK")

                    '3. Generar ASN - Estructura Oracle 
                    Dim objCargaPlanoASN As New clsCargarPlano
                    objCargaPlanoASN.cargarPlanoASN_XDOCK(47)

            '6. Leer Estructura de Oracle de Entradas
                Case 48
                    Dim objSftp As New clsSFTP
                    Dim objConfiguracion As New clsConfiguracion
                    objSftp.PathSFTP = objConfiguracion.RutaFTPOutput_WMS
                    objSftp.Prefijo = "SVS"
                    objSftp.ServidorFTP_WMS = objConfiguracion.ServidorFTP_WMS
                    objSftp.PuertoFTP_WMS = objConfiguracion.PuertoFTP_WMS
                    objSftp.UsuarioFTP_WMS = objConfiguracion.UsuarioFTP_WMS
                    objSftp.ClaveFTP_WMS = objConfiguracion.ClaveFTP_WMS

                    objSftp.descargarArchivos(48, objConfiguracion)

            '7. Crear EC
                Case 49
                    Dim objMonoProceso As New clsMonoProceso
                    objMonoProceso.SincronizacionUnoAUno("f350_consec_docto", "f470_consec_docto")

            '7.1 Crear EOP a bodega XDOCK
                Case 50
                    Dim objVerificacionAsn As New clsVerificacionASN
                    If objVerificacionAsn.validarOPPendienteXDOCK Then
                        Dim objMonoProceso As New clsMonoProceso
                        objMonoProceso.SincronizacionUnoAUno("f350_consec_docto", "f470_consec_docto")
                    End If
'-----------------------------------------------12. Recepción (Entrada) y almacenaje Traslados (CEDI)----------------------------
            '2. Leer WMS ASN Tipo (TRA)
                Case 51
                    Dim objAsn As New clsASN
                    objAsn.FechaInicial = Now.Date.ToString("yyyy-MM-dd")
                    objAsn.FechaFinal = Now.Date.ToString("yyyy-MM-dd")
                    'objAsn.FechaInicial = "2017-11-17"
                    'objAsn.FechaFinal = "2017-11-17"
                    objAsn.importarASN("TRA")
                    objAsn.ActualizarEstadoASNLecturaCompleta()
            '3. Genera documento TR a Bodega No Disponible 3.1 Completa Interface ASN (Shipment_nbr) con documento TRT (TRA)
                Case 83
                    Dim objMonoProceso As New clsMonoProceso
                    Dim objASN As New clsASN
                    objMonoProceso.SincronizacionUnoAUno("f350_consec_docto", "f470_consec_docto")
                    objASN.encabezadoASN_ActualizarConsecutivo_TRA()
            '4. Generar ASN Estructura Oracle - '3.1 Completa interface ASN con numero TR sp_WMS_Traslados_TST_ActualizarConsecutivoASN
                Case 69
                    Dim objCargaPlanoASN As New clsCargarPlano
                    Dim objASN As New clsASN
                    objCargaPlanoASN.cargarPlanoASN_TRA(args(1))

            '7. Leer Estructura de Oracle de Entradas
                Case 70
                    Dim objSftp As New clsSFTP
                    Dim objConfiguracion As New clsConfiguracion
                    objSftp.PathSFTP = objConfiguracion.RutaFTPOutput_WMS
                    objSftp.Prefijo = "SVS"
                    objSftp.ServidorFTP_WMS = objConfiguracion.ServidorFTP_WMS
                    objSftp.PuertoFTP_WMS = objConfiguracion.PuertoFTP_WMS
                    objSftp.UsuarioFTP_WMS = objConfiguracion.UsuarioFTP_WMS
                    objSftp.ClaveFTP_WMS = objConfiguracion.ClaveFTP_WMS

                    objSftp.descargarArchivosSFTP(70, objConfiguracion)

            '08 Confirmar Traslados (TR) a bodegas destino
                Case 145
                    Dim objMonoProceso As New clsMonoProceso
                    objMonoProceso.SincronizacionUnoAUno("f350_consec_docto", "f470_consec_docto")

           '10. Leer estructura Oracle con Historico
                Case 71
                    Dim objSftp As New clsSFTP
                    Dim objConfiguracion As New clsConfiguracion
                    'objSftp.PathLocal = "C:\inetpub\wwwroot\GTIntegration\Planos\WMS\InputGTI\IHT\"
                    objSftp.PathSFTP = objConfiguracion.RutaFTPOutput_WMS
                    objSftp.Prefijo = "IHT"
                    objSftp.ServidorFTP_WMS = objConfiguracion.ServidorFTP_WMS
                    objSftp.PuertoFTP_WMS = objConfiguracion.PuertoFTP_WMS
                    objSftp.UsuarioFTP_WMS = objConfiguracion.UsuarioFTP_WMS
                    objSftp.ClaveFTP_WMS = objConfiguracion.ClaveFTP_WMS

                    objSftp.descargarArchivosSFTP(71, objConfiguracion)

            '12 Genera TR a bodegas disponibles por canal
                Case 88
                    Dim objMonoProceso As New clsMonoProceso
                    objMonoProceso.SincronizacionUnoAUno("f350_consec_docto", "f470_consec_docto")
            '12.1 Las marcadas como segundas van a la bodega correspondiente
                Case 58
                    Dim objMonoProceso As New clsMonoProceso
                    objMonoProceso.Sincronizacion()
'-----------------------------------------------13. Recepción (Entrada) y almacenaje Proveedores y Devolución E-Commerce (CEDI)---------
           '4. Leer estrucutra Oracle de entradas verificacion tipo PRO
                Case 53
                    Dim objSftp As New clsSFTP
                    Dim objConfiguracion As New clsConfiguracion
                    objSftp.PathSFTP = objConfiguracion.RutaFTPOutput_WMS
                    objSftp.Prefijo = "SVS"
                    objSftp.ServidorFTP_WMS = objConfiguracion.ServidorFTP_WMS
                    objSftp.PuertoFTP_WMS = objConfiguracion.PuertoFTP_WMS
                    objSftp.UsuarioFTP_WMS = objConfiguracion.UsuarioFTP_WMS
                    objSftp.ClaveFTP_WMS = objConfiguracion.ClaveFTP_WMS

                    objSftp.descargarArchivosSFTP(53, objConfiguracion)

           '5. Crear EC - DOCTO Entradas por compra PRO
                Case 55
                    Dim objMonoProceso As New clsMonoProceso
                    objMonoProceso.SincronizacionUnoAUno("f350_consec_docto", "f470_consec_docto")

            '7. Leer estructura Oracle con Historico
                'Case 54
                '    Dim objSftp As New clsSFTP
                '    Dim objConfiguracion As New clsConfiguracion
                '    'objSftp.PathLocal = "C:\inetpub\wwwroot\GTIntegration\Planos\WMS\InputGTI\IHT\"
                '    objSftp.PathSFTP = objConfiguracion.RutaFTPOutput_WMS
                '    objSftp.Prefijo = "IHT"
                '    objSftp.descargarArchivos(54, objConfiguracion)
            '9 Genera TR a bodegas disponibles por canal

                Case 89
                    Dim objMonoProceso As New clsMonoProceso
                    objMonoProceso.SincronizacionUnoAUno("f350_consec_docto", "f470_consec_docto")

             '9.1 Las marcadas como segundas van a la bodega correspondiente
                Case 57
                    Dim objMonoProceso As New clsMonoProceso
                    objMonoProceso.Sincronizacion()
'-----------------------------------------------14. Recepción (Entrada) y almacenaje recogidas y devoluciones (Logística Inversa)---------
                '2 Leer WS ASN (Tipo DEV-REC-MUE)
                Case 63
                    Dim objAsn As clsASN
                    objAsn = New clsASN
                    objAsn.FechaInicial = Now.Date.ToString("yyyy-MM-dd")
                    objAsn.FechaFinal = Now.Date.ToString("yyyy-MM-dd")
                    'objAsn.FechaInicial = "2018-01-19"
                    'objAsn.FechaFinal = "2018-01-19"
                    objAsn.importarASN("DEV")
                    objAsn.ActualizarEstadoASNLecturaCompleta_DEV()

                    objAsn = New clsASN
                    objAsn.FechaInicial = Now.Date.ToString("yyyy-MM-dd")
                    objAsn.FechaFinal = Now.Date.ToString("yyyy-MM-dd")
                    objAsn.importarASN("REC")
                    objAsn.ActualizarEstadoASNLecturaCompleta_REC()

                    objAsn = New clsASN
                    objAsn.FechaInicial = Now.Date.ToString("yyyy-MM-dd")
                    objAsn.FechaFinal = Now.Date.ToString("yyyy-MM-dd")
                    objAsn.importarASN("MUE")
                    objAsn.ActualizarEstadoASNLecturaCompleta_MUE()

                    objAsn = New clsASN
                    objAsn.FechaInicial = Now.Date.ToString("yyyy-MM-dd")
                    objAsn.FechaFinal = Now.Date.ToString("yyyy-MM-dd")
                    objAsn.importarASN("TAL")
                    objAsn.ActualizarEstadoASNLecturaCompleta_TAL()

               '3. Genera documento TST a Bodega No Disponible 3.1 Completa Interface ASN (Shipment_nbr) con documento TRT (TRA)
                Case 84
                    Dim objMonoProceso As New clsMonoProceso
                    Dim objASN As New clsASN
                    objMonoProceso.SincronizacionUnoAUno("f350_consec_docto", "f470_consec_docto")
                    objASN.encabezadoASN_ActualizarConsecutivo_DEV()

                    '4 Generar ASN Estructura Oracle
                    Dim objCargaPlanoASN As New clsCargarPlano
                    objCargaPlanoASN.cargarPlanoASN_DEV(64)

                 '7 Leer estructura Oracle de entradas (Historico)
                Case 65
                    Dim objSftp As New clsSFTP
                    Dim objConfiguracion As New clsConfiguracion
                    'objSftp.PathLocal = "C:\inetpub\wwwroot\GTIntegration\Planos\WMS\InputGTI\IHT\"
                    objSftp.PathSFTP = objConfiguracion.RutaFTPOutput_WMSLogInversa_WMS
                    objSftp.Prefijo = "SVS"
                    objSftp.ServidorFTP_WMS = objConfiguracion.ServidorFTP_WMS
                    objSftp.PuertoFTP_WMS = objConfiguracion.PuertoFTP_WMS
                    objSftp.UsuarioFTP_WMS = objConfiguracion.UsuarioFTP_WMS
                    objSftp.ClaveFTP_WMS = objConfiguracion.ClaveFTP_WMS

                    objSftp.descargarArchivosSFTP(65, objConfiguracion)

                    '8 Confirmar los traslados (TR) a bodega destino Logistica Inversa (LI01)
                Case 144
                    Dim objMonoProceso As New clsMonoProceso
                    objMonoProceso.SincronizacionUnoAUno("f350_consec_docto", "f470_consec_docto")

'-----------------------------------------------17. Despacho (Salida) Ventas X Mayor, Franquicias, E - commerce---------
              '1 Consultar Pedidos Por canal 

              '1.1 Consulta Requisiciones y Pedidos Pendientes (Ecommerce)
                Case 73
                    Dim objTraslado As New clsTraslados
                    objTraslado.AlmacenarPedidosPendientesEcommerce()

                    Dim objCargaPlanoPedidos As New clsCargarPlano
                    Dim objConfiguracion As New clsConfiguracion
                    objCargaPlanoPedidos.cargarPedidosCopiaLocalEcommerce((74), objConfiguracion.RutaFTPInput_WMS)

               '1.2 Consulta Requisiciones y Pedidos Pendientes (RQI)
                Case 169
                    Dim objTraslado As New clsTraslados
                    objTraslado.AlmacenarRequisicionesPendientesRQI()

                    Dim objCargaPlanoPedidos As New clsCargarPlano
                    Dim objConfiguracion As New clsConfiguracion
                    objCargaPlanoPedidos.cargarPedidosCopiaLocal((171), objConfiguracion.RutaFTPInput_WMS)

               '1.3 Consulta Requisiciones y Pedidos Pendientes (PV_PVI)
                Case 170
                    Dim objTraslado As New clsTraslados
                    objTraslado.AlmacenarPedidosPendientesPV_PVI()

                    Dim objCargaPlanoPedidos As New clsCargarPlano
                    Dim objConfiguracion As New clsConfiguracion
                    objCargaPlanoPedidos.cargarPedidosCopiaLocal((173), objConfiguracion.RutaFTPInput_WMS)

             '2 Enviar pedidos Consolidados
                Case 74
                    Dim objCargaPlanoPedidos As New clsCargarPlano
                    Dim objConfiguracion As New clsConfiguracion
                    objCargaPlanoPedidos.cargarPedidosCopiaLocal(args(1), objConfiguracion.RutaFTPInput_WMS)

            '6. Leer Archivo Despacho Carga
                Case 76
                    Dim objSftp As New clsSFTP
                    Dim objConfiguracion As New clsConfiguracion
                    objSftp.PathSFTP = objConfiguracion.RutaFTPOutput_WMS
                    objSftp.Prefijo = "SLS"
                    objSftp.ServidorFTP_WMS = objConfiguracion.ServidorFTP_WMS
                    objSftp.PuertoFTP_WMS = objConfiguracion.PuertoFTP_WMS
                    objSftp.UsuarioFTP_WMS = objConfiguracion.UsuarioFTP_WMS
                    objSftp.ClaveFTP_WMS = objConfiguracion.ClaveFTP_WMS

                    objSftp.descargarArchivosSFTP(76, objConfiguracion)

            'Compromisos Ecommerce PREFactura - Descomprometer
                Case 154
                    Dim objMonoProceso As New clsMonoProceso
                    objMonoProceso.SincronizacionUnoAUno("F430_CONSEC_DOCTO", "")

            'Compromiso Ecommerce
                Case 147
                    Dim objMonoProceso As New clsMonoProceso
                    objMonoProceso.SincronizacionUnoAUno("F430_CONSEC_DOCTO", "")

            'Compromiso Ecommerce Descomprometer
                Case 153
                    Dim objMonoProceso As New clsMonoProceso
                    objMonoProceso.SincronizacionUnoAUno("F430_CONSEC_DOCTO", "")

            'Compromiso Ecommerce POS Factura
                Case 152
                    Dim objMonoProceso As New clsMonoProceso
                    objMonoProceso.SincronizacionUnoAUno("F430_CONSEC_DOCTO", "")

           'Remision (RM) desde Pedidos PV Ecommerce
                Case 78
                    Dim objMonoProceso As New clsMonoProceso
                    objMonoProceso.SincronizacionUnoAUno("F430_CONSEC_DOCTO", "")

            'Factura (FV) desde Remision (RM) Ecommerce
                Case 128
                    Dim objMonoProceso As New clsMonoProceso
                    objMonoProceso.SincronizacionUnoAUno("f350_consec_docto", "f350_consec_docto", "f350_consec_docto")

            'Pos Factura - Cruce anticipos/facturas ecommerce
                Case 180
                    Dim objMonoProceso As New clsMonoProceso
                    objMonoProceso.SincronizacionUnoAUno("F350_CONSEC_DOCTO", "F350_CONSEC_DOCTO")

            'Cancelacion Pedidos Ecommerce
                Case 157
                    Dim objMonoProceso As New clsMonoProceso
                    objMonoProceso.SincronizacionUnoAUno("F430_CONSEC_DOCTO", "")

           'Remision (RM) Directa
                Case 131
                    Dim objMonoProceso As New clsMonoProceso
                    objMonoProceso.SincronizacionUnoAUno("F350_CONSEC_DOCTO", "f470_consec_docto")

           'Factura (FV) Directa
                Case 142
                    Dim objMonoProceso As New clsMonoProceso
                    objMonoProceso.SincronizacionUnoAUno("F350_CONSEC_DOCTO", "f470_consec_docto", "F350_CONSEC_DOCTO")

           'Transferencia (TR) desde Requisicion RQI
                Case 146
                    Dim objMonoProceso As New clsMonoProceso
                    objMonoProceso.SincronizacionUnoAUno("f350_consec_docto", "f470_consec_docto")

           'Traslado Directo (TR)
                Case 149
                    Dim objMonoProceso As New clsMonoProceso
                    objMonoProceso.SincronizacionUnoAUno("f350_consec_docto", "f470_consec_docto")

 '-----------------------------------------------17. Despacho a CEDI (Logistica Inversa) y Recepcion y Almacenaje (CEDI)---------
             '1.1 Consulta Traslados (TR)
                Case 79
                    Dim objTraslado As New clsTraslados
                    objTraslado.AlmacenarTrasladosLogisticaInversa()
             '2. Enviar pedidos consolidados
                Case 80
                    Dim objCargaPlanoPedidos As New clsCargarPlano
                    Dim objConfiguracion As New clsConfiguracion
                    objCargaPlanoPedidos.cargarPedidos(args(1), objConfiguracion.RutaFTPInput_WMSLogInversa)
             '9. Leer interface de despacho Logistica Inversa
                Case 92
                    Dim objSftp As New clsSFTP
                    Dim objConfiguracion As New clsConfiguracion
                    objSftp.PathSFTP = objConfiguracion.RutaFTPOutput_WMSLogInversa_WMS
                    objSftp.Prefijo = "SLS"
                    objSftp.ServidorFTP_WMS = objConfiguracion.ServidorFTP_WMS
                    objSftp.PuertoFTP_WMS = objConfiguracion.PuertoFTP_WMS
                    objSftp.UsuarioFTP_WMS = objConfiguracion.UsuarioFTP_WMS
                    objSftp.ClaveFTP_WMS = objConfiguracion.ClaveFTP_WMS

                    objSftp.descargarArchivos(92, objConfiguracion)
                'Lectura de la verificacion Logistica Inversa
                Case 92
                    Dim objSftp As New clsSFTP
                    Dim objConfiguracion As New clsConfiguracion
                    objSftp.PathSFTP = objConfiguracion.RutaFTPOutput_WMSLogInversa_WMS
                    objSftp.Prefijo = "SVS"
                    objSftp.ServidorFTP_WMS = objConfiguracion.ServidorFTP_WMS
                    objSftp.PuertoFTP_WMS = objConfiguracion.PuertoFTP_WMS
                    objSftp.UsuarioFTP_WMS = objConfiguracion.UsuarioFTP_WMS
                    objSftp.ClaveFTP_WMS = objConfiguracion.ClaveFTP_WMS

                    objSftp.descargarArchivos(92, objConfiguracion)

             ' 10. Confirmar el TR a bodega 'No     ' (Entrada en Transito)
            ' Case 81
                Case 143
                    Dim objMonoProceso As New clsMonoProceso
                    objMonoProceso.SincronizacionUnoAUno("f350_consec_docto", "f470_consec_docto")
            ' 13. Leer Interface Historico de Inventarios
                Case 54
                    Dim objSftp As New clsSFTP
                    Dim objConfiguracion As New clsConfiguracion
                    'objSftp.PathLocal = "C:\inetpub\wwwroot\GTIntegration\Planos\WMS\InputGTI\IHT\"
                    objSftp.PathSFTP = objConfiguracion.RutaFTPOutput_WMSLogInversa_WMS
                    objSftp.Prefijo = "IHT"
                    objSftp.ServidorFTP_WMS = objConfiguracion.ServidorFTP_WMS
                    objSftp.PuertoFTP_WMS = objConfiguracion.PuertoFTP_WMS
                    objSftp.UsuarioFTP_WMS = objConfiguracion.UsuarioFTP_WMS
                    objSftp.ClaveFTP_WMS = objConfiguracion.ClaveFTP_WMS

                    objSftp.descargarArchivos(54, objConfiguracion)
                    '14 Generar traslados a Bodega Outlet   
                Case 82
                    Dim objMonoProceso As New clsMonoProceso
                    objMonoProceso.SincronizacionUnoAUno("f350_consec_docto", "f470_consec_docto")

                Case 105
                    Dim objMonoProceso As New clsMonoProceso
                    objMonoProceso.SincronizacionUnoAUno("f350_consec_docto", "f470_consec_docto")
                '----------------------------------------- MOBILISTIC - GESTOR DE PEDIDOS ----------------------------------------------
                'Mobilistic GP_MAESTROS ÍTEMS, CLIENTES/SUCURSALES, INVENTARIO
                Case 121, 122, 123, 124, 125
                    Dim objGTMaestros As New clsGPMaestros
                    objGTMaestros.cargarMaestro()
                'Mobilistic Consultar/Leer Pedidos desde GP
                Case 127
                    Dim objGTPedidos As New clsGPPedidos
                    objGTPedidos.almacenarPedido(127)
                'Mobilistic Actualizar Pedidos al GP
                Case 129
                    Dim objGTMaestros As New clsGPMaestros
                    objGTMaestros.cargarMaestro()
                'Mobilistic Actualizar Pedidos Cancelados al GP
                Case 151
                    Dim objGTMaestros As New clsGPMaestros
                    objGTMaestros.cargarMaestro()
                'Mobilistic - Reportar Pedidos al WMS
                Case 130
                    Dim objCargaPlanoPedidos As New clsCargarPlano
                    Dim objConfiguracion As New clsConfiguracion
                    objCargaPlanoPedidos.cargarPedidosGP(args(1), objConfiguracion.RutaFTPInput_WMS)
                'Mobilistic - Traslados E-commerce GP
                Case 156
                    Dim objGTEcommerce As New clsGPEcommerce
                    objGTEcommerce.transferMovement(156)


                ' PEDIDOS PLAN B
                'PRE-Compromiso (Descomprometer)
                Case 160
                    Dim objMonoProceso As New clsMonoProceso
                    objMonoProceso.SincronizacionUnoAUno("F430_CONSEC_DOCTO", "")
                'Compromiso
                Case 162
                    Dim objMonoProceso As New clsMonoProceso
                    objMonoProceso.SincronizacionUnoAUno("F430_CONSEC_DOCTO", "")
                'Remision (RM) desde Pedidos 
                Case 161
                    Dim objMonoProceso As New clsMonoProceso
                    objMonoProceso.SincronizacionUnoAUno("F430_CONSEC_DOCTO", "")
                'Factura (FV) desde Remision (RM)
                Case 163
                    Dim objMonoProceso As New clsMonoProceso
                    objMonoProceso.SincronizacionUnoAUno("f350_consec_docto", "f350_consec_docto", "f350_consec_docto")
                'Cancelacion Pedidos 
                Case 164
                    Dim objMonoProceso As New clsMonoProceso
                    objMonoProceso.SincronizacionUnoAUno("F430_CONSEC_DOCTO", "")
                'Cancelacion Pedidos vía conector
                Case 182
                    Dim objMonoProceso As New clsMonoProceso
                    objMonoProceso.SincronizacionUnoAUno("f430_consec_docto", "")


                ' REQUISICIONES PLAN B
                'DesCompromiso
                Case 168
                    Dim objMonoProceso As New clsMonoProceso
                    objMonoProceso.SincronizacionUnoAUno("F440_CONSEC_DOCTO", "")
                'Compromiso
                Case 166
                    Dim objMonoProceso As New clsMonoProceso
                    objMonoProceso.SincronizacionUnoAUno("F440_CONSEC_DOCTO", "")

                'Transferencia desde RQI
                Case 77
                    Dim objMonoProceso As New clsMonoProceso
                    objMonoProceso.SincronizacionTransferenciasDesdeRqi("f350_consec_docto", "")

                'Cancelacion Requisicion
                Case 167
                    Dim objMonoProceso As New clsMonoProceso
                    objMonoProceso.SincronizacionUnoAUno("F440_CONSEC_DOCTO", "")

                    'DESCOMPROMISO Y COMPROMISO PARCIAL POR TABLAS - REQUISICIONES

                    'Descompromiso Parcial 
                Case 175
                    Dim objDespachoCargas As New clsDespachoCargas
                    objDespachoCargas.descompromisoParcial("f440_consec_docto")

                    'Compromiso Parcial
                Case 176
                    Dim objDespachoCargas As New clsDespachoCargas
                    objDespachoCargas.compromisoParcial("")

                     'DESCOMPROMISO Y COMPROMISO PARCIAL POR TABLAS - PEDIDOS

                    'Descompromiso Parcial 
                Case 177
                    Dim objDespachoCargas As New clsDespachoCargas
                    objDespachoCargas.descompromisoParcialPedidos("f430_consec_docto")

                    'Compromiso Parcial - Pedidos
                Case 178
                    Dim objDespachoCargas As New clsDespachoCargas
                    objDespachoCargas.compromisoParcialPedidos("")

            End Select
        Catch ex As Exception

        Finally
            Me.Close()
        End Try

    End Sub

End Class