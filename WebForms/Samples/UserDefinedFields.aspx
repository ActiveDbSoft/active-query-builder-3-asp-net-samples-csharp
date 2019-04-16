<%@ Page Title="User-defined fields" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserDefinedFields.aspx.cs" Inherits="WebForms_Samples.Samples.UserDefinedFields" %>
<%@ Register TagPrefix="AQB" Namespace="ActiveQueryBuilder.Web.WebForms" Assembly="ActiveQueryBuilder.Web.WebForms" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <h1>User-defined Fields Demo</h1>
            <p>Let advanced users create own calculated fields.</p>
            <div class="block-flat">
                The datasource context menu contains items to add user-defined fields. User acts with such fields the same ways as ordinary fields, but the component expands them to SQL expressions in the result SQL query text.
            </div>
        </div>
        <div class="col-md-12">
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
                <div class="qb-ui-layout__bottom">
                    <AQB:SqlEditor runat="server" ID="SqlEditor1" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
