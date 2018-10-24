using LuisBot.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;

namespace LuisBot.Utilities
{
    public class MongoConnect
    {
        public static List<QAContent> CallMongo()
        {
            var conString = "mongodb://cosmospolymath:yjj4BCSKzlpzFH546VYK8JtH1J9wIBcNPCYDidH5WUg1ZxHxaB1TRuEcGZ1D2lCO1gx6OLoLgfjSPJ2UIZNcUQ==@cosmospolymath.documents.azure.com:10255/?ssl=true&replicaSet=globaldb";
            var Client = new MongoClient(conString);
            var DB = Client.GetDatabase("HackFest");
            var collection = DB.GetCollection<BsonDocument>("QA");
            var builder = Builders<BsonDocument>.Filter;
            var filt = builder.Eq("id" , "QA1");
            var list = collection.Find(filt).ToList();
            string final = string.Empty;

            foreach (var doc in list)
            {
                string myText = doc.ToString();
                Regex badWords = new Regex("ObjectId|ISODate", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Regex html = new Regex("<[^>]*>");
                myText = badWords.Replace(myText, string.Empty);
                myText = myText.Replace("(", string.Empty);
                myText = myText.Replace(")", string.Empty);
                myText = myText.Replace("NumberLong", string.Empty);
                final = System.Web.HttpUtility.HtmlDecode(myText);
                final = html.Replace(final, "");

            }

          
            JObject obj = JObject.Parse(final);

            var valuesFirstSet = obj[":items"]["root"][":items"]["responsivegrid"][":items"]["contentfragment"]["elements"];
         

            List<QAContent> jcontent = new List<QAContent>();

            foreach(var subvalues in valuesFirstSet)
            {
                foreach(var child in subvalues)
                {
                    JObject objval1 = JObject.Parse(child.ToString());
                    QAContent temp = new QAContent();
                    temp.title = objval1["title"].ToString();
                    temp.value = objval1["value"].ToString();
                    temp.dataType = objval1["dataType"].ToString();
                    temp.type = objval1[":type"].ToString();
                    temp.set = 1;
                    jcontent.Add(temp);

                }
                
                
                   
              

            }

            var valuesSecondSet = obj[":items"]["root"][":items"]["responsivegrid"][":items"]["contentfragment_1487086772"]["elements"];

            foreach (var subvalues in valuesSecondSet)
            {
                foreach (var child in subvalues)
                {
                    JObject objval1 = JObject.Parse(child.ToString());
                    QAContent temp = new QAContent();
                    temp.title = objval1["title"].ToString();
                    temp.value = objval1["value"].ToString();
                    temp.dataType = objval1["dataType"].ToString();
                    temp.type = objval1[":type"].ToString();
                    temp.set = 1;
                    jcontent.Add(temp);

                }





            }

            var valuesThirdSet = obj[":items"]["root"][":items"]["responsivegrid"][":items"]["contentfragment_859424339"]["elements"];

            foreach (var subvalues in valuesThirdSet)
            {
                foreach (var child in subvalues)
                {
                    JObject objval1 = JObject.Parse(child.ToString());
                    QAContent temp = new QAContent();
                    temp.title = objval1["title"].ToString();
                    temp.value = objval1["value"].ToString();
                    temp.dataType = objval1["dataType"].ToString();
                    temp.type = objval1[":type"].ToString();
                    temp.set = 1;
                    jcontent.Add(temp);

                }





            }

            var valuesFourthSet = obj[":items"]["root"][":items"]["responsivegrid"][":items"]["contentfragment_1021302142"]["elements"];

            foreach (var subvalues in valuesFourthSet)
            {
                foreach (var child in subvalues)
                {
                    JObject objval1 = JObject.Parse(child.ToString());
                    QAContent temp = new QAContent();
                    temp.title = objval1["title"].ToString();
                    temp.value = objval1["value"].ToString();
                    temp.dataType = objval1["dataType"].ToString();
                    temp.type = objval1[":type"].ToString();
                    temp.set = 1;
                    jcontent.Add(temp);

                }





            }


            return jcontent;

        }

        public static List<CampaignContent> CallMongoForCampaign()
        {
            var conString = "mongodb://cosmospolymath:yjj4BCSKzlpzFH546VYK8JtH1J9wIBcNPCYDidH5WUg1ZxHxaB1TRuEcGZ1D2lCO1gx6OLoLgfjSPJ2UIZNcUQ==@cosmospolymath.documents.azure.com:10255/?ssl=true&replicaSet=globaldb";
            var Client = new MongoClient(conString);
            var DB = Client.GetDatabase("HackFest");
            var collection = DB.GetCollection<BsonDocument>("QA");
            var builder = Builders<BsonDocument>.Filter;
            var filt = builder.Eq("id", "3");
            var list = collection.Find(filt).ToList();
            string final = string.Empty;

            foreach (var doc in list)
            {
                string myText = doc.ToString();
                Regex badWords = new Regex("ObjectId|ISODate", RegexOptions.IgnoreCase | RegexOptions.Compiled);
               
                myText = badWords.Replace(myText, string.Empty);
                myText = myText.Replace("(", string.Empty);
                myText = myText.Replace(")", string.Empty);

                final = myText;

            }

            JObject obj = JObject.Parse(final);
            var values = (JArray)obj["Value"];
            List<CampaignContent> jcontent = new List<CampaignContent>();

            foreach (JObject parsedObject in values.Children<JObject>())
            {
                CampaignContent a = new CampaignContent();
                foreach (JProperty parsedProperty in parsedObject.Properties())
                {
                    string propertyName = parsedProperty.Name;
                    if (propertyName.Equals("date"))
                    {
                        a.date = (DateTime)parsedProperty.Value;

                    }
                    else if (propertyName.Equals("applicationId"))
                    {
                        a.applicationId = (string)parsedProperty.Value;

                    }
                    else if (propertyName.Equals("campaignId"))
                    {
                        a.campaignId = (string)parsedProperty.Value;

                    }
                    else if (propertyName.Equals("lineId"))
                    {
                        a.lineId = (string)parsedProperty.Value;

                    }
                    else if (propertyName.Equals("currencyCode"))
                    {
                        a.currencyCode = (string)parsedProperty.Value;

                    }
                    else if (propertyName.Equals("spend"))
                    {
                        a.spend = (float)parsedProperty.Value;

                    }
                    else if (propertyName.Equals("budget"))
                    {
                        a.budget = (float)parsedProperty.Value;

                    }
                    else if (propertyName.Equals("impressions"))
                    {
                        a.impressions = (int)parsedProperty.Value;

                    }
                    else if (propertyName.Equals("installs"))
                    {
                        a.installs = (int)parsedProperty.Value;

                    }
                    else if (propertyName.Equals("clicks"))
                    {
                        a.clicks = (int)parsedProperty.Value;
                    }
                    else if (propertyName.Equals("iapInstalls"))
                    {
                        a.iapInstalls = (int)parsedProperty.Value;
                    }
                    else if (propertyName.Equals("activeUsers"))
                    {
                        a.activeUsers = (int)parsedProperty.Value;
                    }
                }
                jcontent.Add(a);

            }


            return jcontent;

        }
    }
}