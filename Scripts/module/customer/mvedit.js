var Customer = Customer || {};
ko.validation.rules['username_exists'] = {
    validator: function (item, array) {
        var result = true;
        if (array().length > 0) {
            for (var i = 0; i < array().length; i++) {
                if (array()[i]["tendn"] == item) {
                    result = false;
                    break;
                }
            }
        }
        return result;
        // return array.indexOf(item) == -1;
    },
    message: 'Tên đăng nhập đã tồn tại'
};
ko.validation.rules['email_exists'] = {
    validator: function (item, array) {
        var result = true;
        if (array().length > 0) {
            debugger
            for (var i = 0; i < array().length; i++) {
                if (array()[i]["email"] == item) {
                    result = false;
                    break;
                }
            }
        }
        return result;
        // return array.indexOf(item) == -1;
    },
    message: 'Email đã tồn tại'
};
ko.validation.registerExtenders();
//Shipping.m_dropdownlist = function () {
//    var self = this;
//    self.Id = ko.observable();
//    self.Name = ko.observable();
//    self.IsSelected = ko.observable(false);
//}
Customer.mvedit = function () {
    var self = this;
    self.mCustomer = ko.observable(new Customer.mCustomer());
    self.HasError = ko.observable(false);
    self.ArrayTp = ko.observableArray([]);
    self.ArrayTinh = ko.observableArray();
    self.TpSelectedValue = ko.observable();
    self.Tp = ko.observable(new Shipping.Chuyenphat_tp());
    self.Tinh = ko.observable(new Shipping.Chuyenphat_tinh());
    self.Validator = ko.computed(function () {
        //  var s = ko.validation.group([self.tendangnhap(), self.email()]);
        var u = ko.validatedObservable(self.mCustomer());
        return (u)();
    });

    self.ArrayCustomer = ko.observableArray();
    self.GetListCus = function () {
        $.ajax({
            type: "GET",
            url: '/Admin/Customers/GetList',
            success: function (data) {
                self.ArrayCustomer(data);
            }
        });
    };
    self.tendangnhap = ko.observable().extend({ username_exists: self.ArrayCustomer });
    self.email = ko.observable().extend({ email_exists: self.ArrayCustomer });
    self.StringCapcha = ko.observable();
    self.Capcha = ko.observable();
    self.RandomCapcha = function () {
        var text = "";
        var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        for (var i = 0; i < 5; i++)
            text += possible.charAt(Math.floor(Math.random() * possible.length));
        self.StringCapcha(text);
    };
    self.thanhcong = ko.observable(false);
    self.waitting = ko.observable(false);
    self.CompareCapcha = ko.computed(function () {
        if (self.Capcha() === self.StringCapcha())
            return true;
        return false;
    });
    self.submit = function () {
        self.mCustomer().email(self.email());
        self.mCustomer().tendn(self.tendangnhap());
        if (self.Validator().isValid() && self.CompareCapcha()) {
            self.waitting(true);
            $.ajax({
                type: 'POST',
                url: CommonUtils.url('/Customer/Register'),
                cache: false,
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                data: ko.toJSON(self.mCustomer()),
                success: function (r) {
                    if (r.success) {
                        self.waitting(false);
                        self.thanhcong(true);
                       // setTimeout("window.location.href='/trang-chu.html';", 2000);
                    }
                    else {
                        alert(r.messaging);
                    }
                }
            }).always(function () {
                self.waitting(false);
            })

        } else {
            self.HasError(true);
            self.Validator().errors.showAllMessages();
        }

    };
    /****************************************************************************/

    self.loadfullTP = ko.observable(false);
    self.loadfullQuan = ko.observable(false);
    self.LoadArrayTp = function () {
        // CommonUtils.showWait(true);
        self.loadfullTP(true);
        $.ajax({
            type: "GET",
            url: '/Admin/Shipping/GetCity',
            success: function (data) {
                //var group = [];
                //group.push(data[1]);
                //ko.utils.arrayForEach(data[0], function (obj) {
                //    group.push(obj);
                //});
                //self.ArrayTp(CommonUtils.MapArray(group, Shipping.Chuyenphat_tp));
                self.ArrayTp(CommonUtils.MapArray(data, Shipping.Chuyenphat_tp));
            }
        }).always(
        function () {
            self.loadfullTP(false);
        }
        );
    };

    self.mCustomer().idtp.subscribe(function (newValue) {
        self.loadfullQuan(true);
        if (newValue == undefined) {
            self.Tp().idquan(undefined);
            self.loadfullQuan(false);
        }
        else {
            $.ajax({
                type: "GET",
                url: '/Admin/Shipping/Gettinhbytp',
                data: { idtp: newValue },
                success: function (data) {
                    self.ArrayTinh(CommonUtils.MapArray(data, Shipping.Chuyenphat_tinh));
                }
            }).always(function () {
                self.loadfullQuan(false);
            });
        }
    });




    self.Start = function () {
        ko.applyBindings(self);
        self.LoadArrayTp();
        self.GetListCus();
        self.RandomCapcha();

    };
}