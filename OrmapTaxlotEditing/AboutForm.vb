#Region "Copyright 2008 ORMAP Tech Group"

' File:  PropertiesForm.vb
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
Imports System.globalization
Imports System.IO.Path
#End Region

''' <summary>
''' Provides information about the extension, including version and license.
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class AboutForm

    ''' <summary>
    ''' Form Load event handler.
    ''' </summary>
    ''' <param name="sender">The object that raised the event.</param>
    ''' <param name="e">The event arguments.</param>
    ''' <remarks></remarks>
    Private Sub AboutForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Set the title of the form.
        Dim ApplicationTitle As String
        If My.Application.Info.Title <> String.Empty Then
            'ApplicationTitle = My.Application.Info.Title
            ApplicationTitle = My.Application.Info.ProductName
        Else
            ApplicationTitle = GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
        End If
        Me.Text = String.Format(CultureInfo.CurrentCulture, "About {0}", ApplicationTitle)
        ' Initialize all of the text displayed on the About Box.
        ' To change these values, customize the application's assembly 
        ' information in the "Application" pane of the project properties 
        ' dialog (under the "Project" menu).
        Me.LabelProductName.Text = My.Application.Info.ProductName
        Me.LabelVersion.Text = String.Format(CultureInfo.CurrentCulture, "Version {0}", My.Application.Info.Version.ToString)
        Me.LabelCopyright.Text = My.Application.Info.Copyright
        Me.LabelCompanyName.Text = My.Application.Info.CompanyName
        Me.TextBoxDescription.Text = My.Application.Info.Description
    End Sub

    ''' <summary>
    ''' Button click event handler.
    ''' </summary>
    ''' <param name="sender">The object that raised the event.</param>
    ''' <param name="e">The event arguments.</param>
    ''' <remarks></remarks>
    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OKButton.Click
        Me.Close()
    End Sub

End Class
