using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingTester
{
    public static class Strings
    {
        public const string LifeCycleFormNotRunning = "Not running";
        public const string LifeCycleFormRunning = "Running";
        public const string LifeCycleFormStopping = "Stopping... please wait";
        public const string LifeCycleExitError = "Stop execution before closing the application.";

        public const string ImportWarningMessage = "Current data will be overwritten by file data, continue?";
        public const string ChartError = "No data to be charted yet.";
        public const string GenericWarningTitle = "Warning!";
        public const string ExportError = "No data to be exported yet.";

        public const string FormValidationAddressError = "Must be a valid Ip or existing hostname.";
        public const string FormValidationTimeoutError = "Must be an integer number in range: (_min_;_max_).";
        public const string FormValidationPingsPerTestError = "Must be an integer number in range: (_min_;_max_).";
        public const string FormValidationSecondsBetweenPingsError = "Must be a double number in range: (_min_;_max_).";
        public const string FormValidationSecondsBetweenTestsError = "Must be a double number in range: (_min_;_max_).";
        public const string FormValidationMaxNetworkUsageError = "Must be a double number in range: (_min_;_max_).";
        public const string FormValidationError = "One ore more invalid input, fix them and try again.";
    }
}
