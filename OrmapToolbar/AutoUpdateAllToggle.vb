#Region "Copyright 2008-2011 ORMAP Tech Group"

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
Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Editor
Imports ESRI.ArcGIS.Framework
#End Region

''' <summary>
''' Provides ArcMap Command with functionality to allow users 
''' to toggle automatic field updates on and off.
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class AutoUpdateAllToggle
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

#Region "Class-Level Constants and Enumerations (none)"
#End Region

#Region "Built-In Class Members (Constructors, Etc.) (none)"

#Region "Constructors"
    Public Sub New()
    End Sub
#End Region

#End Region

#Region "Custom Class Members (none)"

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

#Region "Properties (none)"

#End Region

#Region "Methods"


    ''' <summary>
    ''' Handles the click event of the button.
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overrides Sub OnClick()

        Try
            ' Toggle the checked state of the button.
            Me.Checked = Not Me.Checked

            ' Synch up the extension-level flag for auto updates.
            EditorExtension.AllowedToAutoUpdateAllFields = Me.Checked

            If EditorExtension.AllowedToAutoUpdateAllFields Then
                'MessageBox.Show("Auto update of taxlot fields is ON. The minimum fields" & NewLine & _
                '        " (e.g. autodate, autowho) will be updated, as well as all taxlot fields.", _
                '        "Auto Update All Toggle", MessageBoxButtons.OK, MessageBoxIcon.Information)
                My.ArcMap.Application.StatusBar.Message(esriStatusBarPanes.esriStatusMain) = "Auto update of taxlot fields is ON"
            Else
                'MessageBox.Show("Auto update of taxlot fields is OFF. Only the minimum fields" & NewLine & _
                '        "(e.g. autodate, autowho) will be updated.", _
                '        "Auto Update All Toggle", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                My.ArcMap.Application.StatusBar.Message(esriStatusBarPanes.esriStatusMain) = "Auto update of taxlot fields is OFF"
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

        Me.Checked = EditorExtension.AllowedToAutoUpdateAllFields

        Dim canEnable As Boolean
        canEnable = EditorExtension.CanEnableExtendedEditing
        canEnable = canEnable AndAlso My.ArcMap.Editor.EditState = esriEditState.esriStateEditing
        canEnable = canEnable AndAlso EditorExtension.IsValidWorkspace

        Enabled = canEnable

    End Sub

#End Region

#End Region

#Region "Implemented Interface Members (none)"
#End Region

#Region "Other Members (none)"
#End Region

End Class



