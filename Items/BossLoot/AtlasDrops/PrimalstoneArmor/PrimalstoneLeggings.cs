using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.BossLoot.AtlasDrops.PrimalstoneArmor
{
	[AutoloadEquip(EquipType.Legs)]
	public class PrimalstoneLeggings : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Primalstone Leggings");
			Tooltip.SetDefault("9% increased melee and magic damage\n5% reduced mana usage\n10% reduced movement speed");
		}

		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 30;
			Item.value = 10;
			Item.rare = ItemRarityID.Cyan;
			Item.defense = 17;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Melee) += 0.09f;
			player.GetDamage(DamageClass.Magic) += 0.09f;
			player.manaCost -= .05f;
			player.moveSpeed -= .1f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<ArcaneGeyser>(), 12);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}