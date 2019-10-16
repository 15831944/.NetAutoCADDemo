using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Windows;
using Autodesk.AutoCAD.EditorInput;
using AcadApp = Autodesk.AutoCAD.ApplicationServices.Application;
using Autodesk.AutoCAD.Geometry;
namespace chap21
{
    public partial class ToolPaletteSample
    {
        static PaletteSet ps;
        [CommandMethod("ShowPalette")]
        public void ShowPalette()
        {
            //�����廹û�б�����
            if (ps == null)
            {
                //�½�һ�������󣬱���Ϊ"�������"
                ps = new PaletteSet("�������");
                //����������С�ߴ�Ϊ�ؼ��ĳߴ�
                ps.MinimumSize = new System.Drawing.Size(150, 240);
                //�������������
                ps.Add("�����", new CommandTools());
                //����޸Ĺ��������
                ps.Add("�޸Ĺ���", new ModifyTools());
            }
            //��ȡ�����б༭��������Ҫ��Ϊ������ת����
            Editor ed = AcadApp.DocumentManager.MdiActiveDocument.Editor;
            //������ͣ������֮ǰ�����������ɼ�����������ͣ���ڴ��ڵ����
            ps.Visible = true;
            //������岻ͣ���ڴ��ڵ���һ��
            ps.Dock = DockSides.None;
            //������忪ʼ��λ��
            Point3d pt = new Point3d(400, 800, 0);
            //��Point3d��ֵת��ΪSystem.Point��ֵ������AutoCAD�ĵ�����ת��Ϊ��Ļ���꣬�ٽ�����Ļ����ֵ����Ϊ���ĳ�ʼλ��
            ps.Location = ed.PointToScreen(pt, 0);
            //�������Ϊ��͸��״
            ps.Opacity = 50;
        }

        [CommandMethod("DockRight")]
        public void DockRight()
        {
            //�ж�����Ƿ񱻴���
            if (!((ps == null)))
            {
                //���ͣ���ڴ��ڵ��Ҳ�
                ps.Dock = DockSides.Right;
            }
        }
    }
}
