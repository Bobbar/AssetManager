Namespace ImageCaching
    ''' <summary>
    '''  These methods allow me to add a single line after the 
    '''  InitializeComponent call in a Forms constructor that will efficiently reassign 
    '''  all control images set from the recklessly leaky 'spawn-a-new-instance-of-every-object' - ResourceManager.GetObject method.
    '''  By using a global HashTable to contain and pass out single instances of the images, I can  
    '''  reduce memory usage by comical orders of magnitude.
    '''  ResourceManager.GetObject can "Die a prolonged and relentlessly agonizing death." 
    ''' </summary>
    Module CachedImages
        Private ImageCacheHashTable As New Hashtable

        Public Sub CacheControlImages(control As Object)
            For Each ctl As Control In DirectCast(control, Control).Controls
                If ctl.GetType = GetType(OneClickToolStrip) Or ctl.GetType = GetType(ToolStrip) Then
                    SetToolStripImages(DirectCast(ctl, ToolStrip).Items)
                Else
                    SetControlImage(ctl)
                End If
                If ctl.HasChildren Then CacheControlImages(ctl)
            Next
        End Sub
        Public Function ImageCache(key As Object, ByRef image As Image) As Image
            Dim img As Image = DirectCast(ImageCacheHashTable(key), Image)
            If img Is Nothing Then
                ImageCacheHashTable(key) = image.Clone
                image.Dispose()
                img = DirectCast(ImageCacheHashTable(key), Image)
            End If
            Return img
        End Function

        Private Sub SetToolStripImages(tools As ToolStripItemCollection)
            For Each tool As ToolStripItem In tools
                If tool.Image IsNot Nothing Then
                    Dim img = ImageCache(tool.Name, tool.Image)
                    tool.Image = img
                End If
                If TryCast(tool, ToolStripDropDownButton) IsNot Nothing Then SetToolStripImages(DirectCast(tool, ToolStripDropDownButton).DropDownItems)
            Next
        End Sub

        Private Sub SetControlImage(ctl As Object)
            Select Case True
                Case ctl.GetType Is GetType(Button)
                    Dim but = DirectCast(ctl, Button)
                    If but.BackgroundImage IsNot Nothing Then
                        Dim img = ImageCache(but.Name, but.BackgroundImage)
                        but.BackgroundImage = img
                    End If
                Case ctl.GetType Is GetType(PictureBox)
                    Dim pbox = DirectCast(ctl, PictureBox)
                    If pbox.Image IsNot Nothing Then
                        Dim img = ImageCache(pbox.Name, pbox.Image)
                        pbox.Image = img
                    End If
            End Select
        End Sub

    End Module
End Namespace

