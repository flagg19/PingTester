﻿using PingTester;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PingService
{
    // All possible things that may happend when testing
    public enum PingResultEntryStatus {
        Success,
        GenericFailureSeeReplyStatus,
        PingAbortedForHighNetworkUsage,
        PingAbortedUnableToGetNetworkUsage,
        ExceptionRaisedDuringPing
    };

    // Serializable class representing a single ping result (like doing "ping host -n 1")
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

    // Serializable class representing a group of single ping results, call it a "SessionResult" if you like
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
            // Adding a new entry
            results.Add(newEntry);
            // Reset any previously calculated stats (they where probably already null but who knows...)
            avg = null;
            dev = null;
            max = null;
            min = null;
            avgTime = null;
        }

        public double? getAvg()
        {
            // Calculating avg if not yet calculated
            if (avg == null && results.Where(res => res.Status == PingResultEntryStatus.Success).Count() > 0)
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
            // Calculating dev if not yet calculated
            if (dev == null && results.Where(res => res.Status == PingResultEntryStatus.Success).Count() > 0)
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
            // Calculating max if not yet calculated
            if (max == null && results.Where(res => res.Status == PingResultEntryStatus.Success).Count() > 0)
            {
                max = results.Where(x => x.Status == PingResultEntryStatus.Success).OrderBy(y => y.Rtt).Last().Rtt;
            }
            return max;
        }

        public double? getMin()
        {
            // Calculating min if not yet calculated
            if (min == null && results.Where(res => res.Status == PingResultEntryStatus.Success).Count() > 0)
            {
                min = results.Where(x => x.Status == PingResultEntryStatus.Success).OrderBy(y => y.Rtt).First().Rtt;
            }
            return min;
        }

        public DateTime? getAvgTime()
        {
            // Calculating avgTime if not yet calculated
            if (avgTime == null && results.Count() > 0)
            {
                avgTime = new DateTime((results.First().Time.Ticks + results.Last().Time.Ticks) / 2);
            }
            return avgTime;
        }

        // Group all the given PingResults by days (mon-sun) and convert them in AggregatedResults, which are grouped by hours (0..24)
        public static Dictionary<DayOfWeek, List<AggregatedResult>> AggregatePingResults(List<List<PingResult>> toBeMerged)
        {
            // Flattening the list of lists
            List<PingResult> allResults = toBeMerged.SelectMany(x => x).Where(x => x.avg != null).ToList();

            // Prepare a new list where to put the aggregated results we are about to calculate
            Dictionary<DayOfWeek, List<AggregatedResult>> output = new Dictionary<DayOfWeek, List<AggregatedResult>>();

            // Grouping by days
            IEnumerable<IGrouping<DayOfWeek, PingResult>> groupedByDay = allResults.GroupBy(x => x.avgTime.Value.DayOfWeek);
            foreach (IGrouping<DayOfWeek, PingResult> dayGroup in groupedByDay)
            {
                // Prepare a new list where to put the aggregated results we are about to calculate
                List<AggregatedResult> tmp = new List<AggregatedResult>();

                // Grouping by hours
                IEnumerable<IGrouping<int, PingResult>> groupedByHour = dayGroup.GroupBy(x => x.avgTime.Value.Hour);
                foreach (IGrouping<int, PingResult> hourGroup in groupedByHour)
                {
                    tmp.Add(new AggregatedResult(hourGroup.ToList(), hourGroup.Key));
                }

                output.Add(dayGroup.Key, tmp);
            }
            return output;
        }
    }
}
