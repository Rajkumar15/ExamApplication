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
    var ques_id = cname.id;
    var Qno = $("#" + ques_id + "").text();
    $.ajax({
        type: "GET",
        url: '/StudentExam/ObjectiveQuestion',
        data: { QuestionId: ques_id, QuestionNo: Qno },
        success: function (data, textStatus, jqXHR) {       
            $('#Partiall').html(data);          
        },
        error: function (xhr, textStatus, errorThrown) {
            alert('Session Expired');
            window.location.href = "/StudentExam/StudentLogin";
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
        },
        error: function (xhr, textStatus, errorThrown) {
            alert('Session Expired');
            window.location.href = "/StudentExam/StudentLogin";
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
        },
        error: function (xhr, textStatus, errorThrown) {
            alert('Session Expired');
            window.location.href = "/StudentExam/StudentLogin";
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
        },
        error: function (xhr, textStatus, errorThrown) {
            alert('Session Expired');
            window.location.href = "/StudentExam/StudentLogin";
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
        },
        error: function (xhr, textStatus, errorThrown) {
            alert('Session Expired');
            window.location.href = "/StudentExam/StudentLogin";
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
    if (parseFloat($("#QNo").val()) > 1) {
        var Qno = (parseFloat($("#QNo").val()) - 1);
        var ques_id = $("." + Qno + "").attr("id");
        $.ajax({
            type: "GET",
            url: '/StudentExam/ObjectiveQuestion',
            data: { QuestionId: ques_id, QuestionNo: Qno },
            success: function (data, textStatus, jqXHR) {
                $('#Partiall').html(data);
            },
            error: function (xhr, textStatus, errorThrown) {
                alert('Session Expired');
                window.location.href = "/StudentExam/StudentLogin";
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
            },
            error: function (xhr, textStatus, errorThrown) {
                alert('Session Expired');
                window.location.href = "/StudentExam/StudentLogin";
            }
        });
    }
}
function ResetAnswerFullForm()
{
    $(".STA").val('');
}
function ResetAnswerMatchcontent()
{
    $("#select-to").html('');
    $("#select-to").attr("readonly", false)
    $("#btn-remove").show();
    $("#btn-add").show();
    $(".ssss").hide();
}
function FinalExamSubmit() {
    if (confirm("After Submit Exam Your Exam Will Closed & Submitted.."))
    {
        var e = $("#Examid").val();
        var s = $("#Studentid").val();
        $.getJSON("/StudentExam/SubmitExam?eid=" + e + "&sid=" + s, function (data) {          
            if (data == "s") { window.location.href = "/StudentExam/AfterSubmitForm";}
        })
    }
}