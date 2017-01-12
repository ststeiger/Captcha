

'
' * Original version by Stephen Toub and Shawn Farkas.
' * Random pool and thread safety added by Markus Olsson (freakcode.com).
' * 
' * Original source: http://msdn.microsoft.com/en-us/magazine/cc163367.aspx
' * 
' * Some benchmarks (2009-03-18):
' * 
' *  Results produced by calling Next() 1 000 000 times on my machine (dual core 3Ghz)
' * 
' *      System.Random completed in 20.4993 ms (avg 0 ms) (first: 0.3454 ms)
' *      CryptoRandom with pool completed in 132.2408 ms (avg 0.0001 ms) (first: 0.025 ms)
' *      CryptoRandom without pool completed in 2 sec 587.708 ms (avg 0.0025 ms) (first: 1.4142 ms)
' *      
' *      |---------------------|------------------------------------|
' *      | Implementation      | Slowdown compared to System.Random |
' *      |---------------------|------------------------------------|
' *      | System.Random       | 0                                  |
' *      | CryptoRand w pool   | 6,6x                               |
' *      | CryptoRand w/o pool | 19,5x                              |
' *      |---------------------|------------------------------------|
' * 
' * ent (http://www.fourmilab.ch/) results for 16mb of data produced by this class:
' * 
' *  > Entropy = 7.999989 bits per byte.
' *  >
' *  > Optimum compression would reduce the size of this 16777216 byte file by 0 percent.
' *  >
' *  > Chi square distribution for 16777216 samples is 260.64, 
' *  > and randomly would exceed this value 50.00 percent of the times.
' *  >
' *  > Arithmetic mean value of data bytes is 127.4974 (127.5 = random).
' *  > Monte Carlo value for Pi is 3.141838823 (error 0.01 percent).
' *  > Serial correlation coefficient is 0.000348 (totally uncorrelated = 0.0).
' * 
' *  your mileage may vary ;)
' *  
' 



Imports System.Diagnostics.CodeAnalysis
Imports System.Security.Cryptography


Namespace Captcha.Cryptography
    ''' <summary>
    ''' A random number generator based on the RNGCryptoServiceProvider.
    ''' Adapted from the "Tales from the CryptoRandom" article in MSDN Magazine (September 2007)
    ''' but with explicit guarantee to be thread safe. Note that this implementation also includes
    ''' an optional (enabled by default) random buffer which provides a significant speed boost as
    ''' it greatly reduces the amount of calls into unmanaged land.
    ''' </summary>
    Public Class CryptoRandom
        Inherits System.Random
        Implements System.IDisposable
        Private _rng As New RNGCryptoServiceProvider()

        Private _buffer As Byte()

        Private _bufferPosition As Integer

        ''' <summary>
        ''' Gets a value indicating whether this instance has random pool enabled.
        ''' </summary>
        ''' <value>
        '''     <c>true</c> if this instance has random pool enabled; otherwise, <c>false</c>.
        ''' </value>
        Public Property IsRandomPoolEnabled() As Boolean
            Get
                Return m_IsRandomPoolEnabled
            End Get
            Private Set
                m_IsRandomPoolEnabled = Value
            End Set
        End Property
        Private m_IsRandomPoolEnabled As Boolean

        ''' <summary>
        ''' Initializes a new instance of the <see cref="CryptoRandom"/> class with.
        ''' Using this overload will enable the random buffer pool.
        ''' </summary>
        Public Sub New()
            Me.New(True)
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="CryptoRandom"/> class.
        ''' This method will disregard whatever value is passed as seed and it's only implemented
        ''' in order to be fully backwards compatible with <see cref="System.Random"/>.
        ''' Using this overload will enable the random buffer pool.
        ''' </summary>
        ''' <param name="ignoredSeed">The ignored seed.</param>
        <SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId:="ignoredSeed", Justification:="Cannot remove this parameter as we implement the full API of System.Random")>
        Public Sub New(ignoredSeed As Integer)
            Me.New(True)
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="CryptoRandom"/> class with
        ''' optional random buffer.
        ''' </summary>
        ''' <param name="enableRandomPool">set to <c>true</c> to enable the random pool buffer for increased performance.</param>
        Public Sub New(enableRandomPool As Boolean)
            IsRandomPoolEnabled = enableRandomPool
        End Sub

        Private Sub InitBuffer()
            If IsRandomPoolEnabled Then
                If _buffer Is Nothing OrElse _buffer.Length <> 512 Then
                    _buffer = New Byte(511) {}
                End If
            Else
                If _buffer Is Nothing OrElse _buffer.Length <> 4 Then
                    _buffer = New Byte(3) {}
                End If
            End If

            _rng.GetBytes(_buffer)
            _bufferPosition = 0
        End Sub

        ''' <summary>
        ''' Returns a nonnegative random number.
        ''' </summary>
        ''' <returns>
        ''' A 32-bit signed integer greater than or equal to zero and less than <see cref="F:System.Int32.MaxValue"/>.
        ''' </returns>
        Public Overrides Function [Next]() As Integer
            ' Mask away the sign bit so that we always return nonnegative integers
            Return CInt(GetRandomUInt32()) And &H7FFFFFFF
        End Function

        ''' <summary>
        ''' Returns a nonnegative random number less than the specified maximum.
        ''' </summary>
        ''' <param name="maxValue">The exclusive upper bound of the random number to be generated. <paramref name="maxValue"/> must be greater than or equal to zero.</param>
        ''' <returns>
        ''' A 32-bit signed integer greater than or equal to zero, and less than <paramref name="maxValue"/>; that is, the range of return values ordinarily includes zero but not <paramref name="maxValue"/>. However, if <paramref name="maxValue"/> equals zero, <paramref name="maxValue"/> is returned.
        ''' </returns>
        ''' <exception cref="T:System.ArgumentOutOfRangeException">
        '''     <paramref name="maxValue"/> is less than zero.
        ''' </exception>
        Public Overrides Function [Next](maxValue As Integer) As Integer
            If maxValue < 0 Then
                Throw New ArgumentOutOfRangeException("maxValue")
            End If

            Return [Next](0, maxValue)
        End Function

        ''' <summary>
        ''' Returns a random number within a specified range.
        ''' </summary>
        ''' <param name="minValue">The inclusive lower bound of the random number returned.</param>
        ''' <param name="maxValue">The exclusive upper bound of the random number returned. <paramref name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>.</param>
        ''' <returns>
        ''' A 32-bit signed integer greater than or equal to <paramref name="minValue"/> and less than <paramref name="maxValue"/>; that is, the range of return values includes <paramref name="minValue"/> but not <paramref name="maxValue"/>. If <paramref name="minValue"/> equals <paramref name="maxValue"/>, <paramref name="minValue"/> is returned.
        ''' </returns>
        ''' <exception cref="T:System.ArgumentOutOfRangeException">
        '''     <paramref name="minValue"/> is greater than <paramref name="maxValue"/>.
        ''' </exception>
        Public Overrides Function [Next](minValue As Integer, maxValue As Integer) As Integer
            If minValue > maxValue Then
                Throw New ArgumentOutOfRangeException("minValue")
            End If

            If minValue = maxValue Then
                Return minValue
            End If

            Dim diff As Long = maxValue - minValue

            While True
                Dim rand As UInteger = GetRandomUInt32()

                Dim max As Long = 1 + CLng(UInteger.MaxValue)
                Dim remainder As Long = max Mod diff

                If rand < max - remainder Then
                    Return CInt(minValue + (rand Mod diff))
                End If
            End While
        End Function

        ''' <summary>
        ''' Returns a random number between 0.0 and 1.0.
        ''' </summary>
        ''' <returns>
        ''' A double-precision floating point number greater than or equal to 0.0, and less than 1.0.
        ''' </returns>
        Public Overrides Function NextDouble() As Double
            Return GetRandomUInt32() / (1.0 + UInteger.MaxValue)
        End Function

        ''' <summary>
        ''' Fills the elements of a specified array of bytes with random numbers.
        ''' </summary>
        ''' <param name="buffer">An array of bytes to contain random numbers.</param>
        ''' <exception cref="T:System.ArgumentNullException">
        '''     <paramref name="buffer"/> is null.
        ''' </exception>
        Public Overrides Sub NextBytes(buffer__1 As Byte())
            If buffer__1 Is Nothing Then
                Throw New ArgumentNullException("buffer")
            End If

            SyncLock Me
                If IsRandomPoolEnabled AndAlso _buffer Is Nothing Then
                    InitBuffer()
                End If

                ' Can we fit the requested number of bytes in the buffer?
                If IsRandomPoolEnabled AndAlso _buffer.Length <= buffer__1.Length Then
                    Dim count As Integer = buffer__1.Length

                    EnsureRandomBuffer(count)

                    Buffer.BlockCopy(_buffer, _bufferPosition, buffer__1, 0, count)

                    _bufferPosition += count
                Else
                    ' Draw bytes directly from the RNGCryptoProvider
                    _rng.GetBytes(buffer__1)
                End If
            End SyncLock
        End Sub

        ''' <summary>
        ''' Gets one random unsigned 32bit integer in a thread safe manner.
        ''' </summary>
        Private Function GetRandomUInt32() As UInteger
            SyncLock Me
                EnsureRandomBuffer(4)

                Dim rand As UInteger = BitConverter.ToUInt32(_buffer, _bufferPosition)

                _bufferPosition += 4

                Return rand
            End SyncLock
        End Function

        ''' <summary>
        ''' Ensures that we have enough bytes in the random buffer.
        ''' </summary>
        ''' <param name="requiredBytes">The number of required bytes.</param>
        Private Sub EnsureRandomBuffer(requiredBytes As Integer)
            If _buffer Is Nothing Then
                InitBuffer()
            End If

            If requiredBytes > _buffer.Length Then
                Throw New ArgumentOutOfRangeException("requiredBytes", "cannot be greater than random buffer")
            End If

            If (_buffer.Length - _bufferPosition) < requiredBytes Then
                InitBuffer()
            End If
        End Sub


        Private Sub IDisposable_Dispose() Implements IDisposable.Dispose
            Me.Dispose(True)
            System.GC.SuppressFinalize(Me)
        End Sub

        Public Disposed As Boolean = False

        Protected Overridable Sub Dispose(dispose__1 As Boolean)
            If dispose__1 Then
                If _buffer IsNot Nothing Then
                    System.Array.Clear(_buffer, 0, _buffer.Length)
                    _buffer = Nothing
                End If

                If _rng IsNot Nothing Then
                    _rng.Dispose()
                End If
            End If

            Me.Disposed = True
        End Sub


    End Class
End Namespace
