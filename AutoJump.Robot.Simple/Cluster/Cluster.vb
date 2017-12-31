Imports System.Drawing
Imports System.Numerics
''' <summary>
''' 聚类
''' </summary>
Public Class Cluster
    ''' <summary>
    ''' 顶点集
    ''' </summary>
    Public Property Vertices As New List(Of Vertex)

    ''' <summary>
    ''' 返回当前聚类的中心位置
    ''' </summary>
    Public Function GetCenter() As Vector2
        Dim result As New Vector2
        If (Vertices.Count > 0) Then
            For i = 0 To Vertices.Count - 1
                result += Vertices(i).Position
            Next
            result /= Vertices.Count
        End If
        Return result
    End Function
    ''' <summary>
    ''' 返回当前聚类的包围矩形
    ''' </summary>
    Public Function GetRect() As Rectangle
        Dim minX = Vertices.Min(Function(v) v.Position.X)
        Dim minY = Vertices.Min(Function(v) v.Position.Y)
        Dim maxX = Vertices.Max(Function(v) v.Position.X)
        Dim maxY = Vertices.Max(Function(v) v.Position.Y)
        Return New Rectangle(minX, minY, maxX - minX, maxY - minY)
    End Function
End Class
