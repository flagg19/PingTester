using PingService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PingTester
{
    public partial class PingTesterChart : Form
    {
        private List<PingResult> results;

        public PingTesterChart(List<PingResult> results)
        {
            InitializeComponent();
            this.results = results;
            crtResults.Titles["Title"].Text =
                ((DateTime)results.First().getAvgTime()).ToShortDateString() + " " +
                ((DateTime)results.First().getAvgTime()).ToShortTimeString() + " <---> " +
                ((DateTime)results.Last().getAvgTime()).ToShortDateString() + " " +
                ((DateTime)results.Last().getAvgTime()).ToShortTimeString();
        }

        private void PingTesterChart_Load(object sender, EventArgs e)
        {
            // Add each result to the chart
            foreach (PingResult res in results)
            {
                string tmpXValue = ((DateTime)res.getAvgTime()).ToShortTimeString();
                double? tmpYAvgValue = res.getAvg();
                double? tmpYDevValue = res.getDev();

                /* 
                 * If avg is null, it means that not even one of the pings of the corrent result has been completed successfully.
                 * If so, we consider the whole test failed and we mark it with a "*", just filtering them away it's bad because
                 * the fact that thay failed is meaningful. It's of no use checking for dev too 'cause avg = null --> dev = null. 
                 */
                if (tmpYAvgValue == null)
                {
                    tmpXValue += "*";
                    tmpYAvgValue = 0;
                    tmpYDevValue = 0;
                }

                crtResults.Series["Average"].Points.AddXY(tmpXValue, tmpYAvgValue);
                crtResults.Series["StandardDeviation"].Points.AddXY(tmpXValue, tmpYDevValue);
            }
        }

        private void PingTesterChart_Resize(object sender, EventArgs e)
        {
            // Resizing the chart to match the new size of the form
            crtResults.Size = this.ClientSize;
        }
    }
}
