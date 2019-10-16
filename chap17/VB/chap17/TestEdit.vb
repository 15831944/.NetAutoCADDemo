Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.Colors
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.EditorInput
Imports Autodesk.AutoCAD.Geometry
Imports Autodesk.AutoCAD.Runtime

Public Class TestEdit
    ' ��ѡ��.
    <CommandMethod("testSel")> Public Sub testSelection1()
        Dim db As Database = HostApplicationServices.WorkingDatabase
        Dim ed As Editor = Application.DocumentManager.MdiActiveDocument.Editor
        ' ����һ��ѡ�񼯽�����.
        Dim optSel As New PromptSelectionOptions
        ' ѡ�����ʱ����ʾ�ı�.
        optSel.MessageForAdding = "��ѡ�����"
        ' ����ѡ�񼯵��û���ʾ��.
        Dim resSel As PromptSelectionResult = ed.GetSelection(optSel)
        ' �õ�ѡ�񼯶���.
        Dim sSet As SelectionSet = resSel.Value
        ' �õ�ѡ�������ж����ObjectId����.
        Dim ids As ObjectId() = sSet.GetObjectIds()
        Using trans As Transaction = db.TransactionManager.StartTransaction()
            ' ����ѡ��.
            For Each sSetEntId As ObjectId In ids
                Dim en As Entity = trans.GetObject(sSetEntId, OpenMode.ForRead)
                ed.WriteMessage((vbCrLf & "��ѡ�����: " & en.GetType().Name))
            Next sSetEntId
            trans.Commit()
        End Using
    End Sub

    ' �����˵�ѡ��.
    <CommandMethod("testFilSel")> Public Sub testSelection2()
        Dim db As Database = HostApplicationServices.WorkingDatabase
        Dim ed As Editor = Application.DocumentManager.MdiActiveDocument.Editor

        '�����������
        Dim value1 As TypedValue = New TypedValue(DxfCode.Start, "CIRCLE,LINE")
        Dim value2 As TypedValue = New TypedValue(DxfCode.LayerName, "0")
        Dim value3 As TypedValue = New TypedValue(DxfCode.Color, "1")
        Dim values() As TypedValue = {value1, value2, value3}
        Dim sfilter As New SelectionFilter(values)

        ' ����һ��ѡ�񼯽�����.
        Dim optSel As New PromptSelectionOptions
        ' ѡ�����ʱ����ʾ�ı�.
        optSel.MessageForAdding = "��ѡ��λ��0��ĺ�ɫ��Բ�ͺ�ɫ��ֱ��"
        ' ����ѡ�񼯵��û���ʾ��.
        Dim resSel As PromptSelectionResult = ed.GetSelection(optSel, sfilter)
        ' �õ�ѡ�񼯶���.
        Dim sSet As SelectionSet = resSel.Value
        ' �õ�ѡ�������ж����ObjectId����.
        Dim ids As ObjectId() = sSet.GetObjectIds()
        Using trans As Transaction = db.TransactionManager.StartTransaction()
            ' ����ѡ��.
            For Each sSetEntId As ObjectId In ids
                Dim ent As Entity = trans.GetObject(sSetEntId, OpenMode.ForWrite)
                ' �޸���ѡ��������ɫ.
                ent.Color = Color.FromColorIndex(ColorMethod.ByColor, 2)
                ed.WriteMessage((vbCrLf & "��ѡ�����: " & ent.GetType().Name))
            Next sSetEntId
            trans.Commit()
        End Using
    End Sub

    ' �ƶ�.
    <CommandMethod("netMove")> Public Sub testMove()
        Dim db As Database = HostApplicationServices.WorkingDatabase
        Dim ed As Editor = Application.DocumentManager.MdiActiveDocument.Editor

        Dim optSel As New PromptSelectionOptions
        optSel.MessageForAdding = "��ѡ�����"
        'opt.AllowDuplicates = True

        Dim resSel As PromptSelectionResult = ed.GetSelection
        If resSel.Status <> PromptStatus.OK Then Return

        Dim sset As SelectionSet = resSel.Value
        Dim ids As ObjectId() = sset.GetObjectIds()


        For Each id As ObjectId In ids
            Edit.Move(id, New Point3d(0, 0, 0), New Point3d(300, 200, 0))
        Next id
    End Sub

    ' ����.
    <CommandMethod("netCopy")> Public Sub testCopy()
        Dim db As Database = HostApplicationServices.WorkingDatabase
        Dim ed As Editor = Application.DocumentManager.MdiActiveDocument.Editor

        Dim optSel As New PromptSelectionOptions
        optSel.MessageForAdding = "��ѡ�����"

        Dim resSel As PromptSelectionResult = ed.GetSelection
        If resSel.Status <> PromptStatus.OK Then Return

        Dim sSet As SelectionSet = resSel.Value
        Dim ids As ObjectId() = sSet.GetObjectIds()

        For Each id As ObjectId In ids
            Edit.Copy(id, New Point3d(0, 0, 0), New Point3d(300, 200, 0))
        Next id
    End Sub

    ' ��ת.
    <CommandMethod("netRotate")> Public Sub testRotate()
        Dim db As Database = HostApplicationServices.WorkingDatabase
        Dim ed As Editor = Application.DocumentManager.MdiActiveDocument.Editor

        Dim optSel As New PromptSelectionOptions
        optSel.MessageForAdding = "��ѡ�����"

        Dim resSel As PromptSelectionResult = ed.GetSelection
        If resSel.Status <> PromptStatus.OK Then Return

        Dim sSet As SelectionSet = resSel.Value
        Dim ids As ObjectId() = sSet.GetObjectIds()

        For Each id As ObjectId In ids
            Edit.Rotate(id, New Point3d(0, 0, 0), 30)
        Next id
    End Sub

    ' ����.
    <CommandMethod("netScale")> Public Sub testScale()
        Dim db As Database = HostApplicationServices.WorkingDatabase
        Dim ed As Editor = Application.DocumentManager.MdiActiveDocument.Editor

        Dim optSel As New PromptSelectionOptions
        optSel.MessageForAdding = "��ѡ�����"

        Dim resSel As PromptSelectionResult = ed.GetSelection
        If resSel.Status <> PromptStatus.OK Then Return

        Dim sSet As SelectionSet = resSel.Value
        Dim ids As ObjectId() = sSet.GetObjectIds()

        For Each id As ObjectId In ids
            Edit.Scale(id, New Point3d(0, 0, 0), 3)
        Next id
    End Sub

    ' ����.
    <CommandMethod("netMirror")> Public Sub testMirror()
        Dim db As Database = HostApplicationServices.WorkingDatabase
        Dim ed As Editor = Application.DocumentManager.MdiActiveDocument.Editor

        Dim optSel As New PromptSelectionOptions
        optSel.MessageForAdding = "��ѡ�����"

        Dim resSel As PromptSelectionResult = ed.GetSelection
        If resSel.Status <> PromptStatus.OK Then Return

        Dim sSet As SelectionSet = resSel.Value
        Dim ids As ObjectId() = sSet.GetObjectIds()

        For Each id As ObjectId In ids
            Edit.Mirror(id, New Point3d(0, 0, 0), New Point3d(300, 200, 0), False)
        Next id
    End Sub

    ' ƫ��.
    <CommandMethod("netOffset")> Public Sub testOffset()
        Dim db As Database = HostApplicationServices.WorkingDatabase
        Dim ed As Editor = Application.DocumentManager.MdiActiveDocument.Editor

        'ʰȡ����---------------------------------------------------------- 
        Dim optEnt As New PromptEntityOptions(vbCrLf & "��ѡ��Ҫƫ�ƵĶ���")

        Dim resEnt As PromptEntityResult = ed.GetEntity(optEnt)
        If resEnt.Status = PromptStatus.OK Then
            Using ta As Transaction = ed.Document.TransactionManager.StartTransaction()
                Dim ent As Entity = ta.GetObject(resEnt.ObjectId, OpenMode.ForRead)

                Edit.Offset(ent, -10)
                ta.Commit()
            End Using
        End If
    End Sub

    ' ��������.
    <CommandMethod("netArrayRectang")> Public Sub testArrayRectang()
        Dim db As Database = HostApplicationServices.WorkingDatabase
        Dim ed As Editor = Application.DocumentManager.MdiActiveDocument.Editor

        Dim optSel As New PromptSelectionOptions
        optSel.MessageForAdding = "��ѡ�����"

        Dim resSel As PromptSelectionResult = ed.GetSelection
        If resSel.Status <> PromptStatus.OK Then Return

        Dim sSet As SelectionSet = resSel.Value
        Dim ids As ObjectId() = sSet.GetObjectIds()

        For Each id As ObjectId In ids
            Edit.ArrayRectang(id, 5, 8, 300, 200)
        Next id
    End Sub

    ' ��������.
    <CommandMethod("netArrayPolar")> Public Sub testArrayPolar()
        Dim db As Database = HostApplicationServices.WorkingDatabase
        Dim ed As Editor = Application.DocumentManager.MdiActiveDocument.Editor

        Dim optSel As New PromptSelectionOptions
        optSel.MessageForAdding = "��ѡ�����"

        Dim resSel As PromptSelectionResult = ed.GetSelection
        If resSel.Status <> PromptStatus.OK Then Return

        Dim sSet As SelectionSet = resSel.Value
        Dim ids As ObjectId() = sSet.GetObjectIds()

        For Each id As ObjectId In ids
            Edit.ArrayPolar(id, New Point3d(0, 0, 0), 8, Edit.Rad2Ang(360))
        Next id
    End Sub

    ' ɾ��.
    <CommandMethod("netErase")> Public Sub testErase()
        Dim db As Database = HostApplicationServices.WorkingDatabase
        Dim ed As Editor = Application.DocumentManager.MdiActiveDocument.Editor

        Dim optSel As New PromptSelectionOptions
        optSel.MessageForAdding = "��ѡ�����"

        Dim resSel As PromptSelectionResult = ed.GetSelection
        If resSel.Status <> PromptStatus.OK Then Return

        Dim sSet As SelectionSet = resSel.Value
        Dim ids As ObjectId() = sSet.GetObjectIds()

        Using trans As Transaction = db.TransactionManager.StartTransaction()
            For Each id As ObjectId In ids
                Edit.Erase(id)
            Next id
        End Using
    End Sub
End Class
