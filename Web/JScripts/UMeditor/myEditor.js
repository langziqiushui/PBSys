//官网地址：http://ueditor.baidu.com/website/umeditor.html

var umEditor = {
    //工具栏按钮集合。
    toolbar: ['source fullscreen', 'undo redo | bold italic underline strikethrough | superscript subscript | forecolor backcolor | insertorderedlist insertunorderedlist link unlink | fontsize | removeformat | video'],

    //初始化简单编辑器。
    //ucSRichTextBoxClientIDs UCSRichTextBox控件ID集以|号分隔
    init: function (ucSRichTextBoxClientIDs) {

        //载入JS。
        if (!umEditor.isLoadJs) {
            $.getScript("/JScripts/UMeditor/1_2_2/umeditor.min.js", function () {
                $.getScript("/JScripts/UMeditor/1_2_2/lang/zh-cn/zh-cn.js", function () {
                    //循环初始化编辑器。
                    $.each(ucSRichTextBoxClientIDs.split("|"), function (i, ucSRichTextBoxClientID) {
                        //初始化编辑器。
                        umEditor.initEditor(ucSRichTextBoxClientID);
                        //强制编辑器的高度。
                        var arrWH = $2(ucSRichTextBoxClientID + "_hidWidthHeight").value.split("|");
                        $2(ucSRichTextBoxClientID + "_myEditor").style.width = arrWH[0] + "px";
                        $2(ucSRichTextBoxClientID + "_myEditor").style.height = arrWH[1] + "px";
                    });
                });
            });
        }
    },

    //初始化编辑器。
    initEditor: function (ucSRichTextBoxClientID) {
        //实例化编辑器
        var um = UM.getEditor(ucSRichTextBoxClientID + "_myEditor", {
            toolbar: umEditor.toolbar,
            pasteplain: true
        });

        //设置内容。
        um.setContent($2(ucSRichTextBoxClientID + '_hidMyEditor').value, false);

        //将内容实时更新到隐藏域中。
        um.addListener('blur', function () {            
            $2(ucSRichTextBoxClientID + '_hidMyEditor').value = um.getContent();
        });

    },

    //获取编辑器内容。
    getHtml: function (ucSRichTextBoxClientID) {
        var div = $2(ucSRichTextBoxClientID + '_myEditor');
        var hid = $2(ucSRichTextBoxClientID + '_hidMyEditor');

        //获取编辑器内容。
        var html = null != div ? div.innerHTML : ""; 
        hid.value = html;
        return html;
    }
};
