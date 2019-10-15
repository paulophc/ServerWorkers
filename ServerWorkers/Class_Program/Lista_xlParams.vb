Public Class Lista_xlParams
	Public Property wb_name As String
   Public Property ws_name As String
   Public Property tbl_name As String
   Public Property freeze_row As Integer? = Nothing
	Public Property freeze_column As Integer? = Nothing
   Public Property lPropNames As New List(Of String)
   Public Property dFormats As New Dictionary(Of String, String)
   Public Property dFormulas As New Dictionary(Of String, String)
   Public Property dAligns As New Dictionary(Of String, xlAlign)
   Public Property xlFails As New List(Of Lista_xlFails)
   Public Property xlCellRed As New List(Of Lista_xlCellRed)
   Public Property row As Object
   Public Property lRows As IEnumerable(Of Object)
   Public Property lRows_Fails As IEnumerable(Of Object)
   Public Property dLinesUpdateds As New Dictionary(Of String, Integer)
   Public Property dLinesAddeds As New Dictionary(Of String, Integer)
End Class
