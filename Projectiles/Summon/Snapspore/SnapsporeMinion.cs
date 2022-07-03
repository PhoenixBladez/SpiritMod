using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon.Snapspore
{
	public class SnapsporeMinion : ModProjectile
    {
        Vector2 direction = Vector2.Zero;
        int counter = 0;
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Snapspore");
			Main.projFrames[base.Projectile.type] = 1;
			Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
            ProjectileID.Sets.MinionSacrificable[base.Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[base.Projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.netImportant = true;
			Projectile.width = 28;
			Projectile.height = 28;
			Projectile.friendly = true;
			Projectile.minion = true;
			Projectile.minionSlots = 1;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 18000;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
		}
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.X * .08f;
            bool flag64 = Projectile.type == ModContent.ProjectileType<SnapsporeMinion>();
            Player player = Main.player[Projectile.owner];
            if (Projectile.Distance(player.Center) > 1500)
            {
                Projectile.position = player.position + new Vector2(Main.rand.Next(-125, 126), Main.rand.Next(-125, 126));
            }
            MyPlayer modPlayer = player.GetSpiritPlayer();
            if (flag64)
            {
                if (player.dead)
                    modPlayer.snapsporeMinion = false;

                if (modPlayer.snapsporeMinion)
                    Projectile.timeLeft = 2;
            }

            int range = 26;   //How many tiles away the projectile targets NPCs
            float lowestDist = float.MaxValue;
            if (player.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[player.MinionAttackTargetNPC];
                float dist = Projectile.Distance(npc.Center);
                if (dist / 16 < range)
                {
                    Projectile.ai[1] = npc.whoAmI;
                }
            }
            else
            {
                foreach (NPC npc in Main.npc)
                {
                    //if npc is a valid target (active, not friendly, and not a critter)
                    if (npc.active && !npc.friendly && npc.catchItem == 0)
                    {
                        //if npc is within 50 blocks
                        float dist = Projectile.Distance(npc.Center);
                        if (dist / 16 < range)
                        {
                            //if npc is closer than closest found npc
                            if (dist < lowestDist)
                            {
                                lowestDist = dist;

                                //target this npc
                                Projectile.ai[1] = npc.whoAmI;
                            }
                        }
                    }
                }
            }
            NPC target = (Main.npc[(int)Projectile.ai[1]] ?? new NPC()); //our target
            if (target.active && !target.friendly && target.type != NPCID.TargetDummy && target.type != NPCID.DD2LanePortal)
            {
                counter++;
                if (counter < 179)
                {
                    float num535 = 8f;

                    Vector2 vector38 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
                    float num536 = target.Center.X - vector38.X;
                    float num537 = target.Center.Y - vector38.Y;
                    float num538 = (float)Math.Sqrt((double)(num536 * num536 + num537 * num537));
                    if (num538 > 2000f)
                    {
                        Projectile.position.X = Main.player[Projectile.owner].Center.X - (float)(Projectile.width / 2);
                        Projectile.position.Y = Main.player[Projectile.owner].Center.Y - (float)(Projectile.width / 2);
                    }
                    if (num538 > 70f)
                    {
                        num538 = num535 / num538;
                        num536 *= num538;
                        num537 *= num538;
                        Projectile.velocity.X = (Projectile.velocity.X * 20f + num536) / 21f;
                        Projectile.velocity.Y = (Projectile.velocity.Y * 20f + num537) / 21f;
                    }
                    else
                    {
                        if (Projectile.velocity.X == 0f && Projectile.velocity.Y == 0f)
                        {
                            Projectile.velocity.X = -0.15f;
                            Projectile.velocity.Y = -0.05f;
                        }
                        Projectile.velocity *= 1.01f;
                    }
					if (counter % 60 == 0)
                    {
                        Projectile.velocity.Y -= 3.95f;
						for (int z = 0; z < 2; z++)
                        {
                            int a = Gore.NewGore(Projectile.GetSource_FromAI(), new Vector2(Projectile.Center.X + Main.rand.Next(-10, 10), Projectile.Center.Y + Main.rand.Next(-10, 10)), new Vector2(Main.rand.Next(-2, 2), Main.rand.Next(-2, 2)), 915, Main.rand.NextFloat(.4f, .95f));
                            Main.gore[a].timeLeft = 10;
                        }
                    }
                }
				else
                {
                    Projectile.friendly = true;
                    float num535 = 8f;
					
                    Vector2 vector38 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
                    float num536 = Main.player[Projectile.owner].Center.X - vector38.X;
                    float num537 = Main.player[Projectile.owner].Center.Y - vector38.Y - 60f;
                    float num538 = (float)Math.Sqrt((double)(num536 * num536 + num537 * num537));
                    if (num538 > 2000f)
                    {
                        Projectile.position.X = Main.player[Projectile.owner].Center.X - (float)(Projectile.width / 2);
                        Projectile.position.Y = Main.player[Projectile.owner].Center.Y - (float)(Projectile.width / 2);
                    }
                    if (num538 > 70f)
                    {
                        num538 = num535 / num538;
                        num536 *= num538;
                        num537 *= num538;
                        Projectile.velocity.X = (Projectile.velocity.X * 20f + num536) / 21f;
                        Projectile.velocity.Y = (Projectile.velocity.Y * 20f + num537) / 21f;
                    }
                    else
                    {
                        if (Projectile.velocity.X == 0f && Projectile.velocity.Y == 0f)
                        {
                            Projectile.velocity.X = -0.15f;
                            Projectile.velocity.Y = -0.05f;
                        }
                        Projectile.velocity *= 1.01f;
                    }
                }
				if (counter == 150)
                {
                    for (int num621 = 0; num621 < 8; num621++)
                    {
                        Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Grass, 0f, 0f, 100, default, .7f);
                        Dust dust = Main.dust[Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y + 2f), Projectile.width, Projectile.height, ModContent.DustType<Dusts.PoisonGas>(), Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, new Color(), 5f)];
                        dust.noGravity = true;
                        dust.velocity.X = dust.velocity.X * 0.3f;
                        dust.velocity.Y = (dust.velocity.Y * 0.2f) - 1;
                    }
                    int amountOfProjectiles = Main.rand.Next(2, 4);
                    SoundEngine.PlaySound(SoundID.Item95, Projectile.Center);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                        for (int i = 0; i < amountOfProjectiles; ++i)
                            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X, Projectile.Center.Y, Main.rand.Next(-2, 2), Main.rand.Next(-2, 2), ModContent.ProjectileType<Projectiles.PoisonCloud>(), Projectile.damage / 2, 1, Main.myPlayer, 0, 0);
                }
				if (counter > Main.rand.Next(300, 320))
                {

                    for (int z = 0; z < 2; z++)
                    {
                        int a = Gore.NewGore(Projectile.GetSource_FromAI(), new Vector2(Projectile.Center.X + Main.rand.Next(-10, 10), Projectile.Center.Y + Main.rand.Next(-10, 10)), new Vector2(Main.rand.Next(-2, 2), Main.rand.Next(-2, 2)), 915, Main.rand.NextFloat(.4f, .95f));
                        Main.gore[a].timeLeft = 10;
                    }
                    Projectile.velocity.Y -= 4.95f;
                    counter = 0;
                }
            }
            else
            {
				if (Main.rand.NextBool(320))
                {
                    Projectile.velocity.Y -= 4.95f;
                    for (int z = 0; z < 2; z++)
                    {
                        int a = Gore.NewGore(Projectile.GetSource_FromAI(), new Vector2(Projectile.Center.X + Main.rand.Next(-10, 10), Projectile.Center.Y + Main.rand.Next(-10, 10)), new Vector2(Main.rand.Next(-2, 2), Main.rand.Next(-2, 2)), 915, Main.rand.NextFloat(.4f, .95f));
                        Main.gore[a].timeLeft = 10;
                    }
                }
                Projectile.friendly = true;
                float num535 = 8f;

                Vector2 vector38 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
                float num536 = Main.player[Projectile.owner].Center.X - vector38.X;
                float num537 = Main.player[Projectile.owner].Center.Y - vector38.Y - 60f;
                float num538 = (float)Math.Sqrt((double)(num536 * num536 + num537 * num537));
                if (num538 > 2000f)
                {
                    Projectile.position.X = Main.player[Projectile.owner].Center.X - (float)(Projectile.width / 2);
                    Projectile.position.Y = Main.player[Projectile.owner].Center.Y - (float)(Projectile.width / 2);
                }
                if (num538 > 100f)
                {
                    num538 = num535 / num538;
                    num536 *= num538;
                    num537 *= num538;
                    Projectile.velocity.X = (Projectile.velocity.X * 20f + num536) / 21f;
                    Projectile.velocity.Y = (Projectile.velocity.Y * 20f + num537) / 21f;
                }
                else
                {
                    if (Projectile.velocity.X == 0f && Projectile.velocity.Y == 0f)
                        Projectile.velocity.X = -0.015f;
                    Projectile.velocity *= 1.0001f;
                }
            }

        }
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Color color = Projectile.GetAlpha(Color.White) * (((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) * 0.35f);
                float scale = Projectile.scale * (float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length;

				Main.spriteBatch.Draw(ModContent.Request<Texture2D>("SpiritMod/Projectiles/Summon/Snapspore/SnapsporeMinion_Trail", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value,
                Projectile.oldPos[k] + drawOrigin - Main.screenPosition,
                new Rectangle(0, (TextureAssets.Projectile[Projectile.type].Value.Height / 2) * Projectile.frame, TextureAssets.Projectile[Projectile.type].Value.Width, TextureAssets.Projectile[Projectile.type].Value.Height / 2),
                color,
                Projectile.rotation,
                new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width / 2, TextureAssets.Projectile[Projectile.type].Value.Height / 4),
                scale, default, default);
            }
            return true;
        }
        public override bool MinionContactDamage()
		{
			return true;
		}

	}
}