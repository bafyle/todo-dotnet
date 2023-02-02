namespace todo.Models;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
public class Task
{
    public int TaskId {get; set;}
    [Required]
    public string TaskDescription {get; set;} = "new task";

    public string UserId { get; set; }
    public virtual IdentityUser User { get; set; }

    [DataType(DataType.Date)]
    public DateTime CreateDate {get; set;} = DateTime.Now;

    public bool Completed {get; set;} = false;


    public override string ToString()
    {
        return String.Format("Task ID: {0}, Task Description: {1}", this.TaskId, this.TaskDescription);
    }


}