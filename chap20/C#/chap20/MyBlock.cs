using System;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
namespace chap20
{
    public class MyBlock
    {
        [CommandMethod("CB")]
        public ObjectId CreateBlock()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            ObjectId blockId = ObjectId.Null;//���ڷ����������Ŀ�Ķ���Id
            BlockTableRecord record = new BlockTableRecord();//����һ��BlockTableRecord��Ķ��󣬱�ʾ��Ҫ�����Ŀ�
            record.Name = "Room";//���ÿ���            
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                Point3dCollection points = new Point3dCollection();//���ڱ�ʾ��ɿ�Ķ���ߵĶ���
                points.Add(new Point3d(-18.0, -6.0, 0.0));
                points.Add(new Point3d(18.0, -6.0, 0.0));
                points.Add(new Point3d(18.0, 6.0, 0.0));
                points.Add(new Point3d(-18.0, 6.0, 0.0));
                Polyline2d pline = new Polyline2d(Poly2dType.SimplePoly, points, 0.0, true, 0.0, 0.0, null);//������ɿ�Ķ����
                record.Origin = points[3];//���ÿ�Ļ���
                record.AppendEntity(pline);//������߼��뵽�½���BlockTableRecord����
                BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForWrite);//��д�ķ�ʽ�򿪿��
                if (!bt.Has("Room"))//�ж��Ƿ������Ϊ"Room"�Ŀ�
                {
                    blockId = bt.Add(record);//�ڿ���м���"Room"��
                    trans.AddNewlyCreatedDBObject(record, true);//֪ͨ������
                    trans.Commit();//�ύ����
                }
            }
            return blockId;//���ش����Ŀ�Ķ���Id
        }
        [CommandMethod("CBWA")]
        public ObjectId CreateBlockWithAttributes()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            ObjectId blockId = ObjectId.Null;
            BlockTableRecord record = new BlockTableRecord();
            record.Name = "RMNUM";
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                Point3dCollection points = new Point3dCollection();
                points.Add(new Point3d(-18.0, -6.0, 0.0));
                points.Add(new Point3d(18.0, -6.0, 0.0));
                points.Add(new Point3d(18.0, 6.0, 0.0));
                points.Add(new Point3d(-18.0, 6.0, 0.0));
                Polyline2d pline = new Polyline2d(Poly2dType.SimplePoly, points, 0.0, true, 0.0, 0.0, null);
                record.AppendEntity(pline);
                AttributeDefinition attdef = new AttributeDefinition();
                attdef.Position = new Point3d(0.0, 0.0, 0.0);
                attdef.Height = 8.0;//�������ָ߶�  
                attdef.Rotation = 0.0;//����������ת�Ƕ�  
                attdef.HorizontalMode = TextHorizontalMode.TextMid;//����ˮƽ���뷽ʽ 
                attdef.VerticalMode = TextVerticalMode.TextVerticalMid;//���ô�ֱ���뷽ʽ 
                attdef.Prompt = "Room Number:";//����������ʾ 
                attdef.TextString = "0000";//�������Ե�ȱʡֵ  
                attdef.Tag = "NUMBER";//�������Ա�ǩ  
                attdef.Invisible = false;//���ò��ɼ�ѡ��Ϊ��  
                attdef.Verifiable = false;//������֤��ʽΪ��  
                attdef.Preset = false;//����Ԥ�÷�ʽΪ�� 
                attdef.Constant = false;//���ó�����ʽΪ��
                record.Origin = points[3];
                record.AppendEntity(attdef);
                BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForWrite);
                if (!bt.Has("RMNUM")) //�ж��Ƿ������Ϊ"RMNUM"�Ŀ�
                {
                    blockId = bt.Add(record); //�ڿ���м���"RMNUM"��
                    trans.AddNewlyCreatedDBObject(record, true);
                    trans.Commit();
                }
            }
            return blockId;
        }
        [CommandMethod("InsertBlock")]
        public void InsertBlock()
        {
            //����һ���������Եļ򵥿�"Room"
            InsertBlockRef("Room", new Point3d(100, 100, 0), new Scale3d(2), 0);
        }

        public void InsertBlockRef(string blockName, Point3d point, Scale3d scale, double rotateAngle)
        {
            Database db = HostApplicationServices.WorkingDatabase;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                //�Զ��ķ�ʽ�򿪿��
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                //���û��blockName��ʾ�Ŀ飬����򷵻�
                if (!bt.Has(blockName))
                {
                    return;
                }
                //�Զ��ķ�ʽ��blockName��ʾ�Ŀ�
                BlockTableRecord block = (BlockTableRecord)trans.GetObject(bt[blockName], OpenMode.ForRead);
                //����һ������ղ����ò����
                BlockReference blockRef = new BlockReference(point, bt[blockName]);
                blockRef.ScaleFactors = scale;//���ÿ���յ����ű���
                blockRef.Rotation = rotateAngle;//���ÿ���յ���ת�Ƕ�
                //��д�ķ�ʽ�򿪵�ǰ�ռ䣨ģ�Ϳռ��ͼֽ�ռ䣩
                BlockTableRecord btr = (BlockTableRecord)trans.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
                btr.AppendEntity(blockRef);//�ڵ�ǰ�ռ���봴���Ŀ����
                //֪ͨ��������봴���Ŀ����
                trans.AddNewlyCreatedDBObject(blockRef, true);
                trans.Commit();//�ύ��������ʵ�ֿ���յ���ʵ����
            }
        }

        [CommandMethod("InsertBlockWithAtt")]
        public void InsertBlockWithAtt()
        {
            //����һ�������ԵĿ�"RMNUM"
            InsertBlockRefWithAtt("RMNUM", new Point3d(100, 150, 0), new Scale3d(1), 0.5 * Math.PI, "2007");
            //����һ���������Եļ򵥿�"Room"
            InsertBlockRefWithAtt("Room", new Point3d(100, 200, 0), new Scale3d(1), 0, null);
        }
        public void InsertBlockRefWithAtt(string blockName, Point3d point, Scale3d scale, double rotateAngle, string roomnumber)
        {
            Database db = HostApplicationServices.WorkingDatabase;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                if (!bt.Has(blockName))
                {
                    return;
                }
                BlockTableRecord blockwithatt = (BlockTableRecord)trans.GetObject(bt[blockName], OpenMode.ForRead);
                BlockReference blockRef = new BlockReference(point, bt[blockName]);
                BlockTableRecord btr = (BlockTableRecord)trans.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
                blockRef.ScaleFactors = scale;
                blockRef.Rotation = rotateAngle;
                btr.AppendEntity(blockRef);
                trans.AddNewlyCreatedDBObject(blockRef, true);
                //��ȡblockName��ı���������ʵ�ֶԿ��ж���ķ���
                BlockTableRecordEnumerator iterator = blockwithatt.GetEnumerator();
                //���blockName���������
                if (blockwithatt.HasAttributeDefinitions)
                {
                    //���ÿ�������Կ��еĶ�����б���
                    while (iterator.MoveNext())
                    {
                        //��ȡ���������ǰָ��Ŀ��еĶ���
                        AttributeDefinition attdef = trans.GetObject(iterator.Current, OpenMode.ForRead) as AttributeDefinition;
                        //����һ���µ����Բ��ն���
                        AttributeReference att = new AttributeReference();
                        //�жϿ��������ǰָ��Ŀ��еĶ����Ƿ�Ϊ���Զ���
                        if (attdef != null)
                        {
                            //�����Զ�������м̳���ص����Ե����Բ��ն�����
                            att.SetAttributeFromBlock(attdef, blockRef.BlockTransform);
                            //�������Բ��ն����λ��Ϊ���Զ����λ��+����յ�λ��
                            att.Position = attdef.Position + blockRef.Position.GetAsVector();
                            //�ж����Զ��������
                            switch (attdef.Tag)
                            {
                                //���Ϊ"NUMBER"�������ÿ���յ�����ֵ
                                case "NUMBER":
                                    att.TextString = roomnumber;
                                    break;
                            }
                            //�жϿ�����Ƿ��д���粻��д�����л�Ϊ��д״̬
                            if (!blockRef.IsWriteEnabled)
                            {
                                blockRef.UpgradeOpen();
                            }
                            //����´��������Բ���
                            blockRef.AttributeCollection.AppendAttribute(att);
                            //֪ͨ����������´��������Բ���
                            trans.AddNewlyCreatedDBObject(att, true);
                        }
                    }
                }
                trans.Commit();//�ύ������
            }
        }

        [CommandMethod("BrowseBlock")]
        public void BrowseBlock()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Database db = HostApplicationServices.WorkingDatabase;
            //ֻѡ�����ն���
            TypedValue[] filterValues = { new TypedValue((int)DxfCode.Start, "INSERT") };
            SelectionFilter filter = new SelectionFilter(filterValues);
            PromptSelectionOptions opts = new PromptSelectionOptions();
            //��ʾ�û�����ѡ��
            opts.MessageForAdding = "��ѡ��ͼ���еĿ����";
            //����ѡ�������ѡ����󣬱�����Ϊ����ն���
            PromptSelectionResult res = ed.GetSelection(opts, filter);
            //���ѡ��ʧ�ܣ��򷵻�
            if (res.Status != PromptStatus.OK)
                return;
            //��ȡѡ�񼯶��󣬱�ʾ��ѡ��Ķ��󼯺�
            SelectionSet ss = res.Value;
            //��ȡѡ���а����Ķ���ID�����ڷ���ѡ���еĶ���
            ObjectId[] ids = ss.GetObjectIds();
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                //ѭ������ѡ���еĶ���
                foreach (ObjectId blockId in ids)
                {
                    //��ȡѡ���е�ǰ�Ķ���������ֻѡ�����ն�������ֱ�Ӹ�ֵΪ�����
                    BlockReference blockRef = (BlockReference)trans.GetObject(blockId, OpenMode.ForRead);
                    //��ȡ��ǰ����ն��������Ŀ���¼����
                    BlockTableRecord btr = (BlockTableRecord)trans.GetObject(blockRef.BlockTableRecord, OpenMode.ForRead);
                    //������������ʾ����
                    ed.WriteMessage("\n�飺" + btr.Name);
                    //���ٿ���¼����
                    btr.Dispose();
                    //��ȡ����յ����Լ��϶���
                    AttributeCollection atts = blockRef.AttributeCollection;
                    //ѭ���������Լ��϶���
                    foreach (ObjectId attId in atts)
                    {
                        //��ȡ��ǰ�Ŀ��������
                        AttributeReference attRef = (AttributeReference)trans.GetObject(attId, OpenMode.ForRead);
                        //��ȡ��������Ե�������������ֵ
                        string attString = " ��������" + attRef.Tag + " ����ֵ��" + attRef.TextString;
                        //������������ʾ���������������ֵ
                        ed.WriteMessage(attString);
                    }
                }
            }
        }

        [CommandMethod("ChangeBlockAtt")]
        public void ChangeBlockAtt()
        {
            string roomNumber = "2008";
            ChangeBlock("RMNUM", roomNumber);
        }

        public void ChangeBlock(string blockName, string roomNumber)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Database db = HostApplicationServices.WorkingDatabase;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                //�򿪿��
                BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                //�򿪿������ΪblockName�Ŀ���¼
                BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[blockName], OpenMode.ForRead);
                //��ȡ������ΪblockName�Ŀ����
                ObjectIdCollection blcokRefIds = btr.GetBlockReferenceIds(true, true);
                //ѭ�����������
                foreach (ObjectId blockRefId in blcokRefIds)
                {
                    //�򿪵�ǰ�����
                    BlockReference blockRef = (BlockReference)trans.GetObject(blockRefId, OpenMode.ForRead);
                    //��ȡ��ǰ����յ����Լ���
                    AttributeCollection blockRefAtts = blockRef.AttributeCollection;
                    //ѭ���������Լ���
                    foreach (ObjectId attId in blockRefAtts)
                    {
                        //��ȡ��ǰ���Բ��ն���
                        AttributeReference attRef = (AttributeReference)trans.GetObject(attId, OpenMode.ForRead);
                        //ֻ�ı�NUMBER����ֵΪ"0000"������ֵΪroomNumber
                        switch (attRef.Tag)
                        {
                            case "NUMBER":
                                if (attRef.TextString == "0000")
                                {
                                    attRef.UpgradeOpen();//�л����Բ��ն���Ϊ��д״̬
                                    attRef.TextString = roomNumber;
                                }
                                break;
                        }
                    }
                }
                trans.Commit();
            }
        }
    }
}



