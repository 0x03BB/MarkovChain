Public Class FormMain
    Private markov As MarkovChain 'New MarkovChain({"A", "B", "C"}, {{0.2R, 0.4R}, {0.4R, 0.2R}, {0.4R, 0.4R}})

    Private Sub ButtonGenerate_Click(sender As Object, e As EventArgs) Handles ButtonGenerate.Click
        markov = CreateMarkovChain(TextBox1.Text)
    End Sub

    Private Sub ButtonEnumerate_Click(sender As Object, e As EventArgs) Handles ButtonEnumerate.Click
        FillTextBox()
    End Sub

    Private Sub FillTextBox()
        TextBox1.Text = markov.Take(200).Aggregate(Function(s1, s2) s1 & " " & s2)
    End Sub
End Class