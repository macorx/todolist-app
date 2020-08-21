namespace TodoListApp.WebApp.Models
{
    public class AdminViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalPendingItems  { get; set; }
        public int TotalDoneItems  { get; set; }
        
        public UserDetailsViewModel[] Users { get; set; }
        
    }

    public class UserDetailsViewModel
    {
        public string UserName { get; set; }
        public int TotalPendingItems { get; set; }
        public int TotalDoneItems { get; set; }
    }
}