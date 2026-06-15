namespace ProjectY.Core
{
    public static class GameBootstrap
    {
        public static GameManager CreateGameManager()
        {
            System.Console.WriteLine("Initializing Game Manager...");
            return new GameManager(WorldFactory.CreateStarterWorld());
        }
    }
}
