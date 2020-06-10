//---------------------------------------------------------------------------//
// Developer: Boris Gappov, gappov@gmail.com, +79262302992                   //
// Moscow, 09/26/2014                                                        //
//---------------------------------------------------------------------------//

using System.Collections.Generic;
using System.Linq;

namespace VendingMachineCore
{
    /// <summary>
    ///     Vending Machine
    /// </summary>
    public class VendingMachine
    {
        /// <summary>
        ///     Coin vault
        /// </summary>
        public CoinAcceptor Acceptor = new CoinAcceptor();

        /// <summary>
        ///     Amount deposited by the client
        /// </summary>
        public int Balance;

        /// <summary>
        ///     Products and their quantities
        /// </summary>
        public Dictionary<int, int> Products = new Dictionary<int, int>();

        /// <summary>
        ///     Product Types
        /// </summary>
        public List<ProductType> ProductTypes = new List<ProductType>();

        /// <summary>
        ///     Returns product type by Id
        /// </summary>
        /// <param name="Id">Id product type</param>
        /// <returns>type of product</returns>
        public ProductType GetProductTypeById(int Id)
        {
            return ProductTypes.Where(x => x.Id == Id).FirstOrDefault();
        }

        /// <summary>
        ///     Makes product sale
        /// </summary>
        /// <param name="ProductId">Product id</param>
        /// <returns>Operation result</returns>
        public Result BuyProduct(int ProductId)
        {
            if (Products[ProductId] == 0) return new Result {Success = false, Message = "This product has ended"};
            var ProductType = GetProductTypeById(ProductId);
            if (ProductType.Price > Balance)
            {
                return new Result {Success = false, Message = "Not enough money, top up balance"};
            }

            if (Balance > ProductType.Price && !Acceptor.IsCoinsEnough(Balance - ProductType.Price))
                return new Result
                    {Success = false, Message = "I can’t sell the goods, because I won’t be able to give change"};
            Balance -= ProductType.Price;
            Products[ProductId] = Products[ProductId] - 1;
            return new Result {Success = true, Message = "Product sold successfully"};
        }

        /// <summary>
        ///     Adds a coin to the vault
        /// </summary>
        /// <param name="CoinValue"></param>
        public void PushCoin(int CoinValue)
        {
            Balance += CoinValue;
            Acceptor.Push(CoinValue, 1);
        }

        /// <summary>
        ///     Returns change
        /// </summary>
        /// <param name="Coins">Change</param>
        /// <returns>Operation result</returns>
        public Result GetRemain(out Money[] Coins)
        {
            Coins = new Money[] { };
            if (Balance == 0)
            {
                new Result {Success = false, Message = "The balance is empty, nothing to return"};
            }
            else
            {
                Coins = Acceptor.Get(Balance);
                if (Coins.Length > 0)
                {
                    Balance = 0;
                    return new Result {Success = true, Message = "Change returned successfully"};
                }
            }

            return new Result {Success = false, Message = "Unable to give change"};
        }
    }
}