namespace StazEdhouse;

public class Tree
{
    public Tree()
    {
    }

    public Tree(int height)
    {
        Height = height;
    }


    public int Height { get; set; }

    public bool VisibleFromTop { get; set; }

    public bool VisibleFromBottom { get; set; }

    public bool VisibleFromLeft { get; set; }

    public bool VisibleFromRight { get; set; }
}