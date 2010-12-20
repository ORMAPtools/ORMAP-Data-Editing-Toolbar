#Region "Copyright 2008 ORMAP Tech Group"

' File:  LocateFeature.vb
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
'SCC revision number: $Revision: 443 $
'Date of Last Change: $Date: 2010-06-07 13:02:14 -0700 (Mon, 07 Jun 2010) $
#End Region

#Region "Imported Namespaces"
Imports System.Drawing
Imports System.Environment
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports ESRI.ArcGIS.ADF.BaseClasses
Imports ESRI.ArcGIS.ADF.CATIDs
Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Framework
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Geometry
Imports OrmapTaxlotEditing.DataMonitor
Imports OrmapTaxlotEditing.SpatialUtilities
Imports OrmapTaxlotEditing.StringUtilities
Imports OrmapTaxlotEditing.Utilities
#End Region

''' <summary>
''' Provides an ArcMap Command with functionality to 
''' allow users to find and zoom to MapIndex or Taxlot 
''' features and/or specify a MapIndex (Map) to override the
''' auto attributing of features via the on create event.
''' </summary>
''' <remarks><seealso cref="LocateFeatureDockWin"/><seealso cref="LocateFeatureUserControl"/></remarks>
<ComVisible(True)> _
<ComClass(LocateFeature.ClassId, LocateFeature.InterfaceId, LocateFeature.EventsId), _
ProgId("ORMAPTaxlotEditing.LocateFeature")> _
Public NotInheritable Class LocateFeature
    Inherits BaseCommand
    Implements IDisposable

#Region "Class-Level Constants and Enumerations (none)"
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
        MyBase.m_caption = "LocateFeature"   'localizable text 
        MyBase.m_message = "Locate a Taxlot or Mapindex"   'localizable text 
        MyBase.m_toolTip = "Locate Taxlot or Mapindex" 'localizable text 
        MyBase.m_name = MyBase.m_category & "_LocateFeature"  'unique id, non-localizable (e.g. "MyCategory_ArcMapCommand")

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

#Region "Fields"

    Private _application As IApplication
    Private _bitmapResourceName As String
    Private _locateFeatureDockWinMgr As IDockableWindowManager
    Private _locateFeatureDockWin As IDockableWindow

#End Region

#Region "Properties"

    Private WithEvents _partnerLocateFeatureUserControl As LocateFeatureUserControl

    Friend ReadOnly Property PartnerLocateFeatureUserControl() As LocateFeatureUserControl
        Get
            If _partnerLocateFeatureUserControl Is Nothing OrElse _partnerLocateFeatureUserControl.IsDisposed Then
                setPartnerLocateFeatureUserControl(New LocateFeatureUserControl())
            End If
            Return _partnerLocateFeatureUserControl
        End Get
    End Property

    Private Sub setPartnerLocateFeatureUserControl(ByVal value As LocateFeatureUserControl)
        If value IsNot Nothing Then
            _partnerLocateFeatureUserControl = value
            ' Subscribe to partner form events.
            AddHandler _partnerLocateFeatureUserControl.uxMapNumber.TextChanged, AddressOf uxMapNumber_TextChanged
            AddHandler _partnerLocateFeatureUserControl.uxTaxlot.Enter, AddressOf uxTaxlot_Enter
            AddHandler _partnerLocateFeatureUserControl.uxFind.Click, AddressOf uxFind_Click
            AddHandler _partnerLocateFeatureUserControl.uxHelp.Click, AddressOf uxHelp_Click
            AddHandler _partnerLocateFeatureUserControl.uxSelectFeatures.CheckedChanged, AddressOf uxSelectFeatures_CheckedChanged
            AddHandler _partnerLocateFeatureUserControl.uxSetAttributeMode.Click, AddressOf uxSetAttributeMode_Click
            AddHandler _partnerLocateFeatureUserControl.uxTimer.Tick, AddressOf uxTimer_Tick
            AddHandler _partnerLocateFeatureUserControl.uxORMAPProperties.Click, AddressOf uxORMAPProperties_Click
            AddHandler _partnerLocateFeatureUserControl.uxSetDefinitionQuery.Click, AddressOf uxSetDefinitionQuery_Click
            AddHandler _partnerLocateFeatureUserControl.uxClearDefinitionQuery.Click, AddressOf uxClearDefinitionQuery_Click
        Else
            ' Unsubscribe to partner form events.
            RemoveHandler _partnerLocateFeatureUserControl.uxMapNumber.TextChanged, AddressOf uxMapNumber_TextChanged
            RemoveHandler _partnerLocateFeatureUserControl.uxTaxlot.Enter, AddressOf uxTaxlot_Enter
            RemoveHandler _partnerLocateFeatureUserControl.uxFind.Click, AddressOf uxFind_Click
            RemoveHandler _partnerLocateFeatureUserControl.uxHelp.Click, AddressOf uxHelp_Click
            RemoveHandler _partnerLocateFeatureUserControl.uxSelectFeatures.CheckedChanged, AddressOf uxSelectFeatures_CheckedChanged
            RemoveHandler _partnerLocateFeatureUserControl.uxSetAttributeMode.Click, AddressOf uxSetAttributeMode_Click
            RemoveHandler _partnerLocateFeatureUserControl.uxTimer.Tick, AddressOf uxTimer_Tick
            RemoveHandler _partnerLocateFeatureUserControl.uxORMAPProperties.Click, AddressOf uxORMAPProperties_Click
            RemoveHandler _partnerLocateFeatureUserControl.uxSetDefinitionQuery.Click, AddressOf uxSetDefinitionQuery_Click
            RemoveHandler _partnerLocateFeatureUserControl.uxClearDefinitionQuery.Click, AddressOf uxClearDefinitionQuery_Click
        End If
    End Sub

    Private _mapIndexHasBeenChanged As Boolean = False

    Private _uxSelectFeaturesChecked As Boolean = False
    Friend Property uxSelectFeaturesChecked() As Boolean
        Get
            Return _uxSelectFeaturesChecked
        End Get
        Set(ByVal value As Boolean)
            _uxSelectFeaturesChecked = value
        End Set
    End Property


    Private WithEvents _partnerLocateDefinitionForm As MapDefinitionForm
    Friend ReadOnly Property PartnerMapDefinitionForm() As MapDefinitionForm
        Get
            If _partnerLocateDefinitionForm Is Nothing OrElse _partnerLocateDefinitionForm.IsDisposed Then
                setLocateDefinitionForm(New MapDefinitionForm())
            End If
            Return _partnerLocateDefinitionForm
        End Get
    End Property


    Private Sub setLocateDefinitionForm(ByVal value As MapDefinitionForm)
        If value IsNot Nothing Then
            _partnerLocateDefinitionForm = value
            ' Subscribe to partner form events.
            AddHandler _partnerLocateDefinitionForm.uxCancelSetDefinitionQuery.Click, AddressOf CancelLocateDefinitionQuery_Click
            AddHandler _partnerLocateDefinitionForm.uxSetMapDefinitionQuery.Click, AddressOf SetMapDefinitionQuery_Click
            AddHandler _partnerLocateDefinitionForm.uxMapNumber.TextChanged, AddressOf uxMapNumberTextBox_TextChanged
        Else
            ' Unsubscribe to partner form events.
            RemoveHandler _partnerLocateDefinitionForm.uxCancelSetDefinitionQuery.Click, AddressOf CancelLocateDefinitionQuery_Click
            RemoveHandler _partnerLocateDefinitionForm.uxSetMapDefinitionQuery.Click, AddressOf SetMapDefinitionQuery_Click
            RemoveHandler _partnerLocateDefinitionForm.uxMapNumber.TextChanged, AddressOf uxMapNumberTextBox_TextChanged
        End If
    End Sub


#End Region

#Region "Event Handlers"

    ''' <summary>
    ''' Sets the data source for the MapNumber on the locate feature user control (dockable window).  
    ''' </summary>
    ''' <remarks>MapIndex is the source of the MapNumber list used.</remarks>
    Private Sub PartnerLocateFeatureForm_Load() ' partnerLocateFeatureUserControl

        With PartnerLocateFeatureUserControl
            Try
                .UseWaitCursor = True

                '' NOTE: [SC] Calculating a AutoCompleteCustomSource using the List of strings is considerably faster than
                '' adding with the .add method.
                Dim mapNumberStringList As List(Of String) = New List(Of String)
                Dim theMapIndexFClass As IFeatureClass = MapIndexFeatureLayer.FeatureClass
                Dim theQueryFilter As IQueryFilter = New QueryFilter
                theQueryFilter.SubFields = EditorExtension.MapIndexSettings.MapNumberField
                Dim theFeatCursor As IFeatureCursor = theMapIndexFClass.Search(theQueryFilter, True)
                Dim theFeature As IFeature = theFeatCursor.NextFeature
                If theFeature Is Nothing Then Exit Sub
                Dim theFieldIdx As Integer = theFeature.Fields.FindField(EditorExtension.MapIndexSettings.MapNumberField)
                Do Until theFeature Is Nothing
                    Dim theMapNumberVal As String = theFeature.Value(theFieldIdx).ToString
                    If Not mapNumberStringList.Contains(theMapNumberVal) Then
                        mapNumberStringList.Add(theMapNumberVal)
                    End If
                    theFeature = theFeatCursor.NextFeature
                Loop

                ' Populate the combobox from the List object
                .uxMapNumber.AutoCompleteCustomSource.AddRange(mapNumberStringList.ToArray)

                ' Set control defaults
                .uxMapNumber.Text = String.Empty
                .uxTaxlot.Text = String.Empty
                If uxSelectFeaturesChecked Then .uxSelectFeatures.Checked = True

            Finally
                .UseWaitCursor = False
            End Try
        End With

    End Sub

    ''' <summary>
    ''' Checks once a second to make sure the user is still editing and allowed to auto update all fields.  
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub uxTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) ' partnerLocateFeatureUserControl
        PartnerLocateFeatureUserControl.uxEditingGroupBox.Enabled = EditorExtension.AllowedToAutoUpdateAllFields
    End Sub

    ''' <summary>
    ''' Sets the attribute mode to use the specified MapNumber to auto-populate features instead of the drill down to the MapIndex feature class.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub uxSetAttributeMode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) ' partnerLocateFeatureUserControl

        With PartnerLocateFeatureUserControl

            If EditorExtension.OverrideAutoAttribute Then
                .uxSetAttributeMode.Text = "Set &Manual"
                .uxAttributeMode.Text = "Auto"
                EditorExtension.OverrideAutoAttribute = False
                .uxCurrentlyAttLbl.Visible = False
                .uxCurrentlyAttNum.Visible = False
                EditorExtension.OverrideMapScale = String.Empty
                EditorExtension.OverrideORMapNumber = String.Empty
                EditorExtension.OverrideMapNumber = String.Empty

            Else

                Try
                    .UseWaitCursor = True

                    Dim uxMapnumber As TextBox = .uxMapNumber
                    Dim theMapNumber As String = uxMapnumber.Text.Trim

                    If theMapNumber = String.Empty OrElse Not uxMapnumber.AutoCompleteCustomSource.Contains(theMapNumber) Then
                        .UseWaitCursor = False
                        MessageBox.Show("Invalid MapNumber. Please try again.", "Locate Feature", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Exit Sub
                    End If

                    ' Get the ORMapNumber and MapScale from the MapIndex based on the MapNumber (could use GetMapScale but since we have to get the ORMapNumber also...??)
                    Dim theQueryFilter As IQueryFilter = New QueryFilter
                    Dim theWhereClause As String = "[" & EditorExtension.MapIndexSettings.MapNumberField & "] = '" & theMapNumber & "'"
                    Dim theMapIndexFeatureClass As IFeatureClass = MapIndexFeatureLayer.FeatureClass
                    theQueryFilter.WhereClause = formatWhereClause(theWhereClause, theMapIndexFeatureClass)
                    Dim theFeatCursor As IFeatureCursor = theMapIndexFeatureClass.Search(theQueryFilter, True)
                    Dim thisFeature As IFeature = theFeatCursor.NextFeature
                    Dim theMapScaleFieldIdx As Integer = thisFeature.Fields.FindField(EditorExtension.MapIndexSettings.MapScaleField)
                    Dim theORMapNumberFieldIdx As Integer = thisFeature.Fields.FindField(EditorExtension.MapIndexSettings.OrmapMapNumberField)
                    EditorExtension.OverrideMapScale = thisFeature.Value(theMapScaleFieldIdx).ToString()
                    EditorExtension.OverrideORMapNumber = thisFeature.Value(theORMapNumberFieldIdx).ToString()
                    EditorExtension.OverrideMapNumber = theMapNumber

                    .uxSetAttributeMode.Text = "Set &Auto"
                    .uxAttributeMode.Text = "Manual Override"
                    EditorExtension.OverrideAutoAttribute = True
                    .uxCurrentlyAttLbl.Visible = True
                    .uxCurrentlyAttNum.Visible = True
                    .uxCurrentlyAttNum.Text = theMapNumber

                Finally
                    .UseWaitCursor = False
                End Try

            End If

        End With

    End Sub

    ''' <summary>
    ''' Get's a list of taxlots for the specified MapNumber.
    ''' </summary>
    ''' <remarks>Not called unless the user clicks in the uxTaxlot textbox</remarks>
    Private Sub uxTaxlot_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) ' partnerLocateFeatureUserControl

        ' Only update the Taxlot data source if the mapnumber has changed... otherwise recycle the data source.
        If Not _mapIndexHasBeenChanged Then Exit Sub

        With PartnerLocateFeatureUserControl
            Try
                .UseWaitCursor = True

                Dim uxMapnumber As TextBox = .uxMapNumber
                Dim uxTaxlot As TextBox = .uxTaxlot

                If uxMapnumber.AutoCompleteCustomSource.Contains(uxMapnumber.Text.Trim) Then

                    Dim theMapNumberVal As String = uxMapnumber.Text.Trim
                    If theMapNumberVal = String.Empty Then Exit Sub

                    Dim theTaxlotFClass As IFeatureClass = TaxlotFeatureLayer.FeatureClass

                    Dim theQueryFilter As IQueryFilter = New QueryFilter
                    theQueryFilter.SubFields = EditorExtension.TaxLotSettings.MapNumberField & ", " & EditorExtension.TaxLotSettings.TaxlotField
                    '' NOTE: [SC] The following whereclause was slower than querying the entire recordset and then filtering 
                    '' results (see feature loop below).
                    'Dim theWhereClause As String
                    'theWhereClause = "[" & EditorExtension.TaxLotSettings.MapNumberField & "] = '" & theMapNumberVal & "'"
                    'theQueryFilter.WhereClause = formatWhereClause(theWhereClause, theTaxlotFClass)

                    '' NOTE: [SC] Calculating a AutoCompleteCustomSource using the List of strings is considerably faster than
                    '' adding with the .add method.
                    Dim taxlotStringList As List(Of String) = New List(Of String)
                    Dim theFeatCursor As IFeatureCursor = theTaxlotFClass.Search(theQueryFilter, True)
                    Dim theFeature As IFeature = theFeatCursor.NextFeature
                    If theFeature Is Nothing Then Exit Sub
                    Dim theTaxlotFieldIdx As Integer = theFeature.Fields.FindField(EditorExtension.TaxLotSettings.TaxlotField)
                    Dim theMapNumberFieldIdx As Integer = theFeature.Fields.FindField(EditorExtension.MapIndexSettings.MapNumberField)

                    Do Until theFeature Is Nothing
                        Dim theTaxlotVal As String = theFeature.Value(theTaxlotFieldIdx).ToString
                        If Not uxTaxlot.AutoCompleteCustomSource.Contains(theTaxlotVal) AndAlso theMapNumberVal = theFeature.Value(theMapNumberFieldIdx).ToString Then
                            taxlotStringList.Add(theTaxlotVal)
                        End If
                        theFeature = theFeatCursor.NextFeature
                    Loop

                    uxTaxlot.AutoCompleteCustomSource.AddRange(taxlotStringList.ToArray)
                    _mapIndexHasBeenChanged = False '-- reset 

                End If

            Finally
                .UseWaitCursor = False
            End Try

        End With
    End Sub

    ''' <summary>
    ''' Checks to see if the user is enterning a mapNumber in lowercase or uppercase and modifies thier entry as needed to match the data source.
    ''' </summary>
    ''' <remarks>If a user enters 221014db000 but the data source displays 221014DB000 then the text the user entered will be changed to match what's in the source (ie 221014DB000)</remarks>
    Private Sub uxMapNumber_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) ' partnerLocateFeatureUserControl

        Dim uxMapNumber As TextBox = PartnerLocateFeatureUserControl.uxMapNumber
        Dim uxTaxlot As TextBox = PartnerLocateFeatureUserControl.uxTaxlot

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

        If uxMapNumber.AutoCompleteCustomSource.Contains(uxMapNumber.Text.Trim) Then
            _mapIndexHasBeenChanged = True
            uxTaxlot.AutoCompleteCustomSource.Clear()
            uxTaxlot.Text = String.Empty
        End If

    End Sub

    ''' <summary>
    ''' Zooms to the MapNumber or Taxlot.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub uxFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) ' partnerLocateFeatureUserControl

        With PartnerLocateFeatureUserControl
            Try
                .UseWaitCursor = True

                Dim uxMapnumber As TextBox = PartnerLocateFeatureUserControl.uxMapNumber
                Dim theMapNumberVal As String = uxMapnumber.Text.Trim

                If theMapNumberVal = String.Empty OrElse Not uxMapnumber.AutoCompleteCustomSource.Contains(theMapNumberVal) Then
                    .UseWaitCursor = False
                    MessageBox.Show("Invalid MapNumber. Please try again.", "Locate Feature", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Exit Sub
                End If

                Dim uxTaxlot As TextBox = .uxTaxlot
                Dim theTaxlotVal As String = uxTaxlot.Text.Trim

                If theTaxlotVal <> String.Empty AndAlso Not uxTaxlot.AutoCompleteCustomSource.Contains(theTaxlotVal) Then
                    .UseWaitCursor = False
                    MessageBox.Show("Invalid Taxlot. Please try again.", "Locate Feature", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Exit Sub
                End If

                '-- Make sure these are set... turning the light bulb on/off will reset these.
                If MapIndexFeatureLayer Is Nothing Then CheckValidMapIndexDataProperties()
                If TaxlotFeatureLayer Is Nothing Then CheckValidTaxlotDataProperties()

                Dim theQueryFilter As IQueryFilter = New QueryFilter
                Dim theXFlayer As IFeatureLayer = Nothing '-- Set as either the MapIndex or Taxlot Feature Layer.
                Dim theWhereClause As String

                If theTaxlotVal = String.Empty Then
                    '[Looking for just a MapIndex...]
                    theXFlayer = MapIndexFeatureLayer
                    theWhereClause = "[" & EditorExtension.MapIndexSettings.MapNumberField & "] = '" & theMapNumberVal & "'"
                Else
                    '[Looking for a MapIndex and Taxlot...]
                    theXFlayer = TaxlotFeatureLayer
                    theWhereClause = "[" & EditorExtension.TaxLotSettings.MapNumberField & "] = '" & theMapNumberVal & "' AND [" & EditorExtension.TaxLotSettings.TaxlotField & "] = '" & theTaxlotVal & "'"
                End If

                theQueryFilter.WhereClause = formatWhereClause(theWhereClause, theXFlayer.FeatureClass)

                Dim theXFClass As IFeatureClass = theXFlayer.FeatureClass
                Dim theFeatCursor As IFeatureCursor = theXFClass.Search(theQueryFilter, True)
                Dim thisFeature As IFeature = theFeatCursor.NextFeature

                Dim theFeatureSelection As IFeatureSelection = DirectCast(theXFlayer, IFeatureSelection)
                If .uxSelectFeatures.Checked Then theFeatureSelection.Clear()

                If thisFeature Is Nothing Then '-- Must be due to invalid mapindex or taxlot entered into the text boxes.
                    MessageBox.Show("Feature does not exist.", "Locate Feature", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                Else
                    If .uxSelectFeatures.Checked Then
                        ' Flag the original selection
                        RefreshDisplaySelection()
                        ' Clear all selections
                        Dim theArcMapDoc As IMxDocument = DirectCast(EditorExtension.Application.Document, IMxDocument)
                        theArcMapDoc.FocusMap.ClearSelection()
                    End If

                    Dim theEnvelope As IEnvelope = thisFeature.Shape.Envelope
                    Do Until thisFeature Is Nothing
                        theEnvelope.Union(thisFeature.Shape.Envelope)
                        If .uxSelectFeatures.Checked Then
                            SetSelectedFeature(theXFlayer, thisFeature, True, False)
                        End If
                        thisFeature = theFeatCursor.NextFeature
                    Loop

                    If .uxSelectFeatures.Checked Then
                        ' Flag the new selection
                        RefreshDisplaySelection()
                    End If

                    ZoomToEnvelope(theEnvelope)
                End If
            Finally
                .UseWaitCursor = False
            End Try
        End With

    End Sub

    ''' <summary>
    ''' Clear Definition Queries for participating feature classes.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub uxSelectFeatures_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) ' partnerLocateFeatureUserControl
        uxSelectFeaturesChecked = PartnerLocateFeatureUserControl.uxSelectFeatures.Checked
    End Sub


    Private Sub uxHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) ' partnerLocateFeatureUserControl
        ' TODO: [NIS] Could be replaced with new help mechanism.

        Dim theRTFStream As System.IO.Stream = _
           Me.GetType().Assembly.GetManifestResourceStream("OrmapTaxlotEditing.LocateFeature_help.rtf")
        OpenHelp("Locate Feature Help", theRTFStream)

        ' Get the help form.
        'Dim theHelpForm As New HelpForm
        'theHelpForm.Text = "Locate Feature Help"

        ' KLUDGE: [NIS] Remove comments if file is ready.
        '' Open a custom help text file.
        '' Note: Requires a specific file in the help subdirectory of the application directory.
        'Dim theTextFilePath As String
        'theTextFilePath = My.Application.Info.DirectoryPath & "\help\LocateFeatureHelp.rtf"
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
        'thePdfFilePath = My.Application.Info.DirectoryPath & "\help\LocateFeatureHelp.pdf"
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
        'theVideoFilePath = My.Application.Info.DirectoryPath & "\help\videos\LocateFeature\LocateFeature.html"
        'If Microsoft.VisualBasic.FileIO.FileSystem.FileExists(theVideoFilePath) Then
        '    Dim theUri As New System.Uri("file:///" & theVideoFilePath)
        '    theHelpForm.WebBrowser1.Url = theUri
        'Else
        '    MessageBox.Show("No help file available in the directory " & NewLine & _
        '            My.Application.Info.DirectoryPath & "\help\videos\LocateFeature" & ".")
        '    theHelpForm.TabPage2.Hide()
        'End If

        ' KLUDGE: [NIS] Remove comments if form will be used.
        'theHelpForm.Width = 668
        'theHelpForm.Height = 400
        'theHelpForm.Show()

    End Sub

    ''' <summary>
    ''' Opens the ORMAP Settings Form to the Definition Query tab.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub uxORMAPProperties_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) ' partnerLocateFeatureUserControl

        Dim theORMAPSettingsForm As New OrmapSettingsForm
        theORMAPSettingsForm.uxSettingsTabs.SelectedTab = theORMAPSettingsForm.uxDefinitionQueryTab
        theORMAPSettingsForm.ShowDialog()

    End Sub

    ''' <summary>
    ''' Displays the Map Definition Form.  Passes the mapnumber value that is currently in the uxMapnumber.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub uxSetDefinitionQuery_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) ' partnerLocateFeatureUserControl

        With _partnerLocateDefinitionForm

            ' Set the data source of the Map Definition form's uxMapNumber to the data source of the Locate Feature uxMapNumber
            .uxMapNumber.AutoCompleteCustomSource = _partnerLocateFeatureUserControl.uxMapNumber.AutoCompleteCustomSource
            ' Set the MapNumber of the Map Definition form to the MapNumber on the Locate Feature and find/set the MapScale.
            .uxMapNumber.Text = _partnerLocateFeatureUserControl.uxMapNumber.Text
            .uxMapScale.Text = GetMapScale(.uxMapNumber.Text)

            ' If the Selected Index of the Options have not been set (ie the form is loading the first time) then set them to "Equals".
            If .uxMapNumberOption.SelectedIndex = NotFoundIndex Then
                .uxMapNumberOption.SelectedIndex = 0
                .uxMapScaleOption.SelectedIndex = 0
            End If

            .ShowDialog()
        End With

    End Sub

    ''' <summary>
    ''' Clear Definition Queries for participating feature classes.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub uxClearDefinitionQuery_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) ' partnerLocateFeatureUserControl
        ApplyTheDefinitionQuery()
    End Sub

    ''' <summary>
    ''' Close the Definition Form.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CancelLocateDefinitionQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) ' partnerLocateDefinitionForm
        _partnerLocateDefinitionForm.Close()
    End Sub

    ''' <summary>
    ''' Validates the Definition Form fields and set's the Defintion Queryies for participating feature classes. 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetMapDefinitionQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) ' partnerLocateDefinitionForm

        With _partnerLocateDefinitionForm

            Dim useMapNumber As Boolean = .uxUseMapNumber.Checked
            Dim useMapScale As Boolean = .uxUseMapScale.Checked

            '  Validate the MapNumber and MapScale text boxes before applying the definition query.
            Dim theMapNumber As String = .uxMapNumber.Text.Trim
            If useMapNumber AndAlso theMapNumber <> String.Empty AndAlso Not .uxMapNumber.AutoCompleteCustomSource.Contains(theMapNumber) Then
                MessageBox.Show("Invalid MapNumber. Please try again.", "Definition Query", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If
            Dim theMapScale As String = .uxMapScale.Text.Trim
            If theMapScale = String.Empty Then useMapScale = False ' if the MapScale is an empty string then don't use it.
            If useMapScale AndAlso Not .uxMapScale.AutoCompleteCustomSource.Contains(theMapScale) Then
                .UseWaitCursor = False
                MessageBox.Show("Invalid MapScale. Please try again.", "Definition Query", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            '  If the MapScale is not empty then do some math to convert it (ie 1:100 = 1200) otherwise pass an empty string.
            If useMapNumber And useMapScale Then
                ApplyTheDefinitionQuery(theMapNumber, (Integer.Parse(theMapScale) * 12).ToString)
            Else
                If useMapNumber Then ApplyTheDefinitionQuery(theMapNumber, )
                If useMapScale Then ApplyTheDefinitionQuery(, (Integer.Parse(theMapScale) * 12).ToString)
            End If

            .Close()

        End With

    End Sub

    ''' <summary>
    ''' Resets the MapScale if the user changes the MapNumber. 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub uxMapNumberTextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) ' partnerLocateDefinitionForm
        _partnerLocateDefinitionForm.uxMapScale.Text = GetMapScale(_partnerLocateDefinitionForm.uxMapNumber.Text)
    End Sub


#End Region






#End Region

#Region "Methods"

    ''' <summary>
    ''' Search the table of contents for Layers that are identified as participating in the definition query and applies the Definition query to them.
    ''' </summary>
    ''' <param name="theMapNumber">The mapnumber to use in the definition query</param>
    ''' <param name="theMapScale">The mapscale to use in the definition query</param>
    ''' <remarks>The code checks to make sure the mapnumber and mapscale exist prior to assigning the definition query.</remarks>
    Private Sub ApplyTheDefinitionQuery(Optional ByRef theMapNumber As String = Nothing, Optional ByVal theMapScale As String = Nothing)

        Dim theEnumLayerList As IEnumLayer = GetTOCLayersEnumerator(EsriLayerTypes.FeatureLayer)
        theEnumLayerList.Reset()

        Dim theFeatureLayer As IFeatureLayer = DirectCast(theEnumLayerList.Next, IFeatureLayer)
        Dim theQueryString As String = String.Empty

        Dim theMapNumberField As String = EditorExtension.MapIndexSettings.MapNumberField
        Dim theMapScaleField As String = EditorExtension.MapIndexSettings.MapScaleField


        Do While theFeatureLayer IsNot Nothing
            If theFeatureLayer.Valid AndAlso DefinitionQuerySettings.Default.FeatureLayers.Contains(theFeatureLayer.Name) Then
                Dim theFeatureLayerDefinition As IFeatureLayerDefinition = DirectCast(theFeatureLayer, IFeatureLayerDefinition)

                ' If the MapNumber and the MapScale are nothing then clear the definition query for that layer.
                If theMapNumber Is Nothing AndAlso theMapScale Is Nothing Then
                    theFeatureLayerDefinition.DefinitionExpression = ""
                Else
                    theQueryString = String.Empty ' Reset
                    ' Make sure the feature class has a MapNumber field before applying a Definition Query with MapNumber
                    If Not theMapNumber Is Nothing AndAlso LocateFields(theFeatureLayer.FeatureClass, theMapNumberField) <> NotFoundIndex Then
                        theQueryString = "([" & theMapNumberField & "] " & _partnerLocateDefinitionForm.uxMapNumberOption.Text.Trim & " '" & theMapNumber & "' OR [" & theMapNumberField & "] = '' OR [" & theMapNumberField & "] is null)"
                    End If
                    ' Make sure the feature class has a MapScale field before applying a Definition Query with MapNumber
                    If Not theMapScale Is Nothing AndAlso LocateFields(theFeatureLayer.FeatureClass, theMapScaleField) <> NotFoundIndex Then
                        If theQueryString <> String.Empty Then
                            If _partnerLocateDefinitionForm.uxAnd.Checked Then theQueryString += " AND "
                            If _partnerLocateDefinitionForm.uxOr.Checked Then theQueryString += " OR "
                        End If
                        theQueryString += "([" & theMapScaleField & "] " & _partnerLocateDefinitionForm.uxMapScaleOption.Text.Trim & " " & theMapScale & " OR [" & theMapScaleField & "] = 0 OR [" & theMapScaleField & "] is null)"
                    End If

                    theFeatureLayerDefinition.DefinitionExpression = formatWhereClause(theQueryString, theFeatureLayer.FeatureClass)

                End If

            End If
            theFeatureLayer = DirectCast(theEnumLayerList.Next, IFeatureLayer)
        Loop

        ' Refresh the screen to display the Definition Query.
        Dim theArcMapDoc As IMxDocument = DirectCast(EditorExtension.Application.Document, IMxDocument)
        Dim theMap As IMap = theArcMapDoc.FocusMap
        Dim theActiveView As IActiveView = DirectCast(theMap, IActiveView)
        theActiveView.Refresh()

    End Sub

    ''' <summary>
    ''' Called to get the mapscale of the specified mapnumber
    ''' </summary>
    ''' <returns>A string representing the mapscale for the specified mapnumber</returns>
    ''' <param name="theMapNumber">A MapNumber string</param>
    ''' <remarks>Returns an empty string if no MapNumber exists.   Should this go in SpatialUtilites??</remarks>
    Private Function GetMapScale(ByVal theMapNumber As String) As String

        Dim theMapScale As String = String.Empty

        '-- theMapIndexFeatureLayer is cleared when a user starts editing so check and reset it if needed.
        If MapIndexFeatureLayer Is Nothing Then CheckValidMapIndexDataProperties()

        Dim theMapIndexFClass As IFeatureClass = MapIndexFeatureLayer.FeatureClass
        Dim theQueryString As String = "[" & EditorExtension.MapIndexSettings.MapNumberField & "] = '" & theMapNumber & "'"

        Dim theQueryFilter As IQueryFilter = New QueryFilter
        theQueryFilter.SubFields = EditorExtension.MapIndexSettings.MapScaleField
        theQueryFilter.WhereClause = formatWhereClause(theQueryString, theMapIndexFClass)

        Dim theFeatureCursor As IFeatureCursor = theMapIndexFClass.Search(theQueryFilter, True)
        Dim theFeature As IFeature = theFeatureCursor.NextFeature

        If Not theFeature Is Nothing Then
            Dim theFieldIdx As Integer = theFeature.Fields.FindField(EditorExtension.MapIndexSettings.MapScaleField)
            If IsNumeric(theFeature.Value(theFieldIdx)) Then
                theMapScale = (DirectCast(theFeature.Value(theFieldIdx), Integer) / 12).ToString
            End If
        End If

        Return theMapScale

    End Function



    Friend Sub DoButtonOperation()

        Try
            PartnerLocateFeatureUserControl.uxMapNumber.Enabled = False
            PartnerLocateFeatureUserControl.uxTaxlot.Enabled = False
            PartnerLocateFeatureUserControl.uxFind.Enabled = False

            ' Check for valid data.
            CheckValidMapIndexDataProperties()
            If Not HasValidMapIndexData Then
                MessageBox.Show("Missing data: Valid ORMAP MapIndex layer not found in the map." & NewLine & _
                                "Please load this dataset into your map.", _
                                "Locate Feature", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                Exit Sub
            Else
                PartnerLocateFeatureUserControl.uxMapNumber.Enabled = True
                PartnerLocateFeatureUserControl.uxFind.Enabled = True
            End If

            CheckValidTaxlotDataProperties()
            If HasValidTaxlotData Then
                PartnerLocateFeatureUserControl.uxTaxlot.Enabled = True
            Else
                PartnerLocateFeatureUserControl.uxTaxlot.Enabled = False
            End If

            _locateFeatureDockWin.Show(Not _locateFeatureDockWin.IsVisible)
            If _locateFeatureDockWin.IsVisible AndAlso PartnerLocateFeatureUserControl.uxMapNumber.AutoCompleteCustomSource.Count = 0 Then PartnerLocateFeatureForm_Load()

            If _locateFeatureDockWin.IsVisible() Then
                PartnerLocateFeatureUserControl.uxTimer.Enabled = True
                PartnerLocateFeatureUserControl.uxEditingGroupBox.Enabled = EditorExtension.AllowedToAutoUpdateAllFields
                PartnerLocateFeatureUserControl.uxMapNumber.Focus()
            Else
                PartnerLocateFeatureUserControl.uxTimer.Enabled = False
            End If

        Catch ex As Exception
            EditorExtension.ProcessUnhandledException(ex)

        End Try

    End Sub

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

        Try
            If Not hook Is Nothing Then

                'Disable tool if parent application is not ArcMap
                If TypeOf hook Is IMxApplication Then
                    _application = DirectCast(hook, IApplication)

                    ' Get a reference to the dockable window
                    _locateFeatureDockWinMgr = DirectCast(hook, IDockableWindowManager)
                    Dim locateFeatureDockWinUID As New UID
                    locateFeatureDockWinUID.Value = "{7c5bb546-215f-477a-8df4-16cc1c993309}"
                    _locateFeatureDockWin = _locateFeatureDockWinMgr.GetDockableWindow(locateFeatureDockWinUID)
                    setPartnerLocateFeatureUserControl(DirectCast(_locateFeatureDockWin.UserData, LocateFeatureUserControl))
                    ' Close this when the application starts...
                    _locateFeatureDockWin.Show(False)

                    setLocateDefinitionForm(New MapDefinitionForm())

                    MyBase.m_enabled = True
                Else
                    MyBase.m_enabled = False
                End If
            End If

            ' NOTE: Add other initialization code here...

        Catch ex As Exception
            EditorExtension.ProcessUnhandledException(ex)
        End Try
    End Sub


    Public Overrides ReadOnly Property Checked() As Boolean
        Get
            Checked = _locateFeatureDockWin.IsVisible()
        End Get
    End Property


    Public Overrides Sub OnClick()
        Try
            DoButtonOperation()
        Catch ex As Exception
            EditorExtension.ProcessUnhandledException(ex)
        End Try
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
    Public Const ClassId As String = "ecace054-c346-4be5-a980-8d81ede68fcb"
    Public Const InterfaceId As String = "effc83d3-a320-460c-9b1e-7f48a4b282d8"
    Public Const EventsId As String = "591a6993-fcb1-4c0c-879f-5488e0c88312"
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



