using GolfClub.BLL.Enums;

namespace GolfClub.Blazor.App.Domain.Helpers
{
    public static class UIHelper
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

        public static string Normalize(string input)
        {
            var words = input.Split('-');

            for (int i = 0; i < words.Length; i++)
            {
                words[i] = char.ToUpper(words[i][0]) + words[i][1..];
            }

            return string.Join(" ", words);
        }
    }
}