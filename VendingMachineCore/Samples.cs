//---------------------------------------------------------------------------//
// Developer: Boris Gappov, gappov@gmail.com, +79262302992                   //
// Moscow, 09/26/2014                                                        //
//---------------------------------------------------------------------------//

using System.Collections.Generic;

namespace VendingMachineCore
{
    /// <summary>
    ///     Experimental data
    /// </summary>
    public class Samples
    {
        private static readonly List<ProductType> SampleProductTypes = new List<ProductType>
        {
            new ProductType {Id = 0, Name = "Tee", Price = 13},
            new ProductType {Id = 1, Name = "Coffee", Price = 18},
            new ProductType {Id = 2, Name = "Coffee with milk", Price = 21},
            new ProductType {Id = 3, Name = "The juice", Price = 35}
        };


        # region As in the assignment

        public static VendingMachine CreateSampleVendingMachine1()
        {
            var SampleMachine = new VendingMachine {ProductTypes = SampleProductTypes};
            SampleMachine.Products.Add(0, 10);
            SampleMachine.Products.Add(1, 20);
            SampleMachine.Products.Add(2, 20);
            SampleMachine.Products.Add(3, 15);
            SampleMachine.Acceptor.Push(1, 100);
            SampleMachine.Acceptor.Push(2, 100);
            SampleMachine.Acceptor.Push(5, 100);
            SampleMachine.Acceptor.Push(10, 100);
            return SampleMachine;
        }

        public static Customer CreateSampleCustomer1()
        {
            var SampleCustomer = new Customer();
            SampleCustomer.Purse.Push(1, 10);
            SampleCustomer.Purse.Push(2, 30);
            SampleCustomer.Purse.Push(5, 20);
            SampleCustomer.Purse.Push(10, 15);
            return SampleCustomer;
        }

        #endregion

        # region A small amount of money

        public static VendingMachine CreateSampleVendingMachine2()
        {
            var SampleMachine = new VendingMachine {ProductTypes = SampleProductTypes};
            SampleMachine.Products.Add(0, 10);
            SampleMachine.Products.Add(1, 10);
            SampleMachine.Products.Add(2, 10);
            SampleMachine.Products.Add(3, 10);
            SampleMachine.Acceptor.Push(1, 1);
            SampleMachine.Acceptor.Push(2, 1);
            SampleMachine.Acceptor.Push(5, 1);
            SampleMachine.Acceptor.Push(10, 1);
            return SampleMachine;
        }

        public static Customer CreateSampleCustomer2()
        {
            var SampleCustomer = new Customer();
            SampleCustomer.Purse.Push(1, 1);
            SampleCustomer.Purse.Push(2, 1);
            SampleCustomer.Purse.Push(5, 1);
            SampleCustomer.Purse.Push(10, 20);
            return SampleCustomer;
        }

        #endregion

        # region Exotic coins

        public static VendingMachine CreateSampleVendingMachine3()
        {
            var SampleMachine = new VendingMachine {ProductTypes = SampleProductTypes};
            SampleMachine.Products.Add(0, 3);
            SampleMachine.Products.Add(1, 2);
            SampleMachine.Products.Add(2, 4);
            SampleMachine.Products.Add(3, 5);
            SampleMachine.Acceptor.Push(1, 3);
            SampleMachine.Acceptor.Push(3, 2);
            SampleMachine.Acceptor.Push(7, 4);
            SampleMachine.Acceptor.Push(11, 3);
            return SampleMachine;
        }

        public static Customer CreateSampleCustomer3()
        {
            var SampleCustomer = new Customer();
            SampleCustomer.Purse.Push(1, 5);
            SampleCustomer.Purse.Push(3, 3);
            SampleCustomer.Purse.Push(7, 3);
            SampleCustomer.Purse.Push(11, 3);
            return SampleCustomer;
        }

        #endregion
    }
}