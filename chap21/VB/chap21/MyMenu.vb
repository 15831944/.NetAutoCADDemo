Imports Autodesk.AutoCAD.Runtime
Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.Interop
Public Class MyMenu
<CommandMethod("AddMenu")> _
Public Sub AddMenu()
    'COM��ʽ��ȡAutoCADӦ�ó������
    Dim acadApp As AcadApplication = Application.AcadApplication
    'ΪAutoCAD���һ���µĲ˵��������ñ���Ϊ"�ҵĲ˵�"
    Dim pm As AcadPopupMenu = acadApp.MenuGroups.Item(0).Menus.Add("�ҵĲ˵�")
    '����һ��AutoCAD�����˵�����ڻ�ȡ��ӵĲ˵������
    Dim pmi As AcadPopupMenuItem
    '���½��Ĳ˵������һ����Ϊ"Բ"�Ĳ˵���Ե��û���Բ����
    pmi = pm.AddMenuItem(pm.Count + 1, "Բ", "_Circle ")
    '����״̬����ʾ��Ϣ
    pmi.HelpString = "��ָ���뾶����Բ"
    '�����Ϊ"ֱ��"�Ĳ˵���Ե��û���ֱ������
    pmi = pm.AddMenuItem(pm.Count + 1, "ֱ��", "_Line ")
    '����״̬����ʾ��Ϣ
    pmi.HelpString = "����ֱ�߶�"
    '�����Ϊ"�����"�Ĳ˵���Ե��û��ƶ��������
    pmi = pm.AddMenuItem(pm.Count + 1, "�����", "_Polyline ")
    '����״̬����ʾ��Ϣ
    pmi.HelpString = "������ά�����"
    '�����Ϊ"����"�Ĳ˵���Ե��û��ƾ��ζ��������
    pmi = pm.AddMenuItem(pm.Count + 1, "����", "_Rectangle ")
    '����״̬����ʾ��Ϣ
    pmi.HelpString = "�������ζ����"
    '���һ���ָ��������ֲ�ͬ���͵�����
    pm.AddSeparator(pm.Count + 1)
    '���һ����Ϊ"�޸�"���Ӳ˵�
    Dim menuModify As AcadPopupMenu = pm.AddSubMenu(pm.Count + 1, "�޸�")
    '��"�޸�"�Ӳ˵���������ڸ��ơ�ɾ�����ƶ�����ת�����Ĳ˵����������Ӧ��״̬����ʾ��Ϣ
    pmi = menuModify.AddMenuItem(menuModify.Count + 1, "����", "_Copy ")
    pmi.HelpString = "���ƶ���"
    pmi = menuModify.AddMenuItem(menuModify.Count + 1, "ɾ��", "_Erase ")
    pmi.HelpString = "��ͼ��ɾ������"
    pmi = menuModify.AddMenuItem(menuModify.Count + 1, "�ƶ�", "_Move ")
    pmi.HelpString = "�ƶ�����"
    pmi = menuModify.AddMenuItem(menuModify.Count + 1, "��ת", "_Rotate ")
    pmi.HelpString = "�ƻ�����ת����"
    '������Ĳ˵���ʾ��AutoCAD�˵��������
    pm.InsertInMenuBar(acadApp.MenuBar.Count + 1)
End Sub
End Class
