var Link = Link || {};
Link.mvlist_link = function () {
    var self = this;
    self.listlink = ko.observableArray();
    self.linklinkcon = ko.observableArray();
    self.InitFinish = ko.observable(false);
    self.InitFinish1 = ko.observable(false);
    self.mlink = ko.observable(new Link.mlinkcha());
    self.mlinkcon = ko.observable(new Link.mlinkcon());

    self.TagArticle = ko.observableArray();
    self.LoadData = function () {
        CommonUtils.showWait(true);
        $.ajax({
            type: "GET",
            url: '/Admin/Link/Getdanhmuc',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: { loai: 0 },
            success: function (data) {
                self.listlink(CommonUtils.MapArray(data.Data, Link.mlinkcha));
                self.TagArticle(CommonUtils.MapArray(data.TagArticle, Link.msys_tags_Summary));
            }
        }).always(
                function () {
                    CommonUtils.showWait(false);
                });
    };
    //self.mlink.isaddLink = ko.observable(false);
    self.checkautolink = ko.observable(false);
    self.checkautolinkcon = ko.observable(false);
    //LINK subscribe

    self.checkautolink.subscribe(function (v) {
        self.mlink().isaddlink(v);
    });
    self.checkautolinkcon.subscribe(function (v) {
        self.mlinkcon().isaddlinkcon(v);
    });
    self.Setting = function (data) {
        self.mlink(data);
        self.InitFinish1(true);
        $('#modal-setting').modal('show');
        self.checkautolink(false);
    };

    self.SaveSetting = function (data) {
        CommonUtils.showWait(true);
        if (data.madanhmuc() == undefined) {
            $.ajax({
                type: 'POST',
                url: CommonUtils.url('/Admin/Link/insertLink'),
                cache: false,
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                data: ko.toJSON(data),
                success: function (r) {
                    if (r.success) {
                        $.gritter.add({
                            title: 'Thông báo',
                            text: r.messaging
                        });
                        self.LoadData();
                    }
                    else { alert(r.messaging) }
                    CommonUtils.showWait(false);
                }
            })
        }
        else {
            var input = ko.toJSON({ dm: data, tagRef: self.mlink().ListTagsArticle() });
            $.ajax({
                type: 'POST',
                url: CommonUtils.url('/Admin/Link/updateLink'),
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
                        self.LoadData();
                    }
                    else { alert(r.messaging) }
                }
            })
        }
        $('#modal-setting').modal('hide');
    };
    self.deleteLink = function (data) {
        $(".hide_delete").css("display", "block");
        bootbox.confirm("Bạn muốn xóa danh mục <strong>" + data.tendanhmuc() + "</strong> ?", function (result) {
            if (result) {
                CommonUtils.showWait(true);
                $.ajax({
                    type: 'POST',
                    url: CommonUtils.url('/Admin/Link/RemoveLink'),
                    cache: false,
                    data: { id: data.madanhmuc() },
                    success: function (r) {
                        if (r.success) {
                            $.gritter.add({
                                title: 'Thông báo',
                                text: r.messaging
                            });
                            $(".hide_delete").css("display", "none");
                            $('#modal-setting').modal('hide');
                            self.LoadData();
                        }
                        else {
                            alert(r.messaging)
                            $(".hide_delete").css("display", "none");
                        }
                        CommonUtils.showWait(false);
                    }
                });

            }
            else { $(".hide_delete").css("display", "none"); }
        });
    };

    //LINK CON
    self.detail_linkcon = function (data) {
        debugger
        ko.mapping.fromJS(data, {}, self.mlinkcon);
        self.InitFinish(true);
        $('#modal-setting-children').modal('show');
    };
    self.delete_linkcon = function (data) {
        $('#modal-setting-children').modal('hide');
        bootbox.confirm("Bạn muốn xóa link <strong>" + data.tendanhmuccon() + "</strong> ?", function (result) {
            if (result) {
                CommonUtils.showWait(true);
                $.ajax({
                    type: 'POST',
                    url: CommonUtils.url('/Admin/Link/RemoveLinkCon'),
                    cache: false,
                    data: { id: data.madanhmuccon() },
                    success: function (r) {
                        if (r.success) {
                            $.gritter.add({
                                title: 'Thông báo',
                                text: r.messaging
                            });
                            self.LoadData();
                        }
                        else { alert(r.messaging) }
                        CommonUtils.showWait(false);
                    }
                });

            }
            else { $(".hide_delete").css("display", "none"); }
        });

    };
    self.updateLinkcon = function (data) {
        CommonUtils.showWait(true);
        if (data.madanhmuccon() == undefined) {
            data.madanhmuc = self.mlink().madanhmuc;
            $.ajax({
                type: 'POST',
                url: CommonUtils.url('/Admin/Link/insertLinkCon'),
                cache: false,
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                data: ko.toJSON(data),
                success: function (r) {
                    if (r.success) {
                        $.gritter.add({
                            title: 'Thông báo',
                            text: r.messaging
                        });
                        self.LoadData();
                    }
                    else { alert(r.messaging) }
                }
            });
        }
        else {
            var input = ko.toJSON({ dm: data, tagRef: self.mlinkcon().ListTagsArticle() });
            $.ajax({
                type: 'POST',
                url: CommonUtils.url('/Admin/Link/updateLinkCon'),
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
                        self.LoadData();
                    }
                    else { alert(r.messaging) }
                }
            });
        }
        $('#modal-setting-children').modal('hide');
        CommonUtils.showWait(false);
    };

    self.linkaddLink = function (data) {
        self.mlink(data);
        self.mlinkcon(new Link.mlinkcon());
        // self.InitFinish1(true);
        $('#modal-setting-children').modal('show');
    };
    self.addLink = function () {
        self.mlink(new Link.mlinkcha());
        self.mlink().isaddlink(true);
        self.checkautolink(true);
        // self.InitFinish1(true);
        $('#modal-setting').modal('show');
        // self.Location(new Location.mLocation());
        //  self.Validator = ko.validatedObservable(self.Location());
        //  self.HasError(false);
    };
    //************Tags************   
    self.RemoveTag = function (obj) {
        if (obj.Id()) {
            bootbox.confirm("Bạn muốn xóa tags ?", function (result) {
                if (result) {
                    CommonUtils.showWait(true);
                    if (obj) {
                        $.ajax({
                            type: "POST",
                            url: '/Admin/Product/RemoveTag',
                            data: { IdTags: obj.Id() },
                            success: function (data) {
                                if (data) {
                                    $.gritter.add({
                                        title: 'Thông báo',
                                        text: 'Cập nhật Tag thành công'
                                    });
                                    self.LoadData();
                                    $('#modal-setting').modal('hide');
                                }
                                else {
                                    alert("Xóa tags không thành công");
                                }
                            },
                            error: function () {
                                alert("Load Group that bai");
                            }
                        }).always(function () {
                            CommonUtils.showWait(false);
                        });
                    };
                }
            });
        } else {
            self.mlink().ListTagsArticle.remove(obj);
        }
    };
    self.RemoveTagFORLINKCON = function (obj) {
        if (obj.Id()) {
            bootbox.confirm("Bạn muốn xóa tags ?", function (result) {
                if (result) {
                    CommonUtils.showWait(true);
                    if (obj) {
                        $.ajax({
                            type: "POST",
                            url: '/Admin/Product/RemoveTag',
                            data: { IdTags: obj.Id() },
                            success: function (data) {
                                if (data) {
                                    $.gritter.add({
                                        title: 'Thông báo',
                                        text: 'Cập nhật Tag thành công'
                                    });
                                    self.LoadData();
                                    $('#modal-setting-children').modal('hide');
                                }
                                else {
                                    alert("Xóa tags không thành công");
                                }
                            },
                            error: function () {
                                alert("Load Group that bai");
                            }
                        }).always(function () {
                            CommonUtils.showWait(false);
                        });
                    };
                }
            });
        } else {
            self.mlinkcon().ListTagsArticle.remove(obj);
        }
    };
    self.ClickAddTagsArticle = function (obj) {
        if (obj && !obj.IsActive()) {
            var model = ko.observable(new Link.msys_tags_Ref);
            model().RefId(self.mlink().madanhmuc());
            model().RefTag(obj.Id());
            model().Tags(obj.Tags());
            self.mlink().ListTagsArticle.push(model());
            obj.IsActive(true);
        }
    };
    self.ClickAddTagsArticleFORLINKCON = function (obj) {
        if (obj && !obj.IsActive()) {
            var model = ko.observable(new Link.msys_tags_Ref);
            model().RefId(self.mlinkcon().madanhmuccon());
            model().RefTag(obj.Id());
            model().Tags(obj.Tags());
            self.mlinkcon().ListTagsArticle.push(model());
            obj.IsActive(true);
        }
    };
}