using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AdaptiveCards;
using LuisBot.Utilities;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;

namespace LuisBot.Models
{
   
    [Serializable]
    public class CampaignInformation
    {
        [Prompt("Enter a Campaign ID")]
        public string campaignID;
        [Prompt("Enter a date")]
        public DateTime? date;
       
        public static IForm<CampaignInformation> BuildForm()
        {
            return new FormBuilder<CampaignInformation>().OnCompletion(fullfillAsync).Build();

        }

        private static async Task fullfillAsync(IDialogContext context, CampaignInformation state)
        {
            List<CampaignContent> data = MongoConnect.CallMongoForCampaign();
            int  c = data.Where(a => a.campaignId == state.campaignID).Where(a => a.date == state.date).Count();
            if(c==0)
            {
                await context.PostAsync("There is no data for current selection");
            }
            else
            {
                CampaignContent list = data.Where(a => a.campaignId == state.campaignID).Where(a => a.date == state.date).First();


                Attachment attachment = LuisUtilities.GetAllCampaignDataFromCosmosforLuis(list);
                var replyMessage = context.MakeMessage();

                replyMessage.Attachments = new List<Attachment> { attachment };
                await context.PostAsync(replyMessage);
            }
           


        }

    }
}