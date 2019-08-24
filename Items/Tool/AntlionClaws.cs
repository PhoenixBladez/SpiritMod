using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Tool
{
    public class AntlionClaws : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Antlion Digging Claws");
			Tooltip.SetDefault("Fast speed at the cost of power");
		}


        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 24;
            item.value = 2000;
            item.rare = 2;

            item.pick = 20;

            item.damage = 6;
            item.knockBack = 3;

            item.useStyle = 1;
            item.useTime = 3;
            item.useAnimation = 5;     

            item.melee = true;
            item.autoReuse = true;
            item.useTurn = false;

            item.UseSound = SoundID.Item1;
        }
    }
}