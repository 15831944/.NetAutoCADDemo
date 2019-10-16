using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;

namespace chap21
{
    public class TabbedDialog
    {
        [CommandMethod("CreateNewOptionsTab")]
        public void CreateNewOptionsTab()
        {
            //��DisplayingOptionDialog�¼�������ʱ����ѡ��Ի�����ʾ��������displayingOptionDialog����
            Application.DisplayingOptionDialog += new TabbedDialogEventHandler(displayingOptionDialog);
        }

        void displayingOptionDialog(object sender, TabbedDialogEventArgs e)
        {
            //����һ���Զ���ؼ���ʵ��������ʹ�õ�����ʾͼƬ���Զ���ؼ�
            PictureTab optionTab = new PictureTab();
            //Ϊȷ����ť��Ӷ�����Ҳ����Ϊȡ����Ӧ�á�������ť��Ӷ���
            TabbedDialogAction onOkPress =new TabbedDialogAction(OnOptionOK);
            //����ʾͼƬ�Ŀؼ���Ϊ��ǩҳ��ӵ�ѡ��Ի��򣬲�����ȷ����ť�������Ķ���
            e.AddTab("ͼƬ", new TabbedDialogExtension(optionTab, onOkPress));
        }

        void OnOptionOK()
        {
            //��ȷ����ť������ʱ����ʾһ������Ի���
            Application.ShowAlertDialog("���ѡ��Ի����ǩҳ�ɹ���");
        }
    }
}
