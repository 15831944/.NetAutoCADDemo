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
    public class MyXData
    {
        [CommandMethod("AddXData")]
        public void AddXData()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Database db = HostApplicationServices.WorkingDatabase;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                //����Ĳ�������ѡ��ʵ���������չ����
                PromptEntityOptions opt = new PromptEntityOptions("��ѡ��ʵ���������չ����");
                PromptEntityResult res = ed.GetEntity(opt);
                if (res.Status != PromptStatus.OK)
                {
                    return;
                }
                Circle circ = (Circle)trans.GetObject(res.ObjectId, OpenMode.ForWrite);
                //��ȡ��ǰ���ݿ��ע��Ӧ�ó����
                RegAppTable reg = (RegAppTable)trans.GetObject(db.RegAppTableId, OpenMode.ForWrite);
                //���û����Ϊ"ʵ����չ����"��ע��Ӧ�ó�����¼����
                if (!reg.Has("ʵ����չ����"))
                {
                    //����һ��ע��Ӧ�ó�����¼������ʾ��չ����
                    RegAppTableRecord app = new RegAppTableRecord();
                    //������չ���ݵ�����
                    app.Name = "ʵ����չ����";
                    //��ע��Ӧ�ó���������չ����
                    reg.Add(app);
                    trans.AddNewlyCreatedDBObject(app, true);
                }
                //������չ���ݵ�����
                ResultBuffer rb = new ResultBuffer(
                new TypedValue((int)DxfCode.ExtendedDataRegAppName, "ʵ����չ����"),
                new TypedValue((int)DxfCode.ExtendedDataAsciiString, "�ַ�����չ����"),
                new TypedValue((int)DxfCode.ExtendedDataLayerName, "0"),
                new TypedValue((int)DxfCode.ExtendedDataReal, 1.23479137438413E+40),
                new TypedValue((int)DxfCode.ExtendedDataInteger16, 32767),
                new TypedValue((int)DxfCode.ExtendedDataInteger32, 32767),
                new TypedValue((int)DxfCode.ExtendedDataScale, 10),
                new TypedValue((int)DxfCode.ExtendedDataWorldXCoordinate, new Point3d(10, 10, 0)));
                //���½�����չ���ݸ��ӵ���ѡ���ʵ����
                circ.XData = rb;
                trans.Commit();
            }
        }

        [CommandMethod("ListXData")]
        public void ListXData()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Database db = HostApplicationServices.WorkingDatabase;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                //����Ĳ�������ѡ��ʵ������ʾ������չ����
                PromptEntityOptions opt = new PromptEntityOptions("��ѡ��ʵ������ʾ������չ����");
                PromptEntityResult res = ed.GetEntity(opt);
                if (res.Status != PromptStatus.OK)
                {
                    return;
                }
                Entity ent = (Entity)trans.GetObject(res.ObjectId, OpenMode.ForRead);
                //��ȡ��ѡ��ʵ������Ϊ��ʵ����չ���ݡ�����չ����
                ResultBuffer rb = ent.GetXDataForApplication("ʵ����չ����");
                //���û�У��ͷ���
                if (rb == null)
                {
                    return;
                }
                //ѭ��������չ����
                foreach (TypedValue entXData in rb)
                {
                    ed.WriteMessage(string.Format("\nTypeCode={0},Value={1}", entXData.TypeCode, entXData.Value));
                }
            }
        }
    }
}
