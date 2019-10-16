Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.Runtime
Imports Autodesk.AutoCAD.Geometry
Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.EditorInput

Public Class MyXData
<CommandMethod("AddXData")> _
Public Sub AddXData()
    Dim ed As Editor = Application.DocumentManager.MdiActiveDocument.Editor
    Dim db As Database = HostApplicationServices.WorkingDatabase
    Using trans As Transaction = db.TransactionManager.StartTransaction
        '����Ĳ�������ѡ��ʵ���������չ����
        Dim opt As New PromptEntityOptions("��ѡ��ʵ���������չ����")
        Dim res As PromptEntityResult = ed.GetEntity(opt)
        If res.Status <> PromptStatus.OK Then
            Return
        End If
        Dim circ As Circle = trans.GetObject(res.ObjectId, OpenMode.ForWrite)
        '��ȡ��ǰ���ݿ��ע��Ӧ�ó����
        Dim reg As RegAppTable = trans.GetObject(db.RegAppTableId, OpenMode.ForWrite)
        '���û����Ϊ"ʵ����չ����"��ע��Ӧ�ó�����¼����
        If Not reg.Has("ʵ����չ����") Then
            '����һ��ע��Ӧ�ó�����¼������ʾ��չ����
            Dim app As New RegAppTableRecord
            '������չ���ݵ�����
            app.Name = "ʵ����չ����"
            '��ע��Ӧ�ó���������չ����
            reg.Add(app)
            trans.AddNewlyCreatedDBObject(app, True)
        End If
        '������չ���ݵ�����
        Dim rb As New ResultBuffer( _
        New TypedValue(DxfCode.ExtendedDataRegAppName, "ʵ����չ����"), _
        New TypedValue(DxfCode.ExtendedDataAsciiString, "�ַ�����չ����"), _
        New TypedValue(DxfCode.ExtendedDataLayerName, "0"), _
        New TypedValue(DxfCode.ExtendedDataReal, 1.23479137438413E+40), _
        New TypedValue(DxfCode.ExtendedDataInteger16, 32767), _
        New TypedValue(DxfCode.ExtendedDataInteger32, 32767), _
        New TypedValue(DxfCode.ExtendedDataScale, 10), _
        New TypedValue(DxfCode.ExtendedDataWorldXCoordinate, New Point3d(10, 10, 0)))
        '���½�����չ���ݸ��ӵ���ѡ���ʵ����
        circ.XData = rb
        trans.Commit()
    End Using
End Sub

<CommandMethod("ListXData")> _
Public Sub ListXData()
    Dim ed As Editor = Application.DocumentManager.MdiActiveDocument.Editor
    Dim db As Database = Application.DocumentManager.MdiActiveDocument.Database
    Using trans As Transaction = db.TransactionManager.StartTransaction
        '����Ĳ�������ѡ��ʵ������ʾ������չ����
        Dim opt As New PromptEntityOptions("��ѡ��ʵ������ʾ������չ����")
        Dim res As PromptEntityResult = ed.GetEntity(opt)
        If res.Status <> PromptStatus.OK Then
            Return
        End If
        Dim circ As Circle = trans.GetObject(res.ObjectId, OpenMode.ForRead)
        '��ȡ��ѡ��ʵ������Ϊ��ʵ����չ���ݡ�����չ����
        Dim rb As ResultBuffer = circ.GetXDataForApplication("ʵ����չ����")
        '���û�У��ͷ���
        If rb = Nothing Then
            Return
        End If
        'ѭ��������չ����
        For Each circleXData As TypedValue In rb
            ed.WriteMessage(String.Format(vbCrLf & "TypeCode={0},Value={1}", circleXData.TypeCode, circleXData.Value))
        Next
    End Using
End Sub
End Class
