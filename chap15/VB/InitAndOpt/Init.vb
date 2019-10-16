Imports Autodesk.AutoCAD.Runtime
Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.EditorInput
<Assembly: ExtensionApplication(GetType(ManagedApp.Init))> 
<Assembly: CommandClass(GetType(ManagedApp.Commands))> 
Namespace ManagedApp
    Public Class Init
        Implements IExtensionApplication

        Public Sub Initialize() Implements Autodesk.AutoCAD.Runtime.IExtensionApplication.Initialize
            Dim ed As Editor = Application.DocumentManager.MdiActiveDocument.Editor
            '��AutoCAD����������ʾһЩ��Ϣ�����ǻ��ڳ�������ʱ����ʾ
            ed.WriteMessage("����ʼ��ʼ����")
        End Sub

        Public Sub Terminate() Implements Autodesk.AutoCAD.Runtime.IExtensionApplication.Terminate
            '��Visual Studio 2005�������������ʾ�����������Ϣ
            Debug.WriteLine("��������������������һЩ���������������ر�AutoCAD�ĵ�")
        End Sub
        <CommandMethod("Test")> _
        Public Sub Test()
            Dim ed As Editor = Application.DocumentManager.MdiActiveDocument.Editor
            ed.WriteMessage("Test")
        End Sub
    End Class
    Public Class Commands
        <CommandMethod("LoadAssembly")> _
        Public Sub LoadAssembly()
            Dim ed As Editor = Application.DocumentManager.MdiActiveDocument.Editor
            'Hello.dll���򼯵��ļ���
            Dim fileName As String = "C:\Hello.dll"
            '����Hello.dll����
            ExtensionLoader.Load(fileName)
            '������������ʾ��Ϣ����ʾ�û�net1_VB.dll�����Ѿ�������
            ed.WriteMessage(vbCrLf & fileName & "�����룬������Hello���в��ԣ�")
        End Sub
    End Class
End Namespace
