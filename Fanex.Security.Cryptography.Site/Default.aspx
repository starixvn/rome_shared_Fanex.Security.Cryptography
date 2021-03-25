<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Fanex.Cryptography.Aes.Site._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>ASP.NET</h1>
        <p class="lead">ASP.NET is a free web framework for building great Web sites and Web applications using HTML, CSS, and JavaScript.</p>
        <p><a href="http://www.asp.net" class="btn btn-primary btn-lg">Learn more &raquo;</a></p>
    </div>

    <div class="row">
        <div class="col-md-12">
            <h2>Encrypt/Decrypt by AES algorithm</h2>
            <p>
                Secret key: <input id="secret" type="text" value="1234" maxlength="4" /> (4 characters)
            </p>
            <p>
                Plain text:&nbsp;&nbsp;&nbsp;<input id="plainText" type="text" value="Your password" style="width: 400px" />
            </p>
            <p>
                Encrypted: <input id="encrypted" type="text" style="width: 800px" readonly />
            </p>
            <p>
                Decrypted: <input id="decrypted" type="text" style="width: 800px" readonly />
            </p>
        </div>
    </div>

</asp:Content>
