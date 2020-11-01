"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var http_1 = require("@angular/common/http");
var operators_1 = require("rxjs/operators");
var JwtInterceptor = /** @class */ (function () {
    function JwtInterceptor(auth, router) {
        this.auth = auth;
        this.router = router;
    }
    JwtInterceptor.prototype.intercept = function (request, next) {
        var _this = this;
        return next.handle(request).pipe(operators_1.tap(function (err) {
            if (err instanceof http_1.HttpErrorResponse) {
                console.log(err);
                if (err.status === 401) {
                    _this.router.navigate(['/login']);
                }
            }
        }));
    };
    return JwtInterceptor;
}());
exports.JwtInterceptor = JwtInterceptor;
//# sourceMappingURL=JwtInterceptor.js.map