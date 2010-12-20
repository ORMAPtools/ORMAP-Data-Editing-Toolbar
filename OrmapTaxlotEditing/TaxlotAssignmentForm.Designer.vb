<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TaxlotAssignmentForm
    Inherits System.Windows.Forms.Form

    'UserControl overrides dispose to clean up the component list.
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
        Me.uxTypeLabel = New System.Windows.Forms.Label
        Me.uxType = New System.Windows.Forms.ComboBox
        Me.uxTaxlotNumberingOptions = New System.Windows.Forms.GroupBox
        Me.uxIncrementByNone = New System.Windows.Forms.RadioButton
        Me.uxIncrementBy1 = New System.Windows.Forms.RadioButton
        Me.uxIncrementBy10 = New System.Windows.Forms.RadioButton
        Me.uxIncrementBy100 = New System.Windows.Forms.RadioButton
        Me.uxIncrementBy1000 = New System.Windows.Forms.RadioButton
        Me.uxIncrementByLabel = New System.Windows.Forms.Label
        Me.uxStartingFrom = New System.Windows.Forms.TextBox
        Me.uxStartingFromLabel = New System.Windows.Forms.Label
        Me.uxHelp = New System.Windows.Forms.Button
        Me.uxTaxlotNumberingOptions.SuspendLayout()
        Me.SuspendLayout()
        '
        'uxTypeLabel
        '
        Me.uxTypeLabel.BackColor = System.Drawing.SystemColors.Control
        Me.uxTypeLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me.uxTypeLabel.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxTypeLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.uxTypeLabel.Location = New System.Drawing.Point(10, 10)
        Me.uxTypeLabel.Name = "uxTypeLabel"
        Me.uxTypeLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.uxTypeLabel.Size = New System.Drawing.Size(40, 20)
        Me.uxTypeLabel.TabIndex = 0
        Me.uxTypeLabel.Text = "Type:"
        Me.uxTypeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'uxType
        '
        Me.uxType.BackColor = System.Drawing.SystemColors.Window
        Me.uxType.Cursor = System.Windows.Forms.Cursors.Default
        Me.uxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uxType.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.uxType.Location = New System.Drawing.Point(56, 10)
        Me.uxType.Name = "uxType"
        Me.uxType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.uxType.Size = New System.Drawing.Size(132, 22)
        Me.uxType.TabIndex = 1
        '
        'uxTaxlotNumberingOptions
        '
        Me.uxTaxlotNumberingOptions.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxTaxlotNumberingOptions.Controls.Add(Me.uxIncrementByNone)
        Me.uxTaxlotNumberingOptions.Controls.Add(Me.uxIncrementBy1)
        Me.uxTaxlotNumberingOptions.Controls.Add(Me.uxIncrementBy10)
        Me.uxTaxlotNumberingOptions.Controls.Add(Me.uxIncrementBy100)
        Me.uxTaxlotNumberingOptions.Controls.Add(Me.uxIncrementBy1000)
        Me.uxTaxlotNumberingOptions.Controls.Add(Me.uxIncrementByLabel)
        Me.uxTaxlotNumberingOptions.Controls.Add(Me.uxStartingFrom)
        Me.uxTaxlotNumberingOptions.Controls.Add(Me.uxStartingFromLabel)
        Me.uxTaxlotNumberingOptions.Location = New System.Drawing.Point(10, 40)
        Me.uxTaxlotNumberingOptions.Name = "uxTaxlotNumberingOptions"
        Me.uxTaxlotNumberingOptions.Size = New System.Drawing.Size(258, 94)
        Me.uxTaxlotNumberingOptions.TabIndex = 2
        Me.uxTaxlotNumberingOptions.TabStop = False
        Me.uxTaxlotNumberingOptions.Text = "Taxlot numbering options"
        '
        'uxIncrementByNone
        '
        Me.uxIncrementByNone.Appearance = System.Windows.Forms.Appearance.Button
        Me.uxIncrementByNone.Checked = True
        Me.uxIncrementByNone.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxIncrementByNone.Location = New System.Drawing.Point(7, 66)
        Me.uxIncrementByNone.Name = "uxIncrementByNone"
        Me.uxIncrementByNone.Size = New System.Drawing.Size(44, 20)
        Me.uxIncrementByNone.TabIndex = 3
        Me.uxIncrementByNone.TabStop = True
        Me.uxIncrementByNone.Text = "None"
        Me.uxIncrementByNone.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.uxIncrementByNone.UseVisualStyleBackColor = True
        '
        'uxIncrementBy1
        '
        Me.uxIncrementBy1.Appearance = System.Windows.Forms.Appearance.Button
        Me.uxIncrementBy1.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.Control
        Me.uxIncrementBy1.Location = New System.Drawing.Point(57, 66)
        Me.uxIncrementBy1.Name = "uxIncrementBy1"
        Me.uxIncrementBy1.Size = New System.Drawing.Size(44, 20)
        Me.uxIncrementBy1.TabIndex = 4
        Me.uxIncrementBy1.Text = "1"
        Me.uxIncrementBy1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.uxIncrementBy1.UseVisualStyleBackColor = True
        '
        'uxIncrementBy10
        '
        Me.uxIncrementBy10.Appearance = System.Windows.Forms.Appearance.Button
        Me.uxIncrementBy10.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.Control
        Me.uxIncrementBy10.Location = New System.Drawing.Point(107, 66)
        Me.uxIncrementBy10.Name = "uxIncrementBy10"
        Me.uxIncrementBy10.Size = New System.Drawing.Size(44, 20)
        Me.uxIncrementBy10.TabIndex = 5
        Me.uxIncrementBy10.Text = "10"
        Me.uxIncrementBy10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.uxIncrementBy10.UseVisualStyleBackColor = True
        '
        'uxIncrementBy100
        '
        Me.uxIncrementBy100.Appearance = System.Windows.Forms.Appearance.Button
        Me.uxIncrementBy100.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.Control
        Me.uxIncrementBy100.Location = New System.Drawing.Point(157, 66)
        Me.uxIncrementBy100.Name = "uxIncrementBy100"
        Me.uxIncrementBy100.Size = New System.Drawing.Size(44, 20)
        Me.uxIncrementBy100.TabIndex = 6
        Me.uxIncrementBy100.Text = "100"
        Me.uxIncrementBy100.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.uxIncrementBy100.UseVisualStyleBackColor = True
        '
        'uxIncrementBy1000
        '
        Me.uxIncrementBy1000.AccessibleDescription = ""
        Me.uxIncrementBy1000.Appearance = System.Windows.Forms.Appearance.Button
        Me.uxIncrementBy1000.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.Control
        Me.uxIncrementBy1000.Location = New System.Drawing.Point(207, 66)
        Me.uxIncrementBy1000.Name = "uxIncrementBy1000"
        Me.uxIncrementBy1000.Size = New System.Drawing.Size(44, 20)
        Me.uxIncrementBy1000.TabIndex = 7
        Me.uxIncrementBy1000.Text = "1000"
        Me.uxIncrementBy1000.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.uxIncrementBy1000.UseVisualStyleBackColor = True
        '
        'uxIncrementByLabel
        '
        Me.uxIncrementByLabel.BackColor = System.Drawing.SystemColors.Control
        Me.uxIncrementByLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me.uxIncrementByLabel.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxIncrementByLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.uxIncrementByLabel.Location = New System.Drawing.Point(6, 49)
        Me.uxIncrementByLabel.Name = "uxIncrementByLabel"
        Me.uxIncrementByLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.uxIncrementByLabel.Size = New System.Drawing.Size(75, 17)
        Me.uxIncrementByLabel.TabIndex = 2
        Me.uxIncrementByLabel.Text = "Increment by:"
        Me.uxIncrementByLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'uxStartingFrom
        '
        Me.uxStartingFrom.AcceptsReturn = True
        Me.uxStartingFrom.BackColor = System.Drawing.SystemColors.Window
        Me.uxStartingFrom.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.uxStartingFrom.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxStartingFrom.ForeColor = System.Drawing.SystemColors.WindowText
        Me.uxStartingFrom.Location = New System.Drawing.Point(87, 23)
        Me.uxStartingFrom.MaxLength = 5
        Me.uxStartingFrom.Name = "uxStartingFrom"
        Me.uxStartingFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.uxStartingFrom.Size = New System.Drawing.Size(74, 20)
        Me.uxStartingFrom.TabIndex = 1
        '
        'uxStartingFromLabel
        '
        Me.uxStartingFromLabel.BackColor = System.Drawing.SystemColors.Control
        Me.uxStartingFromLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me.uxStartingFromLabel.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxStartingFromLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.uxStartingFromLabel.Location = New System.Drawing.Point(6, 25)
        Me.uxStartingFromLabel.Name = "uxStartingFromLabel"
        Me.uxStartingFromLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.uxStartingFromLabel.Size = New System.Drawing.Size(75, 17)
        Me.uxStartingFromLabel.TabIndex = 0
        Me.uxStartingFromLabel.Text = "Starting from:"
        Me.uxStartingFromLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'uxHelp
        '
        Me.uxHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxHelp.Location = New System.Drawing.Point(194, 144)
        Me.uxHelp.Name = "uxHelp"
        Me.uxHelp.Size = New System.Drawing.Size(75, 23)
        Me.uxHelp.TabIndex = 3
        Me.uxHelp.Text = "&Help"
        Me.uxHelp.UseVisualStyleBackColor = True
        '
        'TaxlotAssignmentForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(277, 174)
        Me.Controls.Add(Me.uxHelp)
        Me.Controls.Add(Me.uxTypeLabel)
        Me.Controls.Add(Me.uxType)
        Me.Controls.Add(Me.uxTaxlotNumberingOptions)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "TaxlotAssignmentForm"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Taxlot Assignment"
        Me.TopMost = True
        Me.uxTaxlotNumberingOptions.ResumeLayout(False)
        Me.uxTaxlotNumberingOptions.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents uxTaxlotNumberingOptions As System.Windows.Forms.GroupBox
    Friend WithEvents uxIncrementByNone As System.Windows.Forms.RadioButton
    Friend WithEvents uxIncrementBy1 As System.Windows.Forms.RadioButton
    Friend WithEvents uxIncrementBy10 As System.Windows.Forms.RadioButton
    Friend WithEvents uxIncrementBy100 As System.Windows.Forms.RadioButton
    Friend WithEvents uxIncrementBy1000 As System.Windows.Forms.RadioButton
    Friend WithEvents uxTypeLabel As System.Windows.Forms.Label
    Friend WithEvents uxType As System.Windows.Forms.ComboBox
    Friend WithEvents uxStartingFrom As System.Windows.Forms.TextBox
    Friend WithEvents uxStartingFromLabel As System.Windows.Forms.Label
    Friend WithEvents uxIncrementByLabel As System.Windows.Forms.Label
    Friend WithEvents uxHelp As System.Windows.Forms.Button

End Class
