using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Autodesk.AutoCAD.EditorInput;
using AcadApp=Autodesk.AutoCAD.ApplicationServices.Application;
using Autodesk.AutoCAD.ApplicationServices;
namespace chap21
{
    public partial class CommandTools : UserControl
    {
        public CommandTools()
        {
            InitializeComponent();
        }
        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            //��ȡ�����ƶ�������ͼƬ�����
            PictureBox pictureBox = sender as PictureBox;
            //ִֻ������ƶ��������Ա�ʾ�������ϷŲ���
            if (System.Windows.Forms.Control.MouseButtons==MouseButtons.Left)
            {
                //ͼƬ����󴥷��Ϸ��¼��������ϷŲ����¼���������������ͼƬ������Name���Թ��¼������������ж�
                AcadApp.DoDragDrop(this,pictureBox.Name,DragDropEffects.All,new MyDropTarget());
            }
        }
    }

    public class MyDropTarget : Autodesk.AutoCAD.Windows.DropTarget
    {
        public override void OnDrop(System.Windows.Forms.DragEventArgs e)
        {
            Document doc = AcadApp.DocumentManager.MdiActiveDocument;
            //�жϷ����зŲ����Ķ��������
            switch ((string)e.Data.GetData("Text"))
            {
                //�����ԲͼƬ�ؼ�����һ��Բ
                case "pictureBoxCircle":
                    doc.SendStringToExecute("_Circle 100,100 50 ", true, false, true);
                    break;
                //�����ֱ��ͼƬ�ؼ�����һ��ֱ��
                case "pictureBoxLine":
                    doc.SendStringToExecute("_Line 100,100 150,100  ", true, false, true);
                    break;
                //����Ƕ����ͼƬ�ؼ�����һ����ʾ�����εĶ����
                case "pictureBoxPolyline":
                    doc.SendStringToExecute("_Pline 100,100 150,100 100,150 100,100  ", true, false, true);
                    break;
                //����Ǿ���ͼƬ�ؼ�����һ������
                case "pictureBoxRectangle":
                    doc.SendStringToExecute("_Rectangle 50,150 150,50 ", true, false, true);
                    break;
            }
        }
    }
}
