<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="HammerTime._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Assignment</h1>
        <p class="lead">
            Company "Hammer Time" creates various types of hammers. You have been hired by this company to develop an inventory tracking system.
            Your objectives are to create a .Net web application which interfaces with a SQL server database backend. Requirements for this project are that
            there must be an interface to view all hammers as well as an interface to make changes to the on-hand inventory of hammers including but not
            limited to creation of new types of hammers and edits to existing hammers.
            Please develop a Visual Studio solution/project that meets the objective. Please include all relevant code files for your project including SQL
            scripts for any objects you create.
        </p>
        <p><a runat="server" href="~/ViewInventory" class="btn btn-primary btn-lg">View and Manage Hammer Time Inventory &raquo;</a></p>
    </div>
</asp:Content>
