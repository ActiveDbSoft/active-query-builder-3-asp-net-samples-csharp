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
                            <AQB:Grid runat="server" ID="Grid1" UseCustomExpressionBuilder="ExpressionColumn" />
                        </div>
                    </div>
                    <div class="qb-ui-layout__bottom">
                        <AQB:SqlEditor runat="server" ID="SqlEditor1" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row" id="qr">
            <div class="col-md-12">
                <AQB:CriteriaBuilder runat="server" ID="CriteriaBuilder1" />
            </div>
            <div class="col-md-12">
                <div class="alert alert-danger"></div>
                <div id="second-tabs" class="block-flat">
                    <ul>
                        <li><a href="#jx">JqxGrid</a></li>
                        <li><a id="jsgrid-tab" href="#js">JsGrid</a></li>
                        <li><a href="#rg">ReactDataGrid</a></li>
                        <li><a href="#je">JsonEditor</a></li>
                        <li><a href="#ur">Your implementation</a></li>
                    </ul>
                    <div id="jx">
                        <a class="link-to-grid-site" href="https://www.jqwidgets.com">https://www.jqwidgets.com</a>
                        <div id="jqxgrid"></div>
                    </div>
                    <div id="js">
                        <a class="link-to-grid-site" href="http://js-grid.com">http://js-grid.com</a>
                        <div id="jsgrid"></div>
                    </div>
                    <div id="rg">
                        <a class="link-to-grid-site" href="http://adazzle.github.io/react-data-grid">http://adazzle.github.io/react-data-grid</a>
                        <div id="reactgrid"></div>
                    </div>
                    <div id="je">
                        <a class="link-to-grid-site" href="https://github.com/josdejong/jsoneditor">https://github.com/josdejong/jsoneditor</a>
                        <br />
                        <div id="jsoneditor"></div>
                        <button class="prev">Prev</button>
                        <span>Page:<span class="jsonPage"></span></span>
                        <button class="next">Next</button>
                    </div>
                    <div id="ur">
                        <span>Use this transformed query for your own implementation:</span>
                        <br />
                        <span class="sql"></span>
                    </div>
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

    <script>
        var dataUrl = "/QueryResultsDemo/GetData";

        $(function () {
            $('#main-tabs').tabs();
            $('#second-tabs').tabs();
            $('[href="#qr"]').click(onOpenQueryResults);
            $('.next').button().click(function () { fillJsonEditor(fillJsonEditor.page + 1); return false; });
            $('.prev').button().click(function () { fillJsonEditor(fillJsonEditor.page - 1); return false; });
            AQB.Web.onCriteriaBuilderReady(subscribeToChanges);
            AQB.Web.onQueryBuilderReady(createExpressionEditor);

            $('#jsgrid-tab').click(function() {
                $("#jsgrid").jsGrid('refresh');
            });
        });

        function onOpenQueryResults() {
            var cb = AQB.Web.CriteriaBuilderContainer.first();

            onCriteriaBuilderChanged(cb,
                function () {
                    var columns = cb.Columns.map(function (c) {
                        return {
                            key: c.ResultName,
                            name: c.ResultName,
                            text: c.ResultName,
                            datafield: c.ResultName
                        }
                    });

                    createGrids(columns);
                });
        };

        function subscribeToChanges(cb) {
            cb.loadColumns();

            cb.on(cb.Events.CriteriaBuilderChanged,
                function () {
                    onCriteriaBuilderChanged(cb, updateGrids);
                });
        }

        function onCriteriaBuilderChanged(cb, callback) {
            cb.transformSql(function (sql) {
                $('.sql').text(sql);
                callback();
            });
        }

        function createGrids(columns) {
            $('.alert-danger').hide();

            testQuery(function() {
                createJqxGrid(columns);
                createJsGrid(columns);
                createReactGrid(columns);
                fillJsonEditor(0);
            });
        }

        function updateGrids() {
            $('.alert-danger').hide();

            testQuery(function() {
                dataAdapter.dataBind();
                jsgrid.jsGrid();
                reactGrid.updateRows();
                fillJsonEditor(0);
            });
        }

        function testQuery(callback) {
            $.ajax({
                url: dataUrl,
                dataType: "json",
                data: {
                    pagenum: 0,
                    pagesize: 1
                },
                success: function(data) {
                    if (data.Error)
                        showError(data.Error);
                    else {
                        hideError();
                        callback();
                    }
                },
                error: function(xhr, error, text) {
                    showError(text);
                }
            });
        }

        function createJqxGrid(columns) {
            var source = {
                type: 'POST',
                contentType: 'application/json;',
                datatype: 'json',
                url: dataUrl,
                formatData: function (data) {
                    data.params = getParams();
                    return JSON.stringify(data);
                },
                sort: function () {
                    $("#jqxgrid").jqxGrid('updatebounddata');
                },
                datafields: columns.map(function (c) {
                    return { name: c.Name }
                }),
                totalrecords: 9999999
            };

            window.dataAdapter = new $.jqx.dataAdapter(source);

            $("#jqxgrid").jqxGrid({
                width: '100%',
                source: dataAdapter,
                pageable: true,
                sortable: true,
                virtualmode: true,
                rendergridrows: function () {
                    return dataAdapter.loadedData;
                },
                columns: columns
            });
        }

        function createReactGrid(columns) {
            ReactDOM.unmountComponentAtNode(document.getElementById('reactgrid'));

            getData(init, 0);

            function getData(callback, pageNum, sortField, sortOrder) {
                $.ajax({
                    url: dataUrl,
                    dataType: "json",
                    data: {
                        pagenum: pageNum,
                        pagesize: 10,
                        sortdatafield: sortField,
                        sortorder: sortOrder
                    },
                    success: callback
                });
            }

            function init(data) {
                var Grid = React.createClass({
                    getInitialState() {
                        this._columns = columns.map(function (c) {
                            c.sortable = true;
                            c.width = 300;
                            return c;
                        });

                        return { rows: data, page: 0 };
                    },

                    sort(field, order) {
                        getData(function (data) {
                            this.setState({ rows: data });
                        }.bind(this),
                            this.state.page,
                            field,
                            order !== 'NONE' ? order : undefined);

                        this.setState({ field: field, order: order });
                    },

                    page(page) {
                        getData(function (data) {
                            this.setState({ rows: data });
                        }.bind(this),
                            page,
                            this.state.field,
                            this.state.order !== 'NONE' ? this.state.order : undefined);

                        this.setState({ page: page });
                    },

                    updateRows() {
                        getData(function (data) {
                            this.setState({ rows: data });
                        }.bind(this),
                            this.state.page,
                            this.state.field,
                            this.state.order !== 'NONE' ? this.state.order : undefined);
                    },

                    prevPage() {
                        this.page(this.state.page - 1);
                    },

                    nextPage() {
                        this.page(this.state.page + 1);
                    },

                    rowGetter(i) {
                        return this.state.rows[i];
                    },

                    render() {
                        return React.createElement('div',
                            null,
                            [
                                React.createElement(ReactDataGrid,
                                    {
                                        onGridSort: this.sort,
                                        columns: this._columns,
                                        rowGetter: this.rowGetter,
                                        rowsCount: this.state.rows.length,
                                        minHeight: 500
                                    }),
                                React.createElement('span', { onClick: this.prevPage }, ['prev ']),
                                React.createElement('span', { onClick: this.nextPage }, [' next'])
                            ]);
                    }
                });

                var gridElem = React.createElement(Grid);
                window.reactGrid = ReactDOM.render(gridElem, document.getElementById('reactgrid'));
            }
        }

        function createJsGrid(columns) {
            window.jsgrid = $("#jsgrid").jsGrid({
                width: "100%",
                height: "400px",
                sorting: true,
                paging: true,
                pageLoading: true,
                pageSize: 10,
                autoload: true,
                fields: columns,
                controller: {
                    loadData: function (filter) {
                        var d = $.Deferred();

                        $.ajax({
                            url: dataUrl,
                            dataType: "json",
                            data: {
                                pagenum: filter.pageIndex - 1,
                                pagesize: filter.pageSize,
                                sortdatafield: filter.sortField,
                                sortorder: filter.sortOrder
                            }
                        }).done(function (res) {
                            d.resolve({
                                data: res,
                                itemsCount: 9999999
                            });
                        });

                        return d.promise();
                    }
                }
            });

            $('jsgrid-header-cell').click(function () {
                var field = this.innerText;
                $("#jsgrid").jsGrid("sort", field);
            });
        }

        function fillJsonEditor(page) {
            if (page < 0)
                return;

            fillJsonEditor.page = page;

            $('.jsonPage').text(page);

            if (!fillJsonEditor.editor) {
                var container = document.getElementById('jsoneditor');
                fillJsonEditor.editor = new JSONEditor(container, { mode: 'code' });
            }

            $.ajax({
                data: {
                    pagenum: page,
                    pagesize: 10
                },
                url: dataUrl,
                success: function (data) {
                    data = JSON.parse(data);
                    fillJsonEditor.editor.set(data);
                }
            });

        }

        function createExpressionEditor(qb) {
            window.codeMirror = CodeMirror(document.body, {
                mode: 'text/x-sql',
                indentWithTabs: true,
                smartIndent: true,
                lineNumbers: true,
                matchBrackets: true,
                width: '500px',
                height: '250px'
            });

            $('.CodeMirror').hide();

            qb.GridComponent.on(AQB.Web.QueryBuilder.GridComponent.Events.GridBeforeCustomEditCell, BeforeCustomEditCell);
        }

        function BeforeCustomEditCell(data) {
            var row = data.row;
            var cell = data.cell;

            var error = $('<p class="ui-state-error" style="display: none;"></div>');

            var $dialog = $('<div>').dialog({
                modal: true,
                width: 'auto',
                title: 'Custom expression editor',
                buttons: [{
                    text: "OK",
                    click: function () {
                        var newValue = codeMirror.getValue();

                        var ifValid = function () {
                            cell.updateValue(newValue);
                            $dialog.dialog("close");
                        }

                        var ifNotValid = function (message) {
                            error.html(message).show();
                        }

                        validate(newValue, ifValid, ifNotValid);
                    }
                }, {
                    text: "Cancel",
                    click: function () {
                        $dialog.dialog("close");
                    }
                }
                ]
            });

            $dialog.append(error, $('.CodeMirror').show());
            $dialog.parent().css({
                top: '25%',
                left: '30%',
                width: 600
            });

            codeMirror.setValue(row.FormattedExpression || '');
            codeMirror.refresh();
        };
        
        function validate(expression, ifValid, ifNotValid) {
            AQB.Web.QueryBuilder.validateExpression(expression, function (isValid, message) {
                if (isValid)
                    ifValid();
                else
                    ifNotValid(message);
            });
        }

        function showError(statusText) {
            $('.alert-danger').show().text(statusText);
            $("#second-tabs").hide();
        }

        function hideError() {
            $('.alert-danger').hide();
            $("#second-tabs").show();
        }

        function getParams() {
            var result = [];
            var params = getUniqueQueryParams();

            for (var i = 0; i < params.length; i++) {
                result.push({
                    Name: params[i].FullName,
                    Value: $('input.' + params[i].Name).val()
                });
            }

            return result;
        }

        function getUniqueQueryParams() {
            var params = AQB.Web.QueryBuilder.queryParams;
            var result = [];

            for (var i = 0, l = params.length; i < l; i++) {
                var param = params[i];

                if (result.find(r => r.FullName === param.FullName) == null)
                    result.push(param);
            }

            return result;
        }
    </script>
</asp:Content>
