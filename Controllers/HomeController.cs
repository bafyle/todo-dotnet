using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using todo.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace todo.Controllers;

public class HomeController : Controller
{
    private AppDbContext db;

    private string getCurrentUserID()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
    public HomeController(AppDbContext db)
    {
        this.db = db;
    }

    [HttpGet]
    [Route("api/tasks")]
    [Authorize]
    public IActionResult getAllTasks()
    {
        var currentUserId = getCurrentUserID();
        var tasks = db.tasks?.Where(x=> x.UserId == currentUserId).Select(x=>new {x.TaskId, x.TaskDescription, x.Completed, x.CreateDate}).ToList();
        return Json(tasks);
    }

    [HttpPost]
    [Route("api/tasks/create")]
    [Authorize]
    public IActionResult createNewTask()
    {
        var currentUserId = getCurrentUserID();
        string taskDescription = HttpContext.Request.Form["task"];
        if(taskDescription == "" || taskDescription is null)
            return Json(new {error=true, message="Cannot create empty task"});
        Models.Task newTask = new Models.Task(){TaskDescription=taskDescription, Completed=false, UserId=currentUserId};
        db.tasks?.Add(newTask);
        db.SaveChanges();
        return Json(new{error=false,message="Task created successfully", task=newTask});
    }

    [HttpDelete]
    [Route("api/tasks/{taskID}/delete")]
    [Authorize]
    public IActionResult deleteTask(string taskID)
    {
        var task = db.tasks?.FirstOrDefault(x=> x.TaskId.ToString() == taskID);
        if (task == null)
            return Json(new {error=true, message="No task with this id"});
        db.tasks?.Remove(task);
        db.SaveChanges();
        return Json(new{error=false,message="Task deleted successfully"});
    }

    [HttpPut]
    [Route("api/tasks/{taskID}")]
    [Authorize]
    public IActionResult updateTask(string taskID)
    {
        var taskToBeUpdated = db.tasks?.SingleOrDefault(x=> x.TaskId.ToString() == taskID);
        if (taskToBeUpdated == null)
            return Json(new {error=true, message="No task with this id"});
        taskToBeUpdated.Completed = !taskToBeUpdated.Completed;
        db.SaveChanges();
        return Json(new{error=false,message="Task updated successfully", task=new{taskToBeUpdated.TaskId, taskToBeUpdated.TaskDescription, taskToBeUpdated.Completed, taskToBeUpdated.CreateDate}});
    }

    [HttpGet("/old")]
    public IActionResult oldIndex()
    {
        ViewBag.tasks = db.tasks?.ToList();
        if(TempData.Keys.Contains("error"))
        {
            ViewBag.error = TempData["error"];
        }
        if(TempData.Keys.Contains("message"))
        {
            ViewBag.message = TempData["message"];
        }
        return View("Index");
    }

    [HttpPost("/old")]
    public IActionResult oldIndex(Models.Task task)
    {
        if(!ModelState.IsValid)
        {
            TempData["error"] = "Cannot create empty task";
            return RedirectToAction("Index");
        }
        db.tasks?.Add(task);
        db.SaveChanges();
        return RedirectToAction("Index");
    }

    [HttpGet("/old/{id}")]
    public IActionResult Update(int id)
    {
        var task = db.tasks?.FirstOrDefault(item => item.TaskId == id);
        if(task == null)
        {
            TempData["error"] = "Task not found";
            return RedirectToAction("Index");
        }
        return View(task);
    }

    [HttpPost("/old/{id}")]
    public IActionResult Update(int id, Models.Task task)
    {
        var taskFromDB = db.tasks?.FirstOrDefault(item => item.TaskId == id);
        if(!ModelState.IsValid || taskFromDB == null)
        {
            TempData["error"] = "There was an error updating this task, please try again";
            return RedirectToAction("Index");
        }
        taskFromDB.TaskDescription = task.TaskDescription;
        taskFromDB.Completed = task.Completed;
        db.SaveChanges();
        TempData["message"] = "Task updated!";
        return RedirectToAction("Index");
    }
    [HttpDelete("/old/delete/{id}")]
    public IActionResult delete(int id)
    {
        var task = db.tasks?.Where(b => b.TaskId == id).FirstOrDefault();
        if(task == null)
        {
            TempData["error"] = "Task not found";
            return RedirectToAction("Index");
        }
        db.tasks?.Remove(task);
        db.SaveChanges();
        return Json(new {
            @message = "deleted"
        });
    }
    [HttpGet]
    [Route("/")]
    [Authorize]
    public IActionResult Index()
    {
        return View("Tasks");
    }

}
