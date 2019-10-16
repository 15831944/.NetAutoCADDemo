Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.Runtime
Imports Autodesk.AutoCAD.Geometry
Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.EditorInput
Public Class MyXrecord
<CommandMethod("CreateXrecord")> _
Public Sub CreateXrecord()
    Dim ed As Editor = Application.DocumentManager.MdiActiveDocument.Editor
    Dim db As Database = HostApplicationServices.WorkingDatabase
    Using trans As Transaction = db.TransactionManager.StartTransaction
        '�½�һ����չ��¼����
        Dim xrec As New Xrecord()
        '������չ��¼�а����������б������ı������ꡢ��ֵ���Ƕȡ���ɫ
        Dim pt As New Point3d(1.0, 2.0, 0.0)
        xrec.Data = New ResultBuffer( _
                    New TypedValue(DxfCode.Text, "����һ�������õ���չ��¼�б�"), _
                    New TypedValue(DxfCode.XCoordinate, pt), _
                    New TypedValue(DxfCode.Real, 3.14159), _
                    New TypedValue(DxfCode.Angle, 3.14159), _
                    New TypedValue(DxfCode.Color, 1), _
                    New TypedValue(DxfCode.Int16, 180))
        '����Ĳ�������ѡ��Ҫ�����չ��¼�Ķ���
        Dim opt As New PromptEntityOptions("��ѡ��Ҫ�����չ��¼�Ķ���")
        Dim res As PromptEntityResult = ed.GetEntity(opt)
        If res.Status <> PromptStatus.OK Then
            Return
        End If
        Dim ent As Entity = trans.GetObject(res.ObjectId, OpenMode.ForWrite)
        '�ж���ѡ�����Ƿ��Ѱ�����չ��¼
        If ent.ExtensionDictionary <> ObjectId.Null Then
            ed.WriteMessage("�����Ѱ�����չ��¼���޷��ٴ���")
            Return
        End If
        'Ϊ��ѡ��Ķ��󴴽�һ����չ�ֵ�
        ent.CreateExtensionDictionary()
        Dim dictEntId As ObjectId = ent.ExtensionDictionary()
        Dim entXrecord As DBDictionary = trans.GetObject(dictEntId, OpenMode.ForWrite)
        '����չ�ֵ��м��������½�����չ��¼���󣬲�ָ�����������ؼ���ΪMyXrecord
        entXrecord.SetAt("MyXrecord", xrec)
        '֪ͨ�����������չ��¼����ļ���
        trans.AddNewlyCreatedDBObject(xrec, True)
        trans.Commit()
    End Using
End Sub

<CommandMethod("ListXrecord")> _
Public Sub ListXrecord()
    Dim ed As Editor = Application.DocumentManager.MdiActiveDocument.Editor
    Dim db As Database = HostApplicationServices.WorkingDatabase
    Using trans As Transaction = db.TransactionManager.StartTransaction
        '����Ĳ�������ѡ����ʾ��չ��¼�Ķ���
        Dim opt As New PromptEntityOptions("��ѡ��Ҫ��ʾ��չ��¼�Ķ���")
        Dim res As PromptEntityResult = ed.GetEntity(opt)
        If res.Status <> PromptStatus.OK Then
            Return
        End If
        Dim ent As Entity = trans.GetObject(res.ObjectId, OpenMode.ForRead)
        '����ѡ��������չ�ֵ�
        Dim entXrecord As DBDictionary = trans.GetObject(ent.ExtensionDictionary, OpenMode.ForRead)
        '����չ�ֵ��������ؼ���ΪMyXrecord����չ��¼��������ҵ��򷵻�����ObjectId
        Dim xrecordId As ObjectId = entXrecord.GetAt("MyXrecord")
        '���ҵ�����չ��¼����
        Dim xrecord As Xrecord = trans.GetObject(xrecordId, OpenMode.ForRead)
        '��ȡ��չ��¼�а����������б�ѭ��������ʾ����
        Dim rb As ResultBuffer = xrecord.Data
        For Each value As TypedValue In rb
            ed.WriteMessage(String.Format(vbCrLf & "TypeCode={0},Value={1}", value.TypeCode, value.Value))
        Next
        trans.Commit()
    End Using
End Sub
End Class
