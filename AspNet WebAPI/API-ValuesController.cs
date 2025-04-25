using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using Newtonsoft.Json;

namespace wrdsapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "id", "parameters" };
        }

        [HttpGet("{id}/{email}/{parameters}/{location}/{phone}/{name}")]
        public ActionResult<string> Get(int id, string email, string parameters, string location, string phone, string name)
        {
            string sReturn = "";
            if (id == 1)
            {
                //First Save inquiery on database
                var sNewConn= "";
                var valuesArray = new List<Dictionary<string, object>>();
                
                var config = Program.GetConn();
                var sConfig = config.Value;
                if (config.Key)
                {
                    //String Connection OK
                    sNewConn = sConfig;
                }
                
                var sIDQuery = "BOLTINQUIRY" + id + "|" + email + "|" + parameters + "|" + location + "|" + phone + "|" + name;
                
                if(sIDQuery.Length > 0)
                {
                    var query = Program.SQLValidator(sIDQuery, true);
                    var squery = query;

                    using (SqlConnection con = new SqlConnection(sNewConn))
                    {
                        con.Open();
                        
                        try
                        {
                            //return query.Value;
                            using (SqlCommand command = new SqlCommand(squery.Value, con))
                            {

                                SqlDataReader drQuery = command.ExecuteReader();
                                if (drQuery.HasRows)
                                {
                                    while (drQuery.Read())
                                    {
                                        sReturn = "TransactionOK: ";
                                        var fieldValues = new Dictionary<string, object>();
                                        for (int i = 0;i < drQuery.FieldCount;i++)
                                        {
                                            if(i>0)
                                            {
                                                sReturn = sReturn + ", ";
                                            }
                                            
                                            sReturn = sReturn + drQuery[i].ToString();
                                            fieldValues.Add(drQuery.GetName(i), drQuery[i]);
                                            
                                        }
                                        //sReturn = drQuery.GetString(0);
                                        valuesArray.Add(fieldValues);
                                    }
                                    sReturn = JsonConvert.SerializeObject(valuesArray);
                                }
                                else
                                {
                                    sReturn = "No data found...";
                                }
                                drQuery.Close();

                            }
                            con.Close();

                        }
                        catch
                        {
                            sReturn = "Error in ExecuteQuery";
                        }
                    }
                }

                //Send mail
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(name, email));

                IConfigurationBuilder builder = new ConfigurationBuilder();
                builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));

                var root = builder.Build();
                var keystr = root.GetSection("Data:MailBolt").GetChildren().ToList();
                

                message.To.Add(new MailboxAddress(keystr.ElementAt(0).Value, keystr.ElementAt(1).Value));
                message.Subject = keystr.ElementAt(2).Value;
                message.Body = new TextPart("plain")
                {
                    Text = name + " - Email (" + email + ")" + " - Ph (" + phone + "): " + parameters + " | Location: " + location
                };

                using (var client = new SmtpClient())
                {
                    client.Connect(keystr.ElementAt(3).Value, 587, false);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    // Note: since we don't have an OAuth2 token, disable
                    // the XOAUTH2 authentication mechanism.
                    client.Authenticate(keystr.ElementAt(4).Value, keystr.ElementAt(5).Value);
                    client.Send(message);
                    client.Disconnect(true);
                }

                sReturn = sReturn + " and Email Process Completed";
            }
            return sReturn;
        }

        // GET api/values/5
        [HttpGet("{id}/{parameters}/{clientid}")]
        public ActionResult<string> Get(int id, string parameters, string clientid)
        {
            try{

            string sReturn = "";
            var sNewConn = "";
            var sValCrydec = "";
            string sIDQuery = "";
            bool bRecycle = true;
            var valuesArray = new List<Dictionary<string, object>>();
            var config = Program.GetConn();
            var sConfig = config.Value;
            if (config.Key)
            {
                //String Connection OK
                sNewConn = sConfig;
            }

            var crydec = Program.GetCryDec();
            var sCrydec = crydec.Value;
            if (crydec.Key)
            {
                //String Connection OK
                sValCrydec = sCrydec;
            }

            // ClientID Validation
            sIDQuery = clientid;
            if(sIDQuery.Length > 0)
            {
                
                var query = Program.ClientValidator(sIDQuery, bRecycle);
                var squery = query;

                using (SqlConnection con = new SqlConnection(sNewConn))
                {
                    con.Open();
                    
                    try
                    {
                        //return query.Value;
                        using (SqlCommand command = new SqlCommand(squery.Value, con))
                        {

                            SqlDataReader drQuery = command.ExecuteReader();
                            if (drQuery.HasRows)
                            {
                                sReturn = "VALIDATIONOK";
                                //remdeb = Program.RemDebugger("CONTROLER QUERY CLIENT VALIDATIONOK","ADMIN");
                            }
                            else
                            {
                                sReturn = "VALIDATIONFAILED";
                                //remdeb = Program.RemDebugger("CONTROLER QUERY CLIENT VALIDATIONFAILED","ADMIN");
                            }
                            drQuery.Close();

                        }
                        con.Close();

                    }
                    catch(Exception ex)
                    {
                        sReturn = "Error in ExecuteQuery - " + ex.Message;
                            //remdeb = Program.RemDebugger("CONTROLER QUERY CLIENT ERROR IN EXECUTEQUERY - "+ex.Message,"ADMIN");
                    }
                }
            }
            //var remdeb = Program.RemDebugger("CONTROLER QUERIES NEW DEBUG","ADMIN");                    
            
            if (id == -1)
            {
                sReturn = "VALIDATIONOK"; //force encryption only
            }

            //API after validation
            if(sReturn == "VALIDATIONOK")
            {
                if (id == -1)
                {
                    sIDQuery = "ENCRY"+parameters;
                    
                }

                if (id == 0)
                {
                    sIDQuery = "DECRY"+parameters;
                    
                }

                if (id == 1)
                {
                    sIDQuery = "TODAY";
                    
                }
                if (id == -999)
                {
                    sIDQuery = parameters;
                }

                if(sIDQuery.Length > 0)
                {
                    //remdeb = Program.RemDebugger("CONTROLER QUERY" + sIDQuery,"ADMIN");
                    var query = Program.SQLValidator(sIDQuery, bRecycle);
                    var squery = query;

                    using (SqlConnection con = new SqlConnection(sNewConn))
                    {
                        con.Open();
                        
                        try
                        {
                            //return query.Value;
                            using (SqlCommand command = new SqlCommand(squery.Value, con))
                            {

                                SqlDataReader drQuery = command.ExecuteReader();
                                if (drQuery.HasRows)
                                {
                                    while (drQuery.Read())
                                    {
                                        sReturn = "TransactionOK: ";
                                        var fieldValues = new Dictionary<string, object>();
                                        for (int i = 0;i < drQuery.FieldCount;i++)
                                        {
                                            if(i>0)
                                            {
                                                sReturn = sReturn + ", ";
                                            }
                                            
                                            sReturn = sReturn + drQuery[i].ToString();
                                            fieldValues.Add(drQuery.GetName(i), drQuery[i]);
                                            
                                        }
                                        //sReturn = drQuery.GetString(0);
                                        valuesArray.Add(fieldValues);
                                        //remdeb = Program.RemDebugger("CONTROLER QUERY TRANSACTIONOK","ADMIN");
                                    }
                                    sReturn = JsonConvert.SerializeObject(valuesArray);
                                }
                                else
                                {
                                    sReturn = "No data found...";
                                    //remdeb = Program.RemDebugger("CONTROLER QUERY NO DATA FOUND","ADMIN");
                                }
                                drQuery.Close();

                            }
                            con.Close();

                        }
                        catch (Exception ex)
                        {
                            sReturn = "Error in ExecuteQuery - " + ex.Message;
                            //remdeb = Program.RemDebugger("CONTROLER QUERY ERROR IN EXECUTEQUERY - "+ex.Message,"ADMIN");
                        }
                    }
                }
            }
            return sReturn;
        }catch(Exception sexc)
        {
            var remdeb = Program.RemDebugger("CONTROLER ERROR - "+sexc.Message,"ADMIN");
              return sexc.Message;          
        }

            

            
        }


        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }



    }
}



