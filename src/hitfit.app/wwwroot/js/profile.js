function hide_label(labelControl) {
    var labelControlId = labelControl.id;
    var inputControlId = labelControlId.replace('Label', 'Input');
    var inputControl = document.getElementById(inputControlId);

    inputControl.value = labelControl.text.trim();

    labelControl.style.display = 'none';
    inputControl.style.display = 'block';
};

function hide_input(inputControl) {
    var inputControlId = inputControl.id;
    var labelControlId = inputControlId.replace('Input', 'Label');
    var labelControl = document.getElementById(labelControlId);

    labelControl.text = inputControl.value.trim();

    labelControl.style.display = 'block';
    inputControl.style.display = 'none';
};

$("#updatePersonalData").click(function (e) {

    e.preventDefault();

    //var form = $('form')[0];
    var form = document.forms["personalDataForm"];

    var formData = new FormData(form);

    var uploadedFile = document.getElementById("file");
    //formData.append(uploadedFile.name, uploadedFile);

    formData.append(uploadedFile.name, uploadedFile); 

    $.ajax({

        url: $(this).attr("href"),
        type: 'POST',
        contentType: false,
        processData: false,
        data: formData,
        //dataType: 'html',
        success: function () {
            alert("Данные обновлены.");
        }

    });

});