using Playnite.Controls;
using Playnite.FullscreenApp.Controls;
using Playnite.SDK;
using Playnite.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using CoreAudio;
using System.Collections;
using Playnite.Behaviors;
using System.IO;

namespace Playnite.FullscreenApp.Windows
{

    public class AudioSwapWindowFactory : WindowFactory
    {
        public override WindowBase CreateNewWindowInstance()
        {
            return new AudioSwapWindow();
        }
    }
    /// <summary>
    /// Interaction logic for AudioSwapWindow.xaml
    /// </summary>
    public partial class AudioSwapWindow : WindowBase
    {
        MMDeviceEnumerator audioDeviceEnumerator;
        MMDeviceCollection audioDevices;

        List<ButtonEx> buttons = new List<ButtonEx>();

        public AudioSwapWindow()
        {
            InitializeComponent();
            WindowTools.ConfigureChildWindow(this);

            audioDeviceEnumerator = new MMDeviceEnumerator();
            audioDevices = audioDeviceEnumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);

            foreach (MMDevice device in audioDevices)
            {
                ButtonEx button = new ButtonEx();
                button.Command = new RelayCommand(() => setDefaultAudioDevice(device));
                button.Content = new TextBlock() { Text = device.DeviceFriendlyName, Foreground = new SolidColorBrush(Colors.White), FontSize = 30, Padding = new Thickness(10, 5, 10, 5) };
                button.Margin = new Thickness(20, 10, 20, 10);
                FocusBahaviors f = new FocusBahaviors();
                button.Tag = device.ID;

                buttons.Add(button);
                PART_StackPanel.Children.Add(button);
            }

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            string currentDevice = audioDeviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia).ID;
            foreach(var b in buttons)
            {
                if((string)b.Tag == currentDevice)
                {
                    b.Focus();
                    updateButtonStyles();
                }
            }
        }

        private void updateButtonStyles()
        {
            MMDevice device = audioDeviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            foreach (var button in buttons)
            {
                if ((string)button.Tag == device.ID)
                {
                    var panel = new StackPanel() { Orientation = Orientation.Horizontal };
                    var image = new Image() { Source = new BitmapImage(new Uri($"file://{Directory.GetCurrentDirectory()}/Themes/Fullscreen/Default/Images/checkmark_small_circle.png")), MaxHeight = 25, MaxWidth = 25 };
                    var text = new TextBlock() { Text = device.DeviceFriendlyName, Foreground = new SolidColorBrush(Colors.White), FontSize = 30, Padding = new Thickness(10, 5, 10, 5) };

                    RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.HighQuality);

                    panel.Children.Add(image);
                    panel.Children.Add(text);

                    button.Content = panel;
                }
                else
                {
                    string buttonText = audioDeviceEnumerator.GetDevice((string)button.Tag).DeviceFriendlyName;
                    var panel = new StackPanel() { Orientation = Orientation.Horizontal };

                    panel.Children.Add(new Canvas() { MinHeight = 25, MinWidth = 25 });
                    panel.Children.Add(new TextBlock() { Text = buttonText, Foreground = new SolidColorBrush(Colors.White), FontSize = 30, Padding = new Thickness(10, 5, 10, 5) });

                    button.Content = panel;
                }
            }
        }

        private void setDefaultAudioDevice(MMDevice device)
        {
            audioDeviceEnumerator.SetDefaultAudioEndpoint(device);

            updateButtonStyles();
        }
    }
}
