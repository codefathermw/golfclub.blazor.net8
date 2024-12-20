using GolfClub.BLL.Enums;

namespace GolfClub.BLL.Helpers
{
    public static class UiHelper
    {
        public static object NotAvail(object input) => input ?? "N/A";

        public static string GetTaskStatusClass(string status) =>
            Enum.TryParse(status, true, out StatusEnum statusEnum)
                ? statusEnum switch
                {
                    StatusEnum.Acknowledged => "primary",
                    StatusEnum.Prepped => "warning",
                    StatusEnum.Scheduled => "info",
                    StatusEnum.Cancelled or StatusEnum.Suspended => "danger",
                    StatusEnum.Pending => "secondary",
                    StatusEnum.Completed => "success",
                    _ => "light",
                }
                : "light";

        public static (string CssClass, string Icon) GetTaskStatusInfo(string status) =>
            Enum.TryParse(status, true, out StatusEnum statusEnum)
                ? statusEnum switch
                {
                    StatusEnum.Acknowledged => ("primary", "bi-check-circle"),
                    StatusEnum.Prepped => ("info", "bi-hourglass-split"),
                    StatusEnum.Scheduled => ("warning", "bi-calendar-event"),
                    StatusEnum.Cancelled => ("danger", "bi-x-circle"),
                    StatusEnum.Suspended => ("secondary", "bi-pause-circle"),
                    StatusEnum.Pending => ("light", "bi-hourglass"),
                    StatusEnum.Completed => ("success", "bi-check2-circle"),
                    _ => ("light", "bi-question-circle"),
                }
                : ("light", "bi-question-circle");

        public static string Normalize(string input)
        {
            var words = input.Split('-');

            for (int i = 0; i < words.Length; i++)
            {
                words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1);
            }

            return string.Join(" ", words);
        }
    }
}