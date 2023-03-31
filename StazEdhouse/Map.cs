namespace StazEdhouse;

public class Map
{
    public Map(string map)
    {
        SetDimensions(map);
        _map = new Tree[GetRowCount(), GetColumnCount()];

        var index = -1;
        
        for (var i = 0; i < GetColumnCount(); i++)
        {
            for (var j = 0; j < GetRowCount(); j++)
            {
                _map[i, j] = new Tree(GetNextInt(map, index, ref index));
            }
        }

    }
    
    private int _rowCount;
    private int _columnCount;
    private Tree[,] _map;

    private int GetRowCount()
    {
        return _rowCount;
    }
    
    private int GetColumnCount()
    {
        return _columnCount;
    }

    private void SetDimensions(string s)
    {
        int row = 0;
        int column = 0;

        for (var j = 0; j < s.Length; j++)
        {
            if (s[j] == '\r')
            {
                continue;
            }

            if (s[j] == '\n')
            {
                column++;
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

    private int GetNextInt(string s, int number, ref int index)
    {
        number++;
        for (var i = number; i < s.Length; i++)
        {
            if (s[i] <= '9' && s[i] >= '0')
            {
                index = i;
                return int.Parse(s[i].ToString());
            }
        }

        index = -1;
        return -1;
    }

    public int GetVisibleCount()
    {
        MarkVisibleTrees();
        var counter = 0;
        for (var i = 0; i < GetRowCount(); i++)
        {
            for (var j = 0; j < GetColumnCount(); j++)
            {
                if (_map[i, j].VisibleFromLeft || _map[i, j].VisibleFromRight || _map[i, j].VisibleFromTop ||
                    _map[i, j].VisibleFromBottom)
                {
                    counter++;
                }
            }
        }

        return counter;
    }

    private void MarkVisibleTrees()
    {
        var threadsLeft = new Thread[GetRowCount()];
        var threadsRight = new Thread[GetRowCount()];
        var threadsTop = new Thread[GetColumnCount()];
        var threadsBottom = new Thread[GetColumnCount()];

        for (var i = 0; i < GetRowCount(); i++)
        {
            var localNum = i;
            threadsLeft[i] = new Thread(() => MarkVisibleFromLeft(localNum, 0));
        }
        
        for (var i = 0; i < GetRowCount(); i++)
        {
            var localNum = i;
            threadsRight[i] = new Thread(() => MarkVisibleFromRight(localNum, GetColumnCount() - 1));
        }
        
        for (var i = 0; i < GetColumnCount(); i++)
        {
            var localNum = i;
            threadsTop[i] = new Thread(() => MarkVisibleFromTop(0, localNum));
        }
        
        for (var i = 0; i < GetColumnCount(); i++)
        {
            var localNum = i;
            threadsBottom[i] = new Thread(() => MarkVisibleFromBottom(GetRowCount() - 1, localNum));
        }
        
        for (var i = 0; i < GetRowCount(); i++)
        {
            threadsLeft[i].Start();
            threadsRight[i].Start();
        }
        
        for (var i = 0; i < GetColumnCount(); i++)
        {
            threadsTop[i].Start();
            threadsBottom[i].Start();
        }

        for (var i = 0; i < GetRowCount(); i++)
        {
            threadsLeft[i].Join();
            threadsRight[i].Join();
        }
        
        for (var i = 0; i < GetColumnCount(); i++)
        {
            threadsTop[i].Join();
            threadsBottom[i].Join();
        }

    }
    
    private void MarkVisibleFromLeft(int row, int column)
    {
        MarkVisible(row, column, 1, GetRowCount());
    }
    
    private void MarkVisibleFromRight(int row, int column)
    {
        MarkVisible(row, column, 3, GetRowCount());
    }
    
    private void MarkVisibleFromTop(int row, int column)
    {
        MarkVisible(row, column, 2, GetColumnCount());
    }
    
    private void MarkVisibleFromBottom(int row, int column)
    {
        MarkVisible(row, column, 4, GetColumnCount());
    }

    private void MarkVisible(int row, int column, int direction, int times)
    {
        var max = -1; 
        while (times > 0)
        {
            //Console.Write($"row={row} column={column} height={_map[row, column].Height}\n");
            if (max < _map[row, column].Height)
            {
                switch (direction)
                {
                    case 1:
                        _map[row, column].VisibleFromLeft = true;
                        max = _map[row, column].Height;
                        column++;
                        break;
                    case 3:
                        _map[row, column].VisibleFromRight = true;
                        max = _map[row, column].Height;
                        column--;
                        break;
                    case 2:
                        _map[row, column].VisibleFromTop = true;
                        max = _map[row, column].Height;
                        row++;
                        break;
                    default:
                        _map[row, column].VisibleFromBottom = true;
                        max = _map[row, column].Height;
                        row--;
                        break;
                }
            }
            else
            {
                switch (direction)
                {
                    case 1:
                        column++;
                        break;
                    case 3:
                        column--;
                        break;
                    case 2:
                        row++;
                        break;
                    default:
                        row--;
                        break;
                }
            }

            times--;
        }
        
    }
}