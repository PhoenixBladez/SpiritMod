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

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("GraniTec Turret");
		}

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
             Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GraniTech/GraniteSentryGore1"), 1f);
             Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GraniTech/GraniteSentryGore2"), 1f);
             Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GraniTech/GraniteSentryGore3"), 1f);
             Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GraniTech/GraniteSentryGore4"), 1f);
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			int x = spawnInfo.spawnTileX;
			int y = spawnInfo.spawnTileY;
			int tile = (int)Main.tile[x, y].type;
			return (tile == 368) && spawnInfo.spawnTileY > Main.rockLayer && Main.hardMode ? 0.5f : 0f;

		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Vector2 delta = laserEdge - laserOrigin;
            float length = delta.Length();
            float rotation = delta.ToRotation();
            if (chargeUp < 20)
            {
                Color laserColor = chargeUp % 10 > 5 ? Color.White : Color.Red;
                  Main.spriteBatch.Draw(Main.magicPixel, laserOrigin - Main.screenPosition, new Rectangle(0, 0, 1, 1), laserColor * 0.65f, rotation, new Vector2(0f, 1f), new Vector2(length, 3 - (chargeUp / 6f)), SpriteEffects.None, 0f);
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
    }
    public class GraniteSentryBolt : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Laser Bolt");
		}
		public override void SetDefaults()
		{
			projectile.penetrate = 3;
			projectile.tileCollide = false;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.width = projectile.height = 24;
			projectile.tileCollide = true;
		}
		public Color color = Color.White;
		public override void AI()
		{
			if (color == Color.White)
			{
				switch (Main.rand.Next(4))
                {
                    case 0:
                        color = new Color(0,247,255,0);
                        break;
                    case 1:
                        color = new Color(0,132,255,0);
                        break;
                    case 2:
                        color = new Color(62,0,255,0);
                        break;
                    case 3:
                        color = new Color(158,0,255,0);
                        break;
                }
                color.A = 0;
				for (int i = 0; i < 15; i++)
				{
					Dust dust = Dust.NewDustPerfect(projectile.Center + (projectile.velocity), 267, (projectile.velocity.RotatedBy(Main.rand.NextFloat(-1,1)) / 5f) * Main.rand.NextFloat(), 0, color);
					dust.noGravity = !dust.noGravity;
				}
			}
			Lighting.AddLight((int)((projectile.position.X + (float)(projectile.width / 2)) / 16f), (int)((projectile.position.Y + (float)(projectile.height / 2)) / 16f), color.R / 450f, color.G / 450f, color.B / 450f);
			projectile.rotation = projectile.velocity.ToRotation();
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(projectile.position,projectile.width,projectile.height, 267, 0, 0, 0, color);
				Main.dust[dust].noGravity = !Main.dust[dust].noGravity;
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.Center - Main.screenPosition, null, color, projectile.rotation, Main.projectileTexture[projectile.type].Size() / 2, 1f, SpriteEffects.None, 0f);
			return false;
		}
	}
}
