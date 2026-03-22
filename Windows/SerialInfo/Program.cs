using System;
using System.Management;

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
        Console.WriteLine(value ?? "N/A");
        Console.ResetColor();
    }

    static string GetWmiValue(string wmiClass, string property)
    {
        try
        {
            using var searcher = new ManagementObjectSearcher($"SELECT {property} FROM {wmiClass}");
            foreach (ManagementObject obj in searcher.Get())
            {
                var val = obj[property]?.ToString()?.Trim();
                if (!string.IsNullOrEmpty(val))
                    return val;
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
        Console.WriteLine("  System Serial Information");
        Console.ResetColor();

        PrintLine(60);

        string serial = GetWmiValue("Win32_BIOS", "SerialNumber");
        string uuid = GetWmiValue("Win32_ComputerSystemProduct", "UUID");
        string model = GetWmiValue("Win32_ComputerSystem", "Model");
        string maker = GetWmiValue("Win32_ComputerSystem", "Manufacturer");
        string biosVer = GetWmiValue("Win32_BIOS", "SMBIOSBIOSVersion");
        string biosDate = GetWmiValue("Win32_BIOS", "ReleaseDate");
        string mbManufac = GetWmiValue("Win32_BaseBoard", "Manufacturer");
        string mbProduct = GetWmiValue("Win32_BaseBoard", "Product");
        string mbSerial = GetWmiValue("Win32_BaseBoard", "SerialNumber");
        string macAddress = GetWmiValue("Win32_NetworkAdapterConfiguration", "MACAddress");

        if (biosDate != "N/A" && biosDate.Length >= 8)
        {
            try
            {
                biosDate = $"{biosDate[6..8]}/{biosDate[4..6]}/{biosDate[0..4]}";
            }
            catch { }
        }

        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("  [ System ]");
        Console.ResetColor();
        PrintField("PC Serial Number", serial);
        PrintField("Machine UUID", uuid);
        PrintField("PC Model", model);
        PrintField("Manufacturer", maker);
        PrintField("BIOS Version", biosVer);
        PrintField("BIOS Date", biosDate);

        PrintLine(60);

        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("  [ Motherboard ]");
        Console.ResetColor();
        PrintField("MB Manufacturer", mbManufac);
        PrintField("MB Model", mbProduct);
        PrintField("MB Serial Number", mbSerial);

        PrintLine(60);

        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("  [ Network ]");
        Console.ResetColor();
        PrintField("MAC Address", macAddress);

        PrintLine(60);

        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("\n  Press any key to exit...");
        Console.ResetColor();
        Console.ReadKey();
    }
}