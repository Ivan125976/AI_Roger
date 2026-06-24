using Velopack;
using Velopack.Sources;
using Yocto_Roger.UI.CUI;
using static Yocto_Roger.UI.CUI.CUI;

namespace Yocto_Roger.UI.Interfaces
{
    /// <summary>
    /// Class of Update Manager
    /// </summary>
    public class UpdateManagerInterface : IUserInterface
    {
        /// <summary>
        /// Menu of update manager + logic and exceptions. Can writing some text in console, be careful when using it in your project
        /// </summary>
        public void StartInterface()
        {
            VelopackApp.Build().Run();

            GithubSource githubSource = new("https://github.com/Ivan125976/RogerHubEngine", null, false);
            var mgr = new UpdateManager(githubSource);

            if (mgr.IsInstalled)
            {
                try
                {
                    UpdateInfo? info = mgr.CheckForUpdates();

                    if (info != null)
                    {
                        Console.WriteLine($" Update found!. Do you want to install it? \n(y/n) >>>");
                        switch (Console.ReadKey(true).KeyChar)
                        {
                            case 'Y' or 'y':
                                {
                                    try
                                    {
                                        mgr.DownloadUpdates(info);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Failed to download update: {ex}");
                                        Console.Write("Press any key to continue");
                                        Console.ReadKey(true);
                                        break;
                                    }
                                    Send("Updates was downloaded successful!\nTrying to apply it, the app will be restarted in new version...");
                                    Thread.Sleep(3000);
                                    try
                                    {
                                        mgr.ApplyUpdatesAndRestart(info);
                                    }
                                    catch (Exception ex)
                                    {
                                        Send($"Failed to apply updates, here's my error: {ex}", MessageType.error);
                                        Console.Write("Press any key to continue");
                                        Console.ReadKey(true);
                                    }
                                    break;
                                }
                        }
                    }
                    else
                    {
                        Send("Hey, you have the latest version");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey(true);
                    }
                }
                catch (Exception ex)
                {
                    Send($"{ex}", MessageType.error);
                }
            }
            else
                Send("You can't update in development mode", MessageType.error);
            /*
             Development mode is when you compile a program using Visual Studio, which is a simple compiler,
             but updating only works when you compile it using VPK, which is a command-line tool that creates a self-updating file from a simple binary file.
             */
        }
    }
}


