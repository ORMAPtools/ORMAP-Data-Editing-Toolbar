#Region "Copyright 2008-2011 ORMAP Tech Group"

' File:  HelpForm.vb
'
' Original Author:  OPET.NET Migration Team (Shad Campbell, James Moore, 
'                   Nick Seigal)
'
' Date Created:  February 29, 2008
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
'SCC revision number: $Revision: 212 $
'Date of Last Change: $Date: 2008-03-03 00:31:56 -0800 (Mon, 03 Mar 2008) $
#End Region

#Region "Imported Namespaces"
Imports System.Runtime.InteropServices
#End Region

''' <summary>
''' Provides user interface for Help.
''' </summary>
''' <remarks></remarks>
<ComVisible(False)> _
Public Class HelpForm

    Private Sub uxReportOrRequest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxReportOrRequest.Click
        Dim theRTFStream As System.IO.Stream = _
                Me.GetType().Assembly.GetManifestResourceStream("OrmapTaxlotEditing.ReportOrRequest_help.rtf")
        Utilities.OpenHelp("Report Bug or Request New Feature", theRTFStream)
    End Sub

    Private Sub uxRichTextBox_LinkClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.LinkClickedEventArgs) Handles uxRichTextBox.LinkClicked
        System.Diagnostics.Process.Start(e.LinkText)
    End Sub

End Class