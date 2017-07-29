using Microsoft.Bot.Builder.FormFlow;
namespace HackFest.WellHealthBot.Models.BMI
{
    public enum WeightUnit
    {
        //[Describe("KG.Example: 5'7\"")]
        [Terms("kg","kilo","kilogram","k.g.","k")]
        KG = 1,
        //[Describe("LBS.Example: 138")]
        [Terms("lbs","pound","lb","l")]
        LBS = 2
    }

    public enum HeightUnit
    {
        [Terms("feet","foot","ft","f")]
        Feet = 1,
        [Terms("centimeter","cen.","cent","cm","cent.","c")]
        Centimeter = 2
    }
}