<Serializable>
Public Class cmd_img
   Public Property tbl As String 'Embora Produtos e Cadastros nao sejam tbl, esta condicao esta prevista na function Layout_Imagens()
   Public Property appendTo As String
   Public Property update_img As Boolean
   Public Property size_desktop As String
   Public Property size_mobile As String
   Public Property draggable As Boolean
   Public Property margin As String
   Public Property lImgs As New List(Of Lista_Img)
End Class
