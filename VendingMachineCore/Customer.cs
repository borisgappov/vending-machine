//---------------------------------------------------------------------------//
// Developer: Boris Gappov, gappov@gmail.com, +79262302992                   //
// Moscow, 09/26/2014                                                        //
//---------------------------------------------------------------------------//

namespace VendingMachineCore
{
    /// <summary>
    ///     Buyer
    /// </summary>
    public class Customer
    {
        /// <summary>
        ///     Purse
        /// </summary>
        public CoinAcceptor Purse = new CoinAcceptor();
    }
}