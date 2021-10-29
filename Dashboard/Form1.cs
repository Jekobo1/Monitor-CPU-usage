using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Diagnostics;
using System.Threading;

namespace Dashboard
{
    
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
            

        }
       
        ManagementObjectSearcher searcher = new ManagementObjectSearcher();
        private static string GetBattery()
        {
            PowerStatus p = SystemInformation.PowerStatus;
            int a = (int)(p.BatteryLifePercent * 100);
            return a.ToString();
            
        }
        private static string GetBatteryStatus()
        {
            PowerStatus p = SystemInformation.PowerStatus;
            int a = (int)(p.BatteryChargeStatus);
            if (a > 0)
                return "Charging!";
            else
                return "Not charging!";
            

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            float fcpu = CPU.NextValue();
            float dram = RAM.NextValue();
            double temp = TEMP.NextValue();
            circularProgressBar1.Value = (int)fcpu;
            circularProgressBar1.Text = string.Format("{0:0.00}%", fcpu);

            circularProgressBar2.Value = (int)dram;
            circularProgressBar2.Text = string.Format("{0:0.00}%", dram);

            label3.Text = "Temperature: " + (temp - 273.15).ToString();
            label5.Text = (GetTemp() - 273.15).ToString();
            
           
            

        }

        protected static string GetIP()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if(ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
                
            }
            throw new Exception("No network adapters");


        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void iPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(GetIP(), "Your IP Address");
        }

        private void batteryChargeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(GetBattery() + " %", "Your Battery charge");
        }

        private void batteryStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(GetBatteryStatus(), "Your Battery status");
        }
        private static float GetTemp()
        {
            PerformanceCounterCategory performanceCounterCategory = new PerformanceCounterCategory("Thermal Zone Information");
            var instances = performanceCounterCategory.GetInstanceNames();
            List<PerformanceCounter> temperatureCounters = new List<PerformanceCounter>();
            foreach (string instanceName in instances)
            {

                foreach (PerformanceCounter counter in performanceCounterCategory.GetCounters(instanceName))
                {
                    if (counter.CounterName == "Temperature")
                    {
                        temperatureCounters.Add(counter);
                    }
                }
            }


            while (true)
            {
                foreach (PerformanceCounter counter in temperatureCounters)
                {
                    //Console.WriteLine("{0} {1} {2} {3}", counter.CategoryName, counter.CounterName, counter.InstanceName, counter.NextValue());
                    return counter.NextValue();
                }
                
            }
        }

        
    }
}
