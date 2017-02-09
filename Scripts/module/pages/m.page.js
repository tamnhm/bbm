var Pages = Pages || {};
Pages.mPages = function () {
    var self = this;
    self.id = ko.observable();
    self.tieude = ko.observable().extend({
        required: { params: true, message: "Tên bài viết không được bỏ trống" }
    });
    self.tieude_us = ko.observable(0);
    self.hide = ko.observable(0);
    self.url = ko.observable();
    self.noidung = ko.observable();
    self.noidung_us = ko.observable();
    self.showmenu = ko.observable(0);
    self.ngayviet = ko.observable();
    self.sort = ko.observable();
    self.islinkout = ko.observable(0);
    self.metatitle = ko.observable();
    self.keywords = ko.observable();
    self.description = ko.observable();
    self.codetype = ko.observable("HH");
    self.hinhanh = ko.observable();
    self.tieude.subscribe(function (value) {
        self.url(CommonUtils.NonUnicode(value));
    });
    self.linkdetail = ko.computed(function () {
        return CommonUtils.url('#/edit/' + self.id());
    });
};
