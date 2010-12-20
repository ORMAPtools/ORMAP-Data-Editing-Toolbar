#Region "Copyright 2008 ORMAP Tech Group"

' File:  AutoUpdateAllToggle.vb
'
' Original Author:  OPET.NET Migration Team (Shad Campbell, James Moore, 
'                   Nick Seigal)
'
' Date Created:  March 30, 2008
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
'SCC revision number: $Revision: 239 $
'Date of Last Change: $Date: 2008-03-18 02:11:11 -0700 (Tue, 18 Mar 2008) $
#End Region

#Region "Imported Namespaces"
Imports System.Drawing
Imports System.Environment
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports ESRI.ArcGIS.ADF.BaseClasses
Imports ESRI.ArcGIS.ADF.CATIDs
Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Editor
Imports ESRI.ArcGIS.Framework
#End Region

''' <summary>
''' Provides ArcMap Command with functionality to allow users 
''' to toggle automatic field updates on and off.
''' </summary>
''' <remarks></remarks>
<ComVisible(True)> _
<ComClass(AutoUpdateAllToggle.ClassId, AutoUpdateAllToggle.InterfaceId, AutoUpdateAllToggle.EventsId), _
ProgId("ORMAPTaxlotEditing.AutoUpdateAllToggle")> _
Public NotInheritable Class AutoUpdateAllToggle
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
        MyBase.m_caption = "AutoUpdateAllToggle"   'localizable text 
        MyBase.m_message = "Toggle automatic update of all taxlot fields"   'localizable text 
        MyBase.m_toolTip = "Automatic Update" 'localizable text 
        MyBase.m_name = MyBase.m_category & "_AutoUpdateAllToggle"  'unique id, non-localizable (e.g. "MyCategory_ArcMapCommand")
        MyBase.m_checked = False

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

#End Region

#Region "Properties (none)"
#End Region

#Region "Event Handlers (none)"
#End Region

#Region "Methods (none)"
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
            If EditorExtension.AllowedToAutoUpdateAllFields Then
                m_checked = True
            Else
                m_checked = False
            End If
            Dim canEnable As Boolean
            canEnable = EditorExtension.CanEnableExtendedEditing
            canEnable = canEnable AndAlso EditorExtension.Editor.EditState = esriEditState.esriStateEditing
            canEnable = canEnable AndAlso EditorExtension.IsValidWorkspace
            Return canEnable
        End Get
    End Property

    Public Overrides ReadOnly Property Checked() As Boolean
        Get
            Try
                Return MyBase.Checked
            Catch ex As Exception
                EditorExtension.ProcessUnhandledException(ex)
            End Try
        End Get
    End Property

#End Region

#Region "Methods"

    Public Overrides Sub OnCreate(ByVal hook As Object)
        Try
            If Not hook Is Nothing Then
                _application = DirectCast(hook, IApplication)

                'Disable if it is not ArcMap
                If TypeOf hook Is IMxApplication Then
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

    Public Overrides Sub OnClick()
        Try
            ' Toggle the checked state of the button.
            MyBase.m_checked = Not MyBase.m_checked
            ' Synch up the extension-level flag for auto updates.
            EditorExtension.AllowedToAutoUpdateAllFields = MyBase.m_checked

            If EditorExtension.AllowedToAutoUpdateAllFields Then
                'MessageBox.Show("Auto update of taxlot fields is ON. The minimum fields" & NewLine & _
                '        " (e.g. autodate, autowho) will be updated, as well as all taxlot fields.", _
                '        "Auto Update All Toggle", MessageBoxButtons.OK, MessageBoxIcon.Information)
                _application.StatusBar.Message(esriStatusBarPanes.esriStatusMain) = "Auto update of taxlot fields is ON"
            Else
                'MessageBox.Show("Auto update of taxlot fields is OFF. Only the minimum fields" & NewLine & _
                '        "(e.g. autodate, autowho) will be updated.", _
                '        "Auto Update All Toggle", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                _application.StatusBar.Message(esriStatusBarPanes.esriStatusMain) = "Auto update of taxlot fields is OFF"
            End If

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
        Try
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
        Catch ex As Exception
            EditorExtension.ProcessUnhandledException(ex)
        End Try
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
    Public Const ClassId As String = "6a793dd3-2e75-4b39-a3a3-a261ebeee4e9"
    Public Const InterfaceId As String = "12aedc31-e94b-4015-ae1c-ddb004f7f953"
    Public Const EventsId As String = "415ab563-e444-4c50-b54a-9bfa92fdcac9"
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



