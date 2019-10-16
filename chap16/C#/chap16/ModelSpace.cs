using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;

namespace chap16
{
    class ModelSpace
    {
        // �����㴴��ֱ�ߵĺ���.
        public static ObjectId AddLine(Point3d pt1, Point3d pt2)
        {
            Line ent = new Line(pt1, pt2);
            ObjectId entId = AppendEntity(ent);
            return entId;
        }

        // ��Բ�ĺͰ뾶����Բ�ĺ���.
        public static ObjectId AddCircle(Point3d cenPt, double radius)
        {
            Circle ent = new Circle(cenPt, Vector3d.ZAxis, radius);
            ObjectId entId = AppendEntity(ent);
            return entId;
        }

        // �ɸ���Բ�����㴴��Բ�ĺ���.
        public static ObjectId AddCircle(Point2d pt1, Point2d pt2, Point2d pt3)
        {
            const double pi = Math.PI;
            Vector2d va = pt1.GetVectorTo(pt2);
            Vector2d vb = pt1.GetVectorTo(pt3);
            if (va.GetAngleTo(vb) == 0 | va.GetAngleTo(vb) == pi)
            {
                ObjectId nullId = ObjectId.Null;
                return nullId;
            }
            else
            {
                CircularArc2d geoArc = new CircularArc2d(pt1, pt2, pt3);
                Point3d cenPt = new Point3d(geoArc.Center.X, geoArc.Center.Y, 0);
                double radius = geoArc.Radius;
                Circle ent = new Circle(cenPt, Vector3d.ZAxis, radius);
                ObjectId entId = AppendEntity(ent);
                return entId;
            }
        }

        // ��Բ�ġ��뾶����ʼ�ǶȺ���ֹ�Ƕȴ���Բ���ĺ���.
        public static ObjectId AddArc(Point3d cenPt, double radius, double startAng, double endAng)
        {
            Arc ent = new Arc(cenPt, radius, startAng, endAng);
            ObjectId entId = AppendEntity(ent);
            return entId;
        }

        //����Բ���ġ��볤�᷽��ʸ���Ͷ̳���뾶�ȴ�����Բ�ĺ���.
        public static ObjectId AddEllipse(Point3d cenPt, Vector3d majorAxis, double radiusRatio)
        {
            Ellipse ent = new Ellipse(cenPt, Vector3d.ZAxis, majorAxis, radiusRatio, 0, 2 * Math.PI);
            ObjectId entId = AppendEntity(ent);
            return entId;
        }

        // ����ά�㼯�ϴ����������ߵĺ���.
        public static ObjectId AddSpline(Point3dCollection pts)
        {
            Spline ent = new Spline(pts, 4, 0);
            ObjectId entId = AppendEntity(ent);
            return entId;
        }

        // �ɶ�ά�㼯�Ϻ��߿�����ά�Ż�����ߵĺ���.
        public static ObjectId AddPline(Point2dCollection pts, double width)
        {
            try
            {
                int n = pts.Count;
                Polyline ent = new Polyline();
                for (int i = 0; i < n; i++)
                    ent.AddVertexAt(i, pts[i], 0, width, width);
                ObjectId entId = AppendEntity(ent);
                return entId;
            }
            catch
            {
                ObjectId nullId = ObjectId.Null;
                return nullId;
            }
        }

        // ����ά�㼯�ϴ�����ά����ߵĺ���.
        public static ObjectId Add3dPoly(Point3dCollection pts)
        {
            try
            {
                Polyline3d ent = new Polyline3d(Poly3dType.SimplePoly, pts, false);
                ObjectId entId = AppendEntity(ent);
                return entId;
            }
            catch
            {
                ObjectId nullId = ObjectId.Null;
                return nullId;
            }
        }

        // �ɲ���㡢�������ݡ����ָ߶Ⱥ���б�Ƕȴ����������ֵĺ���.
        public static ObjectId AddText(Point3d position, string textString, double height, double oblique)
        {
            try
            {
                DBText ent = new DBText();
                ent.Position = position;
                ent.TextString = textString;
                ent.Height = height;
                ent.Oblique = oblique;
                ObjectId entId = AppendEntity(ent);
                return entId;
            }
            catch
            {
                ObjectId nullId = ObjectId.Null;
                return nullId;
            }
        }

        // �ɲ���㡢�������ݡ�������ʽ�����ָ߶ȡ���б�ǶȺ���ת�Ƕȴ����������ֵĺ���.
        public static ObjectId AddText(Point3d position, string textString, ObjectId style, double height, double oblique, double rotation)
        {
            try
            {
                DBText ent = new DBText();
                ent.Position = position;
                ent.TextString = textString;
                ent.Height = height;
                ent.Oblique = oblique;
                ent.Rotation = rotation;
                ObjectId entId = AppendEntity(ent);
                return entId;
            }
            catch
            {
                ObjectId nullId = ObjectId.Null;
                return nullId;
            }
        }

        // �ɲ���㡢�������ݡ����ָ߶ȡ��ı����ȴ����������ֵĺ���.
        public static ObjectId AddMtext(Point3d location, string textString, double height, double width)
        {
            try
            {
                MText ent = new MText();
                ent.Location = location;
                ent.Contents = textString;
                ent.TextHeight = height;
                ent.Width = width;
                ObjectId entId = AppendEntity(ent);
                return entId;
            }
            catch
            {
                ObjectId nullId = ObjectId.Null;
                return nullId;
            }
        }

        // �ɲ���㡢�������ݡ�������ʽ�����뷽ʽ�����ָ߶ȡ����ֿ�ȴ����������ֵĺ���.
        public static ObjectId AddMtext(Point3d location, string textString, ObjectId style, AttachmentPoint attachmentPoint, double height, double width)
        {
            try
            {
                MText ent = new MText();
                ent.Location = location;
                ent.Contents = textString;
                ent.Attachment = attachmentPoint;
                ent.TextHeight = height;
                ent.Width = width;
                ObjectId entId = AppendEntity(ent);
                return entId;
            }
            catch
            {
                ObjectId nullId = ObjectId.Null;
                return nullId;
            }
        }

        // �ɱ߽���󼯺����顢ͼ��������͡����ͼ�����ơ����ǶȺ�����������ͼ�����ĺ���.
        // partType:0ΪԤ����ͼ����1Ϊ�û�����ͼ����2Ϊ�Զ���ͼ��.
        public static ObjectId AddHatch(ObjectIdCollection[] objIds, HatchPatternType patType, string patName, double patternAngle, double patternScale)
        {
            try
            {
                Hatch ent = new Hatch();
                ent.HatchObjectType = HatchObjectType.HatchObject;
                Database db = HostApplicationServices.WorkingDatabase;
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                    BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                    ObjectId entId = btr.AppendEntity(ent);
                    trans.AddNewlyCreatedDBObject(ent, true);
                    ent.PatternAngle = patternAngle;
                    ent.PatternScale = patternScale;
                    ent.SetHatchPattern(patType, patName);
                    ent.Associative = true;
                    for (int i = 0; i < objIds.Length; i++)
                        ent.InsertLoopAt(i, HatchLoopTypes.Default, objIds[i]);
                    trans.Commit();
                    return entId;
                }
            }
            catch
            {
                ObjectId nullId = ObjectId.Null;
                return nullId;
            }
        }
        // �ɱ߽���󼯺����顢����ɫ������͡�����������ʼ��ɫ������������ֹ��ɫ���������ͼ�����ƺ����Ƕȴ�������ɫ���ĺ���.
        // gradientType: 0ΪԤ����ͼ����1Ϊ�û�����ͼ��.
        // gradientName: "LINEAR"(ֱ���Σ�, "CYLINDER"(Բ����), "INVCYLINDER"(��תԲ����), "SPHERICAL"(����), "HEMISPHERICAL"(������), "CURVED"(������), "INVSPHERICAL"(��ת����), "INVHEMISPHERICAL"(��ת������), "INVCURVED"(��ת������)
        public static ObjectId AddHatch(ObjectIdCollection[] objIds, GradientPatternType gradientType, Color hColor1, Color hColor2, string gradientName, double gradientAngle)
        {
            try
            {
                Hatch ent = new Hatch();
                ent.HatchObjectType = HatchObjectType.GradientObject;
                Database db = HostApplicationServices.WorkingDatabase;
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                    BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                    ObjectId entId = btr.AppendEntity(ent);
                    trans.AddNewlyCreatedDBObject(ent, true);
                    GradientColor gColor0 = new GradientColor(hColor1, 0);
                    GradientColor gColor1 = new GradientColor(hColor2, 1);
                    GradientColor[] gColor = new GradientColor[2] { gColor0, gColor1 };
                    ent.SetGradientColors(gColor);
                    ent.SetGradient(gradientType, gradientName);
                    ent.GradientAngle = gradientAngle;
                    ent.Associative = true;
                    for (int i = 0; i < objIds.Length; i++)
                        ent.InsertLoopAt(i, HatchLoopTypes.Default, objIds[i]);
                    trans.Commit();
                    return entId;
                }
            }
            catch
            {
                ObjectId nullId = ObjectId.Null;
                return nullId;
            }
        }

        // ��ͼ�ζ��󼯺ϴ�������ĺ���.
        public static ObjectIdCollection AddRegion(DBObjectCollection ents)
        {
            try
            {
                DBObjectCollection regions = Region.CreateFromCurves(ents);
                ObjectIdCollection entIds = new ObjectIdCollection();
                for (int i = 0; i < regions.Count; i++)
                {
                    ObjectId entId = AppendEntity((Entity)regions[i]);
                    entIds.Add(entId);
                }
                return entIds;
            }
            catch
            {
                ObjectId nullId = ObjectId.Null;
                ObjectIdCollection nullIds = new ObjectIdCollection();
                nullIds.Add(nullId);
                return nullIds;
            }
        }

        // ��ͼ�ζ���ObjectId���ϴ�������ĺ���.
        public static ObjectIdCollection AddRegion(ObjectIdCollection ids)
        {
            try
            {
                Database db = HostApplicationServices.WorkingDatabase;
                Entity ent;
                DBObjectCollection ents = new DBObjectCollection();
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    for (int i = 0; i < ids.Count; i++)
                    {
                        ent = (Entity)trans.GetObject(ids[i], OpenMode.ForWrite);
                        ents.Add(ent);
                    }
                }
                DBObjectCollection regions = Region.CreateFromCurves(ents);
                ObjectIdCollection entIds = new ObjectIdCollection();
                for (int i = 0; i < regions.Count; i++)
                {
                    ObjectId entId = AppendEntity((Entity)regions[i]);
                    entIds.Add(entId);
                }
                return entIds;
            }
            catch
            {
                ObjectId nullId = ObjectId.Null;
                ObjectIdCollection nullIds = new ObjectIdCollection();
                nullIds.Add(nullId);
                return nullIds;
            }
        }

        // �����ĵ㡢���ȡ���Ⱥ͸߶ȴ���������ĺ���.
        public static ObjectId AddBox(Point3d cenPt, double lengthAlongX, double lengthAlongY, double lengthAlongZ)
        {
            Solid3d ent = new Solid3d();
            ent.CreateBox(lengthAlongX, lengthAlongY, lengthAlongZ);
            Matrix3d mt = Matrix3d.Displacement(cenPt - Point3d.Origin);
            ent.TransformBy(mt);
            ObjectId entId = AppendEntity(ent);
            return entId;
        }

        // �����ĵ㡢�뾶�͸߶ȴ���Բ����ĺ���.
        public static ObjectId AddCylinder(Point3d cenPt, double radius, double height)
        {
            Solid3d ent = new Solid3d();
            ent.CreateFrustum(height, radius, radius, radius);
            Matrix3d mt = Matrix3d.Displacement(cenPt - Point3d.Origin);
            ent.TransformBy(mt);
            ObjectId entId = AppendEntity(ent);
            return entId;
        }

        // �����ĵ㡢�뾶�͸߶ȴ���Բ׶��ĺ���.
        public static ObjectId AddCone(Point3d cenPt, double radius, double height)
        {
            Solid3d ent = new Solid3d();
            ent.CreateFrustum(height, radius, radius, 0);
            Matrix3d mt = Matrix3d.Displacement(cenPt - Point3d.Origin);
            ent.TransformBy(mt);
            ObjectId entId = AppendEntity(ent);
            return entId;
        }

        // �����ĵ�Ͱ뾶��������ĺ���.
        public static ObjectId AddSphere(Point3d cenPt, double radius)
        {
            Solid3d ent = new Solid3d();
            ent.CreateSphere(radius);
            Matrix3d mt = Matrix3d.Displacement(cenPt - Point3d.Origin);
            ent.TransformBy(mt);
            ObjectId entId = AppendEntity(ent);
            return entId;
        }

        // �����ĵ㡢Բ���뾶��Բ�ܰ뾶����Բ����ĺ���.
        public static ObjectId AddTorus(Point3d cenPt, double majorRadius, double minorRadius)
        {
            Solid3d ent = new Solid3d();
            ent.CreateTorus(majorRadius, minorRadius);
            Matrix3d mt = Matrix3d.Displacement(cenPt - Point3d.Origin);
            ent.TransformBy(mt);
            ObjectId entId = AppendEntity(ent);
            return entId;
        }

        // �����ĵ㡢���ȡ���Ⱥ͸߶ȴ���Ш��ĺ���.
        public static ObjectId AddWedge(Point3d cenPt, double lengthAlongX, double lengthAlongY, double lengthAlongZ)
        {
            Solid3d ent = new Solid3d();
            ent.CreateWedge(lengthAlongX, lengthAlongY, lengthAlongZ);
            Matrix3d mt = Matrix3d.Displacement(cenPt - Point3d.Origin);
            ent.TransformBy(mt);
            ObjectId entId = AppendEntity(ent);
            return entId;
        }

        // �ɽ�����������߶Ⱥ�����Ƕȴ���������ĺ���.
        public static ObjectId AddExtrudedSolid(Region region, double height, double taperAngle)
        {
            try
            {
                Solid3d ent = new Solid3d();
                ent.Extrude(region, height, taperAngle);
                ObjectId entId = AppendEntity(ent);
                return entId;
            }
            catch
            {
                ObjectId nullId = ObjectId.Null;
                return nullId;
            }
        }

        // �ɽ�����������·�����ߺ�����Ƕȴ���������ĺ���.
        public static ObjectId AddExtrudedSolid(Region region, Curve path, double taperAngle)
        {
            try
            {
                Solid3d ent = new Solid3d();
                ent.ExtrudeAlongPath(region, path, taperAngle);
                ObjectId entId = AppendEntity(ent);
                return entId;
            }
            catch
            {
                ObjectId nullId = ObjectId.Null;
                return nullId;
            }
        }

        // �ɽ���������ת����㡢��ת���յ����ת�Ƕȴ�����ת��ĺ���.
        public static ObjectId AddRevolvedSolid(Region region, Point3d axisPt1, Point3d axisPt2, double angle)
        {
            try
            {
                Solid3d ent = new Solid3d();
                ent.Revolve(region, axisPt1, axisPt2 - axisPt1, angle);
                ObjectId entId = AppendEntity(ent);
                return entId;
            }
            catch
            {
                ObjectId nullId = ObjectId.Null;
                return nullId;
            }
        }

        // �ɲ����������ͺ�������άʵ�崴����ת��ĺ���.  
        public static void AddBoolSolid(BooleanOperationType boolType, ObjectId solid3dId1, ObjectId solid3dId2)
        {
            Database db = HostApplicationServices.WorkingDatabase;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    Entity ent1 = (Entity)trans.GetObject(solid3dId1, OpenMode.ForWrite);
                    Entity ent2 = (Entity)trans.GetObject(solid3dId2, OpenMode.ForWrite);
                    if (ent1 is Solid3d & ent2 is Solid3d)
                    {
                        Solid3d solid3dEnt1 = (Solid3d)ent1;
                        Solid3d solid3dEnt2 = (Solid3d)ent2;
                        solid3dEnt1.BooleanOperation(boolType, solid3dEnt2);
                        ent2.Erase();
                    }
                    if (ent1 is Region & ent2 is Region)
                    {
                        Region regionEnt1 = (Region)ent1;
                        Region regionEnt2 = (Region)ent2;
                        regionEnt1.BooleanOperation(boolType, regionEnt2);
                        ent2.Erase();
                    }
                }
                catch
                {
                    // �˴�����Ҫ����.
                }
                trans.Commit();
            }
        }

        // �ɳߴ�����ת�Ƕȡ������ߴ����ԭ��ͳߴ��ı�λ�ô���ת�Ǳ�ע�ĺ���.
        public static ObjectId AddDimRotated(double angle, Point3d pt1, Point3d pt2, Point3d ptText)
        {
            Database db = HostApplicationServices.WorkingDatabase;
            ObjectId style = db.Dimstyle;

            Point2d p2dt1 = new Point2d(pt1.X, pt1.Y);
            Point2d p2dt2 = new Point2d(pt2.X, pt2.Y);
            Vector2d vec = p2dt2 - p2dt1;

            string text = Math.Round(Math.Abs(vec.Length*Math.Cos(vec.Angle-angle)),db.Dimdec).ToString();
            RotatedDimension ent = new RotatedDimension(angle, pt1, pt2, ptText, text, style);
            ObjectId entId = AppendEntity(ent);
            return entId;
        }

        // �ɳߴ�����ת�Ƕȡ������ߴ����ԭ�㡢�ߴ��ı�λ�á��ߴ��ı��ͱ�ע��ʽ����ת�Ǳ�ע�ĺ���.
        public static ObjectId AddDimRotated(double angle, Point3d pt1, Point3d pt2, Point3d ptText, string text, ObjectId style)
        {
            RotatedDimension ent = new RotatedDimension(angle, pt1, pt2, ptText, text, style);
            ObjectId entId = AppendEntity(ent);
            return entId;
        }

        // �������ߴ����ԭ��ͳߴ��ı�λ�ô��������ע�ĺ���.
        public static ObjectId AddDimAligned(Point3d pt1, Point3d pt2, Point3d ptText)
        {
            Database db = HostApplicationServices.WorkingDatabase;
            ObjectId style = db.Dimstyle;
            string text = Math.Round(pt1.DistanceTo(pt2),db.Dimdec).ToString();
            AlignedDimension ent = new AlignedDimension(pt1, pt2, ptText, text, style);
            ObjectId entId = AppendEntity(ent);
            return entId;
        }

        // �������ߴ����ԭ�㡢�ߴ��ı�λ�á��ߴ��ı��ͱ�ע��ʽ���������ע�ĺ���.
        public static ObjectId AddDimAligned(Point3d pt1, Point3d pt2, Point3d ptText, string text, ObjectId style)
        {
            AlignedDimension ent = new AlignedDimension(pt1, pt2, ptText, text, style);
            ObjectId entId = AppendEntity(ent);
            return entId;
        }

        // ��Բ�ġ����߸��ŵ�����߳��ȴ����뾶��ע�ĺ���.
        public static ObjectId AddDimRadial(Point3d cenPt, Point3d ptChord, double leaderLength)
        {
            Database db = HostApplicationServices.WorkingDatabase;
            ObjectId style = db.Dimstyle;
            string text = "R" + Math.Round(cenPt.DistanceTo(ptChord),db.Dimdec).ToString();
            RadialDimension ent = new RadialDimension(cenPt, ptChord, leaderLength, text, style);
            ObjectId entId = AppendEntity(ent);
            return entId;
        }

        // ��Բ�ġ����߸��ŵ㡢���߳��ȡ��ߴ��ı��ͱ�ע��ʽ�����뾶��ע�ĺ���.
        public static ObjectId AddDimRadial(Point3d cenPt, Point3d ptChord, double leaderLength, string text, ObjectId style)
        {
            RadialDimension ent = new RadialDimension(cenPt, ptChord, leaderLength, text, style);
            ObjectId entId = AppendEntity(ent);
            return entId;
        }

        // ���������߸��ŵ�����߳��ȴ���ֱ����ע�ĺ���.
        public static ObjectId AddDimDiametric(Point3d ptChord1, Point3d ptChord2, double leaderLength)
        {
            Database db = HostApplicationServices.WorkingDatabase;
            ObjectId style = db.Dimstyle;
            string text = "%%c" + Math.Round(ptChord1.DistanceTo(ptChord2),db.Dimdec).ToString();
            DiametricDimension ent = new DiametricDimension(ptChord1, ptChord2, leaderLength, text, style);
            ObjectId entId = AppendEntity(ent);
            return entId;
        }

        // ���������߸��ŵ㡢���߳��ȡ��ߴ��ı��ͱ�ע��ʽ����ֱ����ע�ĺ���.
        public static ObjectId AddDimDiametric(Point3d ptChord1, Point3d ptChord2, double leaderLength, string text, ObjectId style)
        {
            DiametricDimension ent = new DiametricDimension(ptChord1, ptChord2, leaderLength, text, style);
            ObjectId entId = AppendEntity(ent);
            return entId;
        }

        // ������ֱ�ߵ������յ��Լ��ߴ��ı�λ�ô����Ƕȱ�ע�ĺ���.
        public static ObjectId AddDimLineAngular(Point3d line1StartPt, Point3d line1EndPt, Point3d line2StartPt, Point3d line2EndPt, Point3d arcPt)
        {
            Database db = HostApplicationServices.WorkingDatabase;
            ObjectId style = db.Dimstyle;
            Vector3d vec1 = line1EndPt - line1StartPt;
            Vector3d vec2 = line2EndPt - line2StartPt;
            double ang = vec1.GetAngleTo(vec2) * 180 / Math.PI;
            string text = Math.Round(ang,db.Dimadec).ToString() + "%%d";
            LineAngularDimension2 ent = new LineAngularDimension2(line1StartPt, line1EndPt, line2StartPt, line2EndPt, arcPt, text, style);
            ObjectId entId = AppendEntity(ent);
            return entId;
        }

        // ������ֱ�ߵ������յ��Լ��ߴ��ı�λ�á��ߴ��ı�����ע��ʽ�����Ƕȱ�ע�ĺ���.
        public static ObjectId AddDimLineAngular(Point3d line1StartPt, Point3d line1EndPt, Point3d line2StartPt, Point3d line2EndPt, Point3d arcPt, string text, ObjectId style)
        {
            LineAngularDimension2 ent = new LineAngularDimension2(line1StartPt, line1EndPt, line2StartPt, line2EndPt, arcPt, text, style);
            ObjectId entId = AppendEntity(ent);
            return entId;
        }

        // �ɽǶȶ��㡢�����ߴ����ԭ��ͳߴ��ı�λ�ô����Ƕȱ�ע�ĺ���.
        public static ObjectId AddDimLineAngular(Point3d cenPt, Point3d line1Pt, Point3d line2Pt, Point3d arcPt)
        {
            Database db = HostApplicationServices.WorkingDatabase;
            ObjectId style = db.Dimstyle;
            Vector3d vec1 = line1Pt - cenPt;
            Vector3d vec2 = line2Pt - cenPt;
            double ang = vec1.GetAngleTo(vec2) * 180 / Math.PI;
            string text = Math.Round(ang, db.Dimadec).ToString() + "%%d";
            Point3AngularDimension ent = new Point3AngularDimension(cenPt, line1Pt, line2Pt, arcPt, text, style);
            ObjectId entId = AppendEntity(ent);
            return entId;
        }

        // �ɽǶ��㡢�����ߴ����ԭ�㡢�ߴ��ı�λ�á��ߴ��ı��ͱ�ע��ʽ�����Ƕȱ�ע�ĺ���.
        public static ObjectId AddDimLineAngular(Point3d cenPt, Point3d line1Pt, Point3d line2Pt, Point3d arcPt, string text, ObjectId style)
        {
            Point3AngularDimension ent = new Point3AngularDimension(cenPt, line1Pt, line2Pt, arcPt, text, style);
            ObjectId entId = AppendEntity(ent);
            return entId;
        }

        // ��Բ�ġ������ߴ����ԭ��ͳߴ��ı�λ�ô���������ע�ĺ���.
        public static ObjectId AddDimArc(Point3d cenPt, Point3d pt1, Point3d pt2, Point3d arcPt)
        {
            Database db = HostApplicationServices.WorkingDatabase;
            ObjectId style = db.Dimstyle;
            Vector3d vec1 = cenPt.GetVectorTo(pt1);
            Vector3d vec2 = cenPt.GetVectorTo(pt2);
            double ang = vec1.GetAngleTo(vec2);
            double radius = cenPt.DistanceTo(pt1);
            double arcLength = ang * radius;
            string text = (Math.Round(arcLength, db.Dimdec)).ToString();
            ArcDimension ent = new ArcDimension(cenPt, pt1, pt2, arcPt, text, style);
            ObjectId entId = AppendEntity(ent);
            return entId;
        }

        // ��Բ�ġ������ߴ����ԭ�㡢�ߴ��ı�λ�á��ߴ��ı��ͱ�ע��ʽ����������ע�ĺ���.
        public static ObjectId AddDimArc(Point3d cenPt, Point3d pt1, Point3d pt2, Point3d arcPt, string text, ObjectId style)
        {
            ArcDimension ent = new ArcDimension(cenPt, pt1, pt2, arcPt, text, style);
            ObjectId entId = AppendEntity(ent);
            return entId;
        }

        // �ɱ�ע���ͣ��Ƿ���X���꣩����ע��ͷ����ʼλ�úͱ�ע��ͷ����ֹλ�ô��������ע�ĺ���.
        public static ObjectId AddDimOrdinate(bool useXAxis, Point3d ordPt, Point3d pt)
        {
            Database db = HostApplicationServices.WorkingDatabase;
            ObjectId style = db.Dimstyle;
            string text;
            if (useXAxis == true)
                text = Math.Round(ordPt.X,db.Dimdec).ToString();
            else
                text = Math.Round(ordPt.Y,db.Dimdec).ToString();
            OrdinateDimension ent = new OrdinateDimension(useXAxis, ordPt, pt, text, style);
            ObjectId entId = AppendEntity(ent);
            return entId;
        }

        // �ɱ�ע���ͣ��Ƿ���X���꣩����ע��ͷ����ʼλ�á���ע��ͷ����ֹλ�á��ߴ��ı��ͱ�ע��ʽ���������ע�ĺ���.
        public static ObjectId AddDimOrdinate(bool useXAxis, Point3d ordPt, Point3d pt, string text, ObjectId style)
        {
            OrdinateDimension ent = new OrdinateDimension(useXAxis, ordPt, pt, text, style);
            ObjectId entId = AppendEntity(ent);
            return entId;
        }

        // �ɱ�ע��ͷ����ʼλ�á���ע��ͷ��X��ֹλ�úͱ�ע��ͷ��Y��ֹλ�ô��������ע�ĺ���(X�����Y����).
        public static ObjectIdCollection AddDimOrdinate(Point3d ordPt, Point3d ptX, Point3d ptY)
        {
            Database db = HostApplicationServices.WorkingDatabase;
            ObjectId style = db.Dimstyle;
            string textX = Math.Round(ordPt.X,db.Dimdec).ToString();
            string textY = Math.Round(ordPt.Y,db.Dimdec).ToString();
            OrdinateDimension entX = new OrdinateDimension(true, ordPt, ptX, textX, style);
            OrdinateDimension entY = new OrdinateDimension(false, ordPt, ptY, textY, style);
            ObjectId objIdX = AppendEntity(entX);
            ObjectId objIdY = AppendEntity(entY);
            ObjectIdCollection entIds = new ObjectIdCollection();
            entIds.Add(objIdX);
            entIds.Add(objIdY);
            return entIds;
        }

        // �ɱ�ע��ͷ����ʼλ�á���ע��ͷ��X��ֹλ�á���ע��ͷ��Y��ֹλ�á�X�����ע���֡�Y�����ע���ֺͱ�ע��ʽ���������ע�ĺ���.
        public static ObjectIdCollection AddDimOrdinate(Point3d ordPt, Point3d ptX, Point3d ptY, string textX, string textY, ObjectId style)
        {
            try
            {
                OrdinateDimension entX = new OrdinateDimension(true, ordPt, ptX, textX, style);
                OrdinateDimension entY = new OrdinateDimension(false, ordPt, ptX, textX, style);
                ObjectId objIdX = AppendEntity(entX);
                ObjectId objIdY = AppendEntity(entY);
                ObjectIdCollection entIds = new ObjectIdCollection();
                entIds.Add(objIdX);
                entIds.Add(objIdY);
                return entIds;
            }
            catch
            {
                ObjectId nullId = ObjectId.Null;
                ObjectIdCollection nullIds = new ObjectIdCollection();
                nullIds.Add(nullId);
                return nullIds;
            }
        }

        // ����ά�㼯�ϴ������߱�ע�ĺ���.
        public static ObjectId AddLeader(Point3dCollection pts, bool splBool)
        {
            Leader ent = new Leader();
            ent.IsSplined = splBool;
            for (int i = 0; i < pts.Count; i++)
            {
                ent.AppendVertex(pts[i]);
                ent.SetVertexAt(i, pts[i]);
            }
            ObjectId entId = AppendEntity(ent);
            return entId;
        }

        // ����λ����������֡�����㡢������������λ����x��������������λ�����ע�ĺ���.
        public static ObjectId AddTolerance(string codes, Point3d inPt, Vector3d norVec, Vector3d xVec)
        {
            FeatureControlFrame ent = new FeatureControlFrame(codes, inPt, norVec, xVec);
            ObjectId entId = AppendEntity(ent);
            return entId;
        }

        // �Ȼ����ȵĺ���.
        public static double Rad2Ang(double angle)
        {
            double rad = angle * Math.PI / 180;
            return rad;
        }

        // ��ȡ�������ָ���ǶȺ;���ĵ�.
        public static Point3d PolarPoint(Point3d basePt, double angle, double distance)
        {
            double[] pt = new double[3];
            pt[0] = basePt[0] + distance * Math.Cos(angle);
            pt[1] = basePt[1] + distance * Math.Sin(angle);
            pt[2] = basePt[2];
            Point3d point = new Point3d(pt[0], pt[1], pt[2]);
            return point;
        }

        // ��ͼ�ζ�����뵽ģ�Ϳռ�ĺ���.
        public static ObjectId AppendEntity(Entity ent)
        {
            Database db = HostApplicationServices.WorkingDatabase;
            ObjectId entId;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                entId = btr.AppendEntity(ent);
                trans.AddNewlyCreatedDBObject(ent, true);
                trans.Commit();
            }
            return entId;
        }
    }
}
