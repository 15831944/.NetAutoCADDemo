using System;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;

namespace chap18
{
    public class DimStyle
    {
        public ObjectId ISO25(String dimStyleName)
        {
            Database db = HostApplicationServices.WorkingDatabase;
            ObjectId dimstyleId;
            try
            {
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    DimStyleTable dt = (DimStyleTable)trans.GetObject(db.DimStyleTableId, OpenMode.ForWrite);
                    // �½�һ����ע��ʽ���¼.
                    DimStyleTableRecord dtr = new DimStyleTableRecord();
                    // ���㾫��
                    dtr.Dimaltd = 3;
                    // �����������
                    dtr.Dimaltf = 0.03937008;
                    // ���㹫���
                    dtr.Dimalttd = 3;
                    // ��ͷ��С
                    dtr.Dimasz = 2.5;
                    // Բ�ı�Ǵ�С
                    dtr.Dimcen = 2.5;
                    // ����
                    dtr.Dimdec = 2;
                    // �ߴ��߼��
                    dtr.Dimdli = 3.75;
                    // С���ָ���
                    dtr.Dimdsep = ',';
                    //�ߴ���߳�����
                    dtr.Dimexe = 1.25;
                    // �ߴ����ƫ��
                    dtr.Dimexo = 0.625;
                    // ����ƫ��
                    dtr.Dimgap = 0.625;
                    // ����λ�ô�ֱ
                    dtr.Dimtad = 1;
                    // �����
                    dtr.Dimtdec = 2;
                    // �������ڶ���
                    dtr.Dimtih = false;
                    // �ߴ���ǿ��
                    dtr.Dimtofl = true;
                    // �����ⲿ����
                    dtr.Dimtoh = false;
                    // ����λ�ô�ֱ
                    dtr.Dimtolj = 0;
                    // ���ָ߶�
                    dtr.Dimtxt = 2.5;
                    // ��������
                    dtr.Dimtzin = 8;
                    // ����
                    dtr.Dimzin = 8;
                    //���ñ�ע��ʽ����.
                    dtr.Name = dimStyleName;
                    dimstyleId = dt.Add(dtr);
                    trans.AddNewlyCreatedDBObject(dtr, true);
                    trans.Commit();
                }
                return dimstyleId;
            }
            catch
            {
                ObjectId NullId = ObjectId.Null;
                return NullId;
            }
        }

        [CommandMethod("netdimStyle")]
        public void CreatedimStyle()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                DimStyleTable dt = (DimStyleTable)trans.GetObject(db.DimStyleTableId, OpenMode.ForWrite);
                String dimName = "abc";
                ObjectId dimId = ISO25(dimName);
                if (dimId != ObjectId.Null)
                {
                    DimStyleTableRecord dtr = (DimStyleTableRecord)trans.GetObject(dimId, OpenMode.ForWrite);
                    // �޸ļ�ͷ��С.
                    dtr.Dimasz = 3;
                }
                else
                {
                    ed.WriteMessage("\n��ע��ʽ " + dimName + " �Ѵ��ڣ�");
                }
                trans.Commit();
            }
        }
    }
}


