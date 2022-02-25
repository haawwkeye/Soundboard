using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Soundboard
{
    public class AutoUpdate
    {
        public static bool shouldUpdate = true;
        public static string version = "0.0.1";

        public static bool CheckForUpdate()
        {
            bool hasUpdate = false;

            //TODO: Check for update

            return hasUpdate;
        }

        public static void Update()
        {
            //TODO: Add update stuff
            Application.Exit();
        }
    }
}
