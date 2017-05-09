using POC_SAP.Helpers;
using POC_SAP.Models;
using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;


namespace POC_SAP.Services
{
    public class SapManager
    {

        public SapTestModel QueryTableFromSap()
        {
            using (SapHelper sh = new SapHelper())
            {
                DataSet testTable = new DataSet();
                if (sh.TestConnection())
                {
                    //setting repository;
                    RfcRepository rfcRepository = sh.RfcDestination.Repository;

                    //setting RFC function 
                    IRfcFunction rfcFunction = rfcRepository.CreateFunction("ZBR_GET_SALES_DATA");

                    //setting parameters
                    rfcFunction.SetValue("I_BUKRS", "09");

                    //getting parameter table reference
                    IRfcTable fkdat = rfcFunction.GetTable("I_FKDAT");
                    //setting parameters
                    fkdat.Append();
                    fkdat.SetValue("SIGN", "I");
                    fkdat.SetValue("OPTION", "BT");
                    fkdat.SetValue("LOW", new DateTime(2016, 06, 01));
                    fkdat.SetValue("HIGH", new DateTime(2016, 06, 30));
                    #region optional
                    ////getting parameter table reference OPTIONAL
                    //IRfcTable kunag = rfcFunction.GetTable("I_KUNAG");
                    //kunag.Append();
                    ////setting parameters
                    //kunag.SetValue("SIGN", "I");
                    //kunag.SetValue("OPTION", "EQ");
                    //kunag.SetValue("LOW", "");
                    //kunag.SetValue("HIGH", "");

                    //getting parameter table reference OPTIONAL
                    //IRfcTable vbeln = rfcFunction.GetTable("I_VBELN");
                    //vbeln.Append();
                    ////setting parameters
                    //vbeln.SetValue("SIGN", "I");
                    //vbeln.SetValue("OPTION", "EQ");
                    //vbeln.SetValue("LOW", "");
                    //vbeln.SetValue("HIGH", "");


                    //{ STRUCTURE ZBRS_SALES_DATA{ VBELN: CHAR10, POSNR: NUM(6), VGBEL: CHAR10, VGPOS: NUM(6), AUBEL: CHAR10, AUPOS: NUM(6), AUART: CHAR4, AUTYP: CHAR1, BUKRS: CHAR4, KUNAG: CHAR10, NAME1: CHAR35, KUNAG_END: CHAR10, NAME1_END: CHAR35, FKDAT: DATE, XBLNR: CHAR16, BSTKD: CHAR35, MATNR: CHAR18, KDMAT: CHAR35, FKIMG: BCD[7:3], VRKME: CHAR3} }
                    //IRfcTable tdata = rfcFunction.GetTable("T_DATA");
                    //tdata.Append();
                    ////tdata.SetValue("VBELN", "");
                    //tdata.SetValue("FKDAT", new DateTime(2016, 12, 31));
                    ////tdata.SetValue("MATNR", "*.*");
                    #endregion
                    //Execute query
                    rfcFunction.Invoke(sh.RfcDestination);
                    //Getting response table

                    IRfcTable tblData = rfcFunction.GetTable("T_DATA");
                    IRfcTable tblReturn1 = rfcFunction.GetTable("I_VBELN");
                    //traing to convert to datatable
                    DataTable dt = sh.ConvertToTable(tblData);
                    testTable.Tables.Add(dt);
                }
                return ParseToModel(testTable);
            }
        }


        private SapTestModel ParseToModel(DataSet dataSet)
        {
            SapTestModel model = new SapTestModel();

            foreach (DataTable table in dataSet.Tables)
            {

                List<string> names = new List<string>();

                foreach (DataColumn item in table.Columns)
                {
                    names.Add(item.ColumnName);
                }

                model.Data = new List<Row>();
                model.ColumnsNames = names;

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    Row row = new Row() { Columns = new List<Column>() };
                    for (int j = 0; j < names.Count; j++)
                    {
                        Column col = new Column()
                        {
                            PropertyValue = table.Rows[i].ItemArray[j].ToString()
                        };

                        row.Columns.Add(col);
                    }
                    model.Data.Add(row);
                }
            }
            return model;
        }
    }
}