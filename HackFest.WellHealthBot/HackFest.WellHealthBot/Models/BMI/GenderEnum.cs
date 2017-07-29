using Microsoft.Bot.Builder.FormFlow;

namespace HackFest.WellHealthBot.Models.BMI
{
    public enum Gender
    {
        [Terms("m","male")]
        Male = 1,
        [Terms("f","female")]
        Female = 2
    }
}