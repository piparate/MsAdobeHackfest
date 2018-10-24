using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using LuisBot.Dialogs;
using LuisBot.Models;
using LuisBot.Utilities;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;

namespace Microsoft.Bot.Sample.LuisBot
{
    // For more information about this template visit http://aka.ms/azurebots-csharp-luis
    [Serializable]
    public class BasicLuisDialog : LuisDialog<object>
    {
        string querymessage = string.Empty;
        static string qnamaker_endpointKey = "621e330f-216b-45bc-af38-2907d2e52b0a";
        static string qnamaker_endpointDomain = "polymathqa";

        static string QA_kbID = "e45f2dee-57f7-4622-bcc0-e86c949c8bfe";
        public QnAMakerService hrQnAService = new QnAMakerService("https://" + qnamaker_endpointDomain + ".azurewebsites.net", QA_kbID, qnamaker_endpointKey);
       

        public BasicLuisDialog() : base(new LuisService(new LuisModelAttribute(
            ConfigurationManager.AppSettings["LuisAppId"], 
            ConfigurationManager.AppSettings["LuisAPIKey"], 
            domain: ConfigurationManager.AppSettings["LuisAPIHostName"])))
        {
        }

       
        // Go to https://luis.ai and create a new intent, then train/publish your luis app.
        // Finally replace "Greeting" with the name of your newly created intent in the following handler
        [LuisIntent("Greeting")]
        public async Task GreetingIntent(IDialogContext context, LuisResult result)
        {

            context.Call(new Greeting(), Callback);

           // await context.SayAsync(text: $"Hello there,  What can I do for you today?", speak: "Hello there , What can I do for you today?");
            //await this.ShowLuisResult(context, result);
           // context.Wait(MessageReceived);
        }

        [LuisIntent("QA")]
        public async Task QAIntent(IDialogContext context, LuisResult result)
        {
            var qnaMakerAnswer = await hrQnAService.GetAnswer(result.Query);
            await context.PostAsync($"{qnaMakerAnswer}");
            context.Wait(MessageReceived);
            //string response = LuisUtilities.GetQADataFromCosmosforLuis(result.Query.ToString());
            //await context.SayAsync(text: response, speak: response);
            //context.Wait(MessageReceived);
        }


        [LuisIntent("Other Details")]
        public async Task DetailsIntent(IDialogContext context, LuisResult result)
        {
            string response = string.Empty;
            DateTime dt = DateTime.Now;
            string campaignresponse = string.Empty;
            string dateresponse = string.Empty;
            EntityRecommendation campaign;
            if (result.TryFindEntity("builtin.datetimeV2.date", out campaign))
            {
                var parser = new Chronic.Parser();
                var dateResult = parser.Parse(campaign.Entity);

                DateTime temp = DateTime.Parse(dateResult.Start.ToString());
                if (temp.Year > DateTime.Now.Year)
                {
                    dt = temp.AddYears(-1);
                }
                else
                {
                    dt = temp;
                }

                if (result.TryFindEntity("Campaign", out campaign))
                {

                    int month = dt.Date.Month;
                    int year = dt.Date.Year;
                    campaignresponse = campaign.Entity;
                    List<CampaignContent> QC = MongoConnect.CallMongoForCampaign();
                    List<CampaignContent> filteredlist = QC.Where(u => Convert.ToDateTime(u.date).Month == month).Where(u => Convert.ToDateTime(u.date).Year == year).ToList();
                    if (result.TryFindEntity("Users",out campaign))
                    {
                        foreach (var info in filteredlist)
                        {
                            Attachment attachment = CardUtility.CreateAdapativecardforUsers(info);
                            var replyMessage = context.MakeMessage();

                            replyMessage.Attachments = new List<Attachment> { attachment };
                            await context.PostAsync(replyMessage);
                            context.Wait(MessageReceived);
                        }
                    }
                    else if (result.TryFindEntity("Install", out campaign))
                    {
                        foreach (var info in filteredlist)
                        {
                            Attachment attachment = CardUtility.CreateAdapativecardforInstalls(info);
                            var replyMessage = context.MakeMessage();

                            replyMessage.Attachments = new List<Attachment> { attachment };
                            await context.PostAsync(replyMessage);
                            context.Wait(MessageReceived);
                        }
                    }
                    else if (result.TryFindEntity("Impression", out campaign))
                    {
                        foreach (var info in filteredlist)
                        {
                            Attachment attachment = CardUtility.CreateAdapativecardforImpressions(info);
                            var replyMessage = context.MakeMessage();

                            replyMessage.Attachments = new List<Attachment> { attachment };
                            await context.PostAsync(replyMessage);
                            context.Wait(MessageReceived);
                        }
                    }
                    else if (result.TryFindEntity("Cost", out campaign))
                    {
                        foreach (var info in filteredlist)
                        {
                            Attachment attachment = CardUtility.CreateAdapativecardforBudget(info);
                            var replyMessage = context.MakeMessage();

                            replyMessage.Attachments = new List<Attachment> { attachment };
                            await context.PostAsync(replyMessage);
                            context.Wait(MessageReceived);
                        }
                    }
                   



                }


            }
            else if (result.TryFindEntity("builtin.datetimeV2.daterange", out campaign))
            {
                var parser = new Chronic.Parser();
                var dateResult = parser.Parse(campaign.Entity);

                DateTime temp = DateTime.Parse(dateResult.Start.ToString());
                if (temp.Year > DateTime.Now.Year)
                {
                    dt = temp.AddYears(-1);
                }
                else
                {
                    dt = temp;
                }

                if (result.TryFindEntity("Campaign", out campaign))
                {
                    int month = dt.Date.Month;
                    int year = dt.Date.Year;
                    campaignresponse = campaign.Entity;
                    List<CampaignContent> QC = MongoConnect.CallMongoForCampaign();
                    List<CampaignContent> filteredlist = QC.Where(u => Convert.ToDateTime(u.date).Month == month).Where(u => Convert.ToDateTime(u.date).Year == year).ToList();
                    if (result.TryFindEntity("Users", out campaign))
                    {
                        foreach (var info in filteredlist)
                        {
                            Attachment attachment = CardUtility.CreateAdapativecardforUsers(info);
                            var replyMessage = context.MakeMessage();

                            replyMessage.Attachments = new List<Attachment> { attachment };
                            await context.PostAsync(replyMessage);
                           
                        }
                    }
                    else if (result.TryFindEntity("Install", out campaign))
                    {
                        foreach (var info in filteredlist)
                        {
                            Attachment attachment = CardUtility.CreateAdapativecardforInstalls(info);
                            var replyMessage = context.MakeMessage();

                            replyMessage.Attachments = new List<Attachment> { attachment };
                            await context.PostAsync(replyMessage);
                           
                        }
                    }
                    else if (result.TryFindEntity("Impression", out campaign))
                    {
                        foreach (var info in filteredlist)
                        {
                            Attachment attachment = CardUtility.CreateAdapativecardforImpressions(info);
                            var replyMessage = context.MakeMessage();

                            replyMessage.Attachments = new List<Attachment> { attachment };
                            await context.PostAsync(replyMessage);
                           
                        }
                    }
                    else if (result.TryFindEntity("Cost", out campaign))
                    {
                        foreach (var info in filteredlist)
                        {
                            Attachment attachment = CardUtility.CreateAdapativecardforBudget(info);
                            var replyMessage = context.MakeMessage();

                            replyMessage.Attachments = new List<Attachment> { attachment };
                            await context.PostAsync(replyMessage);
                            
                        }
                    }
                   context.Wait(MessageReceived);
                }


            }
            else if (result.TryFindEntity("CampaignList", out campaign))
            {

                campaignresponse = campaign.Entity;
                List<CampaignContent> data = MongoConnect.CallMongoForCampaign();
                var datalist = data.Where(x => x.campaignId == campaignresponse).ToList();

                if (result.TryFindEntity("Users", out campaign))
                {
                    foreach (var info in datalist)
                    {
                        Attachment attachment = CardUtility.CreateAdapativecardforUsers(info);
                        var replyMessage = context.MakeMessage();

                        replyMessage.Attachments = new List<Attachment> { attachment };
                        await context.PostAsync(replyMessage);
                        context.Wait(MessageReceived);
                    }
                }
                else if (result.TryFindEntity("Install", out campaign))
                {
                    foreach (var info in datalist)
                    {
                        Attachment attachment = CardUtility.CreateAdapativecardforInstalls(info);
                        var replyMessage = context.MakeMessage();

                        replyMessage.Attachments = new List<Attachment> { attachment };
                        await context.PostAsync(replyMessage);
                        context.Wait(MessageReceived);
                    }
                }
                else if (result.TryFindEntity("Impression", out campaign))
                {
                    foreach (var info in datalist)
                    {
                        Attachment attachment = CardUtility.CreateAdapativecardforImpressions(info);
                        var replyMessage = context.MakeMessage();

                        replyMessage.Attachments = new List<Attachment> { attachment };
                        await context.PostAsync(replyMessage);
                        context.Wait(MessageReceived);
                    }
                }
                else if (result.TryFindEntity("Cost", out campaign))
                {
                    foreach (var info in datalist)
                    {
                        Attachment attachment = CardUtility.CreateAdapativecardforBudget(info);
                        var replyMessage = context.MakeMessage();

                        replyMessage.Attachments = new List<Attachment> { attachment };
                        await context.PostAsync(replyMessage);
                        context.Wait(MessageReceived);
                    }
                }
               

            }

            //else if (result.TryFindEntity("Campaign", out campaign))
            //{

            //    campaignresponse = campaign.Entity;
            //    List<CampaignContent> data = MongoConnect.CallMongoForCampaign();

            //    Attachment attachment = CardUtility.CreateAdapativecardforCampaignSummary(data);
            //    var replyMessage = context.MakeMessage();

            //    replyMessage.Attachments = new List<Attachment> { attachment };

            //    await context.PostAsync(replyMessage);



            //    context.Wait(MessageReceived);
            //}



        }


        [LuisIntent("Campaign Summaries")]
        public async Task SummaryIntent(IDialogContext context, LuisResult result)
        {
            string response = string.Empty;
            DateTime dt = DateTime.Now;
            string campaignresponse = string.Empty;
            string dateresponse = string.Empty;
            EntityRecommendation campaign;
            if (result.TryFindEntity("builtin.datetimeV2.date", out campaign))
            {
                var parser = new Chronic.Parser();
                var dateResult = parser.Parse(campaign.Entity);

                DateTime temp = DateTime.Parse(dateResult.Start.ToString());
                if (temp.Year > DateTime.Now.Year)
                {
                    dt = temp.AddYears(-1);
                }
                else
                {
                    dt = temp;
                }

                if (result.TryFindEntity("Campaign", out campaign))
                {
                    int month = dt.Date.Month;
                    int year = dt.Date.Year;
                    campaignresponse = campaign.Entity;
                    List<CampaignContent> QC = MongoConnect.CallMongoForCampaign();
                    List<CampaignContent> filteredlist = QC.Where(u => Convert.ToDateTime(u.date).Month == month).Where(u => Convert.ToDateTime(u.date).Year == year).ToList();

                    if (result.TryFindEntity("Users", out campaign))
                    {
                        foreach (var info in filteredlist)
                        {
                            Attachment attachment = CardUtility.CreateAdapativecardforUsers(info);
                            var replyMessage = context.MakeMessage();

                            replyMessage.Attachments = new List<Attachment> { attachment };
                            await context.PostAsync(replyMessage);
                            context.Wait(MessageReceived);
                        }
                    }
                    else if (result.TryFindEntity("Install", out campaign))
                    {
                        foreach (var info in filteredlist)
                        {
                            Attachment attachment = CardUtility.CreateAdapativecardforInstalls(info);
                            var replyMessage = context.MakeMessage();

                            replyMessage.Attachments = new List<Attachment> { attachment };
                            await context.PostAsync(replyMessage);
                            context.Wait(MessageReceived);
                        }
                    }
                    else if (result.TryFindEntity("Impression", out campaign))
                    {
                        foreach (var info in filteredlist)
                        {
                            Attachment attachment = CardUtility.CreateAdapativecardforImpressions(info);
                            var replyMessage = context.MakeMessage();

                            replyMessage.Attachments = new List<Attachment> { attachment };
                            await context.PostAsync(replyMessage);
                            context.Wait(MessageReceived);
                        }
                    }
                    else if (result.TryFindEntity("Cost", out campaign))
                    {
                        foreach (var info in filteredlist)
                        {
                            Attachment attachment = CardUtility.CreateAdapativecardforBudget(info);
                            var replyMessage = context.MakeMessage();

                            replyMessage.Attachments = new List<Attachment> { attachment };
                            await context.PostAsync(replyMessage);
                            context.Wait(MessageReceived);
                        }
                    }
                    else
                    {
                        foreach (var info in filteredlist)
                        {
                            Attachment attachment = CardUtility.CreateAdapativecardforCampaign(info);
                            var replyMessage = context.MakeMessage();

                            replyMessage.Attachments = new List<Attachment> { attachment };
                            await context.PostAsync(replyMessage);
                            context.Wait(MessageReceived);
                        }
                    }

                }
               

            }
            else if (result.TryFindEntity("builtin.datetimeV2.daterange", out campaign))
            {
                var parser = new Chronic.Parser();
                var dateResult = parser.Parse(campaign.Entity);

                DateTime temp = DateTime.Parse(dateResult.Start.ToString());
                if (temp.Year > DateTime.Now.Year)
                {
                    dt = temp.AddYears(-1);
                }
                else
                {
                    dt = temp;
                }

                if (result.TryFindEntity("Campaign", out campaign))
                {
                    int month = dt.Date.Month;
                    int year = dt.Date.Year;
                    campaignresponse = campaign.Entity;
                    List<CampaignContent> QC = MongoConnect.CallMongoForCampaign();
                    List<CampaignContent> filteredlist = QC.Where(u => Convert.ToDateTime(u.date).Month == month).Where(u => Convert.ToDateTime(u.date).Year == year).ToList();

                    if (result.TryFindEntity("Users", out campaign))
                    {
                        foreach (var info in filteredlist)
                        {
                            Attachment attachment = CardUtility.CreateAdapativecardforUsers(info);
                            var replyMessage = context.MakeMessage();

                            replyMessage.Attachments = new List<Attachment> { attachment };
                            await context.PostAsync(replyMessage);
                            context.Wait(MessageReceived);
                        }
                    }
                    else if (result.TryFindEntity("Install", out campaign))
                    {
                        foreach (var info in filteredlist)
                        {
                            Attachment attachment = CardUtility.CreateAdapativecardforInstalls(info);
                            var replyMessage = context.MakeMessage();

                            replyMessage.Attachments = new List<Attachment> { attachment };
                            await context.PostAsync(replyMessage);
                            context.Wait(MessageReceived);
                        }
                    }
                    else if (result.TryFindEntity("Impression", out campaign))
                    {
                        foreach (var info in filteredlist)
                        {
                            Attachment attachment = CardUtility.CreateAdapativecardforImpressions(info);
                            var replyMessage = context.MakeMessage();

                            replyMessage.Attachments = new List<Attachment> { attachment };
                            await context.PostAsync(replyMessage);
                            context.Wait(MessageReceived);
                        }
                    }
                    else if (result.TryFindEntity("Cost", out campaign))
                    {
                        foreach (var info in filteredlist)
                        {
                            Attachment attachment = CardUtility.CreateAdapativecardforBudget(info);
                            var replyMessage = context.MakeMessage();

                            replyMessage.Attachments = new List<Attachment> { attachment };
                            await context.PostAsync(replyMessage);
                            context.Wait(MessageReceived);
                        }
                    }
                    else
                    {
                        foreach (var info in filteredlist)
                        {
                            Attachment attachment = CardUtility.CreateAdapativecardforCampaign(info);
                            var replyMessage = context.MakeMessage();

                            replyMessage.Attachments = new List<Attachment> { attachment };
                            await context.PostAsync(replyMessage);
                            context.Wait(MessageReceived);
                        }
                    }



                }


            }
            else if (result.TryFindEntity("CampaignList", out campaign))
            {

                campaignresponse = campaign.Entity;
                List<CampaignContent> data = MongoConnect.CallMongoForCampaign();
                var datalist = data.Where(x => x.campaignId == campaignresponse).ToList();

                if (result.TryFindEntity("Users", out campaign))
                {
                    foreach (var info in datalist)
                    {
                        Attachment attachment = CardUtility.CreateAdapativecardforUsers(info);
                        var replyMessage = context.MakeMessage();

                        replyMessage.Attachments = new List<Attachment> { attachment };
                        await context.PostAsync(replyMessage);
                        context.Wait(MessageReceived);
                    }
                }
                else if (result.TryFindEntity("Install", out campaign))
                {
                    foreach (var info in datalist)
                    {
                        Attachment attachment = CardUtility.CreateAdapativecardforInstalls(info);
                        var replyMessage = context.MakeMessage();

                        replyMessage.Attachments = new List<Attachment> { attachment };
                        await context.PostAsync(replyMessage);
                        context.Wait(MessageReceived);
                    }
                }
                else if (result.TryFindEntity("Impression", out campaign))
                {
                    foreach (var info in datalist)
                    {
                        Attachment attachment = CardUtility.CreateAdapativecardforImpressions(info);
                        var replyMessage = context.MakeMessage();

                        replyMessage.Attachments = new List<Attachment> { attachment };
                        await context.PostAsync(replyMessage);
                        context.Wait(MessageReceived);
                    }
                }
                else if (result.TryFindEntity("Cost", out campaign))
                {
                    foreach (var info in datalist)
                    {
                        Attachment attachment = CardUtility.CreateAdapativecardforBudget(info);
                        var replyMessage = context.MakeMessage();

                        replyMessage.Attachments = new List<Attachment> { attachment };
                        await context.PostAsync(replyMessage);
                        context.Wait(MessageReceived);
                    }
                }
               else
                {
                    foreach (CampaignContent info in datalist)
                    {
                        Attachment attachment = CardUtility.CreateAdapativecardforCampaign(info);
                        var replyMessage = context.MakeMessage();

                        replyMessage.Attachments = new List<Attachment> { attachment };

                        await context.PostAsync(replyMessage);
                        context.Wait(MessageReceived);
                    }
                }
               
                
            }

            else if (result.TryFindEntity("Campaign", out campaign))
            {

                campaignresponse = campaign.Entity;
                List<CampaignContent> data = MongoConnect.CallMongoForCampaign();

                Attachment attachment = CardUtility.CreateAdapativecardforCampaignSummary(data);
                    var replyMessage = context.MakeMessage();

                    replyMessage.Attachments = new List<Attachment> { attachment };

                    await context.PostAsync(replyMessage);
                

                
                context.Wait(MessageReceived);
            }
           


        }

        //[LuisIntent("Form")]
        //public async Task FormIntent(IDialogContext context, LuisResult result)
        //{
        //    IFormDialog<CampaignInformation> tmp = MakeRootDialog();
        //    context.Call(tmp, FormCompleteAsync);

        //}
        internal static IFormDialog<CampaignInformation> MakeRootDialog()
        {
            return FormDialog.FromForm(CampaignInformation.BuildForm, options: FormOptions.PromptInStart);

        }
        private async Task FormCompleteAsync(IDialogContext context, IAwaitable<object> result)
        {
            //ProductInformation value = await result;
            //throw new NotImplementedException();

            Activity myActivity = (Activity)context.Activity;
            myActivity.Text = "";
            await MessageReceived(context, Awaitable.FromItem(myActivity));
        }

        [LuisIntent("Greeting WH Questions")]
        public async Task GreetingWHQuestionsIntent(IDialogContext context, LuisResult result)
        {

            await context.SayAsync(text: $"I am great, thanks for asking!", speak: "I am great, thanks for asking");
            //  await context.SayAsync($"I am great, thanks for asking!");
            context.Wait(MessageReceived);
        }
        [LuisIntent("MyIdentity")]
        public async Task MyIdentityIntent(IDialogContext context, LuisResult result)
        {

            await context.SayAsync(text: $"I am Upmobile Bot", speak: "I am Upmobile Bot");

            context.Wait(MessageReceived);
        }
        [LuisIntent("Bye")]
        public async Task ByeIntent(IDialogContext context, LuisResult result)
        {

            await context.SayAsync(text: $"Bye. Looking forward to our next awesome conversation already.", speak: "Bye, Looking forward to our next awesome conversation already");
            context.Done(this);
            //context.Wait(MessageReceived);
        }
        [LuisIntent("Cancel")]
        public async Task CancelIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }
        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {

            await context.SayAsync(text: $"I am still learning", speak: "I am still learning");
            //  await context.SayAsync($"Well hello there.What can I do for you today?");
            context.Wait(MessageReceived);
        }


        [LuisIntent("Help")]
        public async Task HelpIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        private async Task GetInputfromUserForCampaign(IDialogContext context, IAwaitable<string> result)
        {

            string value = await result;
            Activity myActivity = (Activity)context.Activity;
            myActivity.Text = string.Concat(querymessage, " ", value);
            await MessageReceived(context, Awaitable.FromItem(myActivity));

        }

        private async Task ShowLuisResult(IDialogContext context, LuisResult result) 
        {
            await context.PostAsync($"You have reached {result.Intents[0].Intent}. You said: {result.Query}");
            context.Wait(MessageReceived);
        }

        private async Task Callback(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(MessageReceived);
        }
    }
}