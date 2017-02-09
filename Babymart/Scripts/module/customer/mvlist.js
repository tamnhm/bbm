var Customer = Customer || {};
Customer.mvlist = function () {
    var self = this;
    self.DataArray = ko.observableArray();
    self.mCustomer = ko.observable(new Customer.mCustomerAdmin());
    self.Validator = ko.computed(function () {
        var u = ko.validatedObservable(self.mCustomer());
        return (u)();
    });
    self.HasError = ko.observable(false);
    self.donhang = ko.observableArray();

    /********************************************/
    var filters = [
       {
           Type: "text",
           Name: "email",
           Value: ko.observable(""),
           RecordValue: function (record) { return record.tendn; }
       }];
    var sortOptions = [
        //{
        //    Name: "Mã hiệu",
        //    Value: "mahieu",
        //    Sort: function (left, right) { return left.mahieu > right.mahieu; }
        //},
        {
            Name: "Email",
            Value: "email",
            Sort: function (left, right) { return CommonUtils.CompareCaseInsensitive(left.tendn, right.tendn); }
        }
    ];
    self.filter = new CommonUtils.FilterModel(filters, self.DataArray);
    self.sorter = new CommonUtils.SorterModel(sortOptions, self.filter.filteredRecords);
    self.pager = new CommonUtils.PagerModel(self.sorter.orderedRecords);

    /********************************************/
    self.modaldh = function (data) {
        self.Ctdh([]);
        $('#modal-setting').modal('show');
        self.tendh(data.tendn());
        self.donhang(CommonUtils.MapArray(data.donhangs(), Orders.mOrder));
    };
    self.Ctdh = ko.observableArray();
    self.tendh = ko.observable();
    self.tenctdh = ko.observable();
    self.ctdhClick = function (data) {
        var tmp = [];
        self.tenctdh(data.donhang_ct()[0].SoDH());
        ko.utils.arrayForEach(data.donhang_ct(), function (o) {
            tmp.push(o.shop_bienthe);
        });
        self.Ctdh(CommonUtils.MapArray(tmp, Customer.mbienthe));
        $('#modal-setting-ctdh').modal('show');
    };
    self.getCustomer = function (kh) {
        if (kh != null) {
            self.mCustomer(kh);
            $('#modal-khachhang').modal('show');
        }
        else {
            self.mCustomer(new Customer.mCustomerAdmin());
            $('#modal-khachhang').modal('show');
        }
    };
    self.addCustomer = function (kh) {

        self.mCustomer(new Customer.mCustomerAdmin());
        $('#modal-khachhang').modal('show');
    };
    self.UpdateKh = function () {

        if (self.Validator().isValid()) {
            if (self.mCustomer().MaKH() != undefined) {
                var model = ko.toJSON({ kh: self.mCustomer() });
                bootbox.confirm("Bạn muốn cập nhật lại không?", function (result) {
                    if (result) {
                        CommonUtils.showWait(true);
                        $.ajax({
                            type: 'POST',
                            url: CommonUtils.url('/Admin/Customers/Update'),
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

                                    self.LoadData();
                                }
                                else { alert(r.messaging) }

                            }
                        }).always(function () {
                            CommonUtils.showWait(false);
                        });
                    }
                });

            }
            else {
                alert("Khách hàng không hợp lệ");
            }
        } else {
            self.HasError(true);
            self.Validator().errors.showAllMessages();
        }


    };
    self.ResetPass = function (obj) {
        bootbox.confirm("Bạn muốn gởi mail khởi tạo mật khẩu cho khách hàng nay ?", function (result) {
            if (result) {
                if (self.mCustomer().MaKH() != undefined) {
                    CommonUtils.showWait(true);
                    $.ajax({
                        type: 'POST',
                        url: CommonUtils.url('/Admin/Customers/ResetPassword'),
                        cache: false,
                        data: { Id: self.mCustomer().MaKH() },
                        success: function (r) {
                            if (r.success) {
                                $.gritter.add({
                                    title: 'Thông báo',
                                    text: r.messaging
                                });

                                self.LoadData();
                            }
                            else { alert(r.messaging) }

                        }
                    }).always(
                              function () {
                                  CommonUtils.showWait(false);
                              });
                }
                else {
                    alert("Khách hàng không hợp lệ");
                }
            }
        });


    };
    //------------Pagging-------------
    self.FreeSearch = ko.observable();

    self.indexPage = ko.observable(1);
    self.pageSize = ko.observable(10);
    self.pageSize.subscribe(function (obj) {
        self.indexPage(1)
        if (self.isnotload()) {
            self.LoadData();
        }
    });
    self.maxpage = ko.observable();
    self.countRecord = ko.observable();
    self.isnotload = ko.observable(false);
    self.LoadData = function () {
        var model = { page: self.indexPage(), pageSize: self.pageSize(), search: self.FreeSearch() };
        CommonUtils.showWait(true);
        $.ajax({
            type: "POST",
            url: '/Admin/Customers/GetList',
            dataType: 'json',
            cache: false,
            data: model,
            success: function (result) {
                self.isnotload(true);
                self.DataArray(CommonUtils.MapArray(result.data, Customer.mCustomerAdmin));
                self.maxpage(result.maxpage);
                self.countRecord(result.count);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        }).always(function () {
            CommonUtils.showWait(false);
        });
    };
    self.nextPage = function () {
        if (self.indexPage() < self.maxpage()) {
            self.indexPage(self.indexPage() + 1);
            self.LoadData();
        }
    };
    self.previousPage = function () {
        if (self.indexPage() > 1) {
            self.indexPage(self.indexPage() - 1);
            self.LoadData();
        }
    };
    self.Search = ko.computed(function () {
        debugger
        if (self.FreeSearch() && self.FreeSearch().length > 0) {
            debugger
            self.LoadData();
        }
        else {
            self.FreeSearch(null);
        }

    }).extend({ throttle: 500 });

}