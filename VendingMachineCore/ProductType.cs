//---------------------------------------------------------------------------//
// Developer: Boris Gappov, gappov@gmail.com, +79262302992                   //
// Moscow, 09/26/2014                                                        //
//---------------------------------------------------------------------------//

namespace VendingMachineCore
{
    /// <summary>
    ///     type of product
    /// </summary>
    public class ProductType
    {
        /// <summary>
        ///     Unique identificator
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     The product's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Cost per piece
        /// </summary>
        public int Price { get; set; }
    }
}