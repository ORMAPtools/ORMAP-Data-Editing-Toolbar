Imports ESRI.ArcGIS.ADF.CATIDs
Imports ESRI.ArcGIS.ADF.BaseClasses
Imports System.Runtime.InteropServices

<ComClass(OrmapAnnotateToolbar.ClassId, OrmapAnnotateToolbar.InterfaceId, OrmapAnnotateToolbar.EventsId), _
 ProgId("OrmapTaxlotEditing.OrmapAnnotateToolbar")> _
Public NotInheritable Class OrmapAnnotateToolbar
    Inherits BaseToolbar

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
        MxCommandBars.Register(regKey)

    End Sub
    ''' <summary>
    ''' Required method for ArcGIS Component Category unregistration -
    ''' Do not modify the contents of this method with the code editor.
    ''' </summary>
    Private Shared Sub ArcGISCategoryUnregistration(ByVal registerType As Type)
        Dim regKey As String = String.Format("HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID)
        MxCommandBars.Unregister(regKey)

    End Sub

#End Region
#End Region

#Region "COM GUIDs"
    ' These  GUIDs provide the COM identity for this class 
    ' and its COM interfaces. If you change them, existing 
    ' clients will no longer be able to access the class.
    Public Const ClassId As String = "aecf7c1a-2c0c-42bc-a163-a078b2318706"
    Public Const InterfaceId As String = "68eefbdf-9fa3-4530-846a-90ccdf891642"
    Public Const EventsId As String = "9eaa34de-028c-460f-8dee-a3e3fd19e324"
#End Region

    ' A creatable COM class must have a Public Sub New() 
    ' with no parameters, otherwise, the class will not be 
    ' registered in the COM registry and cannot be created 
    ' via CreateObject.
    Public Sub New()

        'BeginGroup() 'Separator
        AddItem("OrmapTaxlotEditing.CreateAnnotation")
        BeginGroup() 'Separator
        AddItem("OrmapTaxlotEditing.TransposeAnnotation")
        AddItem("OrmapTaxlotEditing.RotateAnnotation")
        BeginGroup() 'Separator
        AddItem("OrmapTaxlotEditing.StandardSpaceOneSide")
        AddItem("OrmapTaxlotEditing.StandardSpaceEitherSide")
        AddItem("OrmapTaxlotEditing.BigSpaceOneSide")
        AddItem("OrmapTaxlotEditing.BigSpaceEitherSide")

    End Sub

    Public Overrides ReadOnly Property Caption() As String
        Get
            'TODO: Replace bar caption
            Return "ORMAP Annotate (.NET)"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            'TODO: Replace bar ID
            Return "OrmapAnnotateToolbar"
        End Get
    End Property
End Class
