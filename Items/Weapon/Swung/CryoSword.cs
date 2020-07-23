using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Sword;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
	public class CryoSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rimehowl");
			Tooltip.SetDefault("Right click after 5 swings to summon a pillar that inflicts 'Cryo Crush'\nCryo Crush deals more damage the less life enemies have left\nThis does not affect bosses, and deals a flat rate of damage instead");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Swung/CryoSword_Glow");
		}

		int counter = 0;
		public override void SetDefaults()
		{
			item.damage = 28;
			item.melee = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 5.5f;
			item.value = Terraria.Item.sellPrice(0, 0, 70, 0);
			item.rare = ItemRarityID.Orange;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<CryoPillar>();
			item.shootSpeed = 8;
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(item.position, 0.06f, .16f, .22f);
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				mod.GetTexture("Items/Weapon/Swung/CryoSword_Glow"),
				new Vector2
				(
					item.position.X - Main.screenPosition.X + item.width * 0.5f,
					item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
				),
				new Rectangle(0, 0, texture.Width, texture.Height),
				Color.White,
				rotation,
				texture.Size() * 0.5f,
				scale,
				SpriteEffects.None,
				0f
			);
		}
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			if(Main.rand.Next(4) == 0)
				target.AddBuff(ModContent.BuffType<CryoCrush>(), 300);
		}
		public override bool CanUseItem(Player player)
		{
			if(player.altFunctionUse == 2) {
				if(counter > 0) {
					return false;
				} else {
					return true;
				}

			}
			return true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if(player.altFunctionUse == 2) {
				if(counter > 0) {
					return false;
				} else {
					counter = 5;
					Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 73));
					Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, type, damage, 7, player.whoAmI, 5, 0);
				}

			} else {
				counter--;
			}
			if(counter == 0) {
				Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 20));
				{
					for(int i = 0; i < 7; i++) {
						int num = Dust.NewDust(player.position, player.width, player.height, 68, 0f, -2f, 0, default(Color), 2f);
						Main.dust[num].noGravity = true;
						Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].scale *= .25f;
						if(Main.dust[num].position != player.Center)
							Main.dust[num].velocity = player.DirectionTo(Main.dust[num].position) * 6f;
					}
				}
			}
			return false;
		}
		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if(Main.rand.Next(5) == 0) {
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 180);
				Main.dust[dust].noGravity = true;
			}
		}

		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(ModContent.ItemType<CryoliteBar>(), 15);
			modRecipe.AddTile(TileID.Anvils);
			modRecipe.SetResult(this);
			modRecipe.AddRecipe();
		}

	}
}