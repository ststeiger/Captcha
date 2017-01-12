
Class MathHelpers

    Private Shared seed As New System.Random()


    Public Shared Function rand(min As Integer, max As Integer) As Double
        Return seed.Next(min, max + 1)
    End Function ' rand 


    Public Shared Function addVector(a As Double(), b As Double()) As Double()
        Return New Double() {a(0) + b(0), a(1) + b(1), a(2) + b(2)}
    End Function ' addVector 


    Public Shared Function scalarProduct(vector As Double(), scalar As Double) As Double()
        Return New Double() {vector(0) * scalar, vector(1) * scalar, vector(2) * scalar}
    End Function ' scalarProduct 


    Public Shared Function dotProduct(a As Double(), b As Double()) As Double
        Return a(0) * b(0) + a(1) * b(1) + a(2) * b(2)
    End Function ' dotProduct 


    Public Shared Function norm(vector As Double()) As Double
        Return System.Math.Sqrt(dotProduct(vector, vector))
    End Function ' norm 


    Public Shared Function normalize(vector As Double()) As Double()
        Return scalarProduct(vector, 1 / norm(vector))
    End Function ' normalize 


    ' http://en.wikipedia.org/wiki/Cross_product
    Public Shared Function crossProduct(a As Double(), b As Double()) As Double()
        Return New Double() {(a(1) * b(2) - a(2) * b(1)), (a(2) * b(0) - a(0) * b(2)), (a(0) * b(1) - a(1) * b(0))}
    End Function ' crossProduct 


    Public Shared Function vectorProductIndexed(v As Double(), m As Double(), i As Integer) As Double()
        Return New Double() {v(i + 0) * m(0) + v(i + 1) * m(4) + v(i + 2) * m(8) + v(i + 3) * m(12), v(i + 0) * m(1) + v(i + 1) * m(5) + v(i + 2) * m(9) + v(i + 3) * m(13), v(i + 0) * m(2) + v(i + 1) * m(6) + v(i + 2) * m(10) + v(i + 3) * m(14), v(i + 0) * m(3) + v(i + 1) * m(7) + v(i + 2) * m(11) + v(i + 3) * m(15)}
    End Function ' vectorProductIndexed 


    Public Shared Function vectorProduct(v As Double(), m As Double()) As Double()
        Return vectorProductIndexed(v, m, 0)
    End Function ' vectorProduct 


    Public Shared Function matrixProduct(a As Double(), b As Double()) As Double()
        Dim o1 As Double() = vectorProductIndexed(a, b, 0)
        Dim o2 As Double() = vectorProductIndexed(a, b, 4)
        Dim o3 As Double() = vectorProductIndexed(a, b, 8)
        Dim o4 As Double() = vectorProductIndexed(a, b, 12)

        Return New Double() {o1(0), o1(1), o1(2), o1(3), o2(0), o2(1), _
            o2(2), o2(3), o3(0), o3(1), o3(2), o3(3), _
            o4(0), o4(1), o4(2), o4(3)}
    End Function ' matrixProduct 


    ' http://graphics.idav.ucdavis.edu/education/GraphicsNotes/Camera-Transform/Camera-Transform.html
    Public Shared Function cameraTransform(C As Double(), A As Double()) As Double()
        Dim w As Double() = normalize(addVector(C, scalarProduct(A, -1)))
        Dim y As Double() = New Double() {0, 1, 0}
        Dim u As Double() = normalize(crossProduct(y, w))
        Dim v As Double() = crossProduct(w, u)
        Dim t As Double() = scalarProduct(C, -1)

        Return New Double() {u(0), v(0), w(0), 0, u(1), v(1), _
            w(1), 0, u(2), v(2), w(2), 0, _
            dotProduct(u, t), dotProduct(v, t), dotProduct(w, t), 1}
    End Function ' cameraTransform 


    ' http://graphics.idav.ucdavis.edu/education/GraphicsNotes/Viewing-Transformation/Viewing-Transformation.html
    Public Shared Function viewingTransform(fov As Double, n As Double, f As Double) As Double()
        fov *= (System.Math.PI / 180.0)
        Dim cot As Double = 1 / System.Math.Tan(fov / 2)

        Return New Double() {cot, 0, 0, 0, 0, cot, _
            0, 0, 0, 0, (f + n) / (f - n), -1, _
            0, 0, 2 * f * n / (f - n), 0}
    End Function ' viewingTransform 


End Class ' Helpers 
