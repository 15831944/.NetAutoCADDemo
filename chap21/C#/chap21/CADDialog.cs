using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
namespace chap21
{
    public class CADDialog
    {
        [CommandMethod("ModalForm")]
        public void ShowModalForm()
        {
            //��ʾģ̬�Ի���
            ModalForm modalForm =new ModalForm();
            Application.ShowModalDialog(modalForm);
        }
    }
}
