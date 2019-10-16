using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;

namespace chap19
{
    public class MyGroup
    {
        [CommandMethod("MakeGroup")]
        public void MakeGroup()
        {
            //������ΪMyGroup����
            createGroup("MyGroup");
        }
        private void createGroup(string groupName)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Database db = HostApplicationServices.WorkingDatabase;
            using (Transaction trans=db.TransactionManager.StartTransaction())
            {
                //�½�һ�������
                Group gp = new Group(groupName, true);
                //�򿪵�ǰ���ݿ�����ֵ�����Լ����½��������
                DBDictionary dict = (DBDictionary)trans.GetObject(db.GroupDictionaryId, OpenMode.ForWrite);
                //�����ֵ��н��������Ϊһ������Ŀ���룬��ָ�����������ؼ���ΪgroupName
                dict.SetAt(groupName, gp);
                //����Ĳ�������ѡ������Ҫ�����Ķ���
                PromptSelectionOptions opt = new PromptSelectionOptions();
                opt.MessageForAdding = "��ѡ������Ҫ�����Ķ���";
                PromptSelectionResult res = ed.GetSelection(opt);
                if (res.Status!=PromptStatus.OK)
                {
                    return;
                }
                //��ȡ��ѡ������ObjectId����
                SelectionSet ss = res.Value;
                ObjectIdCollection ids = new ObjectIdCollection(ss.GetObjectIds());
                //��������м�����ѡ��Ķ���
                gp.Append(ids);
                //֪ͨ��������������ļ���
                trans.AddNewlyCreatedDBObject(gp, true);
                trans.Commit();
            }
        }

        [CommandMethod("RemoveButLines")]
        public void RemoveButLines()
        {
            //��MyGroup�����Ƴ����в���ֱ�ߵĶ���
            removeAllButLines("MyGroup");
        }

        private void removeAllButLines(string groupName)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Database db = HostApplicationServices.WorkingDatabase;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                //�򿪵�ǰ���ݿ�����ֵ����
                DBDictionary dict = (DBDictionary)trans.GetObject(db.GroupDictionaryId, OpenMode.ForRead);
                //�����ֵ��������ؼ���ΪgroupName�����������ҵ��򷵻�����ObjectId
                ObjectId gpid = dict.GetAt(groupName);
                //����Ҫ�����н���ȥ������Ĳ����������д�ķ�ʽ���ҵ��������
                Group gp = (Group)trans.GetObject(gpid, OpenMode.ForWrite);
                //��ȡ������е�����ʵ���ObjectId������ѭ������
                ObjectId[] ids = gp.GetAllEntityIds();
                foreach (ObjectId id in ids)
                {
                    //�����еĵ�ǰ���󣬲��ж��Ƿ�Ϊֱ��
                    Line obj = trans.GetObject(id, OpenMode.ForRead) as Line;
                    if (obj==null)
                    {
                        //���������ֱ�ߣ����������Ƴ���
                        gp.Remove(id);
                    }
                }
                //������������ʵ�����ɫΪ��ɫ
                gp.SetColorIndex(1);
                trans.Commit();
            }
        }
    }
}
