<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AC.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ListView ID="ListView1" runat="server" 
                ItemType="AC.Model.Contact" 
                SelectMethod="ListView1_GetData"
                InsertMethod="ListView1_InsertItem"
                UpdateMethod="ListView1_UpdateItem"
                DeleteMethod="ListView1_DeleteItem" InsertItemPosition="FirstItem">
                <LayoutTemplate>
                    <table>
                        <tr>
                            <th>Förnamn</th>
                            <th>Efternamn</th>
                            <th>E-post</th>
                            <th></th>
                            <th></th>
                        </tr>
                    <asp:PlaceHolder runat="server" ID="itemPlaceholder"/>
                    </table>
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
                    <tr>
                        <td>
                            <asp:Label Text="<%# Item.FirstName %>" runat="server" />
                        </td>
                        <td>
                            <asp:Label Text="<%# Item.LastName %>" runat="server" />
                        </td>
                        <td>
                            <asp:Label Text="<%# Item.EmailAddress %>" runat="server" />
                        </td>
                        <td>
                            <asp:LinkButton Text="Redigera" CommandName="Edit" runat="server" />
                        </td>
                        <td>
                            <asp:LinkButton Text="Ta bort" CommandName="Delete" runat="server" />
                        </td>
                    </tr>
                </ItemTemplate>
                <EditItemTemplate>
                    <td>
                        <asp:TextBox runat="server" ID="FirstName" Text="<%# BindItem.FirstName %>"/>  
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="LastName" Text="<%# BindItem.LastName %>"/>  
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="EmailAddress" Text="<%# BindItem.EmailAddress %>"/>  
                    </td>
                    <td>
                        <asp:LinkButton CommandName="Update" runat="server" Text="Spara" />  
                    </td>
                    <td>
                        <asp:LinkButton CommandName="Cancel" runat="server" Text="Avbryt"/>  
                    </td>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <tr>
                        <td>
                            <asp:TextBox runat="server" ID="FirstName"/>  
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="LastName"/>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="EmailAddress"/>
                        </td>
                        <td>
                            <asp:LinkButton Text="Lägg till" CommandName="Insert" runat="server" />
                        </td>
                        <td>
                            <asp:LinkButton Text="Rensa" CommandName="Cancel" runat="server" />
                        </td>
                    </tr>
                </InsertItemTemplate>
            </asp:ListView>
        </div>
    </form>
</body>
</html>
