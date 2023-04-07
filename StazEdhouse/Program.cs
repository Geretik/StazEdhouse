using System.Text;
using StazEdhouse;

var map = new StringBuilder("", 20000);

while (Console.ReadLine() is { } line && line != "")
{
    map.Append(line);
    map.Append('\n');
}

//Console.WriteLine(map.ToString());

var haf = new Map(map.ToString());

Console.WriteLine(haf.GetVisibleCount());
