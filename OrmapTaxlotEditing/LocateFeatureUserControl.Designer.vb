<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LocateFeatureUserControl
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.uxSelectFeatures = New System.Windows.Forms.CheckBox
        Me.uxFind = New System.Windows.Forms.Button
        Me.uxEditingGroupBox = New System.Windows.Forms.GroupBox
        Me.uxCurrentlyAttNum = New System.Windows.Forms.Label
        Me.uxSetAttributeMode = New System.Windows.Forms.Button
        Me.uxAttributeMode = New System.Windows.Forms.Label
        Me.uxCurrentlyAttLbl = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.uxMapNumber = New System.Windows.Forms.TextBox
        Me.uxTaxlot = New System.Windows.Forms.TextBox
        Me.TaxlotLabel = New System.Windows.Forms.Label
        Me.MapnumberLabel = New System.Windows.Forms.Label
        Me.uxHelp = New System.Windows.Forms.Button
        Me.uxTimer = New System.Windows.Forms.Timer(Me.components)
        Me.uxDisplayGroupBox = New System.Windows.Forms.GroupBox
        Me.uxSetDefinitionQuery = New System.Windows.Forms.Button
        Me.uxClearDefinitionQuery = New System.Windows.Forms.Button
        Me.uxORMAPProperties = New System.Windows.Forms.Button
        Me.GroupBox1.SuspendLayout()
        Me.uxEditingGroupBox.SuspendLayout()
        Me.uxDisplayGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.uxSelectFeatures)
        Me.GroupBox1.Controls.Add(Me.uxFind)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 46)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(235, 41)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Locate:"
        '
        'uxSelectFeatures
        '
        Me.uxSelectFeatures.AutoSize = True
        Me.uxSelectFeatures.Location = New System.Drawing.Point(5, 19)
        Me.uxSelectFeatures.Name = "uxSelectFeatures"
        Me.uxSelectFeatures.Size = New System.Drawing.Size(100, 17)
        Me.uxSelectFeatures.TabIndex = 3
        Me.uxSelectFeatures.Text = "Select features."
        Me.uxSelectFeatures.UseVisualStyleBackColor = True
        '
        'uxFind
        '
        Me.uxFind.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxFind.Location = New System.Drawing.Point(154, 13)
        Me.uxFind.Margin = New System.Windows.Forms.Padding(2)
        Me.uxFind.Name = "uxFind"
        Me.uxFind.Size = New System.Drawing.Size(75, 23)
        Me.uxFind.TabIndex = 4
        Me.uxFind.Text = "&Find"
        Me.uxFind.UseVisualStyleBackColor = True
        '
        'uxEditingGroupBox
        '
        Me.uxEditingGroupBox.Controls.Add(Me.uxCurrentlyAttNum)
        Me.uxEditingGroupBox.Controls.Add(Me.uxSetAttributeMode)
        Me.uxEditingGroupBox.Controls.Add(Me.uxAttributeMode)
        Me.uxEditingGroupBox.Controls.Add(Me.uxCurrentlyAttLbl)
        Me.uxEditingGroupBox.Controls.Add(Me.Label1)
        Me.uxEditingGroupBox.Location = New System.Drawing.Point(8, 93)
        Me.uxEditingGroupBox.Name = "uxEditingGroupBox"
        Me.uxEditingGroupBox.Size = New System.Drawing.Size(235, 72)
        Me.uxEditingGroupBox.TabIndex = 14
        Me.uxEditingGroupBox.TabStop = False
        Me.uxEditingGroupBox.Text = "Editing:"
        '
        'uxCurrentlyAttNum
        '
        Me.uxCurrentlyAttNum.AutoSize = True
        Me.uxCurrentlyAttNum.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxCurrentlyAttNum.Location = New System.Drawing.Point(6, 52)
        Me.uxCurrentlyAttNum.Name = "uxCurrentlyAttNum"
        Me.uxCurrentlyAttNum.Size = New System.Drawing.Size(85, 13)
        Me.uxCurrentlyAttNum.TabIndex = 0
        Me.uxCurrentlyAttNum.Text = "<mapnumber>"
        Me.uxCurrentlyAttNum.Visible = False
        '
        'uxSetAttributeMode
        '
        Me.uxSetAttributeMode.Location = New System.Drawing.Point(154, 42)
        Me.uxSetAttributeMode.Name = "uxSetAttributeMode"
        Me.uxSetAttributeMode.Size = New System.Drawing.Size(75, 23)
        Me.uxSetAttributeMode.TabIndex = 5
        Me.uxSetAttributeMode.Text = "Set &Manual"
        Me.uxSetAttributeMode.UseVisualStyleBackColor = True
        '
        'uxAttributeMode
        '
        Me.uxAttributeMode.AutoSize = True
        Me.uxAttributeMode.Location = New System.Drawing.Point(86, 16)
        Me.uxAttributeMode.Name = "uxAttributeMode"
        Me.uxAttributeMode.Size = New System.Drawing.Size(29, 13)
        Me.uxAttributeMode.TabIndex = 0
        Me.uxAttributeMode.Text = "Auto"
        '
        'uxCurrentlyAttLbl
        '
        Me.uxCurrentlyAttLbl.AutoSize = True
        Me.uxCurrentlyAttLbl.Location = New System.Drawing.Point(6, 37)
        Me.uxCurrentlyAttLbl.Name = "uxCurrentlyAttLbl"
        Me.uxCurrentlyAttLbl.Size = New System.Drawing.Size(101, 13)
        Me.uxCurrentlyAttLbl.TabIndex = 0
        Me.uxCurrentlyAttLbl.Text = "Currently Attributing:"
        Me.uxCurrentlyAttLbl.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(79, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Attribute Mode:"
        '
        'uxMapNumber
        '
        Me.uxMapNumber.AllowDrop = True
        Me.uxMapNumber.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.uxMapNumber.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Me.uxMapNumber.Location = New System.Drawing.Point(8, 21)
        Me.uxMapNumber.Margin = New System.Windows.Forms.Padding(2)
        Me.uxMapNumber.MaxLength = 12
        Me.uxMapNumber.Name = "uxMapNumber"
        Me.uxMapNumber.Size = New System.Drawing.Size(134, 20)
        Me.uxMapNumber.TabIndex = 0
        '
        'uxTaxlot
        '
        Me.uxTaxlot.AllowDrop = True
        Me.uxTaxlot.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.uxTaxlot.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Me.uxTaxlot.CausesValidation = False
        Me.uxTaxlot.Location = New System.Drawing.Point(150, 21)
        Me.uxTaxlot.Margin = New System.Windows.Forms.Padding(2)
        Me.uxTaxlot.Name = "uxTaxlot"
        Me.uxTaxlot.Size = New System.Drawing.Size(93, 20)
        Me.uxTaxlot.TabIndex = 1
        '
        'TaxlotLabel
        '
        Me.TaxlotLabel.AutoSize = True
        Me.TaxlotLabel.Location = New System.Drawing.Point(147, 6)
        Me.TaxlotLabel.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.TaxlotLabel.Name = "TaxlotLabel"
        Me.TaxlotLabel.Size = New System.Drawing.Size(39, 13)
        Me.TaxlotLabel.TabIndex = 11
        Me.TaxlotLabel.Text = "Taxlot:"
        '
        'MapnumberLabel
        '
        Me.MapnumberLabel.AutoSize = True
        Me.MapnumberLabel.Location = New System.Drawing.Point(5, 6)
        Me.MapnumberLabel.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.MapnumberLabel.Name = "MapnumberLabel"
        Me.MapnumberLabel.Size = New System.Drawing.Size(71, 13)
        Me.MapnumberLabel.TabIndex = 9
        Me.MapnumberLabel.Text = "Map Number:"
        '
        'uxHelp
        '
        Me.uxHelp.Location = New System.Drawing.Point(162, 230)
        Me.uxHelp.Name = "uxHelp"
        Me.uxHelp.Size = New System.Drawing.Size(75, 23)
        Me.uxHelp.TabIndex = 35
        Me.uxHelp.TabStop = False
        Me.uxHelp.Text = "&Help"
        Me.uxHelp.UseVisualStyleBackColor = True
        '
        'uxTimer
        '
        Me.uxTimer.Interval = 1000
        '
        'uxDisplayGroupBox
        '
        Me.uxDisplayGroupBox.Controls.Add(Me.uxORMAPProperties)
        Me.uxDisplayGroupBox.Controls.Add(Me.uxSetDefinitionQuery)
        Me.uxDisplayGroupBox.Controls.Add(Me.uxClearDefinitionQuery)
        Me.uxDisplayGroupBox.Location = New System.Drawing.Point(8, 172)
        Me.uxDisplayGroupBox.Name = "uxDisplayGroupBox"
        Me.uxDisplayGroupBox.Size = New System.Drawing.Size(239, 52)
        Me.uxDisplayGroupBox.TabIndex = 36
        Me.uxDisplayGroupBox.TabStop = False
        Me.uxDisplayGroupBox.Text = "Definition Query"
        '
        'uxSetDefinitionQuery
        '
        Me.uxSetDefinitionQuery.Location = New System.Drawing.Point(9, 19)
        Me.uxSetDefinitionQuery.Name = "uxSetDefinitionQuery"
        Me.uxSetDefinitionQuery.Size = New System.Drawing.Size(75, 23)
        Me.uxSetDefinitionQuery.TabIndex = 1
        Me.uxSetDefinitionQuery.Text = "Set Query"
        Me.uxSetDefinitionQuery.UseVisualStyleBackColor = True
        '
        'uxClearDefinitionQuery
        '
        Me.uxClearDefinitionQuery.Location = New System.Drawing.Point(91, 19)
        Me.uxClearDefinitionQuery.Name = "uxClearDefinitionQuery"
        Me.uxClearDefinitionQuery.Size = New System.Drawing.Size(64, 23)
        Me.uxClearDefinitionQuery.TabIndex = 2
        Me.uxClearDefinitionQuery.Text = "Clear"
        Me.uxClearDefinitionQuery.UseVisualStyleBackColor = True
        '
        'uxORMAPProperties
        '
        Me.uxORMAPProperties.Location = New System.Drawing.Point(161, 19)
        Me.uxORMAPProperties.Name = "uxORMAPProperties"
        Me.uxORMAPProperties.Size = New System.Drawing.Size(67, 23)
        Me.uxORMAPProperties.TabIndex = 3
        Me.uxORMAPProperties.Text = "Properties"
        Me.uxORMAPProperties.UseVisualStyleBackColor = True
        '
        'LocateFeatureUserControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.uxDisplayGroupBox)
        Me.Controls.Add(Me.uxHelp)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.uxEditingGroupBox)
        Me.Controls.Add(Me.uxMapNumber)
        Me.Controls.Add(Me.uxTaxlot)
        Me.Controls.Add(Me.TaxlotLabel)
        Me.Controls.Add(Me.MapnumberLabel)
        Me.Name = "LocateFeatureUserControl"
        Me.Size = New System.Drawing.Size(250, 269)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.uxEditingGroupBox.ResumeLayout(False)
        Me.uxEditingGroupBox.PerformLayout()
        Me.uxDisplayGroupBox.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents uxSelectFeatures As System.Windows.Forms.CheckBox
    Friend WithEvents uxFind As System.Windows.Forms.Button
    Friend WithEvents uxEditingGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents uxCurrentlyAttNum As System.Windows.Forms.Label
    Friend WithEvents uxSetAttributeMode As System.Windows.Forms.Button
    Friend WithEvents uxAttributeMode As System.Windows.Forms.Label
    Friend WithEvents uxCurrentlyAttLbl As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents uxMapNumber As System.Windows.Forms.TextBox
    Friend WithEvents uxTaxlot As System.Windows.Forms.TextBox
    Friend WithEvents TaxlotLabel As System.Windows.Forms.Label
    Friend WithEvents MapnumberLabel As System.Windows.Forms.Label
    Friend WithEvents uxHelp As System.Windows.Forms.Button
    Friend WithEvents uxTimer As System.Windows.Forms.Timer
    Friend WithEvents uxDisplayGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents uxClearDefinitionQuery As System.Windows.Forms.Button
    Friend WithEvents uxSetDefinitionQuery As System.Windows.Forms.Button
    Friend WithEvents uxORMAPProperties As System.Windows.Forms.Button

End Class
