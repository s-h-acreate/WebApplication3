<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebApplication2.WebForm1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
   <link href="Data/StyleSheet1.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.9.2/dist/umd/popper.min.js" integrity="sha384-IQsoLXl5PILFhosVNubq5LC7Qb9DXgDA9i+tQ8Zj3iwWAwPtgFTxbJ8NT4GN1R8p" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.min.js" integrity="sha384-cVKIPhGWiC2Al4u+LWgxfKTRIcfu0JTxR+EQDz/bgldoEyl4H0zUF0QKbrJ0EcQF" crossorigin="anonymous"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">

            <div class="row height d-flex justify-content-center align-items-center">

                <div class="col-md-8">
                   
                    <div class="search">
                        <i class="fa fa-search"></i>
        
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods = "true">    
    </asp:ScriptManager>  
        <asp:TextBox ID="txtSearch" CssClass="form-control" placeholder="Search for Products here" runat="server"  ></asp:TextBox>   
                        <%--<asp:RequiredFieldValidator ID="txtSearchvalidate" runat="server" ControlToValidate="txtSearch" ErrorMessage="Choose any product." />--%>
                <asp:AutoCompleteExtender ServiceMethod="GetCompletionList" MinimumPrefixLength="1"    
                    CompletionInterval="4" EnableCaching="false" CompletionSetCount="4" TargetControlID="txtSearch"    
                    ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false">    
                </asp:AutoCompleteExtender> 
         <asp:Button ID="Button1" CssClass="btn " runat="server" OnClick="Button1_Click" Text="Search" />
                        </div>
                     <div class="newlabel">
                <asp:Label ID="showMessage" runat="server"  Visible="false"></asp:Label>
            </div>
                    </div>
                </div>
           
            <div class="frame">
            <iframe runat="server" id="iframepdf" height="700" width="1080" src="./Handler1.ashx" ></iframe>
            </div>
            </div>

       
    </form>
</body>
</html>