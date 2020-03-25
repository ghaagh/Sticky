
StickyRetargeting = function () {
    var e, t;
    return StickyRetargeting.addToCartElement = "";
    StickyRetargeting.hostId = 0;
    StickyRetargeting.userId = 1;
    StickyRetargeting.apibaseAddress = "https://api.stickytracker.net";
    StickyRetargeting.Is_Ready = !1;
    e = StickyRetargeting.apibaseAddress, (t = document.createElement("iframe")).src = e + "/iframe.html", t.style.border = "none", t.style.display = "none", document.body.appendChild(t),


        StickyRetargeting.Add_Element = function (e, t, r) {
            if (StickyRetargeting.Is_Ready) {
                var a = {
                    ProductData: []
                },
                    i = 0,
                    n = e.querySelector("[itemprop='productId']");
                if (null != n && (i = n.innerText || n.content || n.value), 0 != i) {
                    var d = ProductInfoExtract(e);
                    d.Added = !0, a.HostId = StickyRetargeting.hostId, a.UserId = StickyRetargeting.userId, a.ProductData.push(d), a.PageAddress = window.location.href, SendDataToServer(a, "/ProductUpdate", t, r)
                }
            } else console.log("Sticy is not loaded")
        };
        StickyRetargeting.View_Products = function (e, t, r) {
        if (StickyRetargeting.Is_Ready) {
            var a = {};
            a.ProductData = [];
            a.HostId = StickyRetargeting.hostId, a.UserId = StickyRetargeting.userId, a.PageAddress = window.location.href;
            for (var i = 0; i < e.length; i++) a.ProductData.push(e[i]);
            StickyRetargeting.SendDataToServer(a, "/ProductUpdate", t, r)
        } else console.log("Sticky is not loaded")
    };
        StickyRetargeting.Add_Products = function (e, t, r) {
        if (StickyRetargeting.Is_Ready) {
            var a = {};
            a.HostId = StickyRetargeting.hostId, a.UserId = StickyRetargeting.userId, a.PageAddress = window.location.href;
            for (var i = 0; i < e.length; i++) e[i].Added = !0, a.ProductData.push(e[i]);
            StickyRetargeting.SendDataToServer(a, "/ProductUpdate", t, r)
        } else console.log("Sticky is not loaded")
    };
        StickyRetargeting.Add_Product_Ids = function (e, t, r) {
        if (StickyRetargeting.Is_Ready) {
            for (var a = {
                ProductData: []
            }, i = 0; i < e.length; i++) a.ProductData.push({
                ProductId: e[i]
            });
            a.HostId = StickyRetargeting.hostId, a.UserId = StickyRetargeting.userId, a.PageAddress = window.location.href, StickyRetargeting.SendDataToServer(a, "/AdToCart", t, r)
        } else console.log("Sticky is not loaded")
    };
        StickyRetargeting.Like_Product_Ids = function (e, t, r) {
        if (StickyRetargeting.Is_Ready) {
            for (var a = {
                ProductData: []
            }, i = 0; i < e.length; i++) a.ProductData.push({
                ProductId: e[i]
            });
            a.HostId = StickyRetargeting.hostId, a.UserId = StickyRetargeting.userId, a.PageAddress = window.location.href, StickyRetargeting.SendDataToServer(a, "/Like", t, r)
        } else console.log("Sticky is not loaded")
    };
        StickyRetargeting.UnLike_Product_Ids = function (e, t, r) {
        if (StickyRetargeting.Is_Ready) {
            for (var a = {
                ProductData: []
            }, i = 0; i < e.length; i++) a.ProductData.push({
                ProductId: e[i]
            });
            a.HostId = StickyRetargeting.hostId, a.UserId = StickyRetargeting.userId, a.PageAddress = window.location.href, StickyRetargeting.SendDataToServer(a, "/UnLike", t, r)
        } else console.log("Sticky is not loaded")
    };
        StickyRetargeting.Clear_Products = function (e, t, r) {
        if (StickyRetargeting.Is_Ready) {
            for (var a = {
                ProductData: []
            }, i = 0; i < e.length; i++) a.ProductData.push({
                ProductId: e[i]
            });
            a.HostId = StickyRetargeting.hostId, a.UserId = StickyRetargeting.userId, a.PageAddress = window.location.href, StickyRetargeting.SendDataToServer(a, "/RemoveCart", t, r)
        } else console.log("Sticky is not loaded")
    };
        StickyRetargeting.Clear_Product_Ids = function (e, t, r) {
        if (StickyRetargeting.Is_Ready) {
            for (var a = {
                ProductData: []
            }, i = 0; i < e.length; i++) a.ProductData.push({
                ProductId: e[i]
            });
            a.HostId = StickyRetargeting.hostId, a.UserId = StickyRetargeting.userId, a.PageAddress = window.location.href, StickyRetargeting.SendDataToServer(a, "/RemoveCart", t, r)
        } else console.log("Sticky is not loaded")
    };
        StickyRetargeting.Buy_Products = function (e, t, r) {
        if (StickyRetargeting.Is_Ready) {
            for (var a = {
                ProductData: []
            }, i = 0; i < e.length; i++) a.ProductData.push({
                ProductId: e[i]
            });
            a.HostId = StickyRetargeting.hostId, a.UserId = StickyRetargeting.userId, a.PageAddress = window.location.href, StickyRetargeting.SendDataToServer(a, "/BuyCart", t, r)
        } else console.log("Sticky is not loaded")
    };
        StickyRetargeting.Buy_Product_Ids = function (e, t, r) {
        if (StickyRetargeting.Is_Ready) {
            for (var a = {
                ProductData: []
            }, i = 0; i < e.length; i++) a.ProductData.push({
                ProductId: e[i]
            });
            a.HostId = StickyRetargeting.hostId, a.UserId = StickyRetargeting.userId, a.PageAddress = window.location.href, StickyRetargeting.SendDataToServer(a, "/BuyCart", t, r)
        } else console.log("Sticky is not loaded")
    };
        StickyRetargeting.SendDataToServer = function (e, t, r = null, a = null) {
        try {
            t = StickyRetargeting.apibaseAddress + t;
            var i = {
                method: "POST",
                mode: "cors",
                cache: "no-cache",
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    Accept: "application/json, text/plain, */*",
                    "Content-Type": "application/json; charset=utf-8"
                },
                body: JSON.stringify(e)
            };
            fetch(t, i).then(function (e) {
                null !== r && r(a)
            })
        } catch (e) {
            null !== r && r(a)
        }
    };
        StickyRetargeting
};
StickyRetargeting();
window[window.addEventListener ? "addEventListener" : "attachEvent"](window.addEventListener ? "message" : "onmessage", function (e) {
    if (e.origin == StickyRetargeting.apibaseAddress && (data = JSON.parse(e.data), "GetCookie" == data.message)) {
        StickyRetargeting.Is_Ready = !0;
        StickyRetargeting.hostId = data.HostId;
        StickyRetargeting.userId = data.UserId;
        setTimeout(function () {
                var e = {
                    Address: window.location.href,
                    HostId: StickyRetargeting.hostId,
                    UserId: StickyRetargeting.userId
                };
                StickyRetargeting.SendDataToServer(e, "/PageLogger");
            
        }, 2e3)
    }
}, !1);