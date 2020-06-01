using Apicalypse.DotNet.Interpreters;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Apicalypse.DotNet.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var arg in args)
                Console.WriteLine(arg);


            //var builder = new RequestBuilder<Game>().Where(g => g.Name.Contains("Name", StringComparison.InvariantCultureIgnoreCase) && g.Checksum.Contains("plop"));
            //var builder2 = new RequestBuilder<Game>().Where<Game>(g => !new int[] { 1, 2, 3 }.Equals(g.AlternativeNames));
        }
    }
}
