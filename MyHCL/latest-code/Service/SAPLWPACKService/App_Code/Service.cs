using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Text;
using System.Data;
using System.Data.SqlClient;
//using Impersonation;
using System.Configuration; 

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class Service : System.Web.Services.WebService
{
    string UserID;
    string Password;
    string Domain;
    public Service () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    
    [WebMethod]
    public void LWPResponseDetails(SAP_Response_LWP[] objFullFinal)
    {
        UserID = ConfigurationManager.AppSettings.Get("UserID");
        Password = ConfigurationManager.AppSettings.Get("Password");
        Domain = ConfigurationManager.AppSettings.Get("Domain");
        try
        {
            FullFinal.LMSDatabase ObjLmsDataBase = new FullFinal.LMSDatabase();
            SAP_Response_LWP Serializer = new SAP_Response_LWP();
            string strXml = Serializer.Serialize(objFullFinal);
            strXml = strXml.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
            strXml = strXml.Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
            strXml = strXml.Replace("<SAP_Response_LWP xsi:nil=\"true\" />", "");
            SqlParameter[] objsqlparam = new SqlParameter[3];
            objsqlparam[0] = new SqlParameter("@xmlDoc", strXml);
            objsqlparam[1] = new SqlParameter("@processType", "SAP");
            objsqlparam[2] = new SqlParameter("@ActionBy", "0");
            //Impersonation.Impersonation Imp = new Impersonation.Impersonation();
            //Imp.impersonateValidUser("BPRDB_LMS", "HCLTECH", "Fkdjo$293732");
           // Imp.impersonateValidUser(UserID, Domain, Password);
            ObjLmsDataBase.OpenConnection();
            ObjLmsDataBase.BeginTransaction();
            Convert.ToString(ObjLmsDataBase.ExecuteNonQueryStoredProcedure("LMS_SP_SAPPI_Update_LWP_UploadedRecords", ref objsqlparam));
            ObjLmsDataBase.CommitTransaction();
            //Imp.undoImpersonation();

        }
        catch (Exception ex)
        {
            throw ex;

        }
    }

    [Serializable]
    public class SAP_Response_LWP
    {
        String _strTranId = null;
        String _strReponseCode = null;
        String _strResponseText = null;

        public string strTranId
        {
            get
            {
                return _strTranId;
            }
            set
            {
                _strTranId = value;
            }
        }

        public string strReponseCode
        {
            get
            {
                return _strReponseCode;

            }
            set
            {
                _strReponseCode = value;
            }
        }

        public string strResponseText
        {
            get
            {
                return _strResponseText;

            }
            set
            {
                _strResponseText = value;

            }
        }

        public string Serialize(object[] objectToBeSerialized)
        {
            if (objectToBeSerialized == null)
                return null;
            XmlSerializer serializer = new XmlSerializer(typeof(SAP_Response_LWP[]));
            MemoryStream memoryStream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(memoryStream, Encoding.UTF8);
            serializer.Serialize(writer, objectToBeSerialized);
            memoryStream = (MemoryStream)writer.BaseStream;
            return Encoding.UTF8.GetString(memoryStream.ToArray());

        }

    }

    
}
