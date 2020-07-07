using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SpiritMod.Items.Armor.ElderbarkArmor
{
	[AutoloadEquip(EquipType.Legs)]
	public class ElderbarkLegs : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Elderbark Leggings");
        }

		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 24;
			item.value = 0;
			item.rare = 0;
			item.defense = 2;
		}
        public override void UpdateEquip(Player player)
        {
            if (player.velocity.X != 0f)
            {
                int dust = Dust.NewDust(new Vector2(player.position.X, player.position.Y + player.height - 4f), player.width, 0, 3);
                Main.dust[dust].velocity *= 0f;
                Main.dust[dust].scale *= .6f;
                Main.dust[dust].noGravity = true;
            }
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<AncientBark>(), 25);
			recipe.AddIngredient(ModContent.ItemType<EnchantedLeaf>(), 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
