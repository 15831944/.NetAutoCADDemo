using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;

namespace chap16
{
    public class Class1
    {
        // ����ֱ�ߵ�����.
        [CommandMethod("FirstLine")]
        public void testLine()
        {
            Line ent = new Line(new Point3d(30, 40, 0), new Point3d(80, 60, 0));
            Database db = HostApplicationServices.WorkingDatabase;
            using (Transaction ta = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = (BlockTable)ta.GetObject(db.BlockTableId, OpenMode.ForRead);
                BlockTableRecord btr = (BlockTableRecord)ta.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                btr.AppendEntity(ent);
                ta.AddNewlyCreatedDBObject(ent, true);
                ta.Commit();
            }
        }

        // ����ֱ�ߵ�����.
        [CommandMethod("netLine")]
        public void CreateLine()
        {
            ObjectId entId = ModelSpace.AddLine(new Point3d(20, 10, 0), new Point3d(90, 50, 0));
        }

        // ����Բ������.
        [CommandMethod("netCircle")]
        public void CreateCircle()
        {
            ObjectId entId = ModelSpace.AddCircle(new Point3d(20, 10, 0), 50);
        }

        // ����Բ������.
        [CommandMethod("netCircle3P")]
        public void CreateCircle3P()
        {
            ObjectId circle3pId = ModelSpace.AddCircle(new Point2d(0, 0), new Point2d(0, 30), new Point2d(20, 15));
        }

        // ����Բ��������.
        [CommandMethod("netArc")]
        public void CreateArc()
        {
            ObjectId arcId = ModelSpace.AddArc(new Point3d(20, 10, 0), 20, ModelSpace.Rad2Ang(60), ModelSpace.Rad2Ang(180));
        }

        // ������Բ������.
        [CommandMethod("netEllipse")]
        public void CreateEllipse()
        {
            ObjectId ellipseId = ModelSpace.AddEllipse(new Point3d(20, 10, 0), new Vector3d(30, 20, 0), 0.5);
        }

        // �����������ߵ�����.
        [CommandMethod("netSpline")]
        public void CreateSpline()
        {
            Point3d[] pt = new Point3d[4];
            pt[0] = new Point3d(0, 0, 0);
            pt[1] = new Point3d(10, 0, 0);
            pt[2] = new Point3d(30, 20, 0);
            pt[3] = new Point3d(60, 50, 0);
            Point3dCollection pts = new Point3dCollection(pt);
            ObjectId splineId = ModelSpace.AddSpline(pts);
        }

        // ������ά�Ż�����ߵ�����.
        [CommandMethod("netPline")]
        public void CreatePline()
        {
            Point2d[] pt = new Point2d[4];
            pt[0] = new Point2d(0, 0);
            pt[1] = new Point2d(10, 0);
            pt[2] = new Point2d(30, 20);
            pt[3] = new Point2d(-20, 50);
            Point2dCollection pts = new Point2dCollection(pt);
            ObjectId plineId = ModelSpace.AddPline(pts, 0);
        }

        // ������ά����ߵ�����.
        [CommandMethod("net3dPoly")]
        public void Create3dPoly()
        {
            Point3d[] pt = new Point3d[4];
            pt[0] = new Point3d(0, 0, 0);
            pt[1] = new Point3d(10, 0, 50);
            pt[2] = new Point3d(30, 20, 60);
            pt[3] = new Point3d(-30, 50, 70);
            Point3dCollection pts = new Point3dCollection(pt);
            ObjectId poly3dId = ModelSpace.Add3dPoly(pts);
        }

        // �����������ֵ�����.
        [CommandMethod("netText")]
        public void CreateText()
        {
            string textStr = "%%u" + "��������ABC123" + "%%u";
            ObjectId textId = ModelSpace.AddText(new Point3d(20, 10, 0), textStr, 5, 0);
        }

        // �����������ֵ�����.
        [CommandMethod("netMtext")]
        public void CreateMtext()
        {
            string mtextStr = MText.UnderlineOn + "����" + MText.UnderlineOff + MText.OverlineOn + "����" + MText.OverlineOff;
            ObjectId mtextId = ModelSpace.AddMtext(new Point3d(60, 30, 0), mtextStr, 5, 0);
        }

        // ����ͼ����������.
        [CommandMethod("netHatch1")]
        public void CreateHatch1()
        {
            // �������߽�.
            ObjectId loopId1 = ModelSpace.AddLine(new Point3d(100, 0, 0), new Point3d(0, 0, 0));
            ObjectId loopId2 = ModelSpace.AddLine(new Point3d(100, 0, 0), new Point3d(80, 60, 0));
            ObjectId loopId3 = ModelSpace.AddLine(new Point3d(80, 60, 0), new Point3d(0, 0, 0));
            ObjectId loopId4 = ModelSpace.AddCircle(new Point3d(150, 50, 0), 40);

            // ��������ObjectId����.
            ObjectIdCollection loops1 = new ObjectIdCollection();
            loops1.Add(loopId1);
            loops1.Add(loopId2);
            loops1.Add(loopId3);
            ObjectIdCollection loops2 = new ObjectIdCollection();
            loops2.Add(loopId4);

            // ����һ��ObjectId��������.
            ObjectIdCollection[] loops = new ObjectIdCollection[2];
            loops.SetValue(loops1, 0);
            loops.SetValue(loops2, 1);

            // ʵʩ���.
            ObjectId hatchId = ModelSpace.AddHatch(loops, 0, "ANGLE", ModelSpace.Rad2Ang(30), 2);
        }

        // ��������ɫ��������.
        [CommandMethod("netHatch2")]
        public void CreateHatch2()
        {
            // �������߽�.
            ObjectId loopId1 = ModelSpace.AddLine(new Point3d(100, 0, 0), new Point3d(0, 0, 0));
            ObjectId loopId2 = ModelSpace.AddLine(new Point3d(100, 0, 0), new Point3d(80, 60, 0));
            ObjectId loopId3 = ModelSpace.AddLine(new Point3d(80, 60, 0), new Point3d(0, 0, 0));
            ObjectId loopId4 = ModelSpace.AddCircle(new Point3d(150, 50, 0), 40);

            // ��������ObjectId����.
            ObjectIdCollection loops1 = new ObjectIdCollection();
            loops1.Add(loopId1);
            loops1.Add(loopId2);
            loops1.Add(loopId3);
            ObjectIdCollection loops2 = new ObjectIdCollection();
            loops2.Add(loopId4);

            // ����һ��ObjectId��������.
            ObjectIdCollection[] loops = new ObjectIdCollection[2];
            loops.SetValue(loops1, 0);
            loops.SetValue(loops2, 1);

            // ʵʩ���.
            Color c1 = Color.FromRgb(200, 200, 100);
            Color c2 = Color.FromRgb(250, 20, 10);
            ObjectId hatchId = ModelSpace.AddHatch(loops, GradientPatternType.PreDefinedGradient, c1, c2, "LINEAR", ModelSpace.Rad2Ang(30));
        }


        // ������������.
        [CommandMethod("netTable")]
        public void CreateTable()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Table tableEnt = new Table();
            // ������.
            tableEnt.InsertColumns(0, 12, 1);
            tableEnt.InsertColumns(1, 40, 1);
            tableEnt.InsertColumns(2, 40, 1);
            tableEnt.InsertColumns(3, 40, 1);
            tableEnt.InsertColumns(4, 16, 1);
            tableEnt.InsertColumns(5, 30, 1);
            // ������.
            tableEnt.InsertRows(0, 8, 10);
            // �������.
            tableEnt.SetTextString(0, 0, "���");
            tableEnt.SetTextString(0, 1, "��׼��");
            tableEnt.SetTextString(0, 2, "����");
            tableEnt.SetTextString(0, 3, "����");
            tableEnt.SetTextString(0, 4, "����");
            tableEnt.SetTextString(0, 5, "��ע");
            tableEnt.SetTextString(1, 0, "1");
            tableEnt.SetTextString(1, 1, "GB000");
            tableEnt.SetTextString(1, 2, "��ĸM12X50");
            tableEnt.SetTextString(1, 3, "SUS303");
            tableEnt.SetTextString(1, 4, "12");
            tableEnt.Position = new Point3d(180, 80, 0);
            ModelSpace.AppendEntity(tableEnt);
        }

        // �������������.
        [CommandMethod("netRegion1")]
        public void CreateRegion1()
        {
            ObjectId loopId1 = ModelSpace.AddLine(new Point3d(100, 0, 0), new Point3d(0, 0, 0));
            ObjectId loopId2 = ModelSpace.AddLine(new Point3d(100, 0, 0), new Point3d(80, 60, 0));
            ObjectId loopId3 = ModelSpace.AddLine(new Point3d(80, 60, 0), new Point3d(0, 0, 0));
            DBObject ent1;
            DBObject ent2;
            DBObject ent3;

            Database db = HostApplicationServices.WorkingDatabase;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                ent1 = (Entity)trans.GetObject(loopId1, OpenMode.ForWrite);
                ent2 = (Entity)trans.GetObject(loopId2, OpenMode.ForWrite);
                ent3 = (Entity)trans.GetObject(loopId3, OpenMode.ForWrite);
                trans.Commit();
            }

            DBObjectCollection objIds = new DBObjectCollection();
            objIds.Add(ent1);
            objIds.Add(ent2);
            objIds.Add(ent3);
            ObjectIdCollection regionId = ModelSpace.AddRegion(objIds);
        }

        // �������������.
        [CommandMethod("netRegion2")]
        public void CreateRegion2()
        {
            // ���ڴ��д�������ı߽����.
            Line ent1 = new Line(new Point3d(100, 0, 0), new Point3d(0, 0, 0));
            Line ent2 = new Line(new Point3d(100, 0, 0), new Point3d(80, 60, 0));
            Line ent3 = new Line(new Point3d(80, 60, 0), new Point3d(0, 0, 0));
            Circle ent4 = new Circle(new Point3d(200, 50, 0), Vector3d.ZAxis, 60);

            // �߽������ӵ����󼯺�.
            DBObjectCollection ents = new DBObjectCollection();
            ents.Add(ent1);
            ents.Add(ent2);
            ents.Add(ent3);
            ents.Add(ent4);

            // �������򲢼��뵽ͼ�����ݿ�.
            ObjectIdCollection regionIds = ModelSpace.AddRegion(ents);
        }

        // ���������������.
        [CommandMethod("netBox")]
        public void CreateBox()
        {
            ObjectId boxId = ModelSpace.AddBox(new Point3d(300, 200, 100), 600, 400, 300);
        }

        // ����Բ���������.
        [CommandMethod("netCylinder")]
        public void CreateCylinder()
        {
            ObjectId cylinderId = ModelSpace.AddCylinder(new Point3d(300, 200, 100), 600, 400);
        }

        // ����Բ׶�������.
        [CommandMethod("netCone")]
        public void CreateCone()
        {
            ObjectId coneId = ModelSpace.AddCone(new Point3d(300, 200, 100), 600, 400);
        }

        // �������������.
        [CommandMethod("netSphere")]
        public void CreateSphere()
        {
            ObjectId SphereId = ModelSpace.AddSphere(new Point3d(300, 200, 100), 600);
        }

        // ����Բ���������.
        [CommandMethod("netTorus")]
        public void CreateTorus()
        {
            ObjectId torusId = ModelSpace.AddTorus(new Point3d(300, 200, 100), 600, 400);
        }

        // ����Ш�������.
        [CommandMethod("netWedge")]
        public void CreateWedge()
        {
            ObjectId wedgeId = ModelSpace.AddWedge(new Point3d(300, 200, 100), 600, 400, 200);
        }

        // ���������������.
        [CommandMethod("netExt1")]
        public void CreateExtrudedSolid()
        {
            // ���ڴ��д�������������.
            Circle ent = new Circle(new Point3d(200, 100, 0), Vector3d.ZAxis, 100);
            // ���������ӵ����󼯺�.
            DBObjectCollection ents = new DBObjectCollection();
            ents.Add(ent);
            // ���ڴ��д���������󼯺�.
            DBObjectCollection regions = Region.CreateFromCurves(ents);
            // ʵʩ���죬������������ӵ�ͼ�����ݿ�.
            ObjectId extrudedSolidId = ModelSpace.AddExtrudedSolid((Region)regions[0], 500, 0);
        }

        // ���������������.
        [CommandMethod("netExt2")]
        public void CreateExtrudeAlongPath()
        {
            // ���ڴ��д�������������.
            Circle ent = new Circle(new Point3d(200, 0, 0), Vector3d.ZAxis, 100);
            // ���������ӵ����󼯺�.
            DBObjectCollection ents = new DBObjectCollection();
            ents.Add(ent);
            // ���ڴ��д���������󼯺�.
            DBObjectCollection regions = Region.CreateFromCurves(ents);
            // ���ڴ��д�������·������.
            Arc pathEnt = new Arc(new Point3d(500, 0, 0), new Vector3d(0, 1, 0), 300, 0, Math.PI);
            // ʵʩ���죬������������ӵ�ͼ�����ݿ�.
            ObjectId extrudeAlongPathId = ModelSpace.AddExtrudedSolid((Region)regions[0], pathEnt, 0);
        }

        // ������ת�������.
        [CommandMethod("netRevolved")]
        public void CreateRevolvedSolid()
        {
            // ���ڴ��д�����ת�������.
            Circle ent = new Circle(new Point3d(200, 0, 0), Vector3d.ZAxis, 100);
            // ���������ӵ����󼯺�.
            DBObjectCollection ents = new DBObjectCollection();
            ents.Add(ent);
            // ���ڴ��д���������󼯺�.
            DBObjectCollection regions = Region.CreateFromCurves(ents);
            // ʵʩ��ת��������ת����ӵ�ͼ�����ݿ�.
            ObjectId revolvedSolidId = ModelSpace.AddRevolvedSolid((Region)regions[0], new Point3d(300, 200, 100), new Point3d(600, 400, 200), 2 * Math.PI);
        }

        // ����ʾ��������.
        [CommandMethod("netBool")]
        public void CreateBoolSolid()
        {
            // ���ڴ��д�����ת�������.
            Solid3d ent1 = new Solid3d();
            Solid3d ent2 = new Solid3d();
            ent1.CreateBox(100, 60, 40);
            ent2.CreateFrustum(90, 20, 20, 20);
            // �����.
            ent1.BooleanOperation(BooleanOperationType.BoolSubtract,ent2);
            ModelSpace.AppendEntity(ent1);
        }

        [CommandMethod("netDim")]
        public void CreateDimension()
        {
            // ����Ҫ��ע��ͼ��---------------------------------------------
            ModelSpace.AddLine(new Point3d(30, 20, 0), new Point3d(120, 20, 0));
            ModelSpace.AddLine(new Point3d(120, 20, 0), new Point3d(120, 40, 0));
            ModelSpace.AddLine(new Point3d(120, 40, 0), new Point3d(90, 80, 0));
            ModelSpace.AddLine(new Point3d(90, 80, 0), new Point3d(30, 80, 0));
            ModelSpace.AddArc(new Point3d(30, 50, 0), 30, ModelSpace.Rad2Ang(90), ModelSpace.Rad2Ang(270));
            ModelSpace.AddCircle(new Point3d(30, 50, 0), 15);
            ModelSpace.AddCircle(new Point3d(70, 50, 0), 10);

            // �õ���ǰ��ע��ʽ---------------------------------------------
            Database db = HostApplicationServices.WorkingDatabase;
            ObjectId curDimstyle = db.Dimstyle;

            // (ˮƽ)ת�Ǳ�ע-----------------------------------------------
            ModelSpace.AddDimRotated(0, new Point3d(30, 20, 0), new Point3d(120, 20, 0), new Point3d(75, 10, 0));
            // (��ֱ)ת�Ǳ�ע-----------------------------------------------
            ModelSpace.AddDimRotated(ModelSpace.Rad2Ang(90), new Point3d(120, 20, 0), new Point3d(120, 40, 0), new Point3d(130, 30, 0));

            // �����ע���ߴ����-------------------------------------------
            ModelSpace.AddDimAligned(new Point3d(120, 40, 0), new Point3d(90, 80, 0), new Point3d(113, 66, 0), "50%%p0.2", curDimstyle);

            // �뾶��ע-----------------------------------------------------
            Point3d ptCen = new Point3d(30, 50, 0);
            Point3d p2 = ModelSpace.PolarPoint(ptCen, ModelSpace.Rad2Ang(30), 15);
            ModelSpace.AddDimRadial(ptCen, p2, 10);

            // ֱ����ע-----------------------------------------------------
            Point3d dcen = new Point3d(70, 50, 0);
            Point3d ptChord1 = ModelSpace.PolarPoint(dcen, ModelSpace.Rad2Ang(45), 10);
            Point3d ptChord2 = ModelSpace.PolarPoint(dcen, ModelSpace.Rad2Ang(-135), 10);
            ModelSpace.AddDimDiametric(ptChord1, ptChord2, 0);

            // �Ƕȱ�ע-----------------------------------------------------
            Point3d angPtCen = new Point3d(120, 20, 0);
            Point3d p5 = ModelSpace.PolarPoint(angPtCen, ModelSpace.Rad2Ang(135), 10);
            ModelSpace.AddDimLineAngular(angPtCen, new Point3d(30, 20, 0), new Point3d(120, 40, 0), p5);

            // ������ע-----------------------------------------------------
            ModelSpace.AddDimArc(new Point3d(30, 50, 0), new Point3d(30, 20, 0), new Point3d(30, 80, 0), new Point3d(-10, 50, 0));

            // �����ע-----------------------------------------------------
            ModelSpace.AddDimOrdinate(new Point3d(70, 50, 0), new Point3d(70, 30, 0), new Point3d(90, 50, 0));

            // ���߱�ע-----------------------------------------------------
            Point3dCollection pts = new Point3dCollection();
            pts.Add(new Point3d(90, 70, 0));
            pts.Add(new Point3d(110, 80, 0));
            pts.Add(new Point3d(120, 80, 0));
            ModelSpace.AddLeader(pts, false);
            // ������߱�ע������.
            ModelSpace.AddMtext(new Point3d(120, 80, 0), "{\\L���߱�עʾ��\\l}", curDimstyle, AttachmentPoint.BottomLeft, 2.5, 0);

            // �ߴ繫���ע--------------------------------------------------
            ModelSpace.AddDimRotated(0, new Point3d(30, 80, 0), new Point3d(90, 80, 0), new Point3d(30, 90, 0), "60{\\H0.7x;\\S+0.026^-0.025;}", curDimstyle);

            // ��λ�����ע--------------------------------------------------
            string dimText = "{\\fgdt;r}" + "%%v" + "{\\fgdt;n0.03}" + "%%v" + "B";
            ModelSpace.AddTolerance(dimText, new Point3d(80, 100, 0), new Vector3d(0, 0, 1), new Vector3d(1, 0, 0));
            // Ϊ��λ�����ע�������.
            Point3dCollection ptss = new Point3dCollection();
            ptss.Add(new Point3d(70, 80, 0));
            ptss.Add(new Point3d(70, 100, 0));
            ptss.Add(new Point3d(80, 100, 0));
            ModelSpace.AddLeader(ptss, false);
        }
    }
}