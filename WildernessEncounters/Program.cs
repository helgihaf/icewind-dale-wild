using System;

class WildernessEncounters
{
    static readonly Random Rng = new Random();

    static int Roll(int sides) => Rng.Next(1, sides + 1);

    static readonly string[] Encounters =
    {
        "",                                    // 0  (unused)
        "Yeti",                                // 1
        "Goliath werebear",                    // 2
        "Crag cats",                           // 3
        "Coldlight walker",                    // 4
        "Ice troll",                           // 5
        "Frost druid and friends",             // 6
        "Chardalyn berserkers",                // 7
        "Frost giant riding a mammoth",        // 8
        "Battlehammer dwarves",                // 9
        "Arveiaturace (ancient white dragon)", // 10
        "Snowy owlbear",                       // 11
        "Gnolls",                              // 12
        "Orcs of the Many-Arrows tribe",       // 13
        "Goliath party",                       // 14
        "Chwinga",                             // 15
        "Awakened beast",                      // 16
        "Icewind kobolds",                     // 17
        "Humans",                              // 18
        "Herd of beasts",                      // 19
        "Perytons",                            // 20
    };

    static readonly string[] Difficulties =
    {
        "",        // 0
        "Varies",  // 1  Yeti
        "Easy",    // 2  Goliath werebear
        "Easy",    // 3  Crag cats
        "Medium",  // 4  Coldlight walker
        "Hard",    // 5  Ice troll
        "Medium",  // 6  Frost druid and friends
        "Hard",    // 7  Chardalyn berserkers
        "Deadly",  // 8  Frost giant riding a mammoth
        "Easy",    // 9  Battlehammer dwarves
        "Deadly",  // 10 Arveiaturace
        "Easy",    // 11 Snowy owlbear
        "Medium",  // 12 Gnolls
        "Hard",    // 13 Orcs of the Many-Arrows tribe
        "Medium",  // 14 Goliath party
        "Easy",    // 15 Chwinga
        "Easy",    // 16 Awakened beast
        "Easy",    // 17 Icewind kobolds
        "Easy",    // 18 Humans
        "Easy",    // 19 Herd of beasts
        "Medium",  // 20 Perytons
    };

    static string TimeOfDay(int d4Roll) => d4Roll switch
    {
        1 => "Morning   (dawn to noon)",
        2 => "Afternoon (noon to dusk)",
        3 => "Evening   (dusk to midnight)",
        4 => "Night     (midnight to dawn)",
        _ => "Unknown"
    };

    static ConsoleColor DifficultyColor(string difficulty) => difficulty switch
    {
        "Easy"   => ConsoleColor.Green,
        "Medium" => ConsoleColor.Yellow,
        "Hard"   => ConsoleColor.DarkYellow,
        "Deadly" => ConsoleColor.Red,
        "Varies" => ConsoleColor.Cyan,
        _        => ConsoleColor.White
    };

    static void Write(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ResetColor();
    }

    static void WriteLine(string text, ConsoleColor color)
    {
        Write(text + "\n", color);
    }

    static void RunSingleEncounter(int encounterNumber, string timeLabel)
    {
        int encounterRoll = Roll(20);
        int blizzardDieRaw = Roll(20);
        int blizzardTotal = blizzardDieRaw + 1;
        bool blizzard = blizzardTotal > encounterRoll;

        string encounter = Encounters[encounterRoll];
        string difficulty = Difficulties[encounterRoll];

        Write($"\n  Encounter {encounterNumber}", ConsoleColor.White);
        if (!string.IsNullOrEmpty(timeLabel))
            Write($" — {timeLabel}", ConsoleColor.DarkCyan);
        Console.WriteLine();

        Console.Write($"  Encounter die : ");
        WriteLine($"{encounterRoll,2}", ConsoleColor.Cyan);

        Console.Write($"  Blizzard die  : {blizzardDieRaw} + 1 = ");
        Write($"{blizzardTotal,2}", ConsoleColor.Cyan);
        Console.Write("  (must exceed encounter die of ");
        Write($"{encounterRoll}", ConsoleColor.Cyan);
        Console.WriteLine(" to trigger blizzard)");

        Console.Write($"  Encounter     : ");
        Write($"{encounter}", ConsoleColor.White);
        Console.Write(" — ");
        WriteLine(difficulty, DifficultyColor(difficulty));

        if (blizzard)
        {
            int houresBefore = Roll(4);
            int hoursAfter = Roll(4);
            Write("  Blizzard      : ", ConsoleColor.White);
            Write("YES", ConsoleColor.Cyan);
            Console.WriteLine($" — begins {houresBefore}d4={houresBefore}h before encounter, ends {hoursAfter}d4={hoursAfter}h after resolution");
        }
        else
        {
            Write("  Blizzard      : ", ConsoleColor.White);
            WriteLine("No", ConsoleColor.DarkGray);
        }
    }

    static void GenerateDayEncounters()
    {
        int d8 = Roll(8);
        Console.Write("\nRolling d8 for number of encounters: ");
        WriteLine($"{d8}", ConsoleColor.Cyan);
        Console.WriteLine(new string('-', 55));

        if (d8 >= 7)
        {
            WriteLine("\nResult: No random encounters today.", ConsoleColor.DarkGray);
            return;
        }

        if (d8 <= 4)
        {
            // Single encounter; d8 value determines the time slot
            string time = TimeOfDay(d8);
            WriteLine($"Result: 1 encounter — {time}", ConsoleColor.White);
            RunSingleEncounter(1, time);
        }
        else
        {
            // d8 = 5 or 6: two encounters, each time slot rolled separately on a d4
            WriteLine("Result: 2 encounters — rolling d4 for each time slot", ConsoleColor.White);

            for (int i = 1; i <= 2; i++)
            {
                int d4 = Roll(4);
                string time = TimeOfDay(d4);
                Console.Write($"\n  Time slot roll (d4): ");
                Write($"{d4}", ConsoleColor.Cyan);
                Console.WriteLine($" → {time}");
                RunSingleEncounter(i, time);
            }
        }
    }

    static void PrintTable()
    {
        Console.WriteLine();
        WriteLine("  Random Wilderness Encounters Table", ConsoleColor.White);
        Console.WriteLine(new string('-', 55));
        Console.WriteLine($"  {"d20",-5}{"Encounter",-38}{"Difficulty"}");
        Console.WriteLine(new string('-', 55));

        for (int i = 1; i <= 20; i++)
        {
            Console.Write($"  {i,-5}{Encounters[i],-38}");
            WriteLine(Difficulties[i], DifficultyColor(Difficulties[i]));
        }

        Console.WriteLine();
        WriteLine("  Blizzard rule: (blizzard die + 1) > encounter die", ConsoleColor.DarkGray);
        WriteLine("  Low rolls (e.g. 1-Yeti) almost always in a blizzard.", ConsoleColor.DarkGray);
        WriteLine("  High rolls (e.g. 20-Perytons) almost never are.", ConsoleColor.DarkGray);
    }

    static void PrintMenu()
    {
        Console.WriteLine();
        WriteLine("  ==========================================", ConsoleColor.DarkBlue);
        WriteLine("    Icewind Dale — Wilderness Encounters", ConsoleColor.White);
        WriteLine("  ==========================================", ConsoleColor.DarkBlue);
        Console.WriteLine();
        Console.WriteLine("  [1]  Generate encounters for the day (roll d8)");
        Console.WriteLine("  [2]  Run a single encounter now");
        Console.WriteLine("  [3]  Show the encounters table");
        Console.WriteLine("  [Q]  Quit");
        Console.WriteLine();
        Console.Write("  Choice: ");
    }

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        while (true)
        {
            try { Console.Clear(); } catch { }
            PrintMenu();

            string input = (Console.ReadLine() ?? "").Trim().ToUpper();

            Console.WriteLine();

            switch (input)
            {
                case "1":
                    GenerateDayEncounters();
                    break;

                case "2":
                    Console.WriteLine("--- Single Encounter ---");
                    RunSingleEncounter(1, "");
                    break;

                case "3":
                    PrintTable();
                    break;

                case "Q":
                    Console.WriteLine("Farewell, brave adventurer.");
                    return;

                default:
                    WriteLine("  Invalid choice — press any key.", ConsoleColor.Red);
                    break;
            }

            Console.WriteLine("\nPress any key to return to the menu...");
            try { Console.ReadKey(true); } catch { }
        }
    }
}
