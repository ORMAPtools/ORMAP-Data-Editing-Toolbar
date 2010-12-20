#Region "Copyright 2008 ORMAP Tech Group"

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
Imports System.Collections.Generic
Imports System.Diagnostics.FileVersionInfo
Imports system.Drawing
Imports System.Environment
Imports System.Security.Permissions
Imports System.Text
Imports System.Windows.Forms
Imports System.Reflection.Assembly
Imports System.Runtime.InteropServices
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Editor
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.ADF.CATIDs
Imports ESRI.ArcGIS.Framework
Imports OrmapTaxlotEditing.DataMonitor
Imports OrmapTaxlotEditing.SpatialUtilities
Imports OrmapTaxlotEditing.StringUtilities
Imports OrmapTaxlotEditing.Utilities
#End Region

'The attribute is placed at the assembly level.
<Assembly: PermissionSetAttribute(SecurityAction.RequestMinimum, Name:="FullTrust")> 

''' <summary>
''' Provides the Editor Extension implementation.
''' </summary>
''' <remarks>
''' <para>Also contains core application-level fields, methods, and event handlers.</para>
''' </remarks>
<ComVisible(True)> _
<ComClass(EditorExtension.ClassId, EditorExtension.InterfaceId, EditorExtension.EventsId), _
ProgId("ORMAPTaxlotEditing.EditorExtension")> _
Public NotInheritable Class EditorExtension
    Implements IExtension
    Implements IExtensionAccelerators
    Implements IPersistVariant

#Region "Class-Level Constants and Enumerations (none)"
#End Region

#Region "Built-In Class Members (Constructors, Etc.)"

#Region "Constructors"

    ' A creatable COM class must have a Public Sub New() 
    ' with no parameters, otherwise, the class will not be 
    ' registered in the COM registry and cannot be created 
    ' via CreateObject.
    Public Sub New()
    End Sub

#End Region

#End Region

#Region "Custom Class Members"

#Region "Properties"

    Private Shared _application As IApplication

    ''' <summary>
    ''' The ArcMap Application associated with the Editor object.
    ''' </summary>
    ''' <value></value>
    ''' <returns>An object that supports <c>IApplication</c>.</returns>
    ''' <remarks></remarks>
    Friend Shared ReadOnly Property Application() As IApplication
        Get
            Return _application
        End Get
    End Property
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="value">An object that supports <c>IApplication</c>.</param>
    ''' <remarks></remarks>
    Private Shared Sub setApplication(ByVal value As IApplication)
        _application = value
    End Sub

    Private Shared _editor As IEditor2
    ''' <summary>
    ''' The ArcMap Editor object.
    ''' </summary>
    ''' <value></value>
    ''' <returns>An object that supports <c>IEditor2</c>.</returns>
    ''' <remarks></remarks>
    Friend Shared ReadOnly Property Editor() As IEditor2
        Get
            Return _editor
        End Get
    End Property
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="value">An object that supports <c>IEditor2</c>.</param>
    ''' <remarks></remarks>
    Private Shared Sub setEditor(ByVal value As IEditor2)
        _editor = value
    End Sub

    Private Shared _editEvents As IEditEvents_Event

    ''' <summary>
    ''' The ArcMap EditEvents object associated with the Editor.
    ''' </summary>
    ''' <value></value>
    ''' <returns>An object that supports <c>IEditEvents_Event</c>.</returns>
    ''' <remarks></remarks>
    Friend Shared ReadOnly Property EditEvents() As IEditEvents_Event
        Get
            Return _editEvents
        End Get
    End Property
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="value">An object that supports <c>IEditEvents_Event</c>.</param>
    ''' <remarks></remarks>
    Private Shared Sub setEditEvents(ByVal value As IEditEvents_Event)
        _editEvents = value
    End Sub

    Private Shared _activeViewEvents As IActiveViewEvents_Event

    ''' <summary>
    ''' The ArcMap ActiveViewEvents object.
    ''' </summary>
    ''' <value></value>
    ''' <returns>An object that supports <c>IActiveViewEvents_Event</c>.</returns>
    ''' <remarks></remarks>
    Friend Shared ReadOnly Property ActiveViewEvents() As IActiveViewEvents_Event
        Get
            Return _activeViewEvents
        End Get
    End Property
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="value">An object that supports <c>IActiveViewEvents_Event</c>.</param>
    ''' <remarks></remarks>
    Private Shared Sub setActiveViewEvents(ByVal value As IActiveViewEvents_Event)
        _activeViewEvents = value
    End Sub

    ''' <summary>
    ''' The ORMAP TableNamesSettings object.
    ''' </summary>
    ''' <value></value>
    ''' <returns>A <c>TableNamesSettings</c> object.</returns>
    ''' <remarks>The object contains all settings for ORMAP table names.</remarks>
    Friend Shared ReadOnly Property TableNamesSettings() As TableNamesSettings
        Get
            Return New TableNamesSettings
        End Get
    End Property

    ''' <summary>
    ''' The ORMAP AnnoTableNamesSettings object.
    ''' </summary>
    ''' <value></value>
    ''' <returns>An <c>AnnoTableNamesSettings</c> object.</returns>
    ''' <remarks>The object contains all settings for ORMAP anno table names.</remarks>
    Friend Shared ReadOnly Property AnnoTableNamesSettings() As AnnoTableNamesSettings
        Get
            Return New AnnoTableNamesSettings
        End Get
    End Property

    ''' <summary>
    ''' The ORMAP AllTablesSettings object.
    ''' </summary>
    ''' <value></value>
    ''' <returns>A <c>AllTablesSettings</c> object.</returns>
    ''' <remarks>The object contains all field name settings for all ORMAP tables.</remarks>
    Friend Shared ReadOnly Property AllTablesSettings() As AllTablesSettings
        Get
            Return New AllTablesSettings
        End Get
    End Property

    ''' <summary>
    ''' The ORMAP MapIndexSettings object.
    ''' </summary>
    ''' <value></value>
    ''' <returns>A <c>MapIndexSettings</c> object.</returns>
    ''' <remarks>The object contains all field name settings for the ORMAP MapIndex table.</remarks>
    Friend Shared ReadOnly Property MapIndexSettings() As MapIndexSettings
        Get
            Return New MapIndexSettings
        End Get
    End Property

    ''' <summary>
    ''' The ORMAP TaxlotSettings object.
    ''' </summary>
    ''' <value></value>
    ''' <returns>A <c>TaxLotSettings</c> object.</returns>
    ''' <remarks>The object contains all field name settings for the ORMAP Taxlot table.</remarks>
    Friend Shared ReadOnly Property TaxLotSettings() As TaxLotSettings
        Get
            Return New TaxLotSettings
        End Get
    End Property

    ''' <summary>
    ''' The ORMAP TaxLotLinesSettings object.
    ''' </summary>
    ''' <value></value>
    ''' <returns>A <c>TaxLotLinesSettings</c> object.</returns>
    ''' <remarks>The object contains all field name settings for the ORMAP TaxLotLines table.</remarks>
    Friend Shared ReadOnly Property TaxLotLinesSettings() As TaxLotLinesSettings
        Get
            Return New TaxLotLinesSettings
        End Get
    End Property

    ''' <summary>
    ''' The ORMAP CartographicLinesSettings object.
    ''' </summary>
    ''' <value></value>
    ''' <returns>A <c>CartographicLinesSettings</c> object.</returns>
    ''' <remarks>The object contains all field name settings for the ORMAP CartographicLines table.</remarks>
    Friend Shared ReadOnly Property CartographicLinesSettings() As CartographicLinesSettings
        Get
            Return New CartographicLinesSettings
        End Get
    End Property

    ''' <summary>
    ''' The ORMAP TaxlotAcreageAnnoSettings object.
    ''' </summary>
    ''' <value></value>
    ''' <returns>A <c>TaxlotAcreageAnnoSettings</c> object.</returns>
    ''' <remarks>The object contains all field name settings for the ORMAP TaxlotAcreageAnno table.</remarks>
    Friend Shared ReadOnly Property TaxlotAcreageAnnoSettings() As TaxlotAcreageAnnoSettings
        Get
            Return New TaxlotAcreageAnnoSettings
        End Get
    End Property

    ''' <summary>
    ''' The ORMAP TaxlotNumberAnnoSettings object.
    ''' </summary>
    ''' <value></value>
    ''' <returns>A <c>TaxlotNumberAnnoSettings</c> object.</returns>
    ''' <remarks>The object contains all field name settings for the ORMAP TaxlotNumberAnno table.</remarks>
    Friend Shared ReadOnly Property TaxlotNumberAnnoSettings() As TaxlotNumberAnnoSettings
        Get
            Return New TaxlotNumberAnnoSettings
        End Get
    End Property

    ''' <summary>
    ''' The ORMAP DefaultValuesSettings object.
    ''' </summary>
    ''' <value></value>
    ''' <returns>A <c>DefaultValuesSettings</c> object.</returns>
    ''' <remarks>The object contains all field name settings for the ORMAP DefaultValues table.</remarks>
    Friend Shared ReadOnly Property DefaultValuesSettings() As DefaultValuesSettings
        Get
            Return New DefaultValuesSettings
        End Get
    End Property

    ''' <summary>
    ''' Can enable the set of ORMAP taxlot tools. 
    ''' </summary>
    ''' <value></value>
    ''' <returns><c>True</c> or <c>False</c>.</returns>
    ''' <remarks></remarks>
    Friend Shared ReadOnly Property CanEnableExtendedEditing() As Boolean
        Get
            Dim canEnable As Boolean = True
            canEnable = canEnable AndAlso EditorExtension.Editor IsNot Nothing
            'canEnable = canEnable AndAlso EditorExtension.Editor.EditState = esriEditState.esriStateEditing
            'canEnable = canEnable AndAlso EditorExtension.IsValidWorkspace
            canEnable = canEnable AndAlso EditorExtension.HasValidLicense
            canEnable = canEnable AndAlso EditorExtension.AllowedToEditTaxlots
            Return canEnable
        End Get
    End Property

    Private Shared _hasValidLicense As Boolean '= False

    ''' <summary>
    ''' Has a valid ArcMap license level. 
    ''' </summary>
    ''' <value></value>
    ''' <returns><c>True</c> or <c>False</c>.</returns>
    ''' <remarks></remarks>
    Friend Shared ReadOnly Property HasValidLicense() As Boolean
        Get
            Return _hasValidLicense
        End Get
    End Property
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="value">A boolean value.</param>
    ''' <remarks></remarks>
    Private Shared Sub setHasValidLicense(ByVal value As Boolean)
        _hasValidLicense = value
    End Sub

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

#End Region

#Region "Event Handlers"

#Region "Editor Event Handlers"

    ''' <summary>
    ''' Updates fields based on the feature that was just changed.
    ''' </summary>
    ''' <param name="obj">The feature that was just changed.</param>
    ''' <remarks>Handles EditEvents.OnCreateFeature events.</remarks>
    Private Sub EditEvents_OnChangeFeature(ByVal obj As ESRI.ArcGIS.Geodatabase.IObject) 'Handles EditEvents.OnChangeFeature

        Try
            If Not EditorExtension.CanEnableExtendedEditing Then Exit Sub
            If Not EditorExtension.AllowedToAutoUpdate Then Exit Sub
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
                If theRowChanges.ValueChanged(TaxlotFeatureLayer.FeatureClass.FindField(TaxLotSettings.TaxlotField)) OrElse _
                        theRowChanges.ValueChanged(TaxlotFeatureLayer.FeatureClass.FindField(TaxLotSettings.MapNumberField)) Then
                    ' Capture the mapnumber and taxlot and record them in CancelledNumbers.
                    ' Taxlots will send their numbers to the CancelledNumbers table
                    ' ONLY if they are unique in the map at the time of deletion/change.
                    ' Don't write a NULL value to the cancelled numbers table.
                    If Not IsDBNull(theRowChanges.OriginalValue(TaxlotFeatureLayer.FeatureClass.FindField(TaxLotSettings.TaxlotField))) Then
                        SendExtinctToCancelledNumbersTable(theTaxlotFeature, True)
                    End If
                End If

                ' Obtain OrmapMapNumber via overlay and calculate other field values.
                CalculateTaxlotValues(DirectCast(obj, IFeature), FindFeatureLayerByDSName(EditorExtension.TableNamesSettings.MapIndexFC))

            ElseIf IsAnno(obj) Then
                '[Edited object is an ORMAP annotation feature...]

                ' Set anno size based on the map scale.
                SetAnnoSize(obj)

            End If

        Catch ex As Exception
            ProcessUnhandledException(ex)

        Finally
            _isDuringAutoUpdate = False

        End Try

    End Sub

    ''' <summary>
    ''' Updates fields based on the feature that was just created.
    ''' </summary>
    ''' <param name="obj">The feature that was just created.</param>
    ''' <remarks>Handles EditEvents.OnCreateFeature events.</remarks>
    Private Sub EditEvents_OnCreateFeature(ByVal obj As ESRI.ArcGIS.Geodatabase.IObject) 'Handles EditEvents.OnCreateFeature

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
            ProcessUnhandledException(ex)

        End Try

    End Sub

    ''' <summary>
    ''' Records in the Cancelled Numbers object class the map number
    ''' and taxlot number from the feature that was just deleted.
    ''' </summary>
    ''' <param name="obj">The feature that was just deleted.</param>
    ''' <remarks>Handles EditEvents.OnDeleteFeature events.</remarks>
    Private Sub EditEvents_OnDeleteFeature(ByVal obj As ESRI.ArcGIS.Geodatabase.IObject) 'Handles EditEvents.OnDeleteFeature

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
            ProcessUnhandledException(ex)

        End Try

    End Sub

    ''' <summary>
    ''' Event handler.
    ''' </summary>
    ''' <remarks>Handles EditEvents.OnStartEditing events.</remarks>
    Private Sub EditEvents_OnStartEditing()

        Try
            '' Check for a valid ArcGIS license.
            'setHasValidLicense((validateLicense(esriLicenseProductCode.esriLicenseProductCodeArcEditor) OrElse _
            '                validateLicense(esriLicenseProductCode.esriLicenseProductCodeArcInfo)))

            ' Check for a valid workspace.
            If EditorExtension.Editor.EditWorkspace.Type = esriWorkspaceType.esriFileSystemWorkspace Then
                'esriFileSystemWorkspace: File-based workspaces. e.g. coverages, shapefiles 
                setIsValidWorkspace(False)
            Else
                'esriLocalDatabaseWorkspace: Geodatabases that are local to your machine, e.g. a File Geodatabase or a Personal Geodatabase stored in an Access file
                'esriRemoteDatabaseWorkspace: Geodatabases that require a remote connection. e.g. ArcSDE, OLE DB
                setIsValidWorkspace(True)
            End If

            If HasValidLicense AndAlso IsValidWorkspace Then

                ' Indicates that insert and update cursors on simple feature classes cannot bypass store events. Required at 10.0
                Dim theWSEditControl As IWorkspaceEditControl = DirectCast(EditorExtension.Editor.EditWorkspace, IWorkspaceEditControl)
                If Not theWSEditControl Is Nothing Then
                    theWSEditControl.SetStoreEventsRequired()
                End If

                '' Set the Application property
                'setApplication(DirectCast(Editor.Parent, IApplication))

                ' Set active view events object
                Dim theMxDoc As ESRI.ArcGIS.ArcMapUI.IMxDocument
                theMxDoc = DirectCast(EditorExtension.Application.Document, ESRI.ArcGIS.ArcMapUI.IMxDocument)
                setActiveViewEvents(DirectCast(theMxDoc.FocusMap, IActiveViewEvents_Event))  ' TODO: [NIS] Reset this when the focus map changes?

                ' Set up document keyboard accelerators for extension commands.
                CreateAccelerators()

                ' Subscribe to edit events.
                AddHandler EditEvents.OnChangeFeature, AddressOf EditEvents_OnChangeFeature
                AddHandler EditEvents.OnCreateFeature, AddressOf EditEvents_OnCreateFeature
                AddHandler EditEvents.OnDeleteFeature, AddressOf EditEvents_OnDeleteFeature

                ' Subscribe to active view events.
                AddHandler ActiveViewEvents.FocusMapChanged, AddressOf ActiveViewEvents_FocusMapChanged
                AddHandler ActiveViewEvents.ItemAdded, AddressOf ActiveViewEvents_ItemAdded
                AddHandler ActiveViewEvents.ItemDeleted, AddressOf ActiveViewEvents_ItemDeleted

                ' Set the valid data properties.
                ClearAllValidDataProperties()

            End If

        Catch ex As Exception
            ProcessUnhandledException(ex)

        End Try

    End Sub


    ''' <summary>
    ''' Event handler.
    ''' </summary>
    ''' <remarks>Handles EditEvents.OnStopEditing events.</remarks>
    Private Sub EditEvents_OnStopEditing(ByVal save As Boolean)

        Try
            If HasValidLicense AndAlso IsValidWorkspace Then

                ' Unsubscribe to edit events.
                RemoveHandler EditEvents.OnChangeFeature, AddressOf EditEvents_OnChangeFeature
                RemoveHandler EditEvents.OnCreateFeature, AddressOf EditEvents_OnCreateFeature
                RemoveHandler EditEvents.OnDeleteFeature, AddressOf EditEvents_OnDeleteFeature

                ' Unsubscribe to active view events.
                RemoveHandler ActiveViewEvents.FocusMapChanged, AddressOf ActiveViewEvents_FocusMapChanged
                RemoveHandler ActiveViewEvents.ItemAdded, AddressOf ActiveViewEvents_ItemAdded
                RemoveHandler ActiveViewEvents.ItemDeleted, AddressOf ActiveViewEvents_ItemDeleted

            End If

        Catch ex As Exception
            ProcessUnhandledException(ex)

        Finally
            'setApplication(Nothing)
            setActiveViewEvents(Nothing)

            SetHasValidTaxlotData(False)
            SetHasValidMapIndexData(False)

        End Try

    End Sub

#End Region

#Region "ActiveViewEvents Event Handlers"

    ''' <summary>
    ''' Event handler.
    ''' </summary>
    ''' <remarks>Handles ActiveViewEvents.FocusMapChanged events.</remarks>
    Public Sub ActiveViewEvents_FocusMapChanged() 'Handles ESRI.ArcGIS.Carto.IActiveViewEvents.FocusMapChanged
        ' TODO: [NIS] Determine why this event never fires...
        Try
            ClearAllValidDataProperties()
        Catch ex As Exception
            ProcessUnhandledException(ex)
        End Try

    End Sub

    ''' <summary>
    ''' Event handler.
    ''' </summary>
    ''' <remarks>Handles ActiveViewEvents.ItemAdded events.</remarks>
    Public Sub ActiveViewEvents_ItemAdded(ByVal Item As Object) 'Handles ESRI.ArcGIS.Carto.IActiveViewEvents.ItemAdded
        Try
            ClearAllValidDataProperties()
        Catch ex As Exception
            ProcessUnhandledException(ex)
        End Try

    End Sub

    ''' <summary>
    ''' Event handler.
    ''' </summary>
    ''' <remarks>Handles ActiveViewEvents.ItemDeleted events.</remarks>
    Public Sub ActiveViewEvents_ItemDeleted(ByVal Item As Object) 'Handles ESRI.ArcGIS.Carto.IActiveViewEvents.ItemDeleted
        Try
            ClearAllValidDataProperties()
        Catch ex As Exception
            ProcessUnhandledException(ex)
        End Try

    End Sub

#End Region

#End Region

#Region "Methods"

    ''' <summary>
    ''' Set a keyboard accelerator. 
    ''' </summary>
    ''' <param name="acceleratorTable">The accelerator table to store the accelerators in.</param>
    ''' <param name="classID">the ClassID of the command to trigger by the accelerator.</param>
    ''' <param name="key">The key to use.</param>
    ''' <param name="usesCtrl"><italic>Flag.</italic> Uses Ctrl key modification.</param>
    ''' <param name="usesAlt"><italic>Flag.</italic> Uses Alt key modification.</param>
    ''' <param name="usesShift"><italic>Flag.</italic> Uses Shift key modification.</param>
    ''' <remarks></remarks>
    Private Shared Sub setAccelerator(ByVal acceleratorTable As IAcceleratorTable, _
            ByVal classID As UID, ByVal key As Integer, _
            ByVal usesCtrl As Boolean, ByVal usesAlt As Boolean, _
            ByVal usesShift As Boolean)

        ' TEST: [NIS] Not sure these accelerators will work with an editor extension.

        ' Create accelerator only if nothing else is using it

        Dim accelerator As IAccelerator

        accelerator = acceleratorTable.FindByKey(key, usesCtrl, usesAlt, usesShift)
        If accelerator Is Nothing Then
            'The clsid of one of the commands in the ext
            acceleratorTable.Add(classID, key, usesCtrl, usesAlt, usesShift)
        End If

    End Sub

    ''' <summary>
    ''' Validate the license (e.g. ArcEditor or ArcInfo).
    ''' </summary>
    ''' <param name="requiredProductCode">An <c>"esriLicenseProductCode"</c> enumerated item.</param>
    ''' <returns><c>True</c> or <c>False</c>, depending on the licence validity.</returns>
    ''' <remarks></remarks>
    Private Shared Function validateLicense(ByVal requiredProductCode As esriLicenseProductCode) As Boolean

        Dim theAoInitializeClass As New AoInitializeClass()
        Dim productCode As esriLicenseProductCode = theAoInitializeClass.InitializedProduct()

        Return (productCode = requiredProductCode)
    End Function

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
        theMapNumFieldIndex = theFeature.Fields.FindField(EditorExtension.MapIndexSettings.MapNumberField)

        If theSearchGeometry Is Nothing Then Exit Sub
        If theSearchGeometry.IsEmpty Then Exit Sub

        If theMapNumFieldIndex > NotFoundIndex AndAlso Not IsMapIndex(obj) Then
            ' Get the Map Index map number for the location of the new feature.
            theMapNumber = GetValue(theSearchGeometry, MapIndexFeatureLayer.FeatureClass, EditorExtension.MapIndexSettings.MapNumberField, EditorExtension.MapIndexSettings.MapNumberField)
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
        theMapScaleFieldIndex = theFeature.Fields.FindField(EditorExtension.MapIndexSettings.MapScaleField)
        If theMapScaleFieldIndex > NotFoundIndex AndAlso Not IsMapIndex(obj) Then
            ' Get the Map Index map scale for the location of the new feature.
            theMapScale = GetValue(theSearchGeometry, MapIndexFeatureLayer.FeatureClass, EditorExtension.MapIndexSettings.MapScaleField, EditorExtension.MapIndexSettings.MapNumberField)
            ' Set the feature map scale.
            If Len(theMapScale) > 0 Then
                theFeature.Value(theMapScaleFieldIndex) = theMapScale
            Else
                theFeature.Value(theMapScaleFieldIndex) = System.DBNull.Value
            End If
        End If
    End Sub

    ''' <summary>
    ''' Creates a trace listener for the event log.
    ''' </summary>
    ''' <remarks>Once created, any <c>Trace</c> call will be written to this log.</remarks>
    Private Shared Sub addTraceListenerForEventLog()
        ' Create a trace listener for the event log.
        Dim theTraceListener As New EventLogTraceListener(My.Application.Info.ProductName)
        theTraceListener.Name = My.Application.Info.AssemblyName & "_EventLogTraceListener"

        ' Add the event log trace listener to the collection.
        Trace.Listeners.Add(theTraceListener)
    End Sub

    ''' <summary>
    ''' Create a trace listener for a file log.
    ''' </summary>
    ''' <param name="inLogFileName">The file log filename.</param>
    ''' <remarks>
    ''' <para>Once created, any <c>Trace</c> call will be written to this log.</para>
    ''' <para>This file will be replaced every 30 days.</para>
    ''' </remarks>
    Private Shared Sub addTraceListenerForFileLog(ByVal inLogFileName As String)
        ' Create a file for output.
        Dim theFileStream As IO.Stream
        If IO.File.Exists(inLogFileName) Then
            If DateDiff(DateInterval.Day, Now, IO.File.GetCreationTime(inLogFileName)) > 30 Then
                IO.File.Delete(inLogFileName)
                theFileStream = IO.File.Create(inLogFileName)
            Else
                theFileStream = IO.File.Open(inLogFileName, IO.FileMode.Append, IO.FileAccess.Write, IO.FileShare.Write)
            End If
        Else
            theFileStream = IO.File.Create(inLogFileName)
        End If

        ' Create a new text writer using the output stream, and add it to
        ' the trace listeners. 
        Dim theTextListener As New TextWriterTraceListener(theFileStream)
        theTextListener.Name = My.Application.Info.AssemblyName & "_FileLogTraceListener"
        Trace.Listeners.Add(theTextListener)
    End Sub

    ''' <summary>
    ''' Removes a trace listener for the event log.
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared Sub removeTraceListenerForEventLog()
        ' Find the trace listener.
        Dim theTraceListenerName As String = My.Application.Info.AssemblyName & "_EventLogTraceListener"
        ' Remove a trace listener for the event log.
        Trace.Listeners.Remove(theTraceListenerName)
    End Sub

    ''' <summary>
    ''' Removes a trace listener for a file log.
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared Sub removeTraceListenerForFileLog()
        ' Find the trace listener.
        Dim theTraceListenerName As String = My.Application.Info.AssemblyName & "_FileLogTraceListener"
        ' Remove a trace listener for a file log.
        Trace.Listeners.Remove(theTraceListenerName)
    End Sub

    ''' <summary>
    ''' Process any unhandled exceptions that occur in the application.
    ''' </summary>
    ''' <param name="ex">The unhandled exception.</param>
    ''' <remarks>
    ''' <para>This code is called by all UI entry points in the application (e.g. button click events)
    ''' when an unhandled exception occurs.</para>
    ''' <para>You could also achieve this by handling the Application.ThreadException event, however
    ''' the VS2005 debugger will break before this event is called.</para>
    ''' </remarks>
    Friend Shared Sub ProcessUnhandledException(ByVal ex As Exception)
        ' An unhandled exception occured somewhere in the application.

        ' Tell the user what has happened
        Dim theUserMessageText As New StringBuilder()
        theUserMessageText.Append("An unexpected exception occured and has been logged for the developers.")
        theUserMessageText.AppendLine()
        theUserMessageText.AppendLine()
        theUserMessageText.Append("Warning: The application may no longer be stable.")
        theUserMessageText.AppendLine()
        theUserMessageText.Append("Save your work just in case.")

        MessageBox.Show(theUserMessageText.ToString, "ORMAP Taxlot Editing Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)

        ' Write a meaningful log entry for the error
        Dim theLogText As New StringBuilder()
        Try
            theLogText.AppendLine()
            theLogText.Append(CChar("_"), 50)
            theLogText.AppendLine()
            theLogText.Append(My.Computer.Clock.GmtTime.ToString)
            theLogText.AppendLine()
            theLogText.AppendLine()
            theLogText.Append("An unexpected exception occured.")
            theLogText.AppendLine()
            theLogText.AppendLine()
            theLogText.Append("UNHANDLED EXCEPTION CALL STACK:")
            theLogText.AppendLine()
            theLogText.Append(ex.ToString)
            theLogText.AppendLine()
            theLogText.Append(CChar("_"), 50)
            theLogText.AppendLine()
            
            Trace.TraceError(theLogText.ToString)
            Trace.Flush()

            ' ENHANCE: [NIS] Get this working? Attach log file (last parameter)?
            'SendEmailMessage("OPET.NET@gmail.com", "nseigal@lcog.org", "ORMAP Taxlot Editing Error", theLogText.ToString, "")

        Catch
            Dim theUnableToLogText As New StringBuilder()
            theUnableToLogText.Append(GetVersionInfo(GetExecutingAssembly.Location).ProductName & _
            " v" & GetVersionInfo(GetExecutingAssembly.Location).ProductVersion)
            'theUnableToLogText.AppendLine()
            'theUnableToLogText.Append(GetVersionInfo(GetExecutingAssembly.Location).Comments)
            theUnableToLogText.AppendLine()
            theUnableToLogText.AppendLine()
            theUnableToLogText.Append("Unable to log an error (see below).")
            theUnableToLogText.AppendLine()
            theUnableToLogText.AppendLine()
            theUnableToLogText.Append("Please contact technical support and provide this information.")
            theUnableToLogText.AppendLine()
            theUnableToLogText.Append(theLogText.ToString)
            theUnableToLogText.AppendLine()
            theUnableToLogText.Append("[Press Ctrl+C on your keyboard to copy this message to the Windows clipboard.]  ")
            theUnableToLogText.AppendLine()

            MessageBox.Show(theUnableToLogText.ToString, "ORMAP Taxlot Editing Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
        End Try

    End Sub

#End Region

#End Region

#Region "Inherited Class Members (none)"
#End Region

#Region "Implemented Interface Members"

#Region "IExtension Interface Implementation"

    ''' <summary>
    ''' Sets the name of the extension.
    ''' </summary>
    ''' <value>The name of the extension.</value>
    ''' <returns>The name.</returns>
    ''' <remarks>Must not exceed 31 characters.</remarks>
    Public ReadOnly Property Name() As String Implements ESRI.ArcGIS.esriSystem.IExtension.Name
        Get
            Return "EditorExtension"
        End Get
    End Property

    ''' <summary>
    ''' Shuts down the extension.
    ''' </summary>
    ''' <remarks><see cref="Startup" /></remarks>
    Public Sub Shutdown() Implements ESRI.ArcGIS.esriSystem.IExtension.Shutdown
        Try
            ' Unsubscribe to edit events.
            RemoveHandler EditEvents.OnStartEditing, AddressOf EditEvents_OnStartEditing
            RemoveHandler EditEvents.OnStopEditing, AddressOf EditEvents_OnStopEditing

            ' Clear extension properties
            setEditor(Nothing)
            setEditEvents(Nothing)
            setApplication(Nothing)

            ' Stop exception logging
            Trace.Flush()
            removeTraceListenerForEventLog()
            removeTraceListenerForFileLog()

        Catch ex As Exception
            ProcessUnhandledException(ex)

        End Try
    End Sub

    ''' <summary>
    ''' Starts up the extension with the given initialization data.
    ''' </summary>
    ''' <param name="initializationData">A reference to the object with 
    ''' which this extension is registered (Editor).</param>
    ''' <remarks>
    ''' <para>
    ''' Registered in this case with the <bold>ESRI Editor Extensions</bold> 
    ''' category.
    ''' </para>
    ''' <para>
    ''' Any extension that is registered with an application is automatically 
    ''' loaded and unloaded by the application; the end user does nothing to 
    ''' load or unload. For example, an extension that has been added to the 
    ''' ESRI Mx Extensions category will be started when ArcMap is started and 
    ''' will be shutdown when ArcMap is shutdown. In this case, the editor
    ''' extension is loaded when the ArcMap Editor object is loaded, which 
    ''' occurs when ArcMap is started. So this startup event occurs when
    ''' ArcMap is started (rather than when an edit session is started).
    ''' </para>
    ''' </remarks>
    Public Sub Startup(ByRef initializationData As Object) Implements ESRI.ArcGIS.esriSystem.IExtension.Startup

        Try
            If Not initializationData Is Nothing AndAlso TypeOf initializationData Is IEditor2 Then

                ' Set up exception logging
                addTraceListenerForEventLog()
                addTraceListenerForFileLog(My.Application.Info.DirectoryPath & "\" & My.Application.Info.AssemblyName & ".trace.log")

                ' Set the Editor and EditEvents properties.
                setEditor(DirectCast(initializationData, IEditor2))
                setEditEvents(DirectCast(EditorExtension.Editor, IEditEvents_Event))
                setApplication(DirectCast(Editor.Parent, IApplication))

                ' Check for a valid ArcGIS license.
                setHasValidLicense((validateLicense(esriLicenseProductCode.esriLicenseProductCodeArcEditor) OrElse _
                                validateLicense(esriLicenseProductCode.esriLicenseProductCodeArcInfo)))

                My.User.InitializeWithWindowsUser()

                ' Subscribe to edit events.
                AddHandler EditEvents.OnStartEditing, AddressOf EditEvents_OnStartEditing
                AddHandler EditEvents.OnStopEditing, AddressOf EditEvents_OnStopEditing

            End If

        Catch ex As Exception
            ProcessUnhandledException(ex)

        End Try

    End Sub

#End Region

#Region "IExtensionAccelerators Interface Implementation"

    ''' <summary>
    ''' Create the keyboard accelerators for this extension.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CreateAccelerators() Implements IExtensionAccelerators.CreateAccelerators

        ' TEST: [NIS] Not sure these accelerators will work with an editor extension.

        Try
            Dim key As Integer
            Dim usesCtrl As Boolean
            Dim usesAlt As Boolean
            Dim usesShift As Boolean
            Dim uid As New UID
            Dim doc As IDocument = EditorExtension.Application.Document
            Dim acceleratorTable As IAcceleratorTable = doc.Accelerators

            ' Set LocateFeature accelerator keys to Ctrl + Alt + L
            key = Convert.ToInt32(Keys.L)
            usesCtrl = True
            usesAlt = True
            usesShift = False
            uid.Value = "OrmapTaxlotEditing.LocateFeature" '"{" & OrmapTaxlotEditing.LocateFeature.ClassId & "}"
            setAccelerator(acceleratorTable, uid, key, usesCtrl, usesAlt, usesShift)

            ' Set TaxlotAssignment accelerator keys to Ctrl + Alt + T
            key = Convert.ToInt32(Keys.T)
            usesCtrl = True
            usesAlt = True
            usesShift = False
            uid.Value = "OrmapTaxlotEditing.TaxlotAssignment" '"{" & OrmapTaxlotEditing.TaxlotAssignment.ClassId & "}"
            setAccelerator(acceleratorTable, uid, key, usesCtrl, usesAlt, usesShift)

            ' Set EditMapIndex accelerator keys to Ctrl + Alt + E
            key = Convert.ToInt32(Keys.E)
            usesCtrl = True
            usesAlt = True
            usesShift = False
            uid.Value = "OrmapTaxlotEditing.EditMapIndex" '"{" & OrmapTaxlotEditing.EditMapIndex.ClassId & "}"
            setAccelerator(acceleratorTable, uid, key, usesCtrl, usesAlt, usesShift)

            ' Set CombineTaxlots accelerator keys to Ctrl + Alt + C
            key = Convert.ToInt32(Keys.C)
            usesCtrl = True
            usesAlt = True
            usesShift = False
            uid.Value = "OrmapTaxlotEditing.CombineTaxlots" '"{" & OrmapTaxlotEditing.CombineTaxlots.ClassId & "}"
            setAccelerator(acceleratorTable, uid, key, usesCtrl, usesAlt, usesShift)

            ' Set AddArrows accelerator keys to Ctrl + Alt + A
            key = Convert.ToInt32(Keys.A)
            usesCtrl = True
            usesAlt = True
            usesShift = False
            uid.Value = "OrmapTaxlotEditing.AddArrows" '"{" & OrmapTaxlotEditing.AddArrows.ClassId & "}"
            setAccelerator(acceleratorTable, uid, key, usesCtrl, usesAlt, usesShift)

        Catch ex As Exception
            ProcessUnhandledException(ex)

        End Try

    End Sub

#End Region

#Region "IPersistVariant Interface Implementation"

    Public ReadOnly Property ID() As ESRI.ArcGIS.esriSystem.UID Implements ESRI.ArcGIS.esriSystem.IPersistVariant.ID
        Get
            Dim uid As New UIDClass()
            Try
                uid.Value = Me.GetType().GUID.ToString("B") '"{" & OrmapTaxlotEditing.EditorExtension.ClassId & "}"

            Catch ex As Exception
                ProcessUnhandledException(ex)
            End Try
            Return uid
        End Get
    End Property

    Public Sub Load(ByVal Stream As ESRI.ArcGIS.esriSystem.IVariantStream) Implements ESRI.ArcGIS.esriSystem.IPersistVariant.Load

        Try
            If Stream Is Nothing Then
                Throw New ArgumentNullException("Stream")
            End If

            AllowedToEditTaxlots = CBool(Stream.Read())
            AllowedToAutoUpdate = CBool(Stream.Read())
            AllowedToAutoUpdateAllFields = CBool(Stream.Read())

            System.Runtime.InteropServices.Marshal.ReleaseComObject(Stream)

        Catch ex As Exception
            ProcessUnhandledException(ex)
        End Try

    End Sub

    Public Sub Save(ByVal Stream As ESRI.ArcGIS.esriSystem.IVariantStream) Implements ESRI.ArcGIS.esriSystem.IPersistVariant.Save

        Try
            If Stream Is Nothing Then
                Throw New ArgumentNullException("Stream")
            End If

            Stream.Write(AllowedToEditTaxlots)
            Stream.Write(AllowedToAutoUpdate)
            Stream.Write(AllowedToAutoUpdateAllFields)

            System.Runtime.InteropServices.Marshal.ReleaseComObject(Stream)

        Catch ex As Exception
            ProcessUnhandledException(ex)
        End Try

    End Sub

#End Region

#End Region

#Region "Other Members"

#Region "COM GUIDs"
    ' These  GUIDs provide the COM identity for this class 
    ' and its COM interfaces. If you change them, existing 
    ' clients will no longer be able to access the class.
    Public Const ClassId As String = "3ffddc1a-bf54-45b4-a9dc-88740d97bcc2"
    Public Const InterfaceId As String = "cf8fd284-b76e-4012-a738-bce6e0cbbff4"
    Public Const EventsId As String = "e5719155-369f-4b3e-9e5e-99856449f05b"
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
        EditorExtensions.Register(regKey)

    End Sub
    ''' <summary>
    ''' Required method for ArcGIS Component Category unregistration -
    ''' Do not modify the contents of this method with the code editor.
    ''' </summary>
    Private Shared Sub ArcGISCategoryUnregistration(ByVal registerType As Type)
        Dim regKey As String = String.Format("HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID)
        EditorExtensions.Unregister(regKey)

    End Sub

#End Region
#End Region

#End Region

End Class
