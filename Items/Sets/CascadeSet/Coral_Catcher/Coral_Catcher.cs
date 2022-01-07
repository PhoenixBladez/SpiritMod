using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using SpiritMod.Items.Sets.CascadeSet;
using SpiritMod.Items.Sets.FloatingItems;

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
            item.CloneDefaults(ItemID.GoldenFishingRod); 
            item.fishingPole = 16;  
            item.value = Item.sellPrice(silver: 25);
            item.rare = ItemRarityID.Blue;  
            item.shoot = mod.ProjectileType("Coral_Catcher_Hook");
            item.shootSpeed = 14f;
        }
        public override void HoldItem(Player player)
		{
			player.sonarPotion = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DeepCascadeShard>(), 8);
			recipe.AddIngredient(ModContent.ItemType<Kelp>(), 14);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}