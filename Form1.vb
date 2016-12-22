Public Class Form1

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        getPackTextures()
        GetBlockTexture()
        setBlockColors()
        GetPacks()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If ComboBox1.Items.Contains(ComboBox1.Text) Then ' makes sure the user can't type anything that is ont a pack 
            If ComboBox1.Text = ComboBox1.Items.Item(1) Then ' Checks for (default)
                blockPath = AppFolder()
                usingPack = False
            Else
                blockPath = AppFolder() & "Resource Packs\" & ComboBox1.Text & "\assets\minecraft\textures\"
                usingPack = True
            End If
        End If

        getPackTextures()
        setBlockColors()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button2.Click
        OpenImage.ShowDialog()
    End Sub

    Private Sub OpenImage_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenImage.FileOk
        PictureBox1.ImageLocation = OpenImage.FileName
        getImgColors()
        CreateImageFromBlocks()
        createMCImage()
        Button4.Enabled = True
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button3.Click
        makeSchematic()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        SaveImage.FileName = OpenImage.FileName
        SaveImage.ShowDialog()
    End Sub

    Private Sub SaveImage_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles SaveImage.FileOk
        Select Case ((IO.Path.GetExtension(SaveImage.FileName)).ToLower).ToString 'decides how to encode depending on the extention
            Case ".bmp", ".dib"
                PictureBox2.Image.Save(SaveImage.FileName, System.Drawing.Imaging.ImageFormat.Bmp)
            Case ".wmf", ".emf", "wmz", "emz"
                PictureBox2.Image.Save(SaveImage.FileName, System.Drawing.Imaging.ImageFormat.Emf)
            Case ".gif"
                PictureBox2.Image.Save(SaveImage.FileName, System.Drawing.Imaging.ImageFormat.Gif)
            Case ".ico"
                PictureBox2.Image.Save(SaveImage.FileName, System.Drawing.Imaging.ImageFormat.Icon)
            Case ".jpg", ".jpeg", ".jpe", ".jif", ".jfif", ".jfi"
                PictureBox2.Image.Save(SaveImage.FileName, System.Drawing.Imaging.ImageFormat.Jpeg)
            Case ".png"
                PictureBox2.Image.Save(SaveImage.FileName, System.Drawing.Imaging.ImageFormat.Png)
            Case ".tiff", ".tif"
                PictureBox2.Image.Save(SaveImage.FileName, System.Drawing.Imaging.ImageFormat.Tiff)
            Case Else
                MsgBox("Sorry, that is not a valid format.", MsgBoxStyle.Critical, "Invalid File Type")
        End Select

    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If ColorDialog1.ShowDialog() = DialogResult.OK Then
            TabPage1.BackColor = ColorDialog1.Color
            TabPage2.BackColor = ColorDialog1.Color
        End If
    End Sub

    Private Sub Form1_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        ColorDialog1.Dispose()
        SaveImage.Dispose()
        SaveSchematic.Dispose()
        OpenImage.Dispose()
    End Sub
End Class
