using System;

using Autodesk.AutoCAD.Runtime;

using Autodesk.AutoCAD.Windows;

using Autodesk.AutoCAD.ApplicationServices;

namespace chap21
{
    public class AppPane
    {
        [CommandMethod("CreateAppPane")]
        public void AddApplicationPane()
        {
            //����һ�����򴰸����
            Pane appPaneButton = new Pane();
            //���ô��������
            appPaneButton.Enabled = true;
            appPaneButton.Visible = true;
            //���ô����ʼ״̬�ǵ�����
            appPaneButton.Style = PaneStyles.Normal;
            //���ô���ı���
            appPaneButton.Text = "���򴰸�";
            //��ʾ�������ʾ��Ϣ
            appPaneButton.ToolTipText = "��ӭ������.net�����磡";
            //���MouseDown�¼�������걻����ʱ����
            appPaneButton.MouseDown += new StatusBarMouseDownEventHandler(OnAppMouseDown);
            //�Ѵ�����ӵ�AutoCAD��״̬������
            Application.StatusBar.Panes.Add(appPaneButton);
        }

        static void OnAppMouseDown(object sender, StatusBarMouseDownEventArgs e)
        {
            //��ȡ����ť����
            Pane paneButton = (Pane)sender;
            string alertMessage;
            //�������Ĳ������������򷵻�
            if (e.Button!=System.Windows.Forms.MouseButtons.Left)
            {
                return;
            }
            //�л�����ť��״̬
            if (paneButton.Style == PaneStyles.PopOut)//�������ť�ǵ����ģ����л�Ϊ����
            {
                paneButton.Style = PaneStyles.Normal;
                alertMessage = "���򴰸�ť������";
            }
            else
            {
                paneButton.Style = PaneStyles.PopOut;
                alertMessage = "���򴰸�ťû�б�����";
            }
            //����״̬���Է�ӳ����ť��״̬�仯
            Application.StatusBar.Update();
            //��ʾ��ӳ����ť�仯����Ϣ
            Application.ShowAlertDialog(alertMessage);
        }
    }
}
