using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.AutoCAD.Runtime;
using System.Reflection;
using Autodesk.AutoCAD.Interop;
using Autodesk.AutoCAD.ApplicationServices;
namespace chap21
{
    public class MyToolBar
    {
        [CommandMethod("AddToolBar")]
        public void AddToolBar()
        {
            //��ȡ��ǰ���еĳ���
            System.Reflection.Module myModule = System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0];
            //��ȡ��ǰ���еĳ��򼯵�����·���������ļ�����
            string modulePath = myModule.FullyQualifiedName;
            //��ȡȥ���ļ�������򼯵�·�������������ڵ��ļ���
            modulePath = modulePath.Substring(0, modulePath.LastIndexOf("\\"));
            //COM��ʽ��ȡAutoCADӦ�ó������
            AcadApplication acadApp = (AcadApplication)Application.AcadApplication;
            //��ȡ��ǰ�˵��飬���ڼ��빤����
            AcadMenuGroup currMenuGroup = acadApp.MenuGroups.Item(0);
            //ΪAutoCAD���һ���µĹ������������ñ���Ϊ"�ҵĹ�����"
            AcadToolbar tbModify = currMenuGroup.Toolbars.Add("�ҵĹ�����");
            //���½��Ĺ����������һ��"����"��ť���Ե��ø�������
            AcadToolbarItem button0 = tbModify.AddToolbarButton("", "����", "���ƶ���", "_Copy ",Type.Missing);
            //���ø��ư�ť��ͼƬ
            button0.SetBitmaps(modulePath + "\\Resources\\Copy.bmp", modulePath + "\\Resources\\Copy.bmp");
            //'���һ��"ɾ��"��ť���Ե���ɾ������
            AcadToolbarItem button1 = tbModify.AddToolbarButton("", "ɾ��", "��ͼ��ɾ������", "_Erase ", Type.Missing);
            //����ɾ����ť��ͼƬ
            button1.SetBitmaps(modulePath + "\\Resources\\Erase.bmp", modulePath + "\\Resources\\Erase.bmp");
            //���һ��"�ƶ�"��ť���Ե���ɾ������
            AcadToolbarItem button2 = tbModify.AddToolbarButton("", "�ƶ�", "�ƶ�����", "_Move ", Type.Missing);
            //�����ƶ���ť��ͼƬ
            button2.SetBitmaps(modulePath + "\\Resources\\Move.bmp", modulePath + "\\Resources\\Move.bmp");
            //���һ��"��ת"��ť���Ե�����ת����
            AcadToolbarItem button3 = tbModify.AddToolbarButton("", "��ת", "�ƻ�����ת����", "_Rotate ", Type.Missing);
            //������ת��ť��ͼƬ
            button3.SetBitmaps(modulePath + "\\Resources\\Rotate.bmp", modulePath + "\\Resources\\Rotate.bmp");

            //���һ��������ť���ð�ťֻ������������Ļ�ͼ������
            AcadToolbarItem FlyoutButton = tbModify.AddToolbarButton("", "��ͼ����", "��ͼ����", " ", true);
            //�����ڶ������������ù�������ͨ��������ť���ӵ���һ����������
            AcadToolbar tbDraw = currMenuGroup.Toolbars.Add("��ͼ������");
            //��������ֱ��ڹ����������û���Բ��ֱ�ߡ�����ߡ����εİ�ť
            AcadToolbarItem button4 = tbDraw.AddToolbarButton("", "Բ", "��ָ���뾶����Բ", "_Circle ", Type.Missing);
            button4.SetBitmaps(modulePath + "\\Resources\\Circle.bmp", modulePath + "\\Resources\\Circle.bmp");
            AcadToolbarItem button5 = tbDraw.AddToolbarButton("", "ֱ��", "����ֱ�߶�", "_Line ", Type.Missing);
            button5.SetBitmaps(modulePath + "\\Resources\\Line.bmp", modulePath + "\\Resources\\Line.bmp");
            AcadToolbarItem button6 = tbDraw.AddToolbarButton("", "�����", "������ά�����", "_Pline ", Type.Missing);
            button6.SetBitmaps(modulePath + "\\Resources\\Polyline.bmp", modulePath + "\\Resources\\Polyline.bmp");
            AcadToolbarItem button7 = tbDraw.AddToolbarButton("", "����", "�������ζ����", "_Rectangle ", Type.Missing);
            button7.SetBitmaps(modulePath + "\\Resources\\Rectangle.bmp", modulePath + "\\Resources\\Rectangle.bmp");
            //���ڶ������������ŵ���һ���������ĵ�����ť��
            FlyoutButton.AttachToolbarToFlyout(currMenuGroup.Name, tbDraw.Name);
            //��ʾ��һ��������
            tbModify.Visible = true;
            //���صڶ���������
            tbDraw.Visible = false;
        }
    }
}
