using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PingTester
{
    static class FormInputValidator
    {
        // Const range values
        public const int timeoutRangeMin = 0;
        public const int timeoutRangeMax = 10000;
        public const int pingsPerTestRangeMin = 1;
        public const int pingsPerTestRangeMax = int.MaxValue;
        public const double secondsBetweenPingsRangeMin = 0.5;
        public const double secondsBetweenPingsRangeMax = double.MaxValue;
        public const double secondsBetweenTestsRangeMin = 0;
        public const double secondsBetweenTestsRangeMax = double.MaxValue;
        public const double maxNetworkInterfaceUsageRangeMin = 0;
        public const double maxNetworkInterfaceUsageRangeMax = 100;

        // Delegate used to absctract from specific number type
        private delegate bool GenericTryParse<T>(TextBox txtBox, out T result) where T : IComparable;

        // Validate the give host in the form of an ip or hostname
        public static bool validateHost(TextBox txtBox, string errorMessage, ErrorProvider ep, out IPAddress result)
        {
            if (!IPAddress.TryParse(txtBox.Text, out result))
            {
                try
                {
                    result = Dns.GetHostEntry(txtBox.Text).AddressList[0];
                    ep.SetError(txtBox, string.Empty);
                    return true;
                }
                catch (Exception)
                {
                    ep.SetError(txtBox, errorMessage);
                    return false;
                }
            }
            else
            {
                ep.SetError(txtBox, string.Empty);
                return true;
            }
        }

        #region numerical input validation

        public static bool validateTimeout(TextBox txtBox, string errorMessage, ErrorProvider ep, out int result)
        {
            return ValidateIntInRange(txtBox, timeoutRangeMin, timeoutRangeMax, errorMessage, ep, out result);
        }

        public static bool validatePingsPerTest(TextBox txtBox, string errorMessage, ErrorProvider ep, out int result)
        {
            return ValidateIntInRange(txtBox, pingsPerTestRangeMin, pingsPerTestRangeMax, errorMessage, ep, out result);
        }

        public static bool validateSecondsBetweenPings(TextBox txtBox, string errorMessage, ErrorProvider ep, out double result)
        {
            return ValidateDoubleInRange(txtBox, secondsBetweenPingsRangeMin, secondsBetweenPingsRangeMax, errorMessage, ep, out result);
        }

        public static bool validateSecondsBetweenTests(TextBox txtBox, string errorMessage, ErrorProvider ep, out double result)
        {
            return ValidateDoubleInRange(txtBox, secondsBetweenTestsRangeMin, secondsBetweenTestsRangeMax, errorMessage, ep, out result);
        }

        public static bool validateMaxNetworkInterfaceUsage(TextBox txtBox, string errorMessage, ErrorProvider ep, out double result)
        {
            return ValidateDoubleInRange(txtBox, maxNetworkInterfaceUsageRangeMin, maxNetworkInterfaceUsageRangeMax, errorMessage, ep, out result);
        }

        #endregion 

        // Wrapper for validateGenericNumberInRange<int>
        private static bool ValidateIntInRange(TextBox txtBox, int rangeMin, int rangeMax, string errorMessage, ErrorProvider ep, out int result)
        {
            GenericTryParse<int> gd = (TextBox tb, out int r) => { return int.TryParse(tb.Text, out r); };
            return validateGenericNumberInRange<int>(txtBox, rangeMin, rangeMax, errorMessage, ep, gd, out result);
        }

        // Wrapper for validateGenericNumberInRange<double>
        private static bool ValidateDoubleInRange(TextBox txtBox, double rangeMin, double rangeMax, string errorMessage, ErrorProvider ep, out double result)
        {
            GenericTryParse<double> gd = (TextBox tb, out double r) => { return double.TryParse(tb.Text, out r); };
            return validateGenericNumberInRange<double>(txtBox, rangeMin, rangeMax, errorMessage, ep, gd, out result);
        }

        // Generic function to parse any kind of number and validate their range
        private static bool validateGenericNumberInRange<T>(TextBox txtBox, T rangeMin, T rangeMax,
            string errorMessage, ErrorProvider ep, GenericTryParse<T> gtp, out T result) where T : IComparable
        {
            T tmpResult;
            if (gtp(txtBox, out tmpResult))
            {
                // Ok it's a double but is it valid?
                if (tmpResult.CompareTo(rangeMin) > 0 && tmpResult.CompareTo(rangeMax) < 0)
                {
                    // Yes it is
                    result = tmpResult;
                    ep.SetError(txtBox, string.Empty);
                    return true;
                }
                else
                {
                    // No
                    result = default(T);
                    ep.SetError(txtBox, errorMessage);
                    return false;
                }
            }
            else
            {
                // Not even a double
                result = default(T);
                ep.SetError(txtBox, errorMessage);
                return false;
            }
        }
    }
}
