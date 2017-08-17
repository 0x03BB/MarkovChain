Public Class FormMain
    Private Sub FormMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim markov = New MarkovChain({"A", "B", "C"}, {{0.2R, 0.4R}, {0.4R, 0.2R}, {0.4R, 0.4R}})

        TextBox1.AppendText(markov.Take(200).Aggregate(Function(s1, s2) s1 & " " & s2))
    End Sub
End Class