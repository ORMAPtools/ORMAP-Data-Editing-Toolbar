#Region "Copyright 2008 ORMAP Tech Group"

' File:  OrmapToolbar.vb
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
Imports ESRI.ArcGIS.ADF.CATIDs
Imports ESRI.ArcGIS.ADF.BaseClasses
Imports ESRI.ArcGIS.esriSystem
#End Region

''' <summary>
''' Provides an ArcMap CommandBar to hold the Commands and 
''' Tools in this assembly.
''' </summary>
''' <remarks></remarks>
<ComVisible(True)> _
<ComClass(OrmapToolbar.ClassId, OrmapToolbar.InterfaceId, OrmapToolbar.EventsId), _
ProgId("ORMAPTaxlotEditing.OrmapToolbar")> _
Public NotInheritable Class OrmapToolbar
    Inherits BaseToolbar

#Region "Class-Level Constants and Enumerations (none)"
#End Region

#Region "Built-In Class Members (Constructors, Etc.)"

#Region "Constructors"

    ' A creatable COM class must have a Public Sub New() 
    ' with no parameters, otherwise, the class will not be 
    ' registered in the COM registry and cannot be created 
    ' via CreateObject.
    Public Sub New()

        ' Define the toolbar by adding items.
        AddItem("ORMAPTaxlotEditing.AutoUpdateAllToggle")
        BeginGroup() 'Separator
        AddItem("ORMAPTaxlotEditing.LocateFeature")
        BeginGroup() 'Separator
        AddItem("ORMAPTaxlotEditing.TaxlotAssignment")
        AddItem("ORMAPTaxlotEditing.EditMapIndex")
        AddItem("ORMAPTaxlotEditing.CombineTaxlots")
        BeginGroup() 'Separator
        AddItem("ORMAPTaxlotEditing.AddArrows")

        ' Example:
        'AddItem("{FBF8C3FB-0480-11D2-8D21-080009EE4E51}", 1) 'undo command
        'AddItem(New Guid("FBF8C3FB-0480-11D2-8D21-080009EE4E51"), 2) 'redo command

    End Sub

#End Region

#End Region

#Region "Custom Class Members"

#Region "Fields (none)"
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

    Public Overrides ReadOnly Property Caption() As String
        Get
            Return "ORMAP Taxlot Editor (.NET)"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            'TEST: [NIS] Confirm that this choice of name will work well in ArcMap UI.
            Return "OrmapToolbar"
        End Get
    End Property

#End Region

#Region "Methods (none)"
#End Region

#End Region

#Region "Implemented Interface Members (none)"
#End Region

#Region "Other Members"

#Region "COM GUIDs"
    ' These  GUIDs provide the COM identity for this class 
    ' and its COM interfaces. If you change them, existing 
    ' clients will no longer be able to access the class.
    Public Const ClassId As String = "07b32247-abf9-4643-b078-4e68be56599e"
    Public Const InterfaceId As String = "98eff0b1-3e68-4881-b5f8-5f8a9e242682"
    Public Const EventsId As String = "8f2ffc36-8a61-44ef-9127-8e4935dcc563"
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
        MxCommandBars.Register(regKey)

    End Sub
    ''' <summary>
    ''' Required method for ArcGIS Component Category unregistration -
    ''' Do not modify the contents of this method with the code editor.
    ''' </summary>
    Private Shared Sub ArcGISCategoryUnregistration(ByVal registerType As Type)
        Dim regKey As String = String.Format("HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID)
        MxCommandBars.Unregister(regKey)

    End Sub

#End Region
#End Region

#End Region

End Class
