using System;
using System.Collections.Generic;
using System.Text;

namespace PotterKata.Models.Extensions
{
    public static class DecimalExtensions
    {
        public static int DecimalPlaces { get; set; } = 2;
        public static string PriceCurrency { get; set; } = "€";
        public static decimal WithDecimalPlaces(this decimal input, int decimalPlaces)
        {
            return Math.Round(input, decimalPlaces);
        }
        public static string AsPriceString(this decimal input)
        {
            return $"{PriceCurrency}{Math.Round(input, DecimalPlaces, MidpointRounding.AwayFromZero).ToString("#.00")}" ;
        }
    }
}


