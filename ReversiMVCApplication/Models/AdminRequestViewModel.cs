namespace ReversiMVCApplication.Models;

public class AdminRequestViewModel
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }
    public DateTime RequestDate { get; set; }
    public string Status { get; set; }
}