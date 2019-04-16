<%@ Page Title="Query Modification Demo" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="QueryModificationDemo.aspx.cs" Inherits="WebForms_Samples.Samples.QueryModificationDemo" %>

<%@ Register TagPrefix="AQB" Namespace="ActiveQueryBuilder.Web.WebForms" Assembly="ActiveQueryBuilder.Web.WebForms" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <h1>Query Modification Demo</h1>
            <p>Modify SQL queries programmatically.</p>
        </div>
        <div class="col-md-12">
            <p>
                This sample project demonstrates ability to analyze SQL queries and modify them programmatically. This could be helpful if you want to correct erroneous queries before execution or to restrict manipulations with specific data.
                This demo allows you to load several sample queries and see how they will be changed.
            </p>
        </div>
        <div class="col-md-12">
            <div id="main-tabs" class="block-flat">
                <ul>
                    <li><a href="#sql">SQL Text</a></li>
                    <li><a href="#qb">Query Builder</a></li>
                </ul>
                <div id="sql">
                    <AQB:SqlEditor runat="server" ID="SqlEditor1" />
                </div>
                <div id="qb">
                    <AQB:QueryBuilderControl ID="QueryBuilderControl1" runat="server" />
                    <div class="qb-ui-layout">
                        <div class="qb-ui-layout__top">
                            <div class="qb-ui-layout__left">
                                <div class="qb-ui-structure-tabs">
                                    <div class="qb-ui-structure-tabs__tab">
                                        <input type="radio" id="tree-tab" name="qb-tabs" checked />
                                        <label for="tree-tab">Database</label>
                                        <div class="qb-ui-structure-tabs__content">
                                            <AQB:ObjectTreeView ID="ObjectTreeView1" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="qb-ui-layout__right">
                                <AQB:SubQueryNavigationBar ID="SubQueryNavigationBar1" runat="server" />
                                <AQB:Canvas runat="server" ID="Canvas1" />
                                <AQB:StatusBar runat="server" ID="StatusBar1" />
                                <AQB:Grid runat="server" ID="Grid1" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-10">
            <h3>Modification Settings:</h3>
            <span>Check tables you wish to add to the query.</span>
            <br />
            <span>Check fields you wish to define criteria for.</span>
            <br />
            <ASP:CheckBox runat="server" AutoPostBack="True" Text="Customers" ID="cbCustomers" />
            <br />
            <ASP:CheckBox runat="server" AutoPostBack="True" Text="CompanyName" ID="cbCompanyName" CssClass="marginLeft" />
            <ASP:TextBox runat="server" ID="tbCompanyName" Text="Like 'C%'"></ASP:TextBox>
            <br />
            <ASP:CheckBox runat="server" AutoPostBack="True" Text="Orders" ID="cbOrders" />
            <br />
            <ASP:CheckBox runat="server" AutoPostBack="True" Text="OrderDate" ID="cbOrderDate" CssClass="marginLeft" />
            <ASP:TextBox runat="server" ID="tbOrderDate" Text="= '01/01/2007'"></ASP:TextBox>
        </div>
        <div class="col-md-2">
            <div class="marginTop">
                <asp:Button runat="server" Text="Load Sample Query 1" OnClick="btnQueryCustomers_Click" CssClass="btn btn-primary" />
                <asp:Button runat="server" Text="Load Sample Query 2" OnClick="btnQueryOrders_Click" CssClass="btn btn-primary" />
                <asp:Button runat="server" Text="Load Sample Query 3" OnClick="btnQueryCustomersOrders_Click" CssClass="btn btn-primary" />
                <br />
                <asp:Button runat="server" Text="Apply Changes" OnClick="btnApply_Click" CssClass="marginTop btn btn-primary" />
            </div>
        </div>
    </div>
    
    <script src="https://code.jquery.com/ui/1.12.0/jquery-ui.min.js"></script>
    <script>
        $(function() {
            $('#main-tabs').tabs();
        });
    </script>
    <style>
        .marginTop {
            margin-top: 20px;    
        }
        .marginLeft {
            margin-left: 10px;
        }

        .btn {
            width: 160px;
        }
    </style>
</asp:Content>
