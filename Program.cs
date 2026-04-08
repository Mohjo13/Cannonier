
internal class Program
{
    private static void Main(string[] args)
    {
        using var game = new Cannoneer.Game1();
        game.Run();
    }
}