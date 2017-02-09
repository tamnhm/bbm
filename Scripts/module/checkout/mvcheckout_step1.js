var Checkout = Checkout || {};
Checkout.mvCheckout_step1 = function () {
    var self = this;
    self.mCheckout = ko.observable(new Checkout.mCheckout());
    /*************************** STEP 1 *****************************/
    self.Isloadercart = ko.observable(false);
    self.CartProduct = ko.observable();
    self.LoadCart = function () {
        self.Isloadercart(true);
        $.ajax({
            type: "POST",
            url: '/Checkout/LoadCart',
            cache: false,
            success: function (data) {
                ko.mapping.fromJS(data, {}, self.mCheckout);
            }
        }).always(function () {
            self.Isloadercart(false);
        });
    };
    self.Start = function () {
        ko.applyBindings(self);
        self.LoadCart();
    };
};