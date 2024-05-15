"use strict";

function _typeof(o) { "@babel/helpers - typeof"; return _typeof = "function" == typeof Symbol && "symbol" == typeof Symbol.iterator ? function (o) { return typeof o; } : function (o) { return o && "function" == typeof Symbol && o.constructor === Symbol && o !== Symbol.prototype ? "symbol" : typeof o; }, _typeof(o); }
function _regeneratorRuntime() { "use strict"; /*! regenerator-runtime -- Copyright (c) 2014-present, Facebook, Inc. -- license (MIT): https://github.com/facebook/regenerator/blob/main/LICENSE */ _regeneratorRuntime = function _regeneratorRuntime() { return e; }; var t, e = {}, r = Object.prototype, n = r.hasOwnProperty, o = Object.defineProperty || function (t, e, r) { t[e] = r.value; }, i = "function" == typeof Symbol ? Symbol : {}, a = i.iterator || "@@iterator", c = i.asyncIterator || "@@asyncIterator", u = i.toStringTag || "@@toStringTag"; function define(t, e, r) { return Object.defineProperty(t, e, { value: r, enumerable: !0, configurable: !0, writable: !0 }), t[e]; } try { define({}, ""); } catch (t) { define = function define(t, e, r) { return t[e] = r; }; } function wrap(t, e, r, n) { var i = e && e.prototype instanceof Generator ? e : Generator, a = Object.create(i.prototype), c = new Context(n || []); return o(a, "_invoke", { value: makeInvokeMethod(t, r, c) }), a; } function tryCatch(t, e, r) { try { return { type: "normal", arg: t.call(e, r) }; } catch (t) { return { type: "throw", arg: t }; } } e.wrap = wrap; var h = "suspendedStart", l = "suspendedYield", f = "executing", s = "completed", y = {}; function Generator() {} function GeneratorFunction() {} function GeneratorFunctionPrototype() {} var p = {}; define(p, a, function () { return this; }); var d = Object.getPrototypeOf, v = d && d(d(values([]))); v && v !== r && n.call(v, a) && (p = v); var g = GeneratorFunctionPrototype.prototype = Generator.prototype = Object.create(p); function defineIteratorMethods(t) { ["next", "throw", "return"].forEach(function (e) { define(t, e, function (t) { return this._invoke(e, t); }); }); } function AsyncIterator(t, e) { function invoke(r, o, i, a) { var c = tryCatch(t[r], t, o); if ("throw" !== c.type) { var u = c.arg, h = u.value; return h && "object" == _typeof(h) && n.call(h, "__await") ? e.resolve(h.__await).then(function (t) { invoke("next", t, i, a); }, function (t) { invoke("throw", t, i, a); }) : e.resolve(h).then(function (t) { u.value = t, i(u); }, function (t) { return invoke("throw", t, i, a); }); } a(c.arg); } var r; o(this, "_invoke", { value: function value(t, n) { function callInvokeWithMethodAndArg() { return new e(function (e, r) { invoke(t, n, e, r); }); } return r = r ? r.then(callInvokeWithMethodAndArg, callInvokeWithMethodAndArg) : callInvokeWithMethodAndArg(); } }); } function makeInvokeMethod(e, r, n) { var o = h; return function (i, a) { if (o === f) throw new Error("Generator is already running"); if (o === s) { if ("throw" === i) throw a; return { value: t, done: !0 }; } for (n.method = i, n.arg = a;;) { var c = n.delegate; if (c) { var u = maybeInvokeDelegate(c, n); if (u) { if (u === y) continue; return u; } } if ("next" === n.method) n.sent = n._sent = n.arg;else if ("throw" === n.method) { if (o === h) throw o = s, n.arg; n.dispatchException(n.arg); } else "return" === n.method && n.abrupt("return", n.arg); o = f; var p = tryCatch(e, r, n); if ("normal" === p.type) { if (o = n.done ? s : l, p.arg === y) continue; return { value: p.arg, done: n.done }; } "throw" === p.type && (o = s, n.method = "throw", n.arg = p.arg); } }; } function maybeInvokeDelegate(e, r) { var n = r.method, o = e.iterator[n]; if (o === t) return r.delegate = null, "throw" === n && e.iterator["return"] && (r.method = "return", r.arg = t, maybeInvokeDelegate(e, r), "throw" === r.method) || "return" !== n && (r.method = "throw", r.arg = new TypeError("The iterator does not provide a '" + n + "' method")), y; var i = tryCatch(o, e.iterator, r.arg); if ("throw" === i.type) return r.method = "throw", r.arg = i.arg, r.delegate = null, y; var a = i.arg; return a ? a.done ? (r[e.resultName] = a.value, r.next = e.nextLoc, "return" !== r.method && (r.method = "next", r.arg = t), r.delegate = null, y) : a : (r.method = "throw", r.arg = new TypeError("iterator result is not an object"), r.delegate = null, y); } function pushTryEntry(t) { var e = { tryLoc: t[0] }; 1 in t && (e.catchLoc = t[1]), 2 in t && (e.finallyLoc = t[2], e.afterLoc = t[3]), this.tryEntries.push(e); } function resetTryEntry(t) { var e = t.completion || {}; e.type = "normal", delete e.arg, t.completion = e; } function Context(t) { this.tryEntries = [{ tryLoc: "root" }], t.forEach(pushTryEntry, this), this.reset(!0); } function values(e) { if (e || "" === e) { var r = e[a]; if (r) return r.call(e); if ("function" == typeof e.next) return e; if (!isNaN(e.length)) { var o = -1, i = function next() { for (; ++o < e.length;) if (n.call(e, o)) return next.value = e[o], next.done = !1, next; return next.value = t, next.done = !0, next; }; return i.next = i; } } throw new TypeError(_typeof(e) + " is not iterable"); } return GeneratorFunction.prototype = GeneratorFunctionPrototype, o(g, "constructor", { value: GeneratorFunctionPrototype, configurable: !0 }), o(GeneratorFunctionPrototype, "constructor", { value: GeneratorFunction, configurable: !0 }), GeneratorFunction.displayName = define(GeneratorFunctionPrototype, u, "GeneratorFunction"), e.isGeneratorFunction = function (t) { var e = "function" == typeof t && t.constructor; return !!e && (e === GeneratorFunction || "GeneratorFunction" === (e.displayName || e.name)); }, e.mark = function (t) { return Object.setPrototypeOf ? Object.setPrototypeOf(t, GeneratorFunctionPrototype) : (t.__proto__ = GeneratorFunctionPrototype, define(t, u, "GeneratorFunction")), t.prototype = Object.create(g), t; }, e.awrap = function (t) { return { __await: t }; }, defineIteratorMethods(AsyncIterator.prototype), define(AsyncIterator.prototype, c, function () { return this; }), e.AsyncIterator = AsyncIterator, e.async = function (t, r, n, o, i) { void 0 === i && (i = Promise); var a = new AsyncIterator(wrap(t, r, n, o), i); return e.isGeneratorFunction(r) ? a : a.next().then(function (t) { return t.done ? t.value : a.next(); }); }, defineIteratorMethods(g), define(g, u, "Generator"), define(g, a, function () { return this; }), define(g, "toString", function () { return "[object Generator]"; }), e.keys = function (t) { var e = Object(t), r = []; for (var n in e) r.push(n); return r.reverse(), function next() { for (; r.length;) { var t = r.pop(); if (t in e) return next.value = t, next.done = !1, next; } return next.done = !0, next; }; }, e.values = values, Context.prototype = { constructor: Context, reset: function reset(e) { if (this.prev = 0, this.next = 0, this.sent = this._sent = t, this.done = !1, this.delegate = null, this.method = "next", this.arg = t, this.tryEntries.forEach(resetTryEntry), !e) for (var r in this) "t" === r.charAt(0) && n.call(this, r) && !isNaN(+r.slice(1)) && (this[r] = t); }, stop: function stop() { this.done = !0; var t = this.tryEntries[0].completion; if ("throw" === t.type) throw t.arg; return this.rval; }, dispatchException: function dispatchException(e) { if (this.done) throw e; var r = this; function handle(n, o) { return a.type = "throw", a.arg = e, r.next = n, o && (r.method = "next", r.arg = t), !!o; } for (var o = this.tryEntries.length - 1; o >= 0; --o) { var i = this.tryEntries[o], a = i.completion; if ("root" === i.tryLoc) return handle("end"); if (i.tryLoc <= this.prev) { var c = n.call(i, "catchLoc"), u = n.call(i, "finallyLoc"); if (c && u) { if (this.prev < i.catchLoc) return handle(i.catchLoc, !0); if (this.prev < i.finallyLoc) return handle(i.finallyLoc); } else if (c) { if (this.prev < i.catchLoc) return handle(i.catchLoc, !0); } else { if (!u) throw new Error("try statement without catch or finally"); if (this.prev < i.finallyLoc) return handle(i.finallyLoc); } } } }, abrupt: function abrupt(t, e) { for (var r = this.tryEntries.length - 1; r >= 0; --r) { var o = this.tryEntries[r]; if (o.tryLoc <= this.prev && n.call(o, "finallyLoc") && this.prev < o.finallyLoc) { var i = o; break; } } i && ("break" === t || "continue" === t) && i.tryLoc <= e && e <= i.finallyLoc && (i = null); var a = i ? i.completion : {}; return a.type = t, a.arg = e, i ? (this.method = "next", this.next = i.finallyLoc, y) : this.complete(a); }, complete: function complete(t, e) { if ("throw" === t.type) throw t.arg; return "break" === t.type || "continue" === t.type ? this.next = t.arg : "return" === t.type ? (this.rval = this.arg = t.arg, this.method = "return", this.next = "end") : "normal" === t.type && e && (this.next = e), y; }, finish: function finish(t) { for (var e = this.tryEntries.length - 1; e >= 0; --e) { var r = this.tryEntries[e]; if (r.finallyLoc === t) return this.complete(r.completion, r.afterLoc), resetTryEntry(r), y; } }, "catch": function _catch(t) { for (var e = this.tryEntries.length - 1; e >= 0; --e) { var r = this.tryEntries[e]; if (r.tryLoc === t) { var n = r.completion; if ("throw" === n.type) { var o = n.arg; resetTryEntry(r); } return o; } } throw new Error("illegal catch attempt"); }, delegateYield: function delegateYield(e, r, n) { return this.delegate = { iterator: values(e), resultName: r, nextLoc: n }, "next" === this.method && (this.arg = t), y; } }, e; }
function asyncGeneratorStep(gen, resolve, reject, _next, _throw, key, arg) { try { var info = gen[key](arg); var value = info.value; } catch (error) { reject(error); return; } if (info.done) { resolve(value); } else { Promise.resolve(value).then(_next, _throw); } }
function _asyncToGenerator(fn) { return function () { var self = this, args = arguments; return new Promise(function (resolve, reject) { var gen = fn.apply(self, args); function _next(value) { asyncGeneratorStep(gen, resolve, reject, _next, _throw, "next", value); } function _throw(err) { asyncGeneratorStep(gen, resolve, reject, _next, _throw, "throw", err); } _next(undefined); }); }; }
// class FeedbackWidget {
//     constructor(elementId) {
//         this._elementId = elementId;
//         this._setupEventListeners();
//     }
//
//     get elementId() {
//         return this._elementId;
//     }
//
//     show(message, type, skipLog, onAccept) {
//         let widget = $(`#${this.elementId}`);
//         widget.css("display", "none" === widget.css("display") ? "block" : "none");
//
//         if (type === "success") {
//             widget.addClass("widget--state-success");
//         } else {
//             widget.addClass("widget--state-danger");
//         }
//
//         if (!skipLog) {
//             this.log({ message, type });
//         }
//
//         // Show the widget only if the user is the game creator
//         if (onAccept && this.isUserGameCreator()) {
//             onAccept();
//         }
//     }
//
//     hide() {
//         let widget = $(`#${this.elementId}`);
//         widget.css("display", "block" === widget.css("display") ? "none" : "block");
//     }
//
//     log(entry) {
//         let logEntries = JSON.parse(localStorage.feedback_widget ?? "[]");
//         if (logEntries.length >= 10) {
//             logEntries.pop();
//         }
//         logEntries.unshift(entry);
//         localStorage.feedback_widget = JSON.stringify(logEntries);
//     }
//
//     removeLog() {
//         localStorage.removeItem("feedback_widget");
//     }
//
//     _setupEventListeners() {
//         let acceptButton = document.getElementById("accept-button");
//         let rejectButton = document.getElementById("reject-button");
//
//         acceptButton.addEventListener("click", () => {
//             this.log({ message: "Accept button clicked", type: "info" });
//             this.hide();
//         });
//
//         rejectButton.addEventListener("click", () => {
//             this.log({ message: "Reject button clicked", type: "info" });
//             this.hide();
//         });
//     }
//
//     history() {
//         let logEntries = JSON.parse(localStorage.feedback_widget ?? "[]");
//         let formattedLog = logEntries.map(entry => `${entry.type} - ${entry.message}`).join("\n");
//         this.show(formattedLog, "success", true);
//     }
//
//     isUserGameCreator() {
//         // Implement your logic to check if the current user is the game creator (speler1Token).
//         // You can compare the user's token with the game creator's token.
//         // Return true if the user is the game creator, otherwise return false.
//         // Example: return currentUserToken === gameCreatorToken;
//         // Replace gameCreatorToken with the actual token of the game creator.
//         // You can retrieve the game creator's token from your backend or session.
//         // Adjust this method according to your application's authentication mechanism.
//         return false; // Placeholder value; replace with your actual logic.
//     }
// }
//
// // Example usage:
// const widget = new FeedbackWidget("feedback-success");
// widget.show("Welcome to the game!", "success", false, redirectToGame);
//
// function redirectToGame() {
//     // Redirect the user to the appropriate page (e.g., game lobby)
//     // Customize this based on your application's structure.
//     window.location.href = '/game-lobby'; // Example URL
// }

var apiUrl = 'https://localhost:44349/api/spel';
var Game = function (url) {
  var configMap = {
    apiUrl: url
  };
  var stateMap = {
    gameState: 0
  };

  // const _getCurrentGameState = function() {
  //     Game.Model.getGameState().then(d => stateMap.gameState = d);
  // }

  // Create a connection to the SignalR hub
  // const connection = new signalR.HubConnectionBuilder()
  //     .withUrl("/SpelHub")
  //     .build();
  //
  // // Start the connection
  // connection.start()
  //     .then(() => console.log("Connection started"))
  //     .catch(err => console.error(err.toString()));

  // Call a method on the hub
  // function sendGameState() {
  //
  //     connection.invoke("SendGameState")
  //         .catch(err => console.error(err.toString()));
  // }

  // Handle a method called by the hub
  // connection.on("ReceiveGameState", function (newGameState) {
  //     stateMap.gameState = newGameState;
  //     console.log(stateMap.gameState);
  //     Game.Reversi.updateGameState(newGameState);
  // });

  var _getCurrentGameState = function _getCurrentGameState() {
    return new Promise(function (resolve, reject) {
      Game.Model.getGameState.then(function (d) {
        stateMap.gameState = d;
        resolve('Success!');
      })["catch"](function (error) {
        reject('Error!');
      });
    });
  };
  var privateInit = function privateInit(afterInit) {
    console.log(configMap.apiUrl);
    // setInterval(_getCurrentGameState, 1000);
    _getCurrentGameState();
    Game.Reversi.init();
    // afterInit();
  };

  // Waarde/object geretourneerd aan de outer scope
  return {
    init: privateInit
    // sendGameState: sendGameState
  };
}(apiUrl);
//! Level 2-6  Module Pattern modules Modules
// Je geeft de modules module twee submodules: modules.Data en modules.Model.
// modules.Data is verantwoordelijk voor de communicatie met de server.
// modules.Model is verantwoordelijk voor de validatie van de gegevens,
// bijvoorbeeld: een speler moet beschikken over een username

//? It then returns a new Promise.
//? A Promise in JavaScript represents a value that may not be available yet but will be resolved at some point in the future

Game.Data = function () {
  var configMap = {
    apiUrl: apiUrl,
    mock: [{
      url: 'https://localhost:44349/api/spel',
      data: 0
    }]
  };
  var stateMap = {
    environment: 'development'
  };
  var get = function get(url) {
    if (stateMap.environment === "production") {
      return $.get(url).then(function (r) {
        return r;
      })["catch"](function (e) {
        console.log(e.message);
        return null;
      });
    } else if (stateMap.environment === "development") {
      return getMockData(url);
    }
  };
  function put(url) {
    if (stateMap.environment === "production") {
      return new Promise(function (resolve) {
        $.ajax({
          url: url,
          type: 'PUT',
          success: function success(r) {
            console.log(r);
            resolve(r);
          }
        });
      });
    }
    if (stateMap.environment === "development") {
      return getMockData(url);
    }
  }
  var getMockData = /*#__PURE__*/function () {
    var _ref = _asyncToGenerator( /*#__PURE__*/_regeneratorRuntime().mark(function _callee(url) {
      var mockData;
      return _regeneratorRuntime().wrap(function _callee$(_context) {
        while (1) switch (_context.prev = _context.next) {
          case 0:
            mockData = configMap.mock.find(function (mock) {
              return mock.url === url;
            });
            if (mockData) {
              _context.next = 3;
              break;
            }
            throw new Error('No mock data found for the given URL');
          case 3:
            return _context.abrupt("return", mockData.data);
          case 4:
          case "end":
            return _context.stop();
        }
      }, _callee);
    }));
    return function getMockData(_x) {
      return _ref.apply(this, arguments);
    };
  }();
  var getDataFromServer = /*#__PURE__*/function () {
    var _ref2 = _asyncToGenerator( /*#__PURE__*/_regeneratorRuntime().mark(function _callee2(url) {
      var response;
      return _regeneratorRuntime().wrap(function _callee2$(_context2) {
        while (1) switch (_context2.prev = _context2.next) {
          case 0:
            _context2.prev = 0;
            _context2.next = 3;
            return $.get(url);
          case 3:
            response = _context2.sent;
            return _context2.abrupt("return", response);
          case 7:
            _context2.prev = 7;
            _context2.t0 = _context2["catch"](0);
            console.error("Error fetching data from ".concat(url, ": ").concat(_context2.t0.message));
            throw _context2.t0;
          case 11:
          case "end":
            return _context2.stop();
        }
      }, _callee2, null, [[0, 7]]);
    }));
    return function getDataFromServer(_x2) {
      return _ref2.apply(this, arguments);
    };
  }();
  var fetchData = /*#__PURE__*/function () {
    var _ref3 = _asyncToGenerator( /*#__PURE__*/_regeneratorRuntime().mark(function _callee3() {
      var url, data;
      return _regeneratorRuntime().wrap(function _callee3$(_context3) {
        while (1) switch (_context3.prev = _context3.next) {
          case 0:
            url = 'https://localhost:44349/';
            _context3.prev = 1;
            _context3.next = 4;
            return $.get(url);
          case 4:
            data = _context3.sent;
            displayMockData(data);
            return _context3.abrupt("return", data);
          case 9:
            _context3.prev = 9;
            _context3.t0 = _context3["catch"](1);
            console.error("Error fetching data from ".concat(url, ": ").concat(_context3.t0.message));
            throw _context3.t0;
          case 13:
          case "end":
            return _context3.stop();
        }
      }, _callee3, null, [[1, 9]]);
    }));
    return function fetchData() {
      return _ref3.apply(this, arguments);
    };
  }();
  var displayMockData = function displayMockData(data) {
    $('#weatherData').html(JSON.stringify(data));
  };
  var _init = function _init(environment) {
    if (environment !== 'production' && environment !== 'development') {
      throw new Error('Invalid environment. The environment should be either "production" or "development".');
    }
    stateMap.environment = environment;
    if (environment === 'production') {
      fetchData().then(function (data) {
        console.log(data);
      })["catch"](function (error) {
        console.error(error);
      });
    }
  };
  return {
    dataInit: _init,
    dataGet: get,
    dataPut: put,
    configMap: configMap
  };
}();
// modules.Model
Game.Model = function () {
  // private variable for cofigMap
  var configMap = {
    apiUrl: 'https://localhost:44349/'
  };
  var _getGameState = /*#__PURE__*/function () {
    var _ref4 = _asyncToGenerator( /*#__PURE__*/_regeneratorRuntime().mark(function _callee4(token) {
      var url;
      return _regeneratorRuntime().wrap(function _callee4$(_context4) {
        while (1) switch (_context4.prev = _context4.next) {
          case 0:
            _context4.next = 2;
            return Game.Data.get("/api/Spel/Beurt/".concat(token));
          case 2:
            url = _context4.sent;
            Game.Data.get(url).then(function (data) {
              // Check if the received data is valid
              if (data.value > 2 || data.value < 0) throw new Error("Game.Data invalid value");
              if (data.value === 0) {
                console.log("No specific value");
                return "novalue";
              } else if (data.value === 1) {
                console.log("White's turn");
                return "white";
              } else if (data.value === 2) {
                console.log("Black's turn");
                return "black";
              }
              return null;
            });
          case 4:
          case "end":
            return _context4.stop();
        }
      }, _callee4);
    }));
    return function _getGameState(_x3) {
      return _ref4.apply(this, arguments);
    };
  }();
  var validateData = function validateData(username) {
    if (typeof username !== 'string') {
      throw new Error('Username must be a string');
    }
  };
  var init = function init(username) {
    validateData(username);
  };
  return {
    publicValidate: init,
    getGameState: _getGameState
  };
}();
Game.Reversi = function () {
  var aanDeBeurt;
  // Counters for white, black, and empty cells
  var whiteCount = 0;
  var blackCount = 0;
  var _winner;
  var apiUrl = "https://localhost:44349/";

  // Function to place a fiche on the board
  var placeFiche = function placeFiche(row, column, color) {
    console.log("Place fiche at row: ".concat(row, " column: ").concat(column, " color: ").concat(color));
    var cell = $("[data-row=\"".concat(row + 1, "\"][data-column=\"").concat(column + 1, "\"]"));
    cell.empty();
    var fiche = $("<div class='reversi-board__fiche animate__animated animate__bounceIn'></div>");
    if (color === "white") {
      fiche.addClass("reversi-board__fiche--white");
    } else if (color === "black") {
      fiche.addClass("reversi-board__fiche--black");
    }
    cell.append(fiche);
    updateGameState();
  };

  // Function to update the game state
  var updateGameState = function updateGameState() {
    console.log("Update game state");
    // Dispatch an event to notify that aanDeBeurt has been updated
    var event = new CustomEvent('aanDeBeurtUpdated', {
      detail: aanDeBeurt
    });
    document.dispatchEvent(event);
  };

  // Function to initialize the game
  var init = function init(spelToken) {
    console.log("Initiating Reversi board");
    console.log("this is the passed spelToken: ".concat(spelToken));
    var board = $(".reversi-board");
    fetch(apiUrl + "api/spel/GetSpelByToken/" + spelToken).then(function (response) {
      return response.json();
    }).then(function (gameData) {
      console.log(gameData);
      aanDeBeurt = gameData.aandeBeurt;
      createBoardCells(gameData, board);
    })["catch"](function (error) {
      return console.error("Error:", error);
    });
  };

  // Function to create board cells
  var createBoardCells = function createBoardCells(gameData, board) {
    var cell;
    for (var x = 0; x <= 7; x++) {
      for (var y = 0; y <= 7; y++) {
        cell = $("<div class=\"reversi-board__cell\" data-row=\"".concat(x + 1, "\" data-column=\"").concat(y + 1, "\"></div>"));
        cell.on("click", cellClickHandler.bind(null, x, y, gameData));
        board.append(cell);
        if (gameData.bord[x][y] !== 0) {
          var color = gameData.bord[x][y] === 1 ? "white" : "black";
          placeFiche(x, y, color);
        }
      }
    }
  };

  // Function to handle cell click events
  var cellClickHandler = function cellClickHandler(x, y, gameData) {
    console.log("The Cell RowNr.: ".concat(x + 1, ", ColumnNr.: ").concat(y + 1, " has been clicked!"));
    var color = aanDeBeurt === 1 ? "white" : "black";
    console.log("@ turn: " + aanDeBeurt);
    var spelerAanDeBeurt = getSpelerAanDeBeurt(gameData);
    var moveData = {
      Speltoken: gameData.token,
      SpelerToken: spelerAanDeBeurt,
      RijZet: x,
      KolomZet: y,
      Pass: false
    };
    console.log(moveData);
    makeMove(moveData);
  };
  var getSpelerAanDeBeurt = function getSpelerAanDeBeurt(gameData) {
    var spelerAanDeBeurt;
    if (gameData.aandeBeurt === 1) {
      spelerAanDeBeurt = gameData.speler1Token;
    } else if (gameData.aandeBeurt === 2) {
      spelerAanDeBeurt = gameData.speler2Token;
    }
    return spelerAanDeBeurt;
  };

  // Function to make a move
  var makeMove = function makeMove(moveData) {
    fetch(apiUrl + "api/Spel/Zet", {
      method: "PUT",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(moveData)
    }).then(function (response) {
      if (response.status === 204) {
        // the game is finished
        gameOver(moveData.Speltoken);
        return;
      }
      if (!response.ok) {
        return response.text().then(function (error) {
          throw new Error(error);
        });
      }
      return response.json();
    }).then(function (result) {
      if (result) {
        console.log("Move was successful");
        placeFiche(moveData.RijZet, moveData.KolomZet, aanDeBeurt === 1 ? "white" : "black");
        refreshBoard(moveData.Speltoken);
      } else {
        console.log(result);
        console.log("Move was not successful");
      }
    })["catch"](function (error) {
      console.error("Error:", error);
      // Display the error message on the screen
      if (error.status === 400) {
        console.log("unauthorized");
      } else {
        alert(error.message);
      }
    });
  };

  //!old
  // Function to handle game over
  // const gameOver = function () {
  //     winner = checkWinner();
  //     disableCellClicks();
  //     document.getElementById('gameOverMessage').textContent = "The winner is: " + winner;
  //     document.getElementById('gameOverModal').style.display = 'block';
  //
  // };

  //! new
  // Function to handle game over
  var gameOver = function gameOver(spelToken) {
    _winner = checkWinner();
    disableCellClicks();
    document.getElementById('gameOverMessage').textContent = "The winner is: " + _winner;
    document.getElementById('gameOverModal').style.display = 'block';

    // // Make a fetch request to the Result action method
    // fetch("/Spellen/Result", {
    //     method: "POST",
    //     headers: {
    //         "Content-Type": "application/json"
    //     },
    //     body: JSON.stringify({
    //         winner: winner,
    //         spelToken: spelToken
    //     })
    // })
    //     .then(response => {
    //         if (!response.ok) {
    //             throw new Error("Error posting game result");
    //         }
    //         // Handle the response from the server
    //         return response.json();
    //     })
    //     .then(data => {
    //         console.log(data);
    //     })
    //     .catch(error => console.error("Error:", error));
  };
  var disableCellClicks = function disableCellClicks() {
    var cells = document.querySelectorAll('.reversi-board__cell');
    cells.forEach(function (cell) {
      cell.onclick = null;
    });
  };

  // Function to handle game over
  // const gameOver = function (spelToken) {
  //     let winner = checkWinner();
  //
  //     // Make a fetch request to the Result action method
  //     fetch( "/Spellen/Result", {
  //         method: "POST",
  //         headers: {
  //             "Content-Type": "application/json"
  //         },
  //         body: JSON.stringify({
  //             winner: winner,
  //             spelToken: spelToken
  //         })
  //     })
  //         .then(response => {
  //             if (!response.ok) {
  //                 throw new Error("Error posting game result");
  //             }
  //             // Redirect to the results page
  //             window.location.href = 'Result';
  //         })
  //         .catch(error => console.error("Error:", error));
  // };

  // Function to check who is winning
  var checkWinner = function checkWinner() {
    if (whiteCount > blackCount) {
      return "White";
    } else if (blackCount > whiteCount) {
      return "Black";
    } else {
      return "It's a tie";
    }
  };

  //! b4 counts  Function to refresh the board
  var refreshBoard = function refreshBoard(spelToken) {
    fetch(apiUrl + "api/spel/GetSpelByToken/" + spelToken).then(function (response) {
      if (response.status === 204) {
        gameOver(spelToken);
        return;
        // the game is finished
        // clearInterval();
      }
      return response.json();
    }).then(function (newGameData) {
      console.log(newGameData);
      aanDeBeurt = newGameData.aandeBeurt;
      updateBoard(newGameData);

      // Update the counts on the page
      document.getElementById('white-counter').textContent = whiteCount;
      document.getElementById('black-counter').textContent = blackCount;
    })["catch"](function (error) {
      return console.error("Error:", error);
    });
  };

  // Function to update the board
  var updateBoard = function updateBoard(newGameData) {
    whiteCount = 0;
    blackCount = 0;
    for (var x = 0; x <= 7; x++) {
      for (var y = 0; y <= 7; y++) {
        if (newGameData.bord[x][y] !== 0) {
          // let color = newGameData.bord[x][y] === 1 ? "white" : "black";
          var color = void 0;
          //! b4
          if (newGameData.bord[x][y] === 1) {
            color = "white";
            whiteCount++;
            console.log("white ++" + whiteCount);
          } else if (newGameData.bord[x][y] === 2) {
            color = "black";
            blackCount++;
            console.log("black ++" + blackCount);
          }
          //! after

          placeFiche(x, y, color);
        }
      }
    }
  };
  return {
    init: init,
    placeFiche: placeFiche,
    updateGameState: updateGameState,
    refreshBoard: refreshBoard,
    aanDeBeurt: aanDeBeurt,
    getWhiteCount: function getWhiteCount() {
      return whiteCount;
    },
    getBlackCount: function getBlackCount() {
      return blackCount;
    },
    winner: function winner() {
      return _winner;
    }
  };
}();