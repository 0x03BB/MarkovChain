Public Module MarkovChainGenerator
    Public Function CreateMarkovChain(corpus As String) As MarkovChain
        If Strings.Left(corpus, 2) <> vbCrLf Then corpus = vbCrLf & corpus
        If Strings.Right(corpus, 2) <> vbCrLf Then corpus = corpus & vbCrLf

        Dim reg = New Text.RegularExpressions.Regex("([-\w']+|[.,;:]|\r\n)", Text.RegularExpressions.RegexOptions.Compiled)
        Dim captures = reg.Matches(corpus)

        Dim dic = New SortedDictionary(Of String, Dictionary(Of String, Integer))

        For i = 0 To captures.Count - 2
            Dim currentString = captures(i).Value
            Dim nextString = captures(i + 1).Value

            If dic.ContainsKey(currentString) Then
                Dim existingEntry = dic(currentString)
                If existingEntry.ContainsKey(nextString) Then
                    existingEntry(nextString) += 1
                Else
                    existingEntry.Add(nextString, 1)
                End If
            Else
                Dim newEntry = New Dictionary(Of String, Integer) From {{nextString, 1}}
                dic.Add(currentString, newEntry)
            End If
        Next

        Dim states = dic.Keys.ToArray

        Dim lookup = New Dictionary(Of String, Integer)
        For i = 0 To dic.Keys.Count - 1
            lookup.Add(states(i), i)
        Next

        Dim chances = New Double(dic.Keys.Count - 1, dic.Keys.Count - 2) {}

        For Each startingPair In dic
            Dim y = lookup(startingPair.Key)
            Dim total = startingPair.Value.Values.Sum
            For Each targetPair In startingPair.Value
                Dim x = lookup(targetPair.Key)
                If x <> dic.Keys.Count - 1 Then
                    chances(y, x) = targetPair.Value / total
                End If
            Next
        Next

        Return New MarkovChain(states, chances)
    End Function
End Module
