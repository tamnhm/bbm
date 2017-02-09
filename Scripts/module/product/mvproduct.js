var Products = Products || {};
Products.mvProduct = function () {
    var self = this;
    self.ShowType = ko.observable();

    var routes = $.sammy(function () {
        this.get("#/list", function () {
            if (!self.mvlist)
                self.mvlist = new Products.mvlist_product();
            self.ShowType("List");
            self.mvlist.LoadDropdown();
        });
        this.get("#/edit/:id", function () {
            var id = this.params['id'];
            self.mvedit = new Products.mvdetail();
            self.mvedit.IsaddNew(false);
            self.mvedit.LoadData(id);
            self.ShowType("Edit");
        });
        this.get("#/new", function () {
            self.mvedit = new Products.mvdetail();
            self.mvedit.IsaddNew(true);
            self.mvedit.LoadData();
            self.ShowType("Edit");
        });
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