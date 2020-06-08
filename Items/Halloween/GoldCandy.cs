namespace SpiritMod.Items.Halloween
{
    public class GoldCandy : CandyBase
    {
        public static int _type;


        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Golden Candy");
            Tooltip.SetDefault("Can't be eaten, but may sell for a lot!");
        }

        public override void SetDefaults() {
            item.width = 20;
            item.height = 30;
            item.rare = 2;
            item.maxStack = 30;
            item.value = 50000;
        }
    }
}
