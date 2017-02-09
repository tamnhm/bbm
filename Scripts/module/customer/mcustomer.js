var Customer = Customer || {};
Customer.mCustomer = function () {
    var self = this;
    self.MaKH = ko.observable();
    self.hoten = ko.observable().extend({
        required: { params: true, message: "Họ tên không được bỏ trống" }
    });
    self.dienthoai = ko.observable().extend({
        required: { params: true, message: "Số điện thoại không được bỏ trống" }
    });
    self.email = ko.observable().extend({
        required: { params: true, message: "Email không được bỏ trống" }
    });
    self.tendn = ko.observable().extend({
        required: { params: true, message: "Tên Đăng nhập không được bỏ trống" },
        minLength: {
            params: 8,
            message: "Tên đăng nhập phải 8 ký tự trở lên."
        }
    });
    self.matkhau = ko.observable().extend({
        required: { params: true, message: "Mật khẩu không được bỏ trống" },
        minLength: {
            params: 8,
            message: "Mật khẩu phải 8 ký tự trở lên."
        }
    });
    self.matkhaunhaplai = ko.observable().extend({
        validation: {
            validator: function (val, someOtherVal) {
                return val === someOtherVal();
            },
            message: 'Mật khẩu nhập lại chưa đúng !',
            params: self.matkhau
        }
    });
    self.idquan = ko.observable();
    self.idtp = ko.observable();
    self.diem = ko.observable();
    self.duong = ko.observable().extend({
        required: { params: true, message: "Tên đường không được bỏ trống" }
    });
    self.diem = ko.observable(0);
    self.donhangs = ko.observableArray();
    self.konhanmail = ko.observable(false);
};
Customer.mCustomerAdmin = function () {
    var self = this;
    self.MaKH = ko.observable();
    self.hoten = ko.observable().extend({
        required: { params: true, message: "Họ Tên không được bỏ trống" }
    });
    self.dienthoai = ko.observable();
    self.email = ko.observable().extend({
        required: { params: true, message: "Email không được bỏ trống" }
    });
    self.tendn = ko.observable().extend({
        required: { params: true, message: "Tên Đăng nhập không được bỏ trống" }
    });
    self.matkhau = ko.observable();
    self.idquan = ko.observable();
    self.idtp = ko.observable();
    self.diem = ko.observable();
    self.duong = ko.observable().extend({
        required: { params: true, message: "Tên đường không được bỏ trống" }
    });
    self.diem = ko.observable(0);
    self.donhangs = ko.observableArray();
    self.konhanmail = ko.observable(false);
};

Customer.mbienthe = function () {
    var self = this;
    self.id = ko.observable();
    self.idsp = ko.observable();
    self.title = ko.observable('default');
    self.tmpgia = ko.observable();
    self.gia = ko.observable(0);
    self.giasosanh = ko.observable(0);
   
    self.tensp = ko.observable();

    self.fullten = ko.computed(function () {
        if (self.title() != 'default')
            return self.tensp() + ' - ' + self.title();
        else
            return self.tensp();

    });
    self.imgsp = ko.observable();
    self.masp = ko.observable();
};