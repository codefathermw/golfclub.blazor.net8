public static class StringHelper{
    public static object NotAvail(object input) {
        if (input is null) {
            return "N/A";
        }

        return input;
    }

    public static string GetTaskStatusClass(string status)
    {
        return status switch
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
    }

    public static (string CssClass, string Icon) GetTaskStatusInfo(string status)
    {
        return status switch
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