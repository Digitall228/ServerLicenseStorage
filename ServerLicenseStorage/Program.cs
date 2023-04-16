using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;

namespace ServerLicenseStorage
{
    class Program
    {
        const int port = 8888;
        static TcpListener listener;
        static void ConsoleChecker()
        {
            while (true)
            {
                string command = Console.ReadLine();
                switch(command.ToLower())
                {
                    case "getalllicenses":
                        for (int i = 0; i < LicensePool.licenseLists.Count; i++)
                        {
                            for (int x = 0; x < LicensePool.licenseLists[i].licenses.Count; x++)
                            {
                                Console.WriteLine(JsonConvert.SerializeObject(LicensePool.licenseLists[i].licenses[x]));
                            }
                        }
                        break;
                }
            }
        }
        static void Main(string[] args)
        {
            License license = new License();
            license.key = "qwqwqwqw";
            license.isActivated = false;
            license.startTime = DateTime.Now;
            license.expireTime = TimeSpan.FromDays(3);
            license.licenseType = LicenseType.SophisticatedProgram;

            Command command = new Command();
            command.commanType = CommanType.CreateNewLicense;
            command.license = license;

            string message = JsonConvert.SerializeObject(command);

            Thread thread = new Thread(ConsoleChecker);
            thread.Start();
            //
            try
            {
                listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
                listener.Start();
                Console.WriteLine("Waiting for connection...");

                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();

                    Task clientThread = new Task(() => Service(client));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (listener != null)
                    listener.Stop();
            }
        }

        static void Service(TcpClient client)
        {
            NetworkStream stream = null;
            try
            {
                stream = client.GetStream();
                byte[] data = new byte[64];
                while (true)
                {
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string message = builder.ToString();

                    Command command = JsonConvert.DeserializeObject<Command>(message);
                    Answer answer = new Answer();

                    switch (command.commanType)
                    {
                        case CommanType.CreateNewLicense:
                            CreateNewLicense(command.license);

                            answer.anwerType = AnwerType.Good;
                            answer.details = JsonConvert.SerializeObject(command.license);

                            break;
                        case CommanType.CheckLicense:
                            if(CheckLicense((string)command.data, out string error))
                            {
                                answer.anwerType = AnwerType.Good;
                                answer.details = error;
                            }
                            else
                            {
                                answer.anwerType = AnwerType.Bad;
                                answer.details = error;
                            }

                            break;
                    }

                    message = JsonConvert.SerializeObject(answer);
                    data = Encoding.Unicode.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                if (client != null)
                    client.Close();
            }
        }
        static void CreateNewLicense(License license)
        {
            LicensePool.AddLicense(license);
        }
        static bool CheckLicense(string licenseKey, out string error)
        {
            error = "";

            License license = LicensePool.FindLicense(licenseKey);
            if(license == null)
            {
                error = "licenseKey is not correct";
                return false;
            }
            else
            {
                if(!license.isActivated)
                {
                    license.isActivated = true;
                    license.startTime = DateTime.Now;

                    return true;
                }
                else
                {
                    if(DateTime.Now > license.startTime + license.expireTime)
                    {
                        error = "License has been expired";
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }
    }
}
