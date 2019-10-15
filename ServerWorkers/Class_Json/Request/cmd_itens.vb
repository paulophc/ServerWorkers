<Serializable>
Public Class cmd_itens
   Public Property template() As String
   Public Property pauta_index As String
   Public Property image As Boolean
   Public Property preview As Boolean
   Public Property red_button As Boolean
   Public Property click_event As String
   Public Property MinLengths As New List(Of String) 'id/value - usado em Resize_Columns
   Public Property MaxLengths As New List(Of String) 'id/value - usado em Resize_Columns
End Class
