Imports ESRI.ArcGIS.Framework
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Geometry
Imports System.Text
Imports System.Windows.Forms
Imports System.IO
Imports System.Diagnostics.FileVersionInfo
Imports System.Reflection.Assembly
Imports OrmapTaxlotEditing.DataMonitor
Imports OrmapTaxlotEditing.SpatialUtilities
Imports OrmapTaxlotEditing.StringUtilities
Imports OrmapTaxlotEditing.Utilities
Imports ESRI.ArcGIS.Desktop.AddIns

Public Class OrmapExtension
    Inherits ESRI.ArcGIS.Desktop.AddIns.Extension
    Implements IExtensionAccelerators

    Public Sub New()
    End Sub

    Protected Overrides Sub OnStartup()

        Try
            ' Set up exception logging
            addTraceListenerForEventLog()

            ' TODO: [anyone] This path could be specified in the configuration settings instead of hard coded here.
            Dim theORMAPLogFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "\" & "ORMAP_LOG"
            addTraceListenerForFileLog(theORMAPLogFolder & "\" & My.ThisAddIn.Name & ".trace.log")



            ' Check for a valid ArcGIS license.
            setHasValidLicense((validateLicense(esriLicenseProductCode.esriLicenseProductCodeStandard) OrElse _
                            validateLicense(esriLicenseProductCode.esriLicenseProductCodeAdvanced)))

            My.User.InitializeWithWindowsUser()

            ' Set up document keyboard accelerators for extension commands.
            'CreateAccelerators()

        Catch ex As Exception
            ProcessUnhandledException(ex)
        End Try

        'AddHandler My.ArcMap.Events.OpenDocument, AddressOf ArcMapOpenDocument

    End Sub

    Protected Overrides Sub OnShutdown()
        Try
            ' Stop exception logging
            Trace.Flush()
            removeTraceListenerForEventLog()
            removeTraceListenerForFileLog()

        Catch ex As Exception
            ProcessUnhandledException(ex)
        End Try

    End Sub


    'Private Sub ArcMapOpenDocument()
    'End Sub


#Region "Properties"

    'Private Shared _application As IApplication

    ' ''' <summary>
    ' ''' The ArcMap Application associated with the Editor object.
    ' ''' </summary>
    ' ''' <value></value>
    ' ''' <returns>An object that supports <c>IApplication</c>.</returns>
    ' ''' <remarks></remarks>
    'Friend Shared ReadOnly Property Application() As IApplication
    '    Get
    '        Return _application
    '    End Get
    'End Property
    ' ''' <summary>
    ' ''' 
    ' ''' </summary>
    ' ''' <param name="value">An object that supports <c>IApplication</c>.</param>
    ' ''' <remarks></remarks>
    'Private Shared Sub setApplication(ByVal value As IApplication)
    '    _application = value
    'End Sub

    'Private Shared _editor As IEditor2
    ' ''' <summary>
    ' ''' The ArcMap Editor object.
    ' ''' </summary>
    ' ''' <value></value>
    ' ''' <returns>An object that supports <c>IEditor2</c>.</returns>
    ' ''' <remarks></remarks>
    'Friend Shared ReadOnly Property Editor() As IEditor2
    '    Get
    '        Return _editor
    '    End Get
    'End Property
    ' ''' <summary>
    ' ''' 
    ' ''' </summary>
    ' ''' <param name="value">An object that supports <c>IEditor2</c>.</param>
    ' ''' <remarks></remarks>
    'Private Shared Sub setEditor(ByVal value As IEditor2)
    '    _editor = value
    'End Sub

    'Private Shared _editEvents As IEditEvents_Event

    ' ''' <summary>
    ' ''' The ArcMap EditEvents object associated with the Editor.
    ' ''' </summary>
    ' ''' <value></value>
    ' ''' <returns>An object that supports <c>IEditEvents_Event</c>.</returns>
    ' ''' <remarks></remarks>
    'Friend Shared ReadOnly Property EditEvents() As IEditEvents_Event
    '    Get
    '        Return _editEvents
    '    End Get
    'End Property
    ' ''' <summary>
    ' ''' 
    ' ''' </summary>
    ' ''' <param name="value">An object that supports <c>IEditEvents_Event</c>.</param>
    ' ''' <remarks></remarks>
    'Private Shared Sub setEditEvents(ByVal value As IEditEvents_Event)
    '    _editEvents = value
    'End Sub






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
    ''' Creates a trace listener for the event log.
    ''' </summary>
    ''' <remarks>Once created, any <c>Trace</c> call will be written to this log.</remarks>
    Private Shared Sub addTraceListenerForEventLog()
        ' Create a trace listener for the event log.
        Dim theTraceListener As New EventLogTraceListener(My.ArcMap.Application.Name)
        theTraceListener.Name = My.ThisAddIn.Name & "_EventLogTraceListener"

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

        Try

            Dim inLogFileFolder As String = IO.Path.GetDirectoryName(inLogFileName)
            IO.Directory.CreateDirectory(inLogFileFolder)

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
            theTextListener.Name = My.ThisAddIn.Name & "_FileLogTraceListener"
            Trace.Listeners.Add(theTextListener)

        Catch ex As Exception
            Return
        End Try

    End Sub

    ''' <summary>
    ''' Removes a trace listener for the event log.
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared Sub removeTraceListenerForEventLog()
        ' Find the trace listener.
        Dim theTraceListenerName As String = My.ThisAddIn.Name & "_EventLogTraceListener"
        ' Remove a trace listener for the event log.
        Trace.Listeners.Remove(theTraceListenerName)
    End Sub

    ''' <summary>
    ''' Removes a trace listener for a file log.
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared Sub removeTraceListenerForFileLog()
        ' Find the trace listener.
        Dim theTraceListenerName As String = My.ThisAddIn.Name & "_FileLogTraceListener"
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
            Dim doc As IDocument = My.ArcMap.Document
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

End Class
