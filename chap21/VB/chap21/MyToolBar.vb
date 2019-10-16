Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Autodesk.AutoCAD.Runtime
Imports Autodesk.AutoCAD.Interop
Imports Autodesk.AutoCAD.ApplicationServices

Public Class MyToolBar

<CommandMethod("AddToolBar")> _
Public Sub AddToolBar()
    '��ȡ��ǰ���еĳ���
    Dim myModule As System.Reflection.Module = System.Reflection.Assembly.GetExecutingAssembly().GetModules()(0)
    '��ȡ��ǰ���еĳ��򼯵�����·���������ļ�����
    Dim modulePath As String = myModule.FullyQualifiedName
    '��ȡȥ���ļ�������򼯵�·�������������ڵ��ļ���
    modulePath = modulePath.Substring(0, modulePath.LastIndexOf("\"))
    'COM��ʽ��ȡAutoCADӦ�ó������
    Dim acadApp As AcadApplication = Application.AcadApplication
    '��ȡ��ǰ�˵��飬���ڼ��빤����
    Dim currMenuGroup As AcadMenuGroup = acadApp.MenuGroups.Item(0)
    'ΪAutoCAD���һ���µĹ������������ñ���Ϊ"�ҵĹ�����"
    Dim tbModify As AcadToolbar = currMenuGroup.Toolbars.Add("�ҵĹ�����")
    '���½��Ĺ����������һ��"����"��ť���Ե��ø�������
    Dim button0 As AcadToolbarItem = tbModify.AddToolbarButton("", "����", "���ƶ���", "_Copy ")
    '���ø��ư�ť��ͼƬ
    button0.SetBitmaps(modulePath + "\Resources\Copy.bmp", modulePath + "\Resources\Copy.bmp")
    '���һ��"ɾ��"��ť���Ե���ɾ������
    Dim button1 As AcadToolbarItem = tbModify.AddToolbarButton("", "ɾ��", "��ͼ��ɾ������", "_Erase ")
    '����ɾ����ť��ͼƬ
    button1.SetBitmaps(modulePath + "\Resources\Erase.bmp", modulePath + "\Resources\Erase.bmp")
    '���һ��"�ƶ�"��ť���Ե���ɾ������
    Dim button2 As AcadToolbarItem = tbModify.AddToolbarButton("", "�ƶ�", "�ƶ�����", "_Move ")
    '�����ƶ���ť��ͼƬ
    button2.SetBitmaps(modulePath + "\Resources\Move.bmp", modulePath + "\Resources\Move.bmp")
    '���һ��"��ת"��ť���Ե�����ת����
    Dim button3 As AcadToolbarItem = tbModify.AddToolbarButton("", "��ת", "�ƻ�����ת����", "_Rotate ")
    '������ת��ť��ͼƬ
    button3.SetBitmaps(modulePath + "\Resources\Rotate.bmp", modulePath + "\Resources\Rotate.bmp")

    '���һ��������ť���ð�ťֻ������������Ļ�ͼ������
    Dim FlyoutButton As AcadToolbarItem = tbModify.AddToolbarButton("", "��ͼ����", "��ͼ����", " ", True)
    '�����ڶ������������ù�������ͨ��������ť���ӵ���һ����������
    Dim tbDraw As AcadToolbar = currMenuGroup.Toolbars.Add("��ͼ������")
    '��������ֱ��ڹ����������û���Բ��ֱ�ߡ�����ߡ����εİ�ť
    Dim button4 As AcadToolbarItem = tbDraw.AddToolbarButton("", "Բ", "��ָ���뾶����Բ", "_Circle ")
    button4.SetBitmaps(modulePath + "\Resources\Circle.bmp", modulePath + "\Resources\Circle.bmp")
    Dim button5 As AcadToolbarItem = tbDraw.AddToolbarButton("", "ֱ��", "����ֱ�߶�", "_Line ")
    button5.SetBitmaps(modulePath + "\Resources\Line.bmp", modulePath + "\Resources\Line.bmp")
    Dim button6 As AcadToolbarItem = tbDraw.AddToolbarButton("", "�����", "������ά�����", "_Pline ")
    button6.SetBitmaps(modulePath + "\Resources\Polyline.bmp", modulePath + "\Resources\Polyline.bmp")
    Dim button7 As AcadToolbarItem = tbDraw.AddToolbarButton("", "����", "�������ζ����", "_Rectangle ")
    button7.SetBitmaps(modulePath + "\Resources\Rectangle.bmp", modulePath + "\Resources\Rectangle.bmp")
    '���ڶ������������ŵ���һ���������ĵ�����ť��
    FlyoutButton.AttachToolbarToFlyout(currMenuGroup.Name, tbDraw.Name)
    '��ʾ��һ��������
    tbModify.Visible = True
    '���صڶ���������
    tbDraw.Visible = False
End Sub
End Class
