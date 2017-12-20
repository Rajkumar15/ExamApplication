/// <reference path="../JS/jquery-1.8.2.min.js" />


    $(document).ready(function () {
        $("#Partiall").load("/StudentExam/ObjectiveQuestion",
           { QuestionId: 1, QuestionNo: 1 });
    })
    function ChangeQuestion(cname) {
        var ques_id = cname.id;
        var Qno = $("#" + ques_id + "").text();
        $("#Partiall").load("/StudentExam/ObjectiveQuestion",
          { QuestionId: ques_id, QuestionNo: Qno });
    }
    function TruefalseChangeQuestion(cname) {
        var ques_id = cname.id;
        var Qno = $("#" + ques_id + "").text();
        $("#Partiall").load("/StudentExam/TrueFalseQuestion",
          { QuestionId: ques_id, QuestionNo: Qno });
    }
    function BlanckSpaceChangeQuestion(cname) {
        var ques_id = cname.id;
        var Qno = $("#" + ques_id + "").text();
        $("#Partiall").load("/StudentExam/FillInBlanckQuestion",
          { QuestionId: ques_id, QuestionNo: Qno });
    }
    function MatchContentChangeQuestion(cname) {
        var ques_id = cname.id;
        var Qno = $("#" + ques_id + "").text();
        $("#Partiall").load("/StudentExam/MatchContentQuestion",
          { QuestionId: ques_id, QuestionNo: Qno });
    }
    function IdentifysignChangeQuestion(cname) {
        var ques_id = cname.id;
        var Qno = $("#" + ques_id + "").text();
        $("#Partiall").load("/StudentExam/IdentifySignQuestion",
          { QuestionId: ques_id, QuestionNo: Qno });
    }
    function FullformChangeQuestion(cname) {
        var ques_id = cname.id;
        var Qno = $("#" + ques_id + "").text();
        $("#Partiall").load("/StudentExam/FullFormQuestion",
          { QuestionId: ques_id, QuestionNo: Qno });
    }
