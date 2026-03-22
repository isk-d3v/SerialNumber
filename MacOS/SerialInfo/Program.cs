using System;
using System.Diagnostics;

class SerialInfo
{
    static void PrintLine(int width = 60)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(new string('─', width));
        Console.ResetColor();
    }

    static void PrintField(string label, string value)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"  {label,-26}: ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(string.IsNullOrWhiteSpace(value) ? "N/A" : value);
        Console.ResetColor();
    }

    static void PrintSection(string title)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"  [ {title} ]");
        Console.ResetColor();
    }

    static string RunCommand(string cmd, string args)
    {
        try
        {
            var psi = new ProcessStartInfo(cmd, args)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            using var p = Process.Start(psi);
            string output = p?.StandardOutput.ReadToEnd()?.Trim() ?? "";
            p?.WaitForExit();
            return string.IsNullOrWhiteSpace(output) ? "N/A" : output;
        }
        catch { return "N/A"; }
    }

    static string ParseSysProfile(string output, string key)
    {
        foreach (var line in output.Split('\n'))
        {
            if (line.IndexOf(key, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                int colon = line.IndexOf(':');
                if (colon >= 0)
                    return line[(colon + 1)..].Trim();
            }
        }
        return "N/A";
    }

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.BackgroundColor = ConsoleColor.Black;
        Console.Clear();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine();
        Console.WriteLine(@"   █████████                      ███            ████ ");
        Console.WriteLine(@"  ███░░░░░███                    ░░░            ░░███ ");
        Console.WriteLine(@" ░███    ░░░   ██████  ████████  ████   ██████   ░███ ");
        Console.WriteLine(@" ░░█████████  ███░░███░░███░░███░░███  ░░░░░███  ░███ ");
        Console.WriteLine(@"  ░░░░░░░░███░███████  ░███ ░░░  ░███   ███████  ░███ ");
        Console.WriteLine(@"  ███    ░███░███░░░   ░███      ░███  ███░░███  ░███ ");
        Console.WriteLine(@"  ░░█████████ ░░██████  █████     █████░░████████ █████");
        Console.WriteLine(@"   ░░░░░░░░░   ░░░░░░  ░░░░░     ░░░░░  ░░░░░░░░ ░░░░░");
        Console.ResetColor();
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("  System Serial Information  —  macOS");
        Console.ResetColor();

        PrintLine(60);

        string hw = RunCommand("system_profiler", "SPHardwareDataType");

        string serial = ParseSysProfile(hw, "Serial Number (system)");
        string uuid = ParseSysProfile(hw, "Hardware UUID");
        string model = ParseSysProfile(hw, "Model Name");
        string maker = "Apple Inc.";
        string biosVer = ParseSysProfile(hw, "Boot ROM Version");
        string mbModel = ParseSysProfile(hw, "Model Identifier");
        string mac = RunCommand("bash", "-c \"ifconfig en0 2>/dev/null | awk '/ether/{print $2}'\"");

        PrintSection("System");
        PrintField("PC Serial Number", serial);
        PrintField("Machine UUID", uuid);
        PrintField("PC Model", model);
        PrintField("Manufacturer", maker);
        PrintField("Boot ROM Version", biosVer);

        PrintLine(60);

        PrintSection("Motherboard");
        PrintField("MB Manufacturer", "Apple Inc.");
        PrintField("MB Model", mbModel);
        PrintField("MB Serial Number", serial);

        PrintLine(60);

        PrintSection("Network");
        PrintField("MAC Address", mac);

        PrintLine(60);

        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("\n  Press any key to exit...");
        Console.ResetColor();
        Console.ReadKey();
    }
}