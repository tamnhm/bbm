(function (ko) {

    ko.bindingHandlers.optionsgroup = {
        init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            $(element).change(function () {
                var val = $(element).val();
                var selectvalue = allBindings().value;

                if (ko.isWriteableObservable(selectvalue)) {
                    selectvalue(val);
                }
            });
        },
        update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            var selectedValue = ko.utils.unwrapObservable(allBindings().value);
            var val = ko.utils.unwrapObservable(valueAccessor());
            $(element).html('');
            ko.utils.arrayForEach(val, function (group) {
                $(element).append("<optgroup label='" + group.name + "'>");
                ko.utils.arrayForEach(group.arrays, function (it) {
                    var ht = "";
                    if (selectedValue == ko.utils.unwrapObservable(it.Id)) {
                        ht = "<option selected='selected' value='" + ko.utils.unwrapObservable(it.Id) + "'>" + ko.utils.unwrapObservable(it.Name) + "</option>";
                    }
                    else {
                        ht = "<option value='" + ko.utils.unwrapObservable(it.Id) + "'>" + ko.utils.unwrapObservable(it.Name) + "</option>";
                    }

                    $(element).append(ht);
                });

                $(element).append("</optgroup>");
            });
        }
    }

    ko.bindingHandlers.grouped = {
        init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            ko.bindingHandlers.foreach.init(element, function () { return []; }, allBindings, viewModel, bindingContext);
        },
        update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            var options = valueAccessor();
            var groups = _(ko.utils.unwrapObservable(options.data)).chain().groupBy(options.by).map(function (value, key) { return { key: key, items: value }; }).value();

            ko.bindingHandlers.foreach.update(element, function () { return groups; }, allBindings, viewModel, bindingContext);
        }
    };

    ko.bindingHandlers.returnKey = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            var value = ko.utils.unwrapObservable(valueAccessor());
            ko.utils.registerEventHandler(element, 'keydown', function (evt) {
                if (evt.keyCode === 13) {
                    evt.preventDefault();
                    evt.target.blur();
                    value(viewModel);
                }
            });
        }
    };
    //Custom binding for googlemap
    ko.bindingHandlers.googlemap = {
        init: function (element, valueAccessor, allBindings) {
            var latLng = new google.maps.LatLng(1, 1);
            mapOptions = {
                zoom: 15,
                center: latLng,
                mapTypeControl: false,
                zoomControl: false,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            var map = new google.maps.Map(element, mapOptions);
            $(element).data('map', map);
        },
        update: function (element, valueAccessor) {
            var value = valueAccessor();
            var valueUnwrapped = ko.utils.unwrapObservable(value);
            var latLng = new google.maps.LatLng(valueUnwrapped.Latitude, valueUnwrapped.Longtitude);
            var map = $(element).data('map');
            var marker = new google.maps.Marker({
                map: map,
                position: latLng
            });
            map.setCenter(latLng);
        }
    };
    //end

    ko.bindingHandlers.featherEditor = {
        update: function (element, valueAccessor, allBindings, bindingContext) {
            var option = allBindings().editoroption || {
                saveCallback: function (doneCallback) { }
            }
            var value = valueAccessor();
            var modelPush = ko.unwrap(bindingContext);
            var jElement = $(element);
            if (value) {
                var imgElementId = value;
                var featherEditor = new Aviary.Feather({
                    apiKey: '1b98363a6794d074',
                    apiVersion: 3,
                    theme: 'light',
                    onSave: function (imageID, newURL) {
                        var img = document.getElementById(imageID);
                        var arr = imageID.split('-');
                        var id = 0;
                        if (arr.length > 1) {
                            id = arr[0];
                        }
                        img.src = newURL;
                        option.saveCallback(newURL, function () {
                            featherEditor.close();
                        }, id);
                    }
                });
                jElement.click(function () {
                    var src = '';
                    if (modelPush.Url != undefined) {
                        src = modelPush.Url();
                    }
                    else {
                        src = document.getElementById(imgElementId).src;
                    }
                    featherEditor.launch({
                        image: imgElementId,
                        url: src
                    });
                });
            }
        }
    }

    ko.bindingHandlers.file = {
        init: function (element, valueAccessor, allBindings) {
            var option = allBindings().fileoption || {
                IsSingle: true,
                Ext: "jpg,gif,png,jpeg",
                IsKeepFileName: false
            }
            var value = valueAccessor();
            var isUploading = option.IsUploading;
            var isError = option.IsError;
            var isFinish = option.IsFinish;
            var isAutoUpload = option.AutoUpload ? option.AutoUpload : true;
            var flagUpload = option.FlagUpload ? option.FlagUpload : null;
            var errorCallback = option.ErrorCallback ? option.ErrorCallback : null;
            var successCallback = option.SuccessCallback ? option.SuccessCallback : null;
            var totalFileUpload = 0;
            var numFileUploaded = 0;
            var additionData = option.AdditionData ? option.AdditionData : null
            var jElement = $(element).get(0);
            var randomId = CommonUtils.randomId();
            if ($(jElement).attr("type") == "file") {
                $(jElement).addClass("input-file-upload");
                $(jElement).addClass("width-100-px");
                $(jElement).after("<label id='" + randomId + "' class='note inline'>Chọn file...</label>");
            };
            var uploader = new plupload.Uploader({
                runtimes: 'html5,flash,silverlight,html4',
                multi_selection: !option.IsSingle,
                browse_button: jElement,
                url: option.Url,
                flash_swf_url: CommonUtils.url('Scripts/Libs/plupload/Moxie.swf'),
                silverlight_xap_url: CommonUtils.url('Scripts/Libs/plupload/Moxie.xap'),
                resize: {
                    width: 2048,
                    height: 2048,
                    quality: 90
                },
                filters: {
                    max_file_size: '10mb',
                    mime_types: [
                        { title: "Files", extensions: option.Ext }
                    ]
                },
                multipart_params: {}
            });
            var buildAdditionData = function (a) {
                if (additionData && additionData()().length > 0) {
                    a.settings.multipart_params = {};
                    ko.utils.arrayForEach(additionData()(), function (item) {
                        a.settings.multipart_params[item.name] = item.data;
                    });
                }
            };
            uploader.bind('FilesAdded', function (a, b) {
                $(jElement).removeClass('btn-danger');
                if ($(jElement).attr("type") == "file") $("#" + randomId).text(b[0].name);
                else $(jElement).val("Tải file");
                if (option.IsSingle)
                    if (typeof (value) == 'function') value(undefined);
                    else if (option.IsKeepFileName && value) b[0].name = value.FileName();
                buildAdditionData(a);
                if (isAutoUpload) setTimeout(function () { a.start(); }, 100);
            });
            uploader.bind('BeforeUpload', function (a, b) { });
            uploader.bind('UploadProgress', function (a, b) {
                totalFileUpload = a.files.length;
                if (isUploading) isUploading(true);
                if ($(jElement).attr("type") != "file") $(jElement).val('Đang tải lên...');
            });
            uploader.bind('FileUploaded', function (a, b, rp) {
                if (isUploading) isUploading(false);
                if ($(jElement).attr("type") != "file") $(jElement).val('Tải file');
                var rt = ko.mapping.fromJSON(rp.response);
                if (rt.IsError && rt.IsError()) {
                    if (isError) isError(true);
                    if (isFinish) isFinish(true);
                    if ($(jElement).attr("type") == "file")
                        toastr.error(rt.Message());
                    else {
                        $(jElement).addClass('btn-danger');
                        $(jElement).val("Lỗi tải file");
                        $(jElement).attr('title', rt.Message())
                    }
                    if (isFinish && isFinish()) {
                        a.splice();
                        a.refresh();
                    }
                    if (errorCallback && typeof (errorCallback) == "function")
                        errorCallback(b.name);
                }
                else {
                    if (rt.Data && typeof (rt.Data) == "object") {
                        var file = new Shared.mfile();
                        ko.mapping.fromJS(rt.Data, {}, file);

                        if (option.IsSingle) {
                            if (typeof (value) == 'function') value(file);
                            else ko.mapping.fromJS(rt.Data, {}, value);
                            if (isFinish) isFinish(true);
                        }
                        else {
                            value.push(file);
                            numFileUploaded++;
                            if (numFileUploaded == totalFileUpload)
                                if (isFinish) isFinish(true);
                        }
                        if (isFinish && isFinish()) {
                            a.splice();
                            a.refresh();
                        }
                    } else {
                        if (isFinish) isFinish(true);
                    }
                    if (successCallback && typeof (successCallback) == "function")
                        successCallback(rt);
                }
            });
            uploader.bind('Error', function (a, b) {
                if (isError) isError(true);
                if (isFinish) isFinish(true);
                $(jElement).addClass('btn-danger');
                $(jElement).val("Lỗi tải file");
                console.log(b);
            });
            uploader.init();
            if (flagUpload && typeof (flagUpload) == "function") {
                flagUpload.subscribe(function (data) {
                    if (data) {
                        $.each(uploader.files, function (i, f) { f.status = 1; });
                        buildAdditionData(uploader);
                        uploader.start();
                    }
                    if (isError) isError(false);
                    if (isFinish) isFinish(false);
                });
            };
        }
    }

    ko.bindingHandlers.tags = {
        init: function (element, valueAccessor, allBindings) {
            var jElement = $(element);
            jElement.tagsinput({
                confirmKeys: [13, 188]
            });
            jElement.change(function () {
                var val = jElement.val();
                var value = valueAccessor();
                if (ko.isObservable(value)) {
                    value(val);
                }
            });
        },
        update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            var value = valueAccessor();
            var valueUnwrapped = ko.utils.unwrapObservable(value);
            var jElement = $(element);
            if (jElement.val() != valueUnwrapped) {
                jElement.tagsinput('removeAll');
                jElement.tagsinput('add', valueUnwrapped);
            }
        }
    };

    ko.bindingHandlers.suggestTags = {
        update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            var value = valueAccessor();
            var valueUnwrapped = ko.utils.unwrapObservable(value);

            var selectedTags = allBindingsAccessor().selectedTags;
            var selectedtagsUnwrapped = ko.utils.unwrapObservable(selectedTags);
            if (selectedtagsUnwrapped)
                selectedtagsUnwrapped = selectedtagsUnwrapped.split(',');

            var jElement = $(element);
            var isShowedAll = jElement.find("div a.collapsed.collapse").length > 0 ? true : false
            var checkSelectedTags = function () {
                if (selectedtagsUnwrapped && selectedtagsUnwrapped.length > 0) {
                    jElement.find('label').each(function (i, it) {
                        var text = $(it).text();
                        var f = ko.utils.arrayFirst(selectedtagsUnwrapped, function (a) {
                            return a == text;
                        });
                        if (f) $(it).addClass('disabled');
                    });
                }
            };
            // handle modal popup close - reset tags control
            $('.modal').on('hidden.bs.modal', function (event) {
                if ($(this).find("div.tags").length > 0) {
                    $(this).find("div.tags div a.collapsed.collapse")
                        .removeClass("collapsed")
                        .addClass("in");
                    isShowedAll = false;
                }
            });
            if (window.event)
                if (window.event.type != 'blur') {
                    jElement.empty();
                    if (valueUnwrapped) {
                        var tags = valueUnwrapped.split(',');
                        if (tags && tags.length > 0) {
                            var maxVisibleLength = 25;
                            var elementBuild = '<div class="input-group"><ul>';
                            ko.utils.arrayForEach(tags, function (tag, index) {
                                elementBuild += '<li' + (!isShowedAll ? (index <= maxVisibleLength - 1 ? '' : ' class="collapse suggest-tag-item no-transition"') : '') + '><label class="item">' + tag + '</label></li>';
                            });
                            elementBuild += '</ul></div>';
                            if (tags.length > maxVisibleLength)
                                elementBuild += "<div class='inline'><a data-toggle='collapse' data-target='.suggest-tag-item' class='suggest-tag-item no-transition mb5 " + (!isShowedAll ? 'collapse in' : 'collapsed collapse') + "'>Hiển thị tất cả các tag</a></div>";

                            jElement.append(elementBuild);
                            checkSelectedTags();
                            if (selectedTags) {
                                jElement.find('label').click(function () {
                                    var text = $(this).text();
                                    var isremove = $(this).hasClass('disabled');

                                    if (!isremove)
                                        if (!selectedTags()) selectedTags(text);
                                        else selectedTags(selectedTags() + ',' + text);
                                    else {
                                        var val = selectedTags();

                                        if (val.indexOf(text) == 0)
                                            val = ',' + val;

                                        if (val.lastIndexOf(text) == (val.length - 1))
                                            val = val + ',';

                                        val = val.replace(',' + text, ',');

                                        if (val.indexOf(',') == 0)
                                            val = val.substring(1, val.length - 1);

                                        if (val.lastIndexOf(',') == (val.length - 1))
                                            val = val.substring(0, val.length - 1);

                                        selectedTags(val);
                                    }
                                });
                            }
                        }
                    }
                } else {
                    jElement.find("label").not(".disabled").filter(function () {
                        return $(this).text() == $(event.target).val();
                    }).click();
                    checkSelectedTags();
                }
        }
    };

    ko.bindingHandlers.textMoney = {
        update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            var value = valueAccessor();
            var valueUnwrapped = ko.utils.unwrapObservable(value);
            $(element).text(Globalize.format(valueUnwrapped, 'n0'));
        }
    };

    ko.bindingHandlers.datepicker = {

        init: function (element, valueAccessor, allBindingsAccessor) {
            var options = allBindingsAccessor().datepickerOptions || {
            };

            options.autoclose = true;
            //options.startDate = options.startDate || Globalize.parseDate('2014-01-01', 'yyyy-MM-dd');
            //   options.format = Globalize.culture().calendar.patterns.d.replace('MM', 'mm');
            //    options.startDate = options.startDate || Globalize.parseDate('dd-MM-yyyy');

            var outputType = options.output;

            $(element).datepicker(options).on('changeDate', function (e) {

                var value = valueAccessor();

                if (ko.isObservable(value)) {
                    if (outputType == 'string') {
                        value(Globalize.format(e.date, 'd'));
                    }
                    else
                        value(e.date);
                }
            });
        },

        update: function (element, valueAccessor, allBindingsAccessor) {
            var widget = $(element).data("datepicker");

            var options = allBindingsAccessor().datepickerOptions;
            var outputType = options ? options.output : '';

            if (widget) {
                var raw = ko.utils.unwrapObservable(valueAccessor());

                if (outputType == 'string') {
                    widget.setDate(Globalize.parseDate(raw));
                }
                else {
                    widget.setDate(raw);
                }
            }
        }
    };

    ko.bindingHandlers.timepicker = {

        init: function (element, valueAccessor, allBindingsAccessor) {

            $(element).timepicker().on('changeTime.timepicker', function (e) {

                var value = valueAccessor();
                if (ko.isObservable(value)) {
                    var current = value();
                    if (!current) current = new Date();

                    if (Globalize.culture().calendar.PM[0] == e.time.meridian) {
                        if (parseInt(e.time.hours) == 12)
                            current.setHours(parseInt(e.time.hours));
                        else
                            current.setHours(12 + parseInt(e.time.hours));
                    }
                    else {
                        if (Globalize.culture().calendar.AM[0] == e.time.meridian && e.time.hours == 12)
                            current.setHours(0);
                        else
                            current.setHours(e.time.hours);
                    }
                    current.setMinutes(e.time.minutes);

                    value(current);
                }
            });
        },

        update: function (element, valueAccessor) {
            var widget = $(element).data("timepicker");
            if (widget) {
                var raw = ko.utils.unwrapObservable(valueAccessor());
                var hour = widget.hour;

                if (widget.meridian == Globalize.culture().calendar.PM[0])
                    hour += 12;

                if (widget.meridian == Globalize.culture().calendar.AM[0] && hour == 0)
                    hour = 12;

                var min = widget.minute;

                widget.setTime(raw);
            }
        }
    };

    ko.bindingHandlers.dateshort = {
        update: function (element, valueAccessor) {
            var value = ko.utils.unwrapObservable(valueAccessor());
            if (value) {
                var date;
                if (value.constructor === Date)
                    date = value;
                else
                    date = new Date(parseInt(value.substr(6)));
                $(element).text(Globalize.format(date, 'd'));
            }
            else
                $(element).text(null);
        }
    }

    ko.bindingHandlers.timeshort = {
        update: function (element, valueAccessor) {
            var value = ko.utils.unwrapObservable(valueAccessor());
            if (value) {
                var date;
                if (value.constructor === Date)
                    date = value;
                else
                    date = new Date(parseInt(value.substr(6)));
                $(element).text(Globalize.format(date, 't'));
            }
            else
                $(element).text(null);
        }
    }

    ko.bindingHandlers.datelong = {
        update: function (element, valueAccessor) {
            var value = ko.utils.unwrapObservable(valueAccessor());
            if (value) {
                var date;
                if (value.constructor === Date)
                    date = value;
                else
                    date = new Date(parseInt(value.substr(6)));

                var month = date.getMonth() + 1;
                var dateString = month + "/" + date.getDate() + "/" + date.getFullYear() + " " + date.getHours() + ":" + date.getMinutes() + ":" + date.getSeconds();
                $(element).text(dateString);//Globalize.format(date, 'f'));
            }
            else
                $(element).text(null);
        }
    }

    ko.bindingHandlers.datetimeshort = {
        update: function (element, valueAccessor) {
            var value = ko.utils.unwrapObservable(valueAccessor());
            $(element).text(CommonUtils.formatDateTimeShort(value));
        }
    }

    ko.bindingHandlers.tinymce1 = {
        init: function (element, valueAccessor, allBindingsAccessor, context) {

            var modelValue = valueAccessor();
            tinyMCE.baseURL = '/Scripts/me/tiny_mce'; //document.root + 'Scripts/Libs/tiny_mce';

            var options = allBindingsAccessor().tinymceOptions || {
                //plugins: "link table image advimage",
                plugins: "link table advimage textcolor code media preview",
                menubar: false,
                statusbar: false,
                //theme_advanced_buttons3_add : "pastetext,pasteword,selectall",
                ////toolbar: "bold italic underline | bullist numlist outdent indent | aligncenter alignright alignjustify | forecolor backcolor | link unlink table image advimage removeformat"
                toolbar: "bold italic underline | bullist numlist outdent indent | aligncenter alignright alignjustify | forecolor backcolor | link unlink table advimage media | removeformat  | preview | code"
            };

            var freeTextBindingValue = allBindingsAccessor().freeTextBinding;

            options.setup = function (ed) {
                var ed = ed;
                if (modelValue()) $(element).val(modelValue());
                ed.on('change', function () {
                    if (ko.isWriteableObservable(modelValue)) {
                        if (freeTextBindingValue) {
                            freeTextBindingValue(ed.getContent({ format: 'text' }));
                        }
                        modelValue(ed.getContent());
                    }
                });
            };

            ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
                $(element).parent().find("span.mceEditor,div.mceEditor").each(function (i, node) {
                    var ed = tinyMCE.get(node.id.replace(/_parent$/, ""));
                    if (ed) {
                        ed.remove();
                    }
                });
            });

            $(element).tinymce(options);

        },

        //update: function (element, valueAccessor, allBindingsAccessor, context) {
        //    var el = $(element);
        //    var value = ko.utils.unwrapObservable(valueAccessor());
        //    var id = el.attr('id');
        //    if (id !== undefined) {
        //        var editor = tinyMCE.get(id);
        //        if (editor !== undefined) {
        //            var link = tinyMCE.activeEditor.selection.getNode().innerHTML;
        //            var content = editor.getContent({ format: 'raw' })
        //            if (content !== value) $(el).val(value || "");
        //        }
        //        else $(el).val(value || "");
        //    }
        //    else $(el).val(value || "");
        //}
    };
    ko.bindingHandlers.tinymce = {
        init: function (element, valueAccessor, allBindingsAccessor, context) {

            var modelValue = valueAccessor();
            tinyMCE.baseURL = '/Scripts/me/tiny_mce';

            var options = allBindingsAccessor().tinymceOptions || {
                //plugins: "link table image advimage",
                plugins: "link table image advimage textcolor code media preview",
                menubar: false,
                forced_root_block: "",
                fontsize_formats: "8pt 9pt 10pt 11pt 12pt 15pt 20pt 26pt 32pt 38pt 45pt 52pt 60pt",
                valid_elements: '*[*]',
                statusbar: true,
                resize: true,
                //theme_advanced_buttons3_add : "pastetext,pasteword,selectall",
                // toolbar: "bold italic underline | bullist numlist outdent indent | aligncenter alignright alignjustify | forecolor backcolor | link unlink table image advimage removeformat"
                //toolbar: "bold italic underline | bullist numlist outdent indent | aligncenter alignright alignjustify | forecolor backcolor | link unlink table advimage removeformat"
                // toolbar: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image",
                toolbar1: "bold italic underline fontsizeselect | styleselect | bullist numlist outdent indent | aligncenter alignright alignjustify | forecolor backcolor | link unlink ",
                toolbar2: "table  image media  | removeformat   preview code"

            };

            var freeTextBindingValue = allBindingsAccessor().freeTextBinding;

            options.setup = function (ed) {
                var ed = ed;
                if (modelValue()) $(element).val(modelValue());
                ed.on('change', function () {
                    if (ko.isWriteableObservable(modelValue)) {
                        if (freeTextBindingValue) {
                            freeTextBindingValue(ed.getContent({ format: 'text' }));
                        }
                        modelValue(ed.getContent());
                    }
                });
            };

            ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
                $(element).parent().find("span.mceEditor,div.mceEditor").each(function (i, node) {
                    var ed = tinyMCE.get(node.id.replace(/_parent$/, ""));
                    if (ed) {
                        ed.remove();
                    }
                });
            });

            $(element).tinymce(options);

        },

        //update: function (element, valueAccessor, allBindingsAccessor, context) {
        //    var el = $(element);
        //    var value = ko.utils.unwrapObservable(valueAccessor());
        //    var id = el.attr('id');
        //    if (id !== undefined) {
        //        var editor = tinyMCE.get(id);
        //        if (editor !== undefined) {
        //            var link = tinyMCE.activeEditor.selection.getNode().innerHTML;
        //            var content = editor.getContent({ format: 'raw' })
        //            if (content !== value) $(el).val(value || "");
        //        }
        //        else $(el).val(value || "");
        //    }
        //    else $(el).val(value || "");
        //}
    };
    ko.bindingHandlers.codemirror = {
        init: function (element, valueAccessor) {
            var dataBinding = valueAccessor();
            if (dataBinding()) element.value = ko.utils.unwrapObservable(dataBinding);

            var onChangeFunc = function () {
                dataBinding(editor.getValue());
            };

            var editor = CodeMirror.fromTextArea(element, {
                mode: "haravan",
                lineNumbers: true,
                lineWrapping: false,
                autofocus: true,
                autoCloseTags: true,
                foldGutter: true,
                gutters: ["CodeMirror-linenumbers", "CodeMirror-foldgutter"],
                extraKeys: {
                    "Ctrl-Space": "autocomplete"
                }
            });
            editor.on("change", function (cm, change) { onChangeFunc(); });

            dataBinding.subscribe(function () {
                if ($(CommonUtils.getBindingRoot()).find($(editor.display.wrapper)).length > 0) {
                    var newValue = ko.utils.unwrapObservable(dataBinding);
                    if (newValue == null)
                        editor.clearHistory();
                    else if (editor && dataBinding() != editor.getValue())
                        editor.setValue(newValue);
                }
            });
        }
    };
    ko.bindingHandlers.numericText = {
        update: function (element, valueAccessor, allBindingsAccessor) {
            var value = ko.utils.unwrapObservable(valueAccessor()),
                precision = ko.utils.unwrapObservable(allBindingsAccessor().precision) || ko.bindingHandlers.numericText.defaultPrecision,
                formattedValue = value.toFixed(precision);

            ko.bindingHandlers.text.update(element, function () { return formattedValue; });
        },
        defaultPrecision: 1
    };
    ko.bindingHandlers.moneyMask = {
        init: function (element, valueAccessor, allBindingsAccessor) {
            var options = allBindingsAccessor().currencyMaskOptions || {};
            $(element).maskMoney({ thousands: Globalize.culture().numberFormat[','], precision: 0, allowZero: false, defaultZero: false });
            ko.utils.registerEventHandler(element, 'focusout', function () {
                var observable = valueAccessor();
                var numericVal = Globalize.parseFloat($(element).val());
                numericVal = isNaN(numericVal) ? null : numericVal;
                observable(numericVal);
                $(this).change();
            });

            ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
                $(element).maskMoney('destroy');
            });
        },

        update: function (element, valueAccessor) {
            var value = ko.utils.unwrapObservable(valueAccessor());
            $(element).val(Globalize.format(value, 'n0'));
        }
    };

    ko.bindingHandlers.numberMask = {
        init: function (element, valueAccessor, allBindingsAccessor) {
            var options = allBindingsAccessor().currencyMaskOptions || {};
            var isValueUpdate = allBindingsAccessor().valueUpdate === "afterkeydown";

            $(element).maskMoney({ thousands: Globalize.culture().numberFormat[','], precision: 0, allowZero: false, defaultZero: false });
            ko.utils.registerEventHandler(element, 'focusout', function () {
                var observable = valueAccessor();
                var numericVal = Globalize.parseFloat($(element).val());
                numericVal = isNaN(numericVal) ? null : numericVal;
                observable(numericVal);
            });

            if (isValueUpdate) {
                ko.utils.registerEventHandler(element, 'keyup', function () {
                    var observable = valueAccessor();
                    var numericVal = Globalize.parseFloat($(element).val());
                    numericVal = isNaN(numericVal) ? null : numericVal;
                    observable(numericVal);
                });
            }

            ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
                $(element).maskMoney('destroy');
            });
        },

        update: function (element, valueAccessor) {
            var value = ko.utils.unwrapObservable(valueAccessor());
            $(element).val(Globalize.format(value, 'n0'));
        }
    };

    ko.bindingHandlers.dynamichtml = {
        init: function () {
            return { controlsDescendantBindings: true };
        },
        update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            ko.utils.setHtml(element, valueAccessor());
            ko.applyBindingsToDescendants(bindingContext, element);
        }
    };

    var initMoney = ko.bindingHandlers['moneyMask'].init;

    ko.bindingHandlers['moneyMask'].init = function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {

        initMoney(element, valueAccessor, allBindingsAccessor);

        return ko.bindingHandlers['validationCore'].init(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext);
    };

    var initNumber = ko.bindingHandlers['numberMask'].init;

    ko.bindingHandlers['numberMask'].init = function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {

        initNumber(element, valueAccessor, allBindingsAccessor);

        return ko.bindingHandlers['validationCore'].init(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext);
    };

    var inittinymce = ko.bindingHandlers['tinymce'].init;

    ko.bindingHandlers['tinymce'].init = function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {

        inittinymce(element, valueAccessor, allBindingsAccessor);

        return ko.bindingHandlers['validationCore'].init(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext);
    };

    var initdatepicker = ko.bindingHandlers['datepicker'].init;

    ko.bindingHandlers['datepicker'].init = function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {

        initdatepicker(element, valueAccessor, allBindingsAccessor);

        return ko.bindingHandlers['validationCore'].init(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext);
    };

    ko.validation.rules['areSame'] = {
        getValue: function (o) {
            return (typeof o === 'function' ? o() : o);
        },
        validator: function (val, otherField) {
            if (val != undefined && otherField != undefined)
                return val === this.getValue(otherField);
            else return true;
        },
        message: 'The fields must have the same value'
    };
})(ko);

