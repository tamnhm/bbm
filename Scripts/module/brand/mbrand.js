var Brand = Brand || {};
Brand.mBrand = function () {
    var self = this;
    self.mahieu = ko.observable();
    self.tenhieu = ko.observable().extend({
        required: { params: true, message: "Tên hiệu không được bỏ trống" }
    });
    self.tenhieu_us = ko.observable();
    self.noidung = ko.observable();
    self.noidung_us = ko.observable();
    self.hinh = ko.observable();
    self.url = ko.observable();
    self.title = ko.observable();
    self.metakeywords = ko.observable();
    self.metadescription = ko.observable();
    self.hide = ko.observable();
    self.showhome = ko.observable();
    self.sort_metro = ko.observable();
    self.isaddlink = ko.observable(false);
    self.tenhieu.subscribe(function (value) {
        if (self.isaddlink()) {
            self.url(CommonUtils.NonUnicode(value));
            self.title(value);
        }
    });
    self.GetLinkPageDetail = ko.computed(function () {
        return CommonUtils.url('brand#/edit/' + encodeURIComponent(self.mahieu()));

    });
};