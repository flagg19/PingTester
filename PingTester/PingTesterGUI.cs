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
    // Vari di esecuzione corrispondenti a una precisa configurazione di componenti grafici abilitati o meno
    enum PingTesterGUIStatus
    {
        AfterLaunch,
        AfterStart,
        AfterStop
    };

    public partial class PingTester : Form
    {
        // Parametri vari da passare alla classe PingHelper
        IPAddress remoteAddr;
        int timeout;
        int count;
        int maxNetworkInterfaceUsagePercentage;
        int secondsBetweenPings;
        int secondsBetweenTests;

        // Variabili locali di questa classe
        PingHelper ph;
        List<PingResult> results;
        object resultsLock;
        PingTesterGUIStatus status;

        // Variabili relative al timer
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

        void AdjustGUIToStatus(PingTesterGUIStatus status)
        {
            this.status = status;
            switch (status)
            {
                case PingTesterGUIStatus.AfterLaunch:
                    grpInputs.Enabled = true;
                    btnStop.Enabled = false;
                    btnStart.Enabled = true;
                    btnImport.Enabled = true;
                    btnExport.Enabled = true;
                    break;
                case PingTesterGUIStatus.AfterStart: // Aka: running
                    grpInputs.Enabled = false;
                    btnStop.Enabled = true;
                    btnStart.Enabled = false;
                    btnImport.Enabled = false;
                    btnExport.Enabled = false;
                    break;
                case PingTesterGUIStatus.AfterStop: // Aka: stopping
                    grpInputs.Enabled = false;
                    btnStop.Enabled = false;
                    btnStart.Enabled = false;
                    btnImport.Enabled = false;
                    btnExport.Enabled = false;
                    break;
            }
        }

        #region roba relativa al timer

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

        #endregion

        private void btnStart_Click(object sender, EventArgs e)
        {
            // Parso i parametri
            remoteAddr = IPAddress.Parse(txtAddress.Text);
            timeout = Int32.Parse(txtTimeout.Text);
            count = Int32.Parse(txtPingPerTest.Text);
            maxNetworkInterfaceUsagePercentage = Int32.Parse(txtMaxNetworkUsage.Text);
            secondsBetweenPings = Int32.Parse(txtSecondsBetweenPings.Text);
            secondsBetweenTests = Int32.Parse(txtSecondsBetweenTests.Text) * 1000;

            // Creo l'oggetto che mi permette di fare i test
            ph = new PingHelper(remoteAddr, timeout, count, maxNetworkInterfaceUsagePercentage, secondsBetweenPings);
            results = new List<PingResult>();

            // Creo e setto il timer che eseguirà i test ogni tot tempo
            timerDead.Reset();
            timer = new System.Timers.Timer();
            timer.AutoReset = false;
            timer.Interval = secondsBetweenTests;
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            //timer.Start();
            /*
             * Workaroud per aggirare il problema che il timer aspetta tutto il periodo dopo lo start per sparare la prima volta
             * Potrei chiamare il suo handler (orrida cosa comunque) come un metodo normale ma poi ci sarebbero casini coi lock
             * perché eseguirebbe sul main thread. Allora mi tocca avviare la prima volta l'handler su un task a parte simulando
             * la situazione che avrei se fosse stato il timer a "sparare".
             */
            Task.Factory.StartNew(() =>
            {
                timer_Elapsed(this, null);
            });


            // Disabilito componenti grafici con cui non si deve poter interagire mentre il programma è in funzione
            AdjustGUIToStatus(PingTesterGUIStatus.AfterStart);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            // Imposto la grafica "spegnimento" in modo che l'utente non possa toccare nulla finché non sono certo che è tutto a posto
            AdjustGUIToStatus(PingTesterGUIStatus.AfterStop);

            // Eseguo quanto segue in un nuovo thread per non bloccare la GUI
            Task.Factory.StartNew(() =>
            {
                // Fermo il timer, se sta eseguendo proprio in questo momento questa chiamata è bloccante e ritorna appena il timer ha finito
                StopTimer();

                // Invoco il l'aggiornamento della grafica sul thread della GUI
                MethodInvoker del = () =>
                {
                    // Ripristino la situazione grafiga iniziale
                    ((PingTester)grpInputs.FindForm()).AdjustGUIToStatus(PingTesterGUIStatus.AfterLaunch);
                };
                grpInputs.BeginInvoke(del);
            });
        }

        private void btnChart_Click(object sender, EventArgs e)
        {
            // Creo una copia della struttura della lista così da potervi accedere dall'altra form senza problemi di concorrenza
            List<PingResult> toBeCharted;
            lock (resultsLock)
            {
                if (results != null && results.Count() > 0)
                {
                    toBeCharted = new List<PingResult>(results);

                    // Apro la form con i dati raccolti fin ora
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
            // Chiedo conferma all'utente perché sovrascriverà i dati in memoria
            DialogResult dialogResult = MessageBox.Show(Strings.ImportWarningMessage, Strings.GenericWarningTitle, MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    // Deserializzo il file
                    FileStream stream = File.OpenRead(dialog.FileName);
                    BinaryFormatter formatter = new BinaryFormatter();
                    results = (List<PingResult>)formatter.Deserialize(stream);
                    stream.Close();
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            // Nome del file suggerito
            dialog.FileName = DateTime.Now.ToString("d.M.yyyy-HH.mm.ss") + ".bin";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (results != null && results.Count() > 0)
                {
                    // Serializzo...
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

        #region gestione chiusura e ridimensionamento form

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
