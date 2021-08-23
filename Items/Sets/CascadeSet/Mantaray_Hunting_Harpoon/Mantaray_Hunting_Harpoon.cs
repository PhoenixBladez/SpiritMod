using Terraria.ModLoader;
using Terraria.ID;
using Terraria;

namespace SpiritMod.Items.Sets.CascadeSet.Mantaray_Hunting_Harpoon
{
    public class Mantaray_Hunting_Harpoon : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 30;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item3;
            item.noMelee = true;
			item.mountType = mod.MountType("Mantaray_Mount");
			item.value = Item.sellPrice(gold: 5);
        }  
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Manta Harpoon");
			Tooltip.SetDefault("Summons a rideable manta ray mount\nThe manta Ray is incapable of movement on land");
		}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Coral, 10);
            recipe.AddIngredient(ItemID.SharkFin, 2);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}