using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Weapon.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Ocean
{
	public class ElectricEel : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Electric Eel");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 60;
			npc.height = 18;
			npc.damage = 22;
			npc.defense = 10;
			npc.lifeMax = 125;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath5;
			npc.value = 340f;
			npc.knockBackResist = .35f;
			npc.aiStyle = 16;
			npc.noGravity = true;
			aiType = NPCID.Shark;
            banner = npc.type;
            bannerItem = ModContent.ItemType<Items.Banners.ElectricEelBanner>();
        }

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if(spawnInfo.playerSafe) {
				return 0f;
			}
			return SpawnCondition.OceanMonster.Chance * 0.08f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if(npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Eel_Gore"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Eel_Gore_2"), 1f);
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Ocean/ElectricEel_Glow"));
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if(Main.rand.Next(8) == 0) {
				target.AddBuff(BuffID.Electrified, 180, true);
			}
		}

		public override void AI()
		{
			npc.spriteDirection = npc.direction;
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.46f, 0.32f, .1f);
		}

		public override void NPCLoot()
		{
			if(Main.rand.Next(20) == 1) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<EelRod>(), 1);
			}
		}
	}
}
