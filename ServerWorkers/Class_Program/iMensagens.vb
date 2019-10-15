Public Class iMensagens
   ''' <summary>
   ''' não pode conter apostrofe.
   ''' Grupo: Apostrofe | Usa Campo | Comum
   ''' </summary>
   Public Property Apostrofe() As String =  "Apostrofe"
   ''' <summary>
   ''' Nenhum CEP encontrado
   ''' Grupo: BD
   ''' </summary>
   Public Property CEPNotFoundBD() As String =  "CEPNotFoundBD"
   ''' <summary>
   ''' Deseja realmente efetivar a exclusão ?
   ''' Grupo: BD | Comum
   ''' </summary>
   Public Property Excluir() As String =  "Excluir"
   ''' <summary>
   ''' 0() já está cadastrada.
   ''' Grupo: BD | Comum
   ''' </summary>
   Public Property JaCadastrada_fn() As String =  "JaCadastrada"
   ''' <summary>
   ''' já está cadastrada na tabela 0()
   ''' Grupo: BD | Usa Campo | Comum
   ''' </summary>
   Public Property JaCadastradaTabela_fn() As String =  "JaCadastradaTabela"
   ''' <summary>
   ''' 0() já está cadastrado.
   ''' Grupo: BD | Comum
   ''' </summary>
   Public Property JaCadastrado_fn() As String =  "JaCadastrado"
   ''' <summary>
   ''' já está cadastrado na tabela 0()
   ''' Grupo: BD | Usa Campo | Comum
   ''' </summary>
   Public Property JaCadastradoTabela_fn() As String =  "JaCadastradoTabela"
   ''' <summary>
   ''' não encontrado no banco de dados.
   ''' Grupo: BD | Usa Campo | Comum
   ''' </summary>
   Public Property NotFoundBD() As String =  "NotFoundBD"
   ''' <summary>
   ''' não encontrada no banco de dados.
   ''' Grupo: BD | Usa Campo | Comum
   ''' </summary>
   Public Property NotFoundBDa() As String =  "NotFoundBDa"
   ''' <summary>
   ''' não encontrado na tabela 0()
   ''' Grupo: BD | Usa Campo | Comum
   ''' </summary>
   Public Property NotFoundTable_fn() As String =  "NotFoundTable"
   ''' <summary>
   ''' não encontrada na tabela 0()
   ''' Grupo: BD | Usa Campo | Comum
   ''' </summary>
   Public Property NotFoundTablea_fn() As String =  "NotFoundTablea"
   ''' <summary>
   ''' Não há informações para serem atualizadas.
   ''' Grupo: BD | Comum
   ''' </summary>
   Public Property NoUpdate() As String =  "NoUpdate"
   ''' <summary>
   ''' Cadastro excluido. Deseja reativar o cadastro ?
   ''' Grupo: BD | Comum
   ''' </summary>
   Public Property Reativar() As String =  "Reativar"
   ''' <summary>
   ''' 15 dias
   ''' Grupo: Button
   ''' </summary>
   Public Property FifteenDays() As String =  "FifteenDays"
   ''' <summary>
   ''' 5 anos
   ''' Grupo: Button
   ''' </summary>
   Public Property FiveYear() As String =  "FiveYear"
   ''' <summary>
   ''' linha
   ''' Grupo: Button
   ''' </summary>
   Public Property linha() As String =  "linha"
   ''' <summary>
   ''' linhas
   ''' Grupo: Button
   ''' </summary>
   Public Property linhas() As String =  "linhas"
   ''' <summary>
   ''' 1 mes
   ''' Grupo: Button
   ''' </summary>
   Public Property OneMonth() As String =  "OneMonth"
   ''' <summary>
   ''' 1 ano
   ''' Grupo: Button
   ''' </summary>
   Public Property OneYear() As String =  "OneYear"
   ''' <summary>
   ''' Pagina
   ''' Grupo: Button
   ''' </summary>
   Public Property Pagina() As String =  "Pagina"
   ''' <summary>
   ''' 7 dias
   ''' Grupo: Button
   ''' </summary>
   Public Property SevenDays() As String =  "SevenDays"
   ''' <summary>
   ''' 6 meses
   ''' Grupo: Button
   ''' </summary>
   Public Property SixMonth() As String =  "SixMonth"
   ''' <summary>
   ''' 3 dias
   ''' Grupo: Button
   ''' </summary>
   Public Property ThreeDays() As String =  "ThreeDays"
   ''' <summary>
   ''' 3 meses
   ''' Grupo: Button
   ''' </summary>
   Public Property ThreeMonth() As String =  "ThreeMonth"
   ''' <summary>
   ''' 2 anos
   ''' Grupo: Button
   ''' </summary>
   Public Property TwoYear() As String =  "TwoYear"
   ''' <summary>
   ''' CEP será mantido inalterado, devido ao país informado.
   ''' Grupo: CEP
   ''' </summary>
   Public Property CEPInalter() As String =  "CEPInalter"
   ''' <summary>
   ''' UF deve ser igual a EX.
   ''' Grupo: CEP
   ''' </summary>
   Public Property UF_EX() As String =  "UF_EX"
   ''' <summary>
   ''' O codigo de barras ja está associado ao produto: 0() - 1();
   ''' No fornecedor: 2().
   ''' Grupo: Codigo
   ''' </summary>
   Public Property CBarrasFound_fn() As String =  "CBarrasFound"
   ''' <summary>
   ''' deve ser diferente do Codigo do Produto em uso.
   ''' Grupo: Codigo | Usa Campo
   ''' </summary>
   Public Property CodigoIgualCodigoAtual() As String =  "CodigoIgualCodigoAtual"
   ''' <summary>
   ''' Para consultar utilizando o código é necessário selecionar também o tipo de cadastro.
   ''' Grupo: Codigo
   ''' </summary>
   Public Property ConsultaInfo1() As String =  "ConsultaInfo1"
   ''' <summary>
   ''' já está cadastrado.
   ''' Grupo: Codigo | Usa Campo | Comum
   ''' </summary>
   Public Property JaCadastrado() As String =  "JaCadastrado"
   ''' <summary>
   ''' Data final não pode ser menor que a data inicial.
   ''' Grupo: Data
   ''' </summary>
   Public Property DataDiverIniFim() As String =  "DataDiverIniFim"
   ''' <summary>
   ''' deve ser maior que 0().
   ''' Grupo: Datas | Usa Campo
   ''' </summary>
   Public Property DataMaiorNow_fn() As String =  "DataMaiorNow"
   ''' <summary>
   ''' não está em ordem sequencial
   ''' Grupo: Datas | Usa Campo
   ''' </summary>
   Public Property DataOutOrder() As String =  "DataOutOrder"
   ''' <summary>
   ''' Valor do Desconto não pode ser maior do que o preço de venda.
   ''' Grupo: Desconto
   ''' </summary>
   Public Property DescontoMaiorPrVenda() As String =  "DescontoMaiorPrVenda"
   ''' <summary>
   ''' Não há empresas cadastras no sistema.
   ''' Grupo: Empresa
   ''' </summary>
   Public Property EmpresasNaoCadastradas() As String =  "EmpresasNaoCadastradas"
   ''' <summary>
   ''' Não há transações habilitadas para esta empresa.
   ''' Grupo: Empresas
   ''' </summary>
   Public Property Emp_Trs() As String =  "Emp_Trs"
   ''' <summary>
   ''' Falha detectada ao ler informações do banco de dados.
   ''' Grupo: Erro BD | Comum
   ''' </summary>
   Public Property ReadDBError() As String =  "ReadDBError"
   ''' <summary>
   ''' Falha detectada ao verificar o código da empresa.
   ''' Codigo relacionado à descrição da Empresa: 0().
   ''' Grupo: Erro Empresa
   ''' </summary>
   Public Property EmpresaReadError() As String =  "EmpresaReadError"
   ''' <summary>
   ''' Falha detectada ao remover o arquivo da pasta.
   ''' 0()
   ''' Grupo: Erro File
   ''' </summary>
   Public Property ImgDelError_fn() As String =  "ImgDelError"
   ''' <summary>
   ''' Falha detectada ao criar a pasta.
   ''' 0()
   ''' Grupo: Erro Folder
   ''' </summary>
   Public Property FolderError_fn() As String =  "FolderError"
   ''' <summary>
   ''' Falha detectada ao verificar o codigo interno associado ao 0()
   ''' Grupo: Erro Hidden Element | Comum
   ''' </summary>
   Public Property HiddenElementError_fn() As String =  "HiddenElementError"
   ''' <summary>
   ''' Falha detectada ao ler arquivo.
   ''' Base64String esta vazio.
   ''' 0()
   ''' Grupo: Erro Imagem
   ''' </summary>
   Public Property Base64StrError1_fn() As String =  "Base64StrError1"
   ''' <summary>
   ''' Falha detectada ao ler arquivo.
   ''' Erro ao verificar a largura (não é 4 ou multiplo de 4).
   ''' 0()
   ''' Grupo: Erro Imagem
   ''' </summary>
   Public Property Base64StrError2_fn() As String =  "Base64StrError2"
   ''' <summary>
   ''' Não foi possivel determinar a identificacao da imagem.
   ''' Certifique-se de que as informações estejam salvas no banco de dados.
   ''' 0()
   ''' Grupo: Erro Imagem Id
   ''' </summary>
   Public Property ImgIdError_fn() As String =  "ImgIdError"
   ''' <summary>
   ''' Falha detectada ao gravar informação da foto no cadastro.
   ''' 0()
   ''' Grupo: Erro Imagem Salvar
   ''' </summary>
   Public Property ImgSaveError_fn() As String =  "ImgSaveError"
   ''' <summary>
   ''' Problema detectado ao ler as informações dos parametros do sistema.
   ''' Grupo: Erro Parametros
   ''' </summary>
   Public Property ParamReadError() As String =  "ParamReadError"
   ''' <summary>
   ''' No momento não é possivel obter informações da url informada.
   ''' Para certificar-se de que é uma url válida, clique no link abaixo:
   ''' <a href="0()" target="_blank">0()</a>.
   ''' Grupo: Erro Url
   ''' </summary>
   Public Property UrlGetInfoError_fn() As String =  "UrlGetInfoError"
   ''' <summary>
   ''' url diferente do padrão original.
   ''' http://www.youtube.com/watch?v=video
   ''' Grupo: Erro Video
   ''' </summary>
   Public Property VideoYouTubeError() As String =  "VideoYouTubeError"
   ''' <summary>
   ''' Falha detectada ao determinar o horário.
   ''' 0()
   ''' Grupo: Erros
   ''' </summary>
   Public Property TimeError_fn() As String =  "TimeError"
   ''' <summary>
   ''' Atualizar informações no banco de dados?
   ''' Grupo: Excel
   ''' </summary>
   Public Property xlUpdateDB() As String =  "xlUpdateDB"
   ''' <summary>
   ''' O arquivo 0() não está no formato correto.
   ''' Grupo: File Alert
   ''' </summary>
   Public Property FileFormatAlert_fn() As String =  "FileFormatAlert"
   ''' <summary>
   ''' O tamanho do arquivo 0() é maior que 20Mb.
   ''' Grupo: File Alert
   ''' </summary>
   Public Property FileSizeAlert_fn() As String =  "FileSizeAlert"
   ''' <summary>
   ''' Falha detectada ao gravar arquivo.
   ''' 0()
   ''' Grupo: File Error Imagem
   ''' </summary>
   Public Property FileSaveError_fn() As String =  "FileSaveError"
   ''' <summary>
   ''' Falha detectada ao mover o arquivo 0() p/ 1().
   ''' 2().
   ''' Grupo: File MoveTo
   ''' </summary>
   Public Property FileMoveTo_fn() As String =  "FileMoveTo"
   ''' <summary>
   ''' 0() está com informação inconsistente ou não foi encontrado na tabela 1().
   ''' Grupo: Inconsistente
   ''' </summary>
   Public Property Inconsist_NotFound_fn() As String =  "Inconsist_NotFound"
   ''' <summary>
   ''' está com informação inconsistente.
   ''' Grupo: Inconsistente | Usa Campo | Comum
   ''' </summary>
   Public Property Inconsistente() As String =  "Inconsistente"
   ''' <summary>
   ''' 0() está com informação inconsistente.
   ''' Grupo: Inconsistente | Comum
   ''' </summary>
   Public Property Inconsistente_fn() As String =  "Inconsistente"
   ''' <summary>
   ''' 0()
   ''' estão com informações inconsistentes ou não foram encontrados na tabela 1().
   ''' Grupo: Inconsistente
   ''' </summary>
   Public Property Inconsists_NotFounds_fn() As String =  "Inconsists_NotFounds"
   ''' <summary>
   ''' está incorreta.
   ''' Grupo: Inconsistente | Usa Campo | Comum
   ''' </summary>
   Public Property Incorreta() As String =  "Incorreta"
   ''' <summary>
   ''' está incorreto.
   ''' Grupo: Inconsistente | Usa Campo | Comum
   ''' </summary>
   Public Property Incorreto() As String =  "Incorreto"
   ''' <summary>
   ''' Foram encontradas algumas linhas c/ inconsistências e/ou excluidas.
   ''' Para maiores detalhes, verifique a planilha 0().
   ''' Grupo: Inconsistente
   ''' </summary>
   Public Property xlInconsistente_fn() As String =  "xlInconsistente"
   ''' <summary>
   ''' está com quantidade de digitos diferentes do layout.
   ''' Grupo: Layout | Usa Campo
   ''' </summary>
   Public Property DigitsDifLayout() As String =  "DigitsDifLayout"
   ''' <summary>
   ''' está com informação inconsistente.
   ''' A sequencia de preenchimento deve ficar uniforme, conforme os exemplos:
   ''' NNNNNNNNNNNN, AAAAAAAA, NNNNNNN_TT_CCC, NNNNN_TTT_CC, AAAAAA_TT_CC, NNNNNNNLLLLL e etc.
   ''' N - Produto (digito numerico) ou A - Produto (digito alfanumerico)
   ''' T - Tamanho (Codigo do Tamanho) ou t - Tamanho (Descricao do tamanho)
   ''' C - Cor (Codigo da Cor) ou c - Cor (Descricao Abreviada - migracao)
   ''' L - Lote.
   ''' Grupo: Layout | Usa Campo
   ''' </summary>
   Public Property LayoutError1() As String =  "LayoutError1"
   ''' <summary>
   ''' está com informação inconsistente.
   ''' A sequencia de caracteres nao pode estar interpolada com caracteres diferentes.
   ''' Grupo: Layout | Usa Campo
   ''' </summary>
   Public Property LayoutError2() As String =  "LayoutError2"
   ''' <summary>
   ''' está com informação inconsistente.
   ''' Ex: NNNNNNNNNNNN, AAAAAAAA, NNNNNNAAAA, AAANNNNNN.
   ''' N - Produto (digito numerico) ou A - Produto (digito alfanumerico).
   ''' Grupo: Layout | Usa Campo
   ''' </summary>
   Public Property LayoutError3() As String =  "LayoutError3"
   ''' <summary>
   ''' deve ter informação maior ou igual a 5 caracteres.
   ''' Grupo: Layout | Usa Campo
   ''' </summary>
   Public Property LayoutError4() As String =  "LayoutError4"
   ''' <summary>
   ''' Layout do Produto informado nos parametros gerais está inconsistente.
   ''' Grupo: Layout
   ''' </summary>
   Public Property LayoutProdErr1() As String =  "LayoutProdErr1"
   ''' <summary>
   ''' Há layouts divergentes no grupo de empresas.
   ''' É necessário que o layout de produtos de todas as empresas do grupo estejam com a mesma informação.
   ''' Grupo: Layout
   ''' </summary>
   Public Property LayoutProdLines() As String =  "LayoutProdLines"
   ''' <summary>
   ''' O número de caracteres deve ser menor que 0() digitos e compativel com o layout informado nos parametros.
   ''' Grupo: Layout
   ''' </summary>
   Public Property TextDiverLenght_fn() As String =  "TextDiverLenght"
   ''' <summary>
   ''' Linha removida.
   ''' Grupo: LinhaRemovida
   ''' </summary>
   Public Property LinhaRemovida() As String =  "LinhaRemovida"
   ''' <summary>
   ''' está fora do intervalo permitido.
   ''' Grupo: MinMax | Usa Campo | Comum
   ''' </summary>
   Public Property DataMinMax() As String =  "DataMinMax"
   ''' <summary>
   ''' Sequência de entrada não está em um formato correto.
   ''' Segue abaixo alguns exemplos:
   ''' D, DD, DD-Mes (Nome), DD-MM, DD/MM e etc.
   ''' DDMMAA, DDMM, DDMMAAAA, DD-Mes (Nome)-AA
   ''' DD/MM/AA, DD/MM/AAAA, DD-MM-AA, DD-MM-AAAA
   ''' Grupo: MinMax | Comum
   ''' </summary>
   Public Property DataMinMaxError() As String =  "DataMinMaxError"
   ''' <summary>
   ''' está fora do intervalo permitido.
   ''' Grupo: MinMax | Usa Campo | Comum
   ''' </summary>
   Public Property MinMax() As String =  "MinMax"
   ''' <summary>
   ''' Sequência de entrada não está em um formato correto.
   ''' Grupo: MinMax | Comum
   ''' </summary>
   Public Property MinMaxError() As String =  "MinMaxError"
   ''' <summary>
   ''' Falha detectada.
   ''' Não foi possível obter informações da tabela 0(), relacionado ao campo: 1().
   ''' Grupo: MinMax
   ''' </summary>
   Public Property MinMaxNotFound_fn() As String =  "MinMaxNotFound"
   ''' <summary>
   ''' Exibição de itens com quantidade iguais ou superiores a zeros, 0().
   ''' Grupo: Movimentos
   ''' </summary>
   Public Property Display_Qtd_0s_fn() As String =  "Display_Qtd_0s"
   ''' <summary>
   ''' Selecione a Transação desejada, para adicionar itens de Produtos, Serviços ou Movimentos de Débitos/Créditos.
   ''' Grupo: Movimentos
   ''' </summary>
   Public Property Info_Trs() As String =  "Info_Trs"
   ''' <summary>
   ''' Não há itens de movimentos. 
   ''' Grupo: Movimentos
   ''' </summary>
   Public Property MovSemItens() As String =  "MovSemItens"
   ''' <summary>
   ''' Movimentação de 0() ativada.
   ''' Grupo: Movimentos
   ''' </summary>
   Public Property Troca_fn() As String =  "Troca"
   ''' <summary>
   ''' Movimentos de Debitos/Creditos só podem ser adicionados em Transações relacionadas a Movimentos de Debitos/Creditos.
   ''' Grupo: Movimentos
   ''' </summary>
   Public Property TrsDiverServ() As String =  "TrsDiverServ"
   ''' <summary>
   ''' Produtos e/ou Serviços não podem ser adicionados em Transações relacionadas a Movimentos de Débitos/Créditos.
   ''' Grupo: Movimentos
   ''' </summary>
   Public Property TrsDiverServ2() As String =  "TrsDiverServ2"
   ''' <summary>
   ''' Serviços só podem ser adicionadas em transações de Vendas.
   ''' Grupo: Movimentos
   ''' </summary>
   Public Property TrsDiverServ3() As String =  "TrsDiverServ3"
   ''' <summary>
   ''' 0() não cadastrado na tabela de Tributos Aproximados fornecida pelo IBPT
   '''  e também não cadastrado na tabela de Tributos Aproximados fornecidos pelo Usuário.
   ''' Grupo: NCM_NBS
   ''' </summary>
   Public Property NCM_NBS_NotFound_IBPT_fn() As String =  "NCM_NBS_NotFound_IBPT"
   ''' <summary>
   ''' 0() não cadastrado na tabela NCMs_NBS
   ''' Grupo: NCM_NBS
   ''' </summary>
   Public Property NCM_NBS_NotFound_NCMs_NBS_fn() As String =  "NCM_NBS_NotFound_NCMs_NBS"
   ''' <summary>
   ''' 0() não cadastrado na tabela NCMs_Perfis
   ''' Grupo: NCM_NBS
   ''' </summary>
   Public Property NCM_NBS_NotFound_NCMs_Perfis_fn() As String =  "NCM_NBS_NotFound_NCMs_Perfis"
   ''' <summary>
   ''' 0() informado não é válido para 1()
   ''' Grupo: NCM_NBS
   ''' </summary>
   Public Property NCM_NBSDiverServico_fn() As String =  "NCM_NBSDiverServico"
   ''' <summary>
   ''' deve ser preenchimento somente com valores numericos
   ''' Grupo: Numeros | Usa Campo | Comum
   ''' </summary>
   Public Property Numeric() As String =  "Numeric"
   ''' <summary>
   ''' Para 0() são permitidos somente os sinais ""."" e ""-"".
   ''' Grupo: Numeros | Comum
   ''' </summary>
   Public Property Sinais_fn() As String =  "Sinais"
   ''' <summary>
   ''' Para 0() não são permitidos sinais de pontuações.
   ''' Grupo: Numeros
   ''' </summary>
   Public Property Sinais2_fn() As String =  "Sinais2"
   ''' <summary>
   ''' Valor deve ser diferente de zeros.
   ''' Grupo: Numeros
   ''' </summary>
   Public Property ValorDiffZeros() As String =  "ValorDiffZeros"
   ''' <summary>
   ''' deve ser maior que zero.
   ''' Grupo: Numeros | Usa Campo
   ''' </summary>
   Public Property ValorMaiorZeros() As String =  "ValorMaiorZeros"
   ''' <summary>
   ''' Valor deve ser menor que zeros.
   ''' Grupo: Numeros
   ''' </summary>
   Public Property ValorMenorZeros() As String =  "ValorMenorZeros"
   ''' <summary>
   ''' Para corrigir a informação, será necessario habilitar em Parametros, o uso dos campos que apresentam inconsistencias.
   ''' Grupo: Parametros
   ''' </summary>
   Public Property EnableParam() As String =  "EnableParam"
   ''' <summary>
   ''' Confirmação não confere com senha informada.
   ''' Grupo: Password
   ''' </summary>
   Public Property PasswordDiffPasswordConfirm() As String =  "PasswordDiffPasswordConfirm"
   ''' <summary>
   ''' Nova senha deve ser diferente da senha atual.
   ''' Grupo: Password
   ''' </summary>
   Public Property PasswordDiffPasswordNew() As String =  "PasswordDiffPasswordNew"
   ''' <summary>
   ''' Preenchimento da Senha requer pelo menos 6 digitos.
   ''' Grupo: Password
   ''' </summary>
   Public Property PasswordMin6Digits() As String =  "PasswordMin6Digits"
   ''' <summary>
   ''' É necessária a informação da Password.
   ''' Grupo: Password
   ''' </summary>
   Public Property PasswordMissing() As String =  "PasswordMissing"
   ''' <summary>
   ''' 0() não permitido.
   ''' Grupo: Permitido
   ''' </summary>
   Public Property Permitido_fn() As String =  "Permitido"
   ''' <summary>
   ''' Alteração de Produto para Serviço ou Movimento de Débito/Crédito não permitida.
   ''' Grupo: Produtos
   ''' </summary>
   Public Property Prod_Serv_Diver() As String =  "Prod_Serv_Diver"
   ''' <summary>
   ''' Alteração de Serviço para Movimento de Crédito/Débito não permitida.
   ''' Grupo: Produtos
   ''' </summary>
   Public Property Prod_Serv_Diver2() As String =  "Prod_Serv_Diver2"
   ''' <summary>
   ''' não está relacionado a 0()
   ''' Grupo: Relation | Usa Campo
   ''' </summary>
   Public Property NotRelation_fn() As String =  "NotRelation"
   ''' <summary>
   ''' 0() não está relacionado a 1()
   ''' Grupo: Relation
   ''' </summary>
   Public Property NotRelationItem_fn() As String =  "NotRelationItem"
   ''' <summary>
   ''' Após a exclusão da Session, não será possível restaurar o histórico de dados desta página, salvos no WebServer. Confirma a exclusão da Session?
   ''' Grupo: Session
   ''' </summary>
   Public Property SessionConfirm() As String =  "SessionConfirm"
   ''' <summary>
   ''' Falha detectada ao serializar Session.
   ''' 0()
   ''' Grupo: Session
   ''' </summary>
   Public Property SessionError1_fn() As String =  "SessionError1"
   ''' <summary>
   ''' Falha detectada ao deserializar Session.
   ''' 0()
   ''' Grupo: Session
   ''' </summary>
   Public Property SessionError2_fn() As String =  "SessionError2"
   ''' <summary>
   ''' Não há nenhuma session gravada relacionada a esta página.
   ''' Grupo: Session
   ''' </summary>
   Public Property SessionNotFound() As String =  "SessionNotFound"
   ''' <summary>
   ''' Session removida.
   ''' Grupo: Session
   ''' </summary>
   Public Property SessionRemoved() As String =  "SessionRemoved"
   ''' <summary>
   ''' Falha detectada ao determinar a página da lista de dados.
   ''' Grupo: Sort
   ''' </summary>
   Public Property Sort_PageError() As String =  "Sort_PageError"
   ''' <summary>
   ''' requer preenchimento obrigatório.
   ''' Grupo: String | Usa Campo | Comum
   ''' </summary>
   Public Property Empty() As String =  "Empty"
   ''' <summary>
   ''' 0() requer preenchimento obrigatório.
   ''' Grupo: String | Comum
   ''' </summary>
   Public Property Empty_fn() As String =  "Empty"
   ''' <summary>
   ''' requer preenchimento obrigatório e com 2 ou mais caracteres.
   ''' Grupo: String | Usa Campo | Comum
   ''' </summary>
   Public Property EmptyL2() As String =  "EmptyL2"
   ''' <summary>
   ''' deve ter 2 ou mais caracteres.
   ''' Grupo: String | Usa Campo | Comum
   ''' </summary>
   Public Property L2() As String =  "L2"
   ''' <summary>
   ''' Sugestão: Complemente a informação PDV (Ponto de Venda) com números ou outra descrição.
   ''' Exemplos: PDV 01, PDV 02, PDV 10, PDV - Nome do Colaborador, Balcão, Caixa e etc.
   ''' Grupo: Sugestao
   ''' </summary>
   Public Property LocalAcessoSugest1() As String =  "LocalAcessoSugest1"
   ''' <summary>
   ''' Sugestão: Informe um nome diferente ligado ao setor ou ao colaborador.
   ''' Exemplos: Contas a Receber, Compras, Diretoria, Computador - Nome do Colaborador e etc.
   ''' Grupo: Sugestao
   ''' </summary>
   Public Property LocalAcessoSugest2() As String =  "LocalAcessoSugest2"
   ''' <summary>
   ''' Falha detectada ao verificar cor: 0().
   ''' Grupo: Tbl Cores
   ''' </summary>
   Public Property CorError_fn() As String =  "CorError"
   ''' <summary>
   ''' não pode ficar duplicado.
   ''' Grupo: Tbl Duplicado | Usa Campo | Comum
   ''' </summary>
   Public Property Dup() As String =  "Dup"
   ''' <summary>
   ''' 0() não pode ficar duplicado.
   ''' Grupo: Tbl Duplicado | Comum
   ''' </summary>
   Public Property Dup_fn() As String =  "Dup"
   ''' <summary>
   ''' Contato deve estar associado ao e-mail
   ''' Grupo: Tbl Emails
   ''' </summary>
   Public Property EmailAssoc() As String =  "EmailAssoc"
   ''' <summary>
   ''' Dados do Endereço, Numero, Compl. e etc. não podem ficar duplicados.
   ''' Grupo: Tbl Enderecos
   ''' </summary>
   Public Property DupEnderEtc() As String =  "DupEnderEtc"
   ''' <summary>
   ''' Dados do Fone não podem ficar duplicados.
   ''' Grupo: Tbl Fones
   ''' </summary>
   Public Property DupFoneEtc() As String =  "DupFoneEtc"
   ''' <summary>
   ''' Contato deve estar associado ao fone.
   ''' Grupo: Tbl Fones
   ''' </summary>
   Public Property FoneAssoc() As String =  "FoneAssoc"
   ''' <summary>
   ''' Não é possivel adicionar novo fornecedor enquanto houver fornecedor sem informações.
   ''' Grupo: Tbl Fornecs
   ''' </summary>
   Public Property AdicionarFornec() As String =  "AdicionarFornec"
   ''' <summary>
   ''' não deve ter informações para esta situação tributária. Deixar a opção em branco.
   ''' Grupo: Tbl ICMSPerfil | Usa Campo
   ''' </summary>
   Public Property Modalidade_Empty() As String =  "Modalidade_Empty"
   ''' <summary>
   ''' requer preenchimento obrigatório para esta situação tributária.
   ''' Grupo: Tbl ICMSPerfil | Usa Campo
   ''' </summary>
   Public Property Modalidade_NotEmpty() As String =  "Modalidade_NotEmpty"
   ''' <summary>
   ''' 0() não pode ser desativado.
   ''' Foram detectadas 1() linhas na tabela NCMs_Perfis.
   ''' Foram detectadas 2() linhas na tabela Natureza_Operações.
   ''' Foram detectadas 3() linhas na tabela Natureza_Operações_Perfis.
   ''' Grupo: Tbl ICMSPerfis
   ''' </summary>
   Public Property DesativarPerfil_ICMS_fn() As String =  "DesativarPerfil_ICMS"
   ''' <summary>
   ''' Exclusão do Perfil de ICMS não permitido.
   ''' Foram detectadas 0() linhas na tabela NCMs_Perfis.
   ''' Foram detectadas 1() linhas na tabela Natureza_Operações.
   ''' Foram detectadas 2() linhas na tabela Natureza_Operações_Perfis.
   ''' Grupo: Tbl ICMSPerfis
   ''' </summary>
   Public Property RemoverTICMSPerfil_fn() As String =  "RemoverTICMSPerfil"
   ''' <summary>
   ''' Não é permitido adicionar Itens de Produtos ou Serviços com Movimentos de Débitos/Créditos.
   ''' Grupo: Tbl Kits
   ''' </summary>
   Public Property KitDiverServico() As String =  "KitDiverServico"
   ''' <summary>
   ''' Produto com itens de kits não podem ter tamanhos ou cores.
   ''' Grupo: Tbl Kits
   ''' </summary>
   Public Property KitDiverTam_Cor() As String =  "KitDiverTam_Cor"
   ''' <summary>
   ''' Não é permitido associar outros produtos que já tenham itens de kits relacionados.
   ''' Grupo: Tbl Kits
   ''' </summary>
   Public Property KitWithOtherKit() As String =  "KitWithOtherKit"
   ''' <summary>
   ''' Valor Parcial não pode ser maior que o Total de Itens.
   ''' Grupo: Tbl MFPs
   ''' </summary>
   Public Property Parcial_Maior_TotalItens() As String =  "Parcial_Maior_TotalItens"
   ''' <summary>
   ''' Exclusão da Forma de Pagamento não permitida.
   ''' Foram detectadas 0() linha(s) associadas a esta forma de pagamento na tabela de Movimentações/Formas de Pagamentos.
   ''' Grupo: Tbl MFPs
   ''' </summary>
   Public Property RemoverMFP_fn() As String =  "RemoverMFP"
   ''' <summary>
   ''' É necessário selecionar a Forma de Pagamento.
   ''' Grupo: Tbl MFPs
   ''' </summary>
   Public Property SelecionarFP() As String =  "SelecionarFP"
   ''' <summary>
   ''' Exclusão do Produto não permitido.
   ''' É necessário remover primeiro as linhas de Cores e/ou Tamanhos relacionadas ao Produto.
   ''' Grupo: Tbl MPDIs
   ''' </summary>
   Public Property RemoverMPDI() As String =  "RemoverMPDI"
   ''' <summary>
   ''' Confirma a atualização para o Estoque de Destino informado?
   ''' Grupo: Tbl MSRs
   ''' </summary>
   Public Property EtqConfirm() As String =  "EtqConfirm"
   ''' <summary>
   ''' O Estoque 0(), não está habilitada para 1().
   ''' Grupo: Tbl MSTs
   ''' </summary>
   Public Property Estoque_Disable_fn() As String =  "Estoque_Disable"
   ''' <summary>
   ''' A transação 0(), não está habilitada para 1().
   ''' Grupo: Tbl MSTs
   ''' </summary>
   Public Property Transacao_Disable_fn() As String =  "Transacao_Disable"
   ''' <summary>
   ''' O Usuário 0(), não está habilitado para 1().
   ''' Para habilitar, selecione a exibição de imagens, clique no usuário,
   ''' e em seguida, adicione usuários ao grupo de usuários da empresa.
   ''' Grupo: Tbl MSTs
   ''' </summary>
   Public Property Usuario_Disable_fn() As String =  "Usuario_Disable"
   ''' <summary>
   ''' Exclusão da Cor não permitida.
   ''' Foram detectadas 0() linha(s) associadas a esta cor na tabela de Cores.
   ''' Grupo: Tbl PCores
   ''' </summary>
   Public Property RemoverPCor_fn() As String =  "RemoverPCor"
   ''' <summary>
   ''' Exclusão do Estoque não permitido.
   ''' Foram detectadas 0() linha(s) associadas a este estoque na tabela de Estoques de Produtos.
   ''' Grupo: Tbl PEQIs
   ''' </summary>
   Public Property RemoverPEQI_fn() As String =  "RemoverPEQI"
   ''' <summary>
   ''' Exclusão da Transação não permitida.
   ''' Foram detectadas 0() linha(s) associadas a esta transação na tabela de Movimentações.
   ''' Grupo: Tbl PETIs
   ''' </summary>
   Public Property RemoverPETI_fn() As String =  "RemoverPETI"
   ''' <summary>
   ''' Exclusão do Fornecedor não permitido.
   ''' É necessário remover primeiro as linhas dos códigos relacionados ao Fornecedor.
   ''' Grupo: Tbl PFCs
   ''' </summary>
   Public Property RemoverPFC() As String =  "RemoverPFC"
   ''' <summary>
   ''' Exclusão do Codigo do Fornecedor não permitido.
   ''' Foram detectadas 0() linha(s) associadas a este codigo do fornecedor na tabela de Movimentações_Itens_Codigos_Fornecedores.
   ''' Grupo: Tbl PFCs
   ''' </summary>
   Public Property RemoverPFC_fn() As String =  "RemoverPFC"
   ''' <summary>
   ''' Exclusão do Item de Filtro não permitido.
   ''' Foram detectadas 0() linha(s) associadas a este item de filtro no Cadastro de 1().
   ''' Grupo: Tbl PFI
   ''' </summary>
   Public Property RemoverPFICad_fn() As String =  "RemoverPFICad"
   ''' <summary>
   ''' Não é possivel adicionar novo filtro enquanto houver filtro sem informações.
   ''' Grupo: Tbl PFiltro
   ''' </summary>
   Public Property AdicionarPFiltro() As String =  "AdicionarPFiltro"
   ''' <summary>
   ''' Exclusão do Filtro não permitido.
   ''' Foram detectadas 0() linha(s) associadas a este filtro no Cadastro de 1().
   ''' Grupo: Tbl PFiltro
   ''' </summary>
   Public Property RemoverPFiltro_fn() As String =  "RemoverPFiltro"
   ''' <summary>
   ''' Exclusão do Filtro não permitido.
   ''' É necessário remover primeiro as linhas relacionadas ao Filtro.
   ''' Grupo: Tbl PFIs
   ''' </summary>
   Public Property RemoverPFI() As String =  "RemoverPFI"
   ''' <summary>
   ''' Exclusão da Forma de Pagamento não permitida.
   ''' É necessário remover primeiro as linhas relacionadas às Formas de Pagamentos.
   ''' Grupo: Tbl PFPIs
   ''' </summary>
   Public Property RemoverPFPI() As String =  "RemoverPFPI"
   ''' <summary>
   ''' A diferença entre o valor e valor anterior não pode ser maior que 0.01.
   ''' Grupo: Tbl PFPs
   ''' </summary>
   Public Property Diff_Maior_001() As String =  "Diff_Maior_001"
   ''' <summary>
   ''' Valor deve ser maior que o valor anterior.
   ''' Grupo: Tbl PFPs
   ''' </summary>
   Public Property Diff_Maior_Previous() As String =  "Diff_Maior_Previous"
   ''' <summary>
   ''' Parcelas sem Juros deve ser menor ou igual ao Total de Parcelas.
   ''' Grupo: Tbl PFPs
   ''' </summary>
   Public Property ParcelaSemJurosErr() As String =  "ParcelaSemJurosErr"
   ''' <summary>
   ''' Exclusão da Forma de Pagamento não permitida.
   ''' Foram detectadas 0() linha(s) associadas a esta forma de pagamento na tabela de Movimentações/Formas de Pagamentos.
   ''' Grupo: Tbl PFPs
   ''' </summary>
   Public Property RemoverPFP_fn() As String =  "RemoverPFP"
   ''' <summary>
   ''' Exclusão da Forma de Pagamento não permitida.
   ''' Foram detectadas 0() linha(s) associadas a esta forma de pagamento na tabela de Produtos/Formas de Pagamentos.
   ''' Grupo: Tbl PFPs
   ''' </summary>
   Public Property RemoverPFP2_fn() As String =  "RemoverPFP2"
   ''' <summary>
   ''' Taxa de Juros deve ser maior que 0,00%, quando a quantidade de parcelas sem juros for menor que a quantidade total de parcelas.
   ''' Grupo: Tbl PFPs
   ''' </summary>
   Public Property TaxaDeJurosErr() As String =  "TaxaDeJurosErr"
   ''' <summary>
   ''' Exclusão do Grupo de Cores não permitida.
   ''' É necessário remover primeiro as linhas relacionadas ao Grupo.
   ''' Grupo: Tbl PGCIs
   ''' </summary>
   Public Property RemoverPGCI() As String =  "RemoverPGCI"
   ''' <summary>
   ''' Exclusão do Grupo de Empresas não permitida.
   ''' É necessário remover primeiro as linhas relacionadas ao Grupo.
   ''' Grupo: Tbl PGEI
   ''' </summary>
   Public Property RemoverPGEI() As String =  "RemoverPGEI"
   ''' <summary>
   ''' Exclusão do Grupo de Empresa não permitida.
   ''' Foram detectadas 0() linha(s) associadas a este grupo de empresa na tabela de Produtos.
   ''' Grupo: Tbl PGEmpresas
   ''' </summary>
   Public Property RemoverPGEmpresa_fn() As String =  "RemoverPGEmpresa"
   ''' <summary>
   ''' Exclusão do Grupo de Tamanhos não permitido.
   ''' É necessário remover primeiro as linhas relacionadas ao Grupo.
   ''' Grupo: Tbl PGTIs
   ''' </summary>
   Public Property RemoverPGTI() As String =  "RemoverPGTI"
   ''' <summary>
   ''' Digitação de informações relacionadas ao preço de custo não está habilitado para esta empresa.
   ''' Para habilitar, altere a opção Digita Preço de Custo no Cadastro da Empresa.
   ''' Grupo: Tbl PPCs
   ''' </summary>
   Public Property PPC_NotDigitaCusto() As String =  "PPC_NotDigitaCusto"
   ''' <summary>
   ''' Exclusão do Preço não permitida.
   ''' Foram detectadas 0() linha(s) associadas a este preço na tabela de Produtos.
   ''' Grupo: Tbl PPreco
   ''' </summary>
   Public Property RemoverPPreco_fn() As String =  "RemoverPPreco"
   ''' <summary>
   ''' Não é permitido remover os preços de categoria: Custo Médio e Compras.
   ''' Grupo: Tbl PPreco
   ''' </summary>
   Public Property RemoverPPreco_Custo() As String =  "RemoverPPreco_Custo"
   ''' <summary>
   ''' Valor do desconto não pode ter valor superior ao preço.
   ''' Grupo: Tbl PPVIs
   ''' </summary>
   Public Property Desc_ValorDiverPreco() As String =  "Desc_ValorDiverPreco"
   ''' <summary>
   ''' Para a opção Leve e Pague são permitidas somente as quantidades (1, 2 ou 3).
   ''' Grupo: Tbl PPVIs
   ''' </summary>
   Public Property PPVI_123Diver() As String =  "PPVI_123Diver"
   ''' <summary>
   ''' O preço promocional informado não pode ser superior ou igual ao preço promocional anterior.
   ''' Grupo: Tbl PPVIs
   ''' </summary>
   Public Property PPVI_123DiverValor() As String =  "PPVI_123DiverValor"
   ''' <summary>
   ''' Preço promocional Leve e Pague, não permite quantidade superior a 3.
   ''' Grupo: Tbl PPVIs
   ''' </summary>
   Public Property PPVI_LevePagDiver() As String =  "PPVI_LevePagDiver"
   ''' <summary>
   ''' Além do Tipo = 'Desconto', somente é permitida a informação em um dos três tipos de preços promocionais
   ''' (A partir de, A cada, Leve e Pague).
   ''' Grupo: Tbl PPVIs
   ''' </summary>
   Public Property PPVI_TipoErr() As String =  "PPVI_TipoErr"
   ''' <summary>
   ''' Preço promocional não pode ter valor igual ou superior ao preço multiplicado pela quantidade.
   ''' Grupo: Tbl PPVIs
   ''' </summary>
   Public Property PPVI_ValorDiverPreco() As String =  "PPVI_ValorDiverPreco"
   ''' <summary>
   ''' Exclusão do Preço não permitido.
   ''' É necessário remover primeiro as linhas relacionadas ao Preço.
   ''' Grupo: Tbl PPVs
   ''' </summary>
   Public Property RemoverPPV() As String =  "RemoverPPV"
   ''' <summary>
   ''' Exclusão da Forma de Pagamento não permitida.
   ''' Foram detectadas 0() linha(s) associadas a esta forma de pagamento na tabela de Movimentações/Formas de Pagamentos.
   ''' Grupo: Tbl PTFPs
   ''' </summary>
   Public Property RemoverPTFPI_fn() As String =  "RemoverPTFPI"
   ''' <summary>
   ''' deve ser diferente do Preço.
   ''' Grupo: Tbl PTransacoes | Usa Campo
   ''' </summary>
   Public Property PrecoEqualPrecoAltern() As String =  "PrecoEqualPrecoAltern"
   ''' <summary>
   ''' Entradas/Saídas por Transferências e Inventários, não podem utilizar o preço de Venda.
   ''' Grupo: Tbl PTransacoes
   ''' </summary>
   Public Property PrecoET_ST_IV_Err() As String =  "PrecoET_ST_IV_Err"
   ''' <summary>
   ''' Exclusão da Transação não permitida.
   ''' Foram detectadas 0() linha(s) associadas a esta transação na tabela de Movimentações.
   ''' Grupo: Tbl PTransacoes
   ''' </summary>
   Public Property RemoverPTransacao_fn() As String =  "RemoverPTransacao"
   ''' <summary>
   ''' Exclusão do Movimento Vinculado não permitido.
   ''' Foram detectadas 0() linha(s) associadas a esta transação na tabela de Movimentos Vinculados.
   ''' Grupo: Tbl PTVs
   ''' </summary>
   Public Property RemoverPTV_fn() As String =  "RemoverPTV"
   ''' <summary>
   ''' Transação de Devoluções de Vendas não está cadastrada na tabela de Transações Vinculadas.
   ''' Grupo: Tbl PTVs
   ''' </summary>
   Public Property TVDVNotFound() As String =  "TVDVNotFound"
   ''' <summary>
   ''' Não é permitido remover a última linha.
   ''' Para remover a última linha com informações,
   '''  acrescente 1 linha em branco,
   '''  e em seguida remova a linha c/ informações.
   ''' Grupo: Tbl Remover
   ''' </summary>
   Public Property RemoverLastLine() As String =  "RemoverLastLine"
   ''' <summary>
   ''' Exclusão do Tamanho não permitido.
   ''' Foram detectadas 0() linha(s) associadas a este tamanho na tabela de Tamanhos.
   ''' Grupo: Tbl Tamanho
   ''' </summary>
   Public Property RemoverPTamanho_fn() As String =  "RemoverPTamanho"
   ''' <summary>
   ''' Falha detectada ao verificar tamanho: 0().
   ''' Grupo: Tbl Tamanhos
   ''' </summary>
   Public Property TamError_fn() As String =  "TamError"
   ''' <summary>
   ''' Exclusão do ICMS não permitido.
   ''' Foram detectadas 0() linha(s) associadas a esta linha de ICMS na tabela de NCMs_Aliquotas.
   ''' Grupo: Tbl TICMs
   ''' </summary>
   Public Property RemoverTICM_fn() As String =  "RemoverTICM"
   ''' <summary>
   ''' Exclusão do Perfil de IPI não permitido.
   ''' Foram detectadas 0() linhas na tabela NCMs_Perfis.
   ''' Foram detectadas 1() linhas na tabela Natureza_Operações_Perfis.
   ''' Grupo: Tbl TIPIPerfis
   ''' </summary>
   Public Property RemoverTIPIPerfil_fn() As String =  "RemoverTIPIPerfil"
   ''' <summary>
   ''' Tipo de Rede Social/Site não podem ficar duplicados.
   ''' Grupo: Tbl TiposWeb
   ''' </summary>
   Public Property DupTWebEtc() As String =  "DupTWebEtc"
   ''' <summary>
   ''' 0() não encontrada na tabela de Aliquotas de ICMS
   ''' Grupo: Tbl TNCMAIs
   ''' </summary>
   Public Property AlqNotFound_fn() As String =  "AlqNotFound"
   ''' <summary>
   ''' 0() inconsistente. Verificar aliquota de ICMS = 0,00% ou alterar aliquota de 0() para 0,00%.
   ''' Grupo: Tbl TNCMAIs
   ''' </summary>
   Public Property AlqSemICMSInfo_fn() As String =  "AlqSemICMSInfo"
   ''' <summary>
   ''' Falha detectada, não há IVAs relacionadas à Origem.
   ''' Selecione uma origem diferente e em seguida selecione a origem desejada.
   ''' Grupo: Tbl TNCMPIs
   ''' </summary>
   Public Property IVAsWithOrigem() As String =  "IVAsWithOrigem"
   ''' <summary>
   ''' Exclusão do Perfil de NCM não permitido.
   ''' Foram detectadas 0() linha(s) associadas a este perfil na tabela de Produtos.
   ''' Grupo: Tbl TNCMPIs
   ''' </summary>
   Public Property RemoverTNCMPI_fn() As String =  "RemoverTNCMPI"
   ''' <summary>
   ''' Exclusão do NCM não permitido.
   ''' Foram detectadas 0() linha(s) associadas a este NCM na tabela de Produtos.
   ''' Grupo: Tbl TNCMs
   ''' </summary>
   Public Property RemoverTNCM_fn() As String =  "RemoverTNCM"
   ''' <summary>
   ''' Exclusão da Natureza da Operação não permitida.
   ''' Foram detectadas 0() linha(s) associadas a esta Natureza na tabela de Observacoes_Itens.
   ''' Grupo: Tbl TNOP
   ''' </summary>
   Public Property RemoverTNOP_fn() As String =  "RemoverTNOP"
   ''' <summary>
   ''' Situação Tributária não permitida para o regime tributário informado.
   ''' Grupo: Tbl TNOP
   ''' </summary>
   Public Property SitTrib() As String =  "SitTrib"
   ''' <summary>
   ''' deve ser compatível com as classificações utilizadas na tabela de CFOPs.
   ''' Grupo: Tbl TNOPPIs | Usa Campo
   ''' </summary>
   Public Property ClassCFOPError() As String =  "ClassCFOPError"
   ''' <summary>
   ''' Associação de Perfis de ICMS não permitida (CST x CSOSN)
   ''' Grupo: Tbl TNOPPIs
   ''' </summary>
   Public Property Perfis_CSTxCSOSN() As String =  "Perfis_CSTxCSOSN"
   ''' <summary>
   ''' Perfis Tributários devem ser diferentes.
   ''' Grupo: Tbl TNOPPIs
   ''' </summary>
   Public Property Perfis_Iguais() As String =  "Perfis_Iguais"
   ''' <summary>
   ''' CFOP não é compatível com a classificação do CFOP.
   ''' Grupo: Tbl TNOPs
   ''' </summary>
   Public Property CFOP_Inconsistente() As String =  "CFOP_Inconsistente"
   ''' <summary>
   ''' Classificação do CFOP está inconsistente com a UF de Destino/Remetente.
   ''' Grupo: Tbl TNOPs
   ''' </summary>
   Public Property ClassCFOP_Inconsistente() As String =  "ClassCFOP_Inconsistente"
   ''' <summary>
   ''' O Tipo de Transação não é compatível com a Classificação do CFOP.
   ''' Grupo: Tbl TNOPs
   ''' </summary>
   Public Property Tipo_Transacao_Inconsistente() As String =  "Tipo_Transacao_Inconsistente"
   ''' <summary>
   ''' Exclusão da Observação não permitida.
   ''' É necessário remover primeiro as linhas relacionadas às Observações.
   ''' Grupo: Tbl TOBIs
   ''' </summary>
   Public Property RemoverTOBI() As String =  "RemoverTOBI"
   ''' <summary>
   ''' Exclusão do Perfil de PIS/COFINS não permitido.
   ''' Foram detectadas 0() linhas na tabela NCMs_Perfis.
   ''' Foram detectadas 1() linhas na tabela Natureza_Operações_Perfis.
   ''' Grupo: Tbl TPCPerfis
   ''' </summary>
   Public Property RemoverTPCPerfil_fn() As String =  "RemoverTPCPerfil"
   ''' <summary>
   ''' Exclusão do Tributo Aproximado não permitido.
   ''' Foram detectadas 0() linha(s) associadas a esta linha de tributos aproximados na tabela NCM_NBS.
   ''' Grupo: Tbl TTAU
   ''' </summary>
   Public Property RemoverTTAU2_fn() As String =  "RemoverTTAU2"
   ''' <summary>
   ''' Exclusão das aliquotas de Tributos Aproximados não permitido.
   ''' Foram detectadas 0() linhas na tabela NCMs_Aliquotas.
   ''' Foram detectadas 1() linhas na tabela NCMs_IVAs.
   ''' Grupo: Tbl TTAUI
   ''' </summary>
   Public Property RemoverTTAUI_fn() As String =  "RemoverTTAUI"
   ''' <summary>
   ''' Exclusão dos Tributos Aproximados não permitido.
   ''' É necessário remover primeiro as linhas relacionadas aos Tributos Aproximados.
   ''' Grupo: Tbl TTAUIs
   ''' </summary>
   Public Property RemoverTTAUI() As String =  "RemoverTTAUI"
   ''' <summary>
   ''' Exclusão da Unidade não permitida.
   ''' Foram detectadas 0() linha(s) associadas a esta unidade na tabela de Produtos.
   ''' Grupo: Tbl TUnidades
   ''' </summary>
   Public Property RemoverTUnid_fn() As String =  "RemoverTUnid"
   ''' <summary>
   ''' Valor da parcela não pode ser maior do que o Valor da Forma de Pagamento.
   ''' Grupo: Tbls MFPs
   ''' </summary>
   Public Property ParcelaDiverValor() As String =  "ParcelaDiverValor"
   ''' <summary>
   ''' Não é possível finalizar o movimento, pois valor do saldo é diferente de zeros.
   ''' Grupo: Tbls MFPs
   ''' </summary>
   Public Property SaldoDiffZeros() As String =  "SaldoDiffZeros"
   ''' <summary>
   ''' Não há Formas de Pagamentos disponiveis para fechar o saldo.
   ''' Verifique as Formas de Pagamentos habilitadas em Parametros/Transações/Formas de Pagamentos.
   ''' Grupo: Tbls MOCs
   ''' </summary>
   Public Property NoFPs() As String =  "NoFPs"
   ''' <summary>
   ''' Foram detectadas alterações nas cores dos produtos, listadas abaixo.
   ''' Esta diferença ocorre porque houve alterações cadastrais relacionadas às Cores,
   ''' enquanto, o movimento ou a planilha do excel estão com informações para serem concluidas.
   ''' Para corrigir, tecle F5 (Refresh) ou modifique as cores informadas na planilha.
   ''' 
   ''' Grupo: Tbls MPDIs
   ''' </summary>
   Public Property CoresDiff() As String =  "CoresDiff"
   ''' <summary>
   ''' Foram detectadas alterações nos tamanhos dos produtos.
   ''' Esta diferença ocorre porque houve alterações cadastrais relacionadas aos Tamanhos,
   ''' enquanto, o movimento ou a planilha do excel estão com informações para serem concluidas.
   ''' Para corrigir, tecle F5 (Refresh) ou modifique os tamanhos informados na planilha.
   ''' 
   ''' Grupo: Tbls MPDIs
   ''' </summary>
   Public Property TamDiff() As String =  "TamDiff"
   ''' <summary>
   ''' Foram detectadas alterações nos tamanhos e/ou cores dos produtos.
   ''' Esta diferença ocorre porque houve alterações cadastrais relacionadas aos Tamanhos e/ou Cores,
   ''' enquanto, o movimento ou a planilha do excel estão com informações para serem concluidas.
   ''' Para corrigir, tecle F5 (Refresh) ou modifique os tamanhos e/ou cores informados na planilha.
   ''' 
   ''' Grupo: Tbls MPDs
   ''' </summary>
   Public Property TamCoresDiff() As String =  "TamCoresDiff"
   ''' <summary>
   ''' não esta relacionado na Unidade/Quantidade Comercial do Produto e nem dos Fornecedores.
   ''' Esta diferença ocorre porque ocorreram alterações cadastrais relacionadas às Unidades/Quantidades Comerciais,
   ''' enquanto o movimento ou a planilha do excel estavam com informações para serem concluidas.
   ''' Para corrigir, remova o produto e informe novamente o mesmo c/ as mesmas quantidades,
   ''' ou para o caso de planilhas do Excel, altere as informações da planilha relacionadas às Unidades/Quantidades Comerciais.
   ''' Grupo: Tbls MPDs | Usa Campo
   ''' </summary>
   Public Property UnqCom_NotFound() As String =  "UnqCom_NotFound"
   ''' <summary>
   ''' Não é permitido serviços de débitos c/ serviços de créditos ou vice-versa.
   ''' Grupo: Tbls MPSVs
   ''' </summary>
   Public Property ServDC() As String =  "ServDC"
   ''' <summary>
   ''' Não é permitido remover o usuário atualmente selecionado no movimento.
   ''' Para remover, selecione outro usuário e em seguida retorne a esta tela.
   ''' Grupo: Tbls MSTs
   ''' </summary>
   Public Property RemoverCurrentUser() As String =  "RemoverCurrentUser"
   ''' <summary>
   ''' Percentual de Desconto ou Valor do Desconto inconsistente.
   ''' Se houver Percentual de Desconto, Valor do Desconto deve ficar c/ zeros ou vice-versa.
   ''' Grupo: Tbls PFPs
   ''' </summary>
   Public Property DescDiverValorDesc() As String =  "DescDiverValorDesc"
   ''' <summary>
   ''' Transação Vinculada não corresponde à Transação. Exemplos:
   ''' Saída por Transferência de Empresas vincula-se a Entrada por Transferência de Empresas e vice-versa.
   ''' Devoluções de Vendas de Clientes vincula-se a Venda p/ Clientes e vice-versa.
   ''' Outras Entradas de Fornecedores vincula-se a Outras Saídas de Fornecedores e vice-versa.
   ''' 
   ''' Grupo: Tbls PTVs
   ''' </summary>
   Public Property TVNotCorresp() As String =  "TVNotCorresp"
   ''' <summary>
   ''' Não é permitido associar Transações de Saídas c/ Transações Vinculadas de Saída,
   ''' e nem Transações de Entradas com Transações Vinculadas de Entradas.
   ''' Grupo: Tbls TVs
   ''' </summary>
   Public Property SaidasEntradasDiver() As String =  "SaidasEntradasDiver"
   ''' <summary>
   ''' Falha detectada ao gerar o thumbnail.
   ''' O arquivo nao tem um formato de imagem válido ou GDI+ não suporta o pixel format do arquivo.
   ''' Grupo: Thumbnail Error
   ''' </summary>
   Public Property ThumbnailError1() As String =  "ThumbnailError1"
   ''' <summary>
   ''' Falha detectada ao gerar o thumbnail.
   ''' 0()
   ''' Grupo: Thumbnail Error
   ''' </summary>
   Public Property ThumbnailError2_fn() As String =  "ThumbnailError2"
   ''' <summary>
   ''' Falha detectada ao obter thumbnail da url:
   ''' 0()
   ''' Grupo: Thumbnail Error
   ''' </summary>
   Public Property ThumbUrlError_fn() As String =  "ThumbUrlError"
   ''' <summary>
   ''' Conteudo do token está vazio.
   ''' Grupo: Token
   ''' </summary>
   Public Property Token1() As String =  "Token1"
   ''' <summary>
   ''' Erro ao descriptografar token.
   ''' Informação do token foi alterada.
   ''' Grupo: Token
   ''' </summary>
   Public Property Token2() As String =  "Token2"
   ''' <summary>
   ''' É necessário efetuar novamente o login.
   ''' Grupo: Token
   ''' </summary>
   Public Property Token3() As String =  "Token3"
   ''' <summary>
   ''' Data de validade do token, foi expirada.
   ''' É necessário efetuar novamente o login.
   ''' Grupo: Token
   ''' </summary>
   Public Property Token4() As String =  "Token4"
   ''' <summary>
   ''' Codigo do usuario informado no token, não foi encontrado no banco de dados.
   ''' Grupo: Token
   ''' </summary>
   Public Property Token5() As String =  "Token5"
   ''' <summary>
   ''' Identificação ou Senha está incorreta.
   ''' Grupo: Token
   ''' </summary>
   Public Property Token6() As String =  "Token6"
   ''' <summary>
   ''' Data de validade da senha foi expirada.
   ''' É necessário efetuar o login novamente.
   ''' Grupo: Token
   ''' </summary>
   Public Property Token7() As String =  "Token7"
   ''' <summary>
   ''' Cancelar
   ''' Grupo: Traducao
   ''' </summary>
   Public Property Cancel() As String =  "Cancel"
   ''' <summary>
   ''' Para exibir informações sobre os campos, selecione um determinado campo e em seguida clique neste botão.
   ''' Grupo: Traducao
   ''' </summary>
   Public Property GetFieldInfoEmpty() As String =  "GetFieldInfoEmpty"
   ''' <summary>
   ''' Não
   ''' Grupo: Traducao
   ''' </summary>
   Public Property No() As String =  "No"
   ''' <summary>
   ''' Sim
   ''' Grupo: Traducao
   ''' </summary>
   Public Property Yes() As String =  "Yes"
   ''' <summary>
   ''' É necessário indicar também a url da imagem do video (thumbnail).
   ''' Grupo: Video
   ''' </summary>
   Public Property VideoMissThumb() As String =  "VideoMissThumb"
   ''' <summary>
   ''' Informação do video não encontrado na url:
   ''' 0()
   ''' Grupo: Video
   ''' </summary>
   Public Property VideoNotFound_fn() As String =  "VideoNotFound"
   ''' <summary>
   ''' Este procedimento pode levar vários minutos, aguarde o termino do processamento...
   ''' Grupo: WaitProcess | Comum
   ''' </summary>
   Public Property WaitProcess() As String =  "WaitProcess"
End Class
