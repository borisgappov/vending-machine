//---------------------------------------------------------------------------//
// Developer: Boris Gappov, gappov@gmail.com, +79262302992                   //
// Moscow, 09/26/2014                                                        //
//---------------------------------------------------------------------------//

using System;
using System.Linq;
using System.Web.UI;
using Ext.Net;
using VendingMachineCore;

namespace VendingMachine
{
    public partial class Default : Page
    {
        /// <summary>
        ///     Buyer
        /// </summary>
        public Customer Customer
        {
            get
            {
                if (Session["customer"] == null) Session["customer"] = Samples.CreateSampleCustomer1();
                return (Customer) Session["customer"];
            }
            set
            {
                Session["customer"] = value; 
            }
        }

        /// <summary>
        ///     Machine
        /// </summary>
        public VendingMachineCore.VendingMachine Machine
        {
            get
            {
                if (Session["machine"] == null) Session["machine"] = Samples.CreateSampleVendingMachine1();
                return (VendingMachineCore.VendingMachine) Session["machine"];
            }
            set
            {
                Session["machine"] = value;
            }
        }


        protected void Page_PreRender(object sender, EventArgs e)
        {
            // Creating wallet buttons
            foreach (var coins in Customer.Purse.Coins)
                PanelPurse.Items.Add(
                    new Button
                    {
                        Text = string.Format("{0} x {1} into machine >>", coins.CoinValue, coins.Count),
                        OnClientClick = string.Format("App.direct.PutCoin({0})", coins.CoinValue),
                        IDMode = IDMode.Static,
                        ID = "ButtonPut_" + coins.CoinValue,
                        Flex = 1
                    });

            // Creation of buttons of the machine
            foreach (var Product in Machine.Products)
            {
                var ProductType = Machine.GetProductTypeById(Product.Key);
                PanelProducts.Items.Add(
                    new Button
                    {
                        Text = string.Format("buy {0}, in stock {1}, price {2}", ProductType.Name, Product.Value,
                            ProductType.Price),
                        OnClientClick = string.Format("App.direct.BuyProduct({0})", ProductType.Id),
                        IDMode = IDMode.Static,
                        ID = "ButtonBuy_" + ProductType.Id,
                        Flex = 1
                    });
            }

            RefreshUi();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     Sending a coin to the machine
        /// </summary>
        /// <param name="CoinValue">Coin value</param>
        [DirectMethod]
        public void PutCoin(int CoinValue)
        {
            if (Customer.Purse.IsCoinExists(CoinValue))
            {
                var Coins = Customer.Purse.Get(CoinValue);
                if (Coins.Length > 0)
                {
                    Machine.PushCoin(Coins[0].CoinValue);
                    BalanceContainer.Update(Machine.Balance);
                    LogWrite(string.Format("The coin value {0} thrown into the machine", CoinValue));
                }
            }
            else
            {
                MessageBox(string.Format("The coins value {0} ended", CoinValue));
            }

            RefreshUi();
        }

        /// <summary>
        ///     Product purchase
        /// </summary>
        /// <param name="ProductId">Product id</param>
        [DirectMethod]
        public void BuyProduct(int ProductId)
        {
            var result = Machine.BuyProduct(ProductId);
            if (result.Success)
            {
                LogWrite(string.Format("Product purchased {0}", Machine.GetProductTypeById(ProductId).Name));
                RefreshUi();
            }
            else
            {
                MessageBox(result.Message);
            }
        }

        /// <summary>
        ///     Return change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void GetRemain(object sender, EventArgs e)
        {
            Money[] remain = { };
            var result = Machine.GetRemain(out remain);
            if (result.Success)
            {
                LogWrite(Money.GetMoneyString(remain));
                LogWrite("Change issued:");
                Customer.Purse.Push(remain);
                RefreshUi();
            }
            else
            {
                MessageBox(result.Message);
            }
        }

        /// <summary>
        ///     Displays a popup message
        /// </summary>
        /// <param name="Message">Message text</param>
        private void MessageBox(string Message)
        {
            new MessageBox().Show(new MessageBoxConfig
            {
                Title = "Message",
                Message = Message,
                Buttons = Ext.Net.MessageBox.Button.OK
            });
        }

        /// <summary>
        ///     Refreshes the contents of labels
        /// </summary>
        private void RefreshUi()
        {
            PanelPurse.Title = "Purse, " + Customer.Purse.Total;
            MachineMoneyDisplay.Title = "Money, " + Machine.Acceptor.Total;
            MachineMoneyDisplay.Update(Machine.Acceptor.GetMoneyString().Replace(Environment.NewLine, "<br />"));
            BalanceContainer.Update(Machine.Balance);
            X.Js.Call("UpdateButtons", new
            {
                CustomerButtons = Customer.Purse.Coins.Select(x => new
                {
                    x.CoinValue,
                    x.Count
                }),
                MachineButtons = from t in Machine.ProductTypes
                    from p in Machine.Products.Select(x => new {Id = x.Key, Count = x.Value})
                        .Where(x => x.Id == t.Id)
                    select new
                    {
                        t.Id,
                        t.Name,
                        t.Price,
                        p.Count
                    }
            });
        }

        /// <summary>
        ///     Adds a line to the top of the text field
        /// </summary>
        /// <param name="message"></param>
        private void LogWrite(string message)
        {
            LogTextArea.Text = message + Environment.NewLine + LogTextArea.Text;
        }

        /// <summary>
        ///     Sets experimental data
        /// </summary>
        /// <param name="Type"></param>
        [DirectMethod]
        public void Reset(int Type)
        {
            switch (Type)
            {
                case 1:
                    Session["customer"] = Samples.CreateSampleCustomer1();
                    Session["machine"] = Samples.CreateSampleVendingMachine1();
                    break;
                case 2:
                    Session["customer"] = Samples.CreateSampleCustomer2();
                    Session["machine"] = Samples.CreateSampleVendingMachine2();
                    break;
                case 3:
                    Session["customer"] = Samples.CreateSampleCustomer3();
                    Session["machine"] = Samples.CreateSampleVendingMachine3();
                    break;
            }

            X.Js.Call("Reload");
        }
    }
}