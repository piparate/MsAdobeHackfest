using AdaptiveCards;
using LuisBot.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuisBot.Utilities
{
    public class CardUtility
    {
        public static string CardforCampaignBrief(CampaignContent data)
        {

            string value = string.Empty;
           
            value = @"{
	""$schema"": ""http://adaptivecards.io/schemas/adaptive-card.json"",
            ""type"": ""AdaptiveCard"",
	""version"": ""1.0"",
	""speak"": """",
	""""body"""": [
		{
			""""type"""": """"Container"""",
			""items"": [
				{
					""type"": ""TextBlock"",
					""text"": ""CAMPAIGN"",
					""size"": ""medium"",
					""isSubtle"": true

                },
				{
					""type"": ""TextBlock"",
					""text"": ""date"",
					""isSubtle"": true
				}
			]
		},
		{
			""type"": ""Container"",
			""spacing"": ""none"",
			""items"": [
				{
					""type"": ""ColumnSet"",
					""columns"": [
						{
							""type"": ""Column"",
							""width"": ""stretch"",
							""items"": [
                                
								
								{
									""type"": ""TextBlock"",
									""text"": ""spend"",
									""size"": ""extraLarge""
								},
                                {
									""type"": ""TextBlock"",
									""text"": ""Total Spent"",
									""size"": ""small"",
									""spacing"": ""none""
								}
							]
						},
						{
							""type"": ""Column"",
							""width"": ""auto"",
							""items"": [
								{
									""type"": ""FactSet"",
									""facts"": [
										{
											""title"": ""Active Users"",
											""value"": ""user""
										},
										{
											""title"": ""Installs"",
											""value"": ""install""
										},
										{
											""title"": ""Impressions"",
											""value"": ""impression""
										}
									]
								}
							]
						}
					]
				}
			]
		}
	]
}";
            value = value.Replace("spend", data.spend+" "+ data.currencyCode);
            value = value.Replace("date", data.date.ToLongDateString());
            value = value.Replace("user", data.activeUsers.ToString());
            value = value.Replace("install", data.installs.ToString());
            value = value.Replace("impression", data.impressions.ToString());
            return value;
        }


        public static Attachment CreateAdapativecardforCampaign(CampaignContent data)
        {


            DateTime dt = DateTime.Parse(data.date.ToLongDateString());
            AdaptiveCard card = new AdaptiveCard();
            card.Speak = "";
            AdaptiveContainer firstcontainer = new AdaptiveContainer();
            var headerrow = new AdaptiveTextBlock();
            headerrow.Text = "Campaign Details";
            headerrow.Size = AdaptiveTextSize.ExtraLarge;
           // headerrow.IsSubtle = true;
            var daterow = new AdaptiveTextBlock();
            daterow.Text = data.date.ToLongDateString();
            daterow.Size = AdaptiveTextSize.Medium;
           // daterow.IsSubtle = true;
            firstcontainer.Items.Add(headerrow);
            firstcontainer.Items.Add(daterow);
            card.Body.Add(firstcontainer);


            AdaptiveContainer secondcontainer = new AdaptiveContainer();
            var colset = new AdaptiveColumnSet();
            var firstcolumn = new AdaptiveColumn();
            var firstvalue = new AdaptiveTextBlock();

            firstvalue.Text = data.campaignId.ToString();
            firstvalue.Size = AdaptiveTextSize.Large;
            var secondvalue = new AdaptiveTextBlock();
            secondvalue.Text = "Campaign ID";
            secondvalue.Size = AdaptiveTextSize.Small;
            firstcolumn.Items.Add(firstvalue);
            firstcolumn.Items.Add(secondvalue);
            colset.Columns.Add(firstcolumn);
            var secondcolumn = new AdaptiveColumn();
            secondcolumn.Width = AdaptiveColumnWidth.Auto;
            var factset = new AdaptiveFactSet();
            var firstfact = new AdaptiveFact();
            var Zerofact = new AdaptiveFact();
            Zerofact.Title = "Total Spent";
            Zerofact.Value = data.spend.ToString() + " " + data.currencyCode.ToString();
            firstfact.Title = "Active Users";
            firstfact.Value = data.activeUsers.ToString();
            var secondfact = new AdaptiveFact();
            secondfact.Title = "Installs";
            secondfact.Value = data.installs.ToString();
            var thirdfact = new AdaptiveFact();
            thirdfact.Title = "Impressions";
            thirdfact.Value = data.impressions.ToString();
            factset.Facts.Add(Zerofact);
            factset.Facts.Add(firstfact);
            factset.Facts.Add(secondfact);
            factset.Facts.Add(thirdfact);
            secondcolumn.Items.Add(factset);
            colset.Columns.Add(secondcolumn);
          
            secondcontainer.Items.Add(colset);
            card.Body.Add(secondcontainer);



            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }

        public static Attachment CreateAdapativecardforCampaignSummary(List<CampaignContent> data)
        {
            AdaptiveCard card = new AdaptiveCard();
                card.Speak = "";
            AdaptiveTextBlock firsttextblock = new AdaptiveTextBlock();
            firsttextblock.Text = "Campaign Summary";
            firsttextblock.Weight = AdaptiveTextWeight.Bolder;
            firsttextblock.Size = AdaptiveTextSize.Large;

            card.Body.Add(firsttextblock);

            foreach (CampaignContent info in data)
            {
                AdaptiveColumnSet colset1 = new AdaptiveColumnSet();
                colset1.Separator = true;
                colset1.Spacing = AdaptiveSpacing.Medium;
                AdaptiveColumn col1 = new AdaptiveColumn();
                col1.Width = AdaptiveColumnSize.Stretch;
                var firsttext = new AdaptiveTextBlock();
                firsttext.IsSubtle = true;
                firsttext.Weight = AdaptiveTextWeight.Bolder;
                firsttext.Text = "Campaign ID";
                col1.Items.Add(firsttext);
                var secondtext = new AdaptiveTextBlock();
                secondtext.Text = "Date";
                secondtext.Spacing = AdaptiveSpacing.Small;
                col1.Items.Add(secondtext);
                var thirdtext = new AdaptiveTextBlock();
                thirdtext.Text = "Total Spent";
                thirdtext.Spacing = AdaptiveSpacing.Small;
                var fourth1text = new AdaptiveTextBlock();
                fourth1text.Text = "Budget";
                fourth1text.Spacing = AdaptiveSpacing.Small;
                col1.Items.Add(thirdtext);
                col1.Items.Add(fourth1text);
                AdaptiveColumn col2 = new AdaptiveColumn();
               // col2.Width = AdaptiveColumnSize.Auto;
                var fourthtext = new AdaptiveTextBlock();
                fourthtext.IsSubtle = true;
                fourthtext.Weight = AdaptiveTextWeight.Bolder;
                fourthtext.Text = info.campaignId;
                fourthtext.HorizontalAlignment = AdaptiveHorizontalAlignment.Right;
                col2.Items.Add(fourthtext);
                var fifthtext = new AdaptiveTextBlock();
                fifthtext.Text = info.date.ToShortDateString();
                fifthtext.Spacing = AdaptiveSpacing.Small;
                fifthtext.HorizontalAlignment = AdaptiveHorizontalAlignment.Right;
                col2.Items.Add(fifthtext);
                var sixthtext = new AdaptiveTextBlock();
                sixthtext.Text = info.spend + " " + info.currencyCode;
                sixthtext.Spacing = AdaptiveSpacing.Small;
                sixthtext.HorizontalAlignment = AdaptiveHorizontalAlignment.Right;
                col2.Items.Add(sixthtext);
                var seventhtext = new AdaptiveTextBlock();
                seventhtext.Text = info.budget + " " + info.currencyCode;
                seventhtext.Spacing = AdaptiveSpacing.Small;
                seventhtext.HorizontalAlignment = AdaptiveHorizontalAlignment.Right;
                col2.Items.Add(seventhtext);
                colset1.Columns.Add(col1);
                colset1.Columns.Add(col2);
                AdaptiveColumnSet colset2 = new AdaptiveColumnSet();
                colset2.Spacing = AdaptiveSpacing.Medium;

                AdaptiveColumn col3 = new AdaptiveColumn();

                AdaptiveTextBlock text1 = new AdaptiveTextBlock();
                text1.Text = "Installs";
                text1.IsSubtle = true;
                text1.Weight = AdaptiveTextWeight.Bolder;
                col3.Items.Add(text1);
                AdaptiveTextBlock text2 = new AdaptiveTextBlock();
                text2.Text = info.installs.ToString();
                text2.Spacing = AdaptiveSpacing.Small;
                col3.Items.Add(text2);

                AdaptiveColumn col4 = new AdaptiveColumn();
                AdaptiveTextBlock text3 = new AdaptiveTextBlock();
                text3.Text = "Impressions";
                text3.IsSubtle = true;
                text3.Weight = AdaptiveTextWeight.Bolder;
                col4.Items.Add(text3);
                AdaptiveTextBlock text4 = new AdaptiveTextBlock();
                text4.Text = info.impressions.ToString();
                text4.Spacing = AdaptiveSpacing.Small;
                col4.Items.Add(text4);


                AdaptiveColumn col5 = new AdaptiveColumn();
                AdaptiveTextBlock text5 = new AdaptiveTextBlock();
                text5.Text = "Active Users";
                text5.IsSubtle = true;
                text5.Weight = AdaptiveTextWeight.Bolder;
                col5.Items.Add(text5);
                AdaptiveTextBlock text6 = new AdaptiveTextBlock();
                text6.Text = info.activeUsers.ToString();
                text6.Spacing = AdaptiveSpacing.Small;
                col5.Items.Add(text6);
                colset2.Columns.Add(col3);
                colset2.Columns.Add(col4);
                colset2.Columns.Add(col5);
                card.Body.Add(colset1);
                card.Body.Add(colset2);

            }

                Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }

        //user cards

        public static Attachment CreateAdapativecardforUsers(CampaignContent data)
        {


            DateTime dt = DateTime.Parse(data.date.ToLongDateString());
            AdaptiveCard card = new AdaptiveCard();
            card.Speak = "";
            AdaptiveContainer firstcontainer = new AdaptiveContainer();
            var headerrow = new AdaptiveTextBlock();
            headerrow.Text = "Campaign ID:"+" "+ data.campaignId;
            headerrow.Size = AdaptiveTextSize.Large;
            headerrow.Weight = AdaptiveTextWeight.Bolder;
            // headerrow.IsSubtle = true;
            var daterow = new AdaptiveTextBlock();
            daterow.Text = data.date.ToLongDateString();
            daterow.Size = AdaptiveTextSize.Small;
            // daterow.IsSubtle = true;
            firstcontainer.Items.Add(headerrow);
            firstcontainer.Items.Add(daterow);
           
            AdaptiveContainer secondcontainer = new AdaptiveContainer();
            var colset1 = new AdaptiveColumnSet();
            var col1 = new AdaptiveColumn();
            var row1 = new AdaptiveTextBlock();
            row1.Text = data.activeUsers.ToString() ;
            row1.Size = AdaptiveTextSize.Large;
            row1.IsSubtle = true;
           // secondcontainer.Items.Add(row1);
            var row2 = new AdaptiveTextBlock();
            row2.Text = "Active Users";
            row2.Size = AdaptiveTextSize.Small;
            row2.IsSubtle = true;
            row2.Spacing = AdaptiveSpacing.None;
            col1.Items.Add(row1);
            col1.Items.Add(row2);
            colset1.Columns.Add(col1);
            secondcontainer.Items.Add(colset1);
            card.Body.Add(firstcontainer);
            card.Body.Add(secondcontainer);
           

            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }

        public static Attachment CreateAdapativecardforTotalSpent(CampaignContent data)
        {


            DateTime dt = DateTime.Parse(data.date.ToLongDateString());
            AdaptiveCard card = new AdaptiveCard();
            card.Speak = "";
            AdaptiveContainer firstcontainer = new AdaptiveContainer();
            var headerrow = new AdaptiveTextBlock();
            headerrow.Text = "Campaign ID:" + " " + data.campaignId;
            headerrow.Size = AdaptiveTextSize.Large;
            headerrow.Weight = AdaptiveTextWeight.Bolder;
            // headerrow.IsSubtle = true;
            var daterow = new AdaptiveTextBlock();
            daterow.Text = data.date.ToLongDateString();
            daterow.Size = AdaptiveTextSize.Small;
            // daterow.IsSubtle = true;
            firstcontainer.Items.Add(headerrow);
            firstcontainer.Items.Add(daterow);

            AdaptiveContainer secondcontainer = new AdaptiveContainer();
            var colset1 = new AdaptiveColumnSet();
            var col1 = new AdaptiveColumn();
            var row1 = new AdaptiveTextBlock();
            row1.Text = data.spend.ToString()+" "+data.currencyCode.ToString();
            row1.Size = AdaptiveTextSize.Large;
            row1.IsSubtle = true;
            //secondcontainer.Items.Add(row1);
            var row2 = new AdaptiveTextBlock();
            row2.Text = "Total Spent";
            row2.Size = AdaptiveTextSize.Small;
            row2.IsSubtle = true;
            row2.Spacing = AdaptiveSpacing.None;
            col1.Items.Add(row1);
            col1.Items.Add(row2);
            colset1.Columns.Add(col1);
            secondcontainer.Items.Add(colset1);
            card.Body.Add(firstcontainer);
            card.Body.Add(secondcontainer);


            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }

        public static Attachment CreateAdapativecardforBudget(CampaignContent data)
        {
            DateTime dt = DateTime.Parse(data.date.ToLongDateString());
            AdaptiveCard card = new AdaptiveCard();
            card.Speak = "";
            AdaptiveContainer firstcontainer = new AdaptiveContainer();
            var headerrow = new AdaptiveTextBlock();
            headerrow.Text = "Campaign ID:" + " " + data.campaignId;
            headerrow.Size = AdaptiveTextSize.Large;
            headerrow.Weight = AdaptiveTextWeight.Bolder;
            // headerrow.IsSubtle = true;
            var daterow = new AdaptiveTextBlock();
            daterow.Text = data.date.ToLongDateString();
            daterow.Size = AdaptiveTextSize.Small;
            // daterow.IsSubtle = true;
            firstcontainer.Items.Add(headerrow);
            firstcontainer.Items.Add(daterow);

            AdaptiveContainer secondcontainer = new AdaptiveContainer();
            var colset1 = new AdaptiveColumnSet();
            var col1 = new AdaptiveColumn();
            var row1 = new AdaptiveTextBlock();
            row1.Text = data.spend.ToString() + " " + data.currencyCode.ToString();
            row1.Size = AdaptiveTextSize.Large;
            row1.IsSubtle = true;
           
            //secondcontainer.Items.Add(row1);
            var row2 = new AdaptiveTextBlock();
            row2.Text = "Total Spent";
            row2.Size = AdaptiveTextSize.Small;
            row2.IsSubtle = true;
            row2.Spacing = AdaptiveSpacing.None;
            var row3 = new AdaptiveTextBlock();
            row3.Text = data.budget.ToString() + " " + data.currencyCode.ToString();
            row3.Size = AdaptiveTextSize.Large;
            row3.IsSubtle = true;
            var row4 = new AdaptiveTextBlock();
            row4.Text = "Budget";
            row4.Size = AdaptiveTextSize.Small;
            row4.IsSubtle = true;
            row4.Spacing = AdaptiveSpacing.None;
            var row5 = new AdaptiveTextBlock();
           
            if (data.budget > data.spend)
            {
                float gainpercent = (data.budget - data.spend) / data.budget * 100;
                row5.Color = AdaptiveTextColor.Good;
                row5.Text ="+"+ (data.budget - data.spend).ToString() + "(" + gainpercent.ToString() + "%)";

            }
            else { row5.Color = AdaptiveTextColor.Warning;
                float gainpercent = ( data.spend- data.budget ) / data.budget * 100;
                row5.Text = "-"+(data.spend- data.budget).ToString() + "(" + gainpercent.ToString() + "%)"; }

                row5.Size = AdaptiveTextSize.Small;
            
            row3.IsSubtle = true;
            col1.Items.Add(row1);
            col1.Items.Add(row2);
            col1.Items.Add(row3);
            col1.Items.Add(row4);
            col1.Items.Add(row5);
            colset1.Columns.Add(col1);
            secondcontainer.Items.Add(colset1);
            card.Body.Add(firstcontainer);
            card.Body.Add(secondcontainer);


            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }


        public static Attachment CreateAdapativecardforInstalls(CampaignContent data)
        {


            DateTime dt = DateTime.Parse(data.date.ToLongDateString());
            AdaptiveCard card = new AdaptiveCard();
            card.Speak = "";
            AdaptiveContainer firstcontainer = new AdaptiveContainer();
            var headerrow = new AdaptiveTextBlock();
            headerrow.Text = "Campaign ID:" + " " + data.campaignId;
            headerrow.Size = AdaptiveTextSize.Large;
            headerrow.Weight = AdaptiveTextWeight.Bolder;
            // headerrow.IsSubtle = true;
            var daterow = new AdaptiveTextBlock();
            daterow.Text = data.date.ToLongDateString();
            daterow.Size = AdaptiveTextSize.Small;
            // daterow.IsSubtle = true;
            firstcontainer.Items.Add(headerrow);
            firstcontainer.Items.Add(daterow);

            AdaptiveContainer secondcontainer = new AdaptiveContainer();
            var colset1 = new AdaptiveColumnSet();
            var col1 = new AdaptiveColumn();
            var row1 = new AdaptiveTextBlock();
            row1.Text = data.installs.ToString();
            row1.Size = AdaptiveTextSize.Large;
            row1.IsSubtle = true;
           // secondcontainer.Items.Add(row1);
            var row2 = new AdaptiveTextBlock();
            row2.Text = "Installs";
            row2.Size = AdaptiveTextSize.Small;
            row2.IsSubtle = true;
            row2.Spacing = AdaptiveSpacing.None;
            col1.Items.Add(row1);
            col1.Items.Add(row2);
            colset1.Columns.Add(col1);
            secondcontainer.Items.Add(colset1);
            card.Body.Add(firstcontainer);
            card.Body.Add(secondcontainer);


            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }

        public static Attachment CreateAdapativecardforImpressions(CampaignContent data)
        {


            DateTime dt = DateTime.Parse(data.date.ToLongDateString());
            AdaptiveCard card = new AdaptiveCard();
            card.Speak = "";
            AdaptiveContainer firstcontainer = new AdaptiveContainer();
            var headerrow = new AdaptiveTextBlock();
            headerrow.Text = "Campaign ID:" + " " + data.campaignId;
            headerrow.Size = AdaptiveTextSize.Large;
            headerrow.Weight = AdaptiveTextWeight.Bolder;
            // headerrow.IsSubtle = true;
            var daterow = new AdaptiveTextBlock();
            
            daterow.Text = data.date.ToLongDateString();
            daterow.Size = AdaptiveTextSize.Small;
            // daterow.IsSubtle = true;
            firstcontainer.Items.Add(headerrow);
            firstcontainer.Items.Add(daterow);

            AdaptiveContainer secondcontainer = new AdaptiveContainer();
            var colset1 = new AdaptiveColumnSet();
            var col1 = new AdaptiveColumn();
            var row1 = new AdaptiveTextBlock();
            row1.Text = data.impressions.ToString();
            row1.Size = AdaptiveTextSize.Large;
            row1.IsSubtle = true;
            secondcontainer.Items.Add(row1);
            var row2 = new AdaptiveTextBlock();
            row2.Text = "Impressions";
            row2.Size = AdaptiveTextSize.Small;
            row2.IsSubtle = true;
            row2.Spacing = AdaptiveSpacing.None;
            col1.Items.Add(row1);
            col1.Items.Add(row2);
            colset1.Columns.Add(col1);
            secondcontainer.Items.Add(colset1);
            card.Body.Add(firstcontainer);
            card.Body.Add(secondcontainer);


            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }

        //cost cards


        //impression cards

        //public static Attachment CreateAdapativecardforCampaignSummary(List<CampaignContent> data)
        //{

        //    AdaptiveCard card = new AdaptiveCard();
        //    card.Speak = "";

        //    AdaptiveTextBlock firsttextblock = new AdaptiveTextBlock();
        //    firsttextblock.Text = "Campaign Summary";
        //    firsttextblock.Weight = AdaptiveTextWeight.Bolder;
        //    firsttextblock.Size = AdaptiveTextSize.Large;

        //    card.Body.Add(firsttextblock);

        //    foreach(CampaignContent info in data)
        //    {
        //        AdaptiveFactSet factset = new AdaptiveFactSet();
        //        factset.Separator = true;
        //        AdaptiveFact campaign = new AdaptiveFact();
        //        campaign.Title = "Campaign ID:";
        //        campaign.Value = info.campaignId;
        //        AdaptiveFact date = new AdaptiveFact();
        //        date.Title = "Date:";
        //        date.Value = info.date.ToShortDateString();
        //        AdaptiveFact spend = new AdaptiveFact();
        //        spend.Title = "Total Spent:";
        //        spend.Value = info.spend+ " "+ info.currencyCode;
        //        AdaptiveFact installs = new AdaptiveFact();
        //        installs.Title = "installs:";
        //        installs.Value = info.installs.ToString();
        //        AdaptiveFact impressions = new AdaptiveFact();
        //        impressions.Title = "impressions:";
        //        impressions.Value = info.impressions.ToString();
        //        AdaptiveFact activeUsers = new AdaptiveFact();
        //        activeUsers.Title = "Active Users:";
        //        activeUsers.Value = info.activeUsers.ToString();
        //        card.Body.Add(factset);
        //    }


        //    Attachment attachment = new Attachment()
        //    {
        //        ContentType = AdaptiveCard.ContentType,
        //        Content = card
        //    };
        //    return attachment;
        //}
    }
}