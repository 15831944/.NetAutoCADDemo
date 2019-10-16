VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "comdlg32.ocx"
Begin VB.Form frmMain 
   Caption         =   "���������滻"
   ClientHeight    =   3885
   ClientLeft      =   60
   ClientTop       =   450
   ClientWidth     =   4500
   LinkTopic       =   "Form1"
   ScaleHeight     =   3885
   ScaleWidth      =   4500
   StartUpPosition =   3  'Windows Default
   Begin MSComDlg.CommonDialog comDlg 
      Left            =   3600
      Top             =   480
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
   End
   Begin VB.TextBox txtFind 
      Height          =   300
      Left            =   120
      TabIndex        =   6
      Top             =   480
      Width           =   1335
   End
   Begin VB.TextBox txtReplace 
      Height          =   300
      Left            =   2280
      TabIndex        =   5
      Top             =   480
      Width           =   1335
   End
   Begin VB.ListBox lstFile 
      Height          =   2010
      Left            =   120
      TabIndex        =   4
      Top             =   1200
      Width           =   4215
   End
   Begin VB.CommandButton cmdOpen 
      Caption         =   "���(&A)"
      Height          =   375
      Left            =   120
      TabIndex        =   3
      Top             =   3360
      Width           =   855
   End
   Begin VB.CommandButton cmdDelete 
      Caption         =   "ɾ��(&D)"
      Height          =   375
      Left            =   1080
      TabIndex        =   2
      Top             =   3360
      Width           =   855
   End
   Begin VB.CommandButton cmdOk 
      Caption         =   "ȷ��(&O)"
      Height          =   375
      Left            =   2040
      TabIndex        =   1
      Top             =   3360
      Width           =   1095
   End
   Begin VB.CommandButton cmdCancel 
      Caption         =   "ȡ��(&C)"
      Height          =   375
      Left            =   3240
      TabIndex        =   0
      Top             =   3360
      Width           =   1095
   End
   Begin VB.Label Label1 
      Caption         =   "�����滻��"
      Height          =   255
      Left            =   120
      TabIndex        =   9
      Top             =   120
      Width           =   975
   End
   Begin VB.Label Label2 
      Caption         =   "�滻Ϊ->"
      Height          =   255
      Left            =   1560
      TabIndex        =   8
      Top             =   555
      Width           =   735
   End
   Begin VB.Label Label3 
      Caption         =   "�ļ��б�"
      Height          =   255
      Left            =   120
      TabIndex        =   7
      Top             =   900
      Width           =   1815
   End
End
Attribute VB_Name = "frmMain"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Dim acadApp As AcadApplication      ' AutoCADӦ�ó������
Dim acadDoc As AcadDocument         ' ��ǰ��ĵ�����

Const LB_ITEMFROMPOINT = &H1A9

Private Declare Function SendMessage Lib "user32" Alias "SendMessageA" _
        (ByVal hWnd As Long, ByVal wMsg As Long, _
        ByVal wParam As Long, lParam As Any) As Long

Private Sub cmdCancel_Click()
    acadApp.Quit
    End
End Sub

Private Sub cmdDelete_Click()
    ' ȷ���б������б���
    If lstFile.ListCount >= 1 Then
        ' ���û��ѡ�е����ݣ�����һ�ε��б���
        If lstFile.ListIndex = -1 Then
            MsgBox "��ѡ���б��е�ͼ�����ƣ�"
            Exit Sub
        End If
        lstFile.RemoveItem (lstFile.ListIndex)
    End If
End Sub

Private Sub cmdOk_Click()
    Dim adText As AcadText
    Dim adMText As AcadMText
    Dim adSS As AcadSelectionSet
    Dim fType(0 To 1) As Integer, fData(0 To 1)
    Dim i As Integer
    
    If txtFind.Text = "" Or txtReplace.Text = "" Then
        MsgBox "������Ҫ�滻���ַ������ݣ�"
        Exit Sub
    End If
    If lstFile.ListCount = 0 Then
        MsgBox "�������Ҫ������ͼ�Σ�"
        Exit Sub
    End If
    
    ' ����滻����
    Dim strFind As String
    Dim strReplace As String
    strFind = txtFind.Text
    strReplace = txtReplace.Text

    ' ��ͼ�ν��в���
    For i = 0 To lstFile.ListCount - 1
        Call ReplaceTextInDwg(lstFile.List(i), strFind, strReplace)
    Next i
    
    ' ���˳�Ӧ�ó���֮ǰ�ر�AutoCAD
    acadApp.Quit
    End
End Sub

Private Sub cmdOpen_Click()
    On Error GoTo errHandle
    
    Dim i As Integer
    Dim Y As Integer
    Dim Z As Integer
    Dim fileNames() As String
    
    With comDlg
        .CancelError = True
        .MaxFileSize = 32767
        .Flags = cdlOFNHideReadOnly Or cdlOFNAllowMultiselect Or cdlOFNExplorer Or cdlOFNNoDereferenceLinks
        .DialogTitle = "ѡ��ͼ���ļ�"
        .Filter = "ͼ���ļ�(*.dwg)|*.dwg|�����ļ�(*.*)|*.*"
        .FileName = ""
        .ShowOpen
    End With
    
    comDlg.FileName = comDlg.FileName & Chr(0)  '��Щ�ļ������ÿ��ַ�Chr(0)�ָ����������ǿո�ָ�������
    
    Z = 1
    For i = 1 To Len(comDlg.FileName)
        'InStr���������� Variant (Long)��ָ��һ�ַ�������һ�ַ��������ȳ��ֵ�λ��
        '�﷨ InStr(���λ��, string1, string2)
        i = InStr(Z, comDlg.FileName, Chr(0))
        If i = 0 Then Exit For
        ReDim Preserve fileNames(Y)
        'Mid���������� Variant (String)�����а����ַ�����ָ���������ַ�
        '�﷨ Mid(string, start[, length])
        fileNames(Y) = Mid(comDlg.FileName, Z, i - Z)
        Z = i + 1
        Y = Y + 1
    Next i

    '���б������Ӷ���
    Dim count As Integer
    count = lstFile.ListCount
    If Y = 1 Then
        If Not HasItem(fileNames(Y - 1)) Then
            lstFile.AddItem fileNames(Y - 1), count
        End If
    Else
        For i = 1 To Y - 1
            If StrComp(Right$(fileNames(0), 1), "\") = 0 Then
                fileNames(i) = fileNames(0) & fileNames(i)
            Else
                fileNames(i) = fileNames(0) & "\" & fileNames(i)
            End If
            
            If Not HasItem(fileNames(i)) Then
                lstFile.AddItem fileNames(i), i - 1 + count
            End If
        Next i
    End If

errHandle:

End Sub

Private Sub Form_Load()
    On Error Resume Next
    ' ����������е�AutoCADӦ�ó������
    Set acadApp = GetObject(, "AutoCAD.Application.17")

    If Err Then
        Err.Clear
        ' ����һ���µ�AutoCADӦ�ó������
        Set acadApp = CreateObject("AutoCAD.Application.17")
        
        If Err Then
            MsgBox Err.Description
            Exit Sub
        End If
    End If
    
    ' ��ʾAutoCADӦ�ó���
    acadApp.Visible = True
    
    lstFile.Clear
End Sub

' ��ĳ��ͼ�ν��������滻
Private Sub ReplaceTextInDwg(ByVal strDwgName As String, ByVal strFind As String, _
                            ByVal strReplace As String)
    ' ��ָ����ͼ��
    acadApp.Documents.Open strDwgName
    Set acadDoc = acadApp.ActiveDocument
    
    Dim ent As AcadEntity
    For Each ent In acadDoc.ModelSpace
        If TypeOf ent Is AcadText Or TypeOf ent Is AcadMText Then
            With ent
                If InStr(.TextString, strFind) Then .TextString = ReplaceStr(.TextString, strFind, strReplace, False)
            End With
        End If
    Next ent
    
    ' ���沢�ر�ͼ��
    acadDoc.Close True
End Sub

' ���ַ�����ָ�����ַ������滻
Public Function ReplaceStr(ByVal searchStr As String, ByVal oldStr As String, _
        ByVal newStr As String, ByVal firstOnly As Boolean) As String
    '�Դ�������Ĵ���
    If searchStr = "" Then Exit Function
    If oldStr = "" Then Exit Function

    ReplaceStr = ""
    Dim i As Integer, oldStrLen As Integer, holdStr As String, StrLoc As Integer
    
    '����ԭ���ַ����ĳ���
    oldStrLen = Len(oldStr)
    StrLoc = InStr(searchStr, oldStr)
    
    While StrLoc > 0
        '���ͼ�������ֶ���λ�ڲ����ַ���֮ǰ���ַ���
        holdStr = holdStr & Left(searchStr, StrLoc - 1) & newStr
        '������ֶ���λ�ڲ����ַ���֮����ַ���
        searchStr = Mid(searchStr, StrLoc + oldStrLen)
        StrLoc = InStr(searchStr, oldStr)
        If firstOnly Then ReplaceStr = holdStr & searchStr: Exit Function
    Wend
    
    ReplaceStr = holdStr & searchStr
End Function

' �б�����Ƿ����ָ�����Ƶ���Ŀ
Private Function HasItem(ByVal strDwgName As String) As Boolean
    HasItem = False
    
    Dim i As Integer
    For i = 0 To lstFile.ListCount - 1
        If StrComp(lstFile.List(i), strDwgName, vbTextCompare) = 0 Then
            HasItem = True
            Exit Function
        End If
    Next i
End Function

Private Sub lstFile_DblClick()
    Dim pt As Variant
    ' �������л���AutoCAD
    ForceForegroundWindow acadApp.hWnd
    pt = acadApp.ActiveDocument.Utility.GetPoint(, "ʰȡһ�㣺")
    
    ' �����л��ص�ǰ�Ĵ���
    ForceForegroundWindow frmMain.hWnd
    
    ' ��ʾ�������
    MsgBox "ʰȡ�������Ϊ��(" & pt(0) & "," & pt(1) & "," & pt(2) & ")"
End Sub

Private Sub lstFile_MouseMove(Button As Integer, Shift As Integer, X As Single, Y As Single)
    Dim lXPoint As Long
    Dim lYPoint As Long
    Dim lIndex As Long
    
    If Button = 0 Then ' ȷ�����ƶ�����ͬʱû�а��¹��ܼ���������
        ' ��ù���λ�ã�������Ϊ��λ
        lXPoint = CLng(X / Screen.TwipsPerPixelX)
        lYPoint = CLng(Y / Screen.TwipsPerPixelY)
        
        ' ��ʾ�б����Ԫ�ص�����
        With lstFile
            ' ��ù�����ڵ��е�����
            lIndex = SendMessage(.hWnd, LB_ITEMFROMPOINT, 0, _
                            ByVal ((lYPoint * 65536) + lXPoint))
            
            ' ��ListBox��Tooltip��������Ϊ���е��ı�
            If (lIndex >= 0) And (lIndex <= .ListCount) Then
                .ToolTipText = .List(lIndex)
            Else
                .ToolTipText = ""
            End If
        End With
    End If
End Sub


