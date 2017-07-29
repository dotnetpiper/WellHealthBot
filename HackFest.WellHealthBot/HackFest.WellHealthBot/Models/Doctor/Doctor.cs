﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HackFest.WellHealthBot.Models.Doctor
{
    [Serializable]
    public class Doctor
    {
        public string Name { get; set; }

        public int Rating { get; set; }

        public int NumberOfReviews { get; set; }

        public string Image { get; set; }

        public string Location { get; set; }
    }
}