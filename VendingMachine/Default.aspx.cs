//---------------------------------------------------------------------------//
//Разработчик: Гаппов Борис, gappov@gmail.com, +79262302992                  //
//Москва, 26.09.2014                                                         //
//---------------------------------------------------------------------------//
using System;
using System.Linq;
using VendingMachineCore;
using Ext.Net;

public partial class _Default : System.Web.UI.Page
{
    /// <summary>
    /// Покупатель
    /// </summary>
    public Customer Customer
    {
        get
        {
            if (Session["customer"] == null) Session["customer"] = Samples.CreateSampleCustomer1();
            return (Customer)Session["customer"];
        }
        set
        {
            Session["customer"] = value;
        }
    }

    /// <summary>
    /// Автомат
    /// </summary>
    public VendingMachine Machine
    {
        get
        {
            if (Session["machine"] == null) Session["machine"] = Samples.CreateSampleVendingMachine1();
            return (VendingMachine)Session["machine"];
        }
        set
        {
            Session["machine"] = value;
        }
    }


    protected void Page_PreRender(object sender, EventArgs e)
    {
        // Создание кнопок кошелька
        foreach (var coins in Customer.Purse.Coins)
        {
            PanelPurse.Items.Add(
                new Ext.Net.Button
                {
                    Text = String.Format("{0} x {1} в автомат >>", coins.CoinValue, coins.Count),
                    OnClientClick = String.Format("App.direct.PutCoin({0})", coins.CoinValue),
                    IDMode = IDMode.Static,
                    ID = "ButtonPut_" + coins.CoinValue,
                    Flex = 1
                });
        }
        // Создание кнопок автомата
        foreach (var Product in Machine.Products)
        {
            var ProductType = Machine.GetProductTypeById(Product.Key);
            PanelProducts.Items.Add(
                new Ext.Net.Button
                {
                    Text = String.Format("купить {0}, в наличии {1} шт, цена {2}", ProductType.Name, Product.Value, ProductType.Price),
                    OnClientClick = String.Format("App.direct.BuyProduct({0})", ProductType.Id),
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
    /// Отправка монеты в автомат
    /// </summary>
    /// <param name="CoinValue">Достоинство монеты</param>
    [DirectMethod]
    public void PutCoin(int CoinValue)
    {
        if(Customer.Purse.IsCoinExists(CoinValue))
        {
            var Coins = Customer.Purse.Get(CoinValue);
            if (Coins.Length > 0)
            {
                Machine.PushCoin(Coins[0].CoinValue);
                BalanceContainer.Update(Machine.Balance);
                LogWrite(string.Format("Монета достоинством {0} брошена в автомат", CoinValue));
            }
        }
        else
            MessageBox(String.Format("Монеты достоинством {0} закончились", CoinValue));

        RefreshUi();
    }

    /// <summary>
    /// Покупка продукта
    /// </summary>
    /// <param name="ProductId">Id продукта</param>
    [DirectMethod]
    public void BuyProduct(int ProductId)
    {
        var result = Machine.BuyProduct(ProductId);
        if (result.Success)
        {
            LogWrite(String.Format("Куплен продукт {0}", Machine.GetProductTypeById(ProductId).Name));
            RefreshUi();
        }
        else
            MessageBox(result.Message);
    }

    /// <summary>
    /// Возврат сдачи
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void GetRemain(object sender, EventArgs e)
    {
        Money[] remain = new Money[]{};
        var result = Machine.GetRemain(out remain);
        if (result.Success)
        {
            LogWrite(Money.GetMoneyString(remain));
            LogWrite("Выдана сдача:");
            Customer.Purse.Push(remain);
            RefreshUi();
        }
        else
            MessageBox(result.Message);
    }

    /// <summary>
    /// Выводит выпадающее сообщение
    /// </summary>
    /// <param name="Message">Текст сообщения</param>
    void MessageBox(string Message)
    {
        new MessageBox().Show(new MessageBoxConfig 
        { 
            Title = "Сообщение", 
            Message = Message,
            Buttons = Ext.Net.MessageBox.Button.OK
        });
    }

    /// <summary>
    /// Освежает содержимое надписей
    /// </summary>
    void RefreshUi()
    {
        PanelPurse.Title = "Кошелек, " + Customer.Purse.Total;
        MachineMoneyDisplay.Title = "Деньги, " + Machine.Acceptor.Total;
        MachineMoneyDisplay.Update(Machine.Acceptor.GetMoneyString().Replace(Environment.NewLine, "<br />"));
        BalanceContainer.Update(Machine.Balance);
        X.Js.Call("UpdateButtons", new object[] { new {
                CustomerButtons = Customer.Purse.Coins.Select(x => new
                {
                    x.CoinValue,
                    x.Count
                }),
                MachineButtons = from t in Machine.ProductTypes
                                 from p in Machine.Products.Select(x => new { Id = x.Key, Count = x.Value}).Where(x => x.Id == t.Id)
                                 select new
                                 {
                                     t.Id,
                                     t.Name,
                                     t.Price,
                                     p.Count
                                 }
            }
        });
    }

    /// <summary>
    /// Добавляет строку в верхнюю часть текстового поля
    /// </summary>
    /// <param name="message"></param>
    void LogWrite(string message)
    {
        LogTextArea.Text = message + Environment.NewLine + LogTextArea.Text;
    }

    /// <summary>
    /// Задает экспериментальные данные
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
