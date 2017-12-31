Imports System.Drawing
''' <summary>
''' 游戏机器人接口
''' </summary>
Public Interface IGameRobot
    ''' <summary>
    ''' 下一次点击
    ''' </summary>
    Function GetNextTap(image As Bitmap) As TapInformation
End Interface
