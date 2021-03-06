﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Apicalypse.DotNet.Tests.Models
{
    class Game
    {
        public string Name { get; set; }

        public string Slug { get; set; }

        public uint Follows { get; set; }

        public IEnumerable<int> AlternativeNames { get; set; }

        public Franchise Franchise { get; set; }
        
        public Cover Cover { get; set; }

        public bool EarlyAccess { get; set; }
    }
}
