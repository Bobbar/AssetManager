Namespace Colors
    Module Colors
        Public ReadOnly Property MissingField As Color = ColorTranslator.FromHtml("#ffcccc") '"#82C1FF") '"#FF9827") '"#75BAFF")
        Public ReadOnly Property CheckIn As Color = ColorTranslator.FromHtml("#B6FCC0")
        Public ReadOnly Property CheckOut As Color = ColorTranslator.FromHtml("#FCB6B6")
        Public ReadOnly Property HighlightBlue As Color = Color.FromArgb(46, 112, 255) 'ColorTranslator.FromHtml("#8BCEE8")
        Public ReadOnly Property SibiSelectColor As Color = Color.FromArgb(185, 205, 255) '(146, 148, 255) '(31, 47, 155)
        Public ReadOnly Property OrangeHighlightColor As Color = ColorTranslator.FromHtml("#FF6600")
        Public ReadOnly Property OrangeSelectColor As Color = ColorTranslator.FromHtml("#FFB917")
        Public ReadOnly Property EditColor As Color = ColorTranslator.FromHtml("#81EAAA")
        Public ReadOnly Property DefaultFormBackColor As Color = Color.FromArgb(232, 232, 232)
        Public ReadOnly Property StatusBarProblem As Color = ColorTranslator.FromHtml("#FF9696")
        Public ReadOnly Property AssetToolBarColor As Color = Color.FromArgb(249, 226, 166)
        Public ReadOnly Property SibiToolBarColor As Color = Color.FromArgb(185, 205, 255) '(148, 213, 255)
        Public Property DefaultGridBackColor As Color
        Public Property DefaultGridSelectColor As Color
    End Module
End Namespace
