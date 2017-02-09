var Landingpage = Landingpage || {};
Landingpage.mvLandingpage = function () {
    var self = this;
    self.mLandingpage = ko.observable(new Landingpage.mLandingpage());
    self.Dropdownlist = ko.observableArray();
    self.selected = ko.observable();
    self.LoadData = function () {
        CommonUtils.showWait(true);
        $.ajax({
            type: "GET",
            url: '/Admin/Landingpage/Get',
            cache: false,
            success: function (data) {
                self.Dropdownlist(data);
            }
        }).always(CommonUtils.showWait(false));
    };
    self.LoadDetail = function (id) {
        CommonUtils.showWait(true);
        $.ajax({
            type: "POST",
            url: '/Admin/Landingpage/GetDetail',
            cache: false,
            data: { maloai: id },
            success: function (data) {
                debugger
                ko.mapping.fromJS(data, {}, self.mLandingpage);

            }
        }).always(CommonUtils.showWait(false));
    };
    self.selected.subscribe(function (data) {
        self.LoadDetail(data);
    });
    self.submit = function (data) {
        $.ajax({
            type: 'POST',
            url: CommonUtils.url('/Admin/Landingpage/Update'),
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

                    self.LoadDetail(r.id);
                }
                else { alert(r.messaging) }
                CommonUtils.showWait(false);
            }
        }).always(CommonUtils.showWait(false));

    };
    self.upload = function (data) {
        if (data)
            $(data).fileupload({
                autoUpload: true,
                url: '/Brand/UploadFile',
                dataType: 'json',
                add: function (e, data) {
                    var jqXHR = data.submit()
                        .success(function (data, textStatus, jqXHR) {
                            if (data.isUploaded) {

                                self.mBrand().hinh('/Content/img/data/brand/' + data.namefile);
                            }
                            else {
                                debugger
                            }
                            alert(data.message);
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
    self.Start = function () {
        ko.applyBindings(self);
        self.LoadData();
    };

}