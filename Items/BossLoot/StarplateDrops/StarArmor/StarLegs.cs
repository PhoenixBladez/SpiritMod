using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.BossLoot.StarplateDrops.StarArmor
{
	[AutoloadEquip(EquipType.Legs)]
	public class StarLegs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Astralite Leggings");
			Tooltip.SetDefault("5% increased ranged damage\nLeave a trail of stars where you walk");
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 0, 35, 0);
			Item.rare = ItemRarityID.Orange;
			Item.defense = 7;
		}
		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Ranged) += .05f;
			if (player.velocity.X != 0f) {
				int dust = Dust.NewDust(new Vector2(player.position.X, player.position.Y + player.height - 4f), player.width, 0, DustID.Electric);
				Main.dust[dust].velocity *= 0f;
				Main.dust[dust].scale *= .4f;
				Main.dust[dust].noGravity = true;
			}
		}

		public override void ArmorSetShadows(Player player) => player.armorEffectDrawShadow = true;

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<CosmiliteShard>(), 11);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
