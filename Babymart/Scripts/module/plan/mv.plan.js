var Plan = Plan || {};
Plan.mvPlan = function () {
    var self = this;
    self.ListplanType = ko.observableArray();
    self.ListProduct = ko.observableArray();
    self.loading = ko.observable(false);
    self.namePlan = ko.observable();
    self.loadingProduct = function (obj) {
        if (obj.typePlan()) {
            self.loading(true);
            self.ListProduct.removeAll();
            var plan = ko.utils.arrayFirst(self.ListplanType(), function (item) {
                return item.Id() == obj.typePlan();
            });
            self.namePlan(plan.Type());
            $('#modal-product').modal('show');
            var idPlan = obj.Id();
            $.ajax({
                type: "POST",
                url: '/Plan/GetListSpForPlan',
                cache: false,
                data: { type: obj.typePlan() },
                success: function (data) {
                    self.ListProduct(CommonUtils.MapArray(data, Plan.mProduct));
                    ko.utils.arrayForEach(self.ListProduct(), function (obj) {
                        obj.idPlan(idPlan);
                    });
                }
            }).always(function () {
                self.loading(false);
            });
        }
    };
    self.ListCart = ko.observableArray();
    self.addCart = function (obj) {
        $('#modal-product').modal('hide');
        ko.utils.arrayForEach(self.ListCart(), function (item) {
            if (item.Id() == obj.idPlan()) {
                item.namePlan(self.namePlan());
                item.kg(obj.kg());
                item.IdProdut(obj.id());
                item.tensp(obj.fullname());
                item.hinhanh(obj.imgsp());
                item.masp(obj.masp());
                item.spurl(obj.spurl());
                item.spurl_us(obj.spurl_us());
                item.gia(obj.giasosanh() > 0 ? obj.giasosanh() : obj.gia());
            }
        });
    };
    self.ListSaleOff = ko.observableArray();
    self.LoadData = function () {
        self.waitloader(true);
        $.ajax({
            type: "GET",
            url: '/Plan/GetlistShop_plan_type',
            cache: false,
            success: function (data) {
                self.waitloader(true);
                self.ListplanType(CommonUtils.MapArray(data.plantype, Plan.mPlan_type));
                self.ListSaleOff(CommonUtils.MapArray(data.saleoff, Plan.mplan_saleoff));
                if (goidoSS) {
                    self.ListCart(CommonUtils.MapArray(goidoSS.PlanModel, Plan.mPlan));
                    self.pushTypePlan();
                    self.IsSessionGoidoSS(true);
                } else {
                    var id = 1;
                    ko.utils.arrayForEach(self.ListplanType(), function (obj) {
                        var model = { typePlan: obj.Id(), Id: id };
                        id++;
                        self.ListCart.push(ko.mapping.fromJS(model, {}, new Plan.mPlan));
                    });
                }
                self.loadGift();
                self.waitloader(false);
            }
        }).always(function () {
            self.waitloader(false);

        });
    };
    self.LstGift = ko.observableArray();
    self.pushTypePlan = function () {
        var model = { Id: self.ListCart().length + 1 };
        self.ListCart.push(ko.mapping.fromJS(model, {}, new Plan.mPlan));
    };
    self.removeCart = function (obj) {
        self.ListCart.remove(obj);
    };
    self.IsSessionGoidoSS = ko.observable(false);
    self.waitloader = ko.observable(false);
    self.Total = ko.computed(function () {
        var sum = 0;
        ko.utils.arrayForEach(self.ListCart(), function (obj) {
            if (obj.Count() && obj.gia()) {
                sum += obj.Count() * obj.gia();
            }
        });
        return sum;
    });
    self.khuyenmai = ko.computed(function () {
        if (self.ListSaleOff() && self.ListSaleOff().length > 0 && self.Total()) {
            var percent = 0;
            var min = ko.utils.arrayFirst(self.ListSaleOff(), function (obj) {
                if (obj.Name() == 'Percent' && obj.OperatorOption() == 'min')
                    return obj;
            });
            var less = ko.utils.arrayFirst(self.ListSaleOff(), function (obj) {
                if (obj.Name() == 'Percent' && obj.OperatorOption() == '<')
                    return obj;
            });
            var more = ko.utils.arrayFirst(self.ListSaleOff(), function (obj) {
                if (obj.Name() == 'Percent' && obj.OperatorOption() == '>=')
                    return obj;
            });
            //if (self.Total() >= min.OperatorValue()) {
            //    if (self.Total() < less.OperatorValue()) {
            //        percent = less.Value();
            //    } else {
            //        percent = more.Value();
            //    }
            //}

            if (self.Total() >= 1000000) {
                percent = 5;
            }
            return percent;
        }

        //if (self.Total() >= 1000000) {
        //    if (self.Total() < 3000000) {
        //        percent = 3;
        //    } else {
        //        percent = 5;
        //    }
        //}

    });
    self.gift = ko.computed(function () {
        if (self.ListSaleOff() && self.ListSaleOff().length > 0 && self.Total()) {
            var gift;
            var more = ko.utils.arrayFirst(self.ListSaleOff(), function (obj) {
                if (obj.Name() == 'Gift' && obj.OperatorOption() == '>')
                    return obj;
            });
            //if (self.Total() >= 5000000) {
            //    return self.LstGift();
            //}
            if (self.Total() >= more.OperatorValue()) {
                return self.LstGift();
            }
            return [];
        }
        return [];
    });

    self.loadGift = function () {
        self.waitloader(true);
        var gift = ko.utils.arrayFirst(self.ListSaleOff(), function (obj) {
            if (obj.Name() == 'Gift')
                return obj;
        });
        var model = { 'ids': gift.Value() };
        $.ajax({
            type: "GET",
            url: '/Plan/GetlistProduct',
            cache: false,
            data: model,
            success: function (data) {
                self.waitloader(true);
                self.LstGift(CommonUtils.MapArray(data, Plan.mGift));
                self.waitloader(false);
            }
        }).always(function () {
            self.waitloader(false);

        });
    };
    self.TotalSum = ko.computed(function () {
        if (self.Total())
            return self.Total() - ((self.Total() * self.khuyenmai()) / 100);
        return 0;
    });
    self.addCartSubmit = function (data) {
        debugger
        if (self.Total() > 0) {
            self.waitloader(true);
            var cart = ko.observable(new Plan.mPlanCart());
            cart().ToTal(self.Total());
            cart().TotalSum(self.TotalSum());
            cart().PlanModel(self.ListCart());
            cart().Percent(self.khuyenmai());
            cart().Gift(self.gift());
            var input = ko.toJSON({ model: cart });
            $.ajax({
                type: 'POST',
                url: CommonUtils.url('/Plan/AddCart'),
                cache: false,
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                data: input,
                success: function (data) {
                    window.location = data;
                }
            }).always(function () {
                self.waitloader(false);
            });
        }
        else {
            alert("Vui lòng chọn sản phẩm! \n Please choose items!");
        }
    };
    self.AddTypePlan = function (obj, event) {
        var context = ko.contextFor(event.target);
        var index = context.$index() + 1;
        var maxidObj = ko.utils.arrayFirst(self.ListCart(), function (obj) {
            return obj.Id() === Math.max.apply(null, ko.utils.arrayMap(self.ListCart(), function (e) {
                return e.Id();
            }));
        });
        var IdMaxs = maxidObj.Id() + 1;
        var model = { typePlan: obj.typePlan(), Id: IdMaxs, IsAddNew: true };
        self.ListCart.splice(index, 0, ko.mapping.fromJS(model, {}, new Plan.mPlan));
    };
    self.CancelPlan = function () {
        bootbox.confirm(IsLangVN ? "Bạn muốn hủy bỏ gói đồ sơ sinh." : "Cancel list order.", function (result) {
            if (result) {

                $.ajax({
                    type: 'POST',
                    url: CommonUtils.url('/Plan/CancelPlan'),
                    cache: false,
                    success: function () {
                        window.location.reload(true);
                    }
                }).always(function () {
                    self.waitloader(false);
                });
            }
        });
    };
    self.Start = function () {
        ko.applyBindings(self);
        self.LoadData();
    };
};