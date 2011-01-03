#Region "Copyright 2008 ORMAP Tech Group"

' File:  CreateAnnotationForm.vb
'
' Original Author:  Robert Gumtow
'
' Date Created:  May 13, 2010
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

#Region "Subversion Keyword expansion"
'Tag for this file: $Name$
'SCC revision number: $Revision: 212 $
'Date of Last Change: $Date: 2008-03-03 00:31:56 -0800 (Mon, 03 Mar 2008) $
#End Region

#Region "Imported Namespaces"
Imports System.Runtime.InteropServices
#End Region

''' <summary>
''' Partner form providing user interface for the CreateAnnotation class.
''' </summary>
''' <remarks><seealso cref="CreateAnnotation"/></remarks>
<ComVisible(False)> _
Public Class CreateAnnotationForm
    Inherits System.Windows.Forms.Form

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub uxParallel_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxParallel.CheckedChanged
        If uxParallel.Checked Then
            uxCurved.Enabled = True
        Else
            uxCurved.Enabled = False
            uxCurved.Checked = False
        End If
    End Sub
End Class