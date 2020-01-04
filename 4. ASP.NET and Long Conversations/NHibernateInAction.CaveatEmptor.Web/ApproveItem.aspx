<%@ Page EnableSessionState="True" Language="C#" AutoEventWireup="true" CodeFile="ApproveItem.aspx.cs" Inherits="ApproveItem" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>NHibernateInAction.CaveatEmptor</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="editItemName" runat="server"></asp:TextBox>
        <asp:Button ID="btnApprove" runat="server" OnClick="btnApprove_Click" Text="Approve" /></div>
    </form>
</body>
</html>
