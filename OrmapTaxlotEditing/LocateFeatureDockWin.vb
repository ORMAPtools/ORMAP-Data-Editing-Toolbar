#Region "Copyright 2008 ORMAP Tech Group"

' File:  EditMapIndexForm.vb
'
' Original Author:  OPET.NET Migration Team (Shad Campbell, James Moore, 
'                   Nick Seigal)
'
' Date Created:  April 29, 2008
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
'SCC revision number: $Revision:$
'Date of Last Change: $Date:$
#End Region

#Region "Imported Namespaces"
Imports ESRI.ArcGIS.ADF.CATIDs
Imports System.Runtime.InteropServices
#End Region

''' <summary>
''' Dockable window used to house the LocateFeatureForm form for the LocateFeature class.
''' </summary>
''' <remarks><seealso cref="LocateFeature"/></remarks>
<ComClass(LocateFeatureDockWin.ClassId, LocateFeatureDockWin.InterfaceId, LocateFeatureDockWin.EventsId), _
 ProgId("OrmapTaxlotEditing.LocateFeatureDockWin")> _
Public Class LocateFeatureDockWin
    Implements ESRI.ArcGIS.Framework.IDockableWindowDef

#Region "Custom Class Members"

#Region "Fields"
    Private _locateFeatureUserControl As LocateFeatureUserControl
#End Region

#Region "Properties (none)"
#End Region

#Region "Event Handlers (none)"
#End Region

#Region "Methods (none)"
#End Region

#End Region

#Region "Inherited Class Members"

#Region "Properties (none)"

#End Region

#Region "Methods"
    Public Sub New()
        MyBase.New()
    End Sub

    Public ReadOnly Property Caption() As String Implements ESRI.ArcGIS.Framework.IDockableWindowDef.Caption
        Get
            Caption = "Locate Feature"
        End Get
    End Property

    Public ReadOnly Property ChildHWND() As Integer Implements ESRI.ArcGIS.Framework.IDockableWindowDef.ChildHWND
        Get
            ChildHWND = _locateFeatureUserControl.Handle.ToInt32

        End Get
    End Property

    Public ReadOnly Property Name() As String Implements ESRI.ArcGIS.Framework.IDockableWindowDef.Name
        Get
            Name = "Locate Feature"
        End Get
    End Property

    Public Sub OnCreate(ByVal hook As Object) Implements ESRI.ArcGIS.Framework.IDockableWindowDef.OnCreate
        _locateFeatureUserControl = New LocateFeatureUserControl
    End Sub

    Public Sub OnDestroy() Implements ESRI.ArcGIS.Framework.IDockableWindowDef.OnDestroy

    End Sub

    Public ReadOnly Property UserData() As Object Implements ESRI.ArcGIS.Framework.IDockableWindowDef.UserData
        Get
            Return _locateFeatureUserControl
        End Get
    End Property
#End Region

#End Region

#Region "Other Members"

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
        MxDockableWindows.Register(regKey)

    End Sub
    ''' <summary>
    ''' Required method for ArcGIS Component Category unregistration -
    ''' Do not modify the contents of this method with the code editor.
    ''' </summary>
    Private Shared Sub ArcGISCategoryUnregistration(ByVal registerType As Type)
        Dim regKey As String = String.Format("HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID)
        MxDockableWindows.Unregister(regKey)

    End Sub

#End Region
#End Region

#Region "COM GUIDs"
    ' These  GUIDs provide the COM identity for this class 
    ' and its COM interfaces. If you change them, existing 
    ' clients will no longer be able to access the class.
    Public Const ClassId As String = "7c5bb546-215f-477a-8df4-16cc1c993309"
    Public Const InterfaceId As String = "527b57cb-719a-414d-9ead-b547353c3a94"
    Public Const EventsId As String = "e7729df7-86d7-481e-bcd7-2c6e4e911f73"
#End Region

#End Region

End Class


