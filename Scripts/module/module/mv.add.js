var Module = Module || {};
Module.mvadd = function () {
    var self = this;
    self.mModule = ko.observable(new Module.mModule);
    self.InitFinish = ko.observable(false);
    self.tieude = ko.observable();
    self.autourl = ko.observable(true);
    self.HasError = ko.observable(false);
    self.urlhead = ko.observable();
    self.hide = ko.observable(false);
    self.Id = ko.observable();
    /******************************************/
    self.NameModule = ko.observable();
    self.ListMenu = ko.observableArray();
    self.SelectTypeModule = ko.observable(1);
    self.SelectMenu = ko.observable();
    self.SelectGroup = ko.observable();
    self.ListGroup = ko.observableArray();
    self.ListData = ko.observableArray();
    self.ListModuel = ko.observableArray([
        { id: 1, name: 'Game', urlhead: 'Game' },
        { id: 2, name: 'Âm Nhạc', urlhead: 'Music' },
        { id: 3, name: 'Hoạt hình', urlhead: 'Animation' },
        { id: 4, name: 'Tin Tức', urlhead: 'News' },
        { id: 5, name: 'Tạp chí', urlhead: 'Magazine' }]);
    self.SelectTypeModule.subscribe(function (obj) {
        if (obj)
            self.LoadMenu(obj);
    });
    self.SelectMenu.subscribe(function (value) {
        if (value) {
            var av = ko.utils.arrayFirst(self.ListMenu(), function (obj) {
                return obj.id() == value;
            });
            debugger
            self.ListGroup(av.module_group());
        }
    });

    self.LoadMenu = function (id) {
        $.ajax({
            type: "POST",
            url: '/Admin/Module/ListMenu',
            dataType: "json",
            data: { typemodule: id },
            success: function (data) {
                debugger
                self.ListMenu(CommonUtils.MapArray(data, Module.mModuleMenu));
            }
        })
    };

    //self.LoadGroup = function (id) {
    //    $.ajax({
    //        type: "GET",
    //        url: '/Admin/Module/ListGroup',
    //        dataType: "json",
    //        data: { idmenu: id },
    //        success: function (data) {
    //            self.ListGroup(CommonUtils.MapArray(data, Module.mModuleGroup));
    //        },
    //        error: function () {
    //            alert("Load Group that bai");
    //        }
    //    });
    //};
    self.Validator = ko.computed(function () {
        return (ko.validatedObservable(self.mModule()))();
    });
    /********************************************/
    self.tieude.subscribe(function (value) {
        self.mModule().title(value);
        if (self.autourl()) {
            self.mModule().url(CommonUtils.NonUnicode(value));
        }
    });

    self.linkback = ko.computed(function () {
        return CommonUtils.url('#/list');
    });
    self.submit = function () {
        self.mModule().groupid(self.SelectGroup());
        self.mModule().menuid(self.SelectMenu());
        self.mModule().typlemodule(self.SelectTypeModule());
        self.mModule().hide(self.hide());
        if (self.Validator().isValid()) {
            CommonUtils.showWait(true);
            var input = ko.toJSON({ model: self.mModule() });
            $.ajax({
                type: 'POST',
                url: CommonUtils.url('/Admin/Module/InsertArticle'),
                cache: false,
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                data: input,
                success: function (r) {
                    if (r.success) {
                        $.gritter.add({
                            title: 'Thông báo',
                            text: r.messaging
                        });
                        window.location.hash = '#/edit/' + r.id;
                    }
                    else { alert(r.messaging) }
                    CommonUtils.showWait(false);
                }

            });

        } else {
            self.HasError(true);
            self.Validator().errors.showAllMessages();
        }
    };
};