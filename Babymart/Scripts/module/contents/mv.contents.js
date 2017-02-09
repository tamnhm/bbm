var Contents = Contents || {};
Contents.mvContents = function () {
    var self = this;
    self.Contents = ko.observableArray();
    self.mContents = ko.observable(new Contents.mContents());
    self.LoadData = function () {
        CommonUtils.showWait(true);
        $.ajax({
            type: "Get",
            url: '/Admin/ContentsMeta/GetList',
            cache: false,
            success: function (data) {
                self.Contents(CommonUtils.MapArray(data, Contents.mContents));
            }
        }).always(function () { CommonUtils.showWait(false) });
    };
    self.EditContents = function (obj) {
        $('#modal-Contents-edit').modal('show');
        self.mContents(obj);
    };
    /*******************************************************************************************************/
    self.SubmitEditContent = function () {
        CommonUtils.showWait(true);
        var model = ko.toJSON(self.mContents());
        $.ajax({
            type: 'POST',
            url: CommonUtils.url('/Admin/ContentsMeta/UpdateContentsMeta'),
            cache: false,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: model,
            success: function (r) {
                if (r.success) {
                    $.gritter.add({
                        title: 'Thông báo',
                        text: r.messaging
                    });
                    self.LoadData();
                    $('#modal-Contents-edit').modal('hide');
                }
                else {
                    alert(r.messaging)
                }
            }
        }).always(function () { CommonUtils.showWait(false) });
    };
    self.Start = function () {
        ko.applyBindings(self);
        self.LoadData();
    };
};