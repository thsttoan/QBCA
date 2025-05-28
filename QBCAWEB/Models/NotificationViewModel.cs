namespace QBCAWEB.Models
{
    public class NotificationViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime ReceivedDate { get; set; }
        public bool IsRead { get; set; }
        public string LinkUrl { get; set; } // Link để nhấp vào thông báo (nếu có)
    }
}