Imports System.Drawing
Imports System.Numerics
''' <summary>
''' 表示具有颜色和位置的顶点
''' </summary>
Public Class Vertex
    ''' <summary>
    ''' 位置
    ''' </summary>
    Public Property Position As Vector2
    ''' <summary>
    ''' 颜色
    ''' </summary>
    Public Property Color As Color
    ''' <summary>
    ''' 创建并初始化一个实例
    ''' </summary>
    Public Sub New(positon As Vector2, color As Color)
        Me.Position = positon
        Me.Color = color
    End Sub
End Class
