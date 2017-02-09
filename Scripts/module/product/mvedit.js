var Products = Products || {};
Products.mvdetail = function () {
    var self = this;
    self.autourl = ko.observable(true);
    self.mProduct = ko.observable(new Products.mProduct());
    self.Imgobject = ko.observable(new Common.mImage());
    self.Collection = ko.observable(new Products.mcollection());
    self.ArrayCollection = ko.observableArray();
    self.ImgArray = ko.observableArray();
    self.ArrayBrand = ko.observableArray();
    self.ArryDanhmuc = ko.observableArray();
    self.ArraySite = ko.observableArray();
    self.selecteddanhmuc = ko.observable()
    self.selecteddanhmuccon = ko.observable();
    self.selectedBrand = ko.observable();
    self.selectedSite = ko.observable();
    self.ischeckshow = ko.observable();
    self.ArryDanhmuccon = ko.observableArray();
    self.HasError = ko.observable(false);
    self.ischeckgift = ko.observable();
    self.timeend = ko.observable();
    self.Validator = ko.computed(function () {
        return (ko.validatedObservable(self.mProduct()))();
    });
    self.IsMoreTag = ko.observable(false);
    self.IsMoreTag1 = ko.observable(false);
    self.InitFinish = ko.observable(false);
    self.InitFinishGift = ko.observable(false);
    self.ischecksaleoff = ko.observable();
    self.ErorAray = ko.observableArray();
    self.Bienthe = ko.observableArray();

    self.ListplanType = ko.observableArray();
    self.tmpplantype = ko.observable();
    self.LoadTypePlan = function () {
        CommonUtils.showWait(true);
        $.ajax({
            type: "GET",
            url: '/Plan/GetlistShop_plan_type',
            cache: false,
            success: function (data) {
                self.ListplanType(CommonUtils.MapArray(data.plantype, Products.mPlan_type));
                self.mProduct().plantype(self.tmpplantype());
            }
        }).always(function () {
            CommonUtils.showWait(false);
        });
    };
    self.ProductId = ko.observable();
    self.TagArticle = ko.observableArray();
    //--------Load-----------------------------
    self.LoadData = function (id) {
        CommonUtils.showWait(true);
        self.ArryDanhmuc([]);
        if (!self.IsaddNew()) {
            self.LoadSite();
            self.LoadBrand();
            self.LoadTypePlan();
            self.ProductId(id);
            if (id != undefined) {
                $.ajax({
                    type: "GET",
                    url: '/Admin/Product/Detail',
                    cache: false,
                    data: { id: id },
                    success: function (data) {
                        var timeend = data.Data.timeend;
                        ko.mapping.fromJS(data.Data, {}, self.mProduct);
                        if (timeend && timeend.constructor != Date)
                            timeend = new Date(parseInt(timeend.substr(6)));
                        self.mProduct().timeend(timeend);
                        self.selectedBrand(data.Data.mahieu);
                        self.tmpplantype(data.Data.plantype);
                        self.mProduct().shop_bienthe(CommonUtils.MapArray(data.Data.shop_bienthe, Products.mbienthe));
                        ko.utils.arrayForEach(self.mProduct().shop_bienthe(), function (item) {
                            item.tmpgia(item.gia())
                        });
                        self.ImgArray(CommonUtils.MapArray(data.Data.shop_image, Common.mImage));
                        //self.ArrayCollection(CommonUtils.MapArray(data.Data.shop_collection, Products.mcollection));
                        // self.selectedBrand(self.mProduct().mahieu());
                        if (self.mProduct().id())
                            self.LoadCollection(self.mProduct().id());
                        self.mProduct().Tags([]);
                        self.ListTags(CommonUtils.MapArray(data.ListTag, Products.msys_tags_Summary));
                        self.mProduct().ListTags(CommonUtils.MapArray(data.Tag, Products.msys_tags_Ref));
                        self.TagArticle(CommonUtils.MapArray(data.TagArticle, Products.msys_tags_Summary));

                    },
                    error: function () {
                        //alert("Du lieu khong hop le");
                        //  window.location = "/Admin/product/";
                        window.location.href = '#/list';

                    }
                }).always(
                function () {
                    self.InitFinish(true);
                    self.Bienthe(self.mProduct().shop_bienthe());
                    CommonUtils.showWait(false);
                });
            }
            else {
                self.selectedSite(0);
            }
        } else {
            var bienthe = new Products.mbienthe();
            bienthe.title('default');
            self.Bienthe.push(bienthe);
            self.LoadSite();
            self.LoadBrand();
            self.mProduct(new Products.mProduct());
            self.InitFinish(true);
            return
        }

    };
    self.LoadSite = function () {
        CommonUtils.showWait(true);
        $.ajax({
            type: "GET",
            url: '/Admin/Landingpage/Get',
            cache: false,
            success: function (data) {
                self.ArraySite(data);
                self.ArraySite.sort(function (a, b) {
                    return b.tenloai < a.tenloai

                });
            }
        }).always(CommonUtils.showWait(false));
    };
    self.LoadDanhmuc = function (id) {
        CommonUtils.showWait(true)
        $.ajax({
            type: "GET",
            url: '/Admin/Link/Getdanhmuc',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: { loai: id },
            success: function (data) {
                self.ArryDanhmuc(CommonUtils.MapArray(data.Data, Products.mdanhmuc));
            },
            error: function () {
                alert("Load danh muc that bai");
            }
        }).always(function () { CommonUtils.showWait(false) });
    };
    self.LoadDanhmuccon = function (id) {
        CommonUtils.showWait(true)
        $.ajax({
            type: "POST",
            url: '/Admin/Link/Getdanhmuccon',
            cache: false,
            data: { id: id },
            success: function (data) {
                self.ArryDanhmuccon(CommonUtils.MapArray(data, Products.mdanhmuccon));
            }
        }).always(function () { CommonUtils.showWait(false) });
    };
    self.LoadBrand = function () {
        CommonUtils.showWait(true)
        $.ajax({
            type: "GET",
            url: '/Admin/Brand/List',
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.ArrayBrand(CommonUtils.MapArray(data, Products.mBrand));
                self.mProduct().mahieu(self.selectedBrand());

            }
        }).always(function () { CommonUtils.showWait(false) });
    };
    self.LoadCollection = function (id) {
        if (id) {
            CommonUtils.showWait(true)
            $.ajax({
                type: "GET",
                url: '/Admin/Collection/List',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: { id: id },
                success: function (data) {
                    self.ArrayCollection(CommonUtils.MapArray(data, Products.mcollection));
                },
                error: function () {
                    alert("Load collection that bai");
                }
            }).always(CommonUtils.showWait(false));
        }
    };
    //self.Tensp.subscribe(function (value) {
    //    self.mProduct().tensp(self.Tensp());
    //    //if (self.autourl()) {
    //    //    self.mProduct().spurl(CommonUtils.NonUnicode(value));
    //    //}
    //});
    self.selecteddanhmuc.subscribe(function (data) {

        if (data != undefined) {
            self.mProduct().madanhmuc(data.madanhmuc());
            self.LoadDanhmuccon(data.madanhmuc());
        }

    });
    self.selectedSite.subscribe(function (data) {

        if (data != undefined) {
            self.mProduct().maloai(data.maloai);
            self.LoadDanhmuc(data.maloai);//self.IsaddNew() ? data : data.maloai);
        }
        else
            self.ArryDanhmuc([]);

    });
    self.selecteddanhmuccon.subscribe(function (data) {
        if (data != undefined)
            self.mProduct().madanhmuccon(data);
    });

    self.autourl.subscribe(function (value) {
        self.autourl(value);
    });

    self.ischeckshow.subscribe(function (val) {
        if (val) {
            self.mProduct().ischeckshow(false);
        }
        else {
            self.mProduct().ischeckshow(true);
        }
    });
    //----------Submit---------------------------------------
    self.submit = function (data) {
        if (self.Validator().isValid()) {
            CommonUtils.showWait(true);
            self.mProduct().shop_bienthe(self.Bienthe())
            if (self.mProduct().id() == undefined) {
                var input = ko.toJSON({ sp: data, l: self.SetdataCollection() });
                $.ajax({
                    type: 'POST',
                    url: CommonUtils.url('/Admin/Product/AddNew'),
                    cache: false,
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8',
                    data: input,
                    success: function (r) {
                        if (r.success) {
                            window.location.href = '#/edit/' + r.id;
                            window.location.reload(true);
                            $.gritter.add({
                                title: 'Thông báo',
                                text: r.messaging
                            });

                            //  self.LoadData(r.id);
                        }
                        else { alert(r.messaging) }
                        CommonUtils.showWait(false);
                    }

                }).always(function () {
                    CommonUtils.showWait(false);
                });
            } else {
                self.mProduct().Tags([]);
                ko.utils.arrayForEach(self.mProduct().ListTags(), function (obj) {
                    self.mProduct().Tags.push(obj.Tags());
                });
                var input = ko.toJSON({ sp: data, tags: self.mProduct().Tags(), tagRef: self.mProduct().ListTagsArticle() });
                $.ajax({
                    type: 'POST',
                    url: CommonUtils.url('/Admin/Product/Update'),
                    cache: false,
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8',
                    data: input,
                    success: function (r) {
                        if (r.success) {
                            $.gritter.add({
                                title: 'Thông báo',
                                text: r.messaging
                            });

                            self.LoadData(r.id);
                        }
                        else { alert(r.messaging) }
                        CommonUtils.showWait(false);
                    }
                }).always(function () {
                    CommonUtils.showWait(false);
                });

            }

        } else {
            self.HasError(true);
            self.Validator().errors.showAllMessages();
        }
    };
    //-----------------Danh Muc-----------------------------
    self.SetdataCollection = function () {
        var collection = new Products.mcollection();
        if (!self.IsaddNew()) {
            collection.idloai(self.selectedSite().maloai);
            collection.iddm(self.selecteddanhmuc().madanhmuc);
            collection.iddmc(self.selecteddanhmuccon().madanhmuccon);
            collection.lable(self.selectedSite().tenloai + ' --> ' + self.selecteddanhmuc().tendanhmuc() + ' --> ' + self.selecteddanhmuccon().tendanhmuccon());
            collection.idsp(self.mProduct().id());
        } else {
            collection.idloai(0);
            collection.iddm(self.selecteddanhmuc().madanhmuc());
            collection.iddmc(self.selecteddanhmuccon().madanhmuccon());
            collection.lable('Trang chủ' + ' --> ' + self.selecteddanhmuc().tendanhmuc() + ' --> ' + self.selecteddanhmuccon().tendanhmuccon());

        }
        return collection;
    };
    self.submitCollection = function () {
        //      self.SetdataCollection();
        CommonUtils.showWait(true);
        $.ajax({
            type: 'POST',
            url: CommonUtils.url('/Admin/Collection/Add'),
            cache: false,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: ko.toJSON(self.SetdataCollection()),
            success: function (r) {
                if (r.success) {
                    $.gritter.add({
                        title: 'Thông báo',
                        text: r.messaging
                    });
                    self.LoadCollection(self.mProduct().id());
                }
                else { alert(r.messaging) }

            }

        }).always(CommonUtils.showWait(false));
    };
    self.removeCollection = function (data) {

        bootbox.confirm("Bạn muốn xóa collection này ?", function (result) {
            if (result) {
                CommonUtils.showWait(true);
                $.ajax({
                    type: 'POST',
                    url: CommonUtils.url('/Admin/Collection/Remove'),
                    cache: false,
                    data: { id: data.id() },
                    success: function (r) {
                        if (r.success) {
                            $.gritter.add({
                                title: 'Thông báo',
                                text: r.messaging
                            });

                            self.LoadCollection(self.mProduct().id());
                        }
                        else { alert(r.messaging) }

                    }

                }).always(CommonUtils.showWait(false));
            }
        })
    };
    self.AddBienthe = function () {
        var bienthe = new Products.mbienthe();
        bienthe.idsp = self.mProduct().id();
        self.Bienthe.push(bienthe);
    };

    self.RemoveBienthe = function (data) {
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
                                self.LoadData(self.mProduct().id());
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
    //----------------IMG-------------------------------
    self.upload = function (data) {
        if (data)
            $(data).fileupload({
                autoUpload: true,
                url: '/Admin/Common/UploadFileProduct',
                dataType: 'json',
                add: function (e, data) {
                    var jqXHR = data.submit()
                        .success(function (data, textStatus, jqXHR) {
                            if (data.isUploaded) {
                                self.InserImg(data.namefile);
                                toastr.success(data.message);
                            }
                            else {
                                toastr.success(data.message);
                            }

                        })
                        .error(function (data, textStatus, errorThrown) {
                            if (typeof (data) != 'undefined' || typeof (textStatus) != 'undefined' || typeof (errorThrown) != 'undefined') {
                                alert(textStatus + errorThrown + data);
                            }
                        });
                },
                fail: function (event, data) {
                    if (data.files[0].error) {
                        alert(data.files[0].error);
                    }
                }
            });
    };
    self.uploadThumbnail = function (data) {
        if (data)
            $(data).fileupload({
                autoUpload: true,
                url: '/Admin/Common/UploadFileProductThumbnail',
                dataType: 'json',
                add: function (e, data) {
                    var jqXHR = data.submit()
                        .success(function (data, textStatus, jqXHR) {
                            if (data.isUploaded) {
                                toastr.success(data.message);
                            }
                            else {
                                toastr.success(data.message);
                            }

                        })
                        .error(function (data, textStatus, errorThrown) {
                            if (typeof (data) != 'undefined' || typeof (textStatus) != 'undefined' || typeof (errorThrown) != 'undefined') {
                                alert(textStatus + errorThrown + data);
                            }
                        });
                },
                fail: function (event, data) {
                    if (data.files[0].error) {
                        alert(data.files[0].error);
                    }
                }
            });
    };
    
    self.InserImg = function (value) {
        var idpro = self.mProduct().id() ? self.mProduct().id() : 0;
        $.ajax({
            type: 'POST',
            cache: false,
            data: { refid: idpro, typeid: 1, url: value },
            url: CommonUtils.url('/Admin/Common/InsertImg'),
            success: function (data) {
                if (self.mProduct().id())
                    self.LoadData(self.mProduct().id());
                else {
                    self.ImgArray.push(ko.mapping.fromJS(data.model, self.Imgobject()));
                    self.mProduct().shop_image().push(ko.mapping.fromJS(data.model, {}, self.Imgobject()));
                }
            },

        });
    };
    self.RemoveImg = function (data) {
        bootbox.confirm("Bạn muốn xóa hình này", function (r) {
            if (r) {
                CommonUtils.showWait(true)
                $.ajax({
                    type: 'POST',
                    url: CommonUtils.url('/Admin/Common/RemoveImage'),
                    cache: false,
                    data: { idimg: data.id() },
                    success: function (result) {
                        //   $('#modal-update-img').modal('hide');
                        if (self.mProduct().id())
                            self.LoadData(self.mProduct().id());
                        else
                            self.ImgArray.remove(data);
                        if (result.success) {
                            $.gritter.add({
                                title: 'Thông báo',
                                text: result.messaging
                            });
                            CommonUtils.showWait(true);
                        }
                        else {
                            alert(result.messaging)
                        }
                        CommonUtils.showWait(false);
                    }
                });

            }
        });
    };
    self.UpdateImg = function (data) {
        self.Imgobject(data);
        $('#modal-update-img').modal('show');

    };
    self.SubmitUpdateImg = function (data) {
        $.ajax({
            type: 'POST',
            url: CommonUtils.url('/Admin/Common/UpdateImg'),
            cache: false,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: ko.toJSON(data),
            success: function (result) {
                $('#modal-update-img').modal('hide');
                if (result.success) {
                    $.gritter.add({
                        title: 'Thông báo',
                        text: result.messaging
                    });
                    if (self.mProduct().id())
                        self.LoadData(self.mProduct().id());
                    CommonUtils.showWait(true);
                }
                else {
                    alert(result.messaging)
                }
                CommonUtils.showWait(false);
            }
        });

    };
    //----------------Link-------------------------------

    self.Cancel_Click = ko.computed(function () {
        return CommonUtils.url('Product#/list');
    });
    self.linkback = ko.computed(function () {
        return CommonUtils.url('product#/list');
    });
    self.linkadd = function () {
        return CommonUtils.url('product#/new');
    };
    self.IsaddNew = ko.observable();
    self.IsaddNew.subscribe(function (v) {
        if (v) {
            self.ImgArray([]);
            self.mProduct(new Products.mProduct);
        }
    });
    self.ResetData = function () {
        self.ImgArray([]);
        self.mProduct().id(undefined);
        self.mProduct().masp("");
        self.mProduct().madanhmuc("");
        self.mProduct().madanhmuccon("");
        self.mProduct().tensp("");
        self.mProduct().gia("");
        self.mProduct().giakm("");
        self.mProduct().chitiet("");
        self.mProduct().thongtin("");

        self.mProduct().spurl("");
        self.mProduct().spkeywords("");
        self.mProduct().spdescription("");

        self.mProduct().hide("");
        self.mProduct().ischecksaleoff("");
        self.mProduct().ischeckout("");
        self.mProduct().ischeckgift("");

        self.mProduct().gift("");
        self.mProduct().timestart("");
        self.mProduct().timeend("");
    };
    self.ischeckgift.subscribe(function (value) {
        self.mProduct().ischeckgift(value);
        self.InitFinishGift(true);

    });
    self.linkout = function () {
        return '/tin-tuc/' + self.mProduct().spurl() + '.html';
    };
    self.RemoveProduct = function (val) {
        bootbox.confirm("Bạn muốn xóa sản phầm này. Sản phẩm này xóa sẽ không được phục hồi", function (r) {
            if (r) {
                CommonUtils.showWait(true)
                $.ajax({
                    type: 'POST',
                    url: CommonUtils.url('/Admin/Product/DeleteProduct'),
                    cache: false,
                    data: { id: val.id() },
                    success: function (result) {
                        if (result.success) {
                            $.gritter.add({
                                title: 'Thông báo',
                                text: result.messaging
                            });
                            window.location.href = '#/list';
                        }
                        else {
                            alert(result.messaging)
                        }
                        CommonUtils.showWait(false);
                    }
                });

            }
        });
    };
    //************Tags************
    self.ListTags = ko.observableArray();
    self.TagAdd = ko.observable();
    self.IsTagAdd = ko.observable(true);
    self.TagAdd.subscribe(function (obj) {
        if (obj) {
            var model = ko.observable(new Products.msys_tags_Ref);
            model().Tags(obj);
            self.mProduct().ListTags.push(model());
            self.TagAdd(undefined);
        }
    });
    self.ClickAddTags = function (obj) {
        if (obj && !obj.IsActive()) {
            var model = ko.observable(new Products.msys_tags_Ref);
            model().Tags(obj.Tags());
            self.mProduct().ListTags.push(model());
            obj.IsActive(true);
        }
    };
    self.RemoveTag = function (obj) {
        if (obj.Id()) {
            bootbox.confirm("Bạn muốn xóa tags ?", function (result) {
                if (result) {
                    CommonUtils.showWait(true);
                    if (obj) {
                        $.ajax({
                            type: "POST",
                            url: '/Admin/Product/RemoveTag',
                            data: { IdTags: obj.Id() },
                            success: function (data) {
                                if (data) {
                                    $.gritter.add({
                                        title: 'Thông báo',
                                        text: 'Cập nhật Tag thành công'
                                    });
                                    self.LoadData(self.ProductId());
                                }
                                else {
                                    alert("Xóa tags không thành công");
                                }
                            },
                            error: function () {
                                alert("Load Group that bai");
                            }
                        }).always(function () {
                            CommonUtils.showWait(false);
                        });
                    };
                }
            });
        } else {
            self.mProduct().ListTags.remove(obj);
        }
    };
    self.ClickAddTagsArticle = function (obj) {
        if (obj && !obj.IsActive()) {
            var model = ko.observable(new Products.msys_tags_Ref);
            model().RefId(self.mProduct().id());
            model().RefTag(obj.Id());
            model().Tags(obj.Tags());
            self.mProduct().ListTagsArticle.push(model());
            obj.IsActive(true);


        }
    };
   
    self.MoreTags = function () {
        if (self.IsMoreTag())
            self.IsMoreTag(false);
        else
            self.IsMoreTag(true);
    };
    self.MoreTags1 = function () {
        if (self.IsMoreTag1())
            self.IsMoreTag1(false);
        else
            self.IsMoreTag1(true);
    };
};


