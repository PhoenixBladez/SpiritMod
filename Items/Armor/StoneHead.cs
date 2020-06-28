using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class StoneHead : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stone Helmet");
			Tooltip.SetDefault("Decreases movement speed by 6%");
		}


		int timer = 0;
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 22;
			item.value = 800;
			item.rare = 1;
			item.defense = 3;
		}
		public override void UpdateEquip(Player player)
		{
			player.moveSpeed -= 0.06f;
			player.maxRunSpeed -= .03f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<StoneBody>() && legs.type == ModContent.ItemType<StoneLegs>();
		}
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Increases falling speed greatly\nIncreases defense by 5 when falling";
			if(player.velocity.Y > 0 && !player.gravControl) {
				player.velocity.Y = 10.53f;
				player.statDefense += 5;
				player.armorEffectDrawShadow = true;
			} else if(player.velocity.Y > 0 && player.ropeCount <= 0) {
				player.velocity.Y = 10.53f;
				player.statDefense += 5;
				player.armorEffectDrawShadow = true;
			}
		}
		public override void ArmorSetShadows(Player player)
		{
			if(player.velocity.Y > 0) {
				player.armorEffectDrawShadow = true;
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.StoneBlock, 35);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}