var Orders = Orders || {};
Orders.mvlist = function () {
    var self = this;
    self.DataArray = ko.observableArray();
    self.sanpham = ko.observableArray();
    self.donhang = ko.observable(new Orders.mOrder());
    self.chitiet = function (data) {
        debugger
        $('#modal-setting').modal('show');
        self.donhang(data);
        var tmp = [];
        ko.utils.arrayForEach(data.donhang_ct(), function (o) {
            tmp.push(o.shop_bienthe);
        });
        self.sanpham(CommonUtils.MapArray(tmp, Products.mbienthe));
    };
    self.indonhang = function (data) {
        debugger
        $.ajax({
            type: 'GET',
            url: '/Admin/Order/Indonhang',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (r) {
            }
        });
    };
    self.giaohang = function () {
        CommonUtils.showWait(true);
        debugger
        var dh = self.donhang().id();
        $.ajax({
            type: 'POST',
            url: '/Admin/Order/Giaohang',
            cache: false,
            data: { id: dh },
            success: function (r) {
                if (r.success) {
                    $.gritter.add({
                        title: 'Thông báo',
                        text: r.messaging
                    });
                    self.LoadData();
                    self.donhang().dagiao(true);
                    self.donhang().ngaygiao(new Date());
                    self.donhang().dahuy(false);
                    self.donhang().ngayhuy(undefined);
                }
                else { alert(r.messaging) }
            }
        }).always(function () {
            CommonUtils.showWait(false)
        });
    };
    self.CancelOrder = function () {
        CommonUtils.showWait(true);
        var dh = self.donhang().id();
        $.ajax({
            type: 'POST',
            url: '/Admin/Order/CancelOrder',
            cache: false,
            data: { id: dh },
            success: function (r) {
                if (r.success) {
                    $.gritter.add({
                        title: 'Thông báo',
                        text: r.messaging
                    });
                    self.LoadData();
                    self.donhang().dagiao(false);
                    self.donhang().ngaygiao(undefined);
                    self.donhang().dahuy(true);
                    self.donhang().ngayhuy(new Date());
                }
                else { alert(r.messaging) }
            }
        }).always(function () {
            CommonUtils.showWait(false)
        });
    };
    self.ghichu = function () {
        CommonUtils.showWait(true);
        var dh = self.donhang().id();
        var note = self.donhang().ghichu();
        $.ajax({
            type: 'POST',
            url: '/Admin/Order/UpdateNote',
            cache: false,
            data: { id: dh, ghichu: note },
            success: function (r) {
                if (r.success) {
                    $.gritter.add({
                        title: 'Thông báo',
                        text: r.messaging
                    });
                    self.LoadData();
                    self.donhang().dagiao(true);
                    self.donhang().ngaygiao(new Date());
                }
                else { alert(r.messaging) }
            }
        }).always(function () {
            CommonUtils.showWait(false)
        });
    };
    //------------Pagging-------------
    self.FreeSearch = ko.observable(0);

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
            type: "GET",
            url: '/Admin/Order/GetlistOrder',
            cache: false,
            data: model,
            success: function (result) {
                self.isnotload(true);
                self.DataArray(CommonUtils.MapArray(result.data, Orders.mOrder));
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
        if (self.FreeSearch() && self.FreeSearch() > 0) {
            debugger
            self.LoadData();
        }
        else {
            self.FreeSearch(0);
        }

    }).extend({ throttle: 500 });
    self.FreeSearch.subscribe(function (obj) {
        debugger
        if (obj == 0 && self.isnotload()) {
            debugger
            self.isnotload(false);
            self.FreeSearch(0);
            self.isnotload(true);
            self.LoadData();
        }
    });
}