//---------------------------------------------------------------------------//
//Разработчик: Гаппов Борис, gappov@gmail.com, +79262302992                  //
//Москва, 26.09.2014                                                         //
//---------------------------------------------------------------------------//
using System.Collections.Generic;
using System.Linq;

namespace VendingMachineCore
{
    /// <summary>
    /// Кофейний терминал
    /// </summary>
    public class VendingMachine
    {
        /// <summary>
        /// Хранилище монет
        /// </summary>
        public CoinAcceptor Acceptor = new CoinAcceptor();
     
        /// <summary>
        /// Типы продуктов
        /// </summary>
        public List<ProductType> ProductTypes = new List<ProductType>();
        
        /// <summary>
        /// Продукты и их количества
        /// </summary>
        public Dictionary<int, int> Products = new Dictionary<int, int>();

        /// <summary>
        /// Сумма внесенных клиентом средств
        /// </summary>
        public int Balance = 0;

        /// <summary>
        /// Возвращает тип продукта по Id
        /// </summary>
        /// <param name="Id">Id типа продукта</param>
        /// <returns>Тип продукта</returns>
        public ProductType GetProductTypeById(int Id)
        {
            return ProductTypes.Where(x => x.Id == Id).FirstOrDefault();
        }

        /// <summary>
        /// Производит продажу продукта
        /// </summary>
        /// <param name="ProductId">Id продукта</param>
        /// <returns>Результат операции</returns>
        public Result BuyProduct(int ProductId)
        {
            if (Products[ProductId] == 0) return new Result { Success = false, Message = "Этот продукт закончился" };
            var ProductType = GetProductTypeById(ProductId);
            if (ProductType.Price > Balance)
            {
                return new Result { Success = false, Message = "Недостаточно средств, пополните баланс" };
            }
            else
            {
                if (Balance > ProductType.Price && !Acceptor.IsCoinsEnough(Balance - ProductType.Price))
                    return new Result { Success = false, Message = "Не могу продать товар, поскольку не получится дать сдачу" };
                Balance -= ProductType.Price;
                Products[ProductId] = Products[ProductId] - 1;
                return new Result { Success = true, Message = "Продукт продан успешно" };
            }
        }

        /// <summary>
        /// Добавляет монету в хранилище
        /// </summary>
        /// <param name="CoinValue"></param>
        public void PushCoin(int CoinValue)
        {
            Balance += CoinValue;
            Acceptor.Push(CoinValue, 1);
        }

        /// <summary>
        /// Возвращает сдачу
        /// </summary>
        /// <param name="Coins">Сдача</param>
        /// <returns>Результат операции</returns>
        public Result GetRemain(out Money[] Coins)
        {
            Coins = new Money[] { };
            if (Balance == 0)
                new Result { Success = false, Message = "Баланс пуст, нечего возвращать" };
            else
            {
                Coins = Acceptor.Get(Balance);
                if (Coins.Length > 0)
                {
                    Balance = 0;
                    return new Result { Success = true, Message = "Сдача возвращена успешно" };
                }
            }
            return new Result { Success = false, Message = "Не удается дать сдачу" };
        }
    }
}
