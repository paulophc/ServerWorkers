﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated from a template.
'
'     Manual changes to this file may cause unexpected behavior in your application.
'     Manual changes to this file will be overwritten if the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Imports System
Imports System.Data.Entity
Imports System.Data.Entity.Infrastructure

Partial Public Class DBSystemEntities
    Inherits DbContext

    Public Sub New()
        MyBase.New("name=DBSystemEntities")
    End Sub

    Protected Overrides Sub OnModelCreating(modelBuilder As DbModelBuilder)
        Throw New UnintentionalCodeFirstException()
    End Sub

    Public Overridable Property Btns() As DbSet(Of Btns)
    Public Overridable Property Campos() As DbSet(Of Campos)
    Public Overridable Property Campos_DataMinMax() As DbSet(Of Campos_DataMinMax)
    Public Overridable Property Campos_Descricoes() As DbSet(Of Campos_Descricoes)
    Public Overridable Property Campos_ImagensUrl() As DbSet(Of Campos_ImagensUrl)
    Public Overridable Property Campos_Length_Limites() As DbSet(Of Campos_Length_Limites)
    Public Overridable Property Campos_Methods() As DbSet(Of Campos_Methods)
    Public Overridable Property Campos_MinMax() As DbSet(Of Campos_MinMax)
    Public Overridable Property Campos_Modulos() As DbSet(Of Campos_Modulos)
    Public Overridable Property Campos_Seq() As DbSet(Of Campos_Seq)
    Public Overridable Property Campos_Seq_Panels() As DbSet(Of Campos_Seq_Panels)
    Public Overridable Property ComboList() As DbSet(Of ComboList)
    Public Overridable Property Mensagens() As DbSet(Of Mensagens)
    Public Overridable Property Methods_Check() As DbSet(Of Methods_Check)
    Public Overridable Property Tbls() As DbSet(Of Tbls)
    Public Overridable Property Tbls_ClickEvents() As DbSet(Of Tbls_ClickEvents)
    Public Overridable Property Tbls_Modulos() As DbSet(Of Tbls_Modulos)
    Public Overridable Property Tbls_PageInfos() As DbSet(Of Tbls_PageInfos)
    Public Overridable Property Tbls_Querys() As DbSet(Of Tbls_Querys)
    Public Overridable Property Tbls_Sorts() As DbSet(Of Tbls_Sorts)
    Public Overridable Property Temas() As DbSet(Of Temas)
    Public Overridable Property Temas_Colors() As DbSet(Of Temas_Colors)
    Public Overridable Property Temas_Css() As DbSet(Of Temas_Css)
    Public Overridable Property Templates() As DbSet(Of Templates)

End Class