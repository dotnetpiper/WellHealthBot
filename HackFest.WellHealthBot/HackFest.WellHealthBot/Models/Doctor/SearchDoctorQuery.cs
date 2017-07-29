using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.FormFlow;

namespace HackFest.WellHealthBot.Models.Doctor
{
    [Serializable]
    public class SearchDoctorQuery
    {
        [Prompt("Please enter your {&}")]
        public string Location { get; set; }
        public Specialist Specialist { get; set; }
    }
}