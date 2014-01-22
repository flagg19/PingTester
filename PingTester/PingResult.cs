using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PingService
{
    // Vari stati che può assumere la risposta di ping in questo contesto
    public enum PingResultEntryStatus {
        Success,
        GenericFailureSeeReplyStatus,
        PingAbortedForHighNetworkUsage,
        PingAbortedUnableToGetNetworkUsage,
        ExceptionRaisedDuringPing
    };

    // Classe wrapper attorno alla PingReply del framework, amplia la possibilità di specificare uno Stato alla risposta
    [Serializable]
    public class PingResultEntry : ISerializable
    {
        public double? Rtt { get; private set; }
        public IPStatus? IpStatus { get; private set; }
        public PingResultEntryStatus Status { get; private set; }
        public DateTime Time { get; private set; }

        public PingResultEntry(double? rtt, IPStatus? ipStatus, PingResultEntryStatus status, DateTime time)
        {
            this.IpStatus = ipStatus;
            this.Rtt = rtt;
            this.Status = status;
            this.Time = time;
        }

        // Implement this method to serialize data. The method is called on serialization.
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Rtt", Rtt, typeof(double?));
            info.AddValue("IpStatus", IpStatus, typeof(IPStatus?));
            info.AddValue("Status", Status, typeof(PingResultEntryStatus));
            info.AddValue("Time", Time.Ticks, typeof(long));
        }

        // The special constructor is used to deserialize values.
        public PingResultEntry(SerializationInfo info, StreamingContext context)
        {
            Rtt = (double?)info.GetValue("Rtt", typeof(double?));
            IpStatus = (IPStatus?)info.GetValue("IpStatus", typeof(IPStatus?));
            Status = (PingResultEntryStatus)info.GetValue("Status", typeof(PingResultEntryStatus));
            Time = new DateTime((long)info.GetValue("Time", typeof(long)));
        }
    }

    // Classe contenitore per un numero arbitrario di risultati di Ping considerando come risultato l'equivalente di "ping host -n 1"
    [Serializable]
    public class PingResult
    {
        private List<PingResultEntry> results;
        private double? avg;
        private double? dev;
        private double? max;
        private double? min;
        private DateTime? avgTime;

        public PingResult()
        {
            results = new List<PingResultEntry>();
        }

        // Implement this method to serialize data. The method is called on serialization.
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("results", results, typeof(List<PingResultEntry>));
        }

        // The special constructor is used to deserialize values.
        public PingResult(SerializationInfo info, StreamingContext context)
        {
            results = (List<PingResultEntry>)info.GetValue("Rtt", typeof(List<PingResultEntry>));
        }

        public void addPingResultEntry(PingResultEntry newEntry)
        {
            // Aggiungo il nuovo risultato
            results.Add(newEntry);
            // Resetto le statistiche in caso fossero state già calcolate
            avg = null;
            dev = null;
            max = null;
            min = null;
            avgTime = null;
        }

        public double? getAvg()
        {
            // Se non è ancora stato calcolato lo calcolo
            if (avg == null && results.Count() > 0)
            {
                avg = new double?(0);
                int cont = 0;
                foreach (PingResultEntry e in results.Where(x => x.Status == PingResultEntryStatus.Success))
                {
                    cont++;
                    avg += e.Rtt;
                }
                if (cont > 0)
                {
                    avg = avg / cont;
                }
            }
            return avg;
        }

        public double? getDev()
        {
            // Se non è ancora stato calcolato lo calcolo
            if (dev == null && results.Count() > 0)
            {
                double? tmpAvg = this.getAvg();
                if (tmpAvg != null)
                {
                    dev = new double?(0);
                    int cont = 0;
                    double tmpDev = 0;
                    foreach (PingResultEntry e in results.Where(x => x.Status == PingResultEntryStatus.Success))
                    {
                        cont++;
                        tmpDev += Math.Pow(((double)e.Rtt - (double)tmpAvg),2);
                    }
                    if (cont > 0)
                    {
                        dev = Math.Pow(tmpDev/cont, 0.5);
                    }
                }
            }
            return dev;
        }

        public double? getMax()
        {
            // Se non è ancora stato calcolato lo calcolo
            if (max == null && results.Count() > 0)
            {
                max = results.Where(x => x.Status == PingResultEntryStatus.Success).OrderBy(y => y.Rtt).Last().Rtt;
            }
            return max;
        }

        public double? getMin()
        {
            // Se non è ancora stato calcolato lo calcolo
            if (min == null && results.Count() > 0)
            {
                min = results.Where(x => x.Status == PingResultEntryStatus.Success).OrderBy(y => y.Rtt).First().Rtt;
            }
            return min;
        }

        public DateTime? getAvgTime()
        {
            // Se non è ancora stato calcolato lo calcolo
            if (avgTime == null && results.Count() > 0)
            {
                avgTime = new DateTime((results.First().Time.Ticks + results.Last().Time.Ticks) / 2);
            }
            return avgTime;
        }
    }
}
