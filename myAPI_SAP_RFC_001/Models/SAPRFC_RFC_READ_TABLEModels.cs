using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace myAPI_SAP_RFC_001.Models
{
    public class SAPRFC_RFC_READ_TABLEModel
    {
        public string Result { get; set; }
        public string iQueryTable { get; set; }

        public void Main()
        {
            SAPLogonCtrl.SAPLogonControlClass login = new SAPLogonCtrl.SAPLogonControlClass();

            login.ApplicationServer = ConfigurationManager.AppSettings["lg_ApplicationServer"];
            login.Client = ConfigurationManager.AppSettings["lg_Client"];
            login.Language = ConfigurationManager.AppSettings["lg_Language"];
            login.User = ConfigurationManager.AppSettings["lg_User"];
            login.Password = ConfigurationManager.AppSettings["lg_Password"];
            login.SystemNumber = Int32.Parse(  ConfigurationManager.AppSettings["lg_SystemNumber"]);

            string str_json = string.Empty;

            SAPLogonCtrl.Connection conn = (SAPLogonCtrl.Connection)login.NewConnection();
            //conn.SAPRouter
            try
            {

                if (conn.Logon(0, true))
                {
                    //lblLogonStatus.Text = "登入SAP成功";

                    SAPFunctionsOCX.SAPFunctionsClass func = new SAPFunctionsOCX.SAPFunctionsClass();
                    func.Connection = conn;

                    SAPFunctionsOCX.IFunction ifunc = (SAPFunctionsOCX.IFunction)func.Add("RFC_READ_TABLE");

                    SAPFunctionsOCX.IParameter iQUERY_TABLE = (SAPFunctionsOCX.IParameter)ifunc.get_Exports("QUERY_TABLE");
                    iQUERY_TABLE.Value = "USR41";
                    SAPFunctionsOCX.IParameter iDELIMITER = (SAPFunctionsOCX.IParameter)ifunc.get_Exports("DELIMITER");
                    iDELIMITER.Value = "|";

                    SAPTableFactoryCtrl.Tables tables = (SAPTableFactoryCtrl.Tables)ifunc.Tables;

                    SAPTableFactoryCtrl.Table tOPTIONS = (SAPTableFactoryCtrl.Table)tables.get_Item("OPTIONS");
                    tOPTIONS.AppendGridData(1, 1, 1, "");

                    SAPTableFactoryCtrl.Table tFIELDS = (SAPTableFactoryCtrl.Table)tables.get_Item("FIELDS");
                    tFIELDS.AppendGridData(1, 1, 1, "BNAME");
                    tFIELDS.AppendGridData(1, 2, 1, "TERMINAL");

                    ifunc.Call();

                    SAPTableFactoryCtrl.Table tDATA = (SAPTableFactoryCtrl.Table)tables.get_Item("DATA");

                    //得到一個DataTable物件
                    DataTable dt = new DataTable();
                    DataTable dt2 = new DataTable();

                    for (int i = 1; i <= tFIELDS.RowCount; i++)
                    {
                        dt.Columns.Add(tFIELDS.get_Cell(i, 1).ToString());
                    } 

                    for (int m = 1; m <= tDATA.RowCount; m++)
                    {
                        DataRow dr = dt.NewRow();
                        int StringStart = 0;
                        for (int n = 1; n <= tFIELDS.RowCount; n++)
                        {
                            int StringEnd = tDATA.get_Cell(m, 1).ToString().IndexOf("|", StringStart);
                            if (StringEnd == -1)
                            {
                                StringEnd = tDATA.get_Cell(m, 1).ToString().Length;
                            }
                            int StringLength = StringEnd - StringStart;
                            dr[tFIELDS.get_Cell(n, 1).ToString()] = tDATA.get_Cell(m, 1).ToString().Substring(StringStart, StringLength);
                            StringStart = StringEnd + 1;
                        }
                        dt.Rows.Add(dr);
                    }

                    //將DataTable轉成JSON字串
                    str_json = JsonConvert.SerializeObject(dt, Formatting.Indented);

                    dt2 = JsonConvert.DeserializeObject<DataTable>(str_json);


                    conn.Logoff();
                }
                else
                {
                    throw new Exception("Logon Fail");
                }
                Result = str_json;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}