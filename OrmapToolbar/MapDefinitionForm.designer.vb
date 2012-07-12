<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MapDefinitionForm
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
        Me.EnterMapNumberLabel = New System.Windows.Forms.Label
        Me.uxMapNumber = New System.Windows.Forms.TextBox
        Me.uxSetMapDefinitionQuery = New System.Windows.Forms.Button
        Me.uxCancelSetDefinitionQuery = New System.Windows.Forms.Button
        Me.uxHelpDefinitionQuery = New System.Windows.Forms.Button
        Me.uxMapNumberOption = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.uxMapScaleOption = New System.Windows.Forms.ComboBox
        Me.uxMapScale = New System.Windows.Forms.TextBox
        Me.WarningLabel = New System.Windows.Forms.Label
        Me.uxUseMapNumber = New System.Windows.Forms.CheckBox
        Me.uxUseMapScale = New System.Windows.Forms.CheckBox
        Me.uxAnd = New System.Windows.Forms.RadioButton
        Me.uxOr = New System.Windows.Forms.RadioButton
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'EnterMapNumberLabel
        '
        Me.EnterMapNumberLabel.AutoSize = True
        Me.EnterMapNumberLabel.Location = New System.Drawing.Point(42, 34)
        Me.EnterMapNumberLabel.Name = "EnterMapNumberLabel"
        Me.EnterMapNumberLabel.Size = New System.Drawing.Size(185, 13)
        Me.EnterMapNumberLabel.TabIndex = 0
        Me.EnterMapNumberLabel.Text = "Display features where MapNumber is"
        '
        'uxMapNumber
        '
        Me.uxMapNumber.AllowDrop = True
        Me.uxMapNumber.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.uxMapNumber.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Me.uxMapNumber.Location = New System.Drawing.Point(295, 30)
        Me.uxMapNumber.Name = "uxMapNumber"
        Me.uxMapNumber.Size = New System.Drawing.Size(110, 20)
        Me.uxMapNumber.TabIndex = 0
        '
        'uxSetMapDefinitionQuery
        '
        Me.uxSetMapDefinitionQuery.Location = New System.Drawing.Point(222, 180)
        Me.uxSetMapDefinitionQuery.Name = "uxSetMapDefinitionQuery"
        Me.uxSetMapDefinitionQuery.Size = New System.Drawing.Size(75, 23)
        Me.uxSetMapDefinitionQuery.TabIndex = 1
        Me.uxSetMapDefinitionQuery.Text = "Set Query"
        Me.uxSetMapDefinitionQuery.UseVisualStyleBackColor = True
        '
        'uxCancelSetDefinitionQuery
        '
        Me.uxCancelSetDefinitionQuery.Location = New System.Drawing.Point(303, 180)
        Me.uxCancelSetDefinitionQuery.Name = "uxCancelSetDefinitionQuery"
        Me.uxCancelSetDefinitionQuery.Size = New System.Drawing.Size(75, 23)
        Me.uxCancelSetDefinitionQuery.TabIndex = 3
        Me.uxCancelSetDefinitionQuery.Text = "Cancel"
        Me.uxCancelSetDefinitionQuery.UseVisualStyleBackColor = True
        '
        'uxHelpDefinitionQuery
        '
        Me.uxHelpDefinitionQuery.Location = New System.Drawing.Point(384, 180)
        Me.uxHelpDefinitionQuery.Name = "uxHelpDefinitionQuery"
        Me.uxHelpDefinitionQuery.Size = New System.Drawing.Size(52, 23)
        Me.uxHelpDefinitionQuery.TabIndex = 4
        Me.uxHelpDefinitionQuery.Text = "Help"
        Me.uxHelpDefinitionQuery.UseVisualStyleBackColor = True
        '
        'uxMapNumberOption
        '
        Me.uxMapNumberOption.AllowDrop = True
        Me.uxMapNumberOption.FormattingEnabled = True
        Me.uxMapNumberOption.Items.AddRange(New Object() {"  =", " <>"})
        Me.uxMapNumberOption.Location = New System.Drawing.Point(245, 30)
        Me.uxMapNumberOption.Name = "uxMapNumberOption"
        Me.uxMapNumberOption.Size = New System.Drawing.Size(44, 21)
        Me.uxMapNumberOption.TabIndex = 5
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(42, 34)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(175, 13)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Display features where MapScale is"
        '
        'uxMapScaleOption
        '
        Me.uxMapScaleOption.FormattingEnabled = True
        Me.uxMapScaleOption.Items.AddRange(New Object() {"  =", " <>"})
        Me.uxMapScaleOption.Location = New System.Drawing.Point(246, 30)
        Me.uxMapScaleOption.Name = "uxMapScaleOption"
        Me.uxMapScaleOption.Size = New System.Drawing.Size(44, 21)
        Me.uxMapScaleOption.TabIndex = 7
        '
        'uxMapScale
        '
        Me.uxMapScale.AllowDrop = True
        Me.uxMapScale.AutoCompleteCustomSource.AddRange(New String() {"10", "20", "30", "40", "50", "60", "100", "200", "400", "800", "2000"})
        Me.uxMapScale.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.uxMapScale.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Me.uxMapScale.Location = New System.Drawing.Point(296, 30)
        Me.uxMapScale.Name = "uxMapScale"
        Me.uxMapScale.Size = New System.Drawing.Size(109, 20)
        Me.uxMapScale.TabIndex = 8
        '
        'WarningLabel
        '
        Me.WarningLabel.ForeColor = System.Drawing.Color.Red
        Me.WarningLabel.Location = New System.Drawing.Point(4, 160)
        Me.WarningLabel.Name = "WarningLabel"
        Me.WarningLabel.Size = New System.Drawing.Size(432, 17)
        Me.WarningLabel.TabIndex = 9
        Me.WarningLabel.Text = "Warning: This tool will clear out any exisiting definition queries in participait" & _
            "ing layers."
        '
        'uxUseMapNumber
        '
        Me.uxUseMapNumber.AutoSize = True
        Me.uxUseMapNumber.Checked = True
        Me.uxUseMapNumber.CheckState = System.Windows.Forms.CheckState.Checked
        Me.uxUseMapNumber.Location = New System.Drawing.Point(10, 14)
        Me.uxUseMapNumber.Name = "uxUseMapNumber"
        Me.uxUseMapNumber.Size = New System.Drawing.Size(206, 17)
        Me.uxUseMapNumber.TabIndex = 12
        Me.uxUseMapNumber.Text = "Use MapNumber in Definition Queries."
        Me.uxUseMapNumber.UseVisualStyleBackColor = True
        '
        'uxUseMapScale
        '
        Me.uxUseMapScale.AutoSize = True
        Me.uxUseMapScale.Checked = True
        Me.uxUseMapScale.CheckState = System.Windows.Forms.CheckState.Checked
        Me.uxUseMapScale.Location = New System.Drawing.Point(10, 14)
        Me.uxUseMapScale.Name = "uxUseMapScale"
        Me.uxUseMapScale.Size = New System.Drawing.Size(196, 17)
        Me.uxUseMapScale.TabIndex = 13
        Me.uxUseMapScale.Text = "Use MapScale in Definition Queries."
        Me.uxUseMapScale.UseVisualStyleBackColor = True
        '
        'uxAnd
        '
        Me.uxAnd.AutoSize = True
        Me.uxAnd.Checked = True
        Me.uxAnd.Location = New System.Drawing.Point(253, 73)
        Me.uxAnd.Name = "uxAnd"
        Me.uxAnd.Size = New System.Drawing.Size(44, 17)
        Me.uxAnd.TabIndex = 14
        Me.uxAnd.TabStop = True
        Me.uxAnd.Text = "And"
        Me.uxAnd.UseVisualStyleBackColor = True
        '
        'uxOr
        '
        Me.uxOr.AutoSize = True
        Me.uxOr.Location = New System.Drawing.Point(302, 73)
        Me.uxOr.Name = "uxOr"
        Me.uxOr.Size = New System.Drawing.Size(36, 17)
        Me.uxOr.TabIndex = 15
        Me.uxOr.Text = "Or"
        Me.uxOr.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.EnterMapNumberLabel)
        Me.GroupBox1.Controls.Add(Me.uxMapNumber)
        Me.GroupBox1.Controls.Add(Me.uxMapNumberOption)
        Me.GroupBox1.Controls.Add(Me.uxUseMapNumber)
        Me.GroupBox1.Location = New System.Drawing.Point(7, 3)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(429, 66)
        Me.GroupBox1.TabIndex = 17
        Me.GroupBox1.TabStop = False
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.uxMapScaleOption)
        Me.GroupBox2.Controls.Add(Me.uxMapScale)
        Me.GroupBox2.Controls.Add(Me.uxUseMapScale)
        Me.GroupBox2.Location = New System.Drawing.Point(7, 91)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(429, 66)
        Me.GroupBox2.TabIndex = 18
        Me.GroupBox2.TabStop = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(162, 75)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(72, 13)
        Me.Label2.TabIndex = 19
        Me.Label2.Text = "Query Option:"
        '
        'MapDefinitionForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(450, 214)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.uxOr)
        Me.Controls.Add(Me.uxAnd)
        Me.Controls.Add(Me.WarningLabel)
        Me.Controls.Add(Me.uxHelpDefinitionQuery)
        Me.Controls.Add(Me.uxCancelSetDefinitionQuery)
        Me.Controls.Add(Me.uxSetMapDefinitionQuery)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "MapDefinitionForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Definition Query"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents EnterMapNumberLabel As System.Windows.Forms.Label
    Friend WithEvents uxMapNumber As System.Windows.Forms.TextBox
    Friend WithEvents uxSetMapDefinitionQuery As System.Windows.Forms.Button
    Friend WithEvents uxCancelSetDefinitionQuery As System.Windows.Forms.Button
    Friend WithEvents uxHelpDefinitionQuery As System.Windows.Forms.Button
    Friend WithEvents uxMapNumberOption As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents uxMapScaleOption As System.Windows.Forms.ComboBox
    Friend WithEvents uxMapScale As System.Windows.Forms.TextBox
    Friend WithEvents WarningLabel As System.Windows.Forms.Label
    Friend WithEvents uxUseMapNumber As System.Windows.Forms.CheckBox
    Friend WithEvents uxUseMapScale As System.Windows.Forms.CheckBox
    Friend WithEvents uxAnd As System.Windows.Forms.RadioButton
    Friend WithEvents uxOr As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
End Class
