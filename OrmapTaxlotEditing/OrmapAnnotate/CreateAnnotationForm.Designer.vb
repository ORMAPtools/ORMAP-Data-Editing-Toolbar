<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CreateAnnotationForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Me.uxCreateAnno = New System.Windows.Forms.Button
        Me.uxOptionsCancel = New System.Windows.Forms.Button
        Me.uxBothSides = New System.Windows.Forms.RadioButton
        Me.uxBothAbove = New System.Windows.Forms.RadioButton
        Me.uxBothBelow = New System.Windows.Forms.RadioButton
        Me.uxPosition = New System.Windows.Forms.GroupBox
        Me.uxParallel = New System.Windows.Forms.RadioButton
        Me.uxHorizontal = New System.Windows.Forms.RadioButton
        Me.uxPerpendicular = New System.Windows.Forms.RadioButton
        Me.uxDirection = New System.Windows.Forms.RadioButton
        Me.uxDistance = New System.Windows.Forms.RadioButton
        Me.uxOrientation = New System.Windows.Forms.GroupBox
        Me.uxCurved = New System.Windows.Forms.CheckBox
        Me.uxTopValue = New System.Windows.Forms.GroupBox
        Me.uxLineWidth = New System.Windows.Forms.GroupBox
        Me.uxWideLine = New System.Windows.Forms.RadioButton
        Me.uxStandardLine = New System.Windows.Forms.RadioButton
        Me.uxOffsetAbove = New System.Windows.Forms.GroupBox
        Me.uxDoubleAbove = New System.Windows.Forms.RadioButton
        Me.uxStandardAbove = New System.Windows.Forms.RadioButton
        Me.uxOffsetBelow = New System.Windows.Forms.GroupBox
        Me.uxDoubleBelow = New System.Windows.Forms.RadioButton
        Me.uxStandardBelow = New System.Windows.Forms.RadioButton
        Me.uxReferenceScale = New System.Windows.Forms.TextBox
        Me.uxReferenceScaleLabel = New System.Windows.Forms.Label
        Me.uxHelp = New System.Windows.Forms.Button
        Me.uxProgressBar = New System.Windows.Forms.ProgressBar
        Me.uxPosition.SuspendLayout()
        Me.uxOrientation.SuspendLayout()
        Me.uxTopValue.SuspendLayout()
        Me.uxLineWidth.SuspendLayout()
        Me.uxOffsetAbove.SuspendLayout()
        Me.uxOffsetBelow.SuspendLayout()
        Me.SuspendLayout()
        '
        'uxCreateAnno
        '
        Me.uxCreateAnno.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.uxCreateAnno.Location = New System.Drawing.Point(190, 273)
        Me.uxCreateAnno.Name = "uxCreateAnno"
        Me.uxCreateAnno.Size = New System.Drawing.Size(93, 23)
        Me.uxCreateAnno.TabIndex = 0
        Me.uxCreateAnno.Text = "&Create Anno"
        Me.uxCreateAnno.UseVisualStyleBackColor = True
        '
        'uxOptionsCancel
        '
        Me.uxOptionsCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.uxOptionsCancel.Location = New System.Drawing.Point(8, 273)
        Me.uxOptionsCancel.Name = "uxOptionsCancel"
        Me.uxOptionsCancel.Size = New System.Drawing.Size(75, 23)
        Me.uxOptionsCancel.TabIndex = 1
        Me.uxOptionsCancel.Text = "&Cancel"
        Me.uxOptionsCancel.UseVisualStyleBackColor = True
        '
        'uxBothSides
        '
        Me.uxBothSides.AutoSize = True
        Me.uxBothSides.Checked = True
        Me.uxBothSides.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxBothSides.Location = New System.Drawing.Point(21, 24)
        Me.uxBothSides.Name = "uxBothSides"
        Me.uxBothSides.Size = New System.Drawing.Size(76, 17)
        Me.uxBothSides.TabIndex = 13
        Me.uxBothSides.TabStop = True
        Me.uxBothSides.Text = "Both Sides"
        Me.uxBothSides.UseVisualStyleBackColor = True
        '
        'uxBothAbove
        '
        Me.uxBothAbove.AutoSize = True
        Me.uxBothAbove.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxBothAbove.Location = New System.Drawing.Point(21, 48)
        Me.uxBothAbove.Name = "uxBothAbove"
        Me.uxBothAbove.Size = New System.Drawing.Size(81, 17)
        Me.uxBothAbove.TabIndex = 14
        Me.uxBothAbove.Text = "Both Above"
        Me.uxBothAbove.UseVisualStyleBackColor = True
        '
        'uxBothBelow
        '
        Me.uxBothBelow.AutoSize = True
        Me.uxBothBelow.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxBothBelow.Location = New System.Drawing.Point(21, 71)
        Me.uxBothBelow.Name = "uxBothBelow"
        Me.uxBothBelow.Size = New System.Drawing.Size(79, 17)
        Me.uxBothBelow.TabIndex = 15
        Me.uxBothBelow.Text = "Both Below"
        Me.uxBothBelow.UseVisualStyleBackColor = True
        '
        'uxPosition
        '
        Me.uxPosition.Controls.Add(Me.uxBothBelow)
        Me.uxPosition.Controls.Add(Me.uxBothAbove)
        Me.uxPosition.Controls.Add(Me.uxBothSides)
        Me.uxPosition.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxPosition.Location = New System.Drawing.Point(8, 12)
        Me.uxPosition.Name = "uxPosition"
        Me.uxPosition.Size = New System.Drawing.Size(121, 120)
        Me.uxPosition.TabIndex = 16
        Me.uxPosition.TabStop = False
        Me.uxPosition.Text = "Postion"
        '
        'uxParallel
        '
        Me.uxParallel.AutoSize = True
        Me.uxParallel.Checked = True
        Me.uxParallel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxParallel.Location = New System.Drawing.Point(24, 71)
        Me.uxParallel.Name = "uxParallel"
        Me.uxParallel.Size = New System.Drawing.Size(59, 17)
        Me.uxParallel.TabIndex = 18
        Me.uxParallel.TabStop = True
        Me.uxParallel.Text = "Parallel"
        Me.uxParallel.UseVisualStyleBackColor = True
        '
        'uxHorizontal
        '
        Me.uxHorizontal.AutoSize = True
        Me.uxHorizontal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxHorizontal.Location = New System.Drawing.Point(24, 48)
        Me.uxHorizontal.Name = "uxHorizontal"
        Me.uxHorizontal.Size = New System.Drawing.Size(72, 17)
        Me.uxHorizontal.TabIndex = 19
        Me.uxHorizontal.Text = "Horizontal"
        Me.uxHorizontal.UseVisualStyleBackColor = True
        '
        'uxPerpendicular
        '
        Me.uxPerpendicular.AutoSize = True
        Me.uxPerpendicular.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxPerpendicular.Location = New System.Drawing.Point(24, 24)
        Me.uxPerpendicular.Name = "uxPerpendicular"
        Me.uxPerpendicular.Size = New System.Drawing.Size(90, 17)
        Me.uxPerpendicular.TabIndex = 20
        Me.uxPerpendicular.Text = "Perpendicular"
        Me.uxPerpendicular.UseVisualStyleBackColor = True
        '
        'uxDirection
        '
        Me.uxDirection.AutoSize = True
        Me.uxDirection.Checked = True
        Me.uxDirection.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxDirection.Location = New System.Drawing.Point(28, 24)
        Me.uxDirection.Name = "uxDirection"
        Me.uxDirection.Size = New System.Drawing.Size(67, 17)
        Me.uxDirection.TabIndex = 21
        Me.uxDirection.TabStop = True
        Me.uxDirection.Text = "Direction"
        Me.uxDirection.UseVisualStyleBackColor = True
        '
        'uxDistance
        '
        Me.uxDistance.AutoSize = True
        Me.uxDistance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxDistance.Location = New System.Drawing.Point(28, 48)
        Me.uxDistance.Name = "uxDistance"
        Me.uxDistance.Size = New System.Drawing.Size(67, 17)
        Me.uxDistance.TabIndex = 22
        Me.uxDistance.Text = "Distance"
        Me.uxDistance.UseVisualStyleBackColor = True
        '
        'uxOrientation
        '
        Me.uxOrientation.Controls.Add(Me.uxCurved)
        Me.uxOrientation.Controls.Add(Me.uxParallel)
        Me.uxOrientation.Controls.Add(Me.uxPerpendicular)
        Me.uxOrientation.Controls.Add(Me.uxHorizontal)
        Me.uxOrientation.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxOrientation.Location = New System.Drawing.Point(136, 12)
        Me.uxOrientation.Name = "uxOrientation"
        Me.uxOrientation.Size = New System.Drawing.Size(121, 120)
        Me.uxOrientation.TabIndex = 17
        Me.uxOrientation.TabStop = False
        Me.uxOrientation.Text = "Orientation"
        '
        'uxCurved
        '
        Me.uxCurved.AutoSize = True
        Me.uxCurved.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxCurved.Location = New System.Drawing.Point(54, 94)
        Me.uxCurved.Name = "uxCurved"
        Me.uxCurved.Size = New System.Drawing.Size(60, 17)
        Me.uxCurved.TabIndex = 19
        Me.uxCurved.Text = "Curved"
        Me.uxCurved.UseVisualStyleBackColor = True
        '
        'uxTopValue
        '
        Me.uxTopValue.Controls.Add(Me.uxDirection)
        Me.uxTopValue.Controls.Add(Me.uxDistance)
        Me.uxTopValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxTopValue.Location = New System.Drawing.Point(263, 12)
        Me.uxTopValue.Name = "uxTopValue"
        Me.uxTopValue.Size = New System.Drawing.Size(121, 120)
        Me.uxTopValue.TabIndex = 18
        Me.uxTopValue.TabStop = False
        Me.uxTopValue.Text = "Top Value"
        '
        'uxLineWidth
        '
        Me.uxLineWidth.Controls.Add(Me.uxWideLine)
        Me.uxLineWidth.Controls.Add(Me.uxStandardLine)
        Me.uxLineWidth.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxLineWidth.Location = New System.Drawing.Point(263, 138)
        Me.uxLineWidth.Name = "uxLineWidth"
        Me.uxLineWidth.Size = New System.Drawing.Size(121, 74)
        Me.uxLineWidth.TabIndex = 23
        Me.uxLineWidth.TabStop = False
        Me.uxLineWidth.Text = "Line Width"
        '
        'uxWideLine
        '
        Me.uxWideLine.AutoSize = True
        Me.uxWideLine.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxWideLine.Location = New System.Drawing.Point(13, 44)
        Me.uxWideLine.Name = "uxWideLine"
        Me.uxWideLine.Size = New System.Drawing.Size(50, 17)
        Me.uxWideLine.TabIndex = 1
        Me.uxWideLine.Text = "Wide"
        Me.uxWideLine.UseVisualStyleBackColor = True
        '
        'uxStandardLine
        '
        Me.uxStandardLine.AutoSize = True
        Me.uxStandardLine.Checked = True
        Me.uxStandardLine.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxStandardLine.Location = New System.Drawing.Point(13, 20)
        Me.uxStandardLine.Name = "uxStandardLine"
        Me.uxStandardLine.Size = New System.Drawing.Size(68, 17)
        Me.uxStandardLine.TabIndex = 0
        Me.uxStandardLine.TabStop = True
        Me.uxStandardLine.Text = "Standard"
        Me.uxStandardLine.UseVisualStyleBackColor = True
        '
        'uxOffsetAbove
        '
        Me.uxOffsetAbove.Controls.Add(Me.uxDoubleAbove)
        Me.uxOffsetAbove.Controls.Add(Me.uxStandardAbove)
        Me.uxOffsetAbove.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxOffsetAbove.Location = New System.Drawing.Point(8, 138)
        Me.uxOffsetAbove.Name = "uxOffsetAbove"
        Me.uxOffsetAbove.Size = New System.Drawing.Size(121, 74)
        Me.uxOffsetAbove.TabIndex = 24
        Me.uxOffsetAbove.TabStop = False
        Me.uxOffsetAbove.Text = "Offset Above"
        '
        'uxDoubleAbove
        '
        Me.uxDoubleAbove.AutoSize = True
        Me.uxDoubleAbove.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxDoubleAbove.Location = New System.Drawing.Point(21, 44)
        Me.uxDoubleAbove.Name = "uxDoubleAbove"
        Me.uxDoubleAbove.Size = New System.Drawing.Size(59, 17)
        Me.uxDoubleAbove.TabIndex = 1
        Me.uxDoubleAbove.Text = "Double"
        Me.uxDoubleAbove.UseVisualStyleBackColor = True
        '
        'uxStandardAbove
        '
        Me.uxStandardAbove.AutoSize = True
        Me.uxStandardAbove.Checked = True
        Me.uxStandardAbove.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxStandardAbove.Location = New System.Drawing.Point(21, 20)
        Me.uxStandardAbove.Name = "uxStandardAbove"
        Me.uxStandardAbove.Size = New System.Drawing.Size(68, 17)
        Me.uxStandardAbove.TabIndex = 0
        Me.uxStandardAbove.TabStop = True
        Me.uxStandardAbove.Text = "Standard"
        Me.uxStandardAbove.UseVisualStyleBackColor = True
        '
        'uxOffsetBelow
        '
        Me.uxOffsetBelow.Controls.Add(Me.uxDoubleBelow)
        Me.uxOffsetBelow.Controls.Add(Me.uxStandardBelow)
        Me.uxOffsetBelow.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxOffsetBelow.Location = New System.Drawing.Point(136, 138)
        Me.uxOffsetBelow.Name = "uxOffsetBelow"
        Me.uxOffsetBelow.Size = New System.Drawing.Size(121, 74)
        Me.uxOffsetBelow.TabIndex = 24
        Me.uxOffsetBelow.TabStop = False
        Me.uxOffsetBelow.Text = "Offset Below"
        '
        'uxDoubleBelow
        '
        Me.uxDoubleBelow.AutoSize = True
        Me.uxDoubleBelow.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxDoubleBelow.Location = New System.Drawing.Point(24, 44)
        Me.uxDoubleBelow.Name = "uxDoubleBelow"
        Me.uxDoubleBelow.Size = New System.Drawing.Size(59, 17)
        Me.uxDoubleBelow.TabIndex = 1
        Me.uxDoubleBelow.Text = "Double"
        Me.uxDoubleBelow.UseVisualStyleBackColor = True
        '
        'uxStandardBelow
        '
        Me.uxStandardBelow.AutoSize = True
        Me.uxStandardBelow.Checked = True
        Me.uxStandardBelow.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxStandardBelow.Location = New System.Drawing.Point(24, 20)
        Me.uxStandardBelow.Name = "uxStandardBelow"
        Me.uxStandardBelow.Size = New System.Drawing.Size(68, 17)
        Me.uxStandardBelow.TabIndex = 0
        Me.uxStandardBelow.TabStop = True
        Me.uxStandardBelow.Text = "Standard"
        Me.uxStandardBelow.UseVisualStyleBackColor = True
        '
        'uxReferenceScale
        '
        Me.uxReferenceScale.Location = New System.Drawing.Point(8, 218)
        Me.uxReferenceScale.Name = "uxReferenceScale"
        Me.uxReferenceScale.Size = New System.Drawing.Size(55, 20)
        Me.uxReferenceScale.TabIndex = 25
        Me.uxReferenceScale.Text = "1200"
        '
        'uxReferenceScaleLabel
        '
        Me.uxReferenceScaleLabel.AutoSize = True
        Me.uxReferenceScaleLabel.Location = New System.Drawing.Point(65, 222)
        Me.uxReferenceScaleLabel.Name = "uxReferenceScaleLabel"
        Me.uxReferenceScaleLabel.Size = New System.Drawing.Size(87, 13)
        Me.uxReferenceScaleLabel.TabIndex = 26
        Me.uxReferenceScaleLabel.Text = "Reference Scale"
        '
        'uxHelp
        '
        Me.uxHelp.Location = New System.Drawing.Point(8, 244)
        Me.uxHelp.Name = "uxHelp"
        Me.uxHelp.Size = New System.Drawing.Size(75, 23)
        Me.uxHelp.TabIndex = 27
        Me.uxHelp.Text = "Help"
        Me.uxHelp.UseVisualStyleBackColor = True
        '
        'uxProgressBar
        '
        Me.uxProgressBar.Location = New System.Drawing.Point(190, 244)
        Me.uxProgressBar.Name = "uxProgressBar"
        Me.uxProgressBar.Size = New System.Drawing.Size(194, 15)
        Me.uxProgressBar.TabIndex = 28
        '
        'CreateAnnotationForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.uxOptionsCancel
        Me.ClientSize = New System.Drawing.Size(391, 306)
        Me.Controls.Add(Me.uxProgressBar)
        Me.Controls.Add(Me.uxHelp)
        Me.Controls.Add(Me.uxReferenceScaleLabel)
        Me.Controls.Add(Me.uxReferenceScale)
        Me.Controls.Add(Me.uxOffsetBelow)
        Me.Controls.Add(Me.uxOffsetAbove)
        Me.Controls.Add(Me.uxLineWidth)
        Me.Controls.Add(Me.uxOptionsCancel)
        Me.Controls.Add(Me.uxCreateAnno)
        Me.Controls.Add(Me.uxPosition)
        Me.Controls.Add(Me.uxOrientation)
        Me.Controls.Add(Me.uxTopValue)
        Me.Name = "CreateAnnotationForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Annotation Options"
        Me.TopMost = True
        Me.uxPosition.ResumeLayout(False)
        Me.uxPosition.PerformLayout()
        Me.uxOrientation.ResumeLayout(False)
        Me.uxOrientation.PerformLayout()
        Me.uxTopValue.ResumeLayout(False)
        Me.uxTopValue.PerformLayout()
        Me.uxLineWidth.ResumeLayout(False)
        Me.uxLineWidth.PerformLayout()
        Me.uxOffsetAbove.ResumeLayout(False)
        Me.uxOffsetAbove.PerformLayout()
        Me.uxOffsetBelow.ResumeLayout(False)
        Me.uxOffsetBelow.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents uxCreateAnno As System.Windows.Forms.Button
    Friend WithEvents uxOptionsCancel As System.Windows.Forms.Button
    Friend WithEvents uxBothSides As System.Windows.Forms.RadioButton
    Friend WithEvents uxBothAbove As System.Windows.Forms.RadioButton
    Friend WithEvents uxBothBelow As System.Windows.Forms.RadioButton
    Friend WithEvents uxPosition As System.Windows.Forms.GroupBox
    Friend WithEvents uxParallel As System.Windows.Forms.RadioButton
    Friend WithEvents uxHorizontal As System.Windows.Forms.RadioButton
    Friend WithEvents uxPerpendicular As System.Windows.Forms.RadioButton
    Friend WithEvents uxDirection As System.Windows.Forms.RadioButton
    Friend WithEvents uxDistance As System.Windows.Forms.RadioButton
    Friend WithEvents uxOrientation As System.Windows.Forms.GroupBox
    Friend WithEvents uxTopValue As System.Windows.Forms.GroupBox
    Friend WithEvents uxLineWidth As System.Windows.Forms.GroupBox
    Friend WithEvents uxWideLine As System.Windows.Forms.RadioButton
    Friend WithEvents uxStandardLine As System.Windows.Forms.RadioButton
    Friend WithEvents uxOffsetAbove As System.Windows.Forms.GroupBox
    Friend WithEvents uxDoubleAbove As System.Windows.Forms.RadioButton
    Friend WithEvents uxStandardAbove As System.Windows.Forms.RadioButton
    Friend WithEvents uxOffsetBelow As System.Windows.Forms.GroupBox
    Friend WithEvents uxDoubleBelow As System.Windows.Forms.RadioButton
    Friend WithEvents uxStandardBelow As System.Windows.Forms.RadioButton
    Friend WithEvents uxReferenceScale As System.Windows.Forms.TextBox
    Friend WithEvents uxReferenceScaleLabel As System.Windows.Forms.Label
    Friend WithEvents uxCurved As System.Windows.Forms.CheckBox
    Friend WithEvents uxHelp As System.Windows.Forms.Button
    Friend WithEvents uxProgressBar As System.Windows.Forms.ProgressBar

End Class
