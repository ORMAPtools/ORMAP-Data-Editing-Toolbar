<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SortCancelledNumbersForm
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
        Me.uxCancelledNumbers = New System.Windows.Forms.ListBox()
        Me.uxTop = New System.Windows.Forms.Button()
        Me.uxUp = New System.Windows.Forms.Button()
        Me.uxDown = New System.Windows.Forms.Button()
        Me.uxBottom = New System.Windows.Forms.Button()
        Me.uxCancel = New System.Windows.Forms.Button()
        Me.uxOK = New System.Windows.Forms.Button()
        Me.uxAdd = New System.Windows.Forms.Button()
        Me.uxDelete = New System.Windows.Forms.Button()
        Me.uxHelp = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.uxFind = New System.Windows.Forms.Button()
        Me.uxMapIndex = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'uxCancelledNumbers
        '
        Me.uxCancelledNumbers.FormattingEnabled = True
        Me.uxCancelledNumbers.Location = New System.Drawing.Point(7, 62)
        Me.uxCancelledNumbers.Name = "uxCancelledNumbers"
        Me.uxCancelledNumbers.Size = New System.Drawing.Size(225, 381)
        Me.uxCancelledNumbers.TabIndex = 0
        '
        'uxTop
        '
        Me.uxTop.Location = New System.Drawing.Point(238, 151)
        Me.uxTop.Name = "uxTop"
        Me.uxTop.Size = New System.Drawing.Size(52, 23)
        Me.uxTop.TabIndex = 1
        Me.uxTop.Text = "Top"
        Me.uxTop.UseVisualStyleBackColor = True
        '
        'uxUp
        '
        Me.uxUp.Location = New System.Drawing.Point(238, 180)
        Me.uxUp.Name = "uxUp"
        Me.uxUp.Size = New System.Drawing.Size(52, 23)
        Me.uxUp.TabIndex = 2
        Me.uxUp.Text = "Up"
        Me.uxUp.UseVisualStyleBackColor = True
        '
        'uxDown
        '
        Me.uxDown.Location = New System.Drawing.Point(238, 222)
        Me.uxDown.Name = "uxDown"
        Me.uxDown.Size = New System.Drawing.Size(52, 23)
        Me.uxDown.TabIndex = 3
        Me.uxDown.Text = "Down"
        Me.uxDown.UseVisualStyleBackColor = True
        '
        'uxBottom
        '
        Me.uxBottom.Location = New System.Drawing.Point(238, 251)
        Me.uxBottom.Name = "uxBottom"
        Me.uxBottom.Size = New System.Drawing.Size(52, 23)
        Me.uxBottom.TabIndex = 4
        Me.uxBottom.Text = "Bottom"
        Me.uxBottom.UseVisualStyleBackColor = True
        '
        'uxCancel
        '
        Me.uxCancel.Location = New System.Drawing.Point(157, 448)
        Me.uxCancel.Name = "uxCancel"
        Me.uxCancel.Size = New System.Drawing.Size(75, 23)
        Me.uxCancel.TabIndex = 5
        Me.uxCancel.Text = "Cancel"
        Me.uxCancel.UseVisualStyleBackColor = True
        '
        'uxOK
        '
        Me.uxOK.Location = New System.Drawing.Point(76, 448)
        Me.uxOK.Name = "uxOK"
        Me.uxOK.Size = New System.Drawing.Size(75, 23)
        Me.uxOK.TabIndex = 6
        Me.uxOK.Text = "OK"
        Me.uxOK.UseVisualStyleBackColor = True
        '
        'uxAdd
        '
        Me.uxAdd.Location = New System.Drawing.Point(238, 348)
        Me.uxAdd.Name = "uxAdd"
        Me.uxAdd.Size = New System.Drawing.Size(52, 23)
        Me.uxAdd.TabIndex = 7
        Me.uxAdd.Text = "Add"
        Me.uxAdd.UseVisualStyleBackColor = True
        '
        'uxDelete
        '
        Me.uxDelete.Location = New System.Drawing.Point(238, 377)
        Me.uxDelete.Name = "uxDelete"
        Me.uxDelete.Size = New System.Drawing.Size(53, 23)
        Me.uxDelete.TabIndex = 8
        Me.uxDelete.Text = "Delete"
        Me.uxDelete.UseVisualStyleBackColor = True
        '
        'uxHelp
        '
        Me.uxHelp.Location = New System.Drawing.Point(238, 448)
        Me.uxHelp.Name = "uxHelp"
        Me.uxHelp.Size = New System.Drawing.Size(52, 23)
        Me.uxHelp.TabIndex = 13
        Me.uxHelp.Text = "Help"
        Me.uxHelp.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.uxFind)
        Me.GroupBox1.Controls.Add(Me.uxMapIndex)
        Me.GroupBox1.Location = New System.Drawing.Point(7, 7)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(283, 49)
        Me.GroupBox1.TabIndex = 14
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Find Map Index:"
        '
        'uxFind
        '
        Me.uxFind.Location = New System.Drawing.Point(218, 17)
        Me.uxFind.Name = "uxFind"
        Me.uxFind.Size = New System.Drawing.Size(52, 23)
        Me.uxFind.TabIndex = 14
        Me.uxFind.Text = "Find"
        Me.uxFind.UseVisualStyleBackColor = True
        '
        'uxMapIndex
        '
        Me.uxMapIndex.AllowDrop = True
        Me.uxMapIndex.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.uxMapIndex.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Me.uxMapIndex.Location = New System.Drawing.Point(100, 19)
        Me.uxMapIndex.Name = "uxMapIndex"
        Me.uxMapIndex.Size = New System.Drawing.Size(112, 20)
        Me.uxMapIndex.TabIndex = 12
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(88, 13)
        Me.Label1.TabIndex = 15
        Me.Label1.Text = "Enter Map Index:"
        '
        'SortCancelledNumbersForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(298, 479)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.uxHelp)
        Me.Controls.Add(Me.uxDelete)
        Me.Controls.Add(Me.uxAdd)
        Me.Controls.Add(Me.uxOK)
        Me.Controls.Add(Me.uxCancel)
        Me.Controls.Add(Me.uxBottom)
        Me.Controls.Add(Me.uxDown)
        Me.Controls.Add(Me.uxUp)
        Me.Controls.Add(Me.uxTop)
        Me.Controls.Add(Me.uxCancelledNumbers)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "SortCancelledNumbersForm"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Sort Cancelled Numbers"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents uxCancelledNumbers As System.Windows.Forms.ListBox
    Friend WithEvents uxTop As System.Windows.Forms.Button
    Friend WithEvents uxUp As System.Windows.Forms.Button
    Friend WithEvents uxDown As System.Windows.Forms.Button
    Friend WithEvents uxBottom As System.Windows.Forms.Button
    Friend WithEvents uxCancel As System.Windows.Forms.Button
    Friend WithEvents uxOK As System.Windows.Forms.Button
    Friend WithEvents uxAdd As System.Windows.Forms.Button
    Friend WithEvents uxDelete As System.Windows.Forms.Button
    Friend WithEvents uxHelp As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents uxFind As System.Windows.Forms.Button
    Friend WithEvents uxMapIndex As System.Windows.Forms.TextBox
End Class
