using System;
using System.Collections.Generic;
using System.Text;

namespace Apicalypse.DotNet.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ExcludeAttribute : Attribute
    {
        public ExcludeAttribute()
        {
        }
    }
}
