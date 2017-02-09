var Orders = Orders || {};
Orders.mOrder = function () {
    var self = this;
    self.id = ko.observable();
    self.makh = ko.observable();
    self.tendn = ko.observable();
    self.diem = ko.observable();
    self.vanglai = ko.observable();
    self.gift = ko.observable();
    self.ghichu = ko.observable();
    self.tongtien = ko.observable();
    self.ship = ko.observable();
    self.idgiogiao = ko.observable();
    self.ngaygiao = ko.observable();
    self.ngaydat = ko.observable();
    self.pttt = ko.observable();
    self.ptgh = ko.observable();
    self.dagiao = ko.observable();
    self.donhang_ct = ko.observableArray();
    self.dienthoai = ko.observable();
    self.email = ko.observable();
    self.noidung = ko.observable();
    self.dahuy = ko.observable();
    self.ngayhuy = ko.observable();
    self.typeconfim = ko.observable();
    self.confrimLable = ko.observable('----');
    self.typeconfim.subscribe(function (value) {
        if (value) {
            switch (value) {
                case 1:
                    self.confrimLable('Babymart.vn gọi điện thoại xác nhận trực tiếp');
                    break;
                case 2:
                    self.confrimLable('Xác nhận qua tin nhắn');
                    break;
                case 3:
                    self.confrimLable('Không cần xác nhận (babymart.vn sẽ giao hàng theo thông tin và thời gian đã định).');
                    break;
            }
        }
    });
};