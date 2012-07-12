#Region "Copyright 2008-2011 ORMAP Tech Group"

' File:  AnnoBothDownWide.vb
'
' Original Author:  Robert Gumtow
'
' Date Created:  June, 2010
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
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Environment
Imports System.Globalization
Imports System.Drawing.Text
Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Editor
Imports ESRI.ArcGIS.Framework
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Display

#End Region
''' <summary>
''' Moves annotation which straddles a line down by one wide space
''' </summary>
Public NotInheritable Class AnnoBothDownWide

#Region "Class-Level Constants and Enumerations (none)"
#End Region

#Region "Built-In Class Members (Constructors, Etc.)"

#Region "Constructors"

    Public Sub New()
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

#Region "Methods"
    Friend Sub DoButtonOperation()

        Try
            DataMonitor.CheckValidMapIndexDataProperties()
            If Not DataMonitor.HasValidMapIndexData Then
                MessageBox.Show("Missing data: Valid ORMAP MapIndex layer not found in the map." & NewLine & _
                                "Please load this dataset into your map.", _
                                "AnnoBothDownWide", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                Exit Sub
            End If
            AnnotationUtilities.MoveAnnotationElements(False, False, False, True, False, True)
        Catch ex As Exception
            OrmapExtension.ProcessUnhandledException(ex)

        End Try

    End Sub
#End Region

#End Region

End Class



