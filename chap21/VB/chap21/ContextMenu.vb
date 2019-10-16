Imports Autodesk.AutoCAD.Runtime
Imports Autodesk.AutoCAD.Windows
Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.EditorInput
Imports Autodesk.AutoCAD.DatabaseServices
Public Class ContextMenu
<CommandMethod("AddDefaultContextMenu")> _
Public Sub AddContextMenu()
    '����һ��ContextMenuExtension�������ڱ�ʾ��ݲ˵�
   Dim contextMenu As New ContextMenuExtension()
    '���ÿ�ݲ˵��ı���
   contextMenu.Title = "�ҵĿ�ݲ˵�"
    '���һ����Ϊ"����"�Ĳ˵�����ڵ��ø�������
   Dim mi As New MenuItem("����")
    'Ϊ"����"�˵�����ӵ����¼�
   AddHandler mi.Click, AddressOf mi_Click
    '��"����"�˵�����ӵ���ݲ˵���
   contextMenu.MenuItems.Add(mi)
    '���һ����Ϊ"ɾ��"�Ĳ˵�����ڵ���ɾ������
   mi = New MenuItem("ɾ��")
    'Ϊ"ɾ��"�˵�����ӵ����¼�
   AddHandler mi.Click, AddressOf mi_Click
    '��"ɾ��"�˵�����ӵ���ݲ˵���
   contextMenu.MenuItems.Add(mi)
    'ΪӦ�ó�����Ӷ���Ŀ�ݲ˵�
   Application.AddDefaultContextMenuExtension(contextMenu)
End Sub

Sub mi_Click(ByVal sender As Object, ByVal e As EventArgs)
    '��ȡ��������Ŀ�ݲ˵���
    Dim mi As MenuItem = sender
    '��ȡ��ǰ��ĵ�
    Dim doc As Document = Application.DocumentManager.MdiActiveDocument
    '���ݿ�ݲ˵�������֣��ֱ���ö�Ӧ������
    If mi.Text = "����" Then
        doc.SendStringToExecute("_Copy ", True, False, True)
    ElseIf mi.Text = "ɾ��" Then
        doc.SendStringToExecute("_Erase ", True, False, True)
    End If
End Sub
<CommandMethod("AddObjectContextMenu")> _
Public Sub AddObjectContextMenu()
    '����һ��ContextMenuExtension�������ڱ�ʾ��ݲ˵�
   Dim contextMenu As New ContextMenuExtension()
    '���ڶ��󼶱�Ŀ�ݲ˵����������ò˵���
   'contextMenu.Title = "Բ�Ŀ�ݲ˵�"
    '���һ����Ϊ"Բ���"�Ĳ˵��������AutoCAD����������ʾ��ѡ���Բ���
   Dim miCircle As New MenuItem("Բ���")
    'Ϊ"Բ���"�˵�����ӵ����¼�
   AddHandler miCircle.Click, AddressOf miCircle_Click
    '��"Բ���"�˵�����ӵ���ݲ˵���
   contextMenu.MenuItems.Add(miCircle)
    'ΪԲ������Ӷ���Ŀ�ݲ˵�
   Application.AddObjectContextMenuExtension(RXClass.GetClass(GetType(Circle)), contextMenu)
End Sub
Sub miCircle_Click(ByVal sender As Object, ByVal e As EventArgs)
   Dim ed As Editor = Application.DocumentManager.MdiActiveDocument.Editor
   Dim db As Database = HostApplicationServices.WorkingDatabase
    '��ȡ��ǰ��ѡ�񼯶���
   Dim ss As SelectionSet = ed.SelectImplied().Value
   Using trans As Transaction = db.TransactionManager.StartTransaction()
        'ѭ������ѡ���еĶ���
        For Each id As ObjectId In ss.GetObjectIds()
            Dim obj As DBObject = trans.GetObject(id, OpenMode.ForRead)
            '�����ѡ��Ķ�����Բ
            If TypeOf (obj) Is Circle Then
                '��ȡ��ѡ���Բ����
                Dim circ As Circle = CType(obj, Circle)
                '������������ʾԲ�����Ϣ
                ed.WriteMessage(vbCrLf & "Բ���Ϊ:" & circ.Area.ToString())
            End If
        Next
   End Using
End Sub
End Class
