using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.CascadeSet.Coral_Catcher
{
    public class Coral_Catcher : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coral Catcher");
			Tooltip.SetDefault("Detects hooked fish");
		}
		
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.GoldenFishingRod); 
            Item.fishingPole = 16;  
            Item.value = Item.sellPrice(silver: 25);
            Item.rare = ItemRarityID.Blue;  
            Item.shoot = ModContent.ProjectileType<Coral_Catcher_Hook>();
            Item.shootSpeed = 14f;
        }

		public override void HoldItem(Player player) => player.sonarPotion = true;
	}
}