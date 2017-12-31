Imports System.Drawing
Imports System.Numerics
Imports AutoJump.Core
''' <summary>
''' 表示一个实现了<see cref="IDevice"/>的Android设备
''' </summary>
Public Class AndroidDevice
    Implements IDevice
    Public ReadOnly Property Availiable As Boolean Implements IDevice.Availiable
        Get
            Return CheckConnection()
        End Get
    End Property
    ''' <summary>
    ''' Adb路径
    ''' </summary>
    Public Property AdbPath As String

    ''' <summary>
    ''' 创建并初始化一个实例
    ''' </summary>
    Public Sub New(adbPath As String)
        Me.AdbPath = adbPath
    End Sub

    Public Sub Press(position As Vector2, millionseconds As Integer) Implements IDevice.Press
        Dim target = position + New Vector2(100, 100)
        Command($"shell input swipe {position.X} {position.Y} {target.X} {target.Y} {millionseconds}")
    End Sub
    Public Function GetScreenImage() As Image Implements IDevice.GetScreenImage
        Dim result As Bitmap = Nothing
        Dim tempFileName = "temp.png"
        Command("shell screencap -p /sdcard/" & tempFileName)
        Command("pull /sdcard/" & tempFileName)
        If System.IO.File.Exists(tempFileName) Then
            Using img = Image.FromFile(tempFileName)
                result = New Bitmap(img)
            End Using
            GC.Collect()
            If System.IO.File.Exists(tempFileName) Then
                Try
                    System.IO.File.Delete(Environment.CurrentDirectory + tempFileName)
                Catch
                End Try
            End If
        End If
        Return result
    End Function

    ''' <summary>
    ''' 检测设备连接
    ''' </summary>
    Private Function CheckConnection() As Boolean
        Dim text = Command("shell getprop ro.product.model")
        If text.Contains("no devices") OrElse String.IsNullOrWhiteSpace(text) Then
            Return False
        Else
            Return True
        End If
    End Function
    ''' <summary>
    ''' 向Adb发送指定的参数命令
    ''' </summary>
    Private Function Command(ByVal arguments As String) As String
        Dim result As String = String.Empty
        Using p As Process = New Process()
            p.StartInfo.FileName = Me.AdbPath
            p.StartInfo.Arguments = arguments
            p.StartInfo.UseShellExecute = False
            p.StartInfo.RedirectStandardInput = True
            p.StartInfo.RedirectStandardOutput = True
            p.StartInfo.RedirectStandardError = True
            p.StartInfo.CreateNoWindow = True
            p.Start()
            result = p.StandardOutput.ReadToEnd()
            p.Close()
        End Using
        Return result
    End Function
End Class
