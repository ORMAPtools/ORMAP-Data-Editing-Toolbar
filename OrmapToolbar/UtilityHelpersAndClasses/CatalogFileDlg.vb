#Region "Copyright 2008-2011 ORMAP Tech Group"

' File:  CatalogFileDlg.vb
'
' Original Author:  OPET.NET Migration Team (Shad Campbell, James Moore, 
'                   Nick Seigal)
'
' Date Created:  20080221
'
' Copyright Holder:  ORMAP Tech Group  
' Contact Info:  ORMAP Tech Group may be reached at 
' ORMAP_ESRI_Programmers@listsmart.osl.state.or.us
'
' This file is part of the ORMAP Taxlot Editing Toolbar.
'
' ORMAP Taxlot Editing Toolbar is free software; you can redistribute it and/or
' modify it under the terms of the Lesser GNU General Public License as 
' published by the Free Software Foundation; either version 3 of the License, 
' or (at your option) any later version.
'
' This program is distributed in the hope that it will be useful, but WITHOUT 
' ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or 
' FITNESS FOR A PARTICULAR PURPOSE.  See the Lesser GNU General Public License 
' located in the COPYING.LESSER.txt file for more details.
'
' You should have received a copy of the Lesser GNU General Public License 
' along with the ORMAP Taxlot Editing Toolbar; if not, write to the Free 
' Software Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 
' 02110-1301 USA.

#End Region

#Region "Subversion Keyword Expansion"
'Tag for this file: $Name:$
'SCC revision number: $Revision: 406 $
'Date of Last Change: $Date: 2009-11-30 22:49:20 -0800 (Mon, 30 Nov 2009) $
#End Region

#Region "Imported Namespaces"
Imports System.Collections
Imports System.Windows.Forms
Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
#End Region

#Region "Class declaration"
''' <summary>
''' Programmatically expose the ArcCatalog file dialog as 
''' one integral unit for simple access and use.
''' </summary>
''' <remarks></remarks>
Public Class CatalogFileDialog

    ' TEST: [NIS] Use of System.Collections.ArrayList instead of VB.Collection object.

#Region "Class level fields"

    Private _theGxDialog As IGxDialog
    Private _selectionList As ArrayList

#End Region

#Region "Built-in class members"

    Public Sub New()
        MyBase.New()
        Try
            _theGxDialog = New GxDialog
            _theGxDialog.RememberLocation = True
            _theGxDialog.AllowMultiSelect = False
        Catch ex As Exception
            OrmapExtension.ProcessUnhandledException(ex)
        End Try
    End Sub

#End Region

#Region "Custom class members"

    ''' <summary>
    ''' The dialog name.
    ''' </summary>
    Public Property Name() As String
        Get
            Name = _theGxDialog.Name
        End Get
        Set(ByVal value As String)
            _theGxDialog.Name = value
        End Set
    End Property

    ''' <summary>
    ''' The file path present when the user specifies a file to open
    ''' or a file name to save in Open/Save As dialog boxes.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property FinalLocation() As String
        Get
            FinalLocation = _theGxDialog.FinalLocation.FullName
        End Get
    End Property

    ''' <summary>
    ''' Constant storing the value returned when there is no selected element (-1).
    ''' </summary>
    Public Const NoSelectedElementIndex As Integer = -1

    ''' <summary>
    ''' Retrieve an item selected from a file dialog box.
    ''' </summary>
    ''' <returns>A file object.</returns>
    ''' <remarks>Return the nth selected element from the most recent file save/open dialog request.</remarks>
    Public Overloads Function SelectedObject() As Object
        Return SelectedObject(NoSelectedElementIndex)
    End Function

    ''' <summary>
    ''' Retrieve an item selected from a file dialog box.
    ''' </summary>
    ''' <param name="selection">Preselect a file from the list.</param>
    ''' <returns>A file object.</returns>
    ''' <remarks>Return the nth selected element from the most recent file save/open dialog request.</remarks>
    Public Overloads Function SelectedObject(ByVal selection As Integer) As Object
        If selection >= _selectionList.Count OrElse selection = NoSelectedElementIndex Then
            Return Nothing
        Else
            Return _selectionList.Item(selection)
        End If
    End Function

    Public Sub SetAllowMultiSelect(ByVal allow As Boolean)
        _theGxDialog.AllowMultiSelect = allow
    End Sub

    Public Sub SetButtonCaption(ByVal caption As String)
        _theGxDialog.ButtonCaption = caption
    End Sub


    ''' <summary>
    ''' Set the initial file path for either open or save dialog boxes.
    ''' </summary>
    ''' <param name="location"></param>
    ''' <remarks></remarks>
    Public Sub SetStartingLocation(ByVal location As System.IntPtr)
        _theGxDialog.StartingLocation = location
    End Sub

    Public Sub SetTitle(ByVal title As String)
        _theGxDialog.Title = title
    End Sub

    ''' <summary>
    ''' Simplify adding a filter to the file dialog box.
    ''' </summary>
    ''' <param name="filter">An ArcObject defined object filter.</param>
    ''' <returns><c>True</c> if successful;<c>False</c> if error.</returns>
    ''' <remarks>Adds a ESRI ArcCatalog defined filter to a file dialog box filter list.</remarks>
    Public Overloads Function SetFilter(ByVal filter As IGxObjectFilter) As Boolean
        Return SetFilter(filter, False, True)
    End Function

    ''' <summary>
    ''' Simplify adding a filter to the file dialog box.
    ''' </summary>
    ''' <param name="filter">An ArcObject defined object filter.</param>
    ''' <param name="isDefault">Indicates if the filter should be the default filter.</param>
    ''' <returns><c>True</c> if successful;<c>False</c> if error.</returns>
    ''' <remarks>Adds a ESRI ArcCatalog defined filter to a file dialog box filter list.</remarks>
    Public Overloads Function SetFilter(ByVal filter As IGxObjectFilter, ByVal isDefault As Boolean) As Boolean
        Return SetFilter(filter, isDefault, True)
    End Function

    ''' <summary>
    ''' Simplify adding a filter to the file dialog box.
    ''' </summary>
    ''' <param name="filter">An ArcObject defined object filter.</param>
    ''' <param name="isDefault">Indicates if the filter should be the default filter.</param>
    ''' <param name="resetAll">Indicates whether or not all of the current filters should be cleared.</param>
    ''' <returns><c>True</c> if successful;<c>False</c> if error.</returns>
    ''' <remarks>Adds a ESRI ArcCatalog defined filter to a file dialog box filter list.</remarks>
    Public Overloads Function SetFilter(ByVal filter As IGxObjectFilter, ByVal isDefault As Boolean, ByVal resetAll As Boolean) As Boolean
        Try
            Dim filters As IGxObjectFilterCollection
            filters = DirectCast(_theGxDialog, IGxObjectFilterCollection)
            If resetAll Then
                filters.RemoveAllFilters()
            End If
            filters.AddFilter(filter, isDefault)
            Return True
        Catch ex As Exception
            OrmapExtension.ProcessUnhandledException(ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Show the ArcCatalog file open dialog box.
    ''' </summary>
    ''' <returns>  A collection of names of objects that have been selected by the user from the dialog box</returns>
    ''' <remarks></remarks>
    Public Function ShowOpen() As ArrayList
        Try
            Dim selection As IEnumGxObject
            Dim thisSelectedObject As IGxObject
            selection = New GxObjectArray

            _selectionList = New ArrayList

            If Not _theGxDialog.DoModalOpen(My.ArcMap.Application.hWnd, selection) Then
                'need to return a empty collection
                Return New ArrayList
            End If

            selection.Reset()
            thisSelectedObject = selection.Next
            Do While Not thisSelectedObject Is Nothing
                _selectionList.Add(thisSelectedObject.FullName)
                thisSelectedObject = selection.Next
            Loop
            Return _selectionList
        Catch ex As Exception
            OrmapExtension.ProcessUnhandledException(ex)
            Return New ArrayList
        End Try
    End Function

    ''' <summary>
    ''' Show the ArcCatalog file save dialog box.
    ''' </summary>
    ''' <returns>A collection holding the full path that is a concatenation of the final path and the specified name</returns>
    ''' <remarks></remarks>
    Public Function ShowSave() As ArrayList
        Try
            If Not _theGxDialog.DoModalSave(My.ArcMap.Application.hWnd) Then
                ' Return an empty collection
                Return New ArrayList
            End If
            Dim selectedObject As IGxObject
            selectedObject = _theGxDialog.FinalLocation
            _selectionList.Add(String.Concat(selectedObject.FullName, "\", _theGxDialog.Name))
            Return _selectionList
        Catch ex As Exception
            OrmapExtension.ProcessUnhandledException(ex)
            Return New ArrayList
        End Try
    End Function

#End Region

End Class
#End Region
