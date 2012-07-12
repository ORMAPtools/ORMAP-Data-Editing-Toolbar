<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SpiralDockWindow
    Inherits System.Windows.Forms.UserControl

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
        Me.uxCreate = New System.Windows.Forms.Button()
        Me.uxHelp = New System.Windows.Forms.Button()
        Me.LabelTargetLayer = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.uxCurvetotheLeft = New System.Windows.Forms.RadioButton()
        Me.uxCurvetotheRight = New System.Windows.Forms.RadioButton()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.uxDeltaAngle = New System.Windows.Forms.TextBox()
        Me.uxArcLenghtValue = New System.Windows.Forms.TextBox()
        Me.uxByDeltaAngle = New System.Windows.Forms.RadioButton()
        Me.uxByArcLength = New System.Windows.Forms.RadioButton()
        Me.uxBeginRadiusValue = New System.Windows.Forms.TextBox()
        Me.uxEndRadiusValue = New System.Windows.Forms.TextBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.uxGetFromPoint = New System.Windows.Forms.Button()
        Me.uxFromPointYValue = New System.Windows.Forms.TextBox()
        Me.uxFromPointXValue = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.uxGetTangentPoint = New System.Windows.Forms.Button()
        Me.uxTangentPointYValue = New System.Windows.Forms.TextBox()
        Me.uxTangentPointXValue = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.uxTargetTemplate = New System.Windows.Forms.ComboBox()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.uxDensityValue = New System.Windows.Forms.TextBox()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.SuspendLayout()
        '
        'uxCreate
        '
        Me.uxCreate.Location = New System.Drawing.Point(4, 447)
        Me.uxCreate.Name = "uxCreate"
        Me.uxCreate.Size = New System.Drawing.Size(75, 23)
        Me.uxCreate.TabIndex = 0
        Me.uxCreate.Text = "Create"
        Me.uxCreate.UseVisualStyleBackColor = True
        '
        'uxHelp
        '
        Me.uxHelp.Location = New System.Drawing.Point(85, 447)
        Me.uxHelp.Name = "uxHelp"
        Me.uxHelp.Size = New System.Drawing.Size(75, 23)
        Me.uxHelp.TabIndex = 2
        Me.uxHelp.Text = "Help"
        Me.uxHelp.UseVisualStyleBackColor = True
        '
        'LabelTargetLayer
        '
        Me.LabelTargetLayer.AutoSize = True
        Me.LabelTargetLayer.Location = New System.Drawing.Point(4, 4)
        Me.LabelTargetLayer.Name = "LabelTargetLayer"
        Me.LabelTargetLayer.Size = New System.Drawing.Size(51, 13)
        Me.LabelTargetLayer.TabIndex = 3
        Me.LabelTargetLayer.Text = "Template"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.uxCurvetotheLeft)
        Me.GroupBox1.Controls.Add(Me.uxCurvetotheRight)
        Me.GroupBox1.Location = New System.Drawing.Point(7, 50)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(287, 45)
        Me.GroupBox1.TabIndex = 5
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Curve to the"
        '
        'uxCurvetotheLeft
        '
        Me.uxCurvetotheLeft.AutoSize = True
        Me.uxCurvetotheLeft.Location = New System.Drawing.Point(65, 19)
        Me.uxCurvetotheLeft.Name = "uxCurvetotheLeft"
        Me.uxCurvetotheLeft.Size = New System.Drawing.Size(43, 17)
        Me.uxCurvetotheLeft.TabIndex = 1
        Me.uxCurvetotheLeft.Text = "Left"
        Me.uxCurvetotheLeft.UseVisualStyleBackColor = True
        '
        'uxCurvetotheRight
        '
        Me.uxCurvetotheRight.AutoSize = True
        Me.uxCurvetotheRight.Checked = True
        Me.uxCurvetotheRight.Location = New System.Drawing.Point(7, 20)
        Me.uxCurvetotheRight.Name = "uxCurvetotheRight"
        Me.uxCurvetotheRight.Size = New System.Drawing.Size(50, 17)
        Me.uxCurvetotheRight.TabIndex = 0
        Me.uxCurvetotheRight.TabStop = True
        Me.uxCurvetotheRight.Text = "Right"
        Me.uxCurvetotheRight.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(21, 106)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(73, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Begin Radius:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(29, 130)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(65, 13)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "End Radius:"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.uxDeltaAngle)
        Me.GroupBox2.Controls.Add(Me.uxArcLenghtValue)
        Me.GroupBox2.Controls.Add(Me.uxByDeltaAngle)
        Me.GroupBox2.Controls.Add(Me.uxByArcLength)
        Me.GroupBox2.Location = New System.Drawing.Point(13, 155)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(280, 80)
        Me.GroupBox2.TabIndex = 8
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Construct Spiral using"
        '
        'uxDeltaAngle
        '
        Me.uxDeltaAngle.Location = New System.Drawing.Point(89, 41)
        Me.uxDeltaAngle.Name = "uxDeltaAngle"
        Me.uxDeltaAngle.Size = New System.Drawing.Size(179, 20)
        Me.uxDeltaAngle.TabIndex = 3
        '
        'uxArcLenghtValue
        '
        Me.uxArcLenghtValue.Location = New System.Drawing.Point(89, 19)
        Me.uxArcLenghtValue.Name = "uxArcLenghtValue"
        Me.uxArcLenghtValue.Size = New System.Drawing.Size(179, 20)
        Me.uxArcLenghtValue.TabIndex = 2
        '
        'uxByDeltaAngle
        '
        Me.uxByDeltaAngle.AutoSize = True
        Me.uxByDeltaAngle.Location = New System.Drawing.Point(7, 44)
        Me.uxByDeltaAngle.Name = "uxByDeltaAngle"
        Me.uxByDeltaAngle.Size = New System.Drawing.Size(80, 17)
        Me.uxByDeltaAngle.TabIndex = 1
        Me.uxByDeltaAngle.Text = "Delta Angle"
        Me.uxByDeltaAngle.UseVisualStyleBackColor = True
        '
        'uxByArcLength
        '
        Me.uxByArcLength.AutoSize = True
        Me.uxByArcLength.Checked = True
        Me.uxByArcLength.Location = New System.Drawing.Point(7, 20)
        Me.uxByArcLength.Name = "uxByArcLength"
        Me.uxByArcLength.Size = New System.Drawing.Size(76, 17)
        Me.uxByArcLength.TabIndex = 0
        Me.uxByArcLength.TabStop = True
        Me.uxByArcLength.Text = "Arc length:"
        Me.uxByArcLength.UseVisualStyleBackColor = True
        '
        'uxBeginRadiusValue
        '
        Me.uxBeginRadiusValue.Location = New System.Drawing.Point(98, 102)
        Me.uxBeginRadiusValue.Name = "uxBeginRadiusValue"
        Me.uxBeginRadiusValue.Size = New System.Drawing.Size(183, 20)
        Me.uxBeginRadiusValue.TabIndex = 9
        Me.uxBeginRadiusValue.Text = "INFINITY"
        '
        'uxEndRadiusValue
        '
        Me.uxEndRadiusValue.Location = New System.Drawing.Point(98, 127)
        Me.uxEndRadiusValue.Name = "uxEndRadiusValue"
        Me.uxEndRadiusValue.Size = New System.Drawing.Size(183, 20)
        Me.uxEndRadiusValue.TabIndex = 10
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.uxGetFromPoint)
        Me.GroupBox3.Controls.Add(Me.uxFromPointYValue)
        Me.GroupBox3.Controls.Add(Me.uxFromPointXValue)
        Me.GroupBox3.Controls.Add(Me.Label5)
        Me.GroupBox3.Controls.Add(Me.Label4)
        Me.GroupBox3.Location = New System.Drawing.Point(14, 242)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(279, 69)
        Me.GroupBox3.TabIndex = 11
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "From Point"
        '
        'uxGetFromPoint
        '
        Me.uxGetFromPoint.Image = Global.OrmapTaxlotEditing.My.Resources.Resources.CenterOnTarget32
        Me.uxGetFromPoint.Location = New System.Drawing.Point(215, 24)
        Me.uxGetFromPoint.Name = "uxGetFromPoint"
        Me.uxGetFromPoint.Size = New System.Drawing.Size(36, 31)
        Me.uxGetFromPoint.TabIndex = 4
        Me.uxGetFromPoint.UseVisualStyleBackColor = True
        '
        'uxFromPointYValue
        '
        Me.uxFromPointYValue.Location = New System.Drawing.Point(34, 41)
        Me.uxFromPointYValue.Name = "uxFromPointYValue"
        Me.uxFromPointYValue.Size = New System.Drawing.Size(157, 20)
        Me.uxFromPointYValue.TabIndex = 3
        '
        'uxFromPointXValue
        '
        Me.uxFromPointXValue.Location = New System.Drawing.Point(34, 20)
        Me.uxFromPointXValue.Name = "uxFromPointXValue"
        Me.uxFromPointXValue.Size = New System.Drawing.Size(157, 20)
        Me.uxFromPointXValue.TabIndex = 2
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(10, 44)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(17, 13)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "Y:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(10, 20)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(17, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "X:"
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.uxGetTangentPoint)
        Me.GroupBox4.Controls.Add(Me.uxTangentPointYValue)
        Me.GroupBox4.Controls.Add(Me.uxTangentPointXValue)
        Me.GroupBox4.Controls.Add(Me.Label6)
        Me.GroupBox4.Controls.Add(Me.Label7)
        Me.GroupBox4.Location = New System.Drawing.Point(13, 317)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(279, 69)
        Me.GroupBox4.TabIndex = 12
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Tangent Point"
        '
        'uxGetTangentPoint
        '
        Me.uxGetTangentPoint.Image = Global.OrmapTaxlotEditing.My.Resources.Resources.CenterOnTarget32
        Me.uxGetTangentPoint.Location = New System.Drawing.Point(216, 22)
        Me.uxGetTangentPoint.Name = "uxGetTangentPoint"
        Me.uxGetTangentPoint.Size = New System.Drawing.Size(34, 33)
        Me.uxGetTangentPoint.TabIndex = 5
        Me.uxGetTangentPoint.UseVisualStyleBackColor = True
        '
        'uxTangentPointYValue
        '
        Me.uxTangentPointYValue.Location = New System.Drawing.Point(34, 41)
        Me.uxTangentPointYValue.Name = "uxTangentPointYValue"
        Me.uxTangentPointYValue.Size = New System.Drawing.Size(157, 20)
        Me.uxTangentPointYValue.TabIndex = 3
        '
        'uxTangentPointXValue
        '
        Me.uxTangentPointXValue.Location = New System.Drawing.Point(34, 20)
        Me.uxTangentPointXValue.Name = "uxTangentPointXValue"
        Me.uxTangentPointXValue.Size = New System.Drawing.Size(157, 20)
        Me.uxTangentPointXValue.TabIndex = 2
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(10, 44)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(17, 13)
        Me.Label6.TabIndex = 1
        Me.Label6.Text = "Y:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(10, 20)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(17, 13)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "X:"
        '
        'uxTargetTemplate
        '
        Me.uxTargetTemplate.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.uxTargetTemplate.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Me.uxTargetTemplate.FormattingEnabled = True
        Me.uxTargetTemplate.Location = New System.Drawing.Point(3, 476)
        Me.uxTargetTemplate.Name = "uxTargetTemplate"
        Me.uxTargetTemplate.Size = New System.Drawing.Size(286, 21)
        Me.uxTargetTemplate.TabIndex = 13
        Me.uxTargetTemplate.Visible = False
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.uxDensityValue)
        Me.GroupBox5.Location = New System.Drawing.Point(16, 395)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(275, 46)
        Me.GroupBox5.TabIndex = 14
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "CurveDensity"
        '
        'uxDensityValue
        '
        Me.uxDensityValue.Location = New System.Drawing.Point(10, 18)
        Me.uxDensityValue.Name = "uxDensityValue"
        Me.uxDensityValue.Size = New System.Drawing.Size(176, 20)
        Me.uxDensityValue.TabIndex = 0
        '
        'SpiralDockWindow
        '
        Me.Controls.Add(Me.GroupBox5)
        Me.Controls.Add(Me.uxTargetTemplate)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.uxEndRadiusValue)
        Me.Controls.Add(Me.uxBeginRadiusValue)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.LabelTargetLayer)
        Me.Controls.Add(Me.uxHelp)
        Me.Controls.Add(Me.uxCreate)
        Me.Name = "SpiralDockWindow"
        Me.Size = New System.Drawing.Size(300, 508)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents uxCreate As System.Windows.Forms.Button
    Friend WithEvents uxHelp As System.Windows.Forms.Button
    Friend WithEvents LabelTargetLayer As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents uxCurvetotheLeft As System.Windows.Forms.RadioButton
    Friend WithEvents uxCurvetotheRight As System.Windows.Forms.RadioButton
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents uxByDeltaAngle As System.Windows.Forms.RadioButton
    Friend WithEvents uxByArcLength As System.Windows.Forms.RadioButton
    Friend WithEvents uxBeginRadiusValue As System.Windows.Forms.TextBox
    Friend WithEvents uxEndRadiusValue As System.Windows.Forms.TextBox
    Friend WithEvents uxDeltaAngle As System.Windows.Forms.TextBox
    Friend WithEvents uxArcLenghtValue As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents uxFromPointYValue As System.Windows.Forms.TextBox
    Friend WithEvents uxFromPointXValue As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents uxTangentPointYValue As System.Windows.Forms.TextBox
    Friend WithEvents uxTangentPointXValue As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents uxGetFromPoint As System.Windows.Forms.Button
    Friend WithEvents uxGetTangentPoint As System.Windows.Forms.Button
    Friend WithEvents uxTargetTemplate As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents uxDensityValue As System.Windows.Forms.TextBox

End Class
