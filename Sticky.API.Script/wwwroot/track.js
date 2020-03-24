
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
        StickyRetargeting.ProductInfoExtract = function (e) {
        var t = {},
            r = 0;
        if (null == e) return t;
        var a = e.querySelector("[itemprop='price']");
        null != a && (priceS = a.innerHTML || a.content || a.value, r = parseInt(priceS), t.Price = isNaN(r) ? 0 : r);
        var i = e.querySelector("[itemprop='image']");
        if (null != i) {
            var n = i.src || i.content || i.querySelector("img").src;
            t.ImageAddress = n
        }
        var d = e.querySelector("[itemprop='url']");
        null != d && (t.PageAddress = d.href || d.innerText || d.content || d.value), t.PageAddress = t.PageAddress || e.href;
        var o = e.querySelector("[itemprop='name']");
        if (null != o) {
            var c = o.innerText || o.content;
            t.Name = c
        }
        var g = e.querySelector("[itemprop='productId']");
        null != g && (t.ProductId = g.innerText || g.content || g.value);
        var s = e.querySelector("[itemprop='category']");
        null != s && (t.Category = s.innerText || s.content || s.value);
        var l = e.querySelector("[itemprop='description']");
        null != l && (t.Description = l.innerText || l.content || l.value);
        var u = e.querySelector('[itemprop="availableFrom"]');
        return t.Available = null == u, t
    };
        StickyRetargeting
};
StickyRetargeting();
window[window.addEventListener ? "addEventListener" : "attachEvent"](window.addEventListener ? "message" : "onmessage", function (e) {
    if (e.origin == StickyRetargeting.apibaseAddress && (data = JSON.parse(e.data), "GetCookie" == data.message)) {
        StickyRetargeting.Is_Ready = !0, StickyRetargeting.hostId = data.HostId, StickyRetargeting.userId = data.UserId, StickyRetargeting.addToCartElement = document.getElementById(data.AddToCart);
        var t = document.querySelectorAll("[itemtype='http://schema.org/Product']");
        null != StickyRetargeting.addToCartElement && 0 != t.length && StickyRetargeting.addToCartElement.addEventListener("click", function (e) {
            var r = {
                ProductData: []
            },
                a = 0,
                i = document.querySelector("[itemprop='productId']");
            if (null != i && (a = i.innerText || i.content || i.value), 0 != a) {
                var n = StickyRetargeting.ProductInfoExtract(t[0]);
                n.Added = !0, r.HostId = hostId, r.UserId = userId, r.ProductData.push(n), r.PageAddress = window.location.href
            }
            StickyRetargeting.SendDataToServer(r, "/ProductUpdate")
        }), setTimeout(function () {
            if (0 == t.length) {
                var e = {
                    Address: window.location.href,
                    HostId: StickyRetargeting.hostId,
                    UserId: StickyRetargeting.userId
                };
                StickyRetargeting.SendDataToServer(e, "/PageLogger");
                if (StickyRetargeting.hostId == 22) {
                    var retargetingProduct = [];
                    var price = 0;
                    var schemaData = document.querySelectorAll('script[type="application/ld+json"]');
                    schemaData.forEach(function (item, index) {
                        var html = item.innerHTML.replace(/@/g, '').replace(/\n/g, '');
                        if (html.indexOf('"type": "Product"') != -1) {
                            var parsed = JSON.parse(html);
                            if (parsed != null) {
                                if (parsed.offers != null) {
                                    price = parsed.offers.price;
                                }
                            }

                        }
                    });
                    detailsElement = document.getElementsByClassName('deal-details')[0];
                    var imageMeta = document.querySelector("meta[name='takhfifan:thumbnail']");
                                        var urlMeta = document.querySelector("meta[property='og:url']");
                    if (detailsElement != null && imageMeta != null && urlMeta!=null) {
                        var details = JSON.parse(detailsElement.getAttribute('data-details'));

                        details.price = price;
                        retargetingProduct.push({ Price: details.price,PageAddress:urlMeta.content, Category: details.category, Name: details.name, ProductId: details.id, Available: true, ImageAddress: imageMeta.content });

                        if (retargetingProduct.length != 0)
                            StickyRetargeting.View_Products(retargetingProduct, null, null);
                    }
                }
            } else {
                var r = {
                    ProductData: []
                };
                r.HostId = StickyRetargeting.hostId, r.PageAddress = window.location.href, r.UserId = StickyRetargeting.userId;
                for (var a = 0; a < t.length; a++) {
                    var i = StickyRetargeting.ProductInfoExtract(t[a]);
                    i.Added = !1, r.ProductData.push(i)
                }
                StickyRetargeting.SendDataToServer(r, "/ProductUpdate")
            }
        }, 2e3)
    }
}, !1);