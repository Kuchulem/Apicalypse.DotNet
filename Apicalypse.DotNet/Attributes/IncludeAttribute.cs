using System;
using System.Collections.Generic;
using System.Text;

namespace Apicalypse.DotNet.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IncludeAttribute : Attribute
    {
        public IncludeAttribute()
        {
        }
    }
}
