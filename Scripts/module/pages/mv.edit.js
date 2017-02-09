var Pages = Pages || {};
Pages.mvedit = function () {
    var self = this;
    self.mPage = ko.observable(new Pages.mPages);
    self.InitFinish = ko.observable(false);
    self.HasError = ko.observable(false);
    self.LoadData = function (id) {
        CommonUtils.showWait(true);
        $.ajax({
            type: "POST",
            url: '/Admin/Page/GetDetail',
            data: { id: id },
            success: function (data) {
                ko.mapping.fromJS(data, {}, self.mPage());
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
    self.RemovePage = function (value) {
        bootbox.confirm("Bạn muốn xóa <strong>" + value.tieude() + "</strong> ?", function (result) {
            if (result) {
                CommonUtils.showWait(true);
                $.ajax({
                    type: 'POST',
                    url: CommonUtils.url('/Admin/Page/DeleteArticle'),
                    cache: false,
                    data: { id: value.id() },
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

    self.Validator = ko.computed(function () {
        return (ko.validatedObservable(self.mPage()))();
    });
    self.submit = function (data) {
        if (self.Validator().isValid()) {
            CommonUtils.showWait(true);
            if (self.mPage().id() == undefined) {
                var input = ko.toJSON({ model: self.mPage() });
                $.ajax({
                    type: 'POST',
                    url: CommonUtils.url('/Admin/Page/InsertPage'),
                    cache: false,
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8',
                    data: input,
                    success: function (r) {
                        if (r.success) {
                            window.location.hash = '#/edit/' + r.id;
                            window.location.reload(true);
                            $.gritter.add({
                                title: 'Thông báo',
                                text: r.messaging
                            });

                        }
                        else { alert(r.messaging) }
                        CommonUtils.showWait(false);
                    }

                });
            } else {
                data.ngayviet(new Date());
                $.ajax({
                    type: 'POST',
                    url: CommonUtils.url('/Admin/Page/UpdateArticle'),
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

                            self.LoadData(r.id);
                        }
                        else { alert(r.messaging) }
                        CommonUtils.showWait(false);
                    }
                });

            }

        } else {
            self.HasError(true);
            self.Validator().errors.showAllMessages();
        }
    };
    self.linkback = ko.computed(function () {
        return CommonUtils.url('#/list');
    });
    self.linkadd = function () {
        return CommonUtils.url('#/new');
    };
};