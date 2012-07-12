#Region "Copyright 2008-2011 ORMAP Tech Group"

' File:  AnnoBothUpWideButton.vb
'
' Original Author:  Robert Gumtow
'
' Date Created:  September, 2011
'
' Copyright Holder:  ORMAP Tech Group  
' Contact Info:  ORMAP Tech Group may be reached at 
' ORMAP_ESRI_Programmers@listsmart.osl.state.or.us
'
' This file is part of the ORMAP Taxlot Editing Toolbar AddIn.
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

Imports ESRI.ArcGIS.Editor

Public Class AnnoBothUpWideButton
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

    Public Sub New()

    End Sub

    Protected Overrides Sub OnClick()
        Dim _wideBothSidesUp As AnnoBothUpWide = New AnnoBothUpWide
        _wideBothSidesUp.DoButtonOperation()
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
End Class
