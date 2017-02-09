var Cart = Cart || {};
Cart.mCart = function () {
    var self = this;
    self.RecordId = ko.observable();
    self.CartId = ko.observable();
    self.ProductId = ko.observable();
    self.Count = ko.observable();
    self.shop_bienthe = ko.observableArray();
};