import { GetExamByIdAsync, GetExamsAsync } from "./MainPageScripts/StudentMainPage/ExamEntity.js";

const submitButton = document.getElementById('submitBtn');

let timeInterval;
let examId = localStorage.getItem("examId");
let studentId = localStorage.getItem("studentId");
let examAttendanceId = localStorage.getItem("examAttendanceId");
let accessToken = localStorage.getItem("AccessToken");
let examData;

document.addEventListener("DOMContentLoaded", async function () {
    await FetchExamData();
    await FetchTimerData();
    const timerElement = document.querySelector('.timer');
    GenerateExamPage(timerElement);
});

submitButton.addEventListener('click', function(event) {
    event.preventDefault(); 
    HandleExamSubmit();
    clearInterval(timeInterval);
});

//Fetch exam data from server
async function FetchExamData() {
    const url = `http://localhost:5113/exams/generate/${examId}/${studentId}`; 
    await fetch(url, {
        method: 'GET',
        headers: {
            'Content-Type' : 'application/json',
            'Authorization' : `Bearer ${accessToken}`
        }
    })
        .then(response => {
            if (!response.ok) {
                console.log("Get request failed");
            } else {
                return response.json();
            }
        })
        .then(data => {
           examData = data;
        })
        .catch(error => {
            console.error('Error:', error.message);
        });
}

//generate exam based on fetched data
async function GenerateExamPage(timerElement) {
    const welcomeHeaders = document.getElementsByClassName('WelcomeHeader');
    welcomeHeaders[0].textContent = welcomeHeaders[0].textContent.replace('{INSERT_COURSE_DOMAIN_HERE}', examData.courseDomain);

    const timer = welcomeHeaders[1].querySelector('.timer');
    timer.setAttribute('data-exam-time', examData.examTime);
    const examTimeMinutes = parseInt(timer.getAttribute("data-exam-time"), 10);
    StartTimer(timerElement);

    document.title = document.title.replace('{INSERT_STUDENT_NAME_HERE}', examData.studentName);
   
    const generatedProblemContent = await GenerateProblemsAsync();
    const brElement = document.querySelector('br');
    brElement.insertAdjacentHTML('afterend', generatedProblemContent);
    
}

//handle exam submit button logic -> send data, get feedback, highlight errors/correct answers
async function HandleExamSubmit(){
    const answers = [];
    const problems = document.getElementsByClassName('problem');

    for(const problem of problems){
        const typeOfProblem = problem.getAttribute('data-problem-type');
        const problemId = problem.getAttribute('data-problem-id');
        let answerOptions = [];
        let checkedAnswers = [];

        switch (typeOfProblem){
            case "SingleAnswer" :
                answerOptions = problem.getElementsByClassName('answerRadio')
                break;

            case "MultipleAnswer" :
                answerOptions = problem.getElementsByClassName('answerCheckbox')
                break;

            case "TextAnswer" :
                answerOptions = problem.getElementsByClassName('answerText')
                break;

            default:
                break;
        }

        for (const answerOption of answerOptions) {
            if (typeOfProblem === "SingleAnswer" || typeOfProblem === "MultipleAnswer") {
                answerOption.disabled = true;
                if (answerOption.checked === true) {
                    checkedAnswers.push(answerOption.value);
                }
            } else if (typeOfProblem === "TextAnswer") {
                answerOption.disabled = true;
                if (answerOption.value !== "") {
                    checkedAnswers.push(answerOption.value);
                }
            }
        }

        answers.push({
            problemId: parseInt(problemId),
            answers: checkedAnswers
        });

    }

    await SendAnswersAsync(answers);
    let examFeedback = await GetExamFeedbackAsync();
    await HighlightAnswersAsync(examFeedback.correctAnswers);

    const goBackBtn = document.getElementById('goBackBtn');
    submitButton.style.display = 'none';
    goBackBtn.style.display = 'block';

    localStorage.removeItem("examId");
    localStorage.removeItem("firstVisitTime");

    goBackBtn.addEventListener('click', () => {
        window.location.href = 'StudentMainPage.html';
    });

    const finalGrade = document.getElementById('finalGrade');
    finalGrade.textContent = "Exam grade:" + examFeedback.finalGrade.toString();
}



async function GetExamFeedbackAsync() {
    const exam = await GetExamByIdAsync(examId);
    const problems = examData.problems;
    
    let correctAnswers = {};

    for(const problem of problems){
        const correctAnswersOfProblem = await GetCorrectAnswersOfProblemAsync(problem.id);

        if (!correctAnswers[problem.id]) {
            correctAnswers[problem.id] = [];
        }

        for(const correctAnswerOfProblem of correctAnswersOfProblem){
            correctAnswers[problem.id].push(correctAnswerOfProblem.answer);
        }
    }

    console.log(correctAnswers);

    return {
        finalGrade: exam.finalGrade,
        correctAnswers: correctAnswers
    };
}

async function SendAnswersAsync(answers) {
    const url = `http://localhost:5113/exams/grade-exam/${examId}/${studentId}`;
    await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type' : 'application/json',
            'Authorization' : `Bearer ${accessToken}`
        },
        body: JSON.stringify(answers)
    })
        .then(response => {
            if (response.ok) {
                console.log("Successful post request!");
            } else {
                console.log("Post request failed!");
            }
        })
        .catch(error => {
            console.error('Error:', error.message);
        });
}

async function HighlightAnswersAsync(correctAnswersFromServer) {
    const problems = document.getElementsByClassName('problem');
    for (const problem of problems) {
        const typeOfProblem = problem.getAttribute('data-problem-type');
        const problemId = problem.getAttribute('data-problem-id');
        const problemPoints = problem.querySelector('.points');
        const maxPoints = problemPoints.textContent;

        let answerOptions = [];

        switch (typeOfProblem){
            case "SingleAnswer" :
                answerOptions = problem.getElementsByClassName('answerRadio')
                break;

            case "MultipleAnswer" :
                answerOptions = problem.getElementsByClassName('answerCheckbox')
                break;

            case "TextAnswer" :
                answerOptions = problem.getElementsByClassName('answerText')
                break;

            default:
                break;
        }

        if(typeOfProblem === "SingleAnswer"){
            const checkedAnswer = problem.querySelector('input[type="radio"]:checked');
            const correctAnswers = correctAnswersFromServer[problemId];
            if(checkedAnswer){
                checkedAnswer.style.setProperty('--radio-checked-color', 'transparent');

                if(correctAnswers.includes(checkedAnswer.value)){
                    checkedAnswer.style.backgroundColor = 'green';
                    problem.style.borderColor = 'green';
                    problemPoints.textContent = `Earned ${maxPoints} / ${maxPoints}`;

                } else {
                    problemPoints.textContent = `Earned 0 / ${maxPoints}`;
                    checkedAnswer.style.backgroundColor = 'red';
                    problem.style.borderColor = 'red';
                    problem.querySelector(`.answerRadio[value="${correctAnswers[0]}"]`).style.backgroundColor = 'green';
                }
            } else {
                problemPoints.textContent = `Earned 0 / ${maxPoints}`;
                problem.style.borderColor = 'red';
                problem.querySelector(`.answerRadio[value="${correctAnswers[0]}"]`).style.backgroundColor = 'green';
            }
            
        }
        else if(typeOfProblem === "MultipleAnswer"){
            const checkedAnswers = problem.querySelectorAll('input[type="checkbox"]:checked');
            const correctAnswers = correctAnswersFromServer[problemId];
            console.log(correctAnswers);

            let wrongAnswers = false;
            let numberOfCorrectAnswersChecked = 0;
            let numberOfWrongAnswersChecked = 0;
            let numberOfCorrectAnswers = correctAnswers.length;
    
            for(const checkedAnswer of checkedAnswers){
                checkedAnswer.style.setProperty('--checkbox-checked-color', 'transparent');
                if(correctAnswers.includes(checkedAnswer.value)){
                    checkedAnswer.style.backgroundColor = 'green';
                    numberOfCorrectAnswersChecked++;
                } else {
                    checkedAnswer.style.backgroundColor = 'red';
                    numberOfWrongAnswersChecked++;
                    wrongAnswers = true;
                }
            }

            if(wrongAnswers === true || numberOfCorrectAnswersChecked < numberOfCorrectAnswers){
                problem.style.borderColor = 'red';
                for(const correctAnswer of correctAnswers){
                    problem.querySelector(`.answerCheckbox[value="${correctAnswer}"]`).style.backgroundColor = 'green';
                }

                if(numberOfCorrectAnswersChecked === 0){
                    problemPoints.textContent = `Earned 0 / ${maxPoints}`;
                    return;
                }

                let pointsPerCorrectAnswer = parseFloat(maxPoints) / numberOfCorrectAnswers;
                console.log(Math.abs(pointsPerCorrectAnswer * (numberOfCorrectAnswersChecked - numberOfWrongAnswersChecked)));
                problemPoints.textContent = `Earned ${Math.abs(pointsPerCorrectAnswer * (numberOfCorrectAnswersChecked - numberOfWrongAnswersChecked))} / ${maxPoints}`;
            } else {
                problem.style.borderColor = 'green';
                problemPoints.textContent = `Earned ${maxPoints} / ${maxPoints}`;
            }
        }
        else{
            const textAnswer = problem.querySelector('input[type="text"]');
            const correctAnswer = correctAnswersFromServer[problemId][0];
         
            if (textAnswer && textAnswer.value.trim().toLowerCase() === correctAnswer.trim().toLowerCase()) {
                textAnswer.style.borderColor = 'green';
                problem.style.borderColor = 'green';
                problemPoints.textContent = `Earned ${maxPoints} / ${maxPoints}`;
            } else {
                problem.style.borderColor = 'red';
                const labelElement = document.createElement('span');
                labelElement.textContent = `Correct answer: ${correctAnswer}`;
                labelElement.style.color = 'green';
                labelElement.style.display = 'block';
                const brElement = problem.querySelector('br');
                problem.insertBefore(labelElement, brElement.nextSibling);
                problemPoints.textContent = `Earned 0 / ${maxPoints}`;
            }
        }
    }
}

//Problem generator
async function GenerateProblemsAsync(){
    let problemHtmlContent = '';

    for(const problem of examData.problems){
        problemHtmlContent += `<div class='problem' data-problem-id='${problem.id}' data-problem-type='${problem.problemType}'><p class='problemText'>${problem.text}</p>`;

        var answerOptions = await GetAnswerOptionsOfProblemAsync(problem.id);
        console.log(problem.problemType);

        switch (problem.problemType) {
            case 'SingleAnswer':
                problemHtmlContent += GenerateProblemsWithSingleAnswerOption(answerOptions);
                break;

            case 'MultipleAnswer':
                problemHtmlContent += GenerateProblemsWithMultipleAnswerOptions(answerOptions);
                break;

            case 'TextAnswer':
                problemHtmlContent += GenerateProblemsWithTextAnswerOption(problem);
                break;
        }

        problemHtmlContent += `<p class='points'>${problem.points} points</p></div><br>`;
    }
   
    return problemHtmlContent;
}

function GenerateProblemsWithSingleAnswerOption(answerOptions) {
    let answerHtml = '';
    for (const answer of answerOptions) {
        console.log(":" + answer);
        answerHtml += 
        `<label>
        <input class='answerRadio' name='${answer.problemId}' type='radio' value='${answer.answer}' />
        <span class='answer-option'>${answer.answer}</span>
        </label>
        <br>`;
    }

    return answerHtml;
}

function GenerateProblemsWithMultipleAnswerOptions(answerOptions) {
    let answerHtml = '';
    for (const answer of answerOptions) {
        answerHtml +=
         `<label>
        <input class='answerCheckbox' name='${answer.problemId}' type='checkbox' value='${answer.answer}' />
        <span class='answer-option'>${answer.answer}</span>
        </label>
        <br>`;
    }

    return answerHtml;
}

function GenerateProblemsWithTextAnswerOption(problem) {
    return `<label><input class='answerText' name='${problem.id}' type='text' /></label><br>`;
}

async function FetchTimerData(){
    const url = `http://localhost:5113/exams/${examId}/remaining-time`;

    try {
        const response = await fetch(url, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization' : `Bearer ${accessToken}`
            }
        });

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();
        const remainingMinutes = data.remainingMinutes;
        const remainingSeconds = data.remainingSeconds;
        const timerElement = document.querySelector('.timer');
        timerElement.setAttribute('data-remaining-minutes', remainingMinutes);
        timerElement.setAttribute('data-remaining-seconds', remainingSeconds);        
    } catch (error) {
        console.error('Fetch error:', error);
    }
}

//timer logic
function StartTimer(display) {
    let remainingMinutes = parseInt(display.getAttribute('data-remaining-minutes'), 10);
    let remainingSeconds = parseInt(display.getAttribute('data-remaining-seconds'), 10);

    timeInterval = setInterval(function () {
        if (remainingMinutes === 0 && remainingSeconds === 0) {
            clearInterval(timeInterval);
            HandleExamSubmit();
            return;
        }

        if (remainingSeconds === 0) {
            remainingMinutes--;
            remainingSeconds = 59;
        } else {
            remainingSeconds--;
        }

        const formattedMinutes = remainingMinutes < 10 ? "0" + remainingMinutes : remainingMinutes;
        const formattedSeconds = remainingSeconds < 10 ? "0" + remainingSeconds : remainingSeconds;

        display.textContent = formattedMinutes + ":" + formattedSeconds;

        remainingMinutes = Math.max(remainingMinutes, 0);
        remainingSeconds = Math.max(remainingSeconds, 0);

        display.dataset.remainingMinutes = remainingMinutes;
        display.dataset.remainingSeconds = remainingSeconds;

    }, 1000);
}

async function GetAnswerOptionsOfProblemAsync(problemId){
    const url = new URL(`http://localhost:5113/problems/answer-options/${problemId}`);
    let fetchedData;
    const requestOptions = {
        method: 'GET',
        headers: {
            'Content-Type' : 'application/json',
            'Authorization' : `Bearer ${accessToken}`
        },
    };

    await fetch(url.href, requestOptions)
    .then(response => response.json())
    .then(data => fetchedData = data)
    .catch(error => console.log(error));

    return fetchedData;
}

async function GetCorrectAnswersOfProblemAsync(problemId){
    const url = new URL(`http://localhost:5113/problems/correct-answers/${problemId}`);
    let fetchedData;
    const requestOptions = {
        method: 'GET',
        headers: {
            'Content-Type' : 'application/json',
            'Authorization' : `Bearer ${accessToken}`
        },
    };

    await fetch(url.href, requestOptions)
    .then(response => response.json())
    .then(data => fetchedData = data)
    .catch(error => console.log(error));

    return fetchedData;
}