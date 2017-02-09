var Checkout = Checkout || {};
Checkout.mCheckout = function () {
    var self = this;
    self.CartItemModel = ko.observableArray();
    self.CartTotal = ko.observable();
    self.KgTotal = ko.observable();
    self.WHL = ko.observable();
};

Checkout.CartItemModel = function () {
    var self = this;
    self.CartId = ko.observable();
    self.Count = ko.observable();
    self.DateCreated = ko.observable();
    self.ProductId = ko.observable();
    self.RecordId = ko.observable();
    self.shop_bienthe = ko.observable(new Checkout.shop_bienthe());
};
Checkout.shop_bienthe = function () {
    var self = this;
    self.id = ko.observable();
    self.gia = ko.observable();
    self.giasosanh = ko.observable();
    self.idsp = ko.observable();
    self.imgsp = ko.observable();
    self.kg = ko.observable();
    self.chieurong = ko.observable();
    self.chieucao = ko.observable();
    self.chieudai = ko.observable();
    self.masp = ko.observable();
    self.soluong = ko.observable();
    self.tensp = ko.observable();
    self.title = ko.observable();

};
Checkout.mDonhang = function () {
    var self = this;
    self.id = ko.observable();
    self.makh = ko.observable();
    self.vanglai = ko.observable();
    self.ghichu = ko.observable();
    self.tongtien = ko.observable();
    self.ship = ko.observable(0);
    self.idgiogiao = ko.observable();
    self.ngaygiao = ko.observable();
    self.ngaydat = ko.observable(new Date());
    self.pttt = ko.observable();
    self.ptgh = ko.observable();
    self.thongtinxedo = ko.observable();
    self.donhang_ct = ko.observableArray();
    self.datru_diem = ko.observable();
    self.typeconfim = ko.observable(0);
    self.NLpayBankType = ko.observable();
    self.BankOnline = ko.observable();
};
Checkout.mDonghangCt = function () {
    var self = this;
    self.Sodh = ko.observable();
    self.Id = ko.observable();
    self.IdPro = ko.observable();
    self.Soluong = ko.observable();
    self.Dongia = ko.observable();
    self.DongiaSS = ko.observable();
    self.shop_bienthe = ko.observable();
}
Checkout.mCustomer = function () {
    var self = this;
    self.MaKH = ko.observable();
    self.hoten = ko.observable().extend({
        required: { params: true, message: isEnLag == 'True' ? "Tên khách hàng không được bỏ trống" : "Name is required" }
    });
    self.duong = ko.observable().extend({
        required: { params: true, message: isEnLag == 'True' ? "Tên đường không được bỏ trống" : "Street is required" }
    });
    self.dienthoai = ko.observable().extend({
        required: { params: true, message: isEnLag == 'True' ? "Điện thoại không được bỏ trống" : "Phone is required" }
    });
    self.email = ko.observable().extend({
        required: { params: true, message: isEnLag == 'True' ? "Địa chỉ Email không được trống." : "Email is required" },
        email: { params: true, message: isEnLag == 'True' ? "Địa chỉ Email không hợp lệ." : "Email is not valid" }
    });

    self.idtp = ko.observable().extend({
        required: { params: true, message: isEnLag == 'True' ? "Thành phố chưa chọn" : "City is required" }
    });
    self.idquan = ko.observable().extend({
        required: { params: true, message: isEnLag == 'True' ? "Quận chưa chọn" : "District is required" }
    });
    self.selectGio = ko.observable().extend({
        required: {
            params: true, message: isEnLag == 'True' ? "Vui lòng chọn thời gian giao hàng." : "Time shipping is required",
            onlyIf: function () { return (self.idtp() === 1); }
        }
    });
    self.selectPayment = ko.observable().extend({
        required: { params: true, message: isEnLag == 'True' ? "Vui lòng chọn phương thức thanh toán." : "Payment method is required" }
    });
    self.SelectedNLpayBankType = ko.observable().extend({
        required: {
            params: true, message: isEnLag == 'True' ? "Hình thức thanh toán online chưa chọn." : "Online payment method is required",
            onlyIf: function () { return (self.selectPayment() == 4);}
        }
    });
    self.SelectedBankOnline = ko.observable().extend({
        required: {
            params: true, message: isEnLag == 'True' ? "Ngân hàng chưa chọn." : "Bank is required",
            onlyIf: function () { return (self.SelectedNLpayBankType() == 'ATM_ONLINE' && self.selectPayment() == 4); }
        }
    });
    
    self.tendn = ko.observable();
    self.diem = ko.observable(0);
    self.diemsp = ko.observable(0);
};
Checkout.Chuyenphat_tinh_tra = function () {
    var self = this;
    self.Id = ko.observable();
    self.IpTp = ko.observable();
    self.IdTinhtra = ko.observable();
    self.Tentinhtra = ko.observable();
};
Checkout.mTinh = function () {
    var self = this;
    self.id = ko.observable();
    self.idtp = ko.observable();
    self.tentinh = ko.observable();

    self.tentinh_us = ko.observable();
    self.vungsau = ko.observable();
    self.ship = ko.observable();

};
Checkout.mThanhpho = function () {
    var self = this;
    self.id = ko.observable();
    self.tentp = ko.observable();

    self.tentp_us = ko.observable();
    self.mavung = ko.observable();
    self.thoigian = ko.observable();
    self.idtinhtra = ko.observable();

};
Checkout.mVung = function () {
    var self = this;
    self.id = ko.observable();
    self.kilogram = ko.observable();
    self.mavung = ko.observable();
    self.ship = ko.observable();

};
Checkout.mGio = function () {
    var self = this;
    self.id = ko.observable();
    self.title = ko.observable();
};