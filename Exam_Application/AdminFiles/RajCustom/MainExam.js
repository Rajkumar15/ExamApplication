/// <reference path="../JS/jquery-1.8.2.min.js" />


//$(document).ready(function () {
//    debugger;
//    var ques_id = $("#Ques_fkid").val();
//    var Qno = $("#Qnono").val();
//    $.ajax({
//        type: "GET",
//        url: '/StudentExam/ObjectiveQuestion',
//        data: { QuestionId: ques_id, QuestionNo: Qno },
//        success: function (data, textStatus, jqXHR) {
//            $('#Partiall').html(data);
//        }
//    });
//})
function ChangeQuestion(cname) {
    debugger;
    var ques_id = cname.id;
    var Qno = $("#" + ques_id + "").text();
    $.ajax({
        type: "GET",
        url: '/StudentExam/ObjectiveQuestion',
        data: { QuestionId: ques_id, QuestionNo: Qno },
        success: function (data, textStatus, jqXHR) {
            $('#Partiall').html(data);
        }
    });

}
function TruefalseChangeQuestion(cname) {
    var ques_id = cname.id;
    var Qno = $("#" + ques_id + "").text();

    $.ajax({
        type: "GET",
        url: '/StudentExam/TrueFalseQuestion',
        data: { QuestionId: ques_id, QuestionNo: Qno },
        success: function (data, textStatus, jqXHR) {
            $('#Partiall').html(data);
        }
    });
}
function BlanckSpaceChangeQuestion(cname) {
    var ques_id = cname.id;
    var Qno = $("#" + ques_id + "").text();

    $.ajax({
        type: "GET",
        url: '/StudentExam/FillInBlanckQuestion',
        data: { QuestionId: ques_id, QuestionNo: Qno },
        success: function (data, textStatus, jqXHR) {
            $('#Partiall').html(data);
        }
    });
}
function MatchContentChangeQuestion(cname) {
    var ques_id = cname.id;
    var Qno = $("#" + ques_id + "").text();

    $.ajax({
        type: "GET",
        url: '/StudentExam/MatchContentQuestion',
        data: { QuestionId: ques_id, QuestionNo: Qno },
        success: function (data, textStatus, jqXHR) {
            $('#Partiall').html(data);
        }
    });
}
function IdentifysignChangeQuestion(cname) {
    var ques_id = cname.id;
    var Qno = $("#" + ques_id + "").text();

    $.ajax({
        type: "GET",
        url: '/StudentExam/IdentifySignQuestion',
        data: { QuestionId: ques_id, QuestionNo: Qno },
        success: function (data, textStatus, jqXHR) {
            $('#Partiall').html(data);
        }
    });
}
function FullformChangeQuestion(cname) {
    var ques_id = cname.id;
    var Qno = $("#" + ques_id + "").text();

    $.ajax({
        type: "GET",
        url: '/StudentExam/FullFormQuestion',
        data: { QuestionId: ques_id, QuestionNo: Qno },
        success: function (data, textStatus, jqXHR) {
            $('#Partiall').html(data);
        }
    });
}
function ResetAnswer() {
    $("#StudentGiveAnswer").val('');
    $("#CorrecrtWrong").val('');
    $("#GainMarks").val('');
    $('input[type=radio]').prop('checked', false);
}
function Pre() {   
    if (parseFloat($("#QNo").val()) > 1)
    {
        var Qno = (parseFloat($("#QNo").val()) - 1);
        var ques_id = $("." + Qno + "").attr("id");
        $.ajax({
            type: "GET",
            url: '/StudentExam/ObjectiveQuestion',
            data: { QuestionId: ques_id, QuestionNo: Qno },
            success: function (data, textStatus, jqXHR) {
                $('#Partiall').html(data);
            }
        });
    }
}
function Next() {
    if (parseFloat($("#QNo").val()) != parseFloat($(".Raj").last().html())) {
        var Qno = (parseFloat($("#QNo").val()) + 1);
        var ques_id = $("." + Qno + "").attr("id");
        $.ajax({
            type: "GET",
            url: '/StudentExam/ObjectiveQuestion',
            data: { QuestionId: ques_id, QuestionNo: Qno },
            success: function (data, textStatus, jqXHR) {
                $('#Partiall').html(data);
            }
        });
    }
}