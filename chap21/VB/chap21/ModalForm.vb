Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports AcadApp = Autodesk.AutoCAD.ApplicationServices.Application
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.EditorInput
Imports Autodesk.AutoCAD.Geometry
Imports Autodesk.AutoCAD.Internal
Imports AcadWnd = Autodesk.AutoCAD.Windows

Public Class ModalForm

Private Sub buttonBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonBrowse.Click
    Dim db As Database = HostApplicationServices.WorkingDatabase
    '�����ʾ�����������б���е�����
    Me.comboBoxBlockName.Items.Clear()
    '�½�һ�����ļ��Ի��򣬲����öԻ���ı������ʾ�ļ�����Ϊdwg����dxf
    Dim dlg As New AcadWnd.OpenFileDialog("ѡ��ͼ���ļ�", Nothing, "dwg;dxf", Nothing, AcadWnd.OpenFileDialog.OpenFileDialogFlags.AllowMultiple)
    '����򿪶Ի���ɹ�
    If dlg.ShowDialog = Windows.Forms.DialogResult.OK Then
        '��ʾ��ѡ���ļ���·��
        Me.labelPath.Text = "·��:  " & dlg.Filename
        '������ѡ���ļ��еĿ�
        Dim blockImport As New BlockImportClass
        blockImport.ImportBlocksFromDwg(dlg.Filename)
        '��ʼ������
        Using trans As Transaction = db.TransactionManager.StartTransaction
            '�򿪿��
            Dim bt As BlockTable = trans.GetObject(db.BlockTableId, OpenMode.ForRead)
            'ѭ����������еĿ���¼
            For Each blockRecordId As ObjectId In bt
                '�򿪿���¼����
                Dim btr As BlockTableRecord = trans.GetObject(blockRecordId, OpenMode.ForRead)
                '�������б����ֻ�����������ͷǲ��ֿ������
                If Not btr.IsAnonymous And Not btr.IsLayout Then
                    Me.comboBoxBlockName.Items.Add(btr.Name)
                End If
            Next
        End Using
    End If
    '�������б������ʾ��ĸ˳�����ڵ�һ���Ŀ���
    If Me.comboBoxBlockName.Items.Count > 0 Then
        Me.comboBoxBlockName.Text = Me.comboBoxBlockName.Items(0)
    End If
End Sub

Private Sub buttonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonCancel.Click
    '�رմ���
    Me.Close()
End Sub

Private Sub comboBoxBlockName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles comboBoxBlockName.SelectedIndexChanged
    Dim db As Database = HostApplicationServices.WorkingDatabase
    '��ȡ�����б�����
    Dim blockNames As ComboBox = sender
    '��ȡ�����б���е�ǰѡ��Ŀ���
    Dim blockName As String = blockNames.SelectedItem.ToString
    '��ʼ������
    Using trans As Transaction = db.TransactionManager.StartTransaction
        '�򿪿��
        Dim bt As BlockTable = trans.GetObject(db.BlockTableId, OpenMode.ForRead)
        '������Ϊ�����б���е�ǰѡ��Ŀ����Ŀ�
        Dim btr As BlockTableRecord = trans.GetObject(bt(blockName), OpenMode.ForRead)
        '����acmgdinternal.dll�еĿ��������������ȡ���Ԥ��ͼ��
        Dim blockThumbnail As Bitmap = BlockThumbnailHelper.GetBlockThumbanail(btr.ObjectId)
        '�����ȡ��Ԥ��ͼ���ɹ���������ͼƬ���е�ͼ��Ϊ��Ԥ��ͼ��
        If Not (blockThumbnail Is Nothing) Then
            Me.pictureBoxBlock.Image = blockThumbnail
        End If
        '���ݵ�ǰѡ��Ŀ飬���ÿ�ĵ�λ�ͱ���
        Select Case btr.Units
            Case UnitsValue.Inches
                Me.textBoxBlockUnit.Text = "Ӣ��"
                Me.textBoxBlockRatio.Text = "25.4"
            Case UnitsValue.Meters
                Me.textBoxBlockUnit.Text = "��"
                Me.textBoxBlockRatio.Text = "1000"
            Case UnitsValue.Millimeters
                Me.textBoxBlockUnit.Text = "����"
                Me.textBoxBlockRatio.Text = "1"
            Case UnitsValue.Undefined
                Me.textBoxBlockUnit.Text = "�޵�λ"
                Me.textBoxBlockRatio.Text = "1"
            Case Else
                Me.textBoxBlockUnit.Text = btr.Units.ToString
                Me.textBoxBlockRatio.Text = ""
        End Select
    End Using
End Sub

Private Sub checkBoxSameScale_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles checkBoxSameScale.CheckedChanged
    '��ȡ�������
    Dim uniformScale As CheckBox = sender
    '������������ѡ��״̬
    If uniformScale.Checked = True Then
        'Y��Z��������ű�����Xһ��
        Me.textBoxScaleY.Text = Me.textBoxScaleX.Text
        Me.textBoxScaleZ.Text = Me.textBoxScaleX.Text
        'Y��Z��������ű��������ı���������
        Me.textBoxScaleY.Enabled = False
        Me.textBoxScaleZ.Enabled = False
    Else
        'Y��Z��������ű��������ı����������
        Me.textBoxScaleY.Enabled = True
        Me.textBoxScaleZ.Enabled = True
    End If
End Sub

Private Sub textBoxScaleX_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles textBoxScaleX.TextChanged
    '���������ѡ��״̬����Y��Z��������ű�����X����һ��
    If Me.checkBoxSameScale.Checked = True Then
        Me.textBoxScaleY.Text = Me.textBoxScaleX.Text
        Me.textBoxScaleZ.Text = Me.textBoxScaleX.Text
    End If
End Sub

Private Sub checkBoxScale_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles checkBoxScale.CheckedChanged
    '��ȡ�������
    Dim bchecked As CheckBox = sender
    '������������ѡ��״̬���������������ű������ı�������
    If bchecked.Checked = True Then
        Me.textBoxScaleX.Enabled = False
        Me.textBoxScaleY.Enabled = False
        Me.textBoxScaleZ.Enabled = False
    '���򣬿������������ű������ı�������
    Else
        Me.textBoxScaleX.Enabled = True
        Me.textBoxScaleY.Enabled = True
        Me.textBoxScaleZ.Enabled = True
    End If
End Sub

Private Sub checkBoxInsertPoint_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles checkBoxInsertPoint.CheckedChanged
    '��ȡ�������
    Dim bchecked As CheckBox = sender
    '������������ѡ��״̬�����������ò������ı�������
    If bchecked.Checked = True Then
        Me.textBoxInsertPointX.Enabled = False
        Me.textBoxInsertPointY.Enabled = False
        Me.textBoxInsertPointZ.Enabled = False
    '���򣬿��������ò������ı�������
    Else
        Me.textBoxInsertPointX.Enabled = True
        Me.textBoxInsertPointY.Enabled = True
        Me.textBoxInsertPointZ.Enabled = True
    End If
End Sub

Private Sub checkBoxRotate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles checkBoxRotate.CheckedChanged
    '��ȡ�������
    Dim bchecked As CheckBox = sender
    '������������ѡ��״̬��������������ת�Ƕȵ��ı�������
    If bchecked.Checked = True Then
        Me.textBoxRotateAngle.Enabled = False
    '���򣬿�����������ת�Ƕȵ��ı�������
    Else
        Me.textBoxRotateAngle.Enabled = True
    End If
End Sub

Private Sub ModalForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '��ʼ״̬��ͼ����û�п飬������ʾΪ�޵�λ�����ͱ���Ϊ1
    Me.textBoxBlockUnit.Text = "�޵�λ"
    Me.textBoxBlockRatio.Text = "1"
End Sub

Private Sub buttonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonOK.Click
    '��ȡ����ղ��������
    Dim insertPointX As Double = Convert.ToDouble(Me.textBoxInsertPointX.Text)
    Dim insertPointY As Double = Convert.ToDouble(Me.textBoxInsertPointY.Text)
    Dim insertPointZ As Double = Convert.ToDouble(Me.textBoxInsertPointZ.Text)
    Dim insertPoint As New Point3d(insertPointX, insertPointY, insertPointZ)
    '��ȡ����յ����ű���
    Dim scaleX As Double = Convert.ToDouble(Me.textBoxScaleX.Text)
    Dim scaleY As Double = Convert.ToDouble(Me.textBoxScaleY.Text)
    Dim scaleZ As Double = Convert.ToDouble(Me.textBoxScaleZ.Text)
    Dim scale As New Scale3d(scaleX, scaleY, scaleZ)
    '��ȡ����յ���ת�Ƕ�
    Dim rotationAngle As Double = Convert.ToDouble(Me.textBoxRotateAngle.Text)
    '�رմ���
    Me.Close()
    '��������
    Dim myBlock As New MyBlock()
    myBlock.InsertBlockRef(Me.comboBoxBlockName.Text, insertPoint, scale, rotationAngle)
End Sub
End Class