using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
    public class BrokenStaff : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Broken Hero Staff");
			Tooltip.SetDefault("'A staff of ages past...'");
		}


        public override void SetDefaults()
        {
            item.width = 54;
            item.height = 54;
            item.value = 40000;
            item.rare = 7;

            item.maxStack = 99;
        }
    }
}