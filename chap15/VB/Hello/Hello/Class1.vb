Imports Autodesk.AutoCAD.Runtime
Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.EditorInput

Public Class Class1
    <CommandMethod("Hello")> _
    Public Sub Hello()
        ' ��ȡ��ǰ��ĵ���Editor����Ҳ����������
        Dim ed As Editor = Application.DocumentManager.MdiActiveDocument.Editor
        ' ����Editor�����WriteMessage����������������ʾ�ı�
        ed.WriteMessage("��ӭ����.NET����AutoCAD�����磡")
    End Sub
End Class
