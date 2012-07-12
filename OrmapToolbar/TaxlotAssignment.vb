#Region "Copyright 2008-2011 ORMAP Tech Group"

' File:  TaxlotAssignment.vb
'
' Original Author:  OPET.NET Migration Team (Shad Campbell, James Moore, 
'                   Nick Seigal)
'
' Date Created:  January 8, 2008
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
'SCC revision number: $Revision: 406 $
'Date of Last Change: $Date: 2009-11-30 22:49:20 -0800 (Mon, 30 Nov 2009) $
#End Region

#Region "Imported Namespaces"
Imports System.Drawing
Imports System.Environment
Imports System.Globalization
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Editor
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Framework
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Geometry
Imports OrmapTaxlotEditing.DataMonitor
Imports OrmapTaxlotEditing.SpatialUtilities
Imports OrmapTaxlotEditing.StringUtilities
Imports OrmapTaxlotEditing.Utilities
#End Region


Public Class TaxlotAssignment
    Inherits ESRI.ArcGIS.Desktop.AddIns.Tool

#Region "Class-Level Constants and Enumerations"

    ' Taxlot number type constants
    ' NOTE: these must be exactly 5 characters long
    Private Const taxlotNumberTypeTaxlot As String = "TAXLOT" 'normal taxlot number
    Private Const taxlotNumberTypeRoads As String = "ROADS"
    Private Const taxlotNumberTypeWater As String = "WATER"
    Private Const taxlotNumberTypeRails As String = "RAILS"
    Private Const taxlotNumberTypeNontaxlot As String = "NONTL"

    Private Const defaultCommand As String = "esriArcMapUI.SelectTool"

#End Region

#Region "Built-In Class Members (Constructors, Etc.)"

#Region "Constructors"

    Public Sub New()

        Try
            MyBase.Cursor = Cursors.Cross
        Catch ex As ArgumentException
            OrmapExtension.ProcessUnhandledException(ex)
        End Try

    End Sub

#End Region

#End Region

#Region "Custom Class Members"

#Region "Fields (none)"
#End Region

#Region "Properties"

    Private _incrementNumber As Integer

    Public ReadOnly Property IncrementNumber() As Integer
        Get
            Select Case True
                Case PartnerTaxlotAssignmentForm.uxIncrementByNone.Checked
                    _incrementNumber = 0
                Case PartnerTaxlotAssignmentForm.uxIncrementBy1.Checked
                    _incrementNumber = 1
                Case PartnerTaxlotAssignmentForm.uxIncrementBy10.Checked
                    _incrementNumber = 10
                Case PartnerTaxlotAssignmentForm.uxIncrementBy100.Checked
                    _incrementNumber = 100
                Case PartnerTaxlotAssignmentForm.uxIncrementBy1000.Checked
                    _incrementNumber = 1000
                Case Else
                    _incrementNumber = 0
            End Select
            Return _incrementNumber
        End Get
    End Property

    Private _taxlotType As String

    Public ReadOnly Property TaxlotType() As String
        Get
            _taxlotType = PartnerTaxlotAssignmentForm.uxType.SelectedItem.ToString
            Return _taxlotType
        End Get
    End Property

    Private _numberStartingFrom As Integer

    Public Property NumberStartingFrom() As Integer
        Get
            _numberStartingFrom = CInt(PartnerTaxlotAssignmentForm.uxStartingFrom.Text)
            Return _numberStartingFrom
        End Get
        Set(ByVal value As Integer)
            _numberStartingFrom = value
            PartnerTaxlotAssignmentForm.uxStartingFrom.Text = CStr(_numberStartingFrom)
        End Set
    End Property

    Private WithEvents _partnerTaxlotAssignmentForm As TaxlotAssignmentForm

    Friend ReadOnly Property PartnerTaxlotAssignmentForm() As TaxlotAssignmentForm
        Get
            If _partnerTaxlotAssignmentForm Is Nothing OrElse _partnerTaxlotAssignmentForm.IsDisposed Then
                setPartnerTaxlotAssignmentForm(New TaxlotAssignmentForm())
            End If
            Return _partnerTaxlotAssignmentForm
        End Get
    End Property

    Private Sub setPartnerTaxlotAssignmentForm(ByVal value As TaxlotAssignmentForm)
        If value IsNot Nothing Then
            _partnerTaxlotAssignmentForm = value
            ' Subscribe to partner form events.
            AddHandler _partnerTaxlotAssignmentForm.Load, AddressOf PartnerTaxlotAssignmentForm_Load
            AddHandler _partnerTaxlotAssignmentForm.uxHelp.Click, AddressOf uxHelp_Click
            AddHandler _partnerTaxlotAssignmentForm.uxType.SelectedValueChanged, AddressOf uxType_SelectedValueChanged
            AddHandler _partnerTaxlotAssignmentForm.FormClosed, AddressOf PartnerTaxlotAssignmentForm_Close
        Else
            ' Unsubscribe to partner form events.
            RemoveHandler _partnerTaxlotAssignmentForm.Load, AddressOf PartnerTaxlotAssignmentForm_Load
            RemoveHandler _partnerTaxlotAssignmentForm.uxHelp.Click, AddressOf uxHelp_Click
            RemoveHandler _partnerTaxlotAssignmentForm.uxType.SelectedValueChanged, AddressOf uxType_SelectedValueChanged
            RemoveHandler _partnerTaxlotAssignmentForm.FormClosed, AddressOf PartnerTaxlotAssignmentForm_Close
        End If
    End Sub

#End Region

#Region "Event Handlers"

    Private Sub PartnerTaxlotAssignmentForm_Close(ByVal sender As Object, ByVal e As System.EventArgs)
        My.ArcMap.Application.CurrentTool = Nothing
    End Sub


    Private Sub PartnerTaxlotAssignmentForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles PartnerTaxlotAssignmentForm.Load

        With PartnerTaxlotAssignmentForm

            'Populate multi-value controls
            .uxType.Items.Add(taxlotNumberTypeTaxlot)
            .uxType.Items.Add(taxlotNumberTypeRoads)
            .uxType.Items.Add(taxlotNumberTypeWater)
            .uxType.Items.Add(taxlotNumberTypeRails)
            .uxType.Items.Add(taxlotNumberTypeNontaxlot)

            ' Set control defaults
            .uxType.Text = taxlotNumberTypeTaxlot
            .uxIncrementByNone.Checked = True
            .uxStartingFrom.Text = "0"

            ' Enable the numbering settings controls by enabling the group
            .uxTaxlotNumberingOptions.Enabled = True
            'With .uxStartingFrom
            '    '.BackColor = System.Drawing.Color.White
            '    .Enabled = True
            'End With
            '.uxIncrementByNone.Enabled = True
            '.uxIncrementBy1.Enabled = True
            '.uxIncrementBy10.Enabled = True
            '.uxIncrementBy100.Enabled = True
            '.uxIncrementBy1000.Enabled = True
        End With


    End Sub

    Private Sub uxHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) 'Handles PartnerTaxlotAssignmentForm.uxHelp.Click
        ' TODO: [NIS] Could be replaced with new help mechanism.

        Dim theRTFStream As System.IO.Stream = _
            Me.GetType().Assembly.GetManifestResourceStream("OrmapTaxlotEditing.TaxlotAssignment_help.rtf")
        OpenHelp("Taxlot Assignment Help", theRTFStream)

        '' Open a custom help text file.
        '' Note: Requires a specific file in the help subdirectory of the application directory.
        'Dim theTextFilePath As String
        'theTextFilePath = My.Application.Info.DirectoryPath & "\help\TaxlotAssignmentHelp.rtf"
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
        'thePdfFilePath = My.Application.Info.DirectoryPath & "\help\TaxlotAssignmentHelp.pdf"
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
        'theVideoFilePath = My.Application.Info.DirectoryPath & "\help\videos\TaxlotAssignment\TaxlotAssignment.html"
        'If Microsoft.VisualBasic.FileIO.FileSystem.FileExists(theVideoFilePath) Then
        '    Dim theUri As New System.Uri("file:///" & theVideoFilePath)
        '    theHelpForm.WebBrowser1.Url = theUri
        'Else
        '    MessageBox.Show("No help file available in the directory " & NewLine & _
        '            My.Application.Info.DirectoryPath & "\help\videos\TaxlotAssignment" & ".")
        '    theHelpForm.TabPage2.Hide()
        'End If

        ' KLUDGE: [NIS] Remove comments if form will be used.
        'theHelpForm.Width = 668
        'theHelpForm.Height = 400
        'theHelpForm.Show()

    End Sub


    Private Sub uxType_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles TaxlotAssignmentForm.uxType.SelectedValueChanged
        With PartnerTaxlotAssignmentForm
            Const NoSelectedIndex As Integer = -1
            If .uxType.SelectedIndex <> NoSelectedIndex Then
                If .uxType.SelectedItem.ToString = taxlotNumberTypeTaxlot Then
                    ' Enable the numbering settings controls by enabling the group
                    .uxTaxlotNumberingOptions.Enabled = True
                Else
                    ' Disable the numbering settings controls by disabling the group
                    .uxTaxlotNumberingOptions.Enabled = False
                End If
            End If
        End With

    End Sub

#End Region

#Region "Methods"

    Friend Sub DoToolOperation(ByVal Button As MouseButtons, ByVal X As Integer, ByVal Y As Integer)

        Dim withinEditOperation As Boolean = False

        Try
            If (Button <> MouseButtons.Left) Then
                ' Exit silently.
                Exit Sub
            End If

            ' Check for valid data
            CheckValidTaxlotDataProperties()
            If Not HasValidTaxlotData Then
                MessageBox.Show("Unable to assign taxlot values to polygons." & NewLine & _
                                "Missing data: Valid ORMAP Taxlot layer not found in the map." & NewLine & _
                                "Please load this dataset into your map.", _
                                "Taxlot Assignment", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                Exit Sub
            End If
            CheckValidMapIndexDataProperties()
            If Not HasValidMapIndexData Then
                MessageBox.Show("Unable to assign taxlot values to polygons." & NewLine & _
                                "Missing data: Valid ORMAP MapIndex layer not found in the map." & NewLine & _
                                "Please load this dataset into your map.", _
                                "Taxlot Assignment", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                Exit Sub
            End If
            CheckValidCancelledNumbersTableDataProperties()
            If Not HasValidCancelledNumbersTableData Then
                MessageBox.Show("Unable to assign taxlot values to polygons." & NewLine & _
                                "Missing data: Valid ORMAP CancelledNumbersTable not found in the map." & NewLine & _
                                "Please load this dataset into your map.", _
                                "Taxlot Assignment", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                Exit Sub
            End If

            ' If taxlot numbering is selected, then make sure value is numeric.
            Dim isTaxlotType As Boolean = (StrComp(Me.TaxlotType, TaxlotAssignment.taxlotNumberTypeTaxlot, CompareMethod.Text) = 0)
            If isTaxlotType Then
                If Not IsNumeric(Me.NumberStartingFrom) Then
                    Throw New InvalidOperationException(String.Format(CultureInfo.CurrentCulture, "Expected a number for {0}, got {1}.", "NumberStartingFrom", Me.NumberStartingFrom)) ' TODO: [NIS] Find a better exception.
                End If
            End If

            ' Create a search shape out of the point that the user clicked.
            Dim thePoint As IPoint = Nothing
            Dim theGeometry As IGeometry = Nothing

            thePoint = My.ArcMap.Editor.Display.DisplayTransformation.ToMapPoint(X, Y)
            theGeometry = thePoint 'QI

            ' Insure the presence of an underlying MapIndex polygon.
            Dim theSpatialFilter As ISpatialFilter
            Dim theShapeFieldName As String
            Dim theMIFCursor As IFeatureCursor
            Dim theMIFeature As IFeature

            theSpatialFilter = New ESRI.ArcGIS.Geodatabase.SpatialFilter
            theSpatialFilter.Geometry = theGeometry
            theShapeFieldName = DataMonitor.MapIndexFeatureLayer.FeatureClass.ShapeFieldName
            theSpatialFilter.OutputSpatialReference(theShapeFieldName) = My.ArcMap.Editor.Map.SpatialReference
            theSpatialFilter.GeometryField = DataMonitor.MapIndexFeatureLayer.FeatureClass.ShapeFieldName
            theSpatialFilter.SpatialRel = ESRI.ArcGIS.Geodatabase.esriSpatialRelEnum.esriSpatialRelIntersects
            theMIFCursor = DataMonitor.MapIndexFeatureLayer.FeatureClass.Search(theSpatialFilter, False)
            theMIFeature = theMIFCursor.NextFeature
            If theMIFeature Is Nothing Then
                MessageBox.Show("Unable to assign taxlot values to polygons" & NewLine & _
                                "that are not within a MapIndex polygon.", _
                                "Taxlot Assignment", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                Exit Sub
            End If

            '=====================================
            ' The Update Operation Starts Here...
            '=====================================

            My.ArcMap.Application.StatusBar.Message(esriStatusBarPanes.esriStatusMain) = "Updating Taxlot feature..."

            '------------------------------------------
            ' Get the taxlot feature to update.
            ' If found, start the feature update operation.
            '------------------------------------------
            Dim theTaxlotFCursor As IFeatureCursor = Nothing
            Dim theTaxlotFeature As IFeature = Nothing
            ' Select any feature under the given point in the target layer
            theSpatialFilter = New ESRI.ArcGIS.Geodatabase.SpatialFilter
            theSpatialFilter.Geometry = theGeometry
            theShapeFieldName = DataMonitor.TaxlotFeatureLayer.FeatureClass.ShapeFieldName
            theSpatialFilter.OutputSpatialReference(theShapeFieldName) = My.ArcMap.Editor.Map.SpatialReference
            theSpatialFilter.GeometryField = DataMonitor.TaxlotFeatureLayer.FeatureClass.ShapeFieldName
            theSpatialFilter.SpatialRel = ESRI.ArcGIS.Geodatabase.esriSpatialRelEnum.esriSpatialRelIntersects
            theTaxlotFCursor = DataMonitor.TaxlotFeatureLayer.FeatureClass.Update(theSpatialFilter, False)
            If theTaxlotFCursor IsNot Nothing Then
                theTaxlotFeature = theTaxlotFCursor.NextFeature
                ' Start the feature update operation
                If theTaxlotFeature IsNot Nothing Then
                    '[At least one taxlot feature is selected...]
                    My.ArcMap.Editor.StartOperation()
                    withinEditOperation = True
                Else
                    '[No taxlot features are selected...]
                    MessageBox.Show("No Taxlot features have been selected.", _
                                    "Taxlot Assignment", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                    Exit Sub
                End If
            End If

            '------------------------------------------
            ' Define the new Taxlot string value.
            '------------------------------------------
            Dim theExistingTaxlot As String = String.Empty 'initialize
            Dim theTLTaxlotFieldIndex As Integer = DataMonitor.TaxlotFeatureLayer.FeatureClass.FindField(OrmapExtension.TaxLotSettings.TaxlotField)
            theExistingTaxlot = CStr(IIf(IsDBNull(theTaxlotFeature.Value(theTLTaxlotFieldIndex)), String.Empty, theTaxlotFeature.Value(theTLTaxlotFieldIndex)))

            ' Check with user before updating Taxlot field
            If Len(theExistingTaxlot) > 0 AndAlso theExistingTaxlot <> "0" Then
                If MessageBox.Show("Taxlot currently has a Taxlot number (" & theExistingTaxlot & ")." & NewLine & _
                          "Update it?" & NewLine & NewLine & _
                          "NOTE: If the old number is unique in the map," & NewLine & _
                          "it will be added to the Cancelled Numbers table.", "Taxlot Assignment", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.OK Then
                    ' continue...
                Else
                    Exit Sub
                End If
            End If

            My.ArcMap.Application.StatusBar.Message(esriStatusBarPanes.esriStatusMain) = "Verifying Taxlot number uniqueness..."

            ' Verify the uniqueness of the specified taxlot number (if taxlot type input).
            If isTaxlotType Then
                '[Taxlot value is a number...]
                If Not IsTaxlotNumberLocallyUnique(CStr(Me.NumberStartingFrom), theGeometry, False) Then
                    If MessageBox.Show("The new Taxlot value (" & Me.NumberStartingFrom & ")" & NewLine & _
                                       "is not unique within this MapIndex." & NewLine & _
                                       "Attribute feature with value anyway?", _
                                       "Taxlot Assignment", MessageBoxButtons.YesNo, _
                                       MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.Yes Then
                        ' continue...
                    Else
                        Exit Sub
                    End If
                End If
            End If

            Dim theNewTLTaxlotNum As String = String.Empty 'initialize
            If isTaxlotType Then
                '[Taxlot value is a number...]
                theNewTLTaxlotNum = CStr(Me.NumberStartingFrom) 'User entered number
                ' Remove leading Zeros for taxlot number if any exist (CInt conversion will remove them)
                theNewTLTaxlotNum = CStr(CInt(theNewTLTaxlotNum))
            Else
                '[Taxlot value is a word...]
                theNewTLTaxlotNum = Me.TaxlotType 'Predefined text enum
            End If

            '------------------------------------------
            ' Set the new value
            '------------------------------------------

            Debug.Assert(theTaxlotFeature.Fields.Field(theTLTaxlotFieldIndex).Length >= theNewTLTaxlotNum.ToString.Length, "theTaxlotFeature.Fields.Field(theTLTaxlotFieldIndex).Length >= theNewTLTaxlotNum.ToString.Length")

            theTaxlotFeature.Value(theTLTaxlotFieldIndex) = theNewTLTaxlotNum

            '------------------------------------------
            ' End the edit operation (store & stop)
            '------------------------------------------
            ' At this point, the OnChangeFeature event handler will capture 
            ' the mapnumber and taxlot and record them in CancelledNumbers.
            ' Taxlots will send their numbers to the CancelledNumbers table
            ' ONLY if they are unique in the map at the time of deletion.
            theTaxlotFeature.Store()
            My.ArcMap.Editor.StopOperation("Assign Taxlot Number (AutoIncrement)")
            withinEditOperation = False

            '------------------------------------------
            ' AutoIncrement if taxlot number type
            '------------------------------------------
            If isTaxlotType AndAlso Me.IncrementNumber > 0 Then
                Me.NumberStartingFrom += Me.IncrementNumber
            End If

            ' Select the feature
            SetSelectedFeature(TaxlotFeatureLayer, theTaxlotFeature, False, True)

            ' Check for stacked features in the same location
            theTaxlotFeature = theTaxlotFCursor.NextFeature
            If Not theTaxlotFeature Is Nothing Then
                MessageBox.Show("Multiple (""vertical"") features found at this location. This tool can only edit one.", _
                                "Taxlot Assignment", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If

            My.ArcMap.Application.StatusBar.Message(esriStatusBarPanes.esriStatusMain) = "Taxlot feature updated"

        Finally
            If withinEditOperation Then
                ' Abort any ongoing edit operations
                My.ArcMap.Editor.AbortOperation()
                withinEditOperation = False
                My.ArcMap.Application.StatusBar.Message(esriStatusBarPanes.esriStatusMain) = "Taxlot feature update aborted"
            End If

            ' Insure that this tool keeps the focus
            Dim pUID As New ESRI.ArcGIS.esriSystem.UID
            Dim pCommandItem As ESRI.ArcGIS.Framework.ICommandItem
            pUID.Value = My.ThisAddIn.IDs.TaxlotAssignment
            pCommandItem = My.ArcMap.Application.Document.CommandBars.Find(pUID, False, False)
            My.ArcMap.Application.CurrentTool = pCommandItem

        End Try
    End Sub

#End Region

#End Region

#Region "Inherited Class Members"

#Region "Properties (none)"
#End Region

#Region "Methods"

    ''' <summary>
    ''' Event Handler 
    ''' </summary>
    ''' <remarks>Handles Me.OnActivate</remarks>
    Protected Overrides Sub OnActivate()
        Try
            ' Show and activate the partner form.
            If PartnerTaxlotAssignmentForm.Visible Then
                PartnerTaxlotAssignmentForm.Activate()
            Else
                PartnerTaxlotAssignmentForm.Show()
            End If

        Catch ex As Exception
            OrmapExtension.ProcessUnhandledException(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Event Handler 
    ''' </summary>
    ''' <remarks>WARNING: Do not put computation-intensive code here. Called by ArcMap once per second to check if the command is enabled.</remarks>
    Protected Overrides Sub OnUpdate()
        Dim canEnable As Boolean
        canEnable = EditorExtension.CanEnableExtendedEditing
        canEnable = canEnable AndAlso My.ArcMap.Editor.EditState = esriEditState.esriStateEditing
        canEnable = canEnable AndAlso EditorExtension.IsValidWorkspace
        Enabled = canEnable
    End Sub

    ''' <summary>
    ''' Event Handler
    ''' </summary>
    ''' <param name="arg"></param>
    ''' <remarks>Handles Me.OnMouseDown</remarks>
    Protected Overrides Sub OnMouseDown(arg As ESRI.ArcGIS.Desktop.AddIns.Tool.MouseEventArgs)

        Try
            If arg.Button = MouseButtons.Left Then
                '[Left button clicked...]
                DoToolOperation(arg.Button, arg.X, arg.Y)
            ElseIf arg.Button = MouseButtons.Right Then
                '[Right button clicked...]
                ' Show and activate the partner form.
                If PartnerTaxlotAssignmentForm.Visible Then
                    PartnerTaxlotAssignmentForm.Activate()
                Else
                    PartnerTaxlotAssignmentForm.Show()
                End If
            End If

        Catch ex As Exception
            OrmapExtension.ProcessUnhandledException(ex)
        End Try

    End Sub

#End Region

#End Region






End Class
