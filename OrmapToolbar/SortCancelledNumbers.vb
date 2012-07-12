#Region "Copyright 2011 ORMAP Tech Group"

' File:  SortCancelledNumbers.vb
'
' Original Author:  Shad Campbell
'
' Date Created:  September 6, 20011
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
'Tag for this file: $Name$
'SCC revision number: $Revision: 437 $
'Date of Last Change: $Date: 2010-05-12 18:22:16 -0700 (Wed, 12 May 2010) $
#End Region

#Region "Imported Namespaces"
Imports ESRI.ArcGIS.Editor
Imports System.Windows.Forms
Imports ESRI.ArcGIS.Framework
Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Geodatabase
Imports Microsoft.VisualBasic.Interaction
Imports OrmapTaxlotEditing.DataMonitor
Imports OrmapTaxlotEditing.SpatialUtilities
Imports OrmapTaxlotEditing.Utilities
Imports ESRI.ArcGIS.Desktop.AddIns
#End Region

Public Class SortCancelledNumbers
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button


#Region "Built-In Class Members (Constructors, Etc.)"

#Region "Constructors"
    Public Sub New()
    End Sub
#End Region

#End Region

#Region "Inherited Class Members"

#Region "Properties"
#End Region

#Region "Methods"

    ''' <summary>
    ''' Event Hanlder
    ''' </summary>
    ''' <remarks>Handles on click event of button</remarks>
    Protected Overrides Sub OnClick()

        Try
            '-- Make sure this is not null
            If CancelledNumbersTable Is Nothing Then CheckValidCancelledNumbersTableDataProperties()

            If CancelledNumbersTable.Table.FindField("SortOrder") = NotFoundIndex Then
                MessageBox.Show("This tool requires a field named 'SortOrder' in your cancelled numbers table.  To start using this tool, please add this field and calculate all features SortOrder value equal to the ObjectId field.  IMPORTANT: Be sure to set your Map Production tools to use the SortOrder field also.", "Missing Field", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            PartnerSortCancelledNumbersForm.ShowDialog()

        Catch ex As Exception
            OrmapExtension.ProcessUnhandledException(ex)
        End Try

    End Sub

    ''' <summary>
    ''' Event Handler 
    ''' </summary>
    ''' <remarks>WARNING: Do not put computation-intensive code here. Called by ArcMap once per second to check if the command is enabled.</remarks>
    Protected Overrides Sub OnUpdate()
        Enabled = My.ArcMap.Editor.EditState
    End Sub

#End Region

#End Region

#Region "Custom Class Members"

#Region "Fields (none)"
#End Region

#Region "Properties"

    Private WithEvents _partnerSortCancelledNumbersForm As SortCancelledNumbersForm

    Friend ReadOnly Property PartnerSortCancelledNumbersForm() As SortCancelledNumbersForm
        Get
            If _partnerSortCancelledNumbersForm Is Nothing OrElse _partnerSortCancelledNumbersForm.IsDisposed Then
                setPartnerSortCancelledNumbersForm(New SortCancelledNumbersForm())
            End If
            Return _partnerSortCancelledNumbersForm
        End Get
    End Property

    Private Sub setPartnerSortCancelledNumbersForm(ByVal value As SortCancelledNumbersForm)
        If value IsNot Nothing Then
            _partnerSortCancelledNumbersForm = value
            ' Subscribe to partner form events.
            AddHandler _partnerSortCancelledNumbersForm.Load, AddressOf PartnerSortCancelledNumbersForm_Load
            AddHandler _partnerSortCancelledNumbersForm.uxTop.Click, AddressOf uxTop_Click
            AddHandler _partnerSortCancelledNumbersForm.uxUp.Click, AddressOf uxUp_Click
            AddHandler _partnerSortCancelledNumbersForm.uxDown.Click, AddressOf uxDown_Click
            AddHandler _partnerSortCancelledNumbersForm.uxBottom.Click, AddressOf uxBottom_Click
            AddHandler _partnerSortCancelledNumbersForm.uxCancel.Click, AddressOf uxCancel_Click
            AddHandler _partnerSortCancelledNumbersForm.uxOK.Click, AddressOf uxOK_Click
            AddHandler _partnerSortCancelledNumbersForm.uxAdd.Click, AddressOf uxAdd_Click
            AddHandler _partnerSortCancelledNumbersForm.uxDelete.Click, AddressOf uxDelete_Click
            AddHandler _partnerSortCancelledNumbersForm.uxCancelledNumbers.SelectedIndexChanged, AddressOf uxCancelledNumbers_SelectedIndexChanged
            AddHandler _partnerSortCancelledNumbersForm.uxFind.Click, AddressOf uxFind_Click
            AddHandler _partnerSortCancelledNumbersForm.uxMapIndex.TextChanged, AddressOf uxMapNumber_TextChanged
            AddHandler _partnerSortCancelledNumbersForm.uxHelp.Click, AddressOf uxHelp_Click



            AddHandler _partnerSortCancelledNumbersForm.uxMapIndex.KeyDown, AddressOf uxMapIndex_KeyDown



        Else
            ' Unsubscribe to partner form events.
            RemoveHandler _partnerSortCancelledNumbersForm.Load, AddressOf PartnerSortCancelledNumbersForm_Load
            RemoveHandler _partnerSortCancelledNumbersForm.uxTop.Click, AddressOf uxTop_Click
            RemoveHandler _partnerSortCancelledNumbersForm.uxUp.Click, AddressOf uxUp_Click
            RemoveHandler _partnerSortCancelledNumbersForm.uxDown.Click, AddressOf uxDown_Click
            RemoveHandler _partnerSortCancelledNumbersForm.uxBottom.Click, AddressOf uxBottom_Click
            RemoveHandler _partnerSortCancelledNumbersForm.uxCancel.Click, AddressOf uxCancel_Click
            RemoveHandler _partnerSortCancelledNumbersForm.uxOK.Click, AddressOf uxOK_Click
            RemoveHandler _partnerSortCancelledNumbersForm.uxAdd.Click, AddressOf uxAdd_Click
            RemoveHandler _partnerSortCancelledNumbersForm.uxDelete.Click, AddressOf uxDelete_Click
            RemoveHandler _partnerSortCancelledNumbersForm.uxCancelledNumbers.SelectedIndexChanged, AddressOf uxCancelledNumbers_SelectedIndexChanged
            RemoveHandler _partnerSortCancelledNumbersForm.uxFind.Click, AddressOf uxFind_Click
            RemoveHandler _partnerSortCancelledNumbersForm.uxMapIndex.TextChanged, AddressOf uxMapNumber_TextChanged
            RemoveHandler _partnerSortCancelledNumbersForm.uxHelp.Click, AddressOf uxHelp_Click
        End If
    End Sub
#End Region

#Region "Event Handlers"

    ''' <summary>
    ''' Event Handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Handles form load</remarks>
    Private Sub PartnerSortCancelledNumbersForm_Load(ByVal sender As Object, ByVal e As System.EventArgs)

        With PartnerSortCancelledNumbersForm
            Try
                .UseWaitCursor = True

                .uxMapIndex.AutoCompleteCustomSource.Clear()
                .uxMapIndex.AutoCompleteCustomSource.AddRange(GetMapNumberArray)

                '-- This code will the the MapNumber from the locate features dialog and use it as a starting point.  Not sure if this will be implemented or not... need input of users.
                'Dim LocateFeatureDockWindowUI As LocateFeatureDockWindow = AddIn.FromID(Of LocateFeatureDockWindow.AddinImpl)(My.ThisAddIn.IDs.LocateFeatureDockWindow).UI
                'If Not LocateFeatureDockWindowUI.uxMapNumber.Text = String.Empty Then
                '.uxMapIndex.Text = LocateFeatureDockWindowUI.uxMapNumber.Text
                'End If

            Finally
                .UseWaitCursor = False
            End Try

        End With

    End Sub

    ''' <summary>
    ''' Event Handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Handles uxMapnumber.TextChanged</remarks>
    Private Sub uxMapNumber_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim uxMapNumber As TextBox = PartnerSortCancelledNumbersForm.uxMapIndex

        If Not uxMapNumber.AutoCompleteCustomSource.Contains(uxMapNumber.Text.Trim) Then
            If uxMapNumber.AutoCompleteCustomSource.Contains(uxMapNumber.Text.ToLower.Trim) Then
                uxMapNumber.Text = uxMapNumber.Text.ToLower.Trim
                uxMapNumber.SelectionStart = uxMapNumber.Text.Length
            End If
            If uxMapNumber.AutoCompleteCustomSource.Contains(uxMapNumber.Text.ToUpper.Trim) Then
                uxMapNumber.Text = uxMapNumber.Text.ToUpper.Trim
                uxMapNumber.SelectionStart = uxMapNumber.Text.Length
            End If
        End If

    End Sub

    ''' <summary>
    ''' Event Handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Handles uxFind.Click</remarks>
    Private Sub uxFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        With PartnerSortCancelledNumbersForm
            Try

                If .uxMapIndex.Text = String.Empty OrElse Not .uxMapIndex.AutoCompleteCustomSource.Contains(.uxMapIndex.Text) Then
                    MessageBox.Show("Invalid MapNumber. Please try again.", "Locate Feature", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Exit Sub
                End If

                .UseWaitCursor = True
                .uxCancelledNumbers.Items.Clear()

                If .uxCancelledNumbers.SelectedIndex = -1 Then .uxDelete.Enabled = False

                Dim theQueryFilter As IQueryFilter = New QueryFilter
                theQueryFilter.WhereClause = formatWhereClause("[MapNumber] = '" & .uxMapIndex.Text & "'", CancelledNumbersTable.Table)

                Dim theTableSort As ITableSort = New TableSort
                With theTableSort
                    .Fields = "SortOrder"
                    .Ascending("SortOrder") = True
                    .QueryFilter = theQueryFilter
                    .Table = CancelledNumbersTable.Table
                End With

                theTableSort.Sort(Nothing)

                Dim theCursor As ICursor = theTableSort.Rows
                Dim theRow As IRow = theCursor.NextRow
                While Not theRow Is Nothing
                    If IsDBNull(theRow.Value(2)) Then
                        .uxCancelledNumbers.Items.Add("Null")
                    Else
                        .uxCancelledNumbers.Items.Add(theRow.Value(2))
                    End If
                    theRow = theCursor.NextRow
                End While

            Catch ex As Exception
                OrmapExtension.ProcessUnhandledException(ex)

            Finally
                .UseWaitCursor = False
            End Try

        End With

    End Sub

    ''' <summary>
    ''' Event Handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Handles uxCancelledNumbers.SelectedIndexChanged</remarks>
    Private Sub uxCancelledNumbers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

        With PartnerSortCancelledNumbersForm
            If .uxCancelledNumbers.SelectedIndex = 0 Then
                .uxUp.Enabled = False
                .uxTop.Enabled = False
            Else
                .uxUp.Enabled = True
                .uxTop.Enabled = True
            End If

            If .uxCancelledNumbers.SelectedIndex = .uxCancelledNumbers.Items.Count - 1 Then
                .uxDown.Enabled = False
                .uxBottom.Enabled = False
            Else
                .uxDown.Enabled = True
                .uxBottom.Enabled = True
            End If

            .uxDelete.Enabled = True

        End With

    End Sub

    ''' <summary>
    ''' Event Handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Handles uxMapIndex.KeyDown</remarks>
    Private Sub uxMapIndex_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Enter Then PartnerSortCancelledNumbersForm.uxFind.PerformClick()
    End Sub


    ''' <summary>
    ''' Event Handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Handles uxTop.Click</remarks>
    Private Sub uxTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MoveItems("++")
    End Sub

    ''' <summary>
    ''' Event Handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Handles uxUp.Click</remarks>
    Private Sub uxUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MoveItems("+")
    End Sub

    ''' <summary>
    ''' Event Handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Handles uxDown.Click </remarks>
    Private Sub uxDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MoveItems("-")
    End Sub

    ''' <summary>
    ''' Event Handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Handles uxBottom.Click</remarks>
    Private Sub uxBottom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MoveItems("--")
    End Sub

    ''' <summary>
    ''' Event Handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Handles uxCancel.Click</remarks>
    Private Sub uxCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        PartnerSortCancelledNumbersForm.Hide()
    End Sub

    ''' <summary>
    ''' Event Handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Hnadles uxOK.Click</remarks>
    Private Sub uxOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        '-- Start edit operation (must start edit operation due to versioning)
        Dim theDataset As IDataset = CancelledNumbersTable.Table
        Dim theWorkspaceEdit As IWorkspaceEdit = theDataset.Workspace
        theWorkspaceEdit.StartEditOperation()

        Dim theQueryFilter As IQueryFilter = New QueryFilter
        theQueryFilter.WhereClause = formatWhereClause("[MapNumber] = '" & PartnerSortCancelledNumbersForm.uxMapIndex.Text & "'", CancelledNumbersTable.Table)

        Dim theCursor As ICursor = CancelledNumbersTable.Table.Update(theQueryFilter, False)
        Dim theRow As IRow = theCursor.NextRow
        While Not theRow Is Nothing
            theRow.Value(3) = PartnerSortCancelledNumbersForm.uxCancelledNumbers.FindString(theRow.Value(2))
            theRow.Store()
            theRow = theCursor.NextRow
        End While

        PartnerSortCancelledNumbersForm.Hide()

        '-- Stop edit operation
        theWorkspaceEdit.StopEditOperation()


    End Sub

    ''' <summary>
    ''' Event Handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Handles uxDelete.Click</remarks>
    Private Sub uxDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        With PartnerSortCancelledNumbersForm
            Try
                If MessageBox.Show("Are you sure you want to delete this?", "Verify Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Exit Sub

                '-- Start edit operation (must start edit operation due to versioning)
                Dim theDataset As IDataset = CancelledNumbersTable.Table
                Dim theWorkspaceEdit As IWorkspaceEdit = theDataset.Workspace
                theWorkspaceEdit.StartEditOperation()

                Dim theQueryFilter As IQueryFilter = New QueryFilter
                If .uxCancelledNumbers.Items(.uxCancelledNumbers.SelectedIndex).ToString() = "Null" Then
                    theQueryFilter.WhereClause = formatWhereClause("MapNumber = '" & .uxMapIndex.Text & "' AND Taxlot IS NULL", CancelledNumbersTable.Table)
                Else
                    theQueryFilter.WhereClause = formatWhereClause("MapNumber = '" & .uxMapIndex.Text & "' AND Taxlot = '" & .uxCancelledNumbers.Items(.uxCancelledNumbers.SelectedIndex).ToString & "'", CancelledNumbersTable.Table)
                End If

                Dim theCursor As ICursor = CancelledNumbersTable.Table.Search(theQueryFilter, False)

                Dim theRow As IRow = theCursor.NextRow
                If Not theRow Is Nothing Then theRow.Delete()

                '-- Stop edit operation
                theWorkspaceEdit.StopEditOperation()

                .uxCancelledNumbers.Items.Remove(.uxCancelledNumbers.Items(.uxCancelledNumbers.SelectedIndex).ToString)
                .uxDelete.Enabled = False

            Catch ex As Exception
                OrmapExtension.ProcessUnhandledException(ex)
            End Try

        End With

    End Sub

    ''' <summary>
    ''' Event Handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Handles uxAdd.Click</remarks>
    Private Sub uxAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim theInputDialog As New InputDialog
        theInputDialog.uxLabel.Text = "Enter the Cancelled Number:"
        theInputDialog.Text = "Add Cancelled Number"
        theInputDialog.Message = "Please enter a Cancelled Number"

        If theInputDialog.ShowDialog = DialogResult.Cancel Then Exit Sub

        With PartnerSortCancelledNumbersForm

            If .uxCancelledNumbers.Items.Contains(theInputDialog.Value) Then
                MessageBox.Show("Duplicate Entry.  Please try again", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            '-- Start edit operation (must start edit operation due to versioning)
            Dim theDataset As IDataset = CancelledNumbersTable.Table
            Dim theWorkspaceEdit As IWorkspaceEdit = theDataset.Workspace
            theWorkspaceEdit.StartEditOperation()

            '-- Add the item to the Cancelled numbers table
            Dim theCancelledNumbersTable As ITable = CancelledNumbersTable.Table
            Dim theCursor As ICursor = theCancelledNumbersTable.Insert(True)
            Dim theRowBuffer As IRowBuffer = theCancelledNumbersTable.CreateRowBuffer
            theRowBuffer.Value(theRowBuffer.Fields.FindField("MapNumber")) = .uxMapIndex.Text
            theRowBuffer.Value(theRowBuffer.Fields.FindField("Taxlot")) = theInputDialog.Value
            theRowBuffer.Value(theRowBuffer.Fields.FindField("SortOrder")) = .uxCancelledNumbers.Items.Count
            theCursor.InsertRow(theRowBuffer)
            theCursor.Flush()

            '-- Stop edit operation
            theWorkspaceEdit.StopEditOperation()

            .uxCancelledNumbers.Items.Insert(.uxCancelledNumbers.Items.Count, theInputDialog.Value)

        End With

    End Sub

    Private Sub uxHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' TODO: [NIS] Could be replaced with new help mechanism.

        Dim theRTFStream As System.IO.Stream = _
           Me.GetType().Assembly.GetManifestResourceStream("OrmapTaxlotEditing.SortCancelledNumbers_help.rtf")
        OpenHelp("Sort Cancelled Numbers Help", theRTFStream)

        ' Get the help form.
        'Dim theHelpForm As New HelpForm
        'theHelpForm.Text = "Edit Map Index Help"

        ' KLUDGE: [NIS] Remove comments if file is ready.
        '' Open a custom help text file.
        '' Note: Requires a specific file in the help subdirectory of the application directory.
        'Dim theTextFilePath As String
        'theTextFilePath = My.Application.Info.DirectoryPath & "\help\EditMapIndexHelp.rtf"
        'If Microsoft.VisualBasic.FileIO.FileSystem.FileExists(theTextFilePath) Then
        '    theHelpForm.RichTextBox1.LoadFile(theTextFilePath, RichTextBoxStreamType.RichText)
        'Else
        '    MessageBox.Show("No help file available in the directory " & NewLine & _
        '            My.Application.Info.DirectoryPath & "\help" & ".")
        '    theHelpForm.TabPage1.Hide()
        'End If

        ' KLUDGE: [NIS] Remove comments if file is ready.
        ' Open a custom help pdf file.
        ' Note: Requires a specific file in the help subdirectory of the application directory.
        ' Requires Adobe Acrobat reader plug-in.
        'Dim thePdfFilePath As String
        'thePdfFilePath = My.Application.Info.DirectoryPath & "\help\EditMapIndexHelp.pdf"
        'If Microsoft.VisualBasic.FileIO.FileSystem.FileExists(thePdfFilePath) Then
        '    Dim theUri As New System.Uri("file:///" & thePdfFilePath)
        '    theHelpForm.WebBrowser1.Url = theUri
        'Else
        '    MessageBox.Show("No help file available in the directory " & NewLine & _
        '            My.Application.Info.DirectoryPath & "\help" & ".")
        '    theHelpForm.TabPage2.Hide()
        'End If

        ' KLUDGE: [NIS] Remove comments if file is ready.
        '' Open a custom help video.
        '' Note: Requires a specific file in the help\videos subdirectory of the application directory.
        'Dim theVideoFilePath As String
        'theVideoFilePath = My.Application.Info.DirectoryPath & "\help\videos\EditMapIndex\EditMapIndex.html"
        'If Microsoft.VisualBasic.FileIO.FileSystem.FileExists(theVideoFilePath) Then
        '    Dim theUri As New System.Uri("file:///" & theVideoFilePath)
        '    theHelpForm.WebBrowser1.Url = theUri
        'Else
        '    MessageBox.Show("No help file available in the directory " & NewLine & _
        '            My.Application.Info.DirectoryPath & "\help\videos\EditMapIndex" & ".")
        '    theHelpForm.TabPage2.Hide()
        'End If

        ' KLUDGE: [NIS] Remove comments if form will be used.
        'theHelpForm.Width = 668
        'theHelpForm.Height = 400
        'theHelpForm.Show()

    End Sub

#End Region

#Region "Methods"

    ''' <summary>
    ''' Moves taxlot numbers up and down in the cancelled numbers listbox
    ''' </summary>
    ''' <param name="Direction">The direction to move the taxlot</param>
    ''' <remarks></remarks>
    Private Sub MoveItems(ByVal Direction As String) 'pass -1 for moveup and + 1 for move down

        With PartnerSortCancelledNumbersForm.uxCancelledNumbers

            Dim currentIndex As Integer = .SelectedIndex
            Dim currentText As String = .Items(.SelectedIndex).ToString

            Dim newIndex As Integer = 0
            Select Case Direction
                Case "+"
                    newIndex = currentIndex - 1
                Case "-"
                    newIndex = currentIndex + 1
                Case "++"
                    newIndex = 0
                Case "--"
                    newIndex = .Items.Count - 1
            End Select

            .Items.Remove(currentText)
            .Items.Insert(newIndex, currentText)
            .SelectedIndex = newIndex

        End With
    End Sub

#End Region

#End Region






End Class
