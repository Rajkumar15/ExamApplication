/// <reference path="tinymce.min.js" />

$(document).ready(function () {
    debugger;
    //For Edit
    if ($("#pkid").val() != "") {
        debugger;
        if ($("#Questtype_fkid").val() == 1) {
            $("#qtype_id1").prop("checked", true);
            $('#myquestiontab').show();
            $("#RAJFTB").hide();
            $('#tf').hide();
            $("#RAJFORM").hide();
            $('#ftb').hide();

            if ($("#CorrectAnswerDD").val() == 1) {
                $("#QuestionAnswer1").prop("checked", true);
            }
            if ($("#CorrectAnswerDD").val() == 2) {
                $("#QuestionAnswer2").prop("checked", true);
            }
            if ($("#CorrectAnswerDD").val() == 3) {
                $("#QuestionAnswer3").prop("checked", true);
            }
            if ($("#CorrectAnswerDD").val() == 4) {
                $("#QuestionAnswer4").prop("checked", true);
            }
        }
        if ($("#Questtype_fkid").val() == 2) {
            $("#qtype_id2").prop("checked", true);
            $('#tf').show();
            $("#RAJFTB").hide();
            $("#RAJFORM").hide();
            $('#myquestiontab').hide();
            $('#ftb').hide();
        }
        if ($("#Questtype_fkid").val() == 3) {
            $("#qtype_id3").prop("checked", true);
            $('#ftb').show();
            $("#RAJFTB").hide();
            $("#RAJFORM").hide();
            $('#myquestiontab').hide();
            $('#tf').hide();
        }
        if ($("#Questtype_fkid").val() == 4) {
            $("#qtype_id4").prop("checked", true);
            $('#ftb').hide();
            $("#RAJFORM").hide();
            $("#RAJFTB").show();
            $('#myquestiontab').hide();
            $('#tf').hide();
        }
        if ($("#Questtype_fkid").val() == 5) {
            $("#qtype_id5").prop("checked", true);
            $('#ftb').hide();
            $("#RAJFTB").hide();
            $("#RAJFORM").hide();
            $("#RAJMATCH").show();
            $('#myquestiontab').hide();
            $('#tf').hide();
        }
        if ($("#Questtype_fkid").val() == 6) {
            $("#qtype_id6").prop("checked", true);
            $('#ftb').hide();
            $("#RAJFTB").hide();
            $("#RAJFORM").hide();
            $("#myidentifytab").show();
            $('#myquestiontab').hide();
            $('#tf').hide();

            if ($("#CorrectAnswerDD").val() == 1) {
                $("#FQuestionAnswer1").prop("checked", true);
            }
            if ($("#CorrectAnswerDD").val() == 2) {
                $("#FQuestionAnswer2").prop("checked", true);
            }
            if ($("#CorrectAnswerDD").val() == 3) {
                $("#FQuestionAnswer3").prop("checked", true);
            }
            if ($("#CorrectAnswerDD").val() == 4) {
                $("#FQuestionAnswer4").prop("checked", true);
            }
        }
        if ($("#Questtype_fkid").val() == 7) {
            $("#qtype_id7").prop("checked", true);
            $('#ftb').hide();
            $("#RAJFTB").hide();
            $("#RAJMATCH").hide();
            $("#RAJFORM").show();
            $('#myquestiontab').hide();
            $('#tf').hide();
        }

        if ($("#TrueFalse").val() == true) {
            $("#QuestionTrueFalseTrue").prop("checked", true);
        }
        else {
            $("#QuestionTrueFalseFalse").prop("checked", true);
        }
    }
    //End Edit



    if ($("#pkid").val() == "") {
        $("#Questtype_fkid").val(1);
        $('#qtype_id1').prop('checked', true);
        $('#tf').hide();
        $('#ftb').hide();
        $("#myidentifytab").hide();
        $("#RAJFORM").hide();
    }

    // Start Click event
    $('#qtype_id1').click(function () {
        $('#myquestiontab').show();
        $("#RAJFTB").hide();
        $('#tf').hide();
        $("#RAJMATCH").hide();
        $("#RAJFORM").hide();
        $("#myidentifytab").hide();
        $('#ftb').hide();
        $("#Questtype_fkid").val($('#qtype_id1').val())
    });
    $('#qtype_id2').click(function () {
        $('#tf').show();
        $("#RAJFTB").hide();
        $('#myquestiontab').hide();
        $("#RAJMATCH").hide();
        $("#RAJFORM").hide();
        $("#myidentifytab").hide();
        $('#ftb').hide();
        $("#Questtype_fkid").val($('#qtype_id2').val())
    });
    $('#qtype_id3').click(function () {
        $('#ftb').show();
        $("#RAJFTB").hide();
        $('#myquestiontab').hide();
        $("#RAJMATCH").hide();
        $("#myidentifytab").hide();
        $("#RAJFORM").hide();
        $('#tf').hide();
        $("#Questtype_fkid").val($('#qtype_id3').val())
    });
    $('#qtype_id4').click(function () {
        $('#ftb').hide();
        $("#RAJFTB").show();
        $("#RAJMATCH").hide();
        $("#myidentifytab").hide();
        $('#myquestiontab').hide();
        $("#RAJFORM").hide();
        $('#tf').hide();
        $("#Questtype_fkid").val($('#qtype_id4').val())
    });
    $('#qtype_id5').click(function () {
        $('#ftb').hide();
        $("#RAJFTB").hide();
        $('#myquestiontab').hide();
        $('#tf').hide();
        $("#RAJFORM").hide();
        $("#myidentifytab").hide();
        $("#RAJMATCH").show();
        $("#Questtype_fkid").val($('#qtype_id5').val())
    });
    $('#qtype_id6').click(function () {
        $('#ftb').hide();
        $("#RAJFTB").hide();
        $('#myquestiontab').hide();
        $("#RAJMATCH").hide();
        $("#RAJFORM").hide();
        $('#tf').hide();
        $("#myidentifytab").show();
        $("#Questtype_fkid").val($('#qtype_id6').val())
    });
    $('#qtype_id7').click(function () {
        $('#ftb').hide();
        $("#RAJFTB").hide();
        $('#myquestiontab').hide();
        $("#RAJMATCH").hide();
        $("#RAJFORM").show();
        $('#tf').hide();
        $("#myidentifytab").hide();
        $("#Questtype_fkid").val($('#qtype_id7').val())
    });

    // End Click Event

    //Question Correcr Answer
    $("#QuestionAnswer1").click(function () {
        $("#CorrectAnswerDD").val($("#QuestionAnswer1").val());
    })
    $("#QuestionAnswer2").click(function () {
        $("#CorrectAnswerDD").val($("#QuestionAnswer2").val());
    })
    $("#QuestionAnswer3").click(function () {
        $("#CorrectAnswerDD").val($("#QuestionAnswer3").val());
    })
    $("#QuestionAnswer4").click(function () {
        $("#CorrectAnswerDD").val($("#QuestionAnswer4").val());
    })


    //True False
    $("#QuestionTrueFalseTrue").click(function () {
        $("#TrueFalse").val($("#QuestionTrueFalseTrue").val());
    })
    $("#QuestionTrueFalseFalse").click(function () {
        $("#TrueFalse").val($("#QuestionTrueFalseFalse").val());
    })

    //Select Level Dropdown
    $("#QuestionDiffId").change(function () {
        $("#SelectLevel").val($("#QuestionDiffId").val());
    })

    // File Matched Correcr Answer
    $("#FQuestionAnswer1").click(function () {
        $("#CorrectAnswerDD").val($("#QuestionAnswer1").val());
    })
    $("#FQuestionAnswer2").click(function () {
        $("#CorrectAnswerDD").val($("#QuestionAnswer2").val());
    })
    $("#FQuestionAnswer3").click(function () {
        $("#CorrectAnswerDD").val($("#QuestionAnswer3").val());
    })
    $("#FQuestionAnswer4").click(function () {
        $("#CorrectAnswerDD").val($("#QuestionAnswer4").val());
    })



    // After Submit Check 
    $(".form-horizontal").submit(function (e) {
        if (isNullOrEmpty($("#Questtype_fkid").val())) {
            alert("Select Question Type.")
            e.preventDefault();
        }
        else if (isNullOrEmpty(tinyMCE.get('Question').getContent())) {
            alert("Please Enter Question")
            e.preventDefault();
        }
        else if (isNullOrEmpty($("#Subject_fkid").val())) {
            alert("Select Course Type.")
            e.preventDefault();
        }
        else if (isNullOrEmpty($("#Marks").val())) {
            alert("Please Enter Marks.")
            e.preventDefault();
        }
        else if (isNullOrEmpty($("#Division_fkid").val())) {
            alert("Please Select Division.")
            e.preventDefault();
        }

        if ($("#Questtype_fkid").val() == "1") {
            if (isNullOrEmpty(tinyMCE.get('Answer1').getContent())) {
                alert("Please Enter Answer 1.")
                e.preventDefault();
            }
            else if (isNullOrEmpty(tinyMCE.get('Answer2').getContent())) {
                alert("Please Enter Answer2")
                e.preventDefault();
            }
            else if (isNullOrEmpty(tinyMCE.get('Answer3').getContent())) {
                alert("Select Enter Answer3.")
                e.preventDefault();
            }
            else if (isNullOrEmpty(tinyMCE.get('Answer4').getContent())) {
                alert("Please Enter Answer4.")
                e.preventDefault();
            }
            else if (isNullOrEmpty($("#CorrectAnswerDD").val())) {
                alert("Please Select Correct Answer.")
                e.preventDefault();
            }
        }
        else if ($("#Questtype_fkid").val() == "2") {
            if (isNullOrEmpty($("#TrueFalse").val())) {
                alert("Select True Or False.")
                e.preventDefault();
            }
        }
        else if ($("#Questtype_fkid").val() == "3") {
            if (isNullOrEmpty($("#BlanckSpace").val())) {
                alert("Enter Blanckspace.")
                e.preventDefault();
            }
        }
        else if ($("#Questtype_fkid").val() == "4") {
            if (isNullOrEmpty($("#SubAnswer").val())) {
                alert("Enter Subjective Answer.")
                e.preventDefault();
            }
        }
        else if ($("#Questtype_fkid").val() == "5") {

        }
        else if ($("#Questtype_fkid").val() == "6") {
            if ($("input:file")[0].files.length != 4) {
                alert("Please Select Four Files.")
                e.preventDefault();
            }
            if (isNullOrEmpty($("#CorrectAnswerDD").val())) {
                alert("Please Select Correct Answer File.")
                e.preventDefault();
            }
        }

      

    });

    // Start Functions
    function isNullOrEmpty(str) {
        var returnValue = false;
        if (!str
            || str == null
            || str === 'null'
            || str === ''
            || str === '{}'
            || str === 'undefined'
            || str.length === 0) {
            returnValue = true;
        }
        return returnValue;
    }

   

});