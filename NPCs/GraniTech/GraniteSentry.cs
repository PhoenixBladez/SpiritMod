using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SpiritMod.Prim;
using SpiritMod.Mechanics.Trails;
using SpiritMod.Utilities;
using SpiritMod.Particles;
using SpiritMod.Items.Sets.GranitechSet;

namespace SpiritMod.NPCs.GraniTech
{
    class GraniteSentry : ModNPC
    {
        public float BaseState { get => npc.ai[0]; private set => npc.ai[0] = value; }
        float scanTimer = 0;
        bool firing = false;
        Vector2 laserEdge = Vector2.Zero;
        Vector2 laserOrigin = Vector2.Zero;
        float chargeUp = 0;
        float recoil = 0;

		private List<(float, int)> laserRotations = new List<(float,int)>();

		public override void SetStaticDefaults() => DisplayName.SetDefault("GraniTec Turret");

		public override void SetDefaults()
        {
            npc.width = 44; //Stats placeholder -->
            npc.height = 46;
            npc.damage = 60;
            npc.defense = 24;
            npc.lifeMax = 1800; // <--
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.value = 00000800;
            npc.knockBackResist = 0f;
            npc.aiStyle = -1;
            npc.DeathSound = SoundID.NPCDeath37;
            npc.HitSound = SoundID.NPCHit4;

            Main.npcFrameCount[npc.type] = 3;
        }

        const int GroundDistance = 80; //Distance it'll scan to look for a valid wall

        public override void AI()
        {
            if (BaseState == 0)
            {
                //lets hit that fat scan
                bool[] validGrounds = new bool[4] { false, false, false, false };
                for (int i = (int)(npc.position.Y / 16f); i < (int)(npc.position.Y / 16f) + GroundDistance; ++i) //above
                    if (Framing.GetTileSafely((int)(npc.position.X / 16f), i).active() && Main.tileSolid[Framing.GetTileSafely((int)(npc.position.X / 16f), i).type])
                        validGrounds[0] = true;
                for (int i = (int)(npc.position.Y / 16f); i > (int)(npc.position.Y / 16f) - GroundDistance; --i) //below
                    if (Framing.GetTileSafely((int)(npc.position.X / 16f), i).active() && Main.tileSolid[Framing.GetTileSafely((int)(npc.position.X / 16f), i).type])
                        validGrounds[1] = true;

                for (int i = (int)(npc.position.X / 16f); i > (int)(npc.position.X / 16f) - GroundDistance; --i) //left
                    if (Framing.GetTileSafely(i, (int)(npc.position.Y / 16f)).active() && Main.tileSolid[Framing.GetTileSafely(i, (int)(npc.position.Y / 16f)).type])
                        validGrounds[2] = true;
                for (int i = (int)(npc.position.X / 16f); i < (int)(npc.position.X / 16f) + GroundDistance; ++i) //right
                    if (Framing.GetTileSafely(i, (int)(npc.position.Y / 16f)).active() && Main.tileSolid[Framing.GetTileSafely(i, (int)(npc.position.Y / 16f)).type])
                        validGrounds[3] = true;

                int index;
                int safety = 0;

                if (!validGrounds.Any(x => x))
                    npc.active = false; //Delete me if I don't have anchoring

                do //Choose a random placement
                {
                    index = Main.rand.Next(2);
                    safety++;
                } while (validGrounds[index] && safety < 100);

                BaseState = index + 1; //woo I did it
                //no you didn't you suck and i hate you :slight_smile:

                switch (BaseState)
                {
                    case 1:
                        for (int i = (int)(npc.position.Y / 16f); i > (int)(npc.position.Y / 16f) - GroundDistance; --i) //above
                        {
                            if (Framing.GetTileSafely((int)(npc.position.X / 16f), i).active() && Main.tileSolid[Framing.GetTileSafely((int)(npc.position.X / 16f), i).type])
                            {
                                npc.position.Y = (i * 16f) + 28;
                                break;
                            }
                        }
                        break;
                    case 2:
                        for (int i = (int)(npc.position.Y / 16f); i < (int)(npc.position.Y / 16f) + GroundDistance; ++i) //below
                        {
                            if (Framing.GetTileSafely((int)(npc.position.X / 16f), i).active() && Main.tileSolid[Framing.GetTileSafely((int)(npc.position.X / 16f), i).type])
                            {
                                npc.position.Y = ((i - 2) * 16f) - 8;
                                break;
                            }
                        }
                        break;
                    default: break;
                }
            }
            else 
            {
                if (chargeUp <= 6)
                    scanTimer+= 0.01f;
                if (!firing)
                {
					if (laserRotations == null)
						laserRotations = new List<(float,int)>();

					Vector2 delta = laserEdge - laserOrigin;
					int length = (int)delta.Length();
					float rotation = delta.ToRotation();
					laserRotations.Add((rotation, length));

					while (laserRotations.Count > 16)
					{
						laserRotations.RemoveAt(0);
					}
					chargeUp = 0;
                    for (int i = 0; i < Main.player.Length; i++)
                    {
                        Player player = Main.player[i];
                        if (player.active)
                        {
                            float collisionPoint = 0f;
                            if (Collision.CheckAABBvLineCollision(player.Hitbox.TopLeft(), player.Hitbox.Size(), laserOrigin, laserEdge, (npc.width + npc.height) * 0.5f * npc.scale, ref collisionPoint)) {
                                firing = true;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    chargeUp++;
					if (laserRotations != null)
						if (laserRotations.Count > 0)
							laserRotations.RemoveAt(0);

					if (chargeUp == 20)
					{
						ParticleHandler.SpawnParticle(new PulseCircle(npc, new Color(25, 132, 247) * 0.4f, 175, 15,
						PulseCircle.MovementType.OutwardsSquareRooted, npc.Center)
						{
							Angle = npc.rotation + 3.14f,
							ZRotation = 0.6f,
							RingColor = new Color(25, 132, 247),
							Velocity = (npc.rotation + 3.14f).ToRotationVector2() * 5
						});
					}
                    if (chargeUp > 20 && chargeUp % 6 == 0)
                    {
                        recoil = Main.rand.NextFloat(-0.1f,0.1f);
                        switch (BaseState) 
                        {
                            case 1: //above
                                npc.rotation = (float)(Math.Sin(scanTimer + recoil) * 1.57f) - 1.57f;
                                break;
                            case 2: //below
                                npc.rotation = (float)(Math.Sin(scanTimer + recoil) * 1.57f) + 1.57f;
                                break;
                        }
                        Projectile.NewProjectile(laserOrigin, (npc.rotation + 3.14f).ToRotationVector2() * 20, ModContent.ProjectileType<GraniteSentryBolt>(), 40, 3, npc.target);
						laserRotations = null;
						Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 91, 0.5f, 0.5f);
                    }
                    recoil *= 0.99f;
                    firing = false;
					for (int i = 0; i < Main.player.Length; i++)
					{
						Player player = Main.player[i];
						if (player.active)
						{
							float collisionPoint = 0f;
							if (Collision.CheckAABBvLineCollision(player.Hitbox.TopLeft(), player.Hitbox.Size(), laserOrigin, laserEdge, (npc.width + npc.height) * npc.scale * 3, ref collisionPoint)) {
								firing = true;
								break;
							}
						}
					}
                }
                switch (BaseState) 
                {
                    case 1: //above
                        laserOrigin = npc.Center - new Vector2(0, 6);
                        npc.rotation = (float)(Math.Sin(scanTimer + recoil) * 1.57f) - 1.57f;
                        break;
                    case 2: //below
                        laserOrigin = npc.Center - new Vector2(0, 6);
                        npc.rotation = (float)(Math.Sin(scanTimer + recoil) * 1.57f) + 1.57f;
                        break;
                }
                for (int i = 0; i < 1600; i += 8)
                {
                    Vector2 toLookAt = laserOrigin + ((npc.rotation.ToRotationVector2().RotatedBy(3.14f)) * i);
                    if (i >= 1590 || (Framing.GetTileSafely((int)(toLookAt.X / 16),(int)(toLookAt.Y / 16)).active() && Main.tileSolid[Framing.GetTileSafely((int)(toLookAt.X / 16),(int)(toLookAt.Y / 16)).type]))
                    {
                        laserEdge = toLookAt;
                        break;
                    }
                }
            }
        }

        public override void NPCLoot()
        {
			for(int i = 1; i <= 4; i++)
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot($"Gores/GraniTech/GraniteSentryGore{i}"), 1f);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			int x = spawnInfo.spawnTileX;
			int y = spawnInfo.spawnTileY;
			int tile = (int)Main.tile[x, y].type;
			return (tile == 368) && spawnInfo.spawnTileY > Main.rockLayer && Main.hardMode ? 0.06f : 0f;

		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Vector2 delta = laserEdge - laserOrigin;
            float length = delta.Length();
            float rotation = delta.ToRotation();
            if (chargeUp < 20)
            {
                Color laserColor = chargeUp % 10 > 5 ? Color.White : Color.Red;
				laserColor.A = 0;
                 Main.spriteBatch.Draw(Main.magicPixel, laserOrigin - Main.screenPosition, new Rectangle(0, 0, 1, 1), laserColor * 0.35f, rotation, new Vector2(0f, 1f), new Vector2(length, 2 - (chargeUp / 12f)), SpriteEffects.None, 0f);

				if (laserRotations != null && laserRotations.Count > 3)
				{
					float oldRot = laserRotations[0].Item1;
					int oldLength = laserRotations[0].Item2;
					float rotDifference = ((((rotation - oldRot) % 6.28f) + 9.42f) % 6.28f) - 3.14f;
					if (rotDifference > 0)
					{
						for (float k = 0; k < rotDifference; k += 0.005f * Math.Sign(rotDifference))
						{
							DrawLaser(spriteBatch, laserColor * 0.35f, k, oldRot, rotation, rotDifference, oldLength, (int)length);
						}
					}
					else
					{
						for (float k = rotDifference; k < 0; k -= 0.005f * Math.Sign(rotDifference))
						{
							DrawLaser(spriteBatch, laserColor * 0.35f, k, oldRot, rotation, rotDifference, oldLength, (int)length);
						}
					}
				}
			}

			Vector2 realPos = npc.position - Main.screenPosition;
            if (BaseState == 1) //On ceiling
                spriteBatch.Draw(Main.npcTexture[npc.type], realPos + new Vector2(44, 2), new Rectangle(0, 32, 44, 18), drawColor, (float)Math.PI, new Vector2(), 1f, SpriteEffects.None, 0f);
            if (BaseState == 2) //On ground
                spriteBatch.Draw(Main.npcTexture[npc.type], realPos + new Vector2(0, 24), new Rectangle(0, 32, 44, 18), drawColor);

            float rot = npc.rotation; //Rotation
            SpriteEffects s = SpriteEffects.None;

            if (BaseState == 1) s = SpriteEffects.FlipVertically;

            if (rot > Math.PI / 2f && rot < (Math.PI * 2) - (Math.PI / 2)) //Face the right direction
            {
                rot -= (float)Math.PI;
                s = SpriteEffects.FlipHorizontally;
                if (BaseState == 1) s = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
            }

            spriteBatch.Draw(Main.npcTexture[npc.type], realPos + new Vector2(22, 15), new Rectangle(0, 0, 44, 30), drawColor, rot, new Vector2(22, 15), 1f, s, 0f);
            return false;
        }

		private void DrawLaser(SpriteBatch spriteBatch, Color laserColor, float k, float oldRot, float currentRotation, float rotDifference, int oldLength, int newLength)
		{
			float rot = k + oldRot;
			float lerper = Math.Abs(k / rotDifference);
			lerper *= lerper * lerper;
			Color color = Color.Lerp(laserColor, new Color(255, 0, 0), (lerper * lerper) / 2);
			color.A = 0;
			float length = MathHelper.Lerp(oldLength, newLength, lerper);
			spriteBatch.Draw(Main.magicPixel, laserOrigin - Main.screenPosition, new Rectangle(0, 0, 1, 1), color * lerper * 0.5f, rot, Vector2.Zero, new Vector2(length, 2), SpriteEffects.None, 0);
		}
    }
    public class GraniteSentryBolt : ModProjectile
	{
		private readonly Color lightCyan = new Color(99, 255, 229);
		private readonly Color midBlue = new Color(25, 132, 247);
		private readonly Color darkBlue = new Color(20, 8, 189);
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Laser Bolt");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		float glow = 0;
		public override void SetDefaults()
		{
			projectile.penetrate = 3;
			projectile.tileCollide = false;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.width = 36;
			projectile.height = 18;
			projectile.tileCollide = true;
			projectile.alpha = 0;
		}
		public override void AI()
		{
			if (glow == 0)
			{
				for (int i = 0; i < 6; i++)
				{
					ParticleHandler.SpawnParticle(new GranitechParticle(projectile.Center + projectile.velocity, projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.6f,0.6f)) * Main.rand.NextFloat(), Main.rand.NextBool() ? lightCyan : midBlue, Main.rand.NextFloat(0.75f, 1.25f), 30, Main.rand.Next(4)));
				}
				glow = Main.rand.NextFloat();
			}
			projectile.rotation = projectile.velocity.ToRotation() + 3.14f;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];

			Vector2 origin = new Vector2(tex.Width / 2, tex.Height / 2);

				Color color = Color.White;
				float num108 = 4;
				float num107 = (float)Math.Cos((double)((glow + Main.GlobalTime) % 1f / 1f * 6.28318548f)) / 3f + 0.25f;
				float num106 = 0f;
				Color color29 = new Color(110 - projectile.alpha, 31 - projectile.alpha, 255 - projectile.alpha, 0).MultiplyRGBA(color);
				for (int num103 = 0; num103 < 4; num103++)
				{
					Color color28 = color29;
					color28 = projectile.GetAlpha(color28);
					color28 *= 1.5f - num107;
					Vector2 vector29 = projectile.Center + ((float)num103 / (float)num108 * 6.28318548f + projectile.rotation + num106).ToRotationVector2() * (num107 + 1f) - Main.screenPosition + new Vector2(0, projectile.gfxOffY);
					spriteBatch.Draw(tex, vector29, null, color28 * .8f, projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0f);
				}

			return false;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 6; i++)
			{
				ParticleHandler.SpawnParticle(new GranitechParticle(projectile.Center, projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.6f, 0.6f)) * Main.rand.NextFloat(), Main.rand.NextBool() ? lightCyan : midBlue, Main.rand.NextFloat(0.75f, 1.25f), 30, Main.rand.Next(4)));
			}
		}
		public override Color? GetAlpha(Color lightColor) => new Color(99, 255, 229, 0);
	}
}
