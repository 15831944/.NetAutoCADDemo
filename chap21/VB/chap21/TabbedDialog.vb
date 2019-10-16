Imports Autodesk.AutoCAD.Runtime
Imports Autodesk.AutoCAD.ApplicationServices
Public Class TabbedDialog
<CommandMethod("CreateNewOptionsTab")> _
Public Sub CreateNewOptionsTab()
    '��DisplayingOptionDialog�¼�������ʱ����ѡ��Ի�����ʾ��������displayingOptionDialog����
   AddHandler Application.DisplayingOptionDialog, AddressOf displayingOptionDialog
End Sub
Sub displayingOptionDialog(ByVal sender As Object, ByVal e As TabbedDialogEventArgs)
    '����һ���Զ���ؼ���ʵ��������ʹ�õ�����ʾͼƬ���Զ���ؼ�
   Dim optionTab As New PictureTab()
    'Ϊȷ����ť��Ӷ�����Ҳ����Ϊȡ����Ӧ�á�������ť��Ӷ���
   Dim onOkPress As New TabbedDialogAction(AddressOf OnOptionOK)
    '����ʾͼƬ�Ŀؼ���Ϊ��ǩҳ��ӵ�ѡ��Ի��򣬲�����ȷ����ť�������Ķ���
   e.AddTab("ͼƬ", New TabbedDialogExtension(optionTab, onOkPress))
End Sub

Sub OnOptionOK()
   '��ȷ����ť������ʱ����ʾһ������Ի���
   Application.ShowAlertDialog("���ѡ��Ի����ǩҳ�ɹ���")
End Sub

End Class
