﻿Public Class MarkovChain
    Implements IEnumerable(Of String)

    Private ReadOnly States As String()
    Private ReadOnly Chances As Double(,)
    Private ReadOnly StateCount As Integer

    Public Sub New(states As String(), chances As Double(,))
        StateCount = chances.GetLength(0)
        If states.Length <> StateCount Then Throw New ArgumentException("The length of ""strings"" does not equal the first dimension of ""chances""", "chances")
        If StateCount <> chances.GetLength(1) + 1 Then Throw New ArgumentException("The first dimension of ""chances"" is not one larger than the second dimension of ""chances""", "chances")

        Me.States = New String(StateCount - 1) {}
        states.CopyTo(Me.States, 0)

        Me.Chances = New Double(StateCount - 1, StateCount - 2) {}
        For y = 0 To StateCount - 1
            Dim chance As Double = 0R
            For x = 0 To StateCount - 2
                If chances(y, x) < -0.000001R OrElse chances(y, x) > 1.000001R Then Throw New ArgumentException()
                chance += chances(y, x)
                Me.Chances(y, x) = chance
            Next
            chance = 1 - chance
            If chance < -0.000001R OrElse chance > 1.000001R Then Throw New ArgumentException()
        Next
    End Sub

    Public Function GetEnumerator() As IEnumerator(Of String) Implements IEnumerable(Of String).GetEnumerator
        Return New Enumerator(Me)
    End Function

    Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return New Enumerator(Me)
    End Function



    Public Structure Enumerator
        Implements IEnumerator(Of String)

        Private MarkovChain As MarkovChain
        Private State As Integer
        Private Shared rng As Random = New Random

        Friend Sub New(markovChain As MarkovChain)
            Me.MarkovChain = markovChain
            State = -1
        End Sub

        Public ReadOnly Property Current As String Implements IEnumerator(Of String).Current
            Get
                If State = -1 Then Throw New InvalidOperationException("""MoveNext"" must be called after construction or calling ""Reset"" for ""Current"" to have a valid value.")
                Return MarkovChain.States(State)
            End Get
        End Property

        Private ReadOnly Property IEnumerator_Current As Object Implements IEnumerator.Current
            Get
                Return Current
            End Get
        End Property

        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do nothing
        End Sub

        Public Sub Reset() Implements IEnumerator.Reset
            State = -1
        End Sub

        Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
            If State <> -1 Then
                Dim nextState = rng.NextDouble()
                For x = 0 To MarkovChain.StateCount - 2
                    If nextState < MarkovChain.Chances(State, x) Then
                        State = x
                        Return True
                    End If
                Next
                State = MarkovChain.StateCount - 1
                Return True
            Else
                State = 0
                Return True
            End If
        End Function
    End Structure
End Class
