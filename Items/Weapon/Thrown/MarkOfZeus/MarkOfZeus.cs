using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Prim;

namespace SpiritMod.Items.Weapon.Thrown.MarkOfZeus
{
	public class MarkOfZeus : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mark Of Zeus");
			Tooltip.SetDefault("Hold and release to throw\nHold it longer for more velocity and damage");
			//  SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Equipment/StarMap_Glow");
		}

		public override void SetDefaults()
		{
			item.damage = 50;
			item.noMelee = true;
			item.channel = true; //Channel so that you can held the weapon [Important]
			item.rare = ItemRarityID.LightRed;
			item.width = 18;
			item.height = 18;
			item.useTime = 15;
			item.useAnimation = 45;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = item.useAnimation = 24;
			item.knockBack = 8;
			item.magic = true;
			item.noMelee = true;
			//   item.UseSound = SoundID.Item20;
			item.autoReuse = false;
			item.noUseGraphic = true;
			item.shoot = ModContent.ProjectileType<MarkOfZeusProj>();
			item.shootSpeed = 0f;
			item.value = Item.sellPrice(0, 2, 0, 0);
		}

		/*   public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
           {
               Lighting.AddLight(item.position, 0.08f, .28f, .38f);
               Texture2D texture;
               texture = Main.itemTexture[item.type];
               spriteBatch.Draw
               (
                   ModContent.GetTexture("SpiritMod/Items/Equipment/StarMap_Glow"),
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
           }*/
	}
	public class MarkOfZeusProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mark of Zeus");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.magic = true;
			projectile.width = 16;
			projectile.height = 16;
			projectile.aiStyle = -1;
			projectile.friendly = false;
			projectile.penetrate = 1;
			projectile.alpha = 0;
			projectile.timeLeft = 999999;
			projectile.tileCollide = false;
		}

		float counter = 3.15f;
		int manaCounter = 0;
		int trailcounter = 0;
		Vector2 holdOffset = new Vector2(0, -3);
		bool charged = false;
		bool firing = false;
		float growCounter;
		public override bool PreAI()
		{
			growCounter++;
			Player player = Main.player[projectile.owner];
			if (player.statMana <= 0)
				projectile.Kill();
			if (projectile.owner == Main.myPlayer)
				{
					Vector2 direction2 = Main.MouseWorld - (projectile.position);
					direction2.Normalize();
					direction2 *= counter;
					projectile.ai[0] = direction2.X;
					projectile.ai[1] = direction2.Y;
					projectile.netUpdate = true;
				}
			Vector2 direction = new Vector2(projectile.ai[0], projectile.ai[1]);
			if (player.channel && !firing) {
				projectile.position = player.position + holdOffset;
				if (Main.rand.Next(3) == 0)
					Dust.NewDust(projectile.Center, 2, 2, 133);
				if (counter < 15) {
					counter += 0.15f;
					manaCounter++;
					if (manaCounter % 7 == 0)
					{
						if (player.statMana > 0)
						{
							player.statMana -= 5;
							player.manaRegenDelay = 60;
						}
						else
						{
							firing = true;
						}
					}
				}
				else if (!charged)
				{
					charged = true;
					Main.PlaySound(SoundID.NPCDeath7, projectile.Center);
				}
				projectile.rotation = direction.ToRotation() - 1.57f;
				if (direction.X > 0) {
					holdOffset.X = -10;
					player.direction = 1;
				}
				else {
					holdOffset.X = 10;
					player.direction = 0;
				}
				trailcounter++;
			}
			else {
				Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 1);
				if (projectile.owner == Main.myPlayer)
				{
					int proj = Projectile.NewProjectile(projectile.Center - (direction * 2), direction, ModContent.ProjectileType<MarkOfZeusProj2>(), (int)(projectile.damage * Math.Sqrt(counter) * 0.5f), projectile.knockBack, projectile.owner);
					if (Main.projectile[proj].modProjectile is MarkOfZeusProj2 modItem)
					{
						modItem.charge = counter;
					}
					if (Main.netMode != NetmodeID.Server)
                	{
                    	SpiritMod.primitives.CreateTrail(new MarkOfZeusPrimTrailTwo(Main.projectile[proj], (float)(Math.Sqrt(counter) / 5)));
                	}
				}
				projectile.active = false;
			}
			player.heldProj = projectile.whoAmI;
			player.itemTime = 30;
			player.itemAnimation = 30;
			//	player.itemRotation = 0;
			return true;
		}
		int flickerTime = 0;
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			float growScale = growCounter > 10 ? 1 : (growCounter / 10f);
			Player player = Main.player[projectile.owner];
			var effects = player.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			Texture2D tex = Main.projectileTexture[projectile.type];
			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition + new Vector2(0, projectile.gfxOffY), new Rectangle(0,0,tex.Width,tex.Height / 2),
							 lightColor, projectile.rotation, new Vector2(tex.Width / 2, tex.Height / 4), projectile.scale * new Vector2(1, 0.75f + counter / 30f) * growScale, effects, 0);
			if (counter >= 15 && flickerTime < 16)
			{
				flickerTime++;
				Color color = Color.White;
				float flickerTime2 = (float)(flickerTime / 20f);
				float alpha = 1.5f - (((flickerTime2 * flickerTime2) / 2) + (2f * flickerTime2));
				if (alpha < 0) {
					alpha = 0;
				}
				spriteBatch.Draw(tex, projectile.Center - Main.screenPosition + new Vector2(0, projectile.gfxOffY), new Rectangle(0,tex.Height / 2,tex.Width,tex.Height / 2),
							 color * alpha, projectile.rotation, new Vector2(tex.Width / 2, tex.Height / 4), projectile.scale * new Vector2(1, 0.75f + counter / 30f) * growScale, effects, 0);
			}
			return false;
		}
	}
	public class MarkOfZeusProj2 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mark Of Zeus");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public float charge;
		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 36;
			projectile.aiStyle = 1;
			projectile.friendly = true;
			projectile.magic = true;
			projectile.penetrate = 2;
			projectile.timeLeft = 600;
			projectile.extraUpdates = 1;
			projectile.light = 0;
			aiType = ProjectileID.ThrowingKnife;
			projectile.alpha = 255;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.timeLeft = 0;
		}
		//public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		//{
		//    Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
		//    for (int k = 0; k < projectile.oldPos.Length; k++)
		//    {
		//        Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
		//        Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
		//        spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
		//    }
		//    return true;
		//}
		public override void Kill(int timeLeft)
		{
			SpiritMod.tremorTime = (int)(charge * 0.66f);
			Main.PlaySound(SoundID.Item70, projectile.Center);
			 for (double i = 0; i < 6.28; i += Main.rand.NextFloat(1f, 2f))
            {
                int lightningproj = Projectile.NewProjectile(projectile.Center + projectile.velocity - (new Vector2((float)Math.Sin(i), (float)Math.Cos(i)) * 5f), new Vector2((float)Math.Sin(i), (float)Math.Cos(i)) * 2.5f, ModContent.ProjectileType<MarkOfZeusProj3>(), projectile.damage, projectile.knockBack, projectile.owner);
                if (Main.netMode != NetmodeID.Server)
                {
					SpiritMod.primitives.CreateTrail(new MarkOfZeusPrimTrail(Main.projectile[lightningproj], (float)(Math.Sqrt(charge) / 3)));
                }
				Main.projectile[lightningproj].timeLeft = (int)(20 * Math.Sqrt(charge));
            }
			for (double i = 0; i < 6.28; i+= 0.1)
            {
                Dust dust = Dust.NewDustPerfect(projectile.Center + projectile.velocity, 133, new Vector2((float)Math.Sin(i) * Main.rand.NextFloat(3f) * (float)(Math.Sqrt(charge) / 3), (float)Math.Cos(i)) * Main.rand.NextFloat(4f) * (float)(Math.Sqrt(charge) / 3));
				 Dust dust2 = Dust.NewDustPerfect(projectile.Center + projectile.velocity, 133, new Vector2((float)Math.Sin(i) * Main.rand.NextFloat(1.8f) * (float)(Math.Sqrt(charge) / 3), (float)Math.Cos(i)) * Main.rand.NextFloat(2.4f) * (float)(Math.Sqrt(charge) / 3));
                dust.noGravity = true;
				dust2.noGravity = true;
				dust.scale = 0.75f;
				dust2.scale = 0.75f;
            }
		}
	}
	public class MarkOfZeusProj3 : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mark of Zeus");
        }

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.aiStyle = -1;
            projectile.magic = true;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.damage = 0;
            projectile.timeLeft = 120;
            projectile.alpha = 255;
            projectile.extraUpdates = 4;
        }

        Vector2 initialVelocity = Vector2.Zero;

        private float lerp;
        public Vector2 DrawPos;
        public int boost;
        public override void AI()
        {
            if (initialVelocity == Vector2.Zero)
            {
                initialVelocity = projectile.velocity;
            }
            if (projectile.timeLeft % 10 == 0)
            {
                projectile.velocity = initialVelocity.RotatedBy(Main.rand.NextFloat(-1, 1));
            }
            /* if (projectile.timeLeft % 2 == 0)
             {
                 Dust dust = Dust.NewDustPerfect(projectile.Center, 226);
                 dust.noGravity = true;
                 dust.scale = (float)Math.Sqrt(projectile.timeLeft) / 4;
                 dust.velocity = Vector2.Zero;
             }*/
            DrawPos = projectile.position;
        }
    }
}