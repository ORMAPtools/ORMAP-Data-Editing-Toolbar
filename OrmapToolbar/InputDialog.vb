Imports System.Windows.Forms

Public Class InputDialog

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        If Me.uxTextBox.Text = String.Empty Then
            MessageBox.Show(_message, "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            uxTextBox.Focus()
        Else
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End If

    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Public ReadOnly Property Value() As String
        Get
            Return Me.uxTextBox.Text()
        End Get
    End Property

    Private _message As String
    Public WriteOnly Property Message() As String
        Set(ByVal value As String)
            _message = value
        End Set
    End Property

End Class
