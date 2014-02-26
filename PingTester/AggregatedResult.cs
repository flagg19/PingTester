using PingService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingTester
{
    /*
     * This class' used for aggregating many PingResults.
     * It's similar to what PingResult is for PingResultEntry.
     */

    public class AggregatedResult
    {
        // The list of result to aggregate
        public List<PingResult> results;
        private double? avg;
        private double? dev;
        // All PingResults in results are taken in the same hour (can have different days), and this is the hour
        public int Hour { get; private set; }

        public AggregatedResult(List<PingResult> results, int hour)
        {
            this.results = results;
            this.Hour = hour;
            avg = null;
            dev = null;
        }

        public double GetAvg()
        {
            if (avg == null)
            {
                avg = results.Average(x => x.getAvg().Value);
            }
            return avg.Value;
        }

        public double? GetDev()
        {
            if (dev == null)
            {
                dev = Math.Pow(results.Aggregate(0.0, (acc, x) => acc + Math.Pow((x.getAvg().Value - avg.Value), 2)) / results.Count, 0.5);
            }
            return dev.Value;
        }

        public int GetCount()
        {
            return results.Count();
        }

        // Can be used to approssimate the number of "total days" our data covers
        public int GetDayCount()
        {
            return results.Select(x => new DateTime(
                x.getAvgTime().Value.Day +
                x.getAvgTime().Value.Month +
                x.getAvgTime().Value.Year))
                .Distinct().Count();
        }
    }
}
