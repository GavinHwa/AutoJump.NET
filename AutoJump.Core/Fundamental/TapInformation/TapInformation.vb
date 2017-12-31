Imports System.Numerics
''' <summary>
''' 触压信息
''' </summary>
Public Class TapInformation
    ''' <summary>
    ''' 位置
    ''' </summary>
    Public Property Position As Vector2
    ''' <summary>
    ''' 持续毫秒数
    ''' </summary>
    Public Property Duration As Integer
    ''' <summary>
    ''' 创建并初始化一个实例
    ''' </summary>
    Public Sub New(position As Vector2, duration As Integer)
        Me.Position = position
        Me.Duration = duration
    End Sub
End Class
