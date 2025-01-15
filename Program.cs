// Task 1: Server (Console)
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class Server
{
    static void Main(string[] args)
    {
        TcpListener listener = new TcpListener(IPAddress.Any, 5000);
        listener.Start();
        Console.WriteLine("Server started...");

        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();
            ThreadPool.QueueUserWorkItem(ClientHandler, client);
        }
    }

    private static void ClientHandler(object obj)
    {
        TcpClient client = (TcpClient)obj;
        NetworkStream stream = client.GetStream();

        byte[] buffer = new byte[1024];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        Console.WriteLine($"[{DateTime.Now:HH:mm}] Received from client: {message}");

        string response = "Hello, client!";
        byte[] responseBytes = Encoding.UTF8.GetBytes(response);
        stream.Write(responseBytes, 0, responseBytes.Length);

        client.Close();
    }
}

// Task 1: Client (WinForms)
// Create a new Windows Forms App and replace Form1.cs code with the following:

using System;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private void btnSend_Click(object sender, EventArgs e)
    {
        using (TcpClient client = new TcpClient("127.0.0.1", 5000))
        {
            NetworkStream stream = client.GetStream();

            string message = "Hello, server!";
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);

            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            MessageBox.Show($"[{DateTime.Now:HH:mm}] Received from server: {response}");
        }
    }
}

// Add a Button (btnSend) to the Form in the designer.

// Task 2: Server (Console)
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class TimeServer
{
    static void Main(string[] args)
    {
        TcpListener listener = new TcpListener(IPAddress.Any, 5001);
        listener.Start();
        Console.WriteLine("Time server started...");

        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();
            NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"[{DateTime.Now:HH:mm}] Request: {request}");

            string response = request.ToLower() == "time" ? DateTime.Now.ToShortTimeString() : DateTime.Now.ToShortDateString();
            byte[] responseBytes = Encoding.UTF8.GetBytes(response);
            stream.Write(responseBytes, 0, responseBytes.Length);

            client.Close();
        }
    }
}

// Task 2: Client (WinForms)
// Create a new Windows Forms App and replace Form1.cs code with the following:

using System;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private void btnRequest_Click(object sender, EventArgs e)
    {
        using (TcpClient client = new TcpClient("127.0.0.1", 5001))
        {
            NetworkStream stream = client.GetStream();

            string request = txtRequest.Text.Trim();
            byte[] data = Encoding.UTF8.GetBytes(request);
            stream.Write(data, 0, data.Length);

            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            MessageBox.Show($"[{DateTime.Now:HH:mm}] Response: {response}");
        }
    }
}

// Add a TextBox (txtRequest) and Button (btnRequest) to the Form in the designer.
