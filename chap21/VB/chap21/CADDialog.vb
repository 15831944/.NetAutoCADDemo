Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Autodesk.AutoCAD.Runtime
Imports Autodesk.AutoCAD.ApplicationServices
Public Class CADDialog
<CommandMethod("ModalForm")> _
Public Sub ShowModalForm()
    '��ʾģ̬�Ի���
    Dim modalForm As New ModalForm()
    Application.ShowModalDialog(modalForm)
End Sub
End Class
