var Plan = Plan || {};
Plan.mPlan = function () {
    var self = this;
    self.Id = ko.observable();
    self.IdProdut = ko.observable();
    self.Count = ko.observable(1);
    self.typePlan = ko.observable();
    self.spurl = ko.observable();
    self.spurl_us = ko.observable();
    self.tensp = ko.observable();
    self.tensp_us = ko.observable();
    self.masp = ko.observable();
    self.hinhanh = ko.observable();
    self.gia = ko.observable();
    self.sum = ko.observable();
    self.gia.subscribe(function (obj) {
        self.sum(self.gia());
    });
    self.Count.subscribe(function (obj) {
        self.sum(obj * self.gia());
    });
    self.namePlan = ko.observable();

    self.namePlan_us = ko.observable();
    self.kg = ko.observable();
    self.IsAddNew = ko.observable(false);
    self.ischeckout = ko.observable(false);
};
Plan.mProduct = function () {
    var self = this;
    self.id = ko.observable();
    self.idsp = ko.observable();
    self.fullname = ko.observable();
    self.masp = ko.observable();
    self.spurl = ko.observable();
    self.gia = ko.observable();
    self.giasosanh = ko.observable();
    self.imgsp = ko.observable();
    self.idPlan = ko.observable();
    self.kg = ko.observable();
    self.ischeckout = ko.observable(false);
};
Plan.mplan_saleoff = function () {
    var self = this;
    self.Id = ko.observable();
    self.Name = ko.observable();
    self.Value = ko.observable();
    self.OperatorOption = ko.observable();
    self.OperatorValue = ko.observable();
    self.Visible = ko.observable();
    self.More = ko.observable();
};
Plan.mPlanCart = function () {
    var self = this;
    self.ToTal = ko.observable();
    self.TotalSum = ko.observable();
    self.Percent = ko.observable();
    self.PlanModel = ko.observableArray();
    self.Gift = ko.observableArray();
};
Plan.mGift = function () {
    var self = this;
    self.tensp = ko.observable();
    self.masp = ko.observable();
    self.img = ko.observable();
    self.url = ko.observable();
    self.url_us = ko.observable();
    self.note = ko.observable();
};
Plan.mPlan_type = function () {
    var self = this;
    self.Id = ko.observable();
    self.Type = ko.observable();
    self.Type_us = ko.observable();
};