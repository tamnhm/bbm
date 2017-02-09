
var Checkoutplan = Checkoutplan || {};
Checkoutplan.mvCheckout = function () {
    var self = this;
    /**********************NL********************************/
    self.ATM_ONLINE = ko.observableArray(
     [{ name: 'Ngân hàng Vietcombank', code: 'VIETCOMBANK' },
      { name: 'Ngân hàng thương mại cổ phần ACB', code: 'ACB' },
      { name: 'Ngân hàng Agribank', code: 'AGRIBANK' },
      { name: 'Ngân hàng Vietinbank', code: 'VIETINBANK' },
      { name: 'Ngân hàng Sacombank', code: 'SACOMBANK' },
      { name: 'Ngân hàng TechcomBank', code: 'TECHCOMBANK' },
      { name: 'Ngân Hàng Đông Á', code: 'DONGABANK' },
      { name: 'Ngân hàng EximBank', code: 'EXIMBANK' },
      { name: 'Ngân hàng BIDV', code: 'BIDV' },
      { name: 'Ngân hàng OceanBank', code: 'OJB' },
      { name: 'Ngân hàng MaritimeBank', code: 'MSBANK' },
      { name: 'Ngân Hàng Quân Đội', code: 'MBBANK' },
      { name: 'Ngân hàng HDBank', code: 'HDBANK' },
      { name: 'Ngân hàng VPBank', code: 'VPBANK' },
      { name: 'Ngân hàng OCB', code: 'OCB' },
      { name: 'Ngân hàng Tiên Phong Bank', code: 'TPBANK' },
      { name: 'Ngân hàng Nam Á', code: 'NAMABANK' },
      { name: 'Ngân hàng Quốc dân', code: 'NCB' }
     ]
      );
    self.NLpayBankType = ko.observableArray(
             [{ name: isEnLag == 'True' ? "ATM nội địa" : "Local ATM", code: 'ATM_ONLINE' },
              { name: isEnLag == 'True' ? "Thẻ tín dụng (Visa/MasterCard/JCB)" : "Credit Card (Visa/MasterCard/JCB)", code: 'VISA' }
             ]
        );
    self.SelectedNLpayBankType = ko.observable();
    self.SelectedBankOnline = ko.observable();
    /**********************NL********************************/
    self.mCheckout = ko.observable(new Checkoutplan.mCheckout());
    self.mCustomer = ko.observable(new Checkoutplan.mCustomer());
    self.Isloadercart = ko.observable(false);
    self.NoteCheckout = ko.observable();
    self.loadfullQuan = ko.observable(false);
    self.messenger = ko.observable();
    self.sessionIdQuan = ko.observable();
    self.typeconfim = ko.observable(0);
    self.ListpaymentCity = ko.observableArray([
       { lable: isEnLag == 'True' ? "Thanh toán trực tuyến" : "Online payment", value: 4 },
      { lable: isEnLag == 'True' ? "Chuyển khoản" : "Bank Transfer", value: 2 },
       { lable: isEnLag == 'True' ? "Bằng thẻ ngân hàng khi nhận hàng" : "Bank card on delivery", value: 5 },
      { lable: isEnLag == 'True' ? "Bằng tiền mặt khi nhận hàng" : "Cash on delevery", value: 1 },
    ]);
    self.ListpaymentNotCity = ko.observableArray([
       { lable: isEnLag == 'True' ? "Thanh toán trực tuyến" : "Online payment", value: 4 },
       { lable: isEnLag == 'True' ? "Chuyển khoản" : "Bank Transfer", value: 2 },
       { lable: isEnLag == 'True' ? "Tiền mặt" : "Cash", value: 3 }
    ]);
    self.ListpaymentNotBus = ko.observableArray([
       { lable: isEnLag == 'True' ? "Thanh toán trực tuyến" : "Online payment", value: 4 },
        { lable: isEnLag == 'True' ? "Chuyển khoản" : "Bank Transfer", value: 2 },
    ]);
    self.ListTypeShip = ko.observableArray([
        { lable: isEnLag == 'True' ? "Bưu điện/Chuyển phát nhanh" : "Post", value: 1 },
        { lable: isEnLag == 'True' ? "Xe khách" : "Coach", value: 2 }
    ]);
    self.giogiao = ko.observableArray([ 
          { gio: isEnLag == 'True' ? "Giờ hành chánh" : "Office hours", value: 2 },
          { gio: isEnLag == 'True' ? "Ngoài giờ hành chánh" : "Out of office hours", value: 3 },
          { gio: isEnLag == 'True' ? "Bất kỳ giờ nào trong ngày" : "Any time of day", value: 4 }
    ]);

    self.waitloader = ko.observable(false);
    self.redirectHome = function () {
        window.location.href = isEnLag == 'True' ? '/' : '/en';
        self.messenger('Đang chuyển trang vui lòng đợi....');
        self.waitloader(true);
    }
    self.Success = function () {
        window.location.href = '#/success';
    };
    self.phithuho = ko.observable(0);
    self.namePtgh = ko.observable();
    self.namePttt = ko.observable();
    self.nameTp = ko.observable();
    self.nameQuan = ko.observable();
    self.nametimeship = ko.observable();
    self.diachi = ko.computed(function () {
        return self.mCustomer().duong() + ' ,' + self.nameQuan() + ' ,' + self.nameTp();
    });
    self.LoadCart = function () {
        self.Isloadercart(true);
        $.ajax({
            type: "POST",
            url: '/CheckoutPlan/LoadCart',
            cache: false,
            success: function (data) {

                ko.mapping.fromJS(data, {}, self.mCheckout);


                var tmpPlanModel = ko.observableArray();
                var tmpPlanGiftModel = ko.observableArray();
                if (self.mCheckout().PlanModel()) {
                    ko.utils.arrayForEach(self.mCheckout().PlanModel(), function (obj) {
                        tmpPlanModel.push(ko.mapping.fromJS(obj, {}, new Checkoutplan.mPlanModel));
                    });
                }
                if (self.mCheckout().Gift()) {
                    tmpPlanGiftModel(CommonUtils.MapArray(self.mCheckout().Gift(), Checkoutplan.mGift));
                    //ko.utils.arrayForEach(self.mCheckout().Gift(), function (obj) {
                    //    tmpPlanGiftModel.push(ko.mapping.fromJS(obj, {}, new Checkoutplan.mGift));
                    //});
                }
                self.mCheckout().PlanModel(tmpPlanModel());
                self.mCheckout().Gift(tmpPlanGiftModel());
            }
        }).always(function () {
            self.Isloadercart(false);
        });
    };
    self.giogiaocuthe = ko.observable();
    self.Thanhpho = ko.observableArray();
    self.Vung = ko.observableArray();
    self.Gio = ko.observableArray();
    self.Quan = ko.observableArray();
    self.selectIdtp = ko.observable();
    self.selectIdquan = ko.observable();
    self.shipvalue = ko.observable(0);
    self.mavung = ko.observable();
    self.selectTypeShip = ko.observable();
    self.VisibleTp = ko.observable(false);
    self.Infxedo = ko.observable();
    self.IsShipxedo = ko.observable(false);
    self.selectTypeShip.subscribe(function (obj) {

        if (obj.value == 2) {
            self.IsShipxedo(true);
            self.phithuho(0);
            self.shipvalue(0);

        } else {
            self.IsShipxedo(false);
            var quan = self.mCustomer().idquan();
            self.mCustomer().idquan(undefined);
            self.mCustomer().idquan(quan);
        }
    });
    self.ListTinhtra = ko.observableArray();
    self.Validator = ko.computed(function () {
        return (ko.validatedObservable(self.mCustomer()))();
    });
    self.HasError = ko.observable();
    self.IssessionKhachhang = ko.observable(false);
    self.thoigiangiaohang = ko.observable();
    self.mCustomer().idtp.subscribe(function (id) {
        self.Isloadercart(true);
        var phat = ko.utils.arrayFirst(self.Thanhpho(), function (obj) {
            return obj.id() == id
        });
        if (id == 1) {
            self.typeconfim(1);
        } else {
            self.typeconfim(0);
        }
        if (phat)
            self.thoigiangiaohang(phat.thoigian);
        $.ajax({
            type: "POST",
            url: '/Admin/Shipping/GetTpById',
            data: { id: id },
            cache: false,
            success: function (data) {
                debugger
                if (id == 1) {
                    self.VisibleTp(true);
                } else {
                    self.VisibleTp(false);
                };
                if (isEnLag == 'False') {
                    ko.utils.arrayForEach(data.donhang_chuyenphat_tinh, function (obj) {

                        if (obj.tentinh_us)
                            obj.tentinh = obj.tentinh_us;
                    })
                };
                self.Quan(CommonUtils.MapArray(data.donhang_chuyenphat_tinh, Checkoutplan.mTinh));
                self.mCustomer().selectPayment(undefined);
                self.mCustomer().selectGio(undefined);
                // if (self.sessionIdtinh())
                //  self.selectIdquan(self.tmpidtinh());
                //  self.getPriceShip(self.selectIdquan());
                self.mCustomer().idquan(self.sessionIdQuan());
            }
        }).always(function () {
            self.Isloadercart(false);
        });
    });
    self.mCustomer().idquan.subscribe(function (id) {

        if (id != undefined) {
            if (self.mCustomer().idtp() == 1) {
                if (self.mCheckout().TotalSum() < 150000
                    && id != 127
                    && id != 134
                    && id != 135
                    && id != 136
                    && id != 137)
                    $('#modal_choosepbuy').modal('show');
            }
            self.getPriceShip(id);
        }
    });
    self.mCustomer().selectGio.subscribe(function (id) {
        debugger
        var idquan = self.mCustomer().idquan();
        if (id == 3) {
            $('#modal_selectngoaigiohanhchanh').modal('show');
            self.getPriceShip(idquan);
        }
        else {
            if (idquan == 127
                || idquan == 134
                || idquan == 135
                || idquan == 136
                || idquan == 137
                || self.mCheckout().TotalSum() < 150000)
                self.getPriceShip(idquan); 
            else
                self.shipvalue(0);
        }
    });
    self.getPriceShip = function (id) {
        if (id && self.mCustomer().idtp()) {
            self.shipvalue(0);
            if (self.mCustomer().idtp() == 1) {
                if (self.mCheckout().TotalSum() < 150000
                    || self.mCustomer().idquan() == 134
                    || self.mCustomer().idquan() == 135
                    || self.mCustomer().idquan() == 136
                    || self.mCustomer().idquan() == 137) {
                    if (self.Quan().length > 0) {
                        ship = ko.utils.arrayFirst(self.Quan(), function (obj) {
                            return obj.id() == id
                        })
                        if (ship != undefined)
                            self.shipvalue(ship.ship());
                    }
                }
                if (self.mCustomer().selectGio() == 3) {
                    if (self.Quan().length > 0) {
                        ship = ko.utils.arrayFirst(self.Quan(), function (obj) {
                            return obj.id() == id
                        })
                        if (ship != undefined)
                            self.shipvalue(ship.ship());
                    }
                }
            }
            else {
                var tinhgiao = ko.utils.arrayFirst(self.Thanhpho(), function (item) {
                    return item.id() == self.mCustomer().idtp();
                });
                var arrVung = [];
                ko.utils.arrayForEach(self.Vung(), function (item) {
                    if (item.mavung() == tinhgiao.mavung())
                        arrVung.push(item);
                });
                var kg = self.mCheckout().KgTotal();

                var ship = 0;
                if (kg <= 2) {
                    switch (true) {
                        case (kg <= 0.05):
                            ko.utils.arrayForEach(arrVung, function (item) {
                                if (item.kilogram() == 0.05)
                                    ship = item.ship();
                            });
                            break;
                        case (kg > 0.05 && kg <= 0.1):
                            ko.utils.arrayForEach(arrVung, function (item) {
                                if (item.kilogram() == 0.1)
                                    ship = item.ship();
                            });
                            break;
                        case (kg > 0.1 && kg <= 0.25):
                            ko.utils.arrayForEach(arrVung, function (item) {
                                if (item.kilogram() == 0.25)
                                    ship = item.ship();
                            });
                            break;
                        case (kg > 0.25 && kg <= 0.50):
                            ko.utils.arrayForEach(arrVung, function (item) {
                                if (item.kilogram() == 0.50)
                                    ship = item.ship();
                            });
                            break;
                        case (kg > 0.5 && kg <= 1):
                            ko.utils.arrayForEach(arrVung, function (item) {
                                if (item.kilogram() == 1)
                                    ship = item.ship();
                            });
                            break;
                        case (kg > 1 && kg <= 1.5):
                            ko.utils.arrayForEach(arrVung, function (item) {
                                if (item.kilogram() == 1.5)
                                    ship = item.ship();
                            });
                            break;
                        case (kg > 1.5 && kg <= 2):
                            ko.utils.arrayForEach(arrVung, function (item) {
                                if (item.kilogram() == 2)
                                    ship = item.ship();
                            });
                            break;
                    }
                }
                else {

                    var kg2 = ko.utils.arrayFirst(arrVung, function (item) {
                        return item.kilogram() == 2;
                    });
                    var kg05 = ko.utils.arrayFirst(arrVung, function (item) {
                        return item.kilogram() == 0;
                    });
                    var kgdu = 0;
                    while (kg > 2) {
                        ship += kg2.ship();
                        kg -= 2;
                        kgdu = kg;
                    }
                    if (kgdu >= 0.5) {
                        while (kgdu >= 0.5) {
                            ship += kg05.ship();
                            kgdu -= 0.5;
                        }

                    }
                }
                ship = ship * 1.23 * 1.1;
                var ischecktinhtra = self.ListTinhtra().map(function (e) { return e.IdTinhtra(); }).indexOf(self.mCustomer().idquan());
                if (ischecktinhtra <= -1)
                    ship = ship * 1.2;
                self.shipvalue(ship);
            }
        }
    };
    self.SubmitCartLink = function () {
        if (self.Validator().isValid()) {

            var Tp = ko.utils.arrayFirst(self.Thanhpho(), function (obj) {
                return obj.id() == self.mCustomer().idtp()
            });
            var Quan = ko.utils.arrayFirst(self.Quan(), function (obj) {
                return obj.id() == self.mCustomer().idquan()
            });
            if (self.mCustomer().idtp() == 1) {
                var pay = ko.utils.arrayFirst(self.ListpaymentCity(), function (obj) {
                    return obj.value == self.mCustomer().selectPayment()
                });
            }
            else if (self.mCustomer().idtp() != 1) {
                var pay = ko.utils.arrayFirst(self.ListpaymentNotCity(), function (obj) {
                    return obj.value == self.mCustomer().selectPayment()
                });
            }
            if (self.mCustomer().idtp() == 1 && self.mCustomer().selectGio()) {
                Timeship = ko.utils.arrayFirst(self.giogiao(), function (obj) {
                    var u = typeof self.mCustomer().selectGio() === 'number' && (self.mCustomer().selectGio() % 1) === 0 ? self.mCustomer().selectGio() : self.mCustomer().selectGio().value;
                    return obj.value == u
                });
                self.nametimeship(Timeship.gio);
            }
            else {
                var ship = ko.utils.arrayFirst(self.ListTypeShip(), function (obj) {
                    return obj.value == self.selectTypeShip().value
                });
                self.namePtgh(ship.lable);
            }
            self.namePttt(pay.lable);
            self.nameTp(Tp.tentp());
            self.nameQuan(Quan.tentinh());
            // self.diemsanpham();
            $('#modal_confimcheckout').modal('show');
        }
        else {
            self.HasError(true);
            self.Validator().errors.showAllMessages();
        }
    };
    self.OrderCart = function () {
        if (self.Validator().isValid()) {
            if (self.mCustomer().idtp() == 1) { 
                if (self.Validator().isValid()) { 
                    $('#modal_confimcall').modal('show');
                }
            }
            else { 
                self.SubmitCartLink();
            }
        }
        else {
            self.HasError(true);
            self.Validator().errors.showAllMessages();
        }
    };
    self.SubmitCartLinkForMobile = function () {
        self.SubmitCartLink();
        self.SubmitCart();
    };
    self.SubmitCart = function () {
        if (self.Validator().isValid()) {
            self.messenger(isEnLag == 'True' ? 'Hệ thống đang xử lý, vui lòng đợi....' : 'Processing, please wait...');
            self.waitloader(true);
            debugger
            var donhang = new Checkoutplan.mDonhang();
            donhang.makh(self.mCustomer().MaKH());
            donhang.ghichu(self.NoteCheckout())
            donhang.ship(self.shipvalue());
            //donhang.tongtien(self.mCheckout().TotalSum());
            donhang.tongtien(self.totalcartmoney());
            donhang.pttt(self.mCustomer().selectPayment());
            donhang.ptgh(self.selectTypeShip().value);
            donhang.thongtinxedo(self.Infxedo());
            donhang.typeconfim(self.typeconfim());
            donhang.NLpayBankType(self.SelectedNLpayBankType());
            donhang.BankOnline(self.SelectedBankOnline());
            ko.utils.arrayForEach(self.mCheckout().PlanModel(), function (obj) {
                var ctdh = new Checkoutplan.mDonghangCt();
                ctdh.IdPro(obj.IdProdut());
                ctdh.Soluong(obj.Count());
                ctdh.Dongia(obj.gia());
                donhang.donhang_ct.push(ctdh);
            });
            var val = $('#contentemail')[0].outerHTML;
            var model = ko.toJSON({ dh: donhang, kh: self.mCustomer(), content: val });
            $.ajax({
                type: "POST",
                url: '/CheckoutPlan/Checkout',
                cache: false,
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                data: model,
                success: function (data) {
                    window.location.href = data;
                }
            }).always(function () {
                self.waitloader(false);
            });
        }
    }
    self.sumdiem = ko.computed(function () {
        return parseInt(self.mCustomer().diem()) + parseInt(self.mCustomer().diemsp());
    });
    self.diemsanpham = function () {
        self.mCustomer().diemsp(0);
        if (self.IssessionKhachhang()
            && self.mCheckout().PlanModel().length > 0
            && self.mCheckout().ToTal() < 1000000) {
            var diem = 0;
            ko.utils.arrayForEach(self.mCheckout().PlanModel(), function (obj) {
                if (obj.gia() > 0) {
                    var u = obj.Count() * obj.gia();
                    diem += u;
                }
            });
            var resultdiem = diem / 1000;
            self.mCustomer().diemsp(resultdiem);
        }
    };
    self.mCustomer().selectPayment.subscribe(function (id) {
        if (id == 3) {
            $('#modal_selectPayment_thuho').modal('show');
            if (self.mCheckout().TotalSum() < 300000) {
                self.phithuho(25000);
            } else if (self.mCheckout().TotalSum() >= 300000 && self.mCheckout().TotalSum() < 600000) {
                self.phithuho(35000);
            }
            else if (self.mCheckout().TotalSum() >= 600000 && self.mCheckout().TotalSum() < 1000000) {
                self.phithuho(40000);
            }
            else if (self.mCheckout().TotalSum() >= 1000000) {
                var o = Math.floor(self.mCheckout().TotalSum() / 1000000);
                self.phithuho(40000 + (o * 20000));
            }
        }
        else {
            self.phithuho(0);
        }
    });
    self.totalcartmoney = ko.computed(function () {
        return self.mCheckout().TotalSum() + self.shipvalue() + self.phithuho();
    });
    self.Start = function () {
        ko.applyBindings(self);
        self.LoadComponese();
    };
    self.LoadComponese = function () {
        self.Isloadercart(true);
        $.ajax({
            type: "GET",
            url: '/CheckoutPlan/LoadComponese',
            success: function (data) {
                if (isEnLag == 'False') {
                    ko.utils.arrayForEach(data.city, function (obj) {

                        if (obj.tentp_us)
                            obj.tentp = obj.tentp_us;
                    })
                };
                self.Thanhpho(CommonUtils.MapArray(data.city, Checkoutplan.mThanhpho));
                self.Vung(CommonUtils.MapArray(data.vung, Checkoutplan.mVung));
                self.ListTinhtra(CommonUtils.MapArray(data.tinhtra, Checkoutplan.Chuyenphat_tinh_tra));
                ko.mapping.fromJS(data.cart, {}, self.mCheckout);
                if (sessionKhachhang) {
                    ko.mapping.fromJS(sessionKhachhang, {}, self.mCustomer());
                    self.sessionIdQuan(sessionKhachhang.idquan);
                    self.IssessionKhachhang(true);
                    self.diemsanpham();
                }
                var tmpPlanModel = ko.observableArray();
                var tmpPlanGiftModel = ko.observableArray();
                if (self.mCheckout().PlanModel()) {
                    ko.utils.arrayForEach(self.mCheckout().PlanModel(), function (obj) {
                        tmpPlanModel.push(ko.mapping.fromJS(obj, {}, new Checkoutplan.mPlanModel));
                    });
                }
                if (self.mCheckout().Gift()) {
                    tmpPlanGiftModel(CommonUtils.MapArray(self.mCheckout().Gift(), Checkoutplan.mGift));
                }
                self.mCheckout().PlanModel(tmpPlanModel());
                self.mCheckout().Gift(tmpPlanGiftModel());


            }
        }).always(function () {
            self.Isloadercart(false);
        });
    }

};