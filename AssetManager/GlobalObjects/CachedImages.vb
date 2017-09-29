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

        ''' <summary>
        ''' Controls passed to this method will have all their child Control images replaced with a single centrally cached instance of that image.
        ''' </summary>
        ''' <param name="control">Parent container object containing the controls you wish to have cached images applied to. Typically a Form.</param>
        Public Sub CacheControlImages(control As Object)
            'Loop through child controls.
            For Each ctl As Control In DirectCast(control, Control).Controls
                If ctl.GetType = GetType(OneClickToolStrip) Or ctl.GetType = GetType(ToolStrip) Then
                    'If control is a toolstrip, pass it to another recursive method to loop through all toolstrip items.
                    SetToolStripImages(DirectCast(ctl, ToolStrip).Items)
                Else
                    'If control is a control, replace images with cached versions.
                    SetControlImage(ctl)
                End If
                'If the control has children, recurse.
                If ctl.HasChildren Then CacheControlImages(ctl)
            Next
        End Sub

        ''' <summary>
        ''' This method manages the image cache. Images not found with the corresponding key are added to the cache. Returns a cached image.
        ''' </summary>
        ''' <param name="key">The identifying key for cached image. Can be anything, but a control.name of a frequently duplicated control is a good value.</param>
        ''' <param name="image">The image to be cached.</param>
        ''' <returns></returns>
        Public Function ImageCache(key As Object, ByRef image As Image) As Image
            'Try to pull an image from the cache with a matching key.
            Dim img As Image = DirectCast(ImageCacheHashTable(key), Image)
            'If no matching image was found, add it to the cache.
            If img Is Nothing Then
                'Add a clone of the original image so the original can be safely disposed.
                ImageCacheHashTable(key) = image.Clone
                'Toss that memory hogging lump in the garbage.
                image.Dispose()
                'Grab the new image from the cache.
                img = DirectCast(ImageCacheHashTable(key), Image)
            End If
            'Will always return a cached image.
            Return img
        End Function

        ''' <summary>
        ''' Recursively loops through a toolstrip and replaces any images found with a cached instance.
        ''' </summary>
        ''' <param name="tools"></param>
        Private Sub SetToolStripImages(tools As ToolStripItemCollection)
            For Each tool As ToolStripItem In tools
                If tool.Image IsNot Nothing Then
                    Dim img = ImageCache(tool.Name, tool.Image)
                    tool.Image = img
                End If
                If TryCast(tool, ToolStripDropDownButton) IsNot Nothing Then SetToolStripImages(DirectCast(tool, ToolStripDropDownButton).DropDownItems)
            Next
        End Sub

        ''' <summary>
        ''' Sets a controls image to a cached instance. Checks the control type and sets the correct image property accordingly.
        ''' </summary>
        ''' <param name="ctl"></param>
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

