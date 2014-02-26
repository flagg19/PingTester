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
using System.Globalization;

namespace PingTester
{
    public partial class PingTesterAnalyzer : Form
    {
        // The collection of aggregated results grouped by day
        private Dictionary<DayOfWeek, List<AggregatedResult>> results;

        public PingTesterAnalyzer(Dictionary<DayOfWeek, List<AggregatedResult>> results)
        {
            InitializeComponent();
            this.results = results;
        }

        private void PingTesterAnalyzer_Load(object sender, EventArgs e)
        {
            // Create combobox dictionary data source
            Dictionary<string, DayOfWeek> ds = new Dictionary<string, DayOfWeek>();
            foreach (DayOfWeek day in results.Keys)
            {
                ds.Add(CultureInfo.CurrentCulture.DateTimeFormat.DayNames[(int)day], day);
            }
            // Bind combobox to dictionary
            cmbDays.DataSource = new BindingSource(ds, null);
            cmbDays.DisplayMember = "Key";
            cmbDays.ValueMember = "Value";
        }

        private void PingTesterAnalyzer_Resize(object sender, EventArgs e)
        {
            // Resizing the chart to match the new size of the form
            crtResults.Size = this.ClientSize;
        }

        private void cmbDays_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Call the func. to refresh the chart
            ChartData(((KeyValuePair<string, DayOfWeek>)cmbDays.SelectedItem).Value);
        }
        
        private void ChartData(DayOfWeek day)
        {
            // Clear the chart
            crtResults.Series["Average"].Points.Clear();
            crtResults.Series["StandardDeviation"].Points.Clear();

            int count = 0;
            // Add each result to the chart
            foreach (AggregatedResult res in results[day])
            {
                count += res.GetDayCount();
                string tmpXValue = res.Hour.ToString();

                crtResults.Series["Average"].Points.AddXY(tmpXValue, res.GetAvg());
                crtResults.Series["StandardDeviation"].Points.AddXY(tmpXValue, res.GetDev());
            }

            // Setting title
            crtResults.Titles["Title"].Text = Strings.AnalyzerChartTitle + " " + (double)count / (double)results[day].Count;
        }
    }
}
