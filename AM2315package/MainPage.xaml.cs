using AM2315package.AM2315;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Timers;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AM2315package
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private AM2315sensor AM2315;
        private float[] values;
        public MainPage()
        {
            this.InitializeComponent();
            values = new float[2];
            AM2315 = new AM2315sensor(0x5C);
            Configure();
            Timer t1 = new Timer(1000);
            t1.Elapsed += async (sender, e) => await ReadSensor();
            t1.Start();
        }

        private async Task ReadSensor()
        {
            values = await AM2315.ReadData();
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,()=>
            {
                txt_Humidity.Text = AM2315.humidity.ToString();
                txt_Temperature.Text = AM2315.temperature.ToString();
                txt_Message.Text = AM2315.message;
            });

        }

        private async System.Threading.Tasks.Task Configure()
        {
            int ret = await AM2315.Connect();
        }

    }
}
