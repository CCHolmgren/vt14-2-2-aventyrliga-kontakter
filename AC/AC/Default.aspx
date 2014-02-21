<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AC.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox runat="server" ID="Textbox" Height="800" Width="800" TextMode="MultiLine" />
    </div>
        <div>
            <asp:ListView ID="ListView1" runat="server" 
                ItemType="AC.Model.Contact" 
                SelectMethod="ListView1_GetData"
                InsertMethod="ListView1_InsertItem"
                UpdateMethod="ListView1_UpdateItem"
                DeleteMethod="ListView1_DeleteItem">
                <LayoutTemplate>
                    <table>
                        <tr>
                            <td></td>
                        </tr>
                    </table>
                    <asp:PlaceHolder runat="server" ID="itemPlaceholder"/>
                    <asp:DataPager PagedControlID="ListView1" runat="server" QueryStringField="page" PageSize="20">
                        <Fields>
                            <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="true" ShowNextPageButton="false" ShowPreviousPageButton="false"/>
                            <asp:NumericPagerField />
                            <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="true" ShowNextPageButton="false" ShowPreviousPageButton="false"/>
                            <asp:TemplatePagerField>
                                <PagerTemplate>
                                    <asp:Label Text="<%# Container.StartRowIndex %>" runat="server" />
                                    <asp:Label Text="<%# Container.StartRowIndex+Container.PageSize %>" runat="server"/>
                                    <asp:Label Text="<%# Container.TotalRowCount %>" runat="server" />
                                </PagerTemplate>
                            </asp:TemplatePagerField>
                        </Fields>
                    </asp:DataPager>
                </LayoutTemplate>
                <ItemTemplate>
                    <li runat="server">
                        <p><%# Item.FirstName %></p>
                    </li>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </form>
</body>
</html>
