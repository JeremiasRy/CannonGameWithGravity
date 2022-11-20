using GameEngine.GameUtility;
using GameEngine;

Console.CursorVisible = false;
if (!OperatingSystem.IsWindows())
{
    Console.WriteLine("Sorry only for windows");
    return;
}
Console.SetWindowSize(200, 50);
ScreenBuffer.Initialize();

Task game = new(StartGame);

game.Start();
game.Wait();


static void StartGame()
{
    ScreenBuffer.DrawText(0, 0, "Press any key to start game loop");
    ScreenBuffer.DrawScreen();
    Console.ReadKey();
    GameState.StartGame();
    while (GameState.Running)
    {
        Thread.Sleep(50);
        //Read user input here
        GameState.GameTick();
    }
}

