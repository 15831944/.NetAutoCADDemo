Imports Autodesk.AutoCAD.Runtime
Imports Autodesk.AutoCAD.Windows
Imports Autodesk.AutoCAD.ApplicationServices

Public Class AppPane

<CommandMethod("CreateAppPane")> _
Public Sub AddApplicationPane()
    '����һ�����򴰸����
    Dim appPaneButton As New Pane
    '���ô��������
    appPaneButton.Enabled = True
    appPaneButton.Visible = True
    '���ô����ʼ״̬�ǵ�����
    appPaneButton.Style = PaneStyles.Normal
    '���ô���ı���
    appPaneButton.Text = "���򴰸�"
    '��ʾ�������ʾ��Ϣ
    appPaneButton.ToolTipText = "��ӭ������.net�����磡"
    '���MouseDown�¼�������걻����ʱ����
    AddHandler appPaneButton.MouseDown, AddressOf OnAppMouseDown
    '�Ѵ�����ӵ�AutoCAD��״̬������
    Application.StatusBar.Panes.Add(appPaneButton)
End Sub

Sub OnAppMouseDown(ByVal sender As Object, ByVal e As StatusBarMouseDownEventArgs)
    '��ȡ����ť����
    Dim paneButton As Pane = CType(sender, Pane)
    Dim alertMessage As String
    '�������Ĳ������������򷵻�
    If e.Button <> Windows.Forms.MouseButtons.Left Then
        Return
    End If
    '�л�����ť��״̬
    If paneButton.Style = PaneStyles.PopOut Then '�������ť�ǵ����ģ����л�Ϊ����
        paneButton.Style = PaneStyles.Normal
        alertMessage = "���򴰸�ť������"
    Else
        paneButton.Style = PaneStyles.PopOut
        alertMessage = "���򴰸�ťû�б����¡�"
    End If
    '����״̬���Է�ӳ����ť��״̬�仯
    Application.StatusBar.Update()
    '��ʾ��ӳ����ť�仯����Ϣ
    Application.ShowAlertDialog(alertMessage)
End Sub
End Class
