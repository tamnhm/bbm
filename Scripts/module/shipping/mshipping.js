var Shipping = Shipping || {};
Shipping.Chuyenphat_vung= function () {
    var self = this;
    self.id = ko.observable();
    self.mavung = ko.observable();
    self.kilogram = ko.observable();
    self.ship = ko.observable();
    self.chars = ko.observable();
  
};
Shipping.Chuyenphat_tp = function () {
    var self = this;
    self.id = ko.observable();
    self.idtinh = ko.observable();
    self.tentp = ko.observable().extend({
        required: { params: true, message: "Tên Thành phố không được bỏ trống" }
    });
    self.tentp_us = ko.observable();
    self.mavung = ko.observable();
    self.thoigian = ko.observable();
    self.idtinhtra = ko.observable();
    self.ArrayTinh = ko.observableArray();
    self.donhang_chuyenphat_tinh = ko.observableArray();
    
};
Shipping.Chuyenphat_tinh = function () {
    var self = this;
    self.id = ko.observable();
    self.iptp = ko.observable();
    self.tentinh = ko.observable();
    self.tentinh_us = ko.observable();
    self.Phivanchuyen = ko.observable();
    self.vungsau = ko.observable(true);
};
Shipping.Chuyenphat_tinh_tra = function () {
    var self = this;
    self.Id = ko.observable();
    self.IpTp = ko.observable();
    self.IdTinhtra = ko.observable();
    self.Tentinhtra = ko.observable();
};
Shipping.char = function () {
    var self = this;
    self.id = ko.observable();
    self.chars = ko.observable();
};