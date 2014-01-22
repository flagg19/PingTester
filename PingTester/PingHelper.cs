using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Diagnostics;

namespace PingService
{
    // All possible results of the network check
    enum CheckNetworkUsageStatus
    {
        Good,
        Crowded,
        UnableToTest
    };

    class PingHelper
    {
        // Private vars ping-related
        IPAddress remoteAddr;
        int timeout;
        int count;
        int maxNetworkInterfaceUsagePercentage;
        int secondsBetweenPings;
        
        // Reference to the network interface that will be used to ping
        NetworkInterface ni;

        // Performance counter needed to check network bandwith usage
        PerformanceCounter bandwidthCounter;
        PerformanceCounter dataSentCounter;
        PerformanceCounter dataReceivedCounter;

        public PingHelper(IPAddress remoteAddr, int timeout, int count, int maxNetworkInterfaceUsagePercentage, int secondsBetweenPings)
        {
            this.remoteAddr = remoteAddr;
            this.timeout = timeout;
            this.count = count;
            this.maxNetworkInterfaceUsagePercentage = maxNetworkInterfaceUsagePercentage;
            this.secondsBetweenPings = secondsBetweenPings;

            // Trying to get the reference to the network interface
            this.ni = getNetworkInterface();
            // If it fails no need for performance counters 'cause we wont be able to use them
            if (ni != null)
            {
                bandwidthCounter = new PerformanceCounter("Network Interface", "Current Bandwidth", ni.Description);
                dataSentCounter = new PerformanceCounter("Network Interface", "Bytes Sent/sec", ni.Description);
                dataReceivedCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", ni.Description);
            }
        }

        public PingResult TestPing()
        {
            Ping pingSender = new Ping();
            PingResult result = new PingResult();

            for (int i = 0; i < count; i++)
            {
                // Checking the network usage...
                CheckNetworkUsageStatus check = checkNetworkUsage();
                switch (check)
                {
                    case (CheckNetworkUsageStatus.Good): // Ok, path clear, start pinging
                        try
                        {
                            PingReply reply = pingSender.Send(remoteAddr, timeout);
                            if (reply.Status == IPStatus.Success)
                            {
                                // All has gone well
                                result.addPingResultEntry(new PingResultEntry(
                                    reply.RoundtripTime, reply.Status, PingResultEntryStatus.Success, System.DateTime.Now));
                            }
                            else
                            {
                                // Something went wrong, wrong but "expected"
                                result.addPingResultEntry(new PingResultEntry(
                                    reply.RoundtripTime, reply.Status, PingResultEntryStatus.GenericFailureSeeReplyStatus, System.DateTime.Now));
                            }
                        }
                        catch
                        {
                            // Something went really wrong, and we should "prepare for unexpected consequences"... oh, we did it with this catch
                            result.addPingResultEntry(new PingResultEntry(
                                null, null, PingResultEntryStatus.ExceptionRaisedDuringPing, System.DateTime.Now));
                        }
                        break;
                    case CheckNetworkUsageStatus.Crowded:
                        // Network usage was too high to give meaningful results
                        result.addPingResultEntry(new PingResultEntry(
                            null, null, PingResultEntryStatus.PingAbortedForHighNetworkUsage, System.DateTime.Now));
                        break;
                    case CheckNetworkUsageStatus.UnableToTest:
                        // Something went wrong in checking the network
                        result.addPingResultEntry(new PingResultEntry(
                            null, null, PingResultEntryStatus.PingAbortedUnableToGetNetworkUsage, System.DateTime.Now));
                        break;
                }

                // Ok, i've done one single ping, now i should wait the time the user setted before doing it again
                System.Threading.Thread.Sleep(secondsBetweenPings * 1000);
            }

            return result;
        }

        // Check if network bandwith usage % is lower (or not) then the value given by the user
        private CheckNetworkUsageStatus checkNetworkUsage()
        {
            // If it has been possible to get the informations about the network interface...
            if (ni != null && bandwidthCounter != null && dataSentCounter != null && dataReceivedCounter != null)
            {
                // Do the check...
                return (getNetworkUtilization() < maxNetworkInterfaceUsagePercentage) ? CheckNetworkUsageStatus.Good : CheckNetworkUsageStatus.Crowded;
            }
            else
            {
                return CheckNetworkUsageStatus.UnableToTest;
            }
            
        }

        /* 
         * As suggested somewhere on stackoverflow.com, this function tries to open an UDP connection
         * to the remote host we want to ping so we can get our local address and find the interface that uses it
         */
        private NetworkInterface getNetworkInterface()
        {
            UdpClient dummyUdpClient = new UdpClient(remoteAddr.ToString(), 1);
            IPAddress localAddr = ((IPEndPoint)dummyUdpClient.Client.LocalEndPoint).Address;
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                IPInterfaceProperties ipProps = nic.GetIPProperties();

                foreach (UnicastIPAddressInformation ip in ipProps.UnicastAddresses)
                {
                    if ((nic.OperationalStatus == OperationalStatus.Up) && (ip.Address.AddressFamily == AddressFamily.InterNetwork))
                    {
                        return nic;
                    }
                } 
            }
            return null;
        }


        /* 
         * As suggested somewhere on stackoverflow.com, this function use the infos provided by the performance counters
         * to stimate the current percentage usage of the network interface. Keep in mind that the % is over the speed
         * of the interface, which is NOT the same as the bandwith your IPS provides you. 
         * Eg:
         * - IPS gives you 7MB adsl
         * - You are connected to your router via ethernet 100MB
         * --> 100MB is the important value, max possible interface usage for you will be around 7%
         * Selecting a max testable usage of 1% is probably good to prevent your ping from being ruined.
         */
        private double getNetworkUtilization()
        {
            const int numberOfIterations = 10;

            float bandwidth = bandwidthCounter.NextValue();

            float sendSum = 0;
            float receiveSum = 0;

            for (int index = 0; index < numberOfIterations; index++)
            {
                sendSum += dataSentCounter.NextValue();
                receiveSum += dataReceivedCounter.NextValue();
            }

            float dataSent = sendSum;
            float dataReceived = receiveSum;

            double utilization = (8 * (dataSent + dataReceived)) / (bandwidth * numberOfIterations) * 100;
            return utilization;
        }
    }
}
