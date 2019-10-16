Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.Runtime
Imports Autodesk.AutoCAD.Geometry
Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.EditorInput

Public Class MyGroup
<CommandMethod("MakeGroup")> _
Public Sub MakeGroup()
    '������ΪMyGroup����
    createGroup("MyGroup")
End Sub
<CommandMethod("RemoveButLines")> _
Public Sub RemoveButLines()
    '��MyGroup�����Ƴ����в���ֱ�ߵĶ���
    removeAllButLines("MyGroup")
End Sub

Private Sub createGroup(ByVal groupName As String)
    Dim ed As Editor = Application.DocumentManager.MdiActiveDocument.Editor
    Dim db As Database = HostApplicationServices.WorkingDatabase
    Using trans As Transaction = db.TransactionManager.StartTransaction
        '�½�һ�������
        Dim gp As New Group(groupName, True)
        '�򿪵�ǰ���ݿ�����ֵ�����Լ����½��������
        Dim dict As DBDictionary = trans.GetObject(db.GroupDictionaryId, OpenMode.ForWrite)
        '�����ֵ��н��������Ϊһ������Ŀ���룬��ָ�����������ؼ���ΪgroupName
        dict.SetAt(groupName, gp)
        '����Ĳ�������ѡ������Ҫ�����Ķ���
        Dim opt As New PromptSelectionOptions()
        opt.MessageForAdding = "��ѡ������Ҫ�����Ķ���"
        Dim res As PromptSelectionResult = ed.GetSelection(opt)
        If res.Status <> PromptStatus.OK Then
            Return
        End If
        '��ȡ��ѡ������ObjectId����
        Dim ss As SelectionSet = res.Value
        Dim ids As New ObjectIdCollection(ss.GetObjectIds())
        '��������м�����ѡ��Ķ���
        gp.Append(ids)
        '֪ͨ��������������ļ���
        trans.AddNewlyCreatedDBObject(gp, True)
        trans.Commit()
    End Using
End Sub
Private Sub removeAllButLines(ByVal groupName As String)
    Dim ed As Editor = Application.DocumentManager.MdiActiveDocument.Editor
    Dim db As Database = Application.DocumentManager.MdiActiveDocument.Database
    Using trans As Transaction = db.TransactionManager.StartTransaction
        '�򿪵�ǰ���ݿ�����ֵ����
        Dim dict As DBDictionary = trans.GetObject(db.GroupDictionaryId, OpenMode.ForRead)
        '�����ֵ��������ؼ���ΪgroupName�����������ҵ��򷵻�����ObjectId
        Dim gpid As ObjectId = dict.GetAt(groupName)
        '����Ҫ�����н���ȥ������Ĳ����������д�ķ�ʽ���ҵ��������
        Dim gp As Group = trans.GetObject(gpid, OpenMode.ForWrite)
        '��ȡ������е�����ʵ���ObjectId������ѭ������
        Dim ids As ObjectId() = gp.GetAllEntityIds()
        For Each id As ObjectId In ids
            '�����еĵ�ǰ���󣬲��ж��Ƿ�Ϊֱ��
            Dim obj As DBObject = trans.GetObject(id, OpenMode.ForRead)
            If Not TypeOf (obj) Is Line Then
                '���������ֱ�ߣ����������Ƴ���
                gp.Remove(id)
            End If
        Next
        '������������ʵ�����ɫΪ��ɫ
        gp.SetColorIndex(1)
        trans.Commit()
    End Using
End Sub
End Class
