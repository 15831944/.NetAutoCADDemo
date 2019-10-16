using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Interop;
using Autodesk.AutoCAD.ApplicationServices;
using System.Reflection;
namespace chap21
{
    public class MyMenu 
    {
        [CommandMethod("AddMenu")]
        public void AddMenu()
        {
            //COM��ʽ��ȡAutoCADӦ�ó������
            AcadApplication acadApp = (AcadApplication)Application.AcadApplication;
            //ΪAutoCAD���һ���µĲ˵��������ñ���Ϊ"�ҵĲ˵�"
            AcadPopupMenu pm = acadApp.MenuGroups.Item(0).Menus.Add("�ҵĲ˵�");
            //����һ��AutoCAD�����˵�����ڻ�ȡ��ӵĲ˵������
            AcadPopupMenuItem pmi;
            //���½��Ĳ˵������һ����Ϊ"Բ"�Ĳ˵���Ե��û���Բ����
            pmi = pm.AddMenuItem(pm.Count + 1, "Բ", "_Circle ");
            //����״̬����ʾ��Ϣ
            pmi.HelpString = "��ָ���뾶����Բ";
            //�����Ϊ"ֱ��"�Ĳ˵���Ե��û���ֱ������
            pmi = pm.AddMenuItem(pm.Count + 1, "ֱ��", "_Line ");
            pmi.HelpString = "����ֱ�߶�";
            //�����Ϊ"�����"�Ĳ˵���Ե��û��ƶ��������
            pmi = pm.AddMenuItem(pm.Count + 1, "�����", "_Polyline ");
            pmi.HelpString = "������ά�����";
            //�����Ϊ"����"�Ĳ˵���Ե��û��ƾ��ζ��������
            pmi = pm.AddMenuItem(pm.Count + 1, "����", "_Rectangle ");
            pmi.HelpString = "�������ζ����";
            //���һ���ָ��������ֲ�ͬ���͵�����
            pm.AddSeparator(pm.Count + 1);
            //���һ����Ϊ"�޸�"���Ӳ˵�
            AcadPopupMenu menuModify = pm.AddSubMenu(pm.Count + 1, "�޸�");
            //��"�޸�"�Ӳ˵���������ڸ��ơ�ɾ�����ƶ�����ת�����Ĳ˵����������Ӧ��״̬����ʾ��Ϣ
            pmi = menuModify.AddMenuItem(menuModify.Count + 1, "����", "_Copy ");
            pmi.HelpString = "���ƶ���";
            pmi = menuModify.AddMenuItem(menuModify.Count + 1, "ɾ��", "_Erase ");
            pmi.HelpString = "��ͼ��ɾ������";
            pmi = menuModify.AddMenuItem(menuModify.Count + 1, "�ƶ�", "_Move ");
            pmi.HelpString = "�ƶ�����";
            pmi = menuModify.AddMenuItem(menuModify.Count + 1, "��ת", "_Rotate ");
            pmi.HelpString = "�ƻ�����ת����";
            //������Ĳ˵���ʾ��AutoCAD�˵��������
            pm.InsertInMenuBar(acadApp.MenuBar.Count + 1);
        }
    }
}
