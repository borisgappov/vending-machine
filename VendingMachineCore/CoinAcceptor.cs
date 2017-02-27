//---------------------------------------------------------------------------//
//Разработчик: Гаппов Борис, gappov@gmail.com, +79262302992                  //
//Москва, 26.09.2014                                                         //
//---------------------------------------------------------------------------//
using System.Collections.Generic;
using System.Linq;

namespace VendingMachineCore
{
    /// <summary>
    /// Приемник (хранилище) монет
    /// </summary>
    public class CoinAcceptor
    {
        /// <summary>
        /// Монеты
        /// </summary>
        public List<Money> Coins = new List<Money>();

        /// <summary>
        /// Хранимая сумма денег
        /// </summary>
        public int Total
        {
            get
            {
                return Coins.Sum(x => x.Amount);
            }
        }

        /// <summary>
        /// Добавляет одну монету в хранилище
        /// </summary>
        /// <param name="Value">Достоинство монеты</param>
        /// <param name="Count">Количество монет</param>
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
        /// Добавляет несколько монет одного достоинства в хранилище 
        /// </summary>
        /// <param name="Incomming"></param>
        public void Push(Money Incomming)
        {
            Push(Incomming.CoinValue, Incomming.Amount / Incomming.CoinValue);
        }

        /// <summary>
        /// Добавляет монеты разного достоинства в хранилище
        /// </summary>
        /// <param name="Incomming"></param>
        public void Push(Money[] Incomming)
        {
            foreach (var item in Incomming) Push(item);
        }

        /// <summary>
        /// Извлекает заданную сумму из хранилища, выдаются комбинацией монет максимального достоинства
        /// Если монет недостаточно для формиования нужной суммы, возвращается пустой массив
        /// </summary>
        /// <param name="Sum">Сумма для извлечения</param>
        /// <returns>Набор (массив) разного достоинства</returns>
        public Money[] Get(int Sum)
        {
            var result = new List<Money>();
            if (Sum <= Coins.Sum(x => x.Amount))
            {
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
                        remainder = remainder % ( moneyByValue.CoinValue * coins );
                        if (remainder == 0) break;
                    }
                }
            }
            return result.ToArray();
        }

        /// <summary>
        /// Определяет, возможно ли выдать требуемую сумму оставшимся набором монет
        /// </summary>
        /// <param name="Sum">Требуемая сумма</param>
        /// <returns>истина, если монет хватает</returns>
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
        /// Определяет, возможно ли выдать монету требуемого достоинства
        /// </summary>
        /// <param name="CoinValue">Достоинство требемой монеты</param>
        /// <returns>истина, если монета есть</returns>
        public bool IsCoinExists(int CoinValue)
        {
            var coins = Coins.Where(x => x.CoinValue == CoinValue).FirstOrDefault();
            return coins != null && coins.Count > 0;
        }

        /// <summary>
        /// Переводит деньги из одного хранилища в другое
        /// </summary>
        /// <param name="Source">Хранилище-источник</param>
        /// <param name="Destination">Хранилище-назначение</param>
        /// <param name="Sum">Переводимая сумма</param>
        /// <returns>истина, если в источнике достаточно монет</returns>
        public static bool TransferMoney(CoinAcceptor Source, CoinAcceptor Destination, int Sum)
        {
            var Enough = Source.IsCoinsEnough(Sum);
            if (Enough)
                Destination.Push(Source.Get(Sum));
            return Enough;
        }

        /// <summary>
        /// Возвращает в виде текста содержимое хранилища либо массива MoneyToStringify
        /// </summary>
        /// <returns></returns>
        public string GetMoneyString()
        {
            return Money.GetMoneyString(Coins.ToArray());
        }

    }
}
