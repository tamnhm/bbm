var Orders = Orders || {};
Orders.mvOrders = function () {
    var self = this;
    self.mvlist = ko.observable();
    self.mvedit = ko.observable();
    self.ShowType = ko.observable();

    var routes = $.sammy(function () {
        this.get("#/list", function () {
            if (!self.mvlist())
                self.mvlist(new Orders.mvlist());
            self.mvlist().LoadData();
            self.ShowType("List");
        });
        //this.get("#/edit/:id", function () {
        //    if (!self.mvedit())
        //        self.mvedit = new Orders.mvedit();
        //    var id = this.params['id'];
        //    self.mvedit.LoadData(id);
        //    self.ShowType("Edit");
        //});
    });

    self.Start = function () {
        ko.applyBindings(self);
        if (window.location.hash == "")
            routes.run("#/list");
        else {
            routes.run(window.location.hash);
        }
    };
};