using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.DonatorItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.DonatorItems
{
	public class EternalAsh : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eternal Ashes");
			Tooltip.SetDefault("Summons a Phoenix Minion to rain down fireballs on your foes ");
		}


		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.QueenSpiderStaff); //only here for values we haven't defined ourselves yet
			item.damage = 41;  //placeholder damage :3
			item.mana = 15;   //somehow I think this might be too much...? -thegamemaster1234
			item.width = 40;
			item.height = 40;
			item.value = Terraria.Item.sellPrice(0, 0, 70, 0);
			item.rare = ItemRarityID.Pink;
			item.knockBack = 3.5f;
			item.UseSound = SoundID.Item25;
			item.shoot = ModContent.ProjectileType<PhoenixMinion>();
			item.shootSpeed = 0f;
		}
		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			//remove any other owned SpiritBow projectiles, just like any other sentry minion
			for (int i = 0; i < Main.projectile.Length; i++) {
				Projectile p = Main.projectile[i];
				if (p.active && p.type == item.shoot && p.owner == player.whoAmI) {
					p.active = false;
				}
			}
			//projectile spawns at mouse cursor
			Vector2 value18 = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
			position = value18;
            player.UpdateMaxTurrets();
            return true;
		}
		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(ItemID.FireFeather, 1);
			modRecipe.AddIngredient(ModContent.ItemType<FleshClump>(), 3);
			modRecipe.AddIngredient(ItemID.PixieDust, 20);
			modRecipe.AddTile(TileID.MythrilAnvil);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();

			ModRecipe modRecipe2 = new ModRecipe(mod);
			modRecipe2.AddIngredient(ItemID.FireFeather, 1);
			modRecipe2.AddIngredient(ModContent.ItemType<PutridPiece>(), 3);
			modRecipe2.AddIngredient(ItemID.PixieDust, 20);
			modRecipe2.AddTile(TileID.MythrilAnvil);
			modRecipe2.SetResult(this, 1);
			modRecipe2.AddRecipe();
		}
	}
}