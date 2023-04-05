namespace StazEdhouse;

public class Tree
{
    public Tree()
    {
        _visible = false;
    }

    public Tree(int height)
    {
        Height = height;
        _visible = false;
    }

    public int Height { get; set; }

    private volatile bool _visible;

    public void SetVisible()
    {
        _visible = true;
    }

    public bool GetVisible()
    {
        return _visible;
    }
}