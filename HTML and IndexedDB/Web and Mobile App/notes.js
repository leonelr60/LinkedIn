var indexedDB = window.indexedDB || window.mozIndexedDB || window.webkitIndexedDB || window.msIndexedDB;

function funcmenuload(){
	
	startdb();
	
	
}

function startdb() {
	
	var dbversion = sessionStorage.getItem("sessiondb_version");
	dataBase = indexedDB.open("dbteacha", dbversion);

        dataBase.onupgradeneeded = function (e) {

            //Already loaded
        };

        dataBase.onsuccess = function (e) {
		
	    loadyear();
	    
        };

        dataBase.onerror = function (e)  {
            alert('Error cargando la base de datos');
        };

}

function alertmsg(smsg) {
	alert(smsg);
}

function loadyear() {
	
	var active = dataBase.result;
	
        var data = active.transaction(["current_year"], "readonly");
	
        var object = data.objectStore("current_year");
	
        var elements = [];


        object.openCursor().onsuccess = function (e) {

            var result = e.target.result;

            if (result === null) {
                return;
            }

            elements.push(result.value);
            result.continue();

        };
	
        data.oncomplete = function () {

            var sVal = '';

            for (var key in elements) {

                sVal = elements[key].idyear;

            }
	    
            document.querySelector("#idyear").value = sVal;
	    loadcurrentcicle();
	    loadgrades();
		
            loadcicles();
        };
}

function loadcurrentcicle() {
	
	var active = dataBase.result;
	
        var data = active.transaction(["currcicle"], "readonly");
	
        var object = data.objectStore("currcicle");
	
        var elements = [];


        object.openCursor().onsuccess = function (e) {

            var result = e.target.result;

            if (result === null) {
                return;
            }

            elements.push(result.value);
            result.continue();

        };
	
        data.oncomplete = function () {

            
            for (var key in elements) {

                iCurrCicle = elements[key].idcicle;

            }
	    
            
        };
}
function loadcicles() {
	var active = dataBase.result;
        var data = active.transaction(["cicles"], "readonly");
        var object = data.objectStore("cicles");
	var index = object.index("by_year");
        var elements = [];
	var sYear = document.querySelector("#idyear").value;
        index.openCursor(sYear).onsuccess = function (e) {

            var result = e.target.result;

            if (result === null) {
                return;
            }

            elements.push(result.value);
            result.continue();

        };
	
        data.oncomplete = function () {

            var outerHTML = '<select id="objselectcicle" onchange="loadsubjects();">';

            for (var key in elements) {
		if(Number(iCurrCicle) == Number(elements[key].idcicle))
		{
                	outerHTML += '\n\
                	<option value="'+elements[key].idcicle+'">' + elements[key].ciclename + '</option>';
		}
                
            }
	    outerHTML += '\n\ </select>';
            elements = [];
            document.querySelector("#divselectcicleobj").innerHTML = outerHTML;
	    loadsubjects();
        };
}

function loadgrades() {
	var active = dataBase.result;
        var data = active.transaction(["grades"], "readonly");
        var object = data.objectStore("grades");
	var index = object.index("by_year");
        var elements = [];
	var sYear = document.querySelector("#idyear").value;
        index.openCursor(sYear).onsuccess = function (e) {

            var result = e.target.result;

            if (result === null) {
                return;
            }

            elements.push(result.value);
            result.continue();

        };
	
        data.oncomplete = function () {

            var outerHTML = '<select id="objselect" onchange="loadsubjects();loadstudents();">';

            for (var key in elements) {

                outerHTML += '\n\
                	<option value="'+elements[key].idgrade+'">' + elements[key].gradename + '</option>';
                
            }
	    outerHTML += '\n\ </select>';
            elements = [];
            document.querySelector("#divselectobj").innerHTML = outerHTML;
	    loadsubjects();
	    loadstudents();
        };
}

function loadsubjects() {
	var active = dataBase.result;
        var data = active.transaction(["subjects"], "readonly");
        var object = data.objectStore("subjects");
	var index = object.index("by_year_grade");
        var elements = [];
	var sYear = document.querySelector("#idyear").value;
	var sGrade = document.querySelector("#objselect").value;
        index.openCursor([sYear, sGrade]).onsuccess = function (e) {

            var result = e.target.result;

            if (result === null) {
                return;
            }

            elements.push(result.value);
            result.continue();

        };
	
        data.oncomplete = function () {

            var outerHTML = '<select id="objselectsub" onchange="loadaspects();">';

            for (var key in elements) {

                outerHTML += '\n\
                	<option value="'+elements[key].idsubject+'">' + elements[key].subjectname + '</option>';
                
            }
	    outerHTML += '\n\ </select>';
            elements = [];
            document.querySelector("#divselectsubobj").innerHTML = outerHTML;
	    loadaspects();
        };
}

function loadstudents() {
	var active = dataBase.result;
        var data = active.transaction(["students"], "readonly");
        var object = data.objectStore("students");
	var index = object.index("by_year_grade");
        var elements = [];
	var sYear = document.querySelector("#idyear").value;
	var sGrade = document.querySelector("#objselect").value;
        index.openCursor([sYear, sGrade]).onsuccess = function (e) {

            var result = e.target.result;

            if (result === null) {
                return;
            }

            elements.push(result.value);
            result.continue();

        };
	
        data.oncomplete = function () {

            var outerHTML = '<select id="objselectstudents" onchange="loadaspects();">';

            for (var key in elements) {

                outerHTML += '\n\
                	<option value="'+elements[key].idstudent+'">' + elements[key].studentsurname + ' ' + elements[key].studentname + '</option>';
                
            }
	    outerHTML += '\n\ </select>';
            elements = [];
            document.querySelector("#divselectstudobj").innerHTML = outerHTML;
	    loadaspects();
        };
}

function loadaspects() {
	var active = dataBase.result;
        var data = active.transaction(["aspects"], "readonly");
        var object = data.objectStore("aspects");
	var index = object.index("by_year_grade_subject_cicle");
        var elements = [];
	var sYear = document.querySelector("#idyear").value;
	var sGrade = document.querySelector("#objselect").value;
	var sSubject = document.querySelector("#objselectsub").value;
	var sCicle = document.querySelector("#objselectcicle").value;
        index.openCursor([sYear, sGrade, sSubject, sCicle]).onsuccess = function (e) {

            var result = e.target.result;

            if (result === null) {
                return;
            }

            elements.push(result.value);
            result.continue();

        };
	
        data.oncomplete = function () {

            var outerHTML = '<select id="objselectasp" onchange="loadactivities();">';

            for (var key in elements) {

                outerHTML += '\n\
                	<option value="'+elements[key].idaspect+'">' + elements[key].aspectname + ' - ' + elements[key].punctuation + ' pts.' + '</option>';
                
            }
	    outerHTML += '\n\ </select>';
            elements = [];
            document.querySelector("#divselectaspobj").innerHTML = outerHTML;
	    loadactivities();
        };
}

function loadactivities() {
	var active = dataBase.result;
        var data = active.transaction(["study_plan"], "readonly");
        var object = data.objectStore("study_plan");
	var index = object.index("by_year_grade_subject_aspect_cicle");
        var elements = [];
	var sYear = document.querySelector("#idyear").value;
	var sGrade = document.querySelector("#objselect").value;
	var sSubject = document.querySelector("#objselectsub").value;
	var sAspect = document.querySelector("#objselectasp").value;
	var sCicle = document.querySelector("#objselectcicle").value;
        index.openCursor([sYear, sGrade, sSubject, sAspect, sCicle]).onsuccess = function (e) {

            var result = e.target.result;

            if (result === null) {
                return;
            }

            elements.push(result.value);
            result.continue();

        };
	
        data.oncomplete = function () {

            var outerHTML = '<select id="objselectactivity" onchange="loaddata();">';

            for (var key in elements) {

                outerHTML += '\n\
                	<option value="'+elements[key].idplan+'">' + elements[key].activitydate + ' - ' + elements[key].activityname + ' - Valor ' + elements[key].punctuation + ' pts.' + '</option>';
                
            }
	    outerHTML += '\n\ </select>';
            elements = [];
            document.querySelector("#divselectactobj").innerHTML = outerHTML;
	    
	    loaddata();
        };
}



function loaddata() {
	var active = dataBase.result;
        var data = active.transaction(["study_plan"], "readonly");
        var object = data.objectStore("study_plan");
	var index = object.index("by_year_grade_subject_aspect_plan_cicle");

        var elements = [];
	
	var sYear = document.querySelector("#idyear").value;
	var sGrade = document.querySelector("#objselect").value;
	var sSubject = document.querySelector("#objselectsub").value;
	var sAspect = document.querySelector("#objselectasp").value;
	var sCicle = document.querySelector("#objselectcicle").value;
	var sActivity = document.querySelector("#objselectactivity").value;
	index.openCursor([sYear, sGrade, sSubject, sAspect, sActivity, sCicle]).onsuccess = function (e) {
        
            var result = e.target.result;

            if (result === null) {
                return;
            }
	    
            elements.push(result.value);
            result.continue();

        };
	
        data.oncomplete = function () {

            var sPunct = '';
		var sObs = '';
		var sDate = '';

            for (var key in elements) {

		sPunct = elements[key].punctuation;
		sObs = elements[key].activityobs;
		sDate = elements[key].activitydate;
            }

            document.querySelector("#punctuation").value = sPunct;
		document.querySelector("#activitydesc").value = sObs;
		document.querySelector("#activitydate").value = sDate;
		loadhistory();
        };
}

function loadhistory() {
	var active = dataBase.result;
        var data = active.transaction(["notes"], "readonly");
        var object = data.objectStore("notes");
	var index = object.index("by_year_grade_subject_aspect_plan_student_cicle");

        var elements = [];

	var sYear = document.querySelector("#idyear").value;
	var sGrade = document.querySelector("#objselect").value;
	var sSubject = document.querySelector("#objselectsub").value;
	var sAspect = document.querySelector("#objselectasp").value;
	var sCicle = document.querySelector("#objselectcicle").value;
	var sActivity = document.querySelector("#objselectactivity").value;
	var sStudent = document.querySelector("#objselectstudents").value;
	index.openCursor([sYear, sGrade, sSubject, sAspect, sActivity, sStudent, sCicle]).onsuccess = function (e) {
        
            var result = e.target.result;

            if (result === null) {
                return;
            }
	    
            elements.push(result.value);
            result.continue();

        };
	
        data.oncomplete = function () {

            var outerHTML = '';

            for (var key in elements) {

		outerHTML += '\n\
                <tr>\n\
                    <td>' + elements[key].idplan + '</td>\n\
		    <td>' + elements[key].notedate + '</td>\n\
                    <td>' + elements[key].noteobs + '</td>\n\
		    <td>' + elements[key].punctuation + '</td>\n\
		    <td>\n\
                        <button type="button" class="img_btn" onclick="deletedata(' + elements[key].id + ')"><img src="images/delete.png" height="32" style="width:auto;" /></button>\n\
                    </td>\n\
                </tr>';
		
            }

            elements = [];
            document.querySelector("#elementsList").innerHTML = outerHTML;
        };
}


function insupddata() {
	var active = dataBase.result;
	var data = active.transaction(["notes"], "readwrite");
	var object = data.objectStore("notes");  
	var sTrim = $('#objselectactivity option:selected').text();
	sTrim = sTrim.trimRight().trimLeft();
	sTrim = sTrim.substring(13);
	var sAct = sTrim + ' ' + document.querySelector("#activityobs").value;
	var sName = $('#objselectstudents option:selected').text();
	var request = object.put({
	   idyear: document.querySelector("#idyear").value,
	   idcicle: document.querySelector("#objselectcicle").value,
	   notedate: document.querySelector("#activitydate").value,
	   idaspect: document.querySelector("#objselectasp").value,
	   idplan: document.querySelector("#objselectactivity").value,
	   noteobs: sAct,
	   punctuation: document.querySelector("#punctuation").value,
	   idgrade: document.querySelector("#objselect").value,
	   idsubject: document.querySelector("#objselectsub").value,
	   idstudent: document.querySelector("#objselectstudents").value,
	   studentname: sName
	});
	
	request.onerror = function (e) {
	    alert(request.error.name + '\n\n' + request.error.message);
	};

	data.oncomplete = function (e) {
	    
	    alertmsg('The record has been saved...');
	    
		document.querySelector("#activitydate").value = "";
		document.querySelector("#punctuation").value = "0";
		document.querySelector("#activityobs").value = "";
		
		loaddata();
		
	};
}

function deletedata(id) {
	var active = dataBase.result;
	var data = active.transaction(["notes"], "readwrite");
	var object = data.objectStore("notes");  

	var request = object.delete(id);

	request.onerror = function (e) {
	    alert(request.error.name + '\n\n' + request.error.message);
	};

	data.oncomplete = function (e) {
	    
	    alertmsg('The record has been deleted...');
	    
		loaddata();
	};
}

function funcunload(){
    //Closing database 20190509
    db = dataBase.result;
    db.close();
}


