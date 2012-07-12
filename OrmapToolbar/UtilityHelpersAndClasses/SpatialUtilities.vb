#Region "Copyright 2008-2011 ORMAP Tech Group"

' File:  SpatialUtilities.vb
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

Imports System.Collections.Generic
Imports System.Environment
Imports System.Globalization
Imports System.Text
Imports System.Threading.Thread
Imports System.Windows.Forms
Imports ESRI.ArcGIS
Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Catalog
Imports ESRI.ArcGIS.DataSourcesGDB
Imports ESRI.ArcGIS.Display
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Geometry
Imports OrmapTaxlotEditing.StringUtilities
Imports OrmapTaxlotEditing.Utilities
Imports OrmapTaxlotEditing.DataMonitor

Imports ESRI.ArcGIS.Framework
Imports System.Runtime.InteropServices



#End Region

#Region "Class Declaration"
''' <summary>
''' Spatial utilities class (singleton).
''' </summary>
''' <remarks>
''' <para>Commonly used ArcObjects procedures and functions.</para>
''' <para><seealso cref="StringUtilities"/></para>
''' <para><seealso cref="Utilities"/></para>
''' </remarks>
Public NotInheritable Class SpatialUtilities

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
    ''' Add the descriptive values from each domain to the drop down comboboxes.
    ''' </summary>
    ''' <param name="inFieldName">Name of the field to draw the domain from.</param>
    ''' <param name="inFeatureClass">The feature class whose fields collection contains 
    ''' <paramref name="inFieldName">fieldName</paramref>.</param>
    ''' <param name="inComboBox">The combobox to populate.</param>
    ''' <param name="inCurrentValue">The current value of the field.</param>
    ''' <param name="allowSpace">Allow a space/null entry in the list.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function AddCodesToCombo(ByVal inFieldName As String, ByVal inFeatureClass As IFeatureClass, ByVal inComboBox As ComboBox, ByVal inCurrentValue As Object, ByVal allowSpace As Boolean) As Boolean
        Dim returnValue As Boolean = False

        Dim theFieldIndex As Integer = inFeatureClass.Fields.FindField(inFieldName)
        If theFieldIndex = NotFoundIndex Then
            '[Field not found...]
            Return False
        End If
        Dim theField As IField
        theField = inFeatureClass.Fields.Field(theFieldIndex)

        Dim theDomain As IDomain
        theDomain = theField.Domain
        If theDomain Is Nothing Then
            '[Not a valid domain...]
            Return False
        End If

        If theDomain.Type = esriDomainType.esriDTCodedValue Then
            ' Populate the list
            Dim theCodedValueDomain As ICodedValueDomain
            theCodedValueDomain = DirectCast(theDomain, ICodedValueDomain)
            Dim codeCount As Integer = theCodedValueDomain.CodeCount
            For i As Integer = 0 To codeCount - 1
                inComboBox.Items.Add(theCodedValueDomain.Name(i))
            Next i
            ' Remove spaces if directed to.
            If Not allowSpace Then
                With inComboBox
                    If .Items.Count > 0 Then
                        'find the blank
                        Dim textPosition As Integer = .FindStringExact(String.Empty, -1)
                        If textPosition > NotFoundIndex Then
                            .Items.RemoveAt(textPosition)
                        End If
                    End If
                End With
            End If
            ' Set the current list index.
            If TypeOf inCurrentValue Is String Then
                ' Select the existing value from the list
                inComboBox.SelectedIndex = inComboBox.FindStringExact(CStr(inCurrentValue), 0)
                If inComboBox.SelectedIndex = NotFoundIndex Then
                    If inCurrentValue.Equals(String.Empty) AndAlso allowSpace Then
                        ' Add the empty string as an item
                        inComboBox.Items.Insert(0, String.Empty)
                    End If
                    ' Select the first item
                    inComboBox.SelectedIndex = 0
                End If
            End If

            returnValue = True
        Else
            ' Get unique values from the table for this field and add to combo list

            Dim theQueryFilter As IQueryFilter = New QueryFilter
            theQueryFilter.SubFields = inFieldName

            Dim theFeatCursor As IFeatureCursor = inFeatureClass.Search(theQueryFilter, False)
            Dim theDataStats As IDataStatistics = New DataStatistics
            Dim theDataStatsEnum As IEnumerator
            With theDataStats
                .Cursor = DirectCast(theFeatCursor, ICursor)
                .Field = inFieldName
                theDataStatsEnum = CType(.UniqueValues, IEnumerator)
            End With
            Do Until theDataStatsEnum.MoveNext = False
                inComboBox.Items.Add(theDataStatsEnum.Current.ToString)
            Loop

        End If

        Return returnValue

    End Function

    ''' <summary>
    ''' Obtains ORMapNum via overlay and calculates other field values.
    ''' </summary>
    ''' <param name="editFeature">A feature from the Taxlot feature class.</param>
    ''' <param name="mapIndexLayer">The Map Index feature layer.</param>
    ''' <remarks><para>Updates the ORMAP fields in <paramref name="editFeature"/> to 
    ''' reflect the current ORMapNum and MapNumber values in the 
    ''' overlaying <paramref name="mapIndexLayer"/> polygon.</para>
    ''' <para>The following fields are updated:</para>
    ''' <list type="table">
    '''   <listheader><term>Field</term><description>Source</description></listheader>
    '''   <item><term>County</term><description>MapIndex</description></item>
    '''   <item><term>Town</term><description>MapIndex</description></item>
    '''   <item><term>TownPart</term><description>MapIndex</description></item>
    '''   <item><term>TownDir</term><description>MapIndex</description></item>
    '''   <item><term>Range</term><description>MapIndex</description></item>
    '''   <item><term>RangePart</term><description>MapIndex</description></item>
    '''   <item><term>RangeDir</term><description>MapIndex</description></item>
    '''   <item><term>Section</term><description>MapIndex</description></item>
    '''   <item><term>Qrtr</term><description>MapIndex</description></item>
    '''   <item><term>QrtrQrtr</term><description>MapIndex</description></item>
    '''   <item><term>MapSufType</term><description>MapIndex</description></item>
    '''   <item><term>MapSufNum</term><description>MapIndex</description></item>
    '''   <item><term>Anomaly</term><description>MapIndex</description></item>
    '''   <item><term>MapNumber</term><description>MapIndex</description></item>
    '''   <item><term>ORMapNum</term><description>MapIndex</description></item>
    '''   <item><term>Taxlot</term><description>(not updated here)</description></item>
    '''   <item><term>MapTaxlot</term><description>Combination of MapIndex.MapNum and current Taxlot</description></item>
    '''   <item><term>ORTaxlot</term><description>Combination of MapIndex.ORMapNum and current Taxlot</description></item>
    '''   <item><term>MapAcres</term><description>(feature area / 43560)</description></item>
    ''' </list>
    ''' </remarks>
    Public Shared Sub CalculateTaxlotValues(ByVal editFeature As ESRI.ArcGIS.Geodatabase.IFeature, ByVal mapIndexLayer As ESRI.ArcGIS.Carto.IFeatureLayer)

        Dim theORMapNumClass As New ORMapNum()

        Try
            ' Check for valid data (will try to load data if not found)
            CheckValidTaxlotDataProperties()
            If Not HasValidTaxlotData Then
                MessageBox.Show("Unable to update Taxlot field values." & NewLine & _
                                "Missing data: Valid ORMAP Taxlot layer not found in the map." & NewLine & _
                                "Please load this dataset into your map.", _
                                "Calculate Taxlot Values", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                Exit Sub
            End If
            CheckValidMapIndexDataProperties()
            If Not HasValidMapIndexData Then
                MessageBox.Show("Unable to update taxlot field values." & NewLine & _
                                "Missing data: Valid ORMAP MapIndex layer not found in the map." & NewLine & _
                                "Please load this dataset into your map.", _
                                "Calculate Taxlot Values", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                Exit Sub
            End If

            ' Get the Taxlot feature class from the feature being edited.
            Dim taxlotFClass As ESRI.ArcGIS.Geodatabase.IFeatureClass
            taxlotFClass = DirectCast(editFeature.Class, ESRI.ArcGIS.Geodatabase.IFeatureClass)

            Dim theCountyFieldIndex As Integer = LocateFields(taxlotFClass, OrmapExtension.TaxLotSettings.CountyField)
            Dim theTownFieldIndex As Integer = LocateFields(taxlotFClass, OrmapExtension.TaxLotSettings.TownshipField)
            Dim theTownPartFieldIndex As Integer = LocateFields(taxlotFClass, OrmapExtension.TaxLotSettings.TownshipPartField)
            Dim theTownDirFieldIndex As Integer = LocateFields(taxlotFClass, OrmapExtension.TaxLotSettings.TownshipDirectionField)
            Dim theRangeFieldIndex As Integer = LocateFields(taxlotFClass, OrmapExtension.TaxLotSettings.RangeField)
            Dim theRangePartFieldIndex As Integer = LocateFields(taxlotFClass, OrmapExtension.TaxLotSettings.RangePartField)
            Dim theRangeDirFieldIndex As Integer = LocateFields(taxlotFClass, OrmapExtension.TaxLotSettings.RangeDirectionField)
            Dim theSectionFieldIndex As Integer = LocateFields(taxlotFClass, OrmapExtension.TaxLotSettings.SectionNumberField)
            Dim theQrtrFieldIndex As Integer = LocateFields(taxlotFClass, OrmapExtension.TaxLotSettings.QuarterSectionField)
            Dim theQrtrQrtrFieldIndex As Integer = LocateFields(taxlotFClass, OrmapExtension.TaxLotSettings.QuarterQuarterSectionField)
            Dim theMapSufTypeFieldIndex As Integer = LocateFields(taxlotFClass, OrmapExtension.TaxLotSettings.MapSuffixTypeField)
            Dim theMapSufNumFieldIndex As Integer = LocateFields(taxlotFClass, OrmapExtension.TaxLotSettings.MapSuffixNumberField)
            Dim theAnomalyFieldIndex As Integer = LocateFields(taxlotFClass, OrmapExtension.TaxLotSettings.AnomalyField)
            Dim theMapNumberFieldIndex As Integer = LocateFields(taxlotFClass, OrmapExtension.TaxLotSettings.MapNumberField)
            Dim theORMapNumFieldIndex As Integer = LocateFields(taxlotFClass, OrmapExtension.TaxLotSettings.OrmapMapNumberField)
            Dim theTaxlotFieldIndex As Integer = LocateFields(taxlotFClass, OrmapExtension.TaxLotSettings.TaxlotField)
            'NOT USED - Dim theSpcIntrstFieldIndex As Integer = LocateFields(taxlotFClass, OrmapExtension.TaxLotSettings.SpecialInterestField)
            Dim theMapTaxlotFieldIndex As Integer = LocateFields(taxlotFClass, OrmapExtension.TaxLotSettings.MapTaxlotField)
            Dim theORTaxlotFieldIndex As Integer = LocateFields(taxlotFClass, OrmapExtension.TaxLotSettings.OrmapTaxlotField)
            Dim theMapAcresFieldIndex As Integer = LocateFields(taxlotFClass, OrmapExtension.TaxLotSettings.MapAcresField)

            '------------------------------------------
            ' Get the area
            '------------------------------------------
            Dim theArea As IArea
            theArea = DirectCast(editFeature.Shape, ESRI.ArcGIS.Geometry.IArea)

            '------------------------------------------
            ' Get the county MapNumber from the MapIndex.
            '------------------------------------------
            Dim theCurrentMapNumber As String = GetValue(editFeature.ShapeCopy, mapIndexLayer.FeatureClass, OrmapExtension.MapIndexSettings.MapNumberField, OrmapExtension.MapIndexSettings.MapNumberField)
            If theCurrentMapNumber.Length = 0 Then
                Exit Sub
            End If

            '------------------------------------------
            ' Reformat Special Interest Code to exactly
            ' 5 characters.
            '------------------------------------------
            'SFBUG START JWM 05/02/2008 Sourceforge Tracker 1922332 ++++++++++
            'Dim theCurrentSpecialInterest As String = "00000"
            'If Not IsDBNull(editFeature.Value(theSpcIntrstFldIdx)) Then
            '    theCurrentSpecialInterest = CStr(editFeature.Value(theSpcIntrstFldIdx))
            '    If theCurrentSpecialInterest.Length < 5 Then
            '        theCurrentSpecialInterest = theCurrentSpecialInterest.PadLeft(5, "0"c)
            '    ElseIf theCurrentSpecialInterest.Length > 5 Then
            '        theCurrentSpecialInterest = theCurrentSpecialInterest.Substring(0, 5)
            '    End If
            'End If
            '++ END JWM 05/02/2008 Sourceforge Tracker 1922332 ++++++++++
            '------------------------------------------
            ' Get the ORMapNum from the MapIndex 
            ' layer and parse it into the ORMapNum 
            ' object(used below for field values).
            '------------------------------------------
            Dim theORMapNum As String = GetValue(editFeature.ShapeCopy, mapIndexLayer.FeatureClass, OrmapExtension.MapIndexSettings.OrmapMapNumberField, OrmapExtension.MapIndexSettings.MapNumberField)
            If Not theORMapNumClass.ParseNumber(theORMapNum) Then
                ' Exit if parse failed
                Exit Sub
            End If

            '------------------------------------------
            ' Set all field values
            '------------------------------------------

            With editFeature

                Debug.Assert(.Fields.Field(theCountyFieldIndex).Length >= CShort(theORMapNumClass.County).ToString.Length, ".Fields.Field(theCountyFieldIndex).Length < CShort(theORMapNumClass.County).ToString.Length")
                Debug.Assert(.Fields.Field(theTownFieldIndex).Length >= CShort(theORMapNumClass.Township).ToString.Length, ".Fields.Field(theTownFieldIndex).Length >= CShort(theORMapNumClass.Township).ToString.Length")
                Debug.Assert(.Fields.Field(theTownPartFieldIndex).Length >= CSng(theORMapNumClass.PartialTownshipCode).ToString.Length, ".Fields.Field(theTownPartFieldIndex).Length >= CSng(theORMapNumClass.PartialTownshipCode).ToString.Length")
                Debug.Assert(.Fields.Field(theTownDirFieldIndex).Length >= theORMapNumClass.TownshipDirectional.ToString.Length, ".Fields.Field(theTownDirFieldIndex).Length >= theORMapNumClass.TownshipDirectional.ToString.Length")
                Debug.Assert(.Fields.Field(theRangeFieldIndex).Length >= CShort(theORMapNumClass.Range).ToString.Length, ".Fields.Field(theRangeFieldIndex).Length >= CShort(theORMapNumClass.Range).ToString.Length")
                Debug.Assert(.Fields.Field(theRangePartFieldIndex).Length >= CSng(theORMapNumClass.PartialRangeCode).ToString.Length, ".Fields.Field(theRangePartFieldIndex).Length >= CSng(theORMapNumClass.PartialRangeCode).ToString.Length")
                Debug.Assert(.Fields.Field(theRangeDirFieldIndex).Length >= theORMapNumClass.RangeDirectional.ToString.Length, ".Fields.Field(theRangeDirFieldIndex).Length >= theORMapNumClass.RangeDirectional.ToString.Length")
                Debug.Assert(.Fields.Field(theSectionFieldIndex).Length >= CShort(theORMapNumClass.Section).ToString.Length, ".Fields.Field(theSectionFieldIndex).Length >= CShort(theORMapNumClass.Section).ToString.Length")
                Debug.Assert(.Fields.Field(theQrtrFieldIndex).Length >= theORMapNumClass.Quarter.ToString.Length, ".Fields.Field(theQrtrFieldIndex).Length >= theORMapNumClass.Quarter.ToString.Length")
                Debug.Assert(.Fields.Field(theQrtrQrtrFieldIndex).Length >= theORMapNumClass.QuarterQuarter.ToString.Length, ".Fields.Field(theQrtrQrtrFieldIndex).Length >= theORMapNumClass.QuarterQuarter.ToString.Length")
                Debug.Assert(.Fields.Field(theMapSufTypeFieldIndex).Length >= theORMapNumClass.SuffixType.ToString.Length, ".Fields.Field(theMapSufTypeFieldIndex).Length >= theORMapNumClass.SuffixType.ToString.Length")
                Debug.Assert(.Fields.Field(theMapSufNumFieldIndex).Length >= CLng(theORMapNumClass.SuffixNumber).ToString.Length, ".Fields.Field(theMapSufNumFieldIndex).Length >= CLng(theORMapNumClass.SuffixNumber).ToString.Length")
                Debug.Assert(.Fields.Field(theAnomalyFieldIndex).Length >= theORMapNumClass.Anomaly.ToString.Length, ".Fields.Field(theAnomalyFieldIndex).Length >= theORMapNumClass.Anomaly.ToString.Length")
                Debug.Assert(.Fields.Field(theMapNumberFieldIndex).Length >= theCurrentMapNumber.ToString.Length, ".Fields.Field(theMapNumberFieldIndex).Length >= theCurrentMapNumber.ToString.Length")
                Debug.Assert(.Fields.Field(theORMapNumFieldIndex).Length >= theORMapNumClass.GetOrmapMapNumber.ToString.Length, ".Fields.Field(theORMapNumFieldIndex).Length >= theORMapNumClass.GetOrmapMapNumber.ToString.Length")
                Debug.Assert(.Fields.Field(theMapAcresFieldIndex).Length >= Left((theArea.Area / 43560).ToString, .Fields.Field(theMapAcresFieldIndex).Length).ToString.Length, ".Fields.Field(theMapAcresFieldIndex).Length >= Left((theArea.Area / 43560).ToString, .Fields.Field(theMapAcresFieldIndex).Length).ToString.Length")

                .Value(theCountyFieldIndex) = CShort(theORMapNumClass.County)
                .Value(theTownFieldIndex) = CShort(theORMapNumClass.Township)
                .Value(theTownPartFieldIndex) = CSng(theORMapNumClass.PartialTownshipCode)
                .Value(theTownDirFieldIndex) = theORMapNumClass.TownshipDirectional
                .Value(theRangeFieldIndex) = CShort(theORMapNumClass.Range)
                .Value(theRangePartFieldIndex) = CSng(theORMapNumClass.PartialRangeCode)
                .Value(theRangeDirFieldIndex) = theORMapNumClass.RangeDirectional
                .Value(theSectionFieldIndex) = CShort(theORMapNumClass.Section)
                .Value(theQrtrFieldIndex) = theORMapNumClass.Quarter
                .Value(theQrtrQrtrFieldIndex) = theORMapNumClass.QuarterQuarter
                .Value(theMapSufTypeFieldIndex) = theORMapNumClass.SuffixType
                .Value(theMapSufNumFieldIndex) = CLng(theORMapNumClass.SuffixNumber)
                .Value(theAnomalyFieldIndex) = theORMapNumClass.Anomaly
                .Value(theMapNumberFieldIndex) = theCurrentMapNumber
                .Value(theORMapNumFieldIndex) = theORMapNumClass.GetORMapNum
                'Taxlot (not updated here)
                'SFBUG START JWM 05/02/2008 Sourceforge Tracker 1922332 ++++++++++
                '.Value(theSpcIntrstFldIdx) = theCurrentSpecialInterest
                'END JWM 05/02/2008 Sourceforge Tracker 1922332 ++++++++++
                'MapTaxlot (see below)
                'ORTaxlot (see below)
                .Value(theMapAcresFieldIndex) = Left((theArea.Area / 43560).ToString, .Fields.Field(theMapAcresFieldIndex).Length)
            End With

            '------------------------------------------
            ' Recalculate ORTaxlot
            '------------------------------------------
            If IsDBNull(editFeature.Value(theTaxlotFieldIndex)) Then
                Exit Sub
            End If

            ' Taxlot has actual taxlot number. ORTaxlot requires a 5-digit number, so leading zeros have to be added.
            Dim theCurrentTaxlotValue As String = CStr(editFeature.Value(theTaxlotFieldIndex))
            theCurrentTaxlotValue = AddLeadingZeros(theCurrentTaxlotValue, ORMapNum.GetTaxlotFieldLength)

            Dim theNewORTaxlot As String
            theNewORTaxlot = String.Concat(theORMapNumClass.GetORMapNum, theCurrentTaxlotValue)

            Dim theCountyCode As Short = CShort(OrmapExtension.DefaultValuesSettings.County)
            Select Case theCountyCode
                Case 1 To 19, 21 To 36
                    editFeature.Value(theMapTaxlotFieldIndex) = GenerateMapTaxlotValue(theNewORTaxlot, OrmapExtension.TaxLotSettings.MapTaxlotFormatMask)
                Case 20
                    ' 1.  Lane County uses a 2-digit numeric identifier for ranges.
                    '     Special handling is required for east ranges, where 02E is
                    '     stored as 25, 03E as 35, etc.
                    ' 2.  ORMAP standards (OCDES (pg 13); Taxmap Data Model (pg 11)) assert that
                    '     this field should be equal to MAPNUMBER + TAXLOT. In this case, MAPNUMBER
                    '     is already in the right format, thus removing the need for the
                    '     CreateMapTaxlotValue function. Also, in this case, TAXLOT is padded
                    '     on the left with zeros to make it always a 5-digit number (see comment
                    '     above).
                    ' Trim the map number to only the left 8 characters (no spaces)
                    editFeature.Value(theMapTaxlotFieldIndex) = String.Concat(theCurrentMapNumber.Substring(0, 8), theCurrentTaxlotValue)
            End Select

            ' Recalculate ORTaxlot
            Dim theNewORTaxlotString As String = generateORMAPTaxlotNumber(theORMapNumClass.GetORMapNum, editFeature, theCurrentTaxlotValue)
            If Not IsDBNull(editFeature.Value(theORTaxlotFieldIndex)) Then
                ' Compare existing value to new value. If no changes, don't save value.
                Dim theExistingORTaxlotString As String = CStr(editFeature.Value(theORTaxlotFieldIndex))
                If String.Compare(theExistingORTaxlotString, theNewORTaxlotString, True, CultureInfo.CurrentCulture) <> 0 Then
                    editFeature.Value(theORTaxlotFieldIndex) = theNewORTaxlotString
                End If
            Else
                ' Save new value.
                editFeature.Value(theORTaxlotFieldIndex) = theNewORTaxlotString
            End If

        Finally
            theORMapNumClass = Nothing

        End Try
    End Sub

    ''' <summary>
    ''' Converts a domain descriptive value to the stored code.
    ''' </summary>
    ''' <param name="fields">An field collection object that supports the IFields interface.</param>
    ''' <param name="fieldName">A field that exists in fields.</param>
    ''' <param name="codedName">A descriptive name to convert to a code value</param>
    ''' <returns>A string that represents the code value that 
    ''' corresponds with the code name (<paramref name="codedName"/>), or 
    ''' a empty string.</returns>
    ''' <remarks></remarks>
    Public Shared Function ConvertCodeValueDomainToCode(ByVal fields As IFields, ByVal fieldName As String, ByVal codedName As String) As String

        Dim fieldIndex As Integer
        Dim returnValue As String = String.Empty

        If (fields Is Nothing) OrElse Not (TypeOf fields Is IFields) Then
            Return returnValue
        End If

        fieldIndex = fields.FindField(fieldName)
        If fieldIndex > NotFoundIndex Then
            Dim field As IField
            field = fields.Field(fieldIndex)
            Dim domain As IDomain = field.Domain

            If domain IsNot Nothing Then
                If domain.Type = esriDomainType.esriDTCodedValue Then
                    Dim thisCodedValueDomain As ICodedValueDomain
                    thisCodedValueDomain = DirectCast(domain, ICodedValueDomain)
                    For domainIndex As Integer = 0 To thisCodedValueDomain.CodeCount - 1
                        If String.Compare(thisCodedValueDomain.Name(domainIndex), codedName, True, CultureInfo.CurrentCulture) = 0 Then
                            returnValue = thisCodedValueDomain.Value(domainIndex).ToString
                        End If
                    Next domainIndex
                Else
                    returnValue = codedName 'if not coded value domain, then just return the input value
                End If
            End If 'if domain is not nothing
        End If

        Return returnValue

    End Function

    ''' <summary>
    ''' Converts a code index to the domain descriptive value.
    ''' </summary>
    ''' <param name="fields">An field collection object that supports the 
    ''' <c>IFields</c> interface.</param>
    ''' <param name="fieldName">A field that exists in fields.</param>
    ''' <param name="codedValue">A coded value to covert to a coded name.</param>
    ''' <returns>A string that represents the domain coded name that 
    ''' corresponds with the coded value name (<paramref name="codedValue"/>), or 
    ''' empty string.</returns>
    ''' <remarks></remarks>
    Public Shared Function ConvertCodeValueDomainToDescription(ByVal fields As IFields, ByVal fieldName As String, ByVal codedValue As String) As String

        Dim returnValue As String = String.Empty
        Dim fieldIndex As Integer
        fieldIndex = fields.FindField(fieldName)
        If fieldIndex > -1 Then
            Dim field As IField
            field = fields.Field(fieldIndex)

            Dim domain As IDomain = field.Domain
            If domain IsNot Nothing Then
                If domain.Type = esriDomainType.esriDTCodedValue Then
                    Dim codedValueDomain As ICodedValueDomain = DirectCast(domain, ICodedValueDomain)
                    For domainIndex As Integer = 0 To codedValueDomain.CodeCount - 1
                        'In the vb6 version the code was comparing an object to a variant to get the result. We don't have that luxury.
                        'codedValueDomain.Value is an object
                        If String.Compare(codedValueDomain.Value(domainIndex).ToString, codedValue, True, CultureInfo.CurrentCulture) = 0 Then
                            returnValue = codedValueDomain.Name(domainIndex)
                        End If
                    Next domainIndex
                Else
                    returnValue = codedValue 'if range domain return the value
                End If
            Else
                Return codedValue
                Exit Function
            End If 'if domain is valid object
        End If

        Return returnValue

    End Function

    ' TODO: [NIS] Add XML comments
    Public Shared Sub FlashFeature(ByVal theFeature As IFeature, ByVal theMxDoc As IMxDocument, ByVal inDurationMilliseconds As Integer)

        ' Start drawing on screen (suppress other draw events). 
        theMxDoc.ActiveView.ScreenDisplay.StartDrawing(0, CShort(esriScreenCache.esriNoScreenCache))

        ' Switch functions based on Geometry type. 
        Select Case theFeature.Shape.GeometryType
            Case esriGeometryType.esriGeometryPolyline
                flashLine(theMxDoc.ActiveView.ScreenDisplay, theFeature.Shape, inDurationMilliseconds)
            Case esriGeometryType.esriGeometryPolygon
                flashPolygon(theMxDoc.ActiveView.ScreenDisplay, theFeature.Shape, inDurationMilliseconds)
            Case esriGeometryType.esriGeometryPoint
                flashPoint(theMxDoc.ActiveView.ScreenDisplay, theFeature.Shape, inDurationMilliseconds)
        End Select

        ' Finish drawing on screen. 
        theMxDoc.ActiveView.ScreenDisplay.FinishDrawing()
    End Sub

    ' TODO: [NIS] Add XML comments
    Private Shared Sub flashLine(ByVal inDisplay As IScreenDisplay, ByVal inGeometry As IGeometry, ByVal inDurationMilliseconds As Integer)
        Dim theLineSymbol As ISimpleLineSymbol
        Dim theSymbol As ISymbol

        theLineSymbol = New SimpleLineSymbol
        theLineSymbol.Width = 4

        Dim theRGBColor As IRgbColor
        theRGBColor = New RgbColor
        theRGBColor.Green = 128

        theSymbol = DirectCast(theLineSymbol, ISymbol)
        theSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen

        inDisplay.SetSymbol(DirectCast(theLineSymbol, ISymbol))
        inDisplay.DrawPolyline(inGeometry)
        Sleep(inDurationMilliseconds)
        inDisplay.DrawPolyline(inGeometry)
    End Sub

    ' TODO: [NIS] Add XML comments
    Private Shared Sub flashPolygon(ByVal inDisplay As IScreenDisplay, ByVal inGeometry As IGeometry, ByVal inDurationMilliseconds As Integer)
        Dim theFillSymbol As ISimpleFillSymbol
        Dim theSymbol As ISymbol

        theFillSymbol = New SimpleFillSymbol
        theFillSymbol.Outline = Nothing

        Dim theRGBColor As IRgbColor
        theRGBColor = New RgbColor
        theRGBColor.Green = 128

        theSymbol = DirectCast(theFillSymbol, ISymbol)
        theSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen

        inDisplay.SetSymbol(DirectCast(theFillSymbol, ISymbol))
        inDisplay.DrawPolygon(inGeometry)
        Sleep(inDurationMilliseconds)
        inDisplay.DrawPolygon(inGeometry)
    End Sub

    ' TODO: [NIS] Add XML comments
    Private Shared Sub flashPoint(ByVal inDisplay As IScreenDisplay, ByVal inGeometry As IGeometry, ByVal inDurationMilliseconds As Integer)
        Dim theMarkerSymbol As ISimpleMarkerSymbol
        Dim theSymbol As ISymbol

        theMarkerSymbol = New SimpleMarkerSymbol
        theMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle

        Dim theRGBColor As IRgbColor
        theRGBColor = New RgbColor
        theRGBColor.Green = 128

        theSymbol = DirectCast(theMarkerSymbol, ISymbol)
        theSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen

        inDisplay.SetSymbol(DirectCast(theMarkerSymbol, ISymbol))
        inDisplay.DrawPoint(inGeometry)
        Sleep(inDurationMilliseconds)
        inDisplay.DrawPoint(inGeometry)
    End Sub


    ''' <summary>
    ''' Gets the layers (recursively) from the TOC as an IEnumLayer. NOTE: Important to check for valid feature layers.  This does not check.
    ''' </summary>
    ''' <param name="theEsriLayerType">An enumerator of layer types that be used to filter against the IEnumLayer.</param>
    ''' <returns>An IEnumLayer containing the layers in the TOC</returns>
    ''' <remarks>The function determines the UID for the layer type and applies it as a filter.</remarks>
    Friend Shared Function GetTOCLayersEnumerator(ByVal theEsriLayerType As EsriLayerTypes) As IEnumLayer

        Dim theMXDocument As IMxDocument = DirectCast(My.ArcMap.Application.Document, IMxDocument)
        Dim theMap As IMap = theMXDocument.FocusMap
        Dim theEnumLayer As IEnumLayer

        If theEsriLayerType = EsriLayerTypes.AllLayerTypes Then
            theEnumLayer = theMap.Layers
        Else
            Dim thisUID As New UID
            Select Case theEsriLayerType
                Case EsriLayerTypes.DataLayer
                    thisUID.Value = "{6CA416B1-E160-11D2-9F4E-00C04F6BC78E}"
                Case EsriLayerTypes.FeatureLayer
                    thisUID.Value = "{40A9E885-5533-11d0-98BE-00805F7CED21}"
                Case EsriLayerTypes.GeoFeatureLayer
                    thisUID.Value = "{E156D7E5-22AF-11D3-9F99-00C04F6BC78E}"
                Case EsriLayerTypes.GraphicsLayer
                    thisUID.Value = "{34B2EF81-F4AC-11D1-A245-080009B6F22B}"
                Case EsriLayerTypes.FDOGraphicsLayer
                    thisUID.Value = "{5CEAE408-4C0A-437F-9DB3-054D83919850}"
                Case EsriLayerTypes.CoverageAnnotationLayer
                    thisUID.Value = "{0C22A4C7-DAFD-11D2-9F46-00C04F6BC78E}"
                Case EsriLayerTypes.GroupLayer
                    thisUID.Value = "{EDAD6644-1810-11D1-86AE-0000F8751720}"

            End Select
            theEnumLayer = theMap.Layers(thisUID, True)

        End If

        Return theEnumLayer

    End Function


    ''' <summary>
    ''' Locate a feature layer by its dataset name.
    ''' </summary>
    ''' <param name="datasetName">The name of the dataset to find.</param>
    ''' <returns>A layer object of that supports the IFeatureLayer interface.</returns>
    ''' <remarks>Searches in the TOC for FeatureLayers and  
    ''' returns the first with a matching dataset name.</remarks>
    Public Shared Function FindFeatureLayerByDSName(ByVal datasetName As String) As ESRI.ArcGIS.Carto.IFeatureLayer

        Dim returnValue As IFeatureLayer = Nothing

        'We want a EnumLayer containing all FeatureLayer objects.
        Dim theFeatureLayers As IEnumLayer = GetTOCLayersEnumerator(EsriLayerTypes.FeatureLayer)
        theFeatureLayers.Reset()

        Dim thisFeatureLayer As IFeatureLayer = DirectCast(theFeatureLayers.Next, IFeatureLayer)

        Dim thisDataSet As IDataset

        Do While thisFeatureLayer IsNot Nothing
            If thisFeatureLayer.Valid Then
                thisDataSet = TryCast(thisFeatureLayer.FeatureClass, IDataset)
                If thisDataSet IsNot Nothing Then
                    If String.Compare(thisDataSet.Name, datasetName, True, CultureInfo.CurrentCulture) = 0 Then
                        returnValue = DirectCast(thisFeatureLayer, IFeatureLayer)
                        Exit Do
                    End If
                End If
            End If
            thisFeatureLayer = DirectCast(theFeatureLayers.Next(), IFeatureLayer)
        Loop

        Return returnValue

    End Function

    ''' <summary>
    ''' Locate a standalone table by its dataset name.
    ''' </summary>
    ''' <param name="datasetName">The name of the table to find.</param>
    ''' <returns>A table object of that supports the ITable interface.</returns>
    ''' <remarks>Returns the first table with a matching dataset name.</remarks>
    Public Shared Function FindStandaloneTableByDSName(ByVal datasetName As String) As IStandaloneTable

        Dim theMXDocument As IMxDocument = DirectCast(My.ArcMap.Application.Document, IMxDocument)
        Dim theMap As IMap = theMXDocument.FocusMap

        Try
            Dim returnValue As IStandaloneTable = Nothing

            ' Get map as table collection
            Dim theStandaloneTableCollection As IStandaloneTableCollection
            theStandaloneTableCollection = DirectCast(theMap, IStandaloneTableCollection)

            Dim theStandaloneTable As IStandaloneTable
            For t As Integer = 0 To (theStandaloneTableCollection.StandaloneTableCount - 1)
                theStandaloneTable = theStandaloneTableCollection.StandaloneTable(t)
                If String.Compare(theStandaloneTable.Name, datasetName, True, CultureInfo.CurrentCulture) = 0 Then
                    returnValue = theStandaloneTable
                    Exit For
                End If
            Next

            Return returnValue

        Finally
            theMap = Nothing

        End Try

    End Function

    ''' <summary>
    ''' Determine the x and y coordinates of the center of envelope, and return them as a Point object.
    ''' </summary>
    ''' <param name="envelope">An envelope object of type IEnvelope.</param>
    ''' <returns>A Point object that represents the center of the envelope.</returns>
    ''' <remarks></remarks>
    Public Shared Function GetCenterOfEnvelope(ByVal envelope As IEnvelope) As IPoint
        Dim center As IPoint
        center = New Point
        center.X = envelope.XMin + (envelope.XMax - envelope.XMin) / 2
        center.Y = envelope.YMin + (envelope.YMax - envelope.YMin) / 2
        Return center
    End Function

    ''' <summary>
    ''' Get the MapIndex feature layer.
    ''' </summary>
    ''' <returns>The MapIndex feature layer.</returns>
    ''' <remarks>This feature layer may be named something other than "MapIndex", 
    ''' depending on user settings.</remarks>
    <ObsoleteAttribute("Use DataMonitor properties instead.", True)> _
    Public Shared Function GetMapIndexFeatureLayer() As IFeatureLayer
        ' Find Map Index feature layer
        Dim theMapIndexFLayer As IFeatureLayer
        With OrmapExtension.TableNamesSettings
            ' Find MapIndex feature layer
            theMapIndexFLayer = FindFeatureLayerByDSName(.MapIndexFC)
            If theMapIndexFLayer Is Nothing Then
                Return Nothing
            End If
        End With
        Return theMapIndexFLayer
    End Function

    ''' <summary>
    '''  Validate and format a map suffix number.
    ''' </summary>
    ''' <param name="feature">An object that supports the IFeature interface.</param>
    ''' <returns>A string the represents a properly formatted Map Suffix.</returns>
    ''' <remarks></remarks>
    Public Shared Function GetMapSuffixNumber(ByVal feature As IFeature) As String

        Dim returnValue As New String("0"c, 3)
        Dim theTaxlotMapSuffixFieldIndex As Integer = LocateFields(DirectCast(feature.Class, IFeatureClass), OrmapExtension.TaxLotSettings.MapSuffixNumberField)
        If theTaxlotMapSuffixFieldIndex > -1 Then
            If Not IsDBNull(feature.Value(theTaxlotMapSuffixFieldIndex)) Then
                returnValue = CStr(feature.Value(theTaxlotMapSuffixFieldIndex))
            End If
            'verify that it is exactly 3 digits
            If returnValue.Length < 3 Then
                returnValue = returnValue.PadLeft(3, "0"c)
            End If
            If returnValue.Length > 3 Then
                returnValue = returnValue.Substring(0, 3)
            End If
        End If

        Return returnValue

    End Function

    ''' <summary>
    ''' Validate and format a map suffix type.
    ''' </summary>
    ''' <param name="theFeature">An object that supports the IFeature interface.</param>
    ''' <returns>A string that represents a properly formatted Map Suffix Type.</returns>
    ''' <remarks>A proper map suffix type is one character.</remarks>
    Public Shared Function GetMapSuffixType(ByVal theFeature As IFeature) As String

        Dim returnValue As New String("0"c, 1)
        Dim theTaxlotMapTypeFieldIndex As Integer
        theTaxlotMapTypeFieldIndex = LocateFields(DirectCast(theFeature.Class, IFeatureClass), OrmapExtension.TaxLotSettings.MapSuffixTypeField)
        If theTaxlotMapTypeFieldIndex > -1 Then
            If Not IsDBNull(theFeature.Value(theTaxlotMapTypeFieldIndex)) Then
                returnValue = CStr(theFeature.Value(theTaxlotMapTypeFieldIndex))
                'verify that it is one digit
                If returnValue.Length > 1 Then
                    returnValue = returnValue.PadLeft(1, "0"c)
                End If
                'verify that it is not more than 1 digit
                If returnValue.Length > 1 Then
                    returnValue = returnValue.Substring(0, 1)
                End If
            End If
            'verify that it is exactly one digit
            If returnValue.Length < 1 Then
                returnValue = "0"
            End If
            If returnValue.Length > 1 Then
                returnValue = returnValue.Substring(0, 1)
            End If
        End If

        Return returnValue

    End Function

    ''' <summary>
    ''' Retrieve a feature-linked annotation feature.
    ''' </summary>
    ''' <param name="anObject">An initialized geodatabase object.</param>
    ''' <returns>An object that supports the <c>IFeature</c> interface.</returns>
    ''' <remarks><para>Finds all related objects to the feature through the 
    ''' first found relationship class, and returns the first related object as 
    ''' the return value.</para>
    ''' <para>This is optimized for annotation because there is a single 
    ''' relationship class.</para></remarks>
    Public Shared Function GetRelatedObjects(ByVal anObject As IObject) As IFeature
        Dim relationshipClassEnum As IEnumRelationshipClass

        relationshipClassEnum = anObject.Class.RelationshipClasses(esriRelRole.esriRelRoleAny)
        If relationshipClassEnum IsNot Nothing Then
            Dim thisRelationshipClass As IRelationshipClass
            thisRelationshipClass = relationshipClassEnum.Next
            If thisRelationshipClass IsNot Nothing Then
                Dim parentSet As ISet
                parentSet = thisRelationshipClass.GetObjectsRelatedToObject(anObject)
                If parentSet IsNot Nothing Then
                    Dim parentFeature As IFeature
                    parentFeature = DirectCast(parentSet.Next, IFeature)
                    If parentFeature IsNot Nothing Then
                        Return parentFeature
                    End If
                End If
            End If
        End If

        Return Nothing

    End Function

    ''' <summary>
    ''' Return a feature cursor of the selected features of the specified layer.
    ''' </summary>
    ''' <param name="layer">The feature layer to return the selection from.</param>
    ''' <param name="isEditable">Whether the returned cursor will be used for editing.</param>
    ''' <returns>An object that supports the <c>IFeatureCursor</c> interface.</returns>
    ''' <remarks>References the currently selected features in layer, and 
    ''' returns a feature cursor with the selected features in it.</remarks>
    Public Shared Function GetSelectedFeatures(ByRef layer As IFeatureLayer, Optional ByVal isEditable As Boolean = False) As IFeatureCursor
        Dim returnValue As IFeatureCursor = Nothing

        Dim theFeatureSelection As IFeatureSelection = DirectCast(layer, IFeatureSelection)
        Dim theSelectionSet As ISelectionSet
        theSelectionSet = theFeatureSelection.SelectionSet

        If theSelectionSet.Count > 0 Then
            Dim thisCursor As ICursor = Nothing
            If isEditable Then
                theSelectionSet.Search(Nothing, False, thisCursor) 'non-recycling cursor
            Else
                theSelectionSet.Search(Nothing, True, thisCursor) 'recycling cursor
            End If
            If theSelectionSet.Count > 0 Then
                returnValue = DirectCast(thisCursor, IFeatureCursor)
            Else
                returnValue = Nothing
            End If
        End If

        Return returnValue

    End Function

    ''' <summary>
    ''' Get the Taxlot feature layer.
    ''' </summary>
    ''' <returns>The Taxlot feature layer.</returns>
    ''' <remarks>This feature layer may be named something other than "Taxlot", 
    ''' depending on user settings.</remarks>
    <ObsoleteAttribute("Use DataMonitor propoerties instead.", True)> _
    Public Shared Function GetTaxlotFeatureLayer() As IFeatureLayer
        ' Find Taxlot feature layer
        Dim theTaxlotFLayer As IFeatureLayer
        With OrmapExtension.TableNamesSettings
            ' Find Taxlot feature layer
            theTaxlotFLayer = FindFeatureLayerByDSName(.TaxLotFC)
            If theTaxlotFLayer Is Nothing Then
                Return Nothing
            End If
        End With
        Return theTaxlotFLayer
    End Function

    ''' <summary>
    ''' Calls the overloaded function GetValue and adds an empty string for the 
    ''' orderBestByFieldName parameter.
    ''' </summary>
    ''' <param name="searchGeometry">The search geometry.</param>
    ''' <param name="overlayFeatureClass">Overlaying feature class.</param>
    ''' <param name="valueFieldName">Name of field to return value for.</param>
    ''' <returns>Returns the value from the specified field (<paramref>valueFieldName</paramref>) 
    ''' as a string.</returns>
    ''' <remarks>See overloaded function GetValue</remarks>
    Public Overloads Shared Function GetValue(ByVal searchGeometry As IGeometry, ByVal overlayFeatureClass As IFeatureClass, ByVal valueFieldName As String) As String
        Return GetValue(searchGeometry, overlayFeatureClass, valueFieldName, String.Empty)
    End Function


    ''' <summary>
    ''' Determine wether return the value from the GetValueViaOverlay function or use
    ''' a value from the attribute override.
    ''' </summary>
    ''' <param name="searchGeometry">The search geometry.</param>
    ''' <param name="overlayFeatureClass">Overlaying feature class.</param>
    ''' <param name="valueFieldName">Name of field to return value for.</param>
    ''' <param name="orderBestByFieldName">Name of field to order by in the 
    ''' case of a tie in area/length of intersection.</param>
    ''' <returns>Returns the value from the specified field (<paramref>valueFieldName</paramref>) 
    ''' as a string.</returns>
    ''' <remarks></remarks>
    Public Overloads Shared Function GetValue(ByVal searchGeometry As IGeometry, ByVal overlayFeatureClass As IFeatureClass, ByVal valueFieldName As String, ByVal orderBestByFieldName As String) As String

        Dim theValue As String = String.Empty
        If EditorExtension.OverrideAutoAttribute Then
            Select Case valueFieldName
                Case OrmapExtension.MapIndexSettings.MapNumberField
                    theValue = EditorExtension.OverrideMapNumber

                Case OrmapExtension.MapIndexSettings.MapScaleField
                    theValue = EditorExtension.OverrideMapScale

                Case OrmapExtension.MapIndexSettings.OrmapMapNumberField
                    theValue = EditorExtension.OverrideORMapNumber

                Case Else
                    If MessageBox.Show("The manual attribute overide option only works for retrieving the MapNumber, MapScale, and ORMapNumber field values.  Would you like to use the 'Auto' method this time?  Choose 'Yes' to use the 'Auto' method or 'No' to not get the value", "Manual Override Not Available.", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = DialogResult.Yes Then
                        theValue = GetValueViaOverlay(searchGeometry, overlayFeatureClass, valueFieldName, orderBestByFieldName)
                    End If

            End Select
        Else
            Return GetValueViaOverlay(searchGeometry, overlayFeatureClass, valueFieldName, orderBestByFieldName)
        End If

        Return theValue

    End Function


    ''' <summary>
    ''' Overlay the passed in feature with a feature class to get a value from 
    ''' the specified field.
    ''' </summary>
    ''' <param name="searchGeometry">The search geometry.</param>
    ''' <param name="overlayFeatureClass">Overlaying feature class.</param>
    ''' <param name="valueFieldName">Name of field to return value for.</param>
    ''' <returns>Returns the value from the specified field (<paramref>valueFieldName</paramref>) 
    ''' as a string.</returns>
    ''' <remarks>Gets the target feature with the largest area of intersection 
    ''' with the geometry and gets its value from the field, or, if tied (unikely 
    ''' but possible), then gets the best (lowest) value from the field, based 
    ''' on the order by field value.</remarks>
    Public Overloads Shared Function GetValueViaOverlay(ByVal searchGeometry As IGeometry, ByVal overlayFeatureClass As IFeatureClass, ByVal valueFieldName As String) As String
        Return GetValueViaOverlay(searchGeometry, overlayFeatureClass, valueFieldName, String.Empty)
    End Function

    ''' <summary>
    ''' Overlay the passed in feature with a feature class to get a value from 
    ''' the specified field.
    ''' </summary>
    ''' <param name="searchGeometry">The search geometry.</param>
    ''' <param name="overlayFeatureClass">Overlaying feature class.</param>
    ''' <param name="valueFieldName">Name of field to return value for.</param>
    ''' <param name="orderBestByFieldName">Name of field to order by in the 
    ''' case of a tie in area/length of intersection.</param>
    ''' <returns>Returns the value from the specified field (<paramref>valueFieldName</paramref>) 
    ''' as a string.</returns>
    ''' <remarks>Gets the target feature with the largest area of intersection 
    ''' with the geometry and gets its value from the field, or, if tied (unikely 
    ''' but possible), then gets the best (lowest) value from the field, based 
    ''' on the order by field value.</remarks>
    Public Overloads Shared Function GetValueViaOverlay(ByVal searchGeometry As IGeometry, ByVal overlayFeatureClass As IFeatureClass, ByVal valueFieldName As String, ByVal orderBestByFieldName As String) As String

        ' ENHANCE: [NIS] Refactor (smaller modules).

        Dim canContinueThisProcess As Boolean = True

        Debug.Assert(Not searchGeometry Is Nothing, "GetValueViaOverlay", "searchGeometry Is Nothing")
        Debug.Assert(Not overlayFeatureClass Is Nothing, "GetValueViaOverlay", "overlayFeatureClass Is Nothing")
        Debug.Assert(Not valueFieldName = String.Empty, "GetValueViaOverlay", "valueFieldName is empty")
        Debug.Assert(Not orderBestByFieldName = String.Empty, "GetValueViaOverlay", "orderBestByFieldName is empty")

        If (searchGeometry Is Nothing) OrElse (overlayFeatureClass Is Nothing) OrElse (valueFieldName.Length <= 0) Then
            canContinueThisProcess = False
        End If

        Dim valueFieldIndex As Integer = NotFoundIndex
        If canContinueThisProcess Then
            valueFieldIndex = overlayFeatureClass.Fields.FindField(valueFieldName)
            If valueFieldIndex = NotFoundIndex Then
                canContinueThisProcess = False
            End If
        End If

        Dim orderBestByFieldIndex As Integer = NotFoundIndex
        If canContinueThisProcess Then
            If orderBestByFieldName.Length = 0 Then
                ' Use the value field as the order-by field
                orderBestByFieldIndex = valueFieldIndex
            Else
                orderBestByFieldIndex = overlayFeatureClass.Fields.FindField(orderBestByFieldName)
                If orderBestByFieldIndex = NotFoundIndex Then
                    ' Field not found. Try the OID field
                    If overlayFeatureClass.HasOID Then
                        orderBestByFieldIndex = overlayFeatureClass.Fields.FindField(overlayFeatureClass.OIDFieldName)
                        Dim msg As String = "Field " & orderBestByFieldName & " not found in " & overlayFeatureClass.AliasName & NewLine & _
                                ". Using " & overlayFeatureClass.OIDFieldName
                        Debug.WriteLine(msg)
                        Trace.WriteLine(msg)
                    Else
                        canContinueThisProcess = False
                    End If
                End If
            End If
        End If

        Dim thisPolygon As IPolygon
        Dim thisArea As IArea
        Dim thisCurve As ICurve
        Dim thisEnvelope As IEnvelope
        Dim intersectFuzzAmount As Double
        Const fuzzFactor As Double = 0.05 ' ENHANCE: [NIS] Re-implement as user setting?

        If canContinueThisProcess Then
            Select Case searchGeometry.GeometryType
                Case esriGeometryType.esriGeometryPolygon
                    thisPolygon = DirectCast(searchGeometry, IPolygon)
                    thisArea = DirectCast(thisPolygon, IArea)
                    intersectFuzzAmount = thisArea.Area * fuzzFactor

                Case esriGeometryType.esriGeometryPolyline, esriGeometryType.esriGeometryLine, esriGeometryType.esriGeometryBezier3Curve, esriGeometryType.esriGeometryCircularArc, esriGeometryType.esriGeometryEllipticArc, esriGeometryType.esriGeometryPath
                    thisCurve = DirectCast(searchGeometry, ICurve)
                    intersectFuzzAmount = thisCurve.Length * fuzzFactor

                Case esriGeometryType.esriGeometryPoint
                    ' no variables settings needed

                Case esriGeometryType.esriGeometryEnvelope
                    thisEnvelope = DirectCast(searchGeometry, IEnvelope)
                    thisPolygon = envelopeToPolygon(thisEnvelope)
                    thisArea = DirectCast(thisPolygon, IArea)
                    intersectFuzzAmount = thisArea.Area * fuzzFactor

                Case Else
                    canContinueThisProcess = False

            End Select
        End If

        Dim theOverlayFeatureCursor As IFeatureCursor
        Dim anOverlayFeature As IFeature
        Dim dictCandidates As New Dictionary(Of Integer, Double) '(key::value) OID::area/length

        If canContinueThisProcess Then

            Dim largestIntersectArea As Double = 0
            Dim longestIntersectLength As Double = 0

            theOverlayFeatureCursor = DoSpatialQuery(overlayFeatureClass, searchGeometry, esriSpatialRelEnum.esriSpatialRelIntersects)
            If theOverlayFeatureCursor IsNot Nothing Then
                anOverlayFeature = theOverlayFeatureCursor.NextFeature
                Dim topoOperator As ITopologicalOperator
                Dim intersectGeometry As IGeometry
                Dim intersectArea As Double = 0
                dictCandidates = New Dictionary(Of Integer, Double)
                While Not (anOverlayFeature Is Nothing)

                    topoOperator = DirectCast(anOverlayFeature.Shape, ITopologicalOperator)
                    If Not topoOperator.IsSimple Then
                        topoOperator.Simplify()
                    End If

                    Select Case searchGeometry.GeometryType

                        Case esriGeometryType.esriGeometryEnvelope, esriGeometryType.esriGeometryPolygon
                            ' Determine if the target feature has the largest area of intersection with the
                            ' current source feature. Set flags used below.
                            intersectGeometry = topoOperator.Intersect(searchGeometry, esriGeometryDimension.esriGeometry2Dimension)
                            If Not intersectGeometry.IsEmpty Then
                                thisPolygon = DirectCast(intersectGeometry, IPolygon)
                                thisArea = DirectCast(thisPolygon, IArea)
                                intersectArea = thisArea.Area
                                If System.Math.Abs(intersectArea - largestIntersectArea) > intersectFuzzAmount Then
                                    '[Difference greater than fuzz amount, so not a "fuzzy tie"...]
                                    ' Determine if this is the feature with the largest area of intersection.
                                    If intersectArea > largestIntersectArea Then
                                        largestIntersectArea = intersectArea
                                        '[New largest intersection...]
                                        ' Get the value only for this feature.
                                        dictCandidates.Clear()
                                        dictCandidates.Add(anOverlayFeature.OID, largestIntersectArea)
                                    Else
                                        '[Smaller intersection...]
                                        ' Don't get the value for this feature.
                                    End If
                                Else
                                    '[Difference not greater than fuzz amount, so a "fuzzy tie"...]
                                    ' Evaluate this feature against other tied candidates (don't clear dictionary).
                                    dictCandidates.Add(anOverlayFeature.OID, largestIntersectArea)
                                End If
                            Else
                                '[Empty intersection geometry...]
                                ' Don't get the value for this feature
                            End If

                        Case esriGeometryType.esriGeometryLine, esriGeometryType.esriGeometryPolyline
                            ' Determine if the target feature has the longest length of intersection with the
                            ' current source feature. Set flags used below.
                            intersectGeometry = topoOperator.Intersect(searchGeometry, esriGeometryDimension.esriGeometry1Dimension)
                            If Not intersectGeometry.IsEmpty Then
                                thisCurve = DirectCast(intersectGeometry, ICurve)
                                Dim intersectLength As Double
                                intersectLength = thisCurve.Length
                                If System.Math.Abs(intersectLength - longestIntersectLength) > intersectFuzzAmount Then
                                    '[Difference greater than fuzz amount, so not a "fuzzy tie"...]
                                    ' Determine if this is the feature with the longest length of intersection
                                    If intersectLength > longestIntersectLength Then
                                        longestIntersectLength = intersectLength
                                        dictCandidates.Clear()
                                        dictCandidates.Add(anOverlayFeature.OID, longestIntersectLength)
                                    Else
                                        '[Smaller intersection...]
                                        ' Don't get the value for this feature
                                    End If
                                Else
                                    '[Difference not greater than fuzz amount, so a "fuzzy tie"...]
                                    'Evaluate this feature against other tied candidates (don't clear dictionary).
                                    dictCandidates.Add(anOverlayFeature.OID, longestIntersectLength)
                                End If
                            Else
                                '[Empty intersection geometry...]
                                ' Don't get the value for this feature
                            End If

                        Case esriGeometryType.esriGeometryPoint
                            '[Tied by definition (0-dimension = zero area & length = tie)...]
                            ' Evaluate this feature against other tied candidates (don't clear dictionary).
                            dictCandidates.Add(anOverlayFeature.OID, 0)

                        Case Else
                            canContinueThisProcess = False

                    End Select
                    anOverlayFeature = theOverlayFeatureCursor.NextFeature
                End While
            End If

        End If

        Dim theBestValue As String = String.Empty

        If canContinueThisProcess Then

            If dictCandidates.Count > 0 Then
                Dim aValue As String = String.Empty
                Dim theBestOrderByValue As String = String.Empty

                Dim whereClause As String = String.Concat(overlayFeatureClass.OIDFieldName, " in (", candidateKeysToDelimitedString(dictCandidates), ")")
                theOverlayFeatureCursor = DoSpatialQuery(overlayFeatureClass, searchGeometry, esriSpatialRelEnum.esriSpatialRelIntersects, whereClause)
                If Not (theOverlayFeatureCursor Is Nothing) Then
                    anOverlayFeature = theOverlayFeatureCursor.NextFeature
                    While Not (anOverlayFeature Is Nothing)
                        If dictCandidates.Count > 1 Then
                            '[Candidates tied for area/length of intersection (unikely but possible)...]
                            ' Get the value only if the candidate source feature has the best (lowest) order-by value
                            Dim anOrderByValue As String = CStr(anOverlayFeature.Value(orderBestByFieldIndex))
                            ' Note: When you compare strings, the string expressions are evaluated based on their 
                            ' alphabetical sort order (e.g. "A" < "B" and "A" < "Ax"), which depends on the Option 
                            ' Compare setting.
                            If theBestOrderByValue.Length = 0 OrElse anOrderByValue < theBestOrderByValue Then
                                '[Any new value beats an empty value...]
                                '[lower in the sort order is assumed to be better...]
                                theBestOrderByValue = anOrderByValue
                                If Not IsDBNull(anOverlayFeature.Value(valueFieldIndex)) Then
                                    aValue = CStr(anOverlayFeature.Value(valueFieldIndex))
                                    theBestValue = aValue
                                Else
                                    MessageBox.Show("The field " & anOverlayFeature.Fields.Field(valueFieldIndex).Name & " contains a Null" & NewLine & "for the feature with OID=" & anOverlayFeature.OID & ".", "Error: GetValueViaOverlay", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                End If
                            End If
                        Else
                            '[Candidates not tied (only one)...]
                            ' Get the value for the only candidate feature
                            If Not IsDBNull(anOverlayFeature.Value(valueFieldIndex)) Then
                                aValue = CStr(anOverlayFeature.Value(valueFieldIndex))
                                theBestValue = aValue
                            Else
                                MessageBox.Show("The field " & anOverlayFeature.Fields.Field(valueFieldIndex).Name & " contains a Null" & NewLine & "for the feature with OID=" & anOverlayFeature.OID & ".", "Error: GetValueViaOverlay", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            End If
                            Exit While
                        End If 'dictCandidates.count > 1
                        anOverlayFeature = theOverlayFeatureCursor.NextFeature
                    End While
                End If 'featurecursor is not nothing
            Else
                MessageBox.Show("No " & overlayFeatureClass.AliasName & "features found " & NewLine & " which intersect this feature.", "Error: GetValueViaOverlay", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If 'dictionary count > zero
        End If 'continueThisProcess 

        Return theBestValue

    End Function




    ''' <summary>
    ''' Returns an array of unique mapnumbers.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>[SC] Calculating a AutoCompleteCustomSource using the List of strings is considerably faster than</remarks>
    Public Shared Function GetMapNumberArray() As System.Array

        Dim mapNumberStringList As List(Of String) = Nothing

        If MapIndexFeatureLayer Is Nothing Then CheckValidMapIndexDataProperties()

        mapNumberStringList = New List(Of String)
        Dim theMapIndexFClass As IFeatureClass = MapIndexFeatureLayer.FeatureClass
        Dim theQueryFilter As IQueryFilter = New QueryFilter
        theQueryFilter.SubFields = "MapNumber"
        Dim theFeatCursor As IFeatureCursor = theMapIndexFClass.Search(theQueryFilter, True)
        Dim thisFeature As IFeature = theFeatCursor.NextFeature
        Dim theFieldIdx As Integer = thisFeature.Fields.FindField("MapNumber")
        Do Until thisFeature Is Nothing
            Dim thisMapNumberVal As String = thisFeature.Value(theFieldIdx).ToString
            If Not mapNumberStringList.Contains(thisMapNumberVal) Then
                mapNumberStringList.Add(thisMapNumberVal)
            End If
            thisFeature = theFeatCursor.NextFeature
        Loop

        Return mapNumberStringList.ToArray

    End Function




    ''' <summary>
    ''' Determines if the feature layer has a selection of sufficient size.
    ''' </summary>
    ''' <param name="inLayer">An object that supports the IFeatureLayer.</param>
    ''' <param name="inMinimumCount">An integer representing the minimum number 
    ''' of features that must be selected to return True.</param>
    ''' <returns><c>True</c> or <c>False</c>.</returns>
    ''' <remarks>Checking the selection set of layer, determine if one, many, 
    ''' or no features are selected.</remarks>
    Public Shared Function HasSelectedFeatureCount(ByVal inLayer As IFeatureLayer, ByVal inMinimumCount As Integer) As Boolean
        Dim returnValue As Boolean = False
        If (inLayer Is Nothing) OrElse Not (TypeOf inLayer Is IFeatureLayer) Then
            Return returnValue
        End If

        ' How many are selected?
        Dim featuresSelected As IFeatureSelection
        featuresSelected = DirectCast(inLayer, IFeatureSelection)
        returnValue = (featuresSelected.SelectionSet.Count >= inMinimumCount)

        Return returnValue

    End Function

    ''' <summary>
    ''' Determine if a feature is annotation.
    ''' </summary>
    ''' <param name="thisObject">A valid initialized geodatabase object.</param>
    ''' <returns><c>True</c> or <c>False</c>.</returns>
    ''' <remarks>Compares the feature type of <paramref name="thisObject"/> with 
    ''' that of annotation and return the truth value of the comparison.</remarks>
    Public Shared Function IsAnno(ByVal thisObject As IObject) As Boolean
        Dim thisObjectClass As IObjectClass
        thisObjectClass = thisObject.Class

        If TypeOf thisObject Is IFeature Then
            Dim thisFeatureClass As IFeatureClass
            thisFeatureClass = DirectCast(thisObjectClass, IFeatureClass)
            If thisFeatureClass.FeatureType = esriFeatureType.esriFTAnnotation Then
                Return True
            Else
                Return False
            End If
        End If
    End Function

    ''' <summary>
    ''' Determine if the feature belongs to the MapIndex feature class.
    ''' </summary>
    ''' <param name="thisObject">A valid initialized geodatabase object.</param>
    ''' <returns><c>True</c> or <c>False</c>.</returns>
    ''' <remarks>Compares the name of the dataset of the <paramref name="thisObject"/> 
    ''' feature class to the Map Index layer name in order to determine if it 
    ''' is the MapIndex feature class.</remarks>
    Public Shared Function IsMapIndex(ByVal thisObject As IObject) As Boolean
        Dim thisObjectClass As IObjectClass
        Dim thisDataset As IDataset
        thisObjectClass = thisObject.Class
        thisDataset = DirectCast(thisObjectClass, IDataset)
        If String.Compare(thisDataset.Name, OrmapExtension.TableNamesSettings.MapIndexFC, True, CultureInfo.CurrentCulture) = 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Determines if a feature class is part of the ORMAP design.
    ''' </summary>
    ''' <param name="theObject">A valid initialized geodatabase object</param>
    ''' <returns>True or False</returns>
    ''' <remarks>This function is not being used... can be deleted after review from other developers. - SC</remarks>
    Public Shared Function IsOrmapFeature(ByVal theObject As IObject) As Boolean
        ' ENHANCE: JWM Refactor to return enum of ORMAP feature class type
        Dim returnValue As Boolean = False
        Dim thisObjectClass As IObjectClass
        thisObjectClass = theObject.Class
        Dim thisDataset As IDataset
        thisDataset = DirectCast(thisObjectClass, IDataset)
        Dim datasetName As String = thisDataset.Name
        Const StringMatch As Integer = 0
        ' Check for a match against any of the ORMAP feature classes.
        returnValue = (String.Compare(datasetName, OrmapExtension.AnnoTableNamesSettings.Anno0010scaleFC, True, CultureInfo.CurrentCulture) = StringMatch)
        returnValue = returnValue OrElse (String.Compare(datasetName, OrmapExtension.AnnoTableNamesSettings.Anno0020scaleFC, True, CultureInfo.CurrentCulture) = StringMatch)
        returnValue = returnValue OrElse (String.Compare(datasetName, OrmapExtension.AnnoTableNamesSettings.Anno0030scaleFC, True, CultureInfo.CurrentCulture) = StringMatch)
        returnValue = returnValue OrElse (String.Compare(datasetName, OrmapExtension.AnnoTableNamesSettings.Anno0040scaleFC, True, CultureInfo.CurrentCulture) = StringMatch)
        returnValue = returnValue OrElse (String.Compare(datasetName, OrmapExtension.AnnoTableNamesSettings.Anno0050scaleFC, True, CultureInfo.CurrentCulture) = StringMatch)
        returnValue = returnValue OrElse (String.Compare(datasetName, OrmapExtension.AnnoTableNamesSettings.Anno0100scaleFC, True, CultureInfo.CurrentCulture) = StringMatch)
        returnValue = returnValue OrElse (String.Compare(datasetName, OrmapExtension.AnnoTableNamesSettings.Anno0200scaleFC, True, CultureInfo.CurrentCulture) = StringMatch)
        returnValue = returnValue OrElse (String.Compare(datasetName, OrmapExtension.AnnoTableNamesSettings.Anno0400scaleFC, True, CultureInfo.CurrentCulture) = StringMatch)
        returnValue = returnValue OrElse (String.Compare(datasetName, OrmapExtension.AnnoTableNamesSettings.Anno0800scaleFC, True, CultureInfo.CurrentCulture) = StringMatch)
        returnValue = returnValue OrElse (String.Compare(datasetName, OrmapExtension.AnnoTableNamesSettings.Anno2000scaleFC, True, CultureInfo.CurrentCulture) = StringMatch)
        returnValue = returnValue OrElse (String.Compare(datasetName, OrmapExtension.AnnoTableNamesSettings.TaxCodeAnnoFC, True, CultureInfo.CurrentCulture) = StringMatch)
        returnValue = returnValue OrElse (String.Compare(datasetName, OrmapExtension.AnnoTableNamesSettings.TaxlotAcreageAnnoFC, True, CultureInfo.CurrentCulture) = StringMatch)
        returnValue = returnValue OrElse (String.Compare(datasetName, OrmapExtension.AnnoTableNamesSettings.TaxlotNumberAnnoFC, True, CultureInfo.CurrentCulture) = StringMatch)
        returnValue = returnValue OrElse (String.Compare(datasetName, OrmapExtension.TableNamesSettings.CartographicLinesFC, True, CultureInfo.CurrentCulture) = StringMatch)
        returnValue = returnValue OrElse (String.Compare(datasetName, OrmapExtension.TableNamesSettings.TaxLotFC, True, CultureInfo.CurrentCulture) = StringMatch)
        returnValue = returnValue OrElse (String.Compare(datasetName, OrmapExtension.TableNamesSettings.MapIndexFC, True, CultureInfo.CurrentCulture) = StringMatch)
        returnValue = returnValue OrElse (String.Compare(datasetName, OrmapExtension.TableNamesSettings.PlatsFC, True, CultureInfo.CurrentCulture) = StringMatch)
        returnValue = returnValue OrElse (String.Compare(datasetName, OrmapExtension.TableNamesSettings.ReferenceLinesFC, True, CultureInfo.CurrentCulture) = StringMatch)
        returnValue = returnValue OrElse (String.Compare(datasetName, OrmapExtension.TableNamesSettings.TaxCodeFC, True, CultureInfo.CurrentCulture) = StringMatch)
        returnValue = returnValue OrElse (String.Compare(datasetName, OrmapExtension.TableNamesSettings.TaxLotLinesFC, True, CultureInfo.CurrentCulture) = StringMatch)

        Return returnValue

    End Function

    ''' <summary>
    ''' Determine if the feature belongs to the Taxlot feature class.
    ''' </summary>
    ''' <param name="thisObject">A valid initialized geodatabase object.</param>
    ''' <returns><c>True</c> or <c>False</c>.</returns>
    ''' <remarks>Determine if thisObject belongs to the Taxlot feature class by 
    ''' checking the name of the dataset of thisObject feature class against 
    ''' the Taxlot Feature Class constant.</remarks>
    Public Shared Function IsTaxlot(ByVal thisObject As IObject) As Boolean
        Dim thisObjectClass As IObjectClass
        Dim thisDataset As IDataset
        thisObjectClass = thisObject.Class
        thisDataset = DirectCast(thisObjectClass, IDataset)
        If String.Compare(thisDataset.Name, OrmapExtension.TableNamesSettings.TaxLotFC, True, CultureInfo.CurrentCulture) = 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Loads the specified feature class into the current map as a feature layer.
    ''' </summary>
    ''' <param name="featureClassName">The feature class to find.</param>
    ''' <returns><c>True</c> for found and loaded, <c>False</c> for not found and loaded.</returns>
    ''' <remarks>Show a dialog box with title that allows the user to select the 
    ''' personal geodatabase that the <paramref name="featureClassName"/> resides in. 
    ''' The feature class is then loaded in the current map from the chosen personal 
    ''' geodatabase.</remarks>
    Public Overloads Shared Function LoadFCIntoMap(ByVal featureClassName As String) As Boolean
        Return LoadFCIntoMap(featureClassName, String.Empty)
    End Function

    ''' <summary>
    ''' Loads the specified feature class into the current map as a feature layer.
    ''' </summary>
    ''' <param name="featureClassName">The feature class to find.</param>
    ''' <param name="dialogTitle">An alternate title for the file dialog box.</param>
    ''' <returns><c>True</c> for found and loaded, <c>False</c> for not found and loaded.</returns>
    ''' <remarks>Show a dialog box with title that allows the user to select the 
    ''' personal geodatabase that the <paramref name="featureClassName"/> resides in. 
    ''' The feature class is then loaded in the current map from the chosen personal 
    ''' geodatabase.</remarks>
    Public Overloads Shared Function LoadFCIntoMap(ByVal featureClassName As String, ByVal dialogTitle As String) As Boolean

        Dim theFileDialog As CatalogFileDialog
        theFileDialog = New CatalogFileDialog()

        With theFileDialog
            .SetAllowMultiSelect(True)
            .SetButtonCaption("Select")
            If dialogTitle.Length > 0 Then
                .SetTitle(dialogTitle)
            Else
                .SetTitle(String.Concat("Find feature class ", featureClassName, "..."))
            End If
            .SetFilter(New GxFilterFeatureClasses, True, True)
            .ShowOpen()
        End With

        ' Exit if there is nothing selected
        If theFileDialog.SelectedObject(0) Is Nothing Then
            Return False
        End If

        Dim theWorkSpace As IWorkspace = getWorkspace(theFileDialog)
        ' Exit if the file selected is not in a valid workspace
        If theWorkSpace Is Nothing Then
            Return False
        End If

        Dim theFeatureWorkspace As IFeatureWorkspace
        theFeatureWorkspace = DirectCast(theWorkSpace, IFeatureWorkspace)

        Dim theFeatureClass As IFeatureClass
        theFeatureClass = theFeatureWorkspace.OpenFeatureClass(featureClassName)

        Dim theFeatureLayer As New FeatureLayer
        theFeatureLayer.FeatureClass = theFeatureClass

        Dim theDataSet As IDataset
        theDataSet = DirectCast(theFeatureClass, IDataset)
        theFeatureLayer.Name = theDataSet.Name

        Dim theMXDocument As IMxDocument = DirectCast(My.ArcMap.Application.Document, IMxDocument)
        Dim theMap As IMap = theMXDocument.FocusMap
        theMap.AddLayer(theFeatureLayer)

        Dim theArcMapDoc As IMxDocument
        theArcMapDoc = DirectCast(My.ArcMap.Application.Document, IMxDocument)
        theArcMapDoc.CurrentContentsView.Refresh(0)

        Return True

    End Function

    ''' <summary>
    ''' Loads the specified object class (table) into the current map.
    ''' </summary>
    ''' <param name="objectClassName">The object class (table) to find.</param>
    ''' <returns><c>True</c> for found and loaded, <c>False</c> for not found and loaded.</returns>
    ''' <remarks>Show a dialog box with title that allows the user to select the 
    ''' personal geodatabase that the <paramref name="objectClassName"/> resides in. 
    ''' The object class is then loaded in the current map from the chosen personal 
    ''' geodatabase.</remarks>
    Public Overloads Shared Function LoadTableIntoMap(ByVal objectClassName As String) As Boolean
        Return LoadTableIntoMap(objectClassName, String.Empty)
    End Function

    ''' <summary>
    ''' Loads the specified object class (table) into the current map.
    ''' </summary>
    ''' <param name="objectClassName">The object class (table) to find.</param>
    ''' <param name="dialogTitle">An alternate title for the file dialog box.</param>
    ''' <returns><c>True</c> for found and loaded, <c>False</c> for not found and loaded.</returns>
    ''' <remarks>Show a dialog box with title that allows the user to select the 
    ''' personal geodatabase that the <paramref name="objectClassName"/> resides in. 
    ''' The object class is then loaded in the current map from the chosen data source.</remarks>
    Public Overloads Shared Function LoadTableIntoMap(ByVal objectClassName As String, ByVal dialogTitle As String) As Boolean

        Dim theFileDialog As CatalogFileDialog
        theFileDialog = New CatalogFileDialog()

        With theFileDialog
            .SetAllowMultiSelect(True)
            .SetButtonCaption("Select")
            If dialogTitle.Length > 0 Then
                .SetTitle(dialogTitle)
            Else
                .SetTitle(String.Concat("Find object class (table) ", objectClassName, "..."))
            End If
            '.SetFilter(New GxFilterTables, True, True)
            .SetFilter(New GxFilterPGDBTables, True, True)
            .SetFilter(New GxFilterSDETables, False, False)
            .SetFilter(New GxFilterFGDBTables, False, False)
            .ShowOpen()
        End With

        ' Exit if there is nothing selected
        If theFileDialog.SelectedObject(0) Is Nothing Then
            Return False
        End If

        Dim theWorkSpace As IWorkspace = getWorkspace(theFileDialog)
        ' Exit if the file selected is not in a valid workspace
        If theWorkSpace Is Nothing Then
            Return False
        End If

        Dim theFeatureWorkspace As IFeatureWorkspace
        theFeatureWorkspace = DirectCast(theWorkSpace, IFeatureWorkspace)

        ' Open the table
        Dim theTable As Geodatabase.ITable
        theTable = theFeatureWorkspace.OpenTable(objectClassName)

        ' Get the map
        Dim theMXDocument As IMxDocument = DirectCast(My.ArcMap.Application.Document, IMxDocument)
        Dim theMap As IMap = theMXDocument.FocusMap

        ' Create a table collection and assign the new table to it
        Dim theStandaloneTable As IStandaloneTable
        Dim theStandaloneTableCollection As IStandaloneTableCollection
        theStandaloneTable = New StandaloneTable
        theStandaloneTable.Table = theTable
        theStandaloneTableCollection = DirectCast(theMap, IStandaloneTableCollection)
        theStandaloneTableCollection.AddStandaloneTable(theStandaloneTable)

        ' Create a new table window for the table
        Dim theTableWindow As ITableWindow
        theTableWindow = New TableWindow
        theTableWindow.Table = theTable
        theTableWindow.ShowAliasNamesInColumnHeadings = True
        theTableWindow.Application = My.ArcMap.Application

        ' Update the document
        theMXDocument.UpdateContents()

        Return True

    End Function

    ''' <summary>
    ''' Find the index of a field in a feature class.
    ''' </summary>
    ''' <param name="featureClass">The feature class to locate a field in.</param>
    ''' <param name="fieldName">The name of the field to find.</param>
    ''' <returns>Index of field or -1.</returns>
    ''' <remarks>This function may return zero because that is a valid index, but -1 is not. The return value of -1 means the field was not found.</remarks>
    Public Shared Function LocateFields(ByVal featureClass As ESRI.ArcGIS.Geodatabase.IFeatureClass, ByVal fieldName As String) As Integer
        Dim returnValue As Integer
        returnValue = featureClass.Fields.FindField(fieldName)
        Return returnValue
    End Function

    ''' <summary>
    ''' Reads a value from a row, given a field name.
    ''' </summary>
    ''' <param name="row">An object that implements the IRow interface.</param>
    ''' <param name="fieldName">A field that exists in row.</param>
    ''' <returns>A string containing the coded name.</returns>
    ''' <remarks>Reads the value of a field with a domain and translates 
    ''' the value from the coded value to the coded name.</remarks>
    Public Overloads Shared Function ReadValue(ByVal row As IRow, ByVal fieldName As String) As String
        Return ReadValue(row, fieldName, "")
    End Function

    ''' <summary>
    ''' Reads a value from a row, given a field name.
    ''' </summary>
    ''' <param name="row">An object that implements the IRow interface.</param>
    ''' <param name="fieldName">A field that exists in row.</param>
    ''' <param name="dataType">A string value indicating data type of the field.</param>
    ''' <returns>A string containing the coded name.</returns>
    ''' <remarks>Reads the value of a field with a domain and translates 
    ''' the value from the coded value to the coded name.</remarks>
    Public Overloads Shared Function ReadValue(ByVal row As IRow, ByVal fieldName As String, ByVal dataType As String) As String

        Dim fieldIndex As Integer
        Dim returnValue As String = String.Empty

        fieldIndex = row.Fields.FindField(fieldName)
        If fieldIndex > -1 Then
            If String.Compare(dataType, "date", True, CultureInfo.CurrentCulture) = 0 Then
                If IsDBNull(row.Value(fieldIndex)) Then
                    returnValue = CStr(System.DateTime.Today)
                Else
                    returnValue = CStr(row.Value(fieldIndex))
                End If
            Else
                If IsDBNull(row.Value(fieldIndex)) Then
                    returnValue = String.Empty
                Else
                    returnValue = CStr(row.Value(fieldIndex))
                End If
            End If
            'Determine if a Domain Field
            Dim field As IField
            field = row.Fields.Field(fieldIndex)
            Dim domain As IDomain
            domain = field.Domain
            If Not domain Is Nothing Then
                If domain.Type = esriDomainType.esriDTCodedValue Then
                    'If TypeOf domain Is ICodedValueDomain Then
                    Dim thisCodedValueDomain As ICodedValueDomain
                    thisCodedValueDomain = DirectCast(domain, ICodedValueDomain)
                    Dim domainValue As Object
                    domainValue = row.Value(fieldIndex)
                    'search domain for the code
                    For domainIndex As Integer = 0 To thisCodedValueDomain.CodeCount - 1
                        If String.Compare(thisCodedValueDomain.Value(domainIndex).ToString, domainValue.ToString, True, CultureInfo.CurrentCulture) = 0 Then
                            returnValue = thisCodedValueDomain.Name(domainIndex)
                            Exit For
                        End If
                    Next domainIndex
                End If
            End If
        End If

        Return returnValue

    End Function

    ''' <summary>
    ''' Update/Initialize feature linked annotation size.
    ''' </summary>
    ''' <param name="annoObject">An annotation object.</param>
    ''' <remarks>
    ''' <para>Given an object, <paramref name="theObject">annoObject</paramref>, 
    ''' determines if <paramref name="theObject">annoObject</paramref> is a taxlot 
    ''' annotation feature, gets the map scale for the Map Index polygon overlaying 
    ''' the annotation or the feature to which it is linked if it is feature-linked, 
    ''' and resets the annotation symbol size.</para>
    ''' </remarks>
    Public Shared Sub SetAnnoSize(ByVal annoObject As IObject)

        Dim theFeature As ESRI.ArcGIS.Geodatabase.IFeature
        Dim theAnnotationFeature As ESRI.ArcGIS.Carto.IAnnotationFeature

        Dim theLinkedFeatureID As Integer
        theAnnotationFeature = DirectCast(annoObject, IAnnotationFeature)

        theLinkedFeatureID = theAnnotationFeature.LinkedFeatureID
        If theLinkedFeatureID > NotFoundIndex Then
            '[Feature linked anno...]
            ' Get the related feature so the map number can be obtained
            theFeature = GetRelatedObjects(annoObject)
            If theFeature Is Nothing Then Exit Sub
        Else
            '[Not feature linked anno...]
            ' Use the annotation feature as the feature
            theFeature = DirectCast(annoObject, IFeature)
        End If

        ' Check for valid data
        If Not HasValidMapIndexData Then
            MessageBox.Show("Unable to set annotation size." & NewLine & _
                            "Missing data: Valid ORMAP MapIndex layer not found in the map." & NewLine & _
                            "Please load this dataset into your map.", _
                            "Set Anno Size", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Exit Sub
        End If

        ' Get the Map Index data.
        Dim theMapIndexFeatureClass As IFeatureClass
        theMapIndexFeatureClass = MapIndexFeatureLayer.FeatureClass

        ' Get the feature geometry.
        Dim theGeometry As IGeometry
        theGeometry = theFeature.Shape
        If theGeometry.IsEmpty Then
            Exit Sub
        End If

        ' Update the annotation size to reflect current mapscale...

        ' Get the Map Index map scale
        Dim theMapScale As String = GetValue(theGeometry, theMapIndexFeatureClass, OrmapExtension.MapIndexSettings.MapScaleField, OrmapExtension.MapIndexSettings.MapNumberField)
        If theMapScale.Length = 0 Then
            Exit Sub
        End If

        ' Determine which annotation class this is
        Dim theAnnoObjectClass As IObjectClass
        theAnnoObjectClass = annoObject.Class

        Dim theAnnoDataSet As IDataset
        theAnnoDataSet = DirectCast(theAnnoObjectClass, IDataset)

        'If taxlot annotation of one kind or another, change annotation size
        Dim isTaxlotNumberAnno As Boolean = (String.Compare(theAnnoDataSet.Name, OrmapExtension.AnnoTableNamesSettings.TaxlotNumberAnnoFC, True, CultureInfo.CurrentCulture) = 0)
        Dim isTaxlotAcreageAnno As Boolean = (String.Compare(theAnnoDataSet.Name, OrmapExtension.AnnoTableNamesSettings.TaxlotAcreageAnnoFC, True, CultureInfo.CurrentCulture) = 0)
        If isTaxlotNumberAnno OrElse isTaxlotAcreageAnno Then

            ' Gets the size of the annotation from the scale of the annotation dataset
            Dim theAnnotationSize As Double = getAnnoSizeByScale(theAnnoDataSet.Name, CInt(theMapScale))

            Dim pwrkspedit As IWorkspaceEdit
            pwrkspedit = DirectCast(theAnnoDataSet.Workspace, IWorkspaceEdit)

            ' set the new annotation size
            Dim theannoelement As IAnnotationElement
            Dim thetextelement As ITextElement
            Dim thetextsymbol As ESRI.ArcGIS.Display.ITextSymbol

            theannoelement = DirectCast(theAnnotationFeature.Annotation, IAnnotationElement)
            If theannoelement Is Nothing Then Exit Sub 'this can occur when the linked attribute expression evaluates to an empty string

            thetextelement = DirectCast(theannoelement, ITextElement)
            thetextsymbol = thetextelement.Symbol

            thetextsymbol.Size = theAnnotationSize

            ' we need to wrap this back together (as was done in vb6 code)
            thetextelement.Symbol = thetextsymbol
            theannoelement = DirectCast(thetextelement, IAnnotationElement)
            theAnnotationFeature.Annotation = DirectCast(theannoelement, IElement)

            ' update the annotation attribute "FontSize" in addition to the font size property...
            Dim theUpdatedFeature As IFeature = DirectCast(theAnnotationFeature, IFeature)
            theUpdatedFeature.Value(theUpdatedFeature.Fields.FindField("FontSize")) = theAnnotationSize

        End If

    End Sub

    ''' <summary>
    ''' Updates Auto fields in a feature class.
    ''' </summary>
    ''' <param name="feature">An object that implements the Ifeature interface.</param>
    ''' <remarks>Update the AutoWho and the AutoDate fields with the current username and date/time, respectively.</remarks>
    Public Shared Sub UpdateMinimumAutoFields(ByVal feature As IFeature)
        Try
            If feature Is Nothing Then
                Exit Sub
            End If

            Dim theAutoDateFieldIndex As Integer
            theAutoDateFieldIndex = feature.Fields.FindField(OrmapExtension.AllTablesSettings.AutoDateField)
            If theAutoDateFieldIndex > NotFoundIndex Then
                feature.Value(theAutoDateFieldIndex) = System.DateTime.Now
            End If

            Dim theAutoWhoFieldIndex As Integer
            theAutoWhoFieldIndex = feature.Fields.FindField(OrmapExtension.AllTablesSettings.AutoWhoField)
            If theAutoWhoFieldIndex > NotFoundIndex Then
                feature.Value(theAutoWhoFieldIndex) = OrmapTaxlotEditing.Utilities.UserName()
            End If

        Catch ex As Exception
            ' Add clean-up code here...

            Throw

        End Try
    End Sub

    ''' <summary>
    ''' Determine the local uniqueness of a taxlot number.
    ''' </summary>
    ''' <param name="taxlotNumber">The taxlot value to validate.</param>
    ''' <param name="thisGeometry">The geometry of the feature to check.</param>
    ''' <param name="allowOneInstance">Allow one instance in the map or none.</param>
    ''' <returns>True or False</returns>
    ''' <remarks>
    ''' <para>Determine if the feature represented by thisGeometry has a 
    ''' taxlot number unique for the corresponding map index.</para>
    ''' <para>Unique can be defined eithe as not being present at all 
    ''' or having only one match present.</para>
    ''' </remarks>
    Public Shared Function IsTaxlotNumberLocallyUnique(ByVal taxlotNumber As String, ByVal thisGeometry As IGeometry, ByVal allowOneInstance As Boolean) As Boolean

        Dim returnValue As Boolean = False

        ' Check for existence of Taxlot layer
        Dim thisTaxlotFeatureLayer As IFeatureLayer = Nothing
        If TaxlotFeatureLayer Is Nothing Then
            thisTaxlotFeatureLayer = FindFeatureLayerByDSName(OrmapExtension.TableNamesSettings.TaxLotFC)
            If thisTaxlotFeatureLayer Is Nothing Then
                MessageBox.Show("Unable to locate the Taxlot layer in Table of Contents." & NewLine & _
                                "This process requires a feature class called " & OrmapExtension.TableNamesSettings.TaxLotFC & ".", _
                                String.Empty, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                Return returnValue
            End If
        Else
            thisTaxlotFeatureLayer = TaxlotFeatureLayer
        End If

        Dim thisTaxlotFeatureClass As IFeatureClass
        thisTaxlotFeatureClass = thisTaxlotFeatureLayer.FeatureClass

        ' Check for existence of MapIndex layer
        Dim thisMapIndexFeatureLayer As IFeatureLayer = Nothing
        If MapIndexFeatureLayer Is Nothing Then
            thisMapIndexFeatureLayer = FindFeatureLayerByDSName(OrmapExtension.TableNamesSettings.MapIndexFC)
            If thisMapIndexFeatureLayer Is Nothing Then
                MessageBox.Show("Unable to locate the MapIndex layer in Table of Contents." & NewLine & _
                                "This process requires a feature class called " & OrmapExtension.TableNamesSettings.MapIndexFC & ".", _
                                String.Empty, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                Return returnValue
            End If
        Else
            thisMapIndexFeatureLayer = MapIndexFeatureLayer
        End If

        Dim mapIndexFeatureClass As IFeatureClass
        mapIndexFeatureClass = thisMapIndexFeatureLayer.FeatureClass

        ' Check for the existence of a current OR Map Number and Taxlot number
        Dim mapIndexValue As String = String.Empty
        mapIndexValue = GetValue(thisGeometry, mapIndexFeatureClass, OrmapExtension.MapIndexSettings.MapNumberField, OrmapExtension.MapIndexSettings.MapNumberField)
        If mapIndexValue.Length = 0 Then
            returnValue = True
        End If

        ' Make sure this number is unique within taxlots with this OR Map Number
        Dim whereClause As String = formatWhereClause(String.Concat("[", OrmapExtension.TaxLotSettings.MapNumberField, "]", " = '", mapIndexValue, "' AND ", "[", OrmapExtension.TaxLotSettings.TaxlotField, "]", " = '", taxlotNumber, "'"), thisTaxlotFeatureClass)
        Dim n As Integer = 0
        Dim cursor As ICursor
        cursor = attributeQuery(DirectCast(thisTaxlotFeatureClass, ITable), whereClause, False)
        If Not (cursor Is Nothing) AndAlso (returnValue = False) Then
            Dim row As IRow
            row = cursor.NextRow
            Do Until row Is Nothing
                n += 1
                row = cursor.NextRow
            Loop
        End If

        If allowOneInstance Then
            If n < 2 Then
                returnValue = True
            Else
                returnValue = False
            End If
        Else
            If n < 1 Then
                returnValue = True
            Else
                returnValue = False
            End If
        End If

        Return returnValue

    End Function

    ''' <summary>
    ''' Zooms to the given envelope.
    ''' </summary>
    ''' <param name="theEnvelope">The envelope to zoom to.</param>
    ''' <remarks>Replaces ZoomToExtent sub.  This sub removes the unneeded pMxDoc parameter.</remarks>
    Public Shared Sub ZoomToEnvelope(ByVal theEnvelope As IEnvelope)

        Dim theArcMapDoc As IMxDocument = DirectCast(My.ArcMap.Application.Document, IMxDocument)
        Dim theMap As IMap = theArcMapDoc.FocusMap
        Dim theActiveView As IActiveView = DirectCast(theMap, IActiveView)

        ' Updates the view's extent
        theEnvelope.Expand(1.1, 1.1, True)
        theActiveView.Extent = theEnvelope
        theActiveView.Refresh()

    End Sub

    ''' <summary>
    ''' Refresh the display's selection cache.
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub RefreshDisplaySelection()
        ' Partially refresh the display
        Dim theArcMapDoc As IMxDocument = DirectCast(My.ArcMap.Application.Document, IMxDocument)
        Dim theMap As IMap = theArcMapDoc.FocusMap
        Dim theActiveView As IActiveView = DirectCast(theMap, IActiveView)
        theActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, Nothing, Nothing)
    End Sub

    ''' <summary>
    ''' Selects a single feature.
    ''' </summary>
    ''' <param name="featureLayer">The feature layer containing the feature.</param>
    ''' <param name="feature">The feature to zoom to.</param> 
    ''' <remarks></remarks>
    Public Shared Sub SetSelectedFeature(ByVal featureLayer As IFeatureLayer, ByVal feature As IFeature, ByVal addToSelection As Boolean, ByVal refreshMap As Boolean)

        Dim theArcMapDoc As IMxDocument = DirectCast(My.ArcMap.Application.Document, IMxDocument)
        Dim theMap As IMap = theArcMapDoc.FocusMap
        Dim theActiveView As IActiveView = DirectCast(theMap, IActiveView)

        ' Partially refresh the display
        ' (When selecting features, you must call PartialRefresh twice, 
        ' once before and once after the selection operation.)
        theActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, Nothing, Nothing)

        ' Select the feature
        If Not addToSelection Then
            theMap.ClearSelection()
        End If
        theMap.SelectFeature(featureLayer, feature)

        If refreshMap Then
            'theActiveView.Refresh()

            ' Partially refresh the display again
            theActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, Nothing, Nothing)
        End If

    End Sub


    Public Overloads Shared Function formatWhereClause(ByVal inWhereClause As String, ByVal inTable As ITable) As String

        Dim theDataset As IDataset = DirectCast(inTable, IDataset)
        Dim theWorkspace As IWorkspace = theDataset.Workspace
        Dim theSQLSyntax As ISQLSyntax = DirectCast(theWorkspace, ISQLSyntax)

        Dim theDelimiterPrefix As String = theSQLSyntax.GetSpecialCharacter(esriSQLSpecialCharacters.esriSQL_DelimitedIdentifierPrefix)
        Dim theDelimiterSuffix As String = theSQLSyntax.GetSpecialCharacter(esriSQLSpecialCharacters.esriSQL_DelimitedIdentifierSuffix)

        Dim formattedWhereClause As String = String.Empty
        formattedWhereClause = inWhereClause.Replace("[", theDelimiterPrefix).Replace("]", theDelimiterSuffix)

        Return formattedWhereClause

    End Function



    ''' <summary>
    ''' Formats a WhereClause based upon the data source. Input format should be based on the personal geodatabase.
    ''' </summary>
    ''' <param name="inWhereClause">The original WhereClause written for a personal geodatabase (i.e. with bracket 
    ''' field name delimiters: "[fieldname]").</param>
    ''' <param name="inFeatureClass">The Feature Class for which the WhereClause is being generated.</param>
    ''' <returns>A String containing the formatted where clause.</returns>
    ''' <remarks>See EDN (http://edndoc.esri.com/arcobjects/9.2/ComponentHelp/esriGeoDatabase//IQueryFilter_WhereClause.htm) 
    ''' for more information on where clause syntax flavors.</remarks>
    Public Overloads Shared Function formatWhereClause(ByVal inWhereClause As String, ByVal inFeatureClass As IFeatureClass) As String

        Dim theDataset As IDataset = DirectCast(inFeatureClass, IDataset)
        Dim theWorkspace As IWorkspace = theDataset.Workspace
        Dim theSQLSyntax As ISQLSyntax = DirectCast(theWorkspace, ISQLSyntax)

        Dim theDelimiterPrefix As String = theSQLSyntax.GetSpecialCharacter(esriSQLSpecialCharacters.esriSQL_DelimitedIdentifierPrefix)
        Dim theDelimiterSuffix As String = theSQLSyntax.GetSpecialCharacter(esriSQLSpecialCharacters.esriSQL_DelimitedIdentifierSuffix)

        Dim formattedWhereClause As String = String.Empty
        formattedWhereClause = inWhereClause.Replace("[", theDelimiterPrefix).Replace("]", theDelimiterSuffix)

        Return formattedWhereClause

    End Function

#End Region

#Region "Private Members"

    ''' <summary>
    ''' Return a cursor that represents the results of an attribute query.
    ''' </summary>
    ''' <param name="table">An object that supports the ITable interface.</param>
    ''' <param name="whereClause">A SQL Where clause without the WHERE.</param>
    ''' <param name="isEditable">A flag representing whether to return an editable 
    ''' (update) cursor or not.</param>
    ''' <returns>Return a cursor that represents the results of an attribute query.</returns>
    ''' <remarks>Creates a cursor from table that contains all feature records that 
    ''' meet the criteria in <paramref name="whereClause">whereClause</paramref>.</remarks>
    Private Shared Function attributeQuery(ByVal table As ITable, Optional ByVal whereClause As String = "", Optional ByVal isEditable As Boolean = False) As ICursor

        Dim thisQueryFilter As IQueryFilter
        thisQueryFilter = New QueryFilter
        thisQueryFilter.WhereClause = whereClause
        Dim thisCursor As ICursor
        If isEditable Then
            thisCursor = table.Update(thisQueryFilter, False) 'non-recycling update cursor
        Else
            thisCursor = table.Search(thisQueryFilter, True) 'recycling search cursor
        End If

        If table.RowCount(thisQueryFilter) = 0 Then
            Return Nothing
        Else
            Return thisCursor
        End If

    End Function

    ''' <summary>
    ''' Calculate ORMAP Taxlot Number when one if its components has changed.
    ''' </summary>
    ''' <param name="existingORMapNum">An ORMAP Number.</param>
    ''' <param name="theFeature">An object that supports the IFeature interface.</param>
    ''' <param name="taxlotValue">A taxlot number.</param>
    ''' <returns>A string that represents an ORMAP number updated with the value from theFeature and taxlotValue.</returns>
    ''' <remarks>Given an ORMAP Number, <paramref name="existingORMapNum">existingORMapNum</paramref>, and feature, <paramref name="theFeature">theFeature</paramref>,
    ''' and a taxlot value,<paramref name="taxlotValue">taxlotValue</paramref>.
    ''' Remove the existing map suffix type and number from <paramref name="existingORMapNum">existingORMapNum</paramref> and replace them with the new values in <paramref name="theFeature">theFeature</paramref> and
    ''' append <paramref name="taxlotValue">taxlotValue</paramref> to form the return value.</remarks>
    Private Shared Function generateORMAPTaxlotNumber(ByVal existingORMapNum As String, ByVal theFeature As IFeature, ByVal taxlotValue As String) As String

        Dim shortORMapNum As String = existingORMapNum.Substring(0, 20) 'replaces the ShortenOMTLNum function 
        Dim taxlotMapSufNumberValue As String = GetMapSuffixNumber(theFeature)
        Dim taxlotMapSufTypeValue As String = GetMapSuffixType(theFeature)
        ' Recreate and return the ORMAP Taxlot number
        Return String.Concat(shortORMapNum, taxlotMapSufTypeValue, taxlotMapSufNumberValue, taxlotValue)

    End Function

    ''' <summary>
    ''' Converts the list of candidate dictionary keys to a string in the 
    ''' format of a comma-delimited list.
    ''' </summary>
    ''' <param name="candidatesDictionary">A dictionary of integer candidate IDs for keys.</param>
    ''' <returns>A string in the format of a comma-delimited list.</returns>
    ''' <remarks>An empty dictionary will return as an empty string.</remarks>
    Private Shared Function candidateKeysToDelimitedString(ByVal candidatesDictionary As Dictionary(Of Integer, Double)) As String
        Dim keyCollection As ICollection(Of Integer) = DirectCast(candidatesDictionary.Keys, ICollection(Of Integer))
        Dim returnValue As String = String.Empty

        If keyCollection.Count > 0 Then
            ' The elements of the keyCollection are strongly typed
            ' with the type that was specified for dictionary values.
            For Each n As Integer In keyCollection
                ' Append keys as strings and commas.
                returnValue &= (CStr(n) & ",")
            Next n
            ' Trim off the last comma.
            returnValue = Left(returnValue, returnValue.Length - 1)
        Else
            ' Will return the default empty string.
        End If
        Return returnValue

    End Function

    ''' <summary>
    ''' Return a feature cursor based on the results of a spatial query.
    ''' </summary>
    ''' <param name="inFeatureClass">Feature class to search.</param>
    ''' <param name="searchGeometry">Geometry to search in relation to spatialRelation.</param>
    ''' <param name="spatialRelation">Geometry relationship to searchGeometry.</param>
    ''' <param name="whereClause">SQL Where clause.</param>
    ''' <param name="isUpdateable">Read/Write state of the return cursor.</param>
    ''' <returns>Returns a feature cursor that represents the results of the spatial query.</returns>
    ''' <remarks>Given a feature class, <paramref name="inFeatureClass">inFeatureClass</paramref>, 
    ''' a search geometry,<paramref name=" searchGeometry">searchGeometry</paramref> , 
    ''' a spatial relationship,<paramref name=" spatialRelation">spatialRelation</paramref> , 
    ''' an Sql search statement, <paramref name=" whereClause">whereClause</paramref>,
    ''' and whether or not the returned cursor should be updateable, <paramref name=" isUpdateable">IsUpdateable</paramref>.
    ''' Perform a spatial query <paramref name="infeatureClass">inFeatureClass</paramref> where feature
    ''' which meet criteria whereClause have a relationship of spatialRelation to searchGeometry. 
    ''' The returned cursor is updatable if IsUpdateable is True.</remarks>
    Public Shared Function DoSpatialQuery(ByVal inFeatureClass As IFeatureClass, ByVal searchGeometry As IGeometry, ByVal spatialRelation As esriSpatialRelEnum, Optional ByVal whereClause As String = "", Optional ByVal isUpdateable As Boolean = False) As IFeatureCursor

        Dim thisSpatialFilter As ISpatialFilter
        thisSpatialFilter = New SpatialFilter
        thisSpatialFilter.Geometry = searchGeometry

        Dim shapeFieldName As String
        shapeFieldName = inFeatureClass.ShapeFieldName
        thisSpatialFilter.GeometryField = shapeFieldName

        thisSpatialFilter.SpatialRel = spatialRelation
        thisSpatialFilter.WhereClause = whereClause

        Dim thisQueryFilter As IQueryFilter
        thisQueryFilter = thisSpatialFilter
        Dim thisFeatureCursor As IFeatureCursor

        If isUpdateable Then
            thisFeatureCursor = inFeatureClass.Update(thisQueryFilter, False)
        Else
            thisFeatureCursor = inFeatureClass.Search(thisQueryFilter, False)
        End If

        Return thisFeatureCursor

    End Function

    ''' <summary>
    ''' Copy envelope points to polygon.
    ''' </summary>
    ''' <param name="envelope"></param>
    ''' <returns>A Polygon.</returns>
    ''' <remarks></remarks>
    Private Shared Function envelopeToPolygon(ByVal envelope As IEnvelope) As IPolygon
        Dim thisPolygon As IPolygon
        thisPolygon = New Polygon
        Dim pointCollection As IPointCollection
        pointCollection = DirectCast(thisPolygon, IPointCollection)
        With pointCollection
            .AddPoint(envelope.UpperRight)
            .AddPoint(envelope.LowerRight)
            .AddPoint(envelope.LowerLeft)
            .AddPoint(envelope.UpperLeft)
        End With
        Return thisPolygon
    End Function

    ''' <summary>
    ''' Figure out what type of workspace to open. It won't always be a personal geodatabase.
    ''' </summary>
    ''' <param name="thisFileDialog"></param>
    ''' <returns>An object implementing IWorkspace.</returns>
    ''' <remarks>Valid workspaces:
    ''' <list type="bullet">
    ''' <item><description>Geodatabases that are local to your machine, e.g. a File Geodatabase or a Personal Geodatabase stored in an Access file</description></item>
    ''' <item><description>Geodatabases that require a remote connection. e.g. ArcSDE</description></item>
    ''' </list>
    ''' </remarks>
    Private Shared Function getWorkspace(ByVal thisFileDialog As CatalogFileDialog) As IWorkspace
        ' HACK: [NIS] Is there a better way to find the correct workspace type?
        Dim thisWorkSpace As IWorkspace = Nothing
        Dim thisAccessWorkspaceFactory As IWorkspaceFactory2 = New AccessWorkspaceFactory
        Dim thisSdeWorkspaceFactory As IWorkspaceFactory2 = New SdeWorkspaceFactory
        Dim thisFileGDBWorkspaceFactory As IWorkspaceFactory2 = New FileGDBWorkspaceFactory
        Try
            Dim thisFileDialogSelectedObject As String = CStr(thisFileDialog.SelectedObject(0))
            Dim theParentPath As String = My.Computer.FileSystem.GetParentPath(thisFileDialogSelectedObject) 'Geodatabase or feature dataset
            Dim theGrandParentPath As String = My.Computer.FileSystem.GetParentPath(theParentPath) 'Needed if parent is a feature dataset
            If thisAccessWorkspaceFactory.IsWorkspace(theParentPath) Then
                thisWorkSpace = thisAccessWorkspaceFactory.OpenFromFile(theParentPath, 0)
            ElseIf thisAccessWorkspaceFactory.IsWorkspace(theGrandParentPath) Then
                thisWorkSpace = thisAccessWorkspaceFactory.OpenFromFile(theGrandParentPath, 0)
            ElseIf thisSdeWorkspaceFactory.IsWorkspace(theParentPath) Then
                thisWorkSpace = thisSdeWorkspaceFactory.OpenFromFile(theParentPath, 0)
            ElseIf thisSdeWorkspaceFactory.IsWorkspace(theGrandParentPath) Then
                thisWorkSpace = thisSdeWorkspaceFactory.OpenFromFile(theParentPath, 0)
            ElseIf thisFileGDBWorkspaceFactory.IsWorkspace(theParentPath) Then
                thisWorkSpace = thisFileGDBWorkspaceFactory.OpenFromFile(theParentPath, 0)
            ElseIf thisFileGDBWorkspaceFactory.IsWorkspace(theGrandParentPath) Then
                thisWorkSpace = thisFileGDBWorkspaceFactory.OpenFromFile(theParentPath, 0)
            End If

            Return thisWorkSpace

        Catch ex As Exception
            thisWorkSpace = Nothing

            Throw

        End Try

    End Function

    ''' <summary>
    ''' Determine the annotation size based on scale.
    ''' </summary>
    ''' <param name="thisFeatureClassName">Feature class to find the proper annotation size for.</param>
    ''' <param name="scale">The scale of the feature class.</param>
    ''' <returns>A double that represents the proper scale factor.</returns>
    ''' <remarks>Determines the proper size for the text in thisFeatureClassName.
    ''' Defaults at size 5 is the <paramref name="scale">scale</paramref> is invalid, and size 10 if
    ''' <paramref name="thisFeatureClassName">thisFeatureClassName</paramref> is not Taxlot Acreage Annotation or Taxlot Number Annotation.</remarks>
    Private Shared Function getAnnoSizeByScale(ByVal thisFeatureClassName As String, ByVal scale As Integer) As Double

        Dim size As String
        If String.Compare(thisFeatureClassName, OrmapExtension.AnnoTableNamesSettings.TaxlotAcreageAnnoFC, True, CultureInfo.CurrentCulture) = 0 Then
            Select Case scale
                Case 120
                    size = OrmapExtension.TaxlotAcreageAnnoSettings.TextSize00120Scale
                Case 240
                    size = OrmapExtension.TaxlotAcreageAnnoSettings.TextSize00240Scale
                Case 360
                    size = OrmapExtension.TaxlotAcreageAnnoSettings.TextSize00360Scale
                Case 480
                    size = OrmapExtension.TaxlotAcreageAnnoSettings.TextSize00480Scale
                Case 600
                    size = OrmapExtension.TaxlotAcreageAnnoSettings.TextSize00600Scale
                Case 1200
                    size = OrmapExtension.TaxlotAcreageAnnoSettings.TextSize01200Scale
                Case 2400
                    size = OrmapExtension.TaxlotAcreageAnnoSettings.TextSize02400Scale
                Case 4800
                    size = OrmapExtension.TaxlotAcreageAnnoSettings.TextSize04800Scale
                Case 9600
                    size = OrmapExtension.TaxlotAcreageAnnoSettings.TextSize09600Scale
                Case 24000
                    size = OrmapExtension.TaxlotAcreageAnnoSettings.TextSize24000Scale
                Case Else
                    ' Default size
                    size = "5"
            End Select
        ElseIf String.Compare(thisFeatureClassName, OrmapExtension.AnnoTableNamesSettings.TaxlotNumberAnnoFC, True, CultureInfo.CurrentCulture) = 0 Then
            Select Case scale
                Case 120
                    size = OrmapExtension.TaxlotNumberAnnoSettings.TextSize00120Scale
                Case 240
                    size = OrmapExtension.TaxlotNumberAnnoSettings.TextSize00240Scale
                Case 360
                    size = OrmapExtension.TaxlotNumberAnnoSettings.TextSize00360Scale
                Case 480
                    size = OrmapExtension.TaxlotNumberAnnoSettings.TextSize00480Scale
                Case 600
                    size = OrmapExtension.TaxlotNumberAnnoSettings.TextSize00600Scale
                Case 1200
                    size = OrmapExtension.TaxlotNumberAnnoSettings.TextSize01200Scale
                Case 2400
                    size = OrmapExtension.TaxlotNumberAnnoSettings.TextSize02400Scale
                Case 4800
                    size = OrmapExtension.TaxlotNumberAnnoSettings.TextSize04800Scale
                Case 9600
                    size = OrmapExtension.TaxlotNumberAnnoSettings.TextSize09600Scale
                Case 24000
                    size = OrmapExtension.TaxlotNumberAnnoSettings.TextSize24000Scale
                Case Else
                    size = "5"
            End Select
        Else
            size = "10" 'default
        End If

        Return CDbl(size)

    End Function

#End Region

#End Region

End Class
#End Region
