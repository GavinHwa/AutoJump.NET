Imports System.Numerics
Imports AutoJump.Core
Imports AutoJump.Platform.Android
Imports AutoJump.Robot.Simple

Public Class Form1

    Private GameManager As New GameManager
    Private CurrentTapInfo As TapInformation

    '窗体加载事件处理程序
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        GameManager.Robot = New SimpleRobot()
        Dim path = InputBox("Please enter an absolute path to adb.exe", "Adb Path", "E:\Android\SDK\platform-tools\adb.exe")
        GameManager.Device = New AndroidDevice(path)
        Timer2.Start()
    End Sub
    'Label事件处理程序
    Private Sub Label1_DoubleClick(sender As Object, e As EventArgs) Handles Label1.DoubleClick
        Label1.Hide()
    End Sub
    'PictureBox事件处理程序
    Private Sub PictureBox1_DoubleClick(sender As Object, e As EventArgs) Handles PictureBox1.DoubleClick
        Label1.Visible = Not Label1.Visible
    End Sub
    Private Sub PictureBox1_MouseDown(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseDown
        Static Start As Vector2
        Static Target As Vector2
        If e.Button = MouseButtons.Left Then
            Start = New Vector2(e.Location.X, e.Location.Y)
            Console.WriteLine(Start)
        ElseIf e.Button = MouseButtons.Right Then
            Target = New Vector2(e.Location.X, e.Location.Y)
            Console.WriteLine(Target)
            PressByPositionPair(Start, Target)
        End If
    End Sub
    'Button事件处理程序
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        GenerateTapInfo()
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        PressByInfo(CurrentTapInfo)
    End Sub
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Timer1.Interval = 5000
        Timer1.Enabled = Not Timer1.Enabled
        Button2.Enabled = Not Timer1.Enabled
        Button3.Enabled = Not Timer1.Enabled
        Button4.Text = $"Auto:{Timer1.Enabled}"
    End Sub
    'Timer事件处理程序
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        GenerateTapInfo()
        PressByInfo(CurrentTapInfo)
    End Sub
    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        Dim enabled = GameManager.Device.Availiable
        Panel1.Enabled = enabled
        PictureBox1.Visible = enabled
        If Not enabled AndAlso Timer1.Enabled Then
            Timer1.Enabled = False
            Button4.Text = $"Auto:{Timer1.Enabled}"
        End If
        ToolStripStatusLabel2.Text = $"Device:{If(enabled, "Connect", "Null")}"
    End Sub

    ''' <summary>
    ''' 由指定的起始点和终止点触压屏幕
    ''' </summary>
    Private Sub PressByPositionPair(start As Vector2, target As Vector2)
        Dim distance As Single = (target - start).Length
        Dim duration As Integer = CInt(distance * 4)
        GameManager.Device.Press(New Vector2(100, 100), duration)
        Console.WriteLine($"两点距离：{distance}，触压时间：{duration}")
    End Sub
    ''' <summary>
    ''' 由指定的<see cref="TapInformation"/>对象触压屏幕
    ''' </summary>
    Private Sub PressByInfo(info As TapInformation)
        GameManager.Device.Press(info.Position, info.Duration)
    End Sub
    ''' <summary>
    ''' 生成触压信息
    ''' </summary>
    Private Sub GenerateTapInfo()
        Dim Image = GetImageFromDevice()
        CurrentTapInfo = GameManager.Robot.GetNextTap(Image)
        PictureBox1.BackgroundImage?.Dispose()
        PictureBox1.BackgroundImage = Image
    End Sub
    ''' <summary>
    ''' 返回移动设备的屏幕图像
    ''' </summary>
    Private Function GetImageFromDevice() As Bitmap
        Return Convert(GameManager.Device.GetScreenImage, New Rectangle(0, 0, PictureBox1.Width, PictureBox1.Height))
    End Function
    ''' <summary>
    ''' 转换指定图像为目标矩形大小
    ''' </summary>
    Private Function Convert(image As Bitmap, destRect As Rectangle) As Bitmap
        Dim result As New Bitmap(destRect.Width, destRect.Height)
        Using pg = Graphics.FromImage(result)
            pg.DrawImage(image, destRect)
        End Using
        Return result
    End Function
End Class
