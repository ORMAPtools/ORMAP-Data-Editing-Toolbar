#Region "Copyright 2011 ORMAP Tech Group"

' File:  SingleSpiral.vb
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



Public Class SingleSpiral
    Inherits ESRI.ArcGIS.Desktop.AddIns.Tool
#Region "constructors"
    ''' <summary>
    ''' Gets the window form ID from partner form
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        Try
            Dim windowID As UID = New UIDClass
            windowID.Value = "ORMAP_SpiralToolbar_SpiralDockWindow"
            _partnerSpiralDockWindow = My.ArcMap.DockableWindowManager.GetDockableWindow(windowID)

            If _partnerSpiralDockWindow.IsVisible Then
                _partnerSpiralDockWindow.Show(False)
            End If
        Catch ex As Exception
            OrmapExtension.ProcessUnhandledException(ex)
        End Try
    End Sub
#End Region

#Region "properties"

    Private _IsGettingFromPoint As Boolean = False
    Private _IsGettingTangentPOint As Boolean = False
    Private _isCircleActive As Boolean = False
    Private _partnerSpiralDockWindow As IDockableWindow

    Private WithEvents _partnerSpiralDockWindowUI As SpiralDockWindow
    ''' <summary>
    ''' Creates the hook to the parnter form
    ''' </summary>
    ''' <value></value>
    ''' <returns>The addin id</returns>
    ''' <remarks></remarks>
    Friend ReadOnly Property partnerSpiralDockWindowUI() As spiraldockwindow
        Get
            If _partnerSpiralDockWindowUI Is Nothing Then

                setPartnerSpiralDockWindowUI(AddIn.FromID(Of SpiralDockWindow.AddinImpl)(My.ThisAddIn.IDs.SpiralDockWindow).UI)

            End If
            Return _partnerSpiralDockWindowUI
        End Get
    End Property
    ''' <summary>
    ''' Adds and removes handlers to the partner form.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <remarks></remarks>
    Private Sub setPartnerSpiralDockWindowUI(ByVal value As SpiralDockWindow)
        If value IsNot Nothing Then
            _partnerSpiralDockWindowUI = value
            'Subscribe to partner event
            AddHandler _partnerSpiralDockWindowUI.uxCreate.Click, AddressOf uxCreate_Click
            AddHandler _partnerSpiralDockWindowUI.uxHelp.Click, AddressOf uxHelp_Click
            AddHandler _partnerSpiralDockWindowUI.uxGetFromPoint.Click, AddressOf uxGetFromPoint_Click
            AddHandler _partnerSpiralDockWindowUI.uxGetTangentPoint.Click, AddressOf uxGetTangentPoint_Click
            AddHandler _partnerSpiralDockWindowUI.uxByArcLength.CheckedChanged, AddressOf uxByArcLength_CheckedChanged
            AddHandler _partnerSpiralDockWindowUI.uxByDeltaAngle.CheckedChanged, AddressOf uxByDeltaAngle_CheckedChanged
        Else
            'unsubscribe to partner event
            RemoveHandler _partnerSpiralDockWindowUI.uxCreate.Click, AddressOf uxCreate_Click
            RemoveHandler _partnerSpiralDockWindowUI.uxHelp.Click, AddressOf uxHelp_Click
            RemoveHandler _partnerSpiralDockWindowUI.uxGetFromPoint.Click, AddressOf uxGetFromPoint_Click
            RemoveHandler _partnerSpiralDockWindowUI.uxGetTangentPoint.Click, AddressOf uxGetFromPoint_Click
            RemoveHandler _partnerSpiralDockWindowUI.uxByArcLength.CheckedChanged, AddressOf uxByArcLength_CheckedChanged
            RemoveHandler _partnerSpiralDockWindowUI.uxByDeltaAngle.CheckedChanged, AddressOf uxByDeltaAngle_CheckedChanged
        End If
    End Sub
    ''' <summary>
    ''' Enables or disables the .uxByArcLengthValue text box.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub uxByArcLength_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

        With _partnerSpiralDockWindowUI
            If .uxByArcLength.Checked Then
                .uxArcLenghtValue.Enabled = True
            Else
                .uxArcLenghtValue.Enabled = False
            End If
        End With

    End Sub
    ''' <summary>
    ''' Enables or disables the .uxDeltaAngle text box
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub uxByDeltaAngle_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

        With _partnerSpiralDockWindowUI
            If .uxByDeltaAngle.Checked Then
                .uxDeltaAngle.Enabled = True
            Else
                .uxDeltaAngle.Enabled = False
            End If
        End With

    End Sub
    ''' <summary>
    ''' Enables onmousedown overrides get the from point from the map
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub uxGetFromPoint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        _IsGettingFromPoint = True
        _isCircleActive = True
        MyBase.Cursor = Cursors.Cross

    End Sub
    ''' <summary>
    ''' Enables onmousedown overrides get the from point from the map
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub uxGetTangentPoint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        _IsGettingTangentPOint = True
        _isCircleActive = True
        MyBase.Cursor = Cursors.Cross

    End Sub

    ''' <summary>
    ''' Validates user inputs.  Intiates the construction of a spiral feature
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub uxCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        'Validate User Data
        With _partnerSpiralDockWindowUI
            'TODO: Delete
            'If Len(Trim(.uxTargetTemplate.Text)) = 0 Then
            '    MessageBox.Show("Please select a target template")
            '    Exit Sub
            'End If
            If Trim(.uxBeginRadiusValue.Text).Length = 0 Then
                MessageBox.Show("please enter a beginning Radius value.")
                Exit Sub
            End If
            If Not IsNumeric(Trim(.uxBeginRadiusValue.Text)) Then
                If Not UCase(.uxBeginRadiusValue.Text) = "INFINITY" Then
                    MessageBox.Show("invalid entry for the beginning radius value." & vbNewLine & _
                                    "Enter the radius value is infinity, us INFINITY or 0")
                    Exit Sub
                End If
            End If
            If Trim(.uxEndRadiusValue.Text).Length = 0 Then
                MessageBox.Show("Please enter an end radius value")
                Exit Sub
            End If
            If Not IsNumeric(Trim(.uxEndRadiusValue.Text)) Then
                If Not UCase(.uxEndRadiusValue.Text) = "INFINITY" Then
                    MessageBox.Show("invalid entry for the ending radius value." & vbNewLine & _
                                    "Enter the radius value is infinity, us INFINITY or 0")
                    Exit Sub
                End If
            End If
            If .uxByArcLength.Checked And Trim(.uxArcLenghtValue.Text).Length = 0 Then
                MessageBox.Show("Please enter a spiral length value.")
                Exit Sub
            End If
            If .uxByArcLength.Checked And Not IsNumeric(Trim(.uxArcLenghtValue.Text)) Then
                MessageBox.Show("Please enter a valid spiral length")
                Exit Sub
            End If
            If .uxByDeltaAngle.Checked And Trim(.uxDeltaAngle.Text).Length = 0 Then
                MessageBox.Show("Please enter a valid delta angle")
                Exit Sub
            End If
            If (Trim(.uxFromPointXValue.Text).Length = 0 Or Not IsNumeric(Trim(.uxFromPointXValue.Text))) Or (Trim(.uxFromPointYValue.Text).Length = 0 Or Not IsNumeric(Trim(.uxFromPointYValue.Text))) Then
                MessageBox.Show("Please add a spiral from point or specify the tangent point")
                Exit Sub
            End If
            If Trim(.uxDensityValue.Text).Length = 0 Then
                MessageBox.Show("Please enter a spiral density value")
                Exit Sub
            End If
            If Not IsNumeric(Trim(.uxDensityValue.Text)) Then
                MessageBox.Show("Please enter a numeric value for the spiral curve density")
                Exit Sub
            End If
            'Sets the input points
            Dim theFromPoint As IPoint = New ESRI.ArcGIS.Geometry.Point
            Dim theTangentPoint As IPoint = New ESRI.ArcGIS.Geometry.Point

            'Sets the Beginning Radius Value
            Dim theBeginRadius As Double
            If Trim(.uxBeginRadiusValue.Text) = "INFINITY" Then
                theBeginRadius = 0
            Else
                theBeginRadius = CDbl(Trim(.uxBeginRadiusValue.Text))
            End If

            'Sets the Ending Radius Value
            Dim TheEndRadius As Double
            If Trim(.uxEndRadiusValue.Text) = "INFINITY" Then
                TheEndRadius = 0
            Else
                TheEndRadius = CDbl(Trim(.uxEndRadiusValue.Text))
            End If

            'Retrieves the points from the partner form
            theFromPoint.PutCoords(CDbl(.uxFromPointXValue.Text), CDbl(.uxFromPointYValue.Text))
            theTangentPoint.PutCoords(CDbl(.uxTangentPointXValue.Text), CDbl(.uxTangentPointYValue.Text))

            Dim theFeatureClass As IFeatureClass = GetTargetFeatureClass()

            If .uxByArcLength.Checked Then
                ConstructSpiralByLength(theFromPoint, theTangentPoint, CDbl(.uxArcLenghtValue.Text), theBeginRadius, TheEndRadius, .uxCurvetotheRight.Checked, theFeatureClass.AliasName, CDbl(Trim(.uxDensityValue.Text)))
            Else
                ConstructSpiralbyDelta(theFromPoint, theTangentPoint, .uxCurvetotheLeft.Checked, theBeginRadius, TheEndRadius, .uxDeltaAngle.Text, theFeatureClass.AliasName, CDbl(Trim(.uxDensityValue.Text)))
            End If

        End With

        My.ArcMap.Document.ActiveView.Refresh()

    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub uxHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim theRTFStream As System.IO.Stream = _
          Me.GetType().Assembly.GetManifestResourceStream("OrmapTaxlotEditing.Spiral_help.rtf")
        OpenHelp("Spiral", theRTFStream)
    End Sub
    ''' <summary>
    ''' Updates the target template drop down with polyline features
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub parnterSpiralDockWindow_load()

        Dim theEditor As IEditor3 = DirectCast(My.ArcMap.Editor, IEditor3)
        Dim theTemplateCounts As Integer = theEditor.TemplateCount - 1

        If _partnerSpiralDockWindowUI.uxTargetTemplate.Items.Count > 0 Then _partnerSpiralDockWindowUI.uxTargetTemplate.Items.Clear()

        For i As Integer = 0 To theTemplateCounts
            Dim thisFeatureLayer As IFeatureLayer2 = DirectCast(theEditor.Template(i).Layer, IFeatureLayer2)
            If IsFeatureClassValidPolyline(thisFeatureLayer.FeatureClass) Then _partnerSpiralDockWindowUI.uxTargetTemplate.Items.Add(theEditor.Template(i).Name)
        Next

    End Sub
#End Region

#Region "methods"
    ''' <summary>
    ''' Calls the partner form.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DoButtonOperation()
        Try
            With partnerSpiralDockWindowUI
                .uxDensityValue.Text = "0.5"
                .uxCreate.Enabled = True
            End With

            _partnerSpiralDockWindow.Show(Not _partnerSpiralDockWindow.IsVisible)

            Dim theEditor As IEditor3 = DirectCast(My.ArcMap.Editor, IEditor3)
            If _partnerSpiralDockWindow.IsVisible And _
                (_partnerSpiralDockWindowUI.uxTargetTemplate.Items.Count = 0 _
                 Or _partnerSpiralDockWindowUI.uxTargetTemplate.Items.Count <> theEditor.TemplateCount) Then parnterSpiralDockWindow_load()
        Catch ex As Exception
            OrmapExtension.ProcessUnhandledException(ex)
        End Try
    End Sub

    ''' <summary>
    ''' If the snapping marker is active, this delete it from the graphics container
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

        Return MyBase.OnDeactivate()
    End Function
    ''' <summary>
    ''' When the tool is activated, calls dobuttonoperation
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overrides Sub OnActivate()
        MyBase.OnActivate()

        If Not _partnerSpiralDockWindow.IsVisible Then DoButtonOperation()

    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overrides Sub OnUpdate()

        Me.Enabled = SpiralUtilities.IsEnable
        If Not Me.Enabled And _partnerSpiralDockWindow.IsVisible Then
            _partnerSpiralDockWindow.Show(False)
            If Not _SnappingMarkerElement Is Nothing Then
                Dim theGraphicsContainer As IGraphicsContainer = DirectCast(My.ArcMap.Document.ActiveView, IGraphicsContainer)
                theGraphicsContainer.DeleteElement(_SnappingMarkerElement)
                _SnappingMarkerElement.Geometry.SetEmpty()
                _SnappingMarkerElement = Nothing
                My.ArcMap.Document.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
            End If
        End If

        Try
            With partnerSpiralDockWindowUI
                Dim theEditor As IEditor3 = DirectCast(My.ArcMap.Editor, IEditor3)
                If Not theEditor.CurrentTemplate Is Nothing Then
                    Dim theCheckFeatureLayer As IFeatureLayer2 = DirectCast(theEditor.CurrentTemplate.Layer, IFeatureLayer2)
                    If IsFeatureClassValidPolyline(theCheckFeatureLayer.FeatureClass) Then
                        .uxCreate.Enabled = True
                        .LabelTargetLayer.Text = "Current Target Layer: " & theEditor.CurrentTemplate.Layer.Name
                        If UCase(theEditor.CurrentTemplate.Name) <> UCase(theEditor.CurrentTemplate.Layer.Name) Then
                            .LabelTargetLayer.Text = .LabelTargetLayer.Text & vbNewLine & "Subtype: " & theEditor.CurrentTemplate.Name
                        End If
                    Else
                        .uxCreate.Enabled = False
                        .LabelTargetLayer.Text = theEditor.CurrentTemplate.Layer.Name & " Is not a polyline layer"
                    End If
                Else
                    .uxCreate.Enabled = False
                    .LabelTargetLayer.Text = "Target Layer not selected"
                End If
            End With
        Catch ex As Exception
            OrmapExtension.ProcessUnhandledException(ex)
        End Try

    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="arg"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnMouseDown(ByVal arg As ESRI.ArcGIS.Desktop.AddIns.Tool.MouseEventArgs)
        MyBase.OnMouseDown(arg)

        If arg.Button = MouseButtons.Left And arg.Shift Then
            If Not _partnerSpiralDockWindow.IsVisible Then DoButtonOperation()
        End If

        If arg.Button = MouseButtons.Left And _IsGettingFromPoint Then
            Dim theFromPoint As IPoint = getSnapPoint(getDataFrameCoords(arg.X, arg.Y))
            _partnerSpiralDockWindowUI.uxFromPointXValue.Text = theFromPoint.X.ToString
            _partnerSpiralDockWindowUI.uxFromPointYValue.Text = theFromPoint.Y.ToString
            _IsGettingFromPoint = False
            MyBase.Cursor = Cursors.Arrow
        ElseIf arg.Button = MouseButtons.Left And _IsGettingTangentPOint Then
            Dim theTangentPoint As IPoint = getSnapPoint(getDataFrameCoords(arg.X, arg.Y))
            _partnerSpiralDockWindowUI.uxTangentPointXValue.Text = theTangentPoint.X.ToString
            _partnerSpiralDockWindowUI.uxTangentPointYValue.Text = theTangentPoint.Y.ToString
            _IsGettingTangentPOint = False
            MyBase.Cursor = Cursors.Arrow
        End If
        _isCircleActive = False
    End Sub

    Private _SnappingMarkerElement As IElement = Nothing
    Private _TheSnapPoint As IPoint
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="arg"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnMouseMove(ByVal arg As ESRI.ArcGIS.Desktop.AddIns.Tool.MouseEventArgs)
        MyBase.OnMouseMove(arg)
        Try
            If _isCircleActive And _SnappingMarkerElement Is Nothing Then
                _SnappingMarkerElement = DirectCast(CreateSnapMarker(), IElement)
                Dim theGraphicsContainer As IGraphicsContainer = DirectCast(My.ArcMap.Document.ActiveView, IGraphicsContainer)
                theGraphicsContainer.AddElement(_SnappingMarkerElement, 0)
                Dim theCurrentPoint As IPoint = getSnapPoint(getDataFrameCoords(arg.X, arg.Y))
                _SnappingMarkerElement.Geometry = theCurrentPoint
                My.ArcMap.Document.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
            ElseIf _isCircleActive And Not _SnappingMarkerElement Is Nothing Then
                _SnappingMarkerElement.Geometry = DirectCast(getSnapPoint(getDataFrameCoords(arg.X, arg.Y)), IPoint)
                My.ArcMap.Document.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
            ElseIf Not _isCircleActive And Not _SnappingMarkerElement Is Nothing Then
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
#End Region

End Class
