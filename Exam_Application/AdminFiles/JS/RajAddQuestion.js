/// <reference path="tinymce.min.js" />

$(document).ready(function () {
    debugger;
    if ($("#pkid").val() != "") {
        debugger;
        if ($("#Questtype_fkid").val() == 1) {
            $("#qtype_id1").prop("checked", true);
            $('#myquestiontab').show();
            $("#RAJFTB").hide();
            $('#tf').hide();
            $('#ftb').hide();
        }
        if ($("#Questtype_fkid").val() == 2) {
            $("#qtype_id2").prop("checked", true);
            $('#tf').show();
            $("#RAJFTB").hide();
            $('#myquestiontab').hide();
            $('#ftb').hide();
        }
        if ($("#Questtype_fkid").val() == 3) {
            $("#qtype_id3").prop("checked", true);
            $('#ftb').show();
            $("#RAJFTB").hide();
            $('#myquestiontab').hide();
            $('#tf').hide();
        }
        if ($("#Questtype_fkid").val() == 4) {
            $("#qtype_id4").prop("checked", true);
            $('#ftb').hide();
            $("#RAJFTB").show();
            $('#myquestiontab').hide();
            $('#tf').hide();
        }

        if ($("#TrueFalse").val() == true) {
            $("#QuestionTrueFalseTrue").prop("checked", true);
        }
        else {
            $("#QuestionTrueFalseFalse").prop("checked", true);
        }
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

    if ($("#pkid").val() == "") {
        $("#Questtype_fkid").val(1);
        $('#qtype_id1').prop('checked', true);
        $('#tf').hide();
        $('#ftb').hide();
    }

    $('#qtype_id1').click(function () {
        $('#myquestiontab').show();
        $("#RAJFTB").hide();
        $('#tf').hide();
        $('#ftb').hide();
        $("#Questtype_fkid").val($('#qtype_id1').val())
    });
    $('#qtype_id2').click(function () {
        $('#tf').show();
        $("#RAJFTB").hide();
        $('#myquestiontab').hide();
        $('#ftb').hide();
        $("#Questtype_fkid").val($('#qtype_id2').val())
    });
    $('#qtype_id3').click(function () {
        $('#ftb').show();
        $("#RAJFTB").hide();
        $('#myquestiontab').hide();
        $('#tf').hide();
        $("#Questtype_fkid").val($('#qtype_id3').val())
    });
    $('#qtype_id4').click(function () {
        $('#ftb').hide();
        $("#RAJFTB").show();
        $('#myquestiontab').hide();
        $('#tf').hide();
        $("#Questtype_fkid").val($('#qtype_id4').val())
    });


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

    $("#QuestionTrueFalseTrue").click(function () {
        $("#TrueFalse").val($("#QuestionTrueFalseTrue").val());
    })
    $("#QuestionTrueFalseFalse").click(function () {
        $("#TrueFalse").val($("#QuestionTrueFalseFalse").val());
    })


    $("#QuestionDiffId").change(function () {
        $("#SelectLevel").val($("#QuestionDiffId").val());
    })

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
            alert("Select Subject Type.")
            e.preventDefault();
        }
        else if (isNullOrEmpty($("#Marks").val())) {
            alert("Please Enter Marks.")
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

    });

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