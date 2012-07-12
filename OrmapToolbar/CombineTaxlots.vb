#Region "Copyright 2008-2011 ORMAP Tech Group"

' File:  CombineTaxlots.vb
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
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Display
Imports ESRI.ArcGIS.Editor
Imports ESRI.ArcGIS.Framework
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Geometry
Imports OrmapTaxlotEditing.DataMonitor
Imports OrmapTaxlotEditing.SpatialUtilities
Imports OrmapTaxlotEditing.StringUtilities
Imports OrmapTaxlotEditing.Utilities
#End Region

''' <summary>
''' Provides an ArcMap Command with functionality to allow users to 
''' combine Taxlot features.
''' </summary>
''' <remarks><seealso cref="CombineTaxlotsForm"/></remarks>
''' 
Public NotInheritable Class CombineTaxlots
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

#Region "Class-Level Constants and Enumerations (none)"
#End Region

#Region "Built-In Class Members (Constructors, Etc.)"

#Region "Constructors"

    Public Sub New()
    End Sub

#End Region

#End Region


#Region "Custom Class Members"

#Region "Fields (none)"
#End Region

#Region "Properties"

    Private WithEvents _partnerCombineTaxlotsForm As CombineTaxlotsForm

    Friend ReadOnly Property PartnerCombineTaxlotsForm() As CombineTaxlotsForm
        Get
            If _partnerCombineTaxlotsForm Is Nothing OrElse _partnerCombineTaxlotsForm.IsDisposed Then
                setPartnerCombineTaxlotsForm(New CombineTaxlotsForm())
            End If
            Return _partnerCombineTaxlotsForm
        End Get
    End Property

    Private Sub setPartnerCombineTaxlotsForm(ByVal value As CombineTaxlotsForm)
        If value IsNot Nothing Then
            _partnerCombineTaxlotsForm = value
            ' Subscribe to partner form events.
            AddHandler _partnerCombineTaxlotsForm.Load, AddressOf PartnerCombineTaxlotsForm_Load
            AddHandler _partnerCombineTaxlotsForm.uxNewTaxlotNumber.SelectedIndexChanged, AddressOf uxNewTaxlotNumber_SelectedIndexChanged
            AddHandler _partnerCombineTaxlotsForm.uxCombine.Click, AddressOf uxCombine_Click
            AddHandler _partnerCombineTaxlotsForm.uxHelp.Click, AddressOf uxHelp_Click
        Else
            ' Unsubscribe to partner form events.
            RemoveHandler _partnerCombineTaxlotsForm.Load, AddressOf PartnerCombineTaxlotsForm_Load
            RemoveHandler _partnerCombineTaxlotsForm.uxNewTaxlotNumber.SelectedIndexChanged, AddressOf uxNewTaxlotNumber_SelectedIndexChanged
            RemoveHandler _partnerCombineTaxlotsForm.uxCombine.Click, AddressOf uxCombine_Click
            RemoveHandler _partnerCombineTaxlotsForm.uxHelp.Click, AddressOf uxHelp_Click
        End If
    End Sub

#End Region

#Region "Event Handlers"

    Private Sub PartnerCombineTaxlotsForm_Load(ByVal sender As Object, ByVal e As System.EventArgs)

        If HasSelectedFeatureCount(TaxlotFeatureLayer, 1) Then

            With PartnerCombineTaxlotsForm

                '-------------------------------
                ' Populate multi-value controls
                '-------------------------------

                .uxNewTaxlotNumber.Items.Clear()

                ' Get a list of distinct taxlot numbers for the set of taxlots to be combined.
                Dim theCurrentTaxlotSelection As IFeatureSelection = DirectCast(TaxlotFeatureLayer, IFeatureSelection)

                ' ENHANCE: [NIS] Escape with error message in the case where selected features span tax maps...

                Dim theQueryFilter As IQueryFilter = New QueryFilter
                theQueryFilter.SubFields = OrmapExtension.TaxLotSettings.TaxlotField

                Dim theCursor As ICursor = Nothing
                theCurrentTaxlotSelection.SelectionSet.Search(theQueryFilter, True, theCursor)

                Dim theDataStats As IDataStatistics = New DataStatistics
                Dim theDataStatsEnum As IEnumerator
                With theDataStats
                    .Cursor = theCursor
                    .Field = OrmapExtension.TaxLotSettings.TaxlotField
                    theDataStatsEnum = CType(.UniqueValues, IEnumerator)
                End With

                Do Until theDataStatsEnum.MoveNext = False
                    .uxNewTaxlotNumber.Items.Add(theDataStatsEnum.Current.ToString)
                Loop

                '-------------------------------
                ' Set control defaults
                '-------------------------------
                .uxNewTaxlotNumber.SelectedIndex = 0

            End With

        End If

    End Sub

    Private Sub uxNewTaxlotNumber_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

        ' Get the user-entered or selected taxlot.
        Dim theTaxlotNumber As String = Nothing
        Dim uxNewTaxlotNumber As ComboBox = PartnerCombineTaxlotsForm.uxNewTaxlotNumber
        theTaxlotNumber = uxNewTaxlotNumber.Text.Trim


        Dim theFlayer As IFeatureLayer = Nothing
        theFlayer = TaxlotFeatureLayer

        Dim theQueryFilter As IQueryFilter = New QueryFilter
        Dim theWhereClause As String
        theQueryFilter.SubFields = "Shape, " & OrmapExtension.TaxLotSettings.MapNumberField
        theWhereClause = "[" & OrmapExtension.TaxLotSettings.TaxlotField & "] = '" & theTaxlotNumber & "'"
        theQueryFilter.WhereClause = formatWhereClause(theWhereClause, theFlayer.FeatureClass)

        Dim theCurrentTaxlotSelection As IFeatureSelection = DirectCast(TaxlotFeatureLayer, IFeatureSelection)

        ' ENHANCE: [NIS] Escape with error message in the case where selected features span tax maps...

        Dim theCursor As ICursor = Nothing
        theCurrentTaxlotSelection.SelectionSet.Search(theQueryFilter, True, theCursor)

        Dim thisFeature As IFeature = DirectCast(theCursor.NextRow, IFeature)

        If thisFeature IsNot Nothing Then
            FlashFeature(thisFeature, DirectCast(My.ArcMap.Application.Document, IMxDocument), 100)
        End If

    End Sub

    Private Sub uxCombine_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        ' Get the user-entered or selected taxlot.
        Dim theTaxlotNumber As String = Nothing
        Dim uxNewTaxlotNumber As ComboBox = PartnerCombineTaxlotsForm.uxNewTaxlotNumber
        theTaxlotNumber = uxNewTaxlotNumber.Text.Trim

        If PartnerCombineTaxlotsForm.Modal Then
            ' Modal form is closed automatically by the 
            ' uxCombine.DialogResult = OK property. 
        Else
            PartnerCombineTaxlotsForm.Close()
        End If

        ' Verify that within this map index, this existing taxlot number is unique.
        ' If not unique, give user option to quit.
        '
        '' Get the selected taxlot from the set of taxlots to be combined.
        'Dim theFeature As IFeature = getSelectedTaxlotFromSet(theTaxlotNumber)
        '
        'If Not IsTaxlotNumberLocallyUnique(theTaxlotNumber, theFeature.Shape, True) Then
        '    If MessageBox.Show("The current Taxlot value (" & theTaxlotNumber & ") is not unique within this MapIndex. " & NewLine & _
        '            "Continue the combine process anyway?", _
        '            "Combine Taxlots", MessageBoxButtons.OKCancel, _
        '            MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Cancel Then
        '        Exit Sub
        '    End If
        'End If

        combine(theTaxlotNumber)
        ' Note: Only the removed taxlots will send their numbers to
        ' the CancelledNumbers table and only if they are unique 
        ' in the map at the time of deletion.

    End Sub

    Private Sub uxHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' TODO: [NIS] Could be replaced with new help mechanism.

        Dim theRTFStream As System.IO.Stream = _
           Me.GetType().Assembly.GetManifestResourceStream("OrmapTaxlotEditing.CombineSelectedTaxlots_help.rtf")
        OpenHelp("Combine Selected Taxlots Help", theRTFStream)

        ' Get the help form.
        'Dim theHelpForm As New HelpForm
        'theHelpForm.Text = "Combine Taxlots Help"

        ' KLUDGE: [NIS] Remove comments if file is ready.
        '' Open a custom help text file.
        '' Note: Requires a specific file in the help subdirectory of the application directory.
        'Dim theTextFilePath As String
        'theTextFilePath = My.Application.Info.DirectoryPath & "\help\CombineTaxlotsHelp.rtf"
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
        'thePdfFilePath = My.Application.Info.DirectoryPath & "\help\CombineTaxlotsHelp.pdf"
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
        'theVideoFilePath = My.Application.Info.DirectoryPath & "\help\videos\CombineTaxlots\CombineTaxlots.html"
        'If Microsoft.VisualBasic.FileIO.FileSystem.FileExists(theVideoFilePath) Then
        '    Dim theUri As New System.Uri("file:///" & theVideoFilePath)
        '    theHelpForm.WebBrowser1.Url = theUri
        'Else
        '    MessageBox.Show("No help file available in the directory " & NewLine & _
        '            My.Application.Info.DirectoryPath & "\help\videos\CombineTaxlots" & ".")
        '    theHelpForm.TabPage2.Hide()
        'End If

        ' KLUDGE: [NIS] Remove comments if form will be used.
        'theHelpForm.Width = 668
        'theHelpForm.Height = 400
        'theHelpForm.Show()

    End Sub

#End Region

#Region "Methods"

    Friend Sub DoButtonOperation()

        Try
            PartnerCombineTaxlotsForm.uxNewTaxlotNumber.Enabled = False
            PartnerCombineTaxlotsForm.uxCombine.Enabled = False

            '---------------------------------------
            ' Check for valid data
            '---------------------------------------
            CheckValidTaxlotDataProperties()
            If Not HasValidTaxlotData Then
                MessageBox.Show("Missing data: Valid ORMAP Taxlot layer not found in the map." & NewLine & _
                                "Please load this dataset into your map.", _
                                "Combine Taxlots", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                Exit Sub
            End If
            CheckValidMapIndexDataProperties()
            If Not HasValidMapIndexData Then
                MessageBox.Show("Missing data: Valid ORMAP MapIndex layer not found in the map." & NewLine & _
                                "Please load this dataset into your map.", _
                                "Combine Taxlots", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                Exit Sub
            End If
            CheckValidCancelledNumbersTableDataProperties()
            If Not HasValidCancelledNumbersTableData Then
                MessageBox.Show("Missing data: Valid ORMAP CancelledNumbersTable not found in the map." & NewLine & _
                                "Please load this dataset into your map.", _
                                "Combine Taxlots", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                Exit Sub
            End If
            CheckValidTaxlotLinesDataProperties()
            If Not HasValidTaxlotLinesData Then
                MessageBox.Show("Missing data: Valid ORMAP TaxlotLines layer not found in the map." & NewLine & _
                                "Please load this dataset into your map.", _
                                "Combine Taxlots", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                Exit Sub
            End If
            CheckValidReferenceLinesDataProperties()
            If Not HasValidReferenceLinesData Then
                MessageBox.Show("Missing data: Valid ORMAP ReferenceLines layer not found in the map." & NewLine & _
                                "Please load this dataset into your map.", _
                                "Combine Taxlots", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                Exit Sub
            End If

            If HasSelectedFeatureCount(TaxlotFeatureLayer, 2) Then
                ' Enable controls to allow the user to select 
                ' the new combined taxlot number from the list.
                PartnerCombineTaxlotsForm.uxNewTaxlotNumber.Enabled = True
                PartnerCombineTaxlotsForm.uxCombine.Enabled = True
            Else
                MessageBox.Show("No features selected in the Taxlot layer." & NewLine & _
                                "Please select at least two features.", _
                                "Combine Taxlots", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                PartnerCombineTaxlotsForm.uxNewTaxlotNumber.Enabled = False
                PartnerCombineTaxlotsForm.uxCombine.Enabled = False
                Exit Sub
            End If

            PartnerCombineTaxlotsForm.ShowDialog()

        Catch ex As Exception
            OrmapExtension.ProcessUnhandledException(ex)

        End Try

    End Sub

    Private Sub combine(ByVal theTaxlotNumber As String)

        Dim withinEditOperation As Boolean = False

        Try

            ' NOTE: Taxlots are already selected and the new combined taxlot number is known at this point.

            Dim theTaxlotFClass As ESRI.ArcGIS.Geodatabase.IFeatureClass
            Dim theTaxlotFieldIndex As Integer
            Dim theTaxlotDataset As ESRI.ArcGIS.Geodatabase.IDataset

            theTaxlotFClass = TaxlotFeatureLayer.FeatureClass
            theTaxlotFieldIndex = LocateFields(TaxlotFeatureLayer.FeatureClass, OrmapExtension.TaxLotSettings.TaxlotField)
            theTaxlotDataset = DirectCast(theTaxlotFClass, IDataset)

            ' Combine taxlots:

            Dim theWorkspaceEdit As IWorkspaceEdit
            theWorkspaceEdit = DirectCast(theTaxlotDataset.Workspace, IWorkspaceEdit)

            If theWorkspaceEdit.IsBeingEdited Then

                Dim theSelectedFeaturesCursor As IFeatureCursor
                Dim theFields As IFields
                Dim thisSelectedFeature As IFeature
                Dim thisPolygon As IPolygon
                Dim thisArea As IArea = Nothing
                Dim thisCurve As ICurve = Nothing
                Dim theAreaSum As Double = 0
                Dim theLengthSum As Double = 0

                Dim theKeepFeature As IFeature = getSelectedTaxlotFromSet(theTaxlotNumber, True)

                '---------------------------------------
                ' Start edit operation.
                '---------------------------------------
                My.ArcMap.Editor.StartOperation()
                withinEditOperation = True

                '---------------------------------------
                ' Add up areas or lengths for the 
                ' selected features, depending on 
                ' feature geometry type.
                '---------------------------------------
                theSelectedFeaturesCursor = GetSelectedFeatures(TaxlotFeatureLayer)

                If Not theSelectedFeaturesCursor Is Nothing Then 'Make sure more than one selected

                    ' Get the first feature
                    thisSelectedFeature = theSelectedFeaturesCursor.NextFeature

                    ' ENHANCE: [NIS] Create these procedures and call instead of first Do...Loop below.
                    'If NeedsAreaForDomain(thisSelectedFeature) Then
                    '    theAreaSum = GetAreaSum(theSelectedFeaturesCursor)
                    'End If
                    'If NeedsLengthForDomain(thisSelectedFeature) Then
                    '    theLengthSum = GetLengthSum(theSelectedFeaturesCursor)
                    'End If

                    Do
                        Select Case thisSelectedFeature.Shape.GeometryType
                            Case esriGeometryType.esriGeometryPolygon
                                thisPolygon = DirectCast(thisSelectedFeature.Shape, IPolygon)
                                thisArea = DirectCast(thisPolygon, IArea)
                                theAreaSum += thisArea.Area

                            Case esriGeometryType.esriGeometryPolyline, esriGeometryType.esriGeometryLine, esriGeometryType.esriGeometryBezier3Curve, esriGeometryType.esriGeometryCircularArc, esriGeometryType.esriGeometryEllipticArc, esriGeometryType.esriGeometryPath
                                thisCurve = DirectCast(thisSelectedFeature.Shape, ICurve)
                                theLengthSum += thisCurve.Length

                            Case Else
                                Throw New Exception("Invalid geometry type: " & thisSelectedFeature.Shape.GeometryType.ToString)

                        End Select

                        thisSelectedFeature = theSelectedFeaturesCursor.NextFeature
                    Loop Until thisSelectedFeature Is Nothing

                End If

                '---------------------------------------
                ' Clear feature cursor variables (first time).
                '---------------------------------------
                theSelectedFeaturesCursor = Nothing
                thisSelectedFeature = Nothing

                '---------------------------------------
                ' Build the combined feature and set its 
                ' attributes (based on merge rules, if 
                ' present)
                '---------------------------------------
                Dim theOutputGeometry As IGeometry = theKeepFeature.ShapeCopy

                theSelectedFeaturesCursor = GetSelectedFeatures(TaxlotFeatureLayer, True)

                If Not theSelectedFeaturesCursor Is Nothing Then 'Make sure more than one selected

                    ' Extract the default subtype from the feature's class.
                    ' Initialize the default values for the new feature.
                    Dim theSubtypes As ISubtypes
                    Dim theDefaultSubtypeCode As Integer
                    theSubtypes = DirectCast(theKeepFeature.Class, ISubtypes)
                    theDefaultSubtypeCode = theSubtypes.DefaultSubtypeCode

                    ' Merge policy
                    Dim theRowSubtypes As IRowSubtypes
                    theRowSubtypes = CType(theKeepFeature, IRowSubtypes)
                    theRowSubtypes.InitDefaultValues()
                    Dim theSubtypeCode As Integer = theRowSubtypes.SubtypeCode

                    thisSelectedFeature = theSelectedFeaturesCursor.NextFeature
                    theFields = theTaxlotFClass.Fields

                    Dim theFeatureCount As Integer
                    theFeatureCount = 1
                    Do Until thisSelectedFeature Is Nothing
                        ' Get the selected feature's geometry
                        Dim theGeometry As IGeometry
                        theGeometry = thisSelectedFeature.ShapeCopy

                        ' Merge the geometry of the features
                        Dim theTopoOperator As ITopologicalOperator
                        theTopoOperator = DirectCast(theOutputGeometry, ITopologicalOperator)
                        If Not theTopoOperator.IsSimple Then theTopoOperator.Simplify()

                        theOutputGeometry = theTopoOperator.Union(theGeometry)
                        If Not theTopoOperator.IsSimple Then theTopoOperator.Simplify()

                        Select Case thisSelectedFeature.Shape.GeometryType
                            Case esriGeometryType.esriGeometryPolygon
                                thisPolygon = DirectCast(thisSelectedFeature.Shape, IPolygon)
                                thisArea = DirectCast(thisPolygon, IArea)

                            Case esriGeometryType.esriGeometryPolyline, esriGeometryType.esriGeometryLine, esriGeometryType.esriGeometryBezier3Curve, esriGeometryType.esriGeometryCircularArc, esriGeometryType.esriGeometryEllipticArc, esriGeometryType.esriGeometryPath
                                thisCurve = DirectCast(thisSelectedFeature.Shape, ICurve)

                            Case Else
                                ' Will have been caught already above

                        End Select

                        ' Go through each field. 
                        ' If it has a domain associated with it, then evaluate the merge policy:

                        For thisFieldIndex As Integer = 0 To (theFields.FieldCount - 1)

                            Dim theField As IField
                            Dim theDomain As IDomain
                            theField = theFields.Field(thisFieldIndex)
                            theDomain = theSubtypes.Domain(theSubtypeCode, theField.Name)

                            If Not theDomain Is Nothing Then
                                Select Case theDomain.MergePolicy

                                    Case esriMergePolicyType.esriMPTSumValues 'Sum values
                                        If theFeatureCount = 1 Then
                                            theKeepFeature.Value(thisFieldIndex) = 0 'initialize the first one
                                        End If
                                        addByDataType(theKeepFeature, thisSelectedFeature, thisFieldIndex)

                                    Case esriMergePolicyType.esriMPTAreaWeighted 'Area/length weighted average
                                        If theFeatureCount = 1 Then
                                            theKeepFeature.Value(thisFieldIndex) = 0 'initialize the first one
                                        End If
                                        areaWeightedAddByDataType(theKeepFeature, thisSelectedFeature, thisArea, thisCurve, theAreaSum, theLengthSum, thisFieldIndex)

                                    Case Else
                                        '[No merge policy...]
                                        ' Use the existing keep feature value
                                        'DO NOT USE: 
                                        'theKeepFeature.Value(thisFieldIndex) = thisSelectedFeature.Value(thisFieldIndex)

                                End Select 'do not need a case for default value as it is set above
                            Else
                                '[Not a domain...]
                                ' Use the existing keep feature value
                                'DO NOT USE: 
                                'If theKeepFeature.Fields.Field(thisFieldIndex).Editable Then 'Don't attempt to copy objectid or other non-editable field
                                '    theKeepFeature.Value(thisFieldIndex) = thisSelectedFeature.Value(thisFieldIndex)
                                'End If
                            End If
                        Next thisFieldIndex

                        thisSelectedFeature = theSelectedFeaturesCursor.NextFeature
                        theFeatureCount += 1

                    Loop

                End If

                '---------------------------------------
                ' Clear the feature cursor variables (second time).
                '---------------------------------------
                theSelectedFeaturesCursor = Nothing
                thisSelectedFeature = Nothing

                '---------------------------------------
                ' Set the new feature geometry to the
                ' combined geometry.
                '---------------------------------------
                theKeepFeature.Shape = theOutputGeometry

                ''---------------------------------------
                '' Set the new combined taxlot number.
                ''---------------------------------------
                'theKeepFeature.Value(theTaxlotFieldIndex) = theTaxlotNumber

                '---------------------------------------
                ' Store the feature edits.
                '---------------------------------------
                theKeepFeature.Store()

                '---------------------------------------
                ' Delete all the combined features other
                ' than the kept feature.
                '---------------------------------------
                deleteUnneededFeatures(theKeepFeature)

                '---------------------------------------
                ' Move deleted taxlot lines to Reference Lines,
                ' line type 33 (historical):
                '---------------------------------------
                moveDeletedLines(theKeepFeature)

                '---------------------------------------
                ' Refresh the display area of the new feature:
                '---------------------------------------
                Dim theInvalidArea As IInvalidArea
                theInvalidArea = New InvalidArea
                theInvalidArea.Display = My.ArcMap.Editor.Display
                theInvalidArea.Add(theKeepFeature)
                theInvalidArea.Invalidate(CShort(esriScreenCache.esriAllScreenCaches))

                '---------------------------------------
                ' Select the new feature:
                '---------------------------------------
                SetSelectedFeature(TaxlotFeatureLayer, theKeepFeature, False, True)

                '---------------------------------------
                ' Finish edit operation.
                '---------------------------------------
                My.ArcMap.Editor.StopOperation("Combine Taxlots")
                withinEditOperation = False

            End If

        Catch ex As Exception
            If withinEditOperation Then
                ' Abort any ongoing edit operations
                My.ArcMap.Editor.AbortOperation()
                withinEditOperation = False
            End If

            Throw

        End Try

    End Sub

    Private Shared Sub addByDataType(ByVal theNewFeature As IFeature, ByVal thisSelectedFeature As IFeature, ByVal thisFieldIndex As Integer)

        Select Case theNewFeature.Fields.Field(thisFieldIndex).Type

            Case esriFieldType.esriFieldTypeDouble
                theNewFeature.Value(thisFieldIndex) = CDbl(theNewFeature.Value(thisFieldIndex)) + CDbl(thisSelectedFeature.Value(thisFieldIndex))

            Case esriFieldType.esriFieldTypeInteger
                theNewFeature.Value(thisFieldIndex) = CInt(theNewFeature.Value(thisFieldIndex)) + CInt(thisSelectedFeature.Value(thisFieldIndex))

            Case esriFieldType.esriFieldTypeSmallInteger
                theNewFeature.Value(thisFieldIndex) = CShort(theNewFeature.Value(thisFieldIndex)) + CShort(thisSelectedFeature.Value(thisFieldIndex))

            Case esriFieldType.esriFieldTypeSingle
                theNewFeature.Value(thisFieldIndex) = CSng(theNewFeature.Value(thisFieldIndex)) + CSng(thisSelectedFeature.Value(thisFieldIndex))

        End Select

    End Sub

    Private Shared Sub areaWeightedAddByDataType(ByVal theNewFeature As IFeature, ByVal thisSelectedFeature As IFeature, ByVal thisArea As IArea, ByVal thisCurve As ICurve, ByVal theAreaSum As Double, ByVal theLengthSum As Double, ByVal thisFieldIndex As Integer)

        Dim theWeight As Double
        Select Case thisSelectedFeature.Shape.GeometryType

            Case esriGeometryType.esriGeometryPolygon
                theWeight = thisArea.Area / theAreaSum

            Case esriGeometryType.esriGeometryPolyline, esriGeometryType.esriGeometryLine, esriGeometryType.esriGeometryBezier3Curve, esriGeometryType.esriGeometryCircularArc, esriGeometryType.esriGeometryEllipticArc, esriGeometryType.esriGeometryPath
                theWeight = thisCurve.Length / theLengthSum

        End Select

        Select Case theNewFeature.Fields.Field(thisFieldIndex).Type

            Case esriFieldType.esriFieldTypeDouble
                theNewFeature.Value(thisFieldIndex) = CDbl(theNewFeature.Value(thisFieldIndex)) + CDbl(thisSelectedFeature.Value(thisFieldIndex)) * theWeight

            Case esriFieldType.esriFieldTypeInteger
                theNewFeature.Value(thisFieldIndex) = CInt(theNewFeature.Value(thisFieldIndex)) + CInt(thisSelectedFeature.Value(thisFieldIndex)) * theWeight

            Case esriFieldType.esriFieldTypeSmallInteger
                theNewFeature.Value(thisFieldIndex) = CShort(theNewFeature.Value(thisFieldIndex)) + CShort(thisSelectedFeature.Value(thisFieldIndex)) * theWeight

            Case esriFieldType.esriFieldTypeSingle
                theNewFeature.Value(thisFieldIndex) = CSng(theNewFeature.Value(thisFieldIndex)) + CSng(thisSelectedFeature.Value(thisFieldIndex)) * theWeight

        End Select

    End Sub

    Private Shared Sub deleteUnneededFeatures(ByVal theKeepFeature As IFeature)
        Dim theSelectedFeaturesCursor As IFeatureCursor
        Dim thisSelectedFeature As IFeature
        '---------------------------------------
        ' Remove the kept feature from the
        ' original selection set.
        '---------------------------------------
        Dim theCurrentTaxlotSelection As IFeatureSelection = DirectCast(TaxlotFeatureLayer, IFeatureSelection)
        Dim theOidList() As Integer
        ReDim theOidList(0)
        theOidList(0) = theKeepFeature.OID
        DirectCast(My.ArcMap.Editor.Map, IActiveView).PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, Nothing, Nothing)
        Dim theSelectionEvents As ISelectionEvents = DirectCast(My.ArcMap.Editor.Map, ISelectionEvents)
        theCurrentTaxlotSelection.SelectionSet.RemoveList(1, theOidList(0))
        theCurrentTaxlotSelection.SelectionChanged()
        theSelectionEvents.SelectionChanged()
        DirectCast(My.ArcMap.Editor.Map, IActiveView).PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, Nothing, Nothing)

        '---------------------------------------
        ' Delete all the other features.
        '---------------------------------------
        theSelectedFeaturesCursor = GetSelectedFeatures(TaxlotFeatureLayer, True)
        If Not theSelectedFeaturesCursor Is Nothing Then
            thisSelectedFeature = theSelectedFeaturesCursor.NextFeature
            Do Until thisSelectedFeature Is Nothing
                thisSelectedFeature.Delete()
                thisSelectedFeature = theSelectedFeaturesCursor.NextFeature
            Loop
        End If

    End Sub

    Private Shared Function getSelectedTaxlotFromSet(ByVal theTaxlotNumber As String, Optional ByVal isEditable As Boolean = False) As IFeature

        ' Get the selected taxlot from the set of taxlots to be combined.

        Dim theCurrentTaxlotSelection As IFeatureSelection = DirectCast(TaxlotFeatureLayer, IFeatureSelection)

        Dim theQueryFilter As IQueryFilter = New QueryFilter
        theQueryFilter.SubFields = "*"
        Dim theSQL As String = "[" & OrmapExtension.TaxLotSettings.TaxlotField & "] = '" & theTaxlotNumber & "'"
        theQueryFilter.WhereClause = formatWhereClause(theSQL, TaxlotFeatureLayer.FeatureClass)

        Dim theCursor As ICursor = Nothing
        If isEditable Then
            theCurrentTaxlotSelection.SelectionSet.Search(theQueryFilter, False, theCursor) 'non-recycling cursor
        Else
            theCurrentTaxlotSelection.SelectionSet.Search(theQueryFilter, True, theCursor) 'recycling cursor
        End If
        Dim theQueryField As Integer = theCursor.FindField(OrmapExtension.TaxLotSettings.TaxlotField)
        Dim theRow As IRow = theCursor.NextRow
        Dim theFeature As IFeature = Nothing
        Do Until theRow Is Nothing
            theFeature = DirectCast(theRow, IFeature)
            theRow = theCursor.NextRow
        Loop

        Return theFeature

    End Function

    Private Shared Sub moveDeletedLines(ByVal theKeepFeature As IFeature)
        '---------------------------------------
        ' Move deleted taxlot lines to Reference Lines,
        ' line type 33 (historical):
        '---------------------------------------
        Dim theTaxlotLinesFClass As IFeatureClass
        theTaxlotLinesFClass = TaxlotLinesFeatureLayer.FeatureClass

        Dim theLineTypeFieldIndex As Integer

        ' ENHANCE: [NIS] Need to add ReferenceLinesSettings and use them here.
        theLineTypeFieldIndex = LocateFields(ReferenceLinesFeatureLayer.FeatureClass, OrmapExtension.TaxLotLinesSettings.LineTypeField)

        Dim theCombinedGeom As IGeometry
        theCombinedGeom = theKeepFeature.Shape

        Dim theTaxlotLinesFCursor As IFeatureCursor
        theTaxlotLinesFCursor = DoSpatialQuery(theTaxlotLinesFClass, theCombinedGeom, esriSpatialRelEnum.esriSpatialRelContains, String.Empty, True)
        If Not theTaxlotLinesFCursor Is Nothing Then
            Dim thisLineFeature As IFeature
            thisLineFeature = theTaxlotLinesFCursor.NextFeature
            Do While Not thisLineFeature Is Nothing
                Dim thisNewLineFeature As IFeature
                thisNewLineFeature = ReferenceLinesFeatureLayer.FeatureClass.CreateFeature
                thisNewLineFeature.Shape = thisLineFeature.ShapeCopy
                thisNewLineFeature.Value(theLineTypeFieldIndex) = 33
                thisNewLineFeature.Store()
                theTaxlotLinesFCursor.DeleteFeature()
                thisLineFeature = theTaxlotLinesFCursor.NextFeature
            Loop
        End If
    End Sub

#End Region

#End Region

#Region "Inherited Class Members"

#Region "Properties"

#End Region

#Region "Methods"

    ''' <summary>
    ''' Handles click event of button
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overrides Sub OnClick()
        My.ArcMap.Application.CurrentTool = Nothing
        DoButtonOperation()
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


#End Region

#End Region

#Region "Implemented Interface Members"

#End Region



End Class




