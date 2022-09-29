using System;
using System.Text.RegularExpressions;

namespace WolfApprove.Model.Extension
{
	public class DateTimeHelper
	{
		public static System.Globalization.CultureInfo _ctliTH = new System.Globalization.CultureInfo("th-TH");
		public static System.Globalization.CultureInfo _ctliEN = new System.Globalization.CultureInfo("en-GB");

		public static string DateTimeToCustomClassString(DateTime? dateTime)
		{
			return dateTime != null ? ((DateTime)dateTime).ToString(GetCustomClassDatetimeFormate, _ctliEN)  : "";
		}
        public static string DateToCustomClassString(DateTime? dateTime)
        {
            return dateTime != null ? ((DateTime)dateTime).ToString(GetCustomClassDateFormate, _ctliEN) : "";
        }

        public static DateTime? CustomClassStringtoDateTime(string dateTimeString)
		{
            try
            {
                if (string.IsNullOrWhiteSpace(dateTimeString))
                {
                    return null;
                }
                return DateTime.ParseExact(dateTimeString, GetCustomClassDatetimeFormate, _ctliEN);
            }
            catch (Exception ex)
            {
                return null;
            }
			
		}
        public static DateTime? CustomClassStringtoDate(string dateTimeString)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dateTimeString))
                {
                    return null;
                }
                return DateTime.ParseExact(dateTimeString, GetCustomClassDateFormate, _ctliEN);
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public static DateTime? CustomClassStringtoDateTimeMMMorMM(string dateTimeString,string symbol)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dateTimeString))
                {
                    return null;
                }
                else
                {
                    string dateTimeStr = dateTimeString.Substring(0, 10);
                    symbol = dateTimeStr.Contains(" ") ? " " : symbol;

                }
                var regex = new Regex(@"(([0-9])|([0-2][0-9])|([3][0-1]))\"+symbol+@"(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\" + symbol+@"((19|20)\d\d) [012]{0,1}[0-9]:[0-6][0-9]:[0-6][0-9]$");
                //var regex = new Regex(@"(([0-9])|([0-2][0-9])|([3][0-1]))\/(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\/((19|20)\d\d) [012]{0,1}[0-9]:[0-6][0-9]:[0-6][0-9]$");
                bool isValid = regex.IsMatch(dateTimeString.Trim());
                if (isValid)
                {
                    //Format is correct
                    return DateTime.ParseExact(dateTimeString, GetCustomClassDatetimeFormateMMM(symbol), _ctliEN);
                }
                else
                {
                    //Format is wrong
                    return DateTime.ParseExact(dateTimeString, GetCustomClassDatetimeFormateMM(symbol), _ctliEN);
                }
                //return DateTime.ParseExact(dateTimeString, GetCustomClassDatetimeFormate, _ctliEN);
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public static string GetCustomClassDatetimeFormate
		{
			get
			{
				return "dd/MM/yyyy HH:mm:ss";
			}
		}
        public static string GetCustomClassDateFormate
        {
            get
            {
                return "dd/MM/yyyy";
            }
        }

        public static string GetSimpleFormate
		{
			get
			{
				return "yyyy-MM-ddTHH:mm:ss.fff";
			}
		}
        public static string GetCustomClassDatetimeFormateMM(string symbol)
        {
            return "dd" + symbol + "MM" + symbol + "yyyy HH:mm:ss";
        }
        public static string GetCustomClassDatetimeFormateMMM(string symbol)
        {
            return "dd" + symbol + "MMM" + symbol + "yyyy HH:mm:ss";
        }
    }
}