#Region "Copyright 2008 ORMAP Tech Group"

' File:  PropertyPage.vb
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
Imports System
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports ESRI.ArcGIS.ADF.CATIDs
Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Editor
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Framework
Imports ESRI.ArcGIS.Geodatabase
#End Region

''' <summary>Provides business logic for the PropertyPage.</summary>
''' <remarks>
''' <para>This is where the user may control basic application settings.</para>
''' <para><seealso cref="PropertiesForm"/></para>
''' </remarks>
<ComVisible(True)> _
<ComClass(PropertyPage.ClassId, PropertyPage.InterfaceId, PropertyPage.EventsId), _
ProgId("ORMAPTaxlotEditing.PropertyPage")> _
Public NotInheritable Class PropertyPage
    Implements IComPropertyPage

#Region "Class-Level Constants and Enumerations (none)"
#End Region

#Region "Built-In Class Members (Constructors, Etc.)"

#Region "Constructors"

    ' A creatable COM class must have a Public Sub New() 
    ' with no parameters, otherwise, the class will not be 
    ' registered in the COM registry and cannot be created 
    ' via CreateObject.
    Public Sub New()
    End Sub

#End Region

#End Region

#Region "Custom Class Members"

#Region "Fields (none)"
#End Region

#Region "Properties"

    Private _pageDirty As Boolean '= False

    Friend ReadOnly Property PageDirty() As Boolean
        Get
            Return _pageDirty
        End Get
    End Property

    Private Sub setPageDirty(ByVal value As Boolean)
        _pageDirty = value
    End Sub

    Private _propertiesPageSite As IComPropertyPageSite

    Friend ReadOnly Property PropertiesPageSite() As IComPropertyPageSite
        Get
            Return _propertiesPageSite
        End Get
    End Property

    Private Sub setPropertiesPageSite(ByVal value As IComPropertyPageSite)
        _propertiesPageSite = value
    End Sub

    Private WithEvents _partnerPropertiesForm As PropertiesForm

    Friend ReadOnly Property PartnerPropertiesForm() As PropertiesForm
        Get
            Return _partnerPropertiesForm
        End Get
    End Property

    Private Sub setPartnerPropertiesForm(ByVal value As PropertiesForm)
        _partnerPropertiesForm = value
    End Sub

#End Region

#Region "Event Handlers"

    Private Sub uxEnableTools_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Try
            ' Enable the checkbox and option buttons if the parent checkbox is checked
            PartnerPropertiesForm.uxEnableAutoUpdate.Enabled = PartnerPropertiesForm.uxEnableTools.Checked
            PartnerPropertiesForm.uxMinimumFieldsOption.Enabled = PartnerPropertiesForm.uxEnableTools.Checked
            PartnerPropertiesForm.uxAllFieldsOption.Enabled = PartnerPropertiesForm.uxEnableTools.Checked

            ' Set dirty flag.
            setPageDirty(True)

            If Not PropertiesPageSite Is Nothing Then
                PropertiesPageSite.PageChanged()
            End If
        Catch ex As Exception
            EditorExtension.ProcessUnhandledException(ex)
        End Try
    End Sub

    Private Sub uxEnableAutoUpdate_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Try
            ' Enable the option buttons if the parent checkbox is checked
            PartnerPropertiesForm.uxMinimumFieldsOption.Enabled = PartnerPropertiesForm.uxEnableAutoUpdate.Checked
            PartnerPropertiesForm.uxAllFieldsOption.Enabled = PartnerPropertiesForm.uxEnableAutoUpdate.Checked

            ' Set dirty flag.
            setPageDirty(True)

            If Not PropertiesPageSite Is Nothing Then
                PropertiesPageSite.PageChanged()
            End If
        Catch ex As Exception
            EditorExtension.ProcessUnhandledException(ex)
        End Try
    End Sub

    Private Sub uxMinimumFieldsOption_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Try
            ' Set dirty flag.
            setPageDirty(True)

            If Not PropertiesPageSite Is Nothing Then
                PropertiesPageSite.PageChanged()
            End If
        Catch ex As Exception
            EditorExtension.ProcessUnhandledException(ex)
        End Try
    End Sub

    Private Sub uxAllFieldsOption_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Try
            ' Set dirty flag.
            setPageDirty(True)

            If Not PropertiesPageSite Is Nothing Then
                PropertiesPageSite.PageChanged()
            End If
        Catch ex As Exception
            EditorExtension.ProcessUnhandledException(ex)
        End Try
    End Sub

    Private Sub uxSettings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Dim settingsForm As New OrmapSettingsForm
            settingsForm.ShowDialog(DirectCast(sender, Control).FindForm)
        Catch ex As Exception
            EditorExtension.ProcessUnhandledException(ex)
        End Try
    End Sub

    Private Sub uxAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Dim aboutForm As New AboutForm
            aboutForm.ShowDialog(DirectCast(sender, Control).FindForm)
        Catch ex As Exception
            EditorExtension.ProcessUnhandledException(ex)
        End Try
    End Sub

    Private Sub uxReportOrRequest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim theRTFStream As System.IO.Stream = _
                Me.GetType().Assembly.GetManifestResourceStream("OrmapTaxlotEditing.ReportOrRequest_help.rtf")
        Utilities.OpenHelp("Report Bug or Request New Feature", theRTFStream)
    End Sub

#End Region

#Region "Methods (none)"
#End Region

#End Region

#Region "Inherited Class Members (none)"

#Region "Properties (none)"
#End Region

#Region "Methods (none)"
#End Region

#End Region

#Region "Implemented Interface Members"

#Region "IComPropertyPage Implementations"

    ''' <summary>
    ''' Return the height of the page in pixels.
    ''' </summary>
    ''' <value>The height of the page in pixels.</value>
    ''' <returns>The height in pixels.</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Height() As Integer Implements IComPropertyPage.Height
        Get
            Return PartnerPropertiesForm.Height
        End Get
    End Property

    ''' <summary>
    ''' Return the help file name for the page.
    ''' </summary>
    ''' <value>The help file name for the page.</value>
    ''' <returns>The help file name.</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property HelpFile() As String Implements IComPropertyPage.HelpFile
        Get
            Return Nothing  ' TODO: [NIS] Implement Help File
        End Get
    End Property

    ''' <summary>
    ''' Return the help context ID for the specified control on the page.
    ''' </summary>
    ''' <param name="controlID"></param>
    ''' <value>The help context ID for the specified control on the page.</value>
    ''' <returns>The help context ID.</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property HelpContextID(ByVal controlID As Integer) As Integer Implements IComPropertyPage.HelpContextID
        Get
            Return 0  ' TODO: [NIS] Implement Help File
        End Get
    End Property

    ''' <summary>
    ''' Indicates if the page made any changes to the object(s).
    ''' </summary>
    ''' <value>If the page made any changes to the object(s).</value>
    ''' <returns><c>True</c> or <c>False</c>, depending on if changes were made.</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property IsPageDirty() As Boolean Implements IComPropertyPage.IsPageDirty
        Get
            Return PageDirty
        End Get
    End Property

    ''' <summary>
    ''' Set the COM property page site.
    ''' </summary>
    ''' <value>The sheet that contains the page.</value>
    ''' <remarks></remarks>
    Public WriteOnly Property PageSite() As ESRI.ArcGIS.Framework.IComPropertyPageSite Implements IComPropertyPage.PageSite
        Set(ByVal value As ESRI.ArcGIS.Framework.IComPropertyPageSite)
            setPropertiesPageSite(value)
        End Set
    End Property

    ''' <summary>
    ''' Returns the page priority.
    ''' </summary>
    ''' <value>The page priority.</value>
    ''' <returns>The priority.</returns>
    ''' <remarks>The higher the priority, the sooner the page appears in the containing property sheet.</remarks>
    Public Property Priority() As Integer Implements IComPropertyPage.Priority
        Get
            Return 0  'Lowest number = last/rightmost tab position in the Properties window.
        End Get
        Set(ByVal value As Integer)
            ' Do not set anything
        End Set
    End Property

    ''' <summary>
    ''' Return the title of the property page.
    ''' </summary>
    ''' <value>The title of the property page.</value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Title() As String Implements IComPropertyPage.Title
        Get
            Return "ORMAP Taxlot Editor"
        End Get
        Set(ByVal value As String)
            ' Do not set anything
        End Set
    End Property

    ''' <summary>
    ''' Return the width of the page in pixels.
    ''' </summary>
    ''' <value>The width of the page in pixels.</value>
    ''' <returns>The width in pixels.</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Width() As Integer Implements IComPropertyPage.Width
        Get
            Return PartnerPropertiesForm.Width
        End Get
    End Property

    ''' <summary>
    ''' Activate the page.
    ''' </summary>
    ''' <returns>The hWnd of the page.</returns>
    ''' <remarks>Occurs on page creation. Return the hWnd of the page here.</remarks>
    Public Function Activate() As Integer Implements IComPropertyPage.Activate
        Return PartnerPropertiesForm.Handle.ToInt32()
    End Function

    ''' <summary>
    ''' Indicates if the page applies to the specified objects.
    ''' </summary>
    ''' <param name="objects">The objects for which property pages may be displayed.</param>
    ''' <returns><c>True</c> or <c>False</c>, depending on if the page applies to the objects.</returns>
    ''' <remarks>If page applies it will be displayed. Triggered by an external ArcGIS event.</remarks>
    Public Function Applies(ByVal objects As ESRI.ArcGIS.esriSystem.ISet) As Boolean Implements IComPropertyPage.Applies
        Try
            ' Do not affirm if the objects list is empty.
            If objects Is Nothing OrElse objects.Count = 0 Then
                Return False
            End If
            objects.Reset()

            ' Get a reference to the editor.
            ' Do not affirm if the editor is not found.
            Dim editor As IEditor2 = TryCast(objects.Next(), IEditor2)
            If editor Is Nothing Then
                Return False
            End If

            '' Do not affirm if the user is not editing.
            'If editor.EditState <> esriEditState.esriStateEditing Then
            '    Return False
            'End If

            ' Otherwise, affirm.
            Return True

        Catch ex As Exception
            EditorExtension.ProcessUnhandledException(ex)
        End Try
    End Function

    ''' <summary>
    ''' Apply any changes to the object(s).
    ''' </summary>
    ''' <remarks>Triggered by an external ArcGIS event.</remarks>
    Public Sub Apply() Implements IComPropertyPage.Apply
        Try
            ' Write to the EditorExtension shared (i.e. by all class objects) properties
            EditorExtension.AllowedToEditTaxlots = PartnerPropertiesForm.uxEnableTools.Checked
            EditorExtension.AllowedToAutoUpdate = PartnerPropertiesForm.uxEnableAutoUpdate.Checked
            EditorExtension.AllowedToAutoUpdateAllFields = PartnerPropertiesForm.uxAllFieldsOption.Checked
            setPageDirty(False)
        Catch ex As Exception
            EditorExtension.ProcessUnhandledException(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Cancel the changes to the object(s).
    ''' </summary>
    ''' <remarks>Triggered by an external ArcGIS event.</remarks>
    Public Sub Cancel() Implements IComPropertyPage.Cancel
        Try
            ' TODO: [NIS] Implement this?
        Catch ex As Exception
            EditorExtension.ProcessUnhandledException(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Destroy the page.
    ''' </summary>
    ''' <remarks>Triggered by an external ArcGIS event.</remarks>
    Public Sub Deactivate() Implements IComPropertyPage.Deactivate
        Try
            If Not _partnerPropertiesForm Is Nothing Then
                PartnerPropertiesForm.Dispose()
            End If
            setPartnerPropertiesForm(Nothing)
            setPropertiesPageSite(Nothing)
        Catch ex As Exception
            EditorExtension.ProcessUnhandledException(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Hide the page.
    ''' </summary>
    ''' <remarks>Triggered by an external ArcGIS event.</remarks>
    Public Sub Hide() Implements IComPropertyPage.Hide
        Try
            PartnerPropertiesForm.Hide()
        Catch ex As Exception
            EditorExtension.ProcessUnhandledException(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Supply the page with the object(s) to be edited.
    ''' </summary>
    ''' <param name="objects">The object(s) to be edited.</param>
    ''' <remarks>Triggered by an external ArcGIS event.</remarks>
    Public Sub SetObjects(ByVal objects As ESRI.ArcGIS.esriSystem.ISet) Implements IComPropertyPage.SetObjects
        Try
            ' Note: The Applies() method should have done preliminary checking of 
            ' editor states before this method is called.

            ' ENHANCE: [NIS] Move (to where)?
            ' Initialize controls based on properties.
            setPartnerPropertiesForm(New PropertiesForm())
            PartnerPropertiesForm.uxEnableTools.Checked = EditorExtension.AllowedToEditTaxlots
            PartnerPropertiesForm.uxEnableAutoUpdate.Checked = EditorExtension.AllowedToAutoUpdate
            PartnerPropertiesForm.uxMinimumFieldsOption.Checked = Not EditorExtension.AllowedToAutoUpdateAllFields
            PartnerPropertiesForm.uxAllFieldsOption.Checked = EditorExtension.AllowedToAutoUpdateAllFields

            ' Subscribe to form events.
            AddHandler PartnerPropertiesForm.uxEnableTools.CheckedChanged, AddressOf uxEnableTools_CheckedChanged
            AddHandler PartnerPropertiesForm.uxEnableAutoUpdate.CheckedChanged, AddressOf uxEnableAutoUpdate_CheckedChanged
            AddHandler PartnerPropertiesForm.uxMinimumFieldsOption.CheckedChanged, AddressOf uxMinimumFieldsOption_CheckedChanged
            AddHandler PartnerPropertiesForm.uxAllFieldsOption.CheckedChanged, AddressOf uxAllFieldsOption_CheckedChanged
            AddHandler PartnerPropertiesForm.uxSettings.Click, AddressOf uxSettings_Click
            AddHandler PartnerPropertiesForm.uxAbout.Click, AddressOf uxAbout_Click
            AddHandler PartnerPropertiesForm.uxReportOrRequest.Click, AddressOf uxReportOrRequest_Click

        Catch ex As Exception
            EditorExtension.ProcessUnhandledException(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Show the page.
    ''' </summary>
    ''' <remarks>Triggered by an external ArcGIS event.</remarks>
    Public Sub Show() Implements IComPropertyPage.Show
        Try
            PartnerPropertiesForm.Show()
        Catch ex As Exception
            EditorExtension.ProcessUnhandledException(ex)
        End Try
    End Sub

#End Region

#End Region

#Region "Other Members"

#Region "COM GUIDs"
    ' These  GUIDs provide the COM identity for this class 
    ' and its COM interfaces. If you change them, existing 
    ' clients will no longer be able to access the class.
    Public Const ClassId As String = "050c23da-ebd8-4a1d-871b-b7a9354d331b"
    Public Const InterfaceId As String = "bae36023-8a03-43b6-bea6-fab534ff7c5e"
    Public Const EventsId As String = "8ab94224-407b-4139-a003-48f5789bf3b3"
#End Region

#Region "COM Registration Function(s)"
    <ComRegisterFunction(), ComVisible(False)> _
    Public Shared Sub RegisterFunction(ByVal registerType As Type)
        ' Required for ArcGIS Component Category Registrar support
        ArcGISCategoryRegistration(registerType)

        '
        ' NOTE: Add any COM registration code here...
        '
    End Sub

    <ComUnregisterFunction(), ComVisible(False)> _
    Public Shared Sub UnregisterFunction(ByVal registerType As Type)
        ' Required for ArcGIS Component Category Registrar support
        ArcGISCategoryUnregistration(registerType)

        '
        ' NOTE: Add any COM unregistration code here...
        '
    End Sub

#Region "ArcGIS Component Category Registrar generated code"
    ''' <summary>
    ''' Required method for ArcGIS Component Category registration -
    ''' Do not modify the contents of this method with the code editor.
    ''' </summary>
    Private Shared Sub ArcGISCategoryRegistration(ByVal registerType As Type)
        Dim regKey As String = String.Format("HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID)
        EditorPropertyPages.Register(regKey)

    End Sub
    ''' <summary>
    ''' Required method for ArcGIS Component Category unregistration -
    ''' Do not modify the contents of this method with the code editor.
    ''' </summary>
    Private Shared Sub ArcGISCategoryUnregistration(ByVal registerType As Type)
        Dim regKey As String = String.Format("HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID)
        EditorPropertyPages.Unregister(regKey)

    End Sub

#End Region
#End Region

#End Region

End Class

