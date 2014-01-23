﻿using PingService;
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
        }

        private void PingTesterChart_Load(object sender, EventArgs e)
        {
            // For each (meaningful) result, add it to the chart
            foreach (PingResult res in results.Where(x => x.getAvg() != null && x.getAvgTime() != null && x.getDev() != null))
            {
                crtResults.Series["Average"].Points.AddXY(((DateTime)res.getAvgTime()).ToShortTimeString(), res.getAvg());
                crtResults.Series["StandardDeviation"].Points.AddXY(((DateTime)res.getAvgTime()).ToShortTimeString(), res.getDev());
            }
        }

        private void PingTesterChart_Resize(object sender, EventArgs e)
        {
            crtResults.Size = this.ClientSize;
        }
    }
}
