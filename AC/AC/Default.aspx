﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AC.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <h1>Äventyrliga kontakter</h1>
    <form id="form1" runat="server">
        <div>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" HeaderText="Ett oväntat fel har uppståt. Åtgärda det och försök igen." />
            <asp:Button Text="Ny kontakt" ID="NewContactButton" OnClick="NewContactButton_Click" runat="server" />
            <asp:ListView ID="ListView1" runat="server" 
                ItemType="AC.Model.Contact" 
                SelectMethod="ListView1_GetData"
                InsertMethod="ListView1_InsertItem"
                UpdateMethod="ListView1_UpdateItem"
                DeleteMethod="ListView1_DeleteItem" 
                InsertItemPosition="FirstItem"
                DataKeyNames="contactId" ViewStateMode="Enabled">
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
                    <asp:DataPager PagedControlID="ListView1" ID="DataPager" runat="server" QueryStringField="page" PageSize="20">
                        <Fields>
                            <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="true" ShowNextPageButton="false" ShowPreviousPageButton="true"/>
                            <asp:NumericPagerField />
                            <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="true" ShowNextPageButton="true" ShowPreviousPageButton="false"/>
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
                        <asp:TextBox runat="server" ID="FirstNameEdit" Text="<%# BindItem.FirstName %>" MaxLength="50" />
                        <asp:RequiredFieldValidator ErrorMessage="Du måste fylla i ett förnamn." ControlToValidate="FirstNameEdit" runat="server" Display="None" />
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="LastNameEdit" Text="<%# BindItem.LastName %>" MaxLength="50" />  
                        <asp:RequiredFieldValidator ErrorMessage="Du måste fylla i ett efternamn." ControlToValidate="LastNameEdit" runat="server" Display="None" />
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="EmailAddressEdit" Text="<%# BindItem.EmailAddress %>" MaxLength="50" />
                        <asp:RequiredFieldValidator ErrorMessage="Du måste fylla i en email-address." ControlToValidate="EmailAddressEdit" runat="server" Display="None" />
                        <asp:RegularExpressionValidator 
                            ErrorMessage="Fyll i en giltig email-address"
                            ControlToValidate="EmailAddressEdit" 
                            ID="EmailAddressEditValidator" 
                            ValidationExpression="^.+@.+$" 
                            runat="server" Display="None" />  
                    </td>
                    <td>
                        <asp:LinkButton CommandName="Update" runat="server" Text="Spara" />  
                    </td>
                    <td>
                        <asp:LinkButton CommandName="Cancel" CausesValidation="false" runat="server" Text="Avbryt"/>  
                    </td>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <tr>
                        <td>
                            <asp:TextBox runat="server" ID="FirstNameInsert" Text="<%# BindItem.FirstName %>" MaxLength="50" />
                            <asp:RequiredFieldValidator ErrorMessage="Du måste fylla i ett förnamn." ControlToValidate="FirstNameInsert" runat="server" Display="None"  />
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="LastNameInsert" Text="<%# BindItem.LastName %>" MaxLength="50" />
                            <asp:RequiredFieldValidator ErrorMessage="Du måste fylla i ett efternamn." ControlToValidate="LastNameInsert" runat="server" Display="None"  />
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="EmailAddressInsert" Text="<%# BindItem.EmailAddress %>" MaxLength="50" />
                            <asp:RequiredFieldValidator ErrorMessage="Du måste fylla i en email-address." ControlToValidate="EmailAddressInsert" runat="server" Display="None"  />
                            <asp:RegularExpressionValidator 
                                ErrorMessage="Fyll i en giltig email-address."
                                ControlToValidate="EmailAddressInsert" 
                                ID="EmailAddressInsertValidator" 
                                ValidationExpression=".+@.+" 
                                runat="server" Display="None" />  
                        </td>
                        <td>
                            <asp:LinkButton Text="Lägg till" CommandName="Insert"  runat="server" />
                        </td>
                        <td>
                            <asp:LinkButton Text="Rensa" CausesValidation="false" CommandName="Cancel" runat="server" />
                        </td>
                    </tr>
                </InsertItemTemplate>
            </asp:ListView>
        </div>
    </form>
</body>
</html>
