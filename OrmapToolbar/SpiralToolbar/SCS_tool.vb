#Region "Copyright 2011 ORMAP Tech Group"

' File:  SCS_tool.vb
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
Imports System.Drawing
Imports System.Environment
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Framework
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.Desktop.AddIns
Imports ESRI.ArcGIS.Display
Imports ESRI.ArcGIS.Editor
Imports OrmapTaxlotEditing.SpiralUtilities
Imports OrmapTaxlotEditing.OrmapExtension

#End Region

''' <summary>
''' Provides an ArcMap Command with functionality to
''' Construct a Spiral-Curve-Spiral transition feature.
''' These features are common in Highways and freeways.
''' </summary>
''' <remarks><seealso cref="SCS_tool"/></remarks>


Public Class SCS_tool
    Inherits ESRI.ArcGIS.Desktop.AddIns.Tool

#Region "Built-In Class Members (Constructors, Etc.)"

#Region "constructors"

    Public Sub New()
        Try

            Dim windowID As UID = New UIDClass
            windowID.Value = "ORMAP_SpiralToolbar_SpiralCurveSpiralDockWindow"
            _partnerSCSDockWindow = My.ArcMap.DockableWindowManager.GetDockableWindow(windowID)

            If _partnerSCSDockWindow.IsVisible Then
                _partnerSCSDockWindow.Show(False)
            End If
        Catch ex As Exception

            MessageBox.Show(ex.ToString)

        End Try

    End Sub

#End Region

#End Region

#Region "Custom Class Members"

#Region "Fields (none)"
#End Region



#Region "Properties"

    Private _IsGettingTangentPoint As Boolean = False
    Private _IsGettingFromPoint As Boolean = False
    Private _IsGettingToPoint As Boolean = False
    Private _partnerSCSDockWindow As IDockableWindow

    Private WithEvents _partnerSCSDockWindowUI As SpiralCurveSpiralDockWindow
    ''' <summary>
    ''' Get the UI id from the SpiralDockWindow
    ''' </summary>
    ''' <value></value>
    ''' <returns>Window UI</returns>
    ''' <remarks></remarks>
    Friend ReadOnly Property partnerSCSDockFormUI() As SpiralCurveSpiralDockWindow
        Get
            If _partnerSCSDockWindowUI Is Nothing Then
                setPartnerSCSDockFormUI(AddIn.FromID(Of SpiralCurveSpiralDockWindow.AddinImpl)(My.ThisAddIn.IDs.SpiralCurveSpiralDockWindow).UI)
            End If
            Return _partnerSCSDockWindowUI
        End Get
    End Property
    ''' <summary>
    ''' Sets the event handlers
    ''' </summary>
    ''' <param name="value"></param>
    ''' <remarks></remarks>
    Private Sub setPartnerSCSDockFormUI(ByVal value As SpiralCurveSpiralDockWindow)

        If value IsNot Nothing Then
            _partnerSCSDockWindowUI = value
            'subscribe to partner form events
            AddHandler _partnerSCSDockWindowUI.uxCreate.Click, AddressOf uxCreate_Click
            AddHandler _partnerSCSDockWindowUI.uxHelp.Click, AddressOf uxHelp_Click
            AddHandler _partnerSCSDockWindowUI.uxGettoPoint.Click, AddressOf uxGettoPoint_Click
            AddHandler _partnerSCSDockWindowUI.uxGetTangentPoint.Click, AddressOf uxGetTangentPoint_Click
            AddHandler _partnerSCSDockWindowUI.uxGetFromPoint.Click, AddressOf uxGetFromPoint_Click
            AddHandler _partnerSCSDockWindowUI.uxCurveByRadius.CheckedChanged, AddressOf uxCurveByRadius_CheckedChanged
            AddHandler _partnerSCSDockWindowUI.uxCurvebyDegree.CheckedChanged, AddressOf uxCurvebyDegree_CheckedChanged
            AddHandler _partnerSCSDockWindowUI.uxTimer.Tick, AddressOf uxTimer_tick
        Else
            'unSubscribe to partner form events
            RemoveHandler _partnerSCSDockWindowUI.uxCreate.Click, AddressOf uxCreate_Click
            RemoveHandler _partnerSCSDockWindowUI.uxHelp.Click, AddressOf uxHelp_Click
            RemoveHandler _partnerSCSDockWindowUI.uxGettoPoint.Click, AddressOf uxGettoPoint_Click
            RemoveHandler _partnerSCSDockWindowUI.uxGetTangentPoint.Click, AddressOf uxGetTangentPoint_Click
            RemoveHandler _partnerSCSDockWindowUI.uxGetFromPoint.Click, AddressOf uxGetFromPoint_Click
            RemoveHandler _partnerSCSDockWindowUI.uxCurveByRadius.CheckedChanged, AddressOf uxCurveByRadius_CheckedChanged
            RemoveHandler _partnerSCSDockWindowUI.uxCurvebyDegree.CheckedChanged, AddressOf uxCurvebyDegree_CheckedChanged
            RemoveHandler _partnerSCSDockWindowUI.uxTimer.Tick, AddressOf uxTimer_tick
        End If
    End Sub

#End Region

#Region "Event Handler"

    ''' <summary>
    ''' Constructs the Spiral-Curve-Spiral based on user inputs in the partner Spiral Curve Spiral Dockable Window
    ''' </summary>
    Private Sub uxCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        'Validate user inputs
        With _partnerSCSDockWindowUI
            If .uxCurveByRadius.Checked And Trim(.uxCurveByRadiusValue.Text).Length = 0 Then
                MessageBox.Show("Please enter a radius value")
                Exit Sub
            ElseIf .uxCurveByRadius.Checked And Not IsNumeric(Trim(.uxCurveByRadiusValue.Text)) Then
                MessageBox.Show("Please us a numeric value in the radius value box")
                Exit Sub
            End If
            If .uxCurvebyDegree.Checked And Trim(.uxCurveDegreeValue.Text).Length = 0 Then
                MessageBox.Show("Please eneter a degree value")
                Exit Sub
            End If
            'To Do: After Code review, delete
            'If Trim(.uxTargetLayers.Text).Length = 0 Then
            '    MessageBox.Show("Please select a target layer")
            '    Exit Sub
            'End If
            If Trim(.uxCurveDensity.Text).Length = 0 Then
                MessageBox.Show("Please enter a spiral curve density value")
                Exit Sub
            End If
            If Not IsNumeric(Trim(.uxCurveDensity.Text)) Then
                MessageBox.Show("Please enter a valid numeric value for the" & vbNewLine & "spiral curve density")
                Exit Sub
            End If
            'Sets the input points
            Dim theFromPoint As IPoint = New ESRI.ArcGIS.Geometry.Point
            Dim theTangent As IPoint = New ESRI.ArcGIS.Geometry.Point
            Dim theToPoint As IPoint = New ESRI.ArcGIS.Geometry.Point

            'Retrieves the points from the partner form
            theFromPoint.PutCoords(CDbl(.uxFromPointXValue.Text), CDbl(.uxFromPointYValue.Text))
            theTangent.PutCoords(CDbl(.uxTangentPointXValue.Text), CDbl(.uxTangentPointYValue.Text))
            theToPoint.PutCoords(CDbl(.uxToPointXValue.Text), CDbl(.uxToPointYValue.Text))

            'Sends data to the SpiralsUtilities ConstructSCSbyLenth function
            'John Prychun, Jan 2012
            'Dim editLayer As IEditLayers = DirectCast(My.ArcMap.Editor, IEditLayers)
            'Dim featureLayer As IFeatureLayer2 = DirectCast(editLayer.CurrentLayer, IFeatureLayer2)
            Dim theFeatureClass As IFeatureClass = GetTargetFeatureClass() 'featureLayer.FeatureClass
            'end John Prychun, Jan 2012 
            'Sends data to the SpiralsUtilities ConstructSCSbyLenth function

            Dim theCurveRadius As Double
            If .uxCurveByRadius.Checked Then
                'John Prychun, Jan 2012
                ConstructSCSbyLength(theFromPoint, theTangent, theToPoint, CDbl(Trim(.uxArcLengthValue.Text)), CDbl(Trim(.uxCurveByRadiusValue.Text)), .uxCurvetotheRight.Checked, theFeatureClass.AliasName, CDbl(Trim(.uxCurveDensity.Text))) 'Trim(.uxTargetLayers.Text), CDbl(Trim(.uxCurveDensity.Text)))
                'end John Prychun, Jan 2012 
            ElseIf .uxCurvebyDegree.Checked Then
                If Not IsNumeric(.uxCurveDegreeValue.Text) Then
                    theCurveRadius = 5729.578 / DMSAngletoDouble(Trim(.uxCurveDegreeValue.Text))
                    If theCurveRadius = 0 Then
                        MessageBox.Show("Please Enter a valid degree value" & vbNewLine _
                                        & "for example, 1-30-00")
                        Exit Sub
                    Else
                        theCurveRadius = 5729.578 / theCurveRadius
                    End If
                Else
                    theCurveRadius = 5729.578 / CDbl(.uxCurveDegreeValue.Text)
                End If

                Try

                    'John Prychun, Jan 2012
                    ConstructSCSbyLength(theFromPoint, theTangent, theToPoint, CDbl(Trim(.uxArcLengthValue.Text)), theCurveRadius, .uxCurvetotheRight.Checked, theFeatureClass.AliasName, CDbl(Trim(.uxCurveDensity.Text))) 'Trim(.uxTargetLayers.Text), CDbl(Trim(.uxCurveDensity.Text)))
                    'ConstructSCSbyLength(theFromPoint, theTangent, theToPoint, CDbl(.uxArcLengthValue.Text), theCurveRadius, .uxCurvetotheRight.Checked, .uxTargetLayers.Text, CDbl(Trim(.uxCurveDensity.Text)))
                    'end John Prychun, Jan 2012 

                Catch ex As Exception
                    OrmapExtension.ProcessUnhandledException(ex)
                End Try


            End If
        End With

        My.ArcMap.Document.ActiveView.Refresh()

    End Sub
    ''' <summary>
    ''' Opens Help Document
    ''' </summary>
    Private Sub uxHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim theRTFStream As System.IO.Stream = _
           Me.GetType().Assembly.GetManifestResourceStream("OrmapTaxlotEditing.SpiralCurveSpiral_help.rtf")
        OpenHelp("Spiral-Curve-Spiral Transition", theRTFStream)
    End Sub
    ''' <summary>
    ''' Gets the end point for the Spiral Curve Spiral Construction from the map.
    ''' </summary>
    Private Sub uxGettoPoint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        _IsGettingToPoint = True
        _IsCircleActive = True
        MyBase.Cursor = Cursors.Cross

    End Sub
    ''' <summary>
    ''' Gets the tangent point for the Spiral Curve Spiral Construction from the map.
    ''' </summary>
    Private Sub uxGetTangentPoint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        _IsGettingTangentPoint = True
        _IsCircleActive = True
        MyBase.Cursor = Cursors.Cross

    End Sub
    ''' <summary>
    ''' Gets the starting point for the Spiral Curve Spiral Construction from the map.
    ''' </summary>
    Private Sub uxGetFromPoint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        _IsGettingFromPoint = True
        _IsCircleActive = True
        MyBase.Cursor = Cursors.Cross

    End Sub
    ''' <summary>
    ''' Defines it the central curve is defined by a radius.
    ''' </summary>
    ''' <remarks>If checked, the .uxCurvebyRadiusValue is enabled.  The .uxCurvebyDegreeValue is disabled</remarks>
    Private Sub uxCurveByRadius_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

        If _partnerSCSDockWindowUI.uxCurveByRadius.Checked Then
            _partnerSCSDockWindowUI.uxCurveByRadiusValue.Enabled = True
        Else
            _partnerSCSDockWindowUI.uxCurveByRadiusValue.Enabled = False
        End If

    End Sub
    ''' <summary>
    ''' Defines if the central curve is defined by a degree of curve.
    ''' </summary>
    ''' <remarks>If checked, the .uxCurvebyDegreeValue is Enable.  The .uxCurvebyRadiusValue is disabled</remarks>
    Private Sub uxCurvebyDegree_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

        If _partnerSCSDockWindowUI.uxCurvebyDegree.Checked Then
            _partnerSCSDockWindowUI.uxCurveDegreeValue.Enabled = True
        Else
            _partnerSCSDockWindowUI.uxCurveDegreeValue.Enabled = False
        End If

    End Sub
    ''' <summary>
    ''' Loads the target layers 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub partnerSCSDockWindow_load()

        Dim theEditor As IEditor3 = DirectCast(My.ArcMap.Editor, IEditor3)
        Dim theTemplateCounts As Integer = theEditor.TemplateCount - 1

        If _partnerSCSDockWindowUI.uxTargetLayers.Items.Count > 0 Then _partnerSCSDockWindowUI.uxTargetLayers.Items.Clear()

        For i As Integer = 0 To theTemplateCounts
            Dim thisFeatureLayer As IFeatureLayer2 = DirectCast(theEditor.Template(i).Layer, IFeatureLayer2)
            If IsFeatureClassValidPolyline(thisFeatureLayer.FeatureClass) Then _partnerSCSDockWindowUI.uxTargetLayers.Items.Add(theEditor.Template(i).Name)
        Next

    End Sub
    'This sub routine exists for testing purposes.
    Private Sub uxTimer_tick(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub
#End Region

#End Region

#Region "Methods"

    ''' <summary>
    ''' Loads and makes the form visible
    ''' </summary>
    ''' <remarks></remarks>
    Friend Sub DoButtonOperation()

        With partnerSCSDockFormUI
            .uxCreate.Enabled = True
            .uxCurveDensity.Text = "0.5"
        End With

        _partnerSCSDockWindow.Show(Not _partnerSCSDockWindow.IsVisible)

        Dim theEditor As IEditor3 = DirectCast(My.ArcMap.Editor, IEditor3)
        If _partnerSCSDockWindow.IsVisible And _
            (_partnerSCSDockWindowUI.uxTargetLayers.Items.Count = 0 _
             Or _partnerSCSDockWindowUI.uxTargetLayers.Items.Count <> theEditor.TemplateCount) Then partnerSCSDockWindow_load()

    End Sub
    ''' <summary>
    ''' On the MouseDown Event this sub routine checks to see if the user is looking for a point required to construct the spiral
    ''' curve spiral construction.  If it is, gets the map coordinates and assigns it to the parnter form window text boxes related
    ''' to the points.
    ''' </summary>
    ''' <remarks>This also enables the snap graphic
    ''' Refer to the OnMouseMove override
    ''' </remarks>
    Protected Overrides Sub OnMouseDown(ByVal arg As ESRI.ArcGIS.Desktop.AddIns.Tool.MouseEventArgs)
        MyBase.OnMouseDown(arg)

        If arg.Button = MouseButtons.Left And arg.Shift Then
            If Not _partnerSCSDockWindow.IsVisible Then DoButtonOperation()
        End If

        If arg.Button = MouseButtons.Left And _IsGettingToPoint Then
            Dim theToPoint As IPoint = getSnapPoint(getDataFrameCoords(arg.X, arg.Y))
            _partnerSCSDockWindowUI.uxToPointXValue.Text = theToPoint.X.ToString
            _partnerSCSDockWindowUI.uxToPointYValue.Text = theToPoint.Y.ToString
            _IsGettingToPoint = False
            MyBase.Cursor = Cursors.Arrow
        ElseIf arg.Button = MouseButtons.Left And _IsGettingFromPoint Then
            Dim theFromPoint As IPoint = getSnapPoint(getDataFrameCoords(arg.X, arg.Y))
            _partnerSCSDockWindowUI.uxFromPointXValue.Text = theFromPoint.X.ToString
            _partnerSCSDockWindowUI.uxFromPointYValue.Text = theFromPoint.Y.ToString
            _IsGettingFromPoint = False
            _IsCircleActive = False
            MyBase.Cursor = Cursors.Arrow
        ElseIf arg.Button = MouseButtons.Left And _IsGettingTangentPoint Then
            Dim theTangentPoint As IPoint = getSnapPoint(getDataFrameCoords(arg.X, arg.Y))
            _partnerSCSDockWindowUI.uxTangentPointXValue.Text = theTangentPoint.X.ToString
            _partnerSCSDockWindowUI.uxTangentPointYValue.Text = theTangentPoint.Y.ToString
            _IsGettingTangentPoint = False
            MyBase.Cursor = Cursors.Arrow
        End If
        _IsCircleActive = False
    End Sub

    'Sets the gobal values for the snapping graphic.  The values are set here so they are proceeding the primary sub routine that uses them.
    Private _SnappingMarkerElement As IElement = Nothing
    Private _IsCircleActive As Boolean = False
    Private _TheSnapPoint As IPoint

    ''' <summary>
    ''' Checks to see if _IsCircleActive.  The circle is used to verify if the cursor is snapping to a snap feature.
    ''' </summary>
    ''' <param name="arg"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnMouseMove(ByVal arg As ESRI.ArcGIS.Desktop.AddIns.Tool.MouseEventArgs)
        MyBase.OnMouseMove(arg)
        Try
            If _IsCircleActive And _SnappingMarkerElement Is Nothing Then
                _SnappingMarkerElement = DirectCast(CreateSnapMarker(), IElement)
                Dim theGraphicsContainer As IGraphicsContainer = DirectCast(My.ArcMap.Document.ActiveView, IGraphicsContainer)
                theGraphicsContainer.AddElement(_SnappingMarkerElement, 0)
                Dim theCurrentPoint As IPoint = getSnapPoint(getDataFrameCoords(arg.X, arg.Y))
                _SnappingMarkerElement.Geometry = theCurrentPoint
                My.ArcMap.Document.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
            ElseIf _IsCircleActive And Not _SnappingMarkerElement Is Nothing Then
                _SnappingMarkerElement.Geometry = DirectCast(getSnapPoint(getDataFrameCoords(arg.X, arg.Y)), IPoint)
                My.ArcMap.Document.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
            ElseIf Not _IsCircleActive And Not _SnappingMarkerElement Is Nothing Then
                Dim theGraphicsContainer As IGraphicsContainer = DirectCast(My.ArcMap.Document.ActiveView, IGraphicsContainer)
                theGraphicsContainer.DeleteElement(_SnappingMarkerElement)
                _SnappingMarkerElement.Geometry.SetEmpty()
                _SnappingMarkerElement = Nothing
                My.ArcMap.Document.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, Nothing, Nothing)
            End If
        Catch ex As Exception
            OrmapExtension.ProcessUnhandledException(ex)
        End Try

    End Sub
    ''' <summary>
    ''' shows the Dockable Window
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overrides Sub OnActivate()
        MyBase.OnActivate()

        If Not _partnerSCSDockWindow.IsVisible Then DoButtonOperation()

    End Sub
    ''' <summary>
    ''' When tool is deactivated it removes snap graphic if visible
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Overrides Function OnDeactivate() As Boolean
        If Not _SnappingMarkerElement Is Nothing Then
            Dim theGraphicsContainer As IGraphicsContainer = DirectCast(My.ArcMap.Document.ActiveView, IGraphicsContainer)
            theGraphicsContainer.DeleteElement(_SnappingMarkerElement)
            _SnappingMarkerElement.Geometry.SetEmpty()
            _SnappingMarkerElement = Nothing
            My.ArcMap.Document.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, Nothing, Nothing)
        End If
        Return MyBase.OnDeactivate
    End Function
    ''' <summary>
    ''' Enables the tool when the edit session is started.  It also hides the SCSDockwindow when the edit session is ended.
    ''' if the snapping marker is visible at the stop editing event, it is deleted.
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overrides Sub OnUpdate()
        Me.Enabled = SpiralUtilities.IsEnable

        If Not Me.Enabled Then
            _partnerSCSDockWindow.Show(False)
            If Not _SnappingMarkerElement Is Nothing Then
                Dim theGraphicsContainer As IGraphicsContainer = DirectCast(My.ArcMap.Document.ActiveView, IGraphicsContainer)
                theGraphicsContainer.DeleteElement(_SnappingMarkerElement)
                _SnappingMarkerElement.Geometry.SetEmpty()
                _SnappingMarkerElement = Nothing
                My.ArcMap.Document.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
            End If
        End If

        Try
            With partnerSCSDockFormUI
                Dim theEditor As IEditor3 = DirectCast(My.ArcMap.Editor, IEditor3)
                If Not theEditor.CurrentTemplate Is Nothing Then
                    Dim theCheckFeatureLayer As IFeatureLayer2 = DirectCast(theEditor.CurrentTemplate.Layer, IFeatureLayer2)
                    If IsFeatureClassValidPolyline(theCheckFeatureLayer.FeatureClass) Then
                        .uxCreate.Enabled = True
                        .LabelTargetFeatureClass.Text = "Current Target Layer: " & theEditor.CurrentTemplate.Layer.Name
                        If UCase(theEditor.CurrentTemplate.Name) <> UCase(theEditor.CurrentTemplate.Layer.Name) Then
                            .LabelTargetFeatureClass.Text = .LabelTargetFeatureClass.Text & vbNewLine & "Subtype: " & theEditor.CurrentTemplate.Name
                        End If
                    Else
                        .uxCreate.Enabled = False
                        .LabelTargetFeatureClass.Text = theEditor.CurrentTemplate.Layer.Name & " Is not a polyline layer"
                    End If
                Else
                    .uxCreate.Enabled = False
                    .LabelTargetFeatureClass.Text = "Target Layer not selected"
                End If
            End With
        Catch ex As Exception
            OrmapExtension.ProcessUnhandledException(ex)
        End Try


    End Sub

#End Region

    Private Function featureclass() As Object
        Throw New NotImplementedException
    End Function



End Class


