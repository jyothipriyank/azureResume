window.addEventListener('DOMContentLoaded', (event) =>{
    getVisitCount();
})


const functionApiUrl = 'https://getresumecounter-deesezfcfsghe0ap.eastus-01.azurewebsites.net/api/GetResumeCounter?code=__FUNCTION_KEY__';


const getVisitCount = () => {
    let count = 30;
    fetch(functionApiUrl).then(response => {
        return response.json()
    }).then(response =>{
        console.log("Website called function API.");
        count =  response.count;
        document.getElementById("visitor-count").innerText = count;
    }).catch(function(error){
        console.log(error);
    });
    return count;
}

