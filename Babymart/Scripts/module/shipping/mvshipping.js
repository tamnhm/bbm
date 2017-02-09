var Shipping = Shipping || {};
ko.bindingHandlers.option = {
    update: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        ko.selectExtensions.writeValue(element, value);
    }
};
Shipping.m_dropdownlist = function () {
    var self = this;
    self.Id = ko.observable();
    self.Name = ko.observable();
    self.IsSelected = ko.observable(false);
}
Shipping.mvShipping = function () {
    var self = this;
    self.OptionArrayTp = ko.observableArray([]);
    self.OptionVung = ko.observableArray(["A", "B", "C", "D", "E", "F", "G", "H", "I"]);

    self.tinhthanhmoi = ko.observableArray();
    self.selectVung = ko.observable();
    self.ArrayTinh = ko.observableArray();
    self.ArrayVung = ko.observableArray();

    self.isNotSelect = ko.observable(false);
    self.Tp = ko.observable(new Shipping.Chuyenphat_tp());
    self.Tinh = ko.observable(new Shipping.Chuyenphat_tinh());
    self.Char = ko.observableArray();
    self.selectchar = ko.observable();
    self.HasError = ko.observable(false);
    self.TpSelectedValue = ko.observable();
    self.LoadArrayTp = function () {
        CommonUtils.showWait(true);
        $.ajax({
            type: "GET",
            url: '/Admin/Shipping/GetCity',
            success: function (data) {
                self.OptionArrayTp(CommonUtils.MapArray(data, Shipping.Chuyenphat_tp));
            }
        }).always(CommonUtils.showWait(false));
    };
    self.tphcm = ko.observable(false);
    self.idtinhtra = ko.observable();
    self.LoadData = function (id) {
        $.ajax({
            type: "GET",
            url: '/Admin/Shipping/GetTpById',
            data: { id: id },
            success: function (data) {
                ko.mapping.fromJS(data, {}, self.Tp());
               // self.idtinhtra(data.idtinhtra);
                self.selectVung(data.mavung);
            }
        }).always(function () {
            self.loadtinhtra(id);
        });  
    };
    self.TpSelectedValue.subscribe(function (newValue) {
        self.Tp().ArrayTinh([]);
        self.tinhthanhmoi([]);
        if (newValue == undefined) {
            self.isNotSelect(false);
        }
        else {
            self.isNotSelect(true);
            if (newValue == 1)
                self.tphcm(true);
            else
                self.tphcm(false);
            self.LoadData(newValue);
        }
    });
    self.selectVung.subscribe(function (obj) {
        $.ajax({
            type: "POST",
            url: '/Admin/Shipping/GetVung',
            data: { mavung: obj },
            success: function (data) {
                self.ArrayVung(CommonUtils.MapArray(data, Shipping.Chuyenphat_vung));
            }
        });
    });
    self.addtinhthanh = function () {
        var d = new Shipping.Chuyenphat_tinh();
        self.tinhthanhmoi.push(d);
    };
    self.removetinhthanh = function (obj) {
        if (obj.id() == undefined) {
            self.tinhthanhmoi.remove(obj);
        }
        else {
            bootbox.confirm("Bạn muốn xóa tỉnh <strong>" + obj.tentinh() + "</strong> ?", function (result) {
                if (result) {
                    $.ajax({
                        type: 'POST',
                        url: CommonUtils.url('/Admin/Shipping/removeTinh'),
                        cache: false,
                        data: { id: obj.id() },
                        success: function (r) {
                            if (r.success) {
                                $.gritter.add({
                                    title: 'Thông báo',
                                    text: r.messaging
                                });
                                self.LoadData(self.Tp().id());
                            }
                            else { alert(r.messaging) }
                        }
                    }).always(CommonUtils.showWait(false));
                }
            });
        }
    };
    self.SubmitSave = function () {
        //self.Tp().idtinhtra(self.idtinhtra());
        self.Tp().mavung(self.selectVung());
        if (self.tinhthanhmoi().length > 0) {
            ko.utils.arrayForEach(self.tinhthanhmoi(), function (o) {
                if (o.tentinh())
                    self.Tp().donhang_chuyenphat_tinh.push(o)
            })
        }
        if (self.Validator().isValid()) {
            var input = ko.toJSON({ model: self.Tp() });
            $.ajax({
                type: 'POST',
                url: CommonUtils.url('/Admin/Shipping/UpdateTp'),
                cache: false,
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                data: input,
                success: function (r) {
                    if (r.success) {
                        $.gritter.add({
                            title: 'Thông báo',
                            text: r.messaging
                        });

                        self.tinhthanhmoi([]);
                        self.LoadData(self.Tp().id());
                    }
                    else { alert(r.messaging) }
                }
            }).always(CommonUtils.showWait(false));
        } else {
            self.HasError(true);
            self.Validator().errors.showAllMessages();
        }
    };

    self.Validator = ko.computed(function () {
        return (ko.validatedObservable(self.Tp()))();
    });
    self.addthanhpho = function (obj) {
        self.Tp(new Shipping.Chuyenphat_tp());
        self.isNotSelect(true);
        $('#modal-setting').modal('show');
    };



    self.chinhsuatinh = function (obj) {
        $('#modal-eidtinh').modal('show');
    };
    self.deletetinh = function (obj) {
        self.Tp().ArrayTinh.remove(obj);
    };
    self.modal_edittinh = function (obj) {
        $.ajax({
            type: 'POST',
            url: CommonUtils.url('/Admin/Shipping/updateTinh'),
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: ko.toJSON(obj),
            success: function (r) {
                if (r.success) {
                    $.gritter.add({
                        title: 'Thông báo',
                        text: r.messaging
                    });

                }
                else { alert(r.messaging) }
            }
        }).always(CommonUtils.showWait(false));
    };

    self.SumitAddTp = function (obj) {
        if (self.Validator().isValid()) {
            $.ajax({
                type: 'POST',
                url: CommonUtils.url('/Admin/Shipping/insertTp'),
                cache: false,
                data: { tentp: obj.tentp() },
                success: function (r) {
                    if (r.success) {
                        $('#modal-setting').modal('hide');
                        $.gritter.add({
                            title: 'Thông báo',
                            text: r.messaging
                        });
                        self.TpSelectedValue(r.id);
                        self.LoadArrayTp();


                    }
                    else { alert(r.messaging) }
                }
            }).always(CommonUtils.showWait(false));
        } else {
            self.HasError(true);
            self.Validator().errors.showAllMessages();
        }
    };


    self.SumitTp = function (obj) {
        if (self.Validator().isValid()) {
            var model = ko.toJSON({ dm: self.Tp(), tinh: self.Tp().ArrayTinh });
            if (self.Tp().id() == undefined) {
                debugger
                $.ajax({
                    type: 'POST',
                    url: CommonUtils.url('/Admin/Shipping/insertTp'),
                    cache: false,
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8',
                    data: model,
                    success: function (r) {
                        if (r.success) {
                            $.gritter.add({
                                title: 'Thông báo',
                                text: r.messaging
                            });
                            self.LoadArrayTp();
                            self.TpSelectedValue(r.id);
                            self.Tp().ArrayTinh([]);
                            self.tinhthanhmoi([]);
                        }
                        else { alert(r.messaging) }
                    }
                }).always(CommonUtils.showWait(false));
            }
            else {
                $.ajax({
                    type: 'POST',
                    url: CommonUtils.url('/Admin/Shipping/updateTp'),
                    cache: false,
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8',
                    data: model,
                    success: function (r) {
                        if (r.success) {
                            $('#modal-setting').modal('hide');
                            $.gritter.add({
                                title: 'Thông báo',
                                text: r.messaging
                            });
                            self.LoadArrayTp();
                            self.Tp().ArrayTinh([]);
                            self.ArrayTinh([]);
                            self.ArrayTinh(CommonUtils.MapArray(r.model, Shipping.Chuyenphat_tinh));

                        }
                        else { alert(r.messaging) }
                    }
                }).always(CommonUtils.showWait(false));
            }
        }
        else {
            debugger
            self.HasError(true);
            CommonUtils.showWait(false);
        }
    };
    self.SumitXoaTp = function (obj) {
        if (obj.id()) {
            bootbox.confirm("Bạn muốn xóa tỉnh <strong>" + obj.tentp() + "</strong> ?", function (result) {
                if (result) {
                    $.ajax({
                        type: 'POST',
                        url: CommonUtils.url('/Admin/Shipping/removeTp'),
                        cache: false,
                        data: { id: obj.id() },
                        success: function (r) {
                            if (r.success) {
                                $.gritter.add({
                                    title: 'Thông báo',
                                    text: r.messaging
                                });
                                self.isNotSelect(false);
                                self.LoadArrayTp();
                            }
                            else { alert(r.messaging) }
                        }
                    }).always(CommonUtils.showWait(false));
                }
            });
        }
    };

    self.shipvung = ko.observable();
    self.kgvung = ko.observable();
    self.removeVung = function (obj) {
        bootbox.confirm("Bạn muốn xóa phí giao hàng này", function (result) {
            if (result) {
                $.ajax({
                    type: 'POST',
                    url: CommonUtils.url('/Admin/Shipping/RemoveVung'),
                    cache: false,
                    data: { id: obj.id() },
                    success: function (r) {
                        if (r.success) {
                            $.gritter.add({
                                title: 'Thông báo',
                                text: r.messaging
                            });

                            self.selectVung(null);
                            self.selectVung(self.selectVung());
                        }
                        else { alert(r.messaging) }
                    }
                }).always(CommonUtils.showWait(false));
            }
        });
    };
    self.AddVung = function () {
        var vung = new Shipping.Chuyenphat_vung();
        vung.mavung(self.selectVung());
        vung.kilogram(self.kgvung());
        vung.ship(self.shipvung());
        var input = ko.toJSON({ model: vung });
        $.ajax({
            type: 'POST',
            url: CommonUtils.url('/Admin/Shipping/AddVung'),
            cache: false,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: input,
            success: function (r) {
                if (r.success) {
                    $.gritter.add({
                        title: 'Thông báo',
                        text: r.messaging
                    });
                    self.shipvung(null);
                    self.kgvung(null);
                    self.selectVung(null);
                    self.selectVung(self.selectVung());
                }
                else { alert(r.messaging) }
            }
        }).always(function () {
            CommonUtils.showWait(false)
        });

    };
    self.ListTinhtra = ko.observableArray();
    self.loadtinhtra = function (idtp) {
        debugger
        CommonUtils.showWait(true);
        $.ajax({
            type: "GET",
            url: '/Admin/Shipping/Gettinhtra',
            cache: false,
            data: { idtp: idtp },
            success: function (data) {
                debugger
                self.ListTinhtra(CommonUtils.MapArray(data, Shipping.Chuyenphat_tinh_tra));
            }
        }).always(CommonUtils.showWait(false));
    }
    self.themtinhtra = function () {
        var input = { idtp: self.TpSelectedValue(), tinh: self.idtinhtra() };
        $.ajax({
            type: 'POST',
            url: CommonUtils.url('/Admin/Shipping/Themtinhtra'),
            cache: false,
            data: input,
            success: function (r) {
                if (r.success) {
                    $.gritter.add({
                        title: 'Thông báo',
                        text: r.messaging
                    });
                    self.loadtinhtra(self.TpSelectedValue());
                }
                else { alert(r.messaging) }
            }
        }).always(function () {
            CommonUtils.showWait(false)
        });
    };
    self.xoatinhtra = function (data) {
        var input = { id: data.Id() };
        $.ajax({
            type: 'POST',
            url: CommonUtils.url('/Admin/Shipping/Xoatinhtra'),
            cache: false,
            data:input,
            success: function (r) {
                if (r.success) {
                    $.gritter.add({
                        title: 'Thông báo',
                        text: r.messaging
                    });
                    self.loadtinhtra(self.TpSelectedValue());
                }
                else { alert(r.messaging) }
            }
        }).always(function () {
            CommonUtils.showWait(false)
        });
    }
    self.Start = function () {
        self.LoadArrayTp();
        ko.applyBindings(self);
    };
};