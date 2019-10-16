using System;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.GraphicsInterface;
using Autodesk.AutoCAD.Runtime;

namespace chap22
{
    public class EllipseJig : EntityJig
    {
        // ����ȫ�ֱ���.
        private Point3d mCenterPt, mMajorPt;
        private Vector3d mNormal, mMajorAxis;
        private int mPromptCounter;
        private double mRadiusRatio, radiusRatio;
        private double startAng, endAng, ang1, ang2;

        // ������Ĺ��캯��.
        public EllipseJig(Point3d center, Vector3d vec)
            : base(new Ellipse())
        {
            mCenterPt = center;
            mNormal = vec;
        }

        // Sampler�������ڼ���û�������.
        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            if (mPromptCounter == 0)
            {
                // ����һ�����϶�������.
                JigPromptPointOptions optJigPoint = new JigPromptPointOptions("\n��ָ����Բ������һ��");
                // ������ק�Ĺ������.
                optJigPoint.Cursor = CursorType.RubberBand;
                // �����϶�������.
                optJigPoint.BasePoint = mCenterPt;
                optJigPoint.UseBasePoint = true;
                // ��AcquirePoint�����õ��û�����ĵ�.
                PromptPointResult resJigPoint = prompts.AcquirePoint(optJigPoint);
                Point3d curPt = resJigPoint.Value;
                if (curPt != mMajorPt)
                    // 
                    mMajorPt = curPt;
                else
                    return SamplerStatus.NoChange;
                if (resJigPoint.Status == PromptStatus.Cancel)
                    return SamplerStatus.Cancel;
                else
                    return SamplerStatus.OK;
            }
            else if (mPromptCounter == 1)
            {
                // ����һ�������϶�������.
                JigPromptDistanceOptions optJigDis = new JigPromptDistanceOptions("\n��ָ����һ������ĳ���");
                // ���ö���ק��Լ��:��ֹ������͸�ֵ.
                optJigDis.UserInputControls = UserInputControls.NoZeroResponseAccepted | UserInputControls.NoNegativeResponseAccepted;
                // ������ק�Ĺ������.
                optJigDis.Cursor = CursorType.RubberBand;
                // �����϶�������.
                optJigDis.BasePoint = mCenterPt;
                optJigDis.UseBasePoint = true;
                // ��AcquireDistance�����õ��û�����ľ���ֵ.
                PromptDoubleResult resJigDis = prompts.AcquireDistance(optJigDis);
                double mRadiusRatioTemp = resJigDis.Value;
                if (mRadiusRatioTemp != mRadiusRatio)
                    // ���浱ǰ����ֵ.
                    mRadiusRatio = mRadiusRatioTemp;
                else
                    return SamplerStatus.NoChange;
                if (resJigDis.Status == PromptStatus.Cancel)
                    return SamplerStatus.Cancel;
                else
                    return SamplerStatus.OK;
            }
            else if (mPromptCounter == 2)
            {
                // ������Բ��0�Ȼ�׼��.
                double baseAng;
                Vector2d mMajorAxis2d = new Vector2d(mMajorAxis.X, mMajorAxis.Y);
                if (radiusRatio < 1)
                    baseAng = mMajorAxis2d.Angle;
                else
                    baseAng = mMajorAxis2d.Angle + 0.5 * Math.PI;
                // ����ϵͳ������ANGBASE��.
                Application.SetSystemVariable("ANGBASE", baseAng);
                // ����һ���Ƕ��϶�������.
                JigPromptAngleOptions optJigAngle1 = new JigPromptAngleOptions("\n��ָ����Բ������ʼ�Ƕ�");
                // ������ק�Ĺ������.
                optJigAngle1.Cursor = CursorType.RubberBand;
                // �����϶�������.
                optJigAngle1.BasePoint = mCenterPt;
                optJigAngle1.UseBasePoint = true;
                // ��AcquireAngle�����õ��û�����ĽǶ�ֵ.
                PromptDoubleResult resJigAngle1 = prompts.AcquireAngle(optJigAngle1);
                ang1 = resJigAngle1.Value;
                if (startAng != ang1)
                    // ���浱ǰ�Ƕ�ֵ.
                    startAng = ang1;
                else
                    return SamplerStatus.NoChange;
                if (resJigAngle1.Status == PromptStatus.Cancel)
                    return SamplerStatus.Cancel;
                else
                    return SamplerStatus.OK;
            }
            else if (mPromptCounter == 3)
            {
                // ����һ���Ƕ��϶�������.
                JigPromptAngleOptions optJigAngle2 = new JigPromptAngleOptions("\n��ָ����Բ������ֹ�Ƕ�");
                // ������ק�Ĺ������.
                optJigAngle2.Cursor = CursorType.RubberBand;
                // �����϶�������.
                optJigAngle2.BasePoint = mCenterPt;
                optJigAngle2.UseBasePoint = true;
                // ��AcquireAngle�����õ��û�����ĽǶ�ֵ.
                PromptDoubleResult resJigAngle2 = prompts.AcquireAngle(optJigAngle2);
                ang2 = resJigAngle2.Value;
                if (endAng != ang2)
                    // ���浱ǰ�Ƕ�ֵ.
                    endAng = ang2;
                else
                    return SamplerStatus.NoChange;
                if (resJigAngle2.Status == PromptStatus.Cancel)
                    return SamplerStatus.Cancel;
                else
                    return SamplerStatus.OK;
            }
            else
                return SamplerStatus.NoChange;
        }

        // Update��������ˢ����Ļ����ʾ��ͼ��.
        protected override bool Update()
        {
            if (mPromptCounter == 0)
            {
                // ��һ����קʱ����Բ�İ뾶��Ϊ1����Ļ����ʾ����һ��Բ.
                radiusRatio = 1;
                mMajorAxis = mMajorPt - mCenterPt;
                startAng = 0;
                endAng = 2 * Math.PI;
            }
            else if (mPromptCounter == 1)
                // �ڶ�����קʱ���޸�����Բ�İ뾶�ȣ���Ļ����ʾ����һ��������Բ.
                radiusRatio = mRadiusRatio / mMajorAxis.Length;
            else if (mPromptCounter == 2)
                // ��������קʱ���޸�����Բ������Ƕȣ���Ļ����ʾ����һ����ֹ�Ƕ�Ϊ360�ȵ���Բ��.
                startAng = ang1;
            else if (mPromptCounter == 3)
                // ���Ĵ���קʱ���޸�����Բ����ֹ�Ƕȣ���Ļ����ʾ����һ�����յ���Բ��.
                endAng = ang2;
            try
            {
                if (radiusRatio < 1)
                    // ������Բ�Ĳ���.
                    ((Ellipse)(Entity)).Set(mCenterPt, mNormal, mMajorAxis, radiusRatio, startAng, endAng);
                else
                {
                    // ����һ�����᳤�ȳ�����Բ�����᷽��ʸ���ĳ��ȣ���Ҫ���¶�����Բ�����᷽��ʸ���ķ���ͳ���.
                    Vector3d mMajorAxis2 = mMajorAxis.RotateBy(0.5 * Math.PI, Vector3d.ZAxis).DivideBy(1 / radiusRatio);
                    // ������Բ�Ĳ���.
                    ((Ellipse)(Entity)).Set(mCenterPt, mNormal, mMajorAxis2, 1 / radiusRatio, startAng, endAng);
                }
            }
            catch
            {
                // '�˴�����Ҫ����.
            }
            return true;
        }

        // GetEntity�������ڵõ��������ʵ��.
        public Entity GetEntity()
        {
            return Entity;
        }

        // setPromptCounter�������ڿ��Ʋ�ͬ����ק.
        public void setPromptCounter(int i)
        {
            mPromptCounter = i;
        }
    }

    public class EntityJig_Ellipse
    {
        [CommandMethod("JigEllipse")]
        public void CreateJigEllipse()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Database db = Application.DocumentManager.MdiActiveDocument.Database;
            // ����ϵͳ������ANGBASE��.
            object oldAngBase = Application.GetSystemVariable("ANGBASE");
            // ��ͨ�ĵ㽻������.
            PromptPointOptions optPoint = new PromptPointOptions("\n��ָ����Բ����Բ��:");
            PromptPointResult resPoint = ed.GetPoint(optPoint);
            if (resPoint.Status != PromptStatus.OK)
                return;
            // ����һ��EntityJig�������ʵ��.
            EllipseJig myJig = new EllipseJig(resPoint.Value, Vector3d.ZAxis);
            // ��һ����ק.
            myJig.setPromptCounter(0);
            PromptResult resJig = ed.Drag(myJig);
            if (resJig.Status == PromptStatus.OK)
            {
                // �ڶ�����ק.
                myJig.setPromptCounter(1);
                resJig = ed.Drag(myJig);
                if (resJig.Status == PromptStatus.OK)
                {
                    // ��������ק.
                    myJig.setPromptCounter(2);
                    resJig = ed.Drag(myJig);
                    if (resJig.Status == PromptStatus.OK)
                    {
                        // ���Ĵ���ק.
                        myJig.setPromptCounter(3);
                        resJig = ed.Drag(myJig);
                        if (resJig.Status == PromptStatus.OK)
                        {
                            using (Transaction trans = db.TransactionManager.StartTransaction())
                            {
                                BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                                BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                                btr.AppendEntity(myJig.GetEntity());
                                trans.AddNewlyCreatedDBObject(myJig.GetEntity(), true);
                                trans.Commit();
                            }
                        }
                    }
                }
            }
            // ��ԭϵͳ������ANGBASE��.
            Application.SetSystemVariable("ANGBASE", oldAngBase);
        }
    }
}
