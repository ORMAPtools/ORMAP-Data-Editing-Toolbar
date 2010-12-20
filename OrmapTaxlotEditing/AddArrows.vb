#Region "Copyright 2008 ORMAP Tech Group"

' File:  AddArrows.vb
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
Imports System.Drawing
Imports System.Environment
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports ESRI.ArcGIS.ADF.BaseClasses
Imports ESRI.ArcGIS.ADF.CATIDs
Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Editor
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Framework
Imports ESRI.ArcGIS.Display
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.Geodatabase
Imports OrmapTaxlotEditing.DataMonitor
Imports OrmapTaxlotEditing.SpatialUtilities
Imports OrmapTaxlotEditing.StringUtilities
Imports OrmapTaxlotEditing.Utilities

Imports System.Text

#End Region

''' <summary>
''' Provides an ArcMap Tool with functionality to allow users to 
''' add dimension arrows, other arrows and hooks (not implemented).
''' </summary>
''' <remarks>
''' <para><seealso cref="AddArrowsForm"/></para>
''' <para><seealso cref="DimensionArrowsForm"/></para>
''' </remarks>
<ComVisible(True)> _
<ComClass(AddArrows.ClassId, AddArrows.InterfaceId, AddArrows.EventsId), _
ProgId("ORMAPTaxlotEditing.AddArrows")> _
Public NotInheritable Class AddArrows
    Inherits BaseTool
    Implements IDisposable

#Region "Class-Level Constants and Enumerations"
    Private Const _ignoreCase As StringComparison = StringComparison.CurrentCultureIgnoreCase
#End Region

#Region "Built-In Class Members (Constructors, Etc.)"

#Region "Constructors"

    ' A creatable COM class must have a Public Sub New() 
    ' with no parameters, otherwise, the class will not be 
    ' registered in the COM registry and cannot be created 
    ' via CreateObject.
    Public Sub New()
        MyBase.New()

        ' Define protected instance field values for the public properties
        MyBase.m_category = "OrmapToolbar"  'localizable text 
        MyBase.m_caption = "AddArrows"   'localizable text 
        MyBase.m_message = "Add arrow features to the Cartographic Lines feature class."   'localizable text 
        MyBase.m_toolTip = "Add Arrows" 'localizable text 
        MyBase.m_name = MyBase.m_category & "_AddArrows"  'unique id, non-localizable (e.g. "MyCategory_ArcMapCommand")

        Try
            ' Set the bitmap based on the name of the class.
            _bitmapResourceName = Me.GetType().Name + ".bmp"
            MyBase.m_bitmap = New Bitmap(Me.GetType(), _bitmapResourceName)
        Catch ex As ArgumentException
            EditorExtension.ProcessUnhandledException(ex)
        End Try

    End Sub

#End Region

#End Region

#Region "Custom Class Members"

#Region "Fields"

    Private _application As IApplication
    Private _bitmapResourceName As String
    Private _theLineSymbol As ILineSymbol
    Private _theArrowPt1 As IPoint
    Private _theArrowPt2 As IPoint
    Private _theArrowPt3 As IPoint
    Private _theArrowPt4 As IPoint
    Private _theTextPoint As IPoint
    Private _theLinePolyline As IPolyline
    Private _theTextSymbol As ITextSymbol
    Private _inUse As Boolean
    Private _theArrowPtTemp As IPoint
    Private _theArrowPtTemp2 As IPoint
    Private _thePt As IPoint
    Private _theSnapAgent As IFeatureSnapAgent
    Private _theActiveView As IActiveView
    Private WithEvents _theEditorEvents As Editor
    Private _toolJustCompletedTask As Boolean
    Private _arrowPointsCollection As Collection
    Private _mouseHasMoved As Boolean
    Private _inTol As Boolean
    '-- HOOKS NOT IMPLEMENTED --
    '--HOOKS-     Private _theFromBreakPoint As IPoint ' hooks
    '--HOOKS-     Private _theStartPoint As IPoint ' hooks 
    '--HOOKS-     Private _theToBreakPoint As IPoint ' hooks  
    '--HOOKS-     Private _hookAngle As Double
    '--HOOKS-     Private _doOnce As Boolean
    '-- END HOOKS

#End Region

#Region "Properties"

    Private WithEvents _partnerAddArrowsForm As AddArrowsForm

    Friend ReadOnly Property PartnerAddArrowsForm() As AddArrowsForm
        Get
            If _partnerAddArrowsForm Is Nothing OrElse _partnerAddArrowsForm.IsDisposed Then
                setPartnerAddArrowsForm(New AddArrowsForm())
            End If
            Return _partnerAddArrowsForm
        End Get
    End Property

    Private Sub setPartnerAddArrowsForm(ByVal value As AddArrowsForm)
        If value IsNot Nothing Then
            _partnerAddArrowsForm = value
            ' Subscribe to partner form events.
            AddHandler _partnerAddArrowsForm.Load, AddressOf PartnerAddArrowsForm_Load
            AddHandler _partnerAddArrowsForm.uxQuit.Click, AddressOf uxQuit_Click
            AddHandler _partnerAddArrowsForm.uxHelp.Click, AddressOf uxHelp_Click
            AddHandler _partnerAddArrowsForm.uxAddStandard.Click, AddressOf uxAddStandard_Click
            AddHandler _partnerAddArrowsForm.uxAddDimension.Click, AddressOf uxAddDimension_Click
            AddHandler _partnerAddArrowsForm.FormClosed, AddressOf PartnerAddArrowsForm_Close

        Else
            ' Unsubscribe to partner form events.
            RemoveHandler _partnerAddArrowsForm.Load, AddressOf PartnerAddArrowsForm_Load
            RemoveHandler _partnerAddArrowsForm.uxQuit.Click, AddressOf uxQuit_Click
            RemoveHandler _partnerAddArrowsForm.uxHelp.Click, AddressOf uxHelp_Click
            RemoveHandler _partnerAddArrowsForm.uxAddStandard.Click, AddressOf uxAddStandard_Click
            RemoveHandler _partnerAddArrowsForm.uxAddDimension.Click, AddressOf uxAddDimension_Click
            RemoveHandler _partnerAddArrowsForm.FormClosed, AddressOf PartnerAddArrowsForm_Close
        End If

    End Sub

    Private WithEvents _partnerDimensionArrowsForm As DimensionArrowsForm

    Friend ReadOnly Property PartnerDimensionArrowsForm() As DimensionArrowsForm
        Get
            If _partnerDimensionArrowsForm Is Nothing OrElse _partnerDimensionArrowsForm.IsDisposed Then
                setPartnerDimensionArrowsForm(New DimensionArrowsForm())
            End If
            Return _partnerDimensionArrowsForm
        End Get
    End Property

    Private Sub setPartnerDimensionArrowsForm(ByVal value As DimensionArrowsForm)
        If value IsNot Nothing Then
            _partnerDimensionArrowsForm = value
            AddHandler _partnerDimensionArrowsForm.Load, AddressOf PartnerDimensionArrowsForm_Load
            AddHandler _partnerDimensionArrowsForm.uxApply.Click, AddressOf uxApply_Click
            AddHandler _partnerDimensionArrowsForm.uxReset.Click, AddressOf uxReset_Click

            ' Subscribe to partner form events.
        Else
            RemoveHandler _partnerDimensionArrowsForm.Load, AddressOf PartnerDimensionArrowsForm_Load
            RemoveHandler _partnerDimensionArrowsForm.uxApply.Click, AddressOf uxApply_Click
            RemoveHandler _partnerDimensionArrowsForm.uxReset.Click, AddressOf uxReset_Click
            ' Unsubscribe to partner form events.
        End If
    End Sub

    Private _ratioLine As Double = 1.75
    Friend Property RatioLine() As Double
        Get
            Return _ratioLine
        End Get
        Set(ByVal value As Double)
            _ratioLine = value
        End Set
    End Property

    Private _ratioCurve As Double = 1.35
    Friend Property RatioCurve() As Double
        Get
            Return _ratioCurve
        End Get
        Set(ByVal value As Double)
            _ratioCurve = value
        End Set
    End Property

    Private _smoothRatio As Double = 10
    Friend Property SmoothRatio() As Double
        Get
            Return _smoothRatio
        End Get
        Set(ByVal value As Double)
            _smoothRatio = value
        End Set
    End Property

    Private _addManually As Boolean = False
    Friend Property AddManually() As Boolean
        Get
            Return _addManually
        End Get
        Set(ByVal value As Boolean)
            _addManually = value
        End Set
    End Property

    Private _arrowType As String = String.Empty
    Friend Property arrowType() As String
        Get
            Return _arrowType
        End Get
        Set(ByVal value As String)
            _arrowType = value.ToUpper
        End Set
    End Property

    Private _arrowLineStyle As Integer
    Friend Property arrowLineStyle() As Integer
        Get
            Return _arrowLineStyle
        End Get
        Set(ByVal value As Integer)
            _arrowLineStyle = value
        End Set
    End Property


#End Region

#Region "Event Handlers"

    Private Sub PartnerAddArrowsForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles PartnerAddArrowsForm.Load

        With PartnerAddArrowsForm
            ' Populate multi-value controls
            If .uxArrowLineStyle.Items.Count = 0 Then '-- Only load the text box the first time the tool is run.
                .uxArrowLineStyle.Items.Add("100 - Anno Arrow")
                .uxArrowLineStyle.Items.Add("101 - Hooks")
                .uxArrowLineStyle.Items.Add("102 - Radius Line")
                .uxArrowLineStyle.Items.Add("120 - Station Reference")
                .uxArrowLineStyle.Items.Add("125 - River Arrow")
                .uxArrowLineStyle.Items.Add("134 - Bearing/Distance Arrow")
                .uxArrowLineStyle.Items.Add("136 - Reference Notes")
                .uxArrowLineStyle.Items.Add("137 - Taxlot Arrow")
                .uxArrowLineStyle.Items.Add("141 - Subdivision Arrow")
                .uxArrowLineStyle.Items.Add("147 - DLC Arrow")
                .uxArrowLineStyle.Items.Add("154 - Code Arrow")
                .uxArrowLineStyle.Items.Add("162 - See Map Arrow")
            End If
            ' Set control defaults
            If .uxArrowLineStyle.SelectedIndex = -1 Then
                .uxArrowLineStyle.SelectedIndex = 0
            End If


        End With

    End Sub

    Private Sub uxQuit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) 'Handles PartnerAddArrowsForm.uxFind.Click
        OnKeyDown(Keys.Q, 1) 'exit tool
        PartnerAddArrowsForm.Close()
    End Sub

    Private Sub uxHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) 'Handles PartnerAddArrowsForm.uxHelp.Click
        ' TODO: [NIS] Could be replaced with new help mechanism.

        Dim theRTFStream As System.IO.Stream = _
           Me.GetType().Assembly.GetManifestResourceStream("OrmapTaxlotEditing.AddArrows_help.rtf")
        OpenHelp("Add Arrows Help", theRTFStream)

        ' Get the help form.
        'Dim theHelpForm As New HelpForm
        'theHelpForm.Text = "Add Arrows Help"

        ' KLUDGE: [NIS] Remove comments if file is ready.
        '' Open a custom help text file.
        '' Note: Requires a specific file in the help subdirectory of the application directory.
        'Dim theTextFilePath As String
        'theTextFilePath = My.Application.Info.DirectoryPath & "\help\AddArrowsHelp.rtf"
        'If Microsoft.VisualBasic.FileIO.FileSystem.FileExists(theTextFilePath) Then
        '    theHelpForm.RichTextBox1.LoadFile(theTextFilePath, RichTextBoxStreamType.RichText)
        'Else
        '    MessageBox.Show("No help file available in the directory " & NewLine & _
        '            My.Application.Info.DirectoryPath & "\help" & ".")
        '    theHelpForm.TabPage1.Hide()
        'End If

        ' KLUDGE: [NIS] Remove comments if file is ready.
        ' Open a custom help pdf file.
        ' Note: Requires a specific file in the help subdirectory of the application directory.
        ' Requires Adobe Acrobat reader plug-in.
        'Dim thePdfFilePath As String
        'thePdfFilePath = My.Application.Info.DirectoryPath & "\help\AddArrowsHelp.pdf"
        'If Microsoft.VisualBasic.FileIO.FileSystem.FileExists(thePdfFilePath) Then
        '    Dim theUri As New System.Uri("file:///" & thePdfFilePath)
        '    theHelpForm.WebBrowser1.Url = theUri
        'Else
        '    MessageBox.Show("No help file available in the directory " & NewLine & _
        '            My.Application.Info.DirectoryPath & "\help" & ".")
        '    theHelpForm.TabPage2.Hide()
        'End If

        ' KLUDGE: [NIS] Remove comments if file is ready.
        '' Open a custom help video.
        '' Note: Requires a specific file in the help\videos subdirectory of the application directory.
        'Dim theVideoFilePath As String
        'theVideoFilePath = My.Application.Info.DirectoryPath & "\help\videos\AddArrows\AddArrows.html"
        'If Microsoft.VisualBasic.FileIO.FileSystem.FileExists(theVideoFilePath) Then
        '    Dim theUri As New System.Uri("file:///" & theVideoFilePath)
        '    theHelpForm.WebBrowser1.Url = theUri
        'Else
        '    MessageBox.Show("No help file available in the directory " & NewLine & _
        '            My.Application.Info.DirectoryPath & "\help\videos\AddArrows" & ".")
        '    theHelpForm.TabPage2.Hide()
        'End If

        ' KLUDGE: [NIS] Remove comments if form will be used.
        'theHelpForm.Width = 668
        'theHelpForm.Height = 400
        'theHelpForm.Show()

    End Sub

    Private Sub uxAddStandard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) 'Handles PartnerAddArrowsForm.uxAddStandard.Click
        arrowType = "ARROW"
        arrowLineStyle = CInt(PartnerAddArrowsForm.uxArrowLineStyle.Text.Substring(0, 3))
        PartnerAddArrowsForm.Close()
    End Sub

    Private Sub uxAddDimension_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) 'Handles PartnerAddArrowsForm.uxAddDimension.Click
        arrowType = "DIMENSION"
        PartnerAddArrowsForm.Close()
    End Sub

    Private Sub uxApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) 'Handles PartnerDimensionArrowsForm.uxApply.Click

        Dim uxRatioOfLine As String = PartnerDimensionArrowsForm.uxRatioOfLine.Text
        Dim uxSmoothRatio As String = PartnerDimensionArrowsForm.uxSmoothRatio.Text
        Dim uxRatioOfCurve As String = PartnerDimensionArrowsForm.uxRatioOfCurve.Text

        ' Clear any previous errors
        PartnerDimensionArrowsForm.uxErrorProvider.Clear()

        Dim errorControl As Control = Nothing
        If Not IsNumeric(uxSmoothRatio) OrElse CDbl(uxSmoothRatio) <= 0 Then errorControl = PartnerDimensionArrowsForm.uxSmoothRatio
        If Not IsNumeric(uxRatioOfLine) OrElse CDbl(uxRatioOfLine) <= 0 Then errorControl = PartnerDimensionArrowsForm.uxRatioOfLine
        If Not IsNumeric(uxRatioOfCurve) OrElse CDbl(uxRatioOfCurve) <= 0 Then errorControl = PartnerDimensionArrowsForm.uxRatioOfCurve

        If Not errorControl Is Nothing Then
            Dim errorText As String = "Please enter a numeric value greater than 0."
            PartnerDimensionArrowsForm.uxErrorProvider.SetError(errorControl, errorText)
            MessageBox.Show(errorText, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        ' Close the form.
        PartnerDimensionArrowsForm.Close()

        RatioLine = CDbl(uxRatioOfLine)
        SmoothRatio = CDbl(uxSmoothRatio)
        RatioCurve = CDbl(uxRatioOfCurve)
        AddManually = PartnerDimensionArrowsForm.uxManuallyAddArrow.Checked

    End Sub

    Private Sub PartnerDimensionArrowsForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) 'Handles PartnerDimensionArrowsForm.Load
        ' Clear any previous errors
        PartnerDimensionArrowsForm.uxErrorProvider.Clear()

        If PartnerDimensionArrowsForm.uxRatioOfCurve.Text = "" Then
            uxReset_Click(sender, e)
        Else
            PartnerDimensionArrowsForm.uxManuallyAddArrow.Checked = AddManually
            PartnerDimensionArrowsForm.uxSmoothRatio.Text = CStr(SmoothRatio)
            PartnerDimensionArrowsForm.uxRatioOfCurve.Text = CStr(RatioCurve)
            PartnerDimensionArrowsForm.uxRatioOfLine.Text = CStr(RatioLine)
        End If

    End Sub


    Private Sub uxReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) 'Handles PartnerDimensionArrowsForm.uxReset.Click
        ' Clear any previous errors
        PartnerDimensionArrowsForm.uxErrorProvider.Clear()

        PartnerDimensionArrowsForm.uxManuallyAddArrow.Checked = False
        PartnerDimensionArrowsForm.uxSmoothRatio.Text = "10"
        PartnerDimensionArrowsForm.uxRatioOfCurve.Text = "1.35"
        PartnerDimensionArrowsForm.uxRatioOfLine.Text = "1.75"

    End Sub

    Private Sub PartnerAddArrowsForm_Close(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs)
        If arrowType = "" Then OnKeyDown(Keys.Q, 1) 'exit tool
    End Sub


#End Region

#Region "Methods"
    Friend Sub DoButtonOperation()

        Try
            ' Check for valid Map Index data.
            CheckValidMapIndexDataProperties()
            If Not HasValidMapIndexData Then
                MessageBox.Show("Missing data: Valid ORMAP MapIndex layer not found in the map." & vbNewLine & _
                                "Please load this dataset into your map.", _
                                "Locate Feature", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                OnKeyDown(Keys.Q, 1) 'exit tool
                Exit Try
            End If

            ' Check for Cartographic Lines data.
            CheckValidCartographicLinesDataProperties()
            If Not HasValidCartographicLinesData Then
                MessageBox.Show("Missing data: Valid ORMAP CartographicLines layer not found in the map." & vbNewLine & _
                                "Please load this dataset into your map.", _
                                "Locate Feature", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                OnKeyDown(Keys.Q, 1) 'exit tool
                Exit Try
            End If

            ' HACK: [SC] Set global variables in DoButtonOperation... These should be re-evaluated.
            If _arrowPointsCollection Is Nothing Then _arrowPointsCollection = New Collection
            Dim theArcMapDoc As IMxDocument = DirectCast(EditorExtension.Application.Document, IMxDocument)
            _theActiveView = theArcMapDoc.ActiveView

            PartnerAddArrowsForm.ShowDialog()

        Catch ex As Exception
            EditorExtension.ProcessUnhandledException(ex)

        End Try

    End Sub

    '-- HOOKS NOT IMPLEMENTED --
    '--HOOKS-     ''' <summary>
    '--HOOKS-     ''' Find a polyline that intersects a text element but omits the section that intersects the text element
    '--HOOKS-     ''' </summary>
    '--HOOKS-     ''' <param name="theTextSymbol">The text to overlay the polyline.</param>
    '--HOOKS-     ''' <param name="thePoint">A filler symbol for the returned polyline.</param>
    '--HOOKS-     ''' <param name="thePolyline">The polyline to intersect.</param>
    '--HOOKS-     ''' <returns> A polyline that represents the difference between the 
    '--HOOKS-     ''' input polyline and the intersection of the input polyline
    '--HOOKS-     ''' and the boundary of the input text symbol
    '--HOOKS-     '''</returns>
    '--HOOKS-     ''' <remarks>Give a screen display, pDisplay, a text symbol, pTextSymbol,
    '--HOOKS-     ''' a point to fill the a polygon with, pPoint, and the polyline to intersect, 
    '--HOOKS-     ''' pPolyline.  Find the bounding polygon of theTextSymbol and fill it with 
    '--HOOKS-     ''' thePoint.  Then determine the intersection between pPolyline and theTextSymbol.  
    '--HOOKS-     ''' Finally, return the difference of the intersection and pPolyline.
    '--HOOKS-     ''' </remarks>
    '--HOOKS-     Friend Function GetSmashedLine(ByVal theTextSymbol As ISymbol, ByVal thePoint As IPoint, ByVal thePolyline As IPolyline) As IPolyline
    '--HOOKS- 
    '--HOOKS-         Try
    '--HOOKS- 
    '--HOOKS-             Dim theBoundary As IPolygon = New Polygon
    '--HOOKS-             theTextSymbol.QueryBoundary(_theActiveView.ScreenDisplay.hDC, _theActiveView.ScreenDisplay.DisplayTransformation, thePoint, theBoundary)
    '--HOOKS- 
    '--HOOKS-             Dim theTopoOperator As ITopologicalOperator = DirectCast(theBoundary, ITopologicalOperator)
    '--HOOKS-             Dim theIntersect As IPolyline = DirectCast(theTopoOperator.Intersect(thePolyline, esriGeometryDimension.esriGeometry1Dimension), IPolyline)
    '--HOOKS- 
    '--HOOKS-             ' Returns the difference between the polyline and the intersection
    '--HOOKS-             theTopoOperator = DirectCast(thePolyline, ITopologicalOperator)
    '--HOOKS- 
    '--HOOKS-             Return DirectCast(theTopoOperator.Difference(theIntersect), IPolyline)
    '--HOOKS- 
    '--HOOKS-         Catch ex As Exception
    '--HOOKS-             EditorExtension.ProcessUnhandledException(ex)
    '--HOOKS-             Return Nothing
    '--HOOKS- 
    '--HOOKS-         End Try
    '--HOOKS- 
    '--HOOKS-     End Function
    '--HOOKS- 
    '--HOOKS- 
    '--HOOKS-     ''' <summary>
    '--HOOKS-     ''' Given a base line, theSketch. Create a land hook based on the endpoints of theSketch.
    '--HOOKS-     ''' </summary>
    '--HOOKS-     ''' <param name="theSketch">An object that supports the IGeometry interface.</param>
    '--HOOKS-     ''' <remarks>
    '--HOOKS-     ''' </remarks>
    '--HOOKS-     Friend Sub GenerateHooks(ByRef theSketch As IGeometry)
    '--HOOKS- 
    '--HOOKS-         Try
    '--HOOKS- 
    '--HOOKS-             ' Initialize the hook angle
    '--HOOKS-             _hookAngle = 20
    '--HOOKS- 
    '--HOOKS-             ' Make sure the edit sketch is a polyline and insure that the polyline only has two vertices (Starting & Ending points only)
    '--HOOKS-             If Not TypeOf theSketch Is IPolyline OrElse Not IsSketcha2PointLine(theSketch) Then Exit Try
    '--HOOKS- 
    '--HOOKS-             ' Retrieve the map scale from the overlaying Map Index layer
    '--HOOKS-             Dim theCurve As ICurve = DirectCast(theSketch, ICurve)
    '--HOOKS-             Dim theMIFclass As IFeatureClass = MapIndexFeatureLayer.FeatureClass
    '--HOOKS-             Dim theMapScale1 As Object = GetValue(theCurve.FromPoint, theMIFclass, EditorExtension.MapIndexSettings.MapScaleField, EditorExtension.MapIndexSettings.MapNumberField)
    '--HOOKS-             Dim theMapScale2 As Object = GetValue(theCurve.ToPoint, theMIFclass, EditorExtension.MapIndexSettings.MapScaleField, EditorExtension.MapIndexSettings.MapNumberField)
    '--HOOKS- 
    '--HOOKS-             ' Insure that the map scales exist and that they are equal
    '--HOOKS-             If IsDBNull(theMapScale1) OrElse IsDBNull(theMapScale2) Then
    '--HOOKS-                 MessageBox.Show("No mapscale for current MapIndex.  Unable to create hooks", "Hook Error", MessageBoxButtons.OK)
    '--HOOKS-                 Exit Try
    '--HOOKS-             End If
    '--HOOKS-             If Not theMapScale1 Is theMapScale2 Then
    '--HOOKS-                 MessageBox.Show("Hook can not span Mapindex polygons with different scale", "Hook Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '--HOOKS-                 Exit Try
    '--HOOKS-             End If
    '--HOOKS- 
    '--HOOKS-             ' Insures that the map scale is supported -- Not all scales are defined (Issue)
    '--HOOKS-             Dim theLineLength As Integer
    '--HOOKS-             If theMapScale1.Equals(600) Then
    '--HOOKS-                 theLineLength = 20
    '--HOOKS-             ElseIf theMapScale1.Equals(1200) Then
    '--HOOKS-                 theLineLength = 20
    '--HOOKS-             ElseIf theMapScale1.Equals(2400) Then
    '--HOOKS-                 theLineLength = 40
    '--HOOKS-             ElseIf theMapScale1.Equals(4800) Then
    '--HOOKS-                 theLineLength = 80
    '--HOOKS-             ElseIf theMapScale1.Equals(24000) Then
    '--HOOKS-                 theLineLength = 400
    '--HOOKS-             Else
    '--HOOKS-                 MessageBox.Show("Not a valid mapscale.  Unable to create hooks", "Hook Error", MessageBoxButtons.OK)
    '--HOOKS-                 Exit Try
    '--HOOKS-             End If
    '--HOOKS- 
    '--HOOKS-             'Get the hook layer
    '--HOOKS-             Dim theHookFLayer As IFeatureLayer = DataMonitor.CartographicLinesFeatureLayer
    '--HOOKS-             Dim theHookFClass As IFeatureClass = theHookFLayer.FeatureClass
    '--HOOKS- 
    '--HOOKS-             ' Locate the line type field
    '--HOOKS-             Dim lineTypeField As Integer = LocateFields(theHookFClass, (EditorExtension.CartographicLinesSettings.LineTypeField))
    '--HOOKS-             If lineTypeField = NotFoundIndex Then Exit Sub
    '--HOOKS- 
    '--HOOKS-             ' Initialize line objects and collections to create a new line
    '--HOOKS-             Dim theNewPointColl As IPointCollection = New Polyline
    '--HOOKS-             Dim theNormalLine As ILine = New Line
    '--HOOKS-             Dim thePointColl As IPointCollection = DirectCast(theSketch, IPointCollection)
    '--HOOKS- 
    '--HOOKS-             ' Adds the head of the hook based on the specified angle and hook length
    '--HOOKS-             Dim hookLength As Double = theLineLength * 0.1
    '--HOOKS-             Dim sideA As Double = (hookLength * System.Math.Sin((360 - _hookAngle) * (3.14 / 180)))
    '--HOOKS-             Dim sideC As Double = hookLength
    '--HOOKS-             Dim sideB As Double = System.Math.Sqrt((sideC * sideC) - (sideA * sideA))
    '--HOOKS-             theCurve.QueryNormal(ESRI.ArcGIS.Geometry.esriSegmentExtension.esriNoExtension, sideB, False, sideA, theNormalLine)
    '--HOOKS-             theNewPointColl.AddPoint(theNormalLine.ToPoint)
    '--HOOKS- 
    '--HOOKS-             ' Adds the line points
    '--HOOKS-             theNewPointColl.AddPoint(thePointColl.Point(0))
    '--HOOKS-             theNewPointColl.AddPoint(thePointColl.Point(1))
    '--HOOKS- 
    '--HOOKS-             ' Adds the tail of the hook based on the specified angle and hook length
    '--HOOKS-             sideA = (hookLength * System.Math.Sin(_hookAngle * (3.14 / 180)))
    '--HOOKS-             sideC = hookLength
    '--HOOKS-             sideB = System.Math.Sqrt((sideC * sideC) - (sideA * sideA))
    '--HOOKS-             theCurve.QueryNormal(ESRI.ArcGIS.Geometry.esriSegmentExtension.esriNoExtension, (theCurve.Length - sideB), False, sideA, theNormalLine)
    '--HOOKS-             theNewPointColl.AddPoint(theNormalLine.ToPoint)
    '--HOOKS- 
    '--HOOKS-             'Now get rid of the line between the start and end points (where user clicked)
    '--HOOKS-             Dim theWholeLine As IPolyline4 = DirectCast(theNewPointColl, IPolyline4)
    '--HOOKS-             Dim createPart As Boolean
    '--HOOKS-             Dim splitHappened As Boolean
    '--HOOKS-             Dim newPartIndex As Integer
    '--HOOKS-             Dim newSegIndex As Integer
    '--HOOKS-             theWholeLine.SplitAtPoint(_theFromBreakPoint, True, createPart, splitHappened, newPartIndex, newSegIndex)
    '--HOOKS-             theWholeLine.SplitAtPoint(_theToBreakPoint, True, createPart, splitHappened, newPartIndex, newSegIndex)
    '--HOOKS- 
    '--HOOKS-             ' Initialize new path objects and collections to create a new polyline
    '--HOOKS-             Dim path1 As ISegmentCollection = New Path
    '--HOOKS-             Dim path2 As ISegmentCollection = New Path
    '--HOOKS-             Dim path3 As ISegmentCollection = New Path
    '--HOOKS- 
    '--HOOKS-             ' QI to get the segment collection of the landhook
    '--HOOKS-             Dim theSegCollection As ISegmentCollection = DirectCast(theWholeLine, ISegmentCollection)
    '--HOOKS- 
    '--HOOKS-             ' Retreive an enumeration of the segments
    '--HOOKS-             Dim theEnumSeg As IEnumSegment = theSegCollection.EnumSegments
    '--HOOKS- 
    '--HOOKS-             ' Add segments to the paths that will make the final land hook
    '--HOOKS-             Dim theSegment As ISegment = Nothing
    '--HOOKS-             Dim partIndex As Integer
    '--HOOKS-             Dim segmentIndex As Integer
    '--HOOKS-             theEnumSeg.Next(theSegment, partIndex, segmentIndex)
    '--HOOKS-             Do While Not theSegment Is Nothing
    '--HOOKS-                 If segmentIndex < 1 Then
    '--HOOKS-                     path1.AddSegment(theSegment)
    '--HOOKS-                 ElseIf segmentIndex = 1 Then
    '--HOOKS-                     path2.AddSegment(theSegment)
    '--HOOKS-                 ElseIf segmentIndex = 2 Then
    '--HOOKS-                     path3.AddSegment(theSegment)
    '--HOOKS-                 End If
    '--HOOKS-                 theEnumSeg.Next(theSegment, partIndex, segmentIndex)
    '--HOOKS-             Loop
    '--HOOKS- 
    '--HOOKS-             ' Add the component paths to the final land hook
    '--HOOKS-             Dim theGeomColl As IGeometryCollection = New Polyline
    '--HOOKS-             theGeomColl.AddGeometry(DirectCast(path1, IGeometry))
    '--HOOKS-             theGeomColl.AddGeometry(DirectCast(path2, IGeometry))
    '--HOOKS-             theGeomColl.AddGeometry(DirectCast(path3, IGeometry))
    '--HOOKS-             theGeomColl.GeometriesChanged()
    '--HOOKS- 
    '--HOOKS-             ' Store the new land hook feature
    '--HOOKS-             EditorExtension.Editor.StartOperation()
    '--HOOKS-             
    '--HOOKS-             Dim theFeature As IFeature = theHookFClass.CreateFeature
    '--HOOKS-             theFeature.Shape = DirectCast(theGeomColl, IGeometry)
    '--HOOKS-             theFeature.Value(lineTypeField) = 101
    '--HOOKS- 
    '--HOOKS-             ' Set the AutoMethod Field
    '--HOOKS-             lineTypeField = LocateFields(theHookFClass, (EditorExtension.AllTablesSettings.AutoMethodField))
    '--HOOKS-             If lineTypeField = NotFoundIndex Then Exit Sub
    '--HOOKS-             theFeature.Value(lineTypeField) = "UNK"
    '--HOOKS- 
    '--HOOKS-             ' Set the AutoWho Field
    '--HOOKS-             lineTypeField = LocateFields(theHookFClass, (EditorExtension.AllTablesSettings.AutoWhoField))
    '--HOOKS-             If lineTypeField = NotFoundIndex Then Exit Sub
    '--HOOKS-             theFeature.Value(lineTypeField) = UserName
    '--HOOKS- 
    '--HOOKS-             ' Set the AutoDate Field
    '--HOOKS-             lineTypeField = LocateFields(theHookFClass, (EditorExtension.AllTablesSettings.AutoDateField))
    '--HOOKS-             If lineTypeField = NotFoundIndex Then Exit Sub
    '--HOOKS-             theFeature.Value(lineTypeField) = Format(Today, "MM/dd/yyyy")
    '--HOOKS- 
    '--HOOKS-             ' Set the MapScale Field
    '--HOOKS-             lineTypeField = LocateFields(theHookFClass, (EditorExtension.MapIndexSettings.MapScaleField))
    '--HOOKS-             If lineTypeField = NotFoundIndex Then Exit Sub
    '--HOOKS-             theFeature.Value(lineTypeField) = theMapScale1
    '--HOOKS- 
    '--HOOKS-             ' Set the MapNumber Field
    '--HOOKS-             Dim curMapNum As String = GetValue(theFeature.Shape, theMIFclass, EditorExtension.MapIndexSettings.MapNumberField, EditorExtension.MapIndexSettings.MapNumberField)
    '--HOOKS-             lineTypeField = LocateFields(theHookFClass, (EditorExtension.MapIndexSettings.MapNumberField))
    '--HOOKS-             If lineTypeField = NotFoundIndex Then Exit Sub
    '--HOOKS-             theFeature.Value(lineTypeField) = curMapNum
    '--HOOKS- 
    '--HOOKS-             theFeature.Store()
    '--HOOKS-             EditorExtension.Editor.StopOperation("Generate Hooks")
    '--HOOKS- 
    '--HOOKS-             _theActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground, Nothing, theFeature.Extent.Envelope)
    '--HOOKS- 
    '--HOOKS-         Catch ex As Exception
    '--HOOKS-             EditorExtension.Editor.AbortOperation()
    '--HOOKS-             Throw
    '--HOOKS- 
    '--HOOKS-         End Try
    '--HOOKS- 
    '--HOOKS-     End Sub
    '--HOOKS- 
    '--HOOKS-     ''' <summary>
    '--HOOKS-     ''' Determine if a geometry is a two-point line
    '--HOOKS-     ''' </summary>
    '--HOOKS-     ''' <param name="theGeometry">The geometry to evaluate.</param>
    '--HOOKS-     ''' <returns> A boolean value that indicates if the geometry is a either a 
    '--HOOKS-     ''' two-point line or not
    '--HOOKS-     '''</returns>
    '--HOOKS-     ''' <remarks>
    '--HOOKS-     ''' </remarks>
    '--HOOKS-     Public Function IsSketcha2PointLine(ByRef theGeometry As IGeometry) As Boolean
    '--HOOKS- 
    '--HOOKS-         Try
    '--HOOKS-             ' Validate the passed geometry
    '--HOOKS-             If Not TypeOf theGeometry Is IPointCollection Then Exit Function
    '--HOOKS- 
    '--HOOKS-             ' Validate the number of points in the collection
    '--HOOKS-             Dim thePointColl As IPointCollection = DirectCast(theGeometry, IPointCollection)
    '--HOOKS-             If thePointColl.PointCount <> 2 Then
    '--HOOKS-                 MessageBox.Show("When creating the parcel hook only digitize two points", "Hook Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    '--HOOKS-                 Return False
    '--HOOKS-             Else
    '--HOOKS-                 Return True
    '--HOOKS-             End If
    '--HOOKS- 
    '--HOOKS-         Catch ex As Exception
    '--HOOKS-             EditorExtension.ProcessUnhandledException(ex)
    '--HOOKS-             Return False
    '--HOOKS- 
    '--HOOKS-         End Try
    '--HOOKS- 
    '--HOOKS-     End Function
    '-- END HOOKS



    ''' <summary>
    ''' Create a temporary polyline
    ''' </summary>
    ''' <remarks>Given four points, m_pArrowPt1 thru m_pArrowPt4. Create a 
    ''' temporary polyline from the given points and display it as a temporary 
    ''' line on the display
    ''' </remarks>
    Private Sub drawArrows()

        Try
            ' Set up line symbol to display temporary line
            _theLineSymbol = New SimpleLineSymbol
            _theLineSymbol.Width = 2
            Dim theRGBColor As IRgbColor = New RgbColor
            With theRGBColor
                .Red = 223
                .Green = 223
                .Blue = 223
            End With

            _theLineSymbol.Color.RGB = theRGBColor.RGB
            Dim pSymbol As ISymbol = DirectCast(_theLineSymbol, ISymbol)
            pSymbol.ROP2 = esriRasterOpCode.esriROPXOrPen

            ' Create the polyline from a point collection
            Dim theArrowLine As IPointCollection4 = New Polyline

            If arrowType.Equals("Arrow", _ignoreCase) Then
                If _arrowPointsCollection.Count() > 1 Then
                    For i As Integer = 1 To _arrowPointsCollection.Count()
                        theArrowLine.AddPoint(DirectCast(_arrowPointsCollection.Item(i), IPoint))
                    Next
                End If
            Else
                If Not _theArrowPt1 Is Nothing Then theArrowLine.AddPoint(_theArrowPt1)
                If Not _theArrowPt2 Is Nothing Then theArrowLine.AddPoint(_theArrowPt2)
                If Not _theArrowPt3 Is Nothing Then theArrowLine.AddPoint(_theArrowPt3)
                If Not _theArrowPt4 Is Nothing Then theArrowLine.AddPoint(_theArrowPt4)
            End If

            Dim theArrowLine2 As IPolyline4 = DirectCast(theArrowLine, IPolyline4)

            _theActiveView.ScreenDisplay.SetSymbol(DirectCast(_theLineSymbol, ISymbol))
            If (theArrowLine2.Length > 0) Then _theActiveView.ScreenDisplay.DrawPolyline(theArrowLine2)
            _theActiveView.ScreenDisplay.FinishDrawing()

        Catch ex As Exception
            EditorExtension.ProcessUnhandledException(ex)

        End Try

    End Sub

    ''' <summary>
    ''' Get the mousepoint using ISnapAgent
    ''' </summary>
    ''' <param name="X">An object that supports the IGeometry interface.</param>
    ''' <param name="Y">An object that supports the IGeometry interface.</param>
    ''' <remarks>
    ''' </remarks>
    Private Sub getMousePoint(ByRef X As Integer, ByRef Y As Integer)

        Try
            ' Get the current map point (and invert the agent at that location)
            _thePt = _theActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y)

            ' Get the snap agent, if it is being used
            _theSnapAgent = Nothing

            Dim theSnapenv As ISnapEnvironment = DirectCast(EditorExtension.Editor, ISnapEnvironment)

            Dim theSnapAgent As ISnapAgent
            Dim theFSnapAgent As IFeatureSnapAgent
            Dim theEdLyrs As IEditLayers
            Dim theLayer As ILayer
            Dim theGeometryHitPartType As esriGeometryHitPartType

            For i As Integer = 0 To theSnapenv.SnapAgentCount - 1
                theSnapAgent = theSnapenv.SnapAgent(i)
                If TypeOf theSnapAgent Is IFeatureSnapAgent Then
                    theFSnapAgent = DirectCast(theSnapAgent, IFeatureSnapAgent)
                    theEdLyrs = DirectCast(EditorExtension.Editor, IEditLayers)
                    theLayer = theEdLyrs.CurrentLayer
                    theGeometryHitPartType = theFSnapAgent.HitType
                    If theGeometryHitPartType <> 0 Then
                        _theSnapAgent = theFSnapAgent
                        Exit For
                    End If
                    theLayer = Nothing
                    theEdLyrs = Nothing
                    theFSnapAgent = Nothing
                End If
            Next i

        Catch ex As Exception
            EditorExtension.ProcessUnhandledException(ex)

        End Try

    End Sub

    ''' <summary>
    ''' Get the mapscale of a temp feature
    ''' </summary>
    ''' <param name="theMIFeatureClass">The Mapindex feature class.</param>
    ''' <returns> A string representing the map scale
    '''</returns>
    ''' <remarks>
    ''' </remarks>
    Private Function getCurrentMapScale(ByRef theMIFeatureClass As IFeatureClass) As String

        Try
            Dim theDimensionArrowLayerTemp As IFeatureLayer = DataMonitor.CartographicLinesFeatureLayer
            Dim theDimensionArrowFCTemp As IFeatureClass = theDimensionArrowLayerTemp.FeatureClass
            Dim theDimensionDSetTemp As IDataset = DirectCast(theDimensionArrowFCTemp, IDataset)
            Dim theDimensionWSEditTemp As IWorkspaceEdit = DirectCast(theDimensionDSetTemp.Workspace, IWorkspaceEdit)

            ' Create the arrow feature
            theDimensionWSEditTemp.StartEditOperation()
            Dim theDimensionFeatureTemp As IFeature = theDimensionArrowFCTemp.CreateFeature
            Dim theDimensionpointsTemp As ESRI.ArcGIS.Geometry.IPointCollection4
            theDimensionpointsTemp = New ESRI.ArcGIS.Geometry.Polyline
            theDimensionpointsTemp.AddPoint(_theArrowPt1)
            theDimensionpointsTemp.AddPoint(_theArrowPt2)

            Dim theDimensionLineTemp As ESRI.ArcGIS.Geometry.IPolyline
            theDimensionLineTemp = DirectCast(theDimensionpointsTemp, IPolyline)
            theDimensionFeatureTemp.Shape = theDimensionLineTemp


            ' Get the current MapNumber
            Dim currentMapScale As String
            currentMapScale = GetValue((theDimensionFeatureTemp.Shape), theMIFeatureClass, EditorExtension.MapIndexSettings.MapScaleField, EditorExtension.MapIndexSettings.MapNumberField)
            Return CStr(CDbl(currentMapScale) / 12)

            theDimensionWSEditTemp.AbortEditOperation()
            theDimensionWSEditTemp.StopEditOperation()

        Catch ex As Exception
            EditorExtension.ProcessUnhandledException(ex)
            Return Nothing

        End Try

    End Function

    ''' <summary>
    ''' Get the side of the line the dimension arrows will be placed on; right or left
    ''' </summary>
    ''' <returns> A string representing the side of the line the third clicked point is on
    '''</returns>
    ''' <remarks>
    ''' </remarks>
    Private Function getDimensionArrowSide() As String

        Try
            ' Determine point location is on the left or right by Dean Anderson, help of Nate Anderson
            Dim lineSlope As Double = 0
            If _theArrowPt2.Y <> _theArrowPt1.Y Then
                lineSlope = (_theArrowPt1.Y - _theArrowPt2.Y) / (_theArrowPt1.X - _theArrowPt2.X)
            End If

            Dim yIntercept As Double = _theArrowPt1.Y - (lineSlope * _theArrowPt1.X)
            Dim z As Double = (lineSlope * _theArrowPt3.X) + yIntercept - _theArrowPt3.Y

            If _theArrowPt1.X = _theArrowPt2.X Then 'vertical
                If _theArrowPt3.X = _theArrowPt1.X Then z = 0
                If _theArrowPt1.Y < _theArrowPt2.Y Then 'going up
                    If _theArrowPt3.X > _theArrowPt1.X Then z = 1
                    If _theArrowPt3.X < _theArrowPt1.X Then z = -1
                End If
                If _theArrowPt1.Y > _theArrowPt2.Y Then 'going down
                    If _theArrowPt3.X < _theArrowPt1.X Then z = 1
                    If _theArrowPt3.X > _theArrowPt1.X Then z = -1
                End If
            End If
            If _theArrowPt1.Y = _theArrowPt2.Y Then 'horizontal
                If _theArrowPt3.Y = _theArrowPt1.Y Then z = 0
                If _theArrowPt1.X < _theArrowPt2.X Then 'going right
                    If _theArrowPt3.Y > _theArrowPt1.Y Then z = -1
                    If _theArrowPt3.Y < _theArrowPt1.Y Then z = 1
                End If
                If _theArrowPt1.X > _theArrowPt2.X Then 'going left
                    If _theArrowPt3.Y < _theArrowPt1.Y Then z = -1
                    If _theArrowPt3.Y > _theArrowPt1.Y Then z = 1
                End If
            End If

            Dim dimensionArrowSide As String = String.Empty
            If z < 0 Then dimensionArrowSide = "left"
            If z > 0 Then dimensionArrowSide = "right"
            If z = 0 Then dimensionArrowSide = "left" '"online"

            If (_theArrowPt1.X > _theArrowPt2.X AndAlso _theArrowPt1.Y > _theArrowPt2.Y) OrElse (_theArrowPt1.X > _theArrowPt2.X AndAlso _theArrowPt2.Y > _theArrowPt1.Y) Then
                If z > 0 Then dimensionArrowSide = "left"
                If z < 0 Then dimensionArrowSide = "right"
                If z = 0 Then dimensionArrowSide = "left" '"online"
            End If

            Return dimensionArrowSide

        Catch ex As Exception
            EditorExtension.ProcessUnhandledException(ex)
            Return Nothing

        End Try

    End Function


    ''' <summary>
    ''' Get the size of the dimension arrows so that all are standard
    ''' </summary>
    ''' <param name="theCurrentMapScale">The current map scale.</param>
    ''' <returns> 
    '''</returns>
    ''' <remarks>
    ''' </remarks>
    Private Function getChange(ByRef theCurrentMapScale As String) As Short

        Try

            Dim theChange As Short = Nothing
            If theCurrentMapScale = "100" Then
                theChange = 15
            ElseIf theCurrentMapScale = "200" Then
                theChange = 30
            ElseIf theCurrentMapScale = "400" Then
                theChange = 60
            ElseIf theCurrentMapScale = "2000" Then
                theChange = 300
            End If

            Return theChange

        Catch ex As Exception
            EditorExtension.ProcessUnhandledException(ex)
            Return Nothing

        End Try

    End Function

    ''' <summary>
    ''' Converts a domain descriptive value to the stored code
    ''' </summary>
    ''' <param name="theFields">An object that supports the IFields interface.</param>
    ''' <param name="fieldName">A field that exists in pFields.</param>
    ''' <param name="codedValue">A coded value to covert to a coded name.</param>
    ''' <returns>A string that represents the domain coded name that corresponds with 
    ''' the coded value, vVal, or a zero-length string.
    '''</returns>
    ''' <remarks>Given a field, sFldName, in a collection of fields, pFields, and a coded 
    ''' value name, vVal.  Locates the sFldName in pFields and gets a reference to the field's 
    ''' domain, and then finds the coded name in the domain that corresponds to the coded value vVal
    ''' </remarks>
    Public Function ConvertToDescription(ByVal theFields As IFields, ByVal fieldName As String, ByVal codedValue As String) As String

        Try

            Dim fieldNumber As Integer = theFields.FindField(fieldName)
            If fieldNumber > NotFoundIndex Then
                'Determine if domain field
                Dim theField As IField = theFields.Field(fieldNumber)
                Dim theDomain As IDomain = theField.Domain
                If theDomain Is Nothing Then
                    Return codedValue
                    Exit Function
                Else
                    'Determine type of domain  -If Coded Value, get the description
                    If TypeOf theDomain Is ICodedValueDomain Then
                        Dim theCVDomain As ICodedValueDomain = DirectCast(theDomain, ICodedValueDomain)
                        'Given the description, search the domain for the code
                        For i As Integer = 0 To theCVDomain.CodeCount - 1
                            If theCVDomain.Value(i).Equals(codedValue) Then
                                ConvertToDescription = theCVDomain.Name(i) 'Return the code value
                                Exit Function
                            End If
                        Next i
                    Else ' If range domain, return the numeric value
                        Return codedValue
                        Exit Function
                    End If
                End If
                Return codedValue
            Else
                'Field not found
                Return String.Empty
            End If

        Catch ex As Exception
            EditorExtension.ProcessUnhandledException(ex)
            Return Nothing

        End Try

    End Function

#End Region

#End Region

#Region "Inherited Class Members"

#Region "Properties"

    ''' <summary>
    ''' Called by ArcMap once per second to check if the command is enabled.
    ''' </summary>
    ''' <remarks>WARNING: Do not put computation-intensive code here.</remarks>
    Public Overrides ReadOnly Property Enabled() As Boolean
        Get
            Try
                Dim canEnable As Boolean
                canEnable = EditorExtension.CanEnableExtendedEditing
                canEnable = canEnable AndAlso EditorExtension.Editor.EditState = esriEditState.esriStateEditing
                canEnable = canEnable AndAlso EditorExtension.IsValidWorkspace
                Return canEnable
            Catch ex As Exception
                EditorExtension.ProcessUnhandledException(ex)
            End Try
        End Get
    End Property

#End Region

#Region "Methods"

    Public Overrides Sub OnCreate(ByVal hook As Object)
        Try
            If Not hook Is Nothing Then

                'Disable tool if parent application is not ArcMap
                If TypeOf hook Is IMxApplication Then
                    _application = DirectCast(hook, IApplication)
                    setPartnerAddArrowsForm(New AddArrowsForm())
                    setPartnerDimensionArrowsForm(New DimensionArrowsForm())
                    MyBase.m_enabled = True
                Else
                    MyBase.m_enabled = False
                    'Disable tool if parent application is not ArcMap
                    If TypeOf hook Is IMxApplication Then
                        _application = DirectCast(hook, IApplication)
                        setPartnerAddArrowsForm(New AddArrowsForm())
                        setPartnerDimensionArrowsForm(New DimensionArrowsForm())
                        MyBase.m_enabled = True
                    Else
                        MyBase.m_enabled = False
                    End If
                End If

            End If
            ' NOTE: Add other initialization code here...

        Catch ex As Exception
            EditorExtension.ProcessUnhandledException(ex)
        End Try

    End Sub

    Public Overrides Sub OnClick()
        Try
            DoButtonOperation()

        Catch ex As Exception
            EditorExtension.ProcessUnhandledException(ex)

        End Try
    End Sub

    Public Overrides Sub OnMouseDown(ByVal Button As Integer, ByVal Shift As Integer, ByVal X As Integer, ByVal Y As Integer)

        If PartnerAddArrowsForm.Visible Then Exit Sub

        Try

            ' Recycled variables
            Dim theField As Integer
            Dim currentMapScale As String
            Dim finishedDrawing As Boolean
            Dim indexNumber As Long

            ' Set the in use flag
            _inUse = True

            ' Retrieve a reference to the Map Index layer
            Dim theMIFeatureClass As IFeatureClass = MapIndexFeatureLayer.FeatureClass

            Select Case arrowType

                '-- HOOKS NOT IMPLEMENTED --
                '--HOOKS-                 Case "HOOK"  'If drawing hooks
                '--HOOKS-                     ' Get point to measure distance from
                '--HOOKS-                     _theStartPoint = _theActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y)
                '--HOOKS-                     _doOnce = False
                '--HOOKS-                     _theFromBreakPoint = _theStartPoint
                '--HOOKS-                     ' Get the scale of the current mapindex
                '--HOOKS-                     Dim mapIndexScale As String = GetValue(_theStartPoint, theMIFeatureClass, EditorExtension.MapIndexSettings.MapScaleField, EditorExtension.MapIndexSettings.MapNumberField)
                '--HOOKS-                     mapIndexScale = ConvertToDescription(theMIFeatureClass.Fields, EditorExtension.MapIndexSettings.MapScaleField, mapIndexScale)
                '-- END HOOKS

                Case "ARROW" 'If drawing annotation arrows
                    If finishedDrawing = False Then
                        If Button = 1 AndAlso Shift = 1 Then 'Right mouse click
                            'Save the first point
                            If _arrowPointsCollection.Count = 0 Then
                                If _theArrowPt1 Is Nothing Then
                                    _theArrowPt1 = _theActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y)
                                    _arrowPointsCollection.Add(_theArrowPt1)

                                    'Clear existing point
                                    _theArrowPt1 = Nothing
                                End If

                                'Save the last point
                            Else
                                If _theArrowPt1 Is Nothing Then
                                    _theArrowPt1 = _theActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y)
                                    _arrowPointsCollection.Add(_theArrowPt1)
                                    drawArrows()

                                    'Clear existing point
                                    _theArrowPt1 = Nothing
                                    finishedDrawing = True
                                End If
                            End If

                            'Left mouse click
                        ElseIf Button = 1 AndAlso Shift = 0 Then
                            'Add vertex
                            If _theArrowPt1 Is Nothing Then
                                _theArrowPt1 = _theActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y)
                                _arrowPointsCollection.Add(_theArrowPt1)

                                'Clear existing point
                                _theArrowPt1 = Nothing
                            End If
                        End If
                    End If


                    If _arrowPointsCollection.Count > 1 AndAlso finishedDrawing = True Then

                        ' Creates a new polygon from the points and smoothes it
                        Dim theArrowPoints As IPointCollection4 = New Polyline

                        For i As Integer = 1 To _arrowPointsCollection.Count
                            theArrowPoints.AddPoint(DirectCast(_arrowPointsCollection.Item(i), IPoint))
                        Next

                        Dim theArrowLine As IPolyline4 = DirectCast(theArrowPoints, IPolyline4)
                        theArrowLine.Smooth(theArrowLine.Length / 10)

                        ' Get a reference to the Cartographic Lines feature class
                        Dim theArrowLayer As IFeatureLayer = DataMonitor.CartographicLinesFeatureLayer
                        Dim theArrowFeatureClass As IFeatureClass = theArrowLayer.FeatureClass
                        Dim theDataSet As IDataset = DirectCast(theArrowFeatureClass, IDataset)
                        Dim theWorkSpaceEdit As IWorkspaceEdit = DirectCast(theDataSet.Workspace, IWorkspaceEdit)

                        ' Start an edit operation to encompass the creation of the feature
                        theWorkSpaceEdit.StartEditOperation()

                        ' Create the arrow feature
                        Dim theFeature As IFeature = theArrowFeatureClass.CreateFeature
                        theFeature.Shape = theArrowLine

                        ' Locates fields in the feature's dataset
                        Dim mapNumberField As Integer = LocateFields(theArrowFeatureClass, EditorExtension.MapIndexSettings.MapNumberField)
                        theField = LocateFields(theArrowFeatureClass, EditorExtension.CartographicLinesSettings.LineTypeField)
                        If theField = NotFoundIndex Then Exit Sub
                        ' Populate field values in the feature
                        Dim curMapNum As String = GetValue(theFeature.Shape, theMIFeatureClass, EditorExtension.MapIndexSettings.MapNumberField, EditorExtension.MapIndexSettings.MapNumberField)
                        theFeature.Value(mapNumberField) = curMapNum
                        theFeature.Value(theField) = arrowLineStyle
                        theFeature.Store()

                        ' Set the AutoMethod Field
                        theField = LocateFields(theArrowFeatureClass, EditorExtension.AllTablesSettings.AutoMethodField)
                        If theField = NotFoundIndex Then Exit Sub
                        theFeature.Value(theField) = "UNK"

                        ' Set the AutoWho Field
                        theField = LocateFields(theArrowFeatureClass, EditorExtension.AllTablesSettings.AutoWhoField)
                        If theField = NotFoundIndex Then Exit Sub
                        theFeature.Value(theField) = OrmapTaxlotEditing.Utilities.UserName

                        ' Set the AutoDate Field
                        theField = LocateFields(theArrowFeatureClass, EditorExtension.AllTablesSettings.AutoDateField)
                        If theField = NotFoundIndex Then Exit Sub
                        theFeature.Value(theField) = Format(Today, "MM/dd/yyyy")

                        ' Set the MapScale Field
                        currentMapScale = GetValue(theFeature.Shape, theMIFeatureClass, EditorExtension.MapIndexSettings.MapScaleField, EditorExtension.MapIndexSettings.MapNumberField)
                        theField = LocateFields(theArrowFeatureClass, EditorExtension.MapIndexSettings.MapScaleField)
                        If theField = NotFoundIndex Then Exit Sub
                        theFeature.Value(theField) = currentMapScale

                        ' Finalize the edit operation
                        theWorkSpaceEdit.StopEditOperation()

                        ' Refresh the display
                        _theActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground, Nothing, theFeature.Extent.Envelope)

                        _arrowPointsCollection = Nothing
                        _arrowPointsCollection = New Collection


                    End If

                Case "DIMENSION"

                    If _theArrowPt1 Is Nothing Then
                        _theArrowPt1 = _thePt
                    ElseIf _theArrowPt2 Is Nothing Then
                        _theArrowPt2 = _thePt
                        drawArrows()
                    ElseIf _theArrowPt3 Is Nothing Then
                        _theArrowPt3 = _theActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y)
                        drawArrows()

                        ' Check to be sure the 2 points are not equal
                        If (_theArrowPt1.X = _theArrowPt2.X) AndAlso (_theArrowPt1.Y = _theArrowPt2.Y) Then
                            MessageBox.Show("Two input points can't be equal, dimension arrows terminated.", "Dimension Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            OnKeyDown(Keys.Q, 1)
                            Exit Try
                        End If

                        ' Get the mapscale
                        currentMapScale = getCurrentMapScale(theMIFeatureClass)

                        ' Get the side the dimension arrows should be on based upon the 3rd input point
                        Dim dimensionArrowSide As String = getDimensionArrowSide()

                        ' Create two dimension arrows, one on the left and one on the right
                        For indexNumber = 1 To 2
                            Dim theDimensionLine As IPolyline4
                            Dim theDimensionPoints As IPointCollection4
                            theDimensionPoints = New Polyline

                            If _addManually = False Then

                                ' Add starting point
                                theDimensionPoints.AddPoint(_theArrowPt1)

                                ' Instanciate the temp points
                                _theArrowPtTemp = New ESRI.ArcGIS.Geometry.Point
                                _theArrowPtTemp2 = New ESRI.ArcGIS.Geometry.Point

                                ' Set the distance of change for the dimension arrows based upon the mapscale
                                Dim iChange As Integer = getChange(currentMapScale)

                                ' Get 3 calculated points for a hook and create the line from input
                                theDimensionPoints.AddPoint(_theArrowPt1)
                                theDimensionPoints.AddPoint(_theArrowPt2)
                                theDimensionLine = DirectCast(theDimensionPoints, IPolyline4)

                                'Create a line iChange from the beginning
                                Dim theLine As ILine = New Line
                                If indexNumber = 1 Then
                                    If Shift = 0 Then 'shift not pressed
                                        If Trim$(UCase$(dimensionArrowSide)) = "RIGHT" Then
                                            theDimensionLine.QueryNormal(esriSegmentExtension.esriExtendAtFrom, iChange, False, ((iChange / RatioLine) / 2), theLine)
                                        ElseIf Trim$(UCase$(dimensionArrowSide)) = "LEFT" Then
                                            theDimensionLine.QueryNormal(esriSegmentExtension.esriExtendAtFrom, iChange, False, -((iChange / RatioLine) / 2), theLine)
                                        End If
                                    ElseIf Shift = 1 Then 'shift pressed
                                        If Trim$(UCase$(dimensionArrowSide)) = "RIGHT" Then
                                            theDimensionLine.QueryNormal(esriSegmentExtension.esriExtendAtFrom, iChange, False, (((iChange * 2) / RatioLine) / 2), theLine)
                                        ElseIf Trim$(UCase$(dimensionArrowSide)) = "LEFT" Then
                                            theDimensionLine.QueryNormal(esriSegmentExtension.esriExtendAtFrom, iChange, False, -(((iChange * 2) / RatioLine) / 2), theLine)
                                        End If
                                    End If
                                ElseIf indexNumber = 2 Then
                                    If Shift = 0 Then 'shift not pressed
                                        If Trim$(UCase$(dimensionArrowSide)) = "RIGHT" Then
                                            theDimensionLine.QueryNormal(esriSegmentExtension.esriExtendAtFrom, (theDimensionLine.Length - iChange), False, ((iChange / RatioLine) / 2), theLine)
                                        ElseIf Trim$(UCase$(dimensionArrowSide)) = "LEFT" Then
                                            theDimensionLine.QueryNormal(esriSegmentExtension.esriExtendAtFrom, (theDimensionLine.Length - iChange), False, -((iChange / RatioLine) / 2), theLine)
                                        End If
                                    ElseIf Shift = 1 Then 'shift pressed
                                        If Trim$(UCase$(dimensionArrowSide)) = "RIGHT" Then
                                            theDimensionLine.QueryNormal(esriSegmentExtension.esriExtendAtFrom, (theDimensionLine.Length - iChange), False, (((iChange * 2) / RatioLine) / 2), theLine)
                                        ElseIf Trim$(UCase$(dimensionArrowSide)) = "LEFT" Then
                                            theDimensionLine.QueryNormal(esriSegmentExtension.esriExtendAtFrom, (theDimensionLine.Length - iChange), False, -(((iChange * 2) / RatioLine) / 2), theLine)

                                        End If
                                    End If
                                End If

                                ' Save the to and from points of the line
                                _theArrowPtTemp.X = theLine.ToPoint.X
                                _theArrowPtTemp.Y = theLine.ToPoint.Y
                                theLine = Nothing

                                ' Create a line (iChange/.25) from the beginning
                                theLine = New Line
                                If indexNumber = 1 Then
                                    If Shift = 0 Then 'shift not pressed 
                                        'jwm use this: If String.Compare(dimensionArrowSide, "RIGHT", True, System.Globalization.CultureInfo.CurrentCulture) = 0 Then
                                        If Trim$(UCase$(dimensionArrowSide)) = "RIGHT" Then
                                            theDimensionLine.QueryNormal(esriSegmentExtension.esriExtendAtFrom, (iChange / RatioCurve), False, ((iChange / RatioLine) / 1.75), theLine)
                                        ElseIf Trim$(UCase$(dimensionArrowSide)) = "LEFT" Then
                                            theDimensionLine.QueryNormal(esriSegmentExtension.esriExtendAtFrom, (iChange / RatioCurve), False, -((iChange / RatioLine) / 1.75), theLine)
                                        End If
                                    ElseIf Shift = 1 Then 'shift pressed
                                        If Trim$(UCase$(dimensionArrowSide)) = "RIGHT" Then
                                            theDimensionLine.QueryNormal(esriSegmentExtension.esriExtendAtFrom, (iChange / RatioCurve), False, (((iChange * 2) / RatioLine) / 1.75), theLine)
                                        ElseIf Trim$(UCase$(dimensionArrowSide)) = "LEFT" Then
                                            theDimensionLine.QueryNormal(esriSegmentExtension.esriExtendAtFrom, (iChange / RatioCurve), False, -(((iChange * 2) / RatioLine) / 1.75), theLine)
                                        End If
                                    End If
                                ElseIf indexNumber = 2 Then
                                    If Shift = 0 Then 'shift not pressed
                                        If Trim$(UCase$(dimensionArrowSide)) = "RIGHT" Then
                                            theDimensionLine.QueryNormal(esriSegmentExtension.esriExtendAtFrom, (theDimensionLine.Length - (iChange / RatioCurve)), False, ((iChange / RatioLine) / 1.75), theLine)
                                        ElseIf Trim$(UCase$(dimensionArrowSide)) = "LEFT" Then
                                            theDimensionLine.QueryNormal(esriSegmentExtension.esriExtendAtFrom, (theDimensionLine.Length - (iChange / RatioCurve)), False, -((iChange / RatioLine) / 1.75), theLine)
                                        End If
                                    ElseIf Shift = 1 Then 'shift pressed
                                        If Trim$(UCase$(dimensionArrowSide)) = "RIGHT" Then
                                            theDimensionLine.QueryNormal(esriSegmentExtension.esriExtendAtFrom, (theDimensionLine.Length - (iChange / RatioCurve)), False, (((iChange * 2) / RatioLine) / 1.75), theLine)
                                        ElseIf Trim$(UCase$(dimensionArrowSide)) = "LEFT" Then
                                            theDimensionLine.QueryNormal(esriSegmentExtension.esriExtendAtFrom, (theDimensionLine.Length - (iChange / RatioCurve)), False, -(((iChange * 2) / RatioLine) / 1.75), theLine)
                                        End If
                                    End If
                                End If

                                ' Save the to and from points of the line
                                _theArrowPtTemp2.X = theLine.ToPoint.X
                                _theArrowPtTemp2.Y = theLine.ToPoint.Y
                                theLine = Nothing

                                theDimensionPoints = Nothing
                                theDimensionPoints = New Polyline

                                ' Add the points to the line/hook to be created
                                If indexNumber = 1 Then
                                    theDimensionPoints.AddPoint(_theArrowPt1)
                                ElseIf indexNumber = 2 Then
                                    theDimensionPoints.AddPoint(_theArrowPt2)
                                End If
                                theDimensionPoints.AddPoint(_theArrowPtTemp2)
                                theDimensionPoints.AddPoint(_theArrowPtTemp)

                            Else 'Adding dimension arrow manually
                                indexNumber = 3
                                theDimensionPoints.AddPoint(_theArrowPt1)
                                theDimensionPoints.AddPoint(_theArrowPt2)
                                theDimensionPoints.AddPoint(_theArrowPt3)

                            End If

                            ' Create the dimension arrow from the collection of 3 points (1 input, 2 calculated)
                            theDimensionLine = DirectCast(theDimensionPoints, IPolyline4)

                            If SmoothRatio > 0 Then
                                theDimensionLine.Smooth(theDimensionLine.Length / SmoothRatio)
                            End If

                            Dim theDimensionArrowLayer As IFeatureLayer = DataMonitor.CartographicLinesFeatureLayer
                            Dim theDimensionArrowFC As IFeatureClass = theDimensionArrowLayer.FeatureClass
                            Dim theDimensionDSet As IDataset = DirectCast(theDimensionArrowFC, IDataset)
                            Dim theDimensionWSEdit As IWorkspaceEdit = DirectCast(theDimensionDSet.Workspace, IWorkspaceEdit)

                            ' Create the arrow feature
                            theDimensionWSEdit.StartEditOperation()

                            Dim theDimensionFeature As IFeature
                            theDimensionFeature = theDimensionArrowFC.CreateFeature


                            theDimensionFeature.Shape = theDimensionLine

                            theField = LocateFields(theDimensionArrowFC, EditorExtension.CartographicLinesSettings.LineTypeField)
                            If theField = NotFoundIndex Then Exit Sub
                            theDimensionFeature.Value(theField) = 134 'Bearing Distance Arrow

                            ' Get the current MapNumber
                            Dim currentMapNums As String
                            currentMapNums = GetValue(theDimensionFeature.Shape, theMIFeatureClass, EditorExtension.MapIndexSettings.MapNumberField, EditorExtension.MapIndexSettings.MapNumberField)
                            theField = LocateFields(theDimensionArrowFC, EditorExtension.MapIndexSettings.MapNumberField)
                            theDimensionFeature.Value(theField) = currentMapNums

                            ' Set the AutoMethod Field
                            theField = LocateFields(theDimensionArrowFC, EditorExtension.AllTablesSettings.AutoMethodField)
                            If theField = NotFoundIndex Then Exit Sub
                            theDimensionFeature.Value(theField) = "UNK"

                            ' Set the AutoWho Field
                            theField = LocateFields(theDimensionArrowFC, EditorExtension.AllTablesSettings.AutoWhoField)
                            If theField = NotFoundIndex Then Exit Sub
                            theDimensionFeature.Value(theField) = OrmapTaxlotEditing.Utilities.UserName

                            ' Set the AutoDate Field
                            theField = LocateFields(theDimensionArrowFC, EditorExtension.AllTablesSettings.AutoDateField)
                            If theField = NotFoundIndex Then Exit Sub
                            theDimensionFeature.Value(theField) = Format(Today, "MM/dd/yyyy")

                            ' Set the MapScale Field
                            theField = LocateFields(theDimensionArrowFC, EditorExtension.MapIndexSettings.MapScaleField)
                            If theField = NotFoundIndex Then Exit Sub
                            theDimensionFeature.Value(theField) = CInt(currentMapScale) * 12

                            theDimensionFeature.Store()
                            theDimensionWSEdit.StopEditOperation()

                            ' Refresh the display
                            _theActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground, Nothing, theDimensionFeature.Extent.Envelope)

                            _theArrowPtTemp = Nothing
                            _theArrowPtTemp2 = Nothing
                            _theTextSymbol = Nothing
                            _theTextPoint = Nothing
                            _theLinePolyline = Nothing
                            _theLineSymbol = Nothing

                        Next

                        _theArrowPt1 = Nothing
                        _theArrowPt2 = Nothing
                        _theArrowPt3 = Nothing
                        _theArrowPt4 = Nothing

                    Else ' Reset everything
                        _theArrowPt1 = Nothing
                        _theArrowPt2 = Nothing
                        _theArrowPt3 = Nothing
                        _theArrowPt4 = Nothing
                        _theArrowPtTemp = Nothing
                        _theArrowPtTemp2 = Nothing
                        _theSnapAgent = Nothing
                        _toolJustCompletedTask = True

                        ' Deactivate the tool
                        _application.CurrentTool = Nothing

                    End If
            End Select


        Catch ex As Exception
            EditorExtension.ProcessUnhandledException(ex)

        End Try



    End Sub



    Public Overrides Sub onmousemove(ByVal button As Integer, ByVal shift As Integer, ByVal x As Integer, ByVal y As Integer)
        MyBase.OnMouseMove(button, shift, x, y)

        Try
            '-- HOOKS NOT IMPLEMENTED --
            '--HOOKS-             'draw the temporary line the user sees while moving the mouse
            '--HOOKS-             If arrowType.Equals("hook", _ignoreCase) Then
            '--HOOKS- 
            '--HOOKS-                 Dim firsttime As Boolean
            '--HOOKS-                 Dim linelength As Long = 0 'todo - sc this does not get set... what's the deal... maybe this is why its not drawing on screen??
            '--HOOKS-                 If (Not _inUse) Then Exit Try
            '--HOOKS- 
            '--HOOKS-                 ' checks to see if the line symbol is defined
            '--HOOKS-                 If (_theLineSymbol Is Nothing) Then firsttime = True
            '--HOOKS- 
            '--HOOKS-                 ' get current point
            '--HOOKS-                 Dim thepoint As IPoint = _theActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y)
            '--HOOKS-                 _theToBreakPoint = thepoint 'the unextended to point used to break the hooks
            '--HOOKS- 
            '--HOOKS-                 ' check to be sure there is a start point for the hook to prevent an error
            '--HOOKS-                 If _theStartPoint Is Nothing Then Exit Try
            '--HOOKS- 
            '--HOOKS-                 ' draw a virtual line that represents the extended line
            '--HOOKS-                 Dim thepolyline As IPolyline = New Polyline
            '--HOOKS-                 thepolyline.FromPoint = _theStartPoint
            '--HOOKS-                 thepolyline.ToPoint = thepoint
            '--HOOKS-                 Dim thecurve As ICurve = thepolyline
            '--HOOKS- 
            '--HOOKS-                 Dim theconstructpoint1 As IConstructPoint = New ESRI.ArcGIS.Geometry.Point
            '--HOOKS-                 theconstructpoint1.ConstructAlong(thecurve, esriSegmentExtension.esriExtendAtTo, thecurve.Length + CDbl(linelength), False)
            '--HOOKS-                 thepoint = DirectCast(theconstructpoint1, IPoint)
            '--HOOKS- 
            '--HOOKS-                 If Not _doOnce Then
            '--HOOKS-                     Dim theconstructpoint2 As IConstructPoint = New ESRI.ArcGIS.Geometry.Point
            '--HOOKS-                     theconstructpoint2.ConstructAlong(thecurve, esriSegmentExtension.esriExtendAtFrom, -(CDbl(linelength)), False)
            '--HOOKS-                     _theStartPoint = DirectCast(theconstructpoint2, IPoint)
            '--HOOKS-                     _doOnce = True
            '--HOOKS-                 End If
            '--HOOKS- 
            '--HOOKS-                 ' draw the line
            '--HOOKS-                 _theActiveView.ScreenDisplay.StartDrawing(_theActiveView.ScreenDisplay.hDC, -1)
            '--HOOKS- 
            '--HOOKS-                 ' initialize or draw the temporary line
            '--HOOKS-                 If firsttime Then
            '--HOOKS-                     ' line symbol
            '--HOOKS-                     _theLineSymbol = New SimpleLineSymbol
            '--HOOKS-                     _theLineSymbol.Width = 2
            '--HOOKS-                     Dim thergbcolor As IRgbColor = New RgbColor
            '--HOOKS-                     With thergbcolor
            '--HOOKS-                         .Red = 223
            '--HOOKS-                         .Green = 223
            '--HOOKS-                         .Blue = 223
            '--HOOKS-                     End With
            '--HOOKS-                     _theLineSymbol.Color = thergbcolor
            '--HOOKS-                     Dim thesymbol As ISymbol = DirectCast(_theLineSymbol, ISymbol)
            '--HOOKS-                     thesymbol.ROP2 = esriRasterOpCode.esriROPXOrPen
            '--HOOKS- 
            '--HOOKS-                     ' text symbol
            '--HOOKS-                     _theTextSymbol = New ESRI.ArcGIS.Display.TextSymbol
            '--HOOKS-                     _theTextSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter
            '--HOOKS-                     _theTextSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter
            '--HOOKS-                     _theTextSymbol.Size = 16
            '--HOOKS-                     _theTextSymbol.Font.Name = "arial"
            '--HOOKS- 
            '--HOOKS-                     thesymbol = DirectCast(_theTextSymbol, ISymbol)
            '--HOOKS-                     Dim thefont As Font = DirectCast(_theTextSymbol.Font, Font)
            '--HOOKS-                     thesymbol.ROP2 = esriRasterOpCode.esriROPXOrPen
            '--HOOKS- 
            '--HOOKS-                     ' create point to draw text in
            '--HOOKS-                     _theTextPoint = New ESRI.ArcGIS.Geometry.Point
            '--HOOKS-                 Else
            '--HOOKS-                     ' use existing symbols and draw existing text and polyline
            '--HOOKS-                     _theActiveView.ScreenDisplay.SetSymbol(DirectCast(_theTextSymbol, ISymbol))
            '--HOOKS-                     _theActiveView.ScreenDisplay.DrawText(_theTextPoint, _theTextSymbol.Text)
            '--HOOKS-                     _theActiveView.ScreenDisplay.SetSymbol(DirectCast(_theLineSymbol, ISymbol))
            '--HOOKS-                     If (_theLinePolyline.Length > 0) Then _
            '--HOOKS-                       _theActiveView.ScreenDisplay.DrawPolyline(_theLinePolyline)
            '--HOOKS-                 End If
            '--HOOKS- 
            '--HOOKS-                 ' get line between from and to points, and angle for text
            '--HOOKS-                 Dim theline As ILine = New ESRI.ArcGIS.Geometry.Line
            '--HOOKS-                 theline.PutCoords(_theStartPoint, thepoint)
            '--HOOKS-                 Dim lineangle As Double = theline.Angle
            '--HOOKS-                 lineangle = lineangle * (180.0# / 3.14159)
            '--HOOKS-                 If ((lineangle > 90.0#) AndAlso (lineangle < 180.0#)) Then
            '--HOOKS-                     lineangle = lineangle + 180.0#
            '--HOOKS-                 ElseIf ((lineangle < 0.0#) AndAlso (lineangle < -90.0#)) Then
            '--HOOKS-                     lineangle = lineangle - 180.0#
            '--HOOKS-                 ElseIf ((lineangle < -90.0#) AndAlso (lineangle > -180)) Then
            '--HOOKS-                     lineangle = lineangle - 180.0#
            '--HOOKS-                 ElseIf (lineangle > 180) Then
            '--HOOKS-                     lineangle = lineangle - 180.0#
            '--HOOKS-                 End If
            '--HOOKS- 
            '--HOOKS-                 ' for drawing text, get text(distance), angle, and point
            '--HOOKS-                 Dim deltax As Double = thepoint.X - _theStartPoint.X
            '--HOOKS-                 Dim deltay As Double = thepoint.Y - _theStartPoint.Y
            '--HOOKS-                 _theTextPoint.X = _theStartPoint.X + deltax / 2.0#
            '--HOOKS-                 _theTextPoint.Y = _theStartPoint.Y + deltay / 2.0#
            '--HOOKS-                 _theTextSymbol.Angle = lineangle
            '--HOOKS-                 _theTextSymbol.Text = ""
            '--HOOKS- 
            '--HOOKS-                 ' draw text
            '--HOOKS-                 _theActiveView.ScreenDisplay.SetSymbol(DirectCast(_theTextSymbol, ISymbol))
            '--HOOKS-                 _theActiveView.ScreenDisplay.DrawText(_theTextPoint, _theTextSymbol.Text)
            '--HOOKS- 
            '--HOOKS-                 ' get polyline with blank space for text
            '--HOOKS-                 thepolyline = New Polyline
            '--HOOKS-                 Dim psegcoll As ISegmentCollection = DirectCast(thepolyline, ISegmentCollection)
            '--HOOKS-                 psegcoll.AddSegment(DirectCast(theline, ISegment))
            '--HOOKS-                 _theLinePolyline = getsmashedline(DirectCast(_theTextSymbol, ISymbol), _theTextPoint, thepolyline)
            '--HOOKS- 
            '--HOOKS-                 ' draw polyline
            '--HOOKS-                 _theActiveView.ScreenDisplay.SetSymbol(DirectCast(_theLineSymbol, ISymbol))
            '--HOOKS-                 If (_theLinePolyline.Length > 0) Then _
            '--HOOKS-                   _theActiveView.ScreenDisplay.DrawPolyline(_theLinePolyline)
            '--HOOKS-                 _theActiveView.ScreenDisplay.FinishDrawing()
            '--HOOKS-             End If
            '-- END HOOKS

            If arrowType.Equals("dimension", _ignoreCase) Then
                _theEditorEvents = DirectCast(EditorExtension.Editor, Editor)

                If _toolJustCompletedTask Then 'gets rid of a stray editor agent
                    _theActiveView.Refresh()
                    _toolJustCompletedTask = False
                    Exit Try
                End If

                If _mouseHasMoved Then
                    ' check to be sure m_ppt has a value, prevents an error if called after other tools
                    If _thePt Is Nothing Then
                        _mouseHasMoved = False
                        Exit Try
                    End If
                    ' erase the old agent
                    EditorExtension.Editor.InvertAgent(_thePt, 0)
                    ' get the new point
                    getMousePoint(x, y)
                Else
                    _thePt = _theActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y)
                End If
                _mouseHasMoved = True

                If Not _theSnapAgent Is Nothing Then
                    Dim thesnapenv As ISnapEnvironment
                    thesnapenv = DirectCast(EditorExtension.Editor, ISnapEnvironment)
                    Dim thetemppt As IPoint = New ESRI.ArcGIS.Geometry.Point
                    thetemppt.PutCoords(_thePt.X, _thePt.Y)
                    Dim thesnaptolerance As Double
                    thesnaptolerance = thesnapenv.SnapTolerance
                    _inTol = thesnapenv.SnapPoint(_thePt)
                    If Not _inTol Then
                        _thePt = New ESRI.ArcGIS.Geometry.Point
                        _thePt.PutCoords(thetemppt.X, thetemppt.Y)  'set the point back b/c it was not in the snap tol
                    End If
                Else
                    _inTol = False
                End If

                EditorExtension.Editor.InvertAgent(_thePt, 0) 'draw the agent

            End If


        Catch ex As Exception
            EditorExtension.ProcessUnhandledException(ex)

        End Try
    End Sub

    '-- HOOKS NOT IMPLEMENTED --
    '--HOOKS-     Public Overrides Sub OnMouseUp(ByVal Button As Integer, ByVal Shift As Integer, ByVal X As Integer, ByVal Y As Integer)
    '--HOOKS-         MyBase.OnMouseUp(Button, Shift, X, Y)
    '--HOOKS- 
    '--HOOKS-         If arrowType.Equals("Hook", _ignoreCase) Then
    '--HOOKS-             If (Not _inUse) OrElse (_theLineSymbol Is Nothing) Then Exit Sub
    '--HOOKS- 
    '--HOOKS-             ' Draws a temporary line on the screen
    '--HOOKS-             _theActiveView.ScreenDisplay.StartDrawing(_theActiveView.ScreenDisplay.hDC, -1)
    '--HOOKS-             _theActiveView.ScreenDisplay.SetSymbol(DirectCast(_theTextSymbol, ISymbol))
    '--HOOKS-             _theActiveView.ScreenDisplay.DrawText(_theTextPoint, _theTextSymbol.Text)
    '--HOOKS-             _theActiveView.ScreenDisplay.SetSymbol(DirectCast(_theLineSymbol, ISymbol))
    '--HOOKS-             If (_theLinePolyline.Length > 0) Then _theActiveView.ScreenDisplay.DrawPolyline(_theLinePolyline)
    '--HOOKS-             _theActiveView.ScreenDisplay.FinishDrawing()
    '--HOOKS- 
    '--HOOKS-             ' Generate hooks based on the graphic polyline
    '--HOOKS-             GenerateHooks(DirectCast(_theLinePolyline, IPolyline))
    '--HOOKS- 
    '--HOOKS-             ' Records that the tool is no longer in use
    '--HOOKS-             _inUse = False
    '--HOOKS- 
    '--HOOKS-             ' Clean up
    '--HOOKS-             _theTextSymbol = Nothing
    '--HOOKS-             _theTextPoint = Nothing
    '--HOOKS-             _theLinePolyline = Nothing
    '--HOOKS-             _theLineSymbol = Nothing
    '--HOOKS-         End If
    '--HOOKS- 
    '--HOOKS-     End Sub
    '-- END HOOKS

    Public Overrides Sub OnKeyDown(ByVal keyCode As Integer, ByVal Shift As Integer)
        MyBase.OnKeyDown(keyCode, Shift)

        ' End the dimension arrow tool if the "q" key is pressed
        If keyCode = System.Windows.Forms.Keys.Q Then
            If arrowType.Equals("Dimension", _ignoreCase) Then
                ' Deactivate the tool and reset
                _theArrowPt1 = Nothing
                _theArrowPt2 = Nothing
                _theArrowPt3 = Nothing
                _theArrowPt4 = Nothing
                _thePt = Nothing
                _theArrowPtTemp = Nothing
                _theArrowPtTemp2 = Nothing
                _application.CurrentTool = Nothing
                _application.RefreshWindow()
                _toolJustCompletedTask = True
            ElseIf arrowType.Equals("Arrow", _ignoreCase) Then
                ' Deactivate the tool and reset
                _theArrowPt1 = Nothing
                _theArrowPt2 = Nothing
                _theArrowPt3 = Nothing
                _theArrowPt4 = Nothing
                _arrowPointsCollection = Nothing
                _application.CurrentTool = Nothing
                _application.RefreshWindow()
                _toolJustCompletedTask = True
            Else ' Hooks or ""
                ' Deactivate the tool and reset
                _application.CurrentTool = Nothing
                _application.RefreshWindow()
                _toolJustCompletedTask = True
            End If
        End If
        If keyCode = System.Windows.Forms.Keys.D Then
            If arrowType.Equals("Dimension", _ignoreCase) Then
                PartnerDimensionArrowsForm.ShowDialog()
            End If
        End If

    End Sub

#End Region

#End Region

#Region "Implemented Interface Members"

#Region "IDisposable Interface Implementation"

    Private _isDuringDispose As Boolean ' Used to track whether Dispose() has been called and is in progress.

    ''' <summary>
    ''' Dispose of managed and unmanaged resources.
    ''' </summary>
    ''' <param name="disposing">True or False.</param>
    ''' <remarks>
    ''' <para>Member of System::IDisposable.</para>
    ''' <para>Dispose executes in two distinct scenarios. 
    ''' If disposing equals true, the method has been called directly
    ''' or indirectly by a user's code. Managed and unmanaged resources
    ''' can be disposed.</para>
    ''' <para>If disposing equals false, the method has been called by the 
    ''' runtime from inside the finalizer and you should not reference 
    ''' other objects. Only unmanaged resources can be disposed.</para>
    ''' </remarks>
    Friend Sub Dispose(ByVal disposing As Boolean)
        Try
            ' Check to see if Dispose has already been called.
            If Not Me._isDuringDispose Then

                ' Flag that disposing is in progress.
                Me._isDuringDispose = True

                If disposing Then
                    ' Free managed resources when explicitly called.

                    ' Dispose managed resources here.
                    '   e.g. component.Dispose()

                End If

                ' Free "native" (shared unmanaged) resources, whether 
                ' explicitly called or called by the runtime.

                ' Call the appropriate methods to clean up 
                ' unmanaged resources here.
                _bitmapResourceName = Nothing
                MyBase.m_bitmap = Nothing

                ' Flag that disposing has been finished.
                _isDuringDispose = False

            End If
        Catch ex As Exception
            EditorExtension.ProcessUnhandledException(ex)
        End Try
    End Sub

#Region " IDisposable Support "

    ''' <summary>
    ''' Dispose of managed and unmanaged resources.
    ''' </summary>
    ''' <remarks>
    ''' <para>This code added by Visual Basic to correctly implement the disposable pattern.</para>
    ''' </remarks>
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region

#End Region

#End Region

#Region "Other Members"

#Region "COM GUIDs"
    ' These  GUIDs provide the COM identity for this class 
    ' and its COM interfaces. If you change them, existing 
    ' clients will no longer be able to access the class.
    Public Const ClassId As String = "952aa746-4886-42e9-bd85-0b3b08fa1a95"
    Public Const InterfaceId As String = "b8b0c98f-df9e-4078-8736-993c7a1e0d1d"
    Public Const EventsId As String = "81c6ba05-d21b-480e-aea7-9990483d3cc1"
#End Region

#Region "COM Registration Function(s)"
    <ComRegisterFunction(), ComVisibleAttribute(False)> _
    Public Shared Sub RegisterFunction(ByVal registerType As Type)
        ' Required for ArcGIS Component Category Registrar support
        ArcGISCategoryRegistration(registerType)

        'Add any COM registration code after the ArcGISCategoryRegistration() call

    End Sub

    <ComUnregisterFunction(), ComVisibleAttribute(False)> _
    Public Shared Sub UnregisterFunction(ByVal registerType As Type)
        ' Required for ArcGIS Component Category Registrar support
        ArcGISCategoryUnregistration(registerType)

        'Add any COM unregistration code after the ArcGISCategoryUnregistration() call

    End Sub

#Region "ArcGIS Component Category Registrar generated code"
    ''' <summary>
    ''' Required method for ArcGIS Component Category registration -
    ''' Do not modify the contents of this method with the code editor.
    ''' </summary>
    Private Shared Sub ArcGISCategoryRegistration(ByVal registerType As Type)
        Dim regKey As String = String.Format("HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID)
        MxCommands.Register(regKey)

    End Sub

    ''' <summary>
    ''' Required method for ArcGIS Component Category registration -
    ''' Do not modify the contents of this method with the code editor.
    ''' </summary>
    Private Shared Sub ArcGISCategoryUnregistration(ByVal registerType As Type)
        Dim regKey As String = String.Format("HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID)
        MxCommands.Unregister(regKey)

    End Sub

#End Region
#End Region

#End Region

End Class



