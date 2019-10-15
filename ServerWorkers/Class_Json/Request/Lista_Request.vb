<Serializable>
Public Class Lista_Request
   Public Property tema As tema
   Public Property cmip As New List(Of cmip) 'parametros dos paths de Imagens
   Public Property cmdi As New List(Of cmdi) 'parametros de tbls 
   Public Property cmpi As cmpi 'parametros de paginas
   Public Property cmti As New List(Of cmti) 'parametros de templates
   Public Property cmbi As New List(Of cmbi) 'parametros de Btns
   Public Property cmri As New List(Of cmri) 'parametros de campos relacionados
   Public Property exec_first() As New List(Of String)
   Public Property remove() As New List(Of String) 'id/remove (true/false)
   Public Property remove_dd As New List(Of String) 'remove as linhas relacionadas ao drilldown menu - parent1; parent2
   Public Property remove_linha As New List(Of String) 'tbl;linha
   Public Property hide() As New List(Of String)
   Public Property cmd As New List(Of cmd)
   Public Property trOptions As New List(Of String)
   Public Property cmd_btn() As New List(Of cmd_btn)
   Public Property cmd_span() As New List(Of cmd_span)
   Public Property cmd_img() As New List(Of cmd_img)
   Public Property autocompl() As New List(Of String)
   Public Property autocompl_enable As New List(Of String) 'id;enable (true/false)
   Public Property combo() As New List(Of String) 'id;value;text
   Public Property value() As New List(Of String) 'id;value
   Public Property text() As New List(Of String) 'id;text
   Public Property prop As New List(Of String) 'id;property;value ou css selector;property;value (css selector inicia com .)
   Public Property css() As New List(Of String) 'selector;property;value (css)
   Public Property tabindex() As New List(Of Lista_Hide)
   Public Property next_focus() As String
   Public Property newtags() As New List(Of Lista_NewTags)
   Public Property box_effects As String
   Public Property token() As String
   Public Property exec() As New List(Of String)
   Public Property yesno() As New List(Of Lista_YesNo)
   Public Property msg() As New List(Of Lista_Msg)
   Public Property extra() As New List(Of Lista_Extra)
   Public Property jfunctions As New List(Of Integer)
End Class
