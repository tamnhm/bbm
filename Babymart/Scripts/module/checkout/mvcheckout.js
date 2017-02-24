var Checkout = Checkout || {};
Checkout.mvCheckout = function () {
    var self = this;

    self.mCheckout = ko.observable(new Checkout.mCheckout());
    self.mCustomer = ko.observable(new Checkout.mCustomer());
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
     ]);
    self.NLpayBankType = ko.observableArray(
             [{ name: isEnLag == 'True' ? "ATM nội địa" : "Local ATM", code: 'ATM_ONLINE' },
              { name: isEnLag == 'True' ? "Thẻ tín dụng (Visa/MasterCard/JCB)" : "Credit Card (Visa/MasterCard/JCB)", code: 'VISA' }
             ]);
    //self.SelectedNLpayBankType = ko.observable();
    //self.SelectedBankOnline = ko.observable();
    /**********************NL********************************/
    self.Isloadercart = ko.observable(false);
    self.NoteCheckout = ko.observable();
    self.loadfullQuan = ko.observable(false);
    self.messenger = ko.observable();
    self.sessionIdQuan = ko.observable();
    self.typeconfim = ko.observable();
    self.ListpaymentCity = ko.observableArray([
       { lable: isEnLag == 'True' ? "Thanh toán trực tuyến" : "Online payment", value: 4 },
       { lable: isEnLag == 'True' ? "Chuyển khoản" : "Bank Transfer", value: 2 },
       { lable: isEnLag == 'True' ? "Bằng thẻ ngân hàng khi nhận hàng" : "Bank card on delivery", value: 5 },
       { lable: isEnLag == 'True' ? "Bằng tiền mặt khi nhận hàng" : "Cash on delevery", value: 1 },
    ]);
    self.ListpaymentNotCity = ko.observableArray([
        { lable: isEnLag == 'True' ? "Thanh toán trực tuyến" : "Online payment", value: 4 },
        { lable: isEnLag == 'True' ? "Chuyển khoản" : "Bank Transfer", value: 2 },
        { lable: isEnLag == 'True' ? "Tiền mặt" : "Cash", value: 3 },
    ]);
    self.ListpaymentNotBus = ko.observableArray([
        { lable: isEnLag == 'True' ? "Thanh toán trực tuyến" : "Online payment", value: 4 },
        { lable: isEnLag == 'True' ? "Chuyển khoản" : "Bank Transfer", value: 2 },
    ]);
    self.ListTypeShip = ko.observableArray([
        { lable: isEnLag == 'True' ? "Bưu điện/Chuyển phát nhanh" : "Post", value: 1 },
        { lable: isEnLag == 'True' ? "Xe khách" : "Coach", value: 2 },
    ]);
    self.giogiao = ko.observableArray([
            { gio: isEnLag == 'True' ? "Giờ hành chính " : "Office hours", value: 2 },
            { gio: isEnLag == 'True' ? "Ngoài giờ hành chính" : "Out of office hours", value: 3 },
            { gio: isEnLag == 'True' ? "Bất kỳ giờ nào trong ngày" : "Any time of day", value: 4 }
    ]);

    self.phithuho = ko.observable(0);
    self.HasError = ko.observable();
    self.DisscountForCustomer = ko.observable(0);
    self.waitloader = ko.observable(false);
    self.redirectHome = function () {
        window.location.href = isEnLag == 'True' ? '/' : '/en';
        self.messenger('Đang chuyển trang vui lòng đợi....');
        self.waitloader(true);
    }
    self.Success = function () {
        window.location.href = '#/success';
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
    self.ListTinhtra = ko.observableArray();
    self.IssessionKhachhang = ko.observable(false);
    self.namePtgh = ko.observable();
    self.namePttt = ko.observable();
    self.nameTp = ko.observable();
    self.nameQuan = ko.observable();
    self.nametimeship = ko.observable();
    self.thoigiangiaohang = ko.observable();
    self.flagquan = ko.observable(false);
    self.diachi = ko.computed(function () {
        return self.mCustomer().duong() + ', ' + self.nameQuan() + ', ' + self.nameTp();
    });
    self.Validator = ko.computed(function () {
        return (ko.validatedObservable(self.mCustomer()));
    });
    self.mCustomer().idtp.subscribe(function (id) {
        self.Isloadercart(true);
        var phat = ko.utils.arrayFirst(self.Thanhpho(), function (obj) {
            return obj.id() == id
        });
        if (id == 1) {
            self.typeconfim(1);
        } else {
            self.typeconfim(3);
        }
        if (phat)
            self.thoigiangiaohang(phat.thoigian);
        $.ajax({
            type: "POST",
            url: '/Admin/Shipping/GetTpById',
            data: { id: id },
            cache: false,
            success: function (data) {
                if (id == 1) {
                    self.VisibleTp(true);
                } else {
                    self.VisibleTp(false);
                }
                if (isEnLag == 'False') {
                    ko.utils.arrayForEach(data.donhang_chuyenphat_tinh, function (obj) {

                        if (obj.tentinh_us)
                            obj.tentinh = obj.tentinh_us;
                    })
                };
                self.Quan(CommonUtils.MapArray(data.donhang_chuyenphat_tinh, Checkout.mTinh));
                self.mCustomer().selectPayment(undefined);
                self.mCustomer().selectGio(undefined);
                self.mCustomer().idquan(self.sessionIdQuan());
            }
        }).always(function () {
            self.Isloadercart(false);
        });
    });
    self.mCustomer().idquan.subscribe(function (id) {
        if (id != undefined) {
            if (self.mCustomer().idtp() == 1) {
                if (self.mCheckout().CartTotal() < 150000
                    && id != 127
                    && id != 134
                    && id != 135
                    && id != 136
                    && id != 137)
                    //$('#modal_choosepbuy').modal('show');
                    self.flagquan(true);
            }
            self.getPriceShip(id);
        }
    });
    self.mCustomer().selectPayment.subscribe(function (id) {
        if (id == 3) {
            //$('#modal_selectPayment_thuho').modal('show');
            if (self.mCheckout().CartTotal() < 300000) {
                self.phithuho(25000);
            } else if (self.mCheckout().CartTotal() >= 300000 && self.mCheckout().CartTotal() < 600000) {
                self.phithuho(35000);
            }
            else if (self.mCheckout().CartTotal() >= 600000 && self.mCheckout().CartTotal() < 1000000) {
                self.phithuho(40000);
            }
            else if (self.mCheckout().CartTotal() >= 1000000) {
                var o = Math.floor(self.mCheckout().CartTotal() / 1000000);
                self.phithuho(40000 + (o * 20000));
            }
        }
        else {
            self.phithuho(0);
        }
    });
    self.mCustomer().selectGio.subscribe(function (id) {
        var idquan = self.mCustomer().idquan();
        if (id == 3) {
            //$('#modal_selectngoaigiohanhchanh').modal('show');

            self.getPriceShip(idquan);
        }
        else {
            if (idquan == 127
                || idquan == 134
                || idquan == 135
                || idquan == 136
                || idquan == 137
                || self.mCheckout().CartTotal() < 150000)
                self.getPriceShip(idquan);
                //if (self.mCheckout().CartTotal() < 150000)
                //    self.getPriceShip(idquan);
            else
                self.shipvalue(0);
        }
    });
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
    self.diemsanpham = function () {
        if (self.IssessionKhachhang() && self.mCheckout().CartItemModel().length > 0) {
            var diem = 0;
            ko.utils.arrayForEach(self.mCheckout().CartItemModel(), function (obj) {
                if (!obj.shop_bienthe.giasosanh() > 0) {
                    var u = obj.Count() * obj.shop_bienthe.gia();
                    diem += u;
                }
            });
            var resultdiem = diem / 1000;
            self.mCustomer().diemsp(resultdiem);
        }
    };
    self.tongtiencothegiam = ko.observable(0);
    self.tinhtongtiencothegiam = function () {
        if (self.mCheckout().CartItemModel().length > 0) {
            var tongtien = 0;
            ko.utils.arrayForEach(self.mCheckout().CartItemModel(), function (obj) {
                if (obj.shop_bienthe.masp() == null) {
                    if (!obj.shop_bienthe.giasosanh() > 0 && obj.ProductId() != 23568 && obj.ProductId() != 23569 && obj.ProductId() != 1049 && obj.ProductId() != 1050 && obj.ProductId() != 1051 && obj.ProductId() != 1052 && obj.shop_bienthe.title().indexOf('TA') == -1 && obj.shop_bienthe.title().indexOf('T0') == -1 && obj.shop_bienthe.title().indexOf('T1') == -1 && obj.shop_bienthe.title().indexOf('T2') == -1 && obj.shop_bienthe.title().indexOf('T3') == -1 && obj.shop_bienthe.title().indexOf('T4') == -1 && obj.shop_bienthe.title().indexOf('T5') == -1 && obj.shop_bienthe.title().indexOf('T6') == -1 && obj.shop_bienthe.title().indexOf('T7') == -1 && obj.shop_bienthe.title().indexOf('T8') == -1 && obj.shop_bienthe.title().indexOf('T9') == -1) {
                        var u = obj.Count() * obj.shop_bienthe.gia();
                        tongtien += u;
                    }
                }
                else {
                    if (!obj.shop_bienthe.giasosanh() > 0 && obj.ProductId() != 23568 && obj.ProductId() != 23569 && obj.ProductId() != 1049 && obj.ProductId() != 1050 && obj.ProductId() != 1051 && obj.ProductId() != 1052 && obj.shop_bienthe.masp().indexOf('TA') != 0 && obj.shop_bienthe.masp().slice(0, 1) != 'T') {
                        var u = obj.Count() * obj.shop_bienthe.gia();
                        tongtien += u;
                    }
                }
            });
            self.tongtiencothegiam(tongtien);
        }
    };
    self.totalmoney = ko.observable(0);
    //-------------------Ship-------------------

    self.getPriceShip = function (id) {
        if (id && self.mCustomer().idtp()) {
            self.shipvalue(0);
            if (self.mCustomer().idtp() == 1) {
                if (self.mCheckout().CartTotal() < 150000
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
                if (tinhgiao != null)
                    ko.utils.arrayForEach(self.Vung(), function (item) {
                        if (item.mavung() == tinhgiao.mavung())
                            arrVung.push(item);
                    });
                var kg = self.mCheckout().KgTotal();
                if (parseInt(self.mCheckout().KgTotal()) < self.mCheckout().WHL()) {
                    kg = self.mCheckout().WHL();
                }


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
                    var kgdu = kg - 2;
                    var rm = kgdu / 0.5;
                    ship = kg2.ship() + (rm * kg05.ship());

                    //var kgdu = 0;
                    //while (kg > 2) {
                    //    ship += kg2.ship();
                    //    kg -= 2;
                    //    kgdu = kg;
                    //}
                    //if (kgdu >= 0.5) {
                    //    while (kgdu >= 0.5) {
                    //        ship += kg05.ship();
                    //        kgdu -= 0.5;
                    //    }

                    //}
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
            //self.diemsanpham();
            //$('#modal_confimcheckout').modal('show');
            if (self.mCustomer().idtp() == 1) {
                $('#modal_confimcall').modal('show');
            }
            else {
                self.SubmitCart();
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

    self.SubmitCart = function () {
        if (self.Validator().isValid()) {
            self.waitloader(true);
            self.messenger(isEnLag == 'True' ? 'Hệ thống đang xử lý, vui lòng đợi....' : 'Processing, please wait...');
            var donhang = new Checkout.mDonhang();
            donhang.makh(self.mCustomer().MaKH());
            donhang.ghichu(self.NoteCheckout())
            donhang.ship(self.shipvalue());
            //donhang.tongtien(self.mCheckout().CartTotal());
            donhang.tongtien(self.totalcartmoney());
            donhang.pttt(self.mCustomer().selectPayment());
            donhang.ptgh(self.selectTypeShip().value);
            donhang.thongtinxedo(self.Infxedo());
            donhang.datru_diem(self.datru_diem());
            donhang.typeconfim(self.typeconfim());
            donhang.NLpayBankType(self.mCustomer().SelectedNLpayBankType());
            donhang.BankOnline(self.mCustomer().SelectedBankOnline());
            ko.utils.arrayForEach(self.mCheckout().CartItemModel(), function (obj) {
                var ctdh = new Checkout.mDonghangCt();
                ctdh.IdPro(obj.ProductId());
                ctdh.Soluong(obj.Count());
                ctdh.Dongia(obj.shop_bienthe.gia());
                ctdh.DongiaSS(obj.shop_bienthe.giasosanh());
                donhang.donhang_ct.push(ctdh);
            });
            var val = $('#contentemail')[0].outerHTML;
            var model = ko.toJSON({ dh: donhang, kh: self.mCustomer(), content: val });
            $.ajax({
                type: "POST",
                url: '/Checkout/Checkout',
                cache: false,
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                data: model,
                success: function (data) {
                    window.location.href = data;
                    //window.location.href = '/xac-nhan-thanh-cong.html/' + data;
                }
            }).always(function () {
                self.waitloader(false);
            });
        }
    }

    //-------------------computed-------------------
    self.totalcartmoney = ko.computed(function () {
        var result = self.mCheckout().CartTotal() + self.shipvalue() + self.phithuho();
        if (self.DisscountForCustomer() > 0)
            result = result - self.DisscountForCustomer()
        return result
    });
    self.sumdiem = ko.computed(function () {
        return parseInt(self.mCustomer().diem()) + parseInt(self.mCustomer().diemsp());
    });

    self.Start = function () {
        ko.applyBindings(self);
        self.LoadComponese();
    };
    self.TotalSaleOff = ko.observable();
    self.LoadComponese = function () {
        self.Isloadercart(true);
        $.ajax({
            type: "GET",
            url: '/Checkout/LoadComponese',
            success: function (data) {
                if (isEnLag == 'False') {
                    ko.utils.arrayForEach(data.city, function (obj) {
                        if (obj.tentp_us)
                            obj.tentp = obj.tentp_us;
                    })
                };
                self.Thanhpho(CommonUtils.MapArray(data.city, Checkout.mThanhpho));
                self.Vung(CommonUtils.MapArray(data.vung, Checkout.mVung));
                self.ListTinhtra(CommonUtils.MapArray(data.tinhtra, Checkout.Chuyenphat_tinh_tra));
                ko.mapping.fromJS(data.cart, {}, self.mCheckout);
                self.totalmoney(self.mCheckout().CartTotal());
                if (sessionKhachhang) {
                    ko.mapping.fromJS(sessionKhachhang, {}, self.mCustomer());
                    var isshow_disscount = false;
                    var countItem = self.mCheckout().CartItemModel().length;
                    var countforItemselfoff = 0;
                    var totalSaleOff = 0;
                    ko.utils.arrayForEach(self.mCheckout().CartItemModel(), function (val) {
                        if (val.shop_bienthe.giasosanh() > 0) {
                            countforItemselfoff = countforItemselfoff + 1;
                        } else {
                            totalSaleOff = totalSaleOff + val.shop_bienthe.gia();
                        }
                    });
                    self.tinhtongtiencothegiam();
                    self.diemsanpham();
                    debugger
                    if (self.mCustomer().diem() >= 1000 && countforItemselfoff != countItem && self.tongtiencothegiam() > 0 && sessiondatrudiem != 1000) {
                        $('#modal_disscount').modal('show');
                    }
                    else if (self.mCustomer().diem() >= 1000 && countforItemselfoff != countItem && self.tongtiencothegiam() > 0 && sessiondatrudiem == 1000) {
                        self.cal_disscount();
                    }
                    
                    self.TotalSaleOff(totalSaleOff);
                    self.sessionIdQuan(sessionKhachhang.idquan);
                    self.IssessionKhachhang(true);
                    
                } else {
                    //if (data.isHasSaleoff)
                    //    $('#modal_showRegister').modal('show');
                }
            }
        }).always(function () {
            self.Isloadercart(false);
        });
    };
    self.datru_diem = ko.observable(0);
    self.isDisplayonePoupDisscountForCustomer = ko.observable(true);
    self.cal_disscount = function () {
        var totaldisscount = 0;
        ko.utils.arrayForEach(self.mCheckout().CartItemModel(), function (obj) {
            if (obj.shop_bienthe.masp() == null) {
                if (!obj.shop_bienthe.giasosanh() > 0 && obj.ProductId() != 23568 && obj.ProductId() != 23569 && obj.ProductId() != 1049 && obj.ProductId() != 1050 && obj.ProductId() != 1051 && obj.ProductId() != 1052 && obj.shop_bienthe.title().indexOf('TA') == -1 && obj.shop_bienthe.title().indexOf('T0') == -1 && obj.shop_bienthe.title().indexOf('T1') == -1 && obj.shop_bienthe.title().indexOf('T2') == -1 && obj.shop_bienthe.title().indexOf('T3') == -1 && obj.shop_bienthe.title().indexOf('T4') == -1 && obj.shop_bienthe.title().indexOf('T5') == -1 && obj.shop_bienthe.title().indexOf('T6') == -1 && obj.shop_bienthe.title().indexOf('T7') == -1 && obj.shop_bienthe.title().indexOf('T8') == -1 && obj.shop_bienthe.title().indexOf('T9') == -1) {
                    var u = obj.Count() * obj.shop_bienthe.gia();
                    totaldisscount += u;
                }
            }
            else {
                if (!obj.shop_bienthe.giasosanh() > 0 && obj.ProductId() != 23568 && obj.ProductId() != 23569 && obj.ProductId() != 1049 && obj.ProductId() != 1050 && obj.ProductId() != 1051 && obj.ProductId() != 1052 && obj.shop_bienthe.masp().indexOf('TA') != 0 && obj.shop_bienthe.masp().slice(0, 1) != 'T') {
                    var u = obj.Count() * obj.shop_bienthe.gia();
                    totaldisscount += u;
                }
            }
        });
        self.DisscountForCustomer((totaldisscount * 5) / 100)

        if (totaldisscount != 0) {
            if (sessiondatrudiem != 1000) {
                var diem = self.mCustomer().diem() - 1000; 
            }
            else {
                var diem = self.mCustomer().diem();
            }
            self.datru_diem(1000);
            self.mCustomer().diem(diem);
        }
        var diemspsaugiam = (self.totalmoney() - (self.DisscountForCustomer())) / 1000;
        self.mCustomer().diemsp(diemspsaugiam.toFixed(0));
        $('#modal_disscount').modal('hide');
        
    };
};
