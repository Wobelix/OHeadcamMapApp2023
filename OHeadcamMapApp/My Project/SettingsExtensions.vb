Namespace My
    Partial Friend NotInheritable Class MySettings
        Public Property MISmoothFrameStepSeconds() As Double
            Get
                Return CType(Me("MISmoothFrameStepSeconds"), Double)
            End Get
            Set(value As Double)
                Me("MISmoothFrameStepSeconds") = value
            End Set
        End Property

        Public Property MISmoothParallelGeneration() As Boolean
            Get
                Return CType(Me("MISmoothParallelGeneration"), Boolean)
            End Get
            Set(value As Boolean)
                Me("MISmoothParallelGeneration") = value
            End Set
        End Property
    End Class
End Namespace
