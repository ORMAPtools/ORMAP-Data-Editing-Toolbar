#Region "Copyright 2008-2011 ORMAP Tech Group"

' File:  AnnotationUtilities.vb
'
' Original Author:  Robert Gumtow
'
' Date Created:  June 9, 2010
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
Imports System.Windows.Forms
Imports System.Environment
Imports System.Globalization
Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.Display

'Imports OrmapTaxlotEditing.SpatialUtilities
'Imports OrmapTaxlotEditing.EditorExtension
'Imports OrmapTaxlotEditing.Utilities

#End Region

#Region "Class Declaration"
''' <summary>
''' Annotation utilities class (static).
''' </summary>
''' <remarks>
''' <para>Commonly used annotation methods and functions.</para>
''' </remarks>
Public NotInheritable Class AnnotationUtilities

#Region "Class-Level Constants and Enumerations"
    ''' <summary>
    ''' Set to "34" (ORMAP standard name for Bearing and Distance annotation)
    ''' </summary>
    ''' <remarks>Needed for the CreateAnnotation class to perform correctly</remarks>
    Public Const AnnotationClassName As String = "34"

    ''' <summary>
    ''' The value 3.1415926535897931
    ''' </summary>
    ''' <remarks></remarks>
    Public Const Pi As Double = 3.1415926535897931

    ''' <summary>
    ''' Default distance for wide-line spacing (60 ft)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const WideLine As Integer = 60

    ''' <summary>
    ''' Annotation pair distance constant 2.55
    ''' </summary>
    ''' <remarks>
    '''calculated from mean distances of annotation pairs
    '''placed by the Label-to-Annotation converter. When multiplied
    '''by the appropriate font, will maintain distances between
    '''the annotation pairs 
    '''</remarks>
    Public Const DistanceBothSides As Double = 2.55

    ''' <summary>
    ''' Annotation pair distance constant 1.75
    ''' </summary>
    ''' <remarks>
    '''calculated from mean distances of annotation pairs
    '''placed by the Label-to-Annotation converter. When multiplied
    '''by the appropriate font, will maintain distances between
    '''the annotation pairs 
    '''</remarks>
    Public Const DistanceBothAbove As Double = 1.75

    ''' <summary>
    ''' Annotation pair distance constant 1.99
    ''' </summary>
    ''' <remarks>
    '''calculated from mean distances of annotation pairs
    '''placed by the Label-to-Annotation converter. When multiplied
    '''by the appropriate font, will maintain distances between
    '''the annotation pairs 
    '''</remarks>
    Public Const DistanceBothBelow As Double = 1.99

    ''' <summary>
    ''' Defines how annotation was originally placed related to its source line
    ''' </summary>
    ''' <remarks>Used to control where annotation is placed relative to the source line</remarks>
    Public Enum AnnotationPlacement As Integer
        ''' <summary>
        ''' Annotation placed on both sides of the line with standard spacing
        ''' </summary>
        ''' <remarks></remarks>
        BothSides
        ''' <summary>
        ''' Annotation placed on both sides of the line with wide spacing
        ''' </summary>
        ''' <remarks></remarks>
        BothSidesWide
        ''' <summary>
        ''' Annotation placed above the line
        ''' </summary>
        ''' <remarks></remarks>
        BothAbove
        ''' <summary>
        ''' Annotation placed below the line
        ''' </summary>
        ''' <remarks></remarks>
        BothBelow
    End Enum

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

#Region "Public Members"

    ''' <summary>
    ''' Gets the name of the Annotation Feature Class based on the Map Scale.
    ''' </summary>
    ''' <param name="theMapScale">The Map Scale from which to determine the Annotation Feature Class name.</param>
    ''' <returns>The Annotation Feature Class name as a string.</returns>
    ''' <remarks>
    ''' Uses ORMAP Settings to find name of appropriate annotation feature class.
    ''' NOTE=> This is identical method to SpatialUtilities.getAnnoSizeByScale which
    ''' couldn't be used because is private to that class.
    ''' </remarks>
    Public Shared Function GetAnnoFeatureClassName(ByVal theMapScale As String) As String
        'TODO: (RG) Need to put Case Else and exception handler in here
        Dim theAnnoFCName As String = String.Empty
        Select Case CInt(theMapScale)
            Case 120
                theAnnoFCName = OrmapExtension.AnnoTableNamesSettings.Anno0010scaleFC
            Case 240
                theAnnoFCName = OrmapExtension.AnnoTableNamesSettings.Anno0020scaleFC
            Case 360
                theAnnoFCName = OrmapExtension.AnnoTableNamesSettings.Anno0030scaleFC
            Case 480
                theAnnoFCName = OrmapExtension.AnnoTableNamesSettings.Anno0040scaleFC
            Case 600
                theAnnoFCName = OrmapExtension.AnnoTableNamesSettings.Anno0050scaleFC
            Case 1200
                theAnnoFCName = OrmapExtension.AnnoTableNamesSettings.Anno0100scaleFC
            Case 2400
                theAnnoFCName = OrmapExtension.AnnoTableNamesSettings.Anno0200scaleFC
            Case 4800
                theAnnoFCName = OrmapExtension.AnnoTableNamesSettings.Anno0400scaleFC
            Case 9600
                theAnnoFCName = OrmapExtension.AnnoTableNamesSettings.Anno0800scaleFC
            Case 24000
                theAnnoFCName = OrmapExtension.AnnoTableNamesSettings.Anno2000scaleFC
        End Select
        Return theAnnoFCName

    End Function

    ''' <summary>
    ''' Gets the Subtype Code for the desired Subtype name. 
    ''' </summary>
    ''' <param name="theAnnoFeatureClass">Name of the field to draw the domain from.</param>
    ''' <param name="theSubtypeName">Name of the Subtype for which the Subtype Code is needed.</param>
    ''' <param name="stringLength">Optional string length of theSubtypeName.</param>
    ''' <returns>An Symbol Id as an integer.</returns>
    ''' <remarks>
    ''' String length can be used to extract only a portion of the subtype name. For example, a name of 
    ''' "34- Distance and Bearing" could be sent with a length of 2, and therefore only "34" would
    ''' be evaluated.
    ''' </remarks>
    Public Shared Function GetSubtypeCode(ByVal theAnnoFeatureClass As IFeatureClass, ByVal theSubtypeName As String, _
                                          Optional ByVal stringLength As Integer = 0) As Integer
        'Cannot use SpatialUtilities.ConvertCodeValueDomainToCode here since this is an annotation subtype, and not a domain.
        Dim theReturnValue As Integer = Nothing
        If stringLength = 0 Then
            stringLength = Len(theSubtypeName)
        End If

        'TODO:  (RG) Need exception handling
        Dim theSubtypes As ISubtypes = DirectCast(theAnnoFeatureClass, ISubtypes)
        Dim theEnumSubtypes As IEnumSubtype = theSubtypes.Subtypes
        Dim thisSubtypeName As String
        Dim thisSubtypeCode As Integer
        theEnumSubtypes.Reset()
        Do Until theEnumSubtypes Is Nothing
            thisSubtypeName = theEnumSubtypes.Next(thisSubtypeCode)
            If String.Compare(thisSubtypeName, 0, theSubtypeName, 0, stringLength, CultureInfo.CurrentCulture, CompareOptions.IgnoreCase) = 0 Then
                theReturnValue = thisSubtypeCode
                Exit Do
            End If
        Loop
        Return theReturnValue
    End Function

    ''' <summary>
    ''' Gets the Symbol ID value based on the Symbol Name
    ''' </summary>
    ''' <param name="theAnnoFeatureClass">The Annotation Feature Class containing the appropriate symbol set.</param>
    ''' <param name="theSymbolName">The name of the symbol for which the symbol id needed.</param>
    ''' <param name="stringLength">Optional string length of theSubtypeName.</param>
    ''' <returns>The Symbol Id as an integer.</returns>
    ''' <remarks>
    ''' String length can be used to extract only a portion of the subtype name. For example, a name of 
    ''' "34- Distance and Bearing" could be sent with a length of 2, and therefore only "34" would
    ''' be evaluated.
    ''' </remarks>
    Public Shared Function GetSymbolId(ByVal theAnnoFeatureClass As IFeatureClass, ByVal theSymbolName As String, _
                                       Optional ByVal stringLength As Integer = 0) As Integer
        'TODO:  (RG) Need exception handling
        Dim theSymbolCollection As ISymbolCollection2 = New SymbolCollectionClass()
        Dim thisSymbolIdentifier As ISymbolIdentifier2 = New SymbolIdentifierClass()
        Dim theAnnoClass As IAnnoClass
        Dim theReturnValue As Integer = -1
        If stringLength = 0 Then
            stringLength = Len(theSymbolName)
        End If

        '------------------------------------------
        'Get the Symbol ID 
        '------------------------------------------
        'Defined in the anno feature class for Symbol Name  
        theAnnoClass = DirectCast(theAnnoFeatureClass.Extension, IAnnoClass)
        theSymbolCollection = DirectCast(theAnnoClass.SymbolCollection, ISymbolCollection2)
        theSymbolCollection.Reset()
        thisSymbolIdentifier = DirectCast(theSymbolCollection.Next, ISymbolIdentifier2)
        Do Until thisSymbolIdentifier Is Nothing
            If String.Compare(thisSymbolIdentifier.Name, 0, theSymbolName, 0, stringLength, True, CultureInfo.CurrentCulture) = 0 Then
                theReturnValue = thisSymbolIdentifier.ID
                Exit Do
            End If
            thisSymbolIdentifier = DirectCast(theSymbolCollection.Next, ISymbolIdentifier2)
        Loop
        Return theReturnValue
    End Function

    ''' <summary>
    ''' Gets the Annotation Feature Class based on the name.
    ''' </summary>
    ''' <param name="theAnnoFCName">The name of the desired Annotation Feature Class.</param>
    ''' <returns>The feature class with the specified name.</returns>
    ''' <remarks></remarks>
    Public Shared Function GetAnnoFeatureClass(ByVal theAnnoFCName As String) As IFeatureClass
        Dim theAnnoEnumLayer As IEnumLayer = SpatialUtilities.GetTOCLayersEnumerator(Utilities.EsriLayerTypes.FDOGraphicsLayer)
        Dim thisAnnoFeatureClass As IFeatureClass = Nothing
        Dim thisAnnoLayer As IFeatureLayer
        thisAnnoLayer = DirectCast(theAnnoEnumLayer.Next, IFeatureLayer)

        '------------------------------------------
        'Find anno feature class (based on name)
        '------------------------------------------
        Do While Not (thisAnnoLayer Is Nothing)
            If String.Compare(thisAnnoLayer.Name, theAnnoFCName, True, CultureInfo.CurrentCulture) = 0 Then
                thisAnnoFeatureClass = thisAnnoLayer.FeatureClass
                Exit Do
            End If
            thisAnnoLayer = DirectCast(theAnnoEnumLayer.Next, IFeatureLayer)
        Loop


        Return thisAnnoFeatureClass
    End Function

    ''' <summary>
    ''' Moves an annotation element based on its TO and FROM point values
    ''' </summary>
    ''' <param name="theToPoint">The point to be used as the "to" point of the move vector.</param>
    ''' <param name="theFromPoint">The point to be used as the "from" point of the move vector.</param>
    ''' <param name="theGraphicsContainer">The graphics container which holds the annotation element.</param>
    ''' <param name="theElement">The annotation element to be moved.</param>
    Public Overloads Shared Sub MoveElement(ByVal theToPoint As IPoint, ByVal theFromPoint As IPoint, ByVal theGraphicsContainer As IGraphicsContainer, ByVal theElement As IElement)
        '------------------------------------------
        'Create a vector line between the two points 
        '------------------------------------------
        Dim theMoveVector As ILine = New ESRI.ArcGIS.Geometry.Line
        theMoveVector.PutCoords(theToPoint, theFromPoint)

        '------------------------------------------
        'Call the overloaded MoveElement function with the vector
        '------------------------------------------
        MoveElement(theMoveVector, theGraphicsContainer, theElement)

    End Sub

    ''' <summary>
    ''' Moves an annotation element based on a move vector
    ''' </summary>
    ''' <param name="theMoveVector">The move vector which defines the element's movement.</param>
    ''' <param name="theGraphicsContainer">The graphics container which holds the annotation element.</param>
    ''' <param name="theElement">The annotation element to be moved.</param>
    Public Overloads Shared Sub MoveElement(ByVal theMoveVector As ILine, ByVal theGraphicsContainer As IGraphicsContainer, ByVal theElement As IElement)
        '------------------------------------------
        'Move the annotation using ITansform2D
        '------------------------------------------
        Dim transform2D As ITransform2D = DirectCast(theElement, ITransform2D)
        transform2D.MoveVector(theMoveVector)

        '------------------------------------------
        'Transformation complete
        '------------------------------------------
        'Update the source objects with the new geometry
        finishTransform(theGraphicsContainer, theElement, transform2D)

    End Sub

    ''' <summary>
    ''' Rotates an annotation element around a specified point.
    ''' </summary>
    ''' <param name="theRotationPoint">The about which the annotation element will be rotated.</param>
    ''' <param name="theGraphicsContainer">The graphics container which holds the annotation element.</param>
    ''' <param name="theElement">The annotation element to be rotated.</param>
    ''' <remarks>Rotates the annotation element 180 degrees (pi) around a specified point.</remarks>
    Public Shared Sub RotateElement(ByVal theRotationPoint As IPoint, ByVal theGraphicsContainer As IGraphicsContainer, ByVal theElement As IElement)
        '------------------------------------------
        'Rotate the annotation 180 degrees (pi)
        '------------------------------------------
        Dim transform2D As ITransform2D = DirectCast(theElement, ITransform2D)
        transform2D.Rotate(theRotationPoint, Pi)

        '------------------------------------------
        'Transformation complete, now update the source objects with the new geometry
        '------------------------------------------
        finishTransform(theGraphicsContainer, theElement, transform2D)

    End Sub

    ''' <summary>
    ''' Gets the annotation envelop. 
    ''' </summary>
    ''' <param name="theDisplay">The current map display.</param>
    ''' <param name="theElement">The annotation element for which the envelop is returned.</param>
    ''' <returns>The annotation element's graphic envelope.</returns>
    Public Shared Function GetAnnoEnvelope(ByVal theDisplay As IDisplay, ByVal theElement As IElement) As IEnvelope
        Dim theBoundaryEnvelope As IEnvelope = New Envelope
        theElement.QueryBounds(theDisplay, theBoundaryEnvelope)
        Return theBoundaryEnvelope
    End Function

    ''' <summary>
    ''' Moves annotation elements based on boolean settings
    ''' </summary>
    ''' <param name="isInverted">Boolean value to determine if annotation should be inverted (rotated)</param>
    ''' <param name="isTransposed">Boolean value to determine if annotation should be transposed (flipped)</param>
    ''' <param name="isMoveUp">Boolean value to determine if annotation should be moved "up"</param>
    ''' <param name="isMoveDown">Boolean value to determine if annotation should be moved "down"</param>
    ''' <param name="isStandardSpace">Boolean value to determine if annotation has standard space</param>
    ''' <param name="isWideSpace">Boolean value to determine if annotation has wide space</param>
    ''' <remarks>
    ''' <para>
    ''' Annotation movement only works for annotation created and placed by the CreateAnnotation class. This class
    ''' places annotation based on a set of rules which comply with annotation spacing, fonts, and font sizes used
    ''' in Polk County taxmaps.
    ''' 
    ''' Moving distance and bearing annotation is accomplished by describing mathematical relationships relating to a "virtual"  
    ''' line (the taxlot line). Distances between the "upper" and "lower" annotation varies depending on its relative location 
    ''' to this line. Annotation on different sides of the line, both above the line, or both below the line have different 
    ''' distances between them. These distances are calculated relative to the centroid of the annotation's envelopes, and are 
    ''' dependent on font size. These distances also vary if the line is "standard" or "big" (wide). Annotation may be placed 
    ''' with wide spacing if it will be located next to map index boundaries, taxcode lines, subdivision boundaries, etc. 
    '''
    ''' Constants were calculated by checking envelope centroid distances for a number of annotation sets of different scales 
    ''' placed above, below, and on both sides of the line at 100, 200, and 400 scales. These constants were calculated 
    ''' from the formula:
    '''</para>
    ''' <code lang="text">
    ''' 
    '''     K = Distance / FontSize
    '''
    ''' and work out to the following values:
    '''
    '''         Anno Location           K
    '''         --------------         ----
    '''         Both Sides             2.55   
    '''         Both Above             1.75
    '''         Both Below             1.99
    ''' </code>
    '''</remarks>
    Public Shared Sub MoveAnnotationElements(ByVal isInverted As Boolean, ByVal isTransposed As Boolean, ByVal isMoveUp As Boolean, _
                                                ByVal isMoveDown As Boolean, ByVal isStandardSpace As Boolean, ByVal isWideSpace As Boolean)
        '------------------------------------------
        ' -- DESIGN COMMENT --
        '   NOTE=>  Annotation movement only works for annotation created and placed by the CreateAnnotation class. This class 
        '           places annotation based on a set of rules which comply with annotation spacing, fonts, and font sizes used
        '           in Polk County taxmaps. 
        '
        '   Moving distance and bearing annotation is accomplished by describing mathematical relationships relating to a "virtual"  
        '   line (the taxlot line). Distances between the "upper" and "lower" annotation varies depending on its relative location 
        '   to this line. Annotation on different sides of the line, both above the line, or both below the line have different 
        '   distances between them. These distances are calculated relative to the centroid of the annotation's envelopes, and are 
        '   dependent on font size. These distances also vary if the line is "standard" or "big" (wide). Annotation may be placed 
        '   with wide spacing if it will be located next to map index boundaries, taxcode lines, subdivision boundaries, etc. 
        '
        '   Constants were calculated by checking envelope centroid distances for a number of annotation sets of different scales 
        '   placed above, below, and on both sides of the line at 100, 200, and 400 scales. These constants were calculated 
        '   from the formula:
        '
        '       K = Distance / FontSize
        '
        '   and work out to the following values:
        '
        '           Anno Location           K
        '           --------------         ----
        '           Both Sides             2.55   
        '           Both Above             1.75
        '           Both Below             1.99
        '
        '------------------------------------------

        Dim theMxDoc As IMxDocument
        Dim theMap As IMap
        theMxDoc = My.Document
        theMap = theMxDoc.FocusMap

        Dim theActiveView As IActiveView = DirectCast(theMap, IActiveView)
        Dim theMapExtent As IEnvelope = theActiveView.Extent

        DataMonitor.CheckValidMapIndexDataProperties()

        Dim theFeatureCollection As Collection = New Collection
        Dim theEnumFeature As IEnumFeature = CType(theMap.FeatureSelection, IEnumFeature)
        Dim theEnumFeatureSetup As IEnumFeatureSetup = CType(theEnumFeature, IEnumFeatureSetup)
        theEnumFeatureSetup.AllFields = True
        Dim thisFeature As IFeature = theEnumFeature.Next
        Do While Not thisFeature Is Nothing
            theFeatureCollection.Add(thisFeature)
            thisFeature = theEnumFeature.Next
        Loop
        If theFeatureCollection.Count Mod 2 > 0 Then
            MessageBox.Show("Cannot Move Annotation: Odd number of annotation items selected... " & NewLine & _
                            "This tool works with pairs of annotation, so you must select them in " & NewLine & _
                            "sets of two (within each annotation feature class).", _
                            "Move Annotation", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Exit Sub
        End If
        Dim theTopSelectedFeature As IFeature
        Dim theBottomSelectedFeature As IFeature


        If theFeatureCollection.Count > 0 Then

            '------------------------------------------
            'Get the graphics container 
            '------------------------------------------
            'Without this, the transform2D will transform the envelope, but NOT the annotation itself!!!!
            Dim theGraphicsContainer As IGraphicsContainer = DirectCast(theMxDoc.ActivatedView, IGraphicsContainer)

            '------------------------------------------
            'Now loop through the feature collection
            '------------------------------------------
            'Since actions work on pairs of anno, double i on each iteration
            Dim i As Integer
            For i = 1 To theFeatureCollection.Count - 1 Step 2
                '------------------------------------------
                'Load the annotation feature pairs
                '------------------------------------------
                'Needed for the transform2d since it accesses the underlying geometry of the annotation
                theTopSelectedFeature = CType(theFeatureCollection.Item(i), IFeature)
                theBottomSelectedFeature = CType(theFeatureCollection.Item(i + 1), IFeature)

                Dim theAnnoFeatureClass As IFeatureClass = GetAnnoFeatureClass(theTopSelectedFeature.Class.AliasName)
                My.Editor.StartOperation()

                If theTopSelectedFeature.OID = theBottomSelectedFeature.OID Then
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    ' -- DESIGN COMMENT --
                    ' There appears to be an ESRI bug here when some of the anno classes are turned off in 
                    ' the TOC... The selection set has multiple item pairs, but OID doesn't change when it hits 
                    ' theSelectedAnnoCursor.NextFeature. Thus the envelope means and thus distance are the same.
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    MessageBox.Show("Cannot Move Annotation: ESRI BUG- All Annotation Classes MUST " & NewLine & _
                                    "be turned on in the TOC to bypass this bug. If this does not resolve" & NewLine & _
                                    "the issue, use [Editor > Options  > ORMAP Taxlot Editor and click" & NewLine & _
                                    "[ Report Bug or Request New Feature ].", _
                                    "Move Annotation", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                    My.Editor.AbortOperation()
                    theActiveView.Refresh()
                    Exit Sub
                End If

                '------------------------------------------
                'Get the selected annotation features annotation elements 
                '------------------------------------------
                'Needed to do transform and update the graphics container
                Dim theTopAnnoFeature As IAnnotationFeature = DirectCast(theTopSelectedFeature, IAnnotationFeature)
                Dim theBottomAnnoFeature As IAnnotationFeature = DirectCast(theBottomSelectedFeature, IAnnotationFeature)

                If isInverted Then
                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    ' -- DESIGN COMMENT --
                    '   The envelope used to calculate the rotation point (centroid) must be retrieved from an element's QueryBounds method. 
                    '   This method takes into account the annotation's text area, whereas the element's envelope is a polyline. Using
                    '   its centroid will actually offset the location of the annotation since the polyline's MaxY is just beneath the text.
                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '------------------------------------------
                    'To invert the annotation as a pair, must rotate each element then transpose them
                    '------------------------------------------
                    RotateElement(SpatialUtilities.GetCenterOfEnvelope(GetAnnoEnvelope(theActiveView.ScreenDisplay, theTopAnnoFeature.Annotation)), _
                                    theGraphicsContainer, theTopAnnoFeature.Annotation)
                    RotateElement(SpatialUtilities.GetCenterOfEnvelope(GetAnnoEnvelope(theActiveView.ScreenDisplay, theBottomAnnoFeature.Annotation)), _
                                    theGraphicsContainer, theBottomAnnoFeature.Annotation)
                End If

                '------------------------------------------
                'Calculate a move vector  
                '------------------------------------------
                'from the X, Y pairs of the 1st and 2nd element's envelopes (mean of lower left & upper right corners)
                Dim theMoveVector As ILine = New ESRI.ArcGIS.Geometry.Line

                theMoveVector.PutCoords(SpatialUtilities.GetCenterOfEnvelope(GetAnnoEnvelope(theActiveView.ScreenDisplay, theTopAnnoFeature.Annotation)), _
                                        SpatialUtilities.GetCenterOfEnvelope(GetAnnoEnvelope(theActiveView.ScreenDisplay, theBottomAnnoFeature.Annotation)))

                Dim theTextElement As ITextElement = DirectCast(theTopAnnoFeature.Annotation, ITextElement)
                Dim theAnnoPlacement As AnnotationPlacement = GetAnnoPlacement(theTextElement.Symbol.Size, theMoveVector.Length)
                Try
                    If theAnnoPlacement = -1 And Not isTransposed Then
                        MessageBox.Show("Cannot Move Annotation: A pair of annotation is at non-standard" & NewLine & _
                                        "placement (was not created by Create Annotation tool or has" & NewLine & _
                                        "been moved). Movement of this annotation pair will be skipped.", _
                                        "Move Annotation", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                        Exit Try
                    End If
                    Dim theToDistance As Double
                    Dim theNewVector As ILine = New ESRI.ArcGIS.Geometry.Line

                    'NOTE=> All movement is from theMoveVector's 'From' end to its 'To' end, so resized vectors utilize
                    '       the esriExtendAtFrom constant with a recalculated 'To' point in the QueryTangent method

                    '------------------------------------------
                    'Move the element
                    '------------------------------------------
                    'Each action below is exclusionary, so placed in nested If-Then-ElseIf blocks
                    If isTransposed Then
                        MoveElement(theMoveVector, theGraphicsContainer, theTopAnnoFeature.Annotation)
                        theMoveVector.ReverseOrientation()
                        MoveElement(theMoveVector, theGraphicsContainer, theBottomAnnoFeature.Annotation)
                    ElseIf isMoveDown Then
                        MoveElement(theMoveVector, theGraphicsContainer, theTopAnnoFeature.Annotation)
                        If isStandardSpace Then
                            theToDistance = DistanceBothSides * theTextElement.Symbol.Size
                            theMoveVector.QueryTangent(esriSegmentExtension.esriExtendAtFrom, 0, False, theToDistance, theNewVector)
                            theMoveVector = theNewVector
                        ElseIf isWideSpace Then
                            theToDistance = DistanceBothSides * theTextElement.Symbol.Size + WideLine
                            theMoveVector.QueryTangent(esriSegmentExtension.esriExtendAtFrom, 0, False, theToDistance, theNewVector)
                            theMoveVector = theNewVector
                        ElseIf theAnnoPlacement = AnnotationPlacement.BothSides Or theAnnoPlacement = AnnotationPlacement.BothSidesWide Then
                            theToDistance = DistanceBothBelow * theTextElement.Symbol.Size
                            theMoveVector.QueryTangent(esriSegmentExtension.esriExtendAtFrom, 0, False, theToDistance, theNewVector)
                            theMoveVector = theNewVector
                        End If
                        MoveElement(theMoveVector, theGraphicsContainer, theBottomAnnoFeature.Annotation)
                    ElseIf isMoveUp Then
                        theMoveVector.ReverseOrientation()
                        MoveElement(theMoveVector, theGraphicsContainer, theBottomAnnoFeature.Annotation)
                        If isStandardSpace Then
                            theToDistance = DistanceBothSides * theTextElement.Symbol.Size
                            theMoveVector.QueryTangent(esriSegmentExtension.esriExtendAtFrom, 0, False, theToDistance, theNewVector)
                            theMoveVector = theNewVector
                        ElseIf isWideSpace Then
                            theToDistance = DistanceBothSides * theTextElement.Symbol.Size + WideLine
                            theMoveVector.QueryTangent(esriSegmentExtension.esriExtendAtFrom, 0, False, theToDistance, theNewVector)
                            theMoveVector = theNewVector
                        ElseIf theAnnoPlacement = AnnotationPlacement.BothSides Or theAnnoPlacement = AnnotationPlacement.BothSidesWide Then
                            theToDistance = DistanceBothAbove * theTextElement.Symbol.Size
                            theMoveVector.QueryTangent(esriSegmentExtension.esriExtendAtFrom, 0, False, theToDistance, theNewVector)
                            theMoveVector = theNewVector
                        End If
                        MoveElement(theMoveVector, theGraphicsContainer, theTopAnnoFeature.Annotation)
                    End If
                Finally
                    '------------------------------------------
                    'Label and stop the edit operation
                    '------------------------------------------
                    If isInverted Then
                        My.Editor.StopOperation("Rotate Annotation")
                    ElseIf isTransposed Then
                        My.Editor.StopOperation("Flip Annotation")
                    ElseIf isMoveUp Then
                        My.Editor.StopOperation("Move Annotation Up")
                    ElseIf isMoveDown Then
                        My.Editor.StopOperation("Move Annotation Down")
                    End If
                End Try
            Next
            theActiveView.Refresh()
        End If
    End Sub

    ''' <summary>
    ''' Sets enumeration value based on how annotation was originally placed relative to its source line
    ''' </summary>
    ''' <param name="theFontSize">The font size of the associated annotation feature class</param>
    ''' <param name="theCalculatedDistance">Scale constant for associated annotation feature class</param>
    ''' <returns>The AnnotationPlacement enumeration</returns>
    Public Shared Function GetAnnoPlacement(ByVal theFontSize As Double, ByVal theCalculatedDistance As Double) As AnnotationPlacement
        Dim thisAnnoPlacement As AnnotationPlacement
        '------------------------------------------
        ' Checks annotation placement 
        '------------------------------------------
        ' Based on the scale constants and the font size for the annotation feature class to determine
        ' which type of anno is being moved (both sides, both above, etc.). Uses a "fuzzy" tolerance
        ' of plus or minus 0.01 to compensate for round-off errors.
        Dim thisPlacementRatio As Double
        Dim theUpperBound As Double
        Dim theLowerBound As Double
        If theCalculatedDistance - WideLine < 0 Then
            thisPlacementRatio = calculateAnnoSpacing(theCalculatedDistance, theFontSize)
        Else
            thisPlacementRatio = calculateAnnoSpacing(theCalculatedDistance - WideLine, theFontSize)
        End If

        theUpperBound = thisPlacementRatio + 0.01
        theLowerBound = thisPlacementRatio - 0.01

        'If distance constants fall between the upper and lower boundaries, then set appropriate anno placement value 
        If DistanceBothSides <= theUpperBound And DistanceBothSides >= theLowerBound Then
            thisAnnoPlacement = AnnotationPlacement.BothSides
        ElseIf DistanceBothAbove <= theUpperBound And DistanceBothAbove >= theLowerBound Then
            thisAnnoPlacement = AnnotationPlacement.BothAbove
        ElseIf DistanceBothBelow <= theUpperBound And DistanceBothBelow >= theLowerBound Then
            thisAnnoPlacement = AnnotationPlacement.BothBelow
        ElseIf DistanceBothSides <= theUpperBound And DistanceBothSides >= theLowerBound Then
            thisAnnoPlacement = AnnotationPlacement.BothSidesWide
        Else
            thisAnnoPlacement = DirectCast(-1, AnnotationPlacement)
        End If
        Return thisAnnoPlacement
    End Function

    ''' <summary>
    ''' Planned for future version
    ''' </summary>
    ''' <param name="isStandardSpace">Boolean for identifying standard spacing</param>
    ''' <param name="isBothSides">Boolean for identifying annotation placement relative to the line</param>
    ''' <param name="theTextElement">Annotation text element</param>
    ''' <returns>The distance annotation should be moved</returns>
    ''' <remarks>
    ''' Unused. May be included in future release for moving annotation when the user is relying on 
    ''' asymmetric offsets (top or bottom only)
    ''' </remarks>
    Public Shared Function GetMoveDistance(ByVal isStandardSpace As Boolean, ByVal isBothSides As Boolean, ByVal theTextElement As ITextElement) As Double
        '------------------------------------------
        'CODE IS CURRENTLY UNUSED... May be needed later
        '------------------------------------------
        'This code was written to handle moving annotation when cartographer is using asymmetric offsets (top or bottom only)
        'and may be needed for the next phase of the OrmapAnnotation project

        '!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        '!!!  NOTE=> Code NOT COMPLETE  !!!
        '!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        Dim theLineWidth As Double = New Double

        'Polk County uses Font Sizes = MapScale/240
        'The cartographers use an additional map unit offset for "big" lines 
        '    100     3
        '    200     6
        '    400    12
        '    800    24
        '   1000    48
        '   2000    60
        If isStandardSpace Then
            If isBothSides Then
                theLineWidth = theTextElement.Symbol.Size * 2
            ElseIf Not isBothSides Then
                theLineWidth = theTextElement.Symbol.Size * 1.5
            End If
        ElseIf Not isStandardSpace Then
            Dim theScaleOffset As Integer = CInt(theTextElement.Symbol.Size Mod 1.5)
            If isBothSides Then
                theLineWidth = theTextElement.Symbol.Size * 2 + theScaleOffset
            ElseIf Not isBothSides Then
                theLineWidth = theTextElement.Symbol.Size * 1.5 + theScaleOffset
            End If
        End If
        Return theLineWidth
    End Function

#End Region

#Region "Private Members"
    ''' <summary>
    ''' Update the source objects with the new geometry
    ''' </summary>
    ''' <param name="theGraphicsContainer">Contains the annotation element.</param>
    ''' <param name="theElement">The annotation element.</param>
    ''' <param name="theTransform">The type of Transform (2D or 3D).</param>
    ''' <remarks></remarks>

    Private Shared Sub finishTransform(ByVal theGraphicsContainer As IGraphicsContainer, ByVal theElement As IElement, ByVal theTransform As ITransform2D)
        theElement = DirectCast(theTransform, IElement)
        theGraphicsContainer.UpdateElement(theElement)
    End Sub
    ''' <summary>
    ''' Calculates the distance annotation pairs should be spaced.
    ''' </summary>
    ''' <param name="theDistance">The distance between the annotation pairs</param>
    ''' <param name="theFontSize">Font size of the annotation feature class</param>
    ''' <returns>The calculated spacing between the annotation pairs</returns>
    ''' <remarks>
    ''' Divides the mean distance between annotation pairs by the font
    ''' size from the annotation feature class to compare against the
    ''' placement constants to determine which type of annotation pair
    ''' is selected (both above, both below, or both sides)
    '''</remarks>
    Private Shared Function calculateAnnoSpacing(ByVal theDistance As Double, ByVal theFontSize As Double) As Double
        Dim theAnnoSpacing As Double
        theAnnoSpacing = Math.Round(theDistance / theFontSize, 2, MidpointRounding.AwayFromZero)
        Return theAnnoSpacing
    End Function

#End Region

End Class
#End Region
