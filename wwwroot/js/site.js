// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function deleteTaskAjax(id)
{
    let xhr = new XMLHttpRequest()
    xhr.onload = function(data)
    {
        if(this.readyState === 4 && this.status === 200)
        {
            jsonResponse = JSON.parse(this.responseText)
            if(jsonResponse.message == "deleted")
            {
                document.getElementById(`${id}-task-div`).remove()
                if(document.getElementsByClassName("tasks").length == 0)
                {
                    document.getElementsByClassName("task-container")[0].innerHTML = "<p>There is no tasks</p>"
                    location.reload()
                }
            }
        }
    }
    xhr.open("DELETE", `/delete/${id}/`)
    xhr.send();
}
