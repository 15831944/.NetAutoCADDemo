Imports Autodesk.AutoCAD.Runtime
Imports Autodesk.AutoCAD.Windows
Imports AcadApp = Autodesk.AutoCAD.ApplicationServices.Application
Imports Autodesk.AutoCAD.EditorInput
Imports Autodesk.AutoCAD.Geometry

Public Class ToolPaletteSample
   Private Shared ps As PaletteSet

   <CommandMethod("ShowPalette")> _
   Public Sub ShowPalette()
      '�����廹û�б�����
      If ps Is Nothing Then
        '�½�һ�������󣬱���Ϊ"�������"
        ps = New PaletteSet("�������")
        '����������С�ߴ�Ϊ�ؼ��ĳߴ�
        ps.MinimumSize = New System.Drawing.Size(150, 240)
        '�������������
        ps.Add("�����", New CommandTools())
            '����޸Ĺ��������
            ps.Add("�޸Ĺ���", New ModifyTools())
      End If
         '��ȡ�����б༭��������Ҫ��Ϊ������ת����
         Dim ed As Editor = AcadApp.DocumentManager.MdiActiveDocument.Editor
         '������ͣ������֮ǰ�����������ɼ�����������ͣ���ڴ��ڵ����
         ps.Visible = True
         '������岻ͣ���ڴ��ڵ���һ��
         ps.Dock = DockSides.None
         '������忪ʼ��λ��
         Dim pt As New Point3d(400, 800, 0)
         '��Point3d��ֵת��ΪSystem.Point��ֵ������AutoCAD�ĵ�����ת��Ϊ��Ļ���꣬�ٽ�����Ļ����ֵ����Ϊ���ĳ�ʼλ��
         ps.Location = ed.PointToScreen(pt, 0)
         '�������Ϊ��͸��״
        ps.Opacity = 50
   End Sub

   <CommandMethod("DockRight")> _
   Public Sub DockRight()
      '�ж�����Ƿ񱻴���
      If Not (ps Is Nothing) Then
         '���ͣ���ڴ��ڵ��Ҳ�
         ps.Dock = DockSides.Right
      End If
   End Sub
End Class
