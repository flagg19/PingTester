namespace PingTester
{
    partial class PingTesterChart
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.crtResults = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.crtResults)).BeginInit();
            this.SuspendLayout();
            // 
            // crtResults
            // 
            this.crtResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.CursorX.AutoScroll = false;
            chartArea1.CursorX.IsUserEnabled = true;
            chartArea1.CursorX.IsUserSelectionEnabled = true;
            chartArea1.Name = "AvgDevChartArea";
            this.crtResults.ChartAreas.Add(chartArea1);
            legend1.Alignment = System.Drawing.StringAlignment.Far;
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend1.Name = "Legend1";
            this.crtResults.Legends.Add(legend1);
            this.crtResults.Location = new System.Drawing.Point(0, 0);
            this.crtResults.MinimumSize = new System.Drawing.Size(640, 480);
            this.crtResults.Name = "crtResults";
            this.crtResults.Padding = new System.Windows.Forms.Padding(5);
            series1.ChartArea = "AvgDevChartArea";
            series1.Legend = "Legend1";
            series1.Name = "Average";
            series2.ChartArea = "AvgDevChartArea";
            series2.Legend = "Legend1";
            series2.Name = "StandardDeviation";
            this.crtResults.Series.Add(series1);
            this.crtResults.Series.Add(series2);
            this.crtResults.Size = new System.Drawing.Size(900, 484);
            this.crtResults.TabIndex = 0;
            this.crtResults.Text = "Results";
            // 
            // PingTesterChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(898, 484);
            this.Controls.Add(this.crtResults);
            this.Name = "PingTesterChart";
            this.Text = "PingTesterChart";
            this.Load += new System.EventHandler(this.PingTesterChart_Load);
            this.Resize += new System.EventHandler(this.PingTesterChart_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.crtResults)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart crtResults;
    }
}