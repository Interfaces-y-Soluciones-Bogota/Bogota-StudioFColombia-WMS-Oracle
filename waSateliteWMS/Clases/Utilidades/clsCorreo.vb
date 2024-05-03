Imports System.Net.Mail
Imports System.Net
Imports System.Text

Public Class clsCorreo
    Inherits clsConfiguracion

    Private nombreEmpresa As String = "SFT"
    Private nombreProceso As String

    Public Function EnviarCorreoTarea(ByVal nombreProceso As String, ByVal CorreosDestinatarios As String, ByVal Mensaje As String, ByVal Tarea As String) As String

        Try

            Me.nombreProceso = nombreProceso

            Dim smtp As New SmtpClient(ServidorDeCorreo)
            smtp.Port = Puerto
            smtp.EnableSsl = SSL
            smtp.Credentials = New NetworkCredential(UsuarioMail, ClaveMail)
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network
            smtp.Timeout = 30000

            Dim fromAddress As New MailAddress(CorreoRemitente, "Integracion WMS")
            Dim subject As String = asuntoCorreo()
            Dim body As String = CuerpoCorreo(nombreProceso, Mensaje, Tarea)

            Dim message As New MailMessage()
            message.To.Add(CorreosDestinatarios)
            message.From = fromAddress
            message.Subject = subject
            message.Body = body
            message.IsBodyHtml = True

            smtp.Send(message)
            Return "Envio correcto"
        Catch ex As Exception
            Return "Error al enviar el correo : " & ex.Message
        End Try

    End Function

    Public Function EnviarCorreoTareaConAdjunto(ByVal nombreProceso As String, ByVal CorreosDestinatarios As String, ByVal rutaArchivo As String, ByVal Tarea As String) As String

        Try
            Dim Mensaje As String = "Este es un mensaje enviado desde el sistema de integración, adjunto a este correo electrónico se envía el reporte con el resultado de la ejecución."
            Me.nombreProceso = nombreProceso

            Dim smtp As New SmtpClient(ServidorDeCorreo)
            smtp.Port = Puerto
            smtp.EnableSsl = SSL
            smtp.Credentials = New NetworkCredential(UsuarioMail, ClaveMail)
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network
            smtp.Timeout = 30000

            Dim fromAddress As New MailAddress(CorreoRemitente, "Integracion WMS")
            Dim subject As String = asuntoCorreo()
            Dim body As String = CuerpoCorreo(nombreProceso, Mensaje, Tarea)
            Dim file As New Mail.Attachment(rutaArchivo)


            Dim message As New MailMessage()
            message.To.Add(CorreosDestinatarios)
            message.From = fromAddress
            message.Subject = subject
            message.Body = body
            message.IsBodyHtml = True
            message.Attachments.Add(file)

            smtp.Send(message)
            Return "Envio correcto"
        Catch ex As Exception
            Return "Error al enviar el correo : " & ex.Message
        End Try

    End Function

    Private Function asuntoCorreo() As String
        Return nombreEmpresa & "-" & Me.nombreProceso
    End Function


    Public Function CuerpoCorreo(ByVal nombreProceso As String, ByVal Mensaje As String, ByVal Tarea As String) As String

        Dim strMensaje As New StringBuilder

        strMensaje.AppendLine("<style type=""text/css"">")
        strMensaje.AppendLine("</style>")
        strMensaje.AppendLine("<BR/><BR/><table style=""color: #666; font-family: 'font-family: 'Georgia'', sans-serif;background-image:url('http://www.generictransfer.com/imagenes/main_bg.png')"" border=""0"" cellpadding=""0"" cellspacing=""0"" ")

        strMensaje.AppendLine("<tr>")
        strMensaje.AppendLine("<td style=""border-bottom-style: solid; border-bottom-width: medium; border-bottom-color: #2AA0D0"" bgcolor=""White"">")
        strMensaje.AppendLine("<img alt="""" src=""http://www.generictransfer.com/imagenes/logo_ge.png"" />")
        strMensaje.AppendLine("</td>")
        strMensaje.AppendLine("</tr>")

        strMensaje.AppendLine("<tr>")
        strMensaje.AppendLine("<td width=""700px"">")
        strMensaje.AppendLine("<br />")
        strMensaje.AppendLine("Generic Transfer Integration")
        strMensaje.AppendLine("</td>")
        strMensaje.AppendLine("</tr>")

        strMensaje.AppendLine("<tr>")
        strMensaje.AppendLine("<td style=""font-size: 18px;font-style: normal;font-weight: normal;font-variant: normal;text-decoration: none;color: #4799CC"">")
        strMensaje.AppendLine("<BR/>Información de la tarea " & Tarea & " automatica de migración de datos - " & nombreProceso & "<BR/>")
        strMensaje.AppendLine("</td>")
        strMensaje.AppendLine("</tr>")

        strMensaje.AppendLine("<tr>")
        strMensaje.AppendLine("<td>")
        strMensaje.AppendLine("&nbsp;")
        strMensaje.AppendLine("</td>")
        strMensaje.AppendLine("</tr>")

        strMensaje.AppendLine("<tr>")
        strMensaje.AppendLine("<td>")


        strMensaje.AppendLine("Resultado de la ejecución: <br />" & Mensaje)

        strMensaje.AppendLine("</td>")
        strMensaje.AppendLine("</tr>")

        strMensaje.AppendLine("<br />")

        strMensaje.AppendLine("<tr>")
        strMensaje.AppendLine("<td align=""center"">")
        strMensaje.AppendLine("<strong><span class=""style2"">")
        strMensaje.AppendLine("<BR/><BR/>Interfaces y Soluciones S.A.S<br /> <BR/><BR/>")
        strMensaje.AppendLine("</span></strong>")
        strMensaje.AppendLine("</td>")
        strMensaje.AppendLine("</tr>")
        strMensaje.AppendLine("</table>")


        Return strMensaje.ToString

    End Function

End Class
