//---------------------------------------------------------------------------//
//Разработчик: Гаппов Борис, gappov@gmail.com, +79262302992                  //
//Москва, 26.09.2014                                                         //
//---------------------------------------------------------------------------//
namespace VendingMachineCore
{
    /// <summary>
    /// Покупатель
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// Кошелек
        /// </summary>
        public CoinAcceptor Purse = new CoinAcceptor();
    }
}
