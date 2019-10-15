<Serializable>
Public Class cmd
   Public Property tbl() As String
   Public Property clear() As Boolean
   Public Property layout_check() As String 'para ajustar o layout de algumas tabelas Ex: MPD, MST, MFP e etc...
   Public Property layout_rows_check() As String 'para ajustar o layout de algumas tabelas apos inicializacao de todas as linhas Ex: MPD, MST, MFP e etc...
   Public Property insertAfter_page() As String 'para inserir itens apos determinada pagina.
   Public Property pages() As New List(Of String)
   Public Property htbl() As New List(Of String)
   Public Property hsel() As New List(Of String) 'Lista de Configuracoes das Selects p/ saber se mantem ou nao todos os options.
   Public Property insertAfter() As New List(Of String)
End Class
