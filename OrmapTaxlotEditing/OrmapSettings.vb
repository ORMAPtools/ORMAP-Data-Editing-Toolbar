#Region "Copyright 2008 ORMAP Tech Group"

' File:  OrmapSettings.vb
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

' TODO: [NIS] Implement this class and move code here from the form so as to seperate the UI and business logic layers.

#Region "Imported Namespaces"
Imports System.Runtime.InteropServices
#End Region

''' <summary>Provides business logic for the OrmapSettings UI.</summary>
''' <remarks>
''' <para>Not in use. Intent is to implement this class and move code here 
''' from the OrmapSettings form so as to seperate the UI and business logic 
''' layers.</para>
''' <para><seealso cref="OrmapSettingsForm"/></para>
''' </remarks>
<ComVisible(False)> _
Public NotInheritable Class OrmapSettings

#Region "Class-Level Constants and Enumerations (none)"
#End Region

#Region "Built-In Class Members (Constructors, Etc.)"

#Region "Constructors"

    ''' <summary>
    ''' Private empty constructor to prevent instantiation.
    ''' </summary>
    ''' <remarks>This class follows the singleton pattern and thus has a 
    ''' private constructor and all shared members. Instances of types 
    ''' that define only shared members do not need to be created, so no
    ''' constructor should be needed. However, many compilers will 
    ''' automatically add a public default constructor if no constructor 
    ''' is specified. To prevent this an empty private constructor is 
    ''' added.</remarks>
    Private Sub New()
    End Sub

#End Region

#End Region


#Region "Custom Class Members"

#Region "Fields (none)"
#End Region

#Region "Properties"

    Private WithEvents _partnerOrmapSettingsForm As OrmapSettingsForm

    Friend ReadOnly Property PartnerOrmapSettingsForm() As OrmapSettingsForm
        Get
            Return _partnerOrmapSettingsForm
        End Get
    End Property

    Private Sub SetPartnerOrmapSettingsForm(ByVal value As OrmapSettingsForm)
        _partnerOrmapSettingsForm = value
    End Sub

#End Region

#Region "Event Handlers (none)"
#End Region

#Region "Methods (none)"
#End Region

#End Region

#Region "Inherited Class Members (none)"

#Region "Properties (none)"
#End Region

#Region "Methods (none)"
#End Region

#End Region

#Region "Implemented Interface Members (none)"
#End Region

#Region "Other Members (none)"
#End Region

End Class
