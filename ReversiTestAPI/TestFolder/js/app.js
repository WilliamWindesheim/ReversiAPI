"use strict";

function _createForOfIteratorHelper(o, allowArrayLike) { var it = typeof Symbol !== "undefined" && o[Symbol.iterator] || o["@@iterator"]; if (!it) { if (Array.isArray(o) || (it = _unsupportedIterableToArray(o)) || allowArrayLike && o && typeof o.length === "number") { if (it) o = it; var i = 0; var F = function F() {}; return { s: F, n: function n() { if (i >= o.length) return { done: true }; return { done: false, value: o[i++] }; }, e: function e(_e) { throw _e; }, f: F }; } throw new TypeError("Invalid attempt to iterate non-iterable instance.\nIn order to be iterable, non-array objects must have a [Symbol.iterator]() method."); } var normalCompletion = true, didErr = false, err; return { s: function s() { it = it.call(o); }, n: function n() { var step = it.next(); normalCompletion = step.done; return step; }, e: function e(_e2) { didErr = true; err = _e2; }, f: function f() { try { if (!normalCompletion && it["return"] != null) it["return"](); } finally { if (didErr) throw err; } } }; }

function _unsupportedIterableToArray(o, minLen) { if (!o) return; if (typeof o === "string") return _arrayLikeToArray(o, minLen); var n = Object.prototype.toString.call(o).slice(8, -1); if (n === "Object" && o.constructor) n = o.constructor.name; if (n === "Map" || n === "Set") return Array.from(o); if (n === "Arguments" || /^(?:Ui|I)nt(?:8|16|32)(?:Clamped)?Array$/.test(n)) return _arrayLikeToArray(o, minLen); }

function _arrayLikeToArray(arr, len) { if (len == null || len > arr.length) len = arr.length; for (var i = 0, arr2 = new Array(len); i < len; i++) { arr2[i] = arr[i]; } return arr2; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); Object.defineProperty(Constructor, "prototype", { writable: false }); return Constructor; }

var Game = function (url) {
  //Configuratie en state waarden
  var configMap = {
    apiUrl: url
  }; // Private function init

  var privateInit = function privateInit(afterInit) {
    console.log(configMap.apiUrl);
    afterInit();
  }; // Waarde/object geretourneerd aan de outer scope


  return {
    init: privateInit
  };
}('/api/url');

Game.Reversi = function () {
  console.log('hallo, vanuit module Reversi');
  var configMap = {}; // Private function init

  var privateInit = function privateInit() {
    console.log('hai');
  }; // Waarde/object geretourneerd aan de outer scope


  return {
    init: privateInit
  };
}();

Game.Data = function () {
  var configMap = {
    apiKey: "350c29f569f2fc0cc78c04ff132fed5e",
    mock: [{
      url: "api / Spel / Beurt",
      data: 0
    }]
  };
  var stateMap = {
    environment: 'development',
    gameState: 0
  };

  var get = function get(url) {
    if (stateMap.environment === "production") {
      return getMockData(url);
    } else if (environment === "development") {
      return $.get(url).then(function (r) {
        return r;
      })["catch"](function (e) {
        console.log(e.message);
      });
    } else {
      new Error("Incorrect input for init");
    }
  };

  var getMockData = function getMockData(url) {
    var mockData = configMap.mock;
    return new Promise(function (resolve, reject) {
      resolve(mockData);
    });
  };

  var _getCurrentGameState = function _getCurrentGameState() {
    setInterval(function () {
      stateMap.gameState = Game.Model.getGameState();
    }, 2000);

    _getCurrentGameState();
  }; // Private function init


  var privateInit = function privateInit(environment) {
    stateMap.environment = environment;

    _getCurrentGameState();
  }; // Waarde/object geretourneerd aan de outer scope


  return {
    init: privateInit,
    get: get
  };
}();

Game.Model = function () {
  var configMap = {}; // Private function init

  var privateInit = function privateInit() {
    console.log('hai');
  };

  var _getGameState = function _getGameState() {
    var token = "token"; //aanvraag via Game.Data

    var result = Game.Data.get("/api/Spel/Beurt/".concat(token)); //controle of ontvangen data valide is

    if (!(result === 0 || result === 1 || result === 2)) {
      new Error("False result");
    }

    return result;
  }; // Waarde/object geretourneerd aan de outer scope


  return {
    init: privateInit,
    getGameState: _getGameState
  };
}();

var FeedbackWidget = /*#__PURE__*/function () {
  function FeedbackWidget(elementId) {
    _classCallCheck(this, FeedbackWidget);

    this._elementId = elementId;
  }

  _createClass(FeedbackWidget, [{
    key: "elementId",
    get: function get() {
      //getter, set keyword voor setter methode
      return this._elementId;
    }
  }, {
    key: "show",
    value: function show(message, type) {
      var x = document.getElementById(this.elementId);
      x.style.display = "block"; //x.style.content = message;

      var item = "#".concat(this.elementId);
      $(item).text(message);
      if (type === "succes") $(item).addClass('alert-success');else if (type === "danger") $(item).addClass('alert-danger');
    }
  }, {
    key: "log",
    value: function log(message) {
      var log = localStorage.getItem(this.toString());
      this.removeLog();
      log = JSON.parse(log);
      if (log == null) log = [];
      log.push(message);
      if (log.length === 11) log.shift();
      localStorage.setItem(this.toString(), JSON.stringify(log));
    }
  }, {
    key: "history",
    value: function history() {
      var logs = localStorage.getItem(this.toString());
      logs = JSON.parse(logs);
      if (logs == null) logs = [];
      var logHistory = "";

      var _iterator = _createForOfIteratorHelper(logs),
          _step;

      try {
        for (_iterator.s(); !(_step = _iterator.n()).done;) {
          var log = _step.value;
          logHistory += "<type |".concat(log.type, "|> - ").concat(log.message);
        }
      } catch (err) {
        _iterator.e(err);
      } finally {
        _iterator.f();
      }

      this.show(logHistory, "succes");
    }
  }, {
    key: "removeLog",
    value: function removeLog() {
      localStorage.removeItem("".concat(this));
    }
  }, {
    key: "hide",
    value: function hide() {
      var x = document.getElementById(this.elementId);
      x.style.display = "none";
    }
  }]);

  return FeedbackWidget;
}();