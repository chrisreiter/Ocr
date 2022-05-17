<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="Sample.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Nicomsoft OCR</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="Label1" runat="server" 
            Text="Nicomsoft OCR ASP.NET C# Minimal Sample" Font-Bold="True" ForeColor="Blue"></asp:Label><br /><br /> 
        <asp:Label ID="Label2" runat="server" Text="Select file for OCR"></asp:Label><br /><br /> 
        <asp:FileUpload ID="FileUpload1" runat="server" Width="546px" /><br /><br />
        <asp:Button ID="Button1" runat="server" Text="OCR" onclick="Button1_Click" /><br /><br /> <br /><br /> 

   <asp:Label ID="Label3" runat="server" Text="Recognized text:"></asp:Label><br /><br /> 
   <asp:Label ID="Label4" runat="server" Text="-----" Visible="False"></asp:Label><br /><br /> 
   
    </div>

    </form>
</body>
</html>
