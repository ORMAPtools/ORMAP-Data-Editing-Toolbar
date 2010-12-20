<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EditMapIndexForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.uxTownship = New System.Windows.Forms.ComboBox
        Me.uxTownshipDirectional = New System.Windows.Forms.ComboBox
        Me.uxTownshipPartial = New System.Windows.Forms.ComboBox
        Me.uxTownshipLabel = New System.Windows.Forms.Label
        Me.uxTownshipDirectionalLabel = New System.Windows.Forms.Label
        Me.uxTownshipPartialLabel = New System.Windows.Forms.Label
        Me.uxTownshipGroupBox = New System.Windows.Forms.GroupBox
        Me.uxRangeGroupBox = New System.Windows.Forms.GroupBox
        Me.uxRangePartialLabel = New System.Windows.Forms.Label
        Me.uxRangeDirectionalLabel = New System.Windows.Forms.Label
        Me.uxRangeLabel = New System.Windows.Forms.Label
        Me.uxRangePartial = New System.Windows.Forms.ComboBox
        Me.uxRangeDirectional = New System.Windows.Forms.ComboBox
        Me.uxRange = New System.Windows.Forms.ComboBox
        Me.uxSectionGroupBox = New System.Windows.Forms.GroupBox
        Me.uxSectionQtrQtrLabel = New System.Windows.Forms.Label
        Me.uxSectionQtrLabel = New System.Windows.Forms.Label
        Me.uxSectionLabel = New System.Windows.Forms.Label
        Me.uxSectionQtrQtr = New System.Windows.Forms.ComboBox
        Me.uxSectionQuarter = New System.Windows.Forms.ComboBox
        Me.uxSection = New System.Windows.Forms.ComboBox
        Me.uxMapInfoGroupBox = New System.Windows.Forms.GroupBox
        Me.uxAnomalyLabel = New System.Windows.Forms.Label
        Me.uxPageLabel = New System.Windows.Forms.Label
        Me.uxScaleLabel = New System.Windows.Forms.Label
        Me.uxReliabilityLabel = New System.Windows.Forms.Label
        Me.uxSuffixTypeLabel = New System.Windows.Forms.Label
        Me.uxSuffixNumberLabel = New System.Windows.Forms.Label
        Me.uxMapNumberLabel = New System.Windows.Forms.Label
        Me.uxAnomaly = New System.Windows.Forms.TextBox
        Me.uxPage = New System.Windows.Forms.TextBox
        Me.uxScale = New System.Windows.Forms.ComboBox
        Me.uxReliability = New System.Windows.Forms.ComboBox
        Me.uxCountyLabel = New System.Windows.Forms.Label
        Me.uxSuffixType = New System.Windows.Forms.ComboBox
        Me.uxSuffixNumber = New System.Windows.Forms.TextBox
        Me.uxMapNumber = New System.Windows.Forms.TextBox
        Me.uxCounty = New System.Windows.Forms.ComboBox
        Me.uxORMAPNumberGroupBox = New System.Windows.Forms.GroupBox
        Me.uxORMAPNumberLabel = New System.Windows.Forms.Label
        Me.uxHelp = New System.Windows.Forms.Button
        Me.uxOK = New System.Windows.Forms.Button
        Me.uxCancel = New System.Windows.Forms.Button
        Me.uxApply = New System.Windows.Forms.Button
        Me.ErrorProviderSuffixNum = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.uxTownshipGroupBox.SuspendLayout()
        Me.uxRangeGroupBox.SuspendLayout()
        Me.uxSectionGroupBox.SuspendLayout()
        Me.uxMapInfoGroupBox.SuspendLayout()
        Me.uxORMAPNumberGroupBox.SuspendLayout()
        CType(Me.ErrorProviderSuffixNum, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'uxTownship
        '
        Me.uxTownship.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.uxTownship.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.uxTownship.FormattingEnabled = True
        Me.uxTownship.Location = New System.Drawing.Point(111, 14)
        Me.uxTownship.Name = "uxTownship"
        Me.uxTownship.Size = New System.Drawing.Size(60, 21)
        Me.uxTownship.TabIndex = 1
        '
        'uxTownshipDirectional
        '
        Me.uxTownshipDirectional.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.uxTownshipDirectional.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.uxTownshipDirectional.FormattingEnabled = True
        Me.uxTownshipDirectional.Location = New System.Drawing.Point(111, 39)
        Me.uxTownshipDirectional.Name = "uxTownshipDirectional"
        Me.uxTownshipDirectional.Size = New System.Drawing.Size(60, 21)
        Me.uxTownshipDirectional.TabIndex = 3
        '
        'uxTownshipPartial
        '
        Me.uxTownshipPartial.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.uxTownshipPartial.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.uxTownshipPartial.FormattingEnabled = True
        Me.uxTownshipPartial.Location = New System.Drawing.Point(111, 64)
        Me.uxTownshipPartial.Name = "uxTownshipPartial"
        Me.uxTownshipPartial.Size = New System.Drawing.Size(60, 21)
        Me.uxTownshipPartial.TabIndex = 5
        '
        'uxTownshipLabel
        '
        Me.uxTownshipLabel.AutoSize = True
        Me.uxTownshipLabel.Location = New System.Drawing.Point(7, 17)
        Me.uxTownshipLabel.Name = "uxTownshipLabel"
        Me.uxTownshipLabel.Size = New System.Drawing.Size(47, 13)
        Me.uxTownshipLabel.TabIndex = 0
        Me.uxTownshipLabel.Text = "Number:"
        '
        'uxTownshipDirectionalLabel
        '
        Me.uxTownshipDirectionalLabel.AutoSize = True
        Me.uxTownshipDirectionalLabel.Location = New System.Drawing.Point(7, 42)
        Me.uxTownshipDirectionalLabel.Name = "uxTownshipDirectionalLabel"
        Me.uxTownshipDirectionalLabel.Size = New System.Drawing.Size(60, 13)
        Me.uxTownshipDirectionalLabel.TabIndex = 2
        Me.uxTownshipDirectionalLabel.Text = "Directional:"
        '
        'uxTownshipPartialLabel
        '
        Me.uxTownshipPartialLabel.AutoSize = True
        Me.uxTownshipPartialLabel.Location = New System.Drawing.Point(7, 67)
        Me.uxTownshipPartialLabel.Name = "uxTownshipPartialLabel"
        Me.uxTownshipPartialLabel.Size = New System.Drawing.Size(67, 13)
        Me.uxTownshipPartialLabel.TabIndex = 4
        Me.uxTownshipPartialLabel.Text = "Partial Code:"
        '
        'uxTownshipGroupBox
        '
        Me.uxTownshipGroupBox.Controls.Add(Me.uxTownshipPartialLabel)
        Me.uxTownshipGroupBox.Controls.Add(Me.uxTownshipPartial)
        Me.uxTownshipGroupBox.Controls.Add(Me.uxTownshipDirectional)
        Me.uxTownshipGroupBox.Controls.Add(Me.uxTownshipDirectionalLabel)
        Me.uxTownshipGroupBox.Controls.Add(Me.uxTownship)
        Me.uxTownshipGroupBox.Controls.Add(Me.uxTownshipLabel)
        Me.uxTownshipGroupBox.Location = New System.Drawing.Point(10, 6)
        Me.uxTownshipGroupBox.Name = "uxTownshipGroupBox"
        Me.uxTownshipGroupBox.Size = New System.Drawing.Size(181, 92)
        Me.uxTownshipGroupBox.TabIndex = 0
        Me.uxTownshipGroupBox.TabStop = False
        Me.uxTownshipGroupBox.Text = "Township"
        '
        'uxRangeGroupBox
        '
        Me.uxRangeGroupBox.Controls.Add(Me.uxRangePartialLabel)
        Me.uxRangeGroupBox.Controls.Add(Me.uxRangeDirectionalLabel)
        Me.uxRangeGroupBox.Controls.Add(Me.uxRangeLabel)
        Me.uxRangeGroupBox.Controls.Add(Me.uxRangePartial)
        Me.uxRangeGroupBox.Controls.Add(Me.uxRangeDirectional)
        Me.uxRangeGroupBox.Controls.Add(Me.uxRange)
        Me.uxRangeGroupBox.Location = New System.Drawing.Point(10, 104)
        Me.uxRangeGroupBox.Name = "uxRangeGroupBox"
        Me.uxRangeGroupBox.Size = New System.Drawing.Size(181, 92)
        Me.uxRangeGroupBox.TabIndex = 1
        Me.uxRangeGroupBox.TabStop = False
        Me.uxRangeGroupBox.Text = "Range"
        '
        'uxRangePartialLabel
        '
        Me.uxRangePartialLabel.AutoSize = True
        Me.uxRangePartialLabel.Location = New System.Drawing.Point(7, 67)
        Me.uxRangePartialLabel.Name = "uxRangePartialLabel"
        Me.uxRangePartialLabel.Size = New System.Drawing.Size(67, 13)
        Me.uxRangePartialLabel.TabIndex = 4
        Me.uxRangePartialLabel.Text = "Partial Code:"
        '
        'uxRangeDirectionalLabel
        '
        Me.uxRangeDirectionalLabel.AutoSize = True
        Me.uxRangeDirectionalLabel.Location = New System.Drawing.Point(7, 42)
        Me.uxRangeDirectionalLabel.Name = "uxRangeDirectionalLabel"
        Me.uxRangeDirectionalLabel.Size = New System.Drawing.Size(60, 13)
        Me.uxRangeDirectionalLabel.TabIndex = 2
        Me.uxRangeDirectionalLabel.Text = "Directional:"
        '
        'uxRangeLabel
        '
        Me.uxRangeLabel.AutoSize = True
        Me.uxRangeLabel.Location = New System.Drawing.Point(7, 17)
        Me.uxRangeLabel.Name = "uxRangeLabel"
        Me.uxRangeLabel.Size = New System.Drawing.Size(47, 13)
        Me.uxRangeLabel.TabIndex = 0
        Me.uxRangeLabel.Text = "Number:"
        '
        'uxRangePartial
        '
        Me.uxRangePartial.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.uxRangePartial.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.uxRangePartial.FormattingEnabled = True
        Me.uxRangePartial.Location = New System.Drawing.Point(111, 64)
        Me.uxRangePartial.Name = "uxRangePartial"
        Me.uxRangePartial.Size = New System.Drawing.Size(60, 21)
        Me.uxRangePartial.TabIndex = 5
        '
        'uxRangeDirectional
        '
        Me.uxRangeDirectional.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.uxRangeDirectional.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.uxRangeDirectional.FormattingEnabled = True
        Me.uxRangeDirectional.Location = New System.Drawing.Point(111, 39)
        Me.uxRangeDirectional.Name = "uxRangeDirectional"
        Me.uxRangeDirectional.Size = New System.Drawing.Size(60, 21)
        Me.uxRangeDirectional.TabIndex = 3
        '
        'uxRange
        '
        Me.uxRange.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.uxRange.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.uxRange.FormattingEnabled = True
        Me.uxRange.Location = New System.Drawing.Point(111, 14)
        Me.uxRange.Name = "uxRange"
        Me.uxRange.Size = New System.Drawing.Size(60, 21)
        Me.uxRange.TabIndex = 1
        '
        'uxSectionGroupBox
        '
        Me.uxSectionGroupBox.Controls.Add(Me.uxSectionQtrQtrLabel)
        Me.uxSectionGroupBox.Controls.Add(Me.uxSectionQtrLabel)
        Me.uxSectionGroupBox.Controls.Add(Me.uxSectionLabel)
        Me.uxSectionGroupBox.Controls.Add(Me.uxSectionQtrQtr)
        Me.uxSectionGroupBox.Controls.Add(Me.uxSectionQuarter)
        Me.uxSectionGroupBox.Controls.Add(Me.uxSection)
        Me.uxSectionGroupBox.Location = New System.Drawing.Point(10, 203)
        Me.uxSectionGroupBox.Name = "uxSectionGroupBox"
        Me.uxSectionGroupBox.Size = New System.Drawing.Size(181, 92)
        Me.uxSectionGroupBox.TabIndex = 2
        Me.uxSectionGroupBox.TabStop = False
        Me.uxSectionGroupBox.Text = "Section"
        '
        'uxSectionQtrQtrLabel
        '
        Me.uxSectionQtrQtrLabel.AutoSize = True
        Me.uxSectionQtrQtrLabel.Location = New System.Drawing.Point(7, 67)
        Me.uxSectionQtrQtrLabel.Name = "uxSectionQtrQtrLabel"
        Me.uxSectionQtrQtrLabel.Size = New System.Drawing.Size(95, 13)
        Me.uxSectionQtrQtrLabel.TabIndex = 4
        Me.uxSectionQtrQtrLabel.Text = "Quarter of Quarter:"
        '
        'uxSectionQtrLabel
        '
        Me.uxSectionQtrLabel.AutoSize = True
        Me.uxSectionQtrLabel.Location = New System.Drawing.Point(7, 42)
        Me.uxSectionQtrLabel.Name = "uxSectionQtrLabel"
        Me.uxSectionQtrLabel.Size = New System.Drawing.Size(45, 13)
        Me.uxSectionQtrLabel.TabIndex = 2
        Me.uxSectionQtrLabel.Text = "Quarter:"
        '
        'uxSectionLabel
        '
        Me.uxSectionLabel.AutoSize = True
        Me.uxSectionLabel.Location = New System.Drawing.Point(7, 17)
        Me.uxSectionLabel.Name = "uxSectionLabel"
        Me.uxSectionLabel.Size = New System.Drawing.Size(47, 13)
        Me.uxSectionLabel.TabIndex = 0
        Me.uxSectionLabel.Text = "Number:"
        '
        'uxSectionQtrQtr
        '
        Me.uxSectionQtrQtr.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.uxSectionQtrQtr.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.uxSectionQtrQtr.FormattingEnabled = True
        Me.uxSectionQtrQtr.Location = New System.Drawing.Point(111, 64)
        Me.uxSectionQtrQtr.Name = "uxSectionQtrQtr"
        Me.uxSectionQtrQtr.Size = New System.Drawing.Size(60, 21)
        Me.uxSectionQtrQtr.TabIndex = 5
        '
        'uxSectionQuarter
        '
        Me.uxSectionQuarter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.uxSectionQuarter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.uxSectionQuarter.FormattingEnabled = True
        Me.uxSectionQuarter.Location = New System.Drawing.Point(111, 39)
        Me.uxSectionQuarter.Name = "uxSectionQuarter"
        Me.uxSectionQuarter.Size = New System.Drawing.Size(60, 21)
        Me.uxSectionQuarter.TabIndex = 3
        '
        'uxSection
        '
        Me.uxSection.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.uxSection.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.uxSection.FormattingEnabled = True
        Me.uxSection.Location = New System.Drawing.Point(111, 14)
        Me.uxSection.Name = "uxSection"
        Me.uxSection.Size = New System.Drawing.Size(60, 21)
        Me.uxSection.TabIndex = 1
        '
        'uxMapInfoGroupBox
        '
        Me.uxMapInfoGroupBox.Controls.Add(Me.uxAnomalyLabel)
        Me.uxMapInfoGroupBox.Controls.Add(Me.uxPageLabel)
        Me.uxMapInfoGroupBox.Controls.Add(Me.uxScaleLabel)
        Me.uxMapInfoGroupBox.Controls.Add(Me.uxReliabilityLabel)
        Me.uxMapInfoGroupBox.Controls.Add(Me.uxSuffixTypeLabel)
        Me.uxMapInfoGroupBox.Controls.Add(Me.uxSuffixNumberLabel)
        Me.uxMapInfoGroupBox.Controls.Add(Me.uxMapNumberLabel)
        Me.uxMapInfoGroupBox.Controls.Add(Me.uxAnomaly)
        Me.uxMapInfoGroupBox.Controls.Add(Me.uxPage)
        Me.uxMapInfoGroupBox.Controls.Add(Me.uxScale)
        Me.uxMapInfoGroupBox.Controls.Add(Me.uxReliability)
        Me.uxMapInfoGroupBox.Controls.Add(Me.uxCountyLabel)
        Me.uxMapInfoGroupBox.Controls.Add(Me.uxSuffixType)
        Me.uxMapInfoGroupBox.Controls.Add(Me.uxSuffixNumber)
        Me.uxMapInfoGroupBox.Controls.Add(Me.uxMapNumber)
        Me.uxMapInfoGroupBox.Controls.Add(Me.uxCounty)
        Me.uxMapInfoGroupBox.Location = New System.Drawing.Point(217, 6)
        Me.uxMapInfoGroupBox.Name = "uxMapInfoGroupBox"
        Me.uxMapInfoGroupBox.Size = New System.Drawing.Size(273, 214)
        Me.uxMapInfoGroupBox.TabIndex = 3
        Me.uxMapInfoGroupBox.TabStop = False
        Me.uxMapInfoGroupBox.Text = "Map Information"
        '
        'uxAnomalyLabel
        '
        Me.uxAnomalyLabel.AutoSize = True
        Me.uxAnomalyLabel.Location = New System.Drawing.Point(9, 42)
        Me.uxAnomalyLabel.Name = "uxAnomalyLabel"
        Me.uxAnomalyLabel.Size = New System.Drawing.Size(50, 13)
        Me.uxAnomalyLabel.TabIndex = 2
        Me.uxAnomalyLabel.Text = "Anomaly:"
        '
        'uxPageLabel
        '
        Me.uxPageLabel.AutoSize = True
        Me.uxPageLabel.Location = New System.Drawing.Point(9, 164)
        Me.uxPageLabel.Name = "uxPageLabel"
        Me.uxPageLabel.Size = New System.Drawing.Size(75, 13)
        Me.uxPageLabel.TabIndex = 12
        Me.uxPageLabel.Text = "Page Number:"
        '
        'uxScaleLabel
        '
        Me.uxScaleLabel.AutoSize = True
        Me.uxScaleLabel.Location = New System.Drawing.Point(9, 115)
        Me.uxScaleLabel.Name = "uxScaleLabel"
        Me.uxScaleLabel.Size = New System.Drawing.Size(58, 13)
        Me.uxScaleLabel.TabIndex = 8
        Me.uxScaleLabel.Text = "MapScale:"
        '
        'uxReliabilityLabel
        '
        Me.uxReliabilityLabel.AutoSize = True
        Me.uxReliabilityLabel.Location = New System.Drawing.Point(9, 188)
        Me.uxReliabilityLabel.Name = "uxReliabilityLabel"
        Me.uxReliabilityLabel.Size = New System.Drawing.Size(54, 13)
        Me.uxReliabilityLabel.TabIndex = 14
        Me.uxReliabilityLabel.Text = "Reliability:"
        '
        'uxSuffixTypeLabel
        '
        Me.uxSuffixTypeLabel.AutoSize = True
        Me.uxSuffixTypeLabel.Location = New System.Drawing.Point(9, 66)
        Me.uxSuffixTypeLabel.Name = "uxSuffixTypeLabel"
        Me.uxSuffixTypeLabel.Size = New System.Drawing.Size(87, 13)
        Me.uxSuffixTypeLabel.TabIndex = 4
        Me.uxSuffixTypeLabel.Text = "Map Suffix Type:"
        '
        'uxSuffixNumberLabel
        '
        Me.uxSuffixNumberLabel.AutoSize = True
        Me.uxSuffixNumberLabel.Location = New System.Drawing.Point(9, 91)
        Me.uxSuffixNumberLabel.Name = "uxSuffixNumberLabel"
        Me.uxSuffixNumberLabel.Size = New System.Drawing.Size(100, 13)
        Me.uxSuffixNumberLabel.TabIndex = 6
        Me.uxSuffixNumberLabel.Text = "Map Suffix Number:"
        '
        'uxMapNumberLabel
        '
        Me.uxMapNumberLabel.AutoSize = True
        Me.uxMapNumberLabel.Location = New System.Drawing.Point(9, 140)
        Me.uxMapNumberLabel.Name = "uxMapNumberLabel"
        Me.uxMapNumberLabel.Size = New System.Drawing.Size(71, 13)
        Me.uxMapNumberLabel.TabIndex = 10
        Me.uxMapNumberLabel.Text = "Map Number:"
        '
        'uxAnomaly
        '
        Me.uxAnomaly.Location = New System.Drawing.Point(115, 39)
        Me.uxAnomaly.Name = "uxAnomaly"
        Me.uxAnomaly.Size = New System.Drawing.Size(45, 20)
        Me.uxAnomaly.TabIndex = 3
        '
        'uxPage
        '
        Me.uxPage.Location = New System.Drawing.Point(115, 161)
        Me.uxPage.Name = "uxPage"
        Me.uxPage.Size = New System.Drawing.Size(45, 20)
        Me.uxPage.TabIndex = 13
        '
        'uxScale
        '
        Me.uxScale.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.uxScale.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.uxScale.FormattingEnabled = True
        Me.uxScale.Location = New System.Drawing.Point(115, 112)
        Me.uxScale.Name = "uxScale"
        Me.uxScale.Size = New System.Drawing.Size(130, 21)
        Me.uxScale.TabIndex = 9
        '
        'uxReliability
        '
        Me.uxReliability.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.uxReliability.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.uxReliability.FormattingEnabled = True
        Me.uxReliability.Location = New System.Drawing.Point(115, 185)
        Me.uxReliability.Name = "uxReliability"
        Me.uxReliability.Size = New System.Drawing.Size(130, 21)
        Me.uxReliability.TabIndex = 15
        '
        'uxCountyLabel
        '
        Me.uxCountyLabel.AutoSize = True
        Me.uxCountyLabel.Location = New System.Drawing.Point(9, 17)
        Me.uxCountyLabel.Name = "uxCountyLabel"
        Me.uxCountyLabel.Size = New System.Drawing.Size(43, 13)
        Me.uxCountyLabel.TabIndex = 0
        Me.uxCountyLabel.Text = "County:"
        '
        'uxSuffixType
        '
        Me.uxSuffixType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.uxSuffixType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.uxSuffixType.FormattingEnabled = True
        Me.uxSuffixType.Location = New System.Drawing.Point(115, 63)
        Me.uxSuffixType.Name = "uxSuffixType"
        Me.uxSuffixType.Size = New System.Drawing.Size(130, 21)
        Me.uxSuffixType.TabIndex = 5
        '
        'uxSuffixNumber
        '
        Me.uxSuffixNumber.Location = New System.Drawing.Point(115, 88)
        Me.uxSuffixNumber.Name = "uxSuffixNumber"
        Me.uxSuffixNumber.Size = New System.Drawing.Size(130, 20)
        Me.uxSuffixNumber.TabIndex = 7
        '
        'uxMapNumber
        '
        Me.uxMapNumber.Location = New System.Drawing.Point(115, 137)
        Me.uxMapNumber.Name = "uxMapNumber"
        Me.uxMapNumber.Size = New System.Drawing.Size(130, 20)
        Me.uxMapNumber.TabIndex = 11
        '
        'uxCounty
        '
        Me.uxCounty.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.uxCounty.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.uxCounty.DropDownWidth = 130
        Me.uxCounty.FormattingEnabled = True
        Me.uxCounty.Location = New System.Drawing.Point(115, 14)
        Me.uxCounty.Name = "uxCounty"
        Me.uxCounty.Size = New System.Drawing.Size(130, 21)
        Me.uxCounty.TabIndex = 1
        '
        'uxORMAPNumberGroupBox
        '
        Me.uxORMAPNumberGroupBox.Controls.Add(Me.uxORMAPNumberLabel)
        Me.uxORMAPNumberGroupBox.Location = New System.Drawing.Point(217, 227)
        Me.uxORMAPNumberGroupBox.Name = "uxORMAPNumberGroupBox"
        Me.uxORMAPNumberGroupBox.Size = New System.Drawing.Size(253, 68)
        Me.uxORMAPNumberGroupBox.TabIndex = 4
        Me.uxORMAPNumberGroupBox.TabStop = False
        Me.uxORMAPNumberGroupBox.Text = "Preview"
        '
        'uxORMAPNumberLabel
        '
        Me.uxORMAPNumberLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.uxORMAPNumberLabel.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxORMAPNumberLabel.Location = New System.Drawing.Point(11, 25)
        Me.uxORMAPNumberLabel.Name = "uxORMAPNumberLabel"
        Me.uxORMAPNumberLabel.Size = New System.Drawing.Size(231, 25)
        Me.uxORMAPNumberLabel.TabIndex = 0
        Me.uxORMAPNumberLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'uxHelp
        '
        Me.uxHelp.Location = New System.Drawing.Point(395, 301)
        Me.uxHelp.Name = "uxHelp"
        Me.uxHelp.Size = New System.Drawing.Size(75, 23)
        Me.uxHelp.TabIndex = 8
        Me.uxHelp.Text = "&Help"
        Me.uxHelp.UseVisualStyleBackColor = True
        '
        'uxOK
        '
        Me.uxOK.Location = New System.Drawing.Point(139, 301)
        Me.uxOK.Name = "uxOK"
        Me.uxOK.Size = New System.Drawing.Size(75, 23)
        Me.uxOK.TabIndex = 5
        Me.uxOK.Text = "&OK"
        Me.uxOK.UseVisualStyleBackColor = True
        '
        'uxCancel
        '
        Me.uxCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.uxCancel.Location = New System.Drawing.Point(301, 301)
        Me.uxCancel.Name = "uxCancel"
        Me.uxCancel.Size = New System.Drawing.Size(75, 23)
        Me.uxCancel.TabIndex = 7
        Me.uxCancel.Text = "&Cancel"
        Me.uxCancel.UseVisualStyleBackColor = True
        '
        'uxApply
        '
        Me.uxApply.Location = New System.Drawing.Point(220, 301)
        Me.uxApply.Name = "uxApply"
        Me.uxApply.Size = New System.Drawing.Size(75, 23)
        Me.uxApply.TabIndex = 6
        Me.uxApply.Text = "&Apply"
        Me.uxApply.UseVisualStyleBackColor = True
        '
        'ErrorProviderSuffixNum
        '
        Me.ErrorProviderSuffixNum.ContainerControl = Me
        '
        'EditMapIndexForm
        '
        Me.AcceptButton = Me.uxOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.uxCancel
        Me.ClientSize = New System.Drawing.Size(502, 333)
        Me.Controls.Add(Me.uxApply)
        Me.Controls.Add(Me.uxCancel)
        Me.Controls.Add(Me.uxOK)
        Me.Controls.Add(Me.uxHelp)
        Me.Controls.Add(Me.uxORMAPNumberGroupBox)
        Me.Controls.Add(Me.uxMapInfoGroupBox)
        Me.Controls.Add(Me.uxSectionGroupBox)
        Me.Controls.Add(Me.uxRangeGroupBox)
        Me.Controls.Add(Me.uxTownshipGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "EditMapIndexForm"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Edit Map Index"
        Me.TopMost = True
        Me.uxTownshipGroupBox.ResumeLayout(False)
        Me.uxTownshipGroupBox.PerformLayout()
        Me.uxRangeGroupBox.ResumeLayout(False)
        Me.uxRangeGroupBox.PerformLayout()
        Me.uxSectionGroupBox.ResumeLayout(False)
        Me.uxSectionGroupBox.PerformLayout()
        Me.uxMapInfoGroupBox.ResumeLayout(False)
        Me.uxMapInfoGroupBox.PerformLayout()
        Me.uxORMAPNumberGroupBox.ResumeLayout(False)
        CType(Me.ErrorProviderSuffixNum, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents uxTownshipDirectionalLabel As System.Windows.Forms.Label
    Friend WithEvents uxTownshipLabel As System.Windows.Forms.Label
    Friend WithEvents uxTownshipPartial As System.Windows.Forms.ComboBox
    Friend WithEvents uxTownshipDirectional As System.Windows.Forms.ComboBox
    Friend WithEvents uxTownship As System.Windows.Forms.ComboBox
    Friend WithEvents uxTownshipPartialLabel As System.Windows.Forms.Label
    Friend WithEvents uxTownshipGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents uxRangeGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents uxSectionGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents uxRangeDirectional As System.Windows.Forms.ComboBox
    Friend WithEvents uxRange As System.Windows.Forms.ComboBox
    Friend WithEvents uxMapInfoGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents uxORMAPNumberGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents uxRangeLabel As System.Windows.Forms.Label
    Friend WithEvents uxRangePartial As System.Windows.Forms.ComboBox
    Friend WithEvents uxRangeDirectionalLabel As System.Windows.Forms.Label
    Friend WithEvents uxRangePartialLabel As System.Windows.Forms.Label
    Friend WithEvents uxSectionLabel As System.Windows.Forms.Label
    Friend WithEvents uxSectionQtrQtr As System.Windows.Forms.ComboBox
    Friend WithEvents uxSectionQuarter As System.Windows.Forms.ComboBox
    Friend WithEvents uxSection As System.Windows.Forms.ComboBox
    Friend WithEvents uxSectionQtrQtrLabel As System.Windows.Forms.Label
    Friend WithEvents uxSectionQtrLabel As System.Windows.Forms.Label
    Friend WithEvents uxSuffixNumber As System.Windows.Forms.TextBox
    Friend WithEvents uxMapNumber As System.Windows.Forms.TextBox
    Friend WithEvents uxCounty As System.Windows.Forms.ComboBox
    Friend WithEvents uxSuffixType As System.Windows.Forms.ComboBox
    Friend WithEvents uxAnomaly As System.Windows.Forms.TextBox
    Friend WithEvents uxPage As System.Windows.Forms.TextBox
    Friend WithEvents uxScale As System.Windows.Forms.ComboBox
    Friend WithEvents uxReliability As System.Windows.Forms.ComboBox
    Friend WithEvents uxCountyLabel As System.Windows.Forms.Label
    Friend WithEvents uxORMAPNumberLabel As System.Windows.Forms.Label
    Friend WithEvents uxPageLabel As System.Windows.Forms.Label
    Friend WithEvents uxScaleLabel As System.Windows.Forms.Label
    Friend WithEvents uxReliabilityLabel As System.Windows.Forms.Label
    Friend WithEvents uxSuffixTypeLabel As System.Windows.Forms.Label
    Friend WithEvents uxSuffixNumberLabel As System.Windows.Forms.Label
    Friend WithEvents uxMapNumberLabel As System.Windows.Forms.Label
    Friend WithEvents uxAnomalyLabel As System.Windows.Forms.Label
    Friend WithEvents uxHelp As System.Windows.Forms.Button
    Friend WithEvents uxOK As System.Windows.Forms.Button
    Friend WithEvents uxCancel As System.Windows.Forms.Button
    Friend WithEvents uxApply As System.Windows.Forms.Button
    Friend WithEvents ErrorProviderSuffixNum As System.Windows.Forms.ErrorProvider
End Class
