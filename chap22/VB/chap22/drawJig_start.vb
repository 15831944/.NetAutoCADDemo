Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.EditorInput
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.Geometry
Imports Autodesk.AutoCAD.GraphicsInterface
Imports Autodesk.AutoCAD.Runtime

Public Class drawJig_start
    ' ��DrawJig��̳�.
    Inherits DrawJig
    ' ��������Ƕ���.
    Private ent As Polyline
    ' ��������ǵ����ĺ�һ������.
    Private mCenterPt, peakPt As Point3d

    <CommandMethod("FiveStart")> Sub CreateDrawJigFiveStart()
        Dim db As Database = HostApplicationServices.WorkingDatabase
        Dim ed As Editor = Application.DocumentManager.MdiActiveDocument.Editor

        ' ��ͨ�ĵ㽻������.
        Dim optPoint As New PromptPointOptions(vbCrLf & "��ָ������ǵ�����")
        Dim resPoint As PromptPointResult = ed.GetPoint(optPoint)
        If resPoint.Status <> PromptStatus.OK Then Return
        mCenterPt = resPoint.Value

        ' ���ڴ��д���һ������10������ķ�ն���߶���.
        Dim pt(9) As Point2d
        pt(0) = New Point2d(0, 0)
        pt(1) = New Point2d(0, 0)
        pt(2) = New Point2d(0, 0)
        pt(3) = New Point2d(0, 0)
        pt(4) = New Point2d(0, 0)
        pt(5) = New Point2d(0, 0)
        pt(6) = New Point2d(0, 0)
        pt(7) = New Point2d(0, 0)
        pt(8) = New Point2d(0, 0)
        pt(9) = New Point2d(0, 0)
        Dim pts As New Point2dCollection(pt)

        ent = New Polyline(10)
        For i As Integer = 0 To 9
            ent.AddVertexAt(i, pts.Item(i), 0, 0, 0)
        Next
        ent.Closed = True

        Using trans As Transaction = db.TransactionManager.StartTransaction()
            Dim bt As BlockTable = trans.GetObject(db.BlockTableId, OpenMode.ForRead)
            Dim btr As BlockTableRecord = trans.GetObject(bt(BlockTableRecord.ModelSpace), OpenMode.ForWrite)
            ' ��ʼ��ק.
            Dim resJig As PromptResult = ed.Drag(Me)
            If resJig.Status = PromptStatus.OK Then
                ' ������Ƕ�����뵽ͼ�����ݿ���.
                btr.AppendEntity(ent)
                trans.AddNewlyCreatedDBObject(ent, True)
                trans.Commit()
            End If
        End Using
    End Sub

    ' Sampler�������ڼ���û�������.
    Protected Overrides Function Sampler(ByVal prompts As JigPrompts) As SamplerStatus
        Dim db As Database = HostApplicationServices.WorkingDatabase
        ' ����һ�����϶�������.
        Dim optJigPoint As New JigPromptPointOptions(vbCrLf & "��ָ������ǵ�һ���ǵ�:")
        ' ������ק�������.
        optJigPoint.Cursor = CursorType.RubberBand
        ' �����϶�������.
        optJigPoint.BasePoint = mCenterPt
        optJigPoint.UseBasePoint = True
        ' ��AcquirePoint�����õ��û�����ĵ�.
        Dim resJigPoint1 As PromptPointResult = prompts.AcquirePoint(optJigPoint)
        Dim curPt As Point3d = resJigPoint1.Value

        If curPt <> peakPt Then
            ' ����������Բ����--------------------------------------------.
            ' ����ǵ�����
            Dim p0 As Point2d = New Point2d(mCenterPt.X, mCenterPt.Y)
            ' ��������ǵĵ�һ����������.
            Dim p1 As New Point2d(curPt(0), curPt(1))
            ' Ϊ��������9��������������׼��.
            Dim d1 As Double = p1.GetDistanceTo(p0)
            Dim d2 As Double = d1 * Math.Sin(Rad2Ang(18)) / Math.Sin(Rad2Ang(54))
            Dim vec As Vector2d = p1 - p0
            Dim ang As Double = vec.Angle

            ' �������������9�����������.
            Dim p2 As Point2d = PolarPoint(p0, ang + Rad2Ang(36), d2)
            Dim p3 As Point2d = PolarPoint(p0, ang + Rad2Ang(72), d1)
            Dim p4 As Point2d = PolarPoint(p0, ang + Rad2Ang(108), d2)
            Dim p5 As Point2d = PolarPoint(p0, ang + Rad2Ang(144), d1)
            Dim p6 As Point2d = PolarPoint(p0, ang + Rad2Ang(180), d2)
            Dim p7 As Point2d = PolarPoint(p0, ang + Rad2Ang(216), d1)
            Dim p8 As Point2d = PolarPoint(p0, ang + Rad2Ang(252), d2)
            Dim p9 As Point2d = PolarPoint(p0, ang + Rad2Ang(288), d1)
            Dim p10 As Point2d = PolarPoint(p0, ang + Rad2Ang(324), d2)

            ' ��������Ǹ������������.
            ent.SetPointAt(0, p1)
            ent.SetPointAt(1, p2)
            ent.SetPointAt(2, p3)
            ent.SetPointAt(3, p4)
            ent.SetPointAt(4, p5)
            ent.SetPointAt(5, p6)
            ent.SetPointAt(6, p7)
            ent.SetPointAt(7, p8)
            ent.SetPointAt(8, p9)
            ent.SetPointAt(9, p10)
            ' ���浱ǰ��.
            peakPt = curPt
            Return SamplerStatus.OK
        Else
            Return SamplerStatus.NoChange
        End If
    End Function

    ' WorldDraw��������ˢ����Ļ����ʾ��ͼ��.
    Protected Overrides Function WorldDraw(ByVal draw As WorldDraw) As Boolean
        ' ˢ�»���.
        draw.Geometry.Draw(ent)
        Return True
    End Function

    ' ��ȡ�������ָ���ǶȺ;���ĵ�.
    Public Function PolarPoint(ByVal basePt As Point2d, ByVal angle As Double, ByVal distance As Double) As Point2d
        Dim pt(2) As Double
        pt(0) = basePt(0) + distance * Math.Cos(angle)
        pt(1) = basePt(1) + distance * Math.Sin(angle)
        PolarPoint = New Point2d(pt(0), pt(1))
    End Function

    ' �Ȼ����ȵĺ���.
    Public Function Rad2Ang(ByVal angle As Double) As Double
        Rad2Ang = angle * Math.PI / 180
    End Function
End Class
