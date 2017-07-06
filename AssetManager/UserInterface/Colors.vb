Module Colors
    Public colCurrentEntry As Color = ColorTranslator.FromHtml("#7AD1FF") '"#7AD1FF"
    Public colMissingField As Color = ColorTranslator.FromHtml("#ffcccc") '"#82C1FF") '"#FF9827") '"#75BAFF")
    Public colCheckIn As Color = ColorTranslator.FromHtml("#B6FCC0")
    Public colCheckOut As Color = ColorTranslator.FromHtml("#FCB6B6")

    '  Public colHighlightOrange As Color = ColorTranslator.FromHtml("#FF9A26") '"#FF9827")
    Public colHighlightBlue As Color = Color.FromArgb(46, 112, 255) 'ColorTranslator.FromHtml("#8BCEE8")

    Public colSibiSelectColor As Color = Color.FromArgb(185, 205, 255) '(146, 148, 255) '(31, 47, 155)
    Public colHighlightColor As Color = ColorTranslator.FromHtml("#FF6600")
    Public colSelectColor As Color = ColorTranslator.FromHtml("#FFB917")

    Public colEditColor As Color = ColorTranslator.FromHtml("#81EAAA")
    Public colFormBackColor As Color = Color.FromArgb(232, 232, 232)
    Public colStatusBarProblem As Color = ColorTranslator.FromHtml("#FF9696")
    Public colAssetToolBarColor As Color = Color.FromArgb(249, 226, 166)
    Public colSibiToolBarColor As Color = Color.FromArgb(185, 205, 255) '(148, 213, 255)
    Public DefGridBC As Color, DefGridSelCol As Color
    Public DefaultSibiTheme As New Grid_Theme(colHighlightBlue, colSibiSelectColor, Color.FromArgb(64, 64, 64))

End Module