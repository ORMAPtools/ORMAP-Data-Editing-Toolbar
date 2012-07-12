#Region "Copyright 2008-2011 ORMAP Tech Group"

' File:  EditorExtension.vb
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
'SCC revision number: $Revision: 437 $
'Date of Last Change: $Date: 2010-05-12 18:22:16 -0700 (Wed, 12 May 2010) $
#End Region

#Region "Imported Namespaces"
Imports ESRI.ArcGIS.Editor
Imports ESRI.ArcGIS.Geodatabase
Imports System.Windows.Forms
Imports System.Environment
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Geometry
Imports OrmapTaxlotEditing.SpatialUtilities
Imports OrmapTaxlotEditing.DataMonitor
Imports OrmapTaxlotEditing.Utilities


#End Region

'''<summary>
'''EditorExtension class implementing custom ESRI Editor Extension functionalities.
'''</summary>
Public Class EditorExtension
    Inherits ESRI.ArcGIS.Desktop.AddIns.Extension

    Public Sub New()
    End Sub


#Region "Custom Class Members"

#Region "Properties"

    Private Shared _isValidWorkspace As Boolean '= False

    ''' <summary>
    ''' Is a valid workspace. 
    ''' </summary>
    ''' <value></value>
    ''' <returns><c>True</c> or <c>False</c>.</returns>
    ''' <remarks></remarks>
    Friend Shared ReadOnly Property IsValidWorkspace() As Boolean
        Get
            Return _isValidWorkspace
        End Get
    End Property
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="value">A boolean value.</param>
    ''' <remarks></remarks>
    Private Shared Sub setIsValidWorkspace(ByVal value As Boolean)
        _isValidWorkspace = value
    End Sub


    ''' <summary>
    ''' Can enable the set of ORMAP taxlot tools. 
    ''' </summary>
    ''' <value></value>
    ''' <returns><c>True</c> or <c>False</c>.</returns>
    ''' <remarks></remarks>
    Friend Shared ReadOnly Property CanEnableExtendedEditing() As Boolean
        Get
            Dim canEnable As Boolean = True
            canEnable = canEnable AndAlso My.ArcMap.Editor IsNot Nothing
            canEnable = canEnable AndAlso OrmapExtension.HasValidLicense
            canEnable = canEnable AndAlso AllowedToEditTaxlots
            Return canEnable
        End Get
    End Property

    Private Shared _allowedToEditTaxlots As Boolean = True

    ''' <summary>
    ''' Allowed to enable the set of ORMAP taxlot tools. 
    ''' </summary>
    ''' <value></value>
    ''' <returns><c>True</c> or <c>False</c>.</returns>
    ''' <remarks>Tools may not be enabled still (see <see>CanEnableExtendedEditing</see>).</remarks>
    Friend Shared Property AllowedToEditTaxlots() As Boolean
        Get
            Return _allowedToEditTaxlots
        End Get
        Set(ByVal value As Boolean)
            _allowedToEditTaxlots = value
        End Set
    End Property

    Private Shared _allowedToAutoUpdate As Boolean = True

    ''' <summary>
    ''' Allowed to auto-update fields on edit events when the ORMAP taxlot tools are allowed. 
    ''' </summary>
    ''' <value></value>
    ''' <returns><c>True</c> or <c>False</c>.</returns>
    ''' <remarks>Some field auto-updates may not be enabled still (see <see>AllowedToAutoUpdateAllFields</see>).</remarks>
    Friend Shared Property AllowedToAutoUpdate() As Boolean
        Get
            Return _allowedToAutoUpdate
        End Get
        Set(ByVal value As Boolean)
            _allowedToAutoUpdate = value
        End Set
    End Property

    Private Shared _allowedToAutoUpdateAllFields As Boolean

    ''' <summary>
    ''' Allowed to auto-update all fields on edit events 
    ''' when the ORMAP taxlot tools are allowed. 
    ''' </summary>
    ''' <value></value>
    ''' <returns><c>True</c> or <c>False</c>.</returns>
    ''' <remarks>If true, field auto-updates are enabled 
    ''' for the maximum set of fields; If false, just the 
    ''' minimum set are auto-updated.</remarks>
    Friend Shared Property AllowedToAutoUpdateAllFields() As Boolean
        Get
            Return _allowedToAutoUpdateAllFields
        End Get
        Set(ByVal value As Boolean)
            _allowedToAutoUpdateAllFields = value
        End Set
    End Property


    Private Shared _overrideAutoAttribute As Boolean

    ''' <summary>
    ''' Override the auto-update (attributing) on edit events
    ''' to use a specified maps values.
    ''' </summary>
    ''' <value></value>
    ''' <returns><c>True</c> or <c>False</c>.</returns>
    ''' <remarks>If true, field values are set manually via a switch in the locate  
    ''' features tool (which records the mapnumber) instead of using auto-update.
    ''' minimum set are auto-updated.</remarks>
    Friend Shared Property OverrideAutoAttribute() As Boolean
        Get
            Return _overrideAutoAttribute
        End Get
        Set(ByVal value As Boolean)
            _overrideAutoAttribute = value
        End Set
    End Property

    Private Shared _overrideMapNumber As String

    ''' <summary>
    ''' The MapNumber to use if OverrideAutoAttribute is true.
    ''' </summary>
    ''' <value></value>
    ''' <returns>A <c>String</c> object containing a mapnumber.</returns>
    ''' <remarks></remarks>
    Friend Shared Property OverrideMapNumber() As String
        Get
            Return _overrideMapNumber
        End Get
        Set(ByVal value As String)
            _overrideMapNumber = value
        End Set
    End Property

    Private Shared _overrideMapScale As String

    ''' <summary>
    ''' The MapScale to use if OverrideAutoAttribute is true.
    ''' </summary>
    ''' <value></value>
    ''' <returns>A <c>String</c> object containing a map scale.</returns>
    ''' <remarks></remarks>
    Friend Shared Property OverrideMapScale() As String
        Get
            Return _overrideMapScale
        End Get
        Set(ByVal value As String)
            _overrideMapScale = value
        End Set
    End Property

    Private Shared _overrideORMapNumber As String

    ''' <summary>
    ''' The ORMapNumber to use if OverrideAutoAttribute is true.
    ''' </summary>
    ''' <value></value>
    ''' <returns>A <c>String</c> object containing a ORMapNumber.</returns>
    ''' <remarks></remarks>
    Friend Shared Property OverrideORMapNumber() As String
        Get
            Return _overrideORMapNumber
        End Get
        Set(ByVal value As String)
            _overrideORMapNumber = value
        End Set
    End Property

#End Region

#Region "Fields"
    Private _isDuringAutoUpdate As Boolean
    Private _activeViewEvents As ESRI.ArcGIS.Carto.IActiveViewEvents_Event
    Private _ActiveViewEventsFocusMapChanged As ESRI.ArcGIS.Carto.IActiveViewEvents_FocusMapChangedEventHandler
    Private _ActiveViewEventsItemAdded As ESRI.ArcGIS.Carto.IActiveViewEvents_ItemAddedEventHandler
    Private _ActiveViewEventsItemDeleted As ESRI.ArcGIS.Carto.IActiveViewEvents_ItemDeletedEventHandler
#End Region

#Region "Event Handlers"

    ''' <summary>
    ''' Handles the startup event of the Editor Extension
    ''' </summary>
    ''' <remarks>Is called the first time the users clicks "Start Editing".  Stays active even if user changes ArcMap projects.</remarks>
    Protected Overrides Sub OnStartup()
        If OrmapExtension.HasValidLicense Then
            AddHandler Events.OnStartEditing, AddressOf Events_OnStartEditing
            AddHandler Events.OnStopEditing, AddressOf Events_OnStopEditing
        End If
    End Sub
    ''' <summary>
    ''' Handles the shutdown event of the Editor Extension
    ''' </summary>
    ''' <remarks>Is called with the user closes ArcMap.   Stays active even if user changes ArcMap projects.</remarks>
    Protected Overrides Sub OnShutdown()
        If OrmapExtension.HasValidLicense Then
            RemoveHandler Events.OnStartEditing, AddressOf Events_OnStartEditing
            RemoveHandler Events.OnStopEditing, AddressOf Events_OnStopEditing
        End If
    End Sub

#Region "Shortcut properties to the various editor event interfaces"
    Private ReadOnly Property Events() As IEditEvents_Event
        Get
            Return TryCast(My.ArcMap.Editor, IEditEvents_Event)
        End Get
    End Property

    Private ReadOnly Property Events2() As IEditEvents2_Event
        Get
            Return TryCast(My.ArcMap.Editor, IEditEvents2_Event)
        End Get
    End Property

    Private ReadOnly Property Events3() As IEditEvents3_Event
        Get
            Return TryCast(My.ArcMap.Editor, IEditEvents3_Event)
        End Get
    End Property

    Private ReadOnly Property Events4() As IEditEvents4_Event
        Get
            Return TryCast(My.ArcMap.Editor, IEditEvents4_Event)
        End Get
    End Property
#End Region

#Region "ActiveViewEvents Event Handlers"

    ''' <summary>
    ''' Event handler.
    ''' </summary>
    ''' <remarks>Handles ActiveViewEvents.FocusMapChanged events.</remarks>
    Public Sub ActiveViewEvents_FocusMapChanged()
        ' TODO: [NIS] Determine why this event never fires...
        Try
            ClearAllValidDataProperties()
        Catch ex As Exception
            OrmapExtension.ProcessUnhandledException(ex)
        End Try

    End Sub

    ''' <summary>
    ''' Event handler.
    ''' </summary>
    ''' <remarks>Handles ActiveViewEvents.ItemAdded events.</remarks>
    Public Sub ActiveViewEvents_ItemAddedOrDeleted(ByVal Item As Object)
        Try
            ClearAllValidDataProperties()
        Catch ex As Exception
            OrmapExtension.ProcessUnhandledException(ex)
        End Try

    End Sub


#End Region

#Region "Editor Event Handlers"

    ''' <summary>
    ''' Event handler.
    ''' </summary>
    ''' <remarks>Handles EditEvents.OnStartEditing events.</remarks>
    Private Sub Events_OnStartEditing()

        Try

            'Since features of shapefiles, coverages etc cannot be validated, ignore wiring events for them
            If My.ArcMap.Editor.EditWorkspace.Type <> esriWorkspaceType.esriFileSystemWorkspace Then

                ' Indicates that insert and update cursors on simple feature classes cannot bypass store events. Required at 10.0
                Dim theWSEditControl As IWorkspaceEditControl = DirectCast(My.ArcMap.Editor.EditWorkspace, IWorkspaceEditControl)
                If Not theWSEditControl Is Nothing Then
                    theWSEditControl.SetStoreEventsRequired()
                End If

                setIsValidWorkspace(True)

                AddHandler Events.OnCreateFeature, AddressOf EditEvents_OnCreateFeature
                AddHandler Events.OnChangeFeature, AddressOf EditEvents_OnChangeFeature
                AddHandler Events.OnDeleteFeature, AddressOf EditEvents_OnDeleteFeature

                _activeViewEvents = TryCast(My.Document.ActiveView.FocusMap, ESRI.ArcGIS.Carto.IActiveViewEvents_Event)

                'Create an instance of the delegate, add it to FocusMapChanged event
                _ActiveViewEventsFocusMapChanged = New IActiveViewEvents_FocusMapChangedEventHandler(AddressOf ActiveViewEvents_FocusMapChanged)
                AddHandler _activeViewEvents.FocusMapChanged, _ActiveViewEventsFocusMapChanged

                'Create an instance of the delegate, add it to ItemAdded event
                _ActiveViewEventsItemAdded = New IActiveViewEvents_ItemAddedEventHandler(AddressOf ActiveViewEvents_ItemAddedOrDeleted)
                AddHandler _activeViewEvents.ItemAdded, _ActiveViewEventsItemAdded

                'Create an instance of the delegate, add it to ItemDeleted event
                _ActiveViewEventsItemDeleted = New IActiveViewEvents_ItemDeletedEventHandler(AddressOf ActiveViewEvents_ItemAddedOrDeleted)
                AddHandler _activeViewEvents.ItemDeleted, _ActiveViewEventsItemDeleted

                ' Set the valid data properties.
                ClearAllValidDataProperties()

            Else
                setIsValidWorkspace(False)
            End If

        Catch ex As Exception
            OrmapExtension.ProcessUnhandledException(ex)

        End Try


    End Sub

    Private Sub Events_OnStopEditing(ByVal Save As Boolean)

        Try
            'Since features of shapefiles, coverages etc cannot be validated, ignore wiring events for them
            If My.ArcMap.Editor.EditWorkspace.Type <> esriWorkspaceType.esriFileSystemWorkspace Then

                ' Unsubscribe to active view events.
                RemoveHandler _activeViewEvents.FocusMapChanged, _ActiveViewEventsFocusMapChanged
                RemoveHandler _activeViewEvents.ItemAdded, _ActiveViewEventsItemAdded
                RemoveHandler _activeViewEvents.ItemDeleted, _ActiveViewEventsItemDeleted

                _activeViewEvents = Nothing
                _ActiveViewEventsFocusMapChanged = Nothing
                _ActiveViewEventsItemAdded = Nothing
                _ActiveViewEventsItemDeleted = Nothing

                RemoveHandler Events.OnCreateFeature, AddressOf EditEvents_OnCreateFeature
                RemoveHandler Events.OnChangeFeature, AddressOf EditEvents_OnChangeFeature
                RemoveHandler Events.OnDeleteFeature, AddressOf EditEvents_OnDeleteFeature

            End If

        Catch ex As Exception
            OrmapExtension.ProcessUnhandledException(ex)

        Finally
            SetHasValidTaxlotData(False)
            SetHasValidMapIndexData(False)
        End Try

    End Sub


    ''' <summary>
    ''' Updates fields based on the feature that was just changed.
    ''' </summary>
    ''' <param name="obj">The feature that was just changed.</param>
    ''' <remarks>Handles EditEvents.OnCreateFeature events.</remarks>
    Private Sub EditEvents_OnChangeFeature(ByVal obj As ESRI.ArcGIS.Geodatabase.IObject)

        Try
            If Not CanEnableExtendedEditing Then Exit Sub
            If Not AllowedToAutoUpdate Then Exit Sub
            If Not (TypeOf obj Is IFeature) Then Exit Sub

            ' Update the minimum auto-calculated fields
            UpdateMinimumAutoFields(DirectCast(obj, IFeature))

            If Not EditorExtension.AllowedToAutoUpdateAllFields Then Exit Sub

            ' Avoid rentrancy
            If _isDuringAutoUpdate = False Then
                _isDuringAutoUpdate = True
            Else
                Throw New InvalidOperationException("Already in AutoUpdate mode. Cannot initiate AutoUpdate.")
                Exit Sub
            End If

            ' Note: Must check here for if required data is available
            ' (in case subroutines called don't check).

            ' Check for valid data (will try to load data if not found).
            CheckValidTaxlotDataProperties()
            If Not HasValidTaxlotData Then
                MessageBox.Show("Missing data: Valid ORMAP Taxlot layer not found in the map." & NewLine & _
                                "Please load this dataset into your map.", _
                                "ORMAP Taxlot Editing (OnChangeFeature)", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                Exit Sub
            End If
            CheckValidMapIndexDataProperties()
            If Not HasValidMapIndexData Then
                MessageBox.Show("Missing data: Valid ORMAP MapIndex layer not found in the map." & NewLine & _
                                "Please load this dataset into your map.", _
                                "ORMAP Taxlot Editing (OnChangeFeature)", MessageBoxButtons.OK, MessageBoxIcon.Stop)

                Exit Sub
            End If

            If IsTaxlot(obj) Then
                '[Edited object is a ORMAP taxlot feature...]

                CheckValidCancelledNumbersTableDataProperties()
                If Not HasValidCancelledNumbersTableData Then
                    MessageBox.Show("Missing data: Valid ORMAP CancelledNumbersTable not found in the map." & NewLine & _
                                    "Please load this dataset into your map.", _
                                    "ORMAP Taxlot Editing (OnChangeFeature)", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                    Exit Sub
                End If

                Dim theTaxlotFeature As IFeature = DirectCast(obj, IFeature)
                Dim theRowChanges As IRowChanges = DirectCast(theTaxlotFeature, IRowChanges)
                If theRowChanges.ValueChanged(TaxlotFeatureLayer.FeatureClass.FindField(OrmapExtension.TaxLotSettings.TaxlotField)) OrElse _
                        theRowChanges.ValueChanged(TaxlotFeatureLayer.FeatureClass.FindField(OrmapExtension.TaxLotSettings.MapNumberField)) Then
                    ' Capture the mapnumber and taxlot and record them in CancelledNumbers.
                    ' Taxlots will send their numbers to the CancelledNumbers table
                    ' ONLY if they are unique in the map at the time of deletion/change.
                    ' Don't write a NULL value to the cancelled numbers table.
                    If Not IsDBNull(theRowChanges.OriginalValue(TaxlotFeatureLayer.FeatureClass.FindField(OrmapExtension.TaxLotSettings.TaxlotField))) Then
                        SendExtinctToCancelledNumbersTable(theTaxlotFeature, True)
                    End If
                End If

                ' Obtain OrmapMapNumber via overlay and calculate other field values.
                CalculateTaxlotValues(DirectCast(obj, IFeature), FindFeatureLayerByDSName(OrmapExtension.TableNamesSettings.MapIndexFC))

            ElseIf IsAnno(obj) Then
                '[Edited object is an ORMAP annotation feature...]

                ' Set anno size based on the map scale.
                SetAnnoSize(obj)

            End If

        Catch ex As Exception
            OrmapExtension.ProcessUnhandledException(ex)

        Finally
            _isDuringAutoUpdate = False

        End Try

    End Sub

    ''' <summary>
    ''' Updates fields based on the feature that was just created.
    ''' </summary>
    ''' <param name="obj">The feature that was just created.</param>
    ''' <remarks>Handles EditEvents.OnCreateFeature events.</remarks>
    Private Sub EditEvents_OnCreateFeature(ByVal obj As ESRI.ArcGIS.Geodatabase.IObject)

        Try
            If Not EditorExtension.CanEnableExtendedEditing Then Exit Sub
            If Not EditorExtension.AllowedToAutoUpdate Then Exit Sub
            If Not (TypeOf obj Is IFeature) Then Exit Sub

            ' Update the minimum auto-calculated fields
            UpdateMinimumAutoFields(DirectCast(obj, IFeature))

            If Not EditorExtension.AllowedToAutoUpdateAllFields Then Exit Sub

            ' Note: Must check here for if required data is available
            ' (in case subroutines called don't check).

            ' Check for valid data (will try to load data if not found).
            CheckValidTaxlotDataProperties()
            If Not HasValidTaxlotData Then
                MessageBox.Show("Missing data: Valid ORMAP Taxlot layer not found in the map." & NewLine & _
                                "Please load this dataset into your map.", _
                                "ORMAP Taxlot Editing (OnCreateFeature)", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                Exit Sub
            End If
            CheckValidMapIndexDataProperties()
            If Not HasValidMapIndexData Then
                MessageBox.Show("Missing data: Valid ORMAP MapIndex layer not found in the map." & NewLine & _
                                "Please load this dataset into your map.", _
                                "ORMAP Taxlot Editing (OnCreateFeature)", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                Exit Sub
            End If

            ' Get the feature
            Dim theFeature As ESRI.ArcGIS.Geodatabase.IFeature
            theFeature = DirectCast(obj, IFeature)
            If theFeature Is Nothing Then Exit Sub

            ' Get the geometry
            Dim theGeometry As ESRI.ArcGIS.Geometry.IGeometry
            theGeometry = theFeature.Shape

            If IsTaxlot(obj) Then
                '[Edited object is a ORMAP taxlot feature...]

                ' Obtain OrmapMapNumber via overlay and calculate other field values.
                CalculateTaxlotValues(theFeature, MapIndexFeatureLayer)

            ElseIf IsAnno(obj) Then
                '[Edited object is an ORMAP annotation feature...]
                ' Update MapScale, MapNumber and Anno Size:

                ' Get the annotation feature.
                Dim theAnnotationFeature As ESRI.ArcGIS.Carto.IAnnotationFeature
                theAnnotationFeature = DirectCast(theFeature, IAnnotationFeature)

                'NOTE: [NIS] This does not appear to do the right thing. 
                '      The feature is set to the geofeature instead of 
                '      the anno, which results in the anno never being 
                '      updated by the subsequent functions.
                ' Get the parent feature
                Dim theParentFeature As ESRI.ArcGIS.Geodatabase.IFeature
                Dim theParentID As Integer
                theParentID = theAnnotationFeature.LinkedFeatureID
                If theParentID > NotFoundIndex Then
                    '[Feature linked anno...]
                    theParentFeature = GetRelatedObjects(obj)
                    If theParentFeature Is Nothing Then Exit Sub

                    theGeometry = theParentFeature.Shape
                Else
                    '[Not feature linked anno...]
                    'Continue
                End If

                If theGeometry IsNot Nothing AndAlso Not theGeometry.IsEmpty Then
                    setMapIndexAndScale(obj, theFeature, theGeometry)
                End If

                ' Set anno size based on the map scale.
                SetAnnoSize(obj)
            Else
                '[Edited object is another kind of ORMAP feature (not taxlot or annotation)...]
                ' Update MapScale and MapNumber (except for on the MapIndex feature class):

                If theGeometry IsNot Nothing AndAlso Not theGeometry.IsEmpty Then
                    setMapIndexAndScale(obj, theFeature, theGeometry)
                End If

            End If

        Catch ex As Exception
            OrmapExtension.ProcessUnhandledException(ex)

        End Try

    End Sub

    ''' <summary>
    ''' Records in the Cancelled Numbers object class the map number
    ''' and taxlot number from the feature that was just deleted.
    ''' </summary>
    ''' <param name="obj">The feature that was just deleted.</param>
    ''' <remarks>Handles EditEvents.OnDeleteFeature events.</remarks>
    Private Sub EditEvents_OnDeleteFeature(ByVal obj As ESRI.ArcGIS.Geodatabase.IObject)

        Try
            If Not EditorExtension.CanEnableExtendedEditing Then Exit Sub
            If Not EditorExtension.AllowedToAutoUpdate Then Exit Sub
            If Not (TypeOf obj Is IFeature) Then Exit Sub
            If Not EditorExtension.AllowedToAutoUpdateAllFields Then Exit Sub

            ' Note: Must check here for if required data is available
            ' (in case subroutines called don't check).

            ' Check for valid data (will try to load data if not found).
            CheckValidTaxlotDataProperties()
            If Not HasValidTaxlotData Then
                MessageBox.Show("Missing data: Valid ORMAP Taxlot layer not found in the map." & NewLine & _
                                "Please load this dataset into your map.", _
                                "ORMAP Taxlot Editing (OnDeleteFeature)", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                Exit Sub
            End If
            CheckValidMapIndexDataProperties()
            If Not HasValidMapIndexData Then
                MessageBox.Show("Missing data: Valid ORMAP MapIndex layer not found in the map." & NewLine & _
                                "Please load this dataset into your map.", _
                                "ORMAP Taxlot Editing (OnDeleteFeature)", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                Exit Sub
            End If

            If IsTaxlot(obj) Then
                '[Deleting taxlots...]

                CheckValidCancelledNumbersTableDataProperties()
                If Not HasValidCancelledNumbersTableData Then
                    MessageBox.Show("Missing data: Valid ORMAP CancelledNumbersTable not found in the map." & NewLine & _
                                    "Please load this dataset into your map.", _
                                    "ORMAP Taxlot Editing (OnDeleteFeature)", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                    Exit Sub
                End If

                ' Capture the mapnumber and taxlot and record them in CancelledNumbers.
                ' Taxlots will send their numbers to the CancelledNumbers table
                ' ONLY if they are unique in the map at the time of deletion.
                Dim theTaxlotFeature As IFeature = DirectCast(obj, IFeature)
                SendExtinctToCancelledNumbersTable(theTaxlotFeature, False)

            End If

        Catch ex As Exception
            OrmapExtension.ProcessUnhandledException(ex)

        End Try

    End Sub

#End Region

#End Region

#Region "Methods"

    ''' <summary>
    ''' Set the map index (if the field exists) to the Map Index map index for the location geometry.
    ''' </summary>
    ''' <param name="obj">An object implementing <c>IObject</c>, either a Feature or a Row.</param>
    ''' <param name="theFeature">An object implementing <c>IFeature</c>.</param>
    ''' <param name="theSearchGeometry">An object implementing <c>IGeometry</c>.</param>
    ''' <remarks></remarks>
    Private Shared Sub setMapIndexAndScale(ByVal obj As IObject, ByVal theFeature As IFeature, ByVal theSearchGeometry As IGeometry)

        Dim theMapScale As String
        Dim theMapNumber As String

        ' Get the Map Index map number field index.
        Dim theMapNumFieldIndex As Integer
        theMapNumFieldIndex = theFeature.Fields.FindField(OrmapExtension.MapIndexSettings.MapNumberField)

        If theSearchGeometry Is Nothing Then Exit Sub
        If theSearchGeometry.IsEmpty Then Exit Sub

        If theMapNumFieldIndex > NotFoundIndex AndAlso Not IsMapIndex(obj) Then
            ' Get the Map Index map number for the location of the new feature.
            theMapNumber = GetValue(theSearchGeometry, MapIndexFeatureLayer.FeatureClass, OrmapExtension.MapIndexSettings.MapNumberField, OrmapExtension.MapIndexSettings.MapNumberField)
            ' Set the feature map number.
            If Len(theMapNumber) > 0 Then
                theFeature.Value(theMapNumFieldIndex) = theMapNumber
            Else
                theFeature.Value(theMapNumFieldIndex) = System.DBNull.Value
            End If
        End If

        ' Set the map scale (if the field exists) to the Map Index map scale for the feature location:

        ' Get the Map Index map scale field index.
        Dim theMapScaleFieldIndex As Integer
        theMapScaleFieldIndex = theFeature.Fields.FindField(OrmapExtension.MapIndexSettings.MapScaleField)
        If theMapScaleFieldIndex > NotFoundIndex AndAlso Not IsMapIndex(obj) Then
            ' Get the Map Index map scale for the location of the new feature.
            theMapScale = GetValue(theSearchGeometry, MapIndexFeatureLayer.FeatureClass, OrmapExtension.MapIndexSettings.MapScaleField, OrmapExtension.MapIndexSettings.MapNumberField)
            ' Set the feature map scale.
            If Len(theMapScale) > 0 Then
                theFeature.Value(theMapScaleFieldIndex) = theMapScale
            Else
                theFeature.Value(theMapScaleFieldIndex) = System.DBNull.Value
            End If
        End If
    End Sub

#End Region

#End Region


End Class
