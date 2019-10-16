using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Windows;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;
namespace chap21
{
    public class ContextMenu
    {
        [CommandMethod("AddDefaultContextMenu")]
        public void AddContextMenu()
        {
            //����һ��ContextMenuExtension�������ڱ�ʾ��ݲ˵�
            ContextMenuExtension contextMenu =new ContextMenuExtension();
            //���ÿ�ݲ˵��ı���
            contextMenu.Title = "�ҵĿ�ݲ˵�";
            //���һ����Ϊ"����"�Ĳ˵�����ڵ��ø�������
            MenuItem mi =new MenuItem("����");
            //Ϊ"����"�˵�����ӵ����¼�
            mi.Click +=new EventHandler(mi_Click);
            //��"����"�˵�����ӵ���ݲ˵���
            contextMenu.MenuItems.Add(mi);
            //���һ����Ϊ"ɾ��"�Ĳ˵�����ڵ���ɾ������
            mi =new MenuItem("ɾ��");
            //Ϊ"ɾ��"�˵�����ӵ����¼�
            mi.Click +=new EventHandler(mi_Click);
            //��"ɾ��"�˵�����ӵ���ݲ˵���
            contextMenu.MenuItems.Add(mi);
            //ΪӦ�ó�����Ӷ���Ŀ�ݲ˵�
            Application.AddDefaultContextMenuExtension(contextMenu);
        }

        void mi_Click(object sender, EventArgs e)
        {
            //��ȡ��������Ŀ�ݲ˵���
            MenuItem mi = sender as MenuItem;
            //��ȡ��ǰ��ĵ�
            Document doc = Application.DocumentManager.MdiActiveDocument;
            //���ݿ�ݲ˵�������֣��ֱ���ö�Ӧ������
            if(mi.Text=="����")
            {
                doc.SendStringToExecute("_Copy ", true, false, true);  
            }
            else if (mi.Text == "ɾ��")
            {
                doc.SendStringToExecute("_Erase ", true, false, true);
            }
        }

        [CommandMethod("AddObjectContextMenu")]
        public void AddObjectContextMenu()
        {
            //����һ��ContextMenuExtension�������ڱ�ʾ��ݲ˵�
            ContextMenuExtension contextMenu =new ContextMenuExtension();
            //���ڶ��󼶱�Ŀ�ݲ˵����������ò˵���
            contextMenu.Title = "Բ�Ŀ�ݲ˵�";
            //���һ����Ϊ"Բ���"�Ĳ˵��������AutoCAD����������ʾ��ѡ���Բ���
            MenuItem miCircle =new MenuItem("Բ���");
            //Ϊ"Բ���"�˵�����ӵ����¼�
            miCircle.Click +=new EventHandler(miCircle_Click);
            //��"Բ���"�˵�����ӵ���ݲ˵���
            contextMenu.MenuItems.Add(miCircle);
            //ΪԲ������Ӷ���Ŀ�ݲ˵�
            Application.AddObjectContextMenuExtension(RXClass.GetClass(typeof(Circle)), contextMenu);
        }

        void miCircle_Click(object sender, EventArgs e)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Database db=HostApplicationServices.WorkingDatabase;
            //��ȡ��ǰ��ѡ�񼯶���
            SelectionSet ss = ed.SelectImplied().Value;
            using (Transaction trans=db.TransactionManager.StartTransaction())
            {
                //ѭ������ѡ���еĶ���
                foreach (ObjectId id in ss.GetObjectIds())
                {
                    Circle circle = trans.GetObject(id, OpenMode.ForRead)as Circle;
                    //�����ѡ��Ķ�����Բ
                    if (circle!=null)
                    {
                        //������������ʾԲ�����Ϣ
                        ed.WriteMessage("\nԲ���Ϊ:"+circle.Area.ToString());
                    }
                }  
            } 
        }
    }
}
