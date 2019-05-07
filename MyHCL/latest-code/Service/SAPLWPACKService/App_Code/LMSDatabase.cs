using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data.SqlClient; 
using System.Configuration; 

namespace FullFinal
{

public class LMSDatabase 
{ 
    
    SqlConnection gObjConnection; 
    SqlTransaction gObjTransaction;
    
    public LMSDatabase() 
    { 
        gObjConnection = new SqlConnection();
        
    } 
    
    public void OpenConnection() 
// ERROR: Optional parameters aren't supported in C# bool fblnFromWebConfig, [System.Runtime.InteropServices.OptionalAttribute, System.Runtime.InteropServices.DefaultParameterValueAttribute("")] // ERROR: Optional parameters aren't supported in C# string fstrConfigFileName) 
    {  
        try { 
            if (gObjConnection.State == ConnectionState.Closed) { 
                
                    gObjConnection.ConnectionString = CreateConnectionString(); 
                
                //else { 
                //    gObjConnection.ConnectionString = CreateConnectionString(ReadConfigFile(enmDBName, fstrConfigFileName)); 
                //} 
                //gObjConnection.Open(); 
            } 
        } 
        catch (Exception exp) {  
            throw exp; 
        } 
    } 
    
    private string CreateConnectionString() 
    { 
        
        string ConnStr = null;

          ConnStr = ConfigurationManager.AppSettings.Get("ConnectionString");
          return ConnStr; 
        
    } 
    
    private ConnectionParameters ReadConfigSetting() 
    { 
        ConnectionParameters cls = new ConnectionParameters(); 
       

            cls.UserId = ConfigurationManager.AppSettings["USERID"].ToString();
            cls.PassWord = ConfigurationManager.AppSettings["PASSWORD"].ToString();
            cls.DataSource = ConfigurationManager.AppSettings["DATASOURCE"].ToString();
            cls.InitialCatalog = ConfigurationManager.AppSettings["INITCAT"].ToString(); 
            return cls; 
        
    } 
    
    private ConnectionParameters ReadConfigFile(string fstrConfigFileName) 
        // ERROR: Optional parameters aren't supported in C# string fstrConfigFileName) 
    { 
        XmlDocument clsXML = new XmlDocument(); 
        XmlNode clsXMLNode = default(XmlNode); 
        ConnectionParameters cls = new ConnectionParameters(); 
        
        
            if (string.IsNullOrEmpty(fstrConfigFileName)) { 
                clsXML.Load("../LMSData.config"); 
            } 
            else { 
                clsXML.Load(fstrConfigFileName); 
            } 
            
            clsXMLNode = clsXML.SelectSingleNode("ROOT/DATABASE/USERID"); 
            cls.UserId = clsXMLNode.InnerText; 
            
            clsXMLNode = clsXML.SelectSingleNode("ROOT/DATABASE/PASSWORD"); 
            cls.PassWord = clsXMLNode.InnerText; 
            
            clsXMLNode = clsXML.SelectSingleNode("ROOT/DATABASE/DATASOURCE"); 
            cls.DataSource = clsXMLNode.InnerText; 
            
            
                clsXMLNode = clsXML.SelectSingleNode("ROOT/DATABASE/INITCAT"); 
                cls.InitialCatalog = clsXMLNode.InnerText; 
            
            return cls; 
       
    } 
    
    public string ReadDetailfromConfig(string ElementName,string fstrConfigFileName) 
        // ERROR: Optional parameters aren't supported in C# string fstrConfigFileName) 
    { 
        XmlDocument clsXML = new XmlDocument(); 
        XmlNode clsXMLNode = default(XmlNode); 
        string tagName = null; 
        string output = null; 
        
        try { 
            
            if (string.IsNullOrEmpty(fstrConfigFileName)) { 
                clsXML.Load("LMSData.config"); 
            } 
            else { 
                clsXML.Load(fstrConfigFileName); 
            }  
                        
            if (!string.IsNullOrEmpty(ElementName)) { 
                tagName = "ROOT/" + ElementName + ""; 
            } 
            
            clsXMLNode = clsXML.SelectSingleNode(tagName); 
            output = clsXMLNode.InnerText; 
            
            return output; 
        } 
        
        catch (Exception exp) { 
            
            return null; 
        } 
    } 
    
    private struct ConnectionParameters 
    { 
        
        public string UserId; 
        public string PassWord; 
        public string DataSource; 
        public string InitialCatalog; 
        
    } 
    
    public void CloseConnection() 
    { 
        if (gObjConnection.State == ConnectionState.Open) { 
            gObjConnection.Close(); 
            gObjConnection.Dispose(); 
        } 
    } 
    
    public void BeginTransaction() 
    {
        if (gObjConnection.State == ConnectionState.Closed)
        {
            gObjConnection.Open();
            
        } 
        gObjTransaction = gObjConnection.BeginTransaction(IsolationLevel.Serializable); 
    } 
    
    public void CommitTransaction() 
    { 
        gObjTransaction.Commit(); 
    } 
    
    public void RollBackTransaction() 
    { 
        gObjTransaction.Rollback(); 
    } 
    
    //Procedure Name :ExecuteNonQueryStoredProcedure 
    //Project :LMS 
    //Input Parameters : 
    //Created By :G.B.Betty, Hcl Technologies,chennai 
    //Created Date :25-10-2004 
    //Purpose :This method is used to insert/update/Delete records without transaction 
    //Modification History : 
    //Modified By Modified Date Purpose Remark 
    public int ExecuteNonQueryStoredProcedure(string StoredProcedureName, ref SqlParameter[] QueryParameters) 
    { 
        SqlCommand lObjCommand = default(SqlCommand); 
        int lReturn = 0; 
        try { 
            lObjCommand = PrepareCommandObject(StoredProcedureName, QueryParameters); 
            lObjCommand.CommandTimeout = 600; 
            lReturn = lObjCommand.ExecuteNonQuery();
            lObjCommand.Parameters.Clear(); 
            lObjCommand.Dispose(); 
            return lReturn; 
        } 
        catch (Exception exp) { 
            throw exp; 
        } 
    } 
    
    //Procedure Name :ExecuteNonQueryStoredProcedure 
    //Project :LMS 
    //Input Parameters : 
    //Created By :G.B.Betty, Hcl Technologies,chennai 
    //Created Date :25-10-2004 
    //Purpose :To retieve data into dataset.Used for stored procedures with parameters 
    //Modification History : 
    //Modified By Modified Date Purpose Remark 
    public void ExecuteQueryStoredProcedure(string StoredProcedureName, SqlParameter[] QueryParameters, ref DataSet ds) 
    { 
        SqlCommand lObjCommand = default(SqlCommand); 
        SqlDataAdapter lObjDataAdapter = default(SqlDataAdapter); 
        DataSet lObjDataSet = new DataSet(); 
        try { 
            lObjCommand = PrepareCommandObject(StoredProcedureName, QueryParameters); 
            lObjCommand.CommandTimeout = 600; 
            lObjDataAdapter = new SqlDataAdapter(lObjCommand); 
            lObjDataAdapter.Fill(lObjDataSet); 
            lObjCommand.Parameters.Clear(); 
            lObjCommand.Dispose(); 
            ds = lObjDataSet; 
        } 
        catch (Exception exp) { 
            throw exp; 
        } 
    } 
    
    //Procedure Name :ExecuteNonQueryStoredProcedure 
    //Project :LMS 
    //Input Parameters : 
    //Created By :G.B.Betty, Hcl Technologies,chennai 
    //Created Date :25-10-2004 
    //Purpose :To retieve data into dataset.Used for stored procedures without parameters 
    //Modification History : 
    //Modified By Modified Date Purpose Remark 
    public void ExecuteQueryStoredProcedure(string StoredProcedureName, ref DataSet ds) 
    { 
        SqlCommand lObjCommand = default(SqlCommand); 
        SqlDataAdapter lObjDataAdapter = default(SqlDataAdapter); 
        DataSet lObjDataSet = new DataSet(); 
        try { 
            lObjCommand = PrepareCommandObject(StoredProcedureName, null); 
            lObjCommand.CommandTimeout = 600; 
            lObjDataAdapter = new SqlDataAdapter(lObjCommand); 
            lObjDataAdapter.Fill(lObjDataSet); 
            lObjCommand.Parameters.Clear(); 
            lObjCommand.Dispose(); 
            ds = lObjDataSet; 
        } 
        catch (Exception exp) { 
            throw exp; 
        } 
    } 
    
    //Procedure Name :ExecuteScalarStoredProcedure 
    //Project :LMS 
    //Input Parameters : 
    //Created By :DHIRAJ CHANDEKAR, Hcl Technologies,Gurgaon 
    //Created Date :07-12-2004 
    //Purpose :To retieve single row single column data from Stored Proc 
    //Modification History : 
    //Modified By Modified Date Purpose Remark 
    //public void ExecuteScalarStoredProcedure(string StoredProcedureName, SqlParameter[] commandParameters) 
    //    //[System.Runtime.InteropServices.OptionalAttribute, System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] ref) 
    //    // ERROR: Optional parameters aren't supported in C# SqlParameter[] commandParameters) 
    //{ 
    //    SqlCommand lObjCommand = default(SqlCommand); 
    //    try { 
    //        lObjCommand = PrepareCommandObject(StoredProcedureName, commandParameters); 
    //        lObjCommand.CommandTimeout = 600; 
    //        object requestedData = lObjCommand.ExecuteScalar(); 
    //        lObjCommand.Dispose(); 
    //        return requestedData; 
    //    } 
    //    catch (Exception ex) { 
    //        Exception objex = new Exception("Could not fetch value.", ex.GetBaseException()); 
    //        throw objex; 
    //    } 
    //} 
    
    //Procedure Name :PrepareCommandObject 
    //Project :LMS 
    //Input Parameters : 
    //Created By :G.B.Betty, Hcl Technologies,chennai 
    //Created Date :25-10-2004 
    //Purpose :To create command object with transaction 
    //Modification History : 
    //Modified By Modified Date Purpose Remark 
    private SqlCommand PrepareCommandObject(string StoredProcedureName, SqlParameter[] QueryParameters) 
    { 
        SqlCommand lObjCommand = new SqlCommand(); 
      //  SqlParameter lObjparam = default(SqlParameter); 
        try { 
            lObjCommand.Connection = gObjConnection; 
            lObjCommand.Transaction = gObjTransaction; 
            
            if ((QueryParameters != null)) 
            { 
                
                foreach (SqlParameter lObjparam in QueryParameters) 
                    { 
                        lObjCommand.Parameters.Add(lObjparam); 
                    } 

            } 

            lObjCommand.CommandText = StoredProcedureName; 
            lObjCommand.CommandType = CommandType.StoredProcedure; 
            
            return lObjCommand; 

        } 

        catch (Exception exp) { 
            throw exp; 
        } 
        
    } 
    
    
    public enum DBName 
    { 
        
        LMSIndia = 0, 
        GeoLMS = 1, 
        LMSJapan = 2 
        
    } 
    
} 

}