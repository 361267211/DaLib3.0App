 
var axios = window.axios;
var basicTokenKey = 'BasicToken';
var ApplicationBuilder = /** @class */ (function () {
    function ApplicationBuilder() {
        this._casBase = '';
    }
    ApplicationBuilder.prototype.ensureToken = function () {
        var token = localStorage.getItem('token');
        if (token)
            return token;
        return '';
    };
    /**确保token */
    ApplicationBuilder.prototype.ensureTokenAsync = function () {
        if (this.ensureToken())
            return MyPromise.resolve(true);
        return this.fetchBasicTokenFromServerAsync().then(function () { return true; });
    };
    /**从localstarge中获取token，如果还未配置在localstage，则让其在后续请求中带上 */
    ApplicationBuilder.prototype.ensureBasicToken = function () {
        var token = localStorage.getItem(basicTokenKey);
        if (token)
            return token;
        return token;
    };
    ApplicationBuilder.prototype.fetchBasicTokenFromServerAsync = function () {
        var instance = axios.create();
        return instance.post(this._tokenRequestConfigure.OrgTokenLink, this._tokenRequestConfigure, {
            headers: {
                "Content-Type": 'application/json',
                "Access-Control-Allow-Origin": '*',
                'Access-Control-Allow-Method': '*'
            }
        }).then(function (x) { return x.data.data; })
            .then(function (x) { return localStorage.setItem(basicTokenKey, x.token); });
    };
    /**给axios添加跨域请求头 */
    ApplicationBuilder.prototype.withCors = function () {
        if (axios) {
            axios.interceptors.request.use(function (configure) {
                configure.headers["Access-Control-Allow-Origin"] = "*";
                configure.headers["Access-Control-Allow-Method"] = "*";
                return configure;
            });
        }
        return this;
    };
    /**携带机构信息 */
    ApplicationBuilder.prototype.withBasicToken = function () {
        var _this = this;
        if (axios) {
            axios.interceptors.request.use(function (config) {
                var basicToken = _this.ensureBasicToken();
                if (basicToken && !config['Authorization'])
                    config.headers["Authorization"] = 'Bearer ' + basicToken;
                return config;
            });
        }
        return this;
    };
    /**给api自动加上请求域名部分 */
    ApplicationBuilder.prototype.withDomainAndToken = function () {
        var _this = this;
        if (axios) {
            axios.interceptors.request.use(function (config) {
                if (_this.startsWith(config.url, 'http://') || _this.startsWith(config.url, 'https://'))
                    return config;
                config.baseURL = _this._apiDomainAndPort;
                return config;
            });
        }
        return this;
    };
    ApplicationBuilder.prototype.oriinalPart = function () {
        return window.location.origin + window.location.pathname;
    };
    /**携带token */
    ApplicationBuilder.prototype.withToken = function () {
        var _this = this;
        if (axios) {
            axios.interceptors.request.use(function (config) {
                var token = _this.ensureToken();
                if (token)
                    config.headers["Authorization"] = 'Bearer ' + token;
                return config;
            });
        }
        return this;
    };
    /**401重新拿token并重试 */
    ApplicationBuilder.prototype.handle401RetypResponse = function () {
        var _this = this;
        if (axios) {
            axios.defaults.retry = 1; //重试次数
            axios.defaults.retryDelay = 1000; //重试延时
            axios.defaults.shouldRetry = function (error) { return error.response.status == 401; }; //401重试
            axios.interceptors.response.use(function (response) {
                return response;
            }, function (error) {
                {
                    var config_1 = error.config;
                    if (!config_1 || !config_1.retry)
                        return MyPromise.reject(error);
                    if (!config_1.shouldRetry || typeof config_1.shouldRetry != 'function') {
                        return MyPromise.reject(error);
                    }
                    //判断是否满足重试条件
                    if (!config_1.shouldRetry(error)) {
                        return MyPromise.reject(error);
                    }
                    // 设置重置次数，默认为0
                    config_1.__retryCount = config_1.__retryCount || 0;
                    // 判断是否超过了重试次数
                    if (config_1.__retryCount >= config_1.retry) {
                        return MyPromise.reject(error);
                    }
                    //重试次数自增
                    config_1.__retryCount += 1;
                    var backoff = new MyPromise(function (resolve) {
                        setTimeout(function () {
                            resolve(1);
                        }, config_1.retryDelay || 1);
                    });
                    localStorage.removeItem('token');
                    return MyPromise.all([backoff, _this.ensureTokenAsync()]).then(function () { return axios(config_1); });
                }
            });
        }
        return this;
    };
    /**403跳转登录 */
    ApplicationBuilder.prototype.handle403Go2LoginResponse = function () {
        var _this = this;
        if (axios) {
            axios.interceptors.response.use(undefined, function (error) {
                if (error.response.status == 403 && error.response.headers.unauth) {
                    localStorage.removeItem('token');
                    var current = window.location.href;
                    localStorage.setItem('COM+', current);
                    window.location.href = _this._casBase + '/cas/login?service=' + encodeURIComponent(_this.oriinalPart())
                }
                return MyPromise.reject(error);
            });
        }
        return this;
    };
    /**410 跳转到验证码 */
    ApplicationBuilder.prototype.handle410CaptchaRequired = function () {
        var _this = this;
        if (axios) {
            axios.interceptors.response.use(undefined, function (error) {
                if (error.response.status == 410) {
                    var current = window.location.href;
                    localStorage.setItem('captchaReturnUrl', current);
                    window.location.href = _this.oriinalPart() + '#/captcha';
                    window.location.reload();
                }
                return MyPromise.reject(error);
            });
        }
        return this;
    };
    /**遭加入黑名单得可怜人 */
    ApplicationBuilder.prototype.handle429BlacklistShow = function () {
        var _this = this;
        if (axios) {
            axios.interceptors.response.use(undefined, function (error) {
                if (error.response.status == 429) {
                    window.location.href = _this.oriinalPart() + '#/blacklistcenter';
                    window.location.reload();
                }
                return MyPromise.reject(error);
            });
        }
        return this;
    };
    /**String.prototype.startsWith的实现 */
    ApplicationBuilder.prototype.startsWith = function (orgStr, str) {
        if (orgStr == null || orgStr == '')
            return false;
        if (str == null || str == '')
            return true;
        if (orgStr.length <= str.length)
            return false;
        for (var index = 0; index < str.length; index++) {
            if (str[index] != orgStr[index])
                return false;
        }
        return true;
    };
    /**配置cas基础地址 */
    ApplicationBuilder.prototype.configureCasBase = function (casBaseUrl) {
        if (casBaseUrl == null)
            throw new Error('请正确配置cas地址');
        this._casBase = casBaseUrl;
        window.casBaseUrl = casBaseUrl;
        return this;
    };
    /**配置默认的token请求服务 */
    ApplicationBuilder.prototype.configureOrgInfo = function (orgTokenConfigure) {
        this._tokenRequestConfigure = orgTokenConfigure;
        return this;
    };
    /**配置api接口的域名和端口部分 */
    ApplicationBuilder.prototype.configureApiBase = function (apiDomainAndPort) {
        this._apiDomainAndPort = apiDomainAndPort;
        window.apiDomainAndPort = apiDomainAndPort;
        return this;
    };
    /**添加默认的axios请求中间件
     * 包括 1 cors跨域中间件
     * 2.携带token中间件
     * 3.用户未登录的时候，携带机构token中间件
     * 4. 401错误，用机构token再次请求
     * 5. 403 错误，跳转登录页
     */
    ApplicationBuilder.prototype.buildDefaultApplication = function () {
        var _this = this;
        this.ensureTokenAsync()
            .then(function () {
            if (_this._tokenRequestConfigure == null)
                throw new Error('在调用此方法前，必须先调用configureOrgInfo以配置机构信息');
            if (axios == null)
                throw new Error("请确保该调用该方法是,axios已被初始化");
            axios.defaults.timeout = 30000;
            axios.defaults.loaded = true;
            _this
                .withDomainAndToken() //给api请求加上域名部分
                .withCors() //跨域请求
                .withToken() //带上token
                .withBasicToken() //如果没有token，带上含有基本信息的token
                .handle401RetypResponse() //后端返回401的时候，表示token失效或过期，需要先带上基本token请求
                .handle403Go2LoginResponse() //当后端返回403的时候，跳转登录页面;
                .handle410CaptchaRequired() //输入验证码
                .handle429BlacklistShow(); //到黑名单
        });
    };
    return ApplicationBuilder;
}());
/**
 * 1. new MyPromise时，需要传递一个 executor 执行器，执行器立刻执行
 * 2. executor 接受两个参数，分别是 resolve 和 reject
 * 3. promise 只能从 pending 到 rejected, 或者从 pending 到 fulfilled
 * 4. promise 的状态一旦确认，就不会再改变
 * 5. promise 都有 then 方法，then 接收两个参数，分别是 promise 成功的回调 onFulfilled,
 *      和 promise 失败的回调 onRejected
 * 6. 如果调用 then 时，promise已经成功，则执行 onFulfilled，并将promise的值作为参数传递进去。
 *      如果promise已经失败，那么执行 onRejected, 并将 promise 失败的原因作为参数传递进去。
 *      如果promise的状态是pending，需要将onFulfilled和onRejected函数存放起来，等待状态确定后，再依次将对应的函数执行(发布订阅)
 * 7. then 的参数 onFulfilled 和 onRejected 可以缺省
 * 8. promise 可以then多次，promise 的then 方法返回一个 promise
 * 9. 如果 then 返回的是一个结果，那么就会把这个结果作为参数，传递给下一个then的成功的回调(onFulfilled)
 * 10. 如果 then 中抛出了异常，那么就会把这个异常作为参数，传递给下一个then的失败的回调(onRejected)
 * 11.如果 then 返回的是一个promise，那么会等这个promise执行完，promise如果成功，
 *   就走下一个then的成功，如果失败，就走下一个then的失败
 */
var PENDING = 'pending';
var FULFILLED = 'fulfilled';
var REJECTED = 'rejected';
function MyPromise(executor) {
    var self = this;
    self.status = PENDING;
    self.onFulfilled = []; //成功的回调
    self.onRejected = []; //失败的回调
    //MyPromiseA+ 2.1
    function resolve(value) {
        if (self.status === PENDING) {
            self.status = FULFILLED;
            self.value = value;
            self.onFulfilled.forEach(function (fn) { return fn(); }); //MyPromiseA+ 2.2.6.1
        }
    }
    function reject(reason) {
        if (self.status === PENDING) {
            self.status = REJECTED;
            self.reason = reason;
            self.onRejected.forEach(function (fn) { return fn(); }); //MyPromiseA+ 2.2.6.2
        }
    }
    try {
        executor(resolve, reject);
    }
    catch (e) {
        reject(e);
    }
}
MyPromise.prototype.then = function (onFulfilled, onRejected) {
    //MyPromiseA+ 2.2.1 / MyPromiseA+ 2.2.5 / MyPromiseA+ 2.2.7.3 / MyPromiseA+ 2.2.7.4
    onFulfilled = typeof onFulfilled === 'function' ? onFulfilled : function (value) { return value; };
    onRejected = typeof onRejected === 'function' ? onRejected : function (reason) { throw reason; };
    var self = this;
    //MyPromiseA+ 2.2.7
    var promise2 = new MyPromise(function (resolve, reject) {
        if (self.status === FULFILLED) {
            //MyPromiseA+ 2.2.2
            //MyPromiseA+ 2.2.4 --- setTimeout
            setTimeout(function () {
                try {
                    //MyPromiseA+ 2.2.7.1
                    var x = onFulfilled(self.value);
                    resolveMyPromise(promise2, x, resolve, reject);
                }
                catch (e) {
                    //MyPromiseA+ 2.2.7.2
                    reject(e);
                }
            });
        }
        else if (self.status === REJECTED) {
            //MyPromiseA+ 2.2.3
            setTimeout(function () {
                try {
                    var x = onRejected(self.reason);
                    resolveMyPromise(promise2, x, resolve, reject);
                }
                catch (e) {
                    reject(e);
                }
            });
        }
        else if (self.status === PENDING) {
            self.onFulfilled.push(function () {
                setTimeout(function () {
                    try {
                        var x = onFulfilled(self.value);
                        resolveMyPromise(promise2, x, resolve, reject);
                    }
                    catch (e) {
                        reject(e);
                    }
                });
            });
            self.onRejected.push(function () {
                setTimeout(function () {
                    try {
                        var x = onRejected(self.reason);
                        resolveMyPromise(promise2, x, resolve, reject);
                    }
                    catch (e) {
                        reject(e);
                    }
                });
            });
        }
    });
    return promise2;
};
function resolveMyPromise(promise2, x, resolve, reject) {
    var self = this;
    //MyPromiseA+ 2.3.1
    if (promise2 === x) {
        reject(new TypeError('Chaining cycle'));
    }
    if (x && typeof x === 'object' || typeof x === 'function') {
        var used_1; //MyPromiseA+2.3.3.3.3 只能调用一次
        try {
            var then = x.then;
            if (typeof then === 'function') {
                //MyPromiseA+2.3.3
                then.call(x, function (y) {
                    //MyPromiseA+2.3.3.1
                    if (used_1)
                        return;
                    used_1 = true;
                    resolveMyPromise(promise2, y, resolve, reject);
                }, function (r) {
                    //MyPromiseA+2.3.3.2
                    if (used_1)
                        return;
                    used_1 = true;
                    reject(r);
                });
            }
            else {
                //MyPromiseA+2.3.3.4
                if (used_1)
                    return;
                used_1 = true;
                resolve(x);
            }
        }
        catch (e) {
            //MyPromiseA+ 2.3.3.2
            if (used_1)
                return;
            used_1 = true;
            reject(e);
        }
    }
    else {
        //MyPromiseA+ 2.3.3.4
        resolve(x);
    }
}
MyPromise.reject = function (reason) {
    return new MyPromise(function (resolve, reject) {
        reject(reason);
    });
};
MyPromise.resolve = function (result) {
    return new MyPromise(function (resolve, reject) {
        resolve(result);
    });
};
MyPromise.all = function (values) {
    if (!Array.isArray(values)) {
        var type = typeof values;
        return new TypeError("TypeError: " + type + " " + values + " is not iterable");
    }
    return new MyPromise(function (resolve, reject) {
        var resultArr = [];
        var orderIndex = 0;
        var processResultByKey = function (value, index) {
            resultArr[index] = value;
            if (++orderIndex === values.length) {
                resolve(resultArr);
            }
        };
        var _loop_1 = function (i) {
            var value = values[i];
            if (value && typeof value.then === 'function') {
                value.then(function (value) {
                    processResultByKey(value, i);
                }, reject);
            }
            else {
                processResultByKey(value, i);
            }
        };
        for (var i = 0; i < values.length; i++) {
            _loop_1(i);
        }
    });
};
new ApplicationBuilder()
    .configureCasBase("http://192.168.21.43:10011")
    .configureApiBase('http://192.168.21.46:8000')
    .configureOrgInfo({
    orgId: "string",
    orgSecret: 'string',
    orgCode: "cqu",
    OrgTokenLink: 'http://192.168.21.46:5002/api/Auth/AccessToken'
})
    .buildDefaultApplication();
