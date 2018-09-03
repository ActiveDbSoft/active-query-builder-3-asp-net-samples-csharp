<%@ Page Title="SQL syntax highlighting Demo" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SQLSyntaxHighlighting.aspx.cs" Inherits="WebForms_Samples.Samples.SQLSyntaxHighlighting" %>
<%@ Register TagPrefix="AQB" Namespace="ActiveQueryBuilder.Web.WebForms" Assembly="ActiveQueryBuilder.Web.WebForms" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <h1>SQL Syntax Highlighting Demo</h1>
            <p>Highlight SQL syntax using a third-party SQL text editor.</p>
        </div>
        <div class="col-md-12">
            <!--Turn the UseDefaultTheme to False for not using the default theme. You will have to load the JQueryUI library then. -->
            <AQB:QueryBuilderControl ID="QueryBuilderControl1" runat="server" UseDefaultTheme="false" />
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
    
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/codemirror/5.26.0/codemirror.min.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/codemirror/5.26.0/codemirror.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/codemirror/5.26.0/mode/sql/sql.min.js"></script>
    <script>
        $(function () {
            AQB.Web.onQueryBuilderReady(function (qb) {
                var codeMirror = CodeMirror(document.body,
                    {
                        mode: 'text/x-sql',
                        indentWithTabs: true,
                        smartIndent: true,
                        lineNumbers: true,
                        matchBrackets: true
                    });

                qb.setEditor({
                    element: document.querySelector('.CodeMirror'),
                    getSql: function () {
                        return codeMirror.getValue();
                    },
                    setSql: function (sql) {
                        codeMirror.setValue(sql);
                    },
                    setCursorPosition: function (pos, col, line) {
                        this.focus();
                        codeMirror.setCursor(line - 1, col - 1, { scroll: true });
                    },
                    focus: function () {
                        codeMirror.focus();
                    },
                    onChange: function (callback) {
                        this.changeCallback = callback;
                        codeMirror.on('change', this.changeCallback);
                    },
                    remove: function () {
                        codeMirror.off('change', this.changeCallback);
                        this.element.remove();
                    }
                });
            });
        });
    </script>

</asp:Content>
