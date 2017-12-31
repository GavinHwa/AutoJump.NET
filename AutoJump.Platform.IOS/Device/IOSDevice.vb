Imports System.Drawing
Imports System.Numerics
Imports AutoJump.Core
''' <summary>
''' 表示一个实现了<see cref="IDevice"/>的IOS设备
''' </summary>
Public Class IOSDevice
    Implements IDevice

    Public ReadOnly Property Availiable As Boolean Implements IDevice.Availiable
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public Sub Press(position As Vector2, millionseconds As Integer) Implements IDevice.Press
        Throw New NotImplementedException()
    End Sub

    Public Function GetScreenImage() As Image Implements IDevice.GetScreenImage
        Throw New NotImplementedException()
    End Function
End Class
