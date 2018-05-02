using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Deployment.Application;
using System.Threading;
using System.Runtime.InteropServices;

namespace _7D2D_ServerInfo
{
    class Program
    {
        const int SearchForUpdateDelayInMin = 15;

        static void Main(string[] args)
        {
            DateTime LastUpdateCheck = DateTime.Now;

            //new Thread(() => new Form1().ShowDialog()).Start();

            var Version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();

            Console.Title = $"7 Days to die - Horde & Airdrop Viewer - {Version}";

            bool debug = false;
            foreach (string arg in args)
            {
                if (arg.ToLower() == "/Debug".ToLower())
                    debug = true;
            }

            _7D2D_ServerInfo i = new _7D2D_ServerInfo(debug);
            IGUI GUI = new GUI_Console(i);

            do
            {
                if (i.Refresh())
                    GUI.Draw();
                else
                    GUI.DrawConnectionError();

                System.Threading.Thread.Sleep((int)(60f / 24f * 1000f));

                if (GUI.UpdateAvailable == false && (DateTime.Now - LastUpdateCheck).Minutes >= SearchForUpdateDelayInMin) 
                {
                    if (CheckForUpdate()) GUI.UpdateAvailable = true;
                    LastUpdateCheck = DateTime.Now;
                }
            } while (true);
        }

        private static bool CheckForUpdate()
        {
            UpdateCheckInfo info = null;

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;

                try
                {
                    info = ad.CheckForDetailedUpdate();

                }
                catch (DeploymentDownloadException dde)
                {
                    //MessageBox.Show("The new version of the application cannot be downloaded at this time. \n\nPlease check your network connection, or try again later. Error: " + dde.Message);
                    return false;
                }
                catch (InvalidDeploymentException ide)
                {
                    //MessageBox.Show("Cannot check for a new version of the application. The ClickOnce deployment is corrupt. Please redeploy the application and try again. Error: " + ide.Message);
                    return false;
                }
                catch (InvalidOperationException ioe)
                {
                    //MessageBox.Show("This application cannot be updated. It is likely not a ClickOnce application. Error: " + ioe.Message);
                    return false;
                }

                if (info.UpdateAvailable)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
