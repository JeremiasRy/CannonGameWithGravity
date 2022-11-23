﻿using GameEngine.GameUtility;
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
        GameState.ConsecutiveKeyPresses++;
        var multiplyAmount = GameState.ConsecutiveKeyPresses > 14 ? 14 : GameState.ConsecutiveKeyPresses;
        foreach (ConsoleKey key in keysPressed)
        {
            switch (key)
            {
                case ConsoleKey.Spacebar: 
                    {
                        GameState.ApplyUserInput(-50 * multiplyAmount, true);
                    } 
                    break;
                case ConsoleKey.RightArrow:
                    {
                        GameState.ApplyUserInput(50 * multiplyAmount, false);
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    {
                        GameState.ApplyUserInput(-50 * multiplyAmount, false);
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
    ScreenBuffer.DrawScreen();
    Console.ReadKey();
    GameState.StartGame();
    while (GameState.Running)
    {
        Thread.Sleep(35);
        PlayerInput();
        GameState.GameTick();
    }
}

