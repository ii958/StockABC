// Global callback object. It seems that this cannot process simultaneous Async callbacks.
var __callbackObject = new Object();

function WebForm_DoCallback(eventTarget, eventArgument, eventCallback, context, errorCallback, useAsync) {

    re = new RegExp("\\x2B", "g");

    var postData = __theFormPostData +

                "__CALLBACKID=" + eventTarget +

                "&__CALLBACKPARAM=" + escape(eventArgument).replace(re, "%2B");

    // For non MS browsers, we can use the "XMLHttpRequest" object.
    if (__nonMSDOMBrowser) {

        var xmlRequest = new XMLHttpRequest();

        if (pageUrl.indexOf("?") != -1) {

            xmlRequest.open("GET", pageUrl + "&" + postData, false);

        }
        else {

            xmlRequest.open("GET", pageUrl + "?" + postData, false);

        }

        xmlRequest.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");

        xmlRequest.send(null);

        response = xmlRequest.responseText;

        if (response.charAt(0) == "s") {

            if (eventCallback != null) {

                eventCallback(response.substring(1), context);

            }

        }
        else {

            if (errorCallback != null) {

                errorCallback(response.substring(1), context);

            }

        }

    }

    // For IE, we use XMLHTTP.
    else {

        var xmlRequest = new ActiveXObject("Microsoft.XMLHTTP");

        // Hook the readystatechange event.
        xmlRequest.onreadystatechange = WebForm_CallbackComplete;

        __callbackObject.xmlRequest = xmlRequest;

        __callbackObject.eventCallback = eventCallback;

        __callbackObject.context = context;

        __callbackObject.errorCallback = errorCallback;

        var usePost = false;

        // POST or use URL query string?
        if (pageUrl.length + postData.length + 1 > 2067) {

            usePost = true;

        }

        if (usePost) {

            xmlRequest.open("POST", pageUrl, useAsync);

            xmlRequest.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");

            xmlRequest.send(postData);

        }
        else {

            if (pageUrl.indexOf("?") != -1) {

                xmlRequest.open("GET", pageUrl + "&" + postData, useAsync);

            }

            else {

                xmlRequest.open("GET", pageUrl + "?" + postData, useAsync);

            }

            xmlRequest.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");

            xmlRequest.send();

        }

    }

}

function WebForm_CallbackComplete() {

    if (__callbackObject.xmlRequest.readyState == 4) {

        response = __callbackObject.xmlRequest.responseText;

        // Success or Exception from the server side?

        if (response.charAt(0) == "s") {

            if (__callbackObject.eventCallback != null) {

                   // Invoke the client side callback function to manipulate DOM.

                __callbackObject.eventCallback(response.substring(1), __callbackObject.context);

            }

        }
        else {

            if (__callbackObject.errorCallback != null) {

                __callbackObject.errorCallback(response.substring(1), __callbackObject.context);

            }

        }

    }

}

var __nonMSDOMBrowser = (window.navigator.appName.toLowerCase().indexOf('explorer') == -1);

var __theFormPostData = "";

var theForm = document.forms[0];

function WebForm_InitCallback() {

    count = theForm.elements.length;

    var element;

    re = new RegExp("\\x2B", "g");

    // Prepare the FORM data we will send to the server.

    for (i = 0; i < count; i++) {

        element = theForm.elements[i];

         if (element.tagName.toLowerCase() == "input") {

            __theFormPostData += element.name + "=" + element.value.replace(re, "%2B") + "&";

         }

         else if (element.tagName.toLowerCase() == "select") {

            selectCount = element.children.length;

            for (j = 0; j < selectCount; j++) {

                selectChild = element.children[j];

                if ((selectChild.tagName.toLowerCase() == "option") && (selectChild.selected == true)) {

                    __theFormPostData += element.name + "=" + selectChild.value.replace(re, "%2B") + "&";

                }

            }

         }

    }

}
