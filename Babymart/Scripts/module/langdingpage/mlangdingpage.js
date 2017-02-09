var Landingpage = Landingpage || {};
Landingpage.mLandingpage = function () {
    var self = this;
    self.maloai = ko.observable();
    self.tenloai = ko.observable().extend({
        required: { params: true, message: "Tên Page không được bỏ trống" }
    });
    self.metadescription = ko.observable();
    self.metakeywords = ko.observable();
    self.title = ko.observable();
};