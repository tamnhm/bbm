var Module = Module || {};
Module.mvlist = function () {
    var self = this;
    self.TypeModule = ko.observable();
    self.ListMenu = ko.observableArray();
    self.SelectMenu = ko.observable();
    self.SelectGroup = ko.observable();
    self.ListGroup = ko.observableArray();
    self.ListData = ko.observableArray();
    self.NameModule = ko.observable();
    self.mGroup = ko.observable(new Module.mModuleGroup);
    self.mMenu = ko.observable(new Module.mModuleMenu);
    self.txt_search = ko.observable();
    self.SelectMenu.subscribe(function (value) {
        if (value) {
            self.mGroup().idmenu(value);
            self.ListGroup([]);
            var tmpMenu = ko.utils.arrayFirst(self.ListMenu(), function (obj) {
                return obj.id() == value
            });
            if (tmpMenu != null)
                self.ListGroup(CommonUtils.MapArray(tmpMenu.module_group(), Module.mModuleGroup));
        }
    });
    self.LoadMenu = function (id) {
        self.mMenu().typemodule(id);
        CommonUtils.showWait(true);
        $.ajax({
            type: "POST",
            url: '/Admin/Module/ListMenu',
            dataType: 'json',
            data: { typemodule: id },
            success: function (data) {
                debugger
                self.ListMenu([]);
                self.ListMenu(CommonUtils.MapArray(data, Module.mModuleMenu));
                self.SelectMenu(undefined);
                self.SelectMenu(data[0].id);
            }
        }).always(function () {
            CommonUtils.showWait(false);
        });
    };
    self.search_query = function () {
        debugger
        if (self.txt_search()) {
            CommonUtils.showWait(true);
            $.ajax({
                type: 'POST',
                url: CommonUtils.url('/Admin/Module/Search_query'),
                cache: false,
                dataType: 'json',
                data: { query: self.txt_search() },
                success: function (data) {
                    self.ListData(CommonUtils.MapArray(data, Module.mModule));
                }
            }).always(function () {
                CommonUtils.showWait(false);
            });
        }
    };
    self.SubmitFind = function () {
        debugger
        var groupid = self.SelectGroup();
        CommonUtils.showWait(true);
        $.ajax({
            type: "POST",
            url: '/Admin/Module/Getlist',
            data: { groupid: groupid },
            success: function (data) {
                self.ListData(CommonUtils.MapArray(data, Module.mModule));
            }
        }).always(function () {
            CommonUtils.showWait(false);
        });
    };
    /*******************************************************************************************************/
    var filters = [
       {
           Type: "text",
           Name: "title",
           Value: ko.observable(""),
           RecordValue: function (record) { return record.title; }
       }

    ];
    var sortOptions = [
        {
            Name: "id",
            Value: "id",
            Sort: function (left, right) { return left.id < right.id; }
        }
    ];
    self.filter = new CommonUtils.FilterModel(filters, self.ListData);
    self.sorter = new CommonUtils.SorterModel(sortOptions, self.filter.filteredRecords);
    self.pager = new CommonUtils.PagerModel(self.sorter.orderedRecords);
    /*******************************************************************************************************/
    self.HideArticle = function (value) {
        bootbox.confirm("Bạn muốn ẩn/hiện <strong>" + value.title() + "</strong> ?", function (result) {
            if (result) {
                CommonUtils.showWait(true);
                $.ajax({
                    type: 'POST',
                    url: CommonUtils.url('/Admin/Module/InvisibleArticle'),
                    cache: false,
                    data: { id: value.id() },
                    success: function (r) {
                        if (r.success) {
                            $.gritter.add({
                                title: 'Thông báo',
                                text: r.messaging
                            });
                            self.SubmitFind();
                        } else { alert(r.messaging) }
                    }
                }).always(function () {
                    CommonUtils.showWait(false);
                });
            }
        })
    };
    self.RemoveArticle = function (value) {
        bootbox.confirm("Bạn muốn xóa <strong>" + value.title() + "</strong> ?", function (result) {
            if (result) {
                CommonUtils.showWait(true);
                $.ajax({
                    type: 'POST',
                    url: CommonUtils.url('/Admin/Module/DeleteArticle'),
                    cache: false,
                    data: { id: value.id() },
                    success: function (r) {
                        if (r.success) {
                            $.gritter.add({
                                title: 'Thông báo',
                                text: r.messaging
                            });
                            self.SubmitFind();
                        } else { alert(r.messaging) }
                    }
                }).always(function () {
                    CommonUtils.showWait(false);
                });
            }
        })
    };
    self.linkadd = ko.computed(function () {
        return CommonUtils.url('#/new');
    });

    /******************************************************/
    self.modalGroup = function () {
        $('#modal_menu_group').modal('show');
    };
    self.modalMenu = function () {
        $('#modal_menu').modal('show');
    };
    self.removeMenu = function (data) {
        if (data.id() == undefined) {
            self.ListMenu.remove(data)
        }
        else {
            bootbox.confirm("Bạn chắc muốn xóa ?", function (result) {
                if (result) {
                    CommonUtils.showWait(true);
                    $.ajax({
                        type: 'POST',
                        url: CommonUtils.url('/Admin/Module/DeleteMenu'),
                        cache: false,
                        data: { id: data.id() },
                        success: function (r) {
                            if (r.success) {
                                $.gritter.add({
                                    title: 'Thông báo',
                                    text: r.messaging
                                });
                                window.location.reload(true);
                            } else { alert(r.messaging) }
                        }
                    }).always(function () {
                        CommonUtils.showWait(false);
                    });
                }
            })
        }
    };
    self.removeGroup = function (data) {
        if (data.id() == undefined) {
            self.ListGroup.remove(data)
        }
        else {
            bootbox.confirm("Bạn chắc muốn xóa ?", function (result) {
                if (result) {
                    CommonUtils.showWait(true);
                    $.ajax({
                        type: 'POST',
                        url: CommonUtils.url('/Admin/Module/DeleteGroup'),
                        cache: false,
                        data: { id: data.id() },
                        success: function (r) {
                            if (r.success) {
                                $.gritter.add({
                                    title: 'Thông báo',
                                    text: r.messaging
                                });
                                window.location.reload(true);
                            } else { alert(r.messaging) }
                        }
                    }).always(function () {
                        CommonUtils.showWait(false);
                    });
                }
            })
        }
    };
    self.addGroup = function () {
        var group = new Module.mModuleGroup;
        group.idmenu(self.mGroup().idmenu());
        self.ListGroup.push(group);
    };
    self.addMenu = function () {
        var menu = new Module.mModuleMenu;
        menu.typemodule(self.mMenu().typemodule());
        self.ListMenu.push(menu);
    };
    self.UpdateMenu = function () {
        CommonUtils.showWait(true);
        var input = ko.toJSON({ model: self.ListMenu() });
        $.ajax({
            type: 'POST',
            url: CommonUtils.url('/Admin/Module/UpdateMenu'),
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
                    window.location.reload(true);
                }
                else { alert(r.messaging) }
            }
        }).always(function () {
            CommonUtils.showWait(false);
        });
    };
    self.UpdateGroup = function () {
        CommonUtils.showWait(true);
        var input = ko.toJSON({ model: self.ListGroup() });
        $.ajax({
            type: 'POST',
            url: CommonUtils.url('/Admin/Module/UpdateGroup'),
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
                    window.location.reload(true);
                }
                else { alert(r.messaging) }
            }
        }).always(function () {
            CommonUtils.showWait(false);
        });
    };
}