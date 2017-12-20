/**
*1. PopUp
*2. Person
*3. PranavAlert <(inherits) newAlert
*4. newAlert
*   4.1  newAlert.show
*   4.2  newAlert.setSlide
*   4.3  newAlert.setDontAutoHide
*   4.4  newAlert.setError
*   4.5  newAlert.setBottomRight
*   4.6  newAlert.setTopRight
*   4.7  newAlert.setContainer
*   4.8  newAlert.setOpts
*   4.9  newAlert.setMessage
*   4.10 newAlert.setTitle
*
*/
 
function PopUp(Message) {
    alert(Message);
}
function Person() {
    //Properties 
    this.name = "aravind";
    this.age = "23";
    //functions
    this.sayHi = function (abc) {
        alert(abc);
        return this.name + " Says Hi";
    }
}

function PranavAlert(title, Message) {
  
    this.prototype = new newAlert();
    
    if (Message == undefined) {
        Message = title;
        title = '<br/>';
    }
    this.prototype.setTitle(title);
    this.prototype.setMessage(Message);

    //Msg.setBottomRight();
    this.prototype.show();
}

function newAlert() {
  
    try{
        var title, message, opts, container;
        if ($("#freeow-br").length == 0) {
            $("body").prepend("<div id='freeow-br' class='freeow freeow-bottom-right'></div>");
        }

        if ($("#freeow-tr").length == 0) {
            $("body").prepend("<div id='freeow-tr' class='freeow freeow-top-right'></div>");
        }

        this.title = $("#freeow-title").val();
        this.message = $("#freeow-message").val();

        this.opts = {};
        this.opts.classes = ["smokey"];//gray,osx,simple
        this.container = "#freeow-tr";



        this.setTitle = function (result) {
            this.title = result;

        };

        this.setMessage = function (result) {
            this.message = result;
        };

        this.setOpts = function (result) {
            this.opts = result;
        };

        this.setContainer = function (result) {
            this.container = result;
        };

        this.setTopRight = function () {
            this.container = "#freeow-tr";
        };


        this.setBottomRight = function () {
            this.container = "#freeow-br";
        };


        this.setPrepend = function () {
            this.opts.prepend = false;
        };
                
        this.setError = function () {
            this.opts.classes.push("error");
        };
    
        this.setDontAutoHide = function () {
            this.opts.classes.push("pushpin");
            this.opts.autoHide = false;
        };

        this.setSlide = function () {
            this.opts.classes.push("slide");
            this.opts.hideStyle = {
                opacity: 0,
                left: "400px"
            };
            this.opts.showStyle = {
                opacity: 1,
                left: 0
            };
        };



        this.show = function () {
            $(this.container).freeow(this.title, this.message, this.opts);
        };
    
    } catch (e) {
        alert(e);
    }
}
