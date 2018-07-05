let closeIcon = '<svg class="svg--delete" width="16px" height="16px" viewBox="0 0 16 16" enable-background="new 0 0 16 16" xml:space="preserve"><g><rect x="7.45" y="0.843" transform="matrix(-0.7071 -0.7071 0.7071 -0.7071 9.0859 18.8636)" fill="#FFFFFF" width="2" height="13.415"/><rect x="7.45" y="0.842" transform="matrix(0.7071 -0.7071 0.7071 0.7071 -2.8633 8.186)" fill="#FFFFFF" width="2" height="13.414"/></g></svg>';

let Mobile = {
    updateLayout: qb => {
        var lay = qb.element.next('.qb-ui-layout');

        if (lay.length === 0)
            return console.warn("It's not a standard layout");

        mobile(qb, lay);
    }
};

function mobile(qb, lay) {
    var left = lay.find('.qb-ui-layout__left');
    if (left.length) {
        if (qb.TreeViewComponent) {
            var treeViewParent = qb.TreeViewComponent.element.parent();
            treeViewParent.parent().click(() => showMobileComponents([treeViewParent, left]));
        }

        if (qb.UserQueriesComponent) {
            var userQueriesParent = qb.UserQueriesComponent.element.parent();
            userQueriesParent.parent().click(() => showMobileComponents([userQueriesParent, left]));
        }

        var closeLeft = createMobileCloseButton('left', qb._useDefaultTheme,
            () => hideMobileComponents([treeViewParent, userQueriesParent, left]));

        left.append(closeLeft);
    }

    var bottom = lay.find('.qb-ui-layout__right-bottom');
    if (bottom.length) {
        if (qb.GridComponent)
            subscribeComponentToTap(qb.GridComponent, bottom);

        if (qb.EditorComponent)
            subscribeComponentToTap(qb.EditorComponent, bottom);

        const closeGrid = createMobileCloseButton('bottom', qb._useDefaultTheme, () => hideMobileComponents([bottom]));
        bottom.append(closeGrid);
    }
}

function subscribeComponentToTap(component, bottom) {
    var parent = component.element.parent();
    parent.parent().click(() => showMobileComponents([bottom]));
}

function createMobileCloseButton(modif, useDefTheme, callback) {
    const cls = `ui-widget-header close-mobile-component close-mobile-component--${modif}`;

    if (useDefTheme)
        cls.addClass('qb-widget-header');

    return $(`<div class="${cls}"></div>`)
        .html(closeIcon)
        .click(callback);
}

const showMobileComponents = (function () {
    let last = undefined;

    return function(elements) {
        if (last != null) {
            for (let e of last)
                e.removeClass('active');
        }

        for (let e of elements)
            if (e)
                e.addClass('active');

        last = elements;
    };
}());

const hideMobileComponents = (function (elements) {
    for (let e of elements)
        if (e)
            e.removeClass('active');
});

window.AQB.Mobile = Mobile;