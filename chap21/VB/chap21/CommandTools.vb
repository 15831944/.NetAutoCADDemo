Imports Autodesk.AutoCAD.ApplicationServices
Imports AcadApp = Autodesk.AutoCAD.ApplicationServices.Application
Public Class CommandTools

Private Sub PictureBox_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pictureBoxCircle.MouseMove, pictureBoxRectangle.MouseMove, pictureBoxPolyline.MouseMove, pictureBoxLine.MouseMove
        '��ȡ�����ƶ�������ͼƬ�����
        Dim pictureBox As Windows.Forms.PictureBox = sender
        'ִֻ������ƶ��������Ա�ʾ�������ϷŲ���
        If System.Windows.Forms.Control.MouseButtons = Windows.Forms.MouseButtons.Left Then
            'ͼƬ����󴥷��Ϸ��¼��������ϷŲ����¼���������������ͼƬ������Name���Թ��¼������������ж�
            AcadApp.DoDragDrop(Me, pictureBox.Name, Windows.Forms.DragDropEffects.All, New MyDropTarget())
        End If
End Sub

End Class
Public Class MyDropTarget
    Inherits Autodesk.AutoCAD.Windows.DropTarget

Public Overrides Sub OnDrop(ByVal e As System.Windows.Forms.DragEventArgs)
    Dim doc As Document = AcadApp.DocumentManager.MdiActiveDocument
        '�жϷ����зŲ����Ķ��������
        Select Case e.Data.GetData("Text")
            '�����ԲͼƬ�ؼ�����һ��Բ
            Case "pictureBoxCircle"
                doc.SendStringToExecute("_Circle 100,100 50 ", True, False, True)
                '�����ֱ��ͼƬ�ؼ�����һ��ֱ��
            Case "pictureBoxLine"
                doc.SendStringToExecute("_Line 100,100 150,100  ", True, False, True)
                '����Ƕ����ͼƬ�ؼ�����һ����ʾ�����εĶ����
            Case "pictureBoxPolyline"
                doc.SendStringToExecute("_Pline 100,100 150,100 100,150 100,100  ", True, False, True)
                '����Ǿ���ͼƬ�ؼ�����һ������
            Case "pictureBoxRectangle"
                doc.SendStringToExecute("_Rectangle 50,150 150,50 ", True, False, True)
        End Select
End Sub
End Class
