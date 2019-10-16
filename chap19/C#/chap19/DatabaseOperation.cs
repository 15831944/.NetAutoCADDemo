using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;
namespace chap19
{
    public class DatabaseOperation
    {
        [CommandMethod("CreateAndSaveDwg")]
        public void CreateAndSaveDwg()
        {
            //�½�һ�����ݿ�����Դ����µ�Dwg�ļ�
            Database db = new Database();
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                //��ȡ���ݿ�Ŀ�����
                BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                //��ȡ���ݿ��ģ�Ϳռ����¼����
                BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                //�½�����Բ
                Circle cir1 = new Circle(new Point3d(1, 1, 0), Vector3d.ZAxis, 1.0);
                Circle cir2 = new Circle(new Point3d(4, 4, 0), Vector3d.ZAxis, 2.0);
                //��ģ�Ϳռ��м����½�������Բ
                btr.AppendEntity(cir1);
                trans.AddNewlyCreatedDBObject(cir1, true);
                btr.AppendEntity(cir2);
                trans.AddNewlyCreatedDBObject(cir2, true);

                //���屣���ļ��Ի���
                PromptSaveFileOptions opt = new PromptSaveFileOptions("\n�������ļ�����");
                //�����ļ��Ի�����ļ���չ���б�
                opt.Filter = "ͼ��(*.dwg)|*.dwg|ͼ��(*.dxf)|*.dxf";
                //�ļ��������б���ȱʡ��ʾ���ļ���չ��
                opt.FilterIndex = 0;
                //�����ļ��Ի���ı���
                opt.DialogCaption = "ͼ�����Ϊ";
                //ȱʡ����Ŀ¼
                opt.InitialDirectory = "C:\\";
                //ȱʡ�����ļ�������չ������չ���б��еĵ�ǰֵȷ����
                opt.InitialFileName = "MyDwg";
                //��ȡ��ǰ���ݿ���󣨲��������½��ģ��������ж���
                Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
                //���ݱ���Ի������û���ѡ�񣬻�ȡ�����ļ���
                string filename = ed.GetFileNameForSave(opt).StringResult;
                //�����ļ�Ϊ��ǰAutoCAD�汾
                db.SaveAs(filename, DwgVersion.Current);
            }

        }

        [CommandMethod("ReadDwg")]
        public void ReadDwg()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //�½�һ�����ݿ�����Զ�ȡDwg�ļ�
            Database db = new Database(false, true);
            //�ļ���
            string fileName = "C:\\MyDwg.dwg";
            //���ָ���ļ������ļ����ڣ���
            if (System.IO.File.Exists(fileName))
            {
                //���ļ����뵽���ݿ���
                db.ReadDwgFile(fileName, System.IO.FileShare.Read, true, null);
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    //��ȡ���ݿ�Ŀ�����
                    BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                    //�����ݿ��ģ�Ϳռ����¼����
                    BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead);
                    //ѭ������ģ�Ϳռ��е�ʵ��
                    foreach (ObjectId id in btr)
                    {
                        Entity ent = trans.GetObject(id, OpenMode.ForRead) as Entity;
                        if (ent != null)
                        {
                            //��ʾʵ�������
                            ed.WriteMessage("\n" + ent.GetType().ToString());
                        }

                    }
                }
            }
            //�������ݿ����
            db.Dispose();
        }

        [CommandMethod("CopyFromOtherDwg")]
        public void CopyFromOtherDwg()
        {
            //�½�һ�����ݿ�����Զ�ȡDwg�ļ�
            Database db = new Database(false, false);
            string fileName = "C:\\Blocks and Tables - Metric.dwg";
            if (System.IO.File.Exists(fileName))
            {
                db.ReadDwgFile(fileName, System.IO.FileShare.Read, true, null);
                //Ϊ���ò����ĺ����ڶ��ͼ���ļ��򿪵�����������ã������ʹ������ĺ�����Dwg�ļ��ر�
                db.CloseInput(true);
                //��ȡ��ǰ���ݿ���󣨲����½������ݿ⣩
                Database curdb = HostApplicationServices.WorkingDatabase;
                //��Դ���ݿ�ģ�Ϳռ��е�ʵ����뵽��ǰ���ݿ��һ���µĿ���¼��
                curdb.Insert(System.IO.Path.GetFileNameWithoutExtension(fileName), db, true);
            }
            //�������ݿ����
            db.Dispose();
        }

        [CommandMethod("OpenDwg")]
        public void OpenDwg()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //��ȡ�ĵ������������Դ�Dwg�ļ�
            DocumentCollection docs = Application.DocumentManager;
            //���ô��ļ��Ի�����й�ѡ��
            PromptOpenFileOptions opt = new PromptOpenFileOptions("\n�������ļ�����");
            opt.Filter = "ͼ��(*.dwg)|*.dwg|ͼ��(*.dxf)|*.dxf";
            opt.FilterIndex = 0;
            //���ݴ��ļ��Ի������û���ѡ�񣬻�ȡ�ļ���
            string filename = ed.GetFileNameForOpen(opt).StringResult;
            //����ѡ���Dwg�ļ�
            Document doc = docs.Open(filename, true);
            //���õ�ǰ�Ļ�ĵ�Ϊ�´򿪵�Dwg�ļ�
            Application.DocumentManager.MdiActiveDocument = doc;
        }


        [CommandMethod("CopyEntities")]
        public void CopyEntities()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //�½�һ�����ݿ����
            Database db = new Database(false, true);
            //��ȡ��ǰ���ݿ����
            Database curdb = HostApplicationServices.WorkingDatabase;
            //����Ĳ���ѡ��Ҫ���Ƶ��½����ݿ��е�ʵ��
            PromptSelectionOptions opts = new PromptSelectionOptions();
            opts.MessageForAdding = "�����븴�Ƶ����ļ���ʵ��";
            SelectionSet ss = ed.GetSelection(opts).Value;
            //��ȡ��ѡʵ���ObjectId����
            ObjectIdCollection ids = new ObjectIdCollection(ss.GetObjectIds());
            //�ѵ�ǰ���ݿ�����ѡ���ʵ�帴�Ƶ��½������ݿ��У���ָ�������Ϊ��ǰ���ݿ�Ļ���
            db = curdb.Wblock(ids, curdb.Ucsorg);
            //��2004��ʽ�������ݿ�ΪDwg�ļ�
            db.SaveAs("C:\test.dwg", DwgVersion.AC1800);
        }
    }
}
