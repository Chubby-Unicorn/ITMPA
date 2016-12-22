Imports Substrate
Module Module1
    Structure TypeColor
        Dim Red As Byte
        Dim Green As Byte
        Dim Blue As Byte
        Dim Alpha As Boolean
    End Structure

    Structure TypeBlocks
        Dim used As Boolean
        Dim InPack As Boolean
        Dim RGB As TypeColor
        Dim Texture As String
    End Structure

    Structure TypeImage
        Dim RGB As TypeColor
        Dim Block As Byte 'Block ID
        Dim Data As Byte  'Data of the block (ex: red wool is 14 (35:14))
    End Structure

    Structure TypeIMG
        Dim Width As Integer 'Width of the image
        Dim Height As Integer 'Height of the image
    End Structure

    Public Blocks(255, 15) As TypeBlocks
    Public blockPath As String = AppFolder()
    Public Img(0, 0) As TypeImage
    Public IMGSize As TypeIMG
    Public usingPack As Boolean = False

    Public Function Abs(ByRef value As Integer) As Integer
        If value < 0 Then
            value = value * -1
        End If
        Return value
    End Function

    Function AppFolder() As String
        Dim P As String
        P = Application.ExecutablePath
        P = IO.Path.GetFullPath(IO.Path.GetDirectoryName(P))
        If P.Last = "\" Then
        Else
            P = P & "\"
        End If
        P = P & "ITMPA Files\"
        Return P
    End Function

    Public Sub GetBlockTexture()
        Dim Loc As String
        'Loading in the variables
        Loc = AppFolder() & "Texture.txt"
FileTest:
        Dim temp As New List(Of String)
        If IO.File.Exists(Loc) Then
            Dim strFileName() As String 'String Array.
            strFileName = IO.File.ReadAllLines(Loc) 'add each line as String Array.
            For Each myLine In strFileName ' loop thru Arrays.
                temp.Add(myLine)
            Next
        Else
            IO.File.CreateText(Loc)
            GoTo FileTest
        End If

        Dim VarString As String
        Dim ValString As String
        Dim DatString As String

        For i = 0 To temp.Count - 1
            VarString = temp.Item(i)
            ValString = temp.Item(i)
            While VarString.Contains("=")
                VarString = VarString.Substring(0, VarString.Length - 1)
            End While
            VarString = VarString.Substring(0, VarString.Length - 1)

            DatString = VarString
            While DatString.Contains(":")
                DatString = DatString.Substring(1, DatString.Length - 1)
            End While

            While VarString.Contains(":")
                VarString = VarString.Substring(0, VarString.Length - 1)
            End While

            While ValString.Contains("=")
                ValString = ValString.Substring(1, ValString.Length - 1)
            End While
            ValString = ValString.Substring(1, ValString.Length - 1)

            Dim int1, int2 As Byte
            int1 = Val(VarString)
            int2 = Val(DatString)
            If ValString.ToLower = "unused" Then
                Blocks(int1, int2).used = False
                Blocks(int1, int2).Texture = ""
            Else
                Blocks(int1, int2).used = True
                Blocks(int1, int2).Texture = ValString
            End If


        Next i
    End Sub

    Public Sub setBlockColors()
        For B = 1 To 255
            For D = 0 To 15
                If Blocks(B, D).used Then
                    Dim tex As String
                    If usingPack Then
                        If Blocks(B, D).InPack Then
                            tex = blockPath & "blocks\" & Blocks(B, D).Texture & ".png"
                        Else
                            tex = AppFolder() & "blocks\" & Blocks(B, D).Texture & ".png"
                        End If
                    Else
                        tex = AppFolder() & "blocks\" & Blocks(B, D).Texture & ".png"
                    End If
                    Dim clrR As ULong
                    Dim clrG As ULong
                    Dim clrB As ULong
                    Dim xmax As UInteger
                    Dim ymax As UInteger
                    Dim x As Integer
                    Dim y As Integer
                    clrR = 0
                    clrG = 0
                    clrB = 0
                    Dim bm As New Bitmap(tex)
                    xmax = bm.Width - 1
                    ymax = bm.Height - 1
                    For y = 0 To ymax
                        For x = 0 To xmax
                            clrR = clrR + bm.GetPixel(x, y).R
                            clrG = clrG + bm.GetPixel(x, y).G
                            clrB = clrB + bm.GetPixel(x, y).B
                        Next x
                    Next y
                    Blocks(B, D).RGB.Red = clrR / (bm.Width * bm.Height)
                    Blocks(B, D).RGB.Green = clrG / (bm.Width * bm.Height)
                    Blocks(B, D).RGB.Blue = clrB / (bm.Width * bm.Height)
                End If
            Next D
        Next B
    End Sub

    Public Sub getImgColors() 'Gathers all of the images pixels as individual RGB values
        Dim bm As New Bitmap(Form1.OpenImage.FileName)
        bm.RotateFlip(RotateFlipType.RotateNoneFlipY) 'I flip the image so the image will be right-side-up
        ' scimatic (0, 0) point is at the lower left, while an image's (0,0) is at the upper right
        IMGSize.Width = bm.Width - 1
        IMGSize.Height = bm.Height - 1
        ReDim Img(IMGSize.Width, IMGSize.Height)
        Dim x, y As UInteger
        x = 0
        y = 0
        For y = 0 To IMGSize.Height
            For x = 0 To IMGSize.Width
                Img(x, y).RGB.Red = bm.GetPixel(x, y).R 'Red 
                Img(x, y).RGB.Green = bm.GetPixel(x, y).G 'Green values
                Img(x, y).RGB.Blue = bm.GetPixel(x, y).B 'Blue values
                If bm.GetPixel(x, y).A = 0 Then
                    Img(x, y).RGB.Alpha = True 'Alpha = transparent
                Else
                    Img(x, y).RGB.Alpha = False
                End If
            Next x
        Next y
    End Sub

    Public Sub GetPacks()
        Dim Folders As String = AppFolder() & "Resource Packs\"
        Dim pac As New List(Of String)

        For Each path In IO.Directory.GetDirectories(Folders)
            pac.Add(path)
        Next

        For i = 0 To pac.Count - 1
            If IO.Directory.Exists(Slash(pac.Item(i)) & "assets\minecraft\textures\blocks") Then
                Dim tPath As String = pac.Item(i)
                While tPath.Contains("\")
                    tPath = tPath.Remove(0, 1)
                End While
                Form1.ComboBox1.Items.Add(tPath)
            End If
        Next
    End Sub

    Public Function Slash(P As String) As String 'adds the slash only if it isn't there
        If P.EndsWith("\") Then
        Else
            P = P & "\"
        End If
        Return P
    End Function

    Public Sub getPackTextures() ' gets the proper texture for each block
        For B = 1 To 255
            For D = 0 To 15
                If Blocks(B, D).used Then
                    If IO.File.Exists(blockPath & "blocks\" & Blocks(B, D).Texture & ".png") Then
                        Blocks(B, D).InPack = True
                    Else
                        GoTo Unused
                    End If
                End If
            Next D
Unused:
        Next B
    End Sub

    Public Sub CreateImageFromBlocks()
        'Fun fackt: when editing this I accedentaly had a stray 'redim' statment causing major headachs
        'Redimming deleats all the old values in a sequence while resizing
        For y = 0 To IMGSize.Height

            For x = 0 To IMGSize.Width
                If Img(x, y).RGB.Alpha = True Then
                    Img(x, y).Block = 0 ' Air
                    Img(x, y).Data = 0
                Else
                    Dim CLR As UInteger
                    CLR = 1000 'the smaller the number becomes the closer of a resembalance the block will be
                    For B = 1 To 255
                        For D = 0 To 15
                            If Blocks(B, D).used Then
                                Dim C As Short
                                C = 0
                                C = Math.Abs(Int(Img(x, y).RGB.Red) - Int(Blocks(B, D).RGB.Red)) + Math.Abs(Int(Img(x, y).RGB.Green) - Int(Blocks(B, D).RGB.Green)) + Math.Abs(Int(Img(x, y).RGB.Blue) - Int(Blocks(B, D).RGB.Blue))
                                If C < CLR Then 'how the number gets smaller
                                    CLR = C
                                    Img(x, y).Block = B
                                    Img(x, y).Data = D
                                End If
                            Else
                                GoTo Unused
                            End If
                        Next D
Unused:
                    Next B
                End If
            Next x
        Next y
    End Sub

    Public Sub makeSchematic()
        Dim world As New ImportExport.Schematic(IMGSize.Width + 1, IMGSize.Height + 1, 1)
        For y = 0 To IMGSize.Height
            For x = 0 To IMGSize.Width
                world.Blocks.SetBlock(x, y, 0, New AlphaBlock(Img(x, y).Block, Img(x, y).Data)) ' should make a structure
            Next x
        Next y
        If Form1.SaveSchematic.ShowDialog() = DialogResult.OK Then
            world.Export(Form1.SaveSchematic.FileName)
        End If
    End Sub

    Public Sub createMCImage()
        Dim im As New Bitmap(Form1.PictureBox1.ImageLocation)
        For y = 0 To im.Height - 1
            For x = 0 To im.Width - 1
                If (Img(x, y).RGB.Alpha) Then
                    im.SetPixel(x, y, Color.Transparent) 'Yes, transparent is a color, It just has no alpha
                Else
                    im.SetPixel(x, y, ColorTranslator.FromOle(RGB((Blocks(Img(x, y).Block, Img(x, y).Data).RGB.Red), (Blocks(Img(x, y).Block, Img(x, y).Data).RGB.Green), (Blocks(Img(x, y).Block, Img(x, y).Data).RGB.Blue))))
                End If
            Next x
        Next y
        im.RotateFlip(RotateFlipType.RotateNoneFlipY)
        Form1.PictureBox2.Image = im
    End Sub
End Module
