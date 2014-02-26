namespace PingTester
{
    partial class PingTesterAnalyzer
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.crtResults = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.cmbDays = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.crtResults)).BeginInit();
            this.SuspendLayout();
            // 
            // crtResults
            // 
            this.crtResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea2.AxisX.Interval = 1D;
            chartArea2.AxisX2.Interval = 1D;
            chartArea2.CursorX.AutoScroll = false;
            chartArea2.CursorX.IsUserEnabled = true;
            chartArea2.CursorX.IsUserSelectionEnabled = true;
            chartArea2.Name = "AvgDevChartArea";
            this.crtResults.ChartAreas.Add(chartArea2);
            legend2.Alignment = System.Drawing.StringAlignment.Far;
            legend2.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend2.Name = "Legend1";
            this.crtResults.Legends.Add(legend2);
            this.crtResults.Location = new System.Drawing.Point(-1, 0);
            this.crtResults.MinimumSize = new System.Drawing.Size(640, 480);
            this.crtResults.Name = "crtResults";
            this.crtResults.Padding = new System.Windows.Forms.Padding(5);
            series3.ChartArea = "AvgDevChartArea";
            series3.IsXValueIndexed = true;
            series3.Legend = "Legend1";
            series3.Name = "Average";
            series4.ChartArea = "AvgDevChartArea";
            series4.IsXValueIndexed = true;
            series4.Legend = "Legend1";
            series4.Name = "StandardDeviation";
            this.crtResults.Series.Add(series3);
            this.crtResults.Series.Add(series4);
            this.crtResults.Size = new System.Drawing.Size(900, 484);
            this.crtResults.TabIndex = 1;
            this.crtResults.Text = "Results";
            title2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            title2.Name = "Title";
            title2.Text = "Title Placeholder";
            this.crtResults.Titles.Add(title2);
            // 
            // cmbDays
            // 
            this.cmbDays.FormattingEnabled = true;
            this.cmbDays.Location = new System.Drawing.Point(731, 12);
            this.cmbDays.Name = "cmbDays";
            this.cmbDays.Size = new System.Drawing.Size(155, 21);
            this.cmbDays.TabIndex = 2;
            this.cmbDays.SelectedIndexChanged += new System.EventHandler(this.cmbDays_SelectedIndexChanged);
            // 
            // PingTesterAnalyzer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(898, 484);
            this.Controls.Add(this.cmbDays);
            this.Controls.Add(this.crtResults);
            this.Name = "PingTesterAnalyzer";
            this.Text = "PingTesterAnalyzer";
            this.Load += new System.EventHandler(this.PingTesterAnalyzer_Load);
            this.Resize += new System.EventHandler(this.PingTesterAnalyzer_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.crtResults)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart crtResults;
        private System.Windows.Forms.ComboBox cmbDays;


    }
}