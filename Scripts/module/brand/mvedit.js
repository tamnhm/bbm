var Brand = Brand || {};
Brand.mvedit = function () {
    var self = this;
    self.InitFinish = ko.observable(false);
    self.checkautolink = ko.observable(false);
    self.mBrand = ko.observable(new Brand.mBrand());
    self.HasError = ko.observable(false);
    self.LoadData = function (id) {
        CommonUtils.showWait(true);
        $.ajax({
            type: "POST",
            url: '/Admin/Brand/Detail',
            cache: false,
            data: { id: id },
            success: function (data) {
                ko.mapping.fromJS(data, {}, self.mBrand);
            }
        }).always(
                function () {
                    self.InitFinish(true);
                    CommonUtils.showWait(false);
                });
    };
    self.linkback = ko.computed(function () {
        return CommonUtils.url('brand#/list');
    });
    self.checkautolink.subscribe(function (v) {
        self.mBrand().isaddlink(v);
    });
    self.Validator = ko.computed(function () {
        return (ko.validatedObservable(self.mBrand()))();
    });
    self.submit = function () {
        if (self.Validator().isValid()) {
            CommonUtils.showWait(true);
            if (self.mBrand().mahieu() == undefined) {
                $.ajax({
                    type: 'POST',
                    url: CommonUtils.url('/Admin/Brand/Insert'),
                    cache: false,
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8',
                    data: ko.toJSON(self.mBrand()),
                    success: function (r) {
                        if (r.success) {
                            debugger
                            window.location.hash = '#/edit/' + r.id;
                            $.gritter.add({
                                title: 'Thông báo',
                                text: r.messaging
                            });
                            self.LoadData(r.id);
                        }
                        else { alert(r.messaging) }
                        CommonUtils.showWait(false);
                    }
                })
            }
            else {
                $.ajax({
                    type: 'POST',
                    url: CommonUtils.url('/Admin/Brand/Update'),
                    cache: false,
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8',
                    data: ko.toJSON(self.mBrand()),
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
                })
            }
        } else {
            self.HasError(true);
            self.Validator().errors.showAllMessages();
        }

    };
    self.upload = function (data) {
        if (data)
            $(data).fileupload({
                autoUpload: true,
                url: '/Admin/Common/UploadFileBrand',
                dataType: 'json',
                add: function (e, data) {
                    var jqXHR = data.submit()
                        .success(function (data, textStatus, jqXHR) {
                            if (data.isUploaded) {
                                self.mBrand().hinh("/Images/hinhdulieu/nhanhieu/" + data.namefile);
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

}