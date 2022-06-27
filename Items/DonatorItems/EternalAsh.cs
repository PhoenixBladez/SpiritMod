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
			Item.CloneDefaults(ItemID.QueenSpiderStaff); //only here for values we haven't defined ourselves yet
			Item.damage = 41;  //placeholder damage :3
			Item.mana = 15;   //somehow I think this might be too much...? -thegamemaster1234
			Item.width = 40;
			Item.height = 40;
			Item.value = Terraria.Item.sellPrice(0, 0, 70, 0);
			Item.rare = ItemRarityID.Pink;
			Item.knockBack = 3.5f;
			Item.UseSound = SoundID.Item25;
			Item.shoot = ModContent.ProjectileType<PhoenixMinion>();
			Item.shootSpeed = 0f;
		}
		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			//remove any other owned SpiritBow projectiles, just like any other sentry minion
			for (int i = 0; i < Main.projectile.Length; i++) {
				Projectile p = Main.projectile[i];
				if (p.active && p.type == Item.shoot && p.owner == player.whoAmI) {
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
			Recipe modRecipe = CreateRecipe(1);
			modRecipe.AddIngredient(ItemID.FireFeather, 1);
			modRecipe.AddIngredient(ItemID.SoulofNight, 3);
			modRecipe.AddIngredient(ItemID.PixieDust, 20);
			modRecipe.AddTile(TileID.MythrilAnvil);
			modRecipe.Register();
		}
	}
}