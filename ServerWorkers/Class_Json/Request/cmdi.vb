<Serializable>
Public Class cmdi
   Public Property tbl() As String
   Public Property start_name() As String
   Public Property table_name() As String
   Public Property tbl_referencia() As String
   Public Property columns() As String 'colunas ordenadas, separadas por ";"
   Public Property fitcontent() As Boolean 'usar Resize_Columns p/ ajustar as colunas
   Public Property resize() As Boolean 'usar resize (jquery ui) 
   Public Property drilldown() As Boolean 'Identifica qual tbl usa drilldown menus
   Public Property table_width As String 'Identifica o tamanho do table quando e' usado o resize.
   Public Property bpath As String 'Identifica o path dos buttons que sera usado junto c/ cmip
   Public Property header() As New cmd_header
   Public Property itens() As New cmd_itens
   Public Property trOptions() As New trOptions
   Public Property iph() As New List(Of Lista_IPH) 'Placeholders p/ inputs, textarea
   Public Property sels As New List(Of Lista_Selects)
End Class
