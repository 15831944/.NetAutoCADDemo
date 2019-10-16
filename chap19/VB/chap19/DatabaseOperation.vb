Imports Autodesk.AutoCAD.Runtime
Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.Geometry
Imports Autodesk.AutoCAD.EditorInput

Public Class DatabaseOperation
<CommandMethod("CreateAndSaveDwg")> _
Public Sub CreateAndSaveDwg()
    '�½�һ�����ݿ�����Դ����µ�Dwg�ļ�
    Dim db As New Database()
    Using trans As Transaction = db.TransactionManager.StartTransaction
        '��ȡ���ݿ�Ŀ�����
        Dim bt As BlockTable = trans.GetObject(db.BlockTableId, OpenMode.ForRead)
        '��ȡ���ݿ��ģ�Ϳռ����¼����
        Dim btr As BlockTableRecord = trans.GetObject(bt(BlockTableRecord.ModelSpace), OpenMode.ForWrite)
        '�½�����Բ
        Dim cir1 As New Circle(New Point3d(1, 1, 0), Vector3d.ZAxis, 1.0)
        Dim cir2 As New Circle(New Point3d(4, 4, 0), Vector3d.ZAxis, 2.0)
        '��ģ�Ϳռ��м����½�������Բ
        btr.AppendEntity(cir1)
        trans.AddNewlyCreatedDBObject(cir1, True)
        btr.AppendEntity(cir2)
        trans.AddNewlyCreatedDBObject(cir2, True)

        '���屣���ļ��Ի���
        Dim opt As New PromptSaveFileOptions(vbCrLf & "�������ļ�����")
        '�����ļ��Ի�����ļ���չ���б�
        opt.Filter = "ͼ��(*.dwg)|*.dwg|ͼ��(*.dxf)|*.dxf"
        '�ļ��������б���ȱʡ��ʾ���ļ���չ��
        opt.FilterIndex = 0
        '�����ļ��Ի���ı���
        opt.DialogCaption = "ͼ�����Ϊ"
        'ȱʡ����Ŀ¼
        opt.InitialDirectory = "C:\"
        'ȱʡ�����ļ�������չ������չ���б��еĵ�ǰֵȷ����
        opt.InitialFileName = "MyDwg"
        '��ȡ��ǰ���ݿ���󣨲��������½��ģ��������ж���
        Dim ed As Editor = Application.DocumentManager.MdiActiveDocument.Editor
        '���ݱ���Ի������û���ѡ�񣬻�ȡ�����ļ���
        Dim filename As String = ed.GetFileNameForSave(opt).StringResult
        '�����ļ�Ϊ��ǰAutoCAD�汾
        db.SaveAs(filename, DwgVersion.Current)
    End Using
    '�������ݿ����
    db.Dispose()
End Sub

<CommandMethod("ReadDwg")> _
Public Sub ReadDwg()
    Dim ed As Editor = Application.DocumentManager.MdiActiveDocument.Editor
    '�½�һ�����ݿ�����Զ�ȡDwg�ļ�
    Dim db As New Database(False, True)
    '�ļ���
    Dim fileName As String = "C:\MyDwg.dwg"
    '���ָ���ļ������ļ����ڣ���
    If System.IO.File.Exists(fileName) Then
        '���ļ����뵽���ݿ���
        db.ReadDwgFile(fileName, System.IO.FileShare.Read, True, Nothing)
        Using trans As Transaction = db.TransactionManager.StartTransaction
            '��ȡ���ݿ�Ŀ�����
            Dim bt As BlockTable = trans.GetObject(db.BlockTableId, OpenMode.ForRead)
            '�����ݿ��ģ�Ϳռ����¼����
            Dim btr As BlockTableRecord = trans.GetObject(bt(BlockTableRecord.ModelSpace), OpenMode.ForRead)
            'ѭ������ģ�Ϳռ��е�ʵ��
            For Each id As ObjectId In btr
                Dim obj As DBObject = trans.GetObject(id, OpenMode.ForRead)
                If TypeOf (obj) Is Entity Then
                    '��ʾʵ�������
                    ed.WriteMessage(vbCrLf & obj.GetType.ToString)
                End If
            Next
        End Using
    End If
    '�������ݿ����
    db.Dispose()
End Sub

<CommandMethod("CopyFromOtherDwg")> _
Public Sub CopyFromOtherDwg()
    '�½�һ�����ݿ�����Զ�ȡDwg�ļ�
    Dim db As New Database(False, False)
    Dim fileName As String = "C:\Blocks and Tables - Metric.dwg"
    If System.IO.File.Exists(fileName) Then
        db.ReadDwgFile(fileName, System.IO.FileShare.Read, True, Nothing)
        'Ϊ���ò����ĺ����ڶ��ͼ���ļ��򿪵�����������ã������ʹ������ĺ�����Dwg�ļ��ر�
        db.CloseInput(True)
        '��ȡ��ǰ���ݿ���󣨲����½������ݿ⣩
        Dim curdb As Database = HostApplicationServices.WorkingDatabase
        '��Դ���ݿ�ģ�Ϳռ��е�ʵ����뵽��ǰ���ݿ��һ���µĿ���¼��
        curdb.Insert(System.IO.Path.GetFileNameWithoutExtension(fileName), db, True)
    End If
    '�������ݿ����
    db.Dispose()
End Sub

<CommandMethod("OpenDwg")> _
Public Sub OpenDwg()
    Dim ed As Editor = Application.DocumentManager.MdiActiveDocument.Editor
    '��ȡ�ĵ������������Դ�Dwg�ļ�
    Dim docs As DocumentCollection = Application.DocumentManager
    '���ô��ļ��Ի�����й�ѡ��
    Dim opt As New PromptOpenFileOptions(vbCrLf & "�������ļ�����")
    opt.Filter = "ͼ��(*.dwg)|*.dwg|ͼ��(*.dxf)|*.dxf"
    opt.FilterIndex = 0
    '���ݴ��ļ��Ի������û���ѡ�񣬻�ȡ�ļ���
    Dim filename As String = ed.GetFileNameForOpen(opt).StringResult
    '����ѡ���Dwg�ļ�
    Dim doc As Document = docs.Open(filename, True)
    '���õ�ǰ�Ļ�ĵ�Ϊ�´򿪵�Dwg�ļ�
    Application.DocumentManager.MdiActiveDocument = doc
    End Sub

<CommandMethod("CopyEntities")> _
Public Sub CopyEntities()
    Dim ed As Editor = Application.DocumentManager.MdiActiveDocument.Editor
    '�½�һ�����ݿ����
    Dim db As New Database(False, True)
    '��ȡ��ǰ���ݿ����
    Dim curdb As Database = HostApplicationServices.WorkingDatabase
    '����Ĳ���ѡ��Ҫ���Ƶ��½����ݿ��е�ʵ��
    Dim opts As New PromptSelectionOptions()
    opts.MessageForAdding = "�����븴�Ƶ����ļ���ʵ��"
    Dim ss As SelectionSet = ed.GetSelection(opts).Value
    '��ȡ��ѡʵ���ObjectId����
    Dim ids As New ObjectIdCollection(ss.GetObjectIds())
    '�ѵ�ǰ���ݿ�����ѡ���ʵ�帴�Ƶ��½������ݿ��У���ָ�������Ϊ��ǰ���ݿ�Ļ���
    db = curdb.Wblock(ids, curdb.Ucsorg)
    '��2004��ʽ�������ݿ�ΪDwg�ļ�
    db.SaveAs("C:\test.dwg", DwgVersion.AC1800)
End Sub

End Class
