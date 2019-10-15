Imports System.Net.Http.Formatting
Imports System.Web.Http

Namespace Controllers
   Public Class WorkersController
      Inherits ApiController

#Region " GetValues - GET: api/Workers"

      Public Function GetValues() As List(Of Workers)
         Dim DB As New DBWorkersEntities

         Return DB.Workers.ToList
      End Function

#End Region

#Region " GetValue - GET: api/Workers/Nome "

      Public Function GetValue(ByVal nome As String) As List(Of Workers)
         Dim DB As New DBWorkersEntities

         Return DB.Workers.Where(Function(x) x.Nome.ToUpper.StartsWith(nome.ToUpper)).ToList
      End Function

#End Region

#Region " PostValue - POST: api/Workers"

      Public Function PostValue(<FromBody()> formData As FormDataCollection) As String
         Dim DB As New DBWorkersEntities
         Dim work As New Workers
         Dim msg As String = Nothing

         If Set_work(formData, work, msg) Then
            If Valida_work(work, msg) Then
               work.Id = If(DB.Workers.Count = 0, 1, DB.Workers.OrderBy(Function(x) x.Id).Select(Function(x) x.Id + 1).ToList.Last())
               DB.Workers.Add(work)

               DB_Update(DB, msg)
            End If
         End If

         Return msg
      End Function

#End Region

#Region " PutValue - PUT: api/Workers. Usado tanto p/ alteracao ou inclusao. Inclui, caso nao encontre a row correspondente ao Id ou Nome"

      Public Function PutValue(<FromBody()> formData As FormDataCollection) As String
         Dim DB As New DBWorkersEntities
         Dim work As New Workers
         Dim msg As String = Nothing

         If Set_work(formData, work, msg) Then
            If Valida_work(work, msg) Then
               Dim _work = DB.Workers.Where(Function(x) x.Id = work.Id).FirstOrDefault

               If _work Is Nothing Then
                  _work = DB.Workers.Where(Function(x) x.Nome = work.Nome).FirstOrDefault
               End If

               If _work IsNot Nothing Then 'Altera
                  With _work
                     .Nome = work.Nome
                     .Email = work.Email
                     .Cargo = work.Cargo
                     .Salario = work.Salario
                     .Data_Contratacao = work.Data_Contratacao
                  End With
               Else 'Inclui
                  work.Id = If(DB.Workers.Count = 0, 1, DB.Workers.OrderBy(Function(x) x.Id).Select(Function(x) x.Id + 1)).ToList.Last()
                  DB.Workers.Add(work)
               End If

               DB_Update(DB, msg)
            End If
         End If

         Return msg
      End Function

#End Region

#Region " DeleteValue - DELETE: api/Workers/Id "

      Public Function DeleteValue(ByVal Id As Integer) As String
         Dim DB As New DBWorkersEntities
         Dim work = DB.Workers.Where(Function(x) x.Id = Id).FirstOrDefault
         Dim msg As String = Nothing

         If work IsNot Nothing Then DB.Workers.Remove(work)

         DB_Update(DB, msg)

         Return msg
      End Function

#End Region

   End Class
End Namespace