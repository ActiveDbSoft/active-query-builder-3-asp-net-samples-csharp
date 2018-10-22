const icons = {
    bottom: '<svg class="svg--chevron" width="16px" height="16px" viewBox="0 0 26 26" enable-background="new 0 0 16 16" xml:space="preserve"><g><polygon fill="#ffffff" points="0.046,2.582 2.13,0.498 12.967,11.334 23.803,0.498 25.887,2.582 12.967,15.502  "/><polygon fill="#ffffff" points="0.046,13.582 2.13,11.498 12.967,22.334 23.803,11.498 25.887,13.582 12.967,26.502  "/></g></svg>',
    left: '<svg class="svg--chevron" width="26px" height="26px" viewBox="0 0 26 26" enable-background="new 0 0 16 16" xml:space="preserve"><g><polygon fill="#ffffff" points="23.885,0.58 25.969,2.664 15.133,13.5 25.969,24.336 23.885,26.42 10.965,13.5  "/><polygon fill="#ffffff" points="12.885,0.58 14.969,2.664 4.133,13.5 14.969,24.336 12.885,26.42 -0.035,13.5  "/></g></svg>'
};

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
        .html(icons[modif])
        .click(callback);
}

const showMobileComponents = (function () {
    let last = undefined;

    return function(elements) {
        if (last != null) {
            for (let e of last)
                e.removeClass('active');
        }

        for (let e of elements) {
            if (e) {
                e.addClass('active');
                e.find('label').hide();
            }
        }

        last = elements;
    };
}());

const hideMobileComponents = (function (elements) {
    for (let e of elements) {
        if (e) {
            e.removeClass('active');
            e.find('label').show();
        }
    }
});

window.AQB.Mobile = Mobile;