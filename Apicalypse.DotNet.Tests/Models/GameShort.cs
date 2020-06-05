using Apicalypse.DotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Apicalypse.DotNet.Tests.Models
{
    class GameShort
    {
        public string Name { get; set; }

        public string Slug { get; set; }

        public uint Follows { get; set; }

        public IEnumerable<int> AlternativeNames { get; set; }

        [Include]
        public Franchise Franchise { get; set; }

        [Exclude]
        public ExcludedChild ExcludedChild { get; set; }
    }
}
