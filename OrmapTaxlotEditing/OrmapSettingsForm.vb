#Region "Copyright 2008 ORMAP Tech Group"

' File:  OrmapSettingsForm.vb
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
Imports System.Runtime.InteropServices
Imports System.Configuration
Imports System.Environment
Imports System.Windows.Forms
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Geodatabase
Imports OrmapTaxlotEditing.SpatialUtilities
Imports OrmapTaxlotEditing.Utilities
Imports OrmapTaxlotEditing.DefinitionQuerySettings

#End Region

''' <summary>
''' Provides functionality and user interface to 
''' allow configuration of user settings.
''' </summary>
''' <remarks>
''' <para><seealso cref="OrmapSettings"/></para>
''' </remarks>
<ComVisible(False)> _
Public Class OrmapSettingsForm

#Region "Class-Level Constants and Enumerations (none)"
#End Region

#Region "Built-In Class Members (Constructors, Etc.)"

#Region "Constructors"

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        SetBindings()

    End Sub

#End Region

#End Region

#Region "Custom Class Members"

#Region "Fields (none)"
#End Region

#Region "Properties (none)"
#End Region

#Region "Event Handlers"

    ''' <summary>
    ''' Loads the layers in the TOC into the CheckedBoxList and looks up whether they are to participate in the definition query or not.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub OrmapSettingsForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not EditorExtension.CanEnableExtendedEditing Then
            '-- THIS IS A BUG THAT NEEDS TO BE FIXED...
            Exit Sub
        End If

        Dim theTOCLayers As IEnumLayer = GetTOCLayersEnumerator(EsriLayerTypes.FeatureLayer)
        theTOCLayers.Reset()

        Dim thisFeatureLayer As IFeatureLayer
        thisFeatureLayer = DirectCast(theTOCLayers.Next, IFeatureLayer)

        While Not (thisFeatureLayer Is Nothing)
            If thisFeatureLayer.Valid Then ' make sure the featurelayer is valid.
                ' If the feature layer does not contain a MapNumber OR a MapScale then do not display it.
                If thisFeatureLayer.FeatureClass.FindField("MapNumber") <> NotFoundIndex Or thisFeatureLayer.FeatureClass.FindField("MapScale") <> NotFoundIndex Then
                    If DefinitionQuerySettings.Default.FeatureLayers.Contains(thisFeatureLayer.Name) Then
                        uxDefQueryLayers.Items.Add(thisFeatureLayer.Name, True)
                    Else
                        uxDefQueryLayers.Items.Add(thisFeatureLayer.Name, False)
                    End If
                End If
            End If
            thisFeatureLayer = DirectCast(theTOCLayers.Next(), IFeatureLayer)
        End While

    End Sub


    ''' <summary>
    ''' Closes the form without saving any modified settings.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub uxCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxCancel.Click

        ReloadSettings()

        If Me.Modal Then
            ' Modal form is closed automatically by the 
            ' uxCancel.DialogResult = Cancel property. 
        Else
            Me.Close()
        End If

    End Sub

    ''' <summary>
    ''' Launches help.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub uxHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxHelp.Click

        Dim theRTFStream As System.IO.Stream = _
        Me.GetType().Assembly.GetManifestResourceStream("OrmapTaxlotEditing.Settings_help.rtf")
        Utilities.OpenHelp("ORMAP Taxlot Editing Settings Help", theRTFStream)

    End Sub

    ''' <summary>
    ''' Imports application settings values and keeps the dialog open. 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub uxImport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxImport.Click

        ImportSettings()

    End Sub
    ''' <summary>
    ''' Exports application settings values and keeps the dialog open. 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub uxExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxExport.Click

        ExportSettings()

    End Sub



    ''' <summary>
    ''' Reloads saved application settings values and keeps the dialog open. 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub uxReload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxReload.Click

        ReloadSettings()

    End Sub

    ''' <summary>
    ''' Resets application settings values to defaults and keeps the dialog open. 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub uxReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxReset.Click

        ResetSettings()

    End Sub

    ''' <summary>
    ''' Saves the current application settings values and closes the dialog.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub uxSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxSave.Click

        SaveSettings()

        If Me.Modal Then
            ' Modal form is closed automatically by the 
            ' uxSave.DialogResult = Cancel property. 
        Else
            Me.Close()
        End If

    End Sub

#End Region

#Region "Methods"

    ''' <summary>
    ''' Imports application settings values from a file.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ImportSettings()

        ' Get the local user.config file path
        Dim config As System.Configuration.Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal)
        Dim theLocalFilePath As String = config.FilePath

        ' Create a back up of this file
        If My.Computer.FileSystem.FileExists(theLocalFilePath) Then
            My.Computer.FileSystem.CopyFile(theLocalFilePath, theLocalFilePath & ".bak", True)
        Else
            MessageBox.Show("The local settings file (user.config) is not available." & NewLine & _
                    "Change one setting, save the settings, and try again.", "Import Settings", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' Prompt user for config file location
        Dim theOpenFileDialog As New System.Windows.Forms.OpenFileDialog
        theOpenFileDialog.Multiselect = False
        theOpenFileDialog.Filter = "config files (*.config)|*.config"
        If theOpenFileDialog.ShowDialog <> Windows.Forms.DialogResult.OK Then Exit Sub
        If theOpenFileDialog.CheckFileExists = False And theOpenFileDialog.CheckPathExists = False Then Exit Sub
        Dim theSourceFilePath As String = theOpenFileDialog.FileName

        ' Copy from source path to local path.
        If My.Computer.FileSystem.FileExists(theSourceFilePath) Then
            My.Computer.FileSystem.CopyFile(theSourceFilePath, theLocalFilePath, True)
            ReloadSettings()
        Else
            MessageBox.Show("The source settings file (user.config) is not available at:" & NewLine & _
                    My.Computer.FileSystem.GetParentPath(theSourceFilePath) & "." & NewLine & NewLine & _
                    "Copy your source settings file to this location and try again.", "Import Settings", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

    End Sub


    ''' <summary>
    ''' Exports application settings values to a file.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ExportSettings()

        ' Get the local user.config file path
        Dim config As System.Configuration.Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal)
        Dim theLocalFilePath As String = config.FilePath()

        ' Create a back up of this file
        If My.Computer.FileSystem.FileExists(theLocalFilePath) Then
            My.Computer.FileSystem.CopyFile(theLocalFilePath, theLocalFilePath & ".bak", True)
        Else
            MessageBox.Show("The local settings file (user.config) is not available." & NewLine & _
                    "Change one setting, save the settings, and try again.", "Import Settings", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' Prompt user for config file location
        Dim theSaveFileDialog As New System.Windows.Forms.SaveFileDialog
        theSaveFileDialog.Filter = "config files (*.config)|*.config"
        If theSaveFileDialog.ShowDialog <> Windows.Forms.DialogResult.OK Then Exit Sub
        If theSaveFileDialog.CheckPathExists = False Then Exit Sub
        Dim theExportFilePath As String = theSaveFileDialog.FileName

        ' Copy from local path to export path.
        My.Computer.FileSystem.CopyFile(theLocalFilePath, theExportFilePath, True)

    End Sub


    ''' <summary>
    ''' Set the control binding sources for the form.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetBindings()

        ' Set the control binding sources for all the controls on the settings tabs.
        TableNamesSettingsBindingSource.DataSource = EditorExtension.TableNamesSettings
        AnnoTableNamesSettingsBindingSource.DataSource = EditorExtension.AnnoTableNamesSettings
        AllTablesSettingsBindingSource.DataSource = EditorExtension.AllTablesSettings
        MapIndexSettingsBindingSource.DataSource = EditorExtension.MapIndexSettings
        TaxLotSettingsBindingSource.DataSource = EditorExtension.TaxLotSettings
        TaxLotLinesSettingsBindingSource.DataSource = EditorExtension.TaxLotLinesSettings
        CartographicLinesSettingsBindingSource.DataSource = EditorExtension.CartographicLinesSettings
        TaxlotAcreageAnnoSettingsBindingSource.DataSource = EditorExtension.TaxlotAcreageAnnoSettings
        TaxlotNumberAnnoSettingsBindingSource.DataSource = EditorExtension.TaxlotNumberAnnoSettings
        DefaultValuesSettingsBindingSource.DataSource = EditorExtension.DefaultValuesSettings


    End Sub

    ''' <summary>
    ''' Stores the current values of the application settings in persistent storage.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SaveSettings()

        Dim settings As ApplicationSettingsBase
        settings = DirectCast(TableNamesSettingsBindingSource.DataSource, ApplicationSettingsBase)
        settings.Save()
        settings = DirectCast(AnnoTableNamesSettingsBindingSource.DataSource, ApplicationSettingsBase)
        settings.Save()
        settings = DirectCast(AllTablesSettingsBindingSource.DataSource, ApplicationSettingsBase)
        settings.Save()
        settings = DirectCast(MapIndexSettingsBindingSource.DataSource, ApplicationSettingsBase)
        settings.Save()
        settings = DirectCast(TaxLotSettingsBindingSource.DataSource, ApplicationSettingsBase)
        settings.Save()
        settings = DirectCast(TaxLotLinesSettingsBindingSource.DataSource, ApplicationSettingsBase)
        settings.Save()
        settings = DirectCast(CartographicLinesSettingsBindingSource.DataSource, ApplicationSettingsBase)
        settings.Save()
        settings = DirectCast(TaxlotAcreageAnnoSettingsBindingSource.DataSource, ApplicationSettingsBase)
        settings.Save()
        settings = DirectCast(TaxlotNumberAnnoSettingsBindingSource.DataSource, ApplicationSettingsBase)
        settings.Save()
        settings = DirectCast(DefaultValuesSettingsBindingSource.DataSource, ApplicationSettingsBase)
        settings.Save()

        '-- Definition Query
        For i As Integer = 0 To uxDefQueryLayers.Items.Count - 1
            If uxDefQueryLayers.GetItemCheckState(i) = CheckState.Checked Then
                If Not DefinitionQuerySettings.Default.FeatureLayers.Contains(uxDefQueryLayers.Items(i).ToString) Then
                    DefinitionQuerySettings.Default.FeatureLayers.Add(uxDefQueryLayers.Items(i).ToString)
                End If
            Else
                If DefinitionQuerySettings.Default.FeatureLayers.Contains(uxDefQueryLayers.Items(i).ToString) Then
                    DefinitionQuerySettings.Default.FeatureLayers.Remove(uxDefQueryLayers.Items(i).ToString)
                End If
            End If
        Next
        DefinitionQuerySettings.Default.Save()

    End Sub

    ''' <summary>
    ''' Refreshes the application settings values from persistent 
    ''' storage.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ReloadSettings()

        Dim settings As ApplicationSettingsBase
        settings = DirectCast(TableNamesSettingsBindingSource.DataSource, ApplicationSettingsBase)
        settings.Reload()
        settings = DirectCast(AnnoTableNamesSettingsBindingSource.DataSource, ApplicationSettingsBase)
        settings.Reload()
        settings = DirectCast(AllTablesSettingsBindingSource.DataSource, ApplicationSettingsBase)
        settings.Reload()
        settings = DirectCast(MapIndexSettingsBindingSource.DataSource, ApplicationSettingsBase)
        settings.Reload()
        settings = DirectCast(TaxLotSettingsBindingSource.DataSource, ApplicationSettingsBase)
        settings.Reload()
        settings = DirectCast(TaxLotLinesSettingsBindingSource.DataSource, ApplicationSettingsBase)
        settings.Reload()
        settings = DirectCast(CartographicLinesSettingsBindingSource.DataSource, ApplicationSettingsBase)
        settings.Reload()
        settings = DirectCast(TaxlotAcreageAnnoSettingsBindingSource.DataSource, ApplicationSettingsBase)
        settings.Reload()
        settings = DirectCast(TaxlotNumberAnnoSettingsBindingSource.DataSource, ApplicationSettingsBase)
        settings.Reload()
        settings = DirectCast(DefaultValuesSettingsBindingSource.DataSource, ApplicationSettingsBase)
        settings.Reload()


    End Sub

    ''' <summary>
    ''' Restores the persisted application settings values to their 
    ''' corresponding default properties.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ResetSettings()

        Dim settings As ApplicationSettingsBase
        settings = DirectCast(TableNamesSettingsBindingSource.DataSource, ApplicationSettingsBase)
        settings.Reset()
        settings = DirectCast(AnnoTableNamesSettingsBindingSource.DataSource, ApplicationSettingsBase)
        settings.Reset()
        settings = DirectCast(AllTablesSettingsBindingSource.DataSource, ApplicationSettingsBase)
        settings.Reset()
        settings = DirectCast(MapIndexSettingsBindingSource.DataSource, ApplicationSettingsBase)
        settings.Reset()
        settings = DirectCast(TaxLotSettingsBindingSource.DataSource, ApplicationSettingsBase)
        settings.Reset()
        settings = DirectCast(TaxLotLinesSettingsBindingSource.DataSource, ApplicationSettingsBase)
        settings.Reset()
        settings = DirectCast(CartographicLinesSettingsBindingSource.DataSource, ApplicationSettingsBase)
        settings.Reset()
        settings = DirectCast(TaxlotAcreageAnnoSettingsBindingSource.DataSource, ApplicationSettingsBase)
        settings.Reset()
        settings = DirectCast(TaxlotNumberAnnoSettingsBindingSource.DataSource, ApplicationSettingsBase)
        settings.Reset()
        settings = DirectCast(DefaultValuesSettingsBindingSource.DataSource, ApplicationSettingsBase)
        settings.Reset()
        DefinitionQuerySettings.Default.FeatureLayers.Clear()
        DefinitionQuerySettings.Default.Save()

    End Sub

#End Region

#End Region

#Region "Inherited Class Members (none)"

#Region "Properties (none)"
#End Region

#Region "Methods (none)"
#End Region

#End Region

#Region "Implemented Interface Members (none) "
#End Region

#Region "Other Members (none)"
#End Region


End Class