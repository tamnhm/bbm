var Checkoutplan = Checkoutplan || {};
Checkoutplan.mCheckout = function () {
    var self = this;
    self.Gift = ko.observableArray();
    self.PlanModel = ko.observableArray();
    self.Percent = ko.observable();
    self.ToTal = ko.observable();
    self.TotalSum = ko.observable();
    self.KgTotal = ko.observable();
};
Checkoutplan.mPlanModel = function () {
    var self = this;
    self.Count = ko.observable();
    self.IdProdut = ko.observable();
    self.gia = ko.observable();
    self.sum = ko.observable();
    self.tensp = ko.observable();
    self.masp = ko.observable();
    self.hinhanh = ko.observable();
    self.typePlan = ko.observable();
    self.Id = ko.observable();
    self.namePlan = ko.observable();
};
Checkoutplan.mGift = function () {
    var self = this;
    self.img = ko.observable();
    self.note = ko.observable();
    self.tensp = ko.observable();
    self.masp = ko.observable();
    self.url = ko.observable();
};
Checkoutplan.CartItemModel = function () {
    var self = this;
    self.CartId = ko.observable();
    self.Count = ko.observable();
    self.DateCreated = ko.observable();
    self.ProductId = ko.observable();
    self.RecordId = ko.observable();
    self.shop_bienthe = ko.observable(new Checkoutplan.shop_bienthe());
};
Checkoutplan.shop_bienthe = function () {
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
Checkoutplan.mDonhang = function () {
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
    self.typeconfim = ko.observable(0);
    self.NLpayBankType = ko.observable();
    self.BankOnline = ko.observable();
};
Checkoutplan.mDonghangCt = function () {
    var self = this;
    self.Sodh = ko.observable();
    self.Id = ko.observable();
    self.IdPro = ko.observable();
    self.Soluong = ko.observable();
    self.Dongia = ko.observable();
    self.DongiaSS = ko.observable();
    self.shop_bienthe = ko.observable();
}
Checkoutplan.mCustomer = function () {
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
            required: true, message: isEnLag == 'True' ? "Vui lòng chọn thời gian giao hàng." : "Time shipping is required",
            onlyIf: function () { return (self.idtp() === 1); }
        }
    });
    self.selectPayment = ko.observable().extend({
        required: { required: true, message: isEnLag == 'True' ? "Vui lòng chọn hình thức thanh toán." : "Payment method is required" }
    });
    self.tendn = ko.observable();
    self.diem = ko.observable(0);
    self.diemsp = ko.observable(0);
};
Checkoutplan.Chuyenphat_tinh_tra = function () {
    var self = this;
    self.Id = ko.observable();
    self.IpTp = ko.observable();
    self.IdTinhtra = ko.observable();
    self.Tentinhtra = ko.observable();
};
Checkoutplan.mTinh = function () {
    var self = this;
    self.id = ko.observable();
    self.idtp = ko.observable();
    self.tentinh = ko.observable();
    self.tentinh_us = ko.observable();
    self.vungsau = ko.observable();
    self.ship = ko.observable();

};
Checkoutplan.mThanhpho = function () {
    var self = this;
    self.id = ko.observable();
    self.tentp = ko.observable();
    self.tentp_us = ko.observable();
    self.mavung = ko.observable();
    self.thoigian = ko.observable();
    self.idtinhtra = ko.observable();

};
Checkoutplan.mVung = function () {
    var self = this;
    self.id = ko.observable();
    self.kilogram = ko.observable();
    self.mavung = ko.observable();
    self.ship = ko.observable();

};
Checkoutplan.mGio = function () {
    var self = this;
    self.id = ko.observable();
    self.title = ko.observable();
};