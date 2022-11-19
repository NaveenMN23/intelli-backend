namespace IntelliCRMAPIService.Model
{
    public class DashBoardDetails
    {

        public int RecentOrders { get; set; }
        public int OrderHolds { get; set; }
        public int OrderCompleted { get; set; }
        public int OrdersInProgress { get; set; }
        public WeekData OrdersCompleted { get; set; }
        public WeekData OrdersCompletedMonth { get; set; }

    }

    public class WeekData
    {
        public int Mon { get; set; }
        public int Tue { get; set; }
        public int Wed { get; set; }
        public int Thu { get; set; }
        public int Fri { get; set; }
        public int Sat { get; set; }
        public int Sun { get; set; }
    }
}
