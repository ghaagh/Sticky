
StickyRetargeting = function () {
    StickyRetargeting.hostId = parseInt('@HostId');
    StickyRetargeting.userId = null;
    StickyRetargeting.apibaseAddress = '@ApiBaseAddress';
    StickyRetargeting.Is_Ready = false;

    let iframe = document.createElement('iframe');
    iframe.src = StickyRetargeting.apibaseAddress + '/iframe.html';
    iframe.style.border = 'none';
    iframe.style.display = 'none';
    document.body.appendChild(iframe);

    StickyRetargeting.ProductUpdate = function (products, callback, callbackData) {
        if (StickyRetargeting.Is_Ready) {
            var data = {};
            data.ProductData = [];
            data.HostId = StickyRetargeting.hostId;
            data.UserId = StickyRetargeting.userId;
            data.PageAddress = window.location.href;
            for (var i = 0; i < products.length; i++) {
                a.ProductData.push(products[i]);
            }
            StickyRetargeting.SendDataToServer(a, '/Product', callback, callbackData)
        } else console.log('Sticky is not loaded')
    };

    StickyRetargeting.Action = function (ids, statType, callback = null, callbackData = null) {
        if (StickyRetargeting.Is_Ready) {
            var data = {};
            data.StatType = statType;
            data.HostId = StickyRetargeting.hostId;
            data.UserId = StickyRetargeting.userId;
            data.PageAddress = window.location.href;
            for (var i = 0; i < ids.length; i++) {
                data.ProductData.push(e[i]);
            }
            StickyRetargeting.SendDataToServer(data, '/ProductUpdate', callback, callbackData)
        } else console.log('Sticky is not loaded')
    };

    StickyRetargeting.SendDataToServer = function (data, uri, callback = null, callbackData = null) {
        try {
            uri = StickyRetargeting.apibaseAddress + uri;
            let request = {
                method: 'POST',
                mode: 'cors',
                cache: 'no-cache',
                headers: {
                    'Access-Control-Allow-Origin': '*',
                    Accept: 'application/json, text/plain, */*',
                    'Content-Type': 'application/json; charset=utf-8'
                },
                body: JSON.stringify(data)
            };
            fetch(uri, request).then(function (e) {
                null !== callback && callback(callbackData)
            })
        } catch (e) {
            null !== callback && callback(callbackData)
        }
    };

    StickyRetargeting
};
StickyRetargeting();


window[window.addEventListener ? 'addEventListener' : 'attachEvent'](window.addEventListener ? 'message' : 'onmessage', function (message) {
    let data = JSON.parse(message.data)
    if ('GetCookie' == data.message && message.origin == StickyRetargeting.apibaseAddress) {
        StickyRetargeting.Is_Ready = true;
        StickyRetargeting.userId = data.UserId;
        setTimeout(function () {
            var e = {
                Address: window.location.href,
                UserId: StickyRetargeting.userId
            };
            StickyRetargeting.SendDataToServer(e, '/PageView');

        }, 1000)
    }
}, false);