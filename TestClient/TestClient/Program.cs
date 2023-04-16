using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TestClient
{
    class Program
    {
        const int port = 8888;
        const string address = "127.0.0.1";
        static void Main(string[] args)
        {
            TcpClient client = null;
            try
            {
                client = new TcpClient(address, port);
                NetworkStream stream = client.GetStream();

                /// 1 - create    2 - check
                //string message = @"{""license"":{""key"":""qwqwqwqw"",""startTime"":""2020-09-30T21:46:10.7540828+03:00"",""expireTime"":""3.00:00:00"",""isActivated"":false,""licenseType"":0},""data"":""qwqwqwqw"",""commanType"":0}";
                string message = @"{""license"":{""key"":""qwqwqwqw"",""startTime"":""2020-09-30T21:46:10.7540828+03:00"",""expireTime"":""3.00:00:00"",""isActivated"":false,""licenseType"":0},""data"":""qwqwqwqw"",""commanType"":3}";

                byte[] data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);

                // получаем ответ
                data = new byte[64]; // буфер для получаемых данных
                StringBuilder builder = new StringBuilder();
                int bytes = 0;
                do
                {
                    bytes = stream.Read(data, 0, data.Length);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (stream.DataAvailable);

                message = builder.ToString();
                Console.WriteLine("Сервер: {0}", message);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                client.Close();
            }
        }
    }
}
