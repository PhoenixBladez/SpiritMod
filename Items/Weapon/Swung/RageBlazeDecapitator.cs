using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SpiritMod.Items.Weapon.Swung
{
	public class RageBlazeDecapitator : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Energized Axe");
			Tooltip.SetDefault("Releases an energy explosion every five strikes");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Swung/RageBlazeDecapitator_Glow");
		}


		public override void SetDefaults()
		{
			item.damage = 44;
			item.melee = true;
			item.width = 31;
			item.height = 25;
			item.useTime = 22;
			item.useAnimation = 22;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 8;
			item.value = Terraria.Item.sellPrice(0, 2, 10, 0);
			item.rare = 5;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.useTurn = true;
		}
		int numTicks;
		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			numTicks++;
			if(numTicks >= 5) {
				numTicks = 0;
				Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 109));
				{
					for(int i = 0; i < 20; i++) {
						int num = Dust.NewDust(target.position, target.width, target.height, 226, 0f, -2f, 0, default(Color), 2f);
						Main.dust[num].noGravity = true;
						Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].scale *= .25f;
						if(Main.dust[num].position != target.Center)
							Main.dust[num].velocity = target.DirectionTo(Main.dust[num].position) * 6f;
					}
				}
				int proj = Projectile.NewProjectile(target.Center.X, target.Center.Y,
					0, 0, mod.ProjectileType("GraniteSpike1"), item.damage, item.knockBack, Main.myPlayer, 0f, 0f);
				Main.projectile[proj].timeLeft = 2;
			}
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(item.position, 0.06f, .16f, .22f);
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				mod.GetTexture("Items/Weapon/Swung/RageBlazeDecapitator_Glow"),
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

		public override void UseStyle(Player player)
		{
			float cosRot = (float)Math.Cos(player.itemRotation - 0.78f * player.direction * player.gravDir);
			float sinRot = (float)Math.Sin(player.itemRotation - 0.78f * player.direction * player.gravDir);
			for(int i = 0; i < 1; i++) {
				float length = (item.width * 1.2f - i * item.width / 9) * item.scale + 16; //length to base + arm displacement
				int dust = Dust.NewDust(new Vector2((float)(player.itemLocation.X + length * cosRot * player.direction), (float)(player.itemLocation.Y + length * sinRot * player.direction)), 0, 0, 226, player.velocity.X * 0.9f, player.velocity.Y * 0.9f, 100, Color.Transparent, .8f);
				Main.dust[dust].velocity *= 0f;
				Main.dust[dust].noGravity = true;
			}
		}
	}
}