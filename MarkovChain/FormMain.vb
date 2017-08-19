Public Class FormMain
    Private markov As MarkovChain 'New MarkovChain({"A", "B", "C"}, {{0.2R, 0.4R}, {0.4R, 0.2R}, {0.4R, 0.4R}})
    Private Shared reg As New Text.RegularExpressions.Regex("[.,;:]")

    Private Sub ButtonGenerate_Click(sender As Object, e As EventArgs) Handles ButtonGenerate.Click
        markov = CreateMarkovChain(TextBox1.Text)
    End Sub

    Private Sub ButtonEnumerate_Click(sender As Object, e As EventArgs) Handles ButtonEnumerate.Click
        FillTextBox()
    End Sub

    Private Sub FillTextBox()
        TextBox1.Text = markov.Take(300).Aggregate(AddressOf SpaceStrings) 'Function(s1, s2) s1 & " " & s2
    End Sub

    Private Function SpaceStrings(s1 As String, s2 As String) As String
        If reg.IsMatch(s2) Then
            Return s1 & s2
        ElseIf Strings.Right(s1, 2) = vbCrLf Then
            Return s1 & s2
        Else
            Return s1 & " " & s2
        End If
    End Function
End Class