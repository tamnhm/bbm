var Module = Module || {};
Module.mvedit = function () {
    var self = this;
    self.mModule = ko.observable(new Module.mModule);
    self.InitFinish = ko.observable(false);
    self.tieude = ko.observable();
    self.autourl = ko.observable(true);
    self.HasError = ko.observable(false);
    self.urlhead = ko.observable();
    self.hide = ko.observable(false);
    self.IdModule = ko.observable();
    /******************************************/
    self.NameModule = ko.observable();
    self.ListMenu = ko.observableArray();
    self.SelectTypeModule = ko.observable();
    self.SelectMenu = ko.observable();
    self.SelectGroup = ko.observable();
    self.ListGroup = ko.observableArray();
    self.ListData = ko.observableArray();

    self.ListGroup2 = ko.observableArray();
    self.ListMenu2 = ko.observableArray();
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
            if (av)
                self.ListGroup(av.module_group());
        }
    });
    self.isShowOneCollection = ko.observable(false);
    self.submitCollection = function () {
        self.mModule().groupid(self.SelectGroup());
        self.mModule().menuid(self.SelectMenu());
        self.mModule().typlemodule(self.SelectTypeModule());
    };
    self.NameCollection = ko.computed(function () {
        var type = ko.utils.arrayFirst(self.ListModuel(), function (item) {
            return item.id == self.mModule().typlemodule();
        });
        var menu = ko.utils.arrayFirst(self.ListMenu(), function (item) {
            return item.id() == self.mModule().menuid();
        });
        var group = ko.utils.arrayFirst(self.ListGroup(), function (item) {
            return item.id() == self.mModule().groupid();
        });

        if (group != null && menu != null && type != null) {
            return type.name + ' -> ' + menu.tenloai() + ' -> ' + group.title();
        }

    });
    self.NameCollection2 = ko.computed(function () {
        var type = ko.utils.arrayFirst(self.ListModuel(), function (item) {
            return item.id == self.mModule().typlemodule();
        });
        var menu = ko.utils.arrayFirst(self.ListMenu2(), function (item) {
            return item.id() == self.mModule().menuid();
        });
        var group = ko.utils.arrayFirst(self.ListGroup2(), function (item) {
            return item.id() == self.mModule().groupid();
        });

        if (group != null && menu != null && type != null)
            return type.name + ' -> ' + menu.tenloai() + ' -> ' + group.title();
    });
    self.LoadMenu = function (id) {
        if (id)
            $.ajax({
                type: "POST",
                url: '/Admin/Module/ListMenu',
                dataType: "json",
                data: { typemodule: id },
                success: function (data) {
                    self.ListMenu([]);
                    self.ListMenu(CommonUtils.MapArray(data, Module.mModuleMenu));
                    self.SelectMenu(self.mModule().menuid());
                }
            })
    };
    //self.LoadGroup = function (id) {
    //    debugger
    //    if (id)
    //        $.ajax({
    //            type: "POST",
    //            url: '/Admin/Module/ListGroup',
    //            dataType: "json",
    //            data: { idmenu: id },
    //            success: function (data) {
    //                self.ListGroup([]);
    //                self.ListGroup(CommonUtils.MapArray(data, Module.mModuleGroup));
    //            },
    //            error: function () {
    //                alert("Load Group that bai");
    //            }
    //        });
    //};

    self.LoadMenu2 = function (id) {
        if (id)
            $.ajax({
                type: "POST",
                url: '/Admin/Module/ListMenu',
                dataType: "json",
                data: { typemodule: id },
                success: function (data) {
                    self.ListMenu2(CommonUtils.MapArray(data, Module.mModuleMenu));
                }
            })
    };
    //self.LoadGroup2 = function (id) {
    //    if (id)
    //        $.ajax({
    //            type: "GET",
    //            url: '/Admin/Module/ListGroup',
    //            dataType: "json",
    //            data: { idmenu: id },
    //            success: function (data) {
    //                self.ListGroup2(CommonUtils.MapArray(data, Module.mModuleGroup));
    //            },
    //            error: function () {
    //                alert("Load Group that bai");
    //            }
    //        });
    //};

    self.Validator = ko.computed(function () {
        return (ko.validatedObservable(self.mModule()))();
    });
    self.ListTagsProduct = ko.observableArray();
    /********************************************/
    self.LoadData = function (id) {
        CommonUtils.showWait(true);
        self.IdModule(id);
        $.ajax({
            type: "POST",
            url: '/Admin/Module/GetArticle',
            dataType: "json",
            data: { id: id },
            success: function (data) {
                ko.mapping.fromJS(data.Data, {}, self.mModule());
                self.mModule().Tags([]);
                self.mModule().TagsProduct([]);
                self.ListTags(CommonUtils.MapArray(data.ListTag, Module.msys_tags_Summary));
                self.mModule().ListTags(CommonUtils.MapArray(data.Tag, Module.msys_tags_Ref));
                self.tieude(self.mModule().title());
                self.hide(self.mModule().hide());
                self.TagAdd(undefined);
                self.TagAddProdcut(undefined);
                self.ListTagsProduct(CommonUtils.MapArray(data.TagsProduct, Module.msys_tags_Summary));
                self.mModule().ListTagsRefProduct(CommonUtils.MapArray(data.TagsProductRef, Module.msys_tags_Ref));
                self.SelectTypeModule(data.Data.typlemodule);
                self.SelectGroup(data.Data.groupid);
            },
            error: function () {
                alert("Load that bai");
            }
        }).always(
                      function () {
                          self.InitFinish(true);
                          CommonUtils.showWait(false);
                      });
    };

    self.tieude.subscribe(function (value) {
        self.mModule().title(value);
        if (self.autourl()) {
            self.mModule().url(CommonUtils.NonUnicode(value));
        }
    });

    self.linkback = ko.computed(function () {
        return CommonUtils.url('#/list');
    });
    self.remove = function (value) {
        bootbox.confirm("Bạn muốn xóa <strong>" + value.title() + "</strong> ?", function (result) {
            if (result) {
                CommonUtils.showWait(true);
                $.ajax({
                    type: 'POST',
                    url: CommonUtils.url('/Admin/Module/DeleteArticle'),
                    cache: false,
                    data: { id: value.id() },
                    dataType: "json",
                    success: function (r) {
                        if (r.success) {
                            $.gritter.add({
                                title: 'Thông báo',
                                text: r.messaging
                            });
                            window.location.hash = "#/list";
                        } else { alert(r.messaging) }
                    }
                }).always(function () {
                    CommonUtils.showWait(false);
                });
            }
        })
    };
    self.submit = function () {
        ko.utils.arrayForEach(self.mModule().ListTags(), function (obj) {
            self.mModule().Tags.push(obj.Tags());
        });
        ko.utils.arrayForEach(self.mModule().ListTagsRefProduct(), function (obj) {
            self.mModule().TagsProduct.push(obj.Tags());
        });
        self.mModule().hide(self.hide());
        if (self.Validator().isValid()) {
            CommonUtils.showWait(true);
            if (self.mModule().createdate() && self.mModule().createdate().constructor != Date)
                self.mModule().createdate(new Date(parseInt(self.mModule().createdate().substr(6))));


            var input = ko.toJSON({ model: self.mModule() });
            $.ajax({
                type: 'POST',
                url: CommonUtils.url('/Admin/Module/UpdateArticle'),
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
                        self.LoadData(r.id);
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
    self.upload = function (data) {
        if (data)
            $(data).fileupload({
                autoUpload: true,
                url: '/Admin/Common/UploadFileModule',
                dataType: 'json',
                add: function (e, data) {
                    var jqXHR = data.submit()
                        .success(function (data, textStatus, jqXHR) {
                            if (data.isUploaded) {
                                self.mModule().img(data.namefile);
                                toastr.success(data.message);
                            }
                            else {
                                toastr.success(data.message);
                            }
                        })
                        .error(function (data, textStatus, errorThrown) {
                            if (typeof (data) != 'undefined' || typeof (textStatus) != 'undefined' || typeof (errorThrown) != 'undefined') {
                                alert(textStatus + errorThrown + data);
                            }
                        });
                },
                fail: function (event, data) {
                    if (data.files[0].error) {
                        alert(data.files[0].error);
                    }
                }
            });
    };
    //************Tags
    self.ListTags = ko.observableArray();
    self.TagAdd = ko.observable();
    self.TagAddProdcut = ko.observable();
    self.IsTagAdd = ko.observable(true);
    self.TagAdd.subscribe(function (obj) {
        if (obj) {
            var model = ko.observable(new Module.msys_tags_Ref);
            model().Tags(obj);
            self.mModule().ListTags.push(model());
            self.TagAdd(undefined);
        }
    });
    self.TagAddProdcut.subscribe(function (obj) {
        if (obj) {
            var model = ko.observable(new Module.msys_tags_Ref);
            model().Tags(obj);
            self.mModule().ListTagsRefProduct.push(model());
            self.TagAddProdcut(undefined);
        }
    });
    self.ClickAddTags = function (obj) {
        if (obj && !obj.IsActive()) {
            var model = ko.observable(new Module.msys_tags_Ref);
            model().Tags(obj.Tags());
            self.mModule().ListTags.push(model());
            obj.IsActive(true);
        }
    };
    self.ClickAddTagsProduct = function (obj) {
        if (obj && !obj.IsActive()) {
            var model = ko.observable(new Module.msys_tags_Ref);
            model().Tags(obj.Tags());
            self.mModule().ListTagsRefProduct.push(model());
            obj.IsActive(true);
        }
    };
    self.RemoveTag = function (obj) {
        if (obj.Id()) {
            bootbox.confirm("Bạn muốn xóa tags ?", function (result) {
                if (result) {
                    CommonUtils.showWait(true);
                    if (obj) {
                        $.ajax({
                            type: "POST",
                            url: '/Admin/Module/RemoveTag',
                            data: { IdTags: obj.Id() },
                            success: function (data) {
                                if (data) {
                                    $.gritter.add({
                                        title: 'Thông báo',
                                        text: 'Cập nhật Tag thành công'
                                    });
                                    self.LoadData(self.IdModule());
                                }
                                else {
                                    alert("Xóa tags không thành công");
                                }
                            }
                        }).always(function () {
                            CommonUtils.showWait(false);
                        });
                    };
                }
            });
        } else {
            self.mModule().ListTags.remove(obj);
        }
    };
    
    self.RemoveTagProduct = function (obj) {
        if (obj.Id()) {
            bootbox.confirm("Bạn muốn xóa tags ?", function (result) {
                if (result) {
                    CommonUtils.showWait(true);
                    if (obj) {
                        $.ajax({
                            type: "POST",
                            url: '/Admin/Module/RemoveTag',
                            data: { IdTags: obj.Id() },
                            success: function (data) {
                                if (data) {
                                    $.gritter.add({
                                        title: 'Thông báo',
                                        text: 'Cập nhật Tag thành công'
                                    });
                                    self.LoadData(self.IdModule());
                                }
                                else {
                                    alert("Xóa tags không thành công");
                                }
                            }
                        }).always(function () {
                            CommonUtils.showWait(false);
                        });
                    };
                }
            });
        } else {
            self.mModule().ListTagsRefProduct.remove(obj);
        }
    };
    self.IsMoreTag = ko.observable(false);
    self.MoreTags = function () {
        if (self.IsMoreTag())
            self.IsMoreTag(false);
        else
            self.IsMoreTag(true);
    };
};