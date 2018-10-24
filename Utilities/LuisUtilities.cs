using LuisBot.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace LuisBot.Utilities
{
    public class LuisUtilities
    {
        public static string GetQADataFromCosmosforLuis(string ChatQuestion)
        {
            
           
            List<QAContent> data = Utilities.MongoConnect.CallMongo();

            QAContent QuestionStore = data.Where(x => x.value == ChatQuestion).First();

            string temp = QuestionStore.title;
            string resultString = Regex.Match(temp, @"\d+").Value;
            int set = QuestionStore.set;
            string search = string.Concat("A", resultString);
            QAContent AnswerStore = data.Where(x => x.title == search).Where(x=>x.set == set).First();
            return AnswerStore.value;

        }



        public static Attachment GetCampaignDataFromCosmosforLuis(string Campaign, DateTime dt)
        {
            List<CampaignContent> data = Utilities.MongoConnect.CallMongoForCampaign();
            try
            {
                CampaignContent info = data.Where(x => x.date.Date == dt.Date).First();

                return CardUtility.CreateAdapativecardforCampaign(info);
            }
            catch
            {
                try
                {
                    int month = dt.Date.Month;
                    int year = dt.Date.Year;
                    CampaignContent info = data.Where(u => Convert.ToDateTime(u.date).Month == month).Where(u => Convert.ToDateTime(u.date).Year == year).First();

                    return CardUtility.CreateAdapativecardforCampaign(info);
                }
                catch
                {
                    CampaignContent info = data.OrderByDescending(x => x.date).First();

                    return CardUtility.CreateAdapativecardforCampaign(info);
                }
               
            }
            
            


        }



        public static Attachment GetAllCampaignDataFromCosmosforLuis(CampaignContent info)
        {
            
                return CardUtility.CreateAdapativecardforCampaign(info);


        }
    }
}