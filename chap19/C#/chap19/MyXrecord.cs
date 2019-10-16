using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace chap19
{
    public class MyXrecord
    {
        [CommandMethod("CreateXrecord")]
        public void CreateXrecord()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Database db = HostApplicationServices.WorkingDatabase;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                //�½�һ����չ��¼����
                Xrecord xrec = new Xrecord();
                //������չ��¼�а����������б������ı������ꡢ��ֵ���Ƕȡ���ɫ
                Point3d pt = new Point3d(1.0, 2.0, 0.0);
                xrec.Data = new ResultBuffer(
                            new TypedValue((int)(int)DxfCode.Text, "����һ�������õ���չ��¼�б�"),
                            new TypedValue((int)DxfCode.XCoordinate, pt),
                            new TypedValue((int)DxfCode.Real, 3.14159),
                            new TypedValue((int)DxfCode.Angle, 3.14159),
                            new TypedValue((int)DxfCode.Color, 1),
                            new TypedValue((int)DxfCode.Int16, 180));
                //����Ĳ�������ѡ��Ҫ�����չ��¼�Ķ���
                PromptEntityOptions opt = new PromptEntityOptions("��ѡ��Ҫ�����չ��¼�Ķ���");
                PromptEntityResult res = ed.GetEntity(opt);
                if (res.Status != PromptStatus.OK)
                    return;
                Entity ent = (Entity)trans.GetObject(res.ObjectId, OpenMode.ForWrite);
                //�ж���ѡ�����Ƿ��Ѱ�����չ��¼
                if (ent.ExtensionDictionary != ObjectId.Null)
                {
                    ed.WriteMessage("�����Ѱ�����չ��¼���޷��ٴ���");
                    return;
                }
                //Ϊ��ѡ��Ķ��󴴽�һ����չ�ֵ�
                ent.CreateExtensionDictionary();
                ObjectId dictEntId = ent.ExtensionDictionary;
                DBDictionary entXrecord = (DBDictionary)trans.GetObject(dictEntId, OpenMode.ForWrite);
                //����չ�ֵ��м��������½�����չ��¼���󣬲�ָ�����������ؼ���ΪMyXrecord
                entXrecord.SetAt("MyXrecord", xrec);
                //֪ͨ�����������չ��¼����ļ���
                trans.AddNewlyCreatedDBObject(xrec, true);
                trans.Commit();
            }
        }

        [CommandMethod("ListXrecord")]
        public void ListXrecord()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Database db = HostApplicationServices.WorkingDatabase;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                //����Ĳ�������ѡ����ʾ��չ��¼�Ķ���
                PromptEntityOptions opt = new PromptEntityOptions("��ѡ��Ҫ��ʾ��չ��¼�Ķ���");
                PromptEntityResult res = ed.GetEntity(opt);
                if (res.Status != PromptStatus.OK)
                    return;
                Entity ent = (Entity)trans.GetObject(res.ObjectId, OpenMode.ForRead);
                //����ѡ��������չ�ֵ�
                DBDictionary entXrecord = (DBDictionary)trans.GetObject(ent.ExtensionDictionary, OpenMode.ForRead);
                //����չ�ֵ��������ؼ���ΪMyXrecord����չ��¼��������ҵ��򷵻�����ObjectId
                ObjectId xrecordId = entXrecord.GetAt("MyXrecord");
                //���ҵ�����չ��¼����
                Xrecord xrecord = (Xrecord)trans.GetObject(xrecordId, OpenMode.ForRead);
                //��ȡ��չ��¼�а����������б�ѭ��������ʾ����
                ResultBuffer rb = xrecord.Data;
                foreach (TypedValue value in rb)
                {
                    ed.WriteMessage(string.Format("\nTypeCode={0},Value={1}", value.TypeCode, value.Value));
                }
                trans.Commit();
            }
        }
    }
}
