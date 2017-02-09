var Products = Products || {};
Products.mvlist_product = function () {
    var self = this;
    self.DataArray = ko.observableArray();
    self.mProdcut = ko.observable(new Products.mProduct());
    self.radius_saleoff = ko.observable();
    self.imgproduct = ko.observable();
    self.Ischecksale = ko.observable();
    self.gia = ko.observable();
    self.datatable = ko.observable(false);
    self.IsOneBienthe = ko.observable(false);
    self.eerro = function () {
        CommonUtils.ErrorImage(this)
    };
    self.HasError = ko.observable(false);
    self.Validator = ko.computed(function () {
        return (ko.validatedObservable(self.mProdcut()))();
    });
    self.Bienthe = ko.observableArray();
    self.txt_search = ko.observable();
    /*************************Load*******************************/
    self.LoadData = function (a, b, c, d) { 
        if (a != 0) {
            CommonUtils.showWait(true);
            $.ajax({
                type: "GET",
                url: '/Admin/Product/List',
                dataType: "json",
                cache: false,
                data: { madanhmuc: a, madanhmuccon: b, thuonghieu: c, site: d },
                success: function (data) {
                    var arry = [];
                    ko.utils.arrayForEach(data, function (obj) {
                        arry.push(obj.shop_sanpham);
                    });

                    self.DataArray(CommonUtils.MapArray(arry, Products.mProduct));
                    ko.utils.arrayForEach(self.DataArray(), function (obj) {
                        if (obj.timeend()) {
                            var timeend = obj.timeend();
                            if (timeend.constructor != Date)
                                obj.timeend(new Date(parseInt(timeend.substr(6))));
                        }
                        obj.shop_bienthe(CommonUtils.MapArray(obj.shop_bienthe(), Products.mbienthe));
                    });

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    if (jqXHR.status == 500) {
                        alert('Internal error: ' + jqXHR.responseText);
                    } else {
                        alert('Unexpected error. ' + jqXHR.responseText);
                    }
                }
            }).always(function () {
                CommonUtils.showWait(false);
            });
        }
        else
            alert("Vui lòng chọn danh mục cần tìm!");
    };
    self.LoadImg = function (id, successs) {
        $.ajax({
            type: "POST",
            url: '/Admin/Product/GetImg',
            cache: false,
            data: { id: id },
            success: successs
        });
    };
    self.LoadSite = function () {
        CommonUtils.showWait(true);
        $.ajax({
            type: "GET",
            url: '/Admin/Landingpage/Get',
            cache: false,
            success: function (data) {
                self.ArraySite(data);
            }
        }).always(function () { CommonUtils.showWait(false) });
    };
    self.LoadBrand = function () {
        CommonUtils.showWait(true);
        $.ajax({
            type: "GET",
            url: '/Admin/Brand/List',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                self.ArrayBrand(CommonUtils.MapArray(data, Products.mBrand));
            }
        }).always(function () {
            CommonUtils.showWait(false);
        });
    };
    /********************dANH MUC*********************************/
    self.ArraySite = ko.observableArray();
    self.selectedSite = ko.observable();
    self.ArrayBrand = ko.observableArray();
    self.ArrayDanhmuc = ko.observableArray();
    self.ArrayDanhmuccon = ko.observableArray();
    self.selectedBrand = ko.observable();
    self.selecteddanhmuc = ko.observable()
    self.selecteddanhmuccon = ko.observable();
    self.LoadDanhmuc = function () {
        CommonUtils.showWait(true);
        $.ajax({
            type: "GET",
            url: '/Admin/Link/Getdanhmuc',
            success: function (data) {
                self.ArrayDanhmuc(CommonUtils.MapArray(data.Data, Products.mdanhmuc));
            },
            error: function () {
                alert("Load danh muc that bai");
            }
        }).always(function () { CommonUtils.showWait(false) });;
    };
    self.LoadDanhmuccon = function (id) {
        CommonUtils.showWait(true);
        $.ajax({
            type: "POST",
            url: '/Admin/Link/Getdanhmuccon',
            cache: false,
            data: { id: id },
            success: function (data) {

                self.ArrayDanhmuccon(CommonUtils.MapArray(data, Products.mdanhmuccon));
            }
        }).always(function () { CommonUtils.showWait(false) });;
    };
    self.selecteddanhmuc.subscribe(function (data) {
        if (data) {
            self.hidedanhmuccon(true);
            self.LoadDanhmuccon(data);
        }
        else { self.hidedanhmuccon(false); }
    });
    //self.selectedSite.subscribe(function (data) {
    //    if (data != undefined)
    //        self.LoadDanhmuc(data.maloai);
    //});
    self.hidedanhmuccon = ko.observable(false);
    self.search = function () {
        self.datatable(true);
        var site = self.selectedSite() == undefined ? -1 : self.selectedSite().maloai;
        var danhmuc = self.selecteddanhmuc() == undefined ? 0 : self.selecteddanhmuc();
        var danhmuccon = self.selecteddanhmuc() == undefined || self.selecteddanhmuccon() == undefined ? 0 : self.selecteddanhmuccon();
        var thuonghieu = self.selectedBrand() == undefined ? 0 : self.selectedBrand();
        self.LoadData(danhmuc, danhmuccon, thuonghieu, site);

    };
    /*************************Action*******************************************************************/
    self.setshow = function (data) {
        var string = data.hide() ? "hiện" : "ẩn";
        bootbox.confirm("Bạn muốn <strong>" + string + "</strong> sản phẩm <strong>" + data.tensp() + "</strong> ?", function (result) {
            if (result) {
                CommonUtils.showWait(true);
                $.ajax({
                    type: 'POST',
                    url: CommonUtils.url('/Admin/Product/SetVisible'),
                    cache: false,
                    data: { id: data.id(), visible: data.hide() ? false : true },
                    success: function (r) {
                        $.gritter.add({
                            title: 'Thông báo',
                            text: r.messaging
                        });
                        self.search();
                    }
                }).always(function () {
                    CommonUtils.showWait(false);
                });

            }
        });


    };
    self.noibat = function (data) {
        var string = data.hide() ? "đặt" : "bỏ";
        bootbox.confirm("Bạn muốn " + string + " <strong>" + data.tensp() + "</strong> nổi bật  ?", function (result) {
            if (result) {
                CommonUtils.showWait(true);
                $.ajax({
                    type: 'POST',
                    url: CommonUtils.url('/Admin/Product/SetShowHome'),
                    cache: false,
                    data: { id: data.id() },
                    success: function (r) {
                        $.gritter.add({
                            title: 'Thông báo',
                            text: r.messaging
                        });
                        self.search();
                    }
                }).always(function () {
                    CommonUtils.showWait(false);
                });

            }
        });

    }
    self.setout = function (data) {
        bootbox.confirm("Sản phẩm <strong>" + data.tensp() + "</strong> này hết hàng ?", function (result) {
            if (result) {
                CommonUtils.showWait(true);
                $.ajax({
                    type: 'POST',
                    url: CommonUtils.url('/Admin/Product/SetOut'),
                    cache: false,
                    data: { id: data.id(), setout: data.ischeckout() ? false : true },
                    success: function (r) {
                        $.gritter.add({
                            title: 'Thông báo',
                            text: r.messaging
                        });
                        self.search();
                    }
                }).always(function () {
                    CommonUtils.showWait(false);
                });

            }
        });
    };
    self.remove = function (data) {
        bootbox.confirm("Bạn muốn xóa sản phẩm <strong>" + data.tensp() + "</strong> ?", function (result) {
            if (result) {
                CommonUtils.showWait(true);
                $.ajax({
                    type: 'POST',
                    url: CommonUtils.url('/Admin/Product/Remove'),
                    cache: false,
                    data: { id: data.id() },
                    success: function (r) {
                        $.gritter.add({
                            title: 'Thông báo',
                            text: r.messaging
                        });
                        self.search();
                    }
                }).always(function () {
                    CommonUtils.showWait(false);
                });

            }
        });
    };
    self.Addbienthe = function (obj) {
        var bienthe = new Products.mbienthe();
        bienthe.idsp = obj.id;
        self.Bienthe.push(bienthe);
        self.IsOneBienthe(false);
    };
    self.removeBienthe = function (data) {
        var title = data.title() != undefined ? data.title() : "";
        if (data.id() != undefined) {
            bootbox.confirm("Bạn muốn xóa biến thể <strong>" + title + "</strong> ?", function (result) {
                if (result) {
                    CommonUtils.showWait(true);
                    $.ajax({
                        type: 'POST',
                        url: CommonUtils.url('/Admin/Product/RemoveBienthe'),
                        cache: false,
                        data: { id: data.id() },
                        success: function (r) {
                            if (r.success) {

                                $.gritter.add({
                                    title: 'Thông báo',
                                    text: r.messaging
                                });
                                self.search();
                                $('#modal-saleoff').modal('hide');
                            } else { alert(r.messaging) }
                        }
                    }).always(function () {
                        CommonUtils.showWait(false);
                    });
                }
            })
        } else {
            self.Bienthe.remove(data);
        }
    }

    self.saleoff = function (data) {
        if (data.shop_bienthe().length == 1)
            self.IsOneBienthe(true);
        else
            self.IsOneBienthe(false);
        self.mProdcut(data);
        self.Bienthe([]);
        self.Validator = ko.validatedObservable(data);
        self.Validator().errors.showAllMessages(false);
        self.HasError(false);
        $('#modal-saleoff').modal('show');
    };
    self.setsaleoff = function (data) {
        if (self.Validator().isValid()) {
            CommonUtils.showWait(true);
            if (self.Bienthe().length > 0)
                ko.utils.arrayForEach(self.Bienthe(), function (obj) {
                    data.shop_bienthe().push(obj)
                });
            $.ajax({
                type: 'POST',
                url: CommonUtils.url('/Admin/Product/Saleoff'),
                cache: false,
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                data: ko.toJSON(data),
                success: function (r) {
                    if (r.success) {
                        $.gritter.add({
                            title: 'Thông báo',
                            text: r.messaging
                        });
                        self.search();
                        $('#modal-saleoff').modal('hide');
                    }
                    else { alert(r.messaging) }
                }
            }).always(function () {
                CommonUtils.showWait(false);
            });


        } else {
            self.HasError(true);
            self.Validator().errors.showAllMessages();
        }
    };
    /**********************************************************************************************************/
    self.LoadDropdown = function () {
        self.LoadDanhmuc();
        //self.LoadSite();
        self.LoadBrand();

    };
    /*******************************************************************************************************/
    var filters = [
       {
           Type: "text",
           Name: "masp",
           Value: ko.observable(""),
           RecordValue: function (record) { return record.masp; }
       }
       //,
       //{
       //    Type: "select",
       //    Name: "Status",
       //    Options: [
       //        GetOption("All", "All", null),
       //        GetOption("None", "None", "None"),
       //        GetOption("New", "New", "New"),
       //        GetOption("Recently Modified", "Recently Modified", "Recently Modified")
       //    ],
       //    CurrentOption: ko.observable(),
       //    RecordValue: function (record) { return record.status; }
       //}
    ];
    var sortOptions = [
        {
            Name: "ID",
            Value: "ID",
            Sort: function (left, right) { return left.id < right.id; }
        },
        {
            Name: "masp",
            Value: "masp",
            Sort: function (left, right) { return CommonUtils.CompareCaseInsensitive(left.masp, right.masp); }
        }
    ];
    self.filter = new CommonUtils.FilterModel(filters, self.DataArray);
    self.sorter = new CommonUtils.SorterModel(sortOptions, self.filter.filteredRecords);
    self.pager = new CommonUtils.PagerModel(self.sorter.orderedRecords);
    /*******************************************************************************************************/
    self.linkadd = ko.computed(function () {
        return CommonUtils.url('product#/new');
    });
    self.GetLinkPageDetail = function (data) {
        location.hash = "/Product/Edit/" + data.id();
    };
    self.UrlNew = ko.computed(function () {
        return CommonUtils.url('customer#/new');
    });
    /****************************************************************/
    self.ResetSaleoff = function () {
        bootbox.confirm("Bạn muốn xóa khuyễn mãi tất cả sản phẩm?", function (result) {
            if (result) {
                CommonUtils.showWait(true);
                $.ajax({
                    type: 'POST',
                    url: CommonUtils.url('/Admin/Product/ResetSalloff'),
                    cache: false,
                    dataType: 'json',
                    success: function (r) {
                        if (r.success) {
                            $.gritter.add({
                                title: 'Thông báo',
                                text: r.messaging
                            });
                            window.location.reload()
                        } else { alert(r.messaging) }
                    }
                }).always(function () {
                    CommonUtils.showWait(false);
                });
            }
        })
    };

    self.ResetProduct_Banner = function () {
        bootbox.confirm("Bạn muốn đưa tất cả sản phẩm ra khỏi banner?", function (result) {
            if (result) {
                CommonUtils.showWait(true);
                $.ajax({
                    type: 'POST',
                    url: CommonUtils.url('/Admin/Product/ResetProduct_Banner'),
                    cache: false,
                    success: function (r) {
                        if (r.success) {
                            $.gritter.add({
                                title: 'Thông báo',
                                text: r.messaging
                            });
                            window.location.reload()
                        } else { alert(r.messaging) }
                    }
                }).always(function () {
                    CommonUtils.showWait(false);
                });
            }
        })
    };
    self.search_query = function () {
        if (self.txt_search()) {
            CommonUtils.showWait(true);
            $.ajax({
                type: 'POST',
                url: CommonUtils.url('/Admin/Product/Search_query'),
                cache: false,
                dataType: 'json',
                data: { query: self.txt_search() },
                success: function (data) {
                    var arry = [];
                    ko.utils.arrayForEach(data, function (obj) {
                        arry.push(obj.shop_sanpham);
                    });

                    self.DataArray(CommonUtils.MapArray(arry, Products.mProduct));
                    ko.utils.arrayForEach(self.DataArray(), function (obj) {
                        if (obj.timeend()) {
                            var timeend = obj.timeend();
                            if (timeend.constructor != Date)
                                obj.timeend(new Date(parseInt(timeend.substr(6))));
                        }
                        obj.shop_bienthe(CommonUtils.MapArray(obj.shop_bienthe(), Products.mbienthe));
                    });
                    self.datatable(true);
                }
            }).always(function () {
                CommonUtils.showWait(false);
            });
        }
    };
    self.UpdateAllSaleOffForMember = function () {
        bootbox.confirm("Bạn muốn tất cả sản phẩm khuyến mãi chỉ giảm giá cho thành viên?", function (result) {
            if (result) {
                CommonUtils.showWait(true);
                $.ajax({
                    type: 'POST',
                    url: CommonUtils.url('/Admin/Product/UpdateAllSaleOffForMember'),
                    cache: false,
                    data: { ischecksaleoff: true },
                    success: function (r) {
                        if (r) {
                            $.gritter.add({
                                title: 'Thông báo',
                                text: 'Cập nhật thành công'
                            });
                        } else { alert('Có lỗi xảy ra.') }
                    }
                }).always(function () {
                    CommonUtils.showWait(false);
                });
            }
        });
    }
    self.UpdateAllSaleOffNotForMember = function () {
        bootbox.confirm("Bạn muốn tất cả sản phẩm khuyến mãi chỉ giảm giá cho thành viên?", function (result) {
            if (result) {
                CommonUtils.showWait(true);
                $.ajax({
                    type: 'POST',
                    url: CommonUtils.url('/Admin/Product/UpdateAllSaleOffForMember'),
                    cache: false,
                    data: { ischecksaleoff: false },
                    success: function (r) {
                        if (r) {
                            $.gritter.add({
                                title: 'Thông báo',
                                text: 'Cập nhật thành công'
                            });
                        } else { alert('Có lỗi xảy ra.') }
                    }
                }).always(function () {
                    CommonUtils.showWait(false);
                });
            }
        });
    }

}