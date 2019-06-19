using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GEOAlgorithmGenerator
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            fromTb.Text = "-4";
            toTb.Text = "12";
            precisionCb.Items.AddRange(new string[] {"0,001", "0,01", "0,1"});
            precisionCb.SelectedIndex = 0;
            iterationsTb.Text = "750";
            tauTb.Text = "1,2";
            var ser = new System.Windows.Forms.DataVisualization.Charting.Series();
            ser.Name = "Best";
            ser.ChartType = SeriesChartType.Line;
            chart.Series.Add(ser);
        }

        private async void StartButton_Click(object sender, EventArgs e)
        {
            statusLabel.Text = "Calculating...";
            startButton.Enabled = false;
            Algorithm algorithm = new Algorithm(precisionCb.SelectedItem.ToString(), fromTb.Text, toTb.Text, tauTb.Text);
            Results results = new Results();
            await Task.Run(() =>
            {
                results = algorithm.Start(int.Parse(iterationsTb.Text));
            });
            xrTb.Text = results.XRealBest.ToString();
            xbTb.Text = results.XBinBest;
            fxTb.Text = results.FXBest.ToString();
            iterationTb.Text = results.Iteration.ToString();
            foreach (var sss in chart.Series)
            {
                sss.Points.Clear();
            }
            chart.Series[0].ChartType = SeriesChartType.Line;
            chart.Series[0].Name = "Results";
            for (int i = 0; i < results.FXs.Count; i++)
            {
                chart.Series["Results"].Points.AddXY(i + 1, results.FXs[i]);
                chart.Series["Best"].Points.AddXY(i + 1, results.Bests[i]);
            }
            statusLabel.Text = "";
            startButton.Enabled = true;
        }
    }
}
