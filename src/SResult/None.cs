namespace SResult;

public class None
{
    private static readonly None instance = new ();

    private None()
    {

    }

    public static None Instance => instance;
}