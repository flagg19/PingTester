namespace PingTester
{
    partial class PingTester
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Liberare le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PingTester));
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnChart = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.grpInputs = new System.Windows.Forms.GroupBox();
            this.lblMaxNetworkUsage = new System.Windows.Forms.Label();
            this.txtMaxNetworkUsage = new System.Windows.Forms.TextBox();
            this.lblSecondsBetweenTests = new System.Windows.Forms.Label();
            this.txtSecondsBetweenTests = new System.Windows.Forms.TextBox();
            this.lblSecondsBetweenPings = new System.Windows.Forms.Label();
            this.txtSecondsBetweenPings = new System.Windows.Forms.TextBox();
            this.lblPingsPerTest = new System.Windows.Forms.Label();
            this.txtPingPerTest = new System.Windows.Forms.TextBox();
            this.lblTimeout = new System.Windows.Forms.Label();
            this.txtTimeout = new System.Windows.Forms.TextBox();
            this.lblAddress = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.ntfPingTesterTray = new System.Windows.Forms.NotifyIcon(this.components);
            this.btnImport = new System.Windows.Forms.Button();
            this.erpInputs = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnAnalyze = new System.Windows.Forms.Button();
            this.grpInputs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.erpInputs)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(268, 28);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(68, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(268, 54);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(68, 23);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnChart
            // 
            this.btnChart.Location = new System.Drawing.Point(268, 80);
            this.btnChart.Name = "btnChart";
            this.btnChart.Size = new System.Drawing.Size(68, 23);
            this.btnChart.TabIndex = 2;
            this.btnChart.Text = "Chart";
            this.btnChart.UseVisualStyleBackColor = true;
            this.btnChart.Click += new System.EventHandler(this.btnChart_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(268, 132);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(68, 23);
            this.btnExport.TabIndex = 3;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // grpInputs
            // 
            this.grpInputs.Controls.Add(this.lblMaxNetworkUsage);
            this.grpInputs.Controls.Add(this.txtMaxNetworkUsage);
            this.grpInputs.Controls.Add(this.lblSecondsBetweenTests);
            this.grpInputs.Controls.Add(this.txtSecondsBetweenTests);
            this.grpInputs.Controls.Add(this.lblSecondsBetweenPings);
            this.grpInputs.Controls.Add(this.txtSecondsBetweenPings);
            this.grpInputs.Controls.Add(this.lblPingsPerTest);
            this.grpInputs.Controls.Add(this.txtPingPerTest);
            this.grpInputs.Controls.Add(this.lblTimeout);
            this.grpInputs.Controls.Add(this.txtTimeout);
            this.grpInputs.Controls.Add(this.lblAddress);
            this.grpInputs.Controls.Add(this.txtAddress);
            this.grpInputs.Location = new System.Drawing.Point(12, 12);
            this.grpInputs.Name = "grpInputs";
            this.grpInputs.Size = new System.Drawing.Size(250, 180);
            this.grpInputs.TabIndex = 4;
            this.grpInputs.TabStop = false;
            this.grpInputs.Text = "Inputs";
            // 
            // lblMaxNetworkUsage
            // 
            this.lblMaxNetworkUsage.AutoSize = true;
            this.lblMaxNetworkUsage.Location = new System.Drawing.Point(23, 151);
            this.lblMaxNetworkUsage.Name = "lblMaxNetworkUsage";
            this.lblMaxNetworkUsage.Size = new System.Drawing.Size(106, 13);
            this.lblMaxNetworkUsage.TabIndex = 11;
            this.lblMaxNetworkUsage.Text = "MaxNetworkUsage%";
            // 
            // txtMaxNetworkUsage
            // 
            this.txtMaxNetworkUsage.Location = new System.Drawing.Point(135, 148);
            this.txtMaxNetworkUsage.Name = "txtMaxNetworkUsage";
            this.txtMaxNetworkUsage.Size = new System.Drawing.Size(92, 20);
            this.txtMaxNetworkUsage.TabIndex = 10;
            this.txtMaxNetworkUsage.Text = "0,5";
            // 
            // lblSecondsBetweenTests
            // 
            this.lblSecondsBetweenTests.AutoSize = true;
            this.lblSecondsBetweenTests.Location = new System.Drawing.Point(12, 125);
            this.lblSecondsBetweenTests.Name = "lblSecondsBetweenTests";
            this.lblSecondsBetweenTests.Size = new System.Drawing.Size(117, 13);
            this.lblSecondsBetweenTests.TabIndex = 9;
            this.lblSecondsBetweenTests.Text = "SecondsBetweenTests";
            // 
            // txtSecondsBetweenTests
            // 
            this.txtSecondsBetweenTests.Location = new System.Drawing.Point(135, 122);
            this.txtSecondsBetweenTests.Name = "txtSecondsBetweenTests";
            this.txtSecondsBetweenTests.Size = new System.Drawing.Size(92, 20);
            this.txtSecondsBetweenTests.TabIndex = 8;
            this.txtSecondsBetweenTests.Text = "600";
            // 
            // lblSecondsBetweenPings
            // 
            this.lblSecondsBetweenPings.AutoSize = true;
            this.lblSecondsBetweenPings.Location = new System.Drawing.Point(12, 99);
            this.lblSecondsBetweenPings.Name = "lblSecondsBetweenPings";
            this.lblSecondsBetweenPings.Size = new System.Drawing.Size(117, 13);
            this.lblSecondsBetweenPings.TabIndex = 7;
            this.lblSecondsBetweenPings.Text = "SecondsBetweenPings";
            // 
            // txtSecondsBetweenPings
            // 
            this.txtSecondsBetweenPings.Location = new System.Drawing.Point(135, 96);
            this.txtSecondsBetweenPings.Name = "txtSecondsBetweenPings";
            this.txtSecondsBetweenPings.Size = new System.Drawing.Size(92, 20);
            this.txtSecondsBetweenPings.TabIndex = 6;
            this.txtSecondsBetweenPings.Text = "1";
            // 
            // lblPingsPerTest
            // 
            this.lblPingsPerTest.AutoSize = true;
            this.lblPingsPerTest.Location = new System.Drawing.Point(59, 73);
            this.lblPingsPerTest.Name = "lblPingsPerTest";
            this.lblPingsPerTest.Size = new System.Drawing.Size(70, 13);
            this.lblPingsPerTest.TabIndex = 5;
            this.lblPingsPerTest.Text = "PingsPerTest";
            // 
            // txtPingPerTest
            // 
            this.txtPingPerTest.Location = new System.Drawing.Point(135, 70);
            this.txtPingPerTest.Name = "txtPingPerTest";
            this.txtPingPerTest.Size = new System.Drawing.Size(92, 20);
            this.txtPingPerTest.TabIndex = 4;
            this.txtPingPerTest.Text = "20";
            // 
            // lblTimeout
            // 
            this.lblTimeout.AutoSize = true;
            this.lblTimeout.Location = new System.Drawing.Point(84, 47);
            this.lblTimeout.Name = "lblTimeout";
            this.lblTimeout.Size = new System.Drawing.Size(45, 13);
            this.lblTimeout.TabIndex = 3;
            this.lblTimeout.Text = "Timeout";
            // 
            // txtTimeout
            // 
            this.txtTimeout.Location = new System.Drawing.Point(135, 44);
            this.txtTimeout.Name = "txtTimeout";
            this.txtTimeout.Size = new System.Drawing.Size(92, 20);
            this.txtTimeout.TabIndex = 2;
            this.txtTimeout.Text = "5000";
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new System.Drawing.Point(84, 21);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(45, 13);
            this.lblAddress.TabIndex = 1;
            this.lblAddress.Text = "Address";
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(135, 18);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(92, 20);
            this.txtAddress.TabIndex = 0;
            this.txtAddress.Text = "192.168.100.1";
            // 
            // ntfPingTesterTray
            // 
            this.ntfPingTesterTray.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.ntfPingTesterTray.BalloonTipText = "PingTester is still runnig in background.";
            this.ntfPingTesterTray.BalloonTipTitle = "Look here please!";
            this.ntfPingTesterTray.Icon = ((System.Drawing.Icon)(resources.GetObject("ntfPingTesterTray.Icon")));
            this.ntfPingTesterTray.Text = "PingTesterTray";
            this.ntfPingTesterTray.Visible = true;
            this.ntfPingTesterTray.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ntfPingTesterTray_MouseDoubleClick);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(268, 106);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(68, 23);
            this.btnImport.TabIndex = 5;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // erpInputs
            // 
            this.erpInputs.ContainerControl = this;
            // 
            // btnAnalyze
            // 
            this.btnAnalyze.Location = new System.Drawing.Point(268, 158);
            this.btnAnalyze.Name = "btnAnalyze";
            this.btnAnalyze.Size = new System.Drawing.Size(68, 23);
            this.btnAnalyze.TabIndex = 6;
            this.btnAnalyze.Text = "Analyze";
            this.btnAnalyze.UseVisualStyleBackColor = true;
            this.btnAnalyze.Click += new System.EventHandler(this.btnAnalyze_Click);
            // 
            // PingTester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(348, 203);
            this.Controls.Add(this.btnAnalyze);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.grpInputs);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnChart);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "PingTester";
            this.Text = "PingTester";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PingTester_FormClosing);
            this.Resize += new System.EventHandler(this.PingTester_Resize);
            this.grpInputs.ResumeLayout(false);
            this.grpInputs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.erpInputs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnChart;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.GroupBox grpInputs;
        private System.Windows.Forms.Label lblMaxNetworkUsage;
        private System.Windows.Forms.TextBox txtMaxNetworkUsage;
        private System.Windows.Forms.Label lblSecondsBetweenTests;
        private System.Windows.Forms.TextBox txtSecondsBetweenTests;
        private System.Windows.Forms.Label lblSecondsBetweenPings;
        private System.Windows.Forms.TextBox txtSecondsBetweenPings;
        private System.Windows.Forms.Label lblPingsPerTest;
        private System.Windows.Forms.TextBox txtPingPerTest;
        private System.Windows.Forms.Label lblTimeout;
        private System.Windows.Forms.TextBox txtTimeout;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.NotifyIcon ntfPingTesterTray;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.ErrorProvider erpInputs;
        private System.Windows.Forms.Button btnAnalyze;
    }
}

