using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using System;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet
{
	public class MinifishProj : ModProjectile
    {
		public override void SetStaticDefaults() => DisplayName.SetDefault("Minifish");

		public override void SetDefaults()
		{
            Projectile.width = 8;
            Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 600;
			Projectile.alpha = 255;
		}

		Vector2 newCenter = Vector2.Zero;
		float offset = 0;
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.WriteVector2(newCenter);
			writer.Write(offset);
			writer.Write(Projectile.localAI[0]);
			writer.Write(Projectile.localAI[1]);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			newCenter = reader.ReadVector2();
			offset = reader.ReadSingle();
			Projectile.localAI[0] = reader.ReadSingle();
			Projectile.localAI[1] = reader.ReadSingle();
		}
		public override void AI()
		{
			Player owner = Main.player[Projectile.owner];
			Vector2 homeCenter = owner.Center;
			homeCenter.Y -= 50;
			if (Main.myPlayer == owner.whoAmI)
			{
				Projectile.ai[0] = Utils.AngleLerp(owner.AngleTo(Main.MouseWorld), Projectile.AngleTo(Main.MouseWorld), 0.75f);
				Projectile.netUpdate = true;
			}

			if (Projectile.localAI[1] == 0) {
				Projectile.rotation = Projectile.ai[0];
				newCenter = Projectile.Center - homeCenter;
				Projectile.localAI[1]++;
				Projectile.netUpdate = true;
			}
			var projsofthistype = Main.projectile.Where(x => x.type == Projectile.type && x.owner == Projectile.owner);
			if (Projectile.Distance(homeCenter) > 30) {
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(homeCenter) * 4, 0.05f);
			}
			newCenter += Projectile.velocity;
			Projectile.Center = homeCenter + newCenter;
			if (owner.HeldItem.type != Mod.Find<ModItem>("Minifish").Type || owner.dead || !owner.active)
				Projectile.ai[1]++;

			Projectile.damage = (int)(owner.GetDamage(DamageClass.Ranged).ApplyTo(owner.HeldItem.damage) * 0.8f);
			Projectile.spriteDirection = (Math.Abs(Projectile.rotation) > MathHelper.PiOver2) ? -1 : 1;

			if (Projectile.ai[1] > 0 || Projectile.timeLeft < 30) {
				Projectile.alpha += 10;
				if (Projectile.alpha >= 255)
					Projectile.Kill();
			}
			else {
				Projectile.alpha = (Projectile.alpha > 0) ? Projectile.alpha - 10 : 0;

				Projectile.rotation = Utils.AngleLerp(Projectile.rotation, Projectile.ai[0], 0.075f);
				offset = (owner.itemTime > 0) ? MathHelper.Lerp(offset, 50, 0.07f) : MathHelper.Lerp(offset, 0, 0.07f);

				if (owner.itemTime > 1) {
					if (--Projectile.localAI[0] == 0) {
						bool canshoot = owner.PickAmmo(owner.inventory[owner.selectedItem], out int shoot, out float speed, out int damage, out float knockback, out int _, true);
						if (canshoot) {
							SoundEngine.PlaySound(SoundID.Item11, Projectile.Center);
							Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center + (Vector2.UnitX.RotatedBy(Projectile.rotation) * offset), 
								Vector2.UnitX.RotatedBy(Projectile.rotation) * (speed + 10), shoot, damage, knockback + Projectile.knockBack, Projectile.owner);

							if (Main.netMode != NetmodeID.SinglePlayer)
								NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj.whoAmI);
						}

						Projectile.netUpdate = true;
					}
				}
				else
					Projectile.localAI[0] = (projsofthistype.Where(x => x.whoAmI < Projectile.whoAmI).Count() + 1) * 4;
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawPos = Projectile.Center + (Vector2.UnitX.RotatedBy(Projectile.rotation) * offset) - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
			Color color = Projectile.GetAlpha(lightColor);
			SpriteEffects spriteeffects = (Projectile.spriteDirection < 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, 
				drawPos, 
				null, 
				color, 
				Projectile.rotation - ((Projectile.spriteDirection < 0) ? MathHelper.Pi : 0), 
				TextureAssets.Projectile[Projectile.type].Value.Size()/2, 
				Projectile.scale, 
				spriteeffects, 
				0f);

			return false;
		}
    }
}
