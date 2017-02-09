var Customer = Customer || {};
Customer.mvCustomer = function () {
    var self = this;
   
    self.ShowType = ko.observable();
    var routes = $.sammy(function () {
        this.get("#/list", function () {
            self.mvlist = new Customer.mvlist();
            self.mvlist.LoadData();
            self.ShowType("List");          
        });
        this.get("#/detail/:id", function () {
            self.mvedit = new Customer.mvedit();
            // self.mvedit = new Brand.mvedit();
            //var id = this.params['id'];
            //self.ShowType("Detail");
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