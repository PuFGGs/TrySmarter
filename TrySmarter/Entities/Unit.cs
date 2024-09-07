namespace TrySmarter.Entities;

public class Unit
{
    private Unit()
    {
    }

    private static Unit? _instance;

    public static Unit Value => _instance ??= new Unit();
}