(function ($, window, document, undefined)
{
    // Define the namespace
    var our = our || {};
    our.umbraco = our.umbraco || {};

    var defaultOptions = { };

    // Class
    our.umbraco.IRImagePicker = function (el, options)
    {
        var self = this;
        var $el = $(el);
        
        // Setup instance
        self.el = el;
        self.imageIdField = $el.find("input[id$=_hdnImageId]")[0];
        self.queryStringField = $el.find("input[id$=_hdnQueryString]")[0];
        self.chooseLink = $el.find("a[id$=_lnkChoose]")[0];
        self.valueContainer = $el.find("div[id$=_pnlValueContainer]")[0];
        self.opts = $.extend({}, defaultOptions, options);
        
        // Initialize
        self._init();

        // Return configures component
        return self;
    };

    // Methods
    our.umbraco.IRImagePicker.prototype = {
      
        // Private methods
        _init: function ()
        {
            var self = this;

            // Wire up the choose link
            $(self.chooseLink).on("click.irip", function (e) {
                e.preventDefault();
                
                UmbClientMgr.openModalWindow($(self.chooseLink).data("treeurl"), irip_choosemodaltext, true, 300, 400, 60, 0, ['#cancelbutton'], function (e2) {
                    var id = e2.outVal;
                    if (id == null) return;
                    self._setValue(id, true);
                });
            });

            // Check to see if we have a value saved
            self._setValue($(self.imageIdField).val());
        },
        
        _setValue: function (id, fromPick)
        {
            var self = this;
            
            // Set the hidden field
            $(self.imageIdField).val(id);
            
            // Destroy current preview
            self._detroyPreviewPanel();

            // Check we have a value
            if (id != "" && id != "0")
            {
                // Grab the media data and set the preview up
                $.ajax({
                    type: "POST",
                    url: irip_umbracopath + "/controls/Images/ImageViewerUpdater.asmx/UpdateImage",
                    data: '{ "mediaId": ' + id + ', "style": "Basic", "linkTarget": ""}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg)
                    {
                        // Parse media info
                        var name = msg.d.alt;
                        var url = msg.d.url;
                        
                        // Setup the preview panel
                        self._createPreviewPanel(url, name);
                        
                        if (fromPick && $(self.el).data("autolaunchcropper"))
                        {
                            $(self.valueContainer)
                                .find("a.irip-editcrop")
                                .triggerHandler("click.irip");
                        }
                    }
                });
            }
            
            if (id == "" || id == "0" || fromPick)
            {
                // Value is empty so clear the querystring too   
                $(self.queryStringField).val("");
            }
        },
        
        _createPreviewPanel: function (url, name)
        {
            var self = this;
            
            // Clear any previous panel
            self._detroyPreviewPanel();
            
            // Prepair variables
            var imageWidth = $(self.el).data("width");
            var imageHeight = $(self.el).data("height");
            var thumbWidth = $(self.el).data("thumbwidth");
            var thumbHeight = $(self.el).data("thumbheight");

            // Create elements
            var outerPanel = $("<div class='propertypane' />");
            var innerPanel = $("<div />");
            innerPanel.css("line-height", thumbHeight + "px");

            var img = $("<img />");
            img.attr("src", url + "?width=" + thumbWidth + "&height=" + thumbHeight +"&mode=crop");
            img.css("width", thumbWidth + "px");
            img.css("height", thumbHeight + "px");

            var filenameSpan = $("<span />");
            filenameSpan.text(name);

            var viewLink = $("<a class='irip-view' />");
            viewLink.attr("href", url + "?width=" + imageWidth + "&height=" + imageHeight +"&mode=crop");
            viewLink.attr("target", "_blank");
            viewLink.text(irip_viewtext);

            var editCropLink = $("<a class='irip-editcrop' />");
            editCropLink.attr("href", "#");
            editCropLink.text(irip_editcroptext);

            var removeLink = $("<a  class='irip-remove' />");
            removeLink.attr("href", "#");
            removeLink.text(irip_removetext);

            // Wire up events
            editCropLink.on("click.irip", function (e)
            {
                e.preventDefault();
                
                // Open cropper
                UmbClientMgr.openModalWindow(irip_umbracopath + "/plugins/irimagepicker/cropper.aspx", irip_cropmodaltext, true, 800, 550, 60, 0, ['#cancelbutton'], function (e2) {
                    var id = e2.outVal;
                    if (id == null) return;
                    self._setValue(id, true);
                });
            });
            
            removeLink.on("click.irip", function (e)
            {
                e.preventDefault();

                // Clear the value
                self._setValue("");
            });

            // Attach to the view
            innerPanel.append(img).append(filenameSpan).append(viewLink).append(editCropLink).append(removeLink);
            outerPanel.append(innerPanel);

            $(self.valueContainer).append(outerPanel);
        },
        
        _detroyPreviewPanel: function ()
        {
            var self = this;
            
            // Detach events
            $(self.valueContainer).find("a").off(".irip");

            // Clear the container
            $(self.valueContainer).empty();
        }

    };
    
    // jQuery API
    $.fn.irImagePicker = function (o)
    {
        var args = arguments;
        if (typeof o === 'string') {
            var api = this.irImagePickerApi();
            if (api[o]) {
                return api[o].apply(api, Array.prototype.slice.call(args, 1));
            } else {
                $.error('Method ' + o + ' does not exist on jQuery.irImagePicker');
            }
        }
        else if (typeof o === 'object' || !o) {
            return this.each(function () {
                var imagePicker = new our.umbraco.IRImagePicker(this, o);
                $(this).data("irimagepickerapi", imagePicker);
            });
        }
    };

    $.fn.irImagePickerApi = function ()
    {
        //ensure there's only 1
        if (this.length != 1) {
            throw "Requesting the API can only match one element";
        }

        //ensure thsi is a collapse panel
        if (this.data("irimagepickerapi") === null) {
            throw "The matching element had not been bound to an IRImagePicker";
        }

        return this.data("irimagepickerapi");
    };

})(jQuery, window, document);