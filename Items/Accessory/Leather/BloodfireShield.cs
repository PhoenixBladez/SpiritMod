
using SpiritMod.Items.Sets.BloodcourtSet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.Leather
{
	[AutoloadEquip(EquipType.Shield)]
	public class BloodfireShield : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sanguine Scutum");
			Tooltip.SetDefault("Reduces life regen to 0\nReduces damage taken by 5% for every nearby enemy, up to 25%");
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
			Item.rare = ItemRarityID.Green;
			Item.defense = 1;
			Item.DamageType = DamageClass.Melee;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().bloodfireShield = true;

			player.endurance += .05f * player.GetSpiritPlayer().bloodfireShieldStacks;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<LeatherShield>(), 1);
			recipe.AddIngredient(ModContent.ItemType<DreamstrideEssence>(), 6);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
