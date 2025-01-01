using Playnite.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace Playnite.FullscreenApp.Windows
{
    public class WindowTools
    {
        public static void ConfigureChildWindow(WindowBase window)
        {
            var model = FullscreenApplication.Current?.MainModel;
            if (model != null)
            {
                window.Width = model.WindowWidth;
                window.Height = model.WindowHeight;
                if (window.FindName("GridMain") is Grid mainGrid)
                {
                    mainGrid.Width = model.ViewportWidth;
                }
            }
        }

        public static void MoveCursorToEdge()
        {
            Cursor.Position = new System.Drawing.Point((int)Screen.PrimaryScreen.Bounds.Width, 
                0);
        }
    }
}
