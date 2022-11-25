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
GameState.ShootCannon();

Task game = new(StartGame);

game.Start();
game.Wait();

static void PlayerInput()
{
    if (KeyboardControl.KeyboardKeyDown(out List<ConsoleKey> keysPressed))
    {
        GameState.ConsecutiveKeyPresses++;
        foreach (ConsoleKey key in keysPressed)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow: 
                    {
                        GameState.MoveTank(GameState.UserAction.Up);
                    } 
                    break;
                case ConsoleKey.RightArrow:
                    {
                        GameState.MoveTank(GameState.UserAction.Right);
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    {
                        GameState.MoveTank(GameState.UserAction.Left);
                    }
                    break;
                case ConsoleKey.Spacebar:
                    {
                        GameState.ShootCannon();
                    }
                    break;
            }
        }
        
    } else
    {
        GameState.ConsecutiveKeyPresses = 0;
    }
}


static void StartGame()
{
    ScreenBuffer.DrawText(0, 0, "Press any key to start game loop");
    GameState.AddTank();

    ScreenBuffer.DrawScreen();
    Console.ReadKey();
    GameState.StartGame();
    while (GameState.Running)
    {
        Thread.Sleep(30);
        PlayerInput();
        GameState.GameTick();
    }
}



