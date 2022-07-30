using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Weapon.Magic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Buffs;
using SpiritMod.Items.Sets.ReefhunterSet;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.Bestiary;

namespace SpiritMod.NPCs.ElectricEel
{
	public class ElectricEel : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Electric Eel");
			Main.npcFrameCount[NPC.type] = 4;
		}

		public override void SetDefaults()
		{
			NPC.width = 60;
			NPC.height = 18;
			NPC.damage = 22;
			NPC.defense = 10;
			NPC.lifeMax = 125;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.buffImmune[ModContent.BuffType<ElectrifiedV2>()] = true;
			NPC.DeathSound = SoundID.NPCDeath5;
			NPC.value = 340f;
			NPC.knockBackResist = .35f;
			NPC.aiStyle = 16;
			NPC.noGravity = true;
			AIType = NPCID.Shark;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.ElectricEelBanner>();
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
				new FlavorTextBestiaryInfoElement("Utilizing the energy it builds up as it swims, this slender fish can release it in one big jolt, packing quite the punch."),
			});
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.15f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.PlayerSafe)
				return 0f;
			return SpawnCondition.OceanMonster.Chance * 0.08f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Eel_Gore").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Eel_Gore_2").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Eel_Gore_1").Type, 1f);
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => GlowmaskUtils.DrawNPCGlowMask(spriteBatch, NPC, Mod.Assets.Request<Texture2D>("NPCs/ElectricEel/ElectricEel_Glow").Value, screenPos);

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.NextBool(8))
				target.AddBuff(BuffID.Electrified, 180, true);
		}

		public override void AI()
		{
			NPC.spriteDirection = NPC.direction;
			Lighting.AddLight(NPC.Center / 16f, 0.46f, 0.32f, .1f);
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddCommon<IridescentScale>(1, 2, 4);
			npcLoot.AddCommon<EelRod>(20);
			npcLoot.AddCommon<Items.Consumable.Food.GoldenCaviar>(110);
		}
	}
}