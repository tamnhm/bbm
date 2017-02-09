var Bannerdong = Bannerdong || {};
Bannerdong.mvBannerdong = function () {
    var self = this;
    self.BannerdongList = ko.observableArray();
    self.BannerdongFile = ko.observable(new Bannerdong.mBannerdong);
    self.ListType = ko.observableArray([
        { name: 'Tất cả', value: 0 },
        { name: 'Banner Home', value: 1 },
        { name: 'Banner left', value: 2 },
        { name: 'Banner right', value: 3 }]);

    self.SelectType = ko.observable(0);
    self.LoadData = function () {
        debugger
        CommonUtils.showWait(true);
        $.ajax({
            type: "POST",
            url: '/Admin/Bannerdong/ListBannerdong',
            data: { type: self.SelectType() },
            dataType: 'json',
            success: function (data) {
                self.BannerdongList(CommonUtils.MapArray(data, Bannerdong.mBannerdong));
            }
        }).always(function () { CommonUtils.showWait(false) });
    };

    self.AddFile = function () {
        self.BannerdongFile(undefined);
        $('#modal-file-edit').modal('show');
    };
    self.EditFile = function (obj) {
        self.BannerdongFile(obj);
        $('#modal-file-edit').modal('show');
    };
    self.SubmitEditFile = function () {
        CommonUtils.showWait(true);
        if (self.BannerdongFile().idbnd()) {
            $.ajax({
                type: 'POST',
                url: CommonUtils.url('/Admin/Banner/Update'),
                cache: false,
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                data: ko.toJSON(self.BannerdongFile()),
                success: function (r) {
                    if (r.success) {
                        $.gritter.add({
                            title: 'Thông báo',
                            text: r.messaging
                        });
                        self.LoadData();
                        $('#modal-file-edit').modal('hide');
                    }
                    else {
                        alert(r.messaging)
                    }
                }
            }).always(function () { CommonUtils.showWait(false) });
        } else {
            $.ajax({
                type: 'POST',
                url: CommonUtils.url('/Admin/Banner/Insert'),
                cache: false,
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                data: ko.toJSON(self.BannerdongFile()),
                success: function (r) {
                    if (r.success) {
                        $.gritter.add({
                            title: 'Thông báo',
                            text: r.messaging
                        });
                        self.LoadData();
                        $('#modal-file-edit').modal('hide');
                    }
                    else {
                        alert(r.messaging)
                    }
                }
            }).always(function () { CommonUtils.showWait(false) });
        }

    };
    self.RemoveFile = function (data) {
        bootbox.confirm("Bạn muốn xóa banner này ?", function (result) {
            if (result) {
                CommonUtils.showWait(true);
                $.ajax({
                    type: 'POST',
                    url: CommonUtils.url('/Admin/Banner/Remove'),
                    cache: false,
                    data: { id: data.Id() },
                    success: function (r) {
                        if (r.success) {
                            $.gritter.add({
                                title: 'Thông báo',
                                text: r.messaging
                            });
                            self.LoadData();
                        }
                        else {
                            alert(r.messaging)
                        }

                    }
                }).always(function () { CommonUtils.showWait(false) });
            }
        });

    };
    self.Start = function () {
        ko.applyBindings(self);
    };
};