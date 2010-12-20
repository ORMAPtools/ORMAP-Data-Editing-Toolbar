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
Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Runtime.InteropServices
#End Region

''' <summary>
''' Partner form providing user interface for the PropertyPage class.
''' </summary>
''' <remarks><seealso cref="PropertyPage"/></remarks>
<ComVisible(False)> _
Friend NotInheritable Class PropertiesForm
    Inherits System.Windows.Forms.Form

#Region "Class-Level Constants and Enumerations (none)"
#End Region

#Region "Built-In Class Members (Constructors, Etc.)"

#Region "Constructors"

    Public Sub New()
        '
        ' Required for Windows Form Designer support
        '
        InitializeComponent()

        ' NOTE: Add any constructor code after InitializeComponent call...

    End Sub

#End Region

#End Region

#Region "Custom Class Members"

#Region "Fields"

    ''' <summary>
    ''' Required designer variable.
    ''' </summary>
    Private components As System.ComponentModel.Container = Nothing

    Friend WithEvents uxEnableTools As CheckBox
    Friend WithEvents uxEnableAutoUpdate As CheckBox
    Friend WithEvents uxDescription As Label
    Friend WithEvents uxMinimumFieldsOption As RadioButton
    Friend WithEvents uxAllFieldsOption As RadioButton
    Friend WithEvents uxAbout As System.Windows.Forms.Button
    Friend WithEvents uxReportOrRequest As System.Windows.Forms.Button
    Friend WithEvents uxSettings As Button

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
    ''' Clean up any resources being used.
    ''' </summary>
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not components Is Nothing Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

#End Region

#End Region

#Region "Implemented Interface Members (none)"
#End Region

#Region "Other Members"

#Region "Windows Form Designer generated code"
    ''' <summary>
    ''' Required method for Designer support - do not modify
    ''' the contents of this method with the code editor.
    ''' </summary>
    Private Sub InitializeComponent()
        Me.uxDescription = New System.Windows.Forms.Label
        Me.uxMinimumFieldsOption = New System.Windows.Forms.RadioButton
        Me.uxAllFieldsOption = New System.Windows.Forms.RadioButton
        Me.uxEnableAutoUpdate = New System.Windows.Forms.CheckBox
        Me.uxEnableTools = New System.Windows.Forms.CheckBox
        Me.uxSettings = New System.Windows.Forms.Button
        Me.uxAbout = New System.Windows.Forms.Button
        Me.uxReportOrRequest = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'uxDescription
        '
        Me.uxDescription.Location = New System.Drawing.Point(11, 13)
        Me.uxDescription.Name = "uxDescription"
        Me.uxDescription.Size = New System.Drawing.Size(370, 34)
        Me.uxDescription.TabIndex = 0
        Me.uxDescription.Text = "The ORMAP Taxlot Editing Toolbar gives the user tools to edit taxlots and related" & _
            " features in compliance with the ORMAP standard."
        '
        'uxMinimumFieldsOption
        '
        Me.uxMinimumFieldsOption.AutoSize = True
        Me.uxMinimumFieldsOption.Location = New System.Drawing.Point(53, 97)
        Me.uxMinimumFieldsOption.Name = "uxMinimumFieldsOption"
        Me.uxMinimumFieldsOption.Size = New System.Drawing.Size(285, 17)
        Me.uxMinimumFieldsOption.TabIndex = 3
        Me.uxMinimumFieldsOption.Text = "Minimum fields only (e.g. AUTODATE and AUTOWHO)"
        Me.uxMinimumFieldsOption.UseVisualStyleBackColor = True
        '
        'uxAllFieldsOption
        '
        Me.uxAllFieldsOption.AutoSize = True
        Me.uxAllFieldsOption.Checked = True
        Me.uxAllFieldsOption.Location = New System.Drawing.Point(53, 121)
        Me.uxAllFieldsOption.Name = "uxAllFieldsOption"
        Me.uxAllFieldsOption.Size = New System.Drawing.Size(63, 17)
        Me.uxAllFieldsOption.TabIndex = 4
        Me.uxAllFieldsOption.TabStop = True
        Me.uxAllFieldsOption.Text = "All fields"
        Me.uxAllFieldsOption.UseVisualStyleBackColor = True
        '
        'uxEnableAutoUpdate
        '
        Me.uxEnableAutoUpdate.Checked = True
        Me.uxEnableAutoUpdate.CheckState = System.Windows.Forms.CheckState.Checked
        Me.uxEnableAutoUpdate.Location = New System.Drawing.Point(33, 73)
        Me.uxEnableAutoUpdate.Name = "uxEnableAutoUpdate"
        Me.uxEnableAutoUpdate.Size = New System.Drawing.Size(284, 17)
        Me.uxEnableAutoUpdate.TabIndex = 2
        Me.uxEnableAutoUpdate.Text = "Enable field auto-updates"
        '
        'uxEnableTools
        '
        Me.uxEnableTools.Checked = True
        Me.uxEnableTools.CheckState = System.Windows.Forms.CheckState.Checked
        Me.uxEnableTools.Location = New System.Drawing.Point(14, 50)
        Me.uxEnableTools.Name = "uxEnableTools"
        Me.uxEnableTools.Size = New System.Drawing.Size(284, 17)
        Me.uxEnableTools.TabIndex = 1
        Me.uxEnableTools.Text = "Enable taxlot editing tools"
        '
        'uxSettings
        '
        Me.uxSettings.Location = New System.Drawing.Point(14, 164)
        Me.uxSettings.Name = "uxSettings"
        Me.uxSettings.Size = New System.Drawing.Size(86, 23)
        Me.uxSettings.TabIndex = 5
        Me.uxSettings.Text = "&Settings..."
        Me.uxSettings.UseVisualStyleBackColor = True
        '
        'uxAbout
        '
        Me.uxAbout.Location = New System.Drawing.Point(11, 362)
        Me.uxAbout.Name = "uxAbout"
        Me.uxAbout.Size = New System.Drawing.Size(86, 23)
        Me.uxAbout.TabIndex = 6
        Me.uxAbout.Text = "&About..."
        Me.uxAbout.UseVisualStyleBackColor = True
        '
        'uxReportOrRequest
        '
        Me.uxReportOrRequest.Location = New System.Drawing.Point(103, 362)
        Me.uxReportOrRequest.Name = "uxReportOrRequest"
        Me.uxReportOrRequest.Size = New System.Drawing.Size(202, 23)
        Me.uxReportOrRequest.TabIndex = 8
        Me.uxReportOrRequest.Text = "&Report Bug or Request New Feature..."
        Me.uxReportOrRequest.UseVisualStyleBackColor = True
        '
        'PropertiesForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(456, 398)
        Me.Controls.Add(Me.uxReportOrRequest)
        Me.Controls.Add(Me.uxAbout)
        Me.Controls.Add(Me.uxSettings)
        Me.Controls.Add(Me.uxAllFieldsOption)
        Me.Controls.Add(Me.uxMinimumFieldsOption)
        Me.Controls.Add(Me.uxDescription)
        Me.Controls.Add(Me.uxEnableAutoUpdate)
        Me.Controls.Add(Me.uxEnableTools)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "PropertiesForm"
        Me.Padding = New System.Windows.Forms.Padding(8, 0, 0, 0)
        Me.Text = "PropertiesForm"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region

#End Region

End Class

