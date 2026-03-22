using System;
using System.IO;

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

    static string ReadFile(string path)
    {
        try
        {
            string val = File.ReadAllText(path).Trim();
            return string.IsNullOrWhiteSpace(val) ? "N/A" : val;
        }
        catch { return "N/A"; }
    }

    static string GetMac()
    {
        string[] ifaces = { "eth0", "ens33", "ens3", "enp0s3", "enp1s0", "wlan0", "wlp2s0" };
        foreach (var iface in ifaces)
        {
            string path = $"/sys/class/net/{iface}/address";
            if (File.Exists(path))
                return ReadFile(path);
        }
        try
        {
            foreach (var dir in Directory.GetDirectories("/sys/class/net/"))
            {
                string name = Path.GetFileName(dir);
                if (name == "lo") continue;
                string addrPath = Path.Combine(dir, "address");
                if (File.Exists(addrPath))
                    return ReadFile(addrPath);
            }
        }
        catch { }
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
        Console.WriteLine("  System Serial Information  —  Linux");
        Console.ResetColor();

        PrintLine(60);

        string serial = ReadFile("/sys/class/dmi/id/product_serial");
        string uuid = ReadFile("/sys/class/dmi/id/product_uuid");
        string model = ReadFile("/sys/class/dmi/id/product_name");
        string maker = ReadFile("/sys/class/dmi/id/sys_vendor");
        string biosVer = ReadFile("/sys/class/dmi/id/bios_version");
        string biosDate = ReadFile("/sys/class/dmi/id/bios_date");
        string mbMaker = ReadFile("/sys/class/dmi/id/board_vendor");
        string mbModel = ReadFile("/sys/class/dmi/id/board_name");
        string mbSerial = ReadFile("/sys/class/dmi/id/board_serial");
        string mac = GetMac();

        PrintSection("System");
        PrintField("PC Serial Number", serial);
        PrintField("Machine UUID", uuid);
        PrintField("PC Model", model);
        PrintField("Manufacturer", maker);
        PrintField("BIOS Version", biosVer);
        PrintField("BIOS Date", biosDate);

        PrintLine(60);

        PrintSection("Motherboard");
        PrintField("MB Manufacturer", mbMaker);
        PrintField("MB Model", mbModel);
        PrintField("MB Serial Number", mbSerial);

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