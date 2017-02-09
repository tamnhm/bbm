window.waitcount = 0;
var CommonUtils = {
    getBindingRoot: function () {
        return document.getElementById('viewareaid');
    },
    getAvatarUrl: function (email, size) {
        var size = size || 40;
        var hash = CryptoJS.MD5(email);
        var ret = 'https://secure.gravatar.com/avatar/' + hash + ".jpg?s=29";
        //ret = ret + '&d=' + encodeURIComponent(location.origin + document.root + 'Content/images/logo40.png');
        ret = ret + '&d=' + encodeURIComponent('https://s.gravatar.com/avatar/18f9f20ec1a7be8020924ce0048b6ef2?s=' + size);
        return ret;
    },
    init: function () {

        infuser.defaults.templateUrl = location.origin + document.root + "Template";
        infuser.defaults.templateSuffix = "";
        infuser.defaults.templatePrefix = "";

        Sammy(function () {
            this.disable_push_state = true;
        })

        ko.validation.rules.pattern.message = 'Không hợp lệ';
        ko.validation.configure({
            registerExtenders: true,
            messagesOnModified: true,
            insertMessages: false,
            parseInputAttributes: true,
            messageTemplate: null,
            decorateElement: true,
            errorClass: "input-validation-error"
        });

        ko.validation.localize({
            required: 'Dữ liệu bắt buộc nhập.',
            min: 'Giá trị nhập phải lớn hơn hoặc bằng {0}.',
            max: 'Giá trị nhập phải nhỏ hơn hoặc bằng {0}.',
            minLength: 'Phải nhập ít nhất {0} ký tự.',
            maxLength: 'Không được nhập quá {0} ký tự.',
            pattern: 'Giá trị không hợp lệ.',
            step: 'The value must increment by {0}',
            email: 'Địa chỉ email không hợp lệ',
            date: 'Please enter a proper date',
            dateISO: 'Please enter a proper date',
            number: 'Please enter a number',
            digit: 'Chỉ được nhập số',
            phoneUS: 'Please specify a valid phone number',
            equal: 'Values must equal',
            notEqual: 'Please choose another value.',
            unique: 'Please make sure the value is unique.'
        });

        var handleTimeout = function (request) {

            if (request.getResponseHeader('SYSTEM') == 'TIMEOUT') {
                CommonUtils.confirm('Thông báo', 'Bạn cần đăng nhập lại, bạn có muốn chuyển đến trang đăng nhập ?\nChú ý : Khi chuyển đến trang đăng nhập, các trạng thái nhập liệu hiện tại sẽ mất',
                    function (isOK) {
                        if (isOK)
                            location.reload();
                    });
                return true;
            }

            var ddos = request.getResponseHeader('DDOS');

            if (ddos) {
                toastr.error(ddos);
                return true;
            }

            return false;
        };

        $(document).ajaxError(function (e, jqxhr, settings, exception) {
            console.log(exception);
            if (!handleTimeout(jqxhr)) {

                if (jqxhr.responseText) {
                    toastr.error(jqxhr.responseText, 'Thông báo');
                }
                else
                    toastr.error('Đã có lỗi xảy ra, lỗi không xác định', 'Thông báo');
            }
        }).ajaxSuccess(function (event, request) {
            handleTimeout(request);
        })
        ;

        $.ajaxPrefilter(function (options, originalOptions, jqXHR) {
            var verificationToken = $("[name='__RequestVerificationToken']").val();
            if (verificationToken) {
                jqXHR.setRequestHeader("X-Request-Verification-Token", verificationToken);
            }
        });

        Globalize.culture('vi-VN');

        //require.config({
        //    baseUrl: document.root,
        //    paths: {
        //        noext: 'Scripts/Libs/noext'
        //    }
        //});

        //require.onError = function (err) {
        //    console.log(err);
        //    toastr.error("Đã có lỗi khi nạp dữ liệu", "Thông báo");
        //};
    },
    confirm: function (title, msg, callback, buttonOK) {
        buttonOK = (typeof buttonOK === 'undefined') ? 'OK' : buttonOK;
        $('#confirmModal').on('show.bs.modal', function () {
            $('#confirmModal .modal-title').text(title);
            $('#confirmModal .modal-body').text(msg);
            $('#confirmModal .btnOk').text(buttonOK);
            $('#confirmModal .btnCancel').bind('click', function () {
                $("#confirmModal").modal('hide');
            });
            $('#confirmModal .btnOk').bind('click', function () {
                callback(true);
                $("#confirmModal").modal('hide');
            });
            $('#confirmModal').off('show.bs.modal');
        }).on('hide.bs.modal', function () {
            $('#confirmModal .btnCancel').unbind('click');
            $('#confirmModal .btnOk').unbind('click');
            $('#confirmModal').off('hide.bs.modal');
        }).modal({
            "backdrop": "static",
            "keyboard": true,
            "show": true
        });
    },
    showWait: function (show) {
        if (show) {
            // window.waitcount = window.waitcount + 1;
            $('.pageloading').show();

        }
        else {
            // window.waitcount = window.waitcount - 1;
            // if (window.waitcount == 0) {
            $('.pageloading').hide();
            //  }
        }
    },
    converReplaceABC: function (str) {
        str = str.toLowerCase();
        str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
        str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
        str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
        str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
        str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
        str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
        str = str.replace(/đ/g, "d");
        str = str.toUpperCase();
        return str;
    },
    isPhoneNumber: function (s) {
        if (s.length == 10 || s.length == 11) {
            var rePhoneNumber = /^\d+$/;
            if (rePhoneNumber.test(s))
                return true;
        }
        return false;
    },
    isMoney: function (s) {
        if (s.length > 0) {
            var rePhoneNumber = /\d|\,/;
            if (rePhoneNumber.test(s))
                return true;
        }
        return false;
    },
    replaceAll: function (iStr, v1, v2) {
        var i = 0, oStr = '', j = v1.length;
        while (i < iStr.length) {
            if (iStr.substr(i, j) == v1) {
                oStr += v2;
                i += j
            }
            else {
                oStr += iStr.charAt(i);
                i++;
            }
        }
        return oStr;
    },
    ajaxGet: function (url, callBack, data_type) {
        if (data_type == undefined) data_type = "json";
        $.ajax({
            url: url,
            type: "GET",
            dataType: data_type,
            cache: false,
            beforeSend: function (xhr) {
                CommonUtils.showWait(true);
            },
            success: function (data) {
                CommonUtils.showWait(false);
                callBack(data);
            },
            error: function (ex) {
                callBack(ex.responseText);
            }
        });
    },


    postAjax: function (url, dataPost, callBack, data_type) {
        $.ajax({
            url: url,
            type: "POST",
            dataType: data_type,
            cache: false,
            data: dataPost,
            beforeSend: function (xhr) {
                CommonUtils.showWait(true);
            },
            success: function (data) {
                CommonUtils.showWait(false);
            },
            error: function (ex) {
                callBack(ex.responseText);
            }

        });
    },
    isUrl: function (s) {
        var reg = /(ftp|http|https):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?/;
        return reg.test(s);
    },
    IsEmail: function (email) {
        var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        if (!filter.test(email)) {
            return false;
        } else {
            return true;
        }
    },

    MapArray: function (rawArray, model) {
        var mappedData = ko.utils.arrayMap(rawArray, function (value) {
            var item = new model();
            ko.mapping.fromJS(value, {}, item);
            return item;
        });
        return mappedData;
    },

    MapKO: function (rawData) {
        var mappedData = ko.utils.arrayMap(rawData, function (value) {
            var result = ko.mapping.fromJS(value);
            return result;
        });
        return mappedData;
    },

    url: function (path) {
        return (path);
    },

    getDomainBySub: function (sub) {
        return (sub + '.' + document.Domain);
    },

    getDefaultIp: function () {
        return document.DefaultIP;
    },

    ToBase64: function (byteArr) {
        if (byteArr == undefined || byteArr == null) return null;
        return btoa(String.fromCharCode.apply(null, new Uint8Array(byteArr)));
    },
    FromDateJsonToDateJS: function (value) {
        var date = new Date(Date.parse(value));
        return date.getUTCDate() + "/" + (date.getMonth() + 1) + "/" + (date.getFullYear());
    },
    FromTimeJsonToTimeJS: function (value) {
        var date = new Date(Date.parse(value));
        return date.getMinutes() + ":" + date.getSeconds();
    },
    SortTable: function (sortcallback, selector) {
        var element = '.sorttable tbody';
        if (selector)
            element = selector;
        $(element).sortable({
            helper: function (e, tr) {
                var $originals = tr.children();
                var $helper = tr.clone();
                $helper.children().each(function (index) {
                    $(this).width($originals.eq(index).width())
                });
                return $helper;
            },
            stop: function (e, ui) {
                var listId = $(this).sortable('toArray', { attribute: 'key' });
                sortcallback(listId);
            },
            placeholder: "ui-state-highlight"
        }).disableSelection();
    },
    DisableSortTable: function (selector) {
        var element = '.sorttable tbody';
        if (selector)
            element = selector;
        $(element).sortable("destroy");
    },
    formatNumber: function (number) {
        number = number.toFixed(0) + '';
        x = number.split('.');
        x1 = x[0];
        x2 = x.length > 1 ? '.' + x[1] : '';
        var rgx = /(\d+)(\d{3})/;
        while (rgx.test(x1)) {
            x1 = x1.replace(rgx, '$1' + ',' + '$2');
        }
        return x1 + x2;
    },

    NonUnicode: function (value) {

        var arr1 = new Array("á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
                                        "đ",
                                        "é", "è", "ẻ", "ẽ", "ẹ", "ê", "ế", "ề", "ể", "ễ", "ệ",
                                        "í", "ì", "ỉ", "ĩ", "ị",
                                        "ó", "ò", "ỏ", "õ", "ọ", "ô", "ố", "ồ", "ổ", "ỗ", "ộ", "ơ", "ớ", "ờ", "ở", "ỡ", "ợ",
                                        "ú", "ù", "ủ", "ũ", "ụ", "ư", "ứ", "ừ", "ử", "ữ", "ự",
                                        "ý", "ỳ", "ỷ", "ỹ", "ỵ", " ", "/", "&", "?");

        var arr2 = new Array("a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
                                        "d",
                                        "e", "e", "e", "e", "e", "e", "e", "e", "e", "e", "e",
                                        "i", "i", "i", "i", "i",
                                        "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o",
                                        "u", "u", "u", "u", "u", "u", "u", "u", "u", "u", "u",
                                        "y", "y", "y", "y", "y", "-", "-", "-", "-");

        if (value != null && value.length > 0) {
            value = value.toLowerCase().substring(0, 100).trim();
            var outString = value;
            var stringLength = 0;
            var countSpace = 0;

            while (stringLength < value.length) {
                if (value[stringLength] == " ")
                    countSpace++;
                else
                    countSpace = 0;

                if (countSpace > 1)
                    outString = outString.replace(" ", "");
                else {
                    var idx = arr1.indexOf(value[stringLength]);
                    if (idx != -1) {
                        outString = outString.replace(arr1[idx], arr2[idx]);
                    }
                }
                stringLength++;
            }
            outString = outString.replace("---", "-").replace("--", "-");
            return outString;
        }
        return value;
    },
    bytesToSizeFormat: function (bytesString) {
        var sizes = ['B', 'kB', 'MB', 'GB', 'TB'];
        if (bytesString == 0) return '0 Bytes';
        var i = parseInt(Math.floor(Math.log(bytesString) / Math.log(1024)));
        return Math.round(bytesString / Math.pow(1024, i), 2) + ' ' + sizes[i];
    },
    isImageFile: function (fileName) {
        return fileName.match(/\.(jpg|jpeg|png|gif|bmp)$/);
    },
    openNewTab: function (url) {
        var win = window.open(url, '_blank');
        win.focus();
    },
    closePopupModal: function (className) {
        $('.' + className).modal('hide');
    },
    openPopupModal: function (className) {
        $('.' + className).modal('show');
    },
    togglePopupModal: function (className) {
        $('.' + className).modal('toggle');
    },
    ImageNotFound: function () {
        $('img').error(function () {
            $(this).attr('src', '/Content/theme/images/img_not_available.png');
        });
    },

    SortArray: function (array, direction, comparison) {
        if (array == null) {
            return [];
        }

        for (var oIndex = 0; oIndex < array.length; oIndex++) {
            var oItem = array[oIndex];
            for (var iIndex = oIndex + 1; iIndex < array.length; iIndex++) {
                var iItem = array[iIndex];
                var isOrdered = comparison(oItem, iItem);
                if (isOrdered == direction) {
                    array[iIndex] = oItem;
                    array[oIndex] = iItem;
                    oItem = iItem;
                }
            }
        }

        return array;
    },
    GetOption: function (name, value, filterValue) {
        var option = {
            Name: name,
            Value: value,
            FilterValue: filterValue
        };
        return option;
    },
    CompareCaseInsensitive: function (left, right) {
        if (left == null) {
            return right == null;
        }
        else if (right == null) {
            return false;
        }

        return left().toUpperCase() <= right().toUpperCase();
    },
    GetObservableArray: function (array) {
        if (typeof (array) == 'function') {
            return array;
        }

        return ko.observableArray(array);
    },
    ExtractModels: function (parent, data, constructor) {
        var models = [];
        if (data == null) {
            return models;
        }

        for (var index = 0; index < data.length; index++) {
            var row = data[index];
            var model = new constructor(row, parent);
            models.push(model);
        }

        return models;
    },
    FilterModel: function (filters, records) {
        var self = this;
        self.records = CommonUtils.GetObservableArray(records);
        self.filters = ko.observableArray(filters);
        self.activeFilters = ko.computed(function () {
            var filters = self.filters();
            var activeFilters = [];
            for (var index = 0; index < filters.length; index++) {
                var filter = filters[index];
                if (filter.CurrentOption) {
                    var filterOption = filter.CurrentOption();
                    if (filterOption && filterOption.FilterValue != null) {
                        var activeFilter = {
                            Filter: filter,
                            IsFiltered: function (filter, record) {
                                var filterOption = filter.CurrentOption();
                                if (!filterOption) {
                                    return;
                                }

                                var recordValue = filter.RecordValue(record);
                                return recordValue != filterOption.FilterValue; NoMat
                            }
                        };
                        activeFilters.push(activeFilter);
                    }
                }
                else if (filter.Value) {
                    var filterValue = filter.Value();
                    if (filterValue && filterValue != "") {
                        var activeFilter = {
                            Filter: filter,
                            IsFiltered: function (filter, record) {
                                var filterValue = filter.Value();
                                filterValue = filterValue.toUpperCase();

                                var recordValue = filter.RecordValue(record);
                                recordValue = recordValue().toUpperCase();
                                return recordValue.indexOf(filterValue) == -1;
                            }
                        };
                        activeFilters.push(activeFilter);
                    }
                }
            }

            return activeFilters;
        });
        self.filteredRecords = ko.computed(function () {
            var records = self.records();
            var filters = self.activeFilters();
            if (filters.length == 0) {
                return records;
            }

            var filteredRecords = [];
            for (var rIndex = 0; rIndex < records.length; rIndex++) {
                var isIncluded = true;
                var record = records[rIndex];
                for (var fIndex = 0; fIndex < filters.length; fIndex++) {
                    var filter = filters[fIndex];
                    var isFiltered = filter.IsFiltered(filter.Filter, record);
                    if (isFiltered) {
                        isIncluded = false;
                        break;
                    }
                }

                if (isIncluded) {
                    filteredRecords.push(record);
                }
            }

            return filteredRecords;
        }).extend({ throttle: 200 });
    },
    PagerModel: function (records) {
        var self = this;
        self.pageSizeOptions = ko.observableArray([1, 5, 25, 50, 100, 250, 500]);

        self.records = CommonUtils.GetObservableArray(records);
        self.currentPageIndex = ko.observable(self.records().length > 0 ? 0 : -1);
        self.currentPageSize = ko.observable(25);
        self.recordCount = ko.computed(function () {
            return self.records().length;
        });
        self.maxPageIndex = ko.computed(function () {
            return Math.ceil(self.records().length / self.currentPageSize()) - 1;
        });
        self.currentPageRecords = ko.computed(function () {
            var newPageIndex = -1;
            var pageIndex = self.currentPageIndex();
            var maxPageIndex = self.maxPageIndex();
            if (pageIndex > maxPageIndex) {
                newPageIndex = maxPageIndex;
            }
            else if (pageIndex == -1) {
                if (maxPageIndex > -1) {
                    newPageIndex = 0;
                }
                else {
                    newPageIndex = -2;
                }
            }
            else {
                newPageIndex = pageIndex;
            }

            if (newPageIndex != pageIndex) {
                if (newPageIndex >= -1) {
                    self.currentPageIndex(newPageIndex);
                }

                return [];
            }

            var pageSize = self.currentPageSize();
            var startIndex = pageIndex * pageSize;
            var endIndex = startIndex + pageSize;
            return self.records().slice(startIndex, endIndex);
        }).extend({ throttle: 5 });
        self.moveFirst = function () {
            self.changePageIndex(0);
        };
        self.movePrevious = function () {
            self.changePageIndex(self.currentPageIndex() - 1);
        };
        self.moveNext = function () {
            self.changePageIndex(self.currentPageIndex() + 1);
        };
        self.moveLast = function () {
            self.changePageIndex(self.maxPageIndex());
        };
        self.changePageIndex = function (newIndex) {
            if (newIndex < 0
                || newIndex == self.currentPageIndex()
                || newIndex > self.maxPageIndex()) {
                return;
            }

            self.currentPageIndex(newIndex);
        };
        self.onPageSizeChange = function () {
            self.currentPageIndex(0);
        };
        self.renderPagers = function () {
            var pager = "<div><a href=\"#\" data-bind=\"click: pager.moveFirst, enable: pager.currentPageIndex() > 0\">&lt;&lt;</a><a href=\"#\" data-bind=\"click: pager.movePrevious, enable: pager.currentPageIndex() > 0\">&lt;</a>Page <span data-bind=\"text: pager.currentPageIndex() + 1\"></span> of <span data-bind=\"text: pager.maxPageIndex() + 1\"></span> [<span data-bind=\"text: pager.recordCount\"></span> Record(s)]<select data-bind=\"options: pager.pageSizeOptions, value: pager.currentPageSize, event: { change: pager.onPageSizeChange }\"></select><a href=\"#\" data-bind=\"click: pager.moveNext, enable: pager.currentPageIndex() < pager.maxPageIndex()\">&gt;</a><a href=\"#\" data-bind=\"click: pager.moveLast, enable: pager.currentPageIndex() < pager.maxPageIndex()\">&gt;&gt;</a></div>";
            $("div.Pager").html(pager);
        };
        self.renderNoRecords = function () {
            var message = "<span data-bind=\"visible: pager.recordCount() == 0\">No records found.</span>";
            $("div.NoRecords").html(message);
        };

        self.renderPagers();
        self.renderNoRecords();
    },

    SorterModel: function (sortOptions, records) {
        var self = this;
        self.records = CommonUtils.GetObservableArray(records);
        self.sortOptions = ko.observableArray(sortOptions);
        self.sortDirections = ko.observableArray([
            {
                Name: "Asc",
                Value: "Asc",
                Sort: false
            },
            {
                Name: "Desc",
                Value: "Desc",
                Sort: true
            }]);
        self.currentSortOption = ko.observable(self.sortOptions()[0]);
        self.currentSortDirection = ko.observable(self.sortDirections()[0]);
        self.orderedRecords = ko.computed(function () {
            var records = self.records();
            var sortOption = self.currentSortOption();
            var sortDirection = self.currentSortDirection();
            if (sortOption == null || sortDirection == null) {
                return records;
            }

            var sortedRecords = records.slice(0, records.length);
            CommonUtils.SortArray(sortedRecords, sortDirection.Sort, sortOption.Sort);
            return sortedRecords;
        }).extend({ throttle: 5 });
    },
    ErrorImage: function (img) {
        img.src = '/Content/me/img/no-image.png';

    }

};

/*Plugin jQuery*/

// Numeric only control handler
jQuery.fn.ForceNumericOnly =
function () {
    return this.each(function () {
        $(this).keydown(function (e) {
            var key = e.charCode || e.keyCode || 0;
            // allow backspace, tab, delete, arrows, numbers and keypad numbers ONLY
            return (
                key == 8 ||
                key == 9 ||
                key == 46 ||
                (key >= 37 && key <= 40) ||
                (key >= 48 && key <= 57) ||
                (key >= 96 && key <= 105));
        });
    });
};


jQuery.fn.ForceClearTextOnly =
function () {
    return this.each(function () {
        $(this).keydown(function (e) {
            var key = e.charCode || e.keyCode || 0;
            // allow backspace, tab, delete, arrows, numbers and keypad numbers ONLY
            return (
                key == 8 ||
                key == 9 ||
                key == 46 ||
                (key >= 37 && key <= 40) ||
                (key >= 48 && key <= 57) ||
                (key >= 65 && key <= 90) ||
                (key >= 97 && key <= 122) ||
                (key >= 96 && key <= 105));
        });
    });
};

// Paging

//function PagerModel(records) {
//    var self = this;
//    self.pageSizeOptions = ko.observableArray([1, 5, 25, 50, 100, 250, 500]);

//    self.records = GetObservableArray(records);
//    self.currentPageIndex = ko.observable(self.records().length > 0 ? 0 : -1);
//    self.currentPageSize = ko.observable(25);
//    self.recordCount = ko.computed(function () {
//        return self.records().length;
//    });
//    self.maxPageIndex = ko.computed(function () {
//        return Math.ceil(self.records().length / self.currentPageSize()) - 1;
//    });
//    self.currentPageRecords = ko.computed(function () {
//        var newPageIndex = -1;
//        var pageIndex = self.currentPageIndex();
//        var maxPageIndex = self.maxPageIndex();
//        if (pageIndex > maxPageIndex) {
//            newPageIndex = maxPageIndex;
//        }
//        else if (pageIndex == -1) {
//            if (maxPageIndex > -1) {
//                newPageIndex = 0;
//            }
//            else {
//                newPageIndex = -2;
//            }
//        }
//        else {
//            newPageIndex = pageIndex;
//        }

//        if (newPageIndex != pageIndex) {
//            if (newPageIndex >= -1) {
//                self.currentPageIndex(newPageIndex);
//            }

//            return [];
//        }

//        var pageSize = self.currentPageSize();
//        var startIndex = pageIndex * pageSize;
//        var endIndex = startIndex + pageSize;
//        return self.records().slice(startIndex, endIndex);
//    }).extend({ throttle: 5 });
//    self.moveFirst = function () {
//        self.changePageIndex(0);
//    };
//    self.movePrevious = function () {
//        self.changePageIndex(self.currentPageIndex() - 1);
//    };
//    self.moveNext = function () {
//        self.changePageIndex(self.currentPageIndex() + 1);
//    };
//    self.moveLast = function () {
//        self.changePageIndex(self.maxPageIndex());
//    };
//    self.changePageIndex = function (newIndex) {
//        if (newIndex < 0
//            || newIndex == self.currentPageIndex()
//            || newIndex > self.maxPageIndex()) {
//            return;
//        }

//        self.currentPageIndex(newIndex);
//    };
//    self.onPageSizeChange = function () {
//        self.currentPageIndex(0);
//    };
//    self.renderPagers = function () {
//        var pager = "Page <span data-bind=\"text: pager.currentPageIndex() + 1\"></span> of <span data-bind=\"text: pager.maxPageIndex() + 1\"></span> [<span data-bind=\"text: pager.recordCount\"></span> Record(s)]&nbsp;&nbsp;<select data-bind=\"options: pager.pageSizeOptions, value: pager.currentPageSize, event: { change: pager.onPageSizeChange }\"></select>";

//        pager += "<div><a class=\"btn-paging\" href=\"#\" data-bind=\"click: pager.moveFirst, enable: pager.currentPageIndex() > 0\">&lt;&lt;</a><a class=\"btn-paging\"  href=\"#\" data-bind=\"click: pager.movePrevious, enable: pager.currentPageIndex() > 0\">&lt;</a><a class=\"btn-paging\" href=\"#\" data-bind=\"click: pager.moveNext, enable: pager.currentPageIndex() < pager.maxPageIndex()\">&gt;</a><a class=\"btn-paging\" href=\"#\" data-bind=\"click: pager.moveLast, enable: pager.currentPageIndex() < pager.maxPageIndex()\">&gt;&gt;</a></div><br/>";

//        $("div.Pager").html(pager);

//    };
//    self.renderNoRecords = function () {
//        var message = "<span data-bind=\"visible: pager.recordCount() == 0\">No records found.</span>";
//        $("div.NoRecords").html(message);
//    };

//    self.renderPagers();
//    self.renderNoRecords();
//}

//function SorterModel(sortOptions, records) {
//    var self = this;
//    self.records = GetObservableArray(records);
//    self.sortOptions = ko.observableArray(sortOptions);
//    self.sortDirections = ko.observableArray([
//        {
//            Name: "Asc",
//            Value: "Asc",
//            Sort: false
//        },
//        {
//            Name: "Desc",
//            Value: "Desc",
//            Sort: true
//        }]);
//    self.currentSortOption = ko.observable(self.sortOptions()[0]);
//    self.currentSortDirection = ko.observable(self.sortDirections()[0]);
//    self.orderedRecords = ko.computed(function () {
//        var records = self.records();
//        var sortOption = self.currentSortOption();
//        var sortDirection = self.currentSortDirection();
//        if (sortOption == null || sortDirection == null) {
//            return records;
//        }

//        var sortedRecords = records.slice(0, records.length);
//        SortArray(sortedRecords, sortDirection.Sort, sortOption.Sort);
//        return sortedRecords;
//    }).extend({ throttle: 5 });
//}

//function FilterModel(filters, records) {
//    var self = this;
//    self.records = GetObservableArray(records);
//    self.filters = ko.observableArray(filters);
//    self.activeFilters = ko.computed(function () {
//        var filters = self.filters();
//        var activeFilters = [];
//        for (var index = 0; index < filters.length; index++) {
//            var filter = filters[index];
//            if (filter.CurrentOption) {
//                var filterOption = filter.CurrentOption();
//                if (filterOption && filterOption.FilterValue != null) {
//                    var activeFilter = {
//                        Filter: filter,
//                        IsFiltered: function (filter, record) {
//                            var filterOption = filter.CurrentOption();
//                            if (!filterOption) {
//                                return;
//                            }

//                            var recordValue = filter.RecordValue(record);
//                            return recordValue != filterOption.FilterValue; NoMat
//                        }
//                    };
//                    activeFilters.push(activeFilter);
//                }
//            }
//            else if (filter.Value) {
//                var filterValue = filter.Value();
//                if (filterValue && filterValue != "") {
//                    var activeFilter = {
//                        Filter: filter,
//                        IsFiltered: function (filter, record) {
//                            var filterValue = filter.Value();
//                            filterValue = filterValue.toUpperCase();

//                            var recordValue = filter.RecordValue(record);
//                            recordValue = recordValue().toUpperCase();
//                            return recordValue.indexOf(filterValue) == -1;
//                        }
//                    };
//                    activeFilters.push(activeFilter);
//                }
//            }
//        }

//        return activeFilters;
//    });
//    self.filteredRecords = ko.computed(function () {
//        var records = self.records();
//        var filters = self.activeFilters();
//        if (filters.length == 0) {
//            return records;
//        }

//        var filteredRecords = [];
//        for (var rIndex = 0; rIndex < records.length; rIndex++) {
//            var isIncluded = true;
//            var record = records[rIndex];
//            for (var fIndex = 0; fIndex < filters.length; fIndex++) {
//                var filter = filters[fIndex];
//                var isFiltered = filter.IsFiltered(filter.Filter, record);
//                if (isFiltered) {
//                    isIncluded = false;
//                    break;
//                }
//            }

//            if (isIncluded) {
//                filteredRecords.push(record);
//            }
//        }

//        return filteredRecords;
//    }).extend({ throttle: 200 });
//}

//function ExtractModels(parent, data, constructor) {
//    var models = [];
//    if (data == null) {
//        return models;
//    }

//    for (var index = 0; index < data.length; index++) {
//        var row = data[index];
//        var model = new constructor(row, parent);
//        models.push(model);
//    }

//    return models;
//}

//function GetObservableArray(array) {
//    if (typeof (array) == 'function') {
//        return array;
//    }

//    return ko.observableArray(array);
//}

//function CompareCaseInsensitive(left, right) {
//    if (left == null) {
//        return right == null;
//    }
//    else if (right == null) {
//        return false;
//    }

//    return left().toUpperCase() <= right().toUpperCase();
//}

//function GetOption(name, value, filterValue) {
//    var option = {
//        Name: name,
//        Value: value,
//        FilterValue: filterValue
//    };
//    return option;
//}

//function SortArray(array, direction, comparison) {
//    if (array == null) {
//        return [];
//    }

//    for (var oIndex = 0; oIndex < array.length; oIndex++) {
//        var oItem = array[oIndex];
//        for (var iIndex = oIndex + 1; iIndex < array.length; iIndex++) {
//            var iItem = array[iIndex];
//            var isOrdered = comparison(oItem, iItem);
//            if (isOrdered == direction) {
//                array[iIndex] = oItem;
//                array[oIndex] = iItem;
//                oItem = iItem;
//            }
//        }
//    }

//    return array;
//}

