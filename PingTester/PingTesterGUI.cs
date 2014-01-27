using PingService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace PingTester
{
    // All possible execution states, corresponding to a precise GUI state 
    enum PingTesterGUIStatus
    {
        AfterLaunch,
        AfterStart,
        AfterStop
    };

    public partial class PingTester : Form
    {
        // PingHelper related params
        IPAddress remoteAddr;
        int timeout;
        int pingsPerTest;
        double secondsBetweenPings;
        double secondsBetweenTests;
        double maxNetworkInterfaceUsagePercentage;

        // Form and ping results related vars
        PingHelper ph;
        List<PingResult> results;
        object resultsLock;
        PingTesterGUIStatus status;

        // Timer related vars
        object timerLock;
        ManualResetEvent timerDead;
        System.Timers.Timer timer;

        public PingTester()
        {
            InitializeComponent();

            resultsLock = new object();
            timerLock = new object();
            timerDead = new ManualResetEvent(false);

            AdjustGUIToStatus(PingTesterGUIStatus.AfterLaunch);
        }

        // Takes care of the GUI, given the execution state
        void AdjustGUIToStatus(PingTesterGUIStatus status)
        {
            this.status = status;
            switch (status)
            {
                case PingTesterGUIStatus.AfterLaunch:
                    grpInputs.Enabled = true;
                    btnStop.Enabled = false;
                    btnStart.Enabled = true;
                    btnChart.Enabled = true;
                    btnImport.Enabled = true;
                    btnExport.Enabled = true;
                    this.Text = Strings.LifeCycleFormNotRunning;
                    break;
                case PingTesterGUIStatus.AfterStart: // Aka: running
                    grpInputs.Enabled = false;
                    btnStop.Enabled = true;
                    btnStart.Enabled = false;
                    btnChart.Enabled = true;
                    btnImport.Enabled = false;
                    btnExport.Enabled = false;
                    this.Text = Strings.LifeCycleFormRunning;
                    break;
                case PingTesterGUIStatus.AfterStop: // Aka: stopping
                    grpInputs.Enabled = false;
                    btnStop.Enabled = false;
                    btnStart.Enabled = false;
                    btnChart.Enabled = false;
                    btnImport.Enabled = false;
                    btnExport.Enabled = false;
                    this.Text = Strings.LifeCycleFormStopping;
                    break;
            }
        }

        #region Timer related code

        /* StopTimer() & timer_Elapsed(..) weird things are needed to be absolutly sure
         * that after the call to StopTimer() returns, the timer is stopped  and
         * its event it's not executing even if its event was executing or was sheduled
         * to be executed soon.
         * */

        private void StopTimer()
        {
            lock (timerLock)
            {
                /* Set the state of the wait handle so if the timer's delegate is executing
                 * (but waiting at the lock) causing the following Stop to have no effect,
                 * at unlock it will return without doing anything.
                 * */
                timerDead.Set();
                timer.Stop();
            }
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (timerLock)
            {
                // "If millisecondsTimeout is zero, the method does not block. It tests the state of the wait handle and returns immediately."
                if (timerDead.WaitOne(0))
                {
                    // If it's set, just return
                    return;
                }
                else
                {
                    // If not do what you need to do
                    PingResult tmp = ph.TestPing();
                    lock (resultsLock)
                    {
                        // Devo proteggere l'accesso alla lista per evitare che si tenti di creare la copia da graficare mentre questa viene modificata
                        results.Add(tmp);
                    }
                    timer.Start();
                }
            }
        }

        #endregion

        private void btnStart_Click(object sender, EventArgs e)
        {
            // Parsing parameters
            string strTimeout = Strings.FormValidationTimeoutError.Replace("_min_", FormInputValidator.timeoutRangeMin.ToString())
                .Replace("_max_", FormInputValidator.timeoutRangeMax.ToString());
            string strPingsPerTest = Strings.FormValidationTimeoutError.Replace("_min_", FormInputValidator.pingsPerTestRangeMin.ToString())
                .Replace("_max_", FormInputValidator.pingsPerTestRangeMax.ToString());
            string strSecondsBetweenPings = Strings.FormValidationTimeoutError.Replace("_min_", FormInputValidator.secondsBetweenPingsRangeMin.ToString())
                .Replace("_max_", FormInputValidator.secondsBetweenPingsRangeMax.ToString());
            string strSecondsBetweenTests = Strings.FormValidationTimeoutError.Replace("_min_", FormInputValidator.secondsBetweenTestsRangeMin.ToString())
                .Replace("_max_", FormInputValidator.secondsBetweenTestsRangeMax.ToString());
            string strMaxNetworkInterfaceUsage = Strings.FormValidationTimeoutError.Replace("_min_", FormInputValidator.maxNetworkInterfaceUsageRangeMin.ToString())
                .Replace("_max_", FormInputValidator.maxNetworkInterfaceUsageRangeMax.ToString());

            bool validationCheck = true;
            validationCheck = validationCheck
                & FormInputValidator.validateHost(txtAddress, Strings.FormValidationAddressError, erpInputs, out remoteAddr)
                & FormInputValidator.validateTimeout(txtTimeout, strTimeout, erpInputs, out timeout)
                & FormInputValidator.validatePingsPerTest(txtPingPerTest, strPingsPerTest, erpInputs, out pingsPerTest)
                & FormInputValidator.validateSecondsBetweenPings(txtSecondsBetweenPings, strSecondsBetweenPings, erpInputs, out secondsBetweenPings)
                & FormInputValidator.validateSecondsBetweenTests(txtSecondsBetweenTests, strSecondsBetweenTests, erpInputs, out secondsBetweenTests)
                & FormInputValidator.validateMaxNetworkInterfaceUsage(txtMaxNetworkUsage, strMaxNetworkInterfaceUsage, erpInputs, out maxNetworkInterfaceUsagePercentage);

            if (validationCheck)
            {
                // Creating the helper object
                ph = new PingHelper(remoteAddr, timeout, pingsPerTest, maxNetworkInterfaceUsagePercentage, secondsBetweenPings);
                results = new List<PingResult>();

                // Create and setup the timer that will start the test every *user given* time
                timerDead.Reset();
                timer = new System.Timers.Timer();
                timer.AutoReset = false;
                timer.Interval = (double)secondsBetweenTests*1000;
                timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
                //timer.Start();
                /*
                 * Workaround the problem that i need the event to be first fired as soon as i start the timer, not after its first
                 * period. But calling its event from the main thread it's not possible due to the locking problem that may occur
                 * if the user tries stops the execution during the first test session. 
                 * To simulate the situation that we will have during all but the first test session, i call the timer_Elapsed(..)
                 * event in a new thread. Performance is not the focus of this app, correctness is.
                 */
                Task.Factory.StartNew(() =>
                {
                    timer_Elapsed(this, null);
                });

                AdjustGUIToStatus(PingTesterGUIStatus.AfterStart);
            }
            else
            {
                MessageBox.Show(Strings.FormValidationError);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            // Setting the GUI to "don't touch anything please, i'm stopping"
            AdjustGUIToStatus(PingTesterGUIStatus.AfterStop);

            // Executing potentially blocking operation in a new thread to prevent GUI freez
            Task.Factory.StartNew(() =>
            {
                // Signaling to skip any remaining scheduled pings, can block if a ping is executing
                // Max block time is ping timeout
                ph.SkipRemainingPingsOnce();

                // Stopping timer, if it's event is executing the call blocks me untill it ends
                StopTimer();

                // We should update the GUI but we are not the GUI thread so we need to invoke
                MethodInvoker del = () =>
                {
                    ((PingTester)grpInputs.FindForm()).AdjustGUIToStatus(PingTesterGUIStatus.AfterLaunch);
                };
                grpInputs.BeginInvoke(del);
            });
        }

        private void btnChart_Click(object sender, EventArgs e)
        {
            // Create a copi of the structure of the result list to be passed to the chart form
            List<PingResult> toBeCharted;
            lock (resultsLock)
            {
                if (results != null && results.Count() > 0)
                {
                    toBeCharted = new List<PingResult>(results);

                    // Opening the chart form
                    PingTesterChart chartForm = new PingTesterChart(toBeCharted);
                    chartForm.Show();
                }
                else
                {
                    MessageBox.Show(Strings.ChartError);
                }
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            // Asking user confirmation (actual data will be overwitten by file data)
            DialogResult dialogResult = MessageBox.Show(Strings.ImportWarningMessage, Strings.GenericWarningTitle, MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    // Deserialization 
                    FileStream stream = File.OpenRead(dialog.FileName);
                    BinaryFormatter formatter = new BinaryFormatter();
                    results = (List<PingResult>)formatter.Deserialize(stream);
                    stream.Close();
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else, nothing ATM
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            // Suggesting file name based on time
            dialog.FileName = DateTime.Now.ToString("d.M.yyyy-HH.mm.ss") + ".bin";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (results != null && results.Count() > 0)
                {
                    // Serializzation
                    FileStream stream = File.Create(dialog.FileName);
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, results);
                    stream.Close();
                }
                else
                {
                    MessageBox.Show(Strings.ExportError);
                }
            }
        }

        #region form close and resize code

        private void PingTester_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Intercept app closing and ensure that the core is not running
            if (status != PingTesterGUIStatus.AfterLaunch)
            {
                e.Cancel = true;
                MessageBox.Show(Strings.LifeCycleExitError);
            }
        }

        private void PingTester_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                ntfPingTesterTray.Visible = true;
                ntfPingTesterTray.ShowBalloonTip(3000);
                this.ShowInTaskbar = false;
            }
        }

        private void ntfPingTesterTray_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            ntfPingTesterTray.Visible = false;
        }

        #endregion

    }
}
