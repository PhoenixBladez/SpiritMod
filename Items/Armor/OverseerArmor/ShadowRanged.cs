
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.OverseerArmor
{
	[AutoloadEquip(EquipType.Head)]
	public class ShadowRanged : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadowspirit Shako");
			Tooltip.SetDefault("32% increased ranged damage\n22% increased ranged critical strike chance\n30% increased movement speed");
		}
		public override void SetDefaults()
		{
			item.width = 40;
			item.height = 30;
			item.value = 200000;
			item.rare = ItemRarityID.Purple;

			item.defense = 22;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<ShadowSBody>() && legs.type == ModContent.ItemType<ShadowLegs>();
		}
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Ranged and thrower hits spawn Soul Shards to chase down foes!\n'You have become the Guardian'\nDouble tap to dash repeatedly\nYou are surrounded by protective Spirits, which deflect projectiles.";
			player.GetSpiritPlayer().rangedshadowSet = true;
			player.GetSpiritPlayer().shadowSet = true;


			if (Main.rand.Next(4) == 1) {

				Dust.NewDust(player.position, player.width, player.height, 187);
			}
		}
		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadow = true;
		}

		public override void UpdateEquip(Player player)
		{
			player.rangedCrit += 22;
			player.rangedDamage += 0.32F;
			player.moveSpeed += 0.30F;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<EternityEssence>(), 6);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
