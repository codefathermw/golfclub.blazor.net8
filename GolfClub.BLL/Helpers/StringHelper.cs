namespace GolfClub.BLL.Helpers
{
    public static class StringHelper
    {
        public static object NotAvail(object input) => input ?? "N/A";

        public static string GetTaskStatusClass(string status) => status?.ToLowerInvariant() switch
        {
            "acknowledged" => "primary",
            "prepped" => "info",
            "scheduled" => "warning",
            "cancelled" => "danger",
            "suspended" => "secondary",
            "pending" => "light",
            "completed" => "success",
            _ => "light",
        };

        public static (string CssClass, string Icon) GetTaskStatusInfo(string status) => status?.ToLowerInvariant() switch
        {
            "acknowledged" => ("primary", "bi-check-circle"),
            "prepped" => ("info", "bi-hourglass-split"),
            "scheduled" => ("warning", "bi-calendar-event"),
            "cancelled" => ("danger", "bi-x-circle"),
            "suspended" => ("secondary", "bi-pause-circle"),
            "pending" => ("light", "bi-hourglass"),
            "completed" => ("success", "bi-check2-circle"),
            _ => ("light", "bi-question-circle"),
        };
    }
}