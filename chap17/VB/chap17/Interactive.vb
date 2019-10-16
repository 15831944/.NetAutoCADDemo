Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.Colors
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.EditorInput
Imports Autodesk.AutoCAD.Geometry
Imports Autodesk.AutoCAD.Runtime

Public Class Interactive
    <CommandMethod("AddPoly")> Public Sub CreatePoly()
        Dim db As Database = HostApplicationServices.WorkingDatabase
        Dim ed As Editor = Application.DocumentManager.MdiActiveDocument.Editor
        ' ��ʼ���߿�.
        Dim width As Double = 0
        ' ��ʼ����ɫ����ֵ.
        Dim colorIndex As Integer = 0
        ' ��ʼ��������.
        Dim index As Integer = 2
        ' ���ڴ��д��������.
        Dim polyEnt As New Polyline
        ' ��������ߵ�ObjectId.
        Dim polyEntId As ObjectId

        ' ����һ������û�������.
        Dim optPoint As New PromptPointOptions(vbCrLf & "�������һ����<100,200>")
        ' �����û��س���Ӧ.
        optPoint.AllowNone = True
        ' ���ص���û���ʾ��.
        Dim resPoint As PromptPointResult = ed.GetPoint(optPoint)
        ' �û�����ESC�����˳�.
        If resPoint.Status = PromptStatus.Cancel Then Return
        ' ������һ�������.
        Dim ptStart As Point3d
        ' �û����س���.
        If resPoint.Status = PromptStatus.None Then
            ' �õ���һ��������Ĭ��ֵ.
            ptStart = New Point3d(100, 200, 0)
        Else
            ' �õ���һ�������.
            ptStart = resPoint.Value
        End If
        ' ���浱ǰ��.
        Dim ptPrevious As Point3d = ptStart

nextPoint:
        ' ����������һ��ĵ㽻����.
        Dim optPtKey As New PromptPointOptions(vbCrLf & "��������һ�����[�߿�(W)/��ɫ(C)/���(O)]<O>")
        ' ����ʹ�û�׼��.
        optPtKey.UseBasePoint = True
        ' ���û�׼��.
        optPtKey.BasePoint = ptPrevious
        ' Ϊ�㽻������ӹؼ���.
        optPtKey.Keywords.Add("W", "W", "W", False, True)
        optPtKey.Keywords.Add("C", "C", "C", False, True)
        optPtKey.Keywords.Add("O", "O", "O", False, True)
        ' ����Ĭ�ϵĹؼ���.
        optPtKey.Keywords.Default = "O"
        ' �����û���ʾ��.
        Dim resKey As PromptPointResult = ed.GetPoint(optPtKey)
        ' �û�����ESC�����˳�.
        If resKey.Status = PromptStatus.Cancel Then Return
        ' ������һ�������.
        Dim ptNext As Point3d
        If resKey.Status = PromptStatus.Keyword Then
            ' ����û�������ǹؼ��ּ��϶����еĹؼ��֡���
            Select Case resKey.StringResult
                Case Is = "W"
                    width = getWidth()
                Case Is = "C"
                    colorIndex = getcolorindex()
                Case Else
                    Using trans As Transaction = db.TransactionManager.StartTransaction
                        Try
                            trans.GetObject(polyEntId, OpenMode.ForWrite)
                            ' �������ǰ�����������߿����ɫ.
                            polyEnt.Color = Color.FromColorIndex(ColorMethod.ByColor, colorIndex)
                            polyEnt.ConstantWidth = width
                        Catch
                            ' �˴��������.
                        End Try
                        trans.Commit()
                    End Using
                    Return
            End Select
            GoTo nextPoint
        Else
            ' �õ����������һ��.
            ptNext = resKey.Value
            If index = 2 Then
                ' ��ȡ��ά���X��Y����ֵ��ת��Ϊ��ά��.
                Dim pt1 As New Point2d(ptPrevious(0), ptPrevious(1))
                Dim pt2 As New Point2d(ptNext(0), ptNext(1))
                ' ���������Ӷ��㣬�����߿�.
                polyEnt.AddVertexAt(0, pt1, 0, width, width)
                polyEnt.AddVertexAt(1, pt2, 0, width, width)
                ' ���ö���ߵ���ɫ.
                polyEnt.Color = Color.FromColorIndex(ColorMethod.ByColor, colorIndex)
                ' ���������ӵ�ͼ�����ݿⲢ����һ��ObjectId(�ڻ�ͼ���ڶ�̬��ʾ�����).
                polyEntId = AppendEntity(polyEnt)
            Else
                Using trans As Transaction = db.TransactionManager.StartTransaction
                    ' �򿪶����.
                    trans.GetObject(polyEntId, OpenMode.ForWrite)
                    ' ������Ӷ���ߵĶ���.
                    Dim ptCurrent As New Point2d(ptNext(0), ptNext(1))
                    polyEnt.AddVertexAt(index - 1, ptCurrent, 0, width, width)
                    ' �������ö���ߵ���ɫ���߿�.
                    polyEnt.Color = Color.FromColorIndex(ColorMethod.ByColor, colorIndex)
                    polyEnt.ConstantWidth = width
                    trans.Commit()
                End Using
            End If
            index = index + 1
        End If
        ptPrevious = ptNext
        GoTo nextPoint
    End Sub

    ' �õ��û������߿�ĺ���.
    Public Function getWidth() As Double
        Dim ed As Editor = Application.DocumentManager.MdiActiveDocument.Editor
        ' ����һ���������û�������. 
        Dim optDou As New PromptDoubleOptions(vbCrLf & "�������߿�")
        ' ���������븺��.
        optDou.AllowNegative = False
        ' ����Ĭ��ֵ.
        optDou.DefaultValue = 0
        Dim resDou As PromptDoubleResult = ed.GetDouble(optDou)
        If resDou.Status = PromptStatus.OK Then
            ' �õ��û�������߿�.
            Dim width As Double = resDou.Value
            Return width
        Else
            Return 0
        End If
    End Function

    ' �õ��û�������ɫ����ֵ�ĺ���.
    Public Function getcolorindex() As Integer
        Dim ed As Editor = Application.DocumentManager.MdiActiveDocument.Editor
        ' ����һ���������û�������. 
        Dim optInt As New PromptIntegerOptions(vbCrLf & "��������ɫ����ֵ(0��256)")
        ' ����Ĭ��ֵ.
        optInt.DefaultValue = 0
        ' ����һ��������ʾ��.
        Dim resInt As PromptIntegerResult = ed.GetInteger(optInt)
        If resInt.Status = PromptStatus.OK Then
            ' �õ��û��������ɫ����ֵ.
            Dim colorIndex As Integer = resInt.Value
            If colorIndex > 256 Or colorIndex < 0 Then
                Return 0
            Else
                Return colorIndex
            End If
        Else
            Return 0
        End If
    End Function

    ' ��ͼ�ζ�����뵽ģ�Ϳռ�ĺ���.
    Public Shared Function AppendEntity(ByVal ent As Entity) As ObjectId
        Dim db As Database = HostApplicationServices.WorkingDatabase
        Dim entId As ObjectId
        Using trans As Transaction = db.TransactionManager.StartTransaction
            Dim bt As BlockTable = trans.GetObject(db.BlockTableId, OpenMode.ForRead)
            Dim btr As BlockTableRecord = trans.GetObject(bt(BlockTableRecord.ModelSpace), OpenMode.ForWrite)
            entId = btr.AppendEntity(ent)
            trans.AddNewlyCreatedDBObject(ent, True)
            trans.Commit()
        End Using
        Return entId
    End Function
End Class
