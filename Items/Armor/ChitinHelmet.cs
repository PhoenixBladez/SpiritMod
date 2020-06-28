using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class ChitinHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chitin Faceguard");

		}


		int timer = 0;
		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 20;
			item.value = 8000;
			item.rare = 1;
			item.defense = 3;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<ChitinChestplate>() && legs.type == ModContent.ItemType<ChitinLeggings>();
		}
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Greatly increases running speed\nHitting an enemy while running kicks up damaging dust and knocks them back";
			if(player.velocity.X != 0) {
				player.GetSpiritPlayer().chitinSet = true;
			}
			player.moveSpeed += .18f;
			player.maxRunSpeed += .25f;
			if(player.velocity.X != 0f) {
				int dust = Dust.NewDust(new Vector2(player.position.X, player.position.Y + player.height - 4f), player.width, 0, 0);
				Main.dust[dust].velocity *= 0f;
				Main.dust[dust].noGravity = true;
			}

		}
		public override void ArmorSetShadows(Player player)
		{
			if(player.velocity.X != 0)
				player.armorEffectDrawShadow = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Chitin>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
