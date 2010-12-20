<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CombineTaxlotsForm
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
        Me.uxNewTaxlotNumber = New System.Windows.Forms.ComboBox
        Me.uxCombine = New System.Windows.Forms.Button
        Me.uxHelp = New System.Windows.Forms.Button
        Me.NewTaxlotNumberLabel = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'uxNewTaxlotNumber
        '
        Me.uxNewTaxlotNumber.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.uxNewTaxlotNumber.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.uxNewTaxlotNumber.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uxNewTaxlotNumber.FormattingEnabled = True
        Me.uxNewTaxlotNumber.Location = New System.Drawing.Point(9, 25)
        Me.uxNewTaxlotNumber.Margin = New System.Windows.Forms.Padding(2)
        Me.uxNewTaxlotNumber.Name = "uxNewTaxlotNumber"
        Me.uxNewTaxlotNumber.Size = New System.Drawing.Size(103, 21)
        Me.uxNewTaxlotNumber.Sorted = True
        Me.uxNewTaxlotNumber.TabIndex = 1
        '
        'uxCombine
        '
        Me.uxCombine.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxCombine.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.uxCombine.Location = New System.Drawing.Point(8, 55)
        Me.uxCombine.Margin = New System.Windows.Forms.Padding(2)
        Me.uxCombine.Name = "uxCombine"
        Me.uxCombine.Size = New System.Drawing.Size(75, 23)
        Me.uxCombine.TabIndex = 2
        Me.uxCombine.Text = "&Combine"
        Me.uxCombine.UseVisualStyleBackColor = True
        '
        'uxHelp
        '
        Me.uxHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxHelp.Location = New System.Drawing.Point(89, 55)
        Me.uxHelp.Margin = New System.Windows.Forms.Padding(2)
        Me.uxHelp.Name = "uxHelp"
        Me.uxHelp.Size = New System.Drawing.Size(75, 23)
        Me.uxHelp.TabIndex = 3
        Me.uxHelp.Text = "&Help"
        Me.uxHelp.UseVisualStyleBackColor = True
        '
        'NewTaxlotNumberLabel
        '
        Me.NewTaxlotNumberLabel.AutoSize = True
        Me.NewTaxlotNumberLabel.Location = New System.Drawing.Point(6, 9)
        Me.NewTaxlotNumberLabel.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.NewTaxlotNumberLabel.Name = "NewTaxlotNumberLabel"
        Me.NewTaxlotNumberLabel.Size = New System.Drawing.Size(104, 13)
        Me.NewTaxlotNumberLabel.TabIndex = 0
        Me.NewTaxlotNumberLabel.Text = "New Taxlot Number:"
        '
        'CombineTaxlotsForm
        '
        Me.AcceptButton = Me.uxCombine
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(173, 86)
        Me.Controls.Add(Me.uxCombine)
        Me.Controls.Add(Me.uxHelp)
        Me.Controls.Add(Me.uxNewTaxlotNumber)
        Me.Controls.Add(Me.NewTaxlotNumberLabel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "CombineTaxlotsForm"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Combine Taxlots"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents uxNewTaxlotNumber As System.Windows.Forms.ComboBox
    Friend WithEvents uxCombine As System.Windows.Forms.Button
    Friend WithEvents uxHelp As System.Windows.Forms.Button
    Friend WithEvents NewTaxlotNumberLabel As System.Windows.Forms.Label
End Class
