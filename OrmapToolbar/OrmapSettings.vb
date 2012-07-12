
#Region "Copyright 2011 ORMAP Tech Group"

' File:  OrmapSettings.vb
'
' Original Author:  Shad Campbell
'
' Date Created:  September 6, 20011
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


Public Class OrmapSettings
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button


#Region "Built-In Class Members (Constructors, Etc.)"

#Region "Constructors"
    Public Sub New()
    End Sub
#End Region

#End Region

#Region "Inherited Class Members"

#Region "Properties (none)"
#End Region

#Region "Methods"

    ''' <summary>
    ''' Handles on click event of button
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overrides Sub OnClick()
        Dim theORMAPSettingsForm As New OrmapSettingsForm
        theORMAPSettingsForm.ShowDialog()
    End Sub

    ''' <summary>
    ''' Event Handler 
    ''' </summary>
    ''' <remarks>WARNING: Do not put computation-intensive code here. Called by ArcMap once per second to check if the command is enabled.</remarks>
    Protected Overrides Sub OnUpdate()
    End Sub

#End Region

#End Region


End Class
