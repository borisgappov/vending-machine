//---------------------------------------------------------------------------//
//Разработчик: Гаппов Борис, gappov@gmail.com, +79262302992                  //
//Москва, 26.09.2014                                                         //
//---------------------------------------------------------------------------//
using System;
using System.Text;

namespace VendingMachineCore
{
    /// <summary>
    /// Деньги (монеты) определенного достоинства
    /// </summary>
    public class Money
    {
        /// <summary>
        /// Сумма
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Достоинство одной монеты, например 10 руб
        /// </summary>
        public int CoinValue { get; set; }

        /// <summary>
        /// Количество монет
        /// </summary>
        public int Count
        {
            get
            {
                return Amount / CoinValue;
            }
        }

        /// <summary>
        /// Возвращает в виде текста массив Money
        /// </summary>
        /// <returns></returns>
        static public string GetMoneyString(Money[] MoneyToStringify)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var coin in MoneyToStringify)
            {
                builder.Append(String.Format("достоинство: {0}, количество: {1}{2}", coin.CoinValue, coin.Count, Environment.NewLine));
            }
            return builder.ToString();
        }
    }
}
