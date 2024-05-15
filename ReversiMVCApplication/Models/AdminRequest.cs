namespace ReversiMVCApplication.Models;

public class AdminRequest
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public DateTime RequestDate { get; set; }
    public string Status { get; set; } // change the status later to enum
}