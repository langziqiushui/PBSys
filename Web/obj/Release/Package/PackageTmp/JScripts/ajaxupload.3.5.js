/**
* Ajax upload
* Project page - http://valums.com/ajax-upload/
* Copyright (c) 2008 Andris Valums, http://valums.com
* Licensed under the MIT license (http://valums.com/mit-license/)
* Version 3.5 (23.06.2009)
*/

/**
* Changes from the previous version:
* 1. Added better JSON handling that allows to use 'application/javascript' as a response
* 2. Added demo for usage with jQuery UI dialog
* 3. Fixed IE "mixed content" issue when used with secure connections
* 
* For the full changelog please visit: 
* http://valums.com/ajax-upload-changelog/
*/

(function () {

    var d = document, w = window;

    /**
    * Get element by id
    */
    function get(element) {
        if (typeof element == "string")
            element = d.getElementById(element);
        return element;
    }

    /**
    * Attaches event to a dom element
    */
    function addEvent(el, type, fn) {
        if (w.addEventListener) {
            el.addEventListener(type, fn, false);
        } else if (w.attachEvent) {
            var f = function () {
                fn.call(el, w.event);
            };
            el.attachEvent('on' + type, f)
        }
    }


    /**
    * Creates and returns element from html chunk
    */
    var toElement = function () {
        var div = d.createElement('div');
        return function (html) {
            div.innerHTML = html;
            var el = div.childNodes[0];
            div.removeChild(el);
            return el;
        }
    } ();

    function hasClass(ele, cls) {
        return ele.className.match(new RegExp('(\\s|^)' + cls + '(\\s|$)'));
    }
    function addClass(ele, cls) {
        if (!hasClass(ele, cls)) ele.className += " " + cls;
    }
    function removeClass(ele, cls) {
        var reg = new RegExp('(\\s|^)' + cls + '(\\s|$)');
        ele.className = ele.className.replace(reg, ' ');
    }

    // getOffset function copied from jQuery lib (http://jquery.com/)
    if (document.documentElement["getBoundingClientRect"]) {
        // Get Offset using getBoundingClientRect
        // http://ejohn.org/blog/getboundingclientrect-is-awesome/
        var getOffset = function (el) {
            var box = el.getBoundingClientRect(),
		doc = el.ownerDocument,
		body = doc.body,
		docElem = doc.documentElement,

            // for ie 
		clientTop = docElem.clientTop || body.clientTop || 0,
		clientLeft = docElem.clientLeft || body.clientLeft || 0,

            // In Internet Explorer 7 getBoundingClientRect property is treated as physical,
            // while others are logical. Make all logical, like in IE8.


		zoom = 1;
            if (body.getBoundingClientRect) {
                var bound = body.getBoundingClientRect();
                zoom = (bound.right - bound.left) / body.clientWidth;
            }
            if (zoom > 1) {
                clientTop = 0;
                clientLeft = 0;
            }
            var top = box.top / zoom + (window.pageYOffset || docElem && docElem.scrollTop / zoom || body.scrollTop / zoom) - clientTop,
		left = box.left / zoom + (window.pageXOffset || docElem && docElem.scrollLeft / zoom || body.scrollLeft / zoom) - clientLeft;

            return {
                top: top,
                left: left
            };
        }

    } else {
        // Get offset adding all offsets 
        var getOffset = function (el) {
            if (w.jQuery) {
                return jQuery(el).offset();
            }

            var top = 0, left = 0;
            do {
                top += el.offsetTop || 0;
                left += el.offsetLeft || 0;
            }
            while (el = el.offsetParent);

            return {
                left: left,
                top: top
            };
        }
    }

    function getBox(el) {
        var left, right, top, bottom;
        var offset = getOffset(el);
        left = offset.left;
        top = offset.top;

        right = left + el.offsetWidth;
        bottom = top + el.offsetHeight;

        return {
            left: left,
            right: right,
            top: top,
            bottom: bottom
        };
    }

    /**
    * Crossbrowser mouse coordinates
    */
    function getMouseCoords(e) {
        // pageX/Y is not supported in IE
        // http://www.quirksmode.org/dom/w3c_cssom.html			
        if (!e.pageX && e.clientX) {
            // In Internet Explorer 7 some properties (mouse coordinates) are treated as physical,
            // while others are logical (offset).
            var zoom = 1;
            var body = document.body;

            if (body.getBoundingClientRect) {
                var bound = body.getBoundingClientRect();
                zoom = (bound.right - bound.left) / body.clientWidth;
            }

            return {
                x: e.clientX / zoom + d.body.scrollLeft + d.documentElement.scrollLeft,
                y: e.clientY / zoom + d.body.scrollTop + d.documentElement.scrollTop
            };
        }

        return {
            x: e.pageX,
            y: e.pageY
        };

    }
    /**
    * Function generates unique id
    */
    var getUID = function () {
        var id = 0;
        return function () {
            return 'ValumsAjaxUpload' + id++;
        }
    } ();

    function fileFromPath(file) {
        return file.replace(/.*(\/|\\)/, "");
    }

    function getExt(file) {
        return (/[.]/.exec(file)) ? /[^.]+$/.exec(file.toLowerCase()) : '';
    }

    // Please use AjaxUpload , Ajax_upload will be removed in the next version
    Ajax_upload = AjaxUpload = function (button, options) {
        if (button.jquery) {
            // jquery object was passed
            button = button[0];
        } else if (typeof button == "string" && /^#.*/.test(button)) {
            button = button.slice(1);
        }
        button = get(button);

        this._input = null;
        this._button = button;
        this._disabled = false;
        this._submitting = false;
        // Variable changes to true if the button was clicked
        // 3 seconds ago (requred to fix Safari on Mac error)
        this._justClicked = false;
        this._parentDialog = d.body;

        if (window.jQuery && jQuery.ui && jQuery.ui.dialog) {
            var parentDialog = jQuery(this._button).parents('.ui-dialog');
            if (parentDialog.length) {
                this._parentDialog = parentDialog[0];
            }
        }

        this._settings = {
            // Location of the server-side upload script
            action: 'upload.php',
            // File upload name
            name: 'userfile',
            // Additional data to send
            data: {},
            // Submit file as soon as it's selected
            autoSubmit: true,
            // The type of data that you're expecting back from the server.
            // Html and xml are detected automatically.
            // Only useful when you are using json data as a response.
            // Set to "json" in that case. 
            responseType: false,
            // When user selects a file, useful with autoSubmit disabled			
            onChange: function (file, extension) { },
            // Callback to fire before file is uploaded
            // You can return false to cancel upload
            onSubmit: function (file, extension) { },
            // Fired when file upload is completed
            // WARNING! DO NOT USE "FALSE" STRING AS A RESPONSE!
            onComplete: function (file, response) { }
        };

        // Merge the users options with our defaults
        for (var i in options) {
            this._settings[i] = options[i];
        }

        this._createInput();
        this._rerouteClicks();
    }

    // assigning methods to our class
    AjaxUpload.prototype = {
        setData: function (data) {
            this._settings.data = data;
        },
        disable: function () {
            this._disabled = true;
        },
        enable: function () {
            this._disabled = false;
        },
        // removes ajaxupload
        destroy: function () {
            if (this._input) {
                if (this._input.parentNode) {
                    this._input.parentNode.removeChild(this._input);
                }
                this._input = null;
            }
        },
        /**
        * Creates invisible file input above the button 
        */
        _createInput: function () {
            var self = this;
            var input = d.createElement("input");
            input.setAttribute('type', 'file');
            input.setAttribute('multiple', 'multiple');
            input.setAttribute('name', this._settings.name);
            var styles = {
                'position': 'absolute'
			, 'margin': '-5px 0 0 -175px'
			, 'padding': 0
			, 'width': '220px'
			, 'height': '30px'
			, 'fontSize': '14px'
			, 'opacity': 0
			, 'cursor': 'pointer'
			, 'display': 'none'
			, 'zIndex': 2147483583 //Max zIndex supported by Opera 9.0-9.2x 
                // Strange, I expected 2147483647					
            };
            for (var i in styles) {
                input.style[i] = styles[i];
            }

            // Make sure that element opacity exists
            // (IE uses filter instead)
            if (!(input.style.opacity === "0")) {
                input.style.filter = "alpha(opacity=0)";
            }

            this._parentDialog.appendChild(input);

            addEvent(input, 'change', function () {
                // get filename from input
                var file = fileFromPath(this.value);
                if (self._settings.onChange.call(self, file, getExt(file)) == false) {
                    return;
                }
                // Submit form when value is changed
                if (self._settings.autoSubmit) {
                    self.submit();
                }
            });

            // Fixing problem with Safari
            // The problem is that if you leave input before the file select dialog opens
            // it does not upload the file.
            // As dialog opens slowly (it is a sheet dialog which takes some time to open)
            // there is some time while you can leave the button.
            // So we should not change display to none immediately
            addEvent(input, 'click', function () {
                self.justClicked = true;
                setTimeout(function () {
                    // we will wait 3 seconds for dialog to open
                    self.justClicked = false;
                }, 3000);
            });

            this._input = input;
        },
        _rerouteClicks: function () {
            var self = this;

            // IE displays 'access denied' error when using this method
            // other browsers just ignore click()
            // addEvent(this._button, 'click', function(e){
            //   self._input.click();
            // });

            var box, dialogOffset = { top: 0, left: 0 }, over = false;
            addEvent(self._button, 'mouseover', function (e) {
                if (!self._input || over) return;
                over = true;
                box = getBox(self._button);

                if (self._parentDialog != d.body) {
                    dialogOffset = getOffset(self._parentDialog);
                }
            });


            // we can't use mouseout on the button,
            // because invisible input is over it
            addEvent(document, 'mousemove', function (e) {
                var input = self._input;
                if (!input || !over) return;

                if (self._disabled) {
                    removeClass(self._button, 'hover');
                    input.style.display = 'none';
                    return;
                }

                var c = getMouseCoords(e);

                if ((c.x >= box.left) && (c.x <= box.right) &&
			(c.y >= box.top) && (c.y <= box.bottom)) {
                    input.style.top = c.y - dialogOffset.top + 'px';
                    input.style.left = c.x - dialogOffset.left + 'px';
                    input.style.display = 'block';
                    addClass(self._button, 'hover');
                } else {
                    // mouse left the button
                    over = false;
                    if (!self.justClicked) {
                        input.style.display = 'none';
                    }
                    removeClass(self._button, 'hover');
                }
            });

        },
        /**
        * Creates iframe with unique name
        */
        _createIframe: function () {
            // unique name
            // We cannot use getTime, because it sometimes return
            // same value in safari :(
            var id = getUID();

            // Remove ie6 "This page contains both secure and nonsecure items" prompt 
            // http://tinyurl.com/77w9wh
            var iframe = toElement('<iframe src="javascript:false;" name="' + id + '" />');
            iframe.id = id;
            iframe.style.display = 'none';
            d.body.appendChild(iframe);
            return iframe;
        },
        /**
        * Upload file without refreshing the page
        */
        submit: function () {
            var self = this, settings = this._settings;

            if (this._input.value === '') {
                // there is no file
                return;
            }

            // get filename from input
            var file = fileFromPath(this._input.value);

            // execute user event
            if (!(settings.onSubmit.call(this, file, getExt(file)) == false)) {
                // Create new iframe for this submission
                var iframe = this._createIframe();

                // Do not submit if user function returns false										
                var form = this._createForm(iframe);
                form.appendChild(this._input);

                form.submit();

                d.body.removeChild(form);
                form = null;
                this._input = null;

                // create new input
                this._createInput();

                var toDeleteFlag = false;

                addEvent(iframe, 'load', function (e) {

                    if (// For Safari
					iframe.src == "javascript:'%3Chtml%3E%3C/html%3E';" ||
                    // For FF, IE
					iframe.src == "javascript:'<html></html>';") {

                        // First time around, do not delete.
                        if (toDeleteFlag) {
                            // Fix busy state in FF3
                            setTimeout(function () {
                                d.body.removeChild(iframe);
                            }, 0);
                        }
                        return;
                    }

                    var doc = iframe.contentDocument ? iframe.contentDocument : frames[iframe.id].document;

                    // fixing Opera 9.26
                    if (doc.readyState && doc.readyState != 'complete') {
                        // Opera fires load event multiple times
                        // Even when the DOM is not ready yet
                        // this fix should not affect other browsers
                        return;
                    }

                    // fixing Opera 9.64
                    if (doc.body && doc.body.innerHTML == "false") {
                        // In Opera 9.64 event was fired second time
                        // when body.innerHTML changed from false 
                        // to server response approx. after 1 sec
                        return;
                    }

                    var response;

                    if (doc.XMLDocument) {
                        // response is a xml document IE property
                        response = doc.XMLDocument;
                    } else if (doc.body) {
                        // response is html document or plain text
                        response = doc.body.innerHTML;
                        if (settings.responseType && settings.responseType.toLowerCase() == 'json') {
                            // If the document was sent as 'application/javascript' or
                            // 'text/javascript', then the browser wraps the text in a <pre>
                            // tag and performs html encoding on the contents.  In this case,
                            // we need to pull the original text content from the text node's
                            // nodeValue property to retrieve the unmangled content.
                            // Note that IE6 only understands text/html
                            if (doc.body.firstChild && doc.body.firstChild.nodeName.toUpperCase() == 'PRE') {
                                response = doc.body.firstChild.firstChild.nodeValue;
                            }
                            if (response) {
                                response = window["eval"]("(" + response + ")");
                            } else {
                                response = {};
                            }
                        }
                    } else {
                        // response is a xml document
                        var response = doc;
                    }

                    settings.onComplete.call(self, file, response);

                    // Reload blank page, so that reloading main page
                    // does not re-submit the post. Also, remember to
                    // delete the frame
                    toDeleteFlag = true;

                    // Fix IE mixed content issue
                    iframe.src = "javascript:'<html></html>';";
                });

            } else {
                // clear input to allow user to select same file
                // Doesn't work in IE6
                // this._input.value = '';
                d.body.removeChild(this._input);
                this._input = null;

                // create new input
                this._createInput();
            }
        },
        /**
        * Creates form, that will be submitted to iframe
        */
        _createForm: function (iframe) {
            var settings = this._settings;

            // method, enctype must be specified here
            // because changing this attr on the fly is not allowed in IE 6/7		
            var form = toElement('<form method="post" enctype="multipart/form-data"></form>');
            form.style.display = 'none';
            form.action = settings.action;
            form.target = iframe.name;
            d.body.appendChild(form);

            // Create hidden input element for each data key
            for (var prop in settings.data) {
                var el = d.createElement("input");
                el.type = 'hidden';
                el.name = prop;
                el.value = settings.data[prop];
                form.appendChild(el);
            }
            return form;
        }
    };
})();


//======= ajax上传文件 =========================================================//

//ajax上传文件。
var ajaxUpload = {

    //获取下拉框实例。
    ins: function (insID) {
        return new ajaxUpload.myUpload(insID);
    },

    //初始化。
    init: function (insID) {
        if (null == $2(insID))
            return;

        var ins = ajaxUpload.ins(insID);
        ins.init();
        ins.bindData();
    },

    //删除文件。
    deleteFile: function (insID, fileName) {
        if (null == $2(insID))
            return;

        ajaxUpload.ins(insID).deleteFile(fileName);
    },

    //当图片被点击。
    onImageClick: function (e, insID, ol, imgSrc) {
        //当文件被点击时。
        ajaxUpload.ins(insID).onFileClick(e, ol, imgSrc);
    },

    //当文件复选点击时。
    onCheckedBoxClick: function (insID) {
        //当文件被点击时。
        ajaxUpload.ins(insID).onCheckedBoxClick();
    },

    //插入图片。
    insertImage: function (folder, imgSrc) {
        if (typeof (ajaxUpload_pasteImageToEditor) != "undefined" && ajaxUpload_pasteImageToEditor)
            ajaxUpload_pasteImageToEditor(folder, imgSrc);
    },

    //重新设置图片大小。
    resizeImage: function (imgID, maxWidth, maxHeight) {
        alert(imgID);
        if (typeof (common) != "undefined") {
            common.resizeImage(imgID, maxWidth, maxHeight);
        }
        else {
            $(document).ready(function () {
                common.resizeImage(imgID, maxWidth, maxHeight);
            });
        }
    },

    //当图片载入缩略图出错时再次获取原图。
    onImageLoadFail: function (img, imageUrl) {
        img.src = imageUrl;
    },

    //封装文件上传相关逻辑。
    //insID  当前用户控件或控件的客户端编号。
    myUpload: function (insID) {
        this.insID = insID;
        this.prefixID = insID + "_";

        this.btnUpload = $('#' + this.prefixID + 'divUpload');
        if (null == this.btnUpload[0])
            return;

        this.divUploadFiles = $2(this.prefixID + "divUploadFiles");
        this.hidUploadFiles = $2(this.prefixID + "hidUploadFiles");
        this.hidMaXNumberUpload = $2(this.prefixID + "hidMaXNumberUpload");
        this.hidIsShowInsertButton = $2(this.prefixID + "hidIsShowInsertButton");
        this.hidModified = $2(this.prefixID + "hidModified");
        this.uploadPath = $2(this.prefixID + "hidUploadPath").value;
        this.hidCheckedFile = $2(this.prefixID + "hidCheckedFile");
        this.thumbSize = $2(this.prefixID + "hidThumbSize").value;
        this.isUploadToImageServer = $2(this.prefixID + "hidIsUploadToImageServer").value;
        this.targetEditor = $2(this.prefixID + "hidTargetEditor").value;
        this.scaleImageSize = $2(this.prefixID + "hidScaleImageSize").value;
        this.dymicFolderName = $2(this.prefixID + "hidDymicFolderName").value;

        this.isShowCheckBox = $2(this.prefixID + "hidIsShowCheckBox").value;
        this.hidCheckedFiles = $2(this.prefixID + "hidCheckedFiles");

        var qsThumb = this.thumbSize.length > 0 ? "&thumbSize=" + this.thumbSize : "";
        var qsSIS = this.scaleImageSize.length > 0 ? "&sis=" + this.scaleImageSize : "";
        var qsDymicFolderName = this.dymicFolderName.length > 0 ? "/" + this.dymicFolderName : "";

        //初始化。
        this.init = function () {
            var _this = this;
            new AjaxUpload(this.btnUpload, {
                action: '/API/MyHandler.ashx?method=uploadfile&path=' + common.EncodeURI(this.uploadPath + qsDymicFolderName) + qsThumb + qsSIS,
                name: 'uploadfile',
                onSubmit: function (file, ext) {
                    //判断上传数量。
                    var mn = parseInt(_this.hidMaXNumberUpload.value);
                    if (_this.getUploadedNumber() >= mn) {
                        common.alert("对不起，最多允许上传“" + mn + "”个文件！");
                        return false;
                    }

                    //判断能否上传。
                    if (typeof (ajaxUpload.validateUploadFile) != "undefined" && ajaxUpload.validateUploadFile) {
                        if (!ajaxUpload.validateUploadFile())
                            return false;
                    }

                    _this.btnUpload.html("Waiting..");
                },
                onComplete: function (file, response) {
                    //上传文件返回。
                    _this.btnUpload.html("上传文件");
                    response = response.replace(/<PRE.*?>/i, "").replace(/<\/PRE>/i, "");
                    var arr = response.split(":");
                    if (arr[0] == "success") {
                        //如果上传成功。
                        _this.hidUploadFiles.value += arr[1] + "###@@$@@###";
                        _this.bindData();
                        _this.hidModified.value = "true";
                    } else {
                        //如果上传失败。
                        common.alert(response);
                    }
                }
            });
        };

        //绑定数据。
        this.bindData = function () {
            var t = "";
            var _this = this;
            var c = 1;
            var checkedFile = this.hidCheckedFile.value;
            var hasCheckedFile = false;
            $.each(this.hidUploadFiles.value.split("###@@$@@###"), function (i, item) {
                if (item.length > 0) {
                    //生成动态目录字符串。
                    var qsDymicFolderName = _this.dymicFolderName.length > 0 ? "/" + _this.dymicFolderName : "";

                    //生成缩略图引用路径。
                    var imgSrc = "";
                    var lastIndex = item.lastIndexOf(".");
                    var ex = item.substring(lastIndex, item.length).toLowerCase();
                    if (ex == ".gif" || ex == ".jpg" || ex == ".png") {
                        imgSrc = _this.uploadPath + qsDymicFolderName + "/thumbs/" + item;
                    }
                    else if (ex == ".doc" || ex == ".docx") {
                        imgSrc = "/App_Themes/images/uploadIcons/doc.png";
                    }
                    else if (ex == ".rar") {
                        imgSrc = "/App_Themes/images/uploadIcons/rar.png";
                    }
                    else if (ex == ".xls" || ex == ".xlsx") {
                        imgSrc = "/App_Themes/images/uploadIcons/xls.png";
                    }
                    else {
                        imgSrc = "/App_Themes/images/uploadIcons/Other.png";
                    }

                    //缩略图大小控制。
                    var arrSize = _this.thumbSize.split(",");
                    var sizeWidth = parseInt(arrSize[0]);
                    var sizeHeight = parseInt(arrSize[1]);
                    _this.thumbSize = sizeWidth + "," + sizeHeight;

                    //处于选择状态的文件边框样式。
                    var spanStyleText = " style='width:" + sizeWidth + "px; height:" + sizeHeight + "px;";
                    if (!hasCheckedFile && checkedFile.length > 0) {
                        if (item.indexOf(checkedFile) >= 0) {
                            spanStyleText += "border-color:red;";
                            hasCheckedFile = true;
                        }
                    }
                    spanStyleText += "' ";

                    //渲染复选框设置。
                    var chkID = "";
                    var chkHtml = "";
                    if (_this.isShowCheckBox == "true") {
                        //判断是否处于选择状态。
                        var isChecked = false;
                        $.each(_this.hidCheckedFiles.value.split("###@@$@@###"), function (j2, item2) {
                            if (item2 == item) {
                                isChecked = true;
                                return true;
                            }
                        });
                        chkID = "chk_" + _this.divUploadFiles + "_Slide" + i;
                        var checkedText = isChecked ? " checked=\"checked\" " : "";
                        chkHtml = "<label title='勾选为滚动主图'><input class=\"checkbox-slider slider-icon colored-darkorange\" onchange=\"ajaxUpload.onCheckedBoxClick('" + _this.insID + "');\" type=\"checkbox\" id=\"" + chkID + "\" " + checkedText + " value=\"" + item + "\" /><i class=\"text\"></i></label>";
                    }

                    var imgHTML = "<img id='myUpload_item_" + _this.insID + "_" + c + "' src='" + imgSrc + "' /></label>";
                    t += "<li style='width:" + (sizeWidth + 4) + "px;'>";
                    t += "<span title='点击设为预览主图' " + spanStyleText + " onclick=\"ajaxUpload.onImageClick(event, '" + _this.insID + "', this, '" + _this.uploadPath + "/" + item + "');\">" + imgHTML + chkHtml + "</span>";
                    t += "<b><a title='预览上传的文件' href='" + _this.uploadPath + qsDymicFolderName + "/" + item + "' target='_blank'>预览</a>&nbsp;";
                    if (_this.hidIsShowInsertButton.value == "true")
                        t += "<a href=\"javascript:void(0);\" title=\"在文本编辑器中插入当前图片\" onclick=\"ajaxUpload.insertImage('" + _this.uploadPath + qsDymicFolderName + "','" + item + "');\">插入</a>&nbsp;";
                    t += "<a href=\"javascript:void(0);\" title=\"删除当前上传的文件\" onclick=\"ajaxUpload.deleteFile('" + _this.insID + "', '" + common.EncodeURI(item) + "');\">删除</a>";
                    t += "</b></li>";
                }

                c += 1;
            });

            this.divUploadFiles.innerHTML = t;
            this.divUploadFiles.style.display = t.length > 0 ? "block" : "none";

            //设置主预览图。
            if (!hasCheckedFile)
                this.hidCheckedFile.value = "";

            //设置勾选图。
            var checkedFiles = "";
            var arrCheckedFiles = this.hidCheckedFiles.value.split("###@@$@@###");
            var arrAll = this.hidUploadFiles.value.split("###@@$@@###");
            $.each(arrCheckedFiles, function (i1, item1) {
                if ($.trim(item1).length > 0 && $.inArray(item1, arrAll) >= 0)
                    checkedFiles += item1 + "###@@$@@###";
            });
            this.hidCheckedFiles.value = checkedFiles;
        };

        //当文件被点击时。
        this.onFileClick = function (e, ol, imgSrc) {
            var ev = window.event || e;
            var srcElement = ev.srcElement ? ev.srcElement : ev.target;
            if (srcElement.tagName.toLowerCase() != "img")
                return;

            $.each($("#" + this.insID + "_divUploadFiles span"), function (i, item) {
                item.style.borderColor = "#E8E4E4";
            });

            ol.style.borderColor = "red";
            var lastIndex = imgSrc.lastIndexOf("/") + 1;
            this.hidCheckedFile.value = imgSrc.substring(lastIndex);
        };

        //当文件复选点击时。
        this.onCheckedBoxClick = function () {
            var cf = "";
            $.each($("#" + this.insID + "_divUploadFiles span input"), function (i, chk) {
                if (chk.checked)
                    cf += chk.value + "###@@$@@###";
            });
            this.hidCheckedFiles.value = cf;
        },

        //获取已经上传的文件数量。
        this.getUploadedNumber = function () {
            var n = 0;
            $.each(this.hidUploadFiles.value.split("###@@$@@###"), function (i, item) {
                if (item.length > 0)
                    n += 1;
            });

            return n;
        };

        //删除指定文件。
        this.deleteFile = function (fileName) {
            if (!confirm("您将要删除文件“" + common.DecodeURI(fileName) + "”，是否确定？"))
                return;

            var _this = this;

            //生成动态目录字符串。
            var qsDymicFolderName = _this.dymicFolderName.length > 0 ? "/" + _this.dymicFolderName : "";

            $.ajax({
                url: "/API/MyHandler.ashx?method=delUploadFile&path=" + common.EncodeURI(_this.uploadPath + qsDymicFolderName) + "&fileName=" + fileName,
                cache: false,
                async: false,
                success: function (response) {
                    if (response == "success") {
                        //删除成功。
                        var v = "";
                        $.each(_this.hidUploadFiles.value.split("###@@$@@###"), function (i, item) {
                            if (item.length > 0 && common.DecodeURI(item) != common.DecodeURI(fileName))
                                v += item + "###@@$@@###";
                        });

                        _this.hidUploadFiles.value = v;
                        _this.hidModified.value = "true";

                        //重新绑定。
                        _this.bindData();
                    }
                    else {
                        common.alert(response);
                    }
                }
            });
        };
    }
};