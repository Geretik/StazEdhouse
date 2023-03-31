using StazEdhouse;

var contents = File.ReadAllText(@"/Users/gerete/Downloads/mapa_mala.txt");


var haf = new Map(contents);

Console.WriteLine(haf.GetVisibleCount());

