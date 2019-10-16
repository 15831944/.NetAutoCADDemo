using System;
using System.Diagnostics;
using Autodesk.AutoCAD.Runtime; 
using Autodesk.AutoCAD.ApplicationServices; 
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;

[assembly: ExtensionApplication(typeof(ManagedApp.Init))]
[assembly: CommandClass(typeof(ManagedApp.Commands))]
namespace ManagedApp
{
    public class Init : IExtensionApplication
    {

        public void Initialize()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("����ʼ��ʼ����");
        }

        public void Terminate()
        {
            Debug.WriteLine("��������������������һЩ���������������ر�AutoCAD�ĵ�");
        }

        [CommandMethod("Test")]
        public void Test()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("Test");
        }
    }
    public class Commands
    {

        [CommandMethod("LoadAssembly")]
        public void LoadAssembly()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            string fileName = "C:\\net1_VB.dll";
            ExtensionLoader.Load(fileName);
            ed.WriteMessage("\n" + fileName + "�����룬������Hello���в��ԣ�");
        }
        [CommandMethod("GetVersion")]
        public void GetVersion()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            foreach (DwgVersion ver in db.GetSupportedDxfOutVersions())
            {
                ed.WriteMessage(ver.ToString());
            }
            foreach (DwgVersion ver in db.GetSupportedSaveVersions())
            {
                ed.WriteMessage(ver.ToString());
            }
        }
    }
}
