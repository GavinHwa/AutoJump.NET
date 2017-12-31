Imports System.Numerics
''' <summary>
''' 位置组合
''' </summary>
Public Class PositionPair
    ''' <summary>
    ''' 起始点
    ''' </summary>
    Public Property Start As Vector2
    ''' <summary>
    ''' 目标点
    ''' </summary>
    Public Property Target As Vector2
    ''' <summary>
    ''' 返回两个位置之间的距离
    ''' </summary>
    Public ReadOnly Property Distance As Single
        Get
            Return (Target - Start).Length
        End Get
    End Property

    ''' <summary>
    ''' 创建并初始化一个实例
    ''' </summary>
    Public Sub New(start As Vector2, target As Vector2)
        Me.Start = start
        Me.Target = target
    End Sub
End Class
