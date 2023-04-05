using System.Runtime.InteropServices.ComTypes;

namespace StazEdhouse;

public class Map
{
    /* Class Constructor */
    public Map(string map)
    {
        // assuming map in string in correct form
        ParseMap(map);
        MarkVisibleTrees();
    }

    /* Object slots */
    private int _rowCount;
    private int _columnCount;
    private Tree[,] _map;

    /* Getters */
    private int GetRowCount()
    {
        return _rowCount;
    }

    private int GetColumnCount()
    {
        return _columnCount;
    }

    private Tree GetMapElement(int row, int column)
    {
        return _map[row, column];
    }


    /* Setters */
    private void SetDimensions(string s)
    {
        // sets row and column dimension
        var row = 0;
        var column = 0;

        for (var j = 0; j < s.Length; j++)
        {
            switch (s[j])
            {
                case '\r':
                    continue;
                case '\n':
                    column++;
                    continue;
            }

            if (column == 0)
            {
                row++;
            }

            if (j == s.Length - 1 && s[j] != '\n')
            {
                column++;
            }
        }

        _rowCount = row;
        _columnCount = column;
    }

    private void ParseMap(string map)
    {
        // Sets maps dimensions
        SetDimensions(map);

        // Local variables
        var index = -1;

        // Preparing 2 dimensions array of Tree class to _class slot 
        _map = new Tree[GetRowCount(), GetColumnCount()];

        // Filling _map with data from string map
        for (var i = 0; i < GetRowCount(); i++)
        {
            for (var j = 0; j < GetColumnCount(); j++)
            {
                _map[i, j] = new Tree(GetNextInt(map, index, ref index));
            }
        }
    }

    private int GetNextInt(string map, int number, ref int index)
    {
        // Returns next int from string map and returns index of the string to the index ref variable
        // Otherwise returns -1 for int value and -1 for index value

        // Starting index for searching
        number++;
        for (var i = number; i < map.Length; i++)
        {
            if (map[i] > '9' || map[i] < '0') continue;
            index = i;
            return int.Parse(map[i].ToString());
        }

        index = -1;
        return -1;
    }

    public int GetVisibleCount()
    {
        // Returns Count of visible trees
        var counter = 0;

        for (var i = 0; i < GetRowCount(); i++)
        {
            for (var j = 0; j < GetColumnCount(); j++)
            {
                if (_map[i, j].GetVisible())
                {
                    counter++;
                }
            }
        }

        return counter;
    }

    private void MarkVisibleTrees()
    {
        // Marking visible trees to the Tree Visible slot

        //thread vars
        var threadsLeft = new Thread[GetColumnCount()];
        var threadsRight = new Thread[GetColumnCount()];
        var threadsTop = new Thread[GetRowCount()];
        var threadsBottom = new Thread[GetRowCount()];

        // preparing left and right Threads
        for (var i = 0; i < GetColumnCount(); i++)
        {
            var row = i;
            threadsLeft[i] = new Thread(() => MarkVisibleTreesFromLeft(row));
            threadsRight[i] = new Thread(() => MarkVisibleTreesFromRight(row));
        }

        // preparing top and bottom Threads
        for (var i = 0; i < GetRowCount(); i++)
        {
            var column = i;
            threadsTop[i] = new Thread(() => MarkVisibleTreesFromTop(column));
            threadsBottom[i] = new Thread(() => MarkVisibleTreesFromBottom(column));
        }

        // starting left and right threads
        for (var i = 0; i < GetColumnCount(); i++)
        {
            threadsLeft[i].Start();
            threadsRight[i].Start();
        }

        // starting top and bottom threads
        for (var i = 0; i < GetRowCount(); i++)
        {
            threadsTop[i].Start();
            threadsBottom[i].Start();
        }

        // waiting for left and right Threads
        for (var i = 0; i < GetColumnCount(); i++)
        {
            threadsLeft[i].Join();
            threadsRight[i].Join();
        }

        // waiting for top and bottom Threads
        for (var i = 0; i < GetRowCount(); i++)
        {
            threadsTop[i].Join();
            threadsBottom[i].Join();
        }
    }

    private void MarkVisibleTreesFromLeft(int row)
    {
        MarkVisibleTrees(row, 0, 0, 1, GetColumnCount());
    }

    private void MarkVisibleTreesFromRight(int row)
    {
        MarkVisibleTrees(row, GetColumnCount() - 1, 0, -1, GetColumnCount());
    }

    private void MarkVisibleTreesFromTop(int column)
    {
        MarkVisibleTrees(0, column, 1, 0, GetRowCount());
    }

    private void MarkVisibleTreesFromBottom(int column)
    {
        MarkVisibleTrees(GetRowCount() - 1, column, -1, 0, GetRowCount());
    }

    private void MarkVisibleTrees(int row, int column, int rowStep, int columnStep, int times)
    {
        var max = -1;
        while (times > 0)
        {
            if (max < _map[row, column].Height)
            {
                _map[row, column].SetVisible();
                max = _map[row, column].Height;
            }

            row += rowStep;
            column += columnStep;
            times--;
        }
    }
}