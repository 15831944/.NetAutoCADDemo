using System;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;

namespace chap18
{
    public class Style
    {
        [CommandMethod("netStyle")]
        public void CreateStyle()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                TextStyleTable st = (TextStyleTable)trans.GetObject(db.TextStyleTableId, OpenMode.ForWrite);
                String StyleName = "����ͼ";
                if (st.Has(StyleName) == false)
                {
                    TextStyleTableRecord str = new TextStyleTableRecord();
                    str.Name = StyleName;
                    str.FileName = "simfang.ttf";
                    //---------------------------------------------
                    // ����SHX����
                    // str.FileName = "gbenor"
                    //���ô�����.
                    // str.BigFontFileName = "gbcbig"
                    // --------------------------------------------
                    str.ObliquingAngle = 15 * Math.PI / 180;
                    str.XScale = 0.67;
                    ObjectId TextstyleId = st.Add(str);
                    trans.AddNewlyCreatedDBObject(str, true);
                    db.Textstyle = TextstyleId;
                    trans.Commit();
                }
            }
        }

        [CommandMethod("getTextStyle")]
        public void TestgetTextStyle()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            PromptEntityOptions optEnt = new PromptEntityOptions("\n��ѡ������");
            optEnt.SetRejectMessage("\n��ѡ��Ķ��������֣�������ѡ��");
            optEnt.AddAllowedClass(typeof(DBText), true);
            optEnt.AddAllowedClass(typeof(MText), true);
            PromptEntityResult resEnt = ed.GetEntity(optEnt);

            if (resEnt.Status != PromptStatus.OK)
                return;
            ObjectId entId = resEnt.ObjectId;

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                Entity ent = (Entity)trans.GetObject(entId, OpenMode.ForRead);
                DBText textEnt;
                MText mtextEnt;
                ObjectId textStyle;
                try
                {
                    textEnt = (DBText)ent;
                    textStyle = textEnt.TextStyle;
                }
                catch
                {
                    mtextEnt = (MText)ent;
                    textStyle = mtextEnt.TextStyle;
                }
                TextStyleTableRecord str = (TextStyleTableRecord)trans.GetObject(textStyle, OpenMode.ForRead);
                if (str.Font.TypeFace == "")
                    Application.ShowAlertDialog("��ѡ���������SHX����" + "\n��������" + str.FileName + "\n����������" + str.BigFontFileName);
                else
                    Application.ShowAlertDialog("��ѡ���������TrueType����" + "\n�����ļ�����" + str.FileName + "\n��������" + str.Font.TypeFace);
            }
        }
    }
}


