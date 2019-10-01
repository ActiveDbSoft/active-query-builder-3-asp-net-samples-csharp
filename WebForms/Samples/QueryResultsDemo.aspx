<%@ Page Title="Query Results Demo" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="QueryResultsDemo.aspx.cs" Inherits="WebForms_Samples.Samples.QueryResultsDemo" %>
<%@ Register TagPrefix="AQB" Namespace="ActiveQueryBuilder.Web.WebForms" Assembly="ActiveQueryBuilder.Web.WebForms" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <h1>Display Query Results Demo</h1>
            <p>Displaying SQL query results and modifying SQL queries while browsing the data.</p>
        </div>
    </div>
    <div id="main-tabs" class="block-flat">
        <ul>
            <li><a href="#qb">Query Builder</a></li>
            <li><a href="#qr">Query Results</a></li>
        </ul>
        <div class="row" id="qb">
            <div class="col-md-12">
                <AQB:QueryBuilderControl ID="QueryBuilderControl1" Theme="jqueryui" runat="server" Language="en" />
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
                            <AQB:Grid runat="server" ID="Grid1" UseCustomExpressionBuilder="AllColumns" />
                        </div>
                    </div>
                    <div class="qb-ui-layout__bottom">
                        <AQB:SqlEditor runat="server" ID="SqlEditor1" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row" id="qr">
            <div class="col-md-3">
                <div class="table-params"></div>
            </div>
            <div class="col-md-2">
                <span class="btn btn-primary execute">Apply</span>
            </div>
            <div class="col-md-12">
                <AQB:CriteriaBuilder runat="server" ID="CriteriaBuilder1" />
            </div>
            <div class="col-md-12">
                <div class="alert alert-danger"></div>
                <div id="dataExplorerContainer" class="block-flat">
                    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="Button1" runat="server" Text="Обновить" OnClick="UpdateDataGrid" />

                            <asp:Button runat="server" Text="Prev" OnClick="Prev_OnClick" />
                            <asp:Label runat="server" ID="aspPage" />
                            <asp:Button runat="server" Text="Next" OnClick="Next_OnClick" />
                            <asp:Label runat="server" ID="recordsCount"></asp:Label>

                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="True" CssClass="table"></asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <!--div id="ur">
                    <span>Use this transformed query for your own implementation:</span>
                    <br />
                    <span class="sql"></span>
                    </div-->
                </div>
            </div>
        </div>
    </div>

    <link type="text/css" rel="stylesheet" href="https://jqwidgets.com/public/jqwidgets/styles/jqx.base.css" />
    <link type="text/css" rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/jsgrid/1.5.3/jsgrid.min.css" />
    <link type="text/css" rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/jsgrid/1.5.3/jsgrid-theme.min.css" />
    <link type="text/css" rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/jsoneditor/5.11.0/jsoneditor.min.css" />
    <link type="text/css" rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/codemirror/5.26.0/codemirror.min.css" />

    <style>
        #qb, #qr {
            padding: 5px;
        }

        #jsoneditor {
            height: 500px;
        }

        .link-to-grid-site {
            float: right;
            color: blue !important;
        }

        .jsonPage {
            margin-left: 5px;
            font-weight: bold;
        }

        .execute, .alert-danger {
            display: none;
        }

        .recordsCount {
            float: right;
        }

        #MainContent_Button1 {
            display: none;
        }
    </style>

    <script src="https://code.jquery.com/ui/1.12.0/jquery-ui.min.js"></script>
    <script src="https://jqwidgets.com/public/jqwidgets/jqx-all.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/react/0.14.1/react.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/react/0.14.1/react-dom.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/react-data-grid/2.0.78/react-data-grid.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jsgrid/1.5.3/jsgrid.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jsoneditor/5.11.0/jsoneditor.min.js"></script>

    <script src="https://cdnjs.cloudflare.com/ajax/libs/codemirror/5.26.0/codemirror.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/codemirror/5.26.0/mode/sql/sql.min.js"></script>

    <script src="../Scripts/queryResults.js"></script>
    <script src="../Scripts/paramsController.js"></script>
    <script src="../Scripts/dataExplorer.js"></script>
    <script src="../Scripts/jqxGridDataExplorer.js"></script>
    <script src="../Scripts/jsGridDataExplorer.js"></script>
    <script src="../Scripts/reactGridDataExplorer.js"></script>
    <script src="../Scripts/jsonDataExplorer.js"></script>
    <script src="../Scripts/aspNetGridViewDataExplorer.js"></script>

    <script>
        var container = $('#dataExplorerContainer');
        var dataUrl = "/GetData";
        var recordsCountUrl = "/SelectRecordsCount";

        //var dataExplorer = new JqxGridDataExplorer(container, dataUrl, recordsCountUrl);
        // var dataExplorer = new JsGridDataExplorer(container, dataUrl, recordsCountUrl);
        // var dataExplorer = new ReactGridDataExplorer(container, dataUrl, recordsCountUrl);
        // var dataExplorer = new JsonDataExplorer(container, dataUrl, recordsCountUrl);
        var dataExplorer = new AspNetGridViewDataExplorer(container, dataUrl, recordsCountUrl);

        QueryResultsDemo(dataExplorer);
    </script>
</asp:Content>
