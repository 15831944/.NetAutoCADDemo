using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
namespace chap20
{
    public class BlockImportClass
    {
        [CommandMethod("ImportBlocks")]
        public void ImportBlocks()
        {
            //�ⲿ�ļ���
            string filename="C:\\Blocks and Tables - Metric.dwg";
            //����ָ�����ⲿ�ļ��е����
            ImportBlocksFromDwg(filename);
        }

        public void ImportBlocksFromDwg(string sourceFileName)
        {
            DocumentCollection dm = Application.DocumentManager;
            Editor ed = dm.MdiActiveDocument.Editor;
            //��ȡ��ǰ���ݿ���ΪĿ�����ݿ�
            Database destDb = dm.MdiActiveDocument.Database;
            //����һ���µ����ݿ������ΪԴ���ݿ⣬�Զ����ⲿ�ļ��еĶ���
            Database sourceDb = new Database(false, true);
            try
            {
                //��DWG�ļ����뵽һ����ʱ�����ݿ���
                sourceDb.ReadDwgFile(sourceFileName, System.IO.FileShare.Read, true, null);
                //����һ�����������洢���ObjectId�б�
                ObjectIdCollection blockIds = new ObjectIdCollection();
                //��ȡԴ���ݿ�������������
                Autodesk.AutoCAD.DatabaseServices.TransactionManager tm = sourceDb.TransactionManager;
                //��Դ���ݿ��п�ʼ������
                using (Transaction myT = tm.StartTransaction())
                {
                    //��Դ���ݿ��еĿ��
                    BlockTable bt = (BlockTable)tm.GetObject(sourceDb.BlockTableId, OpenMode.ForRead, false);
                    //����ÿ����
                    foreach (ObjectId btrId in bt)
                    {
                        BlockTableRecord btr = (BlockTableRecord)tm.GetObject(btrId, OpenMode.ForRead, false);
                        //ֻ����������ͷǲ��ֿ鵽�����б���
                        if (!btr.IsAnonymous && !btr.IsLayout)
                        {
                            blockIds.Add(btrId);
                        }
                        btr.Dispose();
                    }
                    bt.Dispose();
                }
                //����һ��IdMapping����
                IdMapping mapping = new IdMapping();
                //��Դ���ݿ���Ŀ�����ݿ⸴�ƿ���¼
                sourceDb.WblockCloneObjects(blockIds, destDb.BlockTableId, mapping, DuplicateRecordCloning.Replace, false);
                //'������ɺ���������ʾ�����˶��ٸ������Ϣ
                ed.WriteMessage("������ " + blockIds.Count.ToString() + " ���飬�� " + sourceFileName + " ����ǰͼ��");
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                ed.WriteMessage("\nError during copy: " + ex.Message);
            }
            //������ɣ�����Դ���ݿ�
            sourceDb.Dispose();
        }
    }
}
