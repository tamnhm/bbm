var Brand = Brand || {};
Brand.mvBrand = function () {
    var self = this;
    self.mvlist = new Brand.mvlist();
    self.ShowType = ko.observable();

    var routes = $.sammy(function () {
        this.get("#/list", function () {
            self.ShowType("List");
            self.mvlist.LoadData();
        });
        this.get("#/edit/:id", function () {
            self.mvedit = new Brand.mvedit();
            var id = this.params['id'];
            self.mvedit.LoadData(id);
            self.mvedit.checkautolink(false);
            self.ShowType("Edit");
        });
        this.get('#/new', function () {
            self.mvedit = new Brand.mvedit();
            self.mvedit.checkautolink(true);
            self.mvedit.InitFinish(true);
            self.ShowType('New');
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