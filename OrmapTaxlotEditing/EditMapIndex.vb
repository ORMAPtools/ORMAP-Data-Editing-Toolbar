#Region "Copyright 2008 ORMAP Tech Group"

' File:  EditMapIndex.vb
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
'SCC revision number: $Revision: 437 $
'Date of Last Change: $Date: 2010-05-12 18:22:16 -0700 (Wed, 12 May 2010) $
#End Region

#Region "Imported Namespaces"
Imports System
Imports System.Drawing
Imports System.Environment
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports System.Windows.Forms.Control
Imports ESRI.ArcGIS.ADF.BaseClasses
Imports ESRI.ArcGIS.ADF.CATIDs
Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Editor
Imports ESRI.ArcGIS.Framework
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Carto
Imports OrmapTaxlotEditing.DataMonitor
Imports OrmapTaxlotEditing.SpatialUtilities
Imports OrmapTaxlotEditing.StringUtilities
Imports OrmapTaxlotEditing.Utilities
#End Region

''' <summary>
''' Provides an ArcMap Command with functionality to 
''' allow users to edit MapIndex component fields.
''' </summary>
''' <remarks><seealso cref="EditMapIndexForm"/></remarks>
<ComVisible(True)> _
<ComClass(EditMapIndex.ClassId, EditMapIndex.InterfaceId, EditMapIndex.EventsId), _
ProgId("ORMAPTaxlotEditing.EditMapIndex")> _
Public NotInheritable Class EditMapIndex
    Inherits BaseCommand
    Implements IDisposable

#Region "Class-Level Constants and Enumerations"

    Friend Enum StatePassageType As Integer
        Entering = 1
        Exiting = 2
    End Enum

    Friend Enum CommandStateType As Integer
        Enabled = 10
        Disabled = 20
    End Enum

#End Region

#Region "Built-In Class Members (Constructors, Etc.)"

#Region "Constructors"

    ' A creatable COM class must have a Public Sub New() 
    ' with no parameters, otherwise, the class will not be 
    ' registered in the COM registry and cannot be created 
    ' via CreateObject.
    Public Sub New()
        MyBase.New()

        ' Define protected instance field values for the public properties
        MyBase.m_category = "OrmapToolbar"  'localizable text 
        MyBase.m_caption = "EditMapIndex"   'localizable text 
        MyBase.m_message = "Edit the selected MapIndex polygon and underlying Taxlot polygons."   'localizable text 
        MyBase.m_toolTip = "Edit MapIndex" 'localizable text 
        MyBase.m_name = MyBase.m_category & "_EditMapIndex"  'unique id, non-localizable (e.g. "MyCategory_ArcMapCommand")

        Try
            ' Set the bitmap based on the name of the class.
            _bitmapResourceName = Me.GetType().Name + ".bmp"
            MyBase.m_bitmap = New Bitmap(Me.GetType(), _bitmapResourceName)
        Catch ex As ArgumentException
            EditorExtension.ProcessUnhandledException(ex)
        End Try

    End Sub

#End Region

#End Region

#Region "Custom Class Members"

#Region "Structures"
    Friend Structure TaxlotFieldMap
        Friend Anomaly As Integer
        Friend County As Integer
        Friend MapAcres As Integer
        Friend MapNumber As Integer
        Friend MapTaxlotNumber As Integer
        Friend MapTaxlot As Integer
        Friend OrmapTaxlotNumber As Integer
        Friend OrmapMapNumber As Integer
        Friend PartialRangeCode As Integer
        Friend PartialTownshipCode As Integer
        Friend Quarter As Integer
        Friend QuarterQuarter As Integer
        Friend Range As Integer
        Friend RangeDirectional As Integer
        Friend Section As Integer
        Friend SuffixNumber As Integer
        Friend SuffixType As Integer
        Friend Taxlot As Integer
        Friend Township As Integer
        Friend TownshipDirectional As Integer
    End Structure

    Friend Structure MapIndexFieldMap
        Friend MapNumber As Integer
        Friend MapScale As Integer
        Friend ORMAPNumber As Integer
        Friend Page As Integer
        Friend Reliability As Integer
    End Structure
#End Region

#Region "Fields"

    Private _application As IApplication
    Private _bitmapResourceName As String
    Private _mapIndexFeature As IFeature
    Private WithEvents _ormapNumber As ORMapNum
    Private _mapIndexFields As MapIndexFieldMap
    Private _taxlotFields As TaxlotFieldMap
    Private _editingState As Boolean
    Private _isLoadingForm As Boolean

#End Region

#Region "Properties "

    Private _state As CommandStateType = CommandStateType.Disabled

    Friend ReadOnly Property State() As CommandStateType
        Get
            Return _state
        End Get
    End Property

    Private Sub setState(ByVal stateType As CommandStateType)
        _state = stateType
    End Sub

    Friend Property EditingState() As Boolean
        Get
            EditingState = _editingState
        End Get
        Set(ByVal value As Boolean)
            _editingState = value
        End Set
    End Property

    Private WithEvents _partnerEditMapIndexForm As EditMapIndexForm

    Friend ReadOnly Property PartnerEditMapIndexForm() As EditMapIndexForm
        Get
            If _partnerEditMapIndexForm Is Nothing OrElse _partnerEditMapIndexForm.IsDisposed Then
                setPartnerEditMapIndexForm(New EditMapIndexForm())
            End If
            Return _partnerEditMapIndexForm
        End Get
    End Property

    Private Sub setPartnerEditMapIndexForm(ByVal value As EditMapIndexForm)
        If value IsNot Nothing Then
            _partnerEditMapIndexForm = value
            ' Subscribe to partner form events.
            With _partnerEditMapIndexForm
                AddHandler .uxOK.Click, AddressOf Me.uxOK_Click
                AddHandler .uxApply.Click, AddressOf Me.uxApply_Click
                AddHandler .uxCancel.Click, AddressOf Me.uxQuit_Click
                AddHandler .uxHelp.Click, AddressOf Me.uxHelp_Click

                AddHandler .uxTownship.SelectedIndexChanged, AddressOf Me.uxTownship_SelectedIndexChanged
                AddHandler .uxTownshipDirectional.SelectedIndexChanged, AddressOf Me.uxTownshipDirectional_SelectedIndexChanged
                AddHandler .uxTownshipPartial.SelectedIndexChanged, AddressOf Me.uxTownshipPartial_SelectedIndexChanged

                AddHandler .uxRange.SelectedIndexChanged, AddressOf Me.uxRange_SelectedIndexChanged
                AddHandler .uxRangeDirectional.SelectedIndexChanged, AddressOf Me.uxRangeDirectional_SelectedIndexChanged
                AddHandler .uxRangePartial.SelectedIndexChanged, AddressOf Me.uxRangePartial_SelectedIndexChanged

                AddHandler .uxSection.SelectedIndexChanged, AddressOf Me.uxSection_SelectedIndexChanged
                AddHandler .uxSectionQuarter.SelectedIndexChanged, AddressOf Me.uxSectionQuarter_SelectedIndexChanged
                AddHandler .uxSectionQtrQtr.SelectedIndexChanged, AddressOf Me.uxSectionQtrQtr_SelectedIndexChanged

                AddHandler .uxCounty.SelectedIndexChanged, AddressOf Me.uxCounty_SelectedIndexChanged
                ' Map Number - Not part of a ORMAP number
                AddHandler .uxSuffixType.SelectedIndexChanged, AddressOf Me.uxSuffixType_SelectedIndexChanged
                'AddHandler .uxSuffixNumber.TextChanged, AddressOf Me.uxSuffixNumber_TextChanged
                AddHandler .uxSuffixNumber.Validating, AddressOf Me.uxSuffixNumber_Validating
                AddHandler .uxSuffixNumber.Validated, AddressOf Me.uxSuffixNumber_Validated
                AddHandler .uxSuffixType.SelectedIndexChanged, AddressOf Me.uxSuffixType_SelectedIndexChanged
                ' Scale - Not part of a ORMAP number
                AddHandler .uxAnomaly.TextChanged, AddressOf Me.uxAnomaly_TextChanged

                AddHandler .uxPage.TextChanged, AddressOf Me.uxPage_TextChanged
                AddHandler .uxReliability.TextChanged, AddressOf Me.uxReliability_SelectedIndexChanged

                AddHandler .uxORMAPNumberLabel.TextChanged, AddressOf Me.uxORMAPNumberLabel_TextChanged
            End With
        Else
            ' Unsubscribe to partner form events.
            With _partnerEditMapIndexForm
                RemoveHandler .uxOK.Click, AddressOf Me.uxOK_Click
                RemoveHandler .uxApply.Click, AddressOf Me.uxApply_Click
                RemoveHandler .uxCancel.Click, AddressOf Me.uxQuit_Click
                RemoveHandler .uxHelp.Click, AddressOf Me.uxHelp_Click

                RemoveHandler .uxTownship.SelectedIndexChanged, AddressOf Me.uxTownship_SelectedIndexChanged
                RemoveHandler .uxTownshipDirectional.SelectedIndexChanged, AddressOf Me.uxTownshipDirectional_SelectedIndexChanged
                RemoveHandler .uxTownshipPartial.SelectedIndexChanged, AddressOf Me.uxTownshipPartial_SelectedIndexChanged

                RemoveHandler .uxRange.SelectedIndexChanged, AddressOf Me.uxRange_SelectedIndexChanged
                RemoveHandler .uxRangeDirectional.SelectedIndexChanged, AddressOf Me.uxRangeDirectional_SelectedIndexChanged
                RemoveHandler .uxRangePartial.SelectedIndexChanged, AddressOf Me.uxRangePartial_SelectedIndexChanged

                RemoveHandler .uxSection.SelectedIndexChanged, AddressOf Me.uxSection_SelectedIndexChanged
                RemoveHandler .uxSectionQuarter.SelectedIndexChanged, AddressOf Me.uxSectionQuarter_SelectedIndexChanged
                RemoveHandler .uxSectionQtrQtr.SelectedIndexChanged, AddressOf Me.uxSectionQtrQtr_SelectedIndexChanged

                RemoveHandler .uxCounty.SelectedIndexChanged, AddressOf Me.uxCounty_SelectedIndexChanged
                ' Map Number - Not part of a ORMAP number
                RemoveHandler .uxSuffixType.SelectedIndexChanged, AddressOf Me.uxSuffixType_SelectedIndexChanged
                'RemoveHandler .uxSuffixNumber.TextChanged, AddressOf Me.uxSuffixNumber_TextChanged
                RemoveHandler .uxSuffixNumber.Validating, AddressOf Me.uxSuffixNumber_Validating
                RemoveHandler .uxSuffixNumber.Validated, AddressOf Me.uxSuffixNumber_Validated
                RemoveHandler .uxSuffixType.SelectedIndexChanged, AddressOf Me.uxSuffixType_SelectedIndexChanged
                ' Scale - Not part of a ORMAP number
                RemoveHandler .uxAnomaly.TextChanged, AddressOf Me.uxAnomaly_TextChanged

                RemoveHandler .uxPage.TextChanged, AddressOf Me.uxPage_TextChanged
                RemoveHandler .uxReliability.TextChanged, AddressOf Me.uxReliability_SelectedIndexChanged

                RemoveHandler .uxORMAPNumberLabel.TextChanged, AddressOf Me.uxORMAPNumberLabel_TextChanged
            End With
        End If

    End Sub

#End Region

#Region "Event Handlers"

    Private Sub PartnerMapIndexForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles _partnerEditMapIndexForm.Load
        _isLoadingForm = True
        initializeFieldIndices()
        EditingState = (EditorExtension.Editor.EditState = esriEditState.esriStateEditing)
        'PartnerEditMapIndexForm.uxSuffixNumber.MaxLength = 3 '3 chars only
        toggleComponentControls(EditingState)
        initForm()
        _isLoadingForm = False
        ' State Transistion E2
        TransitionE2()
    End Sub

    Private Sub uxOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim theEditWorkSpace As IWorkspaceEdit
        Dim theMapIndexFC As IFeatureClass = DataMonitor.MapIndexFeatureLayer.FeatureClass
        Dim theMapIndexDataset As IDataset = DirectCast(theMapIndexFC, IDataset)
        theEditWorkSpace = TryCast(theMapIndexDataset.Workspace, IWorkspaceEdit)

        Try
            Dim validData As Boolean = True
            validData = validData AndAlso (Len(PartnerEditMapIndexForm.uxReliability.Text) <> 0)
            validData = validData AndAlso (Len(PartnerEditMapIndexForm.uxScale.Text) <> 0)
            validData = validData AndAlso (Len(PartnerEditMapIndexForm.uxMapNumber.Text) <> 0)
            validData = validData AndAlso (Len(PartnerEditMapIndexForm.uxPage.Text) <> 0)
            validData = validData AndAlso _ormapNumber.IsValidNumber

            If Not validData Then
                MessageBox.Show("Invalid data. All fields must be filled in before assigning.", "Edit Map Index", MessageBoxButtons.OK)
                Exit Sub
            End If

            editMapIndex(theEditWorkSpace)

            PartnerEditMapIndexForm.Close()

        Catch ex As Exception
            If theEditWorkSpace.IsBeingEdited Then
                theEditWorkSpace.AbortEditOperation()
            End If
            EditorExtension.ProcessUnhandledException(ex)
        End Try

    End Sub

    Private Sub uxApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim theEditWorkSpace As IWorkspaceEdit
        Dim theMapIndexFC As IFeatureClass = DataMonitor.MapIndexFeatureLayer.FeatureClass
        Dim theMapIndexDataset As IDataset = DirectCast(theMapIndexFC, IDataset)
        theEditWorkSpace = TryCast(theMapIndexDataset.Workspace, IWorkspaceEdit)

        Try
            Dim validData As Boolean = True
            validData = validData AndAlso (Len(PartnerEditMapIndexForm.uxReliability.Text) <> 0)
            validData = validData AndAlso (Len(PartnerEditMapIndexForm.uxScale.Text) <> 0)
            validData = validData AndAlso (Len(PartnerEditMapIndexForm.uxMapNumber.Text) <> 0)
            validData = validData AndAlso (Len(PartnerEditMapIndexForm.uxPage.Text) <> 0)
            validData = validData AndAlso _ormapNumber.IsValidNumber

            If Not validData Then
                MessageBox.Show("Invalid data. All fields must be filled in before assigning.", "Edit Map Index", MessageBoxButtons.OK)
                Exit Sub
            End If

            editMapIndex(theEditWorkSpace)

        Catch ex As Exception
            If theEditWorkSpace.IsBeingEdited Then
                theEditWorkSpace.AbortEditOperation()
            End If
            EditorExtension.ProcessUnhandledException(ex)
        End Try

    End Sub

    Private Sub uxQuit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        PartnerEditMapIndexForm.Close()
    End Sub

    Private Sub uxHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) 'Handles PartnerEditMapIndexForm.uxHelp.Click
        ' TODO: [NIS] Could be replaced with new help mechanism.

        Dim theRTFStream As System.IO.Stream = _
           Me.GetType().Assembly.GetManifestResourceStream("OrmapTaxlotEditing.EditMapIndex_help.rtf")
        OpenHelp("Edit MapIndex Help", theRTFStream)

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


    Private Sub uxORMAPNumberLabel_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' State Transistion E1
        TransitionE1()
    End Sub

    Private Sub uxCounty_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' Validate entry
        Dim theOldSetting As String = _ormapNumber.County
        If PartnerEditMapIndexForm.uxCounty.Text <> String.Empty Then
            _ormapNumber.County = PartnerEditMapIndexForm.uxCounty.Text
        End If
        If _ormapNumber.IsValidNumber Then
            PartnerEditMapIndexForm.uxCounty.Text = _ormapNumber.County
        Else
            _ormapNumber.County = theOldSetting
            PartnerEditMapIndexForm.uxCounty.Text = theOldSetting
        End If
        ' Update map number strings
        updateNumbers()
    End Sub

    Private Sub uxTownship_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' Validate entry
        Dim theOldSetting As String = _ormapNumber.Township
        If PartnerEditMapIndexForm.uxTownship.Text <> String.Empty Then
            _ormapNumber.Township = PartnerEditMapIndexForm.uxTownship.Text
        End If
        If _ormapNumber.IsValidNumber Then
            PartnerEditMapIndexForm.uxTownship.Text = _ormapNumber.Township
        Else
            _ormapNumber.Township = theOldSetting
            PartnerEditMapIndexForm.uxTownship.Text = theOldSetting
        End If
        ' Update map number strings
        updateNumbers()
    End Sub

    Private Sub uxTownshipPartial_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' Validate entry
        Dim theOldSetting As String = _ormapNumber.PartialTownshipCode
        If PartnerEditMapIndexForm.uxTownshipPartial.Text <> String.Empty Then
            _ormapNumber.PartialTownshipCode = PartnerEditMapIndexForm.uxTownshipPartial.Text
        End If
        If _ormapNumber.IsValidNumber Then
            PartnerEditMapIndexForm.uxTownshipPartial.Text = _ormapNumber.PartialTownshipCode
        Else
            _ormapNumber.PartialTownshipCode = theOldSetting
            PartnerEditMapIndexForm.uxTownshipPartial.Text = theOldSetting
        End If
        ' Update map number strings
        updateNumbers()
    End Sub

    Private Sub uxTownshipDirectional_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' Validate entry
        Dim theOldSetting As String = _ormapNumber.TownshipDirectional
        If PartnerEditMapIndexForm.uxTownshipDirectional.Text <> String.Empty Then
            _ormapNumber.TownshipDirectional = PartnerEditMapIndexForm.uxTownshipDirectional.Text
        End If
        If _ormapNumber.IsValidNumber Then
            PartnerEditMapIndexForm.uxTownshipDirectional.Text = _ormapNumber.TownshipDirectional
        Else
            _ormapNumber.TownshipDirectional = theOldSetting
            PartnerEditMapIndexForm.uxTownshipDirectional.Text = theOldSetting
        End If
        ' Update map number strings
        updateNumbers()
    End Sub

    Private Sub uxRange_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' Validate entry
        Dim theOldSetting As String = _ormapNumber.Range
        If PartnerEditMapIndexForm.uxRange.Text <> String.Empty Then
            _ormapNumber.Range = PartnerEditMapIndexForm.uxRange.Text
        End If

        If _ormapNumber.IsValidNumber Then
            PartnerEditMapIndexForm.uxRange.Text = _ormapNumber.Range
        Else
            _ormapNumber.Range = theOldSetting
            PartnerEditMapIndexForm.uxRange.Text = theOldSetting
        End If
        ' Update map number strings
        updateNumbers()
    End Sub

    Private Sub uxRangePartial_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' Validate entry
        Dim theOldSetting As String = _ormapNumber.PartialRangeCode
        If PartnerEditMapIndexForm.uxRangePartial.Text <> String.Empty Then
            _ormapNumber.PartialRangeCode = PartnerEditMapIndexForm.uxRangePartial.Text
        End If

        If _ormapNumber.IsValidNumber Then
            PartnerEditMapIndexForm.uxRangePartial.Text = _ormapNumber.PartialRangeCode
        Else
            _ormapNumber.PartialRangeCode = theOldSetting
            PartnerEditMapIndexForm.uxRangePartial.Text = theOldSetting
        End If
        ' Update map number strings
        updateNumbers()
    End Sub

    Private Sub uxRangeDirectional_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' Validate entry
        Dim theOldSetting As String = _ormapNumber.RangeDirectional
        If PartnerEditMapIndexForm.uxRangeDirectional.Text <> String.Empty Then
            _ormapNumber.RangeDirectional = PartnerEditMapIndexForm.uxRangeDirectional.Text
        End If

        If _ormapNumber.IsValidNumber Then
            PartnerEditMapIndexForm.uxRangeDirectional.Text = _ormapNumber.RangeDirectional
        Else
            _ormapNumber.RangeDirectional = theOldSetting
            PartnerEditMapIndexForm.uxRangeDirectional.Text = theOldSetting
        End If
        ' Update map number strings
        updateNumbers()
    End Sub

    Private Sub uxSection_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' Validate entry
        Dim theOldSetting As String = _ormapNumber.Section
        If PartnerEditMapIndexForm.uxSection.Text <> String.Empty Then
            _ormapNumber.Section = PartnerEditMapIndexForm.uxSection.Text
        End If
        If _ormapNumber.IsValidNumber Then
            PartnerEditMapIndexForm.uxSection.Text = _ormapNumber.Section
        Else
            _ormapNumber.Section = theOldSetting
            PartnerEditMapIndexForm.uxSection.Text = theOldSetting
        End If
        ' Update map number strings
        updateNumbers()
    End Sub

    Private Sub uxSectionQuarter_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' Validate entry
        Dim theOldSetting As String = _ormapNumber.Quarter
        If PartnerEditMapIndexForm.uxSectionQuarter.Text <> String.Empty Then
            _ormapNumber.Quarter = PartnerEditMapIndexForm.uxSectionQuarter.Text
        End If

        If _ormapNumber.IsValidNumber Then
            PartnerEditMapIndexForm.uxSectionQuarter.Text = _ormapNumber.Quarter
        Else
            _ormapNumber.Quarter = theOldSetting
            PartnerEditMapIndexForm.uxSectionQuarter.Text = theOldSetting
        End If
        ' Update map number strings
        updateNumbers()
    End Sub

    Private Sub uxSectionQtrQtr_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' Validate entry
        Dim theOldSetting As String = _ormapNumber.QuarterQuarter
        If PartnerEditMapIndexForm.uxSectionQtrQtr.Text <> String.Empty Then
            _ormapNumber.QuarterQuarter = PartnerEditMapIndexForm.uxSectionQtrQtr.Text
        End If
        If _ormapNumber.IsValidNumber Then
            PartnerEditMapIndexForm.uxSectionQtrQtr.Text = _ormapNumber.QuarterQuarter
        Else
            _ormapNumber.QuarterQuarter = theOldSetting
            PartnerEditMapIndexForm.uxSectionQtrQtr.Text = theOldSetting
        End If
        ' Update map number strings
        updateNumbers()
    End Sub

    Private Sub uxSuffixType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' Validate entry
        Dim theOldSetting As String = _ormapNumber.SuffixType
        If PartnerEditMapIndexForm.uxSuffixType.Text <> String.Empty Then
            _ormapNumber.SuffixType = ConvertCodeValueDomainToCode(DataMonitor.TaxlotFeatureLayer.FeatureClass.Fields, EditorExtension.TaxLotSettings.MapSuffixTypeField, PartnerEditMapIndexForm.uxSuffixType.Text)
        End If
        If _ormapNumber.IsValidNumber Then
            ' Do nothing, text is already valid
        Else
            _ormapNumber.SuffixType = theOldSetting
            PartnerEditMapIndexForm.uxSuffixType.Text = theOldSetting
        End If
        ' Update map number strings
        updateNumbers()
    End Sub

    'Private Sub uxSuffixNumber_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    ' Validate entry
    '    Dim theOldSetting As String = _ormapNumber.SuffixNumber
    '    If PartnerEditMapIndexForm.uxSuffixNumber.Text <> String.Empty Then
    '        _ormapNumber.SuffixNumber = PartnerEditMapIndexForm.uxSuffixNumber.Text
    '    End If
    '    If _ormapNumber.IsValidNumber Then
    '        PartnerEditMapIndexForm.uxSuffixNumber.Text = _ormapNumber.SuffixNumber
    '    Else
    '        _ormapNumber.SuffixNumber = theOldSetting
    '        PartnerEditMapIndexForm.uxSuffixNumber.Text = theOldSetting
    '    End If
    '    ' Update map number strings
    '    updateNumbers()
    'End Sub
    Private Sub uxSuffixNumber_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs)
        ' jwm 6-13-08 Am validating based on Oregon Taxmap ESRI user group geodatabase schema design document dated 7/20/2006
        ' MapSuffixNum is a number between 0 and 999

        ''Dim theOldSetting As String = _ormapNumber.SuffixNumber
        Dim theValue As String = PartnerEditMapIndexForm.uxSuffixNumber.Text
        Dim theValueAsInt As Integer = 0

        If Integer.TryParse(theValue, theValueAsInt) Then
            If theValueAsInt > 999 AndAlso theValueAsInt > 0 Then
                PartnerEditMapIndexForm.ErrorProviderSuffixNum.SetError(PartnerEditMapIndexForm.uxSuffixNumber, "Value must be between 0 and 999")
                e.Cancel = True
            Else
                PartnerEditMapIndexForm.ErrorProviderSuffixNum.SetError(PartnerEditMapIndexForm.uxSuffixNumber, "")
                e.Cancel = False
                _ormapNumber.SuffixNumber = theValue
                If _ormapNumber.IsValidNumber Then
                    PartnerEditMapIndexForm.uxSuffixNumber.Text = _ormapNumber.SuffixNumber
                    'Else
                    '_ormapNumber.SuffixNumber = theOldSetting
                    'PartnerEditMapIndexForm.uxSuffixNumber.Text = theOldSetting
                End If
            End If
        Else
            PartnerEditMapIndexForm.ErrorProviderSuffixNum.SetError(PartnerEditMapIndexForm.uxSuffixNumber, "Not a numeric value.")
            e.Cancel = True
        End If

    End Sub

    Private Sub uxSuffixNumber_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
        updateNumbers()
    End Sub
    Private Sub uxAnomaly_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' Validate entry
        Dim theOldSetting As String = _ormapNumber.Anomaly
        If PartnerEditMapIndexForm.uxAnomaly.Text <> String.Empty Then
            _ormapNumber.Anomaly = PartnerEditMapIndexForm.uxAnomaly.Text
        End If
        If _ormapNumber.IsValidNumber Then
            PartnerEditMapIndexForm.uxAnomaly.Text = _ormapNumber.Anomaly
        Else
            _ormapNumber.Anomaly = theOldSetting
            PartnerEditMapIndexForm.uxAnomaly.Text = theOldSetting
        End If
        ' Update map number strings
        updateNumbers()
    End Sub

    Private Sub uxPage_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' Validate entry
        ' (none)
        ' State Transistion E1
        TransitionE1()
    End Sub

    Private Sub uxReliability_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' Validate entry
        ' (none)
        ' State Transistion E1
        TransitionE1()
    End Sub

#End Region

#Region "Methods"

    Friend Sub DoButtonOperation()

        Try
            ' Check for valid data
            CheckValidMapIndexDataProperties()
            If Not HasValidMapIndexData Then
                MessageBox.Show("Missing data: Valid ORMAP MapIndex layer not found in the map." & NewLine & _
                                "Please load this dataset into your map.", _
                                "Edit Map Index", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                Exit Sub
            Else
                ' TODO: [ALL] Why does this return false when one or more MapIndex features are selected?
                'If Not HasSelectedFeatureCount(MapIndexFeatureLayer, 1) Then
                ' HACK: [NIS] This will get past the problem for now...
                Dim theEnumFeature As IEnumFeature
                'theEnumFeature = EditorExtension.Editor.EditSelection
                Dim theMxDoc As IMxDocument
                Dim theMap As IMap
                theMxDoc = DirectCast(EditorExtension.Application.Document, IMxDocument)
                theMap = theMxDoc.FocusMap
                theEnumFeature = DirectCast(theMap.FeatureSelection, IEnumFeature)
                Dim theFeature As IFeature
                Dim n As Integer = 0 'The MapIndex feature count
                theFeature = theEnumFeature.Next
                Do While (Not theFeature Is Nothing)
                    If theFeature.Class Is DataMonitor.MapIndexFeatureLayer.FeatureClass Then
                        n += 1
                        If n = 1 Then
                            _mapIndexFeature = theFeature
                        End If
                    End If
                    theFeature = theEnumFeature.Next
                Loop
                Select Case n 'The MapIndex feature count
                    Case 1
                        ' Do nothing, this is what we want.
                    Case 0
                        MessageBox.Show("No features selected in the MapIndex layer." & NewLine & _
                                        "Please select one MapIndex feature.", _
                                        "Edit Map Index", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Exit Sub
                    Case Else
                        MessageBox.Show(CStr(n) & " features selected in the MapIndex layer." & NewLine & _
                                        "Please select just one MapIndex feature.", _
                                        "Edit Map Index", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Exit Sub
                End Select
            End If

            CheckValidTaxlotDataProperties()
            If Not HasValidTaxlotData Then
                MessageBox.Show("Missing data: Valid ORMAP MapIndex layer not found in the map." & NewLine & _
                                "Please load this dataset into your map.", _
                                "Edit Map Index", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                Exit Sub
            Else
                PartnerEditMapIndexForm.ShowDialog()
            End If

        Catch ex As Exception
            EditorExtension.ProcessUnhandledException(ex)
        End Try

    End Sub

    Private Sub editMapIndex(ByRef theEditWorkSpace As IWorkspaceEdit)

        Try
            ' Begin edit process
            EditorExtension.Editor.StartOperation()

            With PartnerEditMapIndexForm
                ' Update form caption
                .Text = String.Concat("Edit Map Index - OR Map Number: ", _ormapNumber.GetORMapNum)

                ' Update all taxlot polygons that underlie this mapindex polygon

                ' Set MapNumber
                _mapIndexFeature.Value(_mapIndexFields.MapNumber) = .uxMapNumber.Text

                ' Set Reliability
                Dim value As String = ConvertCodeValueDomainToCode(_mapIndexFeature.Fields, EditorExtension.MapIndexSettings.ReliabilityCodeField, .uxReliability.Text)
                Dim valueAsInteger As Integer
                If Integer.TryParse(value, valueAsInteger) Then
                    _mapIndexFeature.Value(_mapIndexFields.Reliability) = valueAsInteger
                Else
                    _mapIndexFeature.Value(_mapIndexFields.Reliability) = DBNull.Value
                End If

                ' Set MapScale
                value = ConvertCodeValueDomainToCode(_mapIndexFeature.Fields, EditorExtension.MapIndexSettings.MapScaleField, .uxScale.Text)
                If Integer.TryParse(value, valueAsInteger) Then
                    _mapIndexFeature.Value(_mapIndexFields.MapScale) = valueAsInteger
                Else
                    _mapIndexFeature.Value(_mapIndexFields.MapScale) = DBNull.Value
                End If

                ' Set Page
                value = .uxPage.Text
                If Integer.TryParse(value, valueAsInteger) Then
                    _mapIndexFeature.Value(_mapIndexFields.Page) = valueAsInteger
                Else
                    _mapIndexFeature.Value(_mapIndexFields.Page) = DBNull.Value
                End If

                ' Set ORMAPNumber
                _mapIndexFeature.Value(_mapIndexFields.ORMAPNumber) = _ormapNumber.GetORMapNum

                ' Store the edited feature
                _mapIndexFeature.Store()

            End With 'PartnerMapindexForm

            ' Update all taxlot polygons that underlie this polygon
            updateTaxlots(_mapIndexFeature)

            ' Finalize this edit
            EditorExtension.Editor.StopOperation("Edit Map Index")

            ' Update form caption
            PartnerEditMapIndexForm.Text = String.Concat("Map Index (", _ormapNumber.GetORMapNum, ")")

        Catch ex As Exception
            EditorExtension.Editor.AbortOperation()
            _application.StatusBar.Message(esriStatusBarPanes.esriStatusMain) = "MapIndex feature update aborted"

        End Try

    End Sub

    Private Sub initializeFieldIndices()
        Dim mapindexFC As IFeatureClass = DataMonitor.MapIndexFeatureLayer.FeatureClass
        Dim taxlotFC As IFeatureClass = DataMonitor.TaxlotFeatureLayer.FeatureClass

        With _mapIndexFields
            .ORMAPNumber = mapindexFC.FindField(EditorExtension.MapIndexSettings.OrmapMapNumberField)
            .Reliability = mapindexFC.FindField(EditorExtension.MapIndexSettings.ReliabilityCodeField)
            .MapScale = mapindexFC.FindField(EditorExtension.MapIndexSettings.MapScaleField)
            .MapNumber = mapindexFC.FindField(EditorExtension.MapIndexSettings.MapNumberField)
            .Page = mapindexFC.FindField(EditorExtension.MapIndexSettings.PageNumberField)
        End With

        With _taxlotFields
            .Taxlot = taxlotFC.FindField(EditorExtension.TaxLotSettings.TaxlotField)
            .Anomaly = taxlotFC.FindField(EditorExtension.TaxLotSettings.AnomalyField)
            .County = taxlotFC.FindField(EditorExtension.TaxLotSettings.CountyField)
            .MapNumber = taxlotFC.FindField(EditorExtension.TaxLotSettings.MapNumberField)
            .OrmapMapNumber = taxlotFC.FindField(EditorExtension.TaxLotSettings.OrmapMapNumberField)
            .OrmapTaxlotNumber = taxlotFC.FindField(EditorExtension.TaxLotSettings.OrmapTaxlotField)
            .MapTaxlotNumber = taxlotFC.FindField(EditorExtension.TaxLotSettings.MapTaxlotField)
            .PartialRangeCode = taxlotFC.FindField(EditorExtension.TaxLotSettings.RangePartField)
            .PartialTownshipCode = taxlotFC.FindField(EditorExtension.TaxLotSettings.TownshipPartField)
            .Quarter = taxlotFC.FindField(EditorExtension.TaxLotSettings.QuarterSectionField)
            .QuarterQuarter = taxlotFC.FindField(EditorExtension.TaxLotSettings.QuarterQuarterSectionField)
            .Range = taxlotFC.FindField(EditorExtension.TaxLotSettings.RangeField)
            .RangeDirectional = taxlotFC.FindField(EditorExtension.TaxLotSettings.RangeDirectionField)
            .Section = taxlotFC.FindField(EditorExtension.TaxLotSettings.SectionNumberField)
            .SuffixNumber = taxlotFC.FindField(EditorExtension.TaxLotSettings.MapSuffixNumberField)
            .SuffixType = taxlotFC.FindField(EditorExtension.TaxLotSettings.MapSuffixTypeField)
            .Township = taxlotFC.FindField(EditorExtension.TaxLotSettings.TownshipField)
            .TownshipDirectional = taxlotFC.FindField(EditorExtension.TaxLotSettings.TownshipDirectionField)
        End With

    End Sub

    Private Sub toggleComponentControls(ByVal enable As Boolean)
        Try
            Dim ctl As System.Windows.Forms.Control
            For Each ctl In PartnerEditMapIndexForm.Controls
                If TypeOf ctl Is ComboBox OrElse TypeOf ctl Is TextBox Then
                    ctl.Enabled = enable
                End If
                If ctl.Controls.Count > 0 Then
                    For Each subControl As Control In ctl.Controls
                        If TypeOf subControl Is TextBox OrElse TypeOf subControl Is ComboBox Then
                            subControl.Enabled = enable
                        End If
                    Next
                End If
            Next

        Catch ex As Exception
            EditorExtension.ProcessUnhandledException(ex)
        End Try
    End Sub

    Private Sub updateNumbers()
        With PartnerEditMapIndexForm
            '.Text = "Edit Map Index (" & _ormapNumber.GetORMapNum & ")"
            .uxORMAPNumberLabel.Text = _ormapNumber.GetORMapNum
            .uxORMAPNumberLabel.Refresh()
            .Refresh()
        End With
    End Sub

    Private Sub initForm()
        Dim theRow As IRow = _mapIndexFeature.Table.GetRow(_mapIndexFeature.OID)
        If theRow Is Nothing Then
            Exit Sub
        End If

        _ormapNumber = New ORMapNum
        _ormapNumber.ParseNumber(ReadValue(theRow, EditorExtension.MapIndexSettings.OrmapMapNumberField))

        If _ormapNumber.IsValidNumber Then
            initWithFeature(_mapIndexFeature)
        Else
            initEmpty()
        End If

        updateNumbers()

    End Sub

    ''' <summary>
    ''' Initialize the form for a new selection
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>Clear the form and set default value for fields in accordance with a selection that has no values set</remarks>
    Private Function initEmpty() As Boolean
        Try
            _ormapNumber = New ORMapNum
            With _ormapNumber
                .County = Integer.Parse(EditorExtension.DefaultValuesSettings.County).ToString
                .Township = String.Empty
                .TownshipDirectional = EditorExtension.DefaultValuesSettings.TownshipDirection
                .PartialTownshipCode = EditorExtension.DefaultValuesSettings.TownshipPart
                .Range = String.Empty
                .RangeDirectional = EditorExtension.DefaultValuesSettings.RangeDirection
                .PartialRangeCode = EditorExtension.DefaultValuesSettings.RangePart
                .Section = String.Empty
                .Quarter = EditorExtension.DefaultValuesSettings.QuarterSection
                .QuarterQuarter = EditorExtension.DefaultValuesSettings.QuarterQuarterSection
                .SuffixNumber = EditorExtension.DefaultValuesSettings.MapSuffixNumber
                .SuffixType = EditorExtension.DefaultValuesSettings.MapSuffixType
                .Anomaly = EditorExtension.DefaultValuesSettings.Anomaly
            End With

            With PartnerEditMapIndexForm
                resetControlContents(.Controls, True)

                .Text = "Edit Map Index - OR Map Number: (not set)"

                ' Townships
                AddCodesToCombo(EditorExtension.TaxLotSettings.TownshipField, DataMonitor.TaxlotFeatureLayer.FeatureClass, .uxTownship, _ormapNumber.Township, True)
                ' Township directionals
                AddCodesToCombo(EditorExtension.TaxLotSettings.TownshipDirectionField, DataMonitor.TaxlotFeatureLayer.FeatureClass, .uxTownshipDirectional, _ormapNumber.TownshipDirectional, True)
                ' Partial township codes
                AddCodesToCombo(EditorExtension.TaxLotSettings.TownshipPartField, DataMonitor.TaxlotFeatureLayer.FeatureClass, .uxTownshipPartial, ConvertCodeValueDomainToDescription(DataMonitor.TaxlotFeatureLayer.FeatureClass.Fields, EditorExtension.TaxLotSettings.TownshipPartField, _ormapNumber.PartialTownshipCode), True)

                ' Ranges
                AddCodesToCombo(EditorExtension.TaxLotSettings.RangeField, DataMonitor.TaxlotFeatureLayer.FeatureClass, .uxRange, _ormapNumber.Range, True)
                ' Range directionals
                AddCodesToCombo(EditorExtension.TaxLotSettings.RangeDirectionField, DataMonitor.TaxlotFeatureLayer.FeatureClass, .uxRangeDirectional, _ormapNumber.RangeDirectional, True)
                ' Partial range codes
                AddCodesToCombo(EditorExtension.TaxLotSettings.RangePartField, DataMonitor.TaxlotFeatureLayer.FeatureClass, .uxRangePartial, ConvertCodeValueDomainToDescription(DataMonitor.TaxlotFeatureLayer.FeatureClass.Fields, EditorExtension.TaxLotSettings.RangePartField, _ormapNumber.PartialRangeCode), True)

                ' Sections
                AddCodesToCombo(EditorExtension.TaxLotSettings.SectionNumberField, DataMonitor.TaxlotFeatureLayer.FeatureClass, .uxSection, _ormapNumber.Section, True)
                ' Quarters
                AddCodesToCombo(EditorExtension.TaxLotSettings.QuarterSectionField, DataMonitor.TaxlotFeatureLayer.FeatureClass, .uxSectionQuarter, _ormapNumber.Quarter, True)
                ' Quarters of quarter
                AddCodesToCombo(EditorExtension.TaxLotSettings.QuarterQuarterSectionField, DataMonitor.TaxlotFeatureLayer.FeatureClass, .uxSectionQtrQtr, _ormapNumber.QuarterQuarter, True)

                ' Counties
                AddCodesToCombo(EditorExtension.TaxLotSettings.CountyField, DataMonitor.TaxlotFeatureLayer.FeatureClass, .uxCounty, ConvertCodeValueDomainToDescription(DataMonitor.TaxlotFeatureLayer.FeatureClass.Fields, EditorExtension.TaxLotSettings.CountyField, Integer.Parse(_ormapNumber.County).ToString), True)
                ' Suffix number
                .uxSuffixNumber.Text = _ormapNumber.SuffixNumber
                ' Suffix types
                AddCodesToCombo(EditorExtension.TaxLotSettings.MapSuffixTypeField, DataMonitor.TaxlotFeatureLayer.FeatureClass, .uxSuffixType, _ormapNumber.SuffixType, True)
                ' Reliabilities
                AddCodesToCombo(EditorExtension.MapIndexSettings.ReliabilityCodeField, DataMonitor.MapIndexFeatureLayer.FeatureClass, .uxReliability, String.Empty, True)
                ' Scales
                AddCodesToCombo(EditorExtension.MapIndexSettings.MapScaleField, DataMonitor.MapIndexFeatureLayer.FeatureClass, .uxScale, String.Empty, True)
                ' Anomaly
                .uxAnomaly.Text = _ormapNumber.Anomaly
                ' Page
                .uxPage.Text = "0"

            End With

            Return True
        Catch ex As Exception
            EditorExtension.ProcessUnhandledException(ex)
            Return False
        End Try
    End Function

    Private Function initWithFeature(ByVal feature As IFeature) As Boolean
        Try
            Dim thisRow As IRow = feature.Table.GetRow(feature.OID)
            With PartnerEditMapIndexForm
                resetControlContents(.Controls, True)

                .Text = String.Concat("Edit Map Index - OR Map Number: ", _ormapNumber.GetORMapNum)

                ' Townships
                AddCodesToCombo(EditorExtension.TaxLotSettings.TownshipField, DataMonitor.TaxlotFeatureLayer.FeatureClass, .uxTownship, _ormapNumber.Township, True)
                ' Township directionals
                AddCodesToCombo(EditorExtension.TaxLotSettings.TownshipDirectionField, DataMonitor.TaxlotFeatureLayer.FeatureClass, .uxTownshipDirectional, _ormapNumber.TownshipDirectional, True)
                ' Partial township codes
                AddCodesToCombo(EditorExtension.TaxLotSettings.TownshipPartField, DataMonitor.TaxlotFeatureLayer.FeatureClass, .uxTownshipPartial, "0" & _ormapNumber.PartialTownshipCode, True)

                ' Ranges
                AddCodesToCombo(EditorExtension.TaxLotSettings.RangeField, DataMonitor.TaxlotFeatureLayer.FeatureClass, .uxRange, _ormapNumber.Range, True)
                ' Partial range codes
                AddCodesToCombo(EditorExtension.TaxLotSettings.RangePartField, DataMonitor.TaxlotFeatureLayer.FeatureClass, .uxRangePartial, "0" & _ormapNumber.PartialRangeCode, True)
                ' Range directionals
                AddCodesToCombo(EditorExtension.TaxLotSettings.RangeDirectionField, DataMonitor.TaxlotFeatureLayer.FeatureClass, .uxRangeDirectional, _ormapNumber.RangeDirectional, True)

                ' Sections
                AddCodesToCombo(EditorExtension.TaxLotSettings.SectionNumberField, DataMonitor.TaxlotFeatureLayer.FeatureClass, .uxSection, _ormapNumber.Section, True)
                ' Quarters
                AddCodesToCombo(EditorExtension.TaxLotSettings.QuarterSectionField, DataMonitor.TaxlotFeatureLayer.FeatureClass, .uxSectionQuarter, _ormapNumber.Quarter, True)
                ' Quarters of quarter
                AddCodesToCombo(EditorExtension.TaxLotSettings.QuarterQuarterSectionField, DataMonitor.TaxlotFeatureLayer.FeatureClass, .uxSectionQtrQtr, _ormapNumber.QuarterQuarter, True)

                ' Counties
                AddCodesToCombo(EditorExtension.MapIndexSettings.CountyField, DataMonitor.TaxlotFeatureLayer.FeatureClass, .uxCounty, ConvertCodeValueDomainToDescription(DataMonitor.TaxlotFeatureLayer.FeatureClass.Fields, EditorExtension.TaxLotSettings.CountyField, Integer.Parse(_ormapNumber.County).ToString), True)
                ' Suffix number
                .uxSuffixNumber.Text = _ormapNumber.SuffixNumber
                ' Suffix types
                AddCodesToCombo(EditorExtension.TaxLotSettings.MapSuffixTypeField, DataMonitor.TaxlotFeatureLayer.FeatureClass, .uxSuffixType, ConvertCodeValueDomainToDescription(DataMonitor.TaxlotFeatureLayer.FeatureClass.Fields, EditorExtension.TaxLotSettings.MapSuffixTypeField, _ormapNumber.SuffixType), True)
                ' Reliabilities
                AddCodesToCombo(EditorExtension.MapIndexSettings.ReliabilityCodeField, DataMonitor.MapIndexFeatureLayer.FeatureClass, .uxReliability, ReadValue(thisRow, EditorExtension.MapIndexSettings.ReliabilityCodeField), True)
                ' Scales
                AddCodesToCombo(EditorExtension.MapIndexSettings.MapScaleField, DataMonitor.MapIndexFeatureLayer.FeatureClass, .uxScale, ReadValue(thisRow, EditorExtension.MapIndexSettings.MapScaleField), True)
                ' Page
                .uxPage.Text = ReadValue(thisRow, EditorExtension.MapIndexSettings.PageNumberField)
                ' Anomaly
                .uxAnomaly.Text = _ormapNumber.Anomaly

                ' Preview
                .uxMapNumber.Text = ReadValue(thisRow, EditorExtension.MapIndexSettings.MapNumberField)

            End With
            Return True
        Catch ex As Exception
            EditorExtension.ProcessUnhandledException(ex)
            Return False
        End Try

    End Function

    Private Overloads Sub resetControlContents(ByVal inControls As ControlCollection)
        resetControlContents(inControls, True)
    End Sub

    Private Overloads Sub resetControlContents(ByVal inControls As ControlCollection, ByVal inRecursive As Boolean)
        Dim ctl As Control
        For Each ctl In inControls
            If ctl.Controls.Count > 0 Then
                If inRecursive Then
                    resetControlContents(ctl.Controls, inRecursive)
                End If
            ElseIf TypeOf ctl Is TextBox Then
                ctl.Text = String.Empty
            ElseIf TypeOf ctl Is ComboBox Then
                Dim cmb As ComboBox = DirectCast(ctl, ComboBox)
                cmb.Items.Clear()
            End If
        Next ctl
    End Sub

    Private Sub updateTaxlots(ByVal theMapIndexFeature As IFeature)
        Try
            _application.StatusBar.Message(esriStatusBarPanes.esriStatusMain) = "Finding underlying Taxlot features..."

            ' Find any taxlots that are underneath the map index polygon
            Dim thisSpatialQuery As ISpatialFilter = New SpatialFilter
            thisSpatialQuery.Geometry = theMapIndexFeature.ShapeCopy
            thisSpatialQuery.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains
            Dim thisFeatureSelection As IFeatureCursor = DataMonitor.TaxlotFeatureLayer.FeatureClass.Update(thisSpatialQuery, False)

            Dim thisTaxlotFeature As IFeature = thisFeatureSelection.NextFeature
            ' Exit if there are no underlying taxlot features to update.
            If thisTaxlotFeature Is Nothing Then Exit Sub

            ' Loop through the selected features
            Dim taxlot As String
            Dim mapNumber As String
            Do While thisTaxlotFeature IsNot Nothing

                _application.StatusBar.Message(esriStatusBarPanes.esriStatusMain) = "Updating underlying Taxlot " & CStr(thisTaxlotFeature.Value(_taxlotFields.Taxlot))

                ' Gets the formatted taxlot value
                If IsDBNull(thisTaxlotFeature.Value(_taxlotFields.Taxlot)) Then
                    taxlot = New String("0"c, ORMapNum.GetTaxlotFieldLength)
                Else
                    taxlot = AddLeadingZeros(CStr(thisTaxlotFeature.Value(_taxlotFields.Taxlot)), ORMapNum.GetTaxlotFieldLength)
                End If
                ' Special interest has been removed 
                ' See Tracker 1922332 on http://sourceforge.net/tracker/index.php?func=detail&aid=1922332&group_id=151824&atid=782248

                ' Get mapnumber value

                If IsDBNull(thisTaxlotFeature.Value(_mapIndexFields.MapNumber)) Then
                    mapNumber = String.Empty
                Else
                    mapNumber = CStr(thisTaxlotFeature.Value(_mapIndexFields.MapNumber))
                End If

                '' Copy new attributes to  the taxlot table
                'Dim mapTaxlotID As String = _ormapNumber.GetORMapNum & taxlot
                'Dim countyCode As Short = CShort(EditorExtension.DefaultValuesSettings.County)
                'Dim mapTaxlotValue As String = String.Empty
                'Dim taxlotAsInteger As Integer = 0

                'Select Case countyCode
                '    Case 1 To 19, 21 To 36
                '        mapTaxlotValue = GenerateMapTaxlotValue(mapTaxlotID, EditorExtension.TaxLotSettings.MapTaxlotFormatMask)
                '    Case 20
                '        mapTaxlotValue = mapNumber.TrimEnd(CChar(mapNumber.Substring(0, 8))) & taxlot
                'End Select
                'With thisTaxlotFeature
                '    .Value(_taxlotFields.County) = _ormapNumber.County
                '    .Value(_taxlotFields.Township) = _ormapNumber.Township
                '    .Value(_taxlotFields.PartialTownshipCode) = _ormapNumber.PartialTownshipCode
                '    .Value(_taxlotFields.TownshipDirectional) = _ormapNumber.TownshipDirectional
                '    .Value(_taxlotFields.Range) = _ormapNumber.Range
                '    .Value(_taxlotFields.PartialRangeCode) = _ormapNumber.PartialRangeCode
                '    .Value(_taxlotFields.RangeDirectional) = _ormapNumber.RangeDirectional
                '    .Value(_taxlotFields.Section) = _ormapNumber.Section
                '    .Value(_taxlotFields.Quarter) = _ormapNumber.Quarter
                '    .Value(_taxlotFields.QuarterQuarter) = _ormapNumber.QuarterQuarter
                '    .Value(_taxlotFields.SuffixType) = _ormapNumber.SuffixType
                '    .Value(_taxlotFields.SuffixNumber) = _ormapNumber.SuffixNumber
                '    .Value(_taxlotFields.Anomaly) = _ormapNumber.Anomaly
                '    .Value(_taxlotFields.MapNumber) = theMapIndexFeature.Value(_mapIndexFields.MapNumber)
                '    .Value(_taxlotFields.OrmapMapNumber) = _ormapNumber.GetOrmapMapNumber
                '    If Integer.TryParse(taxlot, taxlotAsInteger) Then
                '        .Value(_taxlotFields.Taxlot) = taxlotAsInteger
                '    Else
                '        .Value(_taxlotFields.Taxlot) = taxlot.Trim
                '    End If
                '    ' Special interest field was updated here see above
                '    .Value(_taxlotFields.MapTaxlotNumber) = mapTaxlotValue.Trim
                '    .Value(_taxlotFields.OrmapTaxlotNumber) = String.Concat(_ormapNumber.GetORMapNum, taxlot)
                'End With
                thisTaxlotFeature.Store()
                thisTaxlotFeature = thisFeatureSelection.NextFeature
            Loop

            _application.StatusBar.Message(esriStatusBarPanes.esriStatusMain) = "Underlying taxlot features updated"

            thisTaxlotFeature = Nothing
            thisFeatureSelection = Nothing
            thisSpatialQuery = Nothing

        Catch ex As Exception
            _application.StatusBar.Message(esriStatusBarPanes.esriStatusMain) = String.Empty
            Throw

        End Try

    End Sub

#Region "State Machine"

    ' TODO: [NIS] Embed URL reference to statechart in the XML comments for these methods.

    Private Sub CondState1()
        ' Evaluate condition
        If _ormapNumber.IsValidNumber AndAlso _
                EditorExtension.Editor.EditState = esriEditState.esriStateEditing Then 'AndAlso _
            'CStr(_mapIndexFeature.Value(_mapIndexFields.ORMAPNumber)) <> _ormapNumber.GetORMapNum Then
            StateS1_1(StatePassageType.Entering)
        Else
            StateS1_2(StatePassageType.Entering)
        End If
    End Sub

    Private Sub StateS1(ByVal statePassage As StatePassageType)
        Select Case statePassage
            Case StatePassageType.Entering
                ' Do actions
                ' (none)
                ' Do substate transitions
                StateS1_2(StatePassageType.Entering)
            Case StatePassageType.Exiting
                ' Do actions
                ' (none)
                ' Do substate transitions
                StateS1_1(StatePassageType.Exiting)
                StateS1_2(StatePassageType.Exiting)
        End Select
    End Sub

    Private Sub StateS1_1(ByVal statePassage As StatePassageType)
        '[Tool Enabled...]
        Select Case statePassage
            Case StatePassageType.Entering
                setState(CommandStateType.Enabled)
                ' Do actions
                ' Enable OK/Apply (edit) buttons
                _partnerEditMapIndexForm.uxOK.Enabled = True
                _partnerEditMapIndexForm.uxApply.Enabled = True
                ' Do substate transitions
                ' (none)
            Case StatePassageType.Exiting
                ' Do actions
                ' (none)
                ' Do substate transitions
                ' (none)
        End Select
    End Sub

    Private Sub StateS1_2(ByVal statePassage As StatePassageType)
        '[Not a valid ORMapNumber...]

        Select Case statePassage
            Case StatePassageType.Entering
                setState(CommandStateType.Disabled)
                ' Do actions
                ' Disable OK/Apply (edit) buttons
                _partnerEditMapIndexForm.uxOK.Enabled = False
                _partnerEditMapIndexForm.uxApply.Enabled = False
                ' Do substate transitions
                ' (none)
            Case StatePassageType.Exiting
                ' Do actions
                ' (none)
                ' Do substate transitions
                ' (none)
        End Select
    End Sub

    Private Sub TransitionE1()
        CondState1()
    End Sub

    Private Sub TransitionE2()
        'CondState1()
        StateS1_2(StatePassageType.Entering)
    End Sub

#End Region

#End Region

#End Region

#Region "Inherited Class Members"

#Region "Properties"

    ''' <summary>
    ''' Called by ArcMap once per second to check if the command is enabled.
    ''' </summary>
    ''' <remarks>WARNING: Do not put computation-intensive code here.</remarks>
    Public Overrides ReadOnly Property Enabled() As Boolean
        Get
            Dim canEnable As Boolean
            canEnable = EditorExtension.CanEnableExtendedEditing
            Return canEnable
        End Get
    End Property

#End Region

#Region "Methods"

    ''' <summary>
    ''' Called by ArcMap when this command is created.
    ''' </summary>
    ''' <param name="hook">A generic <c>Object</c> hook to an instance of the application.</param>
    ''' <remarks>The application hook may not point to an <c>IMxApplication</c> object.</remarks>
    Public Overrides Sub OnCreate(ByVal hook As Object)
        If Not hook Is Nothing Then

            'Disable if it is not ArcMap
            If TypeOf hook Is IMxApplication Then
                _application = DirectCast(hook, IApplication)
                setPartnerEditMapIndexForm(New EditMapIndexForm)
                MyBase.m_enabled = True
            Else
                MyBase.m_enabled = False
            End If
        End If

        ' NOTE: Add other initialization code here...

    End Sub

    Public Overrides Sub OnClick()
        DoButtonOperation()
    End Sub


#End Region

#End Region

#Region "Implemented Interface Members"


#Region "IDisposable Interface Implementation"

    Private _isDuringDispose As Boolean ' Used to track whether Dispose() has been called and is in progress.

    ''' <summary>
    ''' Dispose of managed and unmanaged resources.
    ''' </summary>
    ''' <param name="disposing">True or False.</param>
    ''' <remarks>
    ''' <para>Member of System::IDisposable.</para>
    ''' <para>Dispose executes in two distinct scenarios. 
    ''' If disposing equals true, the method has been called directly
    ''' or indirectly by a user's code. Managed and unmanaged resources
    ''' can be disposed.</para>
    ''' <para>If disposing equals false, the method has been called by the 
    ''' runtime from inside the finalizer and you should not reference 
    ''' other objects. Only unmanaged resources can be disposed.</para>
    ''' </remarks>
    Friend Sub Dispose(ByVal disposing As Boolean)
        ' Check to see if Dispose has already been called.
        If Not Me._isDuringDispose Then

            ' Flag that disposing is in progress.
            Me._isDuringDispose = True

            If disposing Then
                ' Free managed resources when explicitly called.

                ' Dispose managed resources here.
                '   e.g. component.Dispose()

            End If

            ' Free "native" (shared unmanaged) resources, whether 
            ' explicitly called or called by the runtime.

            ' Call the appropriate methods to clean up 
            ' unmanaged resources here.
            _bitmapResourceName = Nothing
            MyBase.m_bitmap = Nothing

            ' Flag that disposing has been finished.
            _isDuringDispose = False

        End If

    End Sub

#Region " IDisposable Support "

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region

#End Region

#End Region

#Region "Other Members"

#Region "COM GUIDs"
    ' These  GUIDs provide the COM identity for this class 
    ' and its COM interfaces. If you change them, existing 
    ' clients will no longer be able to access the class.
    Public Const ClassId As String = "2c5ecd6a-2175-4544-9a25-6281febb6d67"
    Public Const InterfaceId As String = "88034039-6ce9-46ed-973e-ffe70c3a3238"
    Public Const EventsId As String = "6432ad18-ea02-44c9-9589-0ef8cfb6898a"
#End Region

#Region "COM Registration Function(s)"
    <ComRegisterFunction(), ComVisibleAttribute(False)> _
    Public Shared Sub RegisterFunction(ByVal registerType As Type)
        ' Required for ArcGIS Component Category Registrar support
        ArcGISCategoryRegistration(registerType)

        'Add any COM registration code after the ArcGISCategoryRegistration() call

    End Sub

    <ComUnregisterFunction(), ComVisibleAttribute(False)> _
    Public Shared Sub UnregisterFunction(ByVal registerType As Type)
        ' Required for ArcGIS Component Category Registrar support
        ArcGISCategoryUnregistration(registerType)

        'Add any COM unregistration code after the ArcGISCategoryUnregistration() call

    End Sub

#Region "ArcGIS Component Category Registrar generated code"
    ''' <summary>
    ''' Required method for ArcGIS Component Category registration -
    ''' Do not modify the contents of this method with the code editor.
    ''' </summary>
    Private Shared Sub ArcGISCategoryRegistration(ByVal registerType As Type)
        Dim regKey As String = String.Format("HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID)
        MxCommands.Register(regKey)

    End Sub

    ''' <summary>
    ''' Required method for ArcGIS Component Category registration -
    ''' Do not modify the contents of this method with the code editor.
    ''' </summary>
    Private Shared Sub ArcGISCategoryUnregistration(ByVal registerType As Type)
        Dim regKey As String = String.Format("HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID)
        MxCommands.Unregister(regKey)

    End Sub

#End Region
#End Region

#End Region


End Class




