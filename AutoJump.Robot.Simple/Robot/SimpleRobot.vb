Imports System.Drawing
Imports System.Numerics
Imports AutoJump.Core
''' <summary>
''' 简单的机器人
''' </summary>
Public Class SimpleRobot
    Implements IGameRobot

    Const PercentOffsetX As Single = 1 / 30
    Const PercentOffsetY As Single = 1 / 3

    Const PercentUponX As Single = 29 / 30
    Const PercentUponY As Single = 2 / 3

    Const PercentCharacter As Single = 1.2 / 32

    Const PercentFlagOffset As Single = 1 / 5

    Const PercentDistance As Single = 2

    Public Function GetNextTap(image As Bitmap) As TapInformation Implements IGameRobot.GetNextTap
        Dim pair As PositionPair = Solve(image)
        Return New TapInformation(New Vector2(100, 100), pair.Distance * PercentDistance)
    End Function

    Private Function Solve(image As Bitmap) As PositionPair
        Dim result As PositionPair
        Dim width = image.Width
        Dim height = image.Height
        Using pg = Graphics.FromImage(image)
            Dim offsetX As Integer = width * PercentOffsetX
            Dim offsetY As Integer = height * PercentOffsetY
            Dim uponX = width * PercentUponX
            Dim uponY = height * PercentUponY

            '生成聚类
            Dim clusters = GenerateClusters(image, offsetX, offsetY, uponX, uponY)
            Dim cluster1 As Cluster = clusters.Item1
            Dim cluster2 As Cluster = clusters.Item2

            '聚类中心(目标落点）
            Dim center1 = cluster1.GetCenter()
            Console.WriteLine($"cluster:{cluster1.Vertices.Count}")
            '聚类中心(小人位置）
            Dim center2 = cluster2.GetCenter() + New Vector2(0, height * PercentCharacter)
            Console.WriteLine($"cluster2:{cluster2.Vertices.Count}")

            '绘制聚类
            DrawCluster(image, Pens.Blue, cluster1)
            DrawCluster(image, Pens.Green, cluster2)
            '绘制小人与目标落点的连线
            pg.DrawLine(Pens.Red, center1.X, center1.Y, center2.X, center2.Y)
            '绘制聚类矩形框
            pg.DrawRectangle(New Pen(Color.FromArgb(100, 0, 0, 255), 4), cluster1.GetRect)
            pg.DrawRectangle(New Pen(Color.FromArgb(100, 0, 255, 0), 4), cluster2.GetRect)
            '绘制聚类中心
            Dim radius As Single = 4
            pg.FillEllipse(Brushes.Yellow, center1.X - radius, center1.Y - radius, radius * 2, radius * 2)
            pg.FillEllipse(Brushes.Yellow, center2.X - radius, center2.Y - radius, radius * 2, radius * 2)
            '绘制水平参考线
            pg.DrawLine(Pens.Black, 0, offsetY, width, offsetY)
            pg.DrawLine(Pens.Black, 0, uponY, width, uponY)
            '绘制垂直参考线
            pg.DrawLine(Pens.Black, offsetX, 0, offsetX, height)
            pg.DrawLine(Pens.Black, uponX, 0, uponX, height)

            result = New PositionPair(center1, center2)
        End Using

        Return result
    End Function

    ''' <summary>
    ''' 返回图像指定区域的聚类集合识别结果
    ''' </summary>
    Private Function GenerateClusters(image As Bitmap, offsetX As Integer, offsetY As Integer, uponX As Integer, uponY As Integer) As Tuple(Of Cluster, Cluster)
        Dim characterColor = Color.FromArgb(255, 54, 57, 100)

        Dim width = image.Width
        Dim height = image.Height

        Dim flag1 As New Vector2
        Dim flag2 As New Vector2

        Dim over1 As Boolean
        Dim over2 As Boolean
        For j = offsetY To uponY
            For i = offsetX To uponX
                Dim current = image.GetPixel(i, j)
                If Not over1 Then
                    '搜索落点顶端像素
                    If ColorHelper.CompareBaseRGB(current, image.GetPixel(i, j - 1), 10) = False Then
                        If ColorHelper.CompareBaseRGB(current, image.GetPixel(i - 1, j), 10) = False Then
                            flag1 = New Vector2(i, j + 3)
                            over1 = True
                        End If
                    End If
                Else
                    '搜索小人顶端像素
                    If ColorHelper.CompareBaseRGB(current, characterColor, 25) = True Then
                        flag2 = New Vector2(i, j)
                        over2 = True
                    End If
                End If
                If over2 Then
                    Exit For
                End If
            Next
        Next

        Dim c1 = image.GetPixel(flag1.X, flag1.Y)
        Dim c2 = image.GetPixel(flag2.X, flag2.Y)

        Dim flagOffset = height * PercentFlagOffset

        Dim upon1 = flag1.Y + flagOffset
        Dim upon2 = flag2.Y + flagOffset

        upon1 = If(upon1 > flag2.Y, flag2.Y, upon1)
        upon1 = If(upon1 - flag1.Y < flagOffset / 2, flag1.Y + flagOffset / 2, upon1)
        upon2 = If(upon2 > uponY, uponY, upon2)

        '生成聚类
        Dim cluster1 = GetCluster(image, c1, 0, uponX, flag1.Y, upon1, 15)
        Dim cluster2 = GetCluster(image, c2, 0, uponX, flag2.Y, upon2, 20)

        Return New Tuple(Of Cluster, Cluster)(cluster1, cluster2)
    End Function

    ''' <summary>
    ''' 返回指定区域内与指定颜色相似的聚类
    ''' </summary>
    Private Function GetCluster(image As Bitmap, target As Color, lowerX As Integer, uponX As Integer, lowerY As Integer, uponY As Integer, Optional distance As Integer = 20) As Cluster
        Dim result As New Cluster
        For j = lowerY To uponY
            For i = lowerX To uponX
                Dim current As Color = image.GetPixel(i, j)
                If ColorHelper.CompareBaseRGB(current, target, distance) Then
                    result.Vertices.Add(New Vertex(New Vector2(i, j), current))
                End If
            Next
        Next
        Return result
    End Function

    ''' <summary>
    ''' 绘制聚类
    ''' </summary>
    Private Sub DrawCluster(image As Bitmap, pen As Pen, cluster As Cluster)
        If (cluster.Vertices.Count > 0) Then
            Using pg = Graphics.FromImage(image)
                For i = 0 To cluster.Vertices.Count - 1
                    Dim position = cluster.Vertices(i).Position
                    pg.DrawRectangle(pen, Position.X, Position.Y, 1, 1)
                Next
            End Using
        End If
    End Sub

End Class
