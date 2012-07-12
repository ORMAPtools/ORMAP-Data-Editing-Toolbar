#Region "Copyright 2008-2011 ORMAP Tech Group"

' File:  StringUtilities.vb
'
' Original Author:  OPET.NET Migration Team (Shad Campbell, James Moore, 
'                   Nick Seigal)
'
' Date Created:  20080221
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
Imports System.Globalization
Imports System.Windows.Forms
Imports System.Text
#End Region

#Region "Class Declaration"
''' <summary>
''' String utilities class (singleton).
''' </summary>
''' <remarks>
''' <para>Commonly used string manipulation procedures and functions.</para>
''' <para><seealso cref="SpatialUtilities"/></para>
''' <para><seealso cref="Utilities"/></para>
''' </remarks>
Public NotInheritable Class StringUtilities

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

#Region "Public Members"

    ''' <summary>
    ''' Add leading zeros if necessary.
    ''' </summary>
    ''' <param name="currentValue">The string to pad with zeros.</param>
    ''' <param name="width">The final length of the string.</param>
    ''' <returns>A string of length width characters.</returns>
    ''' <remarks>Creates a string of width characters padded on the left with zeros.</remarks>
    Public Shared Function AddLeadingZeros(ByVal currentValue As String, ByVal width As Integer) As String
        Try
            If currentValue.Length < width Then
                Return currentValue.PadLeft(width, "0"c)
            Else
                Return currentValue
            End If
        Catch ex As Exception
            OrmapExtension.ProcessUnhandledException(ex)
            Return String.Empty
        End Try
    End Function

    ''' <summary>
    '''  Use the ORMapTaxlot value to create a MapTaxlot value based on a formatting mask.
    ''' </summary>
    ''' <param name="mapTaxlotIdValue">The ORTaxlot value</param>
    ''' <param name="format">The formatting mask as defined for each county</param>
    ''' <returns>A formatted string that can be used as parcel ID or/and as a MapTaxlot value</returns>
    ''' <remarks></remarks>
    Public Shared Function GenerateMapTaxlotValue(ByVal mapTaxlotIdValue As String, ByVal format As String) As String

        If mapTaxlotIdValue Is Nothing OrElse mapTaxlotIdValue.Length = 0 Then
            Throw New ArgumentNullException("mapTaxlotIdValue")
        End If
        If format Is Nothing OrElse format.Length = 0 Then
            Throw New ArgumentNullException("format")
        End If
        If mapTaxlotIdValue.Length < 29 Then
            Throw New ArgumentException("Invalid argument length", "mapTaxlotIdValue")
        End If

        Try
            Dim countyCode As Short
            countyCode = CShort(mapTaxlotIdValue.Substring(0, 2))

            Dim hasTownPart As Boolean
            Dim hasRangePart As Boolean
            Dim hasAlphaQtr As Boolean = False
            Dim hasAlphaQtrQtr As Boolean = False

            'flag for half township
            hasTownPart = (Convert.ToDouble(mapTaxlotIdValue.Substring(4, 3), CultureInfo.CurrentCulture) > 0)
            hasRangePart = (Convert.ToDouble(mapTaxlotIdValue.Substring(10, 3), CultureInfo.CurrentCulture) > 0)

            'flags for section quarters
            Select Case countyCode
                Case 1 To 19, 21 To 36
                    If Not IsNumeric(mapTaxlotIdValue.Substring(16, 1)) Then
                        hasAlphaQtr = True
                    End If
                    If Not IsNumeric(mapTaxlotIdValue.Substring(17, 1)) Then
                        hasAlphaQtrQtr = True
                    End If
                Case 20
                    '[Lane County]
                    ' Note: Lane County has numeric qtr. sections and qtr.qtr. sections.
            End Select

            'We must adjust the mask for Clackamas County if there are no half ranges in the current string
            If format.IndexOf("^"c) > 0 Then
                If hasRangePart = False Then
                    format = format.Remove(format.IndexOf("^"c), 1)
                Else
                    'if there is a range part the letter Q will be  placed in the position where D sits
                    format = format.Remove(format.IndexOf("D"c), 1)
                End If
            End If
            'copy of the formatstring
            Dim maskValues As New StringBuilder(format.ToUpper(CultureInfo.CurrentCulture))
            ' Create a string of spaces to place our results in. This helps a speed up string manipulation a little.
            Dim formattedResult As New StringBuilder(New String(CChar(" "), format.Length), format.Length)

            Dim positionInMask As Integer
            Dim characterCode As Integer
            Dim tokenCount As Integer
            Dim previousCharInMask As Char
            Dim hasProcessedParcelId As Boolean = False
            Dim hasProcessedTownFractional As Boolean = False
            Dim hasProcessedRangeFractional As Boolean = False

            For charIndex As Integer = 0 To maskValues.Length - 1
                positionInMask = format.IndexOf(maskValues.Chars(charIndex).ToString, charIndex, StringComparison.CurrentCultureIgnoreCase)
                characterCode = Convert.ToInt32(maskValues.Chars(charIndex))

                Dim c As Char
                For Each c In format
                    If c.Equals(maskValues.Chars(charIndex)) Then
                        tokenCount += 1 ' Returns how many of these characters appear in the mask
                    End If
                Next c

                Select Case characterCode
                    Case 68 'D
                        If String.CompareOrdinal(previousCharInMask, "^") = 0 Then
                            If String.CompareOrdinal(maskValues.Chars(positionInMask - 2), "T") = 0 Then 'township
                                formattedResult.Chars(positionInMask) = CChar(mapTaxlotIdValue.Substring(7, 1))
                            ElseIf String.CompareOrdinal(maskValues.Chars(positionInMask - 2), "R") = 0 Then 'range
                                formattedResult.Chars(positionInMask) = CChar(mapTaxlotIdValue.Substring(13, 1))
                            End If
                        Else
                            If String.CompareOrdinal(previousCharInMask, "T") = 0 Then 'township
                                formattedResult.Chars(positionInMask) = CChar(mapTaxlotIdValue.Substring(7, 1))
                            ElseIf String.CompareOrdinal(previousCharInMask, "R") = 0 Then 'range
                                formattedResult.Chars(positionInMask) = CChar(mapTaxlotIdValue.Substring(13, 1))
                            End If
                        End If
                    Case 64 '@
                        'Formats for the parcel id
                        If Not hasProcessedParcelId Then
                            'since we are at the end of the string use Insert
                            formattedResult.Insert(positionInMask, mapTaxlotIdValue.Substring(24, 5))
                            hasProcessedParcelId = True
                        End If
                    Case 38 '& Using these characters in mask will strip leading zeros from parcel id
                        If Not hasProcessedParcelId Then '
                            'since we are at the end of the string use Insert
                            Dim s As String = New String(CType(mapTaxlotIdValue.Substring(24, 5), Char()))
                            formattedResult.Insert(positionInMask, stripLeadingZeros(s))
                            hasProcessedParcelId = True
                        End If
                    Case 81 'Q
                        If String.CompareOrdinal(previousCharInMask, "Q") = 0 Then 'qtr qtr
                            If hasAlphaQtrQtr Then
                                formattedResult.Chars(positionInMask) = CChar(mapTaxlotIdValue.Substring(17, 1))
                            Else 'it is not alphabetical; could be a number or a space
                                Dim currentORMAPNumValue As String
                                currentORMAPNumValue = mapTaxlotIdValue.Substring(17, 1).ToUpper(CultureInfo.CurrentCulture)
                                If currentORMAPNumValue Like "[A-J]" Then
                                    formattedResult.Chars(positionInMask) = CChar(currentORMAPNumValue)
                                Else
                                    If countyCode <> 3 AndAlso countyCode <> 22 Then 'Clackamas County, Linn County wants the space/blank value left in the string NO ZEROES PLEASE
                                        formattedResult.Chars(positionInMask) = "0"c
                                    End If
                                End If
                            End If
                        Else 'qtr
                            If hasAlphaQtr Then
                                formattedResult.Chars(positionInMask) = CChar(mapTaxlotIdValue.Substring(16, 1))
                            Else 'it is not alphabetical; could be a number or a space
                                Dim currentORMAPNum As String
                                currentORMAPNum = mapTaxlotIdValue.Substring(16, 1)
                                If currentORMAPNum Like "[A-J]" Then
                                    formattedResult.Chars(positionInMask) = CChar(currentORMAPNum)
                                Else
                                    If countyCode <> 3 AndAlso countyCode <> 22 Then 'Clackamas County, Linn County wants the space/blank value left in the string NO ZEROES PLEASE
                                        formattedResult.Chars(positionInMask) = "0"c
                                    End If
                                End If
                            End If
                        End If

                    Case 82 'Range
                        If String.CompareOrdinal(previousCharInMask, "R") <> 0 Then
                            If tokenCount > 1 Then
                                formattedResult.Insert(positionInMask, mapTaxlotIdValue.Substring(8, tokenCount))
                            Else
                                formattedResult.Chars(positionInMask) = CChar(mapTaxlotIdValue.Substring(9, 1))
                            End If
                        End If
                    Case 83 'S section
                        If String.CompareOrdinal(previousCharInMask, "S") = 0 Then 'second position
                            formattedResult.Chars(positionInMask) = CChar(mapTaxlotIdValue.Substring(15, 1))
                        Else 'first position
                            formattedResult.Chars(positionInMask) = CChar(mapTaxlotIdValue.Substring(14, 1))
                        End If

                    Case 84 'T township
                        If String.CompareOrdinal(previousCharInMask, "T") <> 0 Then
                            If tokenCount > 1 Then
                                formattedResult.Insert(positionInMask, mapTaxlotIdValue.Substring(2, tokenCount))
                            Else
                                formattedResult.Chars(positionInMask) = CChar(mapTaxlotIdValue.Substring(3, 1))
                            End If
                        End If

                    Case 80 'P fractional parts
                        If String.CompareOrdinal(previousCharInMask, "T") = 0 Then
                            If Not hasProcessedRangeFractional Then
                                formattedResult.Insert(positionInMask, mapTaxlotIdValue.Substring(10, tokenCount))
                                hasProcessedRangeFractional = True
                            ElseIf String.CompareOrdinal(previousCharInMask, "R") = 0 Then
                                If Not hasProcessedTownFractional Then
                                    formattedResult.Chars(positionInMask) = CChar(mapTaxlotIdValue.Substring(4, 1))
                                    hasProcessedTownFractional = True
                                End If
                            End If
                        End If

                    Case 94 '^ special case for clackamas county
                        If String.CompareOrdinal(previousCharInMask, "R") = 0 Then
                            If hasRangePart Then
                                formattedResult.Chars(positionInMask) = "Q"c
                            End If
                        ElseIf String.CompareOrdinal(previousCharInMask, "T") = 0 Then 'fractional part of township
                            If hasTownPart Then
                                formattedResult.Chars(positionInMask) = "Q"c
                            End If
                        End If
                End Select
                previousCharInMask = maskValues.Chars(charIndex)
                tokenCount = 0
            Next charIndex

            Return formattedResult.ToString

        Catch ex As Exception
            OrmapExtension.ProcessUnhandledException(ex)
            Return String.Empty

        End Try

    End Function

    ''' <summary>
    ''' Isolate elements of a string.
    ''' </summary>
    ''' <param name="whole">The string to isolate the substring from.</param>
    ''' <param name="lowPart"></param>
    ''' <param name="highPart"></param>
    ''' <returns>A string that is a substring of theWholeString.</returns>
    ''' <remarks></remarks>
    Public Shared Function ExtractString(ByVal whole As String, ByVal lowPart As Integer, ByVal highPart As Integer) As String
        Try
            If lowPart <= highPart Then
                'Return Mid(theWholeString, lowPart, highPart - lowPart + 1)
                Return whole.Substring(lowPart, highPart - lowPart + 1)
            Else
                Return String.Empty
            End If
        Catch ex As Exception
            OrmapExtension.ProcessUnhandledException(ex)
            Return String.Empty
        End Try
    End Function

#End Region

#Region "Private Members"

    ''' <summary>
    ''' Trims leading zeros from a string.
    ''' </summary>
    ''' <param name="stringToParse"></param>
    ''' <returns>A copy of the string without leading zeros</returns>
    ''' <remarks></remarks>
    Private Shared Function stripLeadingZeros(ByVal stringToParse As String) As String
        Try
            Dim sb As New StringBuilder(stringToParse)

            For charIndex As Integer = 0 To sb.Length
                If Char.GetNumericValue(sb.Chars(charIndex)) = 0 Then
                    sb = sb.Replace(sb.Chars(charIndex), " "c, charIndex, 1)
                Else
                    Exit For
                End If
            Next charIndex

            Return sb.ToString  ' do not trim off leading spaces
        Catch ex As Exception
            OrmapExtension.ProcessUnhandledException(ex)
            Return String.Empty
        End Try
    End Function

#End Region

#End Region

End Class
#End Region
