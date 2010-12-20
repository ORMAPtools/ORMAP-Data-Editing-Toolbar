<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DimensionArrowsForm
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
        Me.uxCurveLabel = New System.Windows.Forms.Label
        Me.uxLineLabel = New System.Windows.Forms.Label
        Me.uxSmoothLabel = New System.Windows.Forms.Label
        Me.uxReset = New System.Windows.Forms.Button
        Me.uxApply = New System.Windows.Forms.Button
        Me.uxRatioOfCurve = New System.Windows.Forms.TextBox
        Me.uxRatioOfLine = New System.Windows.Forms.TextBox
        Me.uxSmoothRatio = New System.Windows.Forms.TextBox
        Me.uxManuallyAddArrow = New System.Windows.Forms.CheckBox
        Me.uxDimensionPropertiesGroup = New System.Windows.Forms.GroupBox
        Me.uxErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.uxDimensionPropertiesGroup.SuspendLayout()
        CType(Me.uxErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'uxCurveLabel
        '
        Me.uxCurveLabel.AutoSize = True
        Me.uxCurveLabel.Location = New System.Drawing.Point(7, 23)
        Me.uxCurveLabel.Name = "uxCurveLabel"
        Me.uxCurveLabel.Size = New System.Drawing.Size(96, 13)
        Me.uxCurveLabel.TabIndex = 0
        Me.uxCurveLabel.Text = "Ratio of the Curve:"
        '
        'uxLineLabel
        '
        Me.uxLineLabel.AutoSize = True
        Me.uxLineLabel.Location = New System.Drawing.Point(7, 49)
        Me.uxLineLabel.Name = "uxLineLabel"
        Me.uxLineLabel.Size = New System.Drawing.Size(99, 13)
        Me.uxLineLabel.TabIndex = 2
        Me.uxLineLabel.Text = "Ratio from the Line:"
        '
        'uxSmoothLabel
        '
        Me.uxSmoothLabel.AutoSize = True
        Me.uxSmoothLabel.Location = New System.Drawing.Point(7, 75)
        Me.uxSmoothLabel.Name = "uxSmoothLabel"
        Me.uxSmoothLabel.Size = New System.Drawing.Size(74, 13)
        Me.uxSmoothLabel.TabIndex = 4
        Me.uxSmoothLabel.Text = "Smooth Ratio:"
        '
        'uxReset
        '
        Me.uxReset.Location = New System.Drawing.Point(114, 144)
        Me.uxReset.Name = "uxReset"
        Me.uxReset.Size = New System.Drawing.Size(75, 23)
        Me.uxReset.TabIndex = 3
        Me.uxReset.Text = "&Reset"
        Me.uxReset.UseVisualStyleBackColor = True
        '
        'uxApply
        '
        Me.uxApply.Location = New System.Drawing.Point(33, 144)
        Me.uxApply.Name = "uxApply"
        Me.uxApply.Size = New System.Drawing.Size(75, 23)
        Me.uxApply.TabIndex = 2
        Me.uxApply.Text = "&Apply"
        Me.uxApply.UseVisualStyleBackColor = True
        '
        'uxRatioOfCurve
        '
        Me.uxRatioOfCurve.Location = New System.Drawing.Point(122, 20)
        Me.uxRatioOfCurve.Name = "uxRatioOfCurve"
        Me.uxRatioOfCurve.Size = New System.Drawing.Size(44, 20)
        Me.uxRatioOfCurve.TabIndex = 1
        '
        'uxRatioOfLine
        '
        Me.uxRatioOfLine.Location = New System.Drawing.Point(122, 46)
        Me.uxRatioOfLine.Name = "uxRatioOfLine"
        Me.uxRatioOfLine.Size = New System.Drawing.Size(44, 20)
        Me.uxRatioOfLine.TabIndex = 3
        '
        'uxSmoothRatio
        '
        Me.uxSmoothRatio.Location = New System.Drawing.Point(122, 72)
        Me.uxSmoothRatio.Name = "uxSmoothRatio"
        Me.uxSmoothRatio.Size = New System.Drawing.Size(44, 20)
        Me.uxSmoothRatio.TabIndex = 5
        '
        'uxManuallyAddArrow
        '
        Me.uxManuallyAddArrow.AutoSize = True
        Me.uxManuallyAddArrow.Location = New System.Drawing.Point(10, 120)
        Me.uxManuallyAddArrow.Name = "uxManuallyAddArrow"
        Me.uxManuallyAddArrow.Size = New System.Drawing.Size(120, 17)
        Me.uxManuallyAddArrow.TabIndex = 1
        Me.uxManuallyAddArrow.Text = "Manually Add Arrow"
        Me.uxManuallyAddArrow.UseVisualStyleBackColor = True
        '
        'uxDimensionPropertiesGroup
        '
        Me.uxDimensionPropertiesGroup.Controls.Add(Me.uxRatioOfCurve)
        Me.uxDimensionPropertiesGroup.Controls.Add(Me.uxCurveLabel)
        Me.uxDimensionPropertiesGroup.Controls.Add(Me.uxSmoothRatio)
        Me.uxDimensionPropertiesGroup.Controls.Add(Me.uxLineLabel)
        Me.uxDimensionPropertiesGroup.Controls.Add(Me.uxRatioOfLine)
        Me.uxDimensionPropertiesGroup.Controls.Add(Me.uxSmoothLabel)
        Me.uxDimensionPropertiesGroup.Location = New System.Drawing.Point(10, 7)
        Me.uxDimensionPropertiesGroup.Name = "uxDimensionPropertiesGroup"
        Me.uxDimensionPropertiesGroup.Size = New System.Drawing.Size(179, 103)
        Me.uxDimensionPropertiesGroup.TabIndex = 0
        Me.uxDimensionPropertiesGroup.TabStop = False
        Me.uxDimensionPropertiesGroup.Text = "Dimension Arrow Properties"
        '
        'uxErrorProvider
        '
        Me.uxErrorProvider.ContainerControl = Me
        '
        'DimensionArrowsForm
        '
        Me.AcceptButton = Me.uxApply
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(198, 175)
        Me.Controls.Add(Me.uxDimensionPropertiesGroup)
        Me.Controls.Add(Me.uxManuallyAddArrow)
        Me.Controls.Add(Me.uxApply)
        Me.Controls.Add(Me.uxReset)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "DimensionArrowsForm"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Dimension Arrows"
        Me.TopMost = True
        Me.uxDimensionPropertiesGroup.ResumeLayout(False)
        Me.uxDimensionPropertiesGroup.PerformLayout()
        CType(Me.uxErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents uxCurveLabel As System.Windows.Forms.Label
    Friend WithEvents uxLineLabel As System.Windows.Forms.Label
    Friend WithEvents uxSmoothLabel As System.Windows.Forms.Label
    Friend WithEvents uxReset As System.Windows.Forms.Button
    Friend WithEvents uxApply As System.Windows.Forms.Button
    Friend WithEvents uxRatioOfCurve As System.Windows.Forms.TextBox
    Friend WithEvents uxRatioOfLine As System.Windows.Forms.TextBox
    Friend WithEvents uxSmoothRatio As System.Windows.Forms.TextBox
    Friend WithEvents uxManuallyAddArrow As System.Windows.Forms.CheckBox
    Friend WithEvents uxDimensionPropertiesGroup As System.Windows.Forms.GroupBox
    Friend WithEvents uxErrorProvider As System.Windows.Forms.ErrorProvider
End Class
