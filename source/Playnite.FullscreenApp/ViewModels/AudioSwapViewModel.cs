using Playnite.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Playnite.SDK;

namespace Playnite.FullscreenApp.ViewModels
{
    internal class AudioSwapViewModel
    {
        private static readonly ILogger logger = LogManager.GetLogger();
        private readonly IWindowFactory window;

        public RelayCommand CloseCommand => new RelayCommand(() => Close());

        public AudioSwapViewModel(IWindowFactory window)
        {
            this.window = window;
        }

        public bool? OpenView()
        {
            return window.CreateAndOpenDialog(this);
        }

        public void Close()
        {
            window.Close();
        }
    }
}
