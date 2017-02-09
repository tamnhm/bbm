tinymce.PluginManager.add("advimage", function (editor, url) {
    var self = this;
    var viewModel;
    var idModal = 'mce-modal-image';
    var btnToolTip = "Insert/edit image";
    var clickEvt = function (e) {
        viewModel.mvpopupimagelist(new Shared.mv_popupselectimage(idModal, false, function (data) {
           editor.insertContent("<img src='" + data + "' />");
        }));
        var node = document.getElementById(idModal);
        if (!viewModel.mvpopupimagelist().mpopupimage.IsInit() && !node.hasChildNodes()) {
            ko.applyBindings(viewModel, node);
            viewModel.mvpopupimagelist().mpopupimage.IsInit(true);
        };
        viewModel.mvpopupimagelist().mpopupimage.LoadFileList();
        CommonUtils.openPopupModal(idModal);
    };

    editor.addButton('advimage', {
        icon: 'image',
        tooltip: btnToolTip,
        onclick: clickEvt
    });

    editor.addMenuItem('advimage', {
        icon: 'image',
        tooltip: btnToolTip,
        onclick: clickEvt
    });

    editor.on("init", function () {
        if ($('#' + idModal).length == 0)
            $('#' + editor.id).parent().append("<div id=\"" + idModal + "\" />");
        var node = document.getElementById(idModal);
        viewModel = ko.contextFor(node).$rawData;
        if (!viewModel.mvpopupimagelist)
            viewModel.mvpopupimagelist = ko.observable();
        $(node).attr("data-bind", "template: { name: \"PopupSelectImageTmpl\" }");
    });
});