Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.EditorInput
Imports Autodesk.AutoCAD.Geometry
Imports Autodesk.AutoCAD.GraphicsInterface
Imports Autodesk.AutoCAD.Runtime

Public Class EllipseJig
    ' ��EntityJig��̳�.
    Inherits EntityJig
    ' ����ȫ�ֱ���.
    Private mCenterPt, mMajorPt As Point3d
    Private mNormal, mMajorAxis As Vector3d
    Private mPromptCounter As Integer
    Private mRadiusRatio, radiusRatio As Double
    Private startAng, endAng, ang1, ang2 As Double
    ' ������Ĺ��캯��.
    Public Sub New(ByVal center As Point3d, ByVal vec As Vector3d)
        MyBase.New(New Ellipse())
        mCenterPt = center
        mNormal = vec
    End Sub

    ' Sampler�������ڼ���û�������.
    Protected Overrides Function Sampler(ByVal prompts As JigPrompts) As SamplerStatus
        Select Case mPromptCounter
            Case 0
                ' ����һ�����϶�������.
                Dim optJigPoint As New JigPromptPointOptions(vbCrLf & "��ָ����Բ������һ��")
                ' ������ק�Ĺ������.
                optJigPoint.Cursor = CursorType.RubberBand
                ' �����϶�������.
                optJigPoint.BasePoint = mCenterPt
                optJigPoint.UseBasePoint = True
                ' ��AcquirePoint�����õ��û�����ĵ�.
                Dim resJigPoint As PromptPointResult = prompts.AcquirePoint(optJigPoint)
                Dim curPt As Point3d = resJigPoint.Value
                If curPt <> mMajorPt Then
                    ' ���浱ǰ��. 
                    mMajorPt = curPt
                Else
                    Return SamplerStatus.NoChange
                End If
            Case 1
                ' ����һ�������϶�������.
                Dim optJigDis As New JigPromptDistanceOptions(vbCrLf & "��ָ����һ������ĳ���")
                ' ���ö���ק��Լ��:��ֹ������͸�ֵ.
                optJigDis.UserInputControls = UserInputControls.NoZeroResponseAccepted Or UserInputControls.NoNegativeResponseAccepted
                ' ������ק�Ĺ������.
                optJigDis.Cursor = CursorType.RubberBand
                ' �����϶�������.
                optJigDis.BasePoint = mCenterPt
                optJigDis.UseBasePoint = True
                ' ��AcquireDistance�����õ��û�����ľ���ֵ.
                Dim resJigDis As PromptDoubleResult = prompts.AcquireDistance(optJigDis)
                Dim mRadiusRatioTemp As Double = resJigDis.Value
                If mRadiusRatioTemp <> mRadiusRatio Then
                    ' ���浱ǰ����ֵ.
                    mRadiusRatio = mRadiusRatioTemp
                Else
                    Return SamplerStatus.NoChange
                End If
            Case 2
                ' ������Բ��0�Ȼ�׼��.
                Dim baseAng As Double
                Dim mMajorAxis2d As New Vector2d(mMajorAxis.X, mMajorAxis.Y)
                If radiusRatio < 1 Then
                    baseAng = mMajorAxis2d.Angle
                Else
                    baseAng = mMajorAxis2d.Angle + 0.5 * Math.PI
                End If
                ' �޸�ϵͳ������ANGBASE��.
                Application.SetSystemVariable("ANGBASE", baseAng)

                ' ����һ���Ƕ��϶�������.
                Dim optJigAngle1 As New JigPromptAngleOptions(vbCrLf & "��ָ����Բ������ʼ�Ƕ�")
                ' ������ק�Ĺ������.
                optJigAngle1.Cursor = CursorType.RubberBand
                ' �����϶�������.
                optJigAngle1.BasePoint = mCenterPt
                optJigAngle1.UseBasePoint = True
                ' ��AcquireAngle�����õ��û�����ĽǶ�ֵ.
                Dim resJigAngle1 As PromptDoubleResult = prompts.AcquireAngle(optJigAngle1)
                ang1 = resJigAngle1.Value
                If startAng <> ang1 Then
                    ' ���浱ǰ�Ƕ�ֵ.
                    startAng = ang1
                Else
                    Return SamplerStatus.NoChange
                End If
            Case 3
                ' ����һ���Ƕ��϶�������.
                Dim optJigAngle2 As New JigPromptAngleOptions(vbCrLf & "��ָ����Բ������ֹ�Ƕ�")
                ' ������ק�Ĺ������.
                optJigAngle2.Cursor = CursorType.RubberBand
                ' �����϶�������.
                optJigAngle2.BasePoint = mCenterPt
                optJigAngle2.UseBasePoint = True
                ' ��AcquireAngle�����õ��û�����ĽǶ�ֵ.
                Dim resJigAngle2 As PromptDoubleResult = prompts.AcquireAngle(optJigAngle2)
                ang2 = resJigAngle2.Value
                If endAng <> ang2 Then
                    ' ���浱ǰ��Ƕ�ֵ.
                    endAng = ang2
                Else
                    Return SamplerStatus.NoChange
                End If
        End Select
    End Function

    ' Update��������ˢ����Ļ����ʾ��ͼ��.
    Protected Overrides Function Update() As Boolean
        Select Case mPromptCounter
            Case 0
                ' ��һ����קʱ����Բ�İ뾶��Ϊ1����Ļ����ʾ����һ��Բ.
                radiusRatio = 1
                mMajorAxis = mMajorPt - mCenterPt
                startAng = 0
                endAng = 2 * Math.PI
            Case 1
                ' �ڶ�����קʱ���޸�����Բ�İ뾶�ȣ���Ļ����ʾ����һ��������Բ.
                radiusRatio = mRadiusRatio / mMajorAxis.Length
            Case 2
                ' ��������קʱ���޸�����Բ������Ƕȣ���Ļ����ʾ����һ����ֹ�Ƕ�Ϊ360�ȵ���Բ��.
                startAng = ang1
            Case 3
                ' ���Ĵ���קʱ���޸�����Բ����ֹ�Ƕȣ���Ļ����ʾ����һ�����յ���Բ��.
                endAng = ang2
        End Select

        Try
            If radiusRatio < 1 Then
                ' ������Բ�Ĳ���.
                CType(Entity, Ellipse).Set(mCenterPt, mNormal, mMajorAxis, radiusRatio, startAng, endAng)
            Else
                ' ����һ���᳤�ȳ�����Բ�����᷽��ʸ���ĳ��ȣ���Ҫ���¶�����Բ�����᷽��ʸ���ķ���ͳ���.
                Dim mMajorAxis2 As Vector3d = mMajorAxis.RotateBy(0.5 * Math.PI, Vector3d.ZAxis).DivideBy(1 / radiusRatio)
                ' ������Բ�Ĳ���.
                CType(Entity, Ellipse).Set(mCenterPt, mNormal, mMajorAxis2, 1 / radiusRatio, startAng, endAng)
            End If
        Catch
            '�˴�����Ҫ����.
        End Try
        Return True
    End Function

    ' GetEntity�������ڵõ��������ʵ��.
    Public Function GetEntity() As Entity
        Return Entity
    End Function

    ' setPromptCounter�������ڿ��Ʋ�ͬ����ק.
    Public Sub setPromptCounter(ByVal i As Integer)
        mPromptCounter = i
    End Sub
End Class

Public Class EntityJig_Ellipse
    <CommandMethod("JigEllipse")> Public Sub CreateJigEllipse()
        Dim ed As Editor = Application.DocumentManager.MdiActiveDocument.Editor
        Dim db As Database = Application.DocumentManager.MdiActiveDocument.Database
        ' ����ϵͳ������ANGBASE��.
        Dim oldAngBase As Object = Application.GetSystemVariable("ANGBASE")
        ' ��ͨ�ĵ㽻������.
        Dim optPoint As New PromptPointOptions(vbCrLf & "��ָ����Բ����Բ��:")
        Dim resPoint As PromptPointResult = ed.GetPoint(optPoint)
        If resPoint.Status <> PromptStatus.OK Then Return
        ' ����һ��EntityJig�������ʵ��.
        Dim myJig As New EllipseJig(resPoint.Value, Vector3d.ZAxis)
        ' ��һ����ק.
        myJig.setPromptCounter(0)
        Dim resJig As PromptResult = ed.Drag(myJig)
        If resJig.Status = PromptStatus.OK Then
            ' �ڶ�����ק.
            myJig.setPromptCounter(1)
            resJig = ed.Drag(myJig)
            If resJig.Status = PromptStatus.OK Then
                ' ��������ק.
                myJig.setPromptCounter(2)
                resJig = ed.Drag(myJig)
                If resJig.Status = PromptStatus.OK Then
                    ' ���Ĵ���ק.
                    myJig.setPromptCounter(3)
                    resJig = ed.Drag(myJig)
                    If resJig.Status = PromptStatus.OK Then
                        Using trans As Transaction = db.TransactionManager.StartTransaction()
                            Dim bt As BlockTable = trans.GetObject(db.BlockTableId, OpenMode.ForRead)
                            Dim btr As BlockTableRecord = trans.GetObject(bt(BlockTableRecord.ModelSpace), OpenMode.ForWrite)
                            ' ��EntityJig������ʵ����뵽ͼ�����ݿ���.
                            btr.AppendEntity(myJig.GetEntity())
                            trans.AddNewlyCreatedDBObject(myJig.GetEntity(), True)
                            trans.Commit()
                        End Using
                    End If
                End If
            End If
        End If
        ' ��ԭϵͳ������ANGBASE��.
        Application.SetSystemVariable("ANGBASE", oldAngBase)
    End Sub
End Class
