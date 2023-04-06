using System.Runtime;
using System.Runtime.InteropServices.ComTypes;

namespace StazEdhouse;

public class Map
{
    /* Class Constructor */
    public Map(string map, int rowCount, int columnCount)
    {
        SetDimensions(rowCount, columnCount);
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

    private void SetDimensions(int rowCount, int columnCount)
    {
        _rowCount = rowCount;
        _columnCount = columnCount;
    }

    /* Main methods */
    private void ParseMap(string map)
    {
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
        // prepares threads
        var threadLeft = new Thread(() => CallMarkingFromLeft());
        var threadTop = new Thread(() => CallMarkingFromTop());
        var threadRight = new Thread(() => CallMarkingFromRight());
        var threadBottom = new Thread(() => CallMarkingFromBottom());
        
        // strating threads
        threadLeft.Start();
        threadTop.Start();
        threadRight.Start();
        threadBottom.Start();

        // waiting for all threads
        threadLeft.Join();
        threadTop.Join();
        threadRight.Join();
        threadBottom.Join();
    }
    
    private void CallMarkingFromLeft()
    {
        for (var i = 0; i < GetRowCount(); i++)
        {
            var row = i;
            MarkVisibleTreesFromLeft(row);
        }
    }

    private void CallMarkingFromTop()
    {
        for (var i = 0; i < GetColumnCount(); i++)
        {
            var column = i;
            MarkVisibleTreesFromTop(column);
        }
    }

    private void CallMarkingFromRight()
    {
        for (var i = 0; i < GetRowCount(); i++)
        {
            var row = i;
            MarkVisibleTreesFromRight(row);
        }
    }

    private void CallMarkingFromBottom()
    {
        for (var i = 0; i < GetColumnCount(); i++)
        {
            var column = i;
            MarkVisibleTreesFromBottom(column);
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
