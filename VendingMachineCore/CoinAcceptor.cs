//---------------------------------------------------------------------------//
// Developer: Boris Gappov, gappov@gmail.com, +79262302992                   //
// Moscow, 09/26/2014                                                        //
//---------------------------------------------------------------------------//

using System.Collections.Generic;
using System.Linq;

namespace VendingMachineCore
{
    /// <summary>
    ///     Receiver (vault) of coins
    /// </summary>
    public class CoinAcceptor
    {
        /// <summary>
        ///     Coins
        /// </summary>
        public List<Money> Coins = new List<Money>();

        /// <summary>
        ///     Stored Amount
        /// </summary>
        public int Total
        {
            get { return Coins.Sum(x => x.Amount); }
        }

        /// <summary>
        ///     Adds one coin to the vault
        /// </summary>
        /// <param name="Value">Coin value</param>
        /// <param name="Count">Number of coins</param>
        public void Push(int Value, int Count)
        {
            var moneyByValue = Coins.Where(x => x.CoinValue == Value).FirstOrDefault();
            if (moneyByValue == null)
                Coins.Add(new Money
                {
                    CoinValue = Value,
                    Amount = Value * Count
                });
            else
                moneyByValue.Amount += Value * Count;
        }

        /// <summary>
        ///     Adds multiple coins of the same denomination to the vault
        /// </summary>
        /// <param name="Incomming"></param>
        public void Push(Money Incomming)
        {
            Push(Incomming.CoinValue, Incomming.Amount / Incomming.CoinValue);
        }

        /// <summary>
        ///     Adds coins of various denominations to the vault
        /// </summary>
        /// <param name="Incomming"></param>
        public void Push(Money[] Incomming)
        {
            foreach (var item in Incomming) Push(item);
        }

        /// <summary>
        ///     Retrieves the specified amount from the vault, issued a combination of coins of maximum denomination
        ///     If there are not enough coins to form the desired amount, an empty array is returned
        /// </summary>
        /// <param name="Sum">Amount to Extract</param>
        /// <returns>A set (array) of different denominations</returns>
        public Money[] Get(int Sum)
        {
            var result = new List<Money>();
            if (Sum <= Coins.Sum(x => x.Amount))
                if (IsCoinsEnough(Sum))
                {
                    var remainder = Sum;
                    foreach (var moneyByValue in Coins.OrderByDescending(x => x.CoinValue))
                    {
                        if (moneyByValue.Amount == 0 || remainder / moneyByValue.CoinValue == 0) continue;
                        var coins = remainder / moneyByValue.CoinValue;
                        if (coins > moneyByValue.Count) coins = moneyByValue.Count;
                        moneyByValue.Amount -= coins * moneyByValue.CoinValue;
                        result.Add(new Money
                        {
                            Amount = coins * moneyByValue.CoinValue,
                            CoinValue = moneyByValue.CoinValue
                        });
                        remainder = remainder % (moneyByValue.CoinValue * coins);
                        if (remainder == 0) break;
                    }
                }

            return result.ToArray();
        }

        /// <summary>
        ///     Determines whether it is possible to issue the required amount with the remaining set of coins.
        /// </summary>
        /// <param name="Sum">Amount Required</param>
        /// <returns>true if there are enough coins</returns>
        public bool IsCoinsEnough(int Sum)
        {
            var CheckSum = 0;
            var remainder = Sum;
            foreach (var moneyByValue in Coins.OrderByDescending(x => x.CoinValue))
            {
                if (moneyByValue.Amount == 0 || remainder / moneyByValue.CoinValue == 0) continue;
                var coins = remainder / moneyByValue.CoinValue;
                if (coins > moneyByValue.Count) coins = moneyByValue.Count;
                CheckSum += coins * moneyByValue.CoinValue;
                remainder = remainder % (moneyByValue.CoinValue * coins);
                if (remainder == 0) break;
            }

            return CheckSum == Sum;
        }

        /// <summary>
        ///     Determines whether it is possible to issue a coin of the required value.
        /// </summary>
        /// <param name="CoinValue">Dignity of the required coin</param>
        /// <returns>true if there is a coin</returns>
        public bool IsCoinExists(int CoinValue)
        {
            var coins = Coins.Where(x => x.CoinValue == CoinValue).FirstOrDefault();
            return coins != null && coins.Count > 0;
        }

        /// <summary>
        ///     Transferring money from one vault to another
        /// </summary>
        /// <param name="Source">Source storage</param>
        /// <param name="Destination">Destination Storage</param>
        /// <param name="Sum">Amount to be transferred</param>
        /// <returns>true if there are enough coins in the source</returns>
        public static bool TransferMoney(CoinAcceptor Source, CoinAcceptor Destination, int Sum)
        {
            var Enough = Source.IsCoinsEnough(Sum);
            if (Enough)
                Destination.Push(Source.Get(Sum));
            return Enough;
        }

        /// <summary>
        ///     Returns the contents of the storage or MoneyToStringify array as text
        /// </summary>
        /// <returns></returns>
        public string GetMoneyString()
        {
            return Money.GetMoneyString(Coins.ToArray());
        }
    }
}