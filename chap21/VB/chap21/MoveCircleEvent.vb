Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Autodesk.AutoCAD.Runtime
Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.EditorInput
Imports Autodesk.AutoCAD.Geometry

Namespace CH20
    Public Class MoveCircleEvent
        Dim db As Database = HostApplicationServices.WorkingDatabase
        Dim doc As Document = Application.DocumentManager.MdiActiveDocument
        Dim ed As Editor = Application.DocumentManager.MdiActiveDocument.Editor
        Dim bMove As Boolean
        Dim startPoint As Point3d
        Sub commandWillStart(ByVal sender As Object, ByVal e As CommandEventArgs)
            '���AutoCAD����ΪMOVE��
            If e.GlobalCommandName = "MOVE" Then
                '����ȫ�ֱ���bMoveΪTrue����ʾ�ƶ����ʼ
                bMove = True
            End If
        End Sub


        Sub objectOpenedForModify(ByVal sender As Object, ByVal e As ObjectEventArgs)
            '�ж�AutoCAD�����Ƿ�Ϊ�ƶ�
            If bMove = False Then
                '���AutoCAD����Ϊ���ƶ����򷵻�
                Return
            End If
            '�жϽ�Ҫ�ƶ��Ķ����Ƿ�ΪԲ
            If TypeOf (e.DBObject) Is Circle Then
                '��ȡ��Ҫ�ƶ���Բ���󣬵���û�ƶ�
                Dim circle As Circle = CType(e.DBObject, Circle)
                '��ȡԲ�����ģ�����ͬ��Բ��Բ��
                startPoint = circle.Center
            End If
        End Sub

        Sub objectModified(ByVal sender As Object, ByVal e As ObjectEventArgs)
            '�ж�AutoCAD�����Ƿ�Ϊ�ƶ�
            If bMove = False Then
                '���AutoCAD����Ϊ���ƶ����򷵻�
                Return
            End If
            '�Ͽ����е��¼�������
            removeEvents()
            '�ж��ƶ����Ķ����Ƿ�ΪԲ
            If TypeOf (e.DBObject) Is Circle Then
                '��ȡ�ƶ���Բ����
                Dim startCircle As Circle = CType(e.DBObject, Circle)
                '����ѡ�񼯹�������ֻѡ��ͼ���е�Բ
                Dim values() As TypedValue = {New TypedValue(DxfCode.Start, "Circle")}
                Dim filter As New SelectionFilter(values)
                Dim resSel As PromptSelectionResult = ed.SelectAll(filter)
                '���ѡ�����Բ
                If resSel.Status = PromptStatus.OK Then
                    '��ȡѡ���е�Բ����
                    Dim sSet As SelectionSet = resSel.Value
                    Dim ids As ObjectId() = sSet.GetObjectIds()
                    '��ʼ������
                    Using trans As Transaction = db.TransactionManager.StartTransaction
                        'ѭ������ѡ���е�Բ
                        For Each id As ObjectId In ids
                            '�Զ��ķ�ʽ��Բ����
                            Dim followedCirlce As Circle = CType(trans.GetObject(id, OpenMode.ForRead), Circle)
                            'ͨ���ж�Բ��Բ�������ƶ���Բ��Բ���Ƿ���ͬ�����ƶ����е�ͬ��Բ
                            If followedCirlce.Center = startPoint Then
                                '��Ϊ�����Զ��ķ�ʽ����Բ������Ϊ�˸ı�Բ��Բ�ı���ı�Ϊд
                                followedCirlce.UpgradeOpen()
                                '�ı�Բ��Բ�ģ��Դﵽ�ƶ���Ŀ��
                                followedCirlce.Center = startCircle.Center
                            End If
                        Next id
                        '�ύ������
                        trans.Commit()
                    End Using
                End If
            End If
            '�������е��¼�������
            addEvents()
        End Sub
        Sub commandEnded(ByVal sender As Object, ByVal e As CommandEventArgs)
            '�ж�AutoCAD�����Ƿ�Ϊ�ƶ�
            If bMove = True Then
                '����ȫ�ֱ���bMoveΪFalse����ʾ�ƶ��������
                bMove = False
            End If
        End Sub


        <CommandMethod("AddEvents")> _
        Public Sub addEvents()
            '���¼�����������Ӧ���¼���������
            AddHandler db.ObjectOpenedForModify, AddressOf objectOpenedForModify
            AddHandler db.ObjectModified, AddressOf objectModified
            AddHandler doc.CommandWillStart, AddressOf commandWillStart
            AddHandler doc.CommandEnded, AddressOf commandEnded
        End Sub

        <CommandMethod("RemoveEvents")> _
        Public Sub removeEvents()
            '�Ͽ����е��¼�������
            RemoveHandler db.ObjectOpenedForModify, AddressOf objectOpenedForModify
            RemoveHandler db.ObjectModified, AddressOf objectModified
            RemoveHandler doc.CommandWillStart, AddressOf commandWillStart
            RemoveHandler doc.CommandEnded, AddressOf commandEnded
        End Sub
    End Class
End Namespace
