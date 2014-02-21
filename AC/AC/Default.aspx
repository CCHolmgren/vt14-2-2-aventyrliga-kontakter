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
            <asp:ListView runat="server">
                <ItemTemplate></ItemTemplate>
            </asp:ListView>
        </div>
    </form>
</body>
</html>
