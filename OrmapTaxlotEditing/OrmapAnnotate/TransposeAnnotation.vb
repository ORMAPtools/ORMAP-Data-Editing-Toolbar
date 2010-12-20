#Region "Copyright 2008 ORMAP Tech Group"

' File:  TaxlotAssignment.vb
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
'Imports com.esri.arcgis.support.ms.stdole
Imports System.Runtime.InteropServices
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Environment
Imports System.Globalization
Imports System.Drawing.Text
Imports stdole
Imports ESRI.ArcGIS.ADF.BaseClasses
Imports ESRI.ArcGIS.ADF.CATIDs
Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Editor
Imports ESRI.ArcGIS.Framework
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Display
Imports ESRI.ArcGIS.SystemUI
Imports OrmapTaxlotEditing.DataMonitor
Imports OrmapTaxlotEditing.SpatialUtilities
Imports OrmapTaxlotEditing.EditorExtension
Imports OrmapTaxlotEditing.Utilities

#End Region

<ComClass(TransposeAnnotation.ClassId, TransposeAnnotation.InterfaceId, TransposeAnnotation.EventsId), _
 ProgId("OrmapTaxlotEditing.TransposeAnnotation")> _
Public NotInheritable Class TransposeAnnotation
    Inherits BaseCommand
    Implements IDisposable

#Region "Class-Level Constants and Enumerations (none)"
#End Region

#Region "Built-In Class Members (Constructors, Etc.)"

#Region "Constructors"

    ' A creatable COM class must have a Public Sub New() 
    ' with no parameters, otherwise, the class will not be 
    ' registered in the COM registry and cannot be created 
    ' via CreateObject.
    Public Sub New()
        MyBase.New()

        ' TODO: Define values for the public properties
        MyBase.m_category = "OrmapAnnotate"  'localizable text 
        MyBase.m_caption = "TransposeAnnotation"   'localizable text 
        MyBase.m_message = "Transposes Distance and Direction annotation (annotation on top is moved to bottom and vice versa)"   'localizable text 
        MyBase.m_toolTip = "Transpose Distance && Direction annotation" 'localizable text 
        MyBase.m_name = MyBase.m_category & "_TransposeAnnotation"  'unique id, non-localizable (e.g. "MyCategory_ArcMapCommand")

        Try
            'TODO: change bitmap name if necessary
            Dim bitmapResourceName As String = Me.GetType().Name + ".bmp"
            MyBase.m_bitmap = New Bitmap(Me.GetType(), bitmapResourceName)
        Catch ex As Exception
            System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap")
        End Try

    End Sub
#End Region

#End Region

#Region "Custom Class Members"

#Region "Fields"

    Private _application As IApplication
    Private _bitmapResourceName As String

#End Region

#Region "Properties"

#End Region

#Region "Event Handlers"
    Friend Sub DoButtonOperation()

        Try
            CheckValidMapIndexDataProperties()
            If Not HasValidMapIndexData Then
                MessageBox.Show("Missing data: Valid ORMAP MapIndex layer not found in the map." & NewLine & _
                                "Please load this dataset into your map.", _
                                "Create Annotation", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                Exit Sub
            End If
            performTanspose()
        Catch ex As Exception
            EditorExtension.ProcessUnhandledException(ex)
        End Try

    End Sub
#End Region

#Region "Methods"
    Private Sub performTanspose()
        Dim theMxDoc As IMxDocument
        Dim theMap As IMap
        theMxDoc = DirectCast(EditorExtension.Application.Document, IMxDocument)
        theMap = theMxDoc.FocusMap

        ''Set the reference (Line offsets are based on 1:1200 scale default)
        'theMap.MapScale = ReferenceScale

        Dim theActiveView As IActiveView = CType(theMap, IActiveView)
        Dim theMapExtent As IEnvelope = theActiveView.Extent

        DataMonitor.CheckValidMapIndexDataProperties()

        Dim thisFDOLayersUID As IUID = New UID
        thisFDOLayersUID.Value = "{5CEAE408-4C0A-437F-9DB3-054D83919850}"
        Dim theAnnoEnumLayer As IEnumLayer = theMap.Layers(CType(thisFDOLayersUID, UID), True)
        Dim theAnnoFeatureClass As IFeatureClass = Nothing
        Dim theAnnoLayer As IFeatureLayer
        theAnnoEnumLayer.Reset()
        theAnnoLayer = CType(theAnnoEnumLayer.Next, IFeatureLayer)

        'Now go through anno feature class enum looking for name that matches the map index based on map scale
        Do While Not (theAnnoLayer Is Nothing)
            'If String.Compare(theAnnoLayer.Name, theAnnoFCName, True, CultureInfo.CurrentCulture) = 0 Then
            '    theAnnoFeatureClass = theAnnoLayer.FeatureClass
            '    Exit Do
            'End If
            Dim theAnnoSelection As IFeatureSelection
            Dim theSelectionSet As ISelectionSet2
            theAnnoSelection = CType(theAnnoLayer, IFeatureSelection)
            theSelectionSet = CType(theAnnoSelection.SelectionSet, ISelectionSet2)
            Dim theSelectedAnnoCursor As IFeatureCursor
            theSelectedAnnoCursor = SpatialUtilities.GetSelectedFeatures(theAnnoLayer)
            If Not theSelectedAnnoCursor Is Nothing Then
                Dim theSelectedAnnoFeature As IFeature
                theSelectedAnnoFeature = theSelectedAnnoCursor.NextFeature
                'If Not theSelectedAnnoFeature Is Nothing Then
                Dim i As Integer
                For i = 0 To theSelectionSet.Count Step 2
                    'Get the envelopes for the upper and lower annotation
                    Dim theGeometry As IGeometry = theSelectedAnnoFeature.Shape
                    Dim theUpperEnvelope As IEnvelope2 = CType(theGeometry.Envelope, IEnvelope2)
                    'theSelectedAnnoFeature = theSelectedAnnoCursor.NextFeature
                    'theGeometry = theSelectedAnnoFeature.Shape
                    'Dim theLowerEnvelope As IEnvelope2 = CType(theGeometry.Envelope, IEnvelope2)

                    'Dim ppoint As IPoint
                    'ppoint = New ESRI.ArcGIS.Geometry.Point
                    'Dim ppoint2 As IPoint
                    'ppoint2 = New ESRI.ArcGIS.Geometry.Point
                    'Dim pline As ILine
                    'pline = New Line


                    'ppoint.PutCoords((theLowerEnvelope.LowerLeft.X + theLowerEnvelope.UpperRight.X) / 2.0#, (theLowerEnvelope.LowerLeft.Y + theLowerEnvelope.UpperRight.Y) / 2.0#)
                    'ppoint2.PutCoords((theUpperEnvelope.LowerLeft.X + theUpperEnvelope.UpperRight.X) / 2.0#, (theUpperEnvelope.LowerLeft.Y + theUpperEnvelope.UpperRight.Y) / 2.0#)

                    'pline.PutCoords(ppoint, ppoint2)

                    'pline.ReverseOrientation()

                    Dim pTransform2D As ITransform2D
                    pTransform2D = CType(theGeometry, ITransform2D)
                    pTransform2D.Move(1000, 1000)
                    'pTransform2D.MoveVector(pline)

                Next
            End If
            theAnnoLayer = CType(theAnnoEnumLayer.Next, IFeatureLayer)
        Loop
    End Sub
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
            Dim canEnable As Boolean
            canEnable = EditorExtension.CanEnableExtendedEditing
            canEnable = canEnable AndAlso EditorExtension.Editor.EditState = esriEditState.esriStateEditing
            canEnable = canEnable AndAlso EditorExtension.IsValidWorkspace
            Return canEnable
        End Get
    End Property

#End Region

#Region "Methods"

    Public Overrides Sub OnCreate(ByVal hook As Object)
        If Not hook Is Nothing Then
            _application = CType(hook, IApplication)

            'Disable if it is not ArcMap
            If TypeOf hook Is IMxApplication Then
                MyBase.m_enabled = True
            Else
                MyBase.m_enabled = False
            End If
        End If

        ' TODO:  Add other initialization code
    End Sub

    Public Overrides Sub OnClick()
        DoButtonOperation()
    End Sub
#End Region

#End Region

#Region "Implemented Interface Properties"

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

    End Sub

#Region " IDisposable Support "

    ' This code added by Visual Basic to correctly implement the disposable pattern.
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
    Public Const ClassId As String = "6759a6be-84be-4eaa-955a-c34755abc4b2"
    Public Const InterfaceId As String = "27433845-b0e0-4b30-8bca-6f52c2e75ae2"
    Public Const EventsId As String = "b1c1fff7-e5f0-4a4d-91fb-9c7270f10abf"
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
    Private Shared Sub ArcGISCategoryRegistration(ByVal registerType As Type)
        Dim regKey As String = String.Format("HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID)
        MxCommands.Register(regKey)

    End Sub
    Private Shared Sub ArcGISCategoryUnregistration(ByVal registerType As Type)
        Dim regKey As String = String.Format("HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID)
        MxCommands.Unregister(regKey)

    End Sub

#End Region
#End Region

#End Region

End Class



