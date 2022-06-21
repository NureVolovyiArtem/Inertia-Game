﻿using CommonCodebase.Core;
using CommonCodebase.Entities;
using CommonCodebase.Miscellaneous;
using Pastel;
using System.Text;
using System.Text.RegularExpressions;
using CommonCodebase.Engines;

namespace Engines;

public static class GraphicsEngine
{
    private const int LeftSectionWidth = 38;
    private const int RightSectionWidth = 32;

    private const int PlayZoneScreenWidth = LeftSectionWidth + Map.Width + RightSectionWidth;
    private const int PlayZoneScreenHeight = Map.Height * 2;

    private const int MapLocationX = LeftSectionWidth;
    private const int MapLocationY = PlayZoneScreenHeight / 4;

    public static void SetPlayZoneScreen()
    {
        Console.Clear();
        Console.CursorVisible = false;
        Console.SetWindowSize(PlayZoneScreenWidth, PlayZoneScreenHeight);
        Console.SetBufferSize(PlayZoneScreenWidth, PlayZoneScreenHeight);
    }

    public static void SetScreen(int x, int y)
    {
        Console.Clear();
        Console.CursorVisible = false;
        Console.SetWindowSize(x, y);
        Console.SetBufferSize(x, y);
    }

    public static void DrawControls(object? sender, EventArgs e)
    {
        if (sender is not ControlsTip controlsTip)
        {
            return;
        }

        SetCursorPosition(0, Map.Height - (ControlsTip.Height / 2));

        DrawCentered(LeftSectionWidth, controlsTip.Text);
    }

    public static void DrawCell(object? sender, EventArgs e)
    {
        if (sender is not CellBase cell)
        {
            return;
        }

        var symbol = cell switch
        {
            Empty => " ",
            Player => "I",
            Prize => "@",
            Stop => ".",
            Trap => "%",
            Wall => "#",
            _ => "?" // Error value
        };

        SetCursorPosition(cell.X + MapLocationX, cell.Y + MapLocationY);

        DrawText(symbol.Pastel(cell.Color));
    }

    public static void DrawScore(object? sender, EventArgs e)
    {
        if (sender is not Score score)
        {
            return;
        }

        var text = score.Text + score.ScoreToDraw.ToString().Pastel(score.Color);

        SetCursorPosition(LeftSectionWidth + Map.Width, Map.Height - (Score.Height / 2));

        DrawCentered(RightSectionWidth, text);
    }

    public static void DrawCentered(int width, string text)
    {
        // Needed for correct output
        Console.OutputEncoding = Encoding.UTF8;

        var asciiColorCodeRegex = new Regex("\u001B\\[[;\\d]*[ -/]*[@-~]");

        foreach (var line in text.Split('\n'))
        {
            // Getting a line without ASCII color codes to use its length for correct centering
            var colorlessLine = asciiColorCodeRegex.Replace(line, "");

            Console.WriteLine(new string(' ', (width - colorlessLine.Length) / 2) + line);
        }
    }

    public static void MovementAnimation()
    {
        // Artificial lag for smooth movement
        Thread.Sleep(MovementEngine.FrameMs);
    }

    private static void DrawText(string text)
    {
        Console.Write(text);
    }

    private static void SetCursorPosition(int x, int y)
    {
        Console.SetCursorPosition(x, y);
    }
}