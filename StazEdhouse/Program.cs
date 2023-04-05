using System.Text;
using StazEdhouse;

//var contents = File.ReadAllText(@"/Users/gerete/Downloads/mapa.txt");

//Console.WriteLine(contents.Length);

var map = new StringBuilder("", 20000);

while (Console.ReadLine() is { } line && line != "")
{
    map.Append(line);
    map.Append('\n');
}

var haf = new Map(map.ToString());

Console.WriteLine(haf.GetVisibleCount());