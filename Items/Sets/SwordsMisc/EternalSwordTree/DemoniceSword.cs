using Microsoft.Xna.Framework;
using SpiritMod.Items.Sets.CryoliteSet;
using SpiritMod.Projectiles.DonatorItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Sets.SwordsMisc.EternalSwordTree
{
	public class DemoniceSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vorpal Sword");
			Tooltip.SetDefault("Shoots out an icy razor that clings to tiles upon hitting them");

		}


		public override void SetDefaults()
		{
			Item.damage = 55;
			Item.useTime = 22;
			Item.useAnimation = 22;
			Item.DamageType = DamageClass.Melee;
			Item.width = 60;
			Item.height = 64;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 8;
			Item.value = Terraria.Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.Lime;
			Item.shootSpeed = 12;
			Item.UseSound = SoundID.Item20;
			Item.autoReuse = true;
			Item.useTurn = true;
			Item.shoot = ModContent.ProjectileType<DemonIceProj>();
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(4) == 0) {
				target.AddBuff(BuffID.Frostburn, 180);
			}
		}
		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			{
				if (Main.rand.Next(5) == 0) {
					int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.UnusedWhiteBluePurple);
					Main.dust[dust].noGravity = true;
				}
			}
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<CryoliteBar>(), 12);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 6);
            recipe.AddIngredient(ItemID.SoulofNight, 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}