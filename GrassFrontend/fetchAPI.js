let fetchedDataArray = [];

function clearWeb() {
    let divText = document.getElementById("text");
    divText.innerHTML = "";
}

let GetAllUsers = async () => {
    const response = await fetch("https://localhost:7159/User/GetAllUsers");

    if (response.ok) { // if HTTP-status is 200-299
        const jsonData = await response.json();
        fetchedDataArray = jsonData;
        UserRefreshSite(fetchedDataArray);
    } else {
        alert("HTTP-Error: " + response.status);
        clearWeb();
    }
}

let GetSpecificUser = async () => {
    let userId = prompt("userId?");
    const response = await fetch("https://localhost:7159/User/GetSpecificUser?userId=" + userId);

    if (response.ok) { // if HTTP-status is 200-299
        const jsonData = await response.json();
        fetchedDataArray = jsonData;
        UserRefreshSite(fetchedDataArray);
    } else {
        alert("HTTP-Error: " + response.status);
        clearWeb();
    }
}

let GetUserJobForAll = async () => {
    const response = await fetch("https://localhost:7159/User/GetUserJobsForAll");

    if (response.ok) { // if HTTP-status is 200-299
        const jsonData = await response.json();
        fetchedDataArray = jsonData;
        JobRefreshSite(fetchedDataArray);
    } else {
        alert("HTTP-Error: " + response.status);
        clearWeb();
    }
}

let GetUserJobForSpecific = async () => {
    let userId = prompt("userId?");
    const response = await fetch("https://localhost:7159/User/GetUserJobsForSpecific?userId=" + userId);

    if (response.ok) { // if HTTP-status is 200-299
        const jsonData = await response.json();
        fetchedDataArray = jsonData;
        JobRefreshSite(fetchedDataArray);
    } else {
        alert("HTTP-Error: " + response.status);
        clearWeb();
    }
}

let GetJobInfo = async () => {
    let jobId = prompt("jobId?");
    const response = await fetch("https://localhost:7159/User/GetJobInfo?jobId=" + jobId);

    if (response.ok) { // if HTTP-status is 200-299
        const jsonData = await response.json();
        fetchedDataArray = jsonData;
        JobDetailsRefreshSite(fetchedDataArray);
    } else {
        alert("HTTP-Error: " + response.status);
        clearWeb();
    }
}

let UpdateBalance = async () => {
    let userId = prompt("userId?");
    let balance = prompt("to what balance?")
    let request = new XMLHttpRequest();
    request.open(
        "PUT",
        `https://localhost:7159/User/UpdateBalance?id=${userId}&balance=${balance}`, true
    );
    request.onload = function () {
        if (request.status >= 200 && request.status < 400) {
            const jsonData = JSON.parse(request.responseText);
            fetchedDataArray = jsonData;
        } else {
            alert("HTTP-Error: " + request.status);
            clearWeb();
        }
    };

    request.onerror = function () {
        console.error("An error occurred during the request.");
    };

    request.send();
}

let WithdrawBalance = async () => {
    let userId = prompt("userId?");
    let amount = prompt("how much to withdraw?")
    let request = new XMLHttpRequest();
    request.open(
        "PUT",
        `https://localhost:7159/User/WithdrawBalance?balance=${amount}&id=${userId}`, true
    );
    request.onload = function () {
        if (request.status >= 200 && request.status < 400) {
            const jsonData = JSON.parse(request.responseText);
            fetchedDataArray = jsonData;
        } else {
            alert("HTTP-Error: " + request.status);
            clearWeb();
        }
    };

    request.onerror = function () {
        console.error("An error occurred during the request.");
    };

    request.send();
}

let AddJob = async () => {
    let userId = prompt("userId");
    let day = prompt("day");
    let month = prompt("month");
    let year = prompt("year");
    let jobTime = prompt("JobTime");

    const response = await fetch(`https://localhost:7159/User/AddJob?userId=${userId}&day=${day}&month=${month}&year=${year}&jobTime=${jobTime}`, {method: "POST"});

    if (response.ok) { // if HTTP-status is 200-299
        const jsonData = await response.json();
        fetchedDataArray = jsonData;
    } else {
        alert("HTTP-Error: " + response.status);
    }
}

let UserRefreshSite = (data) => {
    let divText = document.getElementById("text");
    divText.innerHTML = "";

    for (let user of data) {
        let divCreate = document.createElement("div");
        let span = document.createElement("span");
        span.innerHTML = "Id: <b>" + user.id + "</b>, Name: <b>" + user.firstName + " " + user.lastName + "</b>" + ", Balance: <b>" + user.balance + "</b>";
        divCreate.appendChild(span);
        divText.appendChild(divCreate);
    }
}

let JobRefreshSite = (data) => {
    let divText = document.getElementById("text");
    divText.innerHTML = "";

    for (let job of data) {
        let divCreate = document.createElement("div");
        let span = document.createElement("span");
        span.innerHTML = "UserId: <b>" + job.userId + "</b>, JobId: <b>" + job.jobId + "</b>";;
        divCreate.appendChild(span);
        divText.appendChild(divCreate);
    }
}

let JobDetailsRefreshSite = (data) => {
    let divText = document.getElementById("text");
    divText.innerHTML = "";

    for (let job of data) {
        let divCreate = document.createElement("div");
        let span = document.createElement("span");
        span.innerHTML = "JobId: <b>" + job.jobId + "</b>, The <b>" + job.day + "</b> of <b>" + job.month + "</b> <b>" + job.year + "</b>. Jobtime: <b>" + job.jobTime + "</b> min";
        divCreate.appendChild(span);
        divText.appendChild(divCreate);
    }
}