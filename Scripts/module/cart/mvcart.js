var Cart = Cart || {};
Cart.mvCart = function () {
    var self = this;
    self.mCart = ko.observable(new Cart.mCart());
    self.LoadData = function () {
        CommonUtils.showWait(true);
        $.ajax({
            type: "GET",
            url: 'Cart/ListCart',
            cache: false,
            success: function (data) {
                debugger
            }
        }).always(CommonUtils.showWait(false));
    };
    self.Start = function () {
        ko.applyBindings(self);
        self.LoadData();
    };
};