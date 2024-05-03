Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim objAsn As New wsASNOracle.AsnService

        Dim tmp As Object
        tmp = objAsn.GetAvailableAsns("2017-11-01", True, "2018-01-30", True)



    End Sub
End Class