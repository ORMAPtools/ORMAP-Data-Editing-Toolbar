
#Region "Copyright 2011 ORMAP Tech Group"

' File:  SpiralUtilities.vb
'
' Original Author:  Jonathan McDowell, Clackamas County Technology Services 
'
' Date Created:  September 29, 2011
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

#Region "Imported Namespaces"


Imports System.Runtime.InteropServices
Imports System.Drawing
Imports ESRI.ArcGIS.Framework
Imports ESRI.ArcGIS.ArcMapUI
Imports System.Windows.Forms
Imports ESRI.ArcGIS.Editor
Imports ESRI.ArcGIS.Display
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Geometry
Imports System.Xml
Imports System.IO
Imports stdole
Imports OrmapTaxlotEditing.OrmapExtension

#End Region

''' <summary>
''' This module provides helper routines for the construction of spirals.  The routines in this module are used by SCS_Botton.vb adn SpiralConstruction_Button.vb.
''' </summary>
''' <remarks></remarks>
''' 
Module SpiralUtilities
    Dim _editor As IEditor3 = DirectCast(My.ArcMap.Editor, IEditor3)

    ''' <summary>
    ''' Checks the editing State
    ''' </summary>
    ''' <returns>True or False</returns>
    ''' <remarks></remarks>
    Friend Function IsEnable() As Boolean
        Dim IsEditing As Boolean
        If My.ArcMap.Editor.EditState = esriEditState.esriStateNotEditing Then
            IsEditing = False
        Else
            IsEditing = True
        End If
        Return IsEditing
    End Function

    ''' <summary>
    ''' Transforms the display coordinate to a map coordinate.
    ''' </summary>
    ''' <param name="X">Input X value</param>
    ''' <param name="Y">Input Y Value</param>
    ''' <returns>The point as withe map coordinates</returns>
    ''' <remarks></remarks>
    Friend Function getDataFrameCoords(ByVal X As Integer, ByVal Y As Integer) As IPoint
       
        Dim theDisplayTransformation As IDisplayTransformation = My.ThisApplication.Display.DisplayTransformation
        Return theDisplayTransformation.ToMapPoint(X, Y)

    End Function
    ''' <summary>
    ''' Gets the closest snapping environment point
    ''' </summary>
    ''' <param name="point"></param>
    ''' <returns>a point</returns>
    ''' <remarks></remarks>
    Function getSnapPoint(ByVal point As IPoint) As IPoint
        Dim snapEnv As ISnapEnvironment = DirectCast(_editor, ISnapEnvironment)
        snapEnv.SnapPoint(point)
        Return point
    End Function
    Function GetTargetFeatureClass() As IFeatureClass 'ByVal theLayerName As String) As IFeatureClass
        Dim editLayer As IEditLayers = DirectCast(My.ArcMap.Editor, IEditLayers)
        Dim featureLayer As IFeatureLayer2 = DirectCast(editLayer.CurrentLayer, IFeatureLayer2)
        If featureLayer Is Nothing Then
            Return Nothing
        Else
            Return featureLayer.FeatureClass
        End If

        'Dim theEnumLayers As IEnumLayer = My.ArcMap.Editor.Map.Layers
        'theEnumLayers.Reset()
        'Dim theTargetFeatureClass As IFeatureClass = Nothing
        'Dim thisLayer As ILayer2 = DirectCast(theEnumLayers.Next, ILayer2)
        'While Not thisLayer Is Nothing
        '    If thisLayer.Name = theLayerName Then
        '        Dim thisFeatureLayer As IFeatureLayer2 = DirectCast(thisLayer, IFeatureLayer2)
        '        theTargetFeatureClass = thisFeatureLayer.FeatureClass
        '    End If
        '    thisLayer = DirectCast(theEnumLayers.Next, ILayer2)
        'End While
        'Return theTargetFeatureClass
        'end John Prychun, Jan 2012 
    End Function
    Public Sub ConstructSpiralbyDelta(ByVal theFromPoint As IPoint, ByVal theTangentPoint As IPoint, ByVal doesCurveRight As Boolean, ByVal theBeginRadius As Double, ByVal theEndRadius As Double, ByVal theDeltaString As String, ByVal theTargetLayerName As String, ByVal theCurveDensity As Double)
        If My.ArcMap.Editor.EditState = esriEditState.esriStateNotEditing Then Exit Sub

        Try
            Dim theBeginCurvature As Double = 0
            Dim theEndCurvature As Double = 0

            Dim theDelta As Double
            If IsNumeric(theDeltaString) Then
                theDelta = CDbl(theDeltaString) * (Math.PI / 180)
            Else
                theDelta = DMSAngleToDouble(theDeltaString) * (Math.PI / 180)
            End If

            If theBeginRadius <> 0 Then theBeginCurvature = 1 / theBeginRadius
            If theEndRadius <> 0 Then theEndCurvature = 1 / theEndRadius

            Dim theGeometryEnvironment As IGeometryEnvironment4 = New GeometryEnvironment
            Dim TheSpiralConstruction As IConstructClothoid = DirectCast(theGeometryEnvironment, IConstructClothoid)
            Dim theSpiralPolyline As IPolyline6 = DirectCast(New Polyline, IPolyline6)

            Dim isCCW As Boolean = False
            If Not doesCurveRight Then isCCW = True

            theSpiralPolyline = DirectCast(TheSpiralConstruction.ConstructClothoidByAngle(theFromPoint, theTangentPoint, isCCW, theBeginCurvature, theEndCurvature, theDelta, esriCurveDensifyMethod.esriCurveDensifyByLength, theCurveDensity), IPolyline6)



            'John Prychun, Jan 2012
            Dim theFeatureClass As IFeatureClass = GetTargetFeatureClass() 'theTargetLayerName)
            'end John Prychun, Jan 2012

            Dim theSpiralFeature As IFeature = DirectCast(theFeatureClass.CreateFeature, IFeature)

            'John Prychun, Jan 2012
            'Get the edit template so that the default value can be used to set the linetype
            Dim theTemplate As IEditTemplate = DirectCast(_editor.CurrentTemplate, IEditTemplate)
            'end John Prychun, Jan 2012

            Dim theSpiralDirection As String = "R"
            If Not doesCurveRight Then theSpiralDirection = "L"

            My.ArcMap.Editor.StartOperation()
            'The Geometry and fields for the first Spiral
            theSpiralFeature.Shape = DirectCast(theSpiralPolyline, IGeometry)
            If FindField("ARCLENGTH", theSpiralFeature) >= 0 Then theSpiralFeature.Value(FindField("ARCLENGTH", theSpiralFeature)) = FormatNumber(theSpiralPolyline.Length, 2).ToString
            If FindField("RADIUS", theSpiralFeature) >= 0 Then theSpiralFeature.Value(FindField("RADIUS", theSpiralFeature)) = "INFINITY"
            If FindField("RADIUS2", theSpiralFeature) >= 0 Then theSpiralFeature.Value(FindField("RADIUS2", theSpiralFeature)) = FormatNumber(theEndRadius, 2).ToString
            If FindField("SIDE", theSpiralFeature) >= 0 Then theSpiralFeature.Value(FindField("SIDE", theSpiralFeature)) = theSpiralDirection
            If FindField("DISTANCE", theSpiralFeature) >= 0 Then theSpiralFeature.Value(FindField("DISTANCE", theSpiralFeature)) = LineLength(theSpiralPolyline.FromPoint, theSpiralPolyline.ToPoint).ToString
            If FindField("DIRECTION", theSpiralFeature) >= 0 Then theSpiralFeature.Value(FindField("DIRECTION", theSpiralFeature)) = getDirection(theSpiralPolyline.FromPoint, theSpiralPolyline.ToPoint)
            If FindField("LINETYPE", theSpiralFeature) >= 0 Then theSpiralFeature.Value(FindField("LINETYPE", theSpiralFeature)) = theTemplate.DefaultValue("LINETYPE")
            theSpiralFeature.Store()
            My.ArcMap.Editor.StopOperation("Spiral Construction")

        Catch ex As Exception
            OrmapExtension.ProcessUnhandledException(ex)
        End Try

    End Sub
    Public Sub ConstructSpiralByLength(ByVal theFromPoint As IPoint, ByVal theTangentPoint As IPoint, ByVal theSpiralLength As Double, ByVal theBeginRadius As Double, ByVal theEndRadius As Double, ByVal isCCW As Boolean, ByVal theTargetLayerName As String, ByVal theCurveDensity As Double)
        If My.ArcMap.Editor.EditState = esriEditState.esriStateNotEditing Then Exit Sub

        Try
            Dim theBeginCurvature As Double = 0
            Dim theEndCurvature As Double = 0

            If theBeginRadius <> 0 Then theBeginCurvature = 1 / theBeginRadius
            If theEndRadius <> 0 Then theEndCurvature = 1 / theEndRadius

            'Consturcts and validates the polyline
            Dim theSpiralPolyLine As IPolyline6 = ConstructSpiralByLength(theFromPoint, theTangentPoint, theBeginCurvature, theEndCurvature, isCCW, theSpiralLength, theCurveDensity)
            If theSpiralPolyLine Is Nothing Then
                MessageBox.Show("Invalid Spiral Parameters.")
                Exit Sub
            End If

            Dim theTargetFeatureClass As IFeatureClass = GetTargetFeatureClass()

            'John Prychun, Jan 2012
            'Get the edit template so that the default value can be used to set the linetype
            Dim theTemplate As IEditTemplate = DirectCast(_editor.CurrentTemplate, IEditTemplate)
            'end John Prychun, Jan 2012

            Dim theSpiralFeature As IFeature = theTargetFeatureClass.CreateFeature

            Dim theSpiralDirection As String = "L"
            If Not isCCW Then theSpiralDirection = "R"

            My.ArcMap.Editor.StartOperation()
            'The Geometry and fields for the first Spiral
            theSpiralFeature.Shape = DirectCast(theSpiralPolyLine, IGeometry)
            If FindField("ARCLENGTH", theSpiralFeature) >= 0 Then theSpiralFeature.Value(FindField("ARCLENGTH", theSpiralFeature)) = FormatNumber(theSpiralLength, 2).ToString
            If FindField("RADIUS", theSpiralFeature) >= 0 Then theSpiralFeature.Value(FindField("RADIUS", theSpiralFeature)) = "INFINITY"
            If FindField("RADIUS2", theSpiralFeature) >= 0 Then theSpiralFeature.Value(FindField("RADIUS2", theSpiralFeature)) = FormatNumber(theEndRadius, 2).ToString
            If FindField("SIDE", theSpiralFeature) >= 0 Then theSpiralFeature.Value(FindField("SIDE", theSpiralFeature)) = theSpiralDirection
            If FindField("DISTANCE", theSpiralFeature) >= 0 Then theSpiralFeature.Value(FindField("DISTANCE", theSpiralFeature)) = LineLength(theSpiralPolyLine.FromPoint, theSpiralPolyLine.ToPoint).ToString
            If FindField("DIRECTION", theSpiralFeature) >= 0 Then theSpiralFeature.Value(FindField("DIRECTION", theSpiralFeature)) = getDirection(theSpiralPolyLine.FromPoint, theSpiralPolyLine.ToPoint)
            If FindField("LINETYPE", theSpiralFeature) >= 0 Then theSpiralFeature.Value(FindField("LINETYPE", theSpiralFeature)) = theTemplate.DefaultValue("LINETYPE")
            theSpiralFeature.Store()
            My.ArcMap.Editor.StopOperation("Spiral Construction")

        Catch ex As Exception
            OrmapExtension.ProcessUnhandledException(ex)
        End Try

    End Sub
    ''' <summary>
    ''' Constructs to spiral curve sprial transition
    ''' </summary>
    ''' <param name="theFromPoint">As an IPoint.  The beginning point of the spiral-curve-spiral construction</param>
    ''' <param name="theTangentPoint">As an IPoint.  The tangent point, or Point of Intersect of the tangents for the spiral-curve-spiral construction</param>
    ''' <param name="theToPoint">As an IPoint.  The end point of the spiral-curve-spiral transition</param>
    ''' <param name="theSpiralLengths"></param>
    ''' <param name="theRadius">As double, the radius of the central spiral</param>
    ''' <param name="isCCW">as boolena, is curve counter clockwise</param>
    ''' <remarks></remarks>
    Public Sub ConstructSCSByLength(ByVal theFromPoint As IPoint, ByVal theTangentPoint As IPoint, ByVal theToPoint As IPoint, ByVal theSpiralLengths As Double, ByVal theRadius As Double, ByVal isCCW As Boolean, ByVal theTargetLayerName As String, ByVal theCurveDensity As Double)
        If My.ArcMap.Editor.EditState = esriEditState.esriStateNotEditing Then Exit Sub

        Try
            Dim toCurvature As Double = 1 / theRadius
            Dim DensifyParameter As Double = 0.5

            'Constructs the spiral curves
            Dim theFirstSpiralPolyLine As IPolyline6 = ConstructSpiralByLength(theFromPoint, theTangentPoint, 0, toCurvature, isCCW, theSpiralLengths, theCurveDensity)
            If theFirstSpiralPolyLine Is Nothing Then
                MessageBox.Show("Failed to construct the first spiral" & vbNewLine & "Please check construction inputs")
                Exit Sub
            End If

            Dim theSecondSpiralisCCW As Boolean = Not isCCW

            Dim theSecondSpiralPolyLine As IPolyline6 = ConstructSpiralByLength(theToPoint, theTangentPoint, 0, toCurvature, theSecondSpiralisCCW, theSpiralLengths, theCurveDensity)
            If theSecondSpiralPolyLine Is Nothing Then
                MessageBox.Show("Failed to construct the first spiral" & vbNewLine & "Please check construction inputs")
                Exit Sub
            End If

            'Constructs the Central Curve
            Dim TheCentralCurveConstruction As IConstructCircularArc2 = New CircularArc
            Try
                TheCentralCurveConstruction.ConstructEndPointsRadius(theFirstSpiralPolyLine.ToPoint, theSecondSpiralPolyLine.ToPoint, theSecondSpiralisCCW, theRadius, True)
            Catch ex As Exception
                MessageBox.Show("Failed to construct central curve." & vbNewLine & "Please check the curve input.")
            End Try

            Dim theCentralCurve As ICurve3 = DirectCast(TheCentralCurveConstruction, ICurve3)
            Dim TheCurvePolyline As ISegmentCollection = New PolylineClass()
            TheCurvePolyline.AddSegment(DirectCast(TheCentralCurveConstruction, ISegment))

            Dim theFeatureclass As IFeatureClass = GetTargetFeatureClass()
            Dim theFirstSpiralFeature As IFeature = theFeatureclass.CreateFeature
            Dim theSecondSpiralFeature As IFeature = theFeatureclass.CreateFeature
            Dim theCenterCircularFeature As IFeature = theFeatureclass.CreateFeature

            Dim theCurveDirection As String = "R"
            If Not isCCW Then theCurveDirection = "L"

            Dim theTemplate As IEditTemplate = DirectCast(_editor.CurrentTemplate, IEditTemplate)

            'Add the new features to the feature Class
            My.ArcMap.Editor.StartOperation()
            'The Geometry and fields for the first Spiral
            theFirstSpiralFeature.Shape = DirectCast(theFirstSpiralPolyLine, IGeometry)
            If FindField("ARCLENGTH", theFirstSpiralFeature) >= 0 Then theFirstSpiralFeature.Value(FindField("ARCLENGTH", theFirstSpiralFeature)) = FormatNumber(theSpiralLengths, 2).ToString
            If FindField("RADIUS", theFirstSpiralFeature) >= 0 Then theFirstSpiralFeature.Value(FindField("RADIUS", theFirstSpiralFeature)) = "INFINITY"
            If FindField("RADIUS2", theFirstSpiralFeature) >= 0 Then theFirstSpiralFeature.Value(FindField("RADIUS2", theFirstSpiralFeature)) = FormatNumber(theRadius, 2).ToString
            If FindField("SIDE", theFirstSpiralFeature) >= 0 Then theFirstSpiralFeature.Value(FindField("SIDE", theFirstSpiralFeature)) = theCurveDirection
            If FindField("DISTANCE", theFirstSpiralFeature) >= 0 Then theFirstSpiralFeature.Value(FindField("DISTANCE", theFirstSpiralFeature)) = LineLength(theFirstSpiralPolyLine.FromPoint, theFirstSpiralPolyLine.ToPoint).ToString
            If FindField("DIRECTION", theFirstSpiralFeature) >= 0 Then theFirstSpiralFeature.Value(FindField("DIRECTION", theFirstSpiralFeature)) = getDirection(theFirstSpiralPolyLine.FromPoint, theFirstSpiralPolyLine.ToPoint)
            If FindField("LINETYPE", theFirstSpiralFeature) >= 0 Then theFirstSpiralFeature.Value(FindField("LINETYPE", theFirstSpiralFeature)) = theTemplate.DefaultValue("LINETYPE")
            theFirstSpiralFeature.Store()

            'The Geometry and fields for the Central Circular Feature
            theCenterCircularFeature.Shape = DirectCast(TheCurvePolyline, IGeometry)
            If FindField("ARCLENGTH", theCenterCircularFeature) >= 0 Then theCenterCircularFeature.Value(FindField("ARCLENGTH", theCenterCircularFeature)) = FormatNumber(theCentralCurve.Length, 2).ToString
            If FindField("RADIUS", theCenterCircularFeature) >= 0 Then theCenterCircularFeature.Value(FindField("RADIUS", theCenterCircularFeature)) = FormatNumber(theRadius, 2).ToString
            If FindField("SIDE", theCenterCircularFeature) >= 0 Then theCenterCircularFeature.Value(FindField("SIDE", theCenterCircularFeature)) = theCurveDirection
            If FindField("DISTANCE", theCenterCircularFeature) >= 0 Then theCenterCircularFeature.Value(FindField("DISTANCE", theCenterCircularFeature)) = LineLength(theCentralCurve.FromPoint, theCentralCurve.ToPoint).ToString
            If FindField("DELTA", theCenterCircularFeature) >= 0 Then theCenterCircularFeature.Value(FindField("DELTA", theCenterCircularFeature)) = getDelta(DirectCast(theCentralCurve, ICircularArc))
            If FindField("DIRECTION", theCenterCircularFeature) >= 0 Then theCenterCircularFeature.Value(FindField("DIRECTION", theCenterCircularFeature)) = getDirection(theCentralCurve.FromPoint, theCentralCurve.ToPoint)
            If FindField("LINETYPE", theFirstSpiralFeature) >= 0 Then theFirstSpiralFeature.Value(FindField("LINETYPE", theFirstSpiralFeature)) = theTemplate.DefaultValue("LINETYPE")
            theCenterCircularFeature.Store()

            'The Geometry and fileds for the second Central Circulare Feature
            theSecondSpiralFeature.Shape = DirectCast(theSecondSpiralPolyLine, IGeometry)
            If FindField("ARCLENGTH", theSecondSpiralFeature) >= 0 Then theSecondSpiralFeature.Value(FindField("ARCLENGTH", theSecondSpiralFeature)) = FormatNumber(theSpiralLengths, 2).ToString
            If FindField("RADIUS", theSecondSpiralFeature) >= 0 Then theSecondSpiralFeature.Value(FindField("RADIUS", theSecondSpiralFeature)) = "INFINITY"
            If FindField("RADIUS2", theSecondSpiralFeature) >= 0 Then theSecondSpiralFeature.Value(FindField("RADIUS2", theSecondSpiralFeature)) = FormatNumber(theRadius, 2).ToString
            If FindField("SIDE", theSecondSpiralFeature) >= 0 Then theSecondSpiralFeature.Value(FindField("SIDE", theSecondSpiralFeature)) = theCurveDirection
            If FindField("DISTANCE", theSecondSpiralFeature) >= 0 Then theSecondSpiralFeature.Value(FindField("DISTANCE", theSecondSpiralFeature)) = LineLength(theSecondSpiralPolyLine.FromPoint, theSecondSpiralPolyLine.ToPoint).ToString
            If FindField("DIRECTION", theSecondSpiralFeature) >= 0 Then theSecondSpiralFeature.Value(FindField("DIRECTION", theSecondSpiralFeature)) = getDirection(theSecondSpiralPolyLine.ToPoint, theSecondSpiralPolyLine.FromPoint)
            If FindField("LINETYPE", theSecondSpiralFeature) >= 0 Then theSecondSpiralFeature.Value(FindField("LINETYPE", theSecondSpiralFeature)) = theTemplate.DefaultValue("LINETYPE")
            theSecondSpiralFeature.Store()
            My.ArcMap.Editor.StopOperation("Spiral Construction")

        Catch ex As Exception
            OrmapExtension.ProcessUnhandledException(ex)
        End Try


    End Sub
    Function getDirection(ByVal fromPoint As IPoint, ByVal toPoint As IPoint) As String
        Dim theLineDirection As String = Nothing

        Dim theLine As ILine2 = New Line
        theLine.PutCoords(fromPoint, toPoint)

        Dim theAngle As Double = 90 - (theLine.Angle * (180 / Math.PI))

        Dim theDirectionFormat As IDirectionFormat = New DirectionFormat
        'theDirectionFormat.DirectionType = esriDirectionType.esriDTQuadrantBearing
        theDirectionFormat.DirectionUnits = esriDirectionUnits.esriDUDegreesMinutesSeconds
        theDirectionFormat.DisplayFormat = esriDirectionFormatEnum.esriDFQuadrantBearing

        Dim theNumberFormat As INumberFormat = DirectCast(theDirectionFormat, INumberFormat)

        theLineDirection = theNumberFormat.ValueToString(theAngle)

        Return theLineDirection
    End Function
    Function getDelta(ByVal TheCircularArc As ICircularArc) As String

        Dim theDelta As String
        Dim theCentralAngle As Double = TheCircularArc.CentralAngle * (180 / Math.PI)

        Dim theDirectionFormat As IDirectionFormat = New DirectionFormat
        theDirectionFormat.DirectionUnits = esriDirectionUnits.esriDUDecimalDegrees
        theDirectionFormat.DisplayFormat = esriDirectionFormatEnum.esriDFDegreesMinutesSeconds

        Dim theNumberFormat As INumberFormat = DirectCast(theDirectionFormat, INumberFormat)
        theDelta = theNumberFormat.ValueToString(theCentralAngle)
        theDelta = Left(theDelta, Len(theDelta) - 1)
        theDelta = Replace(theDelta, "'", "-")
        theDelta = Replace(theDelta, "°", "-")

        Return theDelta
    End Function
    Function LineLength(ByVal fromPoint As IPoint, ByVal toPoint As IPoint) As Double
        Dim theLength As Double
        Dim theLine As ILine2 = New Line
        theLine.PutCoords(fromPoint, toPoint)
        theLength = CDbl(FormatNumber(theLine.Length, 2))
        Return theLength
    End Function
    ''' <summary>
    ''' Finds the field index for a feature class
    ''' </summary>
    ''' <param name="theFieldName"></param>
    ''' <returns>The index value for the field</returns>
    ''' <remarks></remarks>
    Function FindField(ByVal theFieldName As String, ByVal theFeature As IFeature) As Integer
        Dim theFieldIndex As Integer
        theFieldIndex = theFeature.Fields.FindField(theFieldName)
        Return theFieldIndex
    End Function
    ''' <summary>
    ''' Creates the circle graphic showing where the cursor is snapping to in regards to getting the point inputs.
    ''' </summary>
    ''' <returns>as graphic marker element</returns>
    ''' <remarks></remarks>
    Public Function CreateSnapMarker() As IMarkerElement
        Dim TheMarkerElement As IMarkerElement = New MarkerElement
        Dim theMarkerSymbol As ICharacterMarkerSymbol = New CharacterMarkerSymbol
        Dim theSnapFont As stdole.IFontDisp = DirectCast(New stdole.StdFont, stdole.IFontDisp)

        With theSnapFont
            .Name = "ESRI Default Marker"
            .Size = My.ArcMap.Document.SearchTolerancePixels
        End With

        With theMarkerSymbol
            .Font = theSnapFont
            .CharacterIndex = 40
        End With

        TheMarkerElement.Symbol = theMarkerSymbol

        Return TheMarkerElement
    End Function
    ''' <summary>
    ''' checks to see if the passed feature class is type of polyline
    ''' </summary>
    ''' <param name="theFeatureClass">an featureclass object</param>
    ''' <returns>true or false</returns>
    ''' <remarks></remarks>
    Public Function IsFeatureClassValidPolyline(ByVal theFeatureClass As IFeatureClass) As Boolean
        Dim isValid As Boolean = False
        If theFeatureClass.ShapeType = esriGeometryType.esriGeometryPolyline Then
            isValid = True
        End If
        Return isValid
    End Function

    ''' <summary>
    '''  Creates the spiral Construction by Length
    ''' </summary>
    ''' <param name="theFromPoint"></param>
    ''' <param name="theTangentpoint"></param>
    ''' <param name="theFromCurvature"></param>
    ''' <param name="theToCurvature"></param>
    ''' <param name="isCCW"></param>
    ''' <param name="theSpiralLength"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ConstructSpiralByLength(ByVal theFromPoint As IPoint, ByVal theTangentpoint As IPoint, ByRef theFromCurvature As Double, ByRef theToCurvature As Double, ByVal isCCW As Boolean, ByVal theSpiralLength As Double, ByVal theCurveDensity As Double) As IPolyline6
        Dim thePolyLine As IPolyline6 = DirectCast(New Polyline, IPolyline6)

        Try
            Dim theGeometryEnvironment As IGeometryEnvironment4 = New GeometryEnvironment
            Dim TheSpiralConstruction As IConstructClothoid = DirectCast(theGeometryEnvironment, IConstructClothoid)
            thePolyLine = DirectCast(TheSpiralConstruction.ConstructClothoidByLength(theFromPoint, theTangentpoint, isCCW, theFromCurvature, theToCurvature, theSpiralLength, esriCurveDensifyMethod.esriCurveDensifyByLength, theCurveDensity), IPolyline6)
        Catch ex As Exception
            thePolyLine = Nothing
        End Try

        Return thePolyLine
    End Function


    ''' <summary>
    ''' Takes a Quadrant angle and converts to Decimal Degrees
    ''' </summary>
    ''' <param name="TheCircularDegree"></param>
    ''' <returns>A double value.</returns>
    ''' <remarks>If 0 is returned then an invalid value was sent.</remarks>
    Public Function DMSAngleToDouble(ByVal TheCircularDegree As String) As Double

        Try
            Dim theNewValue As Double = 0

            Dim theDirectionFormat As IDirectionFormat = New DirectionFormat
            Dim theNumberFormat As INumberFormat = DirectCast(theDirectionFormat, INumberFormat)

            theDirectionFormat.DirectionUnits = esriDirectionUnits.esriDUDegreesMinutesSeconds
            theNewValue = theNumberFormat.StringToValue(TheCircularDegree)

            Return theNewValue
        Catch ex As Exception
            OrmapExtension.ProcessUnhandledException(ex)
        End Try


    End Function
   
    Public Sub OpenHelp(ByVal helpFormText As String, ByVal theRTFStream As Stream)

        ' Get the help form.
        Dim theHelpForm As New HelpForm
        theHelpForm.Text = helpFormText

        If Not (theRTFStream Is Nothing) Then
            Dim sr As New StreamReader(theRTFStream)
            theHelpForm.uxRichTextBox.LoadFile(theRTFStream, RichTextBoxStreamType.RichText)
            sr.Close()
        End If

        If helpFormText = "Report Bug or Request New Feature" Then
            theHelpForm.uxReportOrRequest.Visible = False
        End If

        theHelpForm.Show()

    End Sub
End Module

