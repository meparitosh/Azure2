using System;
using System.Data;
using System.Configuration;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;



public partial class TestPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Service obj = new Service();
        DataSet ds = new DataSet();
        FullFinal.LMSDatabase ObjLmsDataBase = new FullFinal.LMSDatabase();
        ObjLmsDataBase.OpenConnection();
        ObjLmsDataBase.BeginTransaction();
        ObjLmsDataBase.ExecuteQueryStoredProcedure("LMS.LMS_LWP_HistoryLWPData", ref ds);
        ObjLmsDataBase.CommitTransaction();        
        Service.SAP_Response_LWP []  Abc = new Service.SAP_Response_LWP[ds.Tables[0].Rows.Count];
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            Service.SAP_Response_LWP k = new Service.SAP_Response_LWP();
            k.strReponseCode = ds.Tables[0].Rows[i]["SAPPI_LWP_ResponseCode"].ToString();
            k.strResponseText = ds.Tables[0].Rows[i]["SAPPI_LWP_ResponseText"].ToString();
            k.strTranId = ds.Tables[0].Rows[i]["Tranid"].ToString();
            Abc[i] = k;
        }
         obj.LWPResponseDetails(Abc);

    }    
  
    





    }

