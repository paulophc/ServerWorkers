Imports System.Net.Http.Formatting

Public Module Module1

   ' Consistencias

#Region " ...Set_work,Convert_value - Inicializa work com as informacoes de formData "

   Public Function Set_work(formData As FormDataCollection, ByRef work As Workers, ByRef msg As String) As Boolean
      Dim ok = True
      Dim value As Object

      msg = Nothing

      Try
         With work.GetType
            Dim lnames = .GetProperties.Where(Function(x) x.PropertyType.Name <> "List`1").Select(Function(x) x.Name).ToList

            For Each item In formData
               If lnames.Contains(item.Key) Then
                  value = Nothing
                  If Convert_value(.GetProperty(item.Key).PropertyType.Name, item.Value, value, msg, ok) Then
                     .GetProperty(item.Key).SetValue(work, value)
                  Else
                     Exit For
                  End If
               End If
            Next
         End With
      Catch ex As Exception
         msg = "Falha detectada ao ler informações." & vbCrLf & ex.Message
         ok = False
      End Try

      Return ok
   End Function

   'Converte o valor recebido de formData que esta em string para o formato correspondente em work
   Private Function Convert_value(wname As String, fvalue As String, ByRef value As Object, ByRef msg As String, ByRef ok As Boolean) As Boolean
      With wname
         If .Equals("Int32") Then
            If Not Integer.TryParse(fvalue, value) Then
               msg = "Não foi possível converter o valor " & fvalue & " do campo " & wname & " para Integer"
               ok = False
            End If
         ElseIf .Equals("Decimal") Then
            If Not Decimal.TryParse(fvalue, value) Then
               msg = "Não foi possível converter o valor " & fvalue & " do campo " & wname & " para Decimal."
               ok = False
            End If

         ElseIf .Equals("DateTime") Then
            If Not DateTime.TryParse(fvalue, value) Then
               msg = "Não foi possível converter o valor " & fvalue & " do campo " & wname & " para DateTime."
               ok = False
            End If
         Else
            value = fvalue
         End If
      End With

      Return ok
   End Function

#End Region

#Region " Valida_work - Efetua a validacao dos campos "

   Public Function Valida_work(work As Workers, ByRef msg As String) As Boolean
      Dim msg2 As String = Nothing
      Dim lMsg As New List(Of String)

      If Not Consiste_Campo_Vazio(work.Nome, "Nome", msg2) Then lMsg.Add(msg2)
      If Not Consiste_Campo_Vazio(work.Cargo, "Cargo", msg2) Then lMsg.Add(msg2)
      If Not Consiste_Email(work.Email, msg2) Then lMsg.Add(msg2)
      If Not Consiste_Salario(work.Salario, msg2) Then lMsg.Add(msg2)
      If Not Consiste_DataContracao(work.Data_Contratacao, msg2) Then lMsg.Add(msg2)

      If lMsg.Count > 0 Then
         msg = String.Empty
         For Each item In lMsg
            msg += If(msg = String.Empty, Nothing, vbCrLf) & item
         Next
      End If

      Return (lMsg.Count = 0)
   End Function

#End Region

#Region " Consiste_Email - Verifica se o email e' valido "

   Private Function Consiste_Email(email As String, ByRef msg As String) As Boolean
      Dim ok = True
      msg = Nothing

      If Not String.IsNullOrWhiteSpace(email) Then
         Try
            Dim em = New Net.Mail.MailAddress(email)
         Catch ex As FormatException
            msg = "Email inválido. " & ex.Message
            ok = False
         End Try
      End If

      Return ok
   End Function

#End Region

#Region " Consiste_Salario - Verifica se o salario e' valido "

   Private Function Consiste_Salario(salario As Decimal, ByRef msg As String) As Boolean
      Dim ok = True
      msg = Nothing

      If salario < 998 OrElse salario > 60000 Then
         ok = False
         msg = "Salário deve ser preenchido com valor entre 998.00 e 60.000."
      End If

      Return ok
   End Function

#End Region

#Region " Consiste_Campo_Vazio - Verifica se o campo esta preenchido "

   Private Function Consiste_Campo_Vazio(campo As String, nome_campo As String, ByRef msg As String) As Boolean
      Dim ok = True
      msg = Nothing

      If String.IsNullOrWhiteSpace(campo) Then
         msg = nome_campo & " requer preenchimento obrigatório."
         ok = False
      End If

      Return ok
   End Function

#End Region

#Region " Consiste_DataContracao - Verifica se a data esta no intervalo correto "

   Private Function Consiste_DataContracao(data_contracao As Date, ByRef msg As String) As Boolean
      Dim ok = True
      msg = Nothing

      If data_contracao < CDate("01/01/2010") OrElse data_contracao > Now Then
         ok = False
         msg = "Data de contratação, fora do intervalo permitido."
      End If

      Return ok
   End Function

#End Region

   ' DB

#Region " DB_Update - Atualiza informacoes no banco de dados "

   Public Function DB_Update(ByRef DB As DBWorkersEntities, ByRef msg As String) As Boolean
      Dim ok = True

      Try
         DB.SaveChanges()
      Catch ex As Exception
         msg = "Falha ao atualizar informações no banco de dados." & vbCrLf & ex.Message
      End Try

      Return ok
   End Function

#End Region

End Module
