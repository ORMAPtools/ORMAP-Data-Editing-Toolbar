#Region "Copyright 2008-2011 ORMAP Tech Group"

' File:  ORMapNum.vb
'
' Original Author:  OPET.NET Migration Team (Shad Campbell, James Moore, 
'                   Nick Seigal)
'
' Date Created:  20080305
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
'Tag for this file: $Name:$
'SCC revision number: $Revision: 406 $
'Date of Last Change: $Date: 2009-11-30 22:49:20 -0800 (Mon, 30 Nov 2009) $
#End Region

#Region "Imported Namespaces"
Imports system.Globalization
Imports System.Runtime.InteropServices
Imports System.Text
#End Region

#Region "Class Declaration"
''' <summary>
''' Encapsulates all elements of an ORMapNum into one class.
''' </summary>
''' <remarks>Encapsulates all elements of a ORMapNum into one 
''' multipurpose class that allows an ORMapNum to be either 
''' created or parsed, manipulated, or validated against the 
''' current ORMapNum model.</remarks>
<ComVisible(False)> _
Public NotInheritable Class ORMapNum

#Region "Built-In Class Members (Constructors, Etc.)"

#Region "Constructors"

    Public Sub New()
        _county = String.Empty
        _township = String.Empty
        _partialRangeCode = String.Empty
        _partialTownshipCode = String.Empty
        _townshipDirectional = String.Empty
        _range = String.Empty
        _rangeDirectional = String.Empty
        _section = String.Empty
        _quarter = String.Empty
        _quarterQuarter = String.Empty
        _suffixType = String.Empty
        _suffixNumber = String.Empty
        _anomaly = String.Empty

    End Sub

#End Region

#End Region

#Region "Custom class members"

#Region "Fields (none)"
#End Region

#Region "Events"
    ''' <summary>
    ''' Event triggered when ORMAP number changes.
    ''' </summary>
    Friend Event OnChange(ByVal newNumber As String)
#End Region

#Region "Properties"

    Private _county As String

    ''' <summary>
    ''' Two digit code for the County -- Default: user-defined.
    ''' </summary>
    Public Property County() As String
        Get
            County = _county
        End Get
        Set(ByVal value As String)
            Dim length As Integer = value.Length
            Select Case length
                Case Is < 2
                    Dim sb As New StringBuilder("00", 2 - length)
                    sb.Append(value)
                    _county = sb.ToString
                Case 2
                    _county = value
                Case Is > 2
                    _county = value.Substring(0, 2) 'left(value,2)
            End Select
            RaiseEvent OnChange(Me.GetOrmapMapNumber)
        End Set

    End Property

    Private _township As String

    ''' <summary>
    ''' Two digit code for the township -- Default: 00.
    ''' </summary>
    Public Property Township() As String
        Get
            Township = _township
        End Get
        Set(ByVal value As String)
            If value.Length <> 2 Then
                _township = "00" ' ENHANCE: [NIS] Add Township to OrmapExtension.DefaultValuesSettings and use here.
            Else
                _township = value
            End If
            RaiseEvent OnChange(Me.GetOrmapMapNumber)
        End Set
    End Property

    Private _partialTownshipCode As String

    ''' <summary>
    ''' Three digit code for the partial township code -- Default: .00 (ORCS ver 1.4) or user-defined.
    ''' </summary>
    Public Property PartialTownshipCode() As String
        Get
            PartialTownshipCode = _partialTownshipCode
        End Get
        Set(ByVal value As String)
            Select Case value
                Case "0.25", "0.50", "0.75"
                    _partialTownshipCode = value.Substring(1, 3) 'same as mid(value,2)
                Case Else
                    _partialTownshipCode = OrmapExtension.DefaultValuesSettings.TownshipPart
            End Select
            RaiseEvent OnChange(Me.GetOrmapMapNumber)
        End Set
    End Property

    Private _townshipDirectional As String

    ''' <summary>
    ''' One digit directional (N or S) for the township -- Default: N or user-defined.
    ''' </summary>
    Public Property TownshipDirectional() As String
        Get
            TownshipDirectional = _townshipDirectional
        End Get
        Set(ByVal value As String)
            Select Case value
                Case "N", "S"
                    _townshipDirectional = value
                Case Else
                    _townshipDirectional = OrmapExtension.DefaultValuesSettings.TownshipDirection
            End Select
            RaiseEvent OnChange(Me.GetOrmapMapNumber)
        End Set
    End Property

    Private _range As String

    ''' <summary>
    ''' Two digit code for the range -- Default: 01.
    ''' </summary>
    Public Property Range() As String
        Get
            Range = _range
        End Get
        Set(ByVal value As String)
            If value.Length <> 2 Then
                _range = "01" ' ENHANCE: [NIS] Add Range to OrmapExtension.DefaultValuesSettings and use here.
            Else
                _range = value
            End If

            RaiseEvent OnChange(Me.GetOrmapMapNumber)
        End Set
    End Property

    Private _partialRangeCode As String

    ''' <summary>
    ''' Three-digit code for the partial range code -- Default: .00 (ORCS ver 1.4) or user-defined.
    ''' </summary>
    Public Property PartialRangeCode() As String
        Get
            PartialRangeCode = _partialRangeCode
        End Get
        Set(ByVal value As String)
            Select Case value
                Case "0.25", "0.50", "0.75"
                    _partialRangeCode = value.Substring(1, 3) 'Mid(value, 2)
                Case Else
                    _partialRangeCode = OrmapExtension.DefaultValuesSettings.RangePart
            End Select
            RaiseEvent OnChange(Me.GetOrmapMapNumber)
        End Set
    End Property

    Private _rangeDirectional As String

    ''' <summary>
    ''' One digit code for the directional (E or W) for the range -- Default: W or user-defined.
    ''' </summary>
    Public Property RangeDirectional() As String
        Get
            RangeDirectional = _rangeDirectional
        End Get
        Set(ByVal value As String)
            Select Case value
                Case "E", "W"
                    _rangeDirectional = value
                Case Else
                    _rangeDirectional = OrmapExtension.DefaultValuesSettings.RangeDirection
            End Select
            RaiseEvent OnChange(Me.GetOrmapMapNumber)
        End Set
    End Property

    Private _section As String

    ''' <summary>
    ''' Two digit code for the section number from 00 to 37 -- Default: 00 (ORCS ver 1.4) or user-defined.
    ''' </summary>
    Public Property Section() As String
        Get
            Section = _section
        End Get
        Set(ByVal value As String)
            Dim valueAsInteger As Integer
            If Integer.TryParse(value, valueAsInteger) Then
                Select Case valueAsInteger
                    Case 0
                        _section = "00" ' ENHANCE: [NIS] Add Section to OrmapExtension.DefaultValuesSettings and use here.
                    Case Is < 10
                        _section = "0" & CShort(value)
                    Case Is <= 37
                        _section = value
                    Case Else
                        _section = "00" ' ENHANCE: [NIS] Add Section to OrmapExtension.DefaultValuesSettings and use here.
                End Select
            Else
                _section = "00" ' ENHANCE: [NIS] Add Section to OrmapExtension.DefaultValuesSettings and use here.
            End If
            RaiseEvent OnChange(Me.GetOrmapMapNumber)
        End Set
    End Property

    Private _quarter As String

    ''' <summary>
    ''' One digit code for the quarter from A to J -- Default: 0 (ORCS ver 1.4) or user-defined.
    ''' </summary>
    Public Property Quarter() As String
        Get
            Quarter = _quarter
        End Get
        Set(ByVal value As String)
            Select Case value.ToUpper(CultureInfo.CurrentCulture)
                Case "0", "A" To "J"
                    _quarter = value
                Case Else
                    _quarter = OrmapExtension.DefaultValuesSettings.QuarterSection
            End Select
            RaiseEvent OnChange(Me.GetOrmapMapNumber)
        End Set
    End Property

    Private _quarterQuarter As String

    ''' <summary>
    ''' One digit code for the quarter/quarter from A to J -- Default: 0 (ORCS ver 1.4) or user-defined.
    ''' </summary>
    Public Property QuarterQuarter() As String
        Get
            QuarterQuarter = _quarterQuarter
        End Get
        Set(ByVal value As String)
            Select Case value.ToUpper(CultureInfo.CurrentCulture)
                Case "0", "A" To "J"
                    _quarterQuarter = value
                Case Else
                    _quarterQuarter = OrmapExtension.DefaultValuesSettings.QuarterQuarterSection
            End Select
            RaiseEvent OnChange(Me.GetOrmapMapNumber)
        End Set
    End Property

    Private _suffixType As String

    ''' <summary>
    ''' One digit code, S, D, T, or 0, for suffix type -- Default: 0 (ORCS ver 1.4) or user-defined.
    ''' </summary>
    Public Property SuffixType() As String
        Get
            SuffixType = _suffixType
        End Get
        Set(ByVal value As String)
            Select Case value.ToUpper(CultureInfo.CurrentCulture)
                Case "0", "D", "S", "T"
                    _suffixType = value
                Case Else
                    _suffixType = OrmapExtension.DefaultValuesSettings.MapSuffixType
            End Select
            RaiseEvent OnChange(Me.GetOrmapMapNumber)
        End Set
    End Property

    Private _suffixNumber As String

    ''' <summary>
    ''' Three digit code for the suffix number from 000 to 999 -- Default: 000 (ORCS ver 1.4) or user-defined..
    ''' </summary>
    Public Property SuffixNumber() As String
        Get
            SuffixNumber = _suffixNumber
        End Get
        Set(ByVal value As String)

            If IsNumeric(value) AndAlso value.Length > 0 Then
                Select Case value.Length
                    Case Is <= 0
                        _suffixNumber = OrmapExtension.DefaultValuesSettings.MapSuffixNumber
                    Case Is = 1, 2
                        _suffixNumber = New String("0"c, 3 - value.Length) & value
                    Case Is = 3
                        _suffixNumber = value
                    Case Is >= 4
                        _suffixNumber = OrmapExtension.DefaultValuesSettings.MapSuffixNumber
                    Case Else
                        _suffixNumber = OrmapExtension.DefaultValuesSettings.MapSuffixNumber
                End Select
            Else
                _suffixNumber = OrmapExtension.DefaultValuesSettings.MapSuffixNumber
            End If
            RaiseEvent OnChange(Me.GetOrmapMapNumber)
        End Set
    End Property

    Private _anomaly As String

    ''' <summary>
    ''' Two digit code for any oddball situations -- Default: -- (ORCS ver 1.4) or user-defined..
    ''' </summary>
    Public Property Anomaly() As String
        Get
            Anomaly = _anomaly
        End Get
        Set(ByVal value As String)
            If value.Length <> 2 Then
                _anomaly = OrmapExtension.DefaultValuesSettings.Anomaly
            Else
                _anomaly = value
            End If
            RaiseEvent OnChange(Me.GetOrmapMapNumber)
        End Set
    End Property

#End Region

#Region "Methods"

    ''' <summary>
    ''' Parse an ORMAP Number into its component pieces.
    ''' </summary>
    Public Function ParseNumber(ByVal number As String) As Boolean
        Dim returnValue As Boolean = False
        If number.Length >= GetMapNumFieldLength() Then
            Me.County = number.Substring(0, 2)
            Me.Township = number.Substring(2, 2)
            Me.PartialTownshipCode = number.Substring(4, 3)
            Me.TownshipDirectional = number.Substring(7, 1)
            Me.Range = number.Substring(8, 2)
            Me.PartialRangeCode = number.Substring(10, 3)
            Me.RangeDirectional = number.Substring(13, 1)
            Me.Section = number.Substring(14, 2)
            Me.Quarter = number.Substring(16, 1)
            Me.QuarterQuarter = number.Substring(17, 1)
            Me.Anomaly = number.Substring(18, 2)
            Me.SuffixType = number.Substring(20, 1)
            Me.SuffixNumber = number.Substring(21, 3)
            returnValue = True
        End If
        Return returnValue
    End Function

    ''' <summary>
    ''' Returns a properly formatted ORMAP Number minus the County code.
    ''' </summary>
    Public Function GetOrmapMapNumber() As String
        If IsValidNumber() Then
            Dim sb As New StringBuilder(_township, ORMapNum.GetMapNumFieldLength())
            sb.Append(_partialTownshipCode)
            sb.Append(_townshipDirectional)
            sb.Append(_range)
            sb.Append(_partialRangeCode)
            sb.Append(_rangeDirectional)
            sb.Append(_section)
            sb.Append(_quarter)
            sb.Append(_quarterQuarter)
            sb.Append(_anomaly)
            sb.Append(_suffixType)
            sb.Append(_suffixNumber)

            Return sb.ToString
        Else
            Return String.Empty
        End If
    End Function

    ''' <summary>
    ''' Get OR Map Number (ORMapNum field).
    ''' </summary>
    ''' <returns>Returns a properly formatted OR Map Number.</returns>
    ''' <remarks>This function returns the same values as the OrmapTaxlotNumber member function in the VB6 version of ORMAPNumber class</remarks>
    Public Function GetORMapNum() As String
        ' Creates a formatted ORMAP Map Number
        If IsValidNumber() Then
            Dim sb As New StringBuilder(_county, ORMapNum.GetMapNumFieldLength())
            sb.Append(_township)
            sb.Append(_partialTownshipCode)
            sb.Append(_townshipDirectional)
            sb.Append(_range)
            sb.Append(_partialRangeCode)
            sb.Append(_rangeDirectional)
            sb.Append(_section)
            sb.Append(_quarter)
            sb.Append(_quarterQuarter)
            sb.Append(_anomaly)
            sb.Append(_suffixType)
            sb.Append(_suffixNumber)

            Return sb.ToString
        Else
            Return String.Empty
        End If
    End Function


    ''' <summary>
    ''' Validate ORMAP Numbers.
    ''' </summary>
    ''' <remarks>Determines validity based on all elements having a length of greater than 0.</remarks>
    ''' <returns>Boolean value representing the Valid status of the number.</returns>
    Public Function IsValidNumber() As Boolean
        Dim returnValue As Boolean = True
        returnValue = returnValue AndAlso _county.Length > 0
        returnValue = returnValue AndAlso _township.Length > 0
        returnValue = returnValue AndAlso _partialTownshipCode.Length > 0
        returnValue = returnValue AndAlso _townshipDirectional.Length > 0
        returnValue = returnValue AndAlso _range.Length > 0
        returnValue = returnValue AndAlso _partialRangeCode.Length > 0
        returnValue = returnValue AndAlso _rangeDirectional.Length > 0
        returnValue = returnValue AndAlso _section.Length > 0
        returnValue = returnValue AndAlso _quarter.Length > 0
        returnValue = returnValue AndAlso _quarterQuarter.Length > 0
        returnValue = returnValue AndAlso _suffixType.Length > 0
        returnValue = returnValue AndAlso _suffixNumber.Length > 0
        returnValue = returnValue AndAlso _anomaly.Length > 0
        Return returnValue
    End Function

    ''' <summary>
    ''' Length of ORMAPMapNum field.
    ''' </summary>
    ''' <returns>Number of characters allowed in this field as integer.</returns>
    Public Shared Function GetMapNumFieldLength() As Integer
        Return 24
    End Function

    ''' <summary>
    ''' Length of the taxlot field.
    ''' </summary>
    ''' <returns>Integer.</returns>
    Public Shared Function GetTaxlotFieldLength() As Integer
        Return 5
    End Function

    ''' <summary>
    ''' Combines MapNumFieldLength and OrmapTaxlotFieldLength.
    ''' </summary>
    ''' <returns>Number of characters allowed in this field as integer.</returns>
    ''' <remarks>Was ORMAP_TAXLOT_FIELD_LENGTH in previous (VB6) version.</remarks>
    Public Shared Function GetORTaxlotFieldLength() As Integer
        Return (GetMapNumFieldLength() + GetTaxlotFieldLength())
    End Function

#End Region

#End Region
End Class
#End Region
