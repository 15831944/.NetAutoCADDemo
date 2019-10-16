Imports System
Imports Autodesk.AutoCAD
Imports Autodesk.AutoCAD.Runtime
Imports Autodesk.AutoCAD.Geometry
Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.EditorInput
Public Class BlockImportClass
<CommandMethod("ImportBlocks")> _
Public Sub ImportBlocks()
    '�ⲿ�ļ���
    Dim filename As String = "C:\Blocks and Tables - Metric.dwg"
    '����ָ�����ⲿ�ļ��е����
    ImportBlocksFromDwg(filename)
End Sub

Public Sub ImportBlocksFromDwg(ByVal sourceFileName As String)
    Dim dm As DocumentCollection = Application.DocumentManager
    Dim ed As Editor = dm.MdiActiveDocument.Editor
    '��ȡ��ǰ���ݿ���ΪĿ�����ݿ�
    Dim destDb As Database = dm.MdiActiveDocument.Database
    '����һ���µ����ݿ������ΪԴ���ݿ⣬�Զ����ⲿ�ļ��еĶ���
    Dim sourceDb As Database = New Database(False, True)
    '����һ���쳣��������Դ�����ܷ������쳣
    Dim ex As Autodesk.AutoCAD.Runtime.Exception
    Try
        '��DWG�ļ����뵽Դ���ݿ���
        sourceDb.ReadDwgFile(sourceFileName, System.IO.FileShare.Read, True, "")
        '����һ�������洢��Ķ���Id�б�
        Dim blockIds As ObjectIdCollection = New ObjectIdCollection()
        '��ȡԴ���ݿ�������������
        Dim tm As Autodesk.AutoCAD.DatabaseServices.TransactionManager = sourceDb.TransactionManager
        '��Դ���ݿ��п�ʼ������
        Dim myT As Transaction = tm.StartTransaction()
        Using myT
            '��Դ���ݿ��еĿ��
            Dim bt As BlockTable = CType(tm.GetObject(sourceDb.BlockTableId, OpenMode.ForRead, False), BlockTable)
            '����ÿ����
            Dim btrId As ObjectId
            For Each btrId In bt
                Dim btr As BlockTableRecord = CType(tm.GetObject(btrId, OpenMode.ForRead, False), BlockTableRecord)
                'ֻ����������ͷǲ��ֿ鵽�����б���
                If Not btr.IsAnonymous And Not btr.IsLayout Then
                    blockIds.Add(btrId)
                    btr.Dispose()
                End If
            Next
            bt.Dispose()
            myT.Dispose()
        End Using
        '����һ��IdMapping����
        Dim mapping As IdMapping = New IdMapping
        '��Դ���ݿ���Ŀ�����ݿ⸴�ƿ���¼
        sourceDb.WblockCloneObjects(blockIds, destDb.BlockTableId, mapping, DuplicateRecordCloning.Replace, False)
        '������ɺ���������ʾ�����˶��ٸ������Ϣ
        ed.WriteMessage("������ " + blockIds.Count.ToString() + " ���飬�� " + sourceFileName + " ����ǰͼ��")
    Catch ex
        ed.WriteMessage("���Ƴ���: " + ex.Message)
    End Try
    '������ɣ�����Դ���ݿ�
    sourceDb.Dispose()
End Sub
End Class
