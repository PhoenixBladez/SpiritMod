using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.BossLoot.StarplateDrops.StarArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class StarPlate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Astralite Chestguard");
			Tooltip.SetDefault("20% chance to not consume ammo\nIncreases ranged damage by 5%");
			SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/BossLoot/StarplateDrops/StarArmor/StarPlate_Glow");
		}
		public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
		{
			glowMaskColor = Color.White;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 0, 38, 0);
			Item.rare = ItemRarityID.Orange;
			Item.defense = 9;
		}
		public override void UpdateEquip(Player player)
		{
			player.ammoCost80 = true;
			player.GetDamage(DamageClass.Ranged) += .05f;
		}
		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadow = true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<CosmiliteShard>(), 14);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
