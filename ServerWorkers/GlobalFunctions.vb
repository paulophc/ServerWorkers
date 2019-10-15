Imports System.IO
Imports System.Security.Cryptography
Imports Newtonsoft.Json
Imports System.Threading
Imports System.Globalization
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization
Imports System.Reflection

Public Enum mode
   Search
   Insert
   Update
End Enum

Public Enum sv
   Produtos
   Servicos
   Movimentos_Financeiros
End Enum

Public Enum pncm
   simples_nacional
   regime_normal
   reducao
   diferimento
End Enum

Public Enum tcond
   isNothing
   isSelectNull
   IsNullOrWhiteSpace
   isByte
   isShort
   isInteger
   isDecimal
   isLong
   is0_8
   is1_11
End Enum

Public Enum xlAlign
   left
   right
   center
End Enum

Public Enum grFails
   Importacao
   Exclusao
   PreUpdate
End Enum

Public Enum mvt
   Novo
   Aberto
   Fechado
   Cancelado
End Enum

Public Module GlobalFunctions
   Private Const kSalt As String = "electrons and quarks"
   Private img_path As New imgpath
   Private imsg As New iMensagens
   Private lastDate As Long
   Public linguagem As Byte = 1 'pt-BR linguagem padrão
   Public cc As CultureInfo = New CultureInfo("pt-BR")

   '--- Inicializacoes / Finalizacoes ---

#Region " ...Inicializacoes / Finalizacoes... "

#Region " LoginOk_Check - Verifica o token e caso nao seja valido altera focus_element e define comando para redirecionar pagina p/ Login.aspx "

   Public Function LoginOk_Check(ByRef caller_event As String, user_token As UserToken, ByRef Lista_Return As Lista_Request) As Boolean
      Dim ok As Boolean = True

      If Not user_token.status.Equals(0) Then
         caller_event = "Start_GoTo_Login"
         Lista_Return.extra.Add(New Lista_Extra With {.newurl = "Login.aspx"})
         ok = False
      End If

      'Salva process_last_check na Session, pois algumas subs fazem a verificacao de LoginOk, ReadDB por exemplo.
      HttpContext.Current.Session.Item("process_last_check") = ok

      Return ok ' Retorna False se for necessario redirecionar p/ Login.aspx
   End Function

#End Region

#Region " User_Check - Processa informacoes relacionadas ao Login de Usuario (Logout, AlterLogin, RemoveSession e etc.) "

   Private Sub User_Check(focus_element As String, caller_event As String, ByRef Lista_Return As Lista_Request, user_token As UserToken)
      With Lista_Return.extra
         If focus_element = "CadUserButton" Then
            If user_token.status = 0 Then
               Dim qsvalue = New Dictionary(Of String, String) From {{" Thenprevious_page", caller_event}, {"Tipo_Cadastro", "U"}, {"Cadastro", user_token.user_id.ToString}}

               .Add(New Lista_Extra With {.qspagina = "Cadastros.aspx", .qsvalue = JsonConvert.SerializeObject(qsvalue), .newurl = "Cadastros.aspx"})
            Else
               Set_Msg(Lista_Return, user_token.msg)
            End If

         ElseIf focus_element = "AlterLoginButton" Then
            .Add(New Lista_Extra With {.newurl = "Login.aspx"})

         ElseIf focus_element = "LogoffButton" Then
            If user_token.status = 0 OrElse user_token.status = 7 Then
               user_token.logon = False
               Lista_Return.token = TokenCrypto(user_token)
               'Teste posteriormente modificar p/ ao inves do reload, ir para a pagina de apresentacao do sistema.
               .Add(New Lista_Extra With {.reload = True})
            Else
               Set_Msg(Lista_Return, user_token.msg)
            End If
         End If
      End With
   End Sub

#End Region

#Region " Last_Check - Atualiza lFocus c/ informacoes de .prop.hide, inicializa .tabindex, salva alguns parametros na Session e chama msg_close_check "

   Public Sub Last_Check(focus_element As String, caller_event As String, ByRef Lista_Cad As Object, ByRef Lista_Return As Lista_Request, ByRef lFocus As List(Of Lista_Hide), modulo As String)
      Dim h As Lista_Hide
      Dim shide As String()

      With Lista_Return
         For Each item In .hide
            shide = Split(item, ";")
            h = lFocus.Find(Function(x) x.id = shide(0))
            If h IsNot Nothing Then h.hide = CBool(shide(1))
         Next

         If .newtags.Count > 0 OrElse Get_Flags("tabindex_check") Then
            .tabindex.Clear()
            .tabindex.AddRange(lFocus)
            Set_Flags("tabindex_check", False)
         End If
      End With

      With HttpContext.Current.Session
         .Item("focus_element") = focus_element
         .Item("caller_event") = caller_event
         .Item("Lista_Cad") = Lista_Cad
         .Item("lFocus") = lFocus
         .Item("Lista_Return") = Lista_Return
         .Item("modulo") = modulo
         .Item("last_access") = Now
      End With
   End Sub

#End Region

#Region " Serialize_JSON - Serializa c/ o formato JSON as informacoes de Lista_Return em result "

   Public Function Serialize_JSON(ByRef Lista_Return As Lista_Request) As String
      Dim result As String

      With Lista_Return
         ' Prepara as listas para serializacao
         Dim box_effects = False

         .jfunctions.Clear()
         If .tema IsNot Nothing Then .jfunctions.Add(0)
         If .cmip.Count > 0 Then .jfunctions.Add(1) Else .cmip = Nothing
         If .cmdi.Count > 0 Then .jfunctions.Add(2) Else .cmdi = Nothing
         If .cmpi IsNot Nothing Then .jfunctions.Add(3)
         If .cmti.Count > 0 Then .jfunctions.Add(4) Else .cmti = Nothing
         If .cmbi.Count > 0 Then .jfunctions.Add(5) Else .cmbi = Nothing
         If .cmri.Count > 0 Then .jfunctions.Add(6) Else .cmri = Nothing
         If .exec_first.Count > 0 Then .jfunctions.Add(7) Else .exec_first = Nothing
         If .remove.Count > 0 Then .jfunctions.Add(8) : box_effects = True Else .remove = Nothing
         If .remove_dd.Count > 0 Then .jfunctions.Add(9) : box_effects = True Else .remove_dd = Nothing
         If .remove_linha.Count > 0 Then .jfunctions.Add(10) : box_effects = True Else .remove_linha = Nothing
         If .hide.Count > 0 Then .jfunctions.Add(11) : box_effects = True Else .hide = Nothing
         If .cmd.Count > 0 Then .jfunctions.Add(12) : box_effects = True Else .cmd = Nothing
         If .trOptions.Count > 0 Then .jfunctions.Add(13) : box_effects = True Else .trOptions = Nothing
         If .cmd_btn.Count > 0 Then .jfunctions.Add(14) Else .cmd_btn = Nothing
         If .cmd_span.Count > 0 Then .jfunctions.Add(15) Else .cmd_span = Nothing
         If .cmd_img.Count > 0 Then .jfunctions.Add(16) Else .cmd_img = Nothing
         If .autocompl.Count > 0 Then .jfunctions.Add(17) Else .autocompl = Nothing
         If .autocompl_enable.Count > 0 Then .jfunctions.Add(18) Else .autocompl_enable = Nothing
         If .combo.Count > 0 Then .jfunctions.Add(19) Else .combo = Nothing
         If .value.Count > 0 Then .jfunctions.Add(20) : box_effects = True Else .value = Nothing
         If .text.Count > 0 Then .jfunctions.Add(21) Else .text = Nothing
         If .prop.Count > 0 Then .jfunctions.Add(22) Else .prop = Nothing
         If .css.Count > 0 Then .jfunctions.Add(23) Else .css = Nothing
         If .tabindex.Count > 0 Then .jfunctions.Add(24) Else .tabindex = Nothing
         If .next_focus IsNot Nothing Then .jfunctions.Add(25) : box_effects = If(.next_focus <> "MessagesPanel", True, box_effects)
         If .newtags.Count > 0 Then .jfunctions.Add(26) : box_effects = True Else .newtags = Nothing
         If box_effects Then .jfunctions.Add(27) : .box_effects = "true" Else .box_effects = Nothing
         If .token IsNot Nothing Then .jfunctions.Add(28)
         If .exec.Count > 0 Then .jfunctions.Add(29) Else .exec = Nothing
         If .cmd IsNot Nothing Then .jfunctions.Add(30)
         If .msg.Count > 0 Then .jfunctions.Add(31) Else .msg = Nothing
         If .yesno.Count > 0 Then .jfunctions.Add(32) Else .yesno = Nothing
         If .extra.Count > 0 Then .jfunctions.Add(33) Else .extra = Nothing
         If .jfunctions.Count = 0 Then .jfunctions = Nothing

         'Serializa as informacoes de Lista_Return
         result = JsonConvert.SerializeObject(Lista_Return, New JsonSerializerSettings With {.NullValueHandling = NullValueHandling.Ignore})

         'Reinstacia as listas c/ Nothing apos a serializacao

         If .cmip Is Nothing Then .cmip = New List(Of cmip)
         If .cmdi Is Nothing Then .cmdi = New List(Of cmdi)
         If .cmti Is Nothing Then .cmti = New List(Of cmti)
         If .cmbi Is Nothing Then .cmbi = New List(Of cmbi)
         If .cmri Is Nothing Then .cmri = New List(Of cmri)
         If .exec_first Is Nothing Then .exec_first = New List(Of String)
         If .remove Is Nothing Then .remove = New List(Of String)
         If .remove_dd Is Nothing Then .remove_dd = New List(Of String)
         If .remove_linha Is Nothing Then .remove_linha = New List(Of String)
         If .hide Is Nothing Then .hide = New List(Of String)
         If .cmd Is Nothing Then .cmd = New List(Of cmd)
         If .trOptions Is Nothing Then .trOptions = New List(Of String)
         If .cmd_btn Is Nothing Then .cmd_btn = New List(Of cmd_btn)
         If .cmd_span Is Nothing Then .cmd_span = New List(Of cmd_span)
         If .cmd_img Is Nothing Then .cmd_img = New List(Of cmd_img)
         If .autocompl Is Nothing Then .autocompl = New List(Of String)
         If .autocompl_enable Is Nothing Then .autocompl_enable = New List(Of String)
         If .combo Is Nothing Then .combo = New List(Of String)
         If .value Is Nothing Then .value = New List(Of String)
         If .text Is Nothing Then .text = New List(Of String)
         If .prop Is Nothing Then .prop = New List(Of String)
         If .css Is Nothing Then .css = New List(Of String)
         If .tabindex Is Nothing Then .tabindex = New List(Of Lista_Hide)
         If .newtags Is Nothing Then .newtags = New List(Of Lista_NewTags)
         If .exec Is Nothing Then .exec = New List(Of String)
         If .yesno Is Nothing Then .yesno = New List(Of Lista_YesNo)
         If .msg Is Nothing Then .msg = New List(Of Lista_Msg)
         If .extra Is Nothing Then .extra = New List(Of Lista_Extra)
         If .jfunctions Is Nothing Then .jfunctions = New List(Of Integer)
      End With

      Return result
   End Function

#End Region

#Region " Method_Check - Efetua as chamadas para as Subs correspondentes "

   Public Sub Method_Check(focus_element As String, caller_event As String, ByRef Lista_Cad As Object, ByRef Lista_Return As Lista_Request, ByRef lFocus As List(Of Lista_Hide), dElements As Dictionary(Of String, String), user_token As UserToken, webform_name As String)
      Dim campo = If(IsNumeric(focus_element.Last), focus_element.Remove(focus_element.LastIndexOf("_")), focus_element)
      Dim classType = Type.GetType(Assembly.GetExecutingAssembly.GetName().Name & "." & webform_name)
      Dim classObject = classType.GetConstructor(Type.EmptyTypes).Invoke(New Object() {})
      Dim classMethod = classType.GetMethod(campo & "_Check")
      Dim parameters As New Object()
      Dim byref_order As Integer() = {}

      parameters = {focus_element, caller_event, Lista_Cad, Lista_Return, lFocus, user_token, dElements}
      byref_order = {2, 3, 4}

      If caller_event.Equals("enter") OrElse caller_event.Equals("change") Then
         If classMethod Is Nothing Then
            Dim dCampos_Methods As Dictionary(Of String, String) = HttpContext.Current.Session.Item("dCampos_Methods")

            If dCampos_Methods.ContainsKey(campo) Then classMethod = classType.GetMethod(dCampos_Methods(campo) & "_Check")
         End If
      Else
         If caller_event = "open" Then
            classMethod = classType.GetMethod(focus_element & "_Open_Check")
            parameters = {focus_element, caller_event, Lista_Cad, Lista_Return, user_token, lFocus}
            byref_order = {2, 3, 5}

         ElseIf caller_event = "sort" Then
            classMethod = classType.GetMethod("Workers_Sort_Check")
            parameters = {focus_element, Lista_Cad, Lista_Return, lFocus}
            byref_order = {1, 2, 3}

         ElseIf caller_event = "close" Then
            classMethod = classType.GetMethod("Close_All_Check")
            parameters = {Lista_Cad, Lista_Return, lFocus}
            byref_order = {0, 1, 2}

         ElseIf caller_event.StartsWith("Start") Then
            If LoginOk_Check(caller_event, user_token, Lista_Return) Then
               classMethod = classType.GetMethod(caller_event & "_Check")
               parameters = {Lista_Cad, Lista_Return, lFocus, user_token}
               byref_order = {0, 1, 2, 4}
            Else
               classMethod = Nothing
            End If

         ElseIf focus_element.EndsWith("Adicionar") OrElse focus_element.EndsWith("Remover") Then
            parameters = {caller_event, Lista_Cad, Lista_Return, lFocus}
            byref_order = {1, 2, 3}

         ElseIf focus_element.StartsWith("Excluir_") Then
            classMethod = classType.GetMethod("Excluir_Check")

         ElseIf focus_element = "DeleteButton" Then
            classMethod = Nothing
            Set_YesNo(Lista_Return, imsg.Excluir, MsgBoxResult.No, True, True)

         ElseIf focus_element = "FieldInfoButton" Then
            classMethod = Nothing
            Get_Field_Info(caller_event, Lista_Return)

         End If
      End If

      Execute_Invoke(classObject, classMethod, parameters, byref_order, Lista_Cad, Lista_Return, lFocus)
   End Sub

#End Region

#Region " Execute_Invoke - Executa Invoke "

   Public Sub Execute_Invoke(classObject As Object, classMethod As MethodInfo, parameters As Object(), byref_order As Integer(), ByRef Lista_Cad As Object, ByRef Lista_Return As Lista_Request, ByRef lFocus As List(Of Lista_Hide))
      If classMethod IsNot Nothing Then
         classMethod.Invoke(classObject, parameters)

         If byref_order.Count > 0 Then Lista_Cad = parameters(byref_order(0))
         If byref_order.Count > 1 Then Lista_Return = parameters(byref_order(1))
         If byref_order.Count > 2 Then lFocus = parameters(byref_order(2))
      End If
   End Sub

#End Region

#End Region

   '--- Serializa / Deserializa informacoes em Lista_Cad ---

#Region " ...Serializa / Deserializa informacoes em Lista_Cad... "

#Region " Set_Lista_Cad - Inicializa Lista_Cad com As informacoes deserializadas recebidas no callback e Lista_Cad_Previous c/ As informacoes de Lista_Cad "

   Public Sub Set_Lista_Cad(ByRef Lista_Cad As Object, dElements As Dictionary(Of String, String))
      Dim TblNames As List(Of Lista_TblNames) = HttpContext.Current.Session.Item("TblNames")
      Dim lNames = Lista_Cad.GetType.GetProperties.Where(Function(x) x.PropertyType.Name <> "List`1").Select(Function(x) x.Name).ToList
      Dim dItens1 = (From x In dElements Where lNames.Contains(x.Key))
      Dim dItens2 As New List(Of KeyValuePair(Of String, String))
      Dim lRows As IEnumerable(Of Object)
      Dim item As KeyValuePair(Of String, String)
      Dim row As Object
      Dim row_type As Type
      Dim linha, pname As String

      With HttpContext.Current.Session
         If .Item("Lista_Cad") IsNot Nothing Then Lista_Cad = .Item("Lista_Cad")
      End With

      For Each item In dItens1
         ' Atualiza Lista_Cad c/ as informacoes de dElements
         Lista_Cad.GetType.GetProperty(item.Key).SetValue(Lista_Cad, item.Value)
      Next

      For Each tbname In TblNames
         If Lista_Cad.GetType.GetProperties.Where(Function(x) x.Name = tbname.tbl AndAlso x.PropertyType.Name = "List`1").Count > 0 Then
            lRows = Lista_Cad.GetType.GetProperty(tbname.tbl).GetValue(Lista_Cad)
            dItens2 = (From x In dElements Where x.Key.StartsWith(tbname.start_name & "_") AndAlso IsNumeric(x.Key.Last) Order By x.Key).ToList

            For Each item In dItens2
               linha = Get_Linha(item.Key)
               row = lRows.Where(Function(x) x.linha = linha).FirstOrDefault

               If row Is Nothing Then
                  row_type = Type.GetType(Assembly.GetExecutingAssembly.GetName().Name & ".Lista_" & tbname.tbl)
                  row = row_type.GetConstructor(Type.EmptyTypes).Invoke(New Object() {})
                  row.GetType.GetProperty("linha").SetValue(row, linha)
                  lRows.GetType.GetMethod("Add").Invoke(lRows, New Object() {row})
                  row = lRows.Last
               End If

               pname = item.Key.Remove(item.Key.LastIndexOf("_")).Remove(0, item.Key.IndexOf("_") + 1)
               row.GetType.GetProperty(pname).SetValue(row, item.Value)
            Next
         End If
      Next
   End Sub

#End Region

#Region " Set_dElements - Inicializa dElements ou lValue com As informacoes de Lista_Cad "

   Public Sub Set_dElements(Lista_Cad As Object, ByRef dElements As Dictionary(Of String, String))
      dElements.Clear()

      With Lista_Cad.GetType
         For Each name In .GetProperties.Where(Function(x) x.PropertyType.Name <> "List`1").Select(Function(x) x.Name).ToList
            dElements.Add(name, .GetProperty(name).GetValue(Lista_Cad))
         Next
      End With
   End Sub

#End Region

#End Region

   '--- Clear Lista_Return, lFocus e etc / lFocus / Sort de Listas 

#Region " ...Clear Lista_Return, lFocus e etc / lFocus / Sort de Listas... "

#Region " Clear_Lista_Return_lFocus_Parcial - Remove alguns itens de Lista_Return e lFocus "

   Public Sub Clear_Lista_Return_lFocus_Parcial(ByRef Lista_Return As Lista_Request, ByRef lFocus As List(Of Lista_Hide), Optional remove_newtags_from_lFocus As Boolean = True)
      With Lista_Return
         If remove_newtags_from_lFocus Then
            For Each tag In .newtags
               lFocus.RemoveAll(Function(x) x.id = tag.id)
            Next
         End If

         .tema = Nothing
         .cmip.Clear()
         .cmdi.Clear()
         .cmpi = Nothing
         .cmti.Clear()
         .cmbi.Clear()
         .cmri.Clear()
         .exec_first.Clear()
         .remove.Clear()
         .remove_linha.Clear()
         .remove_dd.Clear()
         .hide.Clear()
         .cmd.Clear()
         .trOptions.Clear()
         .cmd_btn.Clear()
         .cmd_span.Clear()
         .cmd_img.Clear()
         .autocompl.Clear()
         .autocompl_enable.Clear()
         .combo.Clear()
         .value.Clear()
         .text.Clear()
         .prop.Clear()
         .css.Clear()
         .tabindex.Clear()
         .newtags.Clear()
         .exec.Clear()
         .yesno.Clear()
         .msg.Clear()
         .extra.Clear()
      End With
   End Sub

#End Region

#Region " AddTags_lFocus - Adiciona as novas tags em lFocus "

   Public Sub AddTags_lFocus(last_focus_id As String, lTags As List(Of Lista_Hide), ByRef lFocus As List(Of Lista_Hide))
      For Each tag In lTags
         lFocus.RemoveAll(Function(x) x.id = tag.id)
      Next

      Dim index = If(last_focus_id Is Nothing, -1, lFocus.FindIndex(Function(x) x.id = last_focus_id))

      If index = -1 Then
         lFocus.AddRange(lTags)
      Else
         lFocus.InsertRange(index + 1, lTags)
      End If
   End Sub

#End Region

#Region " Get_Next_Focus - Obtem o focus do proximo elemento "

   Public Function Get_Next_Focus(focus_element As String, lFocus As List(Of Lista_Hide)) As String
      Dim next_focus As String = Nothing
      Dim index = lFocus.FindIndex(Function(x) x.id = focus_element)

      If index >= 0 Then
         ' Procura em lFocus os elementos seguintes ao indice pois pode ter elementos c/ hide = True,
         ' neste caso procura o elemento na proxima ocorrencia c/ hide = False
         For i = (index + 1) To lFocus.Count - 1 Step 1
            If Not lFocus(i).hide Then
               next_focus = lFocus(i).id
               Exit For
            End If
         Next

         ' Caso nao encontre na sequencia posterior, verifica na sequencia anterior
         ' "Not x.id = focus_element" foi adicionado p/ nao retornar o mesmo id, caso so' tenha 
         ' uma informacao valida.
         If next_focus Is Nothing Then
            Dim h = lFocus.Find(Function(x) Not x.hide AndAlso Not x.id = focus_element)
            If h IsNot Nothing Then next_focus = h.id
         End If
      End If

      Return next_focus
   End Function

#End Region

#Region " Sort_List_DrillDown - Efetua o sort das Listas para tabelas que usam drilldown, usando Object e Reflection "

   Public Sub Sort_List_DrillDown(tbl_master As String, tbl_itens As String, qMaster_Obj As IEnumerable(Of Object), qItens_Obj As IEnumerable(Of Object), ByRef Lista_Cad As Object, ByRef Lista_Return As Lista_Request, ByRef lFocus As List(Of Lista_Hide), last_focus_id_master As String, last_focus_id_itens As String, Optional create_cmd As Boolean = True, Optional qMPDAs As IEnumerable(Of Object) = Nothing, Optional qMPromos As IEnumerable(Of Object) = Nothing)
      Dim acc_panel As String = HttpContext.Current.Session.Item("acc_panel")
      Dim TblNames As List(Of Lista_TblNames) = HttpContext.Current.Session.Item("TblNames")
      Dim tb_master = TblNames.Find(Function(x) x.tbl = tbl_master)
      Dim tb_itens = TblNames.Find(Function(x) x.tbl = tbl_itens)
      Dim start_name_master_ = tb_master.start_name & "_"
      Dim start_name_itens_ = tb_itens.start_name & "_"
      Dim dMaster, dItens, dMPDAs, dMPromos, dItens_Excluidos As New Dictionary(Of Object, String)
      Dim Master_Obj = Lista_Cad.GetType.GetProperty(tbl_master).GetValue(Lista_Cad)
      Dim Itens_Obj = Lista_Cad.GetType.GetProperty(tbl_itens).GetValue(Lista_Cad)
      Dim classType = Type.GetType(Assembly.GetExecutingAssembly.GetName().Name & "." & Get_Modulo(True))
      Dim classObject = classType.GetConstructor(Type.EmptyTypes).Invoke(New Object() {})
      Dim classMethod As MethodInfo
      Dim parameters As New Object()
      Dim byref_order As Integer() = {2, 3, 4}
      Dim sequencia_master As Integer = 1
      Dim sequencia_itens As Integer = 1

      'Remove algumas informacoes relacionadas ao Lista_Return, para prever casos de algumas
      'Subs como por exemplo Excel_Import que abrem o menu drilldown, carregando informacoes
      'em Lista_Return, e para nao ficar duplicado e' necessario remover previamente as informacoes.
      Clear_Lista_Return_lFocus_Parcial(Lista_Return, lFocus)

      'Remove de lFocus as informacoes relacionadas a Lista
      lFocus.RemoveAll(Function(x) x.id.StartsWith(start_name_master_) OrElse x.id.StartsWith(start_name_itens_))

      'Como e' indexada somente a lista relacionada ao campo, verifica se a lista esta vazia, entao inicializa c/ a lista correspondente.
      'Ex: p/ PFiltros, caso seja ordenada pelo Nome do Filtro, PFIs estara vazio entao inicializa c/ 'qItens_Obj = Itens_Obj'.
      If qMaster_Obj.Count = 0 Then qMaster_Obj = Master_Obj
      If qItens_Obj.Count = 0 Then qItens_Obj = Itens_Obj

      'E' utilizado o dicionario para armazenar as linhas de "Lista_Master" e "Lista_Itens", por exemplo PFiltro e PFI (PFI.PFiltro_linha)
      'ja que ao modificar a linha do "Lista_Master" por exemplo, PFiltro e' necessario modificar tambem Master_linha de "Lista_Itens" por exemplo PFiltro_linha de PFIs.
      qMaster_Obj.ToList.ForEach(Sub(x) dMaster.Add(x, x.linha))
      qItens_Obj.ToList.ForEach(Sub(x) dItens.Add(x, x.GetType.GetProperty(start_name_master_ & "linha").GetValue(x)))

      If qMPDAs IsNot Nothing Then qMPDAs.ToList.ForEach(Sub(x) dMPDAs.Add(x, x.linha))
      If qMPromos IsNot Nothing Then qMPromos.ToList.ForEach(Sub(x) dMPromos.Add(x, x.linha))

      For Each xmaster In dMaster
         xmaster.Key.linha = sequencia_master.ToString("0000")
         sequencia_master += 1

         For Each xitem In dItens.Where(Function(x) x.Value = xmaster.Value)
            xitem.Key.GetType.GetProperty(start_name_master_ & "linha").SetValue(xitem.Key, xmaster.Key.linha)
            xitem.Key.linha = sequencia_itens.ToString("0000")
            sequencia_itens += 1
         Next

         If Not create_cmd Then
            For Each xmpda In dMPDAs.Where(Function(x) x.Value = xmaster.Value)
               xmpda.Key.linha = xmaster.Key.linha
            Next

            For Each xmpromo In dMPromos.Where(Function(x) x.Value = xmaster.Value)
               xmpromo.Key.linha = xmaster.Key.linha
            Next
         End If
      Next

      If qMaster_Obj.Count > 0 Then
         Master_Obj.Clear()

         dMaster.Keys.ToList.ForEach(Sub(row) Master_Obj.Add(row))

         If create_cmd Then
            classMethod = classType.GetMethod(tbl_master & "_New_TagsFocus")
            parameters = {Master_Obj, False, Lista_Cad, Lista_Return, lFocus, last_focus_id_master, True}
            Execute_Invoke(classObject, classMethod, parameters, byref_order, Lista_Cad, Lista_Return, lFocus)
         End If
      End If

      If qItens_Obj.Count > 0 Then
         Itens_Obj.Clear()

         dItens.Keys.ToList.ForEach(Sub(row) Itens_Obj.Add(row))

         If create_cmd Then
            For Each row In dMaster.Keys.Where(Function(x) x.drilldown_open)
               classMethod = classType.GetMethod(start_name_master_ & "DrillDown_Check")
               parameters = {row.linha, "open_drilldown", Lista_Cad, Lista_Return, lFocus}
               Execute_Invoke(classObject, classMethod, parameters, byref_order, Lista_Cad, Lista_Return, lFocus)
            Next
         End If
      End If

      If qMPDAs IsNot Nothing Then
         Dim MPDAs_Obj = Lista_Cad.GetType.GetProperty("MPDAs").GetValue(Lista_Cad)

         MPDAs_Obj.Clear()
         dMPDAs.Keys.ToList.ForEach(Sub(row) MPDAs_Obj.Add(row))
      End If

      If qMPromos IsNot Nothing Then
         Dim MPromos_Obj = Lista_Cad.GetType.GetProperty("MPromos").GetValue(Lista_Cad)

         MPromos_Obj.Clear()
         dMPromos.Keys.ToList.ForEach(Sub(row) MPromos_Obj.Add(row))
      End If

      If (qMaster_Obj.Count > 0 OrElse qItens_Obj.Count > 0) AndAlso create_cmd Then
         Lista_Return.next_focus = CType(HttpContext.Current.Session.Item("Lista_Return"), Lista_Request).next_focus
      End If
   End Sub

#End Region

#Region " Sort_List - Efetua o sort das Listas para tabelas simples, usando Object e Reflection "

   Public Sub Sort_List(tbl As String, qLista_Obj As IEnumerable(Of Object), ByRef Lista_Cad As Object, ByRef Lista_Return As Lista_Request, ByRef lFocus As List(Of Lista_Hide), last_focus_id As String)
      Dim TblNames As List(Of Lista_TblNames) = HttpContext.Current.Session.Item("TblNames")
      Dim start_name_ As String = TblNames.Find(Function(x) x.tbl = tbl).start_name & "_"
      Dim Lista_Obj = Lista_Cad.GetType.GetProperty(tbl).GetValue(Lista_Cad)
      Dim classType = Type.GetType(Assembly.GetExecutingAssembly.GetName().Name & "." & Get_Modulo(True))
      Dim classObject = classType.GetConstructor(Type.EmptyTypes).Invoke(New Object() {})
      Dim classMethod = classType.GetMethod(tbl & "_New_TagsFocus")
      Dim parameters As New Object()
      Dim byref_order As Integer() = {2, 3, 4}
      Dim sequencia As Integer = 1

      'Remove algumas informacoes relacionadas ao Lista_Return, para prever casos de algumas
      'Subs como por exemplo Excel_Import que abrem o menu drilldown, carregando informacoes
      'em Lista_Return, e para nao ficar duplicado e' necessario remover previamente as informacoes.
      Clear_Lista_Return_lFocus_Parcial(Lista_Return, lFocus)

      'Remove de lFocus as informacoes relacionadas a Lista
      lFocus.RemoveAll(Function(x) x.id.StartsWith(start_name_))

      For Each row In qLista_Obj
         row.linha = sequencia.ToString("0000")
         sequencia += 1
      Next

      If qLista_Obj.Count > 0 Then
         Lista_Obj.Clear()

         qLista_Obj.ToList.ForEach(Sub(row) Lista_Obj.Add(row))

         parameters = {Lista_Obj, False, Lista_Cad, Lista_Return, lFocus, last_focus_id, True}
         Execute_Invoke(classObject, classMethod, parameters, byref_order, Lista_Cad, Lista_Return, lFocus)

         Lista_Return.next_focus = CType(HttpContext.Current.Session.Item("Lista_Return"), Lista_Request).next_focus
      End If
   End Sub

#End Region

#End Region

   '--- Session ---

#Region " ...Session... "

#Region " Session_InUse - Verifica se a session ja esta em uso "

   Public Function Session_InUse(ByRef result) As Boolean
      Dim ok As Boolean

      With HttpContext.Current.Session
         If .Item("SessionInUse") Is Nothing Then
            .Item("SessionInUse") = Now
         Else
            Dim Lista_Return2 As New Lista_Request

            .Item("SessionInUse") = Nothing
            Set_Msg(Lista_Return2, "Session ja esta em uso.<br>" & .Item("SessionInUse"))
            result = JsonConvert.SerializeObject(Lista_Return2)
            ok = True
         End If
      End With

      Return ok
   End Function

#End Region

#Region " Session_Start_Check - Verifica detalhes ref. à inicializacao de algumas variaveis, token, Start, QS, ComeBack, redirecionamento de chamada p/ Login e etc "

   Public Sub Session_Start_Check(ByRef modo As mode, ByRef caller_event As String, ByRef user_token As UserToken, ByRef Lista_Return As Lista_Request, ByRef lFocus As List(Of Lista_Hide), modulo As String)
      With HttpContext.Current.Session
         .Item("modo") = Nothing
         .Item("user_token") = user_token

         Dim process_last_check = LoginOk_Check(caller_event, user_token, Lista_Return)
      End With
   End Sub

#End Region

#Region " Set_Clear_Session - Apaga as informacoes da Session e inicializa a session novamente c/ modo, start_access, last_access, user_token e parametros informados no dicionario "

   Public Sub Set_Clear_Session(lSessionSave As List(Of String), user_token As UserToken, Optional ByRef modo As mode = mode.Search)
      With HttpContext.Current.Session
         Dim dSessionSave As New Dictionary(Of String, Object)

         'Salva informacao previa da session na Lista Lista_Value
         For Each item In lSessionSave
            If .Item(item) IsNot Nothing Then dSessionSave.Add(item, .Item(item))
         Next

         .RemoveAll() 'Apaga informacoes da session do usuario

         .Item("modo") = mode.Search : modo = mode.Search
         .Item("start_access") = Now
         .Item("last_access") = Now
         .Item("user_token") = user_token 'Grava o token do usuario.
         .Item("process_last_check") = True

         'Recupera a informacao do dicionario e salva novamente na session
         For Each item In dSessionSave
            .Item(item.Key) = item.Value
         Next
      End With
   End Sub

#End Region

#Region " Initialize_SomeDBInfo_To_Session - Inicializa TblNames, dCampos_Methods, lMinMax, lDataMinMax, lMensagens e lCDs. Em seguida grava na Session "
   ''' <summary>
   ''' Inicializa TblNames, dCampos_Methods, lMinMax, lDataMinMax, lMensagens e lCDs
   ''' </summary>
   Public Sub Initialize_SomeDBInfo_To_Session()
      Dim DBy As New DBSystemEntities
      Dim modulo = Get_Modulo()
      Dim TblNames = (From x In DBy.Tbls, y In DBy.Tbls_Modulos Where y.tbl = x.tbl AndAlso y.Modulo = modulo Select New Lista_TblNames With {.tbl = x.tbl, .tbl_referencia = x.tbl_referencia, .start_name = x.start_name, .rowspan = x.rowspan}).ToList
      Dim dCampos_Methods = (From x In DBy.Campos_Methods Where x.Modulo = modulo).ToDictionary(Function(x) x.Campo, Function(y) y.Method)

      'Carrega p/ Session Campos_MinMax e Campos_DataMinMax, para um ganho substancial de performance, ja que algumas tabelas
      'como por exemplo: NCMs_Aliquotas ou NCMs_IVAs, e' necessario efetuar a consistencia p/ varias UFs milhares de vezes, com isto a consistencia leva um tempo consideravel, ja que e' necessario acessar o DB milhares de vezes.
      'Usa Lista_MinMax e Lista_DataMinMax ao inves das entidades LinqToSql Campos_MinMax e Campos_DataMinMax, pois ao serializar usando BinaryFormatter no Save/Restore Session p/ o DB, e' gerada exception de que entidade nao esta marcada como serializavel.
      'Tambem p/ Mensagens e Campos_Descricoes ha' um ganho substancial de performance, pois a funcao Get_Message() e' executada milhares de vezes, por exemplo, ao processar Planilhas de Produtos e Cadastros.
      'P/ Mensagens e Campos_Descricoes, posteriormente deve ser otimizado p/ carregar somente as informacoes usadas em cada modulo.

      Dim lMinMax = (From mm In DBy.Campos_MinMax Where Not mm.excluido
                     Select New Lista_MinMax With {.Campo = mm.Campo, .Tipo = mm.Tipo, .Valor_Minimo = mm.Valor_Minimo, .Valor_Maximo = mm.Valor_Maximo, .Definir_Minimo = mm.Definir_Minimo, .Definir_Maximo = mm.Definir_Maximo, .Edicao = mm.Edicao, .Formato = mm.Formato}).ToList

      Dim lDataMinMax = (From dm In DBy.Campos_DataMinMax Where Not dm.excluido
                         Select New Lista_DataMinMax With {.Campo = dm.Campo, .Data_Minima = dm.Data_Minima, .Data_Maxima = dm.Data_Maxima, .Definir_Minimo = dm.Definir_Minimo, .Definir_Maximo = dm.Definir_Maximo, .Minimo_Today = dm.Minimo_Today, .Maximo_Today = dm.Maximo_Today, .Edicao = dm.Edicao, .Formato = dm.Formato}).ToList

      Dim lMensagens = (From x In DBy.Mensagens Where x.Linguagem = linguagem Select New Lista_Mensagens With {.Id = x.Id, .funcao = x.Funcao, .campo = x.Campo, .mensagem = x.Mensagem}).ToList

      Dim lCDs = (From c In DBy.Campos, cs In DBy.Campos_Seq Where cs.Modulo = modulo AndAlso cs.Campo = c.Campo
                  From cd In DBy.Campos_Descricoes.Where(Function(x) x.Campo = c.Campo AndAlso x.Linguagem = linguagem).DefaultIfEmpty
                  Order By cs.Sequencia
                  Select New Lista_Campos_Descricoes With {.Campo = c.Campo, .linha = c.linha, .Tag = c.Tag, .Descricao = If(cd IsNot Nothing, cd.Descricao, Nothing)}).ToList

      With HttpContext.Current.Session
         .Item("TblNames") = TblNames
         .Item("dCampos_Methods") = dCampos_Methods
         .Item("lMinMax") = lMinMax
         .Item("lDataMinMax") = lDataMinMax
         .Item("lMensagens") = lMensagens
         .Item("lCDs") = lCDs
         .Item("lVDs") = Get_Lista_VerbalDates()
      End With
   End Sub

#End Region

#Region " Get_SortOrder - Obtem a ordem do sort a partir da informacao armazenada na session do usuario. "
   ''' <summary>
   ''' Armazena os dados relativos ao sort na session para saber se o sort sera feito em ordem ascendente ou descente
   ''' tambem ha informacao para saber qual e' o ultimo campo que foi efetuado o sort.
   ''' </summary>
   Public Function Get_SortOrder(tbl As String, field As String, Optional inverter_ordem As Boolean = True) As Boolean
      Dim current_session = HttpContext.Current.Session
      Dim lSorts As New List(Of Lista_Sorts)
      Dim _sort As Lista_Sorts
      Dim ascendent = True

      If current_session.Item("lSorts") IsNot Nothing Then
         lSorts = current_session.Item("lSorts")

         lSorts.Where(Function(x) x.tbl = tbl).ToList.ForEach(Sub(x) x.last = False)

         _sort = (lSorts.Where(Function(x) x.tbl = tbl AndAlso x.field = field)).FirstOrDefault

         If _sort IsNot Nothing Then
            ascendent = _sort.ascendent
            _sort.ascendent = If(inverter_ordem, Not ascendent, ascendent) 'Grava a ordem inversa se inverter_ordem = True. Ex: se retornado ascendent = True, grava ascendent = False. Assim na proxima leitura sera efetuado o sort em ordem inversa.
            _sort.last = True
         Else
            lSorts.Add(New Lista_Sorts With {.tbl = tbl, .field = field, .ascendent = If(inverter_ordem, Not ascendent, ascendent), .last = True})
         End If
      Else
         lSorts.Add(New Lista_Sorts With {.tbl = tbl, .field = field, .ascendent = If(inverter_ordem, Not ascendent, ascendent), .last = True})
      End If

      current_session.Item("lSorts") = lSorts.ToList

      Return ascendent
   End Function

#End Region

#Region " Set_SortOrder - Grava a informacao do sort (ascendent ou descendent) na Lista_Sorts armazenada na session do usuario. "

   Public Sub Set_SortOrder(tbl As String, field As String, ascendent As Boolean, Optional clear_all As Boolean = False)
      Dim current_session = HttpContext.Current.Session
      Dim lSorts As New List(Of Lista_Sorts)
      Dim _sort As Lista_Sorts

      If Not clear_all Then
         If current_session.Item("lSorts") IsNot Nothing Then
            lSorts = current_session.Item("lSorts")

            lSorts.Where(Function(x) x.tbl = tbl).ToList.ForEach(Sub(x) x.last = False)

            _sort = (lSorts.Where(Function(x) x.tbl = tbl AndAlso x.field = field)).FirstOrDefault

            If _sort IsNot Nothing Then
               _sort.ascendent = ascendent
               _sort.last = True
            Else
               lSorts.Add(New Lista_Sorts With {.tbl = tbl, .field = field, .ascendent = ascendent, .last = True})
            End If
         Else
            lSorts.Add(New Lista_Sorts With {.tbl = tbl, .field = field, .ascendent = ascendent, .last = True})
         End If
      Else 'Clear LastSort
         If current_session.Item("lSorts") IsNot Nothing Then current_session.Item("lSorts") = Nothing
      End If

      current_session.Item("lSorts") = lSorts.ToList
   End Sub

#End Region

#Region " Get_LastFieldOrder - Obtem o ultimo campo do sort e a ordem (ascendent ou descent) a partir da informacao armazenada na session do usuario. "

   Public Function Get_LastFieldOrder(tbl As String, ByRef field As String, ByRef ascendent As Boolean) As Boolean
      Dim current_session = HttpContext.Current.Session
      Dim lSorts As New List(Of Lista_Sorts)
      Dim _sort As Lista_Sorts

      If current_session.Item("lSorts") IsNot Nothing Then lSorts = current_session.Item("lSorts")

      _sort = (lSorts.Where(Function(x) x.tbl = tbl AndAlso x.last)).FirstOrDefault

      If _sort IsNot Nothing Then
         field = _sort.field
         ascendent = _sort.ascendent
      Else
         field = Nothing
      End If

      Return (_sort IsNot Nothing)
   End Function

#End Region

#Region " Set_LastPage - Grava a informacao da ultima pagina (Paginacao) na session do usuario. "

   Public Sub Set_LastPage(tbl As String, pagina As Integer)
      Dim current_session = HttpContext.Current.Session
      Dim dPages As New Dictionary(Of String, Integer)

      If current_session.Item("LastPage") IsNot Nothing Then dPages = current_session.Item("LastPage")

      With dPages
         If Not .ContainsKey(tbl) Then .Add(tbl, pagina) Else .Item(tbl) = pagina
      End With

      current_session.Item("LastPage") = dPages
   End Sub

#End Region

#Region " Get_LastPage - Obtem a ultima pagina (Paginacao) armazenada na session do usuario. "

   Public Function Get_LastPage(tbl As String) As Integer
      Dim current_session = HttpContext.Current.Session
      Dim pagina As Integer
      Dim pag As Integer
      Dim dPages As New Dictionary(Of String, Integer)

      If current_session.Item("LastPage") IsNot Nothing Then dPages = current_session.Item("LastPage")

      If dPages.TryGetValue(tbl, pag) Then pagina = pag

      Return pagina
   End Function

#End Region

#Region " Get_RP - Obtem os dados de RecentsParameters, caso nao encontre, grava e retorna c/ a configuracao padrao. "
   Public Function Get_RP(modulo As String, Optional return_to_default As Boolean = False) As RecentsParameters
      Dim rp As New RecentsParameters

      With HttpContext.Current.Session

         If .Item("rp") Is Nothing OrElse return_to_default Then
            Dim tipo As String = Nothing
            Dim tipo_descricao As String = Nothing

            If modulo = "Produtos" Then
               tipo = "0"
               tipo_descricao = "Produtos"
            ElseIf modulo = "Cadastros" Then
               tipo = "C"
               tipo_descricao = "Clientes"
            End If

            rp = New RecentsParameters With {.tipo = tipo, .tipo_descricao = tipo_descricao}
            .Item("rp") = rp
         Else
            rp = .Item("rp")
         End If
      End With

      Return rp
   End Function

#End Region

#Region " ...Set_Flags, Get_Flags - Sub e Function relacionadas aos Flags... "

   Public Sub Set_Flags(descricao As String, flag As Boolean)
      Dim dFlags As New Dictionary(Of String, Boolean)

      With HttpContext.Current.Session
         If .Item("dFlags") IsNot Nothing Then
            dFlags = .Item("dFlags")

            If dFlags.ContainsKey(descricao) Then
               dFlags(descricao) = flag
            Else
               dFlags.Add(descricao, flag)
            End If
         Else
            dFlags.Add(descricao, flag)
         End If

         .Item("dFlags") = dFlags
      End With
   End Sub

   Public Function Get_Flags(descricao As String) As Boolean
      Dim flag As Boolean
      Dim dFlags As New Dictionary(Of String, Boolean)

      With HttpContext.Current.Session
         If .Item("dFlags") IsNot Nothing Then
            dFlags = .Item("dFlags")
            If dFlags.ContainsKey(descricao) Then
               flag = dFlags(descricao)
            Else
               dFlags.Add(descricao, False)
               .Item("dFlags") = dFlags
            End If
         Else
            dFlags.Add(descricao, False)
            .Item("dFlags") = dFlags
         End If
      End With

      Return flag
   End Function

#End Region

#Region " Get_Lista_Cad_Excluidos - Obtem Lista_Cad_Excluidos da session ou instancia novo Lista_Cad_Excluidos "

   Public Function Get_Lista_Cad_Excluidos() As Object
      Dim Lista_Cad_Excluidos As Object

      With HttpContext.Current.Session
         If .Item("Lista_Cad_Excluidos") IsNot Nothing Then
            Lista_Cad_Excluidos = .Item("Lista_Cad_Excluidos")
         Else
            Dim obj_type = Type.GetType(Assembly.GetExecutingAssembly.GetName().Name & ".Lista_" & Get_Modulo())
            Lista_Cad_Excluidos = obj_type.GetConstructor(Type.EmptyTypes).Invoke(New Object() {})
            .Item("Lista_Cad_Excluidos") = Lista_Cad_Excluidos
         End If
      End With

      Return Lista_Cad_Excluidos
   End Function

#End Region

#End Region

   '--- Lista_Return (Inicializacoes) ---

#Region " ...Lista_Return (Inicializacoes)... "

#Region " Set_Tema_User - Seta o tema e a foto do usuario "

   Public Sub Set_Tema_User(user_token As UserToken, ByRef Lista_Return As Lista_Request)
      Dim DB As New DBWorkersEntities
      Dim foto_user As String

      If user_token.status = 0 Then
         ' pega tema do usuario
         Get_Tema(0, Lista_Return)  ' Teste, posteriormente alterar p/ codigo do tema do cadastro de usuarios
         foto_user = "images/Sem imagem.png"
      Else
         ' pega tema default (0)
         Get_Tema(0, Lista_Return)
         foto_user = "images/Keys256.png"
      End If

      Set_Src(Lista_Return, "UserId_Img", foto_user)
   End Sub

#End Region

#Region " Get_Tema - Obtem os parametros do tema (color, background-color, background-image e etc). "

   Public Sub Get_Tema(codigo_tema As Integer, ByRef Lista_Return As Lista_Request)
      Dim DBy As New DBSystemEntities
      Dim tema_perfil As New tema

      With tema_perfil
         .round = (From tema In DBy.Temas Where tema.Codigo_Tema = codigo_tema AndAlso Not tema.excluido Select tema.roundbutton).FirstOrDefault

         Dim qColors = From cor In DBy.Temas_Colors Where cor.Codigo_Tema = codigo_tema AndAlso Not cor.excluido
                       Select New tcolor With {.descricao = cor.Descricao, .color = cor.color, .backcolor = cor.backcolor}
         .colors.AddRange(qColors.ToList)
      End With

      With Lista_Return
         .tema = tema_perfil

         Dim qcss = From css In DBy.Temas_Css Where css.Codigo_Tema = codigo_tema AndAlso Not css.excluido
                    Select css.selector & ";" & css.prop & ";" & css.value
         .css.AddRange(qcss.ToList)
      End With
   End Sub

#End Region

#Region " Set_Cmd_Init - Inicializa cmdi e cmpi c/ todos os parametros das tbls relacionadas ao modulo "

   Public Sub Set_Cmd_Init(ByRef Lista_Return As Lista_Request)
      Dim DBy As New DBSystemEntities
      Dim c, c2 As New Tbls
      Dim qce As New Tbls_ClickEvents
      Dim cmdi As New cmdi
      Dim qCMTIs As New List(Of cmti)
      Dim qCMBIs As New List(Of cmbi)
      Dim modulo = Get_Modulo()
      Dim opt_itens As IEnumerable(Of String)
      Dim template_options As String
      Dim ce_itens() As String
      Dim qCmds = (From tm In DBy.Tbls_Modulos, t In DBy.Tbls Where tm.Modulo = modulo AndAlso t.tbl = tm.tbl Select t).ToList
      Dim qCmdsClicks = (From tm In DBy.Tbls_Modulos, tc In DBy.Tbls_ClickEvents Where tm.Modulo = modulo AndAlso tc.tbl = tm.tbl Select tc).ToList
      Dim qCLLs = DBy.Campos_Length_Limites.ToList
      Dim lCDs As List(Of Lista_Campos_Descricoes) = HttpContext.Current.Session.Item("lCDs")
      Dim qlCDs As New List(Of Lista_Campos_Descricoes)
      Dim lCampos_Imgs_Lbls As List(Of String) = HttpContext.Current.Session.Item("lCampos_Imgs_Lbls")
      Dim lPropNames, lCs, lCILs, lColumns, lVMin, lVMax As New List(Of String)
      Dim qIPHs = (From cp In DBy.Campos, cs In DBy.Campos_Seq, cd In DBy.Campos_Descricoes
                   Where cp.linha AndAlso (cp.Tag = "input" OrElse cp.Tag = "textarea") AndAlso cs.Modulo = modulo AndAlso cs.Campo = cp.Campo AndAlso cd.Campo = cp.Campo AndAlso cd.Linguagem = linguagem
                   Order By cs.Sequencia
                   Select New Lista_IPH With {.id = cp.Campo, .iph = If(cd.Placeholder Is Nothing, cd.Descricao, cd.Placeholder)}).ToList

      For Each c In qCmds
         If c.tbl <> "Paginas" Then
            cmdi = New cmdi
            template_options = Nothing

            With cmdi
               .tbl = c.tbl
               .table_name = c.table_name
               .tbl_referencia = c.tbl_referencia
               .start_name = c.start_name
               .fitcontent = c.fitcontent
               .resize = c.resize
               .drilldown = c.drilldown
               .table_width = c.table_width
               .bpath = c.bpath

               'Obtem lColumns de forma semelhante, que esta em Set_Htbl_Cmd_NewTags_lFocus, para manter compativel a ordem das colunas
               lPropNames = Type.GetType(Assembly.GetExecutingAssembly.GetName().Name & ".Lista_" & c.tbl).GetProperties.Select(Function(x) x.Name).ToList
               qlCDs = (From lcd In lCDs Where lcd.Campo.StartsWith(c.start_name & "_") AndAlso lcd.linha).ToList
               lCs = qlCDs.Where(Function(x) lPropNames.Contains(x.Campo.Replace(c.start_name & "_", Nothing)) AndAlso (x.Tag = "input" OrElse x.Tag = "select" OrElse x.Tag = "textarea" OrElse x.Tag = "checkbox")).Select(Function(x) x.Campo).ToList
               lCILs = qlCDs.Where(Function(x) lPropNames.Contains(x.Campo.Replace(c.start_name & "_", Nothing)) AndAlso (x.Tag = "label" OrElse x.Tag = "img")).Select(Function(x) x.Campo).ToList
               lColumns = lCILs.Concat(lCs).ToList.Where(Function(x) lPropNames.Contains(x.Replace(c.start_name & "_", Nothing))).ToList

               If lColumns.Count > 0 Then
                  .columns = String.Empty
                  lColumns.ForEach(Sub(x) .columns += x & ";")
                  .columns = .columns.Remove(.columns.Length - 1)

                  lVMin.Clear() : lVMax.Clear()
                  lVMin = (From x In qCLLs Where lColumns.Contains(x.Campo) AndAlso x.Min_Length > 0 Select x.Campo & ";" & x.Min_Length).ToList
                  lVMax = (From x In qCLLs Where lColumns.Contains(x.Campo) AndAlso x.Max_Length > 0 Select x.Campo & ";" & x.Max_Length).ToList
                  If lVMin.Count > 0 Then .itens.MinLengths.AddRange(lVMin)
                  If lVMax.Count > 0 Then .itens.MaxLengths.AddRange(lVMax)
               End If

               'Header
               With .header
                  .show = c.show_header
                  .template = c.template_header

                  qce = qCmdsClicks.Find(Function(x) x.tbl = c.tbl AndAlso x.tipo = "h")
                  If qce IsNot Nothing Then .click_event = qce.click_event
               End With

               'Itens
               With .itens
                  .template = c.template_itens
                  .pauta_index = c.pauta_index
                  .image = c.image
                  .preview = c.preview
                  .red_button = c.red_button

                  qce = qCmdsClicks.Find(Function(x) x.tbl = c.tbl AndAlso x.tipo = "i")
                  If qce IsNot Nothing Then .click_event = qce.click_event
               End With

               'Options
               If c.template_options IsNot Nothing Then
                  template_options = c.template_options
               Else 'Como algumas tabelas nao possuem template_options, usa o template_options da tbl de referencia
                  c2 = qCmds.Find(Function(x) x.tbl_referencia = c.tbl_referencia AndAlso x.tbl = x.tbl_referencia)
                  If c2 IsNot Nothing AndAlso c2.template_options IsNot Nothing Then
                     template_options = c.template_options
                  End If
               End If

               If template_options IsNot Nothing AndAlso .itens.click_event IsNot Nothing Then
                  ce_itens = Split(.itens.click_event.Replace("Set_Click_Opcoes_Linhas(", Nothing).Replace(")", Nothing).Replace(",#", "||#"), ",")
                  opt_itens = ce_itens.Select(Function(x) x.Replace("""", Nothing).Replace("||#", ",#").Trim)

                  With .trOptions
                     .template = template_options
                     .tagsToHide = opt_itens(2)
                  End With
               End If

               'Placeholders - gera os placeholders p/ os inputs e textarea
               .iph.AddRange(qIPHs.Where(Function(x) x.id.StartsWith(c.start_name & "_")).ToList)
            End With

            Lista_Return.cmdi.Add(cmdi)
         End If
      Next

      'Paginas
      c2 = qCmds.Find(Function(x) x.tbl.Equals("Paginas"))

      If c2 IsNot Nothing Then
         Lista_Return.cmpi = New cmpi With {
                                            .template_header = c2.template_header,
                                            .template_itens = c2.template_itens,
                                            .descLine = Get_Message(Nothing, imsg.linha),
                                            .descLines = Get_Message(Nothing, imsg.linhas),
                                            .descPage = Get_Message(Nothing, imsg.Pagina)
                                           }
      End If

      'Paths de Imagens
      Dim di As New Dictionary(Of String, String)
      di.Add("cad", img_path.cadThumbPath)
      di.Add("prod", img_path.prodThumbPath)
      di.Add("prm", img_path.prmThumbPath)
      di.Add("img", img_path.imgPath)
      Lista_Return.cmip.AddRange(di.Select(Function(x) New cmip With {.bpath = x.Key, .ipath = x.Value}).ToList)

      'Templates
      qCMTIs = (From t In DBy.Templates Select New cmti With {.id = t.template_id, .template = t.template}).ToList
      Lista_Return.cmti.AddRange(qCMTIs)

      'Btns
      qCMBIs = (From b In DBy.Btns Select New cmbi With {.grupo = b.grupo, .template_id = b.template_id, .bpath = b.bpath, .click_event = b.click_event, .classe = b.classe}).ToList
      Lista_Return.cmbi.AddRange(qCMBIs)
   End Sub

#End Region

#Region " Set_Cmri - adiciona em Lista_Return.cmri os campos relacionados (usados em autocomplete) "
   ''' <summary>
   ''' Estes campos sao usados geralmente quando ha' autocomplete.
   ''' Nestes casos, ao inves de enviar todos os dados, envia-se somente alguns campos que estao relacionado ao caller_id
   ''' Por exemplo c/ Nome ou Nome Reduzido de Cadastros deve ser enviado tambem o tipo e o codigo.
   ''' </summary>
   Public Sub Set_Cmri(campos As String(), campos2 As String(), ByRef Lista_Return As Lista_Request)
      For Each campo2 In campos2
         For Each campo In campos
            Lista_Return.cmri.Add(New cmri With {.campo = campo, .campo2 = campo2})
         Next
      Next
   End Sub

#End Region

#Region " Set_HSels_Phd - Gera os placeholders p/ os selects "

   Public Sub Set_HSels_Phd(ByRef Lista_Return As Lista_Request)
      Dim DBy As New DBSystemEntities
      Dim TblNames As List(Of Lista_TblNames) = HttpContext.Current.Session.Item("TblNames")
      Dim start_name As String
      Dim modulo = Get_Modulo()
      Dim qPHDs = (From c In DBy.Campos, cs In DBy.Campos_Seq, cd In DBy.Campos_Descricoes
                   Where c.linha AndAlso c.Tag = "select" AndAlso cs.Modulo = modulo AndAlso cs.Campo = c.Campo AndAlso cd.Campo = c.Campo AndAlso cd.Linguagem = linguagem
                   Order By cs.Sequencia
                   Select New Lista_IPH With {.id = c.Campo, .iph = If(cd.Placeholder Is Nothing, cd.Descricao, cd.Placeholder)}).ToList

      For Each tblcmd In Lista_Return.cmdi
         start_name = TblNames.Find(Function(x) x.tbl = tblcmd.tbl).start_name
         For Each phd In qPHDs.Where(Function(x) x.id.StartsWith(start_name & "_"))
            tblcmd.sels.Add(New Lista_Selects With {.id = phd.id, .options = New List(Of Lista_Options) From {New Lista_Options With {.value = "phd", .text = phd.iph}}})
         Next
      Next
   End Sub

#End Region

#Region " Set_HSels_Ini - Inicializa as Selects em Lista_Return.cmdi "

   Public Sub Set_HSels_Ini(tbl As String, id As String, lOptions As List(Of Lista_Options), inserir_vazio As Boolean, ByRef Lista_Return As Lista_Request)
      Dim tblcmd = Lista_Return.cmdi.Find(Function(x) x.tbl = tbl)
      Dim sel = tblcmd.sels.Find(Function(x) x.id = id)

      If inserir_vazio Then lOptions.Insert(0, New Lista_Options With {.value = "vazio", .text = ""})

      If sel IsNot Nothing Then
         sel.options.AddRange(lOptions)
      Else
         tblcmd.sels.Add(New Lista_Selects With {.id = id, .options = lOptions})
      End If
   End Sub

#End Region

#Region " Set_HSels - Informa em Lista_Return.cmd.hsel a lista de tags que deve ser mantida nos options da select "

   Public Sub Set_HSels(tbl As String, lStartNames_fromDB_Check As String(), lRows As IEnumerable(Of Object), ByRef Lista_Return As Lista_Request, drilldown As Boolean, Optional adicionar As Boolean = True)
      Dim tblcmd = Lista_Return.cmd.Find(Function(x) x.tbl = tbl)

      For Each row In lRows
         For Each start_name In lStartNames_fromDB_Check
            If row.fromDB Then tblcmd.hsel.Add(start_name & "_" & row.linha)
         Next

         If drilldown Then
            Dim TblNames As List(Of Lista_TblNames) = HttpContext.Current.Session.Item("TblNames")
            Dim start_name As String = TblNames.Find(Function(x) x.tbl = tbl).start_name

            Set_Click_Drill_Down(tbl, row.linha, lRows, Lista_Return, adicionar)
         End If
      Next
   End Sub

#End Region

#Region " Set_HSel_Values - Adiciona em Lista_Return.hsels os valores que devem ser 'filtrados' da lista de options "

   Public Sub Set_HSel_Values(ByRef tblcmd As cmd, id As String, lValues As List(Of String))
      If lValues.Count > 0 Then
         Dim line = String.Empty

         lValues.ForEach(Sub(x) line += If(line = String.Empty, Nothing, ";") & x)

         tblcmd.hsel.Add(id & ";""" & line & """")
      End If
   End Sub

#End Region

#Region " Set_Htbl_Cmd_NewTags_lFocus - Adiciona as rows em htbl, e define cmd, newtags, lFocus e placeholders relacionados ao template do tbl "

   Public Sub Set_Htbl_Cmd_NewTags_lFocus(tbl As String, lRows As IEnumerable(Of Object), add As Boolean, ByRef Lista_Cad As Object, ByRef Lista_Return As Lista_Request, ByRef lFocus As List(Of Lista_Hide), last_focus_id As String, Optional clear As Boolean? = Nothing)
      Dim DBy As New DBSystemEntities
      Dim TblNames As List(Of Lista_TblNames) = HttpContext.Current.Session.Item("TblNames")
      Dim tb = TblNames.Find(Function(x) x.tbl = tbl)
      Dim start_name_ As String = tb.start_name & "_"
      Dim lCDs As List(Of Lista_Campos_Descricoes) = HttpContext.Current.Session.Item("lCDs")
      Dim lCampos_Imgs_Lbls As List(Of String) = HttpContext.Current.Session.Item("lCampos_Imgs_Lbls")
      Dim lTags As New List(Of Lista_Hide)
      Dim htbl As New List(Of String)
      Dim line, field As String
      Dim row As Object
      Dim lPropNames = Type.GetType(Assembly.GetExecutingAssembly.GetName().Name & ".Lista_" & tbl).GetProperties.Select(Function(x) x.Name).ToList
      Dim qLCDs = (From lcd In lCDs Where lcd.Campo.StartsWith(start_name_) AndAlso lcd.linha).ToList
      Dim lCs = qLCDs.Where(Function(x) lPropNames.Contains(x.Campo.Replace(start_name_, Nothing)) AndAlso (x.Tag = "input" OrElse x.Tag = "select" OrElse x.Tag = "textarea" OrElse x.Tag = "checkbox")).Select(Function(x) x.Campo).ToList
      Dim lCILs = qLCDs.Where(Function(x) lPropNames.Contains(x.Campo.Replace(start_name_, Nothing)) AndAlso (x.Tag = "label" OrElse x.Tag = "img")).Select(Function(x) x.Campo).ToList
      Dim lColumns = lCILs.Concat(lCs).ToList.Where(Function(x) lPropNames.Contains(x.Replace(start_name_, Nothing))).Select(Function(x) x.Replace(start_name_, Nothing)).ToList
      Dim Lista_Rows = Lista_Cad.GetType.GetProperty(tbl).GetValue(Lista_Cad)
      Dim insertAfter As String = Nothing

      'Obtem insertAfter se for uma tabela de itens em tabelas c/ drilldown menu      
      If tb.tbl <> tb.tbl_referencia AndAlso tb.tbl_referencia <> "TNCMs" AndAlso tb.tbl_referencia <> "TNOPs" Then
         insertAfter = Get_insertAfter(tbl, lRows, Lista_Cad)
      End If

      If add Then
         For Each row In lRows
            Lista_Rows.Add(row)
         Next
      End If

      For i = 0 To lRows.Count - 1
         row = lRows(i)

         'Gera linhas no formato CSV de lRows em Lista_Return.htbl
         line = row.GetType.GetProperty("linha").GetValue(row)
         For Each column In lColumns
            field = row.GetType.GetProperty(column).GetValue(row)
            line += ";" & convert_to_csv(field)
         Next

         htbl.Add(line)

         'Gera newtags e tambem lTags p/ usar em AddTags_lFocus
         For Each campo In lCs
            Lista_Return.newtags.Add(New Lista_NewTags With {.id = campo & "_" & row.linha})
            lTags.Add(New Lista_Hide With {.id = campo & "_" & row.linha, .hide = False})
         Next
      Next

      Set_Cmd(tbl, htbl, Lista_Return, clear, insertAfter)

      AddTags_lFocus(last_focus_id, lTags, lFocus)
   End Sub

#End Region

#Region " Set_lTPIs - Inicializa a session c/ as informacoes de Tbls_PageInfos relacionadas ao modulo, p/ ser usado no processamento da paginacao das listas "
   Public Sub Set_lTPIs()
      Dim DBy As New DBSystemEntities
      Dim modulo = Get_Modulo()
      Dim lTPIs = (From tm In DBy.Tbls_Modulos, tpi In DBy.Tbls_PageInfos
                   Where tm.Modulo = modulo AndAlso tpi.tbl = tm.tbl
                   Select New Lista_PageInfos With {.tbl = tpi.tbl, .Quantidade_Linhas_Pagina = tpi.Quantidade_Linhas_Pagina, .Campo_Default = tpi.Campo_Default, .Label_Id_NotFound = tpi.Label_Id_NotFound}).ToList

      HttpContext.Current.Session.Item("lTPIs") = lTPIs
   End Sub

#End Region

#Region " Set_Cmd = usado p/ adicionar cmd em Lista_Return, c/ a opcao de redefinicao do clear "

   Public Sub Set_Cmd(tbl As String, htbl As List(Of String), ByRef Lista_Return As Lista_Request, Optional clear As Boolean? = Nothing, Optional insertAfter As String = Nothing)
      Dim tblcmd = Lista_Return.cmd.Find(Function(x) x.tbl = tbl)
      Dim found = tblcmd IsNot Nothing

      If Not found Then tblcmd = New cmd

      With tblcmd
         .tbl = tbl
         If clear.HasValue Then .clear = clear
         .htbl.AddRange(htbl)
         If insertAfter IsNot Nothing Then .insertAfter.Add(insertAfter)
      End With

      If Not found Then Lista_Return.cmd.Add(tblcmd)
   End Sub

#End Region

#Region " Set_Cmd_Btn - usado p/ adicionar cmd_btn em Lista_Return, c/ os parametros do cmd_btn (templates, click_events, lista de buttons e etc.) "

   Public Sub Set_Cmd_Btn(grupo As String, lBtns As List(Of Lista_Buttons), ByRef Lista_Return As Lista_Request)
      Dim btncmd As New cmd_btn

      With btncmd
         .grupo = grupo
         .lBtns.AddRange(lBtns)
      End With

      With Lista_Return.cmd_btn
         .RemoveAll(Function(x) x.grupo = grupo)
         .Add(btncmd)
      End With
   End Sub

#End Region

#Region " Set_Cmd_Img - usado p/ adicionar cmd_img em Lista_Return, c/ os parametros do cmd_img (size_desktop, size_mobile, draggable, appendTo) "
   ''' <summary>
   ''' update_img e' usado p/ atualizar a imagem do botao de Opcoes, ou seja,
   ''' apos o usuario clicar em 1 determinada imagem do conjunto, o src da nova imagem sera atualizada no botao de opcoes.
   ''' </summary>

   Public Sub Set_Cmd_Img(tbl As String, appendTo As String, update_img As Boolean, lImgs As List(Of Lista_Img), ByRef Lista_Return As Lista_Request, Optional size_desktop As String = "100", Optional size_mobile As String = "80", Optional draggable As Boolean = True, Optional margin As String = "0px 0px 0px 6px")
      Dim imgcmd As New cmd_img

      With imgcmd
         .tbl = tbl
         .appendTo = appendTo
         .update_img = update_img
         .size_desktop = size_desktop
         .size_mobile = size_mobile
         .draggable = draggable
         .margin = margin
         .lImgs.AddRange(lImgs)
      End With

      With Lista_Return.cmd_img
         .RemoveAll(Function(x) x.tbl = tbl)
         .Add(imgcmd)
      End With
   End Sub

#End Region

#Region " Set_Cmd_Span - usado p/ adicionar cmd_span em Lista_Return, c/ os parametros start_name, appendTo e lista de spans) "

   Public Sub Set_Cmd_Span(start_name As String, bpath As String, appendTo As String, lSpans As List(Of String), ByRef Lista_Return As Lista_Request)
      Dim spancmd As New cmd_span

      With spancmd
         .start_name = start_name
         .bpath = bpath
         .template_id = "span_template"
         .span_color = "pauta" & GetRandom(1, 7)
         .appendTo = appendTo
         .lSpans.AddRange(lSpans)
      End With

      With Lista_Return.cmd_span
         .RemoveAll(Function(x) x.appendTo = appendTo)
         .Add(spancmd)
      End With
   End Sub

#End Region

#Region " Set_trOptions - usado p/ setar Lista_Return.Set_trOptions c/ informacoes p/ adicao ou remocao da linha relacionadas aos Options. "

   Public Sub Set_trOptions(add As Boolean, tbl As String, linha As String, ByRef Lista_Return As Lista_Request)
      Lista_Return.trOptions.Add(add.ToString.ToLower & ";" & tbl & ";" & linha)
      Set_Flags(tbl, add)
   End Sub

#End Region

#Region " Set_YesNo - usado p/ adicionar YesNoCancel em Lista_Return, com os parametros necessarios. "

   Public Sub Set_YesNo(ByRef Lista_Return As Lista_Request, msg_id As String, focus As MsgBoxResult, show_no As Boolean, show_cancel As Boolean, Optional duration As Integer? = Nothing, Optional panel_id As String = "MessagesPanel")
      Dim DBy As New DBSystemEntities
      Dim lMensagens As List(Of Lista_Mensagens) = HttpContext.Current.Session.Item("lMensagens")
      Dim qMsg = (From m In lMensagens Where (m.Id = msg_id OrElse m.Id = imsg.Yes OrElse m.Id = imsg.No OrElse m.Id = imsg.Cancel) Select m.Id, m.mensagem).ToList

      If qMsg.Count = 4 Then
         Dim focus_string As String

         If focus = MsgBoxResult.Yes Then
            focus_string = msg_id & "_YesButton"
         ElseIf focus = MsgBoxResult.No Then
            focus_string = msg_id & "_NoButton"
         Else
            focus_string = msg_id & "_CancelButton"
         End If

         With Lista_Return
            .yesno.Add(New Lista_YesNo With {.id = panel_id,
                                             .msg = qMsg.Find(Function(m) m.Id = msg_id).mensagem,
                                             .btn_yes = msg_id & "_YesButton",
                                             .btn_no = msg_id & "_NoButton",
                                             .btn_cancel = msg_id & "_CancelButton",
                                             .text_yes = qMsg.Find(Function(m) m.Id = imsg.Yes).mensagem,
                                             .text_no = qMsg.Find(Function(m) m.Id = imsg.No).mensagem,
                                             .text_cancel = qMsg.Find(Function(m) m.Id = imsg.Cancel).mensagem,
                                             .focus = focus_string,
                                             .show_no = show_no,
                                             .show_cancel = show_cancel,
                                             .duration = If(duration Is Nothing, 15000, duration)
                                            })
            .next_focus = panel_id
         End With
      End If
   End Sub

#End Region

#Region " Get_Field_Info - Obtem as informacoes relacionadas ao campo "

   Public Sub Get_Field_Info(caller_event As String, ByRef Lista_Return As Lista_Request)
      Dim DBy As New DBSystemEntities
      With Lista_Return.msg
         Dim msg_info, tmp_value As String
         If Not String.IsNullOrWhiteSpace(caller_event) Then
            With caller_event
               tmp_value = If(.Contains("_") AndAlso isDigit(.Last), .Substring(0, .LastIndexOf("_")), caller_event)
            End With

            msg_info = (From ce In DBy.Campos_Descricoes Where ce.Campo = tmp_value Select If(ce.Informacao = Nothing, ce.Descricao, ce.Informacao)).FirstOrDefault
         Else
            msg_info = Get_Message(Nothing, imsg.GetFieldInfoEmpty)
         End If
         Set_Msg(Lista_Return, msg_info, If(Not String.IsNullOrWhiteSpace(caller_event), caller_event, Nothing), String.IsNullOrWhiteSpace(caller_event), False, True)
      End With
   End Sub

#End Region

#End Region

   '--- Lista_Return (Genericas) ---

#Region " ... Lista_Return (Genericas) ... "

#Region " Set_Combo - Atualiza Lista_Return.combo c/ o id, value e text "

   Public Sub Set_Combo(ByRef Lista_Return As Lista_Request, id As String, value As String, text As String)
      Lista_Return.combo.Add(id & ";" & value & ";" & text)
   End Sub

#End Region

#Region " Set_Value - Atualiza Lista_Return.value c/ o id e value "

   Public Sub Set_Value(ByRef Lista_Return As Lista_Request, id As String, value As String)
      Lista_Return.value.Add(id & ";" & value)
   End Sub

#End Region

#Region " Set_Text - Atualiza Lista_Return.text c/ o id e text "

   Public Sub Set_Text(ByRef Lista_Return As Lista_Request, id As String, text As String)
      Lista_Return.text.Add(id & ";" & text)
   End Sub

#End Region

#Region " Set_Hide - Atualiza Lista_Return.hide c/ o id e hide "

   Public Sub Set_Hide(ByRef Lista_Return As Lista_Request, id As String, hide As Boolean)
      Lista_Return.hide.Add(id & ";" & hide)
   End Sub

#End Region

#Region " Set_Src - Atualiza Lista_Return.prop c/ o id, src e path da imagem "

   Public Sub Set_Src(ByRef Lista_Return As Lista_Request, id As String, src As String)
      Lista_Return.prop.Add(id & ";src;" & src)
   End Sub

#End Region

#Region " Set_IPH - Atualiza Lista_Return.prop c/ o id e o placeholder dos inputs "

   Public Sub Set_IPH(ByRef Lista_Return As Lista_Request, id As String, placeholder As String)
      Lista_Return.prop.Add(id & ";placeholder;" & placeholder)
   End Sub

#End Region

#Region " Set_ReadOnly - Atualiza Lista_Return.prop c/ o id e o atributo readonly (true or false) - deve ser serializado sempre c/ letras minusculas "

   Public Sub Set_ReadOnly(ByRef Lista_Return As Lista_Request, id As String, read_only As Boolean)
      Lista_Return.prop.Add(id & ";readonly;" & read_only.ToString.ToLower)
   End Sub

#End Region

#Region " Set_Css - Atualiza Lista_Return.remove c/ o selector, prop e value "

   Public Sub Set_Css(ByRef Lista_Return As Lista_Request, selector As String, prop As String, value As String)
      Lista_Return.css.Add(selector & ";" & prop & ";" & value)
   End Sub

#End Region

#Region " Set_Msg - Atualiza a mensagem e parametros em Lista_Return.msg "

   Public Sub Set_Msg(ByRef Lista_Return As Lista_Request, msg As String, Optional element As String = Nothing, Optional isMessagePanel As Boolean = True, Optional tag_alert As Boolean? = Nothing, Optional green_element As Boolean? = Nothing, Optional blue_element As Boolean? = Nothing)
      Lista_Return.msg.Add(New Lista_Msg With {
                                               .id = If(element Is Nothing, "MessagesPanel", element),
                                               .msg = msg,
                                               .panelMsg = isMessagePanel,
                                               .tag_alert = If(tag_alert.HasValue, tag_alert, Not isMessagePanel),
                                               .green = If(green_element.HasValue, green_element, False),
                                               .blue = If(blue_element.HasValue, blue_element, False),
                                               .close = False
                                              })
   End Sub

#End Region

#Region " Set_ImageUrl - atualiza a url da imagen do button ou img em Lista_Return, usando prop ou css conforme o caso "

   Public Sub Set_ImageUrl(ByRef Lista_Return As Lista_Request, id As String, Optional back_name As Boolean = False)
      Dim DBy As New DBSystemEntities

      Dim qImg = (From c In DBy.Campos, i In DBy.Campos_ImagensUrl
                  Where c.Campo = id AndAlso i.Campo = c.Campo
                  Select c.Campo, c.Tag, i.Descricao_Default, i.Descricao_Back).FirstOrDefault

      If qImg IsNot Nothing Then
         With qImg
            If .Tag.Equals("img") Then
               Set_Src(Lista_Return, qImg.Campo, If(Not back_name, qImg.Descricao_Default, If(Not String.IsNullOrWhiteSpace(qImg.Descricao_Back), qImg.Descricao_Back, qImg.Descricao_Default)))
            ElseIf .Tag.Equals("button") Then
               Set_Css(Lista_Return, "#" & qImg.Campo, "background-image", "url('../" & If(Not back_name, qImg.Descricao_Default, If(Not String.IsNullOrWhiteSpace(qImg.Descricao_Back), qImg.Descricao_Back, qImg.Descricao_Default)) & "')")
            End If
         End With
      End If
   End Sub

#End Region

#Region " Remove_Options - Atualiza Lista_Return.remove c/ o selector que remove todos exceto um "

   Public Sub Remove_Options(ByRef Lista_Return As Lista_Request, tag As String, except_value As String)
      Lista_Return.remove.Add("#" & tag & " option[value!='" & except_value & "']")
   End Sub

#End Region

#End Region

   '--- Linhas ---

#Region " ... Linhas... "

#Region " Get_Next_Line - Obtem a sequencia da proxima linha "

   Public Function Get_Next_Line(Lista_Obj As IEnumerable(Of Object)) As String
      Return If(Lista_Obj.Count = 0, "0001", Lista_Obj.OrderBy(Function(x) x.linha).Select(Function(x) (CInt(x.linha) + 1).ToString("0000")).Last())
   End Function

#End Region

#Region " Get_Next_sequencia - Obtem a sequencia + 1 da ultima row da Lista (usada nas subs Get_Lista_...) "

   Public Function Get_Next_sequencia(Lista_Obj As IEnumerable(Of Object)) As Integer
      Dim row = Lista_Obj.OrderBy(Function(x) x.linha).LastOrDefault
      Dim sequencia = If(row Is Nothing, 1, CType(row.linha, Integer) + 1)

      Return sequencia
   End Function

#End Region

#Region " Get_PropNameFromTag - Obtem o nome correspondente a property da Lista_... do tag passado como parametro "

   Public Function Get_PropNameFromTag(tag As String) As String
      Dim propName As String

      If IsNumeric(tag.Last) Then
         propName = tag.Remove(0, tag.IndexOf("_") + 1)
         propName = propName.Remove(propName.LastIndexOf("_"))
      Else
         propName = tag
      End If

      Return propName
   End Function

#End Region

#Region " Retorna a linha ou codigo que no final do valor e' passado como parametro "

   Public Function Get_Linha(value As String) As String
      Return value.Substring(value.LastIndexOf("_") + 1)
   End Function

#End Region

#Region " Remover_linha - Remove a linha "
   Public Sub Remover_linha(tbl As String, row As Object, ByRef Lista_Obj As Object, ByRef Lista_Return As Lista_Request, ByRef lFocus As List(Of Lista_Hide), Optional row_master As Object = Nothing, Optional isTNCMVI As Boolean = False)
      Dim TblNames As List(Of Lista_TblNames) = HttpContext.Current.Session.Item("TblNames")
      Dim tb = TblNames.Find(Function(x) x.tbl = tbl)

      If row.fromDB Then
         Dim Lista_Cad_Excluidos = Get_Lista_Cad_Excluidos()
         Dim Lista_Excluidos_Obj = Lista_Cad_Excluidos.GetType.GetProperty(tbl).GetValue(Lista_Cad_Excluidos)

         If row_master IsNot Nothing Then
            ' Como itens esta relacionado a master, caso a row seja item e'
            ' adicionado master c/ nova numeracao de linha e alterado tambem a property Master_linha do item
            ' Esta adicao e' necessaria, pois as informacoes da Lista original, podem ser reclassificadas e as linhas alteradas.
            ' Esta sendo clonado a row, pois as alteracoes em row_master refletem na row da Lista original relacionada ao row_master.
            ' Obs: neste caso, como .fromDB do Item e' igual a True, obviamente .fromDB do Master tambem deve ser igual a True.

            Dim tb_master = TblNames.Find(Function(x) x.tbl_referencia = tb.tbl_referencia)
            Dim Lista_Master_Excluidos_Obj = Lista_Cad_Excluidos.GetType.GetProperty(tb_master.tbl).GetValue(Lista_Cad_Excluidos)
            Dim next_line = Get_Next_Line(Lista_Master_Excluidos_Obj)
            Dim row_clone = Clone_Row(row_master)

            row_clone.linha = next_line
            row_clone.excluido = False
            row.GetType.GetProperty(tb_master.start_name & "_linha").SetValue(row, next_line)

            Lista_Master_Excluidos_Obj.Add(row_clone)
         ElseIf tb.tbl <> tb.tbl_referencia Then
            Set_Msg(Lista_Return, "Falha detectada ao remover linha.<br>Não foi informada a row da tabela Master, correspondente ao item")
         End If

         row.excluido = True

         Lista_Excluidos_Obj.Add(row)
      End If

      Lista_Obj.Remove(row)

      lFocus.RemoveAll(Function(x) x.id.StartsWith(tb.start_name & "_") AndAlso x.id.EndsWith(row.linha))

      Lista_Return.remove_linha.Add(tbl & ";" & row.linha)

      If Not Get_Flags("SaveButton") Then Set_ImageUrl(Lista_Return, "SaveButton") : Set_Flags("SaveButton", True)
      Set_Flags("tabindex_check", True)
   End Sub

#End Region

#Region " Get_Row_Header - Obtem a row c/ a informacao de header p/ Excel e tambem lPropNames c/ os properties utilizados na Lista_... "

   Public Sub Get_Row_Header(tbl As String, ByRef row As Object, ByRef lPropNames As List(Of String), Optional has_label As Boolean = False)
      Dim DBy As New DBSystemEntities
      Dim TblNames As List(Of Lista_TblNames) = HttpContext.Current.Session.Item("TblNames")
      Dim tbname As Lista_TblNames = TblNames.Find(Function(x) x.tbl = tbl)
      Dim dTags, dCampos As New Dictionary(Of String, String)
      Dim lProperties_Names As New List(Of String)
      Dim modulo = Get_Modulo()

      row = row.GetType.GetConstructor(Type.EmptyTypes).Invoke(New Object() {})

      If tbname IsNot Nothing Then
         dTags = (From c In DBy.Campos, cs In DBy.Campos_Seq, cd In DBy.Campos_Descricoes
                  Where c.Campo.StartsWith(tbname.start_name & "_") AndAlso c.linha AndAlso If(has_label, True, c.Tag <> "label") AndAlso c.Tag <> "button" AndAlso c.Tag <> "img" AndAlso cs.Modulo = modulo AndAlso cs.Campo = c.Campo AndAlso cd.Campo = c.Campo AndAlso cd.Linguagem = linguagem
                  Order By cs.Sequencia
                  Select c.Campo, descricao = If(cd.Placeholder Is Nothing, cd.Descricao, cd.Placeholder)).ToDictionary(Function(x) x.Campo, Function(y) y.descricao)

      ElseIf tbl = modulo Then
         dTags = (From c In DBy.Campos, cs In DBy.Campos_Seq, cd In DBy.Campos_Descricoes
                  Where Not c.linha AndAlso If(has_label, True, c.Tag <> "label") AndAlso c.Tag <> "button" AndAlso c.Tag <> "img" AndAlso cs.Modulo = modulo AndAlso cs.Campo = c.Campo AndAlso cd.Campo = c.Campo AndAlso cd.Linguagem = linguagem
                  Order By cs.Sequencia
                  Select c.Campo, descricao = If(cd.Placeholder Is Nothing, cd.Descricao, cd.Placeholder)).ToDictionary(Function(x) x.Campo, Function(y) y.descricao)
      End If

      lProperties_Names = row.GetType.GetProperties.Select(Function(x) x.Name).ToList

      If tbname IsNot Nothing Then
         dCampos = (From x In dTags.Where(Function(y) lProperties_Names.Contains(y.Key.Replace(tbname.start_name & "_", Nothing))) Select Key = x.Key.Remove(0, x.Key.IndexOf("_") + 1), x.Value).ToDictionary(Function(x) x.Key, Function(y) y.Value)
      Else
         dCampos = (From x In dTags.Where(Function(y) lProperties_Names.Contains(y.Key))).ToDictionary(Function(x) x.Key, Function(y) y.Value)
      End If

      If tbl IsNot Nothing Then
         For Each campo In dCampos
            row.GetType.GetProperty(campo.Key).SetValue(row, campo.Value)
         Next
      Else
         For Each campo In lProperties_Names
            row.GetType.GetProperty(campo).SetValue(row, campo)
         Next
      End If

      'Se a row for da lista usada p/ serializar as informacoes em Lista_Return,
      'seleciona somente os campos lidos do DB
      If tbl IsNot Nothing AndAlso row.GetType.Name.EndsWith(tbl) Then
         lPropNames = dCampos.Select(Function(x) x.Key).ToList
      Else
         lPropNames = lProperties_Names
      End If
   End Sub

#End Region

#Region " dMsg_Add, lMsgErr2_Add - Adiciona a mensagem em dMsg|Adiciona a mensagem em lMsgErr2 "

   Public Sub dMsg_Add(campo As String, linha As String, msg As String, ByRef dMsg As Dictionary(Of String, String))
      Dim key = campo & If(linha IsNot Nothing, "_" & linha, Nothing)
      If Not dMsg.ContainsKey(key) Then dMsg.Add(key, msg)
   End Sub

   Public Sub lMsgErr2_Add(xlImport As Boolean, linha As String, campo As String, msg As String, ByRef lMsgErr2 As List(Of Lista_MsgErr))
      Dim id As String

      If xlImport Then
         id = campo & If(Not IsNumeric(campo.Last), "_" & (lMsgErr2.Count + 1).ToString("0000"), Nothing)
      Else
         'Como a funcao de exibicao de mensagens (java) remove as mensagens previas p/ o id que esta sendo exibido,
         'e' necessario agrupar as Mensagens de MessagesPanel como uma unica mensagem, portanto verifica se lMsgErr2
         'tem alguma mensagem previa de MessagesPanel, se tiver altera a mensagem e remove a mensagem previa.
         Dim messpanel_prev = lMsgErr2.FindAll(Function(x) x.id = "MessagesPanel").FirstOrDefault

         If messpanel_prev IsNot Nothing Then
            msg = messpanel_prev.msg & "<br>" & msg
            lMsgErr2.Remove(messpanel_prev)
         End If
         id = "MessagesPanel"
      End If

      lMsgErr2.Add(New Lista_MsgErr With {.linha = linha, .id = id, .msg = msg})
   End Sub

#End Region

#Region " lMsgErr_Check - Sub de suporte ao Pre-Update, transfere as informacoes de dMsg p/ dMsgErr e Lista_Return.msg "

   Public Sub lMsgErr_Check(lMsgErr As List(Of Lista_MsgErr), lMsgErr2 As List(Of Lista_MsgErr), ByRef Lista_Return As Lista_Request)
      Dim total_msg = lMsgErr.Count + lMsgErr2.Count
      Dim lMsgAll = lMsgErr.Concat(lMsgErr2).ToList.Where(Function(x, index) index < 30).ToList
      Dim qMsg = From m In lMsgAll Select New Lista_Msg With {.msg = m.msg, .id = m.id, .panelMsg = True, .tag_alert = (m.id <> "MessagesPanel"), .close = False}

      Lista_Return.msg.AddRange(qMsg.ToList)

      'Limita em 30 linhas, as mensagens a serem exibidas em MessagesPanel, Neste caso adiciona mensagen informando o total de mensagens exibidas.
      If total_msg > 30 Then Set_Msg(Lista_Return, "Total de 30/" & total_msg & " mensagens com inconsistencias.", Nothing, True, Nothing, Nothing, True)
   End Sub

#End Region

#Region " dMsg_To_lMsgErr2 - Se todos os itens de lTags estiverem em dMsg, transfere p/ lMsgErr2 "
   ''' <summary>
   ''' E' utilizado p/ prever casos de inconsistencias encontradas na importacao, por exemplo:
   ''' A inconsistencia de todos os campos relacionados em lTags, indica que a linha nao foi adicionada na planilha atual, mas esta na primeira planilha.
   ''' Exemplo: Em Cadastros e' adicionado um Novo Usuario e na planilha de Usuarios nao e' adicionada a informacao relacionada. Ao efetuar
   ''' a consistencia sao geradas linhas de erros de preechimento obrigatorio nos campos que deveriam estar relacionadas na planilha Usuarios.
   ''' Como nao e' adicionada nova row na planilha de Usuarios, nao e' possivel Listar a inconsistencia relacionando com a row da planilha, neste caso
   ''' e' adicionada em lMsgErr2, onde sera adicionada uma linha c/ os dados comuns as demais planilhas e a mensagem de inconsistencia.
   ''' </summary>
   Public Sub dMsg_To_lMsgErr2(linha As String, lTags As List(Of String), dMsg As Dictionary(Of String, String), ByRef lMsgErr2 As List(Of Lista_MsgErr))
      Dim lMsgKeys = dMsg.Select(Function(x) x.Key).ToList

      If lTags.Except(lMsgKeys).Count = 0 Then
         Dim lMsg = dMsg.ToList.Where(Function(x) lTags.Contains(x.Key)).Select(Function(x) x.Key).ToList

         For Each x In lMsg
            lMsgErr2.Add(New Lista_MsgErr With {.linha = linha, .id = x, .msg = dMsg(x)})
         Next
         lMsg.ForEach(Sub(x) dMsg.Remove(x))
      End If
   End Sub

#End Region

#End Region

   '--- Validacoes ---

#Region " ...Validacoes... "

#Region " isSelectNull - Verifica se a informacao associada ao elemento Select esta c/ opcao valida ou nao, verificando (placeholder, null, empty e nothing)."

   Public Function isSelectNull(value) As Boolean
      Return (String.IsNullOrWhiteSpace(value) OrElse value.Equals("phd") OrElse value.Equals("null") OrElse value.Equals(vbNullChar))
   End Function

#End Region

#Region " Consiste_NullErr - Consiste o campo c/ isSelectNull e qValues.Contains(value) e adiciona em lMsg a mensagem de inconsistencia ou preenchimento obrigatorio caso nao esteja ok. "

   Public Function Consiste_NullErr(campo As String, linha As String, value As String, dMsg As Dictionary(Of String, String), qValues As List(Of String), Optional end_message As String = Nothing) As Boolean
      Dim ok = True

      If isSelectNull(value) Then
         dMsg.Add(campo & If(linha IsNot Nothing, "_" & linha, Nothing), Get_Message(campo, imsg.Empty) & If(end_message Is Nothing, Nothing, " " & end_message.Trim))
         ok = False
      ElseIf Not qValues.Contains(value) Then
         dMsg.Add(campo & If(linha IsNot Nothing, "_" & linha, Nothing), Get_Message(campo, imsg.Inconsistente) & If(end_message Is Nothing, Nothing, " " & end_message.Trim))
         ok = False
      End If

      Return ok
   End Function

#End Region

#Region " Consiste_Cbl_NullErr - Consiste o campo c/ isSelectNull e qCbls (Tipo=tipo_cbl e Codigo=value) e adiciona em dMsg a mensagem de inconsistencia ou preenchimento obrigatorio caso nao esteja ok. "

   Public Function Consiste_Cbl_NullErr(campo As String, linha As String, value As String, dMsg As Dictionary(Of String, String), qCbls As IEnumerable(Of Object), tipo_cbl As String, Optional end_message As String = Nothing) As Boolean
      Dim ok = True

      If isSelectNull(value) Then
         dMsg.Add(campo & If(linha IsNot Nothing, "_" & linha, Nothing), Get_Message(campo, imsg.Empty) & If(end_message Is Nothing, Nothing, " " & end_message.Trim))
         ok = False
      ElseIf qCbls.Where(Function(x) x.Tipo = tipo_cbl AndAlso x.Codigo = value).Count = 0 Then
         dMsg.Add(campo & If(linha IsNot Nothing, "_" & linha, Nothing), Get_Message(campo, imsg.Inconsistente) & If(end_message Is Nothing, Nothing, " " & end_message.Trim))
         ok = False
      End If

      Return ok
   End Function

#End Region

#Region " Get_Message - Obtem a mensagem do BD. "
   ''' <summary>
   ''' Obtem a mensagem, pesquisando na Session (lMensagens e lCDs), caso nao encontre na session, pesquisa no DB.
   ''' Este detalhe pode ocorrer, pois em lCDs sao carregadas somente as informacoes relacionadas ao modulo ou acc_panel.
   ''' </summary>
   Public Function Get_Message(campo As String, Id As String, Optional param_value As String = Nothing) As String
      Dim lMensagens As List(Of Lista_Mensagens) = HttpContext.Current.Session.Item("lMensagens")
      Dim lCDs As List(Of Lista_Campos_Descricoes) = HttpContext.Current.Session.Item("lCDs")
      Dim lcd As New Lista_Campos_Descricoes
      Dim msg As String = Nothing

      If campo IsNot Nothing Then
         If lMensagens IsNot Nothing Then 'Verifica se lMensagens IsNot Nothing, pois esta sub pode ser chamada apos expiracao da session, antes de carregar as tabelas. Por exemplo ao processar Token_Check
            msg = lMensagens.FindAll(Function(x) x.Id = Id AndAlso x.funcao = (param_value IsNot Nothing) AndAlso x.campo).Select(Function(x) x.mensagem).FirstOrDefault
            lcd = lCDs.Find(Function(x) x.Campo = campo)
         End If

         If msg IsNot Nothing AndAlso lcd IsNot Nothing Then
            msg = lcd.Descricao & " " & msg
         Else
            Dim DBy As New DBSystemEntities
            msg = (From m In DBy.Mensagens, c In DBy.Campos_Descricoes
                   Where m.Linguagem = linguagem AndAlso m.Id = Id AndAlso m.Funcao = (param_value IsNot Nothing) AndAlso m.Campo AndAlso c.Campo = campo
                   Select c.Descricao & " " & m.Mensagem).FirstOrDefault
         End If
      Else 'Obter somente a mensagem padrao
         If lMensagens IsNot Nothing Then 'Verifica se lMensagens IsNot Nothing, pois esta sub pode ser chamada apos expiracao da session, antes de carregar as tabelas. Por exemplo ao processar Token_Check
            msg = lMensagens.FindAll(Function(x) x.Id = Id AndAlso x.funcao = (param_value IsNot Nothing) AndAlso Not x.campo).Select(Function(x) x.mensagem).FirstOrDefault
         End If

         If msg Is Nothing Then
            Dim DBy As New DBSystemEntities
            msg = (From m In DBy.Mensagens Where m.Linguagem = linguagem AndAlso m.Id = Id AndAlso m.Funcao = (param_value IsNot Nothing) AndAlso Not m.Campo Select m.Mensagem).FirstOrDefault
         End If
      End If

      If param_value IsNot Nothing AndAlso msg IsNot Nothing Then
         Dim pVals = param_value.Split("|".ToCharArray, StringSplitOptions.RemoveEmptyEntries)

         For x As Integer = 0 To pVals.Count - 1
            msg = msg.Replace(x.ToString & "()", pVals(x))
         Next
      End If

      ' Teste - Por seguranca e com o objetivo de monitorar as mensagens,
      '         caso alguma mensagem fique em branco, porem haja inconsistencias
      '         provavelmente falta de informacao no BD ou parametro passado incorretamente. (remover posteriormente)
      If msg Is Nothing Then
         msg = "Não foi possivel obter mensagem do banco de dados para o Id: " & Id & If(campo IsNot Nothing, " - campo: " & campo, Nothing)
      End If

      Return msg
   End Function

#End Region

#Region " MinMax - Consiste o valor passado como parametro e caso haja inconsistencia sera devolvida a mensagem em msg "

   Public Function MinMax(campo As String, valor_string As String, notEmpty As Boolean, ByRef msg As String, Optional ByRef valor_formatado As String = Nothing) As Boolean
      Dim isOk = True
      Dim isIntervalOk = True
      Dim min_ok = True
      Dim max_ok = True

      msg = Nothing
      valor_formatado = Nothing

      If Not String.IsNullOrWhiteSpace(valor_string) Then
         Dim lMinMax As List(Of Lista_MinMax) = HttpContext.Current.Session.Item("lMinMax")
         Dim qMinMax = (From mm In lMinMax Where mm.Campo = campo).FirstOrDefault

         If qMinMax IsNot Nothing Then
            valor_string = valor_string.Replace("%", Nothing)

            With cc.NumberFormat
               If valor_string.Contains(.CurrencyGroupSeparator) Then
                  Dim sNumbers = Split(valor_string, .CurrencyGroupSeparator)
                  If sNumbers.Count = 2 AndAlso sNumbers(1).Count < .CurrencyGroupSizes(0) Then valor_string = valor_string.Replace(.CurrencyGroupSeparator, .CurrencyDecimalSeparator)
               End If
            End With

            With qMinMax
               If .Tipo = "D" Then 'Decimal
                  Dim valor As Decimal
                  If Decimal.TryParse(valor_string, valor) Then
                     If .Definir_Minimo Then min_ok = (valor >= .Valor_Minimo)
                     If .Definir_Maximo Then max_ok = (valor <= .Valor_Maximo)
                     valor_formatado = valor.ToString(.Formato)
                  Else
                     isOk = False
                  End If
               ElseIf .Tipo = "S" Then 'Short
                  Dim valor As Short
                  If Short.TryParse(valor_string, valor) Then
                     If .Definir_Minimo Then min_ok = (valor >= .Valor_Minimo)
                     If .Definir_Maximo Then max_ok = (valor <= .Valor_Maximo)
                     valor_formatado = valor.ToString(.Formato)
                  Else
                     isOk = False
                  End If
               ElseIf .Tipo = "I" Then 'Integer
                  Dim valor As Integer
                  If Integer.TryParse(valor_string, valor) Then
                     If .Definir_Minimo Then min_ok = (valor >= .Valor_Minimo)
                     If .Definir_Maximo Then max_ok = (valor <= .Valor_Maximo)
                     valor_formatado = valor.ToString(.Formato)
                  Else
                     isOk = False
                  End If
               ElseIf .Tipo = "L" Then 'Long
                  Dim valor As Long
                  If Long.TryParse(valor_string, valor) Then
                     If .Definir_Minimo Then min_ok = (valor >= .Valor_Minimo)
                     If .Definir_Maximo Then max_ok = (valor <= .Valor_Maximo)
                     valor_formatado = valor.ToString(.Formato)
                  Else
                     isOk = False
                  End If
               ElseIf .Tipo = "B" Then 'Byte
                  Dim valor As Byte
                  If Byte.TryParse(valor_string, valor) Then
                     If .Definir_Minimo Then min_ok = (valor >= .Valor_Minimo)
                     If .Definir_Maximo Then max_ok = (valor <= .Valor_Maximo)
                     valor_formatado = valor.ToString(.Formato)
                  Else
                     isOk = False
                  End If
               End If
            End With

            isIntervalOk = (min_ok AndAlso max_ok)

            If Not isOk Then
               msg = Get_Message(Nothing, imsg.MinMaxError)
            ElseIf Not isIntervalOk Then
               msg = Get_Message(campo, imsg.MinMax)
            End If
         Else
            msg = Get_Message(Nothing, imsg.MinMaxNotFound_fn, "MinMax|" & campo)
            isOk = False
         End If
      ElseIf notEmpty Then 'Preenchimento obrigatorio
         msg = Get_Message(campo, imsg.Empty)
         isOk = False
      End If

      Return (isOk And isIntervalOk) ' retorna True se não houver restrições
   End Function

#End Region

#Region " DataMinMax - Consiste a data passada como parametro e retorna a data formatada e caso haja inconsistencia sera devolvida a mensagem em msg "

   Public Function DataMinMax(campo As String, data_string As String, ByRef data_formatada As String, notEmpty As Boolean, ByRef msg As String, Optional ByRef data_parsed As Date? = Nothing) As Boolean
      Dim DBy As New DBSystemEntities
      Dim isOk As Boolean = True
      Dim isIntervalOk As Boolean = True

      msg = Nothing
      data_formatada = Nothing

      If Not String.IsNullOrWhiteSpace(data_string) Then
         Dim lDataMinMax As List(Of Lista_DataMinMax) = HttpContext.Current.Session.Item("lDataMinMax")
         Dim qDMMs = (From mm In lDataMinMax Where mm.Campo = campo).FirstOrDefault

         If qDMMs IsNot Nothing Then
            Dim valor As DateTime

            If isDigit(data_string) Then
               With data_string
                  If .Length = 1 Then
                     data_string = CInt(data_string).ToString("00") & "-" & Now.Month.ToString("00") & "-" & Now.Year.ToString("0000")
                  ElseIf .Length = 2 Then
                     data_string = .Substring(0, 2) & "-" & Now.Month.ToString("00") & "-" & Now.Year.ToString("0000")
                  ElseIf .Length = 4 Then
                     data_string = .Substring(0, 2) & "-" & .Substring(2, 2)
                  ElseIf .Length = 6 Then
                     data_string = .Substring(0, 2) & "-" & .Substring(2, 2) & "-" & .Substring(4, 2)
                  ElseIf .Length = 8 Then
                     data_string = .Substring(0, 2) & "-" & .Substring(2, 2) & "-" & .Substring(4, 4)
                  End If
               End With
            End If

            If Date.TryParse(data_string, valor) Then
               With qDMMs
                  data_formatada = valor.ToString(.Formato)
                  data_parsed = valor

                  If (.Definir_Minimo AndAlso valor < .Data_Minima) OrElse (.Definir_Maximo AndAlso valor > .Data_Maxima) OrElse (.Minimo_Today AndAlso New Date(valor.Year, valor.Month, valor.Day) < New Date(Now.Year, Now.Month, Now.Day)) OrElse (.Maximo_Today AndAlso New Date(valor.Year, valor.Month, valor.Day) > New Date(Now.Year, Now.Month, Now.Day)) Then
                     isIntervalOk = False
                  End If
               End With
            Else
               isOk = False
            End If

            If Not isOk Then
               msg = Get_Message(Nothing, imsg.DataMinMaxError)
            ElseIf Not isIntervalOk Then
               msg = Get_Message(campo, imsg.DataMinMax)
            End If
         Else
            msg = Get_Message(Nothing, imsg.MinMaxNotFound_fn, "DataMinMax|" & campo)
            isOk = False
         End If
      ElseIf notEmpty Then 'Preenchimento obrigatorio
         msg = Get_Message(campo, imsg.Empty)
         isOk = False
      End If

      Return (isOk And isIntervalOk) ' retorna True se não houver restrições
   End Function

#End Region

#End Region

   '--- DrillDown ---

#Region " ...DrillDown... "

#Region " Set_Click_Drill_Down - Usa exec p/ ativar a funcao click do DrillDown menu usando a funcao java Set_Click_Drill_Down. "

   Public Sub Set_Click_Drill_Down(tbl As String, linha As String, lRows As IEnumerable(Of Object), ByRef Lista_Return As Lista_Request, adicionar As Boolean)
      Dim TblNames As List(Of Lista_TblNames) = HttpContext.Current.Session.Item("TblNames")
      Dim tb = TblNames.Find(Function(x) x.tbl = tbl)
      Dim start_name_ = TblNames.Find(Function(x) x.tbl = tbl).start_name & "_"
      Dim rowspan = If(tb.rowspan = 1, Nothing, tb.rowspan.ToString)
      Dim first_line_tr = If(adicionar, linha, lRows.Where(Function(x) CInt(x.linha) < CInt(linha)).OrderBy(Function(x) x.linha).Select(Function(x) x.linha).LastOrDefault)
      Dim next_line_tr = lRows.Where(Function(x) CInt(x.linha) > CInt(linha)).OrderBy(Function(x) x.linha).Select(Function(x) x.linha).FirstOrDefault

      If first_line_tr Is Nothing Then first_line_tr = next_line_tr

      Dim id = start_name_ & "DrillDown_" & first_line_tr
      Dim parent_1 = start_name_ & "tr" & rowspan & "_" & first_line_tr
      Dim parent_2 = If(next_line_tr IsNot Nothing, start_name_ & "tr" & If(rowspan IsNot Nothing, "1", Nothing) & "_" & next_line_tr, "end_of_table")

      ' Desabilita o evento click e habilita novamente o evento click com novos parametros.
      Lista_Return.exec.Add("Click_Drill_Down('" & id & "','" & parent_1 & "','" & parent_2 & "')")
   End Sub

   ''' <summary>
   ''' selector e' usado p/ casos especificos como o table de NCMs
   ''' que tem tables inside td, sendo necessario portanto remover as tr c/ estes tables. 
   ''' </summary>
   Public Sub Set_Click_Drill_Down_Selector(id As String, selector As String, ByRef Lista_Return As Lista_Request)
      ' Desabilita o evento click e habilita novamente o evento click com novos parametros.
      Lista_Return.exec.Add("Click_Drill_Down('" & id & "',null,null,'" & selector & "')")
   End Sub

#End Region

#Region " Remove_Current_DrillDown_Lines - Usa exec_first para remover as linhas que estao relacionadas à linha do drilldown "
   ''' <summary>
   ''' As instrucoes abaixo foram adicionadas como seguranca, pois pode ocorrer casos (ex. falha de conexao e etc.) onde ao abrir o menu drilldown,
   ''' ja tenham linhas sendo exibidas e apos adicionar novas linhas, as mesmas ficam duplicadas na tela do usuario. 
   ''' </summary>
   Public Sub Remove_Current_DrillDown_Lines(tbl As String, linha As String, lRows As IEnumerable(Of Object), ByRef Lista_Return As Lista_Request)
      ' Obtem a linha da proxima tr, para remover as linhas usando a funcao jquery .nextUntil()
      Dim TblNames As List(Of Lista_TblNames) = HttpContext.Current.Session.Item("TblNames")
      Dim tb = TblNames.Find(Function(x) x.tbl = tbl)
      Dim start_name_ = TblNames.Find(Function(x) x.tbl = tbl).start_name & "_"
      Dim rowspan = If(tb.rowspan = 1, Nothing, tb.rowspan.ToString)
      Dim next_line_tr = lRows.Where(Function(x) CInt(x.linha) > CInt(linha)).OrderBy(Function(x) x.linha).Select(Function(x) x.linha).FirstOrDefault
      Dim parent1 = start_name_ & "tr" & rowspan & "_" & linha
      Dim parent2 = If(next_line_tr IsNot Nothing, start_name_ & "tr" & If(rowspan IsNot Nothing, "1", Nothing) & "_" & next_line_tr, "end_of_table")

      'Remove as linhas que estao relacionadas à linha do drilldown.
      Lista_Return.remove_dd.Add(tbl & ";" & parent1 & ";" & parent2)
   End Sub

#End Region

#Region " Set_Img_DrillDown - Seta a imagem do drilldown menu, conforme parametros "

   Public Sub Set_Img_DrillDown(drilldown_tag As String, linha As String, open_drilldown As Boolean, ByRef Lista_Return As Lista_Request)
      Set_ImageUrl(Lista_Return, drilldown_tag, open_drilldown)

      With Lista_Return.prop
         Dim index = .FindLastIndex(Function(x) x.StartsWith(drilldown_tag) AndAlso x.Contains("src"))
         .Item(index) = .Item(index).Replace(drilldown_tag, drilldown_tag & "_" & linha)
      End With
   End Sub

#End Region

#Region " Get_insertAfter - Obtem o insertAfter (geralmente usado p/ inicializar insertAfter de itens em tabelas c/ drilldown menu) "

   Public Function Get_insertAfter(tbl_item As String, lRows As IEnumerable(Of Object), Lista_Cad As Object) As String
      Dim TblNames As List(Of Lista_TblNames) = HttpContext.Current.Session.Item("TblNames")
      Dim tb_item = TblNames.Find(Function(x) x.tbl = tbl_item)
      Dim tb_master = TblNames.Find(Function(x) x.tbl = tb_item.tbl_referencia)
      Dim start_name_master_ = tb_master.start_name & "_"
      Dim start_name_item_ = tb_item.start_name & "_"
      Dim rowspan_master = If(tb_master.rowspan = 1, Nothing, tb_master.rowspan.ToString)
      Dim rowspan_item = If(tb_item.rowspan = 1, Nothing, tb_item.rowspan.ToString)
      Dim Itens_Obj As IEnumerable(Of Object) = Lista_Cad.GetType.GetProperty(tb_item.tbl).GetValue(Lista_Cad)
      Dim lLines As New List(Of String)
      Dim has_item = False
      Dim insertAfter As String = Nothing
      Dim value As String = Nothing
      Dim master_linha = lRows.First.GetType.GetProperty(start_name_master_ & "linha").GetValue(lRows.First)

      For Each row In Itens_Obj
         value = row.GetType.GetProperty(start_name_master_ & "linha").GetValue(row)
         If value = master_linha Then
            has_item = True
            lLines.Add(row.linha)
         End If
      Next

      Dim lLn1 = lLines.OrderBy(Function(x) x).ToList
      Dim lLn2 = lRows.Select(Function(x) x.linha.ToString).OrderBy(Function(x) x).ToList

      If has_item AndAlso (lLn1.Except(lLn2).Count > 0 OrElse lLn2.Except(lLn1).Count > 0) AndAlso Not {"MPDs", "MPTs", "MPCs"}.Contains(tb_master.tbl) Then
         insertAfter = start_name_item_ & "tr" & rowspan_item & "_" & lLn1.Last
      Else
         insertAfter = start_name_master_ & "tr" & rowspan_master & "_" & master_linha
      End If

      Return lRows.First.linha & ";" & insertAfter
   End Function

#End Region

#End Region

   '--- BD ---

#Region " ...BD... "

#Region " DB_UpdateRows - Atualiza o BD com as informacoes passadas nos parametros "

   Public Sub DB_UpdateRows(DB As DBWorkersEntities, data_update As Date, ByRef Lista_Return As Lista_Request, user_token As UserToken)
      If DB.ChangeTracker.HasChanges Then
         Try
            DB.SaveChanges()
         Catch ex As Exception
            Set_Msg(Lista_Return, ex.Message.Replace(vbCrLf, "<br>"))
         End Try
      Else
         Set_Msg(Lista_Return, Get_Message(Nothing, imsg.NoUpdate))
      End If
   End Sub

#End Region

#End Region

   '--- Token ---

#Region " ...Token... "

#Region " Token_Check - Efetua a validacao do token e inicializa user_token "

   Public Function Token_Check(token As String, ByRef user_token As UserToken) As Boolean
      Dim ok As Boolean = True

      'Usuario provisorio apenas p/ esta versao, ja que nao tem a pagina Login.aspx e etc..
      With user_token
         .empresa = "1"
         .user_id = "1"
         .login_name = "usuario_teste"
         .password = "CA7163F3FE9726F276FB5E7E1299F8B6"
         .dispositivo = "20"
         .logon = True
         .logon_date = Now
         .status = 0
      End With
      ok = True


      'If Not String.IsNullOrWhiteSpace(token) Then
      '   Dim DB As New DBWorkersEntities
      '   Dim wrapper As New Simple3Des(kSalt)

      '   Try
      '      Dim sToken = Split(token, "|")

      '      user_token.empresa = wrapper.DecryptData(sToken(0), user_token, ok)
      '      If ok Then user_token.user_id = wrapper.DecryptData(sToken(1), user_token, ok)
      '      If ok Then user_token.login_name = wrapper.DecryptData(sToken(2), user_token, ok)
      '      If ok Then user_token.password = wrapper.DecryptData(sToken(3), user_token, ok)
      '      If ok Then user_token.dispositivo = wrapper.DecryptData(sToken(4), user_token, ok)
      '      If ok Then user_token.logon = wrapper.DecryptData(sToken(5), user_token, ok)
      '      If ok Then user_token.logon_date = wrapper.DecryptData(sToken(6), user_token, ok)

      '      If ok Then
      '         With user_token
      '            If .logon Then
      '               If .logon_date >= Now.AddDays(-60) Then  ' Caso o token seja anterior a 30 dias seta ok = False, para solicitar confirmacao da senha.
      '                  Dim _usuario = (From user In DB.Usuarios Where user.Tipo = "U" AndAlso user.Usuario = .user_id).FirstOrDefault

      '                  If _usuario IsNot Nothing Then
      '                     If _usuario.Login_Name = .login_name AndAlso _usuario.Password = .password Then
      '                        If _usuario.Password_Expiration IsNot Nothing AndAlso _usuario.Password_Expiration < Now Then
      '                           .status = 7
      '                           .msg = Get_Message(Nothing, imsg.Token7)
      '                           ok = False
      '                        End If
      '                     Else
      '                        .status = 6
      '                        .msg = Get_Message(Nothing, imsg.Token6)
      '                        ok = False
      '                     End If
      '                  Else
      '                     .status = 5
      '                     .msg = Get_Message(Nothing, imsg.Token5)
      '                     ok = False
      '                  End If
      '               Else
      '                  .status = 4
      '                  .msg = Get_Message(Nothing, imsg.Token4)
      '                  ok = False
      '               End If
      '            Else
      '               .status = 3
      '               .msg = Get_Message(Nothing, imsg.Token3)
      '               ok = False
      '            End If

      '            If ok Then .status = 0
      '         End With
      '      End If
      '   Catch ex As CryptographicException
      '      user_token.status = 2
      '      user_token.msg = Get_Message(Nothing, imsg.Token2)
      '      ok = False
      '   End Try
      'Else
      '   user_token.status = 1
      '   'user_token.msg = Get_Message(Nothing, imsg.Token1) 
      '   user_token.msg = "Token is empty"
      '   ok = False
      'End If

      Return ok
   End Function

#End Region

#Region " TokenCrypto - Criptografa e serializa as informacoes do token "

   Public Function TokenCrypto(user_token As UserToken) As String
      Dim wrapper As New Simple3Des(kSalt)
      Dim UserTokenList As New List(Of String)
      Dim token = String.Empty

      With UserTokenList
         .Add(wrapper.EncryptData(user_token.empresa))
         .Add(wrapper.EncryptData(user_token.user_id))
         .Add(wrapper.EncryptData(user_token.login_name))
         .Add(wrapper.EncryptData(user_token.password))
         .Add(wrapper.EncryptData(user_token.dispositivo))
         .Add(wrapper.EncryptData(user_token.logon))
         .Add(wrapper.EncryptData(user_token.logon_date))
      End With

      UserTokenList.ForEach(Sub(x) token += If(token = String.Empty, Nothing, "|") & x)

      Return token
   End Function

#End Region

#Region " ...HashMe,ByteArrayToString - Função para efetuar a criptografia Salt & Hash... "

   Public Function HashMe(ByVal Data As String) As String
      Dim bytSource() As Byte
      Dim bytHash() As Byte
      Dim rv As String
      'Create a byte array from source data.
      bytSource = ASCIIEncoding.ASCII.GetBytes(Data & kSalt)
      'Compute hash based on source data.
      bytHash = New MD5CryptoServiceProvider().ComputeHash(bytSource)
      rv = ByteArrayToString(bytHash)
      Return rv
   End Function

   Private Function ByteArrayToString(ByVal arrInput() As Byte) As String
      Dim i As Integer
      Dim sOutput As New StringBuilder(arrInput.Length)
      For i = 0 To arrInput.Length - 1
         sOutput.Append(arrInput(i).ToString("X2"))
      Next
      Return sOutput.ToString()
   End Function

#End Region

#End Region

   '--- Generics ---

#Region " ...Generics... "

#Region " Remove_Caracteres_Especiais - Remove caracteres especiais "

   Public Function Remove_Caracteres_Especiais(ByVal msg As String) As String
      Dim texto_com_acentos As String = "àâãáäèêéëìîíïòôõóöùûúüçñÀÂÃÁÄÈÊÉËÌÎÍÏÒÔÕÓÖÙÛÚÜÇÑ"
      Dim texto_sem_acentos As String = "AAAAAEEEEIIIIOOOOOUUUUCNAAAAAEEEEIIIIOOOOOUUUUCN"
      Dim newmsg As String = Nothing
      Dim newcaracter As Char
      Dim caracterOK As Boolean

      For Each caracter As Char In msg.ToCharArray
         If texto_com_acentos.IndexOf(caracter) >= 0 Then
            newcaracter = texto_sem_acentos.Substring(texto_com_acentos.IndexOf(caracter), 1)
         Else
            newcaracter = caracter
         End If
         caracterOK = (Asc(newcaracter) >= 32 AndAlso Asc(newcaracter) <= 90) OrElse (Asc(newcaracter) >= 97 AndAlso Asc(newcaracter) <= 122)
         newmsg += IIf(caracterOK, newcaracter.ToString, " ".ToString)
      Next

      Return newmsg
   End Function

#End Region

#Region " isDigit - Verifica se a string é composta somente por números "

   Public Function isDigit(value As String) As Boolean
      Dim ok As Boolean = True
      If Not String.IsNullOrWhiteSpace(value) Then
         For Each item In value.ToCharArray
            If Not Char.IsDigit(item) Then ok = False
         Next
      Else
         ok = False
      End If
      Return ok
   End Function

#End Region

#Region " hasPunctuation - Verifica se na string tambem existem sinais de pontuacao, como por exemplo . - / ( ) espacos e etc. "

   Public Function hasPunctuation(value As String) As Boolean
      Dim ok As Boolean = False
      If value IsNot String.Empty Then
         For Each item In value.ToCharArray
            If Char.IsPunctuation(item) OrElse Char.IsWhiteSpace(item) Then ok = True
         Next
      End If

      Return ok
   End Function

#End Region

#Region " RemoveSinais - remove os caracteres como por exemplo . - / ( ) espacos e etc. "

   Public Function RemoveSinais(ByVal texto As String) As String
      Dim newtexto As String = Nothing

      If Not String.IsNullOrWhiteSpace(texto) Then
         newtexto = String.Empty
         For Each item In texto.ToCharArray
            If Not Char.IsPunctuation(item) AndAlso Not Char.IsWhiteSpace(item) Then
               newtexto += item
            End If
         Next
      End If

      Return newtexto
   End Function

#End Region

#Region " isUrlValid - Verifica se a URL e' valida. (e' necessario http:// ou https://) "

   Public Function isUrlValid(url As String) As Boolean
      Dim ok As Boolean = True

      If Not url.StartsWith("http") AndAlso Not url.StartsWith("//") Then url = "http://" & url

      Try
         Dim request As System.Net.WebRequest = System.Net.WebRequest.Create(url)
         Dim response As System.Net.WebResponse = request.GetResponse()
      Catch ex As Exception
         ok = False
      End Try

      Return ok
   End Function

#End Region

#Region " convert_to_csv - converte o dado para o formato csv "
   ''' <summary>
   ''' Esta function efetua um replace de double, single quotes, e retorna o valor formatado no padrao csv.
   ''' " p/ "" e' usado no padrao csv e ' p/ || e' para manter a string compativel c/ o json.
   ''' e se houver delimitador (";") no texto, retorna o value entre double quotes.
   ''' </summary>

   Public Function convert_to_csv(value As String) As String
      Dim new_value = ""
      Dim delimiter = False

      If value IsNot Nothing Then
         For i = 0 To value.Length - 1
            If value(i) = """" Then
               new_value += """""" 'Se houver aspas, substitui " por "" (padrao do formato CSV p/ evitar erros quando ha' ; c/ aspas intercalado na informacao.
            Else
               new_value += value(i)
            End If

            If value(i) = ";" Then delimiter = True
         Next
      End If

      Return If(delimiter, """" & new_value & """", new_value)
   End Function

#End Region

#Region " split_csv - Retorna o array da linha que esta no formato CSV "

   Public Function split_csv(line As String) As String()
      Dim sline As String() = Nothing

      If line IsNot Nothing Then
         Dim lColumns As New List(Of String)
         Dim previous_char = String.Empty
         Dim column As String = String.Empty
         Dim qmark = False
         Dim double_qmark = False
         Dim previous_double_qmark = False
         Dim delimiter = ";"
         Dim c As String

         'qmark e' usado como flag de controle para saber se a column esta c/ quotation mark.
         'Ex: ABC;"DEF";GHI   ou   ABC;"Uma frase com ;";DEF
         'double_qmark e' usado como flag de controle para saber quando e' duplo quotation mark. Obs: Faz parte do protocolo CSV
         'Ex: ABC;"DEF "fone e email" GHI";JKL   ou    ABC;"Uma frase c/ ; e "";DEF

         For i = 0 To line.Length - 1
            c = line(i)

            If c = """" Then
               If double_qmark Then
                  column += c
                  double_qmark = False
                  previous_double_qmark = True
               Else
                  If (i = 0 OrElse previous_char = delimiter) AndAlso Not qmark Then
                     qmark = True
                     double_qmark = False
                  ElseIf ((i + 1) <= (line.Length - 1)) AndAlso line(i + 1) = delimiter Then
                     qmark = False
                     double_qmark = False
                  Else
                     double_qmark = Not (previous_char = """" AndAlso Not previous_double_qmark)
                  End If
                  previous_double_qmark = False
               End If
            ElseIf c = delimiter AndAlso Not qmark Then
               lColumns.Add(column)
               column = String.Empty
            Else
               column += c
            End If
            previous_char = c
         Next

         lColumns.Add(column)
         sline = lColumns.ToArray
      End If

      Return sline
   End Function

#End Region

#Region " GetRandom - Obtem o numero randomico inteiro "

   Public Function GetRandom(ByVal Min As Integer, ByVal Max As Integer) As Integer
      ' by making Generator static, we preserve the same instance '
      ' (i.e., do not create new instances with the same seed over and over) '
      ' between calls '
      Static Generator As Random = New Random()
      Return Generator.Next(Min, Max)
   End Function

#End Region

#Region " Retorna o nome do modulo "
   ''' <summary>
   ''' A pagina foi colocado como parametro opcional apenas para este formulario
   ''' ja' que o default e' sempre retornar o nome da pagina que esta sincronizado
   ''' com o campo Modulo do DB.
   ''' </summary>
   ''' <param name="pagina">Para retornar o nome da pagina</param>
   ''' <returns></returns>
   Public Function Get_Modulo(Optional pagina As Boolean = False) As String
      Return If(pagina, HttpContext.Current.Request.CurrentExecutionFilePath.Remove(0, 1).Replace(".aspx", ""), "Parametros")
   End Function

#End Region

#Region " Ajusta_Rateios_Itens - Se houver diferença do total dos valores rateados (itens) em relação ao valor total (header), efetua ajuste rateando a diferença nos itens"

   Public Function Ajusta_Rateios_Itens(campo As String, ByRef qLista_Obj As IEnumerable(Of Object), valor_total As Decimal, valor_total_rateado As Decimal, ByRef Optional Lista_Return As Lista_Request = Nothing, Optional campo_formatado As String = Nothing, Optional mask As String = Nothing) As Boolean
      Dim ajustado = False

      If valor_total - valor_total_rateado <> 0D Then
         Dim lista_name As String = qLista_Obj.GetType.GetProperties.Select(Function(x) x.PropertyType.Name).FirstOrDefault
         Dim TblName As List(Of Lista_TblNames) = HttpContext.Current.Session.Item("TblNames")
         Dim tb_name = TblName.Find(Function(x) x.tbl = lista_name.Replace("Lista_", Nothing))
         Dim start_name = If(tb_name IsNot Nothing, tb_name.start_name, Nothing)
         Dim diferença, valor_decimal, value As Decimal
         Dim index As Integer
         Dim row As Object
         Dim linha As String

         diferença = (valor_total - valor_total_rateado)
         valor_decimal = IIf(diferença > 0D, 0.01, -0.01)

         Do
            row = qLista_Obj(index)
            value = row.GetType.GetProperty(campo).GetValue(row)
            row.GetType.GetProperty(campo).SetValue(row, value + valor_decimal)

            If campo_formatado IsNot Nothing Then
               row.GetType.GetProperty(campo_formatado).SetValue(row, (value + valor_decimal).ToString(mask, cc))
               value = row.GetType.GetProperty(campo_formatado).GetValue(row)
               linha = row.GetType.GetProperty("linha").GetValue(row)
               Set_Value(Lista_Return, start_name & "_" & campo_formatado & "_" & linha, value)
            End If

            diferença -= valor_decimal
            index += 1
         Loop While diferença <> 0D AndAlso index <= qLista_Obj.Count - 1

         ajustado = True
      End If

      Return ajustado
   End Function

#End Region

#Region " Clone_Row - Clona as informacoes da row "

   Public Function Clone_Row(row As Object) As Object
      Dim row_clone As New Object

      If row IsNot Nothing Then
         row_clone = row.GetType.GetConstructor(Type.EmptyTypes).Invoke(New Object() {})

         'copy one object to another via reflection properties
         For Each p As PropertyInfo In row.GetType().GetProperties()
            If p.CanRead Then
               row_clone.GetType().GetProperty(p.Name).SetValue(row_clone, p.GetValue(row))
            End If
         Next
      End If

      Return row_clone
   End Function

#End Region

#Region " Consiste_Email - Verifica se o email e' valido "

   Public Function Consiste_Email(email As String) As Boolean
      Dim ok As Boolean = True

      If Not String.IsNullOrWhiteSpace(email) Then
         Try
            Dim em = New Net.Mail.MailAddress(email)
         Catch ex As FormatException
            ok = False
         End Try
      End If

      Return ok
   End Function

#End Region

#End Region

   '--- Culture / Dates ---

#Region " ...Culture / Dates... "

#Region " Set_enUS_Culture - Define a cultura (en-US ou a cultura corrente Ex: pt-BR "

   Public Sub Set_enUS_Culture(enUS As Boolean)
      Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(If(enUS, "en-US", cc.Name))
      Thread.CurrentThread.CurrentUICulture = New CultureInfo(If(enUS, "en-US", cc.Name))
   End Sub

#End Region

#Region " GetFridayDate - Obtem a data do dia da semana, prox. semana, semana passada e etc. "

   Public Function GetFridayDate(DayToGet As DayOfWeek, iDays As Integer) As Date
      Return Now.AddDays(iDays + (DayToGet - Now.DayOfWeek))
   End Function

#End Region

#Region " GetCurrentDate - Obtem a data/hora atual "
   ''' <summary>
   ''' Esta funcao retorna a data hora, pois, devido 'a precisao do sistema DateTime,
   ''' ao obter valores seguidos, usando DateTime.Now, observa-se que os milisegundos/microsegundos ficam duplicados.
   ''' Para evitar este problema esta sendo usada esta function obtida do forum stackoverflow.
   ''' Com esta funcao os valores de milisegundos/microsegundos ficam unicos p/ cada chamada.
   ''' </summary>
   Public Function GetCurrentDate() As DateTime
      Dim lastDateOrig, lastDateNew As Long

      Do
         lastDateOrig = lastDate
         lastDateNew = lastDateOrig + 1
         lastDateNew = Math.Max(DateTime.Now.Ticks, lastDateNew)
      Loop While Interlocked.CompareExchange(lastDate, lastDateNew, lastDateOrig) <> lastDateOrig

      Return New DateTime(lastDateNew, DateTimeKind.Local)
   End Function

#End Region

#Region " GetInterval_UserView - Retorna o intervalo no formato string, c/ formato que facilita a visualizacao pelo usuario. "

   Public Function GetInterval_UserView(interval_time As TimeSpan) As String
      Dim interval As String = Nothing

      If interval_time.Hours <= 0 Then
         interval += interval_time.Minutes.ToString & ":" & Format(interval_time.Seconds + If(interval_time.Milliseconds > 500, 1, 0), "00") & "s"
      Else
         interval += interval_time.Hours.ToString & ":" & Format(interval_time.Minutes, "#0") & ":" & Format(interval_time.Seconds + If(interval_time.Milliseconds > 500, 1, 0), "00")
      End If

      Return interval
   End Function

#End Region

#Region " Obtem o periodo (formal ou 'verbal') "

#Region " Get_Lista_VerbalDates - Obtem a Lista de VerbalDates "

   Private Function Get_Lista_VerbalDates() As List(Of Lista_VerbalDates)
      Dim lVDs As New List(Of Lista_VerbalDates)
      Dim vd As Lista_VerbalDates

      With lVDs
         .Add(New Lista_VerbalDates With {.Nome = "Janeiro", .Categoria = "M"})
         .Add(New Lista_VerbalDates With {.Nome = "Fevereiro", .Categoria = "M"})
         .Add(New Lista_VerbalDates With {.Nome = "Março", .Categoria = "M"})
         .Add(New Lista_VerbalDates With {.Nome = "Abril", .Categoria = "M"})
         .Add(New Lista_VerbalDates With {.Nome = "Maio", .Categoria = "M"})
         .Add(New Lista_VerbalDates With {.Nome = "Junho", .Categoria = "M"})
         .Add(New Lista_VerbalDates With {.Nome = "Julho", .Categoria = "M"})
         .Add(New Lista_VerbalDates With {.Nome = "Agosto", .Categoria = "M"})
         .Add(New Lista_VerbalDates With {.Nome = "Setembro", .Categoria = "M"})
         .Add(New Lista_VerbalDates With {.Nome = "Outubro", .Categoria = "M"})
         .Add(New Lista_VerbalDates With {.Nome = "Novembro", .Categoria = "M"})
         .Add(New Lista_VerbalDates With {.Nome = "Dezembro", .Categoria = "M"})
         .Add(New Lista_VerbalDates With {.Nome = "Hoje", .Categoria = "D"})
         .Add(New Lista_VerbalDates With {.Nome = "Dia", .Categoria = "D"})
         .Add(New Lista_VerbalDates With {.Nome = "Ontem", .Categoria = "D"})
         .Add(New Lista_VerbalDates With {.Nome = "AnteOntem", .Categoria = "D"})
         .Add(New Lista_VerbalDates With {.Nome = "Amanhã", .Categoria = "D"})
         .Add(New Lista_VerbalDates With {.Nome = "Semana", .Categoria = "W"})
         .Add(New Lista_VerbalDates With {.Nome = "Semana Passada", .Categoria = "W"})
         .Add(New Lista_VerbalDates With {.Nome = "Próxima Semana", .Categoria = "W"})
         .Add(New Lista_VerbalDates With {.Nome = "Mes", .Categoria = "X"})
         .Add(New Lista_VerbalDates With {.Nome = "Mes Passado", .Categoria = "X"})
         .Add(New Lista_VerbalDates With {.Nome = "Próximo Mes", .Categoria = "X"})
         .Add(New Lista_VerbalDates With {.Nome = "Ano", .Categoria = "Y"})
         .Add(New Lista_VerbalDates With {.Nome = "Ano Passado", .Categoria = "Y"})
         .Add(New Lista_VerbalDates With {.Nome = "Próximo Ano", .Categoria = "Y"})
         .Add(New Lista_VerbalDates With {.Nome = "1a. Quinzena", .Categoria = "Q"})
         .Add(New Lista_VerbalDates With {.Nome = "2a. Quinzena", .Categoria = "Q"})
         .Add(New Lista_VerbalDates With {.Nome = "1o. Trimestre", .Categoria = "T"})
         .Add(New Lista_VerbalDates With {.Nome = "2o. Trimestre", .Categoria = "T"})
         .Add(New Lista_VerbalDates With {.Nome = "3o. Trimestre", .Categoria = "T"})
         .Add(New Lista_VerbalDates With {.Nome = "4o. Trimestre", .Categoria = "T"})
         .Add(New Lista_VerbalDates With {.Nome = "1o. Semestre", .Categoria = "6"})
         .Add(New Lista_VerbalDates With {.Nome = "2o. Semestre", .Categoria = "6"})

         'Meses
         For i = 1 To 12
            vd = .Item(i - 1)
            vd.Data_Inicial = New Date(Today.Year, i, 1)
            vd.Data_Final = New Date(Today.Year, i, Date.DaysInMonth(Today.Year, i))
         Next

         'Hoje
         vd = .Find(Function(x) x.Nome = "Hoje")
         vd.Data_Inicial = Today
         vd.Data_Final = vd.Data_Inicial

         'Dia
         vd = .Find(Function(x) x.Nome = "Dia")
         vd.Data_Inicial = Today
         vd.Data_Final = vd.Data_Inicial

         'Ontem
         vd = .Find(Function(x) x.Nome = "Ontem")
         vd.Data_Inicial = Today.AddDays(-1)
         vd.Data_Final = vd.Data_Inicial

         'AnteOntem
         vd = .Find(Function(x) x.Nome = "AnteOntem")
         vd.Data_Inicial = Today.AddDays(-2)
         vd.Data_Final = vd.Data_Inicial

         'Amanhã
         vd = .Find(Function(x) x.Nome = "Amanhã")
         vd.Data_Inicial = Today.AddDays(1)
         vd.Data_Final = vd.Data_Inicial

         'Semana
         vd = .Find(Function(x) x.Nome = "Semana")
         vd.Data_Inicial = GetFridayDate(DayOfWeek.Monday, 0)
         vd.Data_Final = GetFridayDate(DayOfWeek.Sunday, 7)

         'Semana Passada
         vd = .Find(Function(x) x.Nome = "Semana Passada")
         vd.Data_Inicial = GetFridayDate(DayOfWeek.Monday, -7)
         vd.Data_Final = GetFridayDate(DayOfWeek.Sunday, 0)

         'Próxima Semana
         vd = .Find(Function(x) x.Nome = "Próxima Semana")
         vd.Data_Inicial = GetFridayDate(DayOfWeek.Monday, 7)
         vd.Data_Final = GetFridayDate(DayOfWeek.Sunday, 14)

         'Ano
         vd = .Find(Function(x) x.Nome = "Ano")
         vd.Data_Inicial = New Date(Today.Year, 1, 1)
         vd.Data_Final = New Date(Today.Year, 12, 31)

         'Ano Passado
         vd = .Find(Function(x) x.Nome = "Ano Passado")
         vd.Data_Inicial = New Date(Today.Year - 1, 1, 1)
         vd.Data_Final = New Date(Today.Year - 1, 12, 31)

         'Próximo Ano
         vd = .Find(Function(x) x.Nome = "Próximo Ano")
         vd.Data_Inicial = New Date(Today.Year + 1, 1, 1)
         vd.Data_Final = New Date(Today.Year + 1, 12, 31)

         'Mes
         vd = .Find(Function(x) x.Nome = "Mes")
         vd.Data_Inicial = New Date(Today.Year, Today.Month, 1)
         vd.Data_Final = New Date(Today.Year, Today.Month, Date.DaysInMonth(Today.Year, Today.Month))

         'Mes Passado
         vd = .Find(Function(x) x.Nome = "Mes Passado")
         vd.Data_Inicial = New Date(Today.AddMonths(-1).Year, Today.AddMonths(-1).Month, 1)
         vd.Data_Final = New Date(Today.AddMonths(-1).Year, Today.AddMonths(-1).Month, Date.DaysInMonth(Today.AddMonths(-1).Year, Today.AddMonths(-1).Month))

         'Próximo Mes
         vd = .Find(Function(x) x.Nome = "Próximo Mes")
         vd.Data_Inicial = New Date(Today.AddMonths(1).Year, Today.AddMonths(1).Month, 1)
         vd.Data_Final = New Date(Today.AddMonths(1).Year, Today.AddMonths(1).Month, Date.DaysInMonth(Today.AddMonths(1).Year, Today.AddMonths(1).Month))

         '1a. Quinzena
         vd = .Find(Function(x) x.Nome = "1a. Quinzena")
         vd.Data_Inicial = New Date(Today.Year, Today.Month, 1)
         vd.Data_Final = New Date(Today.Year, Today.Month, 15)

         '2a. Quinzena
         vd = .Find(Function(x) x.Nome = "2a. Quinzena")
         vd.Data_Inicial = New Date(Today.Year, Today.Month, 15)
         vd.Data_Final = New Date(Today.Year, Today.Month, Date.DaysInMonth(Today.Year, Today.Month))

         '1o. Trimestre
         vd = .Find(Function(x) x.Nome = "1o. Trimestre")
         vd.Data_Inicial = New Date(Today.Year, 1, 1)
         vd.Data_Final = New Date(Today.Year, 3, Date.DaysInMonth(Today.Year, 3))

         '2o. Trimestre
         vd = .Find(Function(x) x.Nome = "2o. Trimestre")
         vd.Data_Inicial = New Date(Today.Year, 4, 1)
         vd.Data_Final = New Date(Today.Year, 6, Date.DaysInMonth(Today.Year, 6))

         '3o. Trimestre
         vd = .Find(Function(x) x.Nome = "3o. Trimestre")
         vd.Data_Inicial = New Date(Today.Year, 7, 1)
         vd.Data_Final = New Date(Today.Year, 9, Date.DaysInMonth(Today.Year, 9))

         '4o. Trimestre
         vd = .Find(Function(x) x.Nome = "4o. Trimestre")
         vd.Data_Inicial = New Date(Today.Year, 10, 1)
         vd.Data_Final = New Date(Today.Year, 12, Date.DaysInMonth(Today.Year, 12))

         '1o. Semestre
         vd = .Find(Function(x) x.Nome = "1o. Semestre")
         vd.Data_Inicial = New Date(Today.Year, 1, 1)
         vd.Data_Final = New Date(Today.Year, 6, Date.DaysInMonth(Today.Year, 6))

         '2o. Semestre
         vd = .Find(Function(x) x.Nome = "2o. Semestre")
         vd.Data_Inicial = New Date(Today.Year, 6, 1)
         vd.Data_Final = New Date(Today.Year, 12, Date.DaysInMonth(Today.Year, 12))
      End With

      Return lVDs
   End Function

#End Region

#Region " Get_Periodo - Obtem o periodo informado e inicializa as datas iniciais e finais e etc. "

   Public Function Get_Periodo(focus_element As String, campo As String, ByRef periodo As String, ByRef data_inicial As Date?, ByRef data_final As Date?, ByRef fdata_inicial As String, ByRef fdata_final As String, ByRef periodo_verbal As String, ByRef Lista_Return As Lista_Request) As Boolean
      Dim ok As Boolean
      Dim lDates As New List(Of String)
      Dim dTimes As New Dictionary(Of String, Date)
      Dim data_formatada As String = Nothing

      Pre_Formata_Periodo(focus_element, periodo, lDates, dTimes, ok, Lista_Return)

      If ok Then
         If Not VerbalDates_String_Check(lDates, data_inicial, data_final, periodo_verbal) Then
            Dim msg As String = Nothing

            If lDates.Count >= 2 Then
               If DataMinMax(campo, lDates.First, data_formatada, True, msg, data_inicial) Then
                  If DataMinMax(campo, lDates.Last, data_formatada, True, msg, data_final) Then
                     If data_inicial > data_final Then
                        Set_Msg(Lista_Return, Get_Message(Nothing, imsg.DataDiverIniFim), campo, True, True)
                        ok = False
                     End If
                  Else
                     Set_Msg(Lista_Return, msg, campo, True, True)
                     ok = False
                  End If
               Else
                  Set_Msg(Lista_Return, msg, campo, True, True)
                  ok = False
               End If
            Else
               If DataMinMax(campo, lDates.First, data_formatada, True, msg, data_inicial) Then
                  data_final = data_inicial
               Else
                  Set_Msg(Lista_Return, msg, campo, True, True)
                  ok = False
               End If
            End If
         ElseIf data_inicial > data_final Then
            Set_Msg(Lista_Return, Get_Message(Nothing, imsg.DataDiverIniFim), campo, True, True)
            ok = False
         End If

         If ok AndAlso data_inicial.HasValue AndAlso data_final.HasValue Then
            If dTimes.Count > 0 Then
               Dim first = dTimes.ContainsKey("First")
               Dim last = dTimes.ContainsKey("Last")
               Dim time_inicial, time_final As Date

               If first Then
                  time_inicial = dTimes("First")
                  data_inicial = New Date(data_inicial.Value.Year, data_inicial.Value.Month, data_inicial.Value.Day, time_inicial.Hour, time_inicial.Minute, time_inicial.Second)

                  If Not last Then data_final = New Date(data_final.Value.Year, data_final.Value.Month, data_final.Value.Day, time_inicial.Hour, If(time_inicial.Minute <> 0, time_inicial.Minute, 59), 59)
               End If
               If last Then
                  time_final = dTimes("Last")
                  data_final = New Date(data_final.Value.Year, data_final.Value.Month, data_final.Value.Day, time_final.Hour, time_final.Minute, time_final.Second)

                  If Not first Then data_inicial = New Date(data_inicial.Value.Year, data_inicial.Value.Month, data_inicial.Value.Day, time_final.Hour, 0, 0)
               End If

               fdata_inicial = data_inicial.Value.ToString("dd/MM/yyyy HH:mm" & If(time_inicial.Second = 0, Nothing, ":ss"))
               fdata_final = data_final.Value.ToString("dd/MM/yyyy HH:mm" & If(time_final.Second = 0, Nothing, ":ss"))
               periodo_verbal = Nothing
            Else
               fdata_inicial = data_inicial.Value.ToString("dd/MM/yyyy")
               fdata_final = data_final.Value.ToString("dd/MM/yyyy")
            End If

            If periodo_verbal Is Nothing AndAlso dTimes.Count = 0 Then
               VerbalDates_Dates_Check(data_inicial, data_final, periodo_verbal)
            End If

            periodo = If(periodo_verbal IsNot Nothing, periodo_verbal, fdata_inicial & " a " & fdata_final)
            Set_Value(Lista_Return, campo, periodo)
         End If
      End If

      Return ok
   End Function

#End Region

#Region " ...VerbalDates_String_Check, Get_VD - Verifica se a string pode ser formatada c/ VerbalDates e retorna intervalos, string formatada... "

   Private Function VerbalDates_String_Check(lDates As List(Of String), ByRef data_inicial As Date?, ByRef data_final As Date?, ByRef periodo_verbal As String) As Boolean
      Dim lVDs As List(Of Lista_VerbalDates) = HttpContext.Current.Session.Item("lVDs")
      Dim vd, vd2 As New Lista_VerbalDates

      If lDates.Count >= 2 Then
         Get_VD(lDates.First, lDates(1), vd, lVDs)

         If vd IsNot Nothing Then
            data_inicial = vd.Data_Inicial
            data_final = vd.Data_Final

            'Data Final
            Get_VD(lDates(lDates.Count - 2), lDates.Last, vd2, lVDs, vd.Nome)

            If vd2 IsNot Nothing Then data_final = vd2.Data_Final
         End If
      Else
         Get_VD(lDates.First, String.Empty, vd, lVDs)

         If vd IsNot Nothing Then
            data_inicial = vd.Data_Inicial
            data_final = vd.Data_Final
         End If
      End If

      If data_inicial.HasValue AndAlso data_final.HasValue Then
         If vd.Categoria = "M" Then 'Month
            'Se o mes atual for de Janeiro a Fevereiro e o periodo informado for de (Outubro a Dezembro)
            'ao inves de usar o ano atual, usa o ano anterior
            If Now.Month <= 2 AndAlso lVDs.FindIndex(Function(x) x.Nome = vd.Nome) >= 9 Then
               data_inicial = New Date(data_inicial.Value.Year - 1, data_inicial.Value.Month, data_inicial.Value.Day)
               data_final = New Date(data_final.Value.Year - 1, data_final.Value.Month, data_final.Value.Day)
            End If

            'Caso seja informado o ano, por exemplo, junho/2017 ano atual 2018
            'atualiza a data para o ano informado pelo usuario (casos comuns).
            For Each w In lDates
               If w.Contains("/") Then
                  Dim sMY = Split(w, "/")

                  If sMY.Count > 1 AndAlso IsNumeric(sMY(1)) Then
                     Dim year As Integer = sMY(1)

                     If year >= Now.Year - 2 OrElse year <= Now.Year + 1 Then
                        data_inicial = New Date(year, data_inicial.Value.Month, data_inicial.Value.Day)
                        data_final = New Date(year, data_final.Value.Month, data_final.Value.Day)
                     End If
                  End If
               End If
            Next
         End If

         VerbalDates_Dates_Check(data_inicial, data_final, periodo_verbal)
      End If

      Return (data_inicial.HasValue AndAlso data_final.HasValue)
   End Function

   Private Sub Get_VD(word1 As String, word2 As String, ByRef vd As Lista_VerbalDates, lVDs As List(Of Lista_VerbalDates), Optional nome_previous As String = Nothing)
      Dim v1 As String = Nothing
      Dim v2 As String = Nothing
      Dim first = False

      vd = Nothing

      For Each item In lVDs
         Dim svd = Split(item.Nome, Space(1))
         v1 = Remove_Caracteres_Especiais(svd(0).Substring(0, If(svd(0).ToLower.StartsWith("sem"), 4, 3))).ToLower
         If svd.Count >= 2 Then
            'Para categoria Semana ou Semestre usa 4 digitos
            v2 = svd(1).Substring(0, If(svd(1).ToLower.StartsWith("sem"), 4, 3)).ToLower
         Else
            v2 = "###"
         End If

         If Not Char.IsNumber(word1.First) Then
            If word1.StartsWith(v1) OrElse word2.StartsWith(v1) OrElse (word1.StartsWith(v1) AndAlso word2.StartsWith(v2)) Then
               If (Not first AndAlso If(nome_previous IsNot Nothing, nome_previous.Substring(0, 3).ToLower <> v1, True)) OrElse (word1.StartsWith(v1) AndAlso word2.StartsWith(v2)) Then
                  vd = item
                  first = True
               End If
            End If
         ElseIf word1.StartsWith(v1) AndAlso word2.StartsWith(v2) Then
            '1o., 1a, 2o., 2a., 3o., 4o.
            vd = item
            first = True
         End If
      Next
   End Sub

#End Region

#Region " VerbalDates_Dates_Check - Verifica se as datas correspondem a Lista Verbal Dates e retorna a string formatada "

   Public Function VerbalDates_Dates_Check(data_inicial As Date, data_final As Date, ByRef periodo_verbal As String) As Boolean
      Dim lVDs As List(Of Lista_VerbalDates) = HttpContext.Current.Session.Item("lVDs")
      Dim vd, vd2 As New Lista_VerbalDates

      periodo_verbal = Nothing

      vd = lVDs.Find(Function(x) x.Data_Inicial.ToShortDateString = data_inicial.ToShortDateString)
      vd2 = lVDs.Find(Function(x) x.Data_Final.ToShortDateString = data_final.ToShortDateString)

      If vd IsNot Nothing AndAlso vd2 IsNot Nothing Then
         If vd IsNot vd2 AndAlso vd.Categoria = vd2.Categoria Then
            periodo_verbal = vd.Nome & " a " & vd2.Nome
         ElseIf vd Is vd2 Then
            periodo_verbal = vd.Nome
         End If
      End If

      Return (periodo_verbal IsNot Nothing)
   End Function

#End Region

#Region " Pre_Formata_Periodo - Pre-formata a string e retorna array de string e lDates contendo os horarios se houver "

   Private Sub Pre_Formata_Periodo(focus_element As String, value As String, lDates As List(Of String), ByRef dTimes As Dictionary(Of String, Date), ByRef ok As Boolean, ByRef Lista_Return As Lista_Request)
      Dim sDates As String()

      value = Regex.Replace(value, "\s+", " ").Trim.ToLower
      value = Remove_Caracteres_Especiais(value)
      value = value.Replace(" de ", "/").Replace(" / ", "/").Replace(" - ", "/").Replace(" : ", ":").Replace("desde", Nothing).Replace("agora", "hoje").Replace("deste", Nothing).Replace(" horas", ":").Replace("minutos", ":").Replace(" as ", " ").Replace(" às ", " ")
      value = value.Replace("primeiro", "1o.").Replace("primeira", "1a.").Replace("segundo", "2o.").Replace("terceiro", "3o.").Replace("quarto", "4o.").Replace("1o ", "1o. ").Replace("1a ", "1a. ").Replace("2o ", "2o. ").Replace("2a ", "2a. ").Replace("3o ", "3o. ").Replace("4o ", "4o. ").Trim
      If value.StartsWith("de ") Then value = value.Remove(0, 3)

      sDates = Split(value, " ")
      ok = True

      For x = 0 To sDates.Count - 1
         If Not sDates(x).Contains(":") Then
            lDates.Add(sDates(x))
         Else
            Try
               Dim format() = {"HH:", "HH:mm", "HH:mm:ss", "H:", "H:mm", "H:mm:ss"}
               Dim time = Date.ParseExact(sDates(x), format, Globalization.DateTimeFormatInfo.InvariantInfo, Globalization.DateTimeStyles.None)

               If (x = sDates.Count - 1) AndAlso sDates.Count > 2 Then
                  dTimes.Add("Last", time)
               ElseIf Not dTimes.ContainsKey("First") Then
                  dTimes.Add("First", time)
               End If
            Catch ex As Exception
               ok = False
               Set_Msg(Lista_Return, Get_Message(Nothing, imsg.TimeError_fn, ex.Message), focus_element, True, True)
            End Try
         End If
      Next
   End Sub

#End Region

#End Region

#End Region

End Module
