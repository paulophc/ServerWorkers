Imports System.Web.Services
Imports System.Net.Http
Imports Newtonsoft.Json

Public Class WorkersWebForm
   Inherits Page

   Public Shared iprm As New iParametros
   Public Shared imsg As New iMensagens
   Public Shared img_path As New imgpath
   Public Shared htbl_buttons_default = "images/Perspective-Button-Go-icon.png"
   Public Shared htbl_buttons_selected = "images/Perspective-Button-Reboot-icon.png"
   Dim uri As String = "http://localhost:7607/api/Workers"

#Region " Page_Load - Apaga informacoes da Session e usa RegisterAsyncTask "

   Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
      HttpContext.Current.Session.Abandon()
      'RegisterAsyncTask(New PageAsyncTask(Function() Get_Remote_Workers(Nothing)))
   End Sub

#End Region

#Region " WebMethod() - Get_Elementos - Função de intercambio do Callback para obter e fornecer informacoes ao client "

   <WebMethod()>
   Public Shared Function Get_Elementos(Ids As String, Vls As String) As String
      Dim result As String = Nothing

      If Not Session_InUse(result) Then
         Dim modo As mode
         Dim Lista_Return As New Lista_Request
         Dim Lista_Cad As New Lista_Parametros
         Dim dElements As New Dictionary(Of String, String)
         Dim user_token As New UserToken
         Dim token As String = Nothing
         Dim lFocus As New List(Of Lista_Hide)
         Dim focus_element, caller_event As String
         Dim sIds = Split(Ids, ";")
         Dim sValues = split_csv(Vls.Replace("||", "'"))
         Dim modulo As String = Get_Modulo()
         Dim current_session = HttpContext.Current.Session

         current_session.Item("process_last_check") = True

         For x = 2 To sIds.Count - 1
            dElements.Add(sIds(x), sValues(x))
         Next

         focus_element = sValues(0)
         caller_event = sValues(1)

         dElements.TryGetValue("token", token)

         Token_Check(token, user_token)

         If caller_event.StartsWith("Start") Then
            Session_Start_Check(modo, caller_event, user_token, Lista_Return, lFocus, modulo)
         Else
            With current_session
               If .Item("lFocus") IsNot Nothing Then lFocus = .Item("lFocus")
            End With

            If caller_event <> "open" Then Set_Lista_Cad(Lista_Cad, dElements)
         End If

         Method_Check(focus_element, caller_event, Lista_Cad, Lista_Return, lFocus, dElements, user_token, Get_Modulo(True))

         'Somente para casos onde, Start c/ Lista_Return.extra.comeback = True e Start_GoTo_Login (Redirecionamento p/ Login.aspx).
         'Como ambos defininem comandos p/ reload da pagina, a variavel process_last_check e' igual False, para os demais casos e' igual a True.
         If current_session.Item("process_last_check") = True Then Last_Check(focus_element, caller_event, Lista_Cad, Lista_Return, lFocus, modulo)

         result = Serialize_JSON(Lista_Return)

         current_session.Item("SessionInUse") = Nothing
      End If

      Return result
   End Function

#End Region

   ' Subs para processamento das Listas

#Region " ...Tabelas - Workers... "

#Region " Workers_Open_Check - Efetua alguns procedimentos apos abertura do panel pelo accordion "

   Public Sub Workers_Open_Check(focus_element As String, caller_event As String, ByRef Lista_Cad As Lista_Parametros, ByRef Lista_Return As Lista_Request, user_token As UserToken, ByRef lFocus As List(Of Lista_Hide))
      Open_All_Check(Lista_Cad, Lista_Return, lFocus)

      Work_Filter_TagsFocus(Lista_Cad, Lista_Return, lFocus)

      Workers_Get_Lista(Lista_Cad, Lista_Return, lFocus)
   End Sub

#End Region

#Region " Workers_Sort_Check - Efetua o Sort "

   Shared Sub Workers_Sort_Check(focus_element As String, ByRef Lista_Cad As Lista_Parametros, ByRef Lista_Return As Lista_Request, ByRef lFocus As List(Of Lista_Hide))
      Dim qWorkers As New List(Of Lista_Workers)
      Dim campo As String = focus_element.Remove(focus_element.LastIndexOf("_header"))
      Dim value As Decimal
      Dim vdata As Date

      With Lista_Cad
         If campo = iprm.Work_Nome Then
            qWorkers = If(Get_SortOrder(iprm.Workers, iprm.Work_Nome), (From x In .Workers Order By x.Nome).ToList, (From x In .Workers Order By x.Nome Descending).ToList)
         ElseIf campo = iprm.Work_Email Then
            qWorkers = If(Get_SortOrder(iprm.Workers, iprm.Work_Email), (From x In .Workers Order By x.Email).ToList, (From x In .Workers Order By x.Email Descending).ToList)
         ElseIf campo = iprm.Work_Cargo Then
            qWorkers = If(Get_SortOrder(iprm.Workers, iprm.Work_Cargo), (From x In .Workers Order By x.Cargo).ToList, (From x In .Workers Order By x.Cargo Descending).ToList)
         ElseIf campo = iprm.Work_Salario Then
            qWorkers = If(Get_SortOrder(iprm.Workers, iprm.Work_Salario), (From x In .Workers Order By If(Decimal.TryParse(x.Salario, value), Decimal.Parse(x.Salario), 0D)).ToList, (From x In .Workers Order By If(Decimal.TryParse(x.Salario, value), Decimal.Parse(x.Salario), 0D) Descending).ToList)
         ElseIf campo = iprm.Work_Data Then
            qWorkers = If(Get_SortOrder(iprm.Workers, iprm.Work_Data), (From x In .Workers Order By If(Date.TryParse(x.Data, vdata), Date.Parse(x.Data), Date.MinValue)).ToList, (From x In .Workers Order By If(Date.TryParse(x.Data, vdata), Date.Parse(x.Data), Date.MinValue) Descending).ToList)
         End If
      End With

      Sort_List(iprm.Workers, qWorkers, Lista_Cad, Lista_Return, lFocus, Nothing)
   End Sub

#End Region

#Region " Workers_Get_Lista - Obtem a lista de Parametros de Workers do BD "

   Public Sub Workers_Get_Lista(ByRef Lista_Cad As Lista_Parametros, ByRef Lista_Return As Lista_Request, ByRef lFocus As List(Of Lista_Hide))
      Dim DB As New DBWorkersEntities
      Dim qWorkers As New List(Of Lista_Workers)
      Dim sequencia As Integer = 1

      qWorkers = (From x In DB.Workers Order By x.Nome
                  Select New Lista_Workers With {.Id = x.Id,
                                                 .Nome = x.Nome,
                                                 .Email = x.Email,
                                                 .Cargo = x.Cargo,
                                                 .Salario = x.Salario,
                                                 .Data = x.Data_Contratacao,
                                                 .fromDB = True
                                                }).ToList

      Set_enUS_Culture(True)

      If qWorkers.Count > 0 Then
         For Each work In qWorkers
            With work
               .linha = sequencia.ToString("0000")
               .Salario = Decimal.Parse(.Salario).ToString("N2", cc)
               .Data = Format(Date.Parse(.Data), "dd/MM/yy")
               sequencia += 1
            End With
         Next
      Else
         qWorkers.Add(New Lista_Workers With {.linha = "0001"})
      End If

      Set_enUS_Culture(False)

      Workers_New_TagsFocus(qWorkers, True, Lista_Cad, Lista_Return, lFocus, Nothing, True)
   End Sub

#End Region

#Region " xWorkers_Get_Lista - Obtem a lista de Parametros de Workers do BD "
   ''' <summary>
   ''' Teste, pois nao esta chamando a funcao assincrona.
   ''' </summary>
   Public Sub xWorkers_Get_Lista(ByRef Lista_Cad As Lista_Parametros, ByRef Lista_Return As Lista_Request, ByRef lFocus As List(Of Lista_Hide))
      Dim qWorkers As New List(Of Lista_Workers)
      Dim rWorkers As New List(Of Remote_Workers)
      Dim sequencia As Integer = 1
      Dim current_session = HttpContext.Current.Session

      With current_session
         Dim t = Get_Remote_Workers(Nothing)
         t.ConfigureAwait(True)
         Dim teste = t.Result

         If .Item("rWorkers") IsNot Nothing Then
            rWorkers = .Item("rWorkers")
         ElseIf .Item("msg_rworkers") IsNot Nothing Then
            Set_Msg(Lista_Return, .Item("msg_rworkers"))
         End If
      End With

      qWorkers = (From x In rWorkers Order By x.Nome
                  Select New Lista_Workers With {.Id = x.Id,
                                                 .Nome = x.Nome,
                                                 .Email = x.Email,
                                                 .Cargo = x.Cargo,
                                                 .Salario = x.Salario,
                                                 .Data = x.Data_Contratacao,
                                                 .fromDB = True
                                                }).ToList

      Set_enUS_Culture(True)

      If qWorkers.Count > 0 Then
         For Each work In qWorkers
            With work
               .linha = sequencia.ToString("0000")
               .Salario = Decimal.Parse(.Salario).ToString("N2", cc)
               .Data = Date.Parse(.Data).ToShortDateString
               sequencia += 1
            End With
         Next
      Else
         qWorkers.Add(New Lista_Workers With {.linha = "0001"})
      End If

      Set_enUS_Culture(False)

      Workers_New_TagsFocus(qWorkers, True, Lista_Cad, Lista_Return, lFocus, Nothing, True)
   End Sub

#End Region

#Region " Get_Workers - Obtem a lista de Funcionários do WebServer via Web API "

   Public Async Function Get_Remote_Workers(nome As String) As Threading.Tasks.Task(Of List(Of Remote_Workers))
      Dim client = New HttpClient
      Dim response As HttpResponseMessage
      Dim vuri = uri & If(nome IsNot Nothing, "?nome=" & nome, Nothing)
      Dim current_session = HttpContext.Current.Session
      Dim rWorkers As New List(Of Remote_Workers)

      With current_session
         .Item("rWorkers") = Nothing
         .Item("msg_rworkers") = Nothing

         Try
            response = Await client.GetAsync(vuri)

            If response.IsSuccessStatusCode Then
               Dim workersJsonString = response.Content.ReadAsStringAsync
               rWorkers = JsonConvert.DeserializeObject(Of Remote_Workers())(Await workersJsonString).ToList
               .Item("rWorkers") = rWorkers
            Else
               .Item("msg_rworkers") = "Falha detectada ao listar Funcionários" & vbCrLf & "Codigo de resposta: " & response.StatusCode
            End If
         Catch ex As Exception
            .Item("msg_rworkers") = "Falha detectada ao listar Funcionários" & vbCrLf & ex.Message
         End Try
      End With

      Return rWorkers
   End Function

#End Region

#Region " Workers_New_TagsFocus - Adiciona os novos tags em Lista_Return, atualiza Lista_Cad e insere os novos tags em lFocus (Workers). "

   Shared Sub Workers_New_TagsFocus(qWorkers As IEnumerable(Of Object), add As Boolean, ByRef Lista_Cad As Lista_Parametros, ByRef Lista_Return As Lista_Request, ByRef lFocus As List(Of Lista_Hide), last_focus_id As String, Optional clear As Boolean? = Nothing)
      Set_Htbl_Cmd_NewTags_lFocus(iprm.Workers, qWorkers, add, Lista_Cad, Lista_Return, lFocus, last_focus_id, clear)
   End Sub

#End Region

#Region " Work_Filter_TagsFocus - Adiciona o tag Work_Filter_Descricao em Lista_Return, atualiza Lista_Cad e insere o novo tag em lFocus. "

   Shared Sub Work_Filter_TagsFocus(ByRef Lista_Cad As Lista_Parametros, ByRef Lista_Return As Lista_Request, ByRef lFocus As List(Of Lista_Hide))
      Dim qTags = (From t In {iprm.Work_Filter_Descricao} Select New Lista_NewTags With {.id = t}).ToList
      Dim lTags = (From t In {iprm.Work_Filter_Descricao} Select New Lista_Hide With {.id = t, .hide = False}).ToList

      Lista_Return.newtags.AddRange(qTags)

      Set_Value(Lista_Return, iprm.Work_Filter_Descricao, "")

      Lista_Cad.Work_Filter_Descricao = ""

      AddTags_lFocus(Nothing, lTags, lFocus)
   End Sub

#End Region

#Region " Work_Adicionar_Check - Adiciona linha de Work "

   Shared Sub Work_Adicionar_Check(caller_event As String, ByRef Lista_Cad As Lista_Parametros, ByRef Lista_Return As Lista_Request, ByRef lFocus As List(Of Lista_Hide))
      Dim next_line = Get_Next_Line(Lista_Cad.Workers)
      Dim last_tag As String = iprm.Work_Data_ & Lista_Cad.Workers.OrderBy(Function(x) x.linha).Last.linha
      Dim qWorkers As New List(Of Lista_Workers) From {New Lista_Workers With {.linha = next_line}}

      Workers_New_TagsFocus(qWorkers, True, Lista_Cad, Lista_Return, lFocus, last_tag)
      Lista_Return.next_focus = iprm.Work_Nome_ & next_line
   End Sub

#End Region

#Region " Work_Remover_Check - Remove linha de Work "

   Shared Sub Work_Remover_Check(caller_event As String, ByRef Lista_Cad As Lista_Parametros, ByRef Lista_Return As Lista_Request, ByRef lFocus As List(Of Lista_Hide))
      Dim linha = Get_Linha(caller_event)
      Dim work = Lista_Cad.Workers.Find(Function(x) x.linha = linha)

      Remover_linha(iprm.Workers, work, Lista_Cad.Workers, Lista_Return, lFocus)
   End Sub

#End Region

#Region " Work_Selecao_Check - Chama trOptions_Check c/ parametros de imagem e video "

   Shared Sub Work_Selecao_Check(focus_element As String, caller_event As String, ByRef Lista_Cad As Lista_Parametros, ByRef Lista_Return As Lista_Request, ByRef lFocus As List(Of Lista_Hide), user_token As UserToken, dElements As Dictionary(Of String, String))
      trOptions_Check(iprm.Workers, focus_element, caller_event, False, False, False, iprm.Work, Nothing, Lista_Cad, Lista_Return, lFocus, iprm.Workers)
   End Sub

#End Region

#Region " Work_Filter_Descricao_Check - Filtra os nomes "

   Shared Sub Work_Filter_Descricao_Check(focus_element As String, caller_event As String, ByRef Lista_Cad As Lista_Parametros, ByRef Lista_Return As Lista_Request, ByRef lFocus As List(Of Lista_Hide), user_token As UserToken, dElements As Dictionary(Of String, String))
      Dim DB As New DBWorkersEntities
      Dim qWorkers As New List(Of Lista_Workers)
      Dim sequencia As Integer = 1
      Dim nome = Lista_Cad.Work_Filter_Descricao

      nome = If(isSelectNull(nome), Nothing, nome)

      Open_All_Check(Lista_Cad, Lista_Return, lFocus)

      qWorkers = (From x In DB.Workers Where If(nome = Nothing, True, x.Nome.StartsWith(nome)) Order By x.Nome
                  Select New Lista_Workers With {.Id = x.Id,
                                                 .Nome = x.Nome,
                                                 .Email = x.Email,
                                                 .Cargo = x.Cargo,
                                                 .Salario = x.Salario,
                                                 .Data = x.Data_Contratacao,
                                                 .fromDB = True
                                                }).ToList

      Set_enUS_Culture(True)

      If qWorkers.Count > 0 Then
         For Each work In qWorkers
            With work
               .linha = sequencia.ToString("0000")
               .Salario = Decimal.Parse(.Salario).ToString("N2", cc)
               .Data = Format(Date.Parse(.Data), "dd/MM/yy")
               sequencia += 1
            End With
         Next
      Else
         qWorkers.Add(New Lista_Workers With {.linha = "0001"})
      End If

      Set_enUS_Culture(False)

      Workers_New_TagsFocus(qWorkers, True, Lista_Cad, Lista_Return, lFocus, Nothing, True)
      Lista_Return.next_focus = iprm.Work_Filter_Descricao
   End Sub

#End Region

#Region " Work_Nome_Check - Consiste o Nome "

   Shared Sub Work_Nome_Check(focus_element As String, caller_event As String, ByRef Lista_Cad As Lista_Parametros, ByRef Lista_Return As Lista_Request, ByRef lFocus As List(Of Lista_Hide), user_token As UserToken, dElements As Dictionary(Of String, String))
      Dim linha = Get_Linha(focus_element)
      Dim work = Lista_Cad.Workers.Find(Function(x) x.linha = linha)

      If Not String.IsNullOrWhiteSpace(work.Nome) Then
         Dim grp = Lista_Cad.Workers.Where(Function(x) Not String.IsNullOrWhiteSpace(x.Nome)).GroupBy(Function(x) x.Nome.ToUpper).Where(Function(g) g.Count > 1)

         If grp.Count > 0 Then Set_Msg(Lista_Return, Get_Message(iprm.Work_Nome, imsg.Dup), iprm.Work_Nome_ & grp.Last.Last.linha, False)
      End If
   End Sub

#End Region

#Region " Work_Email_Check - Consiste o Email "

   Shared Sub Work_Email_Check(focus_element As String, caller_event As String, ByRef Lista_Cad As Lista_Parametros, ByRef Lista_Return As Lista_Request, ByRef lFocus As List(Of Lista_Hide), user_token As UserToken, dElements As Dictionary(Of String, String))
      Dim linha = Get_Linha(focus_element)
      Dim work = Lista_Cad.Workers.Find(Function(x) x.linha = linha)

      If Not String.IsNullOrWhiteSpace(work.Email) Then
         If Consiste_Email(work.Email) Then
            Dim grp = Lista_Cad.Workers.Where(Function(x) Not String.IsNullOrWhiteSpace(x.Email)).GroupBy(Function(x) x.Email.ToUpper).Where(Function(g) g.Count > 1)

            If grp.Count > 0 Then Set_Msg(Lista_Return, Get_Message(iprm.Work_Email, imsg.Dup), iprm.Work_Email_ & grp.Last.Last.linha, False)
         Else
            Set_Msg(Lista_Return, Get_Message(iprm.Work_Email, imsg.Inconsistente), iprm.Work_Email_ & linha, False, True)
         End If
      End If
   End Sub

#End Region

#Region " Work_Salario_Check - Consiste o Salario "

   Shared Sub Work_Salario_Check(focus_element As String, caller_event As String, ByRef Lista_Cad As Lista_Parametros, ByRef Lista_Return As Lista_Request, ByRef lFocus As List(Of Lista_Hide), user_token As UserToken, dElements As Dictionary(Of String, String))
      Dim linha = Get_Linha(focus_element)
      Dim msg As String = Nothing
      Dim work = Lista_Cad.Workers.Find(Function(x) x.linha = linha)

      With work
         If Not MinMax(iprm.Work_Salario, .Salario, True, msg, .Salario) Then
            Set_Msg(Lista_Return, msg, iprm.Work_Salario_ & .linha, False, True)
         End If

         Set_Value(Lista_Return, iprm.Work_Salario_ & linha, .Salario)
      End With
   End Sub

#End Region

#Region " Work_Data_Check - Consiste a data de contratacao "

   Shared Sub Work_Data_Check(focus_element As String, caller_event As String, ByRef Lista_Cad As Lista_Parametros, ByRef Lista_Return As Lista_Request, ByRef lFocus As List(Of Lista_Hide), user_token As UserToken, dElements As Dictionary(Of String, String))
      Dim msg As String = Nothing
      Dim work = Lista_Cad.Workers.Find(Function(x) x.linha = Get_Linha(focus_element))

      With work
         If DataMinMax(iprm.Work_Data, .Data, .Data, True, msg) Then
            Set_Value(Lista_Return, iprm.Work_Data_ & .linha, .Data)
         Else
            Set_Msg(Lista_Return, msg, iprm.Work_Data_ & .linha, False, True)
         End If

         If Not String.IsNullOrWhiteSpace(.Nome) Then
            If Lista_Cad.Workers.Last.linha = work.linha Then Work_Adicionar_Check(iprm.Work_Selecao_ & work.linha, Lista_Cad, Lista_Return, lFocus)
         End If
      End With
   End Sub

#End Region

#End Region

   ' Algumas Subs de Suporte às Subs de ação do callback

#Region " Open_All_Check - Processa algumas informacoes iniciais, apos a abertura do panel pelo accordion, seta acc_Panel, apaga as informacoes de Lista_Cad, lFocus e etc. "

   Shared Sub Open_All_Check(ByRef Lista_Cad As Lista_Parametros, ByRef Lista_Return As Lista_Request, ByRef lFocus As List(Of Lista_Hide))
      Close_All_Check(Lista_Cad, Lista_Return, lFocus)

      Set_Hide(Lista_Return, iprm.FieldInfoButton, False)
      Set_Hide(Lista_Return, iprm.SaveButton, False)
      Set_Hide(Lista_Return, iprm.ParametrosPanel, False)
      Set_ImageUrl(Lista_Return, iprm.SaveButton, True)
      Set_SortOrder(Nothing, Nothing, False, True)
      Set_Flags(iprm.SaveButton, False)

      Lista_Return.next_focus = "MessagesPanel"
   End Sub

#End Region

#Region " Close_All_Check - Apos fechamento do panel pelo accordion, apaga as informacoes de Lista_Cad, lFocus e etc.  "

   Shared Sub Close_All_Check(ByRef Lista_Cad As Lista_Parametros, ByRef Lista_Return As Lista_Request, ByRef lFocus As List(Of Lista_Hide))
      ' Inicializa Lista_Cad e lFocus e oculta o botao Processar
      Lista_Cad = New Lista_Parametros
      lFocus.Clear()

      With HttpContext.Current.Session
         .Item("Lista_Cad") = Lista_Cad
         .Item("lFocus") = lFocus
      End With

      Set_Hide(Lista_Return, iprm.SaveButton, True)
      Set_Flags(iprm.SaveButton, False)
      Set_SortOrder(Nothing, Nothing, False, True)

      'Inicializa algumas informacoes relacionadas ao navegador do bloco de paginas
      Lista_Return.exec.Add("Set_jqPagination(false,'',1,1)")
   End Sub

#End Region

#Region " SaveButton_Check - Chama PreUpdate_Check e se nao houver inconsistencias, chama UpdateDB "

   Shared Sub SaveButton_Check(focus_element As String, caller_event As String, ByRef Lista_Cad As Lista_Parametros, ByRef Lista_Return As Lista_Request, ByRef lFocus As List(Of Lista_Hide), user_token As UserToken, dElements As Dictionary(Of String, String))
      Dim DB_hasChanges As Boolean

      PreUpdate_Check(Lista_Cad, Lista_Return, user_token, lFocus)
      If Lista_Return.msg.Count = 0 Then
         If user_token.status = 0 Then
            UpdateDB(Lista_Cad, Lista_Return, user_token, DB_hasChanges)
            If Lista_Return.msg.Count = 0 Then
               HttpContext.Current.Session.Item("Lista_Cad_Excluidos") = Nothing
               Set_Flags(iprm.SaveButton, False)
               Set_ImageUrl(Lista_Return, iprm.SaveButton, True)
               Lista_Return.next_focus = CType(HttpContext.Current.Session.Item("Lista_Return"), Lista_Request).next_focus
            ElseIf Not DB_hasChanges Then
               Set_Flags(iprm.SaveButton, False)
               Set_ImageUrl(Lista_Return, iprm.SaveButton, True)
            End If
         Else
            Set_Msg(Lista_Return, user_token.msg)
         End If
      End If
      If Lista_Return.msg.Count > 0 Then Lista_Return.next_focus = "MessagesPanel"
   End Sub

#End Region

#Region " trOptions_Check - Verifica trOptions "
   ''' <summary>
   ''' Informar last_tag_focus somente se houver imagens relacionadas.
   ''' hasImage indica se a linha possui imagem, neste caso sera habilitado a lixeira para remover a imagem usando drag and drop.
   ''' </summary>
   Shared Sub trOptions_Check(acc_Panel As String, focus_element As String, caller_event As String, useImage As Boolean, useVideo As Boolean, hasImage As Boolean, start_name As String, last_tag_focus As String, ByRef Lista_Cad As Lista_Parametros, ByRef Lista_Return As Lista_Request, ByRef lFocus As List(Of Lista_Hide), tbl As String)
      Dim linha = Get_Linha(focus_element)

      If Not caller_event.Equals("Remove_trOptions") Then
         Set_trOptions(True, tbl, linha, Lista_Return)

         HttpContext.Current.Session.Item("last_line_trOption") = linha
      Else
         Dim TblName As List(Of Lista_TblNames) = HttpContext.Current.Session.Item("TblNames")
         Dim tbl_master = TblName.Find(Function(x) x.tbl = tbl).tbl_referencia

         Set_Flags(tbl_master, False)
         HttpContext.Current.Session.Item("last_line_trOption") = Nothing
      End If
   End Sub

#End Region

   ' Subs relacionadas a inicializacao da pagina

#Region " Start_Check - Efetua procedimentos de incializacao dos elementos da pagina "
   ''' <summary>
   ''' Indica que o usuario fez o 1o. acesso ou teclou F5 (PostBack) ou retornou a esta pagina.
   ''' </summary>
   Shared Sub Start_Check(ByRef Lista_Cad As Lista_Parametros, ByRef Lista_Return As Lista_Request, ByRef lFocus As List(Of Lista_Hide), user_token As UserToken)
      Set_Clear_Session(New List(Of String), user_token)
      Initialize_SomeDBInfo_To_Session()
      Set_Tema_User(user_token, Lista_Return)
      Set_Cmd_Init(Lista_Return)
      Set_Hide(Lista_Return, iprm.HeaderPanel, False)

      'Adicionado especificamente para a chamada deste projeto
      Lista_Return.exec.Add("Collect_Params(""Workers"", ""open"", false, true);")
   End Sub

#End Region

#Region " Start_QS - Chama Set_Clear_Session, Start_Check e adiciona qs = True em Lista_Return.extra "

   Shared Sub Start_QS_Check(ByRef Lista_Cad As Lista_Parametros, ByRef Lista_Return As Lista_Request, ByRef lFocus As List(Of Lista_Hide), user_token As UserToken)
      Start_Check(Lista_Cad, Lista_Return, lFocus, user_token)
      Lista_Return.extra.Add(New Lista_Extra With {.qs = True})
   End Sub

#End Region

   ' ***** Funcoes relacionadas a atualizacao e leitura do banco de dados *****

#Region " PreUpdate_Check - Checa a informação de todos os campos antes da atualizacao do Banco de Dados "

   Shared Sub PreUpdate_Check(ByRef Lista_Cad As Lista_Parametros, ByRef Lista_Return As Lista_Request, user_token As UserToken, ByRef lFocus As List(Of Lista_Hide))
      Dim DB As New DBWorkersEntities
      Dim DBy As New DBSystemEntities
      Dim dMsg, dMsg2 As New Dictionary(Of String, String)
      Dim msg As String = Nothing
      Dim data_formatada As String = Nothing

      With Lista_Cad
         ' Workers

         Dim Workers_Aux As New List(Of Lista_Workers)

         For Each work In .Workers.Where(Function(x) Not String.IsNullOrWhiteSpace(x.Nome))
            With work
               Workers_Aux.Add(work)

               If Not isSelectNull(.Email) Then
                  If Not Consiste_Email(work.Email) Then
                     dMsg_Add(iprm.Work_Email, .linha, Get_Message(iprm.Work_Email, imsg.Inconsistente), dMsg)
                  End If
               Else
                  dMsg_Add(iprm.Work_Email, .linha, Get_Message(iprm.Work_Email, imsg.Empty), dMsg)
               End If

               If isSelectNull(.Cargo) Then dMsg_Add(iprm.Work_Cargo, .linha, Get_Message(iprm.Work_Cargo, imsg.Empty), dMsg)

               If Not MinMax(iprm.Work_Salario, .Salario, True, msg, .Salario) Then
                  dMsg_Add(iprm.Work_Salario, .linha, msg, dMsg)
               End If

               If Not DataMinMax(iprm.Work_Data, .Data, .Data, True, msg) Then
                  dMsg_Add(iprm.Work_Data, .linha, msg, dMsg)
               End If
            End With
         Next

         If Workers_Aux.Count > 0 Then
            Dim grp = Workers_Aux.GroupBy(Function(x) x.Nome.ToUpper).Where(Function(g) g.Count > 1)

            If grp.Count > 0 Then dMsg_Add(iprm.Work_Nome, grp.Last.Last.linha, Get_Message(iprm.Work_Nome, imsg.Dup), dMsg)
         End If
      End With

      Dim lMsgErr, lMsgErr2 As New List(Of Lista_MsgErr)

      dMsg.ToList.ForEach(Sub(x) lMsgErr.Add(New Lista_MsgErr With {.id = x.Key, .msg = x.Value}))

      lMsgErr_Check(lMsgErr, lMsgErr2, Lista_Return)
   End Sub

#End Region

#Region " UpdateDB - Insere ou atualiza as informacoes no banco de dados "

   Shared Sub UpdateDB(ByRef Lista_Cad As Lista_Parametros, ByRef Lista_Return As Lista_Request, user_token As UserToken, ByRef DB_hasChanges As Boolean)
      With Lista_Cad
         Dim DB As New DBWorkersEntities
         Dim Excluidos As New Lista_Parametros
         Dim dSeq As New Dictionary(Of String, Integer)
         Dim data_update As Date = Now
         Dim last_sequence As Integer
         Dim found As Boolean

         If HttpContext.Current.Session.Item("Lista_Cad_Excluidos") IsNot Nothing Then
            Excluidos = HttpContext.Current.Session.Item("Lista_Cad_Excluidos")
         End If

         ' Workers
         Dim _work As New Workers

         For Each work In Excluidos.Workers.Where(Function(x) x.excluido)
            _work = DB.Workers.Where(Function(x) x.Id = work.Id).FirstOrDefault
            'If _work IsNot Nothing Then Set_Row_Exclusao(_work, data_update, True)
            If _work IsNot Nothing Then DB.Workers.Remove(_work)
         Next

         last_sequence = (From p In DB.Workers Order By p.Id Descending Select p.Id).FirstOrDefault

         For Each work In .Workers.Where(Function(x) Not String.IsNullOrWhiteSpace(x.Nome))
            With work
               _work = DB.Workers.Where(Function(x) If(.Id.HasValue, x.Id = .Id, False)).FirstOrDefault
               found = _work IsNot Nothing

               If Not found AndAlso Not .Id.HasValue Then 'Caso tenha alguma row excluida c/ a mesma Descricao da nova linha, reativa a row. 
                  _work = DB.Workers.Where(Function(x) x.Nome = .Nome).FirstOrDefault
                  found = _work IsNot Nothing
               End If

               If Not found Then
                  _work = New Workers

                  last_sequence += 1
                  _work.Id = last_sequence
               End If

               _work.Nome = .Nome
               _work.Email = .Email
               _work.Cargo = .Cargo
               _work.Salario = CDec(.Salario)
               _work.Data_Contratacao = CDate(.Data)

               'Set_Row_Exclusao(_work, data_update, False)

               If Not found Then DB.Workers.Add(_work)

               .Id = _work.Id
               .fromDB = True
            End With
         Next

         DB_hasChanges = DB.ChangeTracker.HasChanges

         DB_UpdateRows(DB, data_update, Lista_Return, user_token)
      End With
   End Sub

#End Region

End Class