namespace ETicaretAPI.Application.Exceptions;

public class UserCreateFailedException:Exception
{
    public UserCreateFailedException():base("Kullanici olusturulurken beklenmeyen bir hata meydana geldi")
    {
        
    }

    public UserCreateFailedException(string? message):base(message)
    {
        
    }
    
    public UserCreateFailedException(string? message, Exception? innerException):base(message,innerException)
    {
        
    }
}