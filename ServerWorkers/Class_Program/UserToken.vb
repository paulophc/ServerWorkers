<Serializable>
Public Class UserToken
   Public Property empresa() As String
   ''' <summary>
   ''' Codigo do usuario
   ''' </summary>
   Public Property user_id() As String
   Public Property login_name() As String
   Public Property password() As String
   Public Property dispositivo() As String
   ''' <summary>
   ''' Status de Logon
   ''' True - Login, False Logout
   ''' </summary>
   Public Property logon() As Boolean
   ''' <summary>
   ''' Data em que foi efetuado o Login ou Logout
   ''' </summary>
   Public Property logon_date() As Date
   ''' <summary>
   ''' 0 - Ok
   ''' 1 - conteudo do token esta vazio
   ''' 2 - Erro ao descriptografar o token (informacao do token, alterada).
   ''' 3 - Logout
   ''' 4 - Data de validade do token, expirada
   ''' 5 - Usuario nao encontrado no banco de dados
   ''' 6 - Nome de usuario ou senha diferente do cadastro no BD.
   ''' 7 - Data de validade da password, expirada
   ''' </summary>
   Public Property status() As Integer?
   ''' <summary>
   ''' Mensagem relacionada ao intervalo do status de 1 a 6.
   ''' </summary>
   Public Property msg() As String
End Class

