<%@ Page Title="Bootstrap Theming Demo" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BootstrapUITheming.aspx.cs" Inherits="WebForms_Samples.Samples.BootstrapTheming" %>
<%@ Register TagPrefix="AQB" Namespace="ActiveQueryBuilder.Web.WebForms" Assembly="ActiveQueryBuilder.Web.WebForms" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <h1>Bootstrap Theming Demo</h1>
            <p>
                <span>Apply any Bootstrap theme to Active Query Builder UI:</span>
                <span class="btn btn-default">Simplex</span>
                <span class="btn btn-default">Cerulean</span>
                <span class="btn btn-default">Cosmo</span>
                <span class="btn btn-default">Flatly</span>
                <span class="btn btn-default">Superhero</span>
            </p>
        </div>
        <div class="col-md-12">
            <AQB:QueryBuilderControl ID="QueryBuilderControl1" runat="server" Theme="bootstrap" />
            <div class="qb-ui-layout">
                <div class="qb-ui-layout__top">
                    <div class="qb-ui-layout__left">
                        <div class="qb-ui-structure-tabs">
                            <div class="qb-ui-structure-tabs__tab">
                                <input type="radio" id="tree-tab" name="qb-tabs" checked />
                                <label for="tree-tab">Database</label>
                                <div class="qb-ui-structure-tabs__content">
                                    <AQB:ObjectTreeView runat="server" ID="ObjectTreeView1" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="qb-ui-layout__right">
                        <AQB:SubQueryNavigationBar runat="server" ID="SubQueryNavigationBar1" />
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
    
    <script>
        var last;

        $('.btn.btn-default').click(function () {
            if (last)
                last.remove();

            var url = 'https://bootswatch.com/3/' + this.innerText.toLowerCase() + '/bootstrap.min.css';
            last = $('<link rel="stylesheet" type="text/css" href="' + url + '">');
            $('head').append(last);
        });
    </script>
</asp:Content>
