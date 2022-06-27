using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using SpiritMod.Items.Sets.CascadeSet;
using SpiritMod.Items.Sets.FloatingItems;

namespace SpiritMod.Items.Sets.CascadeSet.Mantaray_Hunting_Harpoon
{
    public class Mantaray_Hunting_Harpoon : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 30;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item3;
            Item.noMelee = true;
			Item.mountType = Mod.Find<ModMount>("Mantaray_Mount").Type;
			Item.value = Item.sellPrice(gold: 5);
        }  
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Manta Harpoon");
			Tooltip.SetDefault("Summons a rideable manta ray mount\nThe manta Ray is incapable of movement on land");
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<DeepCascadeShard>(), 14);
			recipe.AddIngredient(ItemID.SharkFin, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}