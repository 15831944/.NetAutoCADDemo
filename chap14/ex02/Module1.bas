Attribute VB_Name = "Module1"
Option Explicit

Private Declare Function GetWindowThreadProcessId Lib "user32" _
    (ByVal hWnd As Long, lpdwProcessId As Long) As Long
Private Declare Function AttachThreadInput Lib "user32" _
    (ByVal idAttach As Long, ByVal idAttachTo As Long, ByVal fAttach As Long) As Long
Private Declare Function GetForegroundWindow Lib "user32" () As Long
Private Declare Function SetForegroundWindow Lib "user32" (ByVal hWnd As Long) As Long
Private Declare Function IsIconic Lib "user32" (ByVal hWnd As Long) As Long
Private Declare Function ShowWindow Lib "user32" _
    (ByVal hWnd As Long, ByVal nCmdShow As Long) As Long

Private Const SW_SHOW = 5
Private Const SW_RESTORE = 9

Public Function ForceForegroundWindow(ByVal hWnd As Long) As Boolean
   Dim ThreadID1 As Long    ' �߳�ID
   Dim ThreadID2 As Long    ' �߳�ID
   Dim nRet As Long
   
   ' ���ָ���Ĵ����Ѿ���ǰ̨�������κβ���
   If hWnd = GetForegroundWindow() Then
      ForceForegroundWindow = True
   Else
      ' ���Ȼ��ָ��������ص��̺߳͵�ǰǰ̨�������ڵ��߳�
      ThreadID1 = GetWindowThreadProcessId(GetForegroundWindow, ByVal 0&)
      ThreadID2 = GetWindowThreadProcessId(hWnd, ByVal 0&)
      
      ' ͨ����������״̬�������̷߳���ǰ����
      If ThreadID1 <> ThreadID2 Then
         Call AttachThreadInput(ThreadID1, ThreadID2, True)
         nRet = SetForegroundWindow(hWnd)
         Call AttachThreadInput(ThreadID1, ThreadID2, False)
      Else
         nRet = SetForegroundWindow(hWnd)
      End If
      
      ' �ָ����ػ�
      If IsIconic(hWnd) Then
         Call ShowWindow(hWnd, SW_RESTORE)
      Else
         Call ShowWindow(hWnd, SW_SHOW)
      End If
      
      ' ��ȷ�ط��غ���ִ�н��
      ForceForegroundWindow = CBool(nRet)
   End If
End Function

