#Region "Copyright 2008-2011 ORMAP Tech Group"

' File:  Utilities.vb
'
' Original Author:  OPET.NET Migration Team (Shad Campbell, James Moore, 
'                   Nick Seigal)
'
' Date Created:  20080221
'
' Copyright Holder:  ORMAP Tech Group  
' Contact Info:  ORMAP Tech Group may be reached at 
' ORMAP_ESRI_Programmers@listsmart.osl.state.or.us
'
' This file is part of the ORMAP Taxlot Editing Toolbar.
'
' ORMAP Taxlot Editing Toolbar is free software; you can redistribute it and/or
' modify it under the terms of the Lesser GNU General Public License as 
' published by the Free Software Foundation; either version 3 of the License, 
' or (at your option) any later version.
'
' This program is distributed in the hope that it will be useful, but WITHOUT 
' ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or 
' FITNESS FOR A PARTICULAR PURPOSE.  See the Lesser GNU General Public License 
' located in the COPYING.LESSER.txt file for more details.
'
' You should have received a copy of the Lesser GNU General Public License 
' along with the ORMAP Taxlot Editing Toolbar; if not, write to the Free 
' Software Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 
' 02110-1301 USA.

#End Region

#Region "Subversion Keyword Expansion"
'Tag for this file: $Name$
'SCC revision number: $Revision: 406 $
'Date of Last Change: $Date: 2009-11-30 22:49:20 -0800 (Mon, 30 Nov 2009) $
#End Region

#Region "Imported Namespaces"
Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic
Imports System.Environment
Imports System.Text
Imports System.Net.Mail
Imports System.Diagnostics.FileVersionInfo
Imports System.Reflection.Assembly
#End Region

#Region "Class Declaration"
''' <summary>
''' Utilities class (singleton).
''' </summary>
''' <remarks>
''' <para>Commonly used general procedures and functions.</para>
''' <para><seealso cref="SpatialUtilities"/></para>
''' <para><seealso cref="StringUtilities"/></para>
''' </remarks>
Public NotInheritable Class Utilities

#Region "Built-In Class Members (Constructors, Etc.)"

#Region "Constructors"

    ''' <summary>
    ''' Private empty constructor to prevent instantiation.
    ''' </summary>
    ''' <remarks>This class follows the singleton pattern and thus has a 
    ''' private constructor and all shared members. Instances of types 
    ''' that define only shared members do not need to be created, so no
    ''' constructor should be needed. However, many compilers will 
    ''' automatically add a public default constructor if no constructor 
    ''' is specified. To prevent this an empty private constructor is 
    ''' added.</remarks>
    Private Sub New()
    End Sub

#End Region

#End Region

#Region "Custom Class Members"

#Region "Public Members"
    ''' <summary>
    ''' Index returned when field or other collection item not found (-1).
    ''' </summary>
    Public Const NotFoundIndex As Integer = -1

    ''' <summary>
    ''' Enumeration of ESRI mouse button constant values.
    ''' </summary>
    Friend Enum EsriMouseButtons
        Left = 1
        Right = 2
        Middle = 4
    End Enum

    ''' <summary>
    ''' Enumeration of ESRI layer type filters that can be applied to a IEnumLayer.
    ''' </summary>
    Friend Enum EsriLayerTypes
        AllLayerTypes
        DataLayer
        FeatureLayer
        GeoFeatureLayer
        GraphicsLayer
        FDOGraphicsLayer
        CoverageAnnotationLayer
        GroupLayer
    End Enum


    ''' <summary>
    ''' Stores the current computer user name.
    ''' </summary>
    ''' <value></value>
    ''' <returns>A username string.</returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property UserName() As String
        Get
            ' Note: ALL Since this a dll, My.User.InitializeWithWindowsUser()
            ' is called in OrmapExtension.Startup to set this value
            If TypeOf My.User.CurrentPrincipal Is  _
                    Security.Principal.WindowsPrincipal Then
                '[The application is using Windows authentication...]
                '[The name format is "DOMAIN\USERNAME"...]
                ' Parse out USERNAME from DOMAIN\USERNAME pair
                Dim parts() As String = Split(My.User.Name, "\")
                Dim name As String = parts(1)
                Return name
            Else
                ' The application is using custom authentication.
                Return My.User.Name
            End If
        End Get
    End Property

    ''' <summary>
    ''' Determine file existence
    ''' </summary>
    ''' <param name="path">A string that represents the file to check</param>
    ''' <returns>True or False</returns>
    ''' <remarks></remarks>
    Public Shared Function FileExists(ByVal path As String) As Boolean
        Try
            If path Is Nothing OrElse path.Length = 0 Then
                Throw New ArgumentNullException("path")
            End If
            Dim theFileInfo As New FileInfo(path)
            If theFileInfo.Exists Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            OrmapExtension.ProcessUnhandledException(ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Opens a document with its associated application.
    ''' </summary>
    ''' <param name="path">Fully qualified path to document (including file name).</param>
    ''' <remarks></remarks>
    Public Shared Sub StartDoc(ByVal path As String)
        Try
            System.Diagnostics.Process.Start(path)
        Catch fex As FileNotFoundException
            MessageBox.Show("File not Found", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Catch ex As Exception
            OrmapExtension.ProcessUnhandledException(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Send email messages via SMTP.
    ''' </summary>
    ''' <param name="inFrom">The sender.</param>
    ''' <param name="inTo">A string array for multiple recipients.</param>
    ''' <param name="inSubject">The subject.</param>
    ''' <param name="inBody">The message.</param>
    ''' <param name="inAttachments">A string array for multiple filenames.</param>
    ''' <remarks>This procedure takes string array parameters for 
    ''' multiple recipients (inTo) and files (inAttachments).</remarks>
    Public Overloads Shared Sub SendEmailMessage(ByVal inFrom As String, ByVal inTo() _
            As String, ByVal inSubject _
            As String, ByVal inBody _
            As String, ByVal inAttachments() As String)

        Try
            For Each thisRecipient As String In inTo
                ' Create a mail message
                Dim thisMailMsg As New MailMessage(New MailAddress(inFrom.Trim()), New MailAddress(Trim(thisRecipient)))
                thisMailMsg.BodyEncoding = Encoding.Default
                thisMailMsg.Subject = inSubject.Trim()
                thisMailMsg.Body = inBody.Trim() & NewLine
                thisMailMsg.Priority = MailPriority.High
                thisMailMsg.IsBodyHtml = True

                ' Attach each file attachment
                For Each thisAttachment As String In inAttachments
                    If Not thisAttachment = String.Empty Then
                        Dim thisFile As New Attachment(thisAttachment)
                        thisMailMsg.Attachments.Add(thisFile)
                    End If
                Next

                ' Use an SMTP client to send the mail message
                Dim theSmtpClient As New SmtpClient()
                theSmtpClient.Host = "smtp.gmail.com"  'e.g. "10.10.10.10"
                theSmtpClient.Send(thisMailMsg)
            Next

            'Message Successful

        Catch ex As Exception
            'Message Error
        End Try

    End Sub

    ''' <summary>
    ''' Send email messages via SMTP.
    ''' </summary>
    ''' <param name="inFrom">The sender.</param>
    ''' <param name="inTo">The recipient.</param>
    ''' <param name="inSubject">The subject.</param>
    ''' <param name="inBody">The message.</param>
    ''' <param name="inAttachment">The filename.</param>
    ''' <remarks>This procedure takes strings for the recipient and file attachement.</remarks>
    Public Overloads Shared Sub SendEmailMessage(ByVal inFrom As String, ByVal inTo _
            As String, ByVal inSubject _
            As String, ByVal inBody _
            As String, ByVal inAttachment As String)

        Try
            Dim theMailMsg As New MailMessage(New MailAddress(inFrom.Trim()), New MailAddress(inTo))
            theMailMsg.BodyEncoding = Encoding.Default
            theMailMsg.Subject = inSubject.Trim()
            theMailMsg.Body = inBody.Trim() & NewLine
            theMailMsg.Priority = MailPriority.High
            theMailMsg.IsBodyHtml = True

            If Not inAttachment = String.Empty Then
                Dim theAttachment As New Attachment(inAttachment)
                theMailMsg.Attachments.Add(theAttachment)
            End If

            ' Use an SMTP client to send the mail message
            Dim theSmtpClient As New SmtpClient()
            theSmtpClient.Host = "smtp.gmail.com"  'e.g. "10.10.10.10"
            theSmtpClient.Port = 465
            theSmtpClient.Send(theMailMsg)

            'Message Successful

        Catch ex As Exception
            'Message Error
        End Try

    End Sub

    ''' <summary>
    ''' Opens the help form.
    ''' </summary>
    ''' <param name="helpFormText">The text displayed on the form.</param>
    ''' <param name="theRTFStream">The .rtf file stream to populate the rich text box.</param>
    ''' <remarks></remarks>
    Public Shared Sub OpenHelp(ByVal helpFormText As String, ByVal theRTFStream As Stream)

        ' Get the help form.
        Dim theHelpForm As New HelpForm
        theHelpForm.Text = helpFormText

        If Not (theRTFStream Is Nothing) Then
            Dim sr As New StreamReader(theRTFStream)
            theHelpForm.uxRichTextBox.LoadFile(theRTFStream, RichTextBoxStreamType.RichText)
            sr.Close()
        End If

        If helpFormText = "Report Bug or Request New Feature" Then
            theHelpForm.uxReportOrRequest.Visible = False
        End If

        theHelpForm.Show()

    End Sub



#End Region

#Region "Private Members (none)"
#End Region

#End Region

End Class
#End Region
