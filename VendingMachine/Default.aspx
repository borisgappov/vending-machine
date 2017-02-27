<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    <script type="text/javascript">
        function Reload() {
            document.location.reload();
        }
        function UpdateButtons(data) {
            for (i in data.CustomerButtons) {
                Ext.getCmp('ButtonPut_' + data.CustomerButtons[i].CoinValue)
                    .setText(Ext.String.format('{0} x {1}  в автомат >>',
                                data.CustomerButtons[i].CoinValue,
                                data.CustomerButtons[i].Count));
            }
            for (i in data.MachineButtons) {
                Ext.getCmp('ButtonBuy_' + data.MachineButtons[i].Id)
                    .setText(Ext.String.format('купить {0}, в наличии {1} шт, цена {2}',
                                data.MachineButtons[i].Name,
                                data.MachineButtons[i].Count,
                                data.MachineButtons[i].Price));
            }
            
        }
    </script>
</head>
<body style="background-color: lightgray;">
    <ext:ResourceManager runat="server" Theme="Default" />
    <form id="form1" runat="server">
        <ext:Viewport runat="server" Layout="VBoxLayout">
            <LayoutConfig>
                <ext:VBoxLayoutConfig Align="Center" Pack="Center" />
            </LayoutConfig>
            <Items>
                <ext:Toolbar runat="server" Height="28" Width="450" Layout="HBoxLayout">
                    <Items>
                        <ext:Button runat="server" Text="Как в задании" Handler="App.direct.Reset(1)" ></ext:Button>
                        <ext:ToolbarSeparator />
                        <ext:Button runat="server" Text="Деньги на исходе" Handler="App.direct.Reset(2)" ></ext:Button>
                        <ext:ToolbarSeparator />
                        <ext:Button runat="server" Text="Экзотические деньги" Handler="App.direct.Reset(3)" ></ext:Button>
                        <ext:ToolbarSeparator />
                        <ext:HyperLink runat="server" Icon="PageWhiteZip" Text="Скачать" NavigateUrl="sources.zip" ></ext:HyperLink>
                    </Items>
                </ext:Toolbar>
                <ext:Container runat="server" Layout="HBoxLayout" MarginSpec="5 0 0 0">
                    <LayoutConfig>
                        <ext:HBoxLayoutConfig Align="Middle" Pack="Center" />
                    </LayoutConfig>
                    <Items>
                        <ext:Panel runat="server" Title="Покупатель" Height="350" Width="150" BodyPadding="5">
                            <Items>
                                <ext:Panel runat="server" ID="PanelPurse" Title="Кошелек" Height="140" Layout="VBoxLayout" BodyPaddingSummary="0 0 5 0">
                                    <LayoutConfig>
                                        <ext:VBoxLayoutConfig Align="Stretch" DefaultMargins="5 5 0 5"  />
                                    </LayoutConfig>
                                </ext:Panel>
                            </Items>
                        </ext:Panel>
                        <ext:Panel runat="server" Title="Торговый автомат" Height="350" Width="295" MarginSpec="0 0 0 5" Layout="VBoxLayout" BodyPadding="5">
                            <LayoutConfig>
                                <ext:VBoxLayoutConfig Align="Stretch" />
                            </LayoutConfig>
                            <Items>
                                <ext:Panel runat="server" Title="Баланс" Height="60" Layout="HBoxLayout" BodyPadding="5">
                                    <LayoutConfig>
                                        <ext:HBoxLayoutConfig Align="Stretch" />
                                    </LayoutConfig>
                                    <Items>
                                        <ext:Container runat="server" ID="BalanceContainer" Flex="1"></ext:Container>
                                        <ext:Button runat="server" Text="Забрать сдачу" Width="100" ToolTip="Забрать сдачу">
                                            <DirectEvents>
                                                <Click OnEvent="GetRemain" />
                                            </DirectEvents>
                                        </ext:Button>
                                    </Items>
                                </ext:Panel>
                                <ext:Panel runat="server" ID="PanelProducts" Title="Продукты" Height="150" Layout="VBoxLayout" MarginSpec="5 0 0 0" BodyPaddingSummary="0 0 5 0">
                                    <LayoutConfig>
                                        <ext:VBoxLayoutConfig Align="Stretch" DefaultMargins="5 5 0 5" />
                                    </LayoutConfig>
                                </ext:Panel>
                                <ext:Panel runat="server" ID="MachineMoneyDisplay" Title="Деньги" Flex="1" MarginSpec="5 0 0 0" BodyPadding="5">
                                </ext:Panel>
                            </Items>
                        </ext:Panel>
                    </Items>
                </ext:Container>
                <ext:Panel runat="server" Width="450" Title="Лог операций" Height="200" Layout="FitLayout" MarginSpec="5 0 0 0">
                    <Items>
                        <ext:TextArea runat="server" ID="LogTextArea" StyleSpec="border:none;" Border="false" MarginSpec="-1 0 0 -1" ReadOnly="true" />
                    </Items>
                </ext:Panel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>
