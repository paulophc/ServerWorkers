<Serializable>
Public Class ProdSearch
   Public Property ok As Boolean
   Public Property produto_ok As Boolean
   Public Property tamanho_ok As Boolean
   Public Property cor_ok As Boolean
   Public Property lote_ok As Boolean
   Public Property usa_tamanho As Boolean
   Public Property usa_cor As Boolean
   Public Property usa_lote As Boolean
   Public Property produto As String
   Public Property tamanho As String
   Public Property cor As String
   Public Property lote As String
   Public Property produto_nome As String
   Public Property produto_nome_reduzido As String
   Public Property produto_descricao As String
   Public Property tamanho_descricao As String
   Public Property cor_descricao As String
   Public Property servico As Byte?
   Public Property usa_decimal As Boolean
   Public Property un_com As String
   Public Property un_trib As String
   Public Property qtd_com As Integer?
   Public Property qtd_trib As Integer?
   Public Property credito As Boolean 'True - Credito, False - Debito (usado somente em Servicos de Debitos/Creditos)
   Public Property relacionado_ao_caixa As Boolean 'P/ saber se o servico de Debito/Credito esta relacionado ao caixa.
   Public Property fp_padrao As Byte? 'Forma de Pagamento Padrao, p/ agilizar a finalizacao do Movimento (Servico de Debitos/Creditos).
   Public Property isKit As Boolean 'P/ saber se e' Kit de Produtos
   Public Property notEmpty As Boolean
   Public Property msg As String
   Public Property rastreio As String
   Public Property dTams As New Dictionary(Of Short, String)
   Public Property dCores As New Dictionary(Of Short, String)
End Class
