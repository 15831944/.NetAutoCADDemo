Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.Runtime
Imports Autodesk.AutoCAD.Geometry
Imports Autodesk.AutoCAD.EditorInput
Imports Autodesk.AutoCAD.Colors

Public Class MyBlock
<CommandMethod("CB")> _
Public Function CreateBlock() As ObjectId
    Dim db As Database = HostApplicationServices.WorkingDatabase
    '���ڷ����������Ŀ�Ķ���Id
    Dim blockId As ObjectId = ObjectId.Null
    '����һ��BlockTableRecord��Ķ��󣬱�ʾ��Ҫ�����Ŀ�
    Dim record As New BlockTableRecord()
    '���ÿ���
    record.Name = "Room"
    Using trans As Transaction = db.TransactionManager.StartTransaction()
        Dim points As New Point3dCollection() '���ڱ�ʾ��ɿ�Ķ���ߵĶ���
        points.Add(New Point3d(-18.0, -6.0, 0.0))
        points.Add(New Point3d(18.0, -6.0, 0.0))
        points.Add(New Point3d(18.0, 6.0, 0.0))
        points.Add(New Point3d(-18.0, 6.0, 0.0))
        '������ɿ�Ķ����
        Dim pline As New Polyline2d(Poly2dType.SimplePoly, points, 0.0, True, 0.0, 0.0, Nothing)
        record.Origin = points(3) '���ÿ�Ļ���
        record.AppendEntity(pline) '������߼��뵽�½���BlockTableRecord����
        '��д�ķ�ʽ�򿪿��
        Dim bt As BlockTable = trans.GetObject(db.BlockTableId, OpenMode.ForWrite)
        If bt.Has("Room") = False Then '�ж��Ƿ������Ϊ"Room"�Ŀ�
            blockId = bt.Add(record) '�ڿ���м���"Room"��
            trans.AddNewlyCreatedDBObject(record, True) '֪ͨ������
            trans.Commit() '�ύ����
        End If
    End Using
    Return blockId '���ش����Ŀ�Ķ���Id
End Function

<CommandMethod("CBWA")> _
Public Function CreateBlockWithAttributes() As ObjectId
    Dim db As Database = HostApplicationServices.WorkingDatabase
    Dim blockId As ObjectId = ObjectId.Null
    Dim record As New BlockTableRecord()
    record.Name = "RMNUM"
    Using trans As Transaction = db.TransactionManager.StartTransaction()
        Dim points As New Point3dCollection()
        points.Add(New Point3d(-18.0, -6.0, 0.0))
        points.Add(New Point3d(18.0, -6.0, 0.0))
        points.Add(New Point3d(18.0, 6.0, 0.0))
        points.Add(New Point3d(-18.0, 6.0, 0.0))
        Dim pline As New Polyline2d(Poly2dType.SimplePoly, points, 0.0, True, 0.0, 0.0, Nothing)
        record.Origin = points(3)
        record.AppendEntity(pline)
        Dim attdef As New AttributeDefinition
        attdef.Position = New Point3d(0.0, 0.0, 0.0)
        attdef.Height = 8.0 '�������ָ߶�
        attdef.Rotation = 0.0 '����������ת�Ƕ�
        attdef.HorizontalMode = TextHorizontalMode.TextMid '����ˮƽ���뷽ʽ
        attdef.VerticalMode = TextVerticalMode.TextVerticalMid '���ô�ֱ���뷽ʽ
        attdef.Prompt = "Room Number:" '����������ʾ
        attdef.TextString = "0000" '�������Ե�ȱʡֵ
        attdef.Tag = "NUMBER" '�������Ա�ǩ
        attdef.Invisible = False '���ò��ɼ�ѡ��Ϊ�٣�����ɼ�
        attdef.Verifiable = False '������֤��ʽΪ��
        attdef.Preset = False '����Ԥ�÷�ʽΪ��
        attdef.Constant = False '���ó�����ʽΪ��
        record.Origin = points(3)
        record.AppendEntity(attdef)
        Dim bt As BlockTable = trans.GetObject(db.BlockTableId, OpenMode.ForWrite)
        If bt.Has("RMNUM") = False Then '�ж��Ƿ������Ϊ"RMNUM"�Ŀ�
            blockId = bt.Add(record) '�ڿ���м���"RMNUM"��
            trans.AddNewlyCreatedDBObject(record, True)
            trans.Commit()
        End If
    End Using
    Return blockId
End Function

<CommandMethod("InsertBlock")> _
Public Sub InsertBlock()
    '����һ���������Եļ򵥿�"Room"
    InsertBlockRef("Room", New Point3d(100, 100, 0), New Scale3d(2), 0)
End Sub

Public Sub InsertBlockRef(ByVal blockName As String, ByVal point As Point3d, ByVal scale As Scale3d, ByVal rotateAngle As Double)
    Dim db As Database = HostApplicationServices.WorkingDatabase
    Using trans As Transaction = db.TransactionManager.StartTransaction
        '�Զ��ķ�ʽ�򿪿��
        Dim bt As BlockTable = trans.GetObject(db.BlockTableId, OpenMode.ForRead)
        '���û��blockName��ʾ�Ŀ飬����򷵻�
        If (bt.Has(blockName) = False) Then
            Return
        End If
        '�Զ��ķ�ʽ��blockName��ʾ�Ŀ�
        Dim block As BlockTableRecord = trans.GetObject(bt(blockName), OpenMode.ForRead)
        '����һ������ղ����ò����
        Dim blockref As BlockReference = New BlockReference(point, bt(blockName))

        blockref.ScaleFactors = scale '���ÿ���յ����ű���

        blockref.Rotation = rotateAngle '���ÿ���յ���ת�Ƕ�
        '��д�ķ�ʽ�򿪵�ǰ�ռ䣨ģ�Ϳռ��ͼֽ�ռ䣩
        Dim btr As BlockTableRecord = trans.GetObject(db.CurrentSpaceId, OpenMode.ForWrite)
        btr.AppendEntity(blockref) '�ڵ�ǰ�ռ���봴���Ŀ����
        '֪ͨ��������봴���Ŀ����
        trans.AddNewlyCreatedDBObject(blockref, True)
        trans.Commit() '�ύ��������ʵ�ֿ���յ���ʵ����
    End Using
End Sub

<CommandMethod("InsertBlockWithAtt")> _
Public Sub InsertBlockWithAtt()
    '����һ�������ԵĿ�"RMNUM"
    InsertBlockRefWithAtt("RMNUM", New Point3d(100, 150, 0), New Scale3d(1), 0.5 * Math.PI, "2007")
    '����һ���������Եļ򵥿�"Room"
    InsertBlockRefWithAtt("Room", New Point3d(100, 200, 0), New Scale3d(1), 0, Nothing)
End Sub

Public Sub InsertBlockRefWithAtt(ByVal blockName As String, ByVal point As Point3d, ByVal scale As Scale3d, ByVal rotateAngle As Double, ByVal roomnumber As String)
    Dim db As Database = HostApplicationServices.WorkingDatabase
    Using trans As Transaction = db.TransactionManager.StartTransaction
        Dim bt As BlockTable = trans.GetObject(db.BlockTableId, OpenMode.ForRead)
        If (bt.Has(blockName) = False) Then
            Return
        End If
        Dim block As BlockTableRecord = trans.GetObject(bt(blockName), OpenMode.ForRead)
        Dim blockref As BlockReference = New BlockReference(point, bt(blockName))
        blockref.ScaleFactors = scale
        blockref.Rotation = rotateAngle
        Dim btr As BlockTableRecord = trans.GetObject(db.CurrentSpaceId, OpenMode.ForWrite)
        btr.AppendEntity(blockref)
        trans.AddNewlyCreatedDBObject(blockref, True)
        '��ȡblockName��ı���������ʵ�ֶԿ��ж���ķ���
        Dim iterator As BlockTableRecordEnumerator = block.GetEnumerator()
        '���blockName���������
        If block.HasAttributeDefinitions Then
            '���ÿ�������Կ��еĶ�����б���
            While iterator.MoveNext
                '��ȡ���������ǰָ��Ŀ��еĶ���
                Dim obj As DBObject = trans.GetObject(iterator.Current, OpenMode.ForRead)
                '����һ���µ����Բ��ն���
                Dim att As New AttributeReference()
                '�жϿ��������ǰָ��Ŀ��еĶ����Ƿ�Ϊ���Զ���
                If TypeOf (obj) Is AttributeDefinition Then
                    '��ȡ���Զ������
                    Dim attdef As AttributeDefinition = obj
                    '�����Զ�������м̳���ص����Ե����Բ��ն�����
                    att.SetAttributeFromBlock(attdef, blockref.BlockTransform)
                    '�������Բ��ն����λ��Ϊ���Զ����λ��+����յ�λ��
                    att.Position = attdef.Position + blockref.Position.GetAsVector()
                    '�ж����Զ��������
                    Select Case attdef.Tag
                        '���Ϊ"NUMBER"�������ÿ���յ�����ֵ
                        Case "NUMBER"
                            att.TextString = roomnumber
                    End Select
                    '�жϿ�����Ƿ��д���粻��д�����л�Ϊ��д״̬
                    If Not blockref.IsWriteEnabled Then
                        blockref.UpgradeOpen()
                    End If
                    '����´��������Բ���
                    blockref.AttributeCollection.AppendAttribute(att)
                    '֪ͨ����������´��������Բ���
                    trans.AddNewlyCreatedDBObject(att, True)
                End If
            End While
        End If
        trans.Commit() '�ύ������
    End Using
End Sub

<CommandMethod("BrowseBlock")> _
Public Sub BrowseBlock()
    Dim ed As Editor = Application.DocumentManager.MdiActiveDocument.Editor
    Dim db As Database = HostApplicationServices.WorkingDatabase
    'ֻѡ�����ն���
    Dim filterValues As TypedValue() = {New TypedValue(DxfCode.Start, "INSERT")}
    Dim filter As New SelectionFilter(filterValues)
    Dim opts As New PromptSelectionOptions()
    '��ʾ�û�����ѡ��
    opts.MessageForAdding = "��ѡ��ͼ���еĿ����"
    '����ѡ�������ѡ����󣬱�����Ϊ����ն���
    Dim res As PromptSelectionResult = ed.GetSelection(opts, filter)
    '���ѡ��ʧ�ܣ��򷵻�
    If res.Status <> PromptStatus.OK Then
        Return
    End If
    '��ȡѡ�񼯶��󣬱�ʾ��ѡ��Ķ��󼯺�
    Dim ss As SelectionSet = res.Value
    '��ȡѡ���а����Ķ���ID�����ڷ���ѡ���еĶ���
    Dim ids As ObjectId() = ss.GetObjectIds()
    Using trans As Transaction = db.TransactionManager.StartTransaction()
        'ѭ������ѡ���еĶ���
        For Each blockId As ObjectId In ids
            '��ȡѡ���е�ǰ�Ķ���������ֻѡ�����ն�������ֱ�Ӹ�ֵΪ�����
            Dim blockRef As BlockReference = trans.GetObject(blockId, OpenMode.ForRead)
            '��ȡ��ǰ����ն��������Ŀ���¼����
            Dim btr As BlockTableRecord = trans.GetObject(blockRef.BlockTableRecord, OpenMode.ForRead)
            '������������ʾ����
            ed.WriteMessage(vbCrLf & "�飺" & btr.Name)
            '���ٿ���¼����
            btr.Dispose()
            '��ȡ����յ����Լ��϶���
            Dim atts As AttributeCollection = blockRef.AttributeCollection
            'ѭ���������Լ��϶���
            For Each attId As ObjectId In atts
                '��ȡ��ǰ�Ŀ��������
                Dim attRef As AttributeReference = trans.GetObject(attId, OpenMode.ForRead)
                '��ȡ��������Ե�������������ֵ
                Dim attString As String = " ��������" & attRef.Tag & " ����ֵ��" & attRef.TextString
                '������������ʾ���������������ֵ
                ed.WriteMessage(attString)
            Next
        Next
    End Using
End Sub
<CommandMethod("ChangeBlockAtt")> _
Public Sub ChangeBlockAtt()
    '������º���
    Dim roomNumber As String = "2008"
    '�ı����ʵ�����ɫ�Ϳ�������Ե�ֵ
    ChangeBlock("RMNUM", roomNumber)
End Sub

Public Sub ChangeBlock(ByVal blockName As String, ByVal roomNumber As String)
    Dim ed As Editor = Application.DocumentManager.MdiActiveDocument.Editor
    Dim db As Database = HostApplicationServices.WorkingDatabase
    Using trans As Transaction = db.TransactionManager.StartTransaction()
        '�򿪿��
        Dim bt As BlockTable = trans.GetObject(db.BlockTableId, OpenMode.ForRead)
        '�򿪿������ΪblockName�Ŀ���¼
        Dim btr As BlockTableRecord = trans.GetObject(bt(blockName), OpenMode.ForRead)
        '��ȡ������ΪblockName�Ŀ����
        Dim blcokRefIds As ObjectIdCollection = btr.GetBlockReferenceIds(True, True)
        'ѭ�����������
        For Each blockRefId As ObjectId In blcokRefIds
            '�򿪵�ǰ�����
            Dim blockRef As BlockReference = trans.GetObject(blockRefId, OpenMode.ForRead)
            '��ȡ��ǰ����յ����Լ���
            Dim blockRefAtts As AttributeCollection = blockRef.AttributeCollection
            'ѭ���������Լ���
            For Each attId As ObjectId In blockRefAtts
                '��ȡ��ǰ���Բ��ն���
                Dim attRef As AttributeReference = trans.GetObject(attId, OpenMode.ForRead)
                'ֻ�ı�NUMBER����ֵΪ"0000"������ֵΪroomNumber
                Select Case attRef.Tag
                    Case "NUMBER"
                        If attRef.TextString = "0000" Then
                            attRef.UpgradeOpen() '�л����Բ��ն���Ϊ��д״̬
                            attRef.TextString = roomNumber
                        End If
                End Select
            Next attId
        Next blockRefId
        trans.Commit()
    End Using
Application.UpdateScreen()
End Sub
End Class
