#Region "Copyright 2008 ORMAP Tech Group"

' File:  OrmapAnnotateToolbar.vb
'
' Original Author:  Robert Gumtow
'
' Date Created:  May 5, 2010
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

Imports ESRI.ArcGIS.ADF.CATIDs
Imports ESRI.ArcGIS.ADF.BaseClasses
Imports System.Runtime.InteropServices
Imports ESRI.ArcGIS.esriSystem

#End Region

<ComClass(OrmapAnnotateToolbar.ClassId, OrmapAnnotateToolbar.InterfaceId, OrmapAnnotateToolbar.EventsId), _
 ProgId("OrmapTaxlotEditing.OrmapAnnotateToolbar")> _
Public NotInheritable Class OrmapAnnotateToolbar
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

        'BeginGroup() 'Separator
        AddItem("OrmapTaxlotEditing.CreateAnnotation")
        BeginGroup() 'Separator
        AddItem("OrmapTaxlotEditing.TransposeAnnotation")
        AddItem("OrmapTaxlotEditing.InvertAnnotation")
        BeginGroup() 'Separator
        AddItem("OrmapTaxlotEditing.MoveDown")
        AddItem("OrmapTaxlotEditing.StandardBothSidesDown")
        AddItem("OrmapTaxlotEditing.WideBothSidesDown")
        AddItem("OrmapTaxlotEditing.MoveUp")
        AddItem("OrmapTaxlotEditing.StandardBothSidesUp")
        AddItem("OrmapTaxlotEditing.WideBothSidesUp")

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
            Return "ORMAP Annotate (.NET)"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "OrmapAnnotateToolbar"
        End Get
    End Property
#End Region

#Region "Methods (none)"
#End Region

#End Region

#Region "Implemented Interface Members (none)"
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

#Region "COM GUIDs"
    ' These  GUIDs provide the COM identity for this class 
    ' and its COM interfaces. If you change them, existing 
    ' clients will no longer be able to access the class.
    Public Const ClassId As String = "aecf7c1a-2c0c-42bc-a163-a078b2318706"
    Public Const InterfaceId As String = "68eefbdf-9fa3-4530-846a-90ccdf891642"
    Public Const EventsId As String = "9eaa34de-028c-460f-8dee-a3e3fd19e324"
#End Region

#End Region

End Class