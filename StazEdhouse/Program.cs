using System.Text;
using StazEdhouse;

try
{
    var map = new StringBuilder("", 20000);

    while (Console.ReadLine() is { } line && line != "")
    {
        map.Append(line);
        map.Append('\n');
    }

    var visibleTrees = new Map(map.ToString());

    Console.WriteLine(visibleTrees.GetVisibleCount());
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}