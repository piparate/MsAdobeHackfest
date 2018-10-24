using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuisBot.Models
{
   

    public class CampaignContent
    {
        public DateTime date { get; set; }
        public string applicationId { get; set; }
        public string campaignId { get; set; }
        public string lineId { get; set; }
        public string currencyCode { get; set; }
        public float spend { get; set; }
        public float budget { get; set; }
        public int impressions { get; set; }
        public int installs { get; set; }
        public int clicks { get; set; }
        public int iapInstalls { get; set; }
        public int activeUsers { get; set; }
    }

}