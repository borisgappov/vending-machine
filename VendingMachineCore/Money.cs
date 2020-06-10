//---------------------------------------------------------------------------//
// Developer: Boris Gappov, gappov@gmail.com, +79262302992                   //
// Moscow, 09/26/2014                                                        //
//---------------------------------------------------------------------------//

using System;
using System.Text;

namespace VendingMachineCore
{
    /// <summary>
    ///     Money (coins) of a certain denomination
    /// </summary>
    public class Money
    {
        /// <summary>
        ///     Sum
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        ///     The value of one coin, for example 10
        /// </summary>
        public int CoinValue { get; set; }

        /// <summary>
        ///     Number of coins
        /// </summary>
        public int Count => Amount / CoinValue;

        /// <summary>
        ///     Returns an array of Money as text
        /// </summary>
        /// <returns></returns>
        public static string GetMoneyString(Money[] MoneyToStringify)
        {
            var builder = new StringBuilder();
            foreach (var coin in MoneyToStringify)
                builder.Append(string.Format("value: {0}, count: {1}{2}", coin.CoinValue, coin.Count,
                    Environment.NewLine));
            return builder.ToString();
        }
    }
}