using System;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.GraphicsInterface;
using Autodesk.AutoCAD.Runtime;

namespace chap22
{
    public class drawJig_start : DrawJig
    {
        // ��������Ƕ���.
        private Polyline ent;
        // ��������ǵ����ĺ�һ������.
        private Point3d mCenterPt, peakPt;
        [CommandMethod("FiveStart")]
        public void CreateDrawJigFiveStart()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            // ��ͨ�ĵ㽻������.
            PromptPointOptions optPoint = new PromptPointOptions("\n��ָ������ǵ�����");
            PromptPointResult resPoint = ed.GetPoint(optPoint);
            if (resPoint.Status != PromptStatus.OK)
                return;
            mCenterPt = resPoint.Value;

            // ���ڴ��д���һ������10������ķ�ն���߶���.
            Point2d[] pt = new Point2d[10];
            pt[0] = new Point2d(0, 0);
            pt[1] = new Point2d(0, 0);
            pt[2] = new Point2d(0, 0);
            pt[3] = new Point2d(0, 0);
            pt[4] = new Point2d(0, 0);
            pt[5] = new Point2d(0, 0);
            pt[6] = new Point2d(0, 0);
            pt[7] = new Point2d(0, 0);
            pt[8] = new Point2d(0, 0);
            pt[9] = new Point2d(0, 0);
            Point2dCollection pts = new Point2dCollection(pt);
            ent = (Polyline)new Polyline();
            for (int i = 0; i <= 9; i++)
                ent.AddVertexAt(i, pts[i], 0, 0, 0);
            ent.Closed = true;

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                // ��ʼ��ק.
                PromptResult resJig = ed.Drag(this);
                if (resJig.Status == PromptStatus.OK)
                {
                    // ������Ƕ�����뵽ͼ�����ݿ���.
                    btr.AppendEntity(ent);
                    trans.AddNewlyCreatedDBObject(ent, true);
                    trans.Commit();
                }
            }
        }

        // Sampler�������ڼ���û�������.
        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            Database db = HostApplicationServices.WorkingDatabase;
            // ����һ�����϶�������.
            JigPromptPointOptions optJigPoint = new JigPromptPointOptions("\n��ָ������ǵ�һ���ǵ�:");
            // ������ק�������.
            optJigPoint.Cursor = CursorType.RubberBand;
            // �����϶�������.
            optJigPoint.BasePoint = mCenterPt;
            optJigPoint.UseBasePoint = true;
            // ��AcquirePoint�����õ��û�����ĵ�.
            PromptPointResult resJigPoint1 = prompts.AcquirePoint(optJigPoint);
            Point3d curPt = resJigPoint1.Value;
            if (curPt != peakPt)
            {
                // ����������Բ����--------------------------------------------.
                // ����ǵ�����.
                Point2d p0 = new Point2d(mCenterPt.X, mCenterPt.Y);

                // ��������ǵĵ�һ����������.
                Point2d p1 = new Point2d(curPt[0], curPt[1]);

                // Ϊ��������9��������������׼��.
                double d1 = p1.GetDistanceTo(p0);
                double d2 = d1 * Math.Sin(Rad2Ang(18)) / Math.Sin(Rad2Ang(54));
                Vector2d vec = p1 - p0;
                double ang = vec.Angle;

                // �������������9�����������.
                Point2d p2 = PolarPoint(p0, ang + Rad2Ang(36), d2);
                Point2d p3 = PolarPoint(p0, ang + Rad2Ang(72), d1);
                Point2d p4 = PolarPoint(p0, ang + Rad2Ang(108), d2);
                Point2d p5 = PolarPoint(p0, ang + Rad2Ang(144), d1);
                Point2d p6 = PolarPoint(p0, ang + Rad2Ang(180), d2);
                Point2d p7 = PolarPoint(p0, ang + Rad2Ang(216), d1);
                Point2d p8 = PolarPoint(p0, ang + Rad2Ang(252), d2);
                Point2d p9 = PolarPoint(p0, ang + Rad2Ang(288), d1);
                Point2d p10 = PolarPoint(p0, ang + Rad2Ang(324), d2);

                // ��������Ǹ������������.
                ent.SetPointAt(0, p1);
                ent.SetPointAt(1, p2);
                ent.SetPointAt(2, p3);
                ent.SetPointAt(3, p4);
                ent.SetPointAt(4, p5);
                ent.SetPointAt(5, p6);
                ent.SetPointAt(6, p7);
                ent.SetPointAt(7, p8);
                ent.SetPointAt(8, p9);
                ent.SetPointAt(9, p10);
                peakPt = curPt;
                return SamplerStatus.OK;
            }
            else
                return SamplerStatus.NoChange;
        }

        // WorldDraw��������ˢ����Ļ����ʾ��ͼ��.
        protected override bool WorldDraw(WorldDraw draw)
        {
            // ˢ�»���.
            draw.Geometry.Draw(ent);
            return true;
        }

        // ��ȡ�������ָ���ǶȺ;���ĵ�.
        public Point2d PolarPoint(Point2d basePt, double angle, double distance)
        {
            double[] pt = new double[2];
            pt[0] = basePt[0] + distance * Math.Cos(angle);
            pt[1] = basePt[1] + distance * Math.Sin(angle);
            Point2d point = new Point2d(pt[0], pt[1]);
            return point;
        }
        // �Ȼ����ȵĺ���.
        public double Rad2Ang(double angle)
        {
            double rad = angle * Math.PI / 180;
            return rad;
        }
    }
}
