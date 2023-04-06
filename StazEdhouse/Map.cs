using System.Diagnostics;
using System.Text;
using StazEdhouse;

var map = new StringBuilder("", 20000);
var rows = 0;
var cols = 0;
var columns = new List<int>();

while (Console.ReadLine() is { } line && line != "")
{
    foreach (var l in line)
    {
        switch (l)
        {
            case '\r':
                continue;
            case < '0' or > '9':
                Console.WriteLine("Input error");
                return;
            default:
                cols++;
                break;
        }
    }

    columns.Add(cols);
    cols = 0;

    rows++;
    map.Append(line);
    map.Append('\n');
}

cols = columns[0];
if (columns.Any(i => i != cols))
{
    Console.WriteLine("Input error");
    return;
}

var haf = new Map(map.ToString(), rows, columns[0]);

Console.WriteLine(haf.GetVisibleCount());
