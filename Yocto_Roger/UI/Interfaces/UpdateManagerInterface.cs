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

            GithubSource githubSource = new("https://github.com/Ivan125976/AI_Roger", null, false);
            var mgr = new Velopack.UpdateManager(githubSource);

            if (mgr.IsInstalled)
            {

                Console.WriteLine(
                    $"""
                                1. Check for updates and update
                                2. Get outta here to main menu
                                """);
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            UpdateInfo info = mgr.CheckForUpdates();

                            if (info != null)
                            {
                                Console.WriteLine("Updates found! Downloading...");
                                try
                                {
                                    mgr.DownloadUpdates(info);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Failed to download the update: {ex}");
                                    break;
                                }
                                Console.WriteLine("Updates was downloaded successful!\nTrying to apply it, the app will be restarted in new version...");
                                Thread.Sleep(5000); // For user can to read the message
                                try
                                {
                                    mgr.ApplyUpdatesAndRestart(info);
                                }
                                catch (Exception ex)
                                {
                                    Send("Failed to apply updates, here's my error: ", MessageType.error);
                                    Console.WriteLine(ex.ToString(), ConsoleColor.Red);
                                }
                            }
                            else
                            {
                                Send("Hey, hey, calm down, you have the latest version");
                                Thread.Sleep(5000);
                            }
                            break;

                        case 2:
                            break;

                    }
                }
            }
            else
                Send("It's Doesn't work in the developer mode. I guess you run it in visual studio, with simple compiler, but this future works only when it compiled with VPK, so forget about it, you don't need it. If you are an simple user, and u see this message, please write about it in Issues on our Github: https://github.com/Ivan125976/AI_Roger/issues/new/choose", MessageType.error);
        }
    }
}


