using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.SteamRaider
{
	public class ArcadeProbe : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starfarer");
			Main.npcFrameCount[NPC.type] = 1;
		}

		public override void SetDefaults()
		{
			NPC.width = 56;
			NPC.height = 46;
			NPC.damage = 0;
			NPC.defense = 12;
			NPC.noTileCollide = true;
			NPC.dontTakeDamage = true;
			NPC.lifeMax = 65;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.value = 160f;
			NPC.dontCountMe = true;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (NPC.alpha != 255) {
				GlowmaskUtils.DrawNPCGlowMask(spriteBatch, NPC, Mod.Assets.Request<Texture2D>("NPCs/Boss/SteamRaider/LaserBase_Glow").Value, screenPos);
			}
		}
		int lifeSpan = 250;
		int distAbove = 150;
		int fireRate = Main.rand.Next(27, 41);
		public override void SendExtraAI(BinaryWriter writer) => writer.Write(fireRate);
		public override void ReceiveExtraAI(BinaryReader reader) => fireRate = reader.ReadInt32();
		public override bool PreAI()
		{

			NPC.TargetClosest(true);
			if (lifeSpan <= 0) {
				NPC.life = 0;
				NPC.active = false;
			}
			Player player = Main.player[NPC.target];
			if (lifeSpan % 250 == 0) {
				distAbove = 375;
				if (Main.rand.NextBool(2)) {
					NPC.position.X = player.Center.X - Main.rand.Next(300, 500);
					NPC.position.Y = player.Center.Y - distAbove;
					NPC.velocity.X = 3f;
				}
				else {
					NPC.position.X = player.Center.X + Main.rand.Next(300, 500);
					NPC.position.Y = player.Center.Y - distAbove;
					NPC.velocity.X = -3f;
				}
				NPC.rotation = 0f;
				NPC.netUpdate = true;
			}
			NPC.velocity.Y = 0;
			if (lifeSpan % fireRate == 0) {
				SoundEngine.PlaySound(SoundID.Item91, NPC.Center);
				for (int i = 0; i < 16; i++) {
					int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.GoldCoin);

					Main.dust[dust].velocity *= -1f;
					Main.dust[dust].noGravity = true;
					//        Main.dust[dust].scale *= 2f;
					Vector2 vector2_1 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
					vector2_1.Normalize();
					Vector2 vector2_2 = vector2_1 * ((float)Main.rand.Next(50, 100) * 0.04f);
					Main.dust[dust].velocity = vector2_2;
					vector2_2.Normalize();
					Vector2 vector2_3 = vector2_2 * 34f;
					Main.dust[dust].position = (NPC.Center) - vector2_3;
				}
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(0, 10), ModContent.ProjectileType<GlitchLaser>(), NPCUtils.ToActualDamage(55, 1.5f), 1, Main.myPlayer, 0, 0);
			}
			lifeSpan--;
			return false;
		}
	}
}
