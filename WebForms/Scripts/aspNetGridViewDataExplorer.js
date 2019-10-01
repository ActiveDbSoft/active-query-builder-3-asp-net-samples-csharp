function AspNetGridViewDataExplorer() {
    DataExplorer.apply(this, arguments);
    this._setLinkToDataExplorerSite('https://docs.microsoft.com/en-us/dotnet/api/system.web.ui.webcontrols.gridview?view=netframework-4.8');
}

AspNetGridViewDataExplorer.prototype = Object.create(DataExplorer.prototype);
AspNetGridViewDataExplorer.prototype.constructor = AspNetGridViewDataExplorer;

AspNetGridViewDataExplorer.prototype.create = function () {
    this.isInit = true;
    $('#MainContent_Button1').click();
};

AspNetGridViewDataExplorer.prototype.update = function () {
    $('#MainContent_Button1').click();
};

AspNetGridViewDataExplorer.prototype.clear = function() {

};

AspNetGridViewDataExplorer.prototype._updateRecordsCount = function (count) {

};