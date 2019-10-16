using System;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.GraphicsInterface;
using Autodesk.AutoCAD.Runtime;

namespace chap22
{
    public class Jig_Move : DrawJig
    {
        // ����ȫ�ֱ���.
        private Point3d sourcePt,targetPt,curPt;
        private Entity[] entCopy;
        private ObjectId[] ids;

        [CommandMethod("jigMove")]
        public void testJigMove()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            // ��ͨ��ѡ�񼯽�������.
            PromptSelectionOptions opt = new PromptSelectionOptions();
            opt.MessageForAdding = "ѡ�����";
            opt.AllowDuplicates = true;
            PromptSelectionResult res = ed.GetSelection(opt);
            if (res.Status != PromptStatus.OK)
                return;
            SelectionSet sSet = res.Value;
            ids = sSet.GetObjectIds();

            Entity[] oldEnt = new Entity[ids.Length];

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                entCopy = new Entity[ids.Length];
                for (int i = 0; i <= ids.Length - 1; i++)
                {
                    oldEnt[i] = (Entity)trans.GetObject(ids[i], OpenMode.ForWrite);
                    // ��Դ��������Ϊ����״̬.
                    oldEnt[i].Highlight();
                    // ����.
                    entCopy[i] = (Entity)oldEnt[i].Clone();
                }

                // �õ��ƶ���Դ��-----------------------------------------------
                PromptPointOptions optPoint = new PromptPointOptions("\n���������<0,0,0>");
                optPoint.AllowNone = true;
                PromptPointResult resPoint = ed.GetPoint(optPoint);
                if (resPoint.Status != PromptStatus.Cancel)
                {
                    if (resPoint.Status == PromptStatus.None)
                        sourcePt = new Point3d(0, 0, 0);
                    else
                        sourcePt = resPoint.Value;
                }

                // ����Ŀ������ק��ʱ��ĳ�ֵ.
                targetPt = sourcePt;
                curPt = targetPt;

                // ��ʼ��ק.
                PromptResult jigRes = ed.Drag(this);
                if (jigRes.Status == PromptStatus.OK)
                {
                    BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                    BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                    for (int i = 0; i <= ids.Length - 1; i++)
                    {
                        btr.AppendEntity(entCopy[i]);
                        trans.AddNewlyCreatedDBObject(entCopy[i], true);
                    }
                    // ɾ��Դ����.
                    for (int i = 0; i <= ids.Length - 1; i++)
                        oldEnt[i].Erase();
                }
                else
                {
                    // ȡ��Դ����ĸ���״̬.
                    for (int i = 0; i <= ids.Length - 1; i++)
                        oldEnt[i].Unhighlight();
                }
                trans.Commit();
            }
        }

        // Sampler�������ڼ���û�������.
        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            // ����һ�����϶�������.
            JigPromptPointOptions optJig = new JigPromptPointOptions("\n��ָ���ڶ���:");
            // ������ק�������.
            optJig.Cursor = CursorType.RubberBand;
            // �����϶�������.
            optJig.BasePoint = sourcePt;
            optJig.UseBasePoint = true;
            // ��AcquirePoint�����õ��û�����ĵ�.
            PromptPointResult resJig = prompts.AcquirePoint(optJig);
            targetPt = resJig.Value;
            // ����û���ק�����þ���任�ķ����ƶ�ѡ���е�ȫ������.
            if (curPt != targetPt)
            {
                Matrix3d moveMt = Matrix3d.Displacement(targetPt - curPt);
                for (int i = 0; i <= ids.Length - 1; i++)
                    entCopy[i].TransformBy(moveMt);
                // ���浱ǰ��.
                curPt = targetPt;
                return SamplerStatus.OK;
            }
            else
                return SamplerStatus.NoChange;
        }

        // WorldDraw��������ˢ����Ļ����ʾ��ͼ��.
        protected override bool WorldDraw(WorldDraw draw)
        {
            for (int i = 0; i <= ids.Length - 1; i++)
                // ˢ�»���.
                draw.Geometry.Draw(entCopy[i]);
            return true;
        }
    }
}
