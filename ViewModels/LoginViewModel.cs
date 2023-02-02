using System.ComponentModel.DataAnnotations;
namespace todo;
public class LoginViewModel
{
    [Required]
    public string email {get; set;}

    [Required]
    [DataType(DataType.Password)] 
    public string password {get; set;}
}
