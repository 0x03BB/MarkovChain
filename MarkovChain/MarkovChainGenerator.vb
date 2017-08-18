Public Module MarkovChainGenerator
    Public Function CreateMarkovChain(corpus As String) As MarkovChain
        Dim reg = New Text.RegularExpressions.Regex("([-\w]+|[.,;:])", Text.RegularExpressions.RegexOptions.Compiled)
        Dim captures = reg.Matches(corpus)

        Dim dic = New SortedDictionary(Of String, SortedDictionary(Of String, Integer))

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
                Dim newEntry = New SortedDictionary(Of String, Integer) From {{nextString, 1}}
                dic.Add(currentString, newEntry)
            End If
        Next


    End Function
End Module
