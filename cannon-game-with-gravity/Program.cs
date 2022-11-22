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
GameState.AddGameObject();

Task game = new(StartGame);

game.Start();
game.Wait();

static void PlayerInput()
{
    if (KeyboardControl.KeyboardKeyDown(out List<ConsoleKey> keysPressed))
    {
        string keysString = "";
        foreach (ConsoleKey key in keysPressed)
        {
            keysString += "Key Pressed: " + key.ToString() + " ";
        }
        ScreenBuffer.DrawText(1, 0, keysString);
    }
}


static void StartGame()
{
    ScreenBuffer.DrawText(0, 0, "Press any key to start game loop");
    ScreenBuffer.DrawScreen();
    Console.ReadKey();
    GameState.StartGame();
    while (GameState.Running)
    {
        Thread.Sleep(50);
        PlayerInput();
        GameState.GameTick();
    }
}

